// Copies a handful of layouts/default/*.txt presets into webutil/data/starters.json, so the tool
// can offer a starting point without shipping every layout (65 files) to the browser. Run: bun run data.
const fs = require("node:fs");
const path = require("node:path");

const DIR = path.resolve(__dirname, "..", "..", "layouts", "default");
const OUT = path.resolve(__dirname, "..", "data", "starters.json");

// One of each shape worth starting from: a minimal screen, a grid, tanks and columns, and the
// gauge_lab / combo_dashboard showcases from Epic 40.
const NAMES = ["single_line", "survival_essentials", "tank_trio", "vertical_bars", "gauge_lab", "combo_dashboard", "background_lab"];

const starters = NAMES.map((name) => {
  const file = path.join(DIR, name + ".txt");
  if (!fs.existsSync(file)) throw new Error("gen-starters: " + file + " does not exist (renamed or removed preset?)");
  const text = fs.readFileSync(file, "utf8");
  const title = (text.match(/^#\s*(.+)$/m) || [, name])[1];
  return { name, title, dsl: text };
});

fs.mkdirSync(path.dirname(OUT), { recursive: true });
fs.writeFileSync(OUT, JSON.stringify(starters, null, 2) + "\n");
console.log("gen-starters: wrote " + starters.length + " starters to " + path.relative(process.cwd(), OUT));
