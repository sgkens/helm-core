// app.mjs: wires the webutil page together. No layout logic lives here (TASK-39.1/39.5/39.7):
// the DSL text is the source of truth, the WebAssembly build of the real script (wasm-bridge.mjs)
// renders it, and sprite-svg.mjs + se-lcd-bezel.mjs draw the result exactly as the docs preview does.
import { buildBezel, PROFILES } from "../../assets/js/se-lcd-bezel.mjs";
import { renderSprites } from "../../assets/js/sprite-svg.mjs";
import { render as wasmRender } from "./wasm-bridge.mjs";
import { Lume } from "../vendor/lume.mjs";
import logcad from "../vendor/logcad.mjs";

// This module lives in webutil/js/; data/ is its sibling. Resolved from import.meta.url (not a
// hardcoded "/webutil/"), so it works whether the page is served at the site root (local dev) or
// under /webutil/ (production, alongside the docs site).
const DATA_BASE = new URL("../data/", import.meta.url).pathname;
const ASSET_BASE = new URL("../../assets/", import.meta.url).pathname;
const PROJECTS_KEY = "webutil_projects_v1";

const $ = (sel, root = document) => root.querySelector(sel);
const $$ = (sel, root = document) => Array.from(root.querySelectorAll(sel));

const state = {
  dsl: "",
  profile: "TextPanel",
  screenIndex: 0,
  values: {}, // bindName -> 0..100
  seconds: 0,
  playing: true,
  lastResult: null,
};

let blocks = null, itemFields = [], starters = [];
let projects = {};
let debounceTimer = null;
let renderToken = 0;
const logger = { name: "webutil", logname: " HELM ", bg: "#0891b2", logbg: "#0f172a" };

new Lume({ motion: "lift", showDelay: 120, hideDelay: 40, offset: 10 });

async function loadData() {
  const [b, s] = await Promise.all([
    fetch(DATA_BASE + "blocks.json").then((r) => r.json()),
    fetch(DATA_BASE + "starters.json").then((r) => r.json()),
  ]);
  blocks = b.blocks;
  itemFields = b.itemFields;
  starters = s;
}

// ---- DSL <-> binds -------------------------------------------------------

function parseBindNames(dsl) {
  const names = [];
  for (const line of dsl.split(/\r?\n/)) {
    const m = line.match(/^\s*@bind\s+(\S+)\s*=/);
    if (m) names.push(m[1]);
  }
  return names;
}

function insertAtCursor(text) {
  const ta = $("#dsl-editor");
  const start = ta.selectionStart, end = ta.selectionEnd;
  ta.value = ta.value.slice(0, start) + text + ta.value.slice(end);
  ta.selectionStart = ta.selectionEnd = start + text.length;
  ta.focus();
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
    $("#preview-status").textContent = "Rendering…";
    result = await wasmRender(state.dsl, prof.w, prof.h, state.values, state.seconds, state.screenIndex);
  } catch (e) {
    if (myToken !== renderToken) return;
    $("#preview-status").textContent = "The renderer failed to load: " + e.message;
    logcad.logd(logger, "render", "Renderer failed", { message: e.message });
    return;
  }
  if (myToken !== renderToken) return; // a newer render started while this one was in flight

  state.lastResult = result;
  $("#preview-status").textContent = "";

  const bezel = buildBezel(state.profile, {});
  stage.innerHTML = bezel;
  const svg = stage.querySelector("svg");
  svg.removeAttribute("height");
  svg.setAttribute("role", "img");
  svg.setAttribute("aria-label", `HELM preview for ${state.profile}, screen ${state.screenIndex + 1}`);
  const slot = svg.querySelector("#content-slot");
  const ox = (result.texture[0] - result.surface[0]) / 2;
  const oy = (result.texture[1] - result.surface[1]) / 2;
  renderSprites(slot, result.sprites, { ox, oy, fontChoice: "auto" });

  renderScreenPicker(result.screens);
  renderIssues(result.errors, result.warnings);
  renderValueSliders(result.binds);
}

function renderScreenPicker(screens) {
  const sel = $("#screen-picker");
  const wrap = $("#screen-picker-wrap");
  if (screens.length <= 1) { wrap.classList.add("hidden"); return; }
  wrap.classList.remove("hidden");
  if (sel.dataset.count !== String(screens.length)) {
    sel.innerHTML = screens.map((n, i) => `<option value="${i}">${n}</option>`).join("");
    sel.dataset.count = String(screens.length);
  }
  sel.value = String(Math.min(state.screenIndex, screens.length - 1));
}

