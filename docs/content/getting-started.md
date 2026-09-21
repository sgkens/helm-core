---
title: "Getting Started"
description: "Install the script, add a configuration and assign a screen."
order: 2
section: "Getting Started"
badge: "Quickstart"
---

## 1. Prerequisites

Before installing HELM Core:
- Ensure **In-Game Scripts** are enabled in your Space Engineers world settings:
  `World Settings -> Advanced -> Enable In-Game Scripts [x]`.
- Have at least one **Programmable Block** and at least one **Text Panel** or cockpit surface on your grid.

## 2. Installing the Script

1. Build or locate a **Programmable Block** on your grid.
2. Open its terminal interface and click **Edit Code**.
3. Select everything in the editor and replace it with the contents of **`HelmCombined.min.cs`** (under 100,000 characters, the Programmable Block limit). `HelmCombined.cs` is the same script with comments for reading; it is too long to paste.
4. Click **Check Code**.
5. Click **OK** and ensure the Programmable Block is powered **ON**.

```
[Programmable Block] -> Edit -> Paste -> Check Code -> OK
```

## 3. Configuring Custom Data

HELM Core reads its display configuration directly from the Programmable Block's **Custom Data** field.

1. Open the Programmable Block in the terminal.
2. Click **Custom Data** in the lower-right panel.
3. Paste a layout configuration (for example, our [Survival Essentials](/docs/presets-gallery/#survival-essentials) preset):

```helm
# Bind hardware terminal blocks
@bind batt  = battery("Battery")
@bind o2    = gastank("Oxygen Tank")
@bind h2    = gastank("Hydrogen Tank")
@bind cargo = cargo("Large Cargo")

# Define target screen
@screen MainCockpit
output: "Cockpit", 0
profile: CockpitSmall
layout: grid(2, 2)
bg: #090b0f
border_color: #38bdf8

# Top-Left: Power Reserves
@panel POWER at(0, 0, 1, 1) title: "POWER"
line: "BATTERY " bar(batt.percent) gradient(#38bdf8, #818cf8)

# Top-Right: Life Support
@panel LIFE at(1, 0, 1, 1) title: "LIFE SUP"
line: "O2 TANK " bar(o2.percent) flat(#06b6d4)
line: "H2 TANK " bar(h2.percent) flat(#eab308)

# Bottom-Left: Cargo
@panel CARGO at(0, 1, 1, 1) title: "STORAGE"
line: "CARGO   " bar(cargo.percent) flat(#94a3b8)
```

4. Click **OK** to save the Custom Data.
5. In the terminal, click **Run** (with argument `reload` or empty).

HELM will detect the changes, compile the AST, bind to matching grid blocks, and begin rendering immediately.

## 4. Setting Up Target Screens

The `output:` directive specifies where the layout renders:

```helm
output: "<Block Name>", <surface_index>
```

- **Stand-alone LCD Panels**:
  - `output: "LCD Panel Front", 0`
- **Cockpits & Flight Seats**:
  - `output: "Fighter Cockpit", 0` (Primary center screen)
  - `output: "Fighter Cockpit", 1` (Left instrument screen)
  - `output: "Fighter Cockpit", 2` (Right instrument screen)
- **Large Bridge Displays**:
  - `output: "Wide LCD Bridge Center", 0`

> **Tip:** You can declare multiple `@screen` blocks in Custom Data to power 10+ different LCDs from a single Programmable Block!

## 5. Verify & Troubleshoot

Open the Programmable Block terminal. The output pane shows one line per run, with the screen that was drawn, the instruction count and the run time (example values):

```text
HELM [SurvivalCockpit] instr 1234/50000 — 0.2ms
```

If a block is missing or renamed, HELM enters safe fallback mode and marks the telemetry line with `---` without crashing or throwing script exceptions.

### Common Commands

You can run these commands from a timer block, sensor, or cockpit hotbar:

| Argument | Action |
| :--- | :--- |
| `reload` | Re-parses Custom Data and rebuilds screen ASTs. |
| `refresh` | Re-resolves block names and screen surfaces without re-parsing. |
| `scan` (also `diagnose`, `test`) | Prints a diagnostic report of binds, surfaces and instruction use. |
| `alert red` (also `on`) | Turns on the flashing red alert across all screens. |
| `alert off` | Clears the alert state. |
| `alert toggle` | Flips the alert state. |
| `set <var> <val>` | Sets an in-memory dynamic telemetry variable. |

Next, explore the [DSL Syntax Specification](/docs/dsl-syntax/) to customize panels and gauges.
