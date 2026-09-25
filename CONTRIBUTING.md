# Contributing to HELM Core

HELM Core contains the in-game script, its offline parser and test harnesses, the documentation site, and HELM Toolbox. The repository root is the supported entry point for installing and building the web tooling.

## Prerequisites

- [Bun](https://bun.sh/) for the documentation and browser tooling.
- .NET SDK 10 for the HELM Toolbox WebAssembly host and renderer utilities.
- PowerShell 7 and Mono (`mcs`) for the complete release and CI-equivalent validation path.
- Space Engineers is only required for the optional game-assembly compilation check and in-game verification.

## First setup

```bash
git clone https://gitlab.com/sgkens/helm-core.git
cd helm-core
bun run setup
```

Dependencies are installed independently in `docs/` and `helm-toolbox/`. Generated dependencies, WebAssembly output, reference JSON and `public/` are ignored by Git.

## Build and serve the complete site

```bash
bun run build
bun run docs:serve
```

`bun run build` produces the complete static site in `public/`: documentation under `/docs/` and HELM Toolbox under `/helm-toolbox/`. `bun run docs:serve` first rebuilds both applications, then serves the same combined tree with documentation watch/reload at `http://127.0.0.1:8890/docs/`. Open `http://127.0.0.1:8890/helm-toolbox/` for the browser tool.

For focused work, use `bun run helm-toolbox:serve` (port 8891), `bun run helm-toolbox:test`, `bun run docs:dev`, or any of the namespaced scripts in the root `package.json`.

## Repository map

| Path | Responsibility |
| --- | --- |
| `HelmCore.cs` | Space Engineers-independent DSL parser, layout model and rendering core. |
| `Program.cs` | Programmable Block adapter, telemetry collection, scheduling and runtime commands. |
| `Tests.cs` | Core offline unit suite compiled by `mcs` in CI. |
| `layouts/` | Shipped presets and reusable template definitions. |
| `docs/` | Eleventy, Tailwind and DaisyUI documentation source. |
| `helm-toolbox/` | Browser editor, visual inspector, generated palettes and static staging. |
| `tools/helm-toolbox-wasm/` | Real HELM parser/renderer compiled to browser WebAssembly. |
| `tools/spritedump/` | Offline rendering and documentation-frame generation. |
| `tools/pb-check/`, `tools/scope-test/`, `tools/rename-check/` | Game-script compilation, runtime scope and minification-parity gates. |
| `public/` | Generated combined documentation/toolbox output; never edit it directly. |

## Sources of truth

- Edit layouts in `layouts/default/`; `helm-toolbox/scripts/gen-starters.js` regenerates the browser list.
- Edit block and gauge documentation in `docs/content/block-matrix.md` and `docs/content/gauges.md`; the HELM Toolbox data scripts derive their palettes from those pages.
- Edit the Prism grammar once in `docs/helm-prism.js`; both documentation and HELM Toolbox consume it.
- Edit shared SVG rendering in `docs/assets/js/sprite-svg.mjs`; both browser previews consume it.
- Never hand-edit `public/`, `helm-toolbox/data/`, `helm-toolbox/vendor/` or `helm-toolbox/wasm/`.

## Validation before a merge request

```bash
bun run helm-toolbox:test
bun run build
mcs -out:/tmp/test_helm.exe HelmCore.cs Tests.cs
mono /tmp/test_helm.exe
```

For release work, run `pwsh ./Build-Release.ps1`; the GitLab pipeline adds character-budget, combined-compilation, scope, rename-parity, packaging and Pages gates. Keep changes focused, preserve existing generated-file boundaries, and use Conventional Commit subjects.

## Licensing

By contributing, you agree that your contribution is provided under the repository's [MIT License](LICENSE).
