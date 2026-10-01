// Validate the generated discovery surfaces, including production/project-path consistency.
const assert = require("node:assert/strict");
const fs = require("node:fs");
const path = require("node:path");
const site = require("../_data/site.js");

const output = path.resolve(__dirname, process.argv[2] || "../../public");
const read = (file) => fs.readFileSync(path.join(output, file), "utf8");
const sitemap = read("sitemap.xml");
const llms = read("llms.txt");
const urls = [...sitemap.matchAll(/<loc>([^<]+)<\/loc>/g)].map((match) => match[1]);
const pages = ["index.html", ...fs.readdirSync(path.join(output, "docs"), { withFileTypes: true })
  .filter((entry) => entry.isDirectory() && entry.name !== "src")
  .map((entry) => `docs/${entry.name}/index.html`), "docs/index.html"];
assert.equal(urls.length, pages.length, "Sitemap must contain every docs page and the landing page");
assert.equal(new Set(urls).size, urls.length, "Duplicate sitemap URL");
assert.ok(read("robots.txt").includes(`Sitemap: ${site.url}sitemap.xml`));
assert.ok(!llms.includes("&amp;"), "Plain-text context must not contain HTML-escaped titles");
const titles = new Set();
const descriptions = new Set();
for (const file of pages) {
  const html = read(file);
  const head = html.match(/<head>([\s\S]*?)<\/head>/)[1];
  const canonical = site.url + file.replace(/index\.html$/, "");
  assert.ok(urls.includes(canonical), `${file}: missing sitemap URL`);
  assert.equal([...head.matchAll(/rel="canonical"/g)].length, 1);
  assert.ok(head.includes(`rel="canonical" href="${canonical}"`), `${file}: canonical mismatch`);
  assert.ok(head.includes(`property="og:url" content="${canonical}"`));
  assert.ok(head.includes(`name="robots" content="${site.robots}"`));
  assert.ok(head.includes('name="twitter:card" content="summary"'));
  const title = head.match(/<title>([^<]+)<\/title>/)[1];
  const description = head.match(/name="description" content="([^"]+)"/)[1];
  assert.ok(!titles.has(title), `${file}: duplicate title`);
  assert.ok(!descriptions.has(description), `${file}: duplicate description`);
  titles.add(title);
  descriptions.add(description);
  const schema = JSON.parse(head.match(/type="application\/ld\+json">([\s\S]*?)<\/script>/)[1]);
  const page = schema["@graph"].find((entity) => ["WebPage", "TechArticle"].includes(entity["@type"]));
  assert.equal(page.url, canonical, `${file}: structured data URL mismatch`);
  assert.equal(schema["@graph"].find((entity) => entity["@type"] === "SoftwareSourceCode").version, site.version);
  if (file !== "index.html") {
    const markdown = head.match(/type="text\/markdown" href="([^"]+)"/)[1];
    assert.ok(markdown.startsWith(site.url + "docs/src/"));
    assert.ok(fs.existsSync(path.join(output, markdown.slice(site.url.length))), `${file}: missing Markdown`);
    assert.ok(llms.includes(`](${canonical})`), `${file}: missing AI index entry`);
    assert.ok(llms.includes(`](${markdown})`), `${file}: missing Markdown index entry`);
  }
}
console.log(`discovery check passed: ${pages.length} pages, canonical/social/JSON-LD metadata, sitemap and Markdown links`);
