# HELM documentation and HELM Toolbox

The documentation package builds the complete local web experience. Its public tree includes the Eleventy documentation site and the separately built HELM Toolbox application.

## Quick start

From the repository root:

```bash
bun run setup
bun run docs:serve
```

Then open:

- Documentation: `http://127.0.0.1:8890/docs/`
- HELM Toolbox: `http://127.0.0.1:8890/helm-toolbox/`

The serve command performs an initial HELM Toolbox WebAssembly/static build, builds the documentation CSS, and watches the documentation source with browser reload. If you change HELM Toolbox source while the server is running, rerun `bun run helm-toolbox:build`; the shared server notices the updated `public/` files.

## Commands

| Root command | Purpose |
| --- | --- |
| `bun run build` | Build HELM Toolbox and the docs into `public/`. |
| `bun run docs:build` | Same complete web build, named for documentation workflows. |
| `bun run docs:serve` | Build both, then serve/watch the combined tree on port 8890. |
| `bun run docs:dev` | Fast docs-only watch mode; assumes HELM Toolbox is already staged. |
| `bun run docs:css:watch` | Watch only the shared Tailwind/DaisyUI CSS. |
| `bun run docs:frames` | Regenerate rendered preset/example frames. |
| `bun run docs:icons:refresh` | Refresh the attributed Space Engineers block-icon cache. |
| `bun run helm-toolbox:test` | Run the round-trip DSL model and all-preset checks. |

Within this directory, `bun run site:build` builds only Eleventy/CSS. CI uses it after restoring the independently built HELM Toolbox artifact. `bun run build` is the normal developer command and includes both applications.

## Structure

| Path | Contents |
| --- | --- |
| `content/` | Authored documentation pages and HELM DSL examples. |
| `_data/` | Site metadata and preset-gallery data. |
| `_includes/` | Nunjucks page shells and reusable components. |
| `assets/js/` | Documentation behavior plus shared LCD/SVG renderers. |
| `css/` | Tailwind entrypoint and syntax-highlighting theme. |
| `scripts/` | Icon caching, frame checks and example extraction. |
| `eleventy.config.js` | Collections, syntax registration, passthrough assets and output configuration. |
| `serve.js` | Local server for the combined `public/` directory with reload events. |

See the root [contributor guide](../CONTRIBUTING.md) for the complete repository map, prerequisites, generated-file rules and validation commands. See [HELM Toolbox's README](../helm-toolbox/README.md) for its browser/WASM architecture.
