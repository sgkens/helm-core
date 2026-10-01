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

## Search and AI discovery

The landing page and every documentation page include a unique title and description, an absolute canonical URL, indexing/preview directives, Open Graph and Twitter summary metadata, and JSON-LD describing the site, script and page. Documentation heads also link to their existing Markdown copies under `docs/src/`. The build generates `sitemap.xml`, `robots.txt` and `llms.txt` from the same site data and documentation collection. `llms.txt` is optional context for AI systems; it does not guarantee indexing or ranking. The sitemap contains the landing page and documentation HTML pages, while Markdown alternatives are listed in `llms.txt`.

`_data/site.js` owns the production base URL, currently `https://sgkens.gitlab.io/helm-core/`. To move the site, set `HELM_SITE_URL` to the full public base URL including its project path, and set `PAGES_PATH_PREFIX` to that URL's path for navigation/assets. Local development uses `/` for navigation while canonical metadata still points to production. The crawler policy permits general search, `OAI-SearchBot` and `GPTBot`; edit `robots.njk` to change it. The docs site does not need an installable application manifest or a social preview image to expose this metadata.

`robots.txt` rules only apply when served at the **hostname root**. On the shared project-path host, the generated `https://sgkens.gitlab.io/helm-core/robots.txt` must also be merged into the root site's `https://sgkens.gitlab.io/robots.txt` by its owner; publishing the project file alone does not configure host-level crawler access. Submit the project sitemap directly to verified search webmaster tools if the root site is managed separately. A root-level `llms.txt` can likewise link to the project's AI context index.

`bun run site:build` runs `discovery:check` after generation. It verifies all 14 current pages, unique titles/descriptions, matching canonical/Open Graph/JSON-LD/sitemap URLs, and existing Markdown links. Run it independently with `bun run discovery:check`.

After deployment, verify anonymous access:

```bash
curl -I https://sgkens.gitlab.io/helm-core/
curl -fsS https://sgkens.gitlab.io/robots.txt
curl -fsS https://sgkens.gitlab.io/helm-core/sitemap.xml
curl -fsS https://sgkens.gitlab.io/helm-core/llms.txt
```

Pages must return `200 OK` with the metadata in the initial HTML, without authentication or a browser challenge. A check on 1 October 2026 found the production home and hostname-level robots URL redirecting to GitLab Pages authentication. Public search discovery requires the project owner to enable anonymous Pages access. Validate JSON-LD with the [Schema.org validator](https://validator.schema.org/) and follow Google's [canonical URL](https://developers.google.com/search/docs/crawling-indexing/consolidate-duplicate-urls) and [sitemap](https://developers.google.com/search/docs/crawling-indexing/sitemaps/build-sitemap) guidance. The optional AI index follows the [llms.txt proposal](https://llmstxt.org/).
