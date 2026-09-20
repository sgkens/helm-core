---
title: "Terminal Block Binding Matrix"
description: "Supported terminal blocks, readable properties, recommended gauges and default units."
order: 7
section: "Reference"
badge: "Blocks"
---

## Complete Block Matrix

In the Live column, **[x]** means `Program.cs` has code for that block type. **[ ]** means only the generic fallback applies: the value is 1 when the block is enabled and 0 otherwise. Blocks that are not in the table read 0.

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
