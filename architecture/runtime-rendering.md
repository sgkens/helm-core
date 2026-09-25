# Runtime and rendering

`Program.cs` is the Space Engineers entry point. Startup parses Custom Data, discovers target surfaces and bound blocks, allocates screen buffers, and schedules updates. Normal ticks sample telemetry, update only changed model values and submit retained sprites.

## Lifecycle

1. Parse Custom Data through `HelmCore.cs` and report actionable line-level issues.
2. Resolve `@screen`, `@surface`, `@bind`, template and panel declarations.
3. Discover terminal blocks and text surfaces once, refreshing only when configuration changes.
4. Sample telemetry on the configured cadence.
5. Mutate preallocated sprite slots, then draw each active surface frame.

The renderer is designed around predictable Programmable Block limits: avoid per-tick collection creation, string churn and repeated terminal-system scans. Cache work during configuration where possible.

## Multiple renderers

The game, `spritedump` and HELM Toolbox should agree on layout geometry. Shared behavior belongs in `HelmCore.cs`; host-specific surface or timing behavior belongs in its adapter. When browser output differs from the game, first determine whether the discrepancy is a host input (surface size, padding or font metrics) or a duplicated rendering rule.

## Verification

Exercise parser/unit tests for logic, `spritedump` for deterministic frames, HELM Toolbox for interaction, and an in-game programmable block for API and font behavior. The browser preview is a design aid, not a substitute for the final in-game check.
