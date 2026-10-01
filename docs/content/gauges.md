---
title: "Gauge Primitives"
description: "Reference for each gauge type and its options."
order: 4
section: "Visual Primitives"
badge: "15 Types"
---

HELM has 15 distinct visual controls. Examples below are individual lines to place inside a configured screen or panel, with matching `@bind` aliases. In-game sprite rendering, `border: ascii`, and the offline terminal preview differ visually; glyph examples are illustrative, not exact screenshots.

## Linear Progress Bar (`bar`)

Renders a continuous horizontal filled bar reflecting 0.0% to 100.0%.

```helm
line: "BATTERY " bar(batt.percent) gradient(#22c55e, #eab308, #ef4444)
```

- **Visual Output**: `[██████████░░░░░░] 64.2%`
- **ASCII Mode**: `[#####-------] 64.2%`
- **Supported Options**: `gradient(...)`, `flat(...)`, `unit(...)`.

---

## Dynamic Flow Indicator (`flow`)

An animated flow-style indicator driven by the bound value. It does not measure physical transfer rate automatically; use the supported `.rate` field for a rate readout. The terminal preview uses directional glyphs and a cyclic spinner.

```helm
line: "O2 FLOW " flow(o2_tank.percent) gradient(#06b6d4, #3b82f6) dir: up
line: "HYDROGEN" flow(h2_tank.percent) flat(#eab308) dir: down
```

- **Visual Output**: `[  ^^^^^^ / ] 88.0%`
- **Alignment**: Uses only 1-cell-wide glyphs so terminal and monospace fonts stay aligned.

---

## Sparkline Trend Graph (`spark`)

Renders a rolling value history with 8 samples by default; `points(N)` selects 2–40 samples. Samples advance when the screen is evaluated, not on every game tick.

```helm
line: "BATTERY " spark(batt.percent) points(12) flat(#38bdf8)
```

- **Visual Output**: A history graph beside the current battery percentage.
- **Behavior**: Shows sampled values, not their derivative; use `.rate` for a rate of change.

---

## Centered Balance & Trim (`balance`)

A bidirectional gauge centered at 50%: values below 50 extend left, values above 50 extend right. It is not a signed attitude instrument; gyro pitch is a rotation command, not the ship's measured pitch angle.

```helm
line: "BALANCE " balance(trim.val) flat(#f59e0b)
```

- **Binding**: For an external normalized value, use `@bind trim = dynamic("trim", default: 50)` and supply a value from 0 to 100.
- **Visual Output**: A centered indicator with percentage text.
- **Alignment**: Uses 1-cell `<` and `>` glyphs so the row width does not change as the value moves.

---

## Segmented Dots (`dots`)

Discrete segmented meter. Use `segments(N)` to choose the segment count; useful for jump-drive charge or battery fill.

```helm
line: "CHARGE  " dots(jump.percent) segments(10) flat(#e0e7ff)
```

- **Visual Output**: `[●●●●●●○○○○] 60.0%`

---

## Thrust Chevron (`chevron`)

Directional high-intensity chevron bank. Perfect for thrusters, gravity acceleration, and emergency overdrive.

```helm
line: "FORWARD " chevron(thrusters.percent) gradient(#38bdf8, #ef4444)
```

- **Visual Output**: `[>>>>>>>>> ] 90.0%`

---

## Hazard Caution Bar (`hazard`)

Striped diagonal warning gauge with automatic flashing threshold alerts. Recommended for warheads, reactor overload, and hull breach indicators.

```helm
line: "WARHEAD " hazard(bomb.countdown) max(60) flat(#ef4444) unit("s")
```

- **Visual Output**: A striped gauge with the countdown value in seconds. Set `max(...)` to your chosen full-scale countdown.

---

## Segmented LED Bar (`led`)

Proportional illuminated segments, not an on/off status lamp.

```helm
line: "BATTERY " led(batt.percent) segments(10) flat(#22c55e)
```

- **Visual Output**: Lit and unlit segments beside the current percentage.

---

## Binary Toggle & Switch (`toggle`)

Clean binary state readout for toggleable systems such as hydrogen-tank stockpile mode.

```helm
line: "STOCKPILE" toggle(h2.stockpile) flat(#38bdf8)
```

- **Visual Output**: `[ ON ]` or `[OFF ]`

