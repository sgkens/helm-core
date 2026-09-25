import assert from "node:assert/strict";
import { readFile, readdir } from "node:fs/promises";
import { resolve } from "node:path";
import {
  addPanel, addScreen, createBaseConfig, panelInsertionOffset, parseDslModel,
  setLineField, setPanelField, setScreenDirective,
} from "../js/dsl-model.mjs";

const original = `# keep this comment
@screen Main
output: "Bridge LCD", 1
layout: grid(2, 1)
custom_future_directive: untouched
@panel power at(0, 0, 1, 1) title: "POWER" title_align:left
line: "BATTERY" bar(batt.percent) flat(#38bdf8) marker(25) color(batt.percent < 20, #ef4444)
@panel cargo at(1, 0, 1, 1) title: "CARGO"
line: "MASS" text(cargo.mass) unit("kg")
`;

let dsl = setScreenDirective(original, 0, "screen_bg", "rgba(10, 20, 30, 200)");
dsl = setScreenDirective(dsl, 0, "border_color", "#a855f7");
dsl = setPanelField(dsl, 0, 0, "title", "POWER GRID");
dsl = setPanelField(dsl, 0, 0, "title_align", "center");
dsl = setPanelField(dsl, 0, 0, "title_color", "cyan");
dsl = setPanelField(dsl, 0, 0, "col", "1");
dsl = setLineField(dsl, 0, 0, 0, "label", "CHARGE");
dsl = setLineField(dsl, 0, 0, 0, "colorMode", "gradient");
dsl = setLineField(dsl, 0, 0, 0, "colors", "#22c55e, #ef4444");

assert.match(dsl, /^# keep this comment/m);
assert.match(dsl, /^custom_future_directive: untouched$/m);
assert.match(dsl, /marker\(25\)/);
assert.match(dsl, /color\(batt\.percent < 20, #ef4444\)/);
assert.match(dsl, /screen_bg: rgba\(10, 20, 30, 200\)/);
assert.match(dsl, /border_color: #a855f7/);
assert.match(dsl, /@panel power at\(1, 0, 1, 1\).*title: "POWER GRID".*title_align:center.*title_color:cyan/);
assert.match(dsl, /line: "CHARGE" bar\(batt\.percent\).*gradient\(#22c55e, #ef4444\)/);

let nestedColour = 'line: "HEAT" bar(reactor.percent) flat(rgba(10, 20, 30, 180))';
assert.equal(parseDslModel(`@screen Main\n@panel p at(0,0,1,1)\n${nestedColour}\n`).screens[0].panels[0].lines[0].options.colors, "rgba(10, 20, 30, 180)");
nestedColour = setLineField(`@screen Main\n@panel p at(0,0,1,1)\n${nestedColour}\n`, 0, 0, 0, "colors", "rgba(30, 40, 50, 200)");
assert.match(nestedColour, /flat\(rgba\(30, 40, 50, 200\)\)$/m);

const parsed = parseDslModel(dsl);
assert.equal(parsed.screens.length, 1);
assert.equal(parsed.screens[0].panels.length, 2);
assert.equal(parsed.screens[0].panels[0].lines[0].options.colorMode, "gradient");
assert.equal(dsl.slice(panelInsertionOffset(dsl, 0, 0)).trimStart().startsWith("@panel cargo"), true);
assert.equal(parseDslModel(addPanel(dsl, 0)).screens[0].panels.length, 3);
assert.equal(parseDslModel(addScreen(dsl)).screens.length, 2);
assert.equal(parseDslModel(addScreen(dsl)).screens[1].directives.profile.value, "auto");
const baseConfig = parseDslModel(createBaseConfig());
assert.equal(baseConfig.screens[0].panels[0].lines.length, 1);
assert.equal(baseConfig.screens[0].directives.profile.value, "auto");

const implicit = parseDslModel("@screen compact\ntitle: STATUS\nline: \"OK\" text(x.value)\n");
assert.equal(implicit.screens[0].panels[0].implicit, true);
assert.equal(implicit.screens[0].panels[0].directives.title.value, "STATUS");

const presetsDir = resolve(import.meta.dirname, "../../layouts/default");
const presets = (await readdir(presetsDir)).filter((name) => name.endsWith(".txt"));
for (const name of presets) parseDslModel(await readFile(resolve(presetsDir, name), "utf8"));
assert.ok(presets.length >= 64, `expected the full preset library, found ${presets.length}`);

console.log(`dsl-model: ${presets.length} presets and round-trip mutation checks passed`);
