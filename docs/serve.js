// Static dev server for ../public, bound to 127.0.0.1 (IPv4 loopback) with live reload.
// Eleventy's built-in --serve has no host option and listens on the dual-stack wildcard,
// which is slow to reach from Windows through WSL2 port forwarding via 127.0.0.1.
// Usage: bun serve.js   (PORT and HOST environment variables override the defaults)
const fs = require("node:fs");
const path = require("node:path");

const HOST = process.env.HOST || "127.0.0.1";
const PORT = Number(process.env.PORT || 8890);
const ROOT = path.resolve(import.meta.dir, "..", "public");

const RELOAD_SNIPPET =
  '<script>new EventSource("/__reload").onmessage=()=>location.reload();</script>';

const clients = new Set();
const enc = new TextEncoder();

function broadcast() {
  for (const send of clients) send();
}

let timer = null;
fs.mkdirSync(ROOT, { recursive: true });
fs.watch(ROOT, { recursive: true }, () => {
  clearTimeout(timer);
  timer = setTimeout(broadcast, 150);
});

function resolveFile(urlPath) {
  const rel = decodeURIComponent(urlPath).replace(/^\/+/, "");
  const abs = path.resolve(ROOT, rel);
  if (abs !== ROOT && !abs.startsWith(ROOT + path.sep)) return null;
  try {
    const st = fs.statSync(abs);
    if (st.isDirectory()) {
      const idx = path.join(abs, "index.html");
      return fs.existsSync(idx) ? idx : null;
    }
    return abs;
  } catch {
    return null;
  }
}

Bun.serve({
  hostname: HOST,
  port: PORT,
  idleTimeout: 0,
  async fetch(req) {
    const url = new URL(req.url);

    if (url.pathname === "/__reload") {
      let send;
      const stream = new ReadableStream({
        start(controller) {
          send = () => controller.enqueue(enc.encode("data: reload\n\n"));
          clients.add(send);
          controller.enqueue(enc.encode(": connected\n\n"));
        },
        cancel() {
          clients.delete(send);
        },
      });
      return new Response(stream, {
        headers: { "Content-Type": "text/event-stream", "Cache-Control": "no-store" },
      });
    }

    const file = resolveFile(url.pathname);
    if (!file) return new Response("Not found", { status: 404 });

    if (file.endsWith(".html")) {
      const html = (await Bun.file(file).text()).replace("</body>", RELOAD_SNIPPET + "</body>");
      return new Response(html, {
        headers: { "Content-Type": "text/html; charset=utf-8", "Cache-Control": "no-store" },
      });
    }
    return new Response(Bun.file(file), { headers: { "Cache-Control": "no-store" } });
  },
});

console.log(`[serve] http://${HOST}:${PORT}/docs/  (serving ${ROOT})`);