---

## Checkbox (`checkbox`)

Square verification checkbox for locks, clamps, and landing gears.

```helm
line: "GEAR LOK" checkbox(gear.locked) flat(#22c55e)
```

- **Visual Output**: A checked or empty box with a state label; terminal rendering uses ACTIVE or READY.

---

## Ordnance & Ammo Counter (`ammo`)

Discrete pips representing a bound value. For ammunition counts, bind an inventory and select an ammo subtype; turret `.percent` is shooting state, not remaining ammunition.

```helm
line: "AMMO    " ammo(cargo.ammo:NATO_25x184mm) max(1000) flat(#f59e0b)
```

- **Visual Output**: Pips with the number of inventory items (magazines), not the number of individual rounds inside them.

---

## Status Badge (`status`, `badge`, `state`)

A threshold-based status indicator: ONLINE, STBY or OFF.

```helm
line: "POWER   " status(batt.percent) flat(#22c55e)
```

- **Behavior**: A status label rather than a proportional bar or arbitrary string.

---

## Dynamic Text Readout (`text`)

Displays a value without a gauge, including numeric telemetry or text exposed by a binding. See the block matrix and inter-script API for supported fields.

```helm
line: "BATTERY " text(batt.percent) flat(#f43f5e)
```

- **Visual Output**: The formatted battery percentage.

---

## Liquid Tank (`tank`, `tube`, `bucket`)

A tube that fills from the bottom with the value. A line of `~` and `-` characters slides along the liquid surface and three bubbles rise through it. It fills the height of its panel, so a panel that spans the screen is the large size and a grid of panels gives medium ones.

```helm
@panel p at(0, 0, 1, 1) title: "H2"
line: "H2" tank(h2.percent) gradient(#1e3a8a, #38bdf8)
```

- **Layout**: A panel with a tank lays its lines out as columns: value on top, tube in the middle, label below. `orientation: vertical` does the same for other gauges, and a `bar(...)` in such a panel is a vertical bar that fills from the bottom.
- **Colour**: `flat(...)` is one colour, `gradient(...)` runs up through the liquid, `color(...)` rules pick a colour by value, and a rule can answer with a gradient (`red/orange`).
- **Border modes**: With `border: ascii` a tank is drawn as a text line. `helmv.ps1` draws it as a plain bar.
- **Presets**: `tank_fullscreen`, `tank_trio`, `vertical_bars`.

---

## Dial (`dial`, `gauge`, `arc`)

A half circle of segments that light up from left to right with the value, with a needle and a hub.

```helm
line: "POWER" dial(pwr.percent) size(large) gradient(#22c55e, #eab308, #ef4444) segments(20)
```

- **Colour**: A gradient runs along the arc by position (green at the start, red at the end). `color(...)` rules colour all lit segments alike. `track(color)` colours the unlit ones.
- **Size**: The radius follows the row height and the free width, so use `size(large)` or a tall panel.
- **Preset**: `gauge_lab`. `helmv.ps1` shows a dial as a value only.

---

## Options Every Gauge Takes

| Option | Effect |
| :--- | :--- |
| `flat(color)` / `gradient(a, b, ...)` | Fixed colour, or a colour that follows the value |
| `color(bind.field > N : color, ..., default: color)` | Colour rules; a rule may answer with a gradient such as `red/orange` |
| `color_by(bind.field)` | Colour rules and gradients read another value than the one the gauge shows |
| `max(N)` / `max(bind.field)` | The full amount, for item counts and raw values |
| `size(medium)` / `size(large)` | The row takes 2 or 4 shares of its panel and the bar grows with it |
| `ticks(N)`, `marker(a, b)`, `outline(color)`, `dir: left` | Bar dressing: division lines, threshold ticks, a frame, fill from the right |
| `segments(N)` / `points(N)` | Segments of `led`, `dots` and `dial`; columns of `spark` |
| `value(off)` | Hide the value text; the gauge takes its space |
| `icon(Name)` | A built-in sprite before the label |
| `blink(bind.field > N)` | The line flashes while the comparison is true |
| `label_color(color)` | Colour of the label text |
| `track(color)` | Colour of the empty part of the gauge |
| `unit("MW")`, `format:` | Value text |

See [DSL Syntax](/docs/dsl-syntax/) for the rules and examples.
