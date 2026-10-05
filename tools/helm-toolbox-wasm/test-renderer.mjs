// Exercise the published WASM renderer, including the trimmed CI build.
// Usage: node tools/helm-toolbox-wasm/test-renderer.mjs <publish-directory>
import assert from "node:assert/strict";
import { readFileSync } from "node:fs";
import { join, resolve } from "node:path";
import { pathToFileURL } from "node:url";

const dist = resolve(process.argv[2]);
const { dotnet } = await import(pathToFileURL(join(dist, "wwwroot", "_framework", "dotnet.js")).href);
const runtime = await dotnet.withDiagnosticTracing(false).create();
const exports = await runtime.getAssemblyExports(runtime.getConfig().mainAssemblyName);
const dsl = `@bind fuel = group("Hydrogen Tanks", agg: avg)
@screen Animation
output: "LCD", 0
profile: standard
@panel tank at(0, 0, 1, 1)
line: "FUEL" tank(fuel.percent) gradient(#1e3a8a, #38bdf8)
`;

function render(seconds) {
  const frame = JSON.parse(exports.Host.Render(dsl, 512, 512, "fuel=50", seconds, 0));
  assert.deepEqual(frame.errors, [], "Animation layout must render without errors");
  assert.ok(frame.sprites.length > 0, "Animation layout must produce sprites");
  return frame.sprites;
}

const first = render(1);
assert.notDeepEqual(render(1.5), first, "Advancing time must animate tank waves and bubbles");
assert.deepEqual(render(1), first, "Rendering the same time must reproduce the same frame");

const machinery = `@bind machine = group("Assemblers", agg: avg)
@screen Machinery
output: "LCD", 0
bg: #101010
alert_period: 1.6
line: "MACHINE" flow(machine.producing)
`;
function machineFrame(seconds, alert = false, value = 100, layout = machinery) {
  const frame = JSON.parse(exports.Host.Render(layout, 512, 512, `machine=${value};__alert=${alert ? 1 : 0}`, seconds, 0));
  assert.deepEqual(frame.errors, [], "Machinery layout must render without errors");
  return frame.sprites;
}
const angle = frame => frame.find(s => s.d === "Triangle").r || 0;
const firstMachine = machineFrame(1);
assert.notEqual(angle(firstMachine), angle(machineFrame(1.5)), "Running machinery must rotate as time advances");
assert.deepEqual(machineFrame(1), firstMachine, "Returning to a paused machinery time must reproduce its frame");
assert.equal(angle(machineFrame(1, false, 0)), 0, "Idle machinery must not rotate");
for (const seconds of [0.2, 1.1, 1.8, 2.7]) {
  const active = seconds === 0.2 || seconds === 1.8;
  assert.deepEqual(machineFrame(seconds, true)[0].c, active ? [91, 15, 15, 255] : [16, 16, 16, 255], "Alert phase must match the requested time");
  assert.deepEqual(machineFrame(seconds, false)[0].c, [16, 16, 16, 255], "Disabling Alert must restore the normal background");
}
const fastAlert = machinery.replace("alert_period: 1.6", "alert_period: 0.4");
assert.deepEqual(machineFrame(0.1, true, 100, fastAlert)[0].c, [91, 15, 15, 255], "Custom alert period must enter its active phase");
assert.deepEqual(machineFrame(0.3, true, 100, fastAlert)[0].c, [16, 16, 16, 255], "Custom alert period must enter its normal phase");

const themes = readFileSync(new URL("../../layouts/default/power_liquid_themes.txt", import.meta.url), "utf8");
for (const [width, height] of [[512, 128], [256, 128]]) {
  for (let screen = 0; screen < 4; screen++) {
    const themedFrame = seconds => JSON.parse(exports.Host.Render(themes, width, height,
      "reactor=50;battery=50;solar=50;engine=50", seconds, screen));
    const frame = themedFrame(1);
    assert.deepEqual(frame.errors, [], "Each themed power screen must parse and render");
    assert.equal(frame.screens.length, 4, "Power preset must expose all four screens");
    assert.notDeepEqual(themedFrame(1.5).sprites, frame.sprites, "Each themed power gauge must animate");
  }
}

// A six-panel dashboard with four rows per panel reproduces the corner LCD
// density in shipped presets. Check the actual sprites emitted by Program.cs.
const dense = `@bind fuel = group("Hydrogen Tanks", agg: avg)
@screen Dense
output: "LCD", 0
profile: auto
layout: grid(3, 2)
` + Array.from({ length: 6 }, (_, i) => `@panel p${i} at(${i % 3}, ${Math.floor(i / 3)}, 1, 1) title: "POWER"
line: "FUEL ONE" bar(fuel.percent)
line: "FUEL TWO" bar(fuel.percent)
line: "FUEL TRI" bar(fuel.percent)
line: "FUEL END" bar(fuel.percent)
`).join("");

for (const [width, height, scale] of [[512, 128, 0.40], [256, 128, 0.30]]) {
  const frame = JSON.parse(exports.Host.Render(dense, width, height, "fuel=50", 1, 0));
  assert.deepEqual(frame.errors, [], "Dense corner layout must render without errors");
  const labels = frame.sprites.filter(s => s.t === "text" && s.d.startsWith("FUEL "));
  assert.equal(labels.length, 24, "Every dashboard row must be rendered");
  for (const label of labels) {
    assert.ok(Math.abs(label.s - scale) < 0.001, `${width}x${height} must use the reduced text scale`);
  }
  for (let panel = 0; panel < 6; panel++) {
    const rows = labels.slice(panel * 4, panel * 4 + 4);
    for (let i = 1; i < rows.length; i++) {
      assert.ok(rows[i].y - rows[i - 1].y >= 22 * scale, "Adjacent row text must not overlap vertically");
    }
  }
}
console.log("WASM renderer passed: tanks, machinery rotation, fixed-time frames, alert phases and dense corner LCD text sizing");
