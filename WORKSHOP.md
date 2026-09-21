# HELM: High-Efficiency Lightweight Monitor for Space Engineers

> **A declarative LCD telemetry script for Space Engineers.**
> Screens are described in the Programmable Block's Custom Data; no mods are required.

---

## Steam Workshop Description (BBCode Ready)

```text
[h1]HELM: High-Efficiency Lightweight Monitor[/h1]
[b]Declarative LCD dashboard script with 15 visual control types and automatic block discovery.[/b]

Are you tired of complex LCD scripts that lag your dedicated server, generate garbage collection spikes, or break the moment you rename a block? 
[b]HELM[/b] is a declarative HUD and LCD script for vanilla Space Engineers. It needs no mods.

[h2]✦ Key Features[/h2]
[list]
[*] [b]Retained sprite buffers[/b]: Sprites are created once and updated in place, and percentage strings are cached, to keep per-tick allocation low. Measured numbers are not published yet.
[*] [b]Declarative DSL in Custom Data[/b]: Configure multi-screen, multi-panel layouts directly in the Programmable Block's Custom Data with clean, human-readable syntax.
[*] [b]Zero-Friction Block Discovery[/b]: Wildcard pattern matching (e.g. [code]Battery*[/code], [code]*Hydro*[/code]) and automatic typed grid fallbacks. No need to spend 20 minutes renaming every battery or tank on your ship!
[*] [b]15 Specialized Visual Controls[/b]:
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
    • [b]dial()[/b]: Half-circle gauge with a lit arc, needle and hub (also [code]gauge()[/code] and [code]arc()[/code])
    • [b]tank()[/b]: Liquid tube that fills from the bottom, with a moving wave line and rising bubbles (also [code]tube()[/code] and [code]bucket()[/code])
[*] [b]Sizes, Columns and Tanks[/b]: [code]size(medium)[/code] and [code]size(large)[/code] give a row 2 or 4 shares of its panel, [code]orientation: vertical[/code] lays lines out as columns, and a panel that spans the screen is a full-screen tank or bar. Grid layouts combine all of these.
[*] [b]Colours, Gradients and Conditions[/b]: [code]flat()[/code], [code]gradient()[/code] and [code]color()[/code] rules work on every gauge. A rule can answer with a gradient ([code]red/orange[/code]), [code]color_by()[/code] colours a gauge by another value, and [code]track()[/code] and [code]label_color()[/code] colour the empty part and the label.
[*] [b]Bar Dressing and Line Details[/b]: [code]ticks()[/code], [code]marker()[/code], [code]outline()[/code], [code]dir: left[/code], [code]segments()[/code], [code]points()[/code], [code]value(off)[/code], [code]icon()[/code] and [code]blink()[/code].
[*] [b]Rate, ETA and Trend[/b]: [code]text(batt.rate)[/code], [code]eta[/code] and [code]trend[/code] show how fast a battery or tank is filling or draining and when it will be empty or full.
[*] [b]Frames[/b]: [code]border:[/code] styles ([code]frame[/code], [code]corners[/code], [code]double[/code], [code]rounded[/code], [code]heavy[/code]) are drawn in game, for screens and for single panels.
[*] [b]Backgrounds[/b]: Solid or gradient (vertical or horizontal), see-through, textured, colours that follow a value, backgrounds that flash on a threshold, and a red-alert flash.
[*] [b]Contents and Amounts[/b]: Bars and text for items in cargo and reactors ([code]cargo.ingot:Iron[/code]), the five largest items ([code]top1[/code] to [code]top5[/code]), and [code]max(N)[/code] as the full amount.
[*] [b]Mirrors and LCD Text[/b]: [code]mirror:[/code] shows another HELM screen (or one panel of it) on another display. [code]text(notes.line1)[/code] shows a line of the text on any other LCD. Only text can be read; the game gives scripts no access to another screen's picture.
[*] [b]ASCII Banners[/b]: Full-screen ASCII art, or your own boot logo.
[*] [b]Engineering Units & Auto-Scaling[/b]: Units such as MW, GW, kW, kL, km, m/s and t, plus automatic scaling with [code]unit("auto")[/code].
[*] [b]Viewport Profiles[/b]: Fonts and padding adapt to the surface: Text Panel 512x512, Wide LCD 1024x512, Cockpit 512x256, Corner LCD 512x128 and Cockpit Corner 256x128.
[*] [b]Built-in Diagnostics[/b]: Run the script with the argument [code]scan[/code] (or [code]diagnose[/code]) for a report of all binds, surfaces and instruction use.
[*] [b]Boot Sequence[/b]: Optional staggered multi-screen boot with a logo, a custom message, terminal log lines, a gradient progress wipe and a gauge calibration sweep.
[*] [b]Console[/b]: The Programmable Block's own screen shows a status console (screens, binds, errors, run time, instruction use).
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
5. With an empty Custom Data the LCDs stay blank: run the block with the argument [code]starter[/code] to write a sample layout, set [code]output:[/code] to your LCD's name and run [code]reload[/code]. Run [code]probe[/code] to check what part of a screen the script covers.

[h2]✦ In-Game Commands & Micro-API[/h2]
Run the Programmable Block with the following arguments:
[list]
[*] [code]reload[/code]: Re-reads Custom Data.
[*] [code]refresh[/code]: Re-resolves block names and screen surfaces.
[*] [code]scan[/code] (also [code]diagnose[/code], [code]test[/code]): Prints a report of bound blocks, LCD surfaces and instruction use.
[*] [code]set <var> <val>[/code] (or [code]set <var>=<val>[/code]): Sets an in-memory dynamic variable, e.g. [code]set target.dist 1420[/code].
[*] [code]alert <on|off|toggle|red>[/code]: Turns the ship-wide red alert on or off; backgrounds flash to their [code]alert_bg[/code] colour and red gauges flash.
[*] [code]boot[/code]: Plays the boot sequence again on every screen.
[*] [code]starter[/code]: Writes a sample layout into an empty Custom Data (never overwrites) and loads it.
[*] [code]probe[/code]: Draws a test pattern for 20 seconds on the block's screen and on every configured screen.
[*] [code]console on|off[/code]: Shows or hides the console on the Programmable Block's own screen.
[/list]

[h2]✦ Headless Display Server (IGC Micro-Framework)[/h2]
External scripts (autopilots, combat computers, sorters) can broadcast directly to HELM over IGC channel [b]"HELM"[/b] without writing any UI code:
[code]IGC.SendBroadcastMessage("HELM", "target.lock=1;dist=1420;speed=104");[/code]

[h2]✦ Performance Notes[/h2]
• [b]Script size[/b]: about 94.2k characters minified, under the 100k Programmable Block limit.
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
- `border_styles_gallery`: One panel per border style. In game only the screen-level style matters (ascii and none differ); panel-level border: renders in helmv.ps1 only.
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
- `thruster_balance`: Dual-panel flight dynamics HUD with zero-centered horizon balance charts
- `title_alignment`: Left, center and right titles with independent title colors and a border gradient.
- `top_contents`: The five largest items held by a cargo group, ranked by amount and refreshed every 15 seconds. text(bind.top1) is the largest item, top2 the next, and so on. Ore and ingots are named with their kind.
- `trading_station`: Cargo values and connector traffic for a trade dock.
- `transparent_bg`: Edge case: transparent screen background so the LCD's own background shows through.
- `units_all`: Every engineering unit in one panel, for measurement-mode formatting.
- `vertical_bars`: A panel with orientation: vertical lays its lines out as columns: the value on top, the bar in the middle, the label below.

---

## Block Support Matrix
HELM has dedicated readouts for about 30 Space Engineers terminal block types (batteries, reactors, tanks, doors, thrusters, drills, sensors and more). Other functional blocks report their on/off state. The full table is in the docs page "Terminal Block Binding Matrix".
