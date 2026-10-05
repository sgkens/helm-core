# #5 — Toolbox animation and intermittent alerts

Source: [GitLab #5](https://gitlab.com/sgkens/helm-core/-/issues/5).
Updated: 2026-10-06.
Status: fixes implemented and verified locally; user reproduction and published build verification pending.

## Report and environment

The original issue reports broken Play animation in the final WASM build. On 6 October the user confirmed animation still does not work and alerts work only sometimes.

The supplied [raw console capture](animation-issue.md) identifies `http://127.0.0.1:8890/helm-toolbox/`, the documentation server's staged Toolbox. Browser version, served build, exact DSL, selected LCD and reproduction sequence were not captured. The stack uses Firefox-style `resource://devtools/` paths; record the actual browser/version rather than assuming it from that clue.

Expected: Play advances animated sprites, Pause holds the frame, resuming continues animation, and Alert reliably enables/disables the configured flashing background while time advances.

## Console evidence — 2026-10-06

The capture records:

```text
Security Error: Content at http://127.0.0.1:8890/helm-toolbox/ may not load or link to file:///.
Source map error: Error: request failed with status 404
Resource URL: http://127.0.0.1:8890/helm-toolbox/vendor/logcad.mjs
Source Map URL: logcad.esm.js.map
```

The raw file also contains the startup message and source-map worker stack. No WASM exception or failed WASM resource is identified in this excerpt. These messages do not yet establish why playback or alerts fail.

Code inspection confirms the vendored Logcad file references `logcad.esm.js.map`, while [`copy-vendor.js`](../helm-toolbox/scripts/copy-vendor.js) previously copied the JavaScript and license without that map. This explains the missing staged map. Whether the blocked `file:///` reference is related to source maps, WASM startup, or another resource remains unknown.

## Earlier checks — 2026-10-05

Headless Chromium, with reduced motion disabled, passed Tank Trio animation, Pause and Play on the live GitLab Pages URL and both local serving paths (8890 staged site and 8891 source server). The checks observed changing/held SVG frames and no page errors. The live application JavaScript matched the checked local source.

Those checks covered one preset and Chromium. They did not establish Firefox compatibility, all alert transitions, all presets, or the user's reproduction. They are not grounds to close this issue.

The original Node check covered tank sprites, fixed-time frames and corner text sizing. It missed the machinery spinner and exact alert phases; those now have regression coverage. Browser controls are covered separately below.

## Relevant code and serving paths

- [`app.mjs`](../helm-toolbox/js/app.mjs): `doRender`, render tokens, serialized animation timer, Play/Pause, Alert and reduced-motion controls.
- [`wasm-bridge.mjs`](../helm-toolbox/js/wasm-bridge.mjs): runtime loading, seconds and `__alert` payload.
- [`Host.cs`](../tools/helm-toolbox-wasm/Host.cs): constructs a Program per render, injects alert state and replays 40 passes.
- [`Program.cs`](../Program.cs): alert phase, configured alert background and retained sprites.
- `helm-toolbox/` is the editor source; `tools/helm-toolbox-wasm/` is its renderer; `public/helm-toolbox/` is generated deployment output. The docs guide at `/docs/helm-toolbox/` is not another application.
- Pages publishes from `main`. The standalone server serves source at 8891; the docs server serves generated `public/` at 8890. Compare the exact served files/build, not only the source branch.

## Audit and fixes — 2026-10-06

The user's suggested component-metadata path was checked alongside the clock and renderer. Editing a cargo or machinery property did not reproduce a browser exception. There is no evidence that the metadata extraction change caused all reported animation failures. The audit found these concrete defects:

1. **Machinery angle reset each frame.** The WASM host constructs a new Program for every render and replays 40 passes. The spinner incremented its angle per pass, so every requested timestamp ended at the same angle. Before the fix, timestamps 0, 0.2, 0.9, 1.7 and 2 seconds all produced 2.142 radians. The shared core now derives its angle from elapsed time, at approximately the existing Update10 speed. Idle machinery stays still.
2. **Preview time was shifted forward.** The host replay ended at `seconds + 3.9`, moving alert flashes away from the requested phase. Replay now starts at earlier times and ends at exactly the requested time. Tests check both default and custom alert periods, normal backgrounds and disabling Alert.
3. **Bind controls cached only alias names.** Keeping an alias but changing its target, kind or text/numeric metadata left stale controls. The cache now includes that metadata. Loading a project also invalidates controls when slider values are reset.
4. **Post-WASM exceptions could stop playback.** The catch covered only the WASM call. Errors in metadata controls or SVG drawing could reject the animation tick before its next timer was scheduled. The catch now covers the complete preview update, reports the error and permits the next tick.
5. **Missing vendor source maps.** The vendor step now copies the referenced Lume and Logcad maps. Both source and staged URLs return HTTP 200.

Pause deliberately freezes time, including whichever alert flash phase is current. Enabling Alert while paused can therefore leave the normal background visible. A visible reminder now explains this and asks the user to press Play. Play advances waves, rotation and alerts; it does not automatically vary telemetry sliders. A fixed cargo bar is expected when its value stays constant.

The `file:///` console security error has not been reproduced in this audit. No matching URL was found in the staged runtime files; source-map 404s were identified separately. Do not label the security error resolved. The user's 8890 docs-server process was stopped during this session, so the staged build was checked on an independent 8892 server. The user-owned process was left untouched.

## Verification — 2026-10-06

- [`test-renderer.mjs`](../tools/helm-toolbox-wasm/test-renderer.mjs): fresh trimmed WASM; tanks, changing and fixed machinery angles, idle machinery, exact alert phases, custom alert period, all four power themes on both corner profiles and dense corner text sizing pass.
- [`test-preview-browser.mjs`](../helm-toolbox/scripts/test-preview-browser.mjs): Playwright 1.63.0 Chromium and Firefox; both source `http://127.0.0.1:8891/` and staged `http://127.0.0.1:8892/helm-toolbox/` pass.
- Browser checks edit a component field while playing, change a bind target without changing its alias, switch the alias from numeric to text, pause/resume, observe both alert phases over repeated on/off cycles, disable Alert and recover from an injected SVG drawing exception. No unhandled page errors. Both vendor maps return HTTP 200. Reduced-motion preferences start playback paused; explicit Play resumes it in both browsers.
- Additional audit: Tank Trio waves advance before and after slider edits in both browsers. Cargo Contents and Survival Essentials are mostly static with fixed slider values; that alone is not a playback failure.

These checks establish the named fixes and tested environments. The user's browser/version, exact failing DSL and action sequence are still unknown. Check the affected setup with a rebuilt Toolbox before recording acceptance. In-game spinner timing also needs confirmation with real update scheduling.

## Remaining verification

1. Rebuild and retest the user's affected browser, preset and serving path; record the version, DSL, selected LCD, motion preference and Play/Alert actions.
2. If the `file:///` security message remains, capture its network initiator or full devtools context separately from source-map requests.
3. Compare the published build with the tested source, then repeat Play/Pause/Alert transitions. No deployment or GitLab issue closure was performed in this session.

## Follow-up log

- 2026-10-06 — User: animation still fails; alerts work intermittently. Preserved the console capture and kept acceptance pending.
- 2026-10-06 — Audit: fixed the frozen spinner, replay clock offset, stale metadata controls, render-error recovery and map staging. Added reproducible WASM/browser checks; Firefox and Chromium pass both serving paths. User reproduction remains to be checked.
