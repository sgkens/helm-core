---
title: "CLI Visualizer (helmv)"
description: "Command-line visualizer for previewing layouts without the game."
order: 8
section: "Tooling"
badge: "CLI"
---

## Running the Visualizer

Launch with any preset, custom layout file, or interactive flags:

```powershell
# Interactive visualizer with default preset
pwsh ./helmv.ps1

# Inspect the command carrier preset in engineering measurement mode
pwsh ./helmv.ps1 -Preset command_carrier -Measurement

# Test the battery monolith on a cockpit corner pod in strict ASCII mode
pwsh ./helmv.ps1 -Preset battery_monolith -ScreenProfile CockpitCorner -RenderMode Ascii
```

---

## Built-In Help & ASCII Banner

Run `./helmv.ps1 -Help` (or `-h`, `-?`) to display the complete visual reference banner rendered in 256-color ANSI gradients:

```text
  _    _ ______ _      __  __ _      __
 | |  | |  ____| |    |  \/  | |    / /
 | |__| | |__  | |    | \  / | |   / / 
 |  __  |  __| | |    | |\/| | |  / /  
 | |  | | |____| |____| |  | | | / /   
 |_|  |_|______|______|_|  |_|_|/_/    
```

---

## Interactive Runtime Hotkeys

While `helmv.ps1` is running interactively, you can control the simulation using these single-key shortcuts:

| Hotkey | Action | Description |
| :---: | :--- | :--- |
| **`M`** | Units Mode | Switches all panel lines to real-world engineering measurements (`MW`, `kL`, `m/s`, etc.). |
| **`P`** | Percent Mode | Forces pure percentage formatting (`xx.x%`) across all telemetry lines. |
| **`U`** | Cycle Units | Toggles between Auto, Measurement, and Percent modes on the fly. |
| **`Tab`** | Cycle Units | Alternative key to cycle unit formatting modes. |
| **`Space`** | Pause / Cycle | Cycles telemetry display modes during continuous simulation. |
| **`Q`** | Quit | Restores the terminal cursor and cleanly exits the visualizer. |

---

## Command Line Parameters

| Parameter | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| `-Preset` | `string` | `default` | Preset name from `layouts/default/` or `layouts/custom/`. Tab completion supported! |
| `-CustomPreset` | `string` | - | Direct path to any custom `.txt` or `.helm` layout file. |
| `-ScreenProfile` | `string` | `Auto` | `TextPanel` (1:1), `WideLCD` (2:1), `CockpitSmall` (2:1), `CornerLCD` (4:1), `CockpitCorner` (2:1), `Off`. |
| `-RenderMode` | `string` | `Auto` | `Graphic` (UTF-8 block characters and gradients), `Ascii` (strict vanilla SE text), `Auto`. |
| `-Border` | `string` | Layout def | `heavy`, `double`, `single`, `minimal`, `rounded`, `ascii`, `graphic`, `none`. |
| `-Measurement` | `switch` | `$false` | Enables engineering measurements with dynamic width balancing (aliases: `-Units`). |
| `-Percent` | `switch` | `$false` | Forces clean percentage formatting across all gauges. |
| `-SingleFrame` | `switch` | `$false` | Renders a single frame and exits (perfect for headless CI or piping). |
| `-Duration` | `int` | `0` (indefinite)| Automatic exit after N seconds. |
| `-Speed` | `double` | `1.0` | Telemetry simulation oscillation speed multiplier (0.1x to 10x). |
| `-Help` | `switch` | `$false` | Displays visual help screen with ASCII banner and exits immediately. |

---

## VRage MySprite Frame Inspector

Add `-SingleFrame` to verify exact sprite rendering parameters or inspect raw terminal string lengths:

```powershell
pwsh ./helmv.ps1 -Preset survival_essentials -SingleFrame
```

This verifies that the 3-phase dynamic alignment engine maintains an exact card width across every visible line with zero text overflow.
