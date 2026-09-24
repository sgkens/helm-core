// wasm-bridge.mjs: loads the WebAssembly build of the real HELM script (tools/webutil-wasm) and
// exposes one async function, render(...), returning the parsed JSON Host.Render produces.
// The module is not fetched until init() is called, so opening the page costs nothing until the
// first preview is actually needed (about 1.7 MB brotli; see tools/webutil-wasm/README.md).

// Resolved relative to this module's own URL, so it works whether the page is served at the site
// root (local dev) or under /webutil/ (production).
const WASM_BASE = new URL("../wasm/wwwroot", import.meta.url).pathname;

let exportsPromise = null;

function init() {
  if (!exportsPromise) {
    exportsPromise = (async () => {
      // The runtime resolves sibling asset paths (dotnet.native.wasm, the app assembly, ...) relative
      // to dotnet.js's own URL, so importing it from webutil/wasm/wwwroot/_framework/ is enough - no
      // custom resource loader needed even though this page itself is not served from that directory.
      const { dotnet } = await import(`${WASM_BASE}/_framework/dotnet.js`);
      const runtime = await dotnet.withDiagnosticTracing(false).create();
      return await runtime.getAssemblyExports(runtime.getConfig().mainAssemblyName);
    })().catch((e) => { exportsPromise = null; throw e; });
  }
  return exportsPromise;
}

// dsl: the Custom Data text. width/height: the surface size in pixels. values: {bindName: 0-100}.
// seconds: drives the script's clock (waves, bubbles, easing). screenIndex: which @screen to render.
// alert: preview the "alert red" run-argument command (Program.cs's _globalRedAlert) — not a @bind,
// so it rides along as a "__alert" pseudo-entry in the same values string Host.Render already takes,
// rather than changing Render's own parameter list.
export async function render(dsl, width, height, values, seconds, screenIndex, alert = false) {
  const exp = await init();
  const valueStr = Object.entries(values || {}).map(([k, v]) => `${k}=${v}`).join(";") + `;__alert=${alert ? 1 : 0}`;
  const json = exp.Host.Render(dsl, width | 0, height | 0, valueStr, +seconds || 0, screenIndex | 0);
  return JSON.parse(json);
}

// Every @template registered with the real parser: the 8 built into HelmCore.cs plus
// layouts/templates.txt (embedded into the WASM build; see tools/webutil-wasm/Host.cs). A param
// with default === null has no default in @template's header, so @use requires it.
export async function listTemplates() {
  const exp = await init();
  return JSON.parse(exp.Host.ListTemplates());
}

export function isReady() {
  return exportsPromise !== null;
}
