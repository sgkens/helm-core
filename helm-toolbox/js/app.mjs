// app.mjs: wires the helm-toolbox page together. No layout logic lives here (TASK-39.1/39.5/39.7):
// the DSL text is the source of truth, the WebAssembly build of the real script (wasm-bridge.mjs)
// renders it, and sprite-svg.mjs + se-lcd-bezel.mjs draw the result exactly as the docs preview does.
import { buildBezel, PROFILES } from "../../assets/js/se-lcd-bezel.mjs";
import { renderSprites } from "../../assets/js/sprite-svg.mjs";
import { render as wasmRender, listTemplates } from "./wasm-bridge.mjs";
import { applyPaletteInsertion } from "./dsl-insertion.mjs";
import {
  addPanel, addScreen, createBaseConfig, panelInsertionOffset, parseDslModel, parseGrid, parseOutput,
  setLineField, setPanelField, setScreenDirective,
} from "./dsl-model.mjs";
import { Lume } from "../vendor/lume.mjs";
import logcad from "../vendor/logcad.mjs";
import { initializeI18n, t } from "./i18n.mjs";

await initializeI18n();

// This module lives in helm-toolbox/js/; data/ is its sibling. Resolved from import.meta.url (not a
// hardcoded "/helm-toolbox/"), so it works whether the page is served at the site root (local dev) or
// under /helm-toolbox/ (production, alongside the docs site).
const DATA_BASE = new URL("../data/", import.meta.url).pathname;
const ASSET_BASE = new URL("../../assets/", import.meta.url).pathname;
const PROJECTS_KEY = "helm-toolbox_projects_v1";
const EDITOR_HEIGHT_KEY = "helm-toolbox_editor_height_v1";
const DSL_KEY = "helm-toolbox_dsl";
// Carry browser-local work across the product rename. Constructing the previous prefix in two
// pieces keeps the retired name out of current UI/docs searches while retaining one-way migration.
const LEGACY_STORAGE_PREFIX = "web" + "util";

const $ = (sel, root = document) => root.querySelector(sel);
const $$ = (sel, root = document) => Array.from(root.querySelectorAll(sel));

function canonicalDslProfile(value) {
  const p = String(value || "standard").trim().toLowerCase().replace(/[_\s-]/g, "");
  if (p === "textpanel") return "standard";
  if (p === "widelcd") return "wide";
  if (p === "cockpitsmall") return "compact";
  if (p === "cornerlcd" || p === "cockpitcorner") return "corner";
  return p;
}

const state = {
  dsl: "",
  profile: "TextPanel",
  screenIndex: 0,
  values: {}, // bindName -> 0..100
  seconds: 0,
  playing: true,
  alert: false, // preview of the "alert red" run-argument command
  lastResult: null,
};

let blocks = null, itemFields = [], starters = [], components = [], commonOptions = [];
let projects = {};
let currentStarterName = "";
let currentProjectName = ""; // the saved project the editor currently reflects, if any; "" means unsaved/not loaded from one
let dslDirty = false; // true once the loaded content has been edited; guards the starter/project pickers from silently discarding it
let debounceTimer = null;
let renderToken = 0;
let inspectorModel = null;
const inspectorSelection = { screen: 0, panel: 0, line: 0 };
const logger = { name: "helm-toolbox", logname: " HELM ", bg: "#0891b2", logbg: "#0f172a" };

new Lume({ motion: "lift", showDelay: 120, hideDelay: 40, offset: 10 });

async function loadData() {
  const [b, s] = await Promise.all([
    fetch(DATA_BASE + "blocks.json").then((r) => r.json()),
    fetch(DATA_BASE + "starters.json").then((r) => r.json()),
  ]);
  blocks = b.blocks;
  itemFields = b.itemFields;
  starters = s;
  // Reference-only: the Components tab is supplementary, so a missing or stale components.json
  // (e.g. gen-components.js not yet run) degrades to an empty tab rather than failing the whole page.
  try {
    const c = await fetch(DATA_BASE + "components.json").then((r) => r.json());
    components = c.components;
    commonOptions = c.commonOptions;
  } catch (e) {
    logcad.logd(logger, "data", "Could not load components.json", { message: e.message });
  }
}

function insertAtCursor(text) {
  const ta = $("#dsl-editor");
  const start = ta.selectionStart, end = ta.selectionEnd;
  ta.value = ta.value.slice(0, start) + text + ta.value.slice(end);
  ta.selectionStart = ta.selectionEnd = start + text.length;
  ta.focus();
  dslDirty = true;
  onDslChanged();
}

function insertPaletteSnippet(snippet, suggestions = {}) {
  const ta = $("#dsl-editor");
  if (!inspectorModel?.screens.length) {
    ta.value = createBaseConfig();
    state.dsl = ta.value;
    inspectorSelection.screen = inspectorSelection.panel = inspectorSelection.line = 0;
    inspectorModel = parseDslModel(ta.value);
  }
  const offset = panelInsertionOffset(ta.value, inspectorSelection.screen, inspectorSelection.panel);
  const prefix = offset > 0 && ta.value[offset - 1] !== "\n" && ta.value[offset - 1] !== "\r" ? "\n" : "";
  const result = applyPaletteInsertion(ta.value, prefix + snippet, offset, offset, suggestions);
  ta.value = result.text;
  ta.selectionStart = ta.selectionEnd = result.selection;
  ta.focus();
  dslDirty = true;
  onDslChanged();
}

// ---- Preview ---------------------------------------------------------

function scheduleRender() {
  clearTimeout(debounceTimer);
  debounceTimer = setTimeout(doRender, 200);
}

