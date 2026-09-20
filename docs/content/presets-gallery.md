---
title: "Presets & Layout Gallery"
description: "Layout files included in layouts/default/."
order: 9
section: "Gallery"
eleventyComputed:
  badge: "{{ presets.count }} Presets"
---

There are {{ presets.count }} layout files in `layouts/default/`. Each can be loaded directly into your Programmable Block's Custom Data or tested via `helmv.ps1 -Preset <name>`.

## Default Layouts

### 1. Planetary Base (`planetary_base`)

{% presetShot "planetary_base", "TextPanel" %}

- **Use Case**: Surface outposts, alien planet bases, industrial processing stations.
- **Aspect & Grid**: `grid(2, 3)` — 6 panels on a large wall LCD or control center screen.
- **Panels**:
  - `SURFACE POWER`: Solar array, wind turbine, and battery reserve averages.
  - `LIFE SUPPORT`: O2/H2 generation, oxygen tanks, atmospheric vents.
  - `EXCAVATION`: Mining drill statuses, hopper cargo fill %, sorters.
  - `ORE PROCESSING`: Refinery status, blast furnace throughput, assembler queues.
  - `BASE DEFENSE`: Perimeter turrets, sensor scans, landing gear clamps.
  - `INFRASTRUCTURE`: Solar tracking rotors, projector construction, connector status.
- **Color Theme**: Industrial emerald `#060a08` base with `#22c55e` borders.

---

### 2. Command Carrier (`command_carrier`)

{% presetShot "command_carrier", "WideLCD" %}

- **Use Case**: Capital carriers, dreadnoughts, and fleet flagships.
- **Aspect & Grid**: `grid(3, 2)` — 6 panels across a wide bridge monitor (`WideLCD`).
- **Panels**:
  - `FLEET POWER`: Primary reactor output, battery bank, energy distribution.
  - `HANGAR OPS`: Docked fighter count, hangar bay pressurization, magnetic clamps.
  - `WEAPONS BATTERY`: Artillery ammo, railgun capacitor charge, warhead arming state.
  - `POINT DEFENSE`: PDC Gatling coverage, missile bays, ECM countermeasure toggles.
  - `PROPULSION`: Forward main drives, RCS thruster banks, jump drives.
  - `COMMAND NET`: Laser comms, long-range antenna, IGC fleet telemetry.
- **Color Theme**: Deep imperial purple `#080510` with `#7c3aed` borders.

---

### 3. Survival Essentials (`survival_essentials`)

{% presetShot "survival_essentials", "TextPanel" %}

- **Use Case**: The essential solo survival HUD for early-to-mid game rovers, mining sledges, and starter shuttles.
- **Aspect & Grid**: `grid(2, 2)` — Clean 4-quadrant layout for a standard 1x1 LCD.
- **Panels**:
  - `POWER STATUS`: Battery charge %, charge/discharge flow, solar supplement.
  - `LIFE SUPPORT`: O2 tank level, cabin vent pressure, H2 propellant, survival kit.
  - `CARGO & SUPPLY`: Main cargo volume %, component hopper, conveyor flow.
  - `SYSTEM HEALTH`: Docking connector status, landing gear lock, reactor output.
- **Color Theme**: Clean cyan/blue `#090b0f` with `#38bdf8` borders.

---

### 4. Deep Space Explorer (`deep_space_explorer`)

{% presetShot "deep_space_explorer", "TextPanel" %}

- **Use Case**: Long-range scout ships, survey vessels, and interstellar expedition craft.
- **Aspect & Grid**: `grid(2, 2)` on main flight seat LCD.
- **Panels**:
  - `NAVIGATION`: Jump drive charge, jump range sparkline, gyro trim balance.
  - `POWER & RANGE`: Battery reserves, reactor fuel, power trend graph.
  - `SURVEY & SENSORS`: Ore detector range, forward camera, antenna broadcast.
  - `DOCKING & SUPPLY`: Connector clamp, cargo capacity, medical status.
- **Color Theme**: Deep indigo `#07090f` with `#6366f1` borders.

---

### 5. Mining Fleet Coordinator (`mining_fleet_coordinator`)

{% presetShot "mining_fleet_coordinator", "TextPanel" %}

- **Use Case**: Motherships and tender bases coordinating multi-player mining drones.
- **Aspect & Grid**: `grid(2, 3)` — Mixes local hardware blocks and dynamic IGC channels.
- **Panels**:
  - `MINER ALPHA`: Remote drone cargo, drill activation, and battery level via IGC.
  - `MINER BETA`: Remote drone status via IGC broadcast channel.
  - `FLEET CARGO`: Local base cargo containers, ore volume sparkline.
  - `PROCESSING`: Refineries producing status, sorter drain-all toggles.
  - `FLEET POWER`: Power grid distribution and reactor output.
  - `OPERATIONS`: Remote mission progress, ETA, and connector docking locks.
- **Color Theme**: Heavy industrial amber `#080a06` with `#f59e0b` borders.

---

### 6. Atmospheric Lander (`atmospheric_lander`)

{% presetShot "atmospheric_lander", "CockpitSmall" %}

- **Use Case**: Planetary dropships, cargo landers, and heavy atmospheric descent vehicles.
- **Aspect & Grid**: `grid(2, 2)` cockpit instrument cluster.
- **Panels**:
  - `DESCENT VECTOR`: Gyro pitch balance, atmospheric thruster chevrons, speed bar.
  - `LANDING SYSTEMS`: Magplate lock status, connector ready, suspension height balance.
  - `SURFACE OPS`: Surface drill status, cargo fill %, solar recharge.
  - `CRAFT INTEGRITY`: Battery reserves, medical pod, cabin O2 supply.
- **Color Theme**: Warm fiery amber `#0a0800` with `#d97706` borders.

---

## More Layouts

Simple layouts, complex layouts and edge cases that are used to test the parser and the renderers.

{% for p in presets.others %}
### {{ p.title }} (`{{ p.name }}`)

{% presetShot p.name, p.profile %}

{{ p.description }}

{% endfor %}
