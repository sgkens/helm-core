// Parses docs/content/gauges.md into webutil/data/components.json: one entry per documented gauge
// kind (its DSL ids, description, example line and notes), plus the options table every gauge takes,
// cross-referenced against webutil/data/blocks.json so each component lists which blocks support it.
// Run: bun run data (in webutil/) — gen-blocks.js must run first (writes data/blocks.json).
const fs = require("node:fs");
const path = require("node:path");

const SRC = path.resolve(__dirname, "..", "..", "docs", "content", "gauges.md");
const BLOCKS = path.resolve(__dirname, "..", "data", "blocks.json");
const OUT = path.resolve(__dirname, "..", "data", "components.json");

function backticks(s) {
  return [...s.matchAll(/`([^`]+)`/g)].map((m) => m[1]);
}

if (!fs.existsSync(BLOCKS)) {
  throw new Error("gen-components: " + BLOCKS + " does not exist — run `bun run data` (gen-blocks.js writes it first), not this script alone.");
}
const blocksData = JSON.parse(fs.readFileSync(BLOCKS, "utf8"));

const text = fs.readFileSync(SRC, "utf8");
const lines = text.split(/\r?\n/);

// Split into top-level "## " sections, dropping the YAML frontmatter and anything before the first heading.
const sections = [];
let current = null;
for (const line of lines) {
  const h = line.match(/^##\s+(.+?)\s*$/);
  if (h) { current = { heading: h[1], body: [] }; sections.push(current); continue; }
  if (current) current.body.push(line);
}

const components = [];
let commonOptions = [];

for (const { heading, body } of sections) {
  if (/^Options Every Gauge Takes$/i.test(heading)) {
    for (const line of body) {
      if (!line.trim().startsWith("|") || /^\|\s*:?-+:?\s*\|/.test(line)) continue;
      const cells = line.trim().replace(/^\|/, "").replace(/\|$/, "").split("|").map((c) => c.trim());
      if (cells.length < 2 || /^Option$/i.test(cells[0])) continue;
      commonOptions.push({ option: cells[0].replace(/`/g, ""), effect: cells[1] });
    }
    continue;
  }

  const ids = backticks(heading);
  if (ids.length === 0) continue; // not a gauge-kind section (shouldn't happen given gauges.md's structure)
  const parenAt = heading.indexOf("(");
  const label = (parenAt === -1 ? heading : heading.slice(0, parenAt)).trim();

  let description = "";
  let example = "";
  let inFence = false;
  const details = [];
  for (const line of body) {
    if (line.trim().startsWith("```")) { inFence = !inFence; continue; }
    if (inFence) { example += (example ? "\n" : "") + line; continue; }
    const bullet = line.match(/^-\s+\*\*([^*]+)\*\*:\s*(.+)$/);
    if (bullet) { details.push({ label: bullet[1].trim(), text: bullet[2].trim() }); continue; }
    if (!description && line.trim() && !line.trim().startsWith("-")) description = line.trim();
  }

  const compatibleBlocks = blocksData.blocks
    .filter((b) => b.gauges.some((g) => ids.includes(g)))
    .map((b) => b.name)
    .sort((a, b) => a.localeCompare(b));

  components.push({ ids, label, description, example, details, compatibleBlocks });
}

if (components.length === 0) throw new Error("gen-components: no gauge sections parsed from " + SRC + " — has the format changed?");
if (commonOptions.length === 0) throw new Error("gen-components: no rows parsed from the \"Options Every Gauge Takes\" table in " + SRC);

fs.mkdirSync(path.dirname(OUT), { recursive: true });
fs.writeFileSync(OUT, JSON.stringify({ generatedFrom: "docs/content/gauges.md", components, commonOptions }, null, 2) + "\n");
console.log("gen-components: wrote " + components.length + " gauge kinds and " + commonOptions.length + " common options to " + path.relative(process.cwd(), OUT));
