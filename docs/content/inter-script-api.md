---
title: "Inter-Script Micro-API & IGC"
description: "Dynamic binds, IGC messages and argument commands for feeding values from other scripts."
order: 11
section: "Advanced"
badge: "API"
---

## 1. Dynamic In-Memory Directives (`@bind ... = dynamic(...)`)

Bind screen gauges to in-memory variables that do not correspond to any physical block:

```helm
@bind target_range = dynamic("radar.dist", default: 0)
@bind drone_online = dynamic("drone_a.online", default: 0)
@bind mission_eta  = dynamic("nav.eta", default: 0)

@panel RADAR at(0, 0, 1, 1) title: "TARGET RADAR"
line: "RANGE   " bar(target_range.val) gradient(#22c55e, #ef4444) unit("m")
line: "DRONE   " status(drone_online.val) flat(#38bdf8)
line: "ETA     " spark(mission_eta.val) flat(#f59e0b) unit("s")
```

Dynamic variables hold numbers only. `default:` must be a number, and a string default is read as 0. Accepted incoming values are numbers, `true` or `on` (stored as 100), and `false` or `off` (stored as 0); any other text is ignored.

---

## 2. IGC Headless Display Server Channel (`"HELM"`)

Other Programmable Blocks on your grid or across antenna networks can broadcast telemetry updates to HELM over the Space Engineers Inter-Grid Communication (IGC) system.

HELM automatically listens on the unicast and broadcast channel:
- **IGC Channel Tag**: `"HELM"`

### Broadcast Message Format

Send semicolon-delimited key-value pairs in a single string message:

```csharp
// In your combat or autopilot script:
IGC.SendBroadcastMessage("HELM", "radar.dist=1420;drone_a.online=1;nav.eta=45");
```

HELM's parser updates the dynamic values and refreshes all mapped LCDs on the next tick.

---

## 3. Programmable Block Argument Micro-Commands

External scripts, Timer Blocks, Sensors, Event Controllers, and cockpit hotbars can manipulate HELM via argument execution (`pb.TryRun(argument)`):

### Set Variable Command (`set <var> <val>`)

```text
set radar.dist 1850
set alert_status YELLOW
```

### Global Emergency Alert Commands (`alert <state>`)

Trigger global red alert or battle stations flashing across all grid displays:

| Command | Action |
| :--- | :--- |
| `alert red` | Enables continuous flashing red alert borders and hazard banners across every screen. |
| `alert on` | Turns on emergency alert mode. |
| `alert off` | Restores normal display colors and borders. |
| `alert toggle`| Toggles alert mode on and off. |

```csharp
// Example: Trigger Red Alert when enemy detected:
IMyProgrammableBlock helmPB = GridTerminalSystem.GetBlockWithName("HELM PB") as IMyProgrammableBlock;
helmPB?.TryRun("alert red");
```

---

## 4. Performance & Diagnostics

HELM reports all registered dynamic variables in its terminal diagnostics readout:

```text
HELM Core v1.0.0
Screens: 2 active | Dynamic Vars: 3 (radar.dist, drone_a.state, nav.eta)
IGC Listener: ACTIVE ("HELM")
Last IGC Packet: 0 ticks ago
```
