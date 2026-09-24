const fs = require("node:fs");
const path = require("node:path");

const root = path.resolve(__dirname, "..");
const out = path.join(root, "vendor");
const files = [
  ["node_modules/@staticcanvas/lume/dist/lume.mjs", "lume.mjs"],
  ["node_modules/@staticcanvas/lume/src/lume.css", "lume.css"],
  ["node_modules/@staticcanvas/lume/LICENSE", "lume-LICENSE"],
  ["node_modules/@staticcanvas/logcad/dist/logcad.esm.js", "logcad.mjs"],
  ["node_modules/@staticcanvas/logcad/LICENSE", "logcad-LICENSE"],
  ["node_modules/prismjs/prism.js", "prism.js"],
  ["node_modules/prismjs/LICENSE", "prism-LICENSE"],
  ["../docs/helm-prism.js", "helm-prism.js"],
];

fs.mkdirSync(out, { recursive: true });
for (const [source, name] of files) {
  fs.copyFileSync(path.join(root, source), path.join(out, name));
}
console.log(`Copied ${files.length} browser runtime files to ${path.relative(root, out)}/`);
