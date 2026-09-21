# Efficiency Audit: 21 SEP 2026

This audit checks HELM Core for code duplication, per-tick cost, script size and usability problems that can be fixed without removing a feature, and states what should be done before production testing.

- **Scope:** `HelmCore.cs`, `Program.cs`, `Build-Release.ps1`, `.gitlab-ci.yml`, and the claims in `README.md`.
- **Method:** the code was read in full for the tick path. Allocation and time were measured with a scratch benchmark that renders presets 400 times on the stand-in API (`tools/spritedump` shim). Candidate optimizations were applied to scratch copies and re-measured. Nothing in the repository was changed.
- **Limits:** the stand-in does not read real blocks and does not count Space Engineers instructions. Allocation numbers are exact for this code; instruction counts are estimates and are marked as such. Every claim about in-game behaviour still needs the M5 run.

## Verdict

The design is sound: retained sprite buffers, event-driven redraws, one screen per run. The renderer is not yet as efficient as it can be without losing features, and there are two production risks that are not performance tuning. Recommended before production testing: items 1 to 5 in the order list below. They are small, they do not change behaviour, and they either remove a crash risk or give back 5,000 to 10,000 characters of headroom.

## Measured results

Allocation per redraw of one screen, values changing every tick, harness copying removed.

| Preset (lines) | Current | Colour cache | Bind lookup without lambda | Both |
| :--- | ---: | ---: | ---: | ---: |
| `power_only` (4) | 1,171 B | 627 B | 731 B | 187 B |
| `hangar_bay` (6) | 1,659 B | 979 B | 871 B | 191 B |
| `all_gauges_showcase` (16) | 4,218 B | 2,314 B | 2,397 B | 493 B |
| `planetary_base` (25) | 6,532 B | 3,132 B | 3,785 B | 385 B |

Time per redraw on `planetary_base` falls from 31.6 to 24.5 microseconds (22%). Lowercasing the field name at parse time changed nothing on .NET; the Space Engineers runtime may allocate there, which is unmeasured.

A first hypothesis was that colour parsing dominated. It accounts for about half; the other half is the closure and `ToLowerInvariant` in the bind lookup.

## Findings

### Production risks

**R1. Unmatched names run a per-block scan of about 90 string tests (estimate).**
`ResolveFallbackBlocks` (`Program.cs:388`) runs when a bind matches no block, which is the normal state when a player pastes a preset onto a grid with different names. For every block it calls `MatchesType`, a chain of about 35 `hint.Contains(...)` and `is` tests, and it runs the whole pass twice when the first finds nothing. That is on the order of 400 script instructions per block per unmatched bind. On a 500-block grid with several unmatched binds this is far over the 50,000 instruction limit, and it runs in the constructor and every 100th run. Wildcard binds fetch every block once per bind (`Program.cs:326`) and are similar. This needs the M5 run to confirm; it is the most likely cause of a first-run failure.
Fix without losing behaviour: compute the type from the hint once per bind, fetch the block list once per rescan and share it between binds, and test only the chosen type per block.

**R2. Configuration errors are visible for one run.**
`ReloadConfig` writes parse errors with `Echo` (`Program.cs:217`), and the next run replaces the detail text with the status line. At `Update100` a mistake is on screen for under two seconds. Fix: keep the last error text and echo it on every run (about 80 characters).

### Performance

**P1. Colours are parsed from text on every redraw.** `ResolveColor` calls `HelmColor.Parse` for every gauge on every redraw: trims, compares, substrings and `Convert.ToByte`. Cache by name (about 100 characters). Measured above.

**P2. Bind lookup allocates a closure per read.** `ResolveFieldValue` and `ResolvePercentForBind` use `Binds.Find(b => ...)` (`Program.cs:900`, `:922`). Use a dictionary built at reload. Measured above.

**P3. Values are read repeatedly.** A `bar(x.output)` reads every block in the bind twice (`ResolveFieldValue`, then `ResolvePercentForBind`), and two gauges on the same bind read it twice. A per-run cache keyed by bind and field removes the repeats. Not measured, because the stand-in has no blocks; on a 200-panel group it is a direct instruction saving.

**P4. Colour rules are re-parsed on every change.** `EvalSimpleComparison` (`HelmCore.cs:1403`) trims, allocates a five-element array, calls culture-sensitive `StartsWith` and `double.Parse` for every rule each time a `text()` value changes. Compile each rule to an operator and a number at parse time.

**P5. Culture-sensitive string calls.** 47 `StartsWith("...")` calls in `HelmCore.cs` and value formatting with `ToString(fmt)` use the current culture. Parse-time only, so a small cost; but a player with a decimal-comma locale sees `12,3` where the docs show `12.3`. Use ordinal comparison and `InvariantCulture`.

**P6. Refresh rate and damping interact badly.** The script is fixed at `Update100` and renders one screen per run (`Program.cs:114`, `:191`). Bars move 35% of the remaining distance per redraw (`slot.DisplayValue += ... * 0.35f`), so one screen takes about 7 redraws to settle: about 12 seconds alone, and 12 seconds times the screen count with several screens. The README describes `helmv.ps1` at 60, 6 and 0.6 Hz but the script offers no way to choose. Options: a `refresh:` setting, or skipping the damping at `Update100`. Needs a decision and the M5 run.

**P7. Minor allocations.** `RenderFlowText` allocates a `char[]` per call (ASCII border only); `ResolveConditionalColor` uses `FirstOrDefault` with a lambda; `Echo` builds an interpolated string every run.

### Script size

