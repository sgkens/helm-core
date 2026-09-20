# HELM: High-Efficiency Lightweight Monitor for Space Engineers

> **A declarative LCD telemetry script for Space Engineers.**
> Screens are described in the Programmable Block's Custom Data; no mods are required.

---

## Steam Workshop Description (BBCode Ready)

```text
[h1]HELM: High-Efficiency Lightweight Monitor[/h1]
[b]Declarative LCD dashboard script with 13 visual control types and automatic block discovery.[/b]

Are you tired of complex LCD scripts that lag your dedicated server, generate garbage collection spikes, or break the moment you rename a block? 
[b]HELM[/b] is a declarative HUD and LCD script for vanilla Space Engineers. It needs no mods.

[h2]✦ Key Features[/h2]
[list]
[*] [b]Retained sprite buffers[/b]: Sprites are created once and updated in place, and percentage strings are cached, to keep per-tick allocation low. Measured numbers are not published yet.
[*] [b]Declarative DSL in Custom Data[/b]: Configure multi-screen, multi-panel layouts directly in the Programmable Block's Custom Data with clean, human-readable syntax.
[*] [b]Zero-Friction Block Discovery[/b]: Wildcard pattern matching (e.g. [code]Battery*[/code], [code]*Hydro*[/code]) and automatic typed grid fallbacks. No need to spend 20 minutes renaming every battery or tank on your ship!
[*] [b]13 Specialized Visual Controls[/b]:
    • [b]bar()[/b]: Proportional fill bar with smoothed movement
    • [b]led()[/b]: Discrete 10-step segmented LED meter
    • [b]spark()[/b]: Historical 8-tick rolling sparkline trend graph
    • [b]hazard()[/b]: Hazard caution bar
    • [b]chevron()[/b]: Directional tactical flow segments
    • [b]dots()[/b]: High-density circular dot matrix meter
    • [b]balance()[/b]: Zero-centered bilateral horizon chart
    • [b]toggle()[/b]: ON/OFF latch switch
    • [b]checkbox()[/b]: Square verification checkbox indicator
    • [b]status()[/b]: Tri-state badge (ONLINE / STBY / OFFLINE)
    • [b]ammo()[/b]: Magazine bullet counter
    • [b]flow()[/b]: Directional fluid/gas transfer indicator (also written [code]spinner()[/code])
    • [b]text()[/b]: Formatted telemetry readout with threshold coloring
[*] [b]Engineering Units & Auto-Scaling[/b]: Units such as MW, GW, kW, kL, km, m/s and t, plus automatic scaling with [code]unit("auto")[/code].
[*] [b]Viewport Profiles[/b]: Fonts and padding adapt to the surface: Text Panel 512x512, Wide LCD 1024x512, Cockpit 512x256, Corner LCD 512x128 and Cockpit Corner 256x128.
[*] [b]Built-in Diagnostics[/b]: Run the script with the argument [code]scan[/code] (or [code]diagnose[/code]) for a report of all binds, surfaces and instruction use.
[*] [b]Boot Sequence[/b]: Optional staggered multi-screen boot with a custom message, terminal log lines and a gradient progress wipe.
[/list]

[h2]✦ Quick Start[/h2]
1. Build a [b]Programmable Block[/b] on your grid.
2. Paste the code from [b]HelmCombined.min.cs[/b] (or Steam Workshop blueprint).
3. Open the Programmable Block's [b]Custom Data[/b] and define your screens, or start from one of the 8 built-in templates with [code]@use[/code]:
   • [code]@use battery_monolith[/code]
   • [code]@use rover[/code]
   • [code]@use airlock[/code]
   • [code]@use cargo_overview[/code]
   Ready-made full layouts are also in the [b]layouts[/b] folder (see the list below); copy one into Custom Data.
4. Click [b]Check Code[/b] and [b]OK[/b]. Your screens initialize and draw.

[h2]✦ In-Game Commands & Micro-API[/h2]
Run the Programmable Block with the following arguments:
[list]
[*] [code]reload[/code]: Re-reads Custom Data.
[*] [code]refresh[/code]: Re-resolves block names and screen surfaces.
[*] [code]scan[/code] (also [code]diagnose[/code], [code]test[/code]): Prints a report of bound blocks, LCD surfaces and instruction use.
[*] [code]set <var> <val>[/code] (or [code]set <var>=<val>[/code]): Sets an in-memory dynamic variable, e.g. [code]set target.dist 1420[/code].
[*] [code]alert <on|off|toggle|red>[/code]: Turns the ship-wide red alert flashing on or off.
[/list]

[h2]✦ Headless Display Server (IGC Micro-Framework)[/h2]
External scripts (autopilots, combat computers, sorters) can broadcast directly to HELM over IGC channel [b]"HELM"[/b] without writing any UI code:
[code]IGC.SendBroadcastMessage("HELM", "target.lock=1;dist=1420;speed=104");[/code]

[h2]✦ Performance Notes[/h2]
• [b]Script size[/b]: about 98.9k characters minified, under the 100k Programmable Block limit.
• [b]Rendering[/b]: one screen is redrawn per run, so multi-screen setups are spread across ticks.
• [b]Not yet measured[/b]: instruction counts and allocations in game. Numbers will be added once they have been measured.
```

