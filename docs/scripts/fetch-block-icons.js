const fs = require("node:fs");
const path = require("node:path");

const API = "https://spaceengineers.wiki.gg/api.php";
const icons = require("../_data/blockIcons.json");
const output = path.resolve(__dirname, "..", "assets", "blocks");
const refresh = process.argv.includes("--refresh");
const pngSignature = Buffer.from([137, 80, 78, 71, 13, 10, 26, 10]);

function isCachedPng(file) {
  try { return fs.readFileSync(file).subarray(0, 8).equals(pngSignature); } catch { return false; }
}

function filePage(file) {
  return `https://spaceengineers.wiki.gg/wiki/File:${encodeURIComponent(file.replaceAll(" ", "_"))}`;
}

async function resolveImage(file) {
  const params = new URLSearchParams({
    action: "query", format: "json", prop: "imageinfo", iiprop: "url|mime|size", titles: `File:${file}`,
  });
  const response = await fetch(`${API}?${params}`, { headers: { "user-agent": "HELM-Core-docs/1.0 (cached documentation assets)" } });
  if (!response.ok) throw new Error(`wiki API returned ${response.status} for ${file}`);
  const page = Object.values((await response.json()).query.pages)[0];
  const info = page.imageinfo && page.imageinfo[0];
  if (!info || info.mime !== "image/png") throw new Error(`wiki has no PNG named ${file}`);
  return info.url;
}

async function download(name, meta) {
  const destination = path.join(output, `${meta.slug}.png`);
  if (!refresh && isCachedPng(destination)) return { name, meta, cached: true };
  const url = await resolveImage(meta.file);
  const response = await fetch(url, { headers: { "user-agent": "HELM-Core-docs/1.0 (cached documentation assets)" } });
  if (!response.ok) throw new Error(`image download returned ${response.status} for ${meta.file}`);
  const bytes = Buffer.from(await response.arrayBuffer());
  if (!bytes.subarray(0, 8).equals(pngSignature)) throw new Error(`${meta.file} was not a PNG`);
  fs.writeFileSync(destination, bytes);
  return { name, meta, cached: false };
}

async function main() {
  fs.mkdirSync(output, { recursive: true });
  const entries = Object.entries(icons);
  const results = [];
  for (let i = 0; i < entries.length; i += 6) {
    results.push(...await Promise.all(entries.slice(i, i + 6).map(([name, meta]) => download(name, meta))));
  }
  const lines = [
    "# Block icon attribution", "",
    "Cached from the [Official Space Engineers Wiki](https://spaceengineers.wiki.gg/) under its [CC BY-SA 4.0 license](https://creativecommons.org/licenses/by-sa/4.0/).",
    "The icons depict game content owned by Keen Software House and are used here for identification.", "",
    ...results.map(({ name, meta }) => `- ${name}: [${meta.file}](${filePage(meta.file)})`), "",
  ];
  fs.writeFileSync(path.join(output, "ATTRIBUTION.txt"), lines.join("\n"));
  const fetched = results.filter((result) => !result.cached).length;
  console.log(`block-icons: ${results.length} ready (${fetched} fetched, ${results.length - fetched} cached)`);
}

main().catch((error) => { console.error(`block-icons: ${error.message}`); process.exitCode = 1; });
