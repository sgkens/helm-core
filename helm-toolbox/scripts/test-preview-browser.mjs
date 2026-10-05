// Run against a built, running source or staged Toolbox; see README.md.
import assert from "node:assert/strict";
import { chromium, firefox } from "playwright";

const urls = process.argv.slice(2);
if (!urls.length) urls.push("http://127.0.0.1:8891/");
const dsl = `@bind machine = group("Assemblers", agg: avg)
@screen Machinery
output: "LCD", 0
bg: #101010
line: "MACHINE" flow(machine.producing)
`;

for (const [name, engine] of Object.entries({ chromium, firefox })) {
  const browser = await engine.launch();
  try {
    for (const url of urls) {
      const page = await browser.newPage({ reducedMotion: "no-preference" });
      const errors = [], messages = [];
      page.on("pageerror", e => errors.push(e.message));
      page.on("console", m => messages.push(m.text()));
      // Keep this local test independent of the external font service.
      await page.route("https://fonts.googleapis.com/**", r => r.fulfill({ contentType: "text/css", body: "" }));
      await page.route("**/js/app.mjs", async route => {
        const response = await route.fetch();
        const source = await response.text();
        const call = "renderSprites(slot, result.sprites,";
        assert.ok(source.includes(call), "Preview error injection must target the real drawing call");
        await route.fulfill({ response, body: source.replace(call,
          'if (globalThis.__failPreviewOnce) { globalThis.__failPreviewOnce = false; throw new Error("preview-test-failure"); } ' + call) });
      });
      await page.goto(url, { waitUntil: "domcontentloaded" });
      await page.waitForSelector("#content-slot", { timeout: 45000 });
      await page.locator("#dsl-editor").fill(dsl);
      await page.waitForSelector("#content-slot polygon");
      const angle = () => page.locator("#content-slot polygon").first().getAttribute("transform");
      const before = await angle();
      await page.waitForTimeout(550);
      assert.notEqual(await angle(), before, "Machinery must rotate while playing");

      // Editing a component property must not stop the animation timer.
      await page.locator('[data-inspector-tab="component"]').click();
      await page.locator('[data-line-field="ref"]').fill("machine.running");
      await page.locator('[data-line-field="ref"]').dispatchEvent("change");
      await page.waitForTimeout(400);
      const edited = await angle();
      await page.waitForTimeout(550);
      assert.notEqual(await angle(), edited, "Animation must continue after a property edit");

      // Same alias, different metadata: controls must refresh rather than stay stale.
      await page.locator("#dsl-editor").fill(dsl.replace('group("Assemblers", agg: avg)', 'block("Assembler One")'));
      await page.waitForFunction(() => document.querySelector('#value-sliders [data-lume]')?.getAttribute("data-lume") === 'block("Assembler One")');
      await page.locator("#dsl-editor").fill(dsl.replace('group("Assemblers", agg: avg)', 'dynamic("STATE", default: "RUN")'));
      await page.waitForFunction(() => document.querySelectorAll("#value-sliders input").length === 0);
      await page.locator("#dsl-editor").fill(dsl);
      await page.waitForSelector("#value-sliders input", { state: "attached" });

      await page.locator("#play-toggle").click();
      await page.waitForTimeout(400);
      const paused = await angle();
      await page.waitForTimeout(550);
      assert.equal(await angle(), paused, "Pause must hold the machinery frame");
      await page.locator("#alert-toggle").click();
      await page.locator("#playback-note").waitFor({ state: "visible" });
      await page.locator("#play-toggle").click();
      const colors = new Set();
      for (let i = 0; i < 16; i++) {
        await page.waitForTimeout(150);
        colors.add(await page.locator("#content-slot > *").first().getAttribute("fill"));
      }
      assert.ok(colors.has("rgb(91,15,15)") && colors.has("rgb(16,16,16)"), "Alert must visit both flash phases");
      await page.locator("#alert-toggle").click();
      await page.waitForTimeout(400);
      assert.equal(await page.locator("#content-slot > *").first().getAttribute("fill"), "rgb(16,16,16)");

      // Repeat the transition to catch alert state retained from a previous cycle.
      await page.locator("#alert-toggle").click();
      const repeatedColors = new Set();
      for (let i = 0; i < 16; i++) {
        await page.waitForTimeout(150);
        repeatedColors.add(await page.locator("#content-slot > *").first().getAttribute("fill"));
      }
      assert.ok(repeatedColors.has("rgb(91,15,15)") && repeatedColors.has("rgb(16,16,16)"), "Re-enabling Alert must flash again");
      await page.locator("#alert-toggle").click();
      await page.waitForTimeout(400);
      assert.equal(await page.locator("#content-slot > *").first().getAttribute("fill"), "rgb(16,16,16)");

      await page.evaluate(() => { globalThis.__failPreviewOnce = true; });
      await page.waitForFunction(() => globalThis.__failPreviewOnce === false);
      await page.waitForTimeout(500);
      assert.ok(messages.some(m => m.includes("preview-test-failure")), "Injected drawing error must be reported");
      assert.equal(await page.locator("#preview-status").textContent(), "", "Playback must recover from a drawing error");
      const recovered = await angle();
      await page.waitForTimeout(550);
      assert.notEqual(await angle(), recovered, "Drawing failure must not kill the animation timer");
      assert.deepEqual(errors, [], "Browser must have no unhandled page errors");

      for (const map of ["logcad.esm.js.map", "lume.mjs.map"]) {
        const response = await page.request.get(new URL(`vendor/${map}`, url).href);
        assert.equal(response.status(), 200, "Vendored source maps must be served");
      }
      await page.close();
      console.log(`${name}: preview controls, metadata edits, alerts and error recovery passed at ${url}`);
    }
    const motionPage = await browser.newPage({ reducedMotion: "reduce" });
    await motionPage.route("https://fonts.googleapis.com/**", r => r.fulfill({ contentType: "text/css", body: "" }));
    await motionPage.goto(urls[0], { waitUntil: "domcontentloaded" });
    await motionPage.waitForSelector("#content-slot", { timeout: 45000 });
    assert.equal(await motionPage.locator("#play-toggle").getAttribute("aria-pressed"), "false", "Reduced motion must initially pause playback");
    await motionPage.locator("#play-toggle").click();
    assert.equal(await motionPage.locator("#play-toggle").getAttribute("aria-pressed"), "true", "Play must override the initial reduced-motion pause");
    await motionPage.close();
    console.log(`${name}: reduced-motion pause and explicit Play passed`);
  } finally {
    await browser.close();
  }
}
