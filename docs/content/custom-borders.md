---
title: "Borders, Titles & Gradients"
description: "Border styles, panel title position and color, and gradients."
order: 10
section: "Styling"
badge: "Styling"
---

## Modular Border Styles

Set the border style on a screen, or on a single panel by writing `border:` after its `@panel` line:

```helm
border: <style>
```

| Style | In the game (quads) | `helmv.ps1` (characters) |
| :--- | :--- | :--- |
| **`graphic`** (default), **`minimal`** | Top and bottom lines | Divider lines |
| **`frame`**, **`single`** | Lines on all four sides | `┌────┐` / `│    │` / `└────┘` |
| **`heavy`** | Four sides, twice as thick | `╔════╗` / `║    ║` / `╚════╝` |
| **`double`** | Four sides and a second frame inside | `╔════╗` / `║    ║` / `╚════╝` |
| **`rounded`** | Four sides with a circle at each corner | `╭────╮` / `│    │` / `╰────╯` |
| **`corners`** | Bracket corners only | Drawn as a plain frame |
| **`ascii`** | Text renderer with `+----+` frames | `+----+` / `|    |` / `+----+` |
| **`none`** | No frame | No frame |

### Colours

`border_color: <color>` colours the frame. `border_gradient(#a, #b, ...)` blends through the stops from left to right along the top and bottom lines; the left side takes the first stop and the right side the last. `gradient_mode: steps` uses the first stop on top and the last on the bottom.

### Where each style applies

| Renderer | Behavior |
| :--- | :--- |
| In-game script | Screen-level `border:` frames the whole screen. `border:` on a `@panel` frames that panel (`none` removes it; a panel has no frame unless it sets one). `ascii` is screen-level only and switches the screen to the text renderer, where every panel is drawn as text |
| `helmv.ps1` | All styles, at screen and panel level |

`border_color:`, `border_gradient:`, `title_align:` and `title_color:` work at screen and panel level in both renderers. The LCD previews in these docs come from the in-game renderer, so they show the frame the game draws.

---

## Positional Titles (`title_align:`)

Titles can be positioned anywhere along the panel's top frame:

```helm
@panel POWER at(0, 0, 1, 1) title: "SYSTEM RESERVES" title_align: center
```

- **`left`** (default): Aligns title to the top-left edge (`┌─ [ POWER ] ────────────┐`).
- **`center`**: Automatically calculates exact middle alignment (`┌──────── [ POWER ] ────────┐`).
- **`right`**: Aligns title flush against the top-right frame (`┌──────────── [ POWER ] ─┐`).

---

## Independent Title Typography Colors (`title_color:`)

Decouple your title text color from the panel's border color for improved readability and high-tech contrast:

```helm
@panel RADAR at(0, 0, 1, 1) title: "TARGET RADAR" border_color: #1e293b title_color: #f43f5e
```

- In this example, the structural border is subtle dark slate (`#1e293b`), while the title glows in bright tactical red (`#f43f5e`).

---

## Multi-Stop Border Gradients (`border_gradient(...)`)

Apply dynamic multi-color gradients across panel and screen borders:

```helm
@screen CommandDeck
border_gradient(#38bdf8, #818cf8, #c084fc)
```

In sprite mode (`MySprite`), the quad renderer computes linear interpolation across border vertices. In terminal mode (`helmv.ps1`), ANSI 24-bit truecolor or 256-color palettes map each border segment along the gradient curve.

---

## Reusable Templates (`layouts/templates.txt`)

You can define reusable color schemes, border frames, and styling presets in `layouts/templates.txt` and reference them using `@use`:

```helm
@use template("military_hud")
```

This keeps your display layouts clean and consistent across your entire fleet.
