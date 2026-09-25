// Preset list generated from layouts/default/*.txt so counts and gallery entries cannot drift from the files.
const fs = require("node:fs");
const path = require("node:path");
const { hardwareProfile } = require("../profile-hardware.js");

const DIR = path.resolve(__dirname, "..", "..", "layouts", "default");

// Flagship layouts have hand-written sections in presets-gallery.md.
const FLAGSHIP = ["planetary_base", "command_carrier", "survival_essentials", "deep_space_explorer", "mining_fleet_coordinator", "atmospheric_lander"];

// Hardware profile used for the preview when the layout targets something other than a 1x1 panel.
const PROFILE_OVERRIDE = { command_carrier: "WideLCD", atmospheric_lander: "CockpitSmall", corner_gauge_cluster: "CockpitCorner" };

const CATEGORIES = [
  { id: "flight-vehicles", title: "Flight & Vehicles", description: "Cockpits, landers, rovers, propulsion and navigation." },
  { id: "industry-power", title: "Industry & Power", description: "Production, power generation, fuel and machinery." },
  { id: "cargo-logistics", title: "Cargo & Logistics", description: "Storage, docking, construction and station operations." },
  { id: "combat-safety", title: "Combat & Safety", description: "Weapons, alerts, security, hangars and life support." },
  { id: "examples-tests", title: "Examples & Test Labs", description: "Gauge showcases, styling recipes and renderer edge cases." },
];

function categoryFor(name) {
  if (/(fighter_hud|lander|rover|thruster|jump_drive|flight_deck|bridge_matrix|deep_space)/.test(name)) return "flight-vehicles";
  if (/(assembler|battery|reactor|refinery|hydrogen|solar|mining_rig|piston|power_only)/.test(name)) return "industry-power";
  if (/(cargo|docking|projector|trading|planetary_base|mining_fleet|survival_essentials)/.test(name)) return "cargo-logistics";
  if (/(airlock|hazard|medical|security|tactical|hangar|command_carrier)/.test(name)) return "combat-safety";
  return "examples-tests";
}

const isDecoration = (line) => /^#[\s=\-#]*$/.test(line.trim());

function titleCase(s) {
  return s.toLowerCase().replace(/\b[a-z]/g, (c) => c.toUpperCase()).replace(/\bPb\b/g, "PB").replace(/\bIgc\b/g, "IGC").replace(/\bLcd\b/g, "LCD");
}

function describe(name, text) {
  const comments = text.split(/\r?\n/).filter((l) => l.trim().startsWith("#") && !isDecoration(l) && !/^#\s*spritedump:/i.test(l.trim())).map((l) => l.replace(/^#\s*/, "").trim());
  let title = comments[0] || name;
  let rest = comments.slice(1);
  const m = title.match(/^HELM DISPLAY CONFIGURATION:\s*(.+)$/i);
  if (m) title = titleCase(m[1]);
  let description = rest.join(" ").replace(/\s+/g, " ").trim();
  const panels = [...text.matchAll(/^@panel\s+\S+.*?title:\s*"([^"]+)"/gm)].map((x) => x[1]);
  if (!description && panels.length) description = "Panels: " + panels.join(", ") + ".";
  const profileLine = (text.match(/^profile:\s*([^\s#]+)/im) || [])[1] || "standard";
  const profile = PROFILE_OVERRIDE[name] || hardwareProfile(profileLine);
  return { name, title, description, profile, panels, category: categoryFor(name) };
}

const all = fs.existsSync(DIR)
  ? fs.readdirSync(DIR).filter((f) => f.endsWith(".txt")).sort().map((f) => describe(path.basename(f, ".txt"), fs.readFileSync(path.join(DIR, f), "utf8")))
  : [];

module.exports = {
  all,
  count: all.length,
  others: all.filter((p) => !FLAGSHIP.includes(p.name)),
  groups: CATEGORIES.map((category) => ({
    ...category,
    presets: all.filter((p) => !FLAGSHIP.includes(p.name) && p.category === category.id),
  })).filter((group) => group.presets.length),
  // Layouts cycled on the splash page, as "name:Profile" pairs.
  splash: ["planetary_base:TextPanel", "command_carrier:WideLCD", "atmospheric_lander:CockpitSmall", "survival_essentials:TextPanel", "all_gauges_showcase:TextPanel"]
    .filter((s) => all.some((p) => p.name === s.split(":")[0])),
};
