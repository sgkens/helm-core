---
title: "Gauge Primitives"
description: "Reference for each gauge type and its options."
order: 4
section: "Visual Primitives"
badge: "10 Types"
---

## 1. Linear Progress Bar (`bar`)

Renders a continuous horizontal filled bar reflecting 0.0% to 100.0%.

```helm
line: "BATTERY " bar(batt.percent) gradient(#22c55e, #eab308, #ef4444)
```

- **Visual Output**: `[██████████░░░░░░] 64.2%`
- **ASCII Mode**: `[#####-------] 64.2%`
- **Supported Options**: `gradient(...)`, `flat(...)`, `unit(...)`.

---

## 2. Dynamic Flow Indicator (`flow`)

Designed for fluids, gases, and directional transfers. Uses font-safe, single-cell directional arrows (`^^`, `vv`, `==`) and a 4-phase cyclic turbine spinner (`| / - \`).

```helm
line: "O2 FLOW " flow(o2_tank.percent) gradient(#06b6d4, #3b82f6) dir: up
line: "HYDROGEN" flow(h2_tank.percent) flat(#eab308) dir: down
```

- **Visual Output**: `[  ^^^^^^ / ] 88.0%`
- **Alignment**: Uses only 1-cell-wide glyphs so terminal and monospace fonts stay aligned.

---

## 3. Sparkline Trend Graph (`spark`)

Renders a rolling 8-to-12 sample historical trend sparkline showing value velocity over the last several game ticks.

```helm
line: "GRID PWR" spark(power.output) flat(#38bdf8) unit("MW")
```

- **Visual Output**: `[  _.-~=*^# ] 12.4MW`
- **Behavior**: Shifts one column left on each update tick, automatically mapping values to 8 distinct vertical level glyphs.

---

## 4. Centered Balance & Trim (`balance`)

A bidirectional gauge centered at 0.0 with deviation to the left (`<`) or right (`>`). Perfect for gyroscopic pitch/roll, wheel suspension height, and hinge/rotor articulation angles.

```helm
line: "PITCH   " balance(gyro.pitch) flat(#f59e0b) unit("deg")
line: "TRIM    " balance(rudder.angle) flat(#38bdf8)
```

- **Visual Output**: `[  <<<<|     ] -14.2°`
- **Alignment**: Uses 1-cell `<` and `>` glyphs so the row width does not change as the value moves.

---

## 5. Segmented Dots (`dots`)

Discrete block-level meter showing 10 distinct level buckets. Ideal for lighting intensity, jump drive charges, and reactor coolant stages.

```helm
line: "LIGHTS  " dots(lights.percent) flat(#e0e7ff)
```

- **Visual Output**: `[●●●●●●○○○○] 60.0%`

---

## 6. Thrust Chevron (`chevron`)

Directional high-intensity chevron bank. Perfect for thrusters, gravity acceleration, and emergency overdrive.

```helm
line: "FORWARD " chevron(thrusters.percent) gradient(#38bdf8, #ef4444)
```

- **Visual Output**: `[>>>>>>>>> ] 90.0%`

---

## 7. Hazard Caution Bar (`hazard`)

Striped diagonal warning gauge with automatic flashing threshold alerts. Recommended for warheads, reactor overload, and hull breach indicators.

```helm
line: "WARHEAD " hazard(bomb.countdown) flat(#ef4444) unit("s")
```

- **Visual Output**: `[ ////////// ] ARMED`

---

## 8. Status Lamp (`led`)

Multi-state illuminated indicator lamp.

```helm
line: "SHIELD  " led(shield.active) flat(#22c55e)
```

- **Visual Output**: `(●) ONLINE` or `(○) OFFLINE`

---

## 9. Binary Toggle & Switch (`toggle`)

Clean binary state readout for toggleable systems (sorters, spotlights, inertial dampeners).

```helm
line: "DAMPENER" toggle(controller.dampeners) flat(#38bdf8)
```

- **Visual Output**: `[ ON ]` or `[OFF ]`

---

## 10. Checkbox (`checkbox`)

Square verification checkbox for locks, clamps, and landing gears.

```helm
line: "GEAR LOK" checkbox(gear.locked) flat(#22c55e)
```

- **Visual Output**: `[X] LOCKED` or `[ ] READY`

---

## 11. Ordnance & Ammo Counter (`ammo`)

Discrete bullet/missile bar representing current magazine capacity and reserve ordnance.

```helm
line: "GATLING " ammo(turret.percent) flat(#f59e0b) unit("rds")
```

- **Visual Output**: `[||||||||░░] 820rds`

---

## 12. Dynamic Text Readout (`status` / `text`)

Displays formatted string status directly from block states or dynamic IGC broadcast channels.

```helm
line: "TARGET  " text(radar.target) flat(#f43f5e)
```

- **Visual Output**: `FRIGATE-04`