The minified script is 99,165 characters. Where they go (methods, minified): `BuildGraphicBuffer` 10,894; `RenderScreenGraphic` 9,634; `Parse` 6,625; `ParseLine` 5,859; `ResolveBlockField` 5,576; `RegisterBuiltinTemplates` 4,408; `ResolvePercent` 4,153; `ResolveFallbackBlocks` 3,831; `RenderScreenAscii` 3,407.

**S1. The minifier only removes comments, blank lines and indentation.** It keeps every newline (2,904) and every space around operators and braces. A token-aware collapse that leaves strings, chars and interpolated strings untouched was measured on the current file:

| Approach | Characters saved |
| :--- | ---: |
| Newlines and spaces around `{ } ( ) ; ,` | 4,933 |
| Also around operators (needs care with `+ +`, `- -`, generics, `? .`) | 9,773 |

No source change, no behaviour change. Verification: 204 unit tests, the combined compile check and the renderer smoke job. This is the largest single gain. Stripping the redundant `private ` keyword (55 uses) adds about 440.

**S2. Code in the shipped script that the game never calls.** `GradientLerp` 719, `LoadTemplateFile` 700 (only `tools/spritedump` calls it), `GetEffectiveBootLogs` 285, `RenderedLine` about 100. About 1,800 characters. The two used only by tests or the harness can move out of the shipped file.

**S3. Repeated sprite update code.** `var valSprite = buf.Sprites[...]; valSprite.Data = ...; valSprite.Color = ...; buf.Sprites[...] = valSprite;` appears 10 times and the fill-sprite variant 6 times in `RenderScreenGraphic`. A helper saves about 1,000 characters.

**S4. Two block tables.** `ResolvePercent` and `ResolveBlockField` repeat the same 61 type casts. One dispatch that returns the percent and the named field would remove about 2,000 to 3,000 characters. Medium risk: `Program.cs` has no unit tests (L3), so it depends on the renderer smoke job and the in-game run.

### Documented behaviour the script does not implement

- **Boot sequence.** `README.md` 4.11 documents `boot_msg`, `boot_duration`, `boot_stagger`, `boot_gradient` and `boot_log`, the parser reads them, `helmv.ps1` shows them, and the `boot_custom` preset uses them. `Program.cs` contains no boot code. In game nothing boots.
- **Smooth gradients.** Bars pick the nearest stop (`GradientColorAt`) and screen borders use only the first and last stop; `GradientLerp` is never called. README section on `border_gradient` (`README.md:142`) describes smooth transitions.
- **Field case.** The parser keeps `Percent` as typed, so `line.Field == "percent"` comparisons in the renderer miss it while `ResolveBlockField` lowercases. Lowercase once at parse (`HelmCore.cs:1016`).

### Process

**Q1. CI does not catch a stale committed script.** `combined-compile-check` compiles the file that `build-release` regenerates in the runner, not the committed one, so the H1 failure mode (a stale `HelmCombined.min.cs` in the repository) would still pass. Add `git diff --exit-code HelmCombined.cs HelmCombined.min.cs` after the build job.

**Q2. Three implementations of layout.** `Program.cs`, `HelmCore.cs` helpers and `helmv.ps1` (1,792 lines) each render. `spritedump` already runs the real renderer for the docs; `helmv.ps1` remains a separate copy that already differed (the toggle width bug, border styles). Accepted by design; noted as the main drift risk.

## Recommended order

1. R1: compute the fallback type once per bind and share one block list per rescan.
2. R2: keep and re-echo configuration errors.
3. P1 and P2: colour cache and bind dictionary (about 95% fewer allocations per redraw).
4. S1: token-aware minifier (5,000 to 9,800 characters of headroom).
5. Q1: freshness check in CI.
6. P3, P4, P5: per-run read cache, precompiled colour rules, ordinal and invariant string handling.
7. S2, S3: remove dead code and repeated sprite updates (about 2,800 characters).
8. Decide P6 (refresh rate) and the boot and gradient gaps: implement, or remove from the docs.
9. S4 once `Program.cs` has better test coverage.
10. M5: in-game run, which settles R1 and P6.

After items 1 to 5 the script has roughly 6,000 to 10,000 characters of headroom, which pays for text dynamic values (M8, 553) and the boot sequence with room to spare.

## Remediation status (same day)

| Finding | Status |
| :--- | :--- |
| R1 fallback scan | Fixed in `d4b77fd`: keywords matched once per bind, one shared block list per rescan, one pass. The 33 keyword-to-type rules were compared mechanically with the old code. The instruction cost is still an estimate until the M5 run |
| R2 config errors | Fixed: kept and echoed on every run, including output-block problems |
| P1 colour parsing | Fixed: cache by name |
| P2 bind lookup | Fixed: dictionary built at reload |
| P1 + P2 result | Allocation per redraw on `planetary_base` 6,532 to 385 bytes; `all_gauges_showcase` 4,218 to 493 bytes; time about 22% lower. Rendered output byte-identical for 51 layouts, 5 profiles, 48 ticks |
| S1 minifier | Fixed in `51768ab`: 98,740 to 87,822 characters (12,178 remaining, was 1,260), verified by `tools/minify-check` (23,875 tokens identical). The old comment stripper also left some comments in the file; that is fixed |
| Q1 stale committed build | Fixed in `42825a9`: `build-release` fails when the committed combined scripts differ from a fresh build; new `minify-verify` job. Not yet run on GitLab |
| P3 per-run read cache, P4 colour rules, P5 culture-sensitive calls, P7 | Open |
| S2 dead code, S3 repeated sprite updates, S4 merged block table | Open. S1 removed the urgency; do them when a feature needs the space |
| P6 refresh rate and damping | Open, needs a decision and the M5 run |
| Boot sequence, smooth gradients, field case | Open, needs a decision (implement or correct the docs) |
