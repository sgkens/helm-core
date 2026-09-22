// Static dev server for webutil/, serving this directory at the site root so its own relative paths
// (js/, data/, wasm/) resolve the same way they will in production. Falls back to ../public for the
// docs site's shared assets (/assets/js/se-lcd-bezel.mjs, /assets/js/sprite-svg.mjs, /css/style.css),
// matching how they sit side by side once deployed under public/webutil/ and public/assets/, public/css/.
// Usage: bun run serve   (PORT and HOST environment variables override the defaults)
import { existsSync, statSync } from "node:fs";
import { join, resolve, extname } from "node:path";

const HOST = process.env.HOST || "127.0.0.1";
const PORT = Number(process.env.PORT || 8891);
const HERE = resolve(import.meta.dir);
const DOCS_PUBLIC = resolve(HERE, "..", "public");

const TYPES = {
  ".html": "text/html; charset=utf-8", ".js": "text/javascript", ".mjs": "text/javascript",
  ".css": "text/css", ".json": "application/json", ".svg": "image/svg+xml", ".woff": "font/woff",
  ".webp": "image/webp", ".ico": "image/x-icon", ".wasm": "application/wasm", ".dat": "application/octet-stream",
  ".symbols": "text/plain", ".blat": "application/octet-stream", ".pdb": "application/octet-stream",
};

function resolveFile(root, urlPath) {
  const rel = decodeURIComponent(urlPath).replace(/^\/+/, "");
  const abs = resolve(root, rel);
  if (abs !== root && !abs.startsWith(root + "/")) return null; // no escaping the root
  try {
    let st = statSync(abs);
    let file = abs;
    if (st.isDirectory()) { file = join(abs, "index.html"); if (!existsSync(file)) return null; }
    return file;
  } catch { return null; }
}

Bun.serve({
  hostname: HOST,
  port: PORT,
  async fetch(req) {
    const url = new URL(req.url);
    let file = resolveFile(HERE, url.pathname) || resolveFile(DOCS_PUBLIC, url.pathname);
    if (!file) return new Response("Not found: " + url.pathname, { status: 404 });
    const type = TYPES[extname(file)] || "application/octet-stream";
    return new Response(Bun.file(file), { headers: { "content-type": type } });
  },
});

console.log(`webutil dev server: http://${HOST}:${PORT}/`);
if (!existsSync(join(HERE, "wasm", "wwwroot", "_framework", "dotnet.js"))) {
  console.warn("webutil: no build in webutil/wasm/ yet - run `bun run wasm` first, or the preview will fail to load.");
}
