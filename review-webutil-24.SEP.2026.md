# WebUtil completion review — 24 September 2026

## Recommendation

Keep WebUtil in the HELM Core repository and deploy it with the documentation. Also document the root `public/` build as the supported self-hosting artifact.

This is the lowest-risk ownership model because WebUtil directly depends on:

- `HelmCore.cs` and `Program.cs` through the WebAssembly host;
- every layout in `layouts/default/`;
- docs-derived block, component and preset metadata;
- the shared LCD bezel and sprite renderers;
- the docs DaisyUI CSS, Prism grammar and syntax-theme stylesheet.

A separate source repository would need coordinated releases or pinned cross-repository downloads for all six inputs. A local-only tool would remove the easiest trial and onboarding route. Hosting the same static build beside the docs, while allowing users to serve `public/` themselves, supports both audiences without duplicating source or release state.

## Inventory finding

The UI contains three different reusable collections:

| Collection | Current count | Purpose |
| --- | ---: | --- |
| Layout presets | 65 | Complete Custom Data examples from `layouts/default/*.txt` |
| Parser templates | 12 | Reusable `@template` macros: 8 built in and 4 from `layouts/templates.txt` |
| Gauge components | 14 | Insertable gauge/example snippets generated from `docs/content/gauges.md` |

The old layout generator intentionally exposed only 7 presets. This was the source of the apparent missing-template problem; the Templates sidebar itself was showing parser macros, not full layouts.

## Changes completed

- The layout picker is generated from all 65 files and grouped using the docs gallery's category metadata.
- Selecting a preset also selects its recommended LCD hardware profile.
- The live Custom Data editor has Prism highlighting while retaining a native textarea for editing, selection, keyboard input and accessibility.
- Item and gauge example snippets use the same HELM highlighting.
- WebUtil consumes the same `docs/helm-prism.js` grammar and `docs/css/theme-syntax-highlighting.css` theme layer as the documentation.
- Prism is pinned and staged locally with the other browser dependencies; no runtime JavaScript CDN is required.
- The preview/editor split is pointer- and keyboard-resizable, has an expand/restore shortcut, persists locally, and keeps minimum usable space for both panes while the SVG preview scales.
- The play/pause loop serializes WASM renders and preserves elapsed simulated time across pauses, preventing slow renders from continually invalidating the frame that should reach the preview.
- Item and component palette insertion now adds missing example `@bind` declarations at the global bind section, preserves existing case-insensitive aliases, and falls back to a live dynamic preview binding for future unknown examples.
- The right-side Base, Panel and Component inspectors now turn the most common layout work into focused visual controls: output/profile/grid settings, panel placement and spans, titles, colours, borders and common gauge options.
- A New action creates a valid bind, screen, panel and line; adding a screen or panel likewise produces valid DSL and selects it for immediate editing.
- Palette insertions target the selected panel and open the inserted component for customization.
- The inspector's source model is loss-minimizing rather than a full-file serializer. It changes the selected directive or `line:` only, preserving comments, templates, conditional rules and unknown syntax verbatim; complex lines it cannot safely understand remain available as read-only DSL.

## Verification update — 25 September 2026

- `bun run test` validates localized mutations, comments and unknown-directive preservation, hex/named/RGBA colours, title and panel positions, component options, implicit panels, screen/panel creation and parsing all 65 layout presets.
- A real headless Chromium workflow created a configuration, edited Base/Panel/Component fields, added a panel and screen, inserted a palette component with its binding, rendered the result and checked the 1440×1000 layout. It completed with no console or page errors.
- The root build remains the deployable and self-hostable artifact; CI runs the DSL-model test before publishing WebUtil.

## Follow-up boundary

The 65 presets are now available as source examples. This change does not add screenshots for presets that lack them, turn full layouts into `@template` macros, or attempt to visually rewrite arbitrary custom expressions. Those remain separate content/parser-design concerns; unsupported source is deliberately preserved rather than normalized.