async function doRender() {
  const stage = $("#preview-stage");
  const prof = PROFILES[state.profile] || PROFILES.TextPanel;
  const myToken = ++renderToken;
  let result;
  try {
    $("#preview-status").textContent = t("preview.rendering");
    result = await wasmRender(state.dsl, prof.w, prof.h, state.values, state.seconds, state.screenIndex, state.alert);
  } catch (e) {
    if (myToken !== renderToken) return;
    $("#preview-status").textContent = t("preview.loadFailed", { message: e.message });
    logcad.logd(logger, "render", "Renderer failed", { message: e.message });
    return;
  }
  if (myToken !== renderToken) return; // a newer render started while this one was in flight

  state.lastResult = result;
  $("#preview-status").textContent = "";
  renderIssues(result.errors || [], result.warnings || []);
  renderValueSliders(result.binds || []);

  // Host.cs's own catch block (an unexpected exception, not a normal DSL parse error) returns only
  // errors/warnings/screens/sprites, with no surface/texture/binds. Drawing must not assume they
  // exist, or a host-level crash leaves the UI stuck showing "Rendering..." with an unhandled
  // exception instead of the error message rendered above.
  if (!result.surface || !result.texture) {
    stage.innerHTML = `<p class="text-error font-mono text-sm p-2">${escapeHtml(t("preview.drawFailed"))}</p>`;
    return;
  }

  const bezel = buildBezel(state.profile, {});
  stage.innerHTML = bezel;
  const svg = stage.querySelector("svg");
  svg.removeAttribute("height");
  svg.setAttribute("role", "img");
  svg.setAttribute("aria-label", t("preview.aria", { profile: state.profile, screen: state.screenIndex + 1 }));
  const slot = svg.querySelector("#content-slot");
  const ox = (result.texture[0] - result.surface[0]) / 2;
  const oy = (result.texture[1] - result.surface[1]) / 2;
  renderSprites(slot, result.sprites, { ox, oy, fontChoice: "auto" });

  renderScreenPicker(result.screens || []);
}

function renderScreenPicker(screens) {
  const sel = $("#screen-picker");
  const wrap = $("#screen-picker-wrap");
  if (screens.length <= 1) { wrap.classList.add("hidden"); return; }
  wrap.classList.remove("hidden");
  // Keyed by the names themselves, not just the count: two DSLs (e.g. a shared link replacing a
  // starter) can have the same screen count with different @screen names, which must still refresh
  // the option labels. @screen accepts any text (HelmCore.cs takes the rest of the line verbatim),
  // so it is escaped here like any other DSL-derived string reaching innerHTML.
  const key = screens.join("\u0001");
  if (sel.dataset.key !== key) {
    sel.innerHTML = screens.map((n, i) => `<option value="${i}">${escapeHtml(n)}</option>`).join("");
    sel.dataset.key = key;
  }
  sel.value = String(Math.min(state.screenIndex, screens.length - 1));
}

function renderIssues(errors, warnings) {
  const box = $("#issues");
  if (errors.length === 0 && warnings.length === 0) { box.innerHTML = ""; box.classList.add("hidden"); return; }
  box.classList.remove("hidden");
  box.innerHTML =
    errors.map((e) => `<div class="text-error font-mono text-xs">${escapeHtml(t("issues.error"))}: ${escapeHtml(e)}</div>`).join("") +
    warnings.map((w) => `<div class="text-warning font-mono text-xs">${escapeHtml(t("issues.warning"))}: ${escapeHtml(w)}</div>`).join("");
}

function renderValueSliders(binds) {
  const box = $("#value-sliders");
  const names = binds.map((b) => b.name).join(",");
  if (box.dataset.names === names) return; // same binds as last render: keep the sliders (and their values) as they are
  box.dataset.names = names;
  if (binds.length === 0) { box.innerHTML = `<p class="text-xs text-base-content/50 font-mono">${escapeHtml(t("empty.binds"))}</p>`; return; }
  box.innerHTML = binds.map((b) => {
    const v = state.values[b.name] ?? 50;
    if (b.isText) return "";
    return `<label class="flex items-center gap-2 text-xs font-mono">
      <span class="w-20 truncate" tabindex="0" data-lume-title="${escapeAttr(b.name)}" data-lume="${escapeAttr(b.kind)}(&quot;${escapeAttr(b.target)}&quot;)" data-lume-direction="left">${escapeHtml(b.name)}</span>
      <input type="range" min="0" max="100" value="${v}" class="range range-xs range-primary flex-1" data-bind="${escapeAttr(b.name)}">
      <span class="w-9 text-right tabular-nums" data-bind-val="${escapeAttr(b.name)}">${v}</span>
    </label>`;
  }).join("");
  $$("input[data-bind]", box).forEach((inp) => {
    inp.addEventListener("input", () => {
      state.values[inp.dataset.bind] = +inp.value;
      $(`[data-bind-val="${cssEscape(inp.dataset.bind)}"]`, box).textContent = inp.value;
      scheduleRender();
    });
  });
}

