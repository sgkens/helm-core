// help-visualizer.js - draws HELM display frames inside a Space Engineers LCD bezel.
//
// It contains no layout logic. Frames come from tools/spritedump, which runs the real
// Program.cs renderer and exports its sprite buffer (first frame in full, later ticks as
// deltas). This module only turns those sprites into SVG inside the bezel from
// se-lcd-bezel.mjs, whose #content-slot uses exact panel pixel coordinates.
//
// Markup:  <div data-helm-viz data-layout="planetary_base" data-profile="TextPanel"></div>
// Cycling: data-cycle="planetary_base:TextPanel,command_carrier:WideLCD" data-cycle-seconds="9"
//          data-controls="minimal" shows only pause and the current layout name.
// Frames:  <base>/assets/frames/<layout>.json
import { buildBezel, PROFILES } from "./se-lcd-bezel.mjs";

const BASE = ((document.querySelector('meta[name="helm-base"]') || {}).content || "/").replace(/\/$/, "");
const FRAMES_URL = (layout) => `${BASE}/assets/frames/${layout}.json`;

// Font families. "Monospace" in game maps to the IBM VGA 8x14 face, everything else to Share Tech Mono
// (tools/Set-TerminalFont.ps1). Cell width is the harness assumption (13 px per unit of text scale).
const FONTS = {
  standard: { label: "SE Standard (Share Tech Mono)", family: '"Share Tech Mono", monospace', probe: "Share Tech Mono" },
  monospace: { label: "SE Monospace (IBM VGA 8x14)", family: '"Helm IBM VGA", monospace', probe: "Helm IBM VGA" },
};
const CELL_WIDTH = 13;
const advanceCache = {};

function advanceOf(key) {
  if (advanceCache[key]) return advanceCache[key];
  try {
    const ctx = document.createElement("canvas").getContext("2d");
    ctx.font = `100px ${FONTS[key].family}`;
    const w = ctx.measureText("M").width / 100;
    if (w > 0.2 && w < 1.2) return (advanceCache[key] = w);
  } catch (e) {}
  return 0.6;
}

const frameCache = new Map();
function loadFrames(layout) {
  if (!frameCache.has(layout)) {
    frameCache.set(layout, fetch(FRAMES_URL(layout)).then((r) => {
      if (!r.ok) throw new Error("HTTP " + r.status);
      return r.json();
    }));
  }
  return frameCache.get(layout);
}

const SVGNS = "http://www.w3.org/2000/svg";
const el = (name, attrs) => {
  const n = document.createElementNS(SVGNS, name);
  for (const k in attrs) n.setAttribute(k, attrs[k]);
  return n;
};
const hex2 = (n) => n.toString(16).padStart(2, "0");

export class HelmVisualizer {
  constructor(root) {
    this.root = root;
    this.layout = root.dataset.layout;
    this.profile = root.dataset.profile || "TextPanel";
    this.fontChoice = root.dataset.font || "authored";
    this.screenIndex = 0;
    this.playing = !window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    this.tick = 0;
    this.visible = true;
    this.fallback = Array.from(root.children);
    this.timer = null;
    this.minimal = root.dataset.controls === "minimal";
    this.cycle = (root.dataset.cycle || "").split(",").filter(Boolean).map((x) => { const [l, p] = x.split(":"); return { layout: l, profile: p || "TextPanel" }; });
    this.cycleIndex = 0;
    this.cycleSeconds = Number(root.dataset.cycleSeconds) || 9;
  }

  async init() {
    try {
      this.data = await loadFrames(this.layout);
    } catch (e) {
      return false; // no frames generated: keep the static fallback in place
    }
    this.buildUi();
    this.select();
    if (document.fonts && document.fonts.ready) document.fonts.ready.then(() => this.draw());
    new IntersectionObserver((entries) => { this.visible = entries[0].isIntersecting; }, { threshold: 0.05 }).observe(this.root);
    if (this.cycle.length > 1) {
      this.cycleTimer = setInterval(() => { if (this.playing && this.visible && !document.hidden) this.next(); }, this.cycleSeconds * 1000);
      this.cycle.slice(1).forEach((c) => loadFrames(c.layout).catch(() => {})); // warm the cache
    }
    return true;
  }

