# HELM: High-Efficiency Lightweight Monitor for Space Engineers

> **A declarative LCD telemetry script for Space Engineers.**
> Screens are described in the Programmable Block's Custom Data; no mods are required.

---

## Steam Workshop Description (BBCode Ready)

The fenced BBCode below is the published page source, exported to `workshop/build/description.bbcode.txt`. The Markdown sections after it are repository reference appendices, not part of the Steam description. For existing items, paste the full export into Steam's Edit Description page; publishing preserves that description.

```text
[h1]HELM: High-Efficiency Lightweight Monitor[/h1]
[b]A configurable LCD and cockpit dashboard script for vanilla Space Engineers Programmable Blocks.[/b]

HELM turns LCDs and cockpit surfaces into telemetry dashboards. Define screens, panels, gauges, colours and block bindings in the Programmable Block's Custom Data. No client mod is required.

[h2]Online Tools and Documentation[/h2]
[list]
[*][url=https://sgkens.gitlab.io/helm-core/helm-toolbox/]HELM Toolbox[/url] - build layouts visually using the real parser and renderer in your browser.
[*][url=https://sgkens.gitlab.io/helm-core/docs/]Complete Documentation[/url] - installation, DSL reference, gauges, telemetry fields, hardware profiles and preset gallery.
[*][url=https://gitlab.com/sgkens/helm-core]Source and Issue Tracker[/url] - source code, releases and problem reports.
[/list]

[h2]Key Features[/h2]
[list]
[*][b]Declarative layouts[/b] - configure screens in Custom Data instead of editing C#.
[*][b]Automatic block discovery[/b] - bind exact names, groups and wildcards such as [code]Battery*[/code]
[*][b]15 visual controls[/b] - bars, LEDs, sparklines, hazards, chevrons, dots, balance charts, toggles, checkboxes, status badges, ammo, flow indicators, text, dials and tanks.
[*][b]Responsive multi-screen layouts[/b] - grids, panels, row sizing, columns, mirrors and profiles for square, wide and cockpit surfaces.
[*][b]Telemetry and engineering units[/b] - batteries, power, gas, cargo, production, doors, weapons, flight systems and other terminal blocks with automatic unit scaling.
[*][b]Styling and alerts[/b] - gradients, conditions, markers, ticks, frames, backgrounds, textures, icons, blinking and ship-wide red alert state.
[*][b]Rates, ETA and history[/b] - charge or drain rate, estimated time remaining and rolling trends.
[*][b]Low-allocation rendering[/b] - retained sprite buffers and round-robin screen updates limit work per run.
[*][b]Diagnostics[/b] - scan bindings and surfaces, list grid names, probe display geometry and inspect runtime use from the Programmable Block console.
[/list]

[h2]Quick Start[/h2]
[olist]
[*]Enable [b]Experimental Mode[/b] and [b]In-game Scripts[/b] for the world.
[*]Subscribe, place a Programmable Block, open [b]Edit → Browse Scripts[/b], select HELM Core, then run it.
[*]Run the Programmable Block with argument [code]starter[/code] to write a sample into empty Custom Data.
[*]Change the sample [code]output:[/code] name to match your LCD or cockpit block, then run [code]reload[/code]
[*]Use [url=https://sgkens.gitlab.io/helm-core/helm-toolbox/]HELM Toolbox[/url] to choose a preset, edit it visually and copy the finished Custom Data into the block.
[/olist]

[h2]Minimal Display Example[/h2]
[code]
@bind power = group("Main Batteries", agg: avg)

@screen bridge
output: "Bridge LCD", 0
profile: auto
title: "BRIDGE POWER"
line: "BATTERY" bar(power.percent) gradient(green, yellow, red)
[/code]

Name a battery group [b]Main Batteries[/b] and an LCD [b]Bridge LCD[/b], paste the example into Custom Data, and run [code]reload[/code]. See the [url=https://sgkens.gitlab.io/helm-core/docs/getting-started/]Getting Started guide[/url] for multi-screen setup and cockpit surface numbers.

[h2]In-Game Commands & Micro-API[/h2]
Run the Programmable Block with the following arguments:
[list]
[*][code]reload[/code] - re-read Custom Data and rebuild the layout.
[*][code]refresh[/code] - resolve block names and surfaces again.
[*][code]scan[/code] / [code]diagnose[/code] - report bindings, displays, errors and runtime use.
[*][code]starter[/code] - write a sample layout into empty Custom Data without overwriting existing data.
[*][code]names[/code] - append grid block and group names as comments for copying into binds.
[*][code]probe[/code] - draw a temporary geometry test pattern.
[*][code]alert on|off|toggle|red[/code] - control the global alert state.
[*][code]boot[/code] - replay configured boot sequences.
[*][code]console on|off[/code] - show or hide the Programmable Block status console.
[*][code]set <var> <value>[/code] - set an in-memory value for external automation and testing.
[/list]

[h2]Headless Display Server (IGC Micro-Framework)[/h2]
External scripts (autopilots, combat computers, sorters) can broadcast directly to HELM over IGC channel [b]"HELM"[/b] without writing any UI code:
[code]IGC.SendBroadcastMessage("HELM", "target.lock=1;dist=1420;speed=104");[/code]

[h2]Configuration Essentials[/h2]
[list]
[*]Use [code]@bind[/code] for each alias; groups require [code]agg: avg|sum|min|max[/code]. Add [code]scope: all[/code] only when you intentionally want docked ships included.
[*]Use [code]layout: grid(2, 2)[/code] and [code]@panel power at(0, 0, 1, 1)[/code] to split a screen. Reusable [code]@template[/code] and [code]@use[/code] directives avoid repeating layouts.
[*]Raw item counts need a full-scale amount: [code]bar(cargo.ingot:Iron) max(5000)[/code]. Use [code]text(batt.rate)[/code], [code]text(batt.eta)[/code] or [code]text(batt.trend)[/code] for history-derived readouts.
[*]For IGC variables, declare [code]@bind range = dynamic("dist", default: 0)[/code], then display [code]text(range.val)[/code].
[/list]

[h2]Troubleshooting[/h2]
[list]
[*][b]Blank display:[/b] run [code]reload[/code], check the Programmable Block detail output, verify the exact LCD name and confirm the surface index.
[*][b]Bind error or zero value:[/b] every alias used by a gauge needs a matching [code]@bind[/code]. Run [code]names[/code] to copy exact block/group names and consult the [url=https://sgkens.gitlab.io/helm-core/docs/block-matrix/]telemetry matrix[/url].
[*][b]Layout clips or scales incorrectly:[/b] use [code]profile: auto[/code], run [code]probe[/code], and preview the same hardware profile in Toolbox.
[*][b]Script is missing in game:[/b] confirm you subscribed to an item with [b]Type: IngameScript[/b], then use the Programmable Block's Browse Scripts screen rather than the world Mods list.
[*][b]Updated Workshop script:[/b] subscribed scripts update in Browse Scripts, but an existing Programmable Block keeps its pasted copy. Select the updated script again to replace the code; Custom Data layouts remain separate.
[/list]

[h2]Reference Links[/h2]
For deeper help, use the links below or open Toolbox to browse presets and preview your actual screen profile. The repository Workshop reference also includes the layout catalogue and a compact DSL reference.
[list]
[*][url=https://gitlab.com/sgkens/helm-core/-/blob/main/WORKSHOP.md]Workshop reference and layout catalogue[/url]
[*][url=https://sgkens.gitlab.io/helm-core/docs/dsl-syntax/]DSL syntax and examples[/url]
[*][url=https://sgkens.gitlab.io/helm-core/docs/gauges/]Gauge reference[/url]
[*][url=https://sgkens.gitlab.io/helm-core/docs/block-matrix/]Supported blocks and telemetry fields[/url]
[*][url=https://sgkens.gitlab.io/helm-core/docs/hardware-profiles/]LCD and cockpit hardware profiles[/url]
[*][url=https://sgkens.gitlab.io/helm-core/docs/presets-gallery/]Preset gallery[/url]
[*][url=https://sgkens.gitlab.io/helm-core/docs/inter-script-api/]IGC and command API[/url]
[*][url=https://gitlab.com/sgkens/helm-core/-/issues]Report a problem[/url]
[/list]

[h2]Support Information[/h2]
When reporting a problem, include the HELM version, the smallest Custom Data example that reproduces it, block and surface type, Programmable Block detail output, and whether the same layout fails in Toolbox. Remove private server details first.

[b]Platform note:[/b] Programmable Block scripts run on PC and dedicated servers. Consoles cannot execute user scripts unless the script runs server-side on a compatible cross-play server.

```

