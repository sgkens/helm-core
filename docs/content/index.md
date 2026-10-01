---
title: "Overview"
description: "Telemetry display script for Space Engineers Programmable Blocks."
order: 1
permalink: /docs/index.html
section: "Overview"
eleventyComputed:
  badge: "{{ site.versionLabel }}"
---

## What It Does

HELM Core is a Programmable Block script that draws telemetry on LCDs and cockpit screens. Screens are described in the block's Custom Data with a text DSL.

- Binds blocks, block groups, wildcards or in-memory values to names, and shows them through 15 [visual controls](/docs/gauges/), including bars, text, status badges, dials and tanks.
- Draws with sprites (`MySprite`) or plain text, depending on the border and render mode.
- Drives one or more screens from one script, evaluating one normal telemetry screen per run; boot and diagnostic frames are separate.
- Draws liquid [tanks and vertical bars](/docs/gauges/#liquid-tank-tank-tube-bucket), sized rows, gradients and [conditional colours](/docs/dsl-syntax/#colour-choice-on-a-line), and [backgrounds](/docs/dsl-syntax/#backgrounds) that follow a value.
- Can [mirror](/docs/dsl-syntax/#mirroring-a-screen) one HELM screen on another display and show [text from other LCDs](/docs/dsl-syntax/#text-from-another-lcd).
- Provides [screen profiles](/docs/hardware-profiles/) for the standard LCD sizes and [layout files](/docs/presets-gallery/) for common ship types.
- Accepts values from other scripts through [IGC and argument commands](/docs/inter-script-api/).
- Includes a [command-line visualizer](/docs/helmv-cli/) for previewing layouts outside the game.
- Includes the browser-based [HELM Toolbox](/docs/helm-toolbox/) for presets, visual editing, hardware previews and layout import/export.
- Provides rates, ETA, history, item counts and docked-grid scope; see the [telemetry matrix](/docs/block-matrix/) and [DSL reference](/docs/dsl-syntax/).
- Offers `starter`, `names`, `scan`, `diagnose`, `probe`, alert and boot commands for setup and troubleshooting; see [Getting Started](/docs/getting-started/).

Start with [Getting Started](/docs/getting-started/), then read the [DSL Syntax](/docs/dsl-syntax/) reference.

## Programmable Block Limits

| Limit | Value |
| :--- | :--- |
| Script size | 100,000 characters. Use `HelmCombined.min.cs`; `Build-Release.ps1` reports the exact count. |
| Mods | None. The script uses only the vanilla ingame API. |

## Quick Example: Survival HUD

Example Custom Data for a cockpit screen:

```helm
@bind main_power = battery("Power Bank [Alpha]")
@bind o2_tank    = gastank("Life Support O2")
@bind hydrogen   = gastank("Main Hydrogen Tank")
@bind cargo_bay  = cargo("Ore Hopper")

@screen SurvivalCockpit
output: "Cockpit", 0
profile: CockpitSmall
bg: #090b0f
border_color: #38bdf8

@panel POWER at(0, 0, 1, 1) title: "SYSTEM RESERVES"
line: "BATTERY " bar(main_power.percent) gradient(#38bdf8, #818cf8)
line: "O2 TANK " bar(o2_tank.percent)    gradient(#06b6d4, #22c55e)
line: "HYDROGEN" bar(hydrogen.percent)   gradient(#eab308, #ef4444)
line: "CARGO   " bar(cargo_bay.percent)   flat(#94a3b8)
```
