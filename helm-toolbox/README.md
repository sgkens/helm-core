# HELM Toolbox

A browser tool that renders a HELM layout with the real script, compiled to WebAssembly (`tools/helm-toolbox-wasm`), so the preview cannot drift from what the game draws. Deployed alongside the docs site at `/helm-toolbox/` (see the `helm-toolbox` job in `.gitlab-ci.yml`).

## What it does today (an alpha, not the full concept tool of Epic 39)

- Write or paste Custom Data in the syntax-highlighted text pane; the preview re-renders about every 200 ms, with the real parser's errors and warnings shown inline. The editor and palette examples use the docs site's shared Prism HELM grammar and selected syntax theme.
- Drag the divider between the preview and Custom Data panes to give either one more room. The divider supports keyboard resizing, the editor toolbar has an expand/restore shortcut, and the chosen height is remembered locally.
- Pick any layout preset or a screen surface profile. The preset picker contains every file in `layouts/default/` (65 at the time of writing), grouped with the same categories and metadata as the docs gallery.
- A slider per `@bind`, on the script's own 0–100 scale, drives the live values (and the tank wave/bubbles, easing and calibration keep animating on a slow timer). An Alert toggle previews the `alert red` run-argument command (flashes each screen's background to its `alert_bg`).
- Pick any `@screen`, including a `mirror:`/`mirror_panel:` one — its source is rendered alongside it so the copy is never blank, the same as the game's own round-robin fills it in over several ticks.
- A block and field reference on the left, generated from `docs/content/block-matrix.md`; click a field to insert a `line:` for it at the cursor. A Components tab, generated from `docs/content/gauges.md`, lists every supported gauge kind with its example DSL and which blocks support it; click to insert. A Templates tab lists every `@template` the real parser knows — the 8 built into `HelmCore.cs` plus `layouts/templates.txt`'s 4, embedded into the WASM build (`tools/helm-toolbox-wasm/Host.cs`) the same way `helmv.ps1` and `tools/spritedump` load it — as a form of that template's own parameters; fill it in and insert the `@use` line
- Item and component insertions detect the aliases used by their example and add missing `@bind` declarations to the global bind section. Existing declarations are never replaced or duplicated; generated block/group names are starting points for the user's terminal names.
- Use the Base, Panel and Component inspector tabs to create screens and panels or edit output targets, grids, positions, spans, titles, borders, colours and common gauge options without hand-writing the directive. Changes are focused source edits: comments, templates and unknown/custom syntax remain intact, and syntax that cannot be edited safely stays available as read-only text.
- Copy, import or export the Custom Data as plain text. The current DSL is auto-saved to the browser's `localStorage` between visits.
- Save multiple named projects locally, including their selected LCD profile, or copy a share URL that carries the DSL in its fragment.
- Use the keyboard shortcut sheet (`?`) and pause simulated time manually or through the operating system's reduced-motion preference. Animation renders are serialized so slower WASM runs cannot starve the visible preview, and resuming continues from the paused simulated time.

## What it does not do yet

Dragging blocks and gauges directly on the preview canvas, editing mirror relationships (creating ordinary multi-screen layouts is supported), scenario buttons beyond Alert, undo/redo or a command palette, and the CI parity check against `tools/spritedump`. These are tracked under Epic 39 in `DEVTRACKER-PENDING.md` (git-ignored).

## Layout

```
helm-toolbox/
  index.html       page shell (reuses the docs site's compiled Tailwind/daisyUI CSS)
  js/
    app.mjs         state, DOM wiring, visual inspectors, the block palette and value sliders
    dsl-model.mjs   loss-minimizing source model and focused inspector mutations
    wasm-bridge.mjs  loads tools/helm-toolbox-wasm's published build and calls Host.Render
  scripts/
    gen-blocks.js      parses docs/content/block-matrix.md -> data/blocks.json (TASK-39.12)
    gen-starters.js    copies every layouts/default/*.txt preset and the docs gallery metadata
                       -> data/starters.json
    gen-components.js  parses docs/content/gauges.md -> data/components.json (TASK-39.24), cross-referenced
                        against data/blocks.json for each gauge kind's compatible blocks; run after gen-blocks.js
    stage.js           assembles public/helm-toolbox/ for deployment or self-hosting
  data/              generated (gitignored)
  vendor/            pinned Lume/Logcad browser files staged by `bun run vendor` (gitignored)
  wasm/              tools/helm-toolbox-wasm, published here by `bun run wasm` (gitignored, ~9 MB uncompressed)
  server.mjs         local dev server; serves this directory, falling back to ../public for
                      the shared docs assets (assets/js/*.mjs, css/style.css)
```

`../docs/assets/js/sprite-svg.mjs` and `se-lcd-bezel.mjs` are shared with the docs preview widget (`help-visualizer.js`), not duplicated here, so the two cannot draw sprites differently.

## Running it locally

```bash
# repository root, bash. Needs the .NET 10 SDK (see tools/helm-toolbox-wasm/README.md) and Bun.
cd docs && bun run css:build && cd ..   # helm-toolbox reuses the docs site's compiled CSS
cd helm-toolbox
bun install
bun run build   # bun run data && bun run vendor && bun run wasm
bun run serve   # http://127.0.0.1:8891/
```

`bun run dev` does both in one step. A build also stages HELM Toolbox under `../public/helm-toolbox/`; the repository-root build adds the shared docs assets and produces the complete static tree. Rerun `bun run data` after editing `docs/content/block-matrix.md` or `layouts/default/*.txt`, and `bun run wasm` after editing `Program.cs` or `HelmCore.cs`.

Lume supplies mutation-aware contextual help for both static controls and generated field buttons. Logcad supplies the branded startup message and retained error diagnostics. Prism uses `docs/helm-prism.js` and `docs/css/theme-syntax-highlighting.css`, so HELM tokens and colours match the documentation. The vendor step stages all browser runtimes locally, so the deployed tool does not depend on a JavaScript CDN. Shared asset URLs are relative, and CI preserves the `wasm/wwwroot/` directory, so the artifact also works when GitLab Pages hosts it beneath a project path.

## Deployment recommendation

Keep HELM Toolbox in the HELM Core repository and publish it beside the docs. It directly consumes the parser, renderer, WASM host, layouts, docs-generated reference data and shared visual assets; moving it to another repository would require a versioning and synchronization layer for all of those inputs. The root `bun run build` output in `public/` is static and can also be hosted by users who want their own copy, so first-party hosting and self-hosting do not need separate source trees.

The block palette also uses the same cached Official Space Engineers Wiki icon manifest as the docs support matrix. Refresh those attributed, up-to-128 px PNGs with `cd ../docs && bun run icons:refresh`; normal docs builds validate and reuse the cache.

## Verified

The source-model test covers comments and unknown-directive preservation, named/hex/RGBA colours, panel position and title changes, component options, panel/screen creation, implicit panels and every preset in `layouts/default/`. The full interface is also exercised in a real headless Chromium; it has not yet been checked in a non-Chromium browser.
