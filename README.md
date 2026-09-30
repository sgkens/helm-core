<div align="center">

<img src="https://r2os.staticcanvas.me/logos/helm-core/dist/webp/helm-core-logo-256x256.webp">

<p>
  <a href="LICENSE"><img alt="MIT License" src="https://img.shields.io/badge/license-MIT-0891b2?style=flat-square&amp;labelColor=ffffff&amp;logo=opensourceinitiative&amp;logoColor=0891b2"></a>
  <a href="https://gitlab.com/sgkens/helm-core/-/pipelines"><img alt="Pipeline status" src="https://img.shields.io/gitlab/pipeline-status/sgkens/helm-core?branch=main&amp;style=flat-square&amp;label=pipeline&amp;labelColor=ffffff&amp;color=0891b2&amp;logo=gitlab&amp;logoColor=0891b2"></a>
  <a href="https://gitlab.com/sgkens/helm-core/-/tags"><img alt="Latest tag" src="https://img.shields.io/gitlab/v/tag/sgkens/helm-core?style=flat-square&amp;label=tag&amp;labelColor=ffffff&amp;color=0891b2&amp;logo=gitlab&amp;logoColor=0891b2"></a>
  <a href="https://gitlab.com/sgkens/helm-core/-/releases"><img alt="Latest release" src="https://img.shields.io/gitlab/v/release/sgkens/helm-core?style=flat-square&amp;label=release&amp;labelColor=ffffff&amp;color=0891b2&amp;logo=gitlab&amp;logoColor=0891b2"></a>
  <a href="https://app.codecov.io/gitlab/sgkens/helm-core"><img alt="Codecov coverage" src="https://img.shields.io/codecov/c/gitlab/sgkens/helm-core?style=flat-square&amp;label=coverage&amp;labelColor=ffffff&amp;color=0891b2&amp;logo=codecov&amp;logoColor=0891b2"></a>
</p>

# HELM-_Core_

> High-Efficiency Lightweight **Deterministic, low-overhead console telemetry for Space Engineers.**

• Vanilla-compatible
• Zero-dependency \
• Retained sprite rendering \
• Round-robin screen scheduling.

</div>

---

## **Overview & Philosophy**

**HELM** is engineered with a strict performance budget from day one:

1. **Multi-Tick Round-Robin Scheduling:** Normal telemetry updates evaluate one screen per run. Boot sequences and diagnostics can draw additional frames; large grids and inventory scans still consume the game's instruction budget.
2. **Dirty-Flag Skipping:** Unchanged output can skip `DrawFrame()`. The script still runs to sample data and decide whether a redraw is needed.
3. **Decoupled Declarative DSL:** Block binding (`@bind`) is strictly separated from presentation (`@screen`), preventing silent rendering failures and debugging headaches.
4. **Retained Rendering Buffers:** Reused sprite buffers and cached percentage strings reduce rendering work and allocations; this is not a zero-allocation guarantee.

