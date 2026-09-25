// Stages HELM Toolbox into the repository's static public/ tree. The docs build supplies the sibling
// /assets and /css paths that index.html references, producing one deployable/self-hostable tree.
const fs = require("node:fs");
const path = require("node:path");

const root = path.resolve(__dirname, "..");
const out = path.resolve(root, "..", "public", "helm-toolbox");

const copies = [
  ["index.html", "index.html"],
  ["js", "js"],
  ["data", "data"],
  ["vendor", "vendor"],
  [path.join("wasm", "wwwroot"), path.join("wasm", "wwwroot")],
];

fs.rmSync(out, { recursive: true, force: true });
fs.mkdirSync(out, { recursive: true });
for (const [source, destination] of copies) {
  const from = path.join(root, source);
  if (!fs.existsSync(from)) throw new Error(`stage: missing ${from}; run the complete helm-toolbox build first`);
  fs.cpSync(from, path.join(out, destination), { recursive: true });
}

console.log(`stage: wrote static HELM Toolbox to ${path.relative(root, out)}/`);
