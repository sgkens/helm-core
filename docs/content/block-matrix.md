---
title: "Terminal Block Binding Matrix"
description: "Supported terminal blocks, readable properties, recommended gauges and default units."
order: 7
section: "Reference"
badge: "Blocks"
---

## Complete Block Matrix

In the Live column, **[x]** means `Program.cs` has code for that block type. **[ ]** means only the generic fallback applies: the value is 1 when the block is enabled and 0 otherwise. Blocks that are not in the table read 0.

State fields such as `.locked`, `.connected`, `.brake`, `.producing`, `.projecting` and `.working` read 100 when true and 0 when false, and ratios such as a door's `.open` are 0 to 100, the same scale as `.percent`. That is what `checkbox`, `toggle`, `status`, `led` and `bar` expect. Item amounts and other raw values need `max(N)` to be drawn as a bar.

Block icons are cached from the [Official Space Engineers Wiki](https://spaceengineers.wiki.gg/) and link to their source file pages. Wiki content is available under [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/); Space Engineers game content remains owned by Keen Software House.

| Category | In-Game Block Type | SE Interface | Live | Supported Properties | Recommended Gauge | Default Units |
| :--- | :--- | :--- | :---: | :--- | :--- | :--- |
| **Power** | Battery Block | `IMyBatteryBlock` | **[x]** | `.percent`, `.stored`, `.max`, `.input`, `.output` | `bar`, `flow`, `spark` | `MW` / `%` |
| **Power** | Nuclear Reactor | `IMyReactor` | **[x]** | `.percent`, `.output`, `.max`, `.fuel` | `bar`, `spark`, `status` | `GW` / `MW` |
| **Power** | Solar Panel | `IMySolarPanel` | **[x]** | `.percent`, `.output`, `.max` | `bar`, `spark`, `dots` | `kW` |
| **Power** | Wind Turbine | `IMyWindTurbine` | **[x]** | `.percent`, `.output`, `.max` | `bar`, `flow` | `kW` |
| **Power** | Hydrogen Engine | `IMyHydrogenEngine` | **[x]** | `.percent`, `.output`, `.max` | `bar`, `flow`, `status` | `MW` |
| **Gas/Life** | Oxygen / Hydrogen Tank | `IMyGasTank` | **[x]** | `.percent`, `.filled`, `.cap`, `.capacity`, `.stockpile` | `flow`, `bar`, `toggle` | `kL` / `%` |
| **Gas/Life** | O2/H2 Generator | `IMyGasGenerator` | **[x]** | `.percent`, `.producing`, `.auto` | `status`, `toggle`, `flow` | - |
| **Gas/Life** | Air Vent | `IMyAirVent` | **[x]** | `.percent`, `.o2`, `.oxygen`, `.pressurized`, `.depressurize` | `bar`, `flow`, `status` | `kPa` / `%` |
| **Gas/Life** | Medical Room / Survival Kit | `IMyMedicalRoom` | **[ ]** | `.enabled` (on/off only) | `toggle`, `status`, `led` | - |
| **Storage** | Cargo Container | `IMyCargoContainer` | **[x]** | `.percent`, `.volume`, `.max`, `.mass` | `bar`, `spark` | `kL` / `t` |
| **Production**| Projector | `IMyProjector` | **[x]** | `.percent` (built share), `.built`, `.remaining`, `.total`, `.armor`, `.buildable`, `.projecting` | `bar`, `text`, `status` | blocks |
| **Production**| Welder / Grinder | `IMyShipToolBase` | **[x]** | `.percent`, `.working` | `status`, `checkbox` | - |
| **Mechanical**| Wheel Suspension | `IMyMotorSuspension` | **[x]** | `.percent`, `.height`, `.strength`, `.power`, `.friction`, `.steer`, `.propulsion`, `.brake` | `bar`, `text`, `checkbox` | `m` / `%` |
| **Storage** | Connector | `IMyShipConnector` | **[x]** | `.percent`, `.locked`, `.connected`, `.status` | `checkbox`, `status`, `toggle` | - |
| **Storage** | Conveyor Sorter | `IMyConveyorSorter` | **[ ]** | `.enabled` (on/off only) | `toggle`, `status`, `led` | - |
| **Drive** | Thruster (Atmo / Ion / H2) | `IMyThrust` | **[x]** | `.percent`, `.thrust`, `.max`, `.override` | `chevron`, `bar`, `spark` | `kN` / `%` |
| **Drive** | Gyroscope | `IMyGyro` | **[x]** | `.percent`, `.pitch`, `.yaw`, `.roll`, `.override` | `balance`, `bar` | `deg` / `°` |
| **Drive** | Jump Drive | `IMyJumpDrive` | **[x]** | `.percent`, `.stored`, `.max`, `.charge`, `.distance` | `bar`, `dots`, `status` | `km` / `%` |
| **Production**| Refinery / Blast Furnace | `IMyRefinery` | **[x]** | `.percent`, `.producing`, `.queue` | `status`, `bar`, `flow` | - |
| **Production**| Assembler | `IMyAssembler` | **[x]** | `.percent`, `.producing`, `.queue`, `.repeat` | `status`, `bar` | - |
| **Mechanical**| Rotor / Stator | `IMyMotorStator` | **[x]** | `.percent`, `.angle`, `.deg`, `.velocity`, `.rpm`, `.locked` | `balance`, `bar`, `toggle` | `deg`, `RPM` |
| **Mechanical**| Hinge | `IMyMotorAdvancedStator`| **[x]** | `.percent`, `.angle`, `.deg`, `.velocity`, `.locked` | `balance`, `bar`, `toggle` | `deg` |
| **Mechanical**| Piston | `IMyPistonBase` | **[x]** | `.percent`, `.pos`, `.position`, `.extension`, `.velocity` | `bar`, `balance` | `m` |
| **Mechanical**| Landing Gear / Magplate | `IMyLandingGear` | **[x]** | `.percent`, `.locked`, `.ready` | `status`, `checkbox` | - |
| **Weapons** | Gatling / Missile Turret | `IMyLargeTurretBase` | **[x]** | `.percent`, `.shooting`, `.target`, `.range` | `ammo`, `toggle`, `bar` | `m` / `rds` |
| **Weapons** | Artillery / Cannon Turret| `IMyLargeTurretBase` | **[x]** | `.percent`, `.shooting`, `.target`, `.range` | `ammo`, `bar` | `m` / `rds` |
| **Weapons** | Custom Turret Controller | `IMyTurretControlBlock` | **[x]** | `.percent`, `.target`, `.hastarget`, `.azimuth`, `.elevation` | `balance`, `status` | `deg` |
| **Weapons** | Railgun | `IMyUserControllableGun`| **[ ]** | `.enabled` (on/off only) | `toggle`, `status`, `led` | - |
| **Weapons** | Warhead | `IMyWarhead` | **[x]** | `.percent`, `.countdown`, `.armed` | `hazard`, `checkbox` | `s` |
| **Comms** | Radio Antenna | `IMyRadioAntenna` | **[x]** | `.percent`, `.radius`, `.range`, `.broadcasting` | `bar`, `toggle` | `km` |
| **Comms** | Laser Antenna | `IMyLaserAntenna` | **[ ]** | `.enabled` (on/off only) | `toggle`, `status`, `led` | - |
| **Comms** | Beacon | `IMyBeacon` | **[x]** | `.percent`, `.radius`, `.range` | `bar`, `toggle` | `km` |
| **Sensors** | Camera | `IMyCameraBlock` | **[x]** | `.percent`, `.active`, `.range` | `toggle`, `status` | `m` |
| **Sensors** | Sensor | `IMySensorBlock` | **[x]** | `.percent`, `.detected`, `.active` | `checkbox`, `status` | - |
| **Control** | Cockpit / Seat | `IMyShipController` | **[ ]** | `.enabled` (on/off only) | `toggle`, `status`, `led` | - |
| **Control** | Remote Control | `IMyRemoteControl` | **[ ]** | `.enabled` (on/off only) | `toggle`, `status`, `led` | - |
| **Control** | Gravity Generator | `IMyGravityGeneratorBase`| **[x]** | `.percent`, `.g`, `.accel` | `bar`, `spark` | `G`, `m/s²` |

## Item Fields and Text Panels

Three kinds of field work on blocks that are not in the table by type.

| Field | Reads | Example |
| :--- | :--- | :--- |
| `<kind>:<Subtype>` | Amount of one item in the inventories of the bound blocks. `kind` is `ingot`, `ore`, `component`, `ammo` and so on | `bar(cargo.ingot:Iron) max(5000)` |
| `share:<kind>:<Subtype>` | The volume the item takes as a percent of the bound blocks' inventory capacity (amount times the item's volume, divided by capacity). Use `max(100)` on a gauge | `bar(cargo.share:ingot:Iron) max(100)` |
| `top1`, `top2`, ... | The largest item held, the next largest, and so on, as text (`Iron ingot 4.2K`). Scanned a few blocks per run, refreshed every 15 seconds | `text(cargo.top1)` |
| `rate`, `eta`, `trend` | How the bind's percent is changing: change per minute, time until empty or full, and an arrow. Sampled every 5 seconds | `text(batt.eta)` |
| `line1`, `line2`, ... | A line of the text on a text panel or LCD, as text. Read every 2 seconds | `text(notes.line1)` |

See [Item Amounts](/docs/dsl-syntax/#item-amounts-cargo-and-reactor-contents) and [Text From Another LCD](/docs/dsl-syntax/#text-from-another-lcd).