---

## DSL Quick Reference

### Screen Directives

```bash
@screen "Cockpit LCD"
output: "Cockpit", 0             # Block name, Surface Index
profile: auto                    # auto, standard, compact, wide, corner
border: frame                    # graphic, frame, corners, single, heavy, double, rounded, ascii or none
border_color: cyan               # Named color (#hex or color name)
bg: #0a0c10                      # Background color (or bg_gradient(#000, #123), screen_bg:, panel_bg:)
refresh: 600                     # Draw at most every N ticks (optional)
gap: 8                           # Pixels between panels (padding: N sets the screen edge)
mirror: "Other Screen"           # Copy another screen instead of drawing panels
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
line: "CARGO  " bar(cargo.percent) size(medium) gradient(#22c55e, #ef4444) track(#111827)
line: "IRON   " bar(cargo.ingot:Iron) max(5000) flat(cyan)
line: "H2     " tank(h2.percent) gradient(#1e3a8a, #38bdf8)
line: "POWER  " dial(batt.percent) size(large) gradient(#22c55e, #eab308, #ef4444)
line: "BATT   " bar(batt.percent) ticks(5) marker(25) outline(#94a3b8)
line: "ETA    " text(batt.eta) flat(#94a3b8)
```

---

## Included Layouts

The `layouts` folder contains 65 ready-made layouts. Copy one into the Programmable Block's Custom Data.

