// Parses docs/content/block-matrix.md into webutil/data/blocks.json, so the palette and field
// picker never drift from the documented block matrix (TASK-39.12). Run: bun run data (in webutil/).
const fs = require("node:fs");
const path = require("node:path");

const SRC = path.resolve(__dirname, "..", "..", "docs", "content", "block-matrix.md");
const OUT = path.resolve(__dirname, "..", "data", "blocks.json");

function cells(line) {
  return line.trim().replace(/^\|/, "").replace(/\|$/, "").split("|").map((c) => c.trim());
}

function backticks(s) {
  return [...s.matchAll(/`([^`]+)`/g)].map((m) => m[1]);
}

const text = fs.readFileSync(SRC, "utf8");
const lines = text.split(/\r?\n/);

const blocks = [];
const itemFields = [];
let section = null;
for (const line of lines) {
  if (/^##\s+Complete Block Matrix/.test(line)) { section = "blocks"; continue; }
  if (/^##\s+Item Fields/.test(line)) { section = "items"; continue; }
  if (/^##\s/.test(line)) { section = null; continue; }
  if (!line.trim().startsWith("|") || /^\|\s*:?-+:?\s*\|/.test(line)) continue;

  if (section === "blocks" && /\*\*\[x\]\*\*|\*\*\[ \]\*\*/.test(line)) {
    const c = cells(line);
    if (c.length < 7 || /^Category$/i.test(c[0])) continue;
    blocks.push({
      category: c[0].replace(/\*\*/g, ""),
      name: c[1],
      interface: backticks(c[2])[0] || "",
      live: c[3].includes("[x]"),
      fields: backticks(c[4]).map((f) => f.replace(/^\./, "")),
      gauges: backticks(c[5]),
      units: c[6] === "-" ? null : c[6].replace(/`/g, ""),
    });
  } else if (section === "items" && line.includes("`")) {
    const c = cells(line);
    if (c.length < 3 || /^Field$/i.test(c[0])) continue;
    itemFields.push({ field: backticks(c[0])[0] || c[0], reads: c[1].replace(/`/g, ""), example: backticks(c[2])[0] || "" });
  }
}

if (blocks.length === 0) throw new Error("gen-blocks: no block rows parsed from " + SRC + " — has the table format changed?");

fs.mkdirSync(path.dirname(OUT), { recursive: true });
fs.writeFileSync(OUT, JSON.stringify({ generatedFrom: "docs/content/block-matrix.md", blocks, itemFields }, null, 2) + "\n");
console.log("gen-blocks: wrote " + blocks.length + " block kinds and " + itemFields.length + " item fields to " + path.relative(process.cwd(), OUT));
