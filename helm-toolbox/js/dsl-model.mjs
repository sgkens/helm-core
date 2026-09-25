// Loss-minimizing DSL model for HELM Toolbox's visual inspector. It records source lines and only
// rewrites the directive the user changed; comments and unknown/custom syntax pass through intact.

const SCREEN_KEYS = new Set([
  "output", "profile", "layout", "font", "font_size", "screen_bg", "bg", "background",
  "border", "border_color", "padding", "gap", "title_align", "title_color",
]);
const PANEL_KEYS = new Set([
  "title", "font", "font_size", "bg", "background", "border", "border_color",
  "title_align", "title_color", "orientation",
]);

function splitDsl(dsl) {
  const newline = dsl.includes("\r\n") ? "\r\n" : "\n";
  return { newline, lines: dsl.replace(/\r\n/g, "\n").split("\n") };
}

function directive(line) {
  const match = line.trim().match(/^([A-Za-z_][\w-]*):\s*(.*)$/);
  return match ? { key: match[1].toLowerCase(), value: match[2] } : null;
}

function inlineValue(line, key) {
  const escaped = key.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
  const pattern = key === "title"
    ? new RegExp(`(?:^|\\s)${escaped}:\\s*"([^"]*)"`, "i")
    : new RegExp(`(?:^|\\s)${escaped}:\\s*([^\\s]+)`, "i");
  const match = line.match(pattern);
  return match ? match[1] : "";
}

function parsePanelHeader(line, index) {
  const match = line.trim().match(/^@panel\s+(\S+)\s+at\(\s*(-?\d+)\s*,\s*(-?\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)(.*)$/i);
  if (!match) return { name: line.trim().slice(6).trim() || "panel", col: 0, row: 0, spanX: 1, spanY: 1, headerIndex: index, directives: {}, lines: [] };
  const panel = {
    name: match[1], col: +match[2], row: +match[3], spanX: +match[4], spanY: +match[5],
    headerIndex: index, directives: {}, lines: [],
  };
  for (const key of PANEL_KEYS) {
    const value = inlineValue(line, key);
    if (value !== "") panel.directives[key] = { index, value, inline: true };
  }
  return panel;
}

export function parseLineDirective(raw, index = -1) {
  const match = raw.trim().match(/^line:\s*"([^"]*)"\s+([A-Za-z_]\w*)\(([^)]*)\)(.*)$/i);
  if (!match) return { index, raw, label: "", kind: "", ref: "", options: {}, parsed: false };
  const tail = match[4] || "";
  const option = (name, quoted = false) => {
    const call = findCall(tail, name);
    if (!call) return "";
    const value = call.value.trim();
    return quoted ? value.replace(/^"|"$/g, "") : value;
  };
  const dir = (tail.match(/(?:^|\s)dir:\s*([\w-]+)/i) || [])[1] || "";
  const flat = option("flat"), gradient = option("gradient");
  return {
    index, raw, parsed: true, label: match[1], kind: match[2], ref: match[3].trim(), tail,
    options: {
      colorMode: gradient ? "gradient" : flat ? "flat" : "none",
      colors: gradient || flat, unit: option("unit", true), size: option("size"), dir,
      labelColor: option("label_color"), track: option("track"),
    },
  };
}

// Find a function-style option while allowing nested colour functions such as
// flat(rgba(10, 20, 30, 180)). A regex ending at the first ')' would truncate those values.
function findCall(text, name) {
  const escaped = name.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
  const match = new RegExp(`(?:^|\\s)${escaped}\\(`, "i").exec(text);
  if (!match) return null;
  const start = match.index + (match[0].startsWith(" ") ? 1 : 0);
  const open = text.indexOf("(", start);
  let depth = 0, quote = false;
  for (let i = open; i < text.length; i++) {
    if (text[i] === '"' && text[i - 1] !== "\\") quote = !quote;
    if (quote) continue;
    if (text[i] === "(") depth++;
    else if (text[i] === ")" && --depth === 0) return { start, end: i + 1, value: text.slice(open + 1, i) };
  }
  return null;
}

export function parseDslModel(dsl) {
  const { newline, lines } = splitDsl(dsl);
  const model = { dsl, newline, lines, screens: [] };
  let screen = null, panel = null;
  for (let i = 0; i < lines.length; i++) {
    const trimmed = lines[i].trim();
    if (/^@screen\b/i.test(trimmed)) {
      if (screen) screen.endIndex = i;
      screen = { name: trimmed.slice(7).trim(), headerIndex: i, endIndex: lines.length, directives: {}, panels: [], looseLines: [] };
      model.screens.push(screen);
      panel = null;
      continue;
    }
    if (!screen) continue;
    if (/^@panel\b/i.test(trimmed)) {
      panel = parsePanelHeader(lines[i], i);
      screen.panels.push(panel);
      continue;
    }
    if (/^line:/i.test(trimmed)) {
      if (!panel) {
        panel = { name: "main", col: 0, row: 0, spanX: 1, spanY: 1, headerIndex: null, directives: {}, lines: [], implicit: true };
        screen.panels.push(panel);
      }
      panel.lines.push(parseLineDirective(lines[i], i));
      continue;
    }
    const parsed = directive(lines[i]);
    if (!parsed) continue;
    // The game parser treats a standalone title before the first line as the title of an
    // implicit full-screen panel. Mirror that shape so the inspector can select its lines while
    // still leaving the implicit panel read-only (there is no @panel header to rewrite safely).
    if (!panel && parsed.key === "title") {
      panel = { name: "main", col: 0, row: 0, spanX: 1, spanY: 1, headerIndex: null, directives: {}, lines: [], implicit: true };
      screen.panels.push(panel);
    }
    if (panel && PANEL_KEYS.has(parsed.key)) panel.directives[parsed.key] = { index: i, value: parsed.value, inline: false };
    else if (SCREEN_KEYS.has(parsed.key)) screen.directives[parsed.key] = { index: i, value: parsed.value, inline: false };
  }
  return model;
}