- `airlock_security`: Pressure cycle and bulkhead gate security monitor
- `alert_states`: Values at and past alert thresholds: hazard bars, status badges and ammo counters.
- `all_gauges_showcase`: Every gauge kind on one screen, one kind per line, for renderer regression checks.
- `assembler_monolith`: Full-screen automated manufacturing plant with active machinery spinners
- `atmospheric_lander`: Panels: DESCENT VECTOR, LANDING SYSTEMS, SURFACE OPS, CRAFT INTEGRITY.
- `background_lab`: What backgrounds can do: a gradient (vertical or horizontal), deliberate two-tone panels, a see-through screen, an alert background, a panel that changes colour with a value, a panel that flashes on a threshold and a texture. 1. Vertical gradient behind translucent panel cards (panel_bg: with an alpha value). 2. Two tones on purpose: each panel has its own bg: after its @panel line. 3. See-through: no background and no cards, so only the bars and text are drawn. 4. Alert: run the block with the argument "alert red" and the background flashes to alert_bg. 5. A panel that follows a value: red under 25%, amber under 60%, otherwise green. A second panel flashes below 15%. 6. A horizontal gradient with a built-in texture over it. Run the block with the argument "scan" to list the texture names.
- `bar_sizes`: One panel with a large, a medium and a normal bar. size(...) sets how much of the panel a row takes (4, 2 and 1 shares); the bar grows with it.
- `battery_monolith`: High-impact full-screen liquid power bank display (1x1 or Wide LCD)
- `boot_custom`: Custom boot message, log lines, color and gradient, with a longer duration.
- `border_styles_gallery`: One panel per border style; compare in-game screen and panel frames with the offline preview.
- `bridge_matrix`: Capital ship 4-quadrant command matrix
- `cargo_contents`: Amounts of named items in a cargo group, and a reactor's fuel. Field format: kind:Subtype.
- `cargo_minimal`: Two-line strip for a slanted CornerLCD (4:1).
- `cargo_monolith`: Full-screen monolithic automated logistics bay (1x1 or 1x2 LCD)
- `cargo_overview`: Dedicated station/ship logistics and material manifest display
- `combo_dashboard`: Mixes the layout tools: a tank column, sized bars, and colours that change with a condition (a rule can answer with a gradient such as red/orange).
- `command_carrier`: Panels: FLEET POWER, HANGAR OPS, WEAPONS BATTERY, POINT DEFENSE, PROPULSION, COMMAND NET.
- `corner_gauge_cluster`: Density limit for a 256x128 cockpit corner surface: four short lines.
- `deep_space_explorer`: Panels: NAVIGATION, POWER & RANGE, SURVEY & SENSORS, DOCKING & SUPPLY.
- `docking_control`: Connector status, landing gear and docking sensors for a dock arm.
- `dynamic_strings`: Text values on dynamic binds: a quoted default, replaced by text sent over IGC or the run command.
- `dynamic_telemetry`: Demonstrates HELM acting as a micro-framework receiving IGC / CLI data
- `fighter_ammo`: Ammunition and missiles as bars against a full load, with fuel and an ammunition trend. max(N) is the full load: the bar shows amount / N, the text shows the real amount.
- `fighter_hud`: Combat strike craft flight dynamics and tactical status
- `gauge_lab`: The gauge options in one screen: a dial, a bar with ticks, markers and an outline, a bar that fills from the right, a 20-segment led bar, a 24-column sparkline, and framed panels.
- `gradient_stress`: Edge case: gradients with many stops, two identical stops (a single-stop gradient is a parse error), and named versus hex colors.
- `grid_4x4`: Sixteen panels on one large screen: density and per-panel viewport limits.
- `hangar_bay`: Doors, connector, landing gear and pressure for a hangar.
- `hazard_alert`: Tactical defense and reactor criticality warning display
- `hydrogen_plant`: Gas generators, tanks and hydrogen engines.
- `jump_drive_spool`: Warp capacitor charging, hyperdrive coil alignment, and navigation lock
- `long_labels`: Edge case: labels and titles longer than the card, and long bind names.
- `medical_life_support`: O2 pressurization, cryo pod telemetry, medical bay biometrics, and vent flow
- `mining_fleet_coordinator`: Panels: MINER ALPHA, MINER BETA, FLEET CARGO, PROCESSING, FLEET POWER, OPERATIONS.
- `mining_rig`: Drill array load and ore extraction monitor
- `missing_binds`: Binds that match no block: every value reads as an error.
- `multi_screen_one_pb`: Three screens driven by one Programmable Block; one screen is redrawn per run (round-robin).
- `numeric_extremes`: Edge case: values at and beyond the gauge range (0, 100, negative, above 100).
- `panel_spans`: Panels spanning several columns and rows on a 3x3 grid.
- `piston_crane`: Piston extension and rotor angle for a crane arm.
- `planetary_base`: Panels: SURFACE POWER, LIFE SUPPORT, EXCAVATION, ORE PROCESSING, BASE DEFENSE, INFRASTRUCTURE.
- `power_only`: Batteries, reactor and solar on one panel; minimal binds.
- `projector_welder`: Build progress of a projected blueprint and whether the welders are working.
- `reactor_monolith`: Full-screen monolithic nuclear power station display (1x1 or 1x2 LCD)
- `refinery_monolith`: Smelting furnace throughput, arc blast furnace loads, and slag flow
- `rover_cockpit`: Ground vehicle powertrain, chassis, and logistics cockpit layout
- `rover_suspension`: Height, strength, power, friction and brake of a rover's wheels. Strength, power and friction are 0 to 100.
- `sensor_security`: Sensors, cameras, turrets and door interlocks for a perimeter.
- `single_line`: Smallest useful configuration: one screen, one panel, one line.
- `solar_farm`: Many solar panels aggregated, with battery storage and wind support.
- `split_flight_deck`: Ultra-widescreen split cockpit monitor (1x2 LCD or 5x3 Bridge Display)
- `startup_banner`: The same ASCII art as a full-screen banner and as the boot logo of a second screen. The in-game script and the docs preview draw banners; helmv.ps1 does not.
- `survival_essentials`: Panels: POWER STATUS, LIFE SUPPORT, CARGO & SUPPLY, SYSTEM HEALTH.
- `tactical_defense`: Full-screen combat bridge display with weapons, ammo, shields, and toggles
- `tank_fullscreen`: One liquid tube filling the whole screen. The liquid rises with the value, a wave line of characters slides along the surface and bubbles rise through it. The gradient runs through the liquid from the bottom to the top.
- `tank_trio`: Medium tanks: three panels side by side, each one a tank that fills its panel.
- `template_nested`: A template that invokes another template: a ship pod that stamps one nacelle screen per engine.
- `template_stamp`: A user-defined template stamped out for two ships with single-line @use calls.
- `thruster_balance`: Dual-panel flight dynamics HUD with balance charts centered at 50%.
- `title_alignment`: Left, center and right titles with independent title colors and a border gradient.
- `top_contents`: The five largest items held by a cargo group, ranked by amount and refreshed every 15 seconds. text(bind.top1) is the largest item, top2 the next, and so on. Ore and ingots are named with their kind.
- `trading_station`: Cargo values and connector traffic for a trade dock.
- `transparent_bg`: Edge case: transparent screen background so the LCD's own background shows through.
- `units_all`: Every engineering unit in one panel, for measurement-mode formatting.
- `vertical_bars`: A panel with orientation: vertical lays its lines out as columns: the value on top, the bar in the middle, the label below.

---

## Block Support Matrix

HELM has dedicated readouts for batteries, reactors, tanks, doors, thrusters, drills, sensors and many other terminal block types. Other functional blocks report their on/off state. See the [Terminal Block Binding Matrix](https://sgkens.gitlab.io/helm-core/docs/block-matrix/) for the current supported fields and units.