function renderIssues(errors, warnings) {
  const box = $("#issues");
  if (errors.length === 0 && warnings.length === 0) { box.innerHTML = ""; box.classList.add("hidden"); return; }
  box.classList.remove("hidden");
  box.innerHTML =
    errors.map((e) => `<div class="text-error font-mono text-xs">error: ${escapeHtml(e)}</div>`).join("") +
    warnings.map((w) => `<div class="text-warning font-mono text-xs">warning: ${escapeHtml(w)}</div>`).join("");
}

function renderValueSliders(binds) {
  const box = $("#value-sliders");
  const names = binds.map((b) => b.name).join(",");
  if (box.dataset.names === names) return; // same binds as last render: keep the sliders (and their values) as they are
  box.dataset.names = names;
  if (binds.length === 0) { box.innerHTML = '<p class="text-xs text-base-content/50 font-mono">No @bind lines yet.</p>'; return; }
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

function readProjects() {
  try {
    const value = JSON.parse(localStorage.getItem(PROJECTS_KEY) || "{}");
    return value && typeof value === "object" && !Array.isArray(value) ? value : {};
  } catch (e) { return {}; }
}

function writeProjects() {
  try { localStorage.setItem(PROJECTS_KEY, JSON.stringify(projects)); return true; } catch (e) { return false; }
}

function refreshProjectPicker(selected = "") {
  const picker = $("#project-picker");
  const names = Object.keys(projects).sort((a, b) => a.localeCompare(b));
  picker.innerHTML = '<option value="">Saved projects</option>' + names.map((name) =>
    `<option value="${escapeAttr(name)}">${escapeHtml(name)}</option>`).join("");
  picker.value = names.includes(selected) ? selected : "";
}

function replaceDsl(dsl) {
  $("#dsl-editor").value = dsl;
  state.values = {};
  state.screenIndex = 0;
  onDslChanged();
}

function saveProject() {
  const name = $("#project-name").value.trim();
  if (!name) { flashButton($("#save-project"), "Name required"); $("#project-name").focus(); return; }
  projects[name] = { dsl: state.dsl, profile: state.profile, updated: new Date().toISOString() };
  if (!writeProjects()) { flashButton($("#save-project"), "Storage failed"); return; }
  refreshProjectPicker(name);
  flashButton($("#save-project"), "Saved");
}

function loadProject(name) {
  const project = projects[name];
  if (!project || typeof project.dsl !== "string") return;
  $("#project-name").value = name;
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

function onDslChanged() {
  state.dsl = $("#dsl-editor").value;
  try { localStorage.setItem("webutil_dsl", state.dsl); } catch (e) {}
  scheduleRender();
}

// ---- Block / field reference (click to insert a line:) ---------------

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
        <div class="rounded border border-base-300 p-2 mb-1.5">
          <div class="flex items-start gap-2">
            <a href="${escapeAttr(b.icon.source)}" target="_blank" rel="noopener" aria-label="Open ${escapeAttr(b.name)} icon source on the Official Space Engineers Wiki" class="shrink-0 rounded bg-base-300/40" data-lume="Icon source: Official Space Engineers Wiki" data-lume-direction="right">
              <img src="${ASSET_BASE}blocks/${escapeAttr(b.icon.slug)}.png" alt="" width="40" height="40" class="w-10 h-10 object-contain" loading="lazy">
            </a>
            <div class="min-w-0 flex-1">
              <div class="flex items-baseline justify-between gap-2">
                <span class="font-mono text-xs font-bold">${escapeHtml(b.name)}</span>
                ${b.units ? `<span class="text-[10px] text-base-content/40 font-mono">${escapeHtml(b.units)}</span>` : ""}
              </div>
              <div class="flex flex-wrap gap-1 mt-1">
                ${b.fields.map((f) => `<button type="button" class="btn btn-2xs btn-ghost font-mono px-1.5" data-insert-field="${escapeAttr(f)}" data-insert-gauge="${escapeAttr(b.gauges[0] || "bar")}" data-lume-title=".${escapeAttr(f)}" data-lume="Insert a ${escapeAttr(b.gauges[0] || "bar")} line using this telemetry field." data-lume-direction="right">.${escapeHtml(f)}</button>`).join("")}
              </div>
            </div>
          </div>
        </div>`).join("")}
    </div>`).join("") || '<p class="text-xs text-base-content/50 font-mono p-2">No match.</p>';

  $$("[data-insert-field]", box).forEach((btn) => {
    btn.addEventListener("click", () => {
      const field = btn.dataset.insertField, gauge = btn.dataset.insertGauge;
      insertAtCursor(`line: "LABEL   " ${gauge}(bind.${field}) flat(#38bdf8)\n`);
    });
  });
}