[Documentation](https://sgkens.gitlab.io/helm-core/docs/) · [HELM Toolbox](https://sgkens.gitlab.io/helm-core/helm-toolbox/) · [Workshop page source](WORKSHOP.md) · [Releases](https://gitlab.com/sgkens/helm-core/-/releases)

---

## **Quick Start:**

> [info]
> Using HELM in Game

You do not need Visual Studio or MDK to run HELM. Enable Experimental Mode and In-Game Scripts for your world. Install the subscribed Workshop item through the Programmable Block's **Edit → Browse Scripts**, or download the minified script from a [release](https://gitlab.com/sgkens/helm-core/-/releases).

### Option A: Direct Paste (No Tools Needed)

1. Open your ship or station's **Programmable Block** in the Terminal and click **Edit**.
2. Select everything in the code editor, then paste the entire downloaded `HelmCombined.min.cs`. For a local build, run `pwsh ./Build-Release.ps1` to generate this file; generated scripts are not tracked in Git. Use the minified output to meet the game's 100,000-character limit.
3. Click **Check Code**, then click **OK**.
4. Open the Programmable Block's **Custom Data** and paste a configuration (see [Code Examples](#production-code-examples) below).
5. Ensure your target LCDs or Cockpit blocks have matching names. The script will initialize and begin telemetry immediately.
6. With an empty Custom Data the LCDs stay blank. Run the block with the argument `starter` to write a sample layout, set `output:` to your LCD's name, then run `reload`. Run `probe` to check what part of a screen the script covers.

### Option B: Local Development

To build from source:

- Core logic lives in [`HelmCore.cs`](HelmCore.cs) (zero SE dependencies, unit-testable offline).
- Game adapter lives in [`Program.cs`](Program.cs).
- Run `pwsh ./Build-Release.ps1`, then paste the generated minified script into your Programmable Block. See [build, test and release](architecture/build-test-release.md) for the tooling and checks.

Updating a Workshop subscription does not replace code already pasted into a Programmable Block. Select the updated script again in Browse Scripts, check the code, then run `reload`; your layout remains in the block's separate Custom Data.

---

## **Configuration DSL Reference**

Configurations are written directly in the Programmable Block's **Custom Data**.

```bash
# --- Reusable Parameterized Templates ---
@template <name>(<param1>, <param2>=<default>, ...)
...
@end

@use <template_name>(<param1> = "Value", <param2> = "Value")

# --- Direct Configuration ---
@bind <name> = <source_type>("<Block or Group Name>" [, agg: <avg|sum|min|max>])

@screen <screen_name>
output: "<Target Block Name>" [, <surface_index>]
font: <Monospace | Debug | White | Red | Blue | Green | DarkBlue>
bg: <#RRGGBB | R,G,B | color_name | transparent | none>
border: <graphic | ascii | none>
border_color: <color>
layout: grid(<cols>, <rows>)
font_size: <scale>
refresh: <ticks>

# Optional TUI Snapped Sub-Panels (Defaults to 1 full-screen panel)
@panel <panel_name> at(<col>, <row> [, <span_cols>, <span_rows>]) [title: "<TITLE>"]
font: <font_name>
bg: <color>
border_color: <color>
line: "<LABEL>" bar(<bind>.<field>) [gradient(<colors>) | flat(<color>)]
line: "<LABEL>" led(<bind>.<field>) [gradient(<colors>) | flat(<color>)]
line: "<LABEL>" spark(<bind>.<field>) [gradient(<colors>) | flat(<color>)]
line: "<LABEL>" text(<bind>.<field>) [color(<conditions>, default: <color>)]
```

### Directives Reference

| Directive                         | Scope                | Description                                                                                                        | Default         | Required?  |
| --------------------------------- | -------------------- | ------------------------------------------------------------------------------------------------------------------ | --------------- | ---------- |
| `@template <name>(...)`           | Global               | Declares a reusable macro template with parameter substitution                                                     | —               | No         |
| `@use <name>(...)`                | Global               | Instantiates a template with custom arguments; templates may nest 4 levels                                         | —               | No         |
| `@bind <name> = ...`              | Global               | Binds a hardware device or block group to an alias                                                                 | —               | Yes        |
| `@screen <name>`                  | Global               | Starts a new physical screen configuration                                                                         | —               | Yes        |
| `output:`                         | `@screen`            | Physical block name and surface index (0-indexed)                                                                  | Surface 0       | **Yes**    |
| `layout:`                         | `@screen`            | Defines a 2D matrix grid: `grid(<cols>, <rows>)`                                                                   | `grid(1, 1)`    | No         |
| `@panel <name> at(...)`           | `@screen`            | Defines a snapped sub-panel at `(col, row, spanX, spanY)`                                                          | Full Screen     | No         |
| `title:`                          | `@panel`             | Panel header title with divider accent bar                                                                         | `none`          | No         |
| `font:`                           | `@screen` / `@panel` | Vanilla font atlas (`Monospace`, `Debug`, `White`, `Red`, etc.)                                                    | `Monospace`     | No         |
| `bg:` / `background:`             | `@screen` / `@panel` | Custom background color (`#HEX`, `R,G,B`, `color_name`, `none`)                                                    | `#0a0c10`       | No         |
| `border:`                         | `@screen`            | Display frame style: `graphic` (default), `none`, or `ascii`                                                       | `graphic`       | No         |
| `border_color:`                   | `@screen` / `@panel` | Frame and accent divider color (`#HEX`, `R,G,B`, `name`)                                                           | `#283c55`       | No         |
| `font_size:`                      | `@screen` / `@panel` | Text and gauge scaling factor                                                                                      | `1.0`           | No         |
| `width:`                          | `@screen`            | Content width in characters (for ASCII mode)                                                                       | `30`            | No         |
| `refresh:`                        | `@screen`            | Draw this screen at most every N game ticks (`600` = every 10 s). Saves instructions on screens that change slowly | `0` (every run) | No         |
| `alert_period:`                   | `@screen`            | Seconds for one red-alert flash cycle (`0.8s`)                                                                     | `1.6`           | No         |
| `alert_bg:`                       | `@screen`            | Colour the background flashes to on a red alert; `none` turns it off                                               | `#5b0f0f`       | No         |
| `screen_bg:`                      | `@screen`            | Background of the whole screen wherever the line is written                                                        | —               | No         |
| `panel_bg:`                       | `@screen`            | Card colour behind every panel that has no `bg:` of its own                                                        | none            | No         |
| `bg_gradient(...)`                | `@screen` / `@panel` | Gradient background; `bg_gradient_dir:` `vertical` or `horizontal`                                                 | —               | No         |
| `bg: color(...)` / `flash:`       | `@screen` / `@panel` | Background that follows a value, or flashes on a threshold                                                         | —               | No         |
| `bg_texture:`                     | `@screen`            | Built-in sprite drawn over the background (`bg_texture_color:` tints it)                                           | —               | No         |
| `gradient_mode:`                  | `@screen`            | `smooth` blends between gradient stops, `steps` picks the nearest                                                  | `smooth`        | No         |
| `orientation:`                    | `@panel`             | `vertical` lays lines out as columns (tanks and vertical bars)                                                     | `horizontal`    | No         |
| `mirror:` / `mirror_panel:`       | `@screen`            | Copy another screen (or one of its panels) to this display, scaled                                                 | —               | No         |
| `banner:` / `boot_logo:`          | `@screen`            | Show an `@banner ... @end` ASCII block full screen, or as the boot logo                                            | —               | No         |
| `boot_batch:` / `boot_calibrate:` | `@screen`            | Screens that boot together; seconds the gauges sweep 0 to 100 after boot                                           | `1` / `1.2`     | No         |
| `gap:` / `padding:`               | `@screen`            | Pixels between panels, and around the screen (`0` puts the border on the edge)                                     | `8` / about 2%  | No         |
| `line:`                           | `@screen` / `@panel` | Visual gauge (`bar`, `led`, `spark`) or text readout definition                                                    | —               | At least 1 |

---

## Syntax & Supported Features

### Modular Border Engine, Positional Titles & Gradients

HELM provides modular border and frame options for both the in-game VRage quad renderer and the offline TUI visualizer ([`helmv.ps1`](helmv.ps1)):

#### Border Styles (`border:`)

Configure borders globally per screen or locally per panel:

- `border: heavy` — Heavy reinforced industrial bulkhead box (`┏━━━━┓` / `┃    ┃` / `┗━━━━┛`).
- `border: double` — Military double-line tactical framing (`╔════╗` / `║    ║` / `╚════╝`).
- `border: single` — Clean single-line engineering border (`┌────┐` / `│    │` / `└────┘`).
- `border: rounded` — Smooth rounded aerospace cockpit corners (`╭────╮` / `│    │` / `╰────╯`).
- `border: minimal` — Divider-only frames with open side rails for ultra-dense telemetry walls.
- `border: ascii` — Strict vanilla Keen `Monospace` printable ASCII (`+----+` / `|    |` / `+----+`).
- `border: frame` and `border: corners` — In-game frame drawn with quads: all four sides, or bracket corners only.

In the game every style is drawn with quads: `graphic` and `minimal` are the top and bottom lines, `frame`, `single`, `heavy` (thicker) and `double` (a second inner frame) add the sides, `rounded` adds a circle at each corner, and `corners` draws brackets. `border_color` and `border_gradient` colour them, and `border:` on an `@panel` line frames that panel.

- `border: graphic` — Default sci-fi HUD framing with recessed slot tracks.
- `border: none` — Flush, edge-to-edge monitors without frame boundaries.

Where each style applies:

| Renderer                      | Behavior                                                                                                                                                                                                          |
| :---------------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| In-game script (`Program.cs`) | Screen-level `border:` only. `none` removes the frame, `ascii` uses the text renderer with `+----+` frames, and every other style draws the standard sprite frame. A panel-level `border:` is parsed but ignored. |
| `helmv.ps1`                   | All styles, at screen and panel level.                                                                                                                                                                            |

`border_color:`, `border_gradient:`, `title_align:` and `title_color:` work at screen and panel level in both renderers.

#### Positional Titles (`title_align:`)

Snap headers dynamically across top border frames without ragged line wrapping:

- `title_align: left` (Default) — Classic left-aligned bracket header `+-- [ TITLE ] --------+`.
- `title_align: center` — Balanced aerospace horizon title `+-------- [ TITLE ] --------+`.
- `title_align: right` — High-visibility right-aligned alert header `+-------- [ TITLE ] --+`.

#### Independent Title Typography Colors (`title_color:`)

Decouple header text color from border frame lines:

- `title_color: #fef08a` or `title_color: yellow` — Renders title text in crisp contrasting typography while borders retain their structural theme.
- `border_color: #3b82f6` — Sets solid border stroke color.

#### Procedural Multi-Stop Border Gradients (`border_gradient:`)

Apply smooth multi-stop RGB transitions across border frames:

- `border_gradient: #38bdf8, #2563eb, #1e40af` (or functional syntax `border_gradient(cyan, blue, darkblue)`).
- Procedurally interpolates border lines across color stops in both in-game VRage sprites and ANSI terminal outputs.

```bash
@screen tactical_display
output: "Bridge Main LCD", 0
border: double
border_gradient: #ef4444, #7f1d1d
title_align: center
title_color: #fca5a5
layout: grid(2, 1)

@panel weapons at(0, 0, 1, 1) title: "ORDNANCE" border: double title_align: right title_color: #ef4444
line: "TURRETS " hazard(guns.percent) gradient(#ef4444, #7f1d1d)

@panel nav at(1, 0, 1, 1) title: "PROPULSION" border: rounded title_align: center title_color: #38bdf8 border_gradient: #38bdf8, #1e40af
line: "BURN    " bar(h2.percent) gradient(#38bdf8, #60a5fa)
```

---

### Block Binding & Group Aggregations

HELM eliminates guesswork when binding blocks:

- **Single Block Binding:**

    ```bash
    @bind pwr = battery("Main Battery")
    @bind o2  = airvent("Air Vent Cockpit")
    @bind tank = tank("Hydrogen Tank 1")
    @bind jump = jumpdrive("Jump Drive Forward")
    ```

    _(Note: Helper keywords `battery`, `tank`, `airvent`, `jumpdrive`, `cargo`, and `block` are interchangeable aliases for block lookups)._

- **Group Binding with Mandatory Aggregation:**
  When querying multiple blocks in a group, HELM **strictly forbids silent defaults**—you must explicitly state how values are aggregated:

    ```bash
    # Average charge across all main batteries:
    @bind fleet_pwr = group("Main Battery Bank", agg: avg)

    # Total stored capacity:
    @bind total_pwr = group("Capacitor Array", agg: sum)

    # Find the lowest tank in the reserve:
    @bind critical_h2 = group("Hydrogen Tanks", agg: min)

    # Peak charge in the array:
    @bind peak_charge = group("Auxiliary Cells", agg: max)
    ```

- **Heterogeneous & Multi-Target Group Aggregation:**
  You can bind multiple distinct block groups or blocks together under a single aggregated source without crashes:
    ```bash
    # Mix batteries, reactors, and solar arrays into a single unified power telemetry stream:
    @bind power_grid = group("Main Batteries", "Aux Reactors", "Solar Arrays", agg: avg)
    ```
    Valid aggregation operators: `avg`, `sum`, `min`, `max`.

### Supported Hardware & Telemetry Fields

HELM reads telemetry properties natively from standard vanilla blocks with safe fallbacks:

| Block Type                                     | Ingame API Source                     | Supported Fields | Status / Fallback Behavior  |
| ---------------------------------------------- | ------------------------------------- | ---------------- | --------------------------- |
| **Batteries** (`IMyBatteryBlock`)              | `CurrentStoredPower / MaxStoredPower` | `percent`        | Native ratio                |
| **Jump Drives** (`IMyJumpDrive`)               | `CurrentStoredPower / MaxStoredPower` | `percent`        | Native ratio                |
| **Gas Tanks** (`IMyGasTank` — O2 & H2)         | `FilledRatio`                         | `percent`        | Native ratio                |
| **Air Vents** (`IMyAirVent`)                   | `GetOxygenLevel()`                    | `percent`        | Native ratio                |
| **Cargo Containers** (`IMyCargoContainer`)     | `CurrentVolume / MaxVolume`           | `percent`        | Native ratio                |
| **Nuclear Reactors** (`IMyReactor`)            | `CurrentOutput / MaxOutput`           | `percent`        | Native ratio                |
| **Solar Panels** (`IMySolarPanel`)             | `CurrentOutput / MaxOutput`           | `percent`        | Native ratio                |
| **Wind Turbines** (`IMyWindTurbine`)           | `CurrentOutput / MaxOutput`           | `percent`        | Native ratio                |
| **Hydrogen Engines** (Generic Power Source)    | `CurrentOutput / MaxOutput`           | `percent`        | Output load ratio           |
| **Doors & Airlocks** (`IMyDoor`)               | `OpenRatio * 100.0`                   | `percent`        | Open percentage (0%=Closed) |
| **Thrusters** (`IMyThrust`)                    | `CurrentThrust / MaxEffectiveThrust`  | `percent`        | Current thrust output %     |
| **Wheel Suspensions** (`IMyMotorSuspension`)   | `Brake` / `SteeringOverride`          | `percent`        | Brake / mechanical load     |
| **Ship Connectors** (`IMyShipConnector`)       | `Status`                              | `percent`        | Connected/Locked (100%)     |
| **Generic Functional Block** (`IMyFunctional`) | `Enabled`                             | `percent`        | 100% (Enabled) / 0% (Off)   |

### Display Line Functions & Smart Sprite Patterns

HELM supports multiple visual primitives, all rendered with hardware-accelerated vanilla textured quads:

#### Fluid Damped Bars (`bar`)

Draws a sleek, anti-aliased geometric bar on a recessed dark track. Features **aerospace fluid damping (springs)** so needles and bars glide smoothly instead of snapping abruptly:

- **Flat Color:**
    ```bash
    line: "O2 " bar(o2.percent) flat(cyan)
    ```
- **Multi-Stop Color Gradient:**
  Picks the active color stop based on the percentage (e.g. green → yellow → red):
    ```bash
    line: "PWR" bar(pwr.percent) gradient(red, yellow, green)
    line: "CRG" bar(cargo.percent) gradient(green, yellow, red)
    ```

#### Segmented LED Chiclets (`led`)

Draws a bank of 10 discrete illuminated rectangular chiclets—perfect for heavy industrial loaders, miners, and military consoles:

```bash
line: "SHLD" led(shield.percent) gradient(red, yellow, cyan)
line: "CAPS" led(cap_bank.percent) flat(lime)
```

#### Diagonal Hazard Caution Stripes (`hazard` / `pattern: hazard`)

Draws angled 30° diagonal caution hazard stripes using hardware-accelerated rotated quads. Ideal for reactor overloads, jump drive spin-up, and warhead arming status:

```bash
line: "WARN" hazard(core_temp.percent) flat(yellow)
line: "OVER" bar(overload.percent) pattern(hazard) gradient(yellow, red)
```

#### Directional Chevron Gauges (`chevron`)

Draws directional progress arrowheads (`> > > >`) using vanilla `Triangle` sprites. Perfect for atmospheric ascent velocity, thruster output, and jump spooling:

```bash
line: "THRN" chevron(main_thrust.percent) gradient(blue, cyan, white)
line: "JUMP" chevron(jump_charge.percent) flat(magenta)
```

#### Discrete Dot Matrix Arrays (`dots`)

Draws discrete circular or square point indicators for battery cell health, magazine rounds, and ammunition counts:

```bash
line: "CELL" dots(power_cells.percent) gradient(red, yellow, green)
line: "AMMO" dots(magazine_count.percent) flat(yellow)
```

#### Trend Sparklines (`spark` or `sparkline`)

Draws an 8-column historical telemetry graph showing trend history directly on your bridge display:

```bash
line: "DRAIN" spark(pwr.percent) flat(cyan)
line: "ORE_RT" spark(cargo.percent) gradient(green, yellow, red)
```

#### Numeric Readouts & Threshold Colors (`text`)

Displays formatted values with rule-based conditional coloring:

```bash
line: "CARGO" text(cargo.percent) color(cargo.percent > 90 : red, cargo.percent > 75 : yellow, default: white)
line: "HYDRG" text(h2.percent) color(h2.percent < 20 : red, default: cyan)
```

_(Note: A `default:` fallback color is strictly required on all `color()` directives)._

#### Centered Balance Charts (`balance` / `horizon` / `centered`)

Draws a bidirectional gauge centered at 50%. Supply a normalized 0–100 value: below 50 extends left, above 50 extends right. This is not a signed attitude indicator; gyro pitch/roll fields are control commands, not measured ship orientation.

```bash
@bind trim = dynamic("trim", default: 50)
line: "TRIM " balance(trim.val) gradient(cyan, white, cyan)
line: "LAT  " pattern(balance) bar(th_lat.percent) gradient(red, yellow, green, yellow, red)
```

- **Sprite Mode:** Recessed track with a center divider and quads extending left (`< 50%`) or right (`> 50%`) from the midpoint.
- **ASCII Monospace Mode:** Strict SE fixed-width character representation: `PITCH [---<<<|----] 35%`.

#### Toggle Switches (`toggle` / `switch`)

Interactive visual toggle switch displaying distinct sliding knob positions and clear ON/OFF states:

```bash
line: "MAGNETS" toggle(containment.enabled) flat(cyan)
line: "LIGHTS " switch(interior_lights.status) flat(yellow)
```

- **Sprite Mode:** Recessed pill track with sliding circular knob quad that illuminates green/cyan when active (>= 50%) and dims dark gray when off.
- **TUI / Text Mode:** Formats cleanly as `[====O] ON` / `[O====] OFF`.

#### Checkboxes (`checkbox` / `check`)

Status indicator box for automated protocols, subsystem interlocks, and security latches:

```bash
line: "ISOLATE" checkbox(reactor.scram) flat(red)
line: "RADAR_TR" check(targeting.enabled) flat(cyan)
```

- **Sprite Mode:** Outer square boundary frame with an illuminated inner check quad when active.
- **TUI / Text Mode:** Formats as `[X] ACTIVE` / `[ ] READY `.

#### Online/Offline Status Badges (`status` / `badge` / `state`)

Rounded badge indicator displaying tri-state system connectivity:

```bash
line: "GRID_STS" status(shields.status) flat(lime)
line: "REACTOR " badge(core.online) flat(green)
```

- **States:** `ONLINE` (>= 75%), `STANDBY` (25% - 74%), `OFFLINE` (< 25%).
- **TUI / Text Mode:** Formats as `[● ONLINE ]` / `[◐ STANDBY]` / `[○ OFFLINE]` in Unicode, and `[# ONLINE ]` / `[: STANDBY]` / `[. OFFLINE]` in strict SE ASCII.

#### Ammo Magazine Counters (`ammo` / `pips`)

Discrete pip cartridge readout representing remaining ordnance, rounds, or torpedos:

```bash
line: "PDC_AMMO" ammo(gatling.ammo) flat(yellow)
line: "MISSILES" pips(launch_tubes.percent) gradient(red, yellow, cyan)
```

- **Sprite Mode:** 8 discrete segmented pip cartridges illuminated in sequence with active color gradient.
- **TUI / Text Mode:** `[▮▮▮▮▮▯▯▯]` (Unicode) and `[||||....]` (Strict SE ASCII).

#### Dynamic Charging & Discharging Flow Indicators (`flow` / `rate` / `spinner`)

Real-time animated flow rate and charging/discharging dynamic spinner for battery banks, reactors, and fuel transfer manifolds:

```bash
line: "FLOW    " flow(reactor.flow) flat(cyan)
line: "NET_RATE" rate(sorters.rate) flat(orange)
```

- **States:** Charging (`CHG`, > 55%), Discharging (`DSC`, < 45%), Standby (`STBY`, 45% - 55%).
- **Sprite Mode:** Dynamic rotated directional indicator (`Triangle`) pointing up during recharge and down during discharge.
- **TUI / Text Mode:** Rotating animated spinner: `⠋ ▲▲ CHG` vs `⠙ ▼▼ DSC` vs `●● STBY` (Unicode) and `| ^^ CHG` vs `/ vv DSC` vs `== STBY` (Strict SE ASCII).

#### Dynamic Label & Title Formatting (`{percent}`, `{val}`)

Any line label or panel title can interpolate live percentage or raw numeric values directly:

```bash
line: "PWR [{percent}]" bar(pwr.percent) gradient(red, yellow, green)
line: "CHARGE {val}%" dots(cells.percent) flat(cyan)
```

#### Liquid Tanks and Vertical Bars (`tank` / `tube` / `bucket`)

A tube that fills from the bottom, with a wave line of characters on the surface and rising bubbles. A panel that contains a tank, or has `orientation: vertical`, lays its lines out as columns. A panel spanning the screen is the large size; a grid of panels gives medium ones:

```bash
@panel fuel at(0, 0, 1, 1) title: "H2"
line: "H2" tank(h2.percent) gradient(#1e3a8a, #38bdf8)
```

See `tank_fullscreen`, `tank_trio` and `vertical_bars`.

#### Sizes, Colours and Conditions (options for every gauge)

```bash
line: "POWER" bar(pwr.percent) size(large) gradient(#ef4444, #eab308, #22c55e) track(#111827) label_color(#94a3b8)
line: "BATT " bar(pwr.percent) color(pwr.percent < 25 : red/orange, default: green/cyan)
line: "CARGO" bar(cargo.percent) gradient(#22c55e, #ef4444) color_by(pwr.percent)
```

- `size(medium|large)`: the row takes 2 or 4 shares of its panel (normal is 1) and the bar grows with it.
- `color(...)` rules, then `gradient(...)`, then `flat(...)` decide the colour of every gauge kind. A rule can answer with a gradient written `a/b`. Inside `color(...)` use colour names for these; a `#` after a space starts a comment.
- `color_by(bind.field)`: colour follows another value than the one the gauge shows. `track(color)` colours the empty part, `label_color(color)` the label.

#### Dial (`dial` / `gauge` / `arc`)

A half circle of segments that light up with the value, with a needle and a hub. A gradient runs along the arc by position:

```bash
line: "POWER" dial(pwr.percent) size(large) gradient(#22c55e, #eab308, #ef4444) segments(20)
```

#### Bar Dressing, Segments and Line Details

```bash
line: "H2   " bar(h2.percent) ticks(5) marker(25, 80) outline(#94a3b8)   # division lines, threshold ticks, a frame
line: "CARGO" bar(cargo.percent) dir: left                                # fills from the right end
line: "LED  " led(pwr.percent) segments(20)                              # 2 to 40 segments (dots and dial too)
line: "TREND" spark(h2.percent) points(24)                               # 2 to 40 columns
line: "BATT " bar(pwr.percent) value(off) icon(IconEnergy) blink(pwr.percent < 15)
```

`value(off)` hides the value text, `icon(Name)` draws a built-in sprite before the label (`scan` lists the names), and `blink(...)` flashes a line while a condition holds, in any colour. See `gauge_lab`.

#### Rate, ETA and Trend

`text(batt.rate)` is the change per minute (`-2.4%/min`), `text(batt.eta)` the time until empty or full at that rate (`empty 18m 20s`), and `text(batt.trend)` an arrow (`^` `v` `=`). Sampled every 5 s and smoothed.

#### Item Amounts, Contents and LCD Text

- `bar(cargo.ingot:Iron) max(5000)`: amount of one item (`kind:Subtype`); `max(N)` or `max(bind.field)` is the full amount.
- `text(cargo.top1)`: the largest item held, `top2` the next and so on (refreshed every 15 s).
- `text(notes.line1)`: a line of the text on a text panel or LCD that is not a HELM screen (read every 2 s). Only text can be read; the game gives scripts no access to another screen's picture.
- `scope: all` on a bind also reads blocks on ships docked on a connector.

#### Mirroring Screens

`mirror: "Bridge"` on a screen copies another screen's drawing to this display, scaled to fit; `mirror_panel: "POWER"` copies one panel. Only screens of the same Programmable Block can be mirrored.

### Emergency Hazard Alert Pulsing

Any system that enters an emergency state (resolving to `red` via gradients or threshold rules) **automatically pulses** between emergency crimson and bright alert red in sync across all screens on your ship or station.

### Color Science & Hex / RGB Parsing

HELM supports full 24-bit color fidelity across all directives (`flat(...)`, `gradient(...)`, `color(...)`, `bg:`, and `border_color:`):

- **Hex 6-Digit:** `#RRGGBB` (e.g. `#38bdf8`, `#22c55e`, `#ef4444`)
- **Hex 3-Digit:** `#RGB` (e.g. `#0fc`, `#f80`, `#f43`)
- **RGB Triples:** `R, G, B` (e.g. `255, 128, 0` or `14, 18, 24`)
- **Named Keywords:** `red`, `green`, `lime`, `yellow`, `orange`, `cyan`, `blue`, `magenta`, `purple`, `white`, `black`, `gray`, `darkgray`.
- **Transparency:** `transparent` or `none` (omits background quad to let underlying LCD textures show through).

### Vanilla Font Family Selection

Control the typography per-screen or per-panel using Keen's native bitmap font atlases:

- `font: Monospace` (Default fixed-pitch font for console readouts)
- `font: Debug` (Crisp military HUD terminal font)
- `font: White`, `Red`, `Green`, `Blue`, `DarkBlue` (Vanilla colored font atlases)
- `font: UrlNormal`, `UrlHighlight`

### CLI/TUI Grid Layout Engine & Sub-Panels

Bring curses/TUI multi-panel displays to Space Engineers. Define a matrix grid and snap sub-panels with coordinate isolation:

```bash
@screen bridge_matrix
output: "Bridge Main LCD", 0
layout: grid(2, 2)
bg: #0a0d14
border_color: #2563eb

# Quadrant (0, 0): Upper-Left Power Panel
@panel pwr at(0, 0, 1, 1) title: "REACTOR CORE"
font: Debug
line: "R1" bar(r1.percent) gradient(green, yellow, red)

# Quadrant (0, 1): Lower-Left Life Support Panel
@panel o2 at(0, 1, 1, 1) title: "LIFE SUPPORT"
line: "O2" bar(o2.percent) flat(cyan)

# Right Column (1, 0): Spans 2 Rows vertically
@panel nav at(1, 0, 1, 2) title: "PROPULSION & JUMP"
font: Monospace
line: "HYDR" bar(h2.percent) gradient(green, yellow, red)
line: "JUMP" led(jump.percent) flat(magenta)
line: "HIST" spark(h2.percent) flat(cyan)
```

- Each panel calculates its sub-viewport automatically with uniform gutters and margins.
- Panels can override `font:`, `font_size:`, `bg:`, and `border_color:`.
- Includes an optional title header with a distinct accent divider line.

### Parameterized Template System

Define an entire screen or subsystem once, then stamp it out across multiple LCDs by simply passing parameter overrides:

```bash
@template engine_pod($screen, $batt_group, $tank_group)
@bind pwr_$screen = group("$batt_group", agg: avg)
@bind h2_$screen  = group("$tank_group", agg: avg)
@screen $screen
output: "$screen LCD", 0
font: Debug
line: "PWR" bar(pwr_$screen.percent) gradient(green, yellow, red)
line: "H2 " bar(h2_$screen.percent) gradient(green, yellow, red)
@end

# Rapid deployment with single-line calls:
@use engine_pod(screen="Port Nacelle", batt_group="Port Batts", tank_group="Port H2")
@use engine_pod(screen="Starboard Nacelle", batt_group="Starboard Batts", tank_group="Starboard H2")
```

### Built-In Predefined Template Gallery

HELM ships with production-grade templates built right into the parser. You can use them immediately with zero boilerplate:

#### `fighter_hud`

Compact 2x1 cockpit layout with propulsion and storage sub-panels:

```bash
@use fighter_hud(screen="Cockpit Display", pwr="Fighter Batteries", h2="Hydrogen Tanks", cargo="Cargo Containers")
```

#### `reactor_matrix`

Dual 1x2 industrial power grid monitor (Reactors + Battery Bank):

```bash
@use reactor_matrix(screen="Reactor LCD", reactors="Main Reactors", batteries="Main Batteries")
```

#### `bridge_status`

Full 2x2 capital bridge display (Power Grid, Life Support, Navigation Jump, Logistics):

```bash
@use bridge_status(screen="Bridge LCD", power="Batteries", o2="Air Vents", cargo="Cargo", jump="Jump Drives")
```

#### `mining_ops`

Industrial 1x2 ore hold capacity and drill status monitor:

```bash
@use mining_ops(screen="Drill LCD", cargo="Cargo Containers", drills="Drills")
```

#### `airlock`

Atmosphere and dual-door security status display:

```bash
@use airlock(screen="Airlock Console LCD", vent="Airlock Vent", inner_door="Inner Security Door", outer_door="Outer Security Door")
```

#### `cargo_overview`

Comprehensive 4-quadrant logistics matrix (Ores, Ingots, Components, Munitions):

```bash
@use cargo_overview(screen="Cargo Manifest LCD", ores="Ore Cargo", ingots="Ingot Silos", comps="Component Bay", ammo="Ammo Container")
```

#### `battery_monolith`

Dedicated full-screen monolithic battery monitor with directional vertical meniscus fill:

```bash
@use battery_monolith(screen="Battery Wall LCD", batt="Main Battery Bank")
```

#### Modular Border & Frame Templates (`layouts/templates.txt`)

External template definitions loaded dynamically via `[HelmParser]::LoadTemplateFile(...)` or reusable directly:

- `heavy_command(screen="...", core="...", batt="...")`: Heavy reinforced industrial bulkhead frame with centered title and multi-stop blue gradient.
- `double_tactical(screen="...", guns="...", ammo="...")`: Double-line crisp military tactical border with right-aligned title and amber/red munitions hazard bars.
- `rounded_avionics(screen="...", gyro="...", fuel="...")`: Smooth curved aerospace cockpit border with centered title and zero-center pitch balance horizon gauge.
- `minimal_telemetry(screen="...", cargo="...")`: High-density divider frame with open side rails for maximum data throughput.

```bash
@use heavy_command(screen="Command Screen", surf=0, core="Reactor*", batt="Battery*")
@use double_tactical(screen="Tactical HUD", surf=0, guns="Gatling*", ammo="Ammo*")
@use rounded_avionics(screen="Flight Deck", surf=0, gyro="Gyro*", fuel="Hydrogen*")
@use minimal_telemetry(screen="Logistics Monitor", surf=0, cargo="Cargo*")
```

### Staggered Boot Sequence & Aerospace Diagnostics

When the script starts, each screen plays a short cold-start boot: the startup logo with a colour gradient, a header message and diagnostic lines that appear one by one, then the live display. Screens power on one after another. The run argument `boot` plays it again; editing the layout does not. `boot_duration: 0` skips it for a screen. Screens without boot settings use the defaults below.

After the boot, each screen runs a calibration sweep: its gauges run from 0 to 100 and settle on the live value (`boot_calibrate: <seconds>`, default `1.2`; `0` or `off` skips it). State gauges and text are not swept.

- `boot_msg: "..."`: Custom header message displayed upon power-on.
- `boot_stagger: <ticks>`: Stagger interval in game ticks between sequential screens across your ship or station.
- `boot_calibrate: <seconds | off>`: Length of the calibration sweep after the boot (default `1.2s`).
- `boot_batch: <count>`: How many screens power on together before the next stagger step (default `1`). With `boot_batch: 4` and `boot_stagger: 30`, four screens start, then the next four half a second later. At most four boot frames are drawn per run, so large setups stay within the instruction limit.
- `boot_duration: <seconds | ticks>`: Total boot sequence duration. `3s` is seconds; a bare number is game ticks (60 per second), so `180` is also 3 seconds. `0` disables the boot. Default `2.5s`.
- `boot_color: <color>`: Solid ANSI/SE color for the startup diagnostic logo.
- `boot_gradient(stop1, stop2, ...)`: Multi-stop procedural vertical color gradient across the ASCII banner lines (e.g. `boot_gradient(cyan, blue, magenta)`).
- `boot_log: "..."`: Streaming trailing diagnostic log messages (repeating directive). If none are defined, HELM defaults to standard aerospace diagnostics.

```bash
@screen bridge_matrix
output: "Bridge Main LCD", 0
layout: grid(2, 2)
boot_msg: "FLEET COMMAND HELM INITIALIZED"
boot_duration: 3s
boot_gradient(cyan, blue, magenta)
boot_log: "initiating fleet command bus..."
boot_log: "polling bridge telemetry matrix..."
boot_log: "synchronizing reactor core..."
boot_log: "warp flight deck ready"
boot_log: "helm-core online -- FLEET COMMAND READY"
```

### Modular Layout Library (`layouts/default/`)

HELM provides an extensive catalog of pre-tuned, production-grade `.txt` display layouts ready for immediate deployment in-game or visual preview in [`helmv.ps1`](helmv.ps1):

| Layout File                    | Profile   | Description                                                                                                 |
| :----------------------------- | :-------- | :---------------------------------------------------------------------------------------------------------- |
| `planetary_base.txt`           | 2x3 Grid  | Outpost master control: solar, wind, battery bank, life support, excavation, ore processing, base defense.  |
| `command_carrier.txt`          | 3x2 Grid  | Flagship capital carrier: fleet power, hangar ops, weapons battery, point defense, propulsion, command net. |
| `survival_essentials.txt`      | 2x2 Grid  | Clean 4-quadrant survival HUD: power status, life support, cargo & supply, system health.                   |
| `deep_space_explorer.txt`      | 2x2 Grid  | Long-range survey HUD: jump drive range, fuel reserves, sensors & survey, docking.                          |
| `mining_fleet_coordinator.txt` | 2x3 Grid  | Multi-ship coordinator with dynamic IGC binds for drone miners and shared refinery throughput.              |
| `atmospheric_lander.txt`       | 2x2 Grid  | Planetary dropship HUD: pitch attitude balance gauge, atmo thrusters, landing gear, surface drills.         |
| `bridge_matrix.txt`            | 2x2 Grid  | Capital ship 4-quadrant bridge display: power grid, life support, navigation, cargo stores.                 |
| `tactical_defense.txt`         | 2x2 Grid  | Combat fire control: railgun capacitor, turret ammunition, missile status, decoy arrays.                    |
| `thruster_balance.txt`         | 2x2 Grid  | 6-axis directional propulsion and hydrogen distribution balance HUD.                                        |
| `split_flight_deck.txt`        | 2x1 Grid  | Asymmetric flight instrumentation for pilot and co-pilot cockpits.                                          |
| `assembler_monolith.txt`       | 1x1 Grid  | Full-screen manufacturing and queue throughput monitor.                                                     |
| `refinery_monolith.txt`        | 1x1 Grid  | Smelting throughput, yield enhancement, and ingot stores monitor.                                           |
| `reactor_monolith.txt`         | 1x1 Grid  | Monolithic uranium burn rate, temperature, and reactor core matrix.                                         |
| `battery_monolith.txt`         | 1x1 Grid  | Full-surface monolithic battery bank with vertical liquid fill.                                             |
| `cargo_monolith.txt`           | 1x1 Grid  | High-capacity bulk ore hopper fill gauge with dynamic meniscus pulse.                                       |
| `jump_drive_spool.txt`         | 1x2 Grid  | Jump drive spool capacitor, recharge countdown, and jump readiness.                                         |
| `medical_life_support.txt`     | 1x2 Grid  | Pressurization vents, O2/H2 generation, and medical bay status.                                             |
| `airlock_security.txt`         | 1x2 Grid  | Dual-door security airlock with atmospheric pressurization monitor.                                         |
| `rover_cockpit.txt`            | 2x2 Grid  | Wheeled planetary rover: powertrain, suspension friction, cargo hold.                                       |
| `mining_rig.txt`               | 1x2 Grid  | Stationary drill tower vibration, ore capacity, and sorter drain status.                                    |
| `fighter_hud.txt`              | 2x1 Grid  | Compact fighter cockpit display: battery reserves, hydrogen burn, cargo fill.                               |
| `hazard_alert.txt`             | 1x1 Grid  | Overload containment alert with synchronized emergency flashing.                                            |
| `dynamic_telemetry.txt`        | 1x2 Grid  | Inter-script IGC display server test layout demonstrating dynamic binding through IGC.                      |
| `tank_fullscreen.txt`          | 1x1 Grid  | One liquid tube filling the whole screen, wave line and bubbles, bottom-to-top gradient.                    |
| `tank_trio.txt`                | 3x1 Grid  | Medium tanks: hydrogen, oxygen and power side by side.                                                      |
| `vertical_bars.txt`            | 1x1 Grid  | A vertical panel: value on top, bar in the middle, label below.                                             |
| `bar_sizes.txt`                | 1x1 Grid  | A large, a medium and a normal bar in one panel (`size(...)`), with `track` and `label_color`.              |
| `combo_dashboard.txt`          | 3x2 Grid  | Tank column, sized bars and condition-driven gradients in one layout.                                       |
| `background_lab.txt`           | 6 screens | Gradient, two-tone, see-through, alert, value-driven, flashing and textured backgrounds.                    |
| `startup_banner.txt`           | 2 screens | The same ASCII art as a full-screen banner and as a boot logo.                                              |
| `top_contents.txt`             | 1x1 Grid  | The five largest items of a cargo group, ranked.                                                            |
| `cargo_contents.txt`           | 1x1 Grid  | Amounts of named items and a reactor's fuel (`kind:Subtype`).                                               |
| `fighter_ammo.txt`             | 1x1 Grid  | Ammunition and missiles against a full load with `max(N)`.                                                  |
| `projector_welder.txt`         | 1x1 Grid  | Build progress of a projected blueprint and welder activity.                                                |
| `rover_suspension.txt`         | 1x1 Grid  | Height, strength, power, friction and brake of a rover's wheels.                                            |
| `dynamic_strings.txt`          | 1x1 Grid  | Text values on dynamic binds, replaced over IGC or the run command.                                         |
| `template_nested.txt`          | 1x1 Grid  | A template that invokes another template.                                                                   |
| `missing_binds.txt`            | 1x1 Grid  | Binds that match no block: every value reads as an error.                                                   |

The folder holds more layouts than this table lists (65 in all); the [presets gallery](docs/content/presets-gallery.md) is generated from the files and lists every one.

---

## Production Code Examples

### Example 1: Fighter Cockpit Display

A compact single-screen layout designed for a cockpit keyboard or center console (Surface 0). Features fluid-damped bars and a real-time battery drain sparkline:

```bash
@bind pwr  = battery("Fighter Battery")
@bind h2   = tank("H2 Tank Small")
@bind o2   = airvent("Cockpit Vent")
@bind jump = jumpdrive("Micro Jump Drive")

@screen cockpit_hud
output: "Fighter Cockpit", 0
border: none
font: Debug
bg: #090c10
font_size: 0.9
line: "BAT" bar(pwr.percent) gradient(red, yellow, green)
line: "PWR" spark(pwr.percent) flat(cyan)
line: "H2 " bar(h2.percent) gradient(red, yellow, cyan)
line: "O2 " text(o2.percent) color(o2.percent < 20 : red, default: white)
line: "JMP" bar(jump.percent) flat(magenta)
```

### Example 2: Industrial Mining Outpost & Cargo Bay

Monitors heavy battery banks and multiple cargo containers with discrete LED chiclets:

```bash
@bind power_grid = group("Station Batteries", agg: avg)
@bind o2_reserve = group("Oxygen Tanks", agg: avg)
@bind main_cargo = cargo("Ore Hopper Large")

@screen station_overview
output: "Bridge LCD 1", 0
border: graphic
font_size: 1.0
line: "GRID" bar(power_grid.percent) gradient(red, yellow, green)
line: "O2  " bar(o2_reserve.percent) flat(cyan)
line: "ORE " led(main_cargo.percent) gradient(green, yellow, red)
line: "STAT" text(main_cargo.percent) color(main_cargo.percent >= 95 : red, main_cargo.percent >= 80 : yellow, default: green)
```

### Example 3: Capital Ship Multi-Screen Bridge

Renders across two separate LCD panels using multi-tick round-robin scheduling without instruction spikes:

```bash
# --- Telemetry Bindings ---
@bind batt_bank = group("Batt Bank Main", agg: avg)
@bind jump_core = group("Fleet Jump Drives", agg: min)
@bind h2_fuel   = group("H2 Tanks", agg: avg)
@bind life_supp = airvent("Bridge Vent")
@bind hold_vol  = cargo("Main Cargo Hold")

# --- Screen 1: Power & Propulsion ---
@screen helm_eng
output: "Helm Console LCD Left", 0
border: graphic
font_size: 0.95
line: "PWR " bar(batt_bank.percent) gradient(red, yellow, green)
line: "HIST" spark(batt_bank.percent) flat(cyan)
line: "H2  " bar(h2_fuel.percent) gradient(red, yellow, cyan)
line: "JUMP" led(jump_core.percent) flat(magenta)

# --- Screen 2: Life Support & Storage ---
@screen helm_logistics
output: "Helm Console LCD Right", 0
border: graphic
font_size: 0.95
line: "O2  " bar(life_supp.percent) flat(cyan)
line: "HOLD" bar(hold_vol.percent) gradient(green, yellow, red)
line: "ALRT" text(hold_vol.percent) color(hold_vol.percent > 90 : red, default: white)
```

---

### In-Game Programmable Block Arguments

You can trigger the Programmable Block via cockpit toolbars, sensor blocks, or timer blocks by passing arguments into `Main`:

| Argument                                   | Description                                                                                          | Use Case                                                          |
| ------------------------------------------ | ---------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------- |
| `reload`                                   | Re-parses Custom Data immediately                                                                    | Quickly test configuration changes without recompiling the script |
| `refresh`                                  | Re-scans all physical blocks and groups on the grid                                                  | Call after docking, undocking, or welding new blocks              |
| `boot`                                     | Plays the boot sequence again on every screen                                                        | Check the boot logo and calibration sweep                         |
| `starter`                                  | Writes a sample layout into an empty Custom Data and loads it (never overwrites)                     | First run on a new block                                          |
| `probe`                                    | Draws a test pattern for 20 s on the block's screen and every configured screen                      | Check what part of a real screen the script covers                |
| `console on` / `console off`               | Shows or hides the console on the block's own screen                                                 | Give the block's screen back to the game                          |
| `names`                                    | Writes every block name and group name of the grid to the end of Custom Data as comment lines (once) | Copy exact names into `@bind` lines                               |
| `scan` (also `diagnose`, `test`)           | Prints a report of binds, surfaces, instruction use and the sprite names for `bg_texture`            | Find why a gauge shows `ERR`                                      |
| `alert red` / `alert off` / `alert toggle` | Red alert: backgrounds flash to `alert_bg` and red gauges flash                                      | Wire to a sensor or hotbar                                        |
| `set <var> <val>`                          | Sets an in-memory dynamic variable                                                                   | Feed a value or text to a `dynamic` bind                          |

### Offline TUI Simulator & Visualizer (`helmv.ps1` / `Test-HelmLayout.ps1`)

You can design, test, and export your HELM displays completely offline without launching Space Engineers using the terminal visualizer [`helmv.ps1`](helmv.ps1):

```powershell
# Preview any preset or layout file:
pwsh ./helmv.ps1 -Preset atmospheric_lander
pwsh ./helmv.ps1 -Preset command_carrier
pwsh ./helmv.ps1 -Preset planetary_base

# 1b. Preview one screen of a multi-screen layout (name or number):
pwsh ./helmv.ps1 background_lab -ScreenName Alert

# Swap border styles dynamically (-Border):
pwsh ./helmv.ps1 -Preset atmospheric_lander -Border rounded
pwsh ./helmv.ps1 -Preset command_carrier -Border double
pwsh ./helmv.ps1 -Preset planetary_base -Border heavy
pwsh ./helmv.ps1 -Preset survival_essentials -Border minimal

# Position titles across header frames (-TitleAlign):
pwsh ./helmv.ps1 -Preset atmospheric_lander -Border rounded -TitleAlign center
pwsh ./helmv.ps1 -Preset command_carrier -Border double -TitleAlign right

# Simulate Space Engineers physical hardware screen profiles (-ScreenProfile):
pwsh ./helmv.ps1 -Preset atmospheric_lander -ScreenProfile CockpitSmall  # 512x256 2:1 instrument HUD
pwsh ./helmv.ps1 -Preset command_carrier -ScreenProfile WideLCD         # 1024x512 2:1 ultra-wide screen
pwsh ./helmv.ps1 -Preset planetary_base -ScreenProfile TextPanel        # 512x512 1:1 wall LCD
pwsh ./helmv.ps1 -Preset thruster_balance -ScreenProfile CornerLCD      # 512x128 4:1 slanted strip

# Space Engineers visual font simulation (-FontMode):
# (Requires BigBlue Terminal or Share Tech Mono in tools/fonts/ on supported terminals)
pwsh ./helmv.ps1 -Preset bridge -FontMode Monospace    # Simulates SE Debug/Monospace bitmap font
pwsh ./helmv.ps1 -Preset bridge -FontMode Standard     # Simulates SE white_shadow default font

# Test custom inline DSL or external template files:
pwsh ./helmv.ps1 -CustomPreset '@use rounded_avionics(screen="Flight Deck", gyro="Gyro*", fuel="Hydrogen*")'
pwsh ./helmv.ps1 -CustomPreset ./layouts/templates.txt

# Match exact in-game Programmable Block refresh cadences:
pwsh ./helmv.ps1 -Preset bridge -UpdateFrequency 1x    # 60 FPS (~16.6ms / tick)
pwsh ./helmv.ps1 -Preset bridge -UpdateFrequency 10x   # 6 FPS (~166ms / tick)
pwsh ./helmv.ps1 -Preset bridge -UpdateFrequency 100x  # 0.6 FPS (~1660ms / tick)

# Toggle telemetry formatting: Percentage vs. Real Engineering Units (-Measurement / -Percent):
pwsh ./helmv.ps1 -Preset battery_monolith -Measurement   # Shows MW, GW, kW, kL, m/s, kN, rds
pwsh ./helmv.ps1 -Preset dynamic_telemetry -Percent      # Forces standard percentage (xx.x%)
pwsh ./helmv.ps1 -Preset command_carrier -Interactive    # Press [M]easurements / [P]ercent live!

# Inspect hardware sprite allocations and copy to clipboard:
pwsh ./Test-HelmLayout.ps1 -Preset atmospheric_lander -ShowSprites -ShowStats -CopyToClipboard
```

**In-game update rate.** The script chooses its own rate. It runs at `Update10` (6 runs a second, one normal telemetry screen per run) while a boot, a changing value, an easing gauge or an alert is active, and drops to `Update100` after every screen has been idle for a few runs. The per-screen `refresh:` setting limits normal redraws within that round-robin schedule; boot frames are separate.

**VRage Hardware Sprite Buffer Inspector (`-ShowSprites`):**
Passing `-ShowSprites` appends a real-time hardware telemetry card showing the exact `MySprite` budget that Space Engineers would allocate for that screen layout:

- **Total Allocated `MySprite` structs**: Exact quad and texture footprint pre-allocated in `ScreenBuffer`.
- **In-Place Mutated Quads / Tick**: Models updates to existing sprite values (`Size.X`, `Color`, `Data`); it does not measure total game allocations.
- **Per-Tick GC Allocation**: reported by the simulator as 0 KB. This is a model of the intended design, not a measurement from the game.
- **Laser Alignment**: Bounded to the exact card width (41 columns in single-column mode, 84 columns in multi-column mode) with zero border drift.

**Game-Accurate Animation & Fluid Damping:**
[`helmv.ps1`](helmv.ps1) mirrors the in-game script's exact aerospace fluid damping equation (`displayValue += (target - displayValue) * 0.35f`). Gauges and needles glide smoothly toward their targets rather than jumping abruptly, accurately modeling physical cockpit response at 60 Hz (`Update1`), 6 Hz (`Update10`), or 0.6 Hz (`Update100`).

**What the Preview Does Not Draw:**
[`helmv.ps1`](helmv.ps1) is a text rendering. After each run it prints a `helmv:` note for anything in the layout it cannot draw (tanks, vertical panels, `size(...)`, `label_color` / `track` / `color_by`, gradient and texture backgrounds, mirrors, banners, panel positions) and for parser warnings. A layout with several screens shows the first one; `-ScreenName` picks another. See [helmv-cli](docs/content/helmv-cli.md).

**Tear-Free Terminal Guarantee:**
[`helmv.ps1`](helmv.ps1) adheres strictly to the **Padding Rule**—calculating exact character bounding boxes _before_ encoding ANSI color sequences. Multi-column grid rendering compresses bridge dashboards from 28 lines down to 18 lines, comfortably fitting standard 24/25-row terminals. In interactive environments, it switches to the VT100 Alternate Screen Buffer (`$ESC[?1049h`), uses line erase sequences (`$ESC[K`), and display clears (`$ESC[J`), completely eliminating screen scrolling, line drifting, and duplicate text overlapping.

**Space Engineers Monospace ASCII Character Fidelity:**
When `-StrictSEAscii` is passed, the simulator restricts all visual primitives to the strict printable character set present in Keen's vanilla `Monospace` font atlas (`FontData.xml`):

- Solid Bars: `#` (filled) and `-` (empty) (`##########--------`)
- LED Chiclets: `[#]` and `[ ]`
- Hazard Caution: `//` and `..`
- Chevrons: `>>` and `..`
- Dot Matrix: `* ` and `. `
- Sparkline Density: `.`, `:`, `;`, `=`, `+`, `*`, `#`, `%`

---

## Architecture & Performance Audit

### Performance Expectations

Normal telemetry evaluation rotates through one screen per run and reuses retained sprite buffers. Unchanged output can avoid drawing a new frame, but data sampling, history and scheduling still run. Boot frames, diagnostics, large block groups and inventory queries add work.

Use `scan` or `diagnose` and the Programmable Block's runtime statistics to measure your actual layout and grid. This repository does not establish comparative timings, multiplayer packet sizes, zero allocations or a guarantee against instruction-limit failures for arbitrary configurations.

### Feature Audit Matrix (PDR Compliance)

An audit against the project's Preliminary Design Review:

- [x] **Tier 1 — Core Monospace ASCII Rendering:** Implemented & verified (`border: ascii`).
- [x] **Tier 1 — Round-Robin Multi-Tick Scheduler:** Implemented & verified. One screen evaluated per tick; state cycles deterministically.
- [x] **Tier 1 — Dirty-Flag Skip:** Screen output signatures are cached; unchanged output can skip `DrawFrame()`, not script execution.
- [x] **Tier 1 — Explicit Group Aggregations:** Implemented & verified. Group binds enforce `agg: avg|sum|min|max` at parse time.
- [x] **Tier 1 — Flexible Screen Width (`width:`):** Implemented & verified. Resolves screen clipping on small cockpit panels and corner LCDs.
- [x] **Tier 1 — Hardware Integration:** Batteries, Gas Tanks (O2/H2), Air Vents, Cargo, and Jump Drives verified.
- [x] **Tier 1 — Surface Provider Fallback:** Supports both `IMyTextSurfaceProvider` and direct `IMyTextSurface` blocks.
- [x] **Tier 1 — Dynamic Text Height:** Dynamically queries `MeasureStringInPixels` with safe constant fallback.
- [x] **Tier 1 — Terminal Control:** `reload` and `refresh` arguments supported.
- [x] **Tier 2 — Retained Virtual Sprite Buffer:** Hardware-accelerated geometric bars on dark recessed tracks with sub-pixel precision.
- [x] **Tier 2 — Percentage String Cache:** Cached percentage strings (`0%`–`100%`) reduce repeated formatting allocations.
- [x] **Tier 2 — Fluid Damped Bars (Springs):** Aerospace avionics linear interpolation for silky smooth bar movement.
- [x] **Tier 2 — Segmented LED Chiclets (`led`):** 10-segment illuminated bar blocks for heavy industrial and military consoles.
- [x] **Tier 2 — Trend Sparklines (`spark` / `sparkline`):** 8-point historical telemetry mini-graph showing power/cargo drain trends.
- [x] **Tier 2 — Emergency Hazard Alert Pulsing:** Automatic synchronized red alert pulses on critical threshold breaches.
- [x] **Tier 2 — CLI/TUI Grid Layout Engine (`layout: grid`, `@panel`):** 2D matrix grids with coordinate isolation and snapped sub-panels.
- [x] **Tier 2 — Parameterized Template Engine (`@template`, `@use`):** Reusable HUD macros with `$VAR` substitution and rapid group renaming.
- [x] **Tier 2 — Full 24-Bit Color Science & Hex Parsing:** Supports `#RRGGBB`, `#RGB`, `R,G,B`, and transparent backgrounds.
- [x] **Tier 2 — Continuous RGB Multi-Stop Gradient Interpolation (`GradientLerp`):** Smooth RGB fades across stops.
- [x] **Tier 2 — Centered Balance Charts (`balance` / `horizon`):** Bidirectional gauges centered at 50%.
- [x] **Tier 2 — Native Font Atlas Selection (`font:`):** Custom font assignment per screen or sub-panel (`Monospace`, `Debug`, etc.).
- [x] **Tier 2 — Offline PowerShell Cmdlet (`Test-HelmLayout.ps1`):** Tab-completion, clipboard export, and ANSI preview.
- [x] **Tier 2 — Backgrounds:** solid, vertical and horizontal gradients, value-driven and flashing backgrounds, textures, `screen_bg:`, `panel_bg:`, red-alert background.
- [x] **Tier 2 — Boot, Console and Diagnostics:** boot sequence with logo and calibration sweep, `starter` and `probe`, console on the block's own screen and in the info pane.
- [x] **Tier 2 — Item Amounts and Contents:** `kind:Subtype` fields, `max(N)`, ranked `topN`, `scope: all` for docked ships.
- [x] **Tier 2 — Liquid Tanks and Vertical Panels:** `tank` / `tube` / `bucket`, `orientation: vertical`, large and medium sizes through panel spans.
- [x] **Tier 2 — Row Sizes and Colour Options:** `size(...)`, colour rules that answer with gradients, `color_by`, `track`, `label_color`.
- [x] **Tier 2 — Dial, Bar Dressing and Frames:** `dial`, `ticks` / `marker` / `outline` / `dir: left`, `segments` / `points`, in-game border styles for screens and panels (`frame`, `corners`, `double`, `rounded`, `heavy`).
- [x] **Tier 2 — Rate, ETA and Trend:** `text(bind.rate)`, `eta` and `trend` from a smoothed 5-second history.
- [x] **Tier 2 — Layout Options:** `gap:`, `padding:`, `value(off)`, `icon(Name)`, `blink(condition)`.
- [x] **Tier 2 — Mirroring and LCD Text:** `mirror:` / `mirror_panel:` copy HELM screens; `lineN` fields show the text of other LCDs.
- [ ] **Tier 2 — Verbatim Metadata Display:** Block metadata properties (`DetailedInfo`, `CustomName`, `OwnerId`) scheduled for future release.
- [ ] **Tier 3 / Deferred — Donut / Radial Charts:** Explicitly deferred per PDR §6: Space Engineers sprite API provides only `TEXT`, `TEXTURE`, and `CLIP_RECT`.

---

## Unit Testing & Validation

Core parsing, AST generation, macro expansion, grid math, and color science are decoupled from the Space Engineers API and can be compiled and verified locally with any standard C# compiler:

```bash
csc -out:Tests.exe HelmCore.cs Tests.cs
mono Tests.exe
```

```text
272 passed, 0 failed.
```

`Build-Release.ps1` runs these tests first. Three more checks cover the game script itself: `tools/scope-test` (fake blocks and screens through the real script), `tools/rename-check/Test-RenameParity.ps1` (the minified script draws exactly what the source draws on every preset) and `tools/pb-check` (compiles the minified script against the game's own assemblies when `SE_BIN64` points at the game's `Bin64` folder).

---

## License

MIT License. Free to use, adapt, and distribute on the Space Engineers Steam Workshop.
