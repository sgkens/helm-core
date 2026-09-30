---
title: "Inter-Script Micro-API & IGC"
description: "Feed HELM from other scripts with batched commands, broadcast or unicast telemetry, and stale-data indicators."
order: 11
section: "Advanced"
badge: "API"
---

## Dynamic In-Memory Directives (`@bind ... = dynamic(...)`)

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

Dynamic variables hold a number or a piece of text.

- **Numbers.** `default: 64` or an incoming `64`. `true` or `on` is stored as 100, and `false` or `off` as 0. Every gauge can read these.
- **Text.** A quoted default, `default: "STANDBY"`, or any other incoming value, such as `ENGAGING` or `Escort Alpha`. Only `text(...)` lines display it:

```helm
@bind drone_state = dynamic("drone_a.state", default: "STANDBY")
@bind drone_fuel  = dynamic("drone_a.fuel", default: 64)
...
line: "STATE   " text(drone_state.val) flat(#a78bfa)
line: "FUEL    " bar(drone_fuel.percent) gradient(#ef4444, #eab308, #22c55e)
```

Sending a number to a variable that held text replaces the text, and sending text replaces the number. Other gauges (`bar`, `status`, colour rules) read only the number, so a text value cannot change a colour. The `dynamic_strings` preset shows this. The `helmv.ps1` terminal simulator shows the number for a text value; the in-game script and the docs visualizer show the text.

Incoming text sets the numeric value to zero. `var(...)` and `mem(...)` are aliases for `dynamic(...)`.

### Freshness and heartbeat

Use `ttl:` to detect a stopped sender without discarding its last value. This complete layout uses HELM's implicit panel:

```helm
@bind fuel = dynamic("drone_a.fuel", default: 0, ttl: 5)
@bind state = dynamic("drone_a.state", default: "WAITING", ttl: 5)
@screen drone
output: "Bridge LCD", 0
title: "DRONE TELEMETRY"
line: "STATE " text(state.val)
line: "FUEL  " bar(fuel.percent) color(fuel.fresh < 1 : #ef4444, default: #22c55e)
line: "LINK  " checkbox(fuel.fresh)
line: "AGE   " text(fuel.age) unit("s")
```

| Field | Meaning |
| :--- | :--- |
| `.val`, `.percent` | Last numeric value, or the configured default before receiving data. `text(...)` displays text values. |
| `.age` | Seconds since the last accepted update for this key; `-1` if none has arrived. |
| `.fresh` | `100` while age is less than `ttl:`, otherwise `0`. Defaults are never fresh. |

The default timeout is 10 seconds; `ttl:` accepts positive, finite seconds, including fractions. Send a heartbeat more often than the timeout. Repeating an unchanged value refreshes its timestamp. Each key has its own timestamp, including text keys; two binds referencing the same key may use different timeouts. Age uses HELM's elapsed runtime clock. `reload` preserves values and timestamps; recompiling or restarting the script resets them. A timeout changes `.fresh`, while the last value remains available.

---

## IGC Headless Display Server Channel (`"HELM"`)

Other Programmable Blocks on your grid or across antenna networks can broadcast telemetry updates to HELM over the Space Engineers Inter-Grid Communication (IGC) system.

HELM listens for string messages tagged exactly `"HELM"` on its broadcast and unicast listeners.

### Broadcast Message Format

Send semicolon-delimited key-value pairs in a single string message:

```csharp
// In your combat or autopilot script:
IGC.SendBroadcastMessage("HELM", "radar.dist=1420;drone_a.online=1;nav.eta=45");
```

To target one HELM instance, send to its Programmable Block's entity ID:

```csharp
var helmPB = GridTerminalSystem.GetBlockWithName("HELM PB") as IMyProgrammableBlock;
if (helmPB != null)
    IGC.SendUnicastMessage(helmPB.EntityId, "HELM", "drone_a.fuel=64;drone_a.state=ESCORT");
```

Broadcast reaches listening HELM instances within IGC connectivity; unicast addresses one instance. HELM consumes and ignores unicast messages with other tags or non-string data. Use a dedicated HELM PB when sharing a grid with other scripts that receive messages.

### Payload contract

- Separate pairs with `;`, and a key from its value with `=` (`:` also works). Keys are case-insensitive; surrounding whitespace is trimmed.
- Use invariant numbers with a decimal point. `true`/`on` become `100`; `false`/`off` become `0`. Other values are text. `NaN` and infinity are ignored without refreshing the timestamp.
- Semicolons are reserved: there is no quoting or escaping. Text may contain spaces and additional equals signs. A pair without a separator is ignored.
- The last accepted update wins. `alert` is reserved for global alert state, so choose another key for a dynamic bind.

HELM accepts up to eight messages from each listener per run. Scheduled runs drain remaining messages. Displays follow the usual round-robin rendering and configured `refresh:` interval, so receiving telemetry does not redraw every LCD immediately. Message processing is bounded by message count, rather than payload length; send compact batches.

---

## Programmable Block Argument Micro-Commands

External scripts, Timer Blocks, Sensors, Event Controllers, and cockpit hotbars can manipulate HELM via argument execution (`pb.TryRun(argument)`):

### Set Variable Command (`set <var> <val>`)

```text
set radar.dist 1850
set alert_status YELLOW
set drone_a.fuel=64;drone_a.state=ESCORT
```

The batched equals form accepts the same payload as IGC. On the same grid, another script can call HELM directly:

```csharp
var helmPB = GridTerminalSystem.GetBlockWithName("HELM PB") as IMyProgrammableBlock;
bool accepted = helmPB != null && helmPB.TryRun("set drone_a.fuel=64;drone_a.state=ESCORT");
// If accepted is false, keep the update and retry on a later scheduled run.
```

Send a batch when values change and repeat it periodically as a heartbeat. The return value reports whether the PB ran; there is no reply protocol or delivery acknowledgement from HELM.

### Global Emergency Alert Commands (`alert <state>`)

Override the background alert state across HELM's configured screens:

| Command | Action |
| :--- | :--- |
| `alert red` | Flashes screen backgrounds using `alert_bg` and `alert_period`. |
| `alert on` | Turns on emergency alert mode. |
| `alert off` | Disables the global alert override; local alert rules still apply. |
| `alert toggle`| Toggles alert mode on and off. |

```csharp
// Example: Trigger Red Alert when enemy detected:
IMyProgrammableBlock helmPB = GridTerminalSystem.GetBlockWithName("HELM PB") as IMyProgrammableBlock;
helmPB?.TryRun("alert red");
```

---

## Performance & Diagnostics

Use `scan` or `diagnose` for configuration, binding, surface and runtime diagnostics. Dynamic bind entries show their payload keys and numeric values; text entries have a numeric value of zero. To isolate a delivery problem, run `set radar.dist 1420` and display the matching bind with `text(...)`. If local updates work, check the sender's tag (`HELM`), keys and target entity ID. Display `.age` and `.fresh` to detect missing heartbeats. Diagnostics do not report packet counts or a full payload log.
