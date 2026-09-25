# System overview

HELM Core turns a line-oriented layout definition into Space Engineers text-surface sprites. The same parser and rendering model power the in-game script, offline tests, documentation captures and HELM Toolbox.

```text
Custom Data / layout file
          |
          v
  HelmCore.cs parser ----> layout model
          |                    |
          |                    +--> Program.cs telemetry + in-game surfaces
          |                    +--> spritedump documentation frames
          |                    +--> helm-toolbox-wasm browser renderer
          v
   validation issues
```

## Component ownership

- `HelmCore.cs` owns DSL parsing, validation, layout data and engine-independent rendering logic.
- `Program.cs` owns the Space Engineers adapter: terminal block discovery, telemetry sampling, commands, scheduling and `IMyTextSurface` output.
- `HelmCombined.cs` and `HelmCombined.min.cs` are release products assembled from the two source files. Do not edit them as primary source.
- `layouts/default/` is the canonical preset library. Generated Toolbox starter data is derived from it.
- `docs/` owns the Eleventy documentation application and shared browser presentation assets.
- `helm-toolbox/` owns the browser editor and inspector. Its WASM host references the real core sources.
- `tools/spritedump/` renders deterministic examples; `tools/helm-toolbox-wasm/` exposes the renderer to JavaScript.

## Change boundaries

A DSL change normally requires parser tests, renderer coverage, documentation, and regenerated browser data. A visual-only documentation change should not alter the in-game renderer. A runtime optimization must preserve parser results and minified-release parity.
