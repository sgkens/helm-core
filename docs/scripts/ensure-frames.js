// Regenerates docs/assets/frames when it is missing or older than the layouts or the renderer sources.
// The frames are git-ignored output of tools/spritedump; CI builds them in the docs-frames job. Run before serving locally.
const fs = require("node:fs");
const path = require("node:path");
const { spawnSync } = require("node:child_process");

const docs = path.resolve(__dirname, "..");
const root = path.resolve(docs, "..");
const framesDir = path.join(docs, "assets", "frames");

function newest(p) {
  if (!fs.existsSync(p)) return 0;
  const st = fs.statSync(p);
  if (!st.isDirectory()) return st.mtimeMs;
  return fs.readdirSync(p).reduce((m, f) => Math.max(m, newest(path.join(p, f))), 0);
}

function oldest(p) {
  if (!fs.existsSync(p)) return 0;
  const files = fs.readdirSync(p).filter((f) => f.endsWith(".json")).map((f) => fs.statSync(path.join(p, f)).mtimeMs);
  return files.length ? Math.min(...files) : 0;
}

const layouts = fs.readdirSync(path.join(root, "layouts", "default")).filter((f) => f.endsWith(".txt")).map((f) => f.slice(0, -4));
const have = new Set(fs.existsSync(framesDir) ? fs.readdirSync(framesDir).map((f) => f.replace(/\.json$/, "")) : []);
const missing = layouts.filter((n) => !have.has(n));
const sources = Math.max(newest(path.join(root, "layouts", "default")), newest(path.join(root, "Program.cs")), newest(path.join(root, "HelmCore.cs")), newest(path.join(root, "tools", "spritedump", "Dump.cs")));
const stale = oldest(framesDir) < sources;

if (!missing.length && !stale) { console.log("frames: up to date (" + have.size + " files)"); process.exit(0); }
console.log("frames: " + (missing.length ? missing.length + " missing (" + missing.slice(0, 3).join(", ") + (missing.length > 3 ? ", ..." : "") + ")" : "older than the layouts or the renderer") + "; regenerating (about 40 s)");

const run = (cmd, args, cwd) => spawnSync(cmd, args, { cwd, stdio: "inherit" });
let r = run("node", ["scripts/extract-examples.js"], docs);
if (r.status === 0) r = run("dotnet", ["run", "--project", "../tools/spritedump", "--", "--out", "assets/frames", "--ticks", "48", "../layouts/default", ".examples"], docs);
if (r.status !== 0) console.warn("frames: could not regenerate (is the .NET SDK installed?). Previews of the affected layouts will 404 until it runs: bun run frames");
process.exit(0);