  // ---- UI -------------------------------------------------------------
  buildUi() {
    this.fallback.forEach((n) => n.remove());
    this.root.classList.add("hv-ready");
    this.stage = document.createElement("div");
    this.stage.className = "hv-stage";
    this.controls = document.createElement("div");
    this.controls.className = "hv-controls";
    this.root.append(this.stage, this.controls);

    if (this.minimal) {
      this.playBtn = this.button(this.playing ? "Pause" : "Play", () => this.toggle());
      this.caption = document.createElement("span");
      this.caption.className = "hv-caption";
      this.controls.append(this.playBtn, this.caption);
      return;
    }
    const profiles = Object.keys(PROFILES).filter((p) => this.data.frames.some((f) => f.profile === p));
    this.playBtn = this.button(this.playing ? "Pause" : "Play", () => this.toggle());
    this.controls.append(
      this.playBtn,
      this.select_("Screen profile", profiles.map((p) => [p, `${p} ${PROFILES[p].w}x${PROFILES[p].h}`]), this.profile, (v) => { this.profile = v; this.select(); }),
      this.select_("Font", [["authored", "Font: as authored"], ["standard", FONTS.standard.label], ["monospace", FONTS.monospace.label]], this.fontChoice, (v) => { this.fontChoice = v; this.draw(); }),
    );
    const scan = document.createElement("label");
    scan.className = "hv-check";
    scan.innerHTML = '<input type="checkbox"> CRT lines';
    scan.querySelector("input").addEventListener("change", (e) => {
      const s = this.stage.querySelector("#scanlines");
      if (s) s.setAttribute("opacity", e.target.checked ? 1 : 0);
      this.scanlines = e.target.checked;
    });
    this.controls.append(scan);
  }

  button(label, fn) {
    const b = document.createElement("button");
    b.type = "button"; b.className = "hv-btn"; b.textContent = label;
    b.addEventListener("click", fn);
    return b;
  }

  select_(label, options, value, onChange) {
    const s = document.createElement("select");
    s.className = "hv-select"; s.setAttribute("aria-label", label);
    options.forEach(([v, t]) => { const o = document.createElement("option"); o.value = v; o.textContent = t; if (v === value) o.selected = true; s.append(o); });
    s.addEventListener("change", () => onChange(s.value));
    return s;
  }

  toggle() {
    this.playing = !this.playing;
    this.playBtn.textContent = this.playing ? "Pause" : "Play";
  }

  // ---- Frame selection and drawing ------------------------------------
  select() {
    const list = this.data.frames.filter((f) => f.profile === this.profile);
    this.entry = list[this.screenIndex] || list[0];
    if (!this.entry) return;
    this.sprites = this.entry.sprites.map((s) => Object.assign({}, s));
    this.tick = 0;
    this.draw();
    this.start();
    if (this.caption) this.caption.textContent = `${this.layout} - ${this.entry.profile} ${this.entry.surface[0]}x${this.entry.surface[1]}`;
  }

  // Switches to the next layout in data-cycle; on load failure the current one stays.
  async next() {
    const c = this.cycle[(this.cycleIndex + 1) % this.cycle.length];
    try {
      const data = await loadFrames(c.layout);
      this.cycleIndex = (this.cycleIndex + 1) % this.cycle.length;
      this.data = data; this.layout = c.layout; this.profile = c.profile; this.screenIndex = 0;
      this.select();
    } catch (e) {}
  }

  draw() {
    if (!this.entry) return;
    const e = this.entry;
    const bezel = buildBezel(e.profile, { scanlines: !!this.scanlines });
    this.stage.innerHTML = bezel;
    const svg = this.stage.querySelector("svg");
    svg.removeAttribute("height");
    svg.setAttribute("role", "img");
    svg.setAttribute("aria-label", `${this.layout} on ${e.profile}`);
    this.slot = svg.querySelector("#content-slot");
    this.ox = (e.texture[0] - e.surface[0]) / 2;
    this.oy = (e.texture[1] - e.surface[1]) / 2;
    this.nodes = this.sprites.map((s) => this.make(s));
    this.nodes.forEach((n) => n && this.slot.append(n));
  }

