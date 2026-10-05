# #6 — Corner LCD text sizing

Source: [GitLab #6](https://gitlab.com/sgkens/helm-core/-/issues/6).
Updated: 2026-10-06.
Status: implemented locally for 0.40.1; in-game acceptance pending.

## Report

Text overlaps and becomes unreadable on 512×128 and 256×128 displays. The user confirmed Toolbox output closely reflects the in-game output and requested smaller text in the shared core.

## Audit and change

Toolbox compiles the real `Program.cs` and `HelmCore.cs` into WASM. Its SVG renderer paints the exported text scale; there is no independent Toolbox font-size rule. Browser font metrics remain approximations of game fonts.

[`Layout.GetProfileFontSize`](../HelmCore.cs) previously applied `0.55 × screen font_size` to both corner resolutions. Dense layouts placed roughly 12-pixel text into rows about 10 pixels high.

| Corner surface | Previous multiplier | New multiplier |
| --- | --- | --- |
| 512×128 | 0.55 | 0.40 |
| 256×128 | 0.55 | 0.30 |

The corner multiplier is 0.30 when the longest surface side is at most 256 pixels, otherwise 0.40. Rotated surfaces receive the same result. `profile: auto` resolves to corner on these surfaces; explicit corner aliases also apply it. Explicit standard/wide/compact profiles retain their own multiplier. Panel `font_size:` overrides the screen's scaled size.

Changes: [`HelmCore.cs`](../HelmCore.cs), [`Tests.cs`](../Tests.cs), [hardware profile documentation](../docs/content/hardware-profiles.md), and [`test-renderer.mjs`](../tools/helm-toolbox-wasm/test-renderer.mjs). Version and [release notes](../release-notes/0.40.1.md) were prepared locally for 0.40.1.

## Verification — 2026-10-05

Chromium SVG text bounding-box checks, with browser fonts loaded, found these intersecting text pairs:

| Preset | 512×128 before → after | 256×128 before → after |
| --- | --- | --- |
| `survival_essentials` | 24 → 0 | 24 → 0 |
| `command_carrier` | 36 → 0 | 96 → 0 |
| `fighter_ammo` | 0 → 0 | 0 → 0 |

These counts cover the three named presets, not every possible layout. They measure browser text intersections, not in-game readability or text/texture intersections.

- Core suite: 325 passed, zero failed; includes aliases, auto profiles, rotated surfaces and font-size overrides.
- Fresh published WASM: animation and dense corner text checks passed.
- Rendering parity: 65 presets, 27,970 sprite/echo lines identical between source and renamed release script.
- Minification: 38,288 tokens identical; final script 97,910 characters, 2,090 below the 100,000 limit.
- Offline smoke export: 66 layouts. Documentation frames: 68 layouts. Docs generation and discovery passed.
- Real game API compilation was skipped because `SE_BIN64` was unavailable. In-game validation remains pending.
- Full content validation still fails on existing unbalanced `WORKSHOP.md` BBCode.

## Remaining work

Check both physical LCD types in game using the generated 0.40.1 script, the named presets and the user's Custom Data. Record readability, remaining overlap and any explicit profile/font overrides. Dense vertical gauges need separate layout guidance: see the [backlog review](backlog-2026-10-06.md).

## Follow-up log

- 2026-10-06: recorded the font audit, implementation, measured results and limits. No in-game acceptance or release publication recorded.

- 2026-10-06: full content validation now passes after correcting the Workshop simulator link. Documentation site generation and discovery pass.
- 2026-10-06: final patch rebuild passes 325 core checks and the runtime suite. Rename parity: 66 presets, 28,694 sprite/echo lines; minification: 38,325 tokens identical. Script: 98,023 characters, 1,977 remaining. Documentation frames: 70 layouts. Earlier measurements above are retained as historical evidence.
- 2026-10-06: Firefox and Chromium checks of the new panel-span example and four power-theme screens found zero text intersections on both corner profiles (20 screen/profile/browser combinations). Physical LCD confirmation remains pending.
