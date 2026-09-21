---
title: "DSL Syntax Specification"
description: "Syntax reference for @bind, @screen, @panel and line directives."
order: 3
section: "Core Architecture"
badge: "DSL"
---

## Core Directives Overview

A HELM configuration consists of four primary building blocks:

1. `@bind`: Maps a semantic name to a physical block, block group, or dynamic variable.
2. `@screen`: Defines a target display surface, resolution profile, grid layout, and boot sequence.
3. `@panel`: Defines a visual card located at a specific grid coordinate with telemetry lines.
4. `line`: Renders an individual gauge or text readout linked to a bound block property.

---

## 1. Hardware Binding (`@bind`)

Bindings map game terminal blocks to variable names used across panels:

```helm
@bind <name> = <kind>("<selector>")
```

### Supported Binding Kinds

| Kind | Target | Example |
| :--- | :--- | :--- |
| `block` | Exact terminal block name | `@bind main_reactor = block("Large Reactor Alpha")` |
| `group` | Terminal block group | `@bind batteries = group("Main Battery Group")` |
| `battery` | Battery block or group | `@bind batt = battery("Battery Bank")` |
| `gastank` | O2 or H2 tanks | `@bind h2 = gastank("Hydrogen Tank 1")` |
| `cargo` | Cargo container | `@bind freight = cargo("Cargo Container Large")` |
| `reactor` | Nuclear reactor | `@bind nuke = reactor("Reactor Core")` |
| `solar` | Solar panel array | `@bind solar = solar("Solar Array Forward")` |
| `thrusters` | Thrusters group | `@bind retro = thrusters("Atmo Thruster Retro")` |
| `gyro` | Gyroscope | `@bind nav_gyro = gyro("Nav Gyroscope")` |
| `dynamic` | In-memory / IGC variable | `@bind target = dynamic("target.dist", default: 0)` |

Wildcards (`*`) are supported in names (e.g. `battery("Battery *")`).

---

## 2. Screen Declaration (`@screen`)

A screen maps to a physical LCD surface on your grid.

```helm
@screen <ScreenIdentifier>
output: "<TerminalBlockName>", <SurfaceIndex>
profile: <auto|standard|compact|wide|corner|CockpitSmall|WideLCD|TextPanel>
layout: grid(<columns>, <rows>)
bg: #<hexcolor>
border_color: #<hexcolor>
border: <heavy|double|single|minimal|rounded|ascii|graphic|none>
title_align: <left|center|right>
title_color: #<hexcolor>
```

### Screen Directives

- `output: "Name", index`: Terminal block name and 0-based surface index.
- `profile`: Hardware screen profile controlling aspect ratio and viewport adaptation.
- `layout: grid(cols, rows)`: Defines the layout grid dimensions (e.g. `grid(2, 2)`).
- `bg: #hex`: Screen background color (e.g. `#090b0f`).
- `border_color: #hex`: Screen border frame color.
- `border: style`: Modular border character set.

### Boot Sequence Directives

Each screen plays a boot sequence when the script starts, and again on the `boot` run argument. Editing the layout does not replay it. `boot_duration: 0` skips it, `3s` is seconds and a bare number is game ticks (`180` is 3 seconds):

```helm
boot_duration: 3s
boot_msg: "INITIALIZING COMMAND HUD"
boot_gradient(#38bdf8, #818cf8, #c084fc)
boot_log: "Calibrating inertial gyroscopes..."
boot_log: "Checking reactor core coolant loops..."
boot_log: "Telemetry telemetry bus online."
```

---

## 3. Panel Cards (`@panel`)

Panels organize telemetry lines into visual cards arranged on the screen grid:

```helm
@panel <PanelName> at(<col>, <row>, <span_cols>, <span_rows>) title: "<TITLE>"
font: <Monospace|Debug|White|Red|Green|Blue>
border_color: #<hexcolor>
title_align: <left|center|right>
title_color: #<hexcolor>
```

- `at(col, row, span_col, span_row)`: 0-indexed grid position and span.
- `title: "TEXT"`: Panel header text.
- `title_align: left|center|right`: Snaps panel title along the top border.
- `title_color: #hex`: Independent typography color for the title.

---

## 4. Telemetry Lines (`line:`)

Telemetry lines render within panels, binding a property to a gauge widget:

```helm
line: "<LABEL8>" <kind>(<bind>.<field>) <color_spec> [unit("<unit>")] [dir:<up|down|auto>]
```

### Line Syntax Rules

1. **Label**: Exactly 8 characters (padded with spaces, e.g. `"BATTERY "`, `"O2 TANK "`, `"CARGO   "`).
2. **Gauge Kind**: One of the 10+ supported widgets (`bar`, `flow`, `spark`, `balance`, `dots`, `chevron`, `hazard`, `led`, `toggle`, `checkbox`, `ammo`, `status`, `text`).
3. **Property Target**: `<bind_name>.<property_name>`.
4. **Color Specification**:
   - `flat(#hex)`: Single uniform color (e.g. `flat(#38bdf8)`).
   - `gradient(#hex1, #hex2, ...)`: Procedural multi-stop color gradient calculated from current value.
5. **Optional Modifiers**:
   - `unit("MW")`: Overrides or specifies engineering measurement units.
   - `dir: up|down`: Sets flow direction.

### Complete Panel Example

```helm
@panel REACTOR at(0, 0, 1, 1) title: "REACTOR ALPHA" title_align: center title_color: #38bdf8
line: "OUTPUT  " bar(reactor.output) gradient(#22c55e, #eab308, #ef4444) unit("MW")
line: "COOLANT " flow(coolant_tank.percent) gradient(#06b6d4, #3b82f6) dir: up
line: "DRAIN   " spark(reactor.output) flat(#38bdf8)
line: "STATUS  " status(reactor.status) flat(#22c55e)
```

Read next: [Visual Gauge Primitives](/docs/gauges/) for the full catalog of visual widgets.
