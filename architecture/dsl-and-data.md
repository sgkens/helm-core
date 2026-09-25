# DSL and generated data

The DSL is intentionally line-oriented so it remains practical in Space Engineers Custom Data. `HelmCore.cs` performs template expansion, directive parsing, typed value conversion, validation and model construction. Errors should identify the source line and preserve valid neighboring screens where safe.

## Sources of truth

| Data | Edit here | Derived consumers |
| --- | --- | --- |
| Parser, built-in templates and render rules | `HelmCore.cs` | game, tests, WASM and offline tools |
| Shipped layouts | `layouts/default/` | Toolbox starters and documentation examples |
| Block telemetry reference | `docs/content/block-matrix.md` | Toolbox block palette |
| Gauge/component reference | `docs/content/gauges.md` | Toolbox component palette |
| Syntax grammar | `docs/helm-prism.js` | docs and Toolbox editor |
| Shared SVG surface rendering | `docs/assets/js/sprite-svg.mjs` | docs and Toolbox |

Generated folders (`public/`, `helm-toolbox/data/`, `helm-toolbox/vendor/` and `helm-toolbox/wasm/`) must not become editing sources. Change their input and rerun the relevant build.

## Compatibility

New directives should be optional and defaults must preserve existing layouts. Treat directive names, field names, template names and command arguments as stable technical tokens; localization must not translate them. Add malformed-input and boundary cases alongside the happy path.
