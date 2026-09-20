// Space Engineers LCD bezel generator.
// Works in browser (ES module) and Node. Returns an SVG string.
//
// COORDINATE CONTRACT: inside <svg id="content"> the coordinate system is exactly
// the panel's pixel resolution (0,0 = top-left of the screen, w x h). So a script's
// drawing coordinates map 1:1 - no scaling maths in your GUI.

export const PROFILES = {
  TextPanel:     { w: 512,  h: 512, hw: 'bolts'    },
  WideLCD:       { w: 1024, h: 512, hw: 'clamps'   },
  CockpitSmall:  { w: 512,  h: 256, hw: 'screws'   },
  CornerLCD:     { w: 512,  h: 128, hw: 'brackets' },
  CockpitCorner: { w: 256,  h: 128, hw: 'mounts'   },
  Auto:          { w: 512,  h: 512, hw: 'bolts'    }, // inferred from opts.cols/rows (512px per panel)
  Off:           { w: 512,  h: 512, hw: 'none'     }, // headless: no bezel, size from opts.width/height
};

const n = v => +v.toFixed(2);
const clamp = (v, a, b) => Math.max(a, Math.min(b, v));

const hex = (cx, cy, r) =>
  Array.from({ length: 6 }, (_, k) => {
    const a = (Math.PI / 3) * k;
    return `${n(cx + r * Math.cos(a))},${n(cy + r * Math.sin(a))}`;
  }).join(' ');

const bolt = (cx, cy, r) =>
  `<circle cx="${n(cx)}" cy="${n(cy)}" r="${n(r)}" fill="url(#se-metal)" stroke="#12161a" stroke-width="1"/>` +
  `<polygon points="${hex(cx, cy, r * 0.55)}" fill="#1f252a" stroke="#0b0e10" stroke-width=".6"/>`;

const screw = (cx, cy, r, rot) =>
  `<g transform="translate(${n(cx)} ${n(cy)}) rotate(${rot})">` +
  `<circle r="${n(r)}" fill="url(#se-metal)" stroke="#12161a" stroke-width="1"/>` +
  `<rect x="${n(-r * .7)}" y="${n(-r * .13)}" width="${n(r * 1.4)}" height="${n(r * .26)}" fill="#1a1f23"/>` +
  `<rect x="${n(-r * .13)}" y="${n(-r * .7)}" width="${n(r * .26)}" height="${n(r * 1.4)}" fill="#1a1f23"/></g>`;

const clampX = (cx, cy, s) =>
  `<g transform="translate(${n(cx)} ${n(cy)})">` +
  `<g transform="rotate(45)">` +
  `<rect x="${n(-s)}" y="${n(-s * .3)}" width="${n(s * 2)}" height="${n(s * .6)}" rx="1.5" fill="url(#se-metal)" stroke="#12161a" stroke-width="1"/>` +
  `<rect x="${n(-s * .3)}" y="${n(-s)}" width="${n(s * .6)}" height="${n(s * 2)}" rx="1.5" fill="url(#se-metal)" stroke="#12161a" stroke-width="1"/>` +
  `</g>${bolt(0, 0, s * .28)}</g>`;

// Housing outline at a given inset. Chamfered for angled-bracket strips, rounded otherwise.
const shape = (W, H, i, r, chamfer, attrs) => {
  if (chamfer) {
    const c = Math.max(0, chamfer - i);
    const pts = [[i + c, i], [W - i - c, i], [W - i, i + c], [W - i, H - i - c],
                 [W - i - c, H - i], [i + c, H - i], [i, H - i - c], [i, i + c]]
      .map(p => p.map(n).join(',')).join(' ');
    return `<polygon points="${pts}" ${attrs}/>`;
  }
  return `<rect x="${n(i)}" y="${n(i)}" width="${n(W - 2 * i)}" height="${n(H - 2 * i)}" rx="${n(Math.max(0, r - i))}" ${attrs}/>`;
};

function hardware(kind, W, H, B) {
  const m = B / 2;
  switch (kind) {
    case 'bolts': {
      const r = B * 0.3;
      return [[m, m], [W - m, m], [m, H - m], [W - m, H - m]].map(([x, y]) => bolt(x, y, r)).join('');
    }
    case 'clamps': {
      const s = B * 0.42;
      const corners = [[m, m], [W - m, m], [m, H - m], [W - m, H - m]].map(([x, y]) => clampX(x, y, s)).join('');
      const bars = [m, H - m].map(y =>
        `<rect x="${n(W / 2 - B * 1.1)}" y="${n(y - B * .16)}" width="${n(B * 2.2)}" height="${n(B * .32)}" rx="2" fill="url(#se-metal)" stroke="#12161a" stroke-width="1"/>`
      ).join('');
      return corners + bars;
    }
    case 'screws': {
      const r = B * 0.24;
      return [[m, m, 20], [W - m, m, 70], [m, H - m, 115], [W - m, H - m, 160]]
        .map(([x, y, a]) => screw(x, y, r, a)).join('');
    }
    case 'brackets': {
      const d = B * 0.24, sw = B * 0.17;
      const slash = (cx, off) =>
        `<line x1="${n(cx + off - d)}" y1="${n(H / 2 + d)}" x2="${n(cx + off + d)}" y2="${n(H / 2 - d)}" stroke="#7d8892" stroke-width="${n(sw)}" stroke-linecap="round"/>` +
        `<line x1="${n(cx + off - d)}" y1="${n(H / 2 + d + 1)}" x2="${n(cx + off + d)}" y2="${n(H / 2 - d + 1)}" stroke="#12161a" stroke-width="${n(sw * .35)}" stroke-linecap="round" opacity=".6"/>`;
      return [m, W - m].map(cx => slash(cx, -B * .16) + slash(cx, B * .16)).join('');
    }
    case 'mounts': {
      const d = B * 0.26;
      const chev = (cx, dir) =>
        `<polyline points="${n(cx - dir * d)},${n(H / 2 - d * 1.3)} ${n(cx + dir * d)},${n(H / 2)} ${n(cx - dir * d)},${n(H / 2 + d * 1.3)}" fill="none" stroke="#7d8892" stroke-width="${n(B * .16)}" stroke-linecap="round" stroke-linejoin="round"/>`;
      return chev(m, 1) + chev(W - m, -1);
    }
    default: return '';
  }
}

