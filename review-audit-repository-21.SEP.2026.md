# Repository Audit: 21 SEP 2026

This audit checks HELM Core for build health, documentation accuracy, release packaging and repository hygiene, with the evidence for each finding so it can be re-checked.

- **Branch:** `develop`, 99 tracked files, `.git` 1.7 MB, no remote configured, nothing pushed.
- **Scope:** source and build, tests, CI, release scripts, README, `WORKSHOP.md`, the docs site, and tracker consistency.
- **Method:** commands were run against the working tree. Nothing was changed while auditing.

## Summary

| Area | Result |
| :--- | :--- |
| Unit tests (`HelmCore.cs` + `Tests.cs`) | 200 passed, 0 failed |
| Fresh combined build compiles against the API stand-in | Yes, 0 errors |
| Committed `HelmCombined*.cs` compiles | No, 4 errors (stale) |
| Every PowerShell script parses | No: `Publish-Release.ps1` had 8 parse errors (H5) |
| In-game verification | None: `Program.cs` has never run in Space Engineers |
| Line endings | Clean (all LF) |
| Secrets in the tree | None found |
| Docs site build and browser checks | Pass |

## Findings

### High

**H1. Committed combined scripts are stale and do not compile.**
`HelmCombined.cs` and `HelmCombined.min.cs` predate the `Program.cs` fixes (`AggregationKind` should be `Agg`; `Vector2?` needs `.Value`). Compiled against the stand-in API the committed minified file gives 4 errors (`CS0103` twice, `CS1061` twice). A fresh `Build-Release.ps1 -NoTest` output compiles with 0 errors. Anyone pasting the committed file into the game gets compile errors.

**H2. Script size headroom is 1,129 characters.**
A fresh minified build is 98,871 characters against the 100,000 ceiling (98.9%). The CI soft limit is 96,000, which is already exceeded. Any feature that adds more than about 1 KB blocks a release. Nothing in the tracker plans for this.

**H3. Release packaging silently loses files after the reorganisation.**
`demo.ps1`, `logo-helm-core.svg`, `PDR.md` and two audit files are deleted in the working tree (moved to `decom/` and `context-source/`). Still referenced:
- `Publish-Release.ps1:247-248` (archive includes) and `:372` (Workshop `preview.svg`).
- `.gitlab-ci.yml:93` (artifact) and `:194` (`zip` input).
- `Test-HelmLayout.ps1:134,160-172` (fallback to `demo.ps1`).
- `tools/fonts/README.md:4`, `README.md:690` (link to `PDR.md`).

Both the PowerShell archiver and `zip` skip missing inputs with a warning and exit 0 (tested), so releases would succeed without the logo or Workshop preview image. The Workshop preview is the one that matters: the VDF still points at `preview.svg`.

**H4. `WORKSHOP.md` contains several factual errors.** See the dedicated section below.

**H5. `Publish-Release.ps1` did not parse.**
Line 156 used `$env:$EnvName`, which is invalid PowerShell (8 parse errors on the committed version), so the release script could not run at all. Found while testing H3.

### Medium

**M1. Block support is overstated in the docs.**
`docs/content/block-matrix.md` marks these as live with specific properties, but `Program.cs` has no code for them: `IMyShipController` (`.speed`, `.mass`), `IMyRemoteControl` (`.autopilot`), `IMyMedicalRoom`, `IMyConveyorSorter`, `IMyLaserAntenna`, `IMyUserControllableGun` (`.charged`), `IMyHydrogenEngine` (`.fuel`). At best they fall back to the enabled state (`IMyFunctionalBlock`). `IMyMotorAdvancedStator` works through its base type. About 31 block interfaces are implemented, against the "over 50" claimed in `WORKSHOP.md` and the docs.

**M2. Border styles are documented as universal but only `ascii` and `none` differ in game.**
`Program.cs` reads only screen-level `Border` and only distinguishes `ascii` and `none` (lines 959, 1011). `heavy`, `double`, `single`, `rounded`, `minimal` and `graphic` all draw the same frame, and panel-level `border:` is ignored. Only `helmv.ps1` implements the full set. `docs/content/custom-borders.md` and `README.md:109` present them as one feature. The `border_styles_gallery` preset shows eight identical panels in the visualizer for this reason.

**M3. `README.md` has nine absolute `file:///mnt/g/...` links** (lines 31, 40, 41, 109, 489, 603, 658, 661, 690). They resolve only on this machine.

**M4. The version has no single source.**
`VERSION` is empty and only the docs read it. `Publish-Release.ps1` takes `-Version` or `HELM_VERSION`; CI takes the tag, `HELM_VERSION` or the commit SHA. `docs/package.json` says 1.0.0.

**M5. Unverified performance claims.**
`WORKSHOP.md` ("Performance Guarantee": 25 to 65 instructions per tick, 0 bytes per tick) and `README.md:653-654` ("0.000 KB, true 0-byte allocation") state measurements. The `Program.cs` header says it was never compiled or run in game. The numbers come from the offline simulator.

**M6. `Program.cs` header comment is stale.**
It says the file is not compiled and cites "41 passing local tests". It now compiles against a stand-in and there are 200 tests. It still needs the in-game run.

**M7. Nested empty repository `docs/.git`.**
`docs/.git` (branch `master`, no commits) shadows the outer repository for tools run inside `docs/` (`git log` there reports "no commits yet"). It did not block commits from the repository root. Its origin is unknown.

