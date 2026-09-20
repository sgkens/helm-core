// Extracts complete HELM examples from the docs (```helm fences that contain both @screen and @panel)
// into .examples/<id>.txt so tools/spritedump can parse them and export display frames.
// The id is derived from the block text, and eleventy.config.js computes the same id to place a
// visualizer under the code block. A parse error in any example fails the frame export.
const fs = require("node:fs");
const path = require("node:path");
const crypto = require("node:crypto");

const CONTENT = path.resolve(__dirname, "..", "content");
const OUT = path.resolve(__dirname, "..", ".examples");

const FENCE = /^```helm[^\n]*\n([\s\S]*?)^```/gm;

function exampleId(text) {
  return "ex-" + crypto.createHash("sha1").update(text.trim()).digest("hex").slice(0, 10);
}

function isCompleteExample(text) {
  return /^@screen\b/m.test(text) && /^@panel\b/m.test(text);
}

module.exports = { exampleId, isCompleteExample };

if (require.main === module) {
  fs.rmSync(OUT, { recursive: true, force: true });
  fs.mkdirSync(OUT, { recursive: true });
  let count = 0;
  for (const f of fs.readdirSync(CONTENT).filter((n) => n.endsWith(".md")).sort()) {
    const md = fs.readFileSync(path.join(CONTENT, f), "utf8");
    for (const m of md.matchAll(FENCE)) {
      if (!isCompleteExample(m[1])) continue;
      fs.writeFileSync(path.join(OUT, exampleId(m[1]) + ".txt"), m[1].trim() + "\n");
      count++;
    }
  }
  console.log(`extract-examples: ${count} complete example(s) written to ${path.relative(process.cwd(), OUT)}`);
}
