// Copies every layouts/default/*.txt preset into helm-toolbox/data/starters.json. Metadata comes from
// the docs gallery's own data module so the two catalogues cannot disagree. Run: bun run data.
const fs = require("node:fs");
const path = require("node:path");
const presets = require("../../docs/_data/presets.js");

const DIR = path.resolve(__dirname, "..", "..", "layouts", "default");
const OUT = path.resolve(__dirname, "..", "data", "starters.json");

const categoryTitles = Object.fromEntries(presets.groups.map((group) => [group.id, group.title]));
const starters = presets.all.map((preset) => {
  const file = path.join(DIR, preset.name + ".txt");
  if (!fs.existsSync(file)) throw new Error("gen-starters: " + file + " does not exist (renamed or removed preset?)");
  return {
    name: preset.name,
    title: preset.title,
    description: preset.description,
    category: preset.category,
    categoryTitle: categoryTitles[preset.category] || "Other",
    profile: preset.profile,
    dsl: fs.readFileSync(file, "utf8"),
  };
});

fs.mkdirSync(path.dirname(OUT), { recursive: true });
fs.writeFileSync(OUT, JSON.stringify(starters, null, 2) + "\n");
console.log("gen-starters: wrote " + starters.length + " starters to " + path.relative(process.cwd(), OUT));