function cssEscape(s) { return s.replace(/[^a-zA-Z0-9_-]/g, (c) => "\\" + c); }
function escapeHtml(s) { return s.replace(/[&<>]/g, (c) => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;" }[c])); }
function escapeAttr(s) { return escapeHtml(s).replace(/"/g, "&quot;"); }
function highlightHelm(source) {
  if (window.Prism && window.Prism.languages.helm) return window.Prism.highlight(source, window.Prism.languages.helm, "helm");
  return escapeHtml(source);
}

function updateDslHighlight() {
  $("#dsl-highlight-code").innerHTML = highlightHelm(state.dsl) + (state.dsl.endsWith("\n") ? " " : "");
  syncEditorScroll();
}

function syncEditorScroll() {
  const editor = $("#dsl-editor"), highlight = $("#dsl-highlight");
  highlight.scrollTop = editor.scrollTop;
  highlight.scrollLeft = editor.scrollLeft;
}

function directiveValue(owner, key, fallback = "") {
  return owner?.directives?.[key]?.value ?? fallback;
}

function setInspectorControl(selector, value) {
  const control = $(selector);
  if (!control) return;
  value = String(value ?? "");
  if (control.tagName === "SELECT" && value && !Array.from(control.options).some((option) => option.value === value)) {
    const old = control.querySelector("option[data-custom]");
    if (old) old.remove();
    control.add(new Option(value, value));
    control.lastElementChild.dataset.custom = "true";
  }
  control.value = value;
}

function syncColorPicker(selector, value) {
  const picker = $(selector);
  const first = String(value || "").split(",")[0].trim();
  if (/^#[0-9a-f]{6}$/i.test(first)) picker.value = first;
}

function cursorLineNumber() {
  const ta = $("#dsl-editor");
  return ta.value.slice(0, ta.selectionStart).split(/\r?\n/).length - 1;
}

function followInspectorCursor(model) {
  const at = cursorLineNumber();
  const screenIndex = model.screens.findIndex((screen) => at >= screen.headerIndex && at <= screen.endIndex);
  if (screenIndex < 0) return;
  inspectorSelection.screen = screenIndex;
  const screen = model.screens[screenIndex];
  let panelIndex = screen.panels.findIndex((panel, index) => {
    const start = panel.headerIndex ?? panel.lines[0]?.index ?? screen.headerIndex;
    const next = screen.panels[index + 1];
    const end = next ? (next.headerIndex ?? next.lines[0]?.index ?? screen.endIndex) - 1 : screen.endIndex;
    return at >= start && at <= end;
  });
  if (panelIndex < 0) panelIndex = 0;
  inspectorSelection.panel = panelIndex;
  const lines = screen.panels[panelIndex]?.lines || [];
  const exact = lines.findIndex((line) => line.index === at);
  if (exact >= 0) inspectorSelection.line = exact;
  else {
    const preceding = lines.map((line, index) => ({ line, index })).filter((entry) => entry.line.index <= at).pop();
    if (preceding) inspectorSelection.line = preceding.index;
  }
}

function setInspectorDisabled(rootSelector, disabled) {
  $$("input, select, button", $(rootSelector)).forEach((control) => { control.disabled = disabled; });
}

function refreshInspector(followCursor = false) {
  inspectorModel = parseDslModel(state.dsl);
  if (followCursor) followInspectorCursor(inspectorModel);
  inspectorSelection.screen = Math.max(0, Math.min(inspectorSelection.screen, inspectorModel.screens.length - 1));
  const screen = inspectorModel.screens[inspectorSelection.screen];
  const screenSelect = $("#config-screen");
  screenSelect.innerHTML = inspectorModel.screens.length
    ? inspectorModel.screens.map((item, index) => `<option value="${index}">${escapeHtml(item.name || `Screen ${index + 1}`)}</option>`).join("")
    : `<option value="">${escapeHtml(t("empty.screens"))}</option>`;
  screenSelect.value = screen ? String(inspectorSelection.screen) : "";
  setInspectorDisabled("#base-config-fields", !screen);
  if (!screen) {
    $("#config-panel").innerHTML = `<option value="">${escapeHtml(t("empty.panels"))}</option>`;
    $("#config-line").innerHTML = `<option value="">${escapeHtml(t("empty.components"))}</option>`;
    setInspectorDisabled("#panel-config-fields", true);
    setInspectorDisabled("#line-config-fields", true);
    return;
  }

  const output = parseOutput(directiveValue(screen, "output"));
  const grid = parseGrid(directiveValue(screen, "layout"));
  const screenValues = {
    name: screen.name, outputName: output.name, outputSurface: output.surface,
    profile: canonicalDslProfile(directiveValue(screen, "profile", "standard")), gridCols: grid.cols, gridRows: grid.rows,
    font: directiveValue(screen, "font", "Monospace"), font_size: directiveValue(screen, "font_size", "1"),
    border: directiveValue(screen, "border", "graphic"), padding: directiveValue(screen, "padding", "8"),
    gap: directiveValue(screen, "gap", "8"),
    screen_bg: directiveValue(screen, "screen_bg", directiveValue(screen, "bg", directiveValue(screen, "background", "#0a0c10"))),
    border_color: directiveValue(screen, "border_color", "#283c55"),
  };
  for (const [key, value] of Object.entries(screenValues)) setInspectorControl(`[data-base-field="${key}"]`, value);
  syncColorPicker('[data-color-picker="screen_bg"]', screenValues.screen_bg);
  syncColorPicker('[data-color-picker="border_color"]', screenValues.border_color);

  inspectorSelection.panel = Math.max(0, Math.min(inspectorSelection.panel, screen.panels.length - 1));
  const panel = screen.panels[inspectorSelection.panel];
  $("#config-panel").innerHTML = screen.panels.length
    ? screen.panels.map((item, index) => `<option value="${index}">${escapeHtml(item.name || `Panel ${index + 1}`)}${item.implicit ? " (implicit)" : ""}</option>`).join("")
    : `<option value="">${escapeHtml(t("empty.panels"))}</option>`;
  $("#config-panel").value = panel ? String(inspectorSelection.panel) : "";
  $("#implicit-panel-note").classList.toggle("hidden", !panel?.implicit);
  setInspectorDisabled("#panel-config-fields", !panel || panel.implicit);
  if (panel) {
    const panelValues = {
      name: panel.name, title: directiveValue(panel, "title"), col: panel.col, row: panel.row,
      spanX: panel.spanX, spanY: panel.spanY, title_align: directiveValue(panel, "title_align", "left"),
      orientation: directiveValue(panel, "orientation", "horizontal"), border: directiveValue(panel, "border"),
      title_color: directiveValue(panel, "title_color"),
      bg: directiveValue(panel, "bg", directiveValue(panel, "background")), border_color: directiveValue(panel, "border_color"),
    };
    for (const [key, value] of Object.entries(panelValues)) setInspectorControl(`[data-panel-field="${key}"]`, value);
    syncColorPicker('[data-panel-color-picker="title_color"]', panelValues.title_color);
    syncColorPicker('[data-panel-color-picker="bg"]', panelValues.bg);
    syncColorPicker('[data-panel-color-picker="border_color"]', panelValues.border_color);
  }

  inspectorSelection.line = Math.max(0, Math.min(inspectorSelection.line, (panel?.lines.length || 0) - 1));
  const line = panel?.lines[inspectorSelection.line];
  $("#config-line").innerHTML = panel?.lines.length
    ? panel.lines.map((item, index) => `<option value="${index}">${escapeHtml(item.label || `Line ${index + 1}`)} · ${escapeHtml(item.kind || "custom")}</option>`).join("")
    : `<option value="">${escapeHtml(t("empty.panelComponents"))}</option>`;
  $("#config-line").value = line ? String(inspectorSelection.line) : "";
  $("#unparsed-line-note").classList.toggle("hidden", !line || line.parsed);
  setInspectorDisabled("#line-config-fields", !line?.parsed);
  if (line?.parsed) {
    const lineValues = { label: line.label, kind: line.kind, ref: line.ref, ...line.options };
    for (const [key, value] of Object.entries(lineValues)) setInspectorControl(`[data-line-field="${key}"]`, value);
    syncColorPicker('[data-line-color-picker="colors"]', line.options.colors);
    syncColorPicker('[data-line-color-picker="labelColor"]', line.options.labelColor);
    syncColorPicker('[data-line-color-picker="track"]', line.options.track);
  }
}

function showInspectorTab(name) {
  $$('[data-inspector-tab]').forEach((tab) => {
    const active = tab.dataset.inspectorTab === name;
    tab.classList.toggle("tab-active", active);
    tab.setAttribute("aria-selected", String(active));
  });
  $$('[data-inspector-panel]').forEach((panel) => panel.classList.toggle("hidden", panel.dataset.inspectorPanel !== name));
}

function applyInspectorDsl(dsl) {
  const ta = $("#dsl-editor"), selection = Math.min(ta.selectionStart, dsl.length);
  ta.value = dsl;
  ta.selectionStart = ta.selectionEnd = selection;
  dslDirty = true;
  currentStarterName = "";
  $("#starter-picker").selectedIndex = -1;
  onDslChanged(false);
}

function wireInspector() {
  $$('[data-inspector-tab]').forEach((tab) => tab.addEventListener("click", () => showInspectorTab(tab.dataset.inspectorTab)));
  $("#config-screen").addEventListener("change", (event) => {
    inspectorSelection.screen = +event.target.value || 0;
    inspectorSelection.panel = inspectorSelection.line = 0;
    state.screenIndex = inspectorSelection.screen;
    $("#screen-picker").value = String(state.screenIndex);
    refreshInspector(false);
    scheduleRender();
  });
  $("#config-panel").addEventListener("change", (event) => { inspectorSelection.panel = +event.target.value || 0; inspectorSelection.line = 0; refreshInspector(false); });
  $("#config-line").addEventListener("change", (event) => { inspectorSelection.line = +event.target.value || 0; refreshInspector(false); });

  $("#new-config").addEventListener("click", () => {
    if (state.dsl.trim() && !confirm("Replace the current layout with a new configuration?")) return;
    inspectorSelection.screen = inspectorSelection.panel = inspectorSelection.line = 0;
    replaceDsl(createBaseConfig());
    dslDirty = true;
    currentProjectName = "";
    $("#project-name").value = "";
    $("#project-picker").value = "";
    showInspectorTab("base");
  });
  $("#add-screen").addEventListener("click", () => {
    const nextScreen = inspectorModel?.screens.length || 0;
    applyInspectorDsl(addScreen(state.dsl));
    inspectorSelection.screen = nextScreen;
    inspectorSelection.panel = inspectorSelection.line = 0;
    state.screenIndex = nextScreen;
    refreshInspector(false);
    showInspectorTab("base");
  });
  $("#add-panel").addEventListener("click", () => {
    const screen = inspectorModel?.screens[inspectorSelection.screen];
    if (!screen) return;
    const nextPanel = screen.panels.length;
    applyInspectorDsl(addPanel(state.dsl, inspectorSelection.screen));
    inspectorSelection.panel = nextPanel;
    inspectorSelection.line = 0;
    refreshInspector(false);
    showInspectorTab("panel");
  });

  $$('[data-base-field]').forEach((control) => control.addEventListener("change", () => {
    const key = control.dataset.baseField;
    let dsl = state.dsl;
    if (key === "outputName" || key === "outputSurface") {
      const name = $('[data-base-field="outputName"]').value.replace(/"/g, "'");
      const surface = Math.max(0, +$('[data-base-field="outputSurface"]').value || 0);
      dsl = setScreenDirective(dsl, inspectorSelection.screen, "output", `"${name}", ${surface}`);
    } else if (key === "gridCols" || key === "gridRows") {
      const cols = Math.max(1, +$('[data-base-field="gridCols"]').value || 1);
      const rows = Math.max(1, +$('[data-base-field="gridRows"]').value || 1);
      dsl = setScreenDirective(dsl, inspectorSelection.screen, "layout", `grid(${cols}, ${rows})`);
    } else dsl = setScreenDirective(dsl, inspectorSelection.screen, key, control.value);
    applyInspectorDsl(dsl);
  }));
  $$('[data-panel-field]').forEach((control) => control.addEventListener("change", () => {
    applyInspectorDsl(setPanelField(state.dsl, inspectorSelection.screen, inspectorSelection.panel, control.dataset.panelField, control.value));
  }));
  $$('[data-line-field]').forEach((control) => control.addEventListener("change", () => {
    applyInspectorDsl(setLineField(state.dsl, inspectorSelection.screen, inspectorSelection.panel, inspectorSelection.line, control.dataset.lineField, control.value));
  }));

  for (const [pickerSelector, fieldSelector] of [
    ['[data-color-picker="screen_bg"]', '[data-base-field="screen_bg"]'],
    ['[data-color-picker="border_color"]', '[data-base-field="border_color"]'],
    ['[data-panel-color-picker="title_color"]', '[data-panel-field="title_color"]'],
    ['[data-panel-color-picker="bg"]', '[data-panel-field="bg"]'],
    ['[data-panel-color-picker="border_color"]', '[data-panel-field="border_color"]'],
    ['[data-line-color-picker="colors"]', '[data-line-field="colors"]'],
    ['[data-line-color-picker="labelColor"]', '[data-line-field="labelColor"]'],
    ['[data-line-color-picker="track"]', '[data-line-field="track"]'],
  ]) {
    $(pickerSelector).addEventListener("input", (event) => {
      const field = $(fieldSelector);
      if (field.dataset.lineField === "colors" && field.value.includes(",")) {
        const stops = field.value.split(","); stops[0] = event.target.value; field.value = stops.join(",");
      } else field.value = event.target.value;
      field.dispatchEvent(new Event("change", { bubbles: true }));
    });
  }
}

function wirePaneResizer() {
  const grid = $(".helm-toolbox-grid"), handle = $("#pane-resizer"), toggle = $("#expand-editor");
  const MIN_EDITOR = 160, MIN_PREVIEW = 220, DEFAULT_EDITOR = 220;
  let pointerId = null, startY = 0, startHeight = DEFAULT_EDITOR, restoreHeight = DEFAULT_EDITOR;

  function limits() {
    const rowGap = parseFloat(getComputedStyle(grid).rowGap) || 0;
    const dividerHeight = handle.getBoundingClientRect().height || 12;
    return { min: MIN_EDITOR, max: Math.max(MIN_EDITOR, grid.clientHeight - MIN_PREVIEW - dividerHeight - rowGap * 2) };
  }

  function currentHeight() {
    return parseFloat(getComputedStyle(grid).getPropertyValue("--editor-height")) || DEFAULT_EDITOR;
  }

  function setEditorHeight(height, persist = false) {
    const { min, max } = limits();
    const value = Math.round(Math.min(max, Math.max(min, height)));
    grid.style.setProperty("--editor-height", `${value}px`);
    handle.setAttribute("aria-valuemin", String(min));
    handle.setAttribute("aria-valuemax", String(Math.round(max)));
    handle.setAttribute("aria-valuenow", String(value));
    const expanded = value >= max - 2;
    toggle.textContent = expanded ? t("preview.restore") : t("action.expand");
    toggle.setAttribute("aria-expanded", String(expanded));
    if (persist) {
      try { localStorage.setItem(EDITOR_HEIGHT_KEY, String(value)); } catch (e) {}
    }
    return value;
  }

  function toggleExpanded() {
    const { max } = limits(), current = currentHeight();
    if (current >= max - 2) setEditorHeight(restoreHeight, true);
    else {
      restoreHeight = current;
      setEditorHeight(max, true);
    }
  }

  try {
    const saved = Number(readStoredValue(EDITOR_HEIGHT_KEY, "editor_height_v1"));
    setEditorHeight(Number.isFinite(saved) && saved > 0 ? saved : DEFAULT_EDITOR);
  } catch (e) { setEditorHeight(DEFAULT_EDITOR); }

  handle.addEventListener("pointerdown", (event) => {
    if (event.button !== 0) return;
    pointerId = event.pointerId;
    startY = event.clientY;
    startHeight = currentHeight();
    restoreHeight = startHeight;
    handle.setPointerCapture(pointerId);
    document.body.classList.add("pane-resizing");
    event.preventDefault();
  });
  handle.addEventListener("pointermove", (event) => {
    if (event.pointerId !== pointerId) return;
    setEditorHeight(startHeight + startY - event.clientY);
  });
  function finishResize(event) {
    if (event.pointerId !== pointerId) return;
    pointerId = null;
    document.body.classList.remove("pane-resizing");
    setEditorHeight(currentHeight(), true);
  }
  handle.addEventListener("pointerup", finishResize);
  handle.addEventListener("pointercancel", finishResize);
  handle.addEventListener("lostpointercapture", finishResize);
  handle.addEventListener("dblclick", toggleExpanded);
  handle.addEventListener("keydown", (event) => {
    const step = event.shiftKey ? 50 : 20;
    if (event.key === "ArrowUp") setEditorHeight(currentHeight() + step, true);
    else if (event.key === "ArrowDown") setEditorHeight(currentHeight() - step, true);
    else if (event.key === "Home") setEditorHeight(limits().min, true);
    else if (event.key === "End") { restoreHeight = currentHeight(); setEditorHeight(limits().max, true); }
    else return;
    event.preventDefault();
  });
  toggle.addEventListener("click", toggleExpanded);
  window.addEventListener("resize", () => setEditorHeight(currentHeight()));
}

function readStoredValue(key, legacySuffix) {
  const current = localStorage.getItem(key);
  if (current !== null) return current;
  const legacy = localStorage.getItem(`${LEGACY_STORAGE_PREFIX}_${legacySuffix}`);
  if (legacy !== null) localStorage.setItem(key, legacy);
  return legacy;
}

function readProjects() {
  try {
    const value = JSON.parse(readStoredValue(PROJECTS_KEY, "projects_v1") || "{}");
    return value && typeof value === "object" && !Array.isArray(value) ? value : {};
  } catch (e) { return {}; }
}

function writeProjects() {
  try { localStorage.setItem(PROJECTS_KEY, JSON.stringify(projects)); return true; } catch (e) { return false; }
}

function refreshProjectPicker(selected = "") {
  const picker = $("#project-picker");
  const names = Object.keys(projects).sort((a, b) => a.localeCompare(b));
  picker.innerHTML = `<option value="">${escapeHtml(t("editor.savedProjects"))}</option>` + names.map((name) =>
    `<option value="${escapeAttr(name)}">${escapeHtml(name)}</option>`).join("");
  picker.value = names.includes(selected) ? selected : "";
}

function replaceDsl(dsl) {
  $("#dsl-editor").value = dsl;
  state.values = {};
  state.screenIndex = 0;
  onDslChanged();
  dslDirty = false; // baseline: this is what was just loaded, not yet edited
}

function saveProject() {
  const name = $("#project-name").value.trim();
  if (!name) { flashButton($("#save-project"), t("action.nameRequired")); $("#project-name").focus(); return; }
  // Only the project this editor was actually loaded from (or just saved as) skips confirmation;
  // typing a different, colliding name is most often a "save as" typo, not an intended overwrite.
  if (name !== currentProjectName && projects[name] && !confirm(`Overwrite the saved project "${name}"?`)) return;
  projects[name] = { dsl: state.dsl, profile: state.profile, updated: new Date().toISOString() };
  if (!writeProjects()) { flashButton($("#save-project"), t("action.storageFailed")); return; }
  currentProjectName = name;
  refreshProjectPicker(name);
  flashButton($("#save-project"), "Saved");
}

function deleteProject() {
  const name = $("#project-picker").value || $("#project-name").value.trim();
  if (!name || !projects[name]) { flashButton($("#delete-project"), "Nothing selected"); return; }
  if (!confirm(`Delete the saved project "${name}"? This cannot be undone.`)) return;
  delete projects[name];
  if (!writeProjects()) { flashButton($("#delete-project"), "Storage failed"); return; }
  if (currentProjectName === name) {
    currentProjectName = "";
    $("#project-name").value = "";
  }
  refreshProjectPicker();
  flashButton($("#delete-project"), "Deleted");
}

function loadProject(name) {
  const project = projects[name];
  if (!project || typeof project.dsl !== "string") return;
  $("#project-name").value = name;
  currentProjectName = name;
  if (project.profile && PROFILES[project.profile]) {
    state.profile = project.profile;
    $("#profile-picker").value = project.profile;
  }
  replaceDsl(project.dsl);
}

function projectFilename() {
  const base = $("#project-name").value.trim() || "helm-layout";
  return base.toLowerCase().replace(/[^a-z0-9]+/g, "-").replace(/^-|-$/g, "") || "helm-layout";
}

function exportDsl() {
  const blob = new Blob([state.dsl], { type: "text/plain;charset=utf-8" });
  const link = document.createElement("a");
  link.href = URL.createObjectURL(blob);
  link.download = `${projectFilename()}.txt`;
  link.click();
  setTimeout(() => URL.revokeObjectURL(link.href), 0);
  flashButton($("#export-dsl"), "Exported");
}

function encodeShare(text) {
  let binary = "";
  for (const byte of new TextEncoder().encode(text)) binary += String.fromCharCode(byte);
  return btoa(binary).replaceAll("+", "-").replaceAll("/", "_").replace(/=+$/, "");
}

function decodeShare() {
  if (!location.hash.startsWith("#dsl=")) return null;
  try {
    let encoded = location.hash.slice(5).replaceAll("-", "+").replaceAll("_", "/");
    while (encoded.length % 4) encoded += "=";
    const binary = atob(encoded);
    return new TextDecoder().decode(Uint8Array.from(binary, (char) => char.charCodeAt(0)));
  } catch (e) { return null; }
}

function onDslChanged(followInspector = true) {
  state.dsl = $("#dsl-editor").value;
  updateDslHighlight();
  try { localStorage.setItem(DSL_KEY, state.dsl); } catch (e) {}
  refreshInspector(followInspector);
  scheduleRender();
}

// ---- Block / field reference (click to insert a line:) ---------------

function aliasForBlock(name) {
  const known = {
    "Battery Block": "batt", "Cargo Container": "cargo", "Oxygen / Hydrogen Tank": "tank",
    "Thruster (Atmo / Ion / H2)": "thrusters", "Gyroscope": "gyro", "Air Vent": "vent",
    "Landing Gear / Magplate": "gear", "Gatling / Missile Turret": "turret", "Warhead": "bomb",
  };
  return known[name] || name.toLowerCase().replace(/\([^)]*\)/g, "").replace(/[^a-z0-9]+/g, "_").replace(/^_|_$/g, "") || "block";
}

function renderBlockPalette() {
  const box = $("#block-palette");
  const q = ($("#block-search").value || "").toLowerCase();
  const groups = new Map();
  for (const b of blocks) {
    if (q && !(b.name.toLowerCase().includes(q) || b.category.toLowerCase().includes(q) || b.fields.some((f) => f.includes(q)))) continue;
    if (!groups.has(b.category)) groups.set(b.category, []);
    groups.get(b.category).push(b);
  }
  box.innerHTML = Array.from(groups.entries()).map(([cat, list]) => `
    <div class="mb-3">
      <div class="text-[10px] uppercase tracking-wider text-base-content/40 font-mono mb-1">${escapeHtml(cat)}</div>
      ${list.map((b) => `
        <div class="ref-card p-2 mb-2">
          <div class="flex items-start gap-2">
            <a href="${escapeAttr(b.icon.source)}" target="_blank" rel="noopener" aria-label="Open ${escapeAttr(b.name)} icon source on the Official Space Engineers Wiki" class="shrink-0 rounded bg-base-300/40" data-lume="Icon source: Official Space Engineers Wiki" data-lume-direction="right">
              <img src="${ASSET_BASE}blocks/${escapeAttr(b.icon.slug)}.png" alt="" width="40" height="40" class="w-10 h-10 object-contain" loading="lazy">
            </a>
            <div class="min-w-0 flex-1">
              <div class="flex items-baseline justify-between gap-2 min-w-0">
                <span class="font-mono text-xs font-bold truncate" title="${escapeAttr(b.name)}">${escapeHtml(b.name)}</span>
                ${b.units ? `<span class="text-[10px] text-base-content/40 font-mono shrink-0">${escapeHtml(b.units)}</span>` : ""}
              </div>
              <div class="flex flex-wrap gap-1 mt-1.5">
                ${b.fields.map((f) => `<button type="button" class="btn btn-2xs chip-btn chip-blocks font-mono px-1.5" data-insert-field="${escapeAttr(f)}" data-insert-gauge="${escapeAttr(b.gauges[0] || "bar")}" data-insert-block="${escapeAttr(b.name)}" data-lume-title=".${escapeAttr(f)}" data-lume="Insert a ${escapeAttr(b.gauges[0] || "bar")} line and its missing @bind, then customize it in the Component inspector." data-lume-direction="right">.${escapeHtml(f)}</button>`).join("")}
              </div>
            </div>
          </div>
        </div>`).join("")}
    </div>`).join("") || `<p class="text-xs text-base-content/50 font-mono p-2">${escapeHtml(t("empty.match"))}</p>`;

  $$("[data-insert-field]", box).forEach((btn) => {
    btn.addEventListener("click", () => {
      const field = btn.dataset.insertField, gauge = btn.dataset.insertGauge, blockName = btn.dataset.insertBlock;
      const alias = aliasForBlock(blockName);
      insertPaletteSnippet(`line: "LABEL   " ${gauge}(${alias}.${field}) flat(#38bdf8)\n`, {
        [alias]: `block("${blockName.replaceAll('"', "'")}")`,
      });
      showInspectorTab("component");
    });
  });
}

function renderItemFields() {
  $("#item-fields").innerHTML = itemFields.map((f, i) => `
    <div class="ref-card p-2 mb-2">
      <div class="font-mono text-xs font-bold truncate" title="${escapeAttr(f.field)}">${escapeHtml(f.field)}</div>
      <div class="text-[11px] text-base-content/60">${escapeHtml(f.reads)}</div>
      <button type="button" class="btn btn-2xs chip-btn chip-items font-mono mt-1.5 px-1.5 h-auto py-1 w-full justify-start text-left whitespace-normal language-helm" data-insert-item="${i}">${highlightHelm(f.example)}</button>
    </div>`).join("");
  $$("[data-insert-item]", $("#item-fields")).forEach((btn) => {
    btn.addEventListener("click", () => {
      insertPaletteSnippet(`line: "LABEL   " ${itemFields[+btn.dataset.insertItem].example}\n`);
      showInspectorTab("component");
    });
  });
}

function renderComponents() {
  $("#components-palette").innerHTML = components.map((c, i) => `
    <div class="ref-card p-2 mb-2">
      <div class="flex items-baseline justify-between gap-2 min-w-0">
        <span class="font-mono text-xs font-bold truncate" title="${escapeAttr(c.label)}">${escapeHtml(c.label)}</span>
        <span class="text-[10px] text-base-content/40 font-mono shrink-0">${c.ids.map((id) => escapeHtml(id)).join(" / ")}</span>
      </div>
      <div class="text-[11px] text-base-content/60 mt-0.5">${escapeHtml(c.description)}</div>
      <button type="button" class="btn btn-2xs chip-btn chip-components font-mono mt-1.5 px-1.5 h-auto py-1 w-full justify-start text-left whitespace-pre-wrap language-helm" data-insert-component="${i}" data-lume-title="Insert example" data-lume="Insert this component's example line(s) at the cursor." data-lume-direction="right">${highlightHelm(c.example)}</button>
      ${c.compatibleBlocks.length ? `<div class="text-[10px] text-base-content/40 mt-1" tabindex="0" data-lume-title="Compatible blocks" data-lume="${escapeAttr(c.compatibleBlocks.join(", "))}" data-lume-direction="right">Blocks: ${escapeHtml(c.compatibleBlocks.slice(0, 5).join(", "))}${c.compatibleBlocks.length > 5 ? ` +${c.compatibleBlocks.length - 5} more` : ""}</div>` : ""}
    </div>`).join("") + (commonOptions.length ? `
    <details class="mt-2 text-[11px]">
      <summary class="cursor-pointer text-base-content/60 font-mono">Options every gauge takes</summary>
      <dl class="mt-1 space-y-1">
        ${commonOptions.map((o) => `<div><dt class="font-mono inline text-base-content/80">${escapeHtml(o.option)}</dt> <dd class="inline text-base-content/50">— ${escapeHtml(o.effect)}</dd></div>`).join("")}
      </dl>
    </details>` : "");
  $$("[data-insert-component]", $("#components-palette")).forEach((btn) => {
    btn.addEventListener("click", () => {
      insertPaletteSnippet(components[+btn.dataset.insertComponent].example + "\n");
      showInspectorTab("component");
    });
  });
}

function renderTemplates(templates) {
  const box = $("#templates-palette");
  if (templates.length === 0) { box.innerHTML = `<p class="text-xs text-base-content/50 font-mono p-2">${escapeHtml(t("empty.templates"))}</p>`; return; }
  box.innerHTML = templates.map((t, i) => `
    <div class="ref-card p-2 mb-2" data-template-card="${i}">
      <div class="font-mono text-xs font-bold truncate" title="${escapeAttr(t.name)}">${escapeHtml(t.name)}</div>
      <div class="grid grid-cols-2 gap-x-2 gap-y-1 mt-1.5">
        ${t.params.map((p) => `<label class="text-[10px] font-mono flex flex-col gap-0.5 min-w-0">
          <span class="text-base-content/50 truncate" title="${escapeAttr(p.name)}">${escapeHtml(p.name)}${p.default === null ? " *" : ""}</span>
          <input type="text" class="input input-xs input-bordered font-mono text-[10px] w-full" data-template-param="${escapeAttr(p.name)}" value="${escapeAttr(p.default ?? "")}" placeholder="${p.default === null ? "required" : ""}">
        </label>`).join("")}
      </div>
      <button type="button" class="btn btn-2xs chip-btn chip-templates font-mono mt-2 w-full" data-insert-template="${i}" data-lume-title="Insert @use" data-lume="Fill in the template's own parameters, then insert @use ${escapeAttr(t.name)}(...) with these values at the cursor." data-lume-direction="right">Insert @use ${escapeHtml(t.name)}(...)</button>
    </div>`).join("") + '<p class="text-[10px] text-base-content/40 mt-1">* required — no default in @template\'s header.</p>';

  $$("[data-insert-template]", box).forEach((btn) => {
    btn.addEventListener("click", () => {
      const t = templates[+btn.dataset.insertTemplate];
      const card = $(`[data-template-card="${btn.dataset.insertTemplate}"]`, box);
      const args = [];
      for (const input of $$("[data-template-param]", card)) {
        const p = t.params.find((x) => x.name === input.dataset.templateParam);
        const v = input.value.trim();
        if (!v) {
          if (p.default === null) { flashButton(btn, `${p.name} is required`); input.focus(); return; }
          continue; // left blank and optional: @use falls back to @template's own default
        }
        args.push(`${p.name}="${v.replaceAll('"', "'")}"`);
      }
      insertAtCursor(`@use ${t.name}(${args.join(", ")})\n`);
    });
  });
}

// ---- Wiring -------------------------------------------------------------

function loadStarter(name) {
  const s = starters.find((x) => x.name === name) || starters[0];
  // Otherwise the "Project name" field (and picker selection) from a previously loaded project
  // lingers, and the next Save would silently overwrite that project with this starter's DSL.
  currentProjectName = "";
  $("#project-name").value = "";
  $("#project-picker").value = "";
  currentStarterName = s.name;
  $("#starter-picker").value = s.name;
  if (s.profile && PROFILES[s.profile]) {
    state.profile = s.profile;
    $("#profile-picker").value = s.profile;
  }
  replaceDsl(s.dsl);
}

function wireControls() {
  wirePaneResizer();
  wireInspector();
  $("#dsl-editor").addEventListener("input", () => {
    // Once a shared layout is edited, a refresh should restore that edit from autosave rather
    // than silently replacing it with the original fragment.
    if (location.hash.startsWith("#dsl=")) history.replaceState(null, "", location.pathname + location.search);
    dslDirty = true;
    onDslChanged();
  });
  $("#dsl-editor").addEventListener("scroll", syncEditorScroll);

  const starterSel = $("#starter-picker");
  const starterGroups = new Map();
  for (const starter of starters) {
    const label = starter.categoryTitle || "Layouts";
    if (!starterGroups.has(label)) starterGroups.set(label, []);
    starterGroups.get(label).push(starter);
  }
  starterSel.innerHTML = Array.from(starterGroups, ([label, layouts]) =>
    `<optgroup label="${escapeAttr(label)}">${layouts.map((s) => `<option value="${escapeAttr(s.name)}" title="${escapeAttr(s.description || s.title)}">${escapeHtml(s.title)}</option>`).join("")}</optgroup>`
  ).join("");
  $("#layout-count").textContent = t("preview.layoutCount", { count: starters.length });
  starterSel.addEventListener("change", () => {
    if (dslDirty && !confirm(t("confirm.discard"))) { starterSel.value = currentStarterName; return; }
    loadStarter(starterSel.value);
  });

  const profileSel = $("#profile-picker");
  profileSel.innerHTML = Object.keys(PROFILES).filter((p) => p !== "Auto" && p !== "Off").map((p) => `<option value="${p}">${escapeHtml(t("preview.profile", { profile: p, width: PROFILES[p].w, height: PROFILES[p].h }))}</option>`).join("");
  profileSel.value = state.profile;
  profileSel.addEventListener("change", () => { state.profile = profileSel.value; scheduleRender(); });

  $("#screen-picker").addEventListener("change", (e) => {
    state.screenIndex = +e.target.value;
    const selectedName = e.target.options[e.target.selectedIndex]?.textContent;
    const sourceIndex = inspectorModel?.screens.findIndex((screen) => screen.name === selectedName) ?? -1;
    if (sourceIndex >= 0) {
      inspectorSelection.screen = sourceIndex;
      inspectorSelection.panel = inspectorSelection.line = 0;
      refreshInspector(false);
    }
    scheduleRender();
  });

  $("#copy-dsl").addEventListener("click", async () => {
    try {
      await navigator.clipboard.writeText(state.dsl);
      flashButton($("#copy-dsl"), t("action.copied"));
    } catch (e) { flashButton($("#copy-dsl"), t("action.copyFailed")); }
  });

  projects = readProjects();
  refreshProjectPicker();
  $("#save-project").addEventListener("click", saveProject);
  $("#project-name").addEventListener("keydown", (event) => { if (event.key === "Enter") { event.preventDefault(); saveProject(); } });
  $("#delete-project").addEventListener("click", deleteProject);
  $("#project-picker").addEventListener("change", (event) => {
    if (!event.target.value) return; // "Saved projects" placeholder re-selected; nothing to load
    if (dslDirty && !confirm("Discard unsaved changes in the editor?")) { event.target.value = currentProjectName; return; }
    loadProject(event.target.value);
  });
  $("#import-dsl").addEventListener("click", () => $("#import-file").click());
  $("#import-file").addEventListener("change", async (event) => {
    const file = event.target.files && event.target.files[0];
    if (!file) return;
    try {
      replaceDsl(await file.text());
      currentProjectName = ""; // an imported file is not "the" saved project even if its name collides with one
      $("#project-name").value = file.name.replace(/\.(?:helm(?:\.txt)?|txt)$/i, "");
      $("#project-picker").value = "";
      flashButton($("#import-dsl"), "Imported");
    } catch (e) {
      flashButton($("#import-dsl"), "Import failed");
      logcad.logd(logger, "import", "Could not read layout file", { message: e.message });
    }
    event.target.value = "";
  });
  $("#export-dsl").addEventListener("click", exportDsl);
  $("#share-dsl").addEventListener("click", async () => {
    const url = new URL(location.href);
    url.hash = `dsl=${encodeShare(state.dsl)}`;
    try {
      await navigator.clipboard.writeText(url.href);
      flashButton($("#share-dsl"), t("action.linkCopied"));
    } catch (e) { flashButton($("#share-dsl"), "Copy failed"); }
  });

  $("#block-search").addEventListener("input", renderBlockPalette);

  // A modest serial tick rate, not a 60 fps loop: each render replays 40 script passes
  // (tools/helm-toolbox-wasm/Host.cs). Waiting for one render before scheduling the next prevents a slow
  // WASM startup/render from being continually superseded by newer timer ticks.
  let animTimer = null, animStart = null, animBase = state.seconds, animGeneration = 0;
  const TICK_MS = 150;
  const playToggle = $("#play-toggle");
  async function tick(generation) {
    if (!state.playing || generation !== animGeneration) return;
    state.seconds = animBase + (performance.now() - animStart) / 1000;
    await doRender();
    if (state.playing && generation === animGeneration) {
      animTimer = setTimeout(() => tick(generation), TICK_MS);
    }
  }
  function setPlaying(playing) {
    if (!playing && state.playing && animStart !== null) {
      state.seconds = animBase + (performance.now() - animStart) / 1000;
    }
    state.playing = playing;
    playToggle.textContent = state.playing ? t("preview.pause") : t("preview.play");
    clearTimeout(animTimer);
    animGeneration++;
    if (state.playing) {
      animBase = state.seconds;
      animStart = performance.now();
      const generation = animGeneration;
      animTimer = setTimeout(() => tick(generation), 0);
    } else animStart = null;
  }
  playToggle.addEventListener("click", () => {
    if (!state.playing) $("#reduce-motion").checked = false;
    setPlaying(!state.playing);
  });

  const alertToggle = $("#alert-toggle");
  alertToggle.addEventListener("click", () => {
    state.alert = !state.alert;
    alertToggle.classList.toggle("btn-error", state.alert);
    alertToggle.setAttribute("aria-pressed", String(state.alert));
    scheduleRender();
  });

  const shortcutDialog = $("#shortcut-dialog");
  const reduceMotion = $("#reduce-motion");
  reduceMotion.checked = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
  reduceMotion.addEventListener("change", () => setPlaying(!reduceMotion.checked));
  $("#close-shortcuts").addEventListener("click", () => shortcutDialog.close());
  document.addEventListener("keydown", (event) => {
    const mod = event.ctrlKey || event.metaKey;
    if (mod && event.key.toLowerCase() === "s") { event.preventDefault(); saveProject(); }
    else if (mod && event.key.toLowerCase() === "o") { event.preventDefault(); $("#import-file").click(); }
    else if (mod && event.shiftKey && event.key.toLowerCase() === "e") { event.preventDefault(); exportDsl(); }
    else if (event.key === "?" && !/^(INPUT|TEXTAREA|SELECT)$/.test(document.activeElement && document.activeElement.tagName)) {
      event.preventDefault(); shortcutDialog.showModal();
    }
  });
  setPlaying(!reduceMotion.checked);
}

function flashButton(btn, text) {
  const original = btn.textContent;
  btn.textContent = text;
  setTimeout(() => { btn.textContent = original; }, 1200);
}

async function main() {
  logcad.logc([
    { text: " HELM Toolbox ", c: "#08111f", bg: "#38bdf8", b: true },
    { text: " renderer starting", c: "#94a3b8" },
  ]);
  await loadData();
  wireControls();
  renderBlockPalette();
  renderItemFields();
  renderComponents();
  // Not awaited: this loads the same WASM module the first preview render does (wasm-bridge.mjs
  // memoizes it, so this doesn't duplicate that fetch), and the Templates tab is reference-only —
  // it can fill in once ready rather than delaying the rest of the page.
  listTemplates().then(renderTemplates).catch((e) => {
    $("#templates-palette").innerHTML = `<p class="text-xs text-base-content/50 font-mono p-2">${escapeHtml(t("empty.templateFailure"))}</p>`;
    logcad.logd(logger, "data", "Could not load templates", { message: e.message });
  });

  const shared = decodeShare();
  if (location.hash.startsWith("#dsl=") && !(shared && shared.trim())) {
    // Malformed or empty share link: drop it instead of leaving it in the address bar, where it
    // would look like the page loaded the shared layout when it actually fell through to autosave/starter.
    history.replaceState(null, "", location.pathname + location.search);
  }
  let saved = null;
  try { saved = readStoredValue(DSL_KEY, "dsl"); } catch (e) {}
  if (shared && shared.trim()) replaceDsl(shared);
  else if (saved && saved.trim()) replaceDsl(saved);
  else loadStarter(starters[0].name);
}

main().catch((e) => {
  $("#preview-status").textContent = t("preview.startFailed", { message: e.message });
  logcad.logd(logger, "start", "Application failed to start", { message: e.message });
});
