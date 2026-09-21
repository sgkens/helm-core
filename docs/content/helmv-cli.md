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
pwsh ./helmv.ps1 command_carrier -Measurement

# Test the battery monolith on a cockpit corner pod in strict ASCII mode
pwsh ./helmv.ps1 battery_monolith -ScreenProfile CockpitCorner -RenderMode Ascii

# One screen of a multi-screen layout
pwsh ./helmv.ps1 background_lab -ScreenName Alert
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
| `-Preset` | `string` | `bridge` | Name of a file in `layouts/default/` or `layouts/custom/` (without `.txt`). Tab completion lists the files, so new presets appear without editing the script |
| `-ScreenName` | `string` | first screen | Which screen of a multi-screen layout to draw, by name or by position (`0` is the first) |
| `-CustomPreset` | `string` | - | Direct path to a layout file, or the layout text itself |
| `-Frames` | `int` | `60` | Number of frames to simulate before exiting |
| `-Interactive` | `switch` | `$false` | Run until stopped, with the hotkeys listed above |
| `-UpdateFrequency` | `string` | `10x` | Simulated refresh rate: `1x`, `10x`, `100x` or `Custom` (with `-IntervalMs`) |
| `-IntervalMs` | `int` | - | Frame interval in milliseconds for `-UpdateFrequency Custom` |
| `-Boot` | `switch` | `$false` | Plays the boot sequence first |
| `-BootDuration` | `double` | screen value | Seconds the boot sequence takes |
| `-RedAlert` | `switch` | `$false` | Turns on the red alert: card backgrounds flash to the screen's `alert_bg` colour, as `alert red` does in game |
| `-ScreenProfile` | `string` | `Auto` | `TextPanel` (1:1), `WideLCD` (2:1), `CockpitSmall` (2:1), `CornerLCD` (4:1), `CockpitCorner` (2:1), `Off` |
| `-RenderMode` | `string` | `Auto` | `Graphic` (block characters and gradients), `Ascii` (strict vanilla text), `Auto` |
| `-StrictSEAscii` | `switch` | `$false` | Only characters present in the game's Monospace font |
| `-Border` | `string` | layout | `heavy`, `double`, `single`, `minimal`, `rounded`, `ascii`, `graphic`, `none` |
| `-TitleAlign` | `string` | layout | `left`, `center` or `right` |
| `-Measurement` / `-Percent` | `switch` | `$false` | Engineering units, or plain percentages, on every gauge |
| `-SingleColumn` / `-MultiColumn` | `switch` | auto | Stack the panels, or set them two to a row |
| `-ShowStats` / `-ShowSprites` | `switch` | `$false` | Print allocation counts, or the raw sprite list |
| `-FontMode` / `-FontSize` | | `Off` / `16` | Terminal font simulation (`Monospace`, `Standard`, `JetBrains`, `Cascadia`) |
| `-Help` | `switch` | `$false` | Displays the help screen and exits |

`Test-HelmLayout.ps1` takes the same layout parameters (`-Preset`, `-ScreenName`, `-Path`, `-LayoutText`, `-Boot`, `-Frames`, and so on) and adds `-CopyToClipboard` and `-PassThru`.

---

## What the Preview Does Not Draw

`helmv` is a text preview of the layout and its colours. It does not run the game's sprite renderer, so after the run it prints a `helmv:` note for each feature in the layout that it cannot show:

| Feature | In the preview | In the game |
| :--- | :--- | :--- |
| `tank`, `tube`, `bucket` | Plain bar | Liquid tube with wave and bubbles |
| `orientation: vertical` | Horizontal rows | Columns |
| `size(medium\|large)` | Same row height | Rows of 2 and 4 shares |
| `label_color`, `track`, `color_by` | Ignored | Applied |
| `dial` | Shown as a value | Arc of segments, needle and hub |
| `ticks`, `marker`, `outline`, `dir: left` | Not drawn | Drawn on the bar |
| `segments`, `points`, `icon`, `blink`, `value(off)` | Ignored | Applied |
| `rate`, `eta`, `trend` | No value | Change per minute, time to empty or full, arrow |
| `gap`, `padding` | Ignored | Applied |
| `border: frame`, `border: corners` | Plain frame | Sides, or corner brackets |
| A colour rule that answers `a/b` | Not blended | Gradient |
| `bg_gradient`, `bg_texture`, value-driven `bg:` | One card colour | Drawn as set |
| `mirror`, `mirror_panel`, `banner` | Not drawn | Copy of the screen, the ASCII art |
| `at(col, row, spanX, spanY)` | Panels stacked or two to a row | Placed on the grid |

Parser warnings (for example a `bg:` that colours one panel only) are printed in the same place, and a parse error stops the run with the line it came from. A layout with several screens shows the first one unless `-ScreenName` is given, and says so.

---

## VRage MySprite Frame Inspector

Add `-Frames 1 -ShowSprites` to verify exact sprite rendering parameters or inspect raw terminal string lengths:

```powershell
pwsh ./helmv.ps1 survival_essentials -Frames 1 -ShowSprites
```

This verifies that the 3-phase dynamic alignment engine maintains an exact card width across every visible line with zero text overflow.