**M8. Dynamic values are numeric only; the docs showed strings.**
`docs/content/inter-script-api.md` used `default: "STANDBY"` and an IGC message `drone_a.state=ENGAGING`. `HelmCore.cs` (line 940) parses `default:` as a number, and `Program.cs` `ApplyDynamicPayload` accepts only numbers, `true`/`on` (100) and `false`/`off` (0); other text is ignored. Found while building the `dynamic_strings` preset. Docs corrected; any text display of a dynamic string is not supported.

**M9. Nested templates are not supported.**
A `@use` inside a `@template` body is reported as `unrecognized directive` and produces no screen. The tracker's backlog assumed this worked (`template_nested`).

### Low

- **L1.** Untracked directories `brand/`, `context-source/`, `decom/`, `resources/`, and an empty `VERSION`: not tracked, not ignored.
- **L2.** `docs/content/getting-started.md` command table omits `scan` and `refresh` (both exist).
- **L3.** `Tests.cs` covers `HelmCore.cs` only; nothing covers `Program.cs`. The only check on it is the new stand-in compile in `tools/spritedump`.
- **L4.** No CI job compiles `Program.cs` or the combined script. `unit-tests` compiles only `HelmCore.cs` and `Tests.cs`, so H1 could not be caught by CI.
- **L5.** `README.md` and `docs` use marketing wording in places ("high-performance", "zero-GC") against the documentation style rules.

## WORKSHOP.md

`WORKSHOP.md` is not up to date. Checked against the code:

| Claim | Actual |
| :--- | :--- |
| "~93k characters minified (6.6k headroom)" | 98,871 characters, 1,129 headroom |
| `@use "battery_monolith"` (with quotes) | Syntax is `@use name(...)`. A quoted name is an unknown template |
| "one of 14 built-in presets"; `@use` examples `bridge_matrix`, `split_flight_deck`, `tactical_defense` | 8 built-in templates (`fighter_hud`, `reactor_matrix`, `bridge_status`, `mining_ops`, `airlock`, `rover`, `cargo_overview`, `battery_monolith`). The other three are layout files in `layouts/default/`, not templates |
| Commands `reboot` and `toggle <screen>` | Do not exist. Real commands: `reload`, `refresh`, `scan`/`diagnose`/`test`, `set <k> <v>` or `<k>=<v>`, `alert [on/off/toggle/red]` |
| `status()`: "OK / WARN / CRIT" | ONLINE / STBY / OFFLINE |
| "Cockpit 256x256" profile | No such profile. Profiles: 512x512, 1024x512, 512x256, 512x128, 256x128 |
| `border: graphic, double, single, heavy, none` | Only `ascii` and `none` change the in-game frame (M2) |
| Preset list of 17 layouts | 33 files in `layouts/default/`; several listed names are correct, many are missing |
| "over 50 terminal block types" | About 31 interfaces implemented (M1) |
| "Performance Guarantee" numbers | Unverified in game (M5) |
| `spinner(...)` | Valid alias of `flow(...)` |
| 13 visual controls | Correct (12 gauges plus `text`) |
| `boot_gradient: cyan, blue` | Valid (both `:` and `(...)` forms parse) |

## Recommended order

1. Regenerate and commit `HelmCombined*.cs` (H1). Add a CI compile check (L4).
2. Decide the Workshop preview asset and the fate of the moved files (H3).
3. Correct `WORKSHOP.md`, the block matrix, the border docs, the README links and the getting-started commands (H4, M1, M2, M3, L2).
4. Decide how to handle the 1,129-character headroom (H2).
5. Point the release scripts and CI at `VERSION` (M4).
6. Remove or explain `docs/.git`; decide whether the four untracked directories are tracked or ignored (M7, L1).
7. Run the script in game and record real instruction and allocation numbers before publishing performance claims (M5).

## Remediation status (same day)

| Finding | Status |
| :--- | :--- |
| H1 stale combined scripts | Fixed: regenerated (98,871 chars, compiles with 0 errors). CI job `combined-compile-check` added (L4) |
| H2 headroom | Open, decision needed (TASK-31.8) |
| H3 removed files | Partly fixed: `demo.ps1` references removed. Logo and Workshop preview need a decision (TASK-31.7). CI zip no longer lists the logo |
| H4 `WORKSHOP.md` | Fixed: see the table above. Unverified performance claims replaced by a not-measured note |
| H5 `Publish-Release.ps1` | Fixed; an archive-only local run now succeeds |
| M1 block matrix | Fixed: 7 rows marked as fallback only |
| M2 border docs | Fixed in README and docs; `border_styles_gallery` comment updated |
| M3 README links | Fixed: 9 absolute links made relative (the `PDR.md` link points at `context-source/PDR.md`, which is untracked) |
| M4 version source | Fixed: release script and CI package job fall back to `VERSION`. The file is still empty |
| M5 performance claims | Softened in `WORKSHOP.md` and README; real measurement still needed (TASK-31.10) |
| M6 `Program.cs` header | Fixed |
| M7 `docs/.git` | Open, decision needed (TASK-31.9) |
| L1 untracked directories | Open, decision needed (TASK-31.9) |
| L2 getting-started commands | Fixed |
| L3 no `Program.cs` tests | Open |
| L5 marketing wording | Open (README, some docs) |
