// sprite-svg.mjs - turns a HELM sprite (the JSON the real script's renderer exports, whether from
// tools/spritedump's frame files or a live tools/helm-toolbox-wasm render) into an SVG element.
//
// This is the one place that maps a sprite {t, d, x, y, w, h, c, f, a, s, r} to markup, used by both
// the docs preview (help-visualizer.js) and the helm-toolbox live preview, so the two cannot drift apart
// (TASK-39.11). It contains no layout logic and no fetching: callers own the sprite list and the
// bezel; this module only makes and paints nodes inside a #content-slot whose coordinate system is
// exact panel pixels (see se-lcd-bezel.mjs).
//
// Sprite fields, matching tools/spritedump/Dump.cs and tools/helm-toolbox-wasm/Host.cs:
//   t: "text" | "tex" | "clip"   d: text or texture name   x, y: centre (or text origin)
//   w, h: size (texture only)    c: [r,g,b,a]               f: font id (text only)
//   a: "left" | "center" | "right"  s: text scale, or a texture's rotation in radians when t != "text"
//   r: kept for callers that still use the older field name for a texture's rotation

const SVGNS = "http://www.w3.org/2000/svg";

// Font families used in the game: "Monospace" maps to the IBM VGA 8x14 face (tools/Set-TerminalFont.ps1
// installs "Helm IBM VGA"), everything else to Share Tech Mono. Cell width is the harness assumption of
// 13 px per unit of text scale, matched against the game's MeasureStringInPixels in earlier testing.
export const FONTS = {
  standard: { label: "SE Standard (Share Tech Mono)", family: '"Share Tech Mono", monospace', probe: "Share Tech Mono" },
  monospace: { label: "SE Monospace (IBM VGA 8x14)", family: '"Helm IBM VGA", monospace', probe: "Helm IBM VGA" },
};
export const CELL_WIDTH = 13;

const advanceCache = {};
export function advanceOf(key) {
  if (advanceCache[key]) return advanceCache[key];
  try {
    const ctx = document.createElement("canvas").getContext("2d");
    ctx.font = `100px ${FONTS[key].family}`;
    const w = ctx.measureText("M").width / 100;
    if (w > 0.2 && w < 1.2) return (advanceCache[key] = w);
  } catch (e) {}
  return 0.6;
}

export const el = (name, attrs) => {
  const n = document.createElementNS(SVGNS, name);
  for (const k in attrs) n.setAttribute(k, attrs[k]);
  return n;
};
export const hex2 = (n) => n.toString(16).padStart(2, "0");

// "Monospace" font ids draw with the IBM VGA face; anything else (including a missing id) is Standard.
export function fontKeyFor(s, fontChoice) {
  if (fontChoice === "standard" || fontChoice === "monospace") return fontChoice;
  return /mono/i.test(s.f || "") ? "monospace" : "standard";
}

// Creates the right element kind for a sprite; paintSpriteNode still needs to be called to size and
// colour it. Returns null for a sprite kind this renderer does not draw (e.g. "clip").
export function makeSpriteNode(s) {
  return s.t === "text" ? el("text", {})
    : s.d === "Circle" || s.d === "CircleHollow" ? el("ellipse", {})
    : s.d === "Triangle" ? el("polygon", {})
    : s.t === "tex" ? el("rect", {})
    : null;
}

// opts: { ox, oy } the texture-to-surface offset (see se-lcd-bezel.mjs's coordinate contract) and
// fontChoice ("auto", "standard" or "monospace").
export function paintSpriteNode(node, s, opts) {
  const ox = opts.ox || 0, oy = opts.oy || 0;
  const x = (s.x || 0) - ox, y = (s.y || 0) - oy;
  const c = s.c || [255, 255, 255, 255];
  const fill = `rgb(${c[0]},${c[1]},${c[2]})`;
  node.setAttribute("fill", fill);
  node.setAttribute("fill-opacity", (c[3] / 255).toFixed(3));
  if (s.t === "text") {
    const key = fontKeyFor(s, opts.fontChoice);
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
  const rot = s.r || 0; // "s" is reserved for a text sprite's scale; a texture's rotation is always "r"
  if (s.d === "Circle" || s.d === "CircleHollow") {
    node.setAttribute("cx", x); node.setAttribute("cy", y);
    node.setAttribute("rx", w / 2); node.setAttribute("ry", h / 2);
  } else if (s.d === "Triangle") {
    node.setAttribute("points", `${x},${y - h / 2} ${x + w / 2},${y + h / 2} ${x - w / 2},${y + h / 2}`);
  } else {
    node.setAttribute("x", x - w / 2); node.setAttribute("y", y - h / 2);
    node.setAttribute("width", w); node.setAttribute("height", h);
  }
  if (rot) node.setAttribute("transform", `rotate(${(rot * 180) / Math.PI} ${x} ${y})`);
  node.dataset.hex = "#" + hex2(c[0]) + hex2(c[1]) + hex2(c[2]);
  if (!node.firstChild) node.append(el("title", {}));
  node.firstChild.textContent = `${node.dataset.hex}${c[3] < 255 ? " alpha " + c[3] : ""}`;
}

// Convenience: build every node for a sprite list and append it to a slot element (a #content-slot).
export function renderSprites(slot, sprites, opts) {
  const nodes = sprites.map((s) => {
    const node = makeSpriteNode(s);
    if (node) paintSpriteNode(node, s, opts || {});
    return node;
  });
  nodes.forEach((n) => n && slot.append(n));
  return nodes;
}