  fontKey(s) {
    if (this.fontChoice === "standard" || this.fontChoice === "monospace") return this.fontChoice;
    return /mono/i.test(s.f || "") ? "monospace" : "standard";
  }

  make(s) {
    const node = s.t === "text" ? el("text", {}) : s.d === "Circle" ? el("ellipse", {}) : s.d === "Triangle" ? el("polygon", {}) : s.t === "tex" ? el("rect", {}) : null;
    if (node) this.paint(node, s);
    return node;
  }

  paint(node, s) {
    const x = (s.x || 0) - this.ox, y = (s.y || 0) - this.oy;
    const c = s.c || [255, 255, 255, 255];
    const fill = `rgb(${c[0]},${c[1]},${c[2]})`;
    node.setAttribute("fill", fill);
    node.setAttribute("fill-opacity", (c[3] / 255).toFixed(3));
    if (s.t === "text") {
      const key = this.fontKey(s);
      const size = (CELL_WIDTH * (s.s || 1)) / advanceOf(key);
      node.setAttribute("x", x);
      node.setAttribute("y", y);
      node.setAttribute("font-family", FONTS[key].family);
      node.setAttribute("font-size", size.toFixed(2));
      node.setAttribute("text-anchor", s.a === "center" ? "middle" : s.a === "right" ? "end" : "start");
      node.setAttribute("dominant-baseline", "hanging");
      node.setAttribute("xml:space", "preserve");
      node.style.whiteSpace = "pre";
      if (node.textContent !== s.d) node.textContent = s.d;
      return;
    }
    const w = s.w || 0, h = s.h || 0;
    if (s.d === "Circle") {
      node.setAttribute("cx", x); node.setAttribute("cy", y);
      node.setAttribute("rx", w / 2); node.setAttribute("ry", h / 2);
    } else if (s.d === "Triangle") {
      node.setAttribute("points", `${x},${y - h / 2} ${x + w / 2},${y + h / 2} ${x - w / 2},${y + h / 2}`);
    } else {
      node.setAttribute("x", x - w / 2); node.setAttribute("y", y - h / 2);
      node.setAttribute("width", w); node.setAttribute("height", h);
    }
    if (s.r) node.setAttribute("transform", `rotate(${(s.r * 180) / Math.PI} ${x} ${y})`);
    node.dataset.hex = "#" + hex2(c[0]) + hex2(c[1]) + hex2(c[2]);
    if (!node.firstChild) node.append(el("title", {}));
    node.firstChild.textContent = `${node.dataset.hex}${c[3] < 255 ? " alpha " + c[3] : ""}`;
  }

  // ---- Playback -------------------------------------------------------
  start() {
    clearInterval(this.timer);
    const deltas = this.entry.deltas;
    if (!deltas || !deltas.length) return;
    this.timer = setInterval(() => {
      if (!this.playing || !this.visible || document.hidden) return;
      this.step(deltas[this.tick % deltas.length]);
      this.tick++;
    }, 1000 / (this.entry.fps || 6));
  }

  step(delta) {
    if (delta.full) {
      this.sprites = delta.full.map((s) => Object.assign({}, s));
      this.nodes.forEach((n) => n && n.remove());
      this.nodes = this.sprites.map((s) => this.make(s));
      this.nodes.forEach((n) => n && this.slot.append(n));
      return;
    }
    for (const d of delta) {
      const s = this.sprites[d.i];
      if (!s) continue;
      for (const k in d) if (k !== "i") s[k] = d[k];
      if (this.nodes[d.i]) this.paint(this.nodes[d.i], s);
    }
  }
}

export function initAll(scope = document) {
  const roots = Array.from(scope.querySelectorAll("[data-helm-viz]:not([data-hv-init])"));
  const io = new IntersectionObserver((entries) => {
    entries.forEach((en) => {
      if (!en.isIntersecting) return;
      io.unobserve(en.target);
      new HelmVisualizer(en.target).init();
    });
  }, { rootMargin: "300px" });
  roots.forEach((r) => { r.dataset.hvInit = "1"; io.observe(r); });
}

window.HelmVisualizer = HelmVisualizer;
if (document.readyState === "loading") document.addEventListener("DOMContentLoaded", () => initAll());
else initAll();
