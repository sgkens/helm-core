# webutil-wasm

A WebAssembly build of the real HELM script and parser, used to prove that the web utility can render a layout in the browser on every edit (TASK-39.0).

## What it builds

`Program.cs` and `HelmCore.cs` are compiled unchanged, together with the stand-in game API in `tools/spritedump/SeShim*.cs`, into one WebAssembly module. `Host.cs` exports two functions to JavaScript:

```text
Render(dsl, width, height, values, seconds, screenIndex) -> JSON
ListTemplates() -> JSON
```

`values` is `"bind=50;other=12"` on the script's 0 to 100 scale, plus an optional `__alert=1` pseudo-entry (not a `@bind`) that previews the `alert red` run-argument command by setting `Program`'s own `_globalRedAlert` field before rendering. `seconds` drives the script's clock (waves, easing), and the result holds `errors`, `warnings`, the screen names and the sprite list (same fields as the docs frames: `t`, `d`, `x`, `y`, `w`, `h`, `c`, `f`, `a`, `s`, `r`).

If the requested screen is a `mirror:`, its source screen is rendered first on every pass so the source's `_screenBuffers` entry exists by the time `RenderScreen` reads it — otherwise (only the requested screen ever rendering, unlike the game's own round-robin across ticks) a mirror always came back with zero sprites, silently, since `RenderMirror` fails closed rather than erroring.

`ListTemplates()` reflects into `HelmParser`'s template table (built-ins plus `layouts/templates.txt`, embedded as a resource in `WebUtilWasm.csproj` — a browser has no filesystem, so it can't be read at run time the way `helmv.ps1` and `tools/spritedump/Dump.cs` read it) and returns `[{"name":..,"params":[{"name":..,"default":..}]}]`; a `default` of `null` means the `@template` header gave that parameter no default, so `@use` requires it.

## Build and run

```bash
# repository root, bash. Needs the .NET 10 SDK (the WebAssembly SDK is part of it; no workload is required).
dotnet publish tools/webutil-wasm -c Release -o /tmp/webutil
node tools/webutil-wasm/run-node.mjs /tmp/webutil layouts/default/tank_trio.txt
```

## Spike results (22 SEP 2026)

| Measure | Result |
| :--- | :--- |
| Renders | Every preset tried (tank_trio, combo_dashboard, bar_sizes, background_lab, all_gauges_showcase, command_carrier, survival_essentials) renders without errors |
| Start (Node) | 200 to 340 ms |
| One render (40 passes) | median 8 to 31 ms, worst 85 ms |
| Download, untrimmed | 24 MB raw, about 7 MB brotli |
| Download, trimmed (`PublishTrimmed`, `TrimMode=full`, host assembly rooted) | 5.6 MB raw, 2.1 MB gzip, 1.7 MB brotli |
| Same output trimmed and untrimmed | Yes, for the four presets compared |

Further savings: the stand-in API uses `dynamic`, which pulls in `System.Linq.Expressions` and `Microsoft.CSharp` (about 660 KB raw). Typing the shim would remove them.

CI image: `sgkens/phellams-automator:2.22.0` (.NET SDK 8.0.424 and 10.0.400, Bun, Node 24) publishes this project and runs `run-node.mjs` on it with the same result, so a web utility job needs no extra image.

Not yet checked: a run in a real browser (only Node so far), output parity against `tools/spritedump` for every preset, and multi-screen `mirror:` (the host renders one screen at a time).
