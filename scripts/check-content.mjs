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

// Keep the published entry points aligned with the implemented visual controls.
const readText = (relative) => fs.readFileSync(path.join(root, relative), "utf8");
const core = readText("HelmCore.cs");
const gaugeKinds = core.match(/enum LineKind\s*\{([^}]+)\}/)?.[1].split(",").map((kind) => kind.trim()) || [];
const gauges = readText("docs/content/gauges.md");
const gaugeHeadings = [...gauges.matchAll(/^## .+\(`([^`]+)`/gm)].map((match) => match[1]);
const canonicalControls = [...core.matchAll(/if \(func == "([^"]+)"[^\n]+def\.Kind = LineKind\.(\w+);/g)];
for (const [, control, kind] of canonicalControls) {
  if (gaugeKinds.includes(kind) && !gaugeHeadings.includes(control)) failures.push(`Gauge reference: missing distinct ${control} section (${kind})`);
}
if (!gaugeKinds.length || canonicalControls.length !== gaugeKinds.length || gaugeHeadings.length !== gaugeKinds.length) {
  failures.push(`Gauge reference: ${gaugeHeadings.length} sections / ${canonicalControls.length} documented parser mappings / ${gaugeKinds.length} implemented controls`);
}
if (!gauges.includes(`badge: "${gaugeKinds.length} Types"`)) failures.push("Gauge reference: stale count badge");

const workshop = readText("WORKSHOP.md");
const workshopBody = workshop.match(/^## Steam Workshop Description[^]*?^```text\s*\n([^]*?)\n```/m)?.[1]?.trim();
if (!workshopBody) failures.push("WORKSHOP.md: missing publishable BBCode block");
else {
  if (workshopBody.length > 8000) failures.push(`WORKSHOP.md: description exceeds 8,000 characters (${workshopBody.length})`);
  if (!workshopBody.includes(`${gaugeKinds.length} visual controls`)) failures.push("WORKSHOP.md: stale visual-control count");
  for (const example of workshopBody.matchAll(/\[code\]\s*\n([^]*?)\[\/code\]/g)) {
    if (example[1].includes("@screen") && /^@end\s*$/m.test(example[1]) && !/@(?:template|banner)\b/.test(example[1])) {
      failures.push("WORKSHOP.md: screen example incorrectly closed with @end");
    }
  }
  const stack = [];
  for (const match of workshopBody.matchAll(/\[(\/)?(h1|h2|b|list|olist|url|code)(?:=[^\]]*)?\]/g)) {
    if (!match[1]) stack.push(match[2]);
    else if (stack.pop() !== match[2]) failures.push(`WORKSHOP.md: unbalanced BBCode near ${match[0]}`);
  }
  if (stack.length) failures.push("WORKSHOP.md: unclosed BBCode tags");
}
for (const relative of ["README.md", "WORKSHOP.md", "docs/content/getting-started.md", "steam-guide/en/01-first-display.txt"]) {
  if (/^surface:\s*block\(/m.test(readText(relative))) failures.push(`${relative}: unsupported surface: example; use output:`);
}
for (const relative of ["README.md", "WORKSHOP.md"]) {
  for (const required of ["https://sgkens.gitlab.io/helm-core/docs/", "https://sgkens.gitlab.io/helm-core/helm-toolbox/"]) {
    if (!readText(relative).includes(required)) failures.push(`${relative}: missing ${required}`);
  }
}

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
console.log(`content check passed: ${gaugeKinds.length} visual controls, Workshop BBCode, ${toolboxManifest.locales.length} Toolbox locale(s), ${guide.sections.length} Steam Guide sections`);