function renderItemFields() {
  $("#item-fields").innerHTML = itemFields.map((f) => `
    <div class="rounded border border-base-300 p-2 mb-1.5">
      <div class="font-mono text-xs font-bold">${escapeHtml(f.field)}</div>
      <div class="text-[11px] text-base-content/60">${escapeHtml(f.reads)}</div>
      <button type="button" class="btn btn-2xs btn-ghost font-mono mt-1 px-1.5" data-insert-line="${escapeAttr(f.example)}">${escapeHtml(f.example)}</button>
    </div>`).join("");
  $$("[data-insert-line]", $("#item-fields")).forEach((btn) => {
    btn.addEventListener("click", () => insertAtCursor(`line: "LABEL   " ${btn.dataset.insertLine}\n`));
  });
}

// ---- Wiring -------------------------------------------------------------

function loadStarter(name) {
  const s = starters.find((x) => x.name === name) || starters[0];
  replaceDsl(s.dsl);
}

function wireControls() {
  $("#dsl-editor").addEventListener("input", () => {
    // Once a shared layout is edited, a refresh should restore that edit from autosave rather
    // than silently replacing it with the original fragment.
    if (location.hash.startsWith("#dsl=")) history.replaceState(null, "", location.pathname + location.search);
    onDslChanged();
  });

  const starterSel = $("#starter-picker");
  starterSel.innerHTML = starters.map((s) => `<option value="${s.name}">${escapeHtml(s.title)}</option>`).join("");
  starterSel.addEventListener("change", () => loadStarter(starterSel.value));

  const profileSel = $("#profile-picker");
  profileSel.innerHTML = Object.keys(PROFILES).filter((p) => p !== "Auto" && p !== "Off").map((p) => `<option value="${p}">${p} (${PROFILES[p].w}x${PROFILES[p].h})</option>`).join("");
  profileSel.value = state.profile;
  profileSel.addEventListener("change", () => { state.profile = profileSel.value; scheduleRender(); });

  $("#screen-picker").addEventListener("change", (e) => { state.screenIndex = +e.target.value; scheduleRender(); });

  $("#copy-dsl").addEventListener("click", async () => {
    try {
      await navigator.clipboard.writeText(state.dsl);
      flashButton($("#copy-dsl"), "Copied");
    } catch (e) { flashButton($("#copy-dsl"), "Could not copy"); }
  });

  projects = readProjects();
  refreshProjectPicker();
  $("#save-project").addEventListener("click", saveProject);
  $("#project-picker").addEventListener("change", (event) => loadProject(event.target.value));
  $("#import-dsl").addEventListener("click", () => $("#import-file").click());
  $("#import-file").addEventListener("change", async (event) => {
    const file = event.target.files && event.target.files[0];
    if (!file) return;
    try {
      replaceDsl(await file.text());
      $("#project-name").value = file.name.replace(/\.(?:helm(?:\.txt)?|txt)$/i, "");
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
      flashButton($("#share-dsl"), "Link copied");
    } catch (e) { flashButton($("#share-dsl"), "Copy failed"); }
  });

  $("#block-search").addEventListener("input", renderBlockPalette);

  // A modest tick rate, not a 60 fps loop: each render replays 40 script passes (tools/webutil-wasm/Host.cs),
  // so driving it every animation frame would spend most of a core on renders nothing can see.
  let animTimer = null, animStart = null;
  const TICK_MS = 150;
  const playToggle = $("#play-toggle");
  function tick() {
    if (animStart === null) animStart = performance.now();
    state.seconds = (performance.now() - animStart) / 1000;
    doRender();
  }
  function setPlaying(playing) {
    state.playing = playing;
    playToggle.textContent = state.playing ? "Pause animation" : "Play animation";
    clearInterval(animTimer);
    if (state.playing) { animStart = null; animTimer = setInterval(tick, TICK_MS); }
  }
  playToggle.addEventListener("click", () => {
    if (!state.playing) $("#reduce-motion").checked = false;
    setPlaying(!state.playing);
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
    { text: " HELM webutil ", c: "#08111f", bg: "#38bdf8", b: true },
    { text: " renderer starting", c: "#94a3b8" },
  ]);
  await loadData();
  wireControls();
  renderBlockPalette();
  renderItemFields();

  const shared = decodeShare();
  let saved = null;
  try { saved = localStorage.getItem("webutil_dsl"); } catch (e) {}
  if (shared && shared.trim()) replaceDsl(shared);
  else if (saved && saved.trim()) replaceDsl(saved);
  else loadStarter(starters[0].name);
}

main().catch((e) => {
  $("#preview-status").textContent = "Could not start: " + e.message;
  logcad.logd(logger, "start", "Application failed to start", { message: e.message });
});
