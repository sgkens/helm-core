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
| `block` | Terminal block whose name contains the text | `@bind main_reactor = block("Large Reactor Alpha")` |
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

### Which Blocks a Name Matches

- **A name matches any block whose name contains the text.** `block("Cockpit")` matches "Cockpit", "Fighter Cockpit" and "Cockpit 2". When several match, the first one the game finds is used. Give the block a name that only it contains, or use a group.
- **Groups work the same way** and use the first group whose name contains the text.
- **Only your own construct is used:** the same grid, plus subgrids attached with rotors, pistons or hinges. A ship docked on a connector is ignored, even when its block names match yours, and HELM never draws on a docked ship's screens. A ship joined by a merge block counts as part of your construct, because the game treats the merged grids as one.
- If the first match belongs to another ship, HELM looks through your own blocks for one that matches.
- Run the block with argument `scan` to see how many blocks were ignored because they are on docked ships.
- **To read a docked ship on purpose**, add `scope: all` to the bind: `@bind visitor = group("Guest Batteries", agg: avg, scope: all)`. That bind alone then also matches blocks on ships docked on a connector, so a station can show a visitor's cargo. Use a name that only the visiting ship has.

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
- `refresh: <ticks>`: Draws this screen at most every N game ticks (`600` is every 10 seconds). Use it on screens that change slowly to save instructions. Default: every run.
- `alert_period: <seconds>`: Length of one red-alert flash cycle (default `1.6`). The flash follows the clock, not the run rate.
- `bg: #hex`: Screen background color (e.g. `#090b0f`). See [Backgrounds](#backgrounds) for gradients, panel colours and the alert background.
- `border_color: #hex`: Screen border frame color. `border_gradient(#a, #b, ...)` blends through the stops from left to right along the top and bottom lines (`gradient_mode: steps` uses the first stop on top and the last on the bottom).
- `gradient_mode: smooth` or `steps`: How gradients pick a colour. Default `smooth`.
- `border: style`: Modular border character set.

### Tanks and Vertical Bars

A panel with `orientation: vertical`, or with a `tank(...)` line, lays its lines out as **columns**: the value on top, the bar or tube in the middle, the label below. The bar or tube fills the height of its panel.

```helm
@panel p at(0, 0, 1, 1) title: "SHIP"
orientation: vertical
line: "PWR" bar(pwr.percent) gradient(#ef4444, #eab308, #22c55e)
line: "H2 " tank(h2.percent) gradient(#1e3a8a, #38bdf8)
```

- **`tank(...)`** (also `tube(...)` and `bucket(...)`) draws a liquid tube. The liquid rises from the bottom with the value, a line of characters slides along its surface, and three bubbles rise through it. It moves, so a screen with a tank redraws at the fast rate.
- **Colour:** `flat(...)` colours the whole liquid. `gradient(...)` runs through the liquid from the bottom to the top (a thermometer). `color(bind.field > 90 : red, default: cyan)` picks one colour from the value. On a vertical `bar(...)` a gradient follows the value, as on any bar.
- **Sizes:** the gauge fills its panel, so a panel that spans the whole screen is the large size, and a grid of panels gives medium ones (`grid(3, 1)` is three tubes side by side). See the `tank_fullscreen`, `tank_trio` and `vertical_bars` presets.
- **`max(N)`** works as on any gauge, so an item amount can fill a tube. `helmv.ps1` does not draw vertical panels or tanks.

### Bar Sizes

`size(medium)` and `size(large)` give a line more of its panel. Rows share the panel's height 1 : 2 : 4 (normal, medium, large), and the bar of a medium or large row grows to 70% of its row instead of keeping the normal height. Without `size(...)` every row is the same, as before.

```helm
@panel main at(0, 0, 1, 1) title: "SHIP"
line: "POWER" bar(pwr.percent) size(large) gradient(#ef4444, #eab308, #22c55e)
line: "H2"    bar(h2.percent) size(medium) flat(#38bdf8)
line: "CARGO" bar(cargo.percent) flat(#94a3b8)
```

Sizes combine with the layout tools: `grid(...)` and `at(col, row, spanX, spanY)` set the size of a panel, `size(...)` the share of each row inside it, `tank(...)` and `orientation: vertical` make columns. The presets `bar_sizes` and `combo_dashboard` mix them.

### Colour Choice on a Line

Every kind of line (bar, led, dots, toggle, checkbox, status, text, tank) picks its colour the same way: `color(...)` rules first, then `gradient(...)` at the current value, then `flat(...)`. Text lines follow the same order, so `text(bind.field) flat(cyan)` is cyan; without any of the three a text line is white.

- **Conditional gradients:** a rule can answer with a gradient written as colours joined by `/`. `color(pwr.percent < 25 : red/orange, default: green/cyan)` runs red to orange across the bar when power is under 25, and green to cyan otherwise. On a tank the gradient runs up the liquid.
- `label_color(color)` sets the colour of the label text, and `track(color)` the empty part of a bar or tube.
- Inside `color(...)` write colours as names or `/` gradients of names. A `#` that follows a space starts a comment, so `default: #22c55e` is cut off; `gradient(#..., #...)`, `flat(#...)`, `label_color(#...)` and `track(#...)` are not affected.

### Mirroring a Screen

`mirror: "Screen name"` on a screen copies the drawing of another `@screen` of the same Programmable Block to this screen's display, scaled to fit and centred.

```helm
@screen Bridge
output: "Bridge LCD", 0
# ... panels ...

@screen Hallway
output: "Hallway LCD", 0
mirror: "Bridge"
```

- The name is matched without regard to case. The source must be a graphic screen (not `border: ascii` and not a banner).
- The copy is redrawn only when the source has redrawn. A mirror has no panels of its own; anything else written under it is ignored.
- Only screens drawn by this script can be mirrored. The game does not let a script read the picture of another block's screen.

### Text From Another LCD

`text(bind.line1)`, `line2`, and so on show a line of the text held by a text panel or LCD that is not a HELM screen, so notes, other scripts' text output and the like can appear inside a HELM display.

```helm
@bind notes = block("Ship Notes")

@screen Bridge
output: "Bridge LCD", 0
@panel NOTES at(0, 0, 1, 1) title: "NOTES"
line: "1" text(notes.line1) flat(#94a3b8)
line: "2" text(notes.line2) flat(#94a3b8)
```

- The bind must name one block with a text surface. The text is read every 2 seconds; a line the panel does not have shows empty.
- Only text can be read. The game gives a script no access to the picture on another screen, so images and the drawing of another script cannot be copied. To copy a HELM screen, use `mirror:` above.

### Backgrounds

A screen is drawn in layers: the screen background, then a card behind each panel of a multi-panel screen, then the panel contents.

| Directive | Where | Meaning |
| :--- | :--- | :--- |
| `bg: <color>` | `@screen` | Solid screen background. `transparent` draws none, so the game's own screen colour (black) shows through |
| `bg_gradient(<color>, <color>, ...)` | `@screen` | Vertical gradient, top to bottom, drawn as 16 bands. Replaces `bg:` |
| `bg: <color>` | after an `@panel` line | Background of that panel only. Use different colours for a deliberate two-tone screen |
| `screen_bg: <color>` | `@screen` | The background of the whole screen, wherever the line is written. Same as `bg:` before the first `@panel` |
| `panel_bg: <color>` | `@screen` | A card behind every panel that has no `bg:` of its own. Off by default, so the screen is one colour. Use an `r,g,b,a` colour for a translucent card |
| `bg: color(<bind>.<field> <op> N : <color>, ..., default: <color>)` | `@screen` or after an `@panel` line | Background that follows a value, like the colour rules of `text()` lines. `bg: color(batt.percent < 25 : #7f1d1d, batt.percent < 60 : #78350f, default: #14532d)` |
| `flash: <bind>.<field> <op> N` | `@screen` or after an `@panel` line | Flashes that background to `alert_bg` while the comparison is true, for example `flash: batt.percent < 15` |
| `bg_gradient_dir: vertical` or `horizontal` | `@screen` or after an `@panel` line | Direction of `bg_gradient(...)` (default vertical). A gradient can also be given to a single panel |
| `bg_texture: <name>` | `@screen` | Draws one of the game's built-in sprites over the background, scaled to the screen. Run the block with the argument `scan` to list the names the game offers |
| `bg_texture_color: <color>` | `@screen` | Tint of the texture (default white at 24/255 opacity) |
| `alert_bg: <color>` or `none` | `@screen` | Colour the background flashes to while the red alert is on (default `#5b0f0f`). `none` turns the flash off for this screen |

Colours are `#rrggbb`, a name, or `r,g,b,a` with an alpha from 0 to 255, so a background or card can be translucent. Without `panel_bg:` or a panel `bg:`, nothing is drawn behind a panel and the screen background shows through as one colour.

- **Where you write `bg:` matters.** Before the first `@panel` it colours the whole screen. After an `@panel` line it colours that panel only. If you put `bg:` at the bottom of a screen block, only the last panel gets the colour and the rest of the screen keeps the default one: half one colour, half another. The block's output pane and console show a note when only some panels of a screen have their own `bg:`. `screen_bg: <color>` sets the whole screen from anywhere in the block, so it is the safe choice.
- **Red alert:** `alert red` flashes every screen's background between `bg` and `alert_bg` (half a cycle each, about every second and a half at the fast rate) until `alert off`. Gauges whose colour is red also flash, as before.
- **Values:** `bg: color(...)` and `flash:` read a block field on every redraw, so a panel can turn red as a value falls. `flash:` uses the same `alert_period:` as the red alert.
- The `background_lab` preset shows a gradient (vertical and horizontal), two deliberate tones, a see-through screen, an alert background, a panel that follows a value, a panel that flashes and a texture. `helmv.ps1` draws the card backgrounds (solid, per-panel, value-driven, a vertical gradient across each card and `panel_bg:`) and, with `-RedAlert`, the alert flash. It does not draw horizontal gradients, textures or the gutters between cards.

### Item Amounts (Cargo and Reactor Contents)

A field of the form `kind:Subtype` reads how much of one item a block holds, adding up all of its inventories. For a group, the bind's `agg:` decides whether the blocks are summed, averaged, and so on.

```helm
@bind cargo   = group("Cargo Containers", agg: sum)
@bind reactor = block("Main Reactor")
...
line: "IRON    " text(cargo.ingot:Iron) unit("kg")
line: "ICE     " text(cargo.ore:Ice) unit("kg")
line: "STEEL   " text(cargo.component:SteelPlate)
line: "URANIUM " text(reactor.ingot:Uranium) unit("kg")
```

| Kind | Items | Example |
| :--- | :--- | :--- |
| `ingot` | Ingots | `ingot:Iron`, `ingot:Uranium` |
| `ore` | Ores | `ore:Ice`, `ore:Iron` |
| `component` | Components | `component:SteelPlate` |
| `ammo` | Ammunition magazines | `ammo:NATO_25x184mm` |

- The subtype is the game's internal name (`SteelPlate`, not "Steel Plate"), and it must not contain a dot.
- Use `text(...)` for the amount alone. Add `max(N)` to draw a bar or a chart against a full amount, for example an ammunition load: `bar(guns.ammo:NATO_25x184mm) max(1200)`. Without `max(N)`, a `bar(...)` on an item field draws the block's fill percentage and prints the amount.
- Each item line reads every block in its bind on every redraw. Bind a group instead of many single blocks, and check the `instr` figure in the console on a large grid before adding many item lines.
- **Largest items:** `text(bind.top1)` shows the largest item held by the bind's blocks, `top2` the next, up to any rank you use, for example `Iron ingot 1.5k`. Amounts are summed over every inventory of every block in the bind. Ore and ingots are named with their kind, other items by their subtype. A rank with no item shows `-`.
- The ranking is scanned a few blocks per run within an instruction budget and refreshed every 15 seconds, so it lags the real contents. Listing every item is not supported.

### ASCII Banners

A banner is a block of your own ASCII art. Paste it between `@banner <name>` and `@end`. The lines are kept exactly as written, so `#`, quotes and spaces in the art are safe, and the common left indent is removed.

```helm
@banner emblem
   /\
  /  \
 /____\
@end

@screen Splash
output: "Bridge LCD", 0
banner: emblem
banner_gradient(#e879f9, #7c3aed)
bg: #0a0c10
```

| Directive | Where | Meaning |
| :--- | :--- | :--- |
| `@banner <name>` ... `@end` | Top level | Defines the art |
| `banner: <name>` | `@screen` | Shows the art full screen, scaled to fill the surface. The screen draws nothing else |
| `boot_logo: <name>` | `@screen` | Uses the art as the boot logo instead of the default emblem |
| `banner_color: <color>` / `banner_gradient(...)` | `@screen` | Colour of the art (the same setting as `boot_color` and `boot_gradient`) |

Keep lines within the width of the display: about 30 characters on a 1x1 panel, 60 on a wide panel and 16 on a corner LCD. A banner that does not exist is reported as a configuration error. `helmv.ps1` does not draw banners.

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
   - `gradient(#hex1, #hex2, ...)`: Multi-stop color gradient. The colour blends smoothly between the two nearest stops as the value changes. Add `gradient_mode: steps` to a screen to pick the nearest stop instead (the older, stepped look). A red stop still flashes on a red alert either way.
5. **Optional Modifiers**:
   - `unit("MW")`: Overrides or specifies engineering measurement units.
   - `max(bind.field)`: The same, with the full amount read from a block field, for example `bar(fuel.ingot:Uranium) max(reactor.max)`.
   - `max(N)`: Treats `N` as the full amount. Bars, sparklines, gradients and colour rules work on the percentage of `N` (capped at 100%), and the printed value stays the real amount. Use it with [item amounts](#item-amounts-cargo-and-reactor-contents) or any raw value:
     `line: "GATLING " bar(guns.ammo:NATO_25x184mm) max(1200) gradient(#ef4444, #eab308, #22c55e)`.
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
