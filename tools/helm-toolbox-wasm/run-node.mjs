// Spike harness for TASK-39.0: loads the published WebAssembly build in Node and renders layouts.
//   dotnet publish -c Release -o <dist> ; node run-node.mjs <dist> <layout.txt> [width height]
import { readFileSync } from "node:fs";
import { pathToFileURL } from "node:url";
import { join, resolve } from "node:path";

const dist = resolve(process.argv[2]);
const layout = readFileSync(process.argv[3], "utf8");
const w = +(process.argv[4] || 512), h = +(process.argv[5] || 512);

const t0 = performance.now();
const { dotnet } = await import(pathToFileURL(join(dist, "wwwroot", "_framework", "dotnet.js")).href);
const rt = await dotnet.withDiagnosticTracing(false).create();
const cfg = rt.getConfig();
const exp = await rt.getAssemblyExports(cfg.mainAssemblyName);
const tStart = performance.now() - t0;

const render = (values, seconds, screen = 0) => JSON.parse(exp.Host.Render(layout, w, h, values, seconds, screen));
const first = render("", 3.0);
const times = [];
for (let i = 0; i < 20; i++) { const a = performance.now(); render("h2=" + (10 + i * 4), 3 + i * 0.1); times.push(performance.now() - a); }
times.sort((a, b) => a - b);
console.log(JSON.stringify({
  startMs: Math.round(tStart), firstRenderErrors: first.errors, warnings: first.warnings, screens: first.screens,
  sprites: first.sprites.length, medianRenderMs: +times[10].toFixed(1), maxRenderMs: +times[19].toFixed(1),
}));