export function buildBezel(profile = 'TextPanel', opts = {}) {
  const p = PROFILES[profile];
  if (!p) throw new Error(`Unknown profile "${profile}". Valid: ${Object.keys(PROFILES).join(', ')}`);

  let { w, h } = p;
  if (profile === 'Auto') { w = 512 * (opts.cols ?? 1); h = 512 * (opts.rows ?? 1); }
  if (profile === 'Off')  { w = opts.width ?? w; h = opts.height ?? h; }
  if (opts.width && profile !== 'Off' && profile !== 'Auto') w = opts.width;   // manual override
  if (opts.height && profile !== 'Off' && profile !== 'Auto') h = opts.height;

  const bare = profile === 'Off';
  const B = bare ? 0 : (opts.bezel ?? Math.round(clamp(Math.min(w, h) * 0.1, 26, 44)));
  const W = w + 2 * B, H = h + 2 * B;
  const r = B * 0.4;
  const chamfer = p.hw === 'brackets' ? B : 0;
  const bg = opts.background ?? '#000';
  const content = opts.content ?? '';
  const title = `SE LCD - ${profile} ${w}x${h}`;

  const defs = `<defs>
    <linearGradient id="se-housing" x1="0" y1="0" x2="0" y2="1">
      <stop offset="0" stop-color="#5b656f"/><stop offset=".5" stop-color="#3b434a"/><stop offset="1" stop-color="#2a3035"/>
    </linearGradient>
    <linearGradient id="se-metal" x1="0" y1="0" x2="1" y2="1">
      <stop offset="0" stop-color="#8b96a0"/><stop offset=".55" stop-color="#4c565f"/><stop offset="1" stop-color="#2c3238"/>
    </linearGradient>
    <linearGradient id="se-glass" x1="0" y1="0" x2="1" y2="1">
      <stop offset="0" stop-color="#fff" stop-opacity=".10"/><stop offset=".35" stop-color="#fff" stop-opacity=".02"/>
      <stop offset=".36" stop-color="#fff" stop-opacity="0"/><stop offset="1" stop-color="#9fd0ff" stop-opacity=".05"/>
    </linearGradient>
    <pattern id="se-scan" width="4" height="4" patternUnits="userSpaceOnUse">
      <rect width="4" height="1.5" fill="#000" opacity=".22"/>
    </pattern>
  </defs>`;

  const housing = bare ? '' : `
  <g id="bezel">
    ${shape(W, H, 0, r, chamfer, 'fill="url(#se-housing)" stroke="#0d1013" stroke-width="2"')}
    ${shape(W, H, 1.5, r, chamfer, 'fill="none" stroke="#8d99a4" stroke-opacity=".35" stroke-width="1"')}
    ${shape(W, H, B * .62, Math.max(2, r * .5), 0, 'fill="none" stroke="#12161a" stroke-opacity=".55" stroke-width="1.2"')}
    <rect x="${B - 4}" y="${B - 4}" width="${w + 8}" height="${h + 8}" rx="3" fill="#020304" stroke="#000" stroke-width="2"/>
    <rect x="${B - 1}" y="${B - 1}" width="${w + 2}" height="${h + 2}" fill="none" stroke="#7d8892" stroke-opacity=".25" stroke-width="1"/>
  </g>
  <g id="hardware">${hardware(p.hw, W, H, B)}</g>`;

  const guide = bare
    ? `<rect x=".5" y=".5" width="${w - 1}" height="${h - 1}" fill="none" stroke="#888" stroke-dasharray="6 4" stroke-opacity=".5"/>`
    : '';

  const glass = bare ? '' : `
  <g id="glass" pointer-events="none">
    <svg x="${B}" y="${B}" width="${w}" height="${h}" viewBox="0 0 ${w} ${h}" overflow="hidden">
      <rect id="scanlines" width="${w}" height="${h}" fill="url(#se-scan)" opacity="${opts.scanlines ? 1 : 0}"/>
      <rect width="${w}" height="${h}" fill="url(#se-glass)"/>
    </svg>
  </g>`;

  return `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 ${W} ${H}" width="${W}" height="${H}" data-profile="${profile}" data-screen-width="${w}" data-screen-height="${h}" data-bezel="${B}">
  <title>${title}</title>
  ${defs}${housing}
  <!-- Screen content: local coordinate system = ${w}x${h} panel pixels -->
  <svg id="content" x="${B}" y="${B}" width="${w}" height="${h}" viewBox="0 0 ${w} ${h}" overflow="hidden">
    <rect id="screen-bg" width="${w}" height="${h}" fill="${bg}"/>
    ${guide}
    <g id="content-slot">${content}</g>
  </svg>${glass}
</svg>
`;
}
