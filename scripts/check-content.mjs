import fs from "node:fs";
import path from "node:path";
import process from "node:process";
import { fileURLToPath } from "node:url";

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const failures = [];
const readJson = (relative) => JSON.parse(fs.readFileSync(path.join(root, relative), "utf8"));
const requireFile = (relative) => {
  if (!fs.existsSync(path.join(root, relative))) failures.push(`missing ${relative}`);
};

const localeManifest = readJson("locales/locales.json");
if (localeManifest.default !== "en") failures.push("locales/locales.json: English must remain the fallback locale");
for (const locale of localeManifest.locales) {
  requireFile(`locales/${locale.id}/terminology.json`);
}

const toolboxManifest = readJson("helm-toolbox/locales/index.json");
const english = readJson("helm-toolbox/locales/en.json");
const toolboxSources = ["helm-toolbox/index.html", "helm-toolbox/js/app.mjs"]
  .map((relative) => fs.readFileSync(path.join(root, relative), "utf8")).join("\n");
const referencedKeys = new Set();
for (const match of toolboxSources.matchAll(/(?:data-i18n(?:-placeholder|-aria-label)?=["']|\bt\(["'])([\w.-]+)/g)) referencedKeys.add(match[1]);
for (const key of referencedKeys) {
  if (!String(english[key] || "").trim()) failures.push(`helm-toolbox/locales/en.json: missing referenced key ${key}`);
}
for (const locale of toolboxManifest.locales) {
  const relative = `helm-toolbox/locales/${locale.id}.json`;
  requireFile(relative);
  if (!fs.existsSync(path.join(root, relative))) continue;
  const catalog = readJson(relative);
  for (const key of Object.keys(english)) {
    if (!String(catalog[key] || "").trim()) failures.push(`${relative}: missing English key ${key}`);
  }
}

const guide = readJson("steam-guide/manifest.json");
if (guide.language !== "en") failures.push("steam-guide/manifest.json: source guide language must be en");
for (const section of guide.sections) {
  const relative = `steam-guide/${section.file}`;
  requireFile(relative);
  if (!fs.existsSync(path.join(root, relative))) continue;
  const text = fs.readFileSync(path.join(root, relative), "utf8");
  if (text.length > 8000) failures.push(`${relative}: ${text.length} characters exceeds the 8,000-character publishing guard`);
  if (/\b(?:TODO|TBD)\b/.test(text)) failures.push(`${relative}: contains an unfinished placeholder`);
}
const guideText = guide.sections.map((section) => {
  const file = path.join(root, "steam-guide", section.file);
  return fs.existsSync(file) ? fs.readFileSync(file, "utf8") : "";
}).join("\n");
for (const required of ["reload", "https://sgkens.gitlab.io/helm-core/docs/", "https://gitlab.com/sgkens/helm-core"]) {
  if (!guideText.includes(required)) failures.push(`Steam Guide: missing required reference ${required}`);
}

for (const relative of [
  "architecture/README.md", "architecture/system-overview.md", "architecture/runtime-rendering.md",
  "architecture/dsl-and-data.md", "architecture/build-test-release.md",
  "architecture/decisions/README.md", "architecture/decisions/0000-template.md",
]) requireFile(relative);

if (failures.length) {
  console.error(`content check failed:\n- ${failures.join("\n- ")}`);
  process.exit(1);
}
console.log(`content check passed: ${toolboxManifest.locales.length} Toolbox locale(s), ${guide.sections.length} Steam Guide sections`);
