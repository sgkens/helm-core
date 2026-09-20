---
title: "Hardware Screen Profiles"
description: "Screen profiles, resolutions and aspect ratios."
order: 5
section: "Display & Hardware"
badge: "Profiles"
---

## Supported Screen Profiles

| Profile Name | Resolution | Aspect Ratio | Mounting Clamp | Typical Game Usage |
| :--- | :--- | :--- | :--- | :--- |
| **`TextPanel`** | 512 x 512 | 1:1 Square | `[#]` Heavy Bolts | Standard 1x1 Wall LCDs, Base Control Rooms |
| **`WideLCD`** | 1024 x 512 | 2:1 Wide | `[x]` Bridge Clamps | Wide Bridge Displays, Fleet Command Consoles |
| **`CockpitSmall`** | 512 x 256 | 2:1 Compact | `[+]` Avionics Screws | Cockpit Primary HUDs, Fighter Flight Seats |
| **`CornerLCD`** | 512 x 128 | 4:1 Strip | `//` Angled Brackets | Slanted Ceiling/Floor LCD Strips, Strip Telemetry |
| **`CockpitCorner`** | 256 x 128 | 2:1 Mini | `|>` Console Mounts | Cockpit Mini Gauge Clusters, Secondary HUD Pods |
| **`Auto`** | Inferred | Dynamic | Adapts | Automatically infers dimensions from panel count |
| **`Off`** | Unbounded | Virtual | None | Headless stream output without bezels |

---

## 1. TextPanel (512x512, 1:1)

The classic Space Engineers 1x1 wall-mounted LCD panel. Offers balanced vertical and horizontal density, ideal for 2x2 or 2x3 panel grids.

```helm
@screen BasePowerGrid
output: "Control Room LCD", 0
profile: TextPanel
layout: grid(2, 2)
```

- **Visual Characteristics**: Heavy corner industrial bolt anchors (`[#]`), balanced 38-to-40 column cards.

---

## 2. WideLCD (1024x512, 2:1)

Large 2x1 ship bridge screen. Designed for high-density multi-column horizontal layouts.

```helm
@screen BridgeFleetNet
output: "Bridge Wide LCD", 0
profile: WideLCD
layout: grid(3, 2)
```

- **Visual Characteristics**: Multi-column side-by-side rendering (84+ characters wide), extended progress bars (54 columns wide in single cards), and bridge structural clamps (`[x]`).

---

## 3. CockpitSmall (512x256, 2:1)

Compact cockpit HUD fitted into the center console of fighters, rovers, and dropships.

```helm
@screen FighterHUD
output: "Fighter Cockpit", 0
profile: CockpitSmall
layout: grid(2, 1)
```

- **Visual Characteristics**: Condensed line density (capped at 3 lines per panel card) to prevent vertical overflow, with avionics corner fasteners (`[+]`).

---

## 4. CornerLCD (512x128, 4:1)

The angled LCD strip block placed along corridor ceilings or cockpit floor borders.

```helm
@screen StripStatus
output: "Corner LCD Top", 0
profile: CornerLCD
layout: grid(2, 1)
```

- **Visual Characteristics**: 2-line telemetry strips with angled frame brackets (`//` and `\\`).

---

## 5. CockpitCorner (256x128, 2:1)

The small corner instrument cluster screen embedded in cockpits and flight seats.

```helm
@screen EnginePod
output: "Cockpit Seat", 2
profile: CockpitCorner
layout: grid(1, 1)
```

- **Visual Characteristics**: Compact card layout with font-safe 1-cell width gauges and console mounts (`|>` and `<|`).

---

## Testing Profiles in the CLI Simulator

You can instantly preview any preset across all hardware profiles without opening Space Engineers:

```powershell
# Preview battery monolith on a cockpit corner pod
pwsh ./helmv.ps1 -Preset battery_monolith -ScreenProfile CockpitCorner

# Preview command carrier on a wide bridge LCD
pwsh ./helmv.ps1 -Preset command_carrier -ScreenProfile WideLCD -Measurement
```