function serialize(lines, newline) { return lines.join(newline); }
function cleanText(value) { return String(value ?? "").replace(/[\r\n]/g, " ").trim(); }
function cleanQuoted(value) { return cleanText(value).replace(/"/g, "'"); }

function replaceLine(lines, index, content) {
  const indent = (lines[index].match(/^\s*/) || [""])[0];
  lines[index] = indent + content;
}

function screenInsertIndex(screen) {
  const candidates = screen.panels.flatMap((p) => [p.headerIndex, ...p.lines.map((line) => line.index)]).filter((x) => x !== null && x !== undefined);
  return candidates.length ? Math.min(...candidates) : screen.endIndex;
}

export function setScreenDirective(dsl, screenIndex, key, value) {
  const model = parseDslModel(dsl), screen = model.screens[screenIndex];
  if (!screen) return dsl;
  const lines = [...model.lines];
  value = cleanText(value);
  if (key === "name") {
    if (value) replaceLine(lines, screen.headerIndex, `@screen ${value}`);
    return serialize(lines, model.newline);
  }
  const existing = screen.directives[key] || (key === "screen_bg" ? screen.directives.bg || screen.directives.background : null);
  if (existing && !existing.inline) {
    if (value) replaceLine(lines, existing.index, `${key}: ${value}`);
    else lines.splice(existing.index, 1);
  } else if (value) {
    lines.splice(screenInsertIndex(screen), 0, `${key}: ${value}`);
  }
  return serialize(lines, model.newline);
}

function replaceInlineField(line, key, value) {
  const escaped = key.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
  const pattern = key === "title"
    ? new RegExp(`\\s+${escaped}:\\s*"[^"]*"`, "i")
    : new RegExp(`\\s+${escaped}:\\s*[^\\s]+`, "i");
  const replacement = value ? ` ${key}:${key === "title" ? ` "${cleanQuoted(value)}"` : cleanText(value)}` : "";
  return pattern.test(line) ? line.replace(pattern, replacement) : line + replacement;
}

export function setPanelField(dsl, screenIndex, panelIndex, key, value) {
  const model = parseDslModel(dsl), panel = model.screens[screenIndex]?.panels[panelIndex];
  if (!panel || panel.headerIndex === null) return dsl;
  const lines = [...model.lines];
  value = cleanText(value);
  let header = lines[panel.headerIndex];
  if (key === "name") header = header.replace(/(@panel\s+)\S+/i, `$1${value || panel.name}`);
  else if (["col", "row", "spanX", "spanY"].includes(key)) {
    const next = { col: panel.col, row: panel.row, spanX: panel.spanX, spanY: panel.spanY, [key]: Math.max(key.startsWith("span") ? 1 : 0, Number(value) || 0) };
    header = header.replace(/at\([^)]*\)/i, `at(${next.col}, ${next.row}, ${next.spanX}, ${next.spanY})`);
  } else {
    const existing = panel.directives[key] || (key === "bg" ? panel.directives.background : null);
    if (existing && !existing.inline) {
      if (value) replaceLine(lines, existing.index, `${key}: ${key === "title" ? `"${cleanQuoted(value)}"` : value}`);
      else lines.splice(existing.index, 1);
      return serialize(lines, model.newline);
    }
    header = replaceInlineField(header, key, value);
  }
  lines[panel.headerIndex] = header;
  return serialize(lines, model.newline);
}

function replaceOption(raw, name, value, quoted = false) {
  const existing = findCall(raw, name);
  const replacement = value ? `${name}(${quoted ? `"${cleanQuoted(value)}"` : cleanText(value)})` : "";
  if (!existing) return replacement ? raw + " " + replacement : raw;
  const leadingSpace = existing.start > 0 && /\s/.test(raw[existing.start - 1]);
  return raw.slice(0, existing.start - (leadingSpace ? 1 : 0)) + (replacement ? " " + replacement : "") + raw.slice(existing.end);
}

export function setLineField(dsl, screenIndex, panelIndex, lineIndex, key, value) {
  const model = parseDslModel(dsl), line = model.screens[screenIndex]?.panels[panelIndex]?.lines[lineIndex];
  if (!line?.parsed) return dsl;
  const lines = [...model.lines];
  let raw = lines[line.index];
  if (key === "label") raw = raw.replace(/^(\s*line:\s*)"[^"]*"/i, `$1"${cleanQuoted(value)}"`);
  else if (key === "kind") raw = raw.replace(/("[^"]*"\s+)[A-Za-z_]\w*(\()/, `$1${cleanText(value) || line.kind}$2`);
  else if (key === "ref") raw = raw.replace(/("[^"]*"\s+[A-Za-z_]\w*\()[^)]*(\))/, `$1${cleanText(value)}$2`);
  else if (key === "colorMode" || key === "colors") {
    let mode = key === "colorMode" ? value : line.options.colorMode;
    let colors = key === "colors" ? value : line.options.colors;
    if (mode && mode !== "none" && !colors) colors = mode === "gradient" ? "#22c55e, #ef4444" : "#38bdf8";
    if ((!mode || mode === "none") && colors) mode = "flat";
    for (const optionName of ["flat", "gradient"]) {
      let call;
      while ((call = findCall(raw, optionName))) {
        const leadingSpace = call.start > 0 && /\s/.test(raw[call.start - 1]);
        raw = raw.slice(0, call.start - (leadingSpace ? 1 : 0)) + raw.slice(call.end);
      }
    }
    if (mode && mode !== "none" && colors) raw = replaceOption(raw, mode, colors);
  } else if (key === "dir") {
    const pattern = /\s+dir:\s*[\w-]+/i;
    const replacement = value ? ` dir: ${cleanText(value)}` : "";
    raw = pattern.test(raw) ? raw.replace(pattern, replacement) : raw + replacement;
  } else {
    const map = { unit: ["unit", true], size: ["size", false], labelColor: ["label_color", false], track: ["track", false] };
    if (map[key]) raw = replaceOption(raw, map[key][0], value, map[key][1]);
  }
  lines[line.index] = raw;
  return serialize(lines, model.newline);
}

export function addPanel(dsl, screenIndex) {
  const model = parseDslModel(dsl), screen = model.screens[screenIndex];
  if (!screen) return dsl;
  const lines = [...model.lines], index = screen.endIndex;
  const number = screen.panels.length + 1;
  lines.splice(index, 0, `@panel panel_${number} at(0, 0, 1, 1) title: "PANEL ${number}"`);
  return serialize(lines, model.newline);
}

export function addScreen(dsl) {
  const model = parseDslModel(dsl), newline = model.newline;
  let number = model.screens.length + 1;
  const names = new Set(model.screens.map((screen) => screen.name.toLowerCase()));
  while (names.has(`screen_${number}`)) number++;
  const prefix = dsl.trim() ? dsl.replace(/\s+$/, "") + newline + newline : "";
  return prefix + [
    `@screen screen_${number}`,
    `output: "LCD ${number}", 0`,
    "profile: auto",
    "layout: grid(1, 1)",
    "screen_bg: #0a0c10",
    "border: frame",
    "border_color: #38bdf8",
    `@panel main_${number} at(0, 0, 1, 1) title: "SCREEN ${number}"`,
    "",
  ].join(newline);
}

export function createBaseConfig(dsl = "") {
  const newline = dsl.includes("\r\n") ? "\r\n" : "\n";
  const prefix = dsl.trim() ? dsl.replace(/\s+$/, "") + newline + newline : "";
  return prefix + [
    "@bind demo = dynamic(\"helm-toolbox.demo\", default: 50)",
    "@screen Main",
    "output: \"Main LCD\", 0",
    "profile: auto",
    "layout: grid(1, 1)",
    "font: Monospace",
    "font_size: 1",
    "screen_bg: #0a0c10",
    "border: frame",
    "border_color: #38bdf8",
    "padding: 8",
    "gap: 8",
    "@panel main at(0, 0, 1, 1) title: \"SYSTEM STATUS\" title_align: left title_color: #f8fafc",
    "line: \"VALUE\" bar(demo.percent) flat(#38bdf8)",
    "",
  ].join(newline);
}

export function parseOutput(value = "") {
  const match = value.match(/^\s*"([^"]*)"\s*(?:,\s*(\d+))?/);
  return { name: match?.[1] || value.split(",")[0].replace(/"/g, "").trim(), surface: +(match?.[2] || 0) };
}

export function parseGrid(value = "") {
  const match = value.match(/grid\(\s*(\d+)\s*,\s*(\d+)\s*\)/i);
  return { cols: +(match?.[1] || 1), rows: +(match?.[2] || 1) };
}

export function panelInsertionOffset(dsl, screenIndex, panelIndex) {
  const model = parseDslModel(dsl), screen = model.screens[screenIndex], panel = screen?.panels[panelIndex];
  if (!screen || !panel) return dsl.length;
  const nextPanel = screen.panels[panelIndex + 1];
  const targetLine = nextPanel ? (nextPanel.headerIndex ?? nextPanel.lines[0]?.index ?? screen.endIndex) : screen.endIndex;
  let offset = 0;
  for (let i = 0; i < targetLine; i++) offset += model.lines[i].length + model.newline.length;
  return Math.min(offset, dsl.length);
}
