# webutil

A browser tool that renders a HELM layout with the real script, compiled to WebAssembly (`tools/webutil-wasm`), so the preview cannot drift from what the game draws. Deployed alongside the docs site at `/webutil/` (see the `webutil` job in `.gitlab-ci.yml`).

## What it does today (an alpha, not the full concept tool of Epic 39)

- Write or paste Custom Data in the text pane; the preview re-renders about every 200 ms, with the real parser's errors and warnings shown inline.
- Pick a starter layout or a screen surface profile.
- A slider per `@bind`, on the script's own 0–100 scale, drives the live values (and the tank wave/bubbles, easing and calibration keep animating on a slow timer).
- A block and field reference on the left, generated from `docs/content/block-matrix.md`; click a field to insert a `line:` for it at the cursor.
- Copy the Custom Data to the clipboard. The DSL is auto-saved to the browser's `localStorage` between visits.

## What it does not do yet

Dragging blocks and gauges onto a canvas, editing panel spans visually, colour and gradient pickers, mirror/multi-screen editing beyond picking which screen to preview, scenario buttons, saved named projects, a share link, and the CI parity check against `tools/spritedump`. These are TASK-39.2 to 39.4, 39.6, 39.8, 39.13, 39.14, 39.15, 39.17 and 39.18 in `DEVTRACKER-PENDING.md` (git-ignored).

## Layout

```
webutil/
  index.html       page shell (reuses the docs site's compiled Tailwind/daisyUI CSS)
  js/
    app.mjs         state, DOM wiring, the block palette and value sliders
    wasm-bridge.mjs  loads tools/webutil-wasm's published build and calls Host.Render
  scripts/
    gen-blocks.js    parses docs/content/block-matrix.md -> data/blocks.json (TASK-39.12)
    gen-starters.js  copies a few layouts/default/*.txt presets -> data/starters.json
  data/              generated (gitignored)
  vendor/            pinned Lume/Logcad browser files staged by `bun run vendor` (gitignored)
  wasm/              tools/webutil-wasm, published here by `bun run wasm` (gitignored, ~9 MB uncompressed)
  server.mjs         local dev server; serves this directory, falling back to ../public for
                      the shared docs assets (assets/js/*.mjs, css/style.css)
```

`../docs/assets/js/sprite-svg.mjs` and `se-lcd-bezel.mjs` are shared with the docs preview widget (`help-visualizer.js`), not duplicated here, so the two cannot draw sprites differently.

## Running it locally

```bash
# repository root, bash. Needs the .NET 10 SDK (see tools/webutil-wasm/README.md) and Bun.
cd docs && bun run css:build && cd ..   # webutil reuses the docs site's compiled CSS
cd webutil
bun install
bun run build   # bun run data && bun run vendor && bun run wasm
bun run serve   # http://127.0.0.1:8891/
```

`bun run dev` does both in one step. Rerun `bun run data` after editing `docs/content/block-matrix.md` or `layouts/default/*.txt`, and `bun run wasm` after editing `Program.cs` or `HelmCore.cs`.

Lume supplies mutation-aware contextual help for both static controls and generated field buttons. Logcad supplies the branded startup message and retained error diagnostics. The vendor step stages both packages locally, so the deployed tool does not depend on a CDN. Shared asset URLs are relative, and CI preserves the `wasm/wwwroot/` directory, so the artifact also works when GitLab Pages hosts it beneath a project path.

The block palette also uses the same cached Official Space Engineers Wiki icon manifest as the docs support matrix. Refresh those attributed, up-to-128 px PNGs with `cd ../docs && bun run icons:refresh`; normal docs builds validate and reuse the cache.

## Verified

Loaded and exercised in a real headless Chromium (22 SEP 2026): the block palette, starter presets, the multi-screen picker, value sliders re-rendering the preview, and a parser error surfacing with its real line number, all with no console errors. Not yet checked in a non-Chromium browser.