---

## DSL Quick Reference

### Screen Directives
```bash
@screen "Cockpit LCD"
output: "Cockpit", 0             # Block name, Surface Index
profile: auto                    # auto, standard, compact, wide, corner
border: graphic                  # ascii or none change the in-game frame; other styles draw the standard frame
border_color: cyan               # Named color (#hex or color name)
bg: #0a0c10                      # Background color
boot_duration: 2.0s              # Boot sequence duration
boot_gradient: cyan, blue        # Boot progress gradient
layout: grid(2, 2)               # Multi-panel grid (cols, rows)
```

### Bind Directives
```bash
# Single block or wildcard matching
@bind batt = block("Main Battery")
@bind hydro = block("*Hydrogen Tank*")

# Group with aggregation (avg, sum, min, max)
@bind cargo = group("Cargo*", agg: sum)
@bind drills = group("Drill*", agg: avg)
```

### Panel Directives
```bash
@panel power at(0, 0, 1, 1)
title: "POWER GRID"
line: "BATTERY" bar(batt.percent) gradient(red, yellow, green) unit("MW")
line: "TREND  " spark(batt.percent) flat(cyan)
line: "REACTOR" led(batt.output) flat(orange) unit("auto")
line: "ENGINES" toggle(batt.percent) flat(green)
line: "ASSEMBL" flow(factory.producing) flat(lime)
```

---

## Included Layouts
The `layouts` folder contains ready-made layouts. Copy one into the Programmable Block's Custom Data.

- `airlock_security`: Pressure cycle and bulkhead gate security monitor
- `all_gauges_showcase`: Every gauge kind on one screen, one kind per line, for renderer regression checks.
- `assembler_monolith`: Full-screen automated manufacturing plant with active machinery spinners
- `atmospheric_lander`: Panels: DESCENT VECTOR, LANDING SYSTEMS, SURFACE OPS, CRAFT INTEGRITY.
- `battery_monolith`: High-impact full-screen liquid power bank display (1x1 or Wide LCD)
- `border_styles_gallery`: One panel per border style.
- `bridge_matrix`: Capital ship 4-quadrant command matrix
- `cargo_minimal`: Two-line strip for a slanted CornerLCD (4:1).
- `cargo_monolith`: Full-screen monolithic automated logistics bay (1x1 or 1x2 LCD)
- `cargo_overview`: Dedicated station/ship logistics and material manifest display
- `command_carrier`: Panels: FLEET POWER, HANGAR OPS, WEAPONS BATTERY, POINT DEFENSE, PROPULSION, COMMAND NET.
- `corner_gauge_cluster`: Density limit for a 256x128 cockpit corner surface: four short lines.
- `deep_space_explorer`: Panels: NAVIGATION, POWER & RANGE, SURVEY & SENSORS, DOCKING & SUPPLY.
- `dynamic_telemetry`: Demonstrates HELM acting as a micro-framework receiving IGC / CLI data
- `fighter_hud`: Combat strike craft flight dynamics and tactical status
- `hazard_alert`: Tactical defense and reactor criticality warning display
- `jump_drive_spool`: Warp capacitor charging, hyperdrive coil alignment, and navigation lock
- `long_labels`: Edge case: labels and titles longer than the card, and long bind names.
- `medical_life_support`: O2 pressurization, cryo pod telemetry, medical bay biometrics, and vent flow
- `mining_fleet_coordinator`: Panels: MINER ALPHA, MINER BETA, FLEET CARGO, PROCESSING, FLEET POWER, OPERATIONS.
- `mining_rig`: Drill array load and ore extraction monitor
- `numeric_extremes`: Edge case: values at and beyond the gauge range (0, 100, negative, above 100).
- `planetary_base`: Panels: SURFACE POWER, LIFE SUPPORT, EXCAVATION, ORE PROCESSING, BASE DEFENSE, INFRASTRUCTURE.
- `power_only`: Batteries, reactor and solar on one panel; minimal binds.
- `reactor_monolith`: Full-screen monolithic nuclear power station display (1x1 or 1x2 LCD)
- `refinery_monolith`: Smelting furnace throughput, arc blast furnace loads, and slag flow
- `rover_cockpit`: Ground vehicle powertrain, chassis, and logistics cockpit layout
- `single_line`: Smallest useful configuration: one screen, one panel, one line.
- `split_flight_deck`: Ultra-widescreen split cockpit monitor (1x2 LCD or 5x3 Bridge Display)
- `survival_essentials`: Panels: POWER STATUS, LIFE SUPPORT, CARGO & SUPPLY, SYSTEM HEALTH.
- `tactical_defense`: Full-screen combat bridge display with weapons, ammo, shields, and toggles
- `thruster_balance`: Dual-panel flight dynamics HUD with zero-centered horizon balance charts
- `title_alignment`: Left, center and right titles with independent title colors and a border gradient.

---

## Block Support Matrix
HELM has dedicated readouts for about 30 Space Engineers terminal block types (batteries, reactors, tanks, doors, thrusters, drills, sensors and more). Other functional blocks report their on/off state. The full table is in the docs page "Terminal Block Binding Matrix".
