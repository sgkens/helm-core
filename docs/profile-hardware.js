// Maps a DSL layout profile to the physical preview used by documentation examples.
// The C# renderer remains authoritative for layout scaling; this only chooses a bezel/resolution.
function hardwareProfile(value) {
  const p = String(value || "standard").trim().toLowerCase().replace(/[_\s-]/g, "");
  if (p === "cockpitcorner") return "CockpitCorner";
  if (p === "corner" || p === "cornerlcd") return "CornerLCD";
  if (p === "compact" || p === "cockpitsmall") return "CockpitSmall";
  if (p === "wide" || p === "widelcd") return "WideLCD";
  return "TextPanel";
}

module.exports = { hardwareProfile };
