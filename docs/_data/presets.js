// Preset list generated from layouts/default/*.txt so counts and gallery entries cannot drift from the files.
const fs = require("node:fs");
const path = require("node:path");

const DIR = path.resolve(__dirname, "..", "..", "layouts", "default");

// Flagship layouts have hand-written sections in presets-gallery.md.
const FLAGSHIP = ["planetary_base", "command_carrier", "survival_essentials", "deep_space_explorer", "mining_fleet_coordinator", "atmospheric_lander"];

// Hardware profile used for the preview when the layout targets something other than a 1x1 panel.
const PROFILE_OVERRIDE = { command_carrier: "WideLCD", atmospheric_lander: "CockpitSmall", corner_gauge_cluster: "CockpitCorner" };

const isDecoration = (line) => /^#[\s=\-#]*$/.test(line.trim());

function titleCase(s) {
  return s.toLowerCase().replace(/\b[a-z]/g, (c) => c.toUpperCase()).replace(/\bPb\b/g, "PB").replace(/\bIgc\b/g, "IGC").replace(/\bLcd\b/g, "LCD");
}

function describe(name, text) {
  const comments = text.split(/\r?\n/).filter((l) => l.trim().startsWith("#") && !isDecoration(l)).map((l) => l.replace(/^#\s*/, "").trim());
  let title = comments[0] || name;
  let rest = comments.slice(1);
  const m = title.match(/^HELM DISPLAY CONFIGURATION:\s*(.+)$/i);
  if (m) title = titleCase(m[1]);
  let description = rest.join(" ").replace(/\s+/g, " ").trim();
  const panels = [...text.matchAll(/^@panel\s+\S+.*?title:\s*"([^"]+)"/gm)].map((x) => x[1]);
  if (!description && panels.length) description = "Panels: " + panels.join(", ") + ".";
  const profileLine = (text.match(/^profile:\s*(\w+)/m) || [])[1] || "standard";
  const profile = PROFILE_OVERRIDE[name] || (profileLine === "corner" ? "CornerLCD" : "TextPanel");
  return { name, title, description, profile, panels };
}

const all = fs.existsSync(DIR)
  ? fs.readdirSync(DIR).filter((f) => f.endsWith(".txt")).sort().map((f) => describe(path.basename(f, ".txt"), fs.readFileSync(path.join(DIR, f), "utf8")))
  : [];

module.exports = {
  all,
  count: all.length,
  others: all.filter((p) => !FLAGSHIP.includes(p.name)),
  // Layouts cycled on the splash page, as "name:Profile" pairs.
  splash: ["planetary_base:TextPanel", "command_carrier:WideLCD", "atmospheric_lander:CockpitSmall", "survival_essentials:TextPanel", "all_gauges_showcase:TextPanel"]
    .filter((s) => all.some((p) => p.name === s.split(":")[0])),
};
