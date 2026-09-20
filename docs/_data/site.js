// Site-wide data: version comes from the repository VERSION file.
const fs = require("node:fs");
const path = require("node:path");

function readVersion() {
  try {
    const v = fs.readFileSync(path.resolve(__dirname, "..", "..", "VERSION"), "utf8").trim();
    if (v) return v.replace(/^v/i, "");
  } catch {}
  return "dev";
}

const version = readVersion();

// Code themes defined in css/theme-syntax-highlighting.css (data-syntax-theme values).
const syntaxThemes = [
  ["zerodark", "Zero Dark"], ["gruvbox", "Gruvbox"], ["tokyonight", "Tokyo Night"], ["macchiato", "Macchiato"],
  ["mocha", "Mocha"], ["cyberpink", "Cyberpink"], ["bearded-arc", "Bearded Arc"], ["ember", "Ember"],
  ["peach", "Peach"], ["marigold", "Marigold"], ["saffron", "Saffron"], ["jetamber", "Jet Amber"],
  ["citrus", "Citrus"], ["graphitelime", "Graphite Lime"], ["reef", "Reef"], ["basalt", "Basalt"],
  ["umbra", "Umbra"], ["inkviolet", "Ink Violet"], ["charcoalorchid", "Charcoal Orchid"],
  ["smokefuchsia", "Smoke Fuchsia"], ["rose", "Rose"], ["voidcoral", "Void Coral"],
  ["bearded-light", "Bearded Light (light)"], ["paperindigo", "Paper Indigo (light)"],
  ["linenteal", "Linen Teal (light)"], ["chalkcoral", "Chalk Coral (light)"],
  ["creamslate", "Cream Slate (light)"], ["fogviolet", "Fog Violet (light)"],
].map(([id, label]) => ({ id, label }));

module.exports = {
  syntaxThemes,
  version,
  versionLabel: /^\d/.test(version) ? "v" + version : version,
  repoUrl: "https://gitlab.com/sgkens/helm-core",
  branch: "main",
};
