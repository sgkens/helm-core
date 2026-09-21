---
title: "Borders, Titles & Gradients"
description: "Border styles, panel title position and color, and gradients."
order: 10
section: "Styling"
badge: "Styling"
---

## 1. Modular Border Styles

You can set the border style at the screen level (and per panel in `helmv.ps1`):

```helm
border: <style>
```

| Style | Description | Character Set / Frame Preview |
| :--- | :--- | :--- |
| **`heavy`** | Thick box-drawing frames | `╔════╗` / `║    ║` / `╚════╝` |
| **`double`** | High-tech double line frames | `╔════╗` / `║    ║` / `╚════╝` |
| **`single`** | Clean single line borders | `┌────┐` / `│    │` / `└────┘` |
| **`rounded`** | Rounded corner frames | `╭────╮` / `│    │` / `╰────╯` |
| **`minimal`** | Space-saving dotted borders | `·────·` / `·    ·` / `·────·` |
| **`ascii`** | Strict ASCII 7-bit characters | `+----+` / `|    |` / `+----+` |
| **`graphic`** | Heavy solid block edges | `█▀▀▀▀█` / `█    █` / `█▄▄▄▄█` |
| **`none`** | Borderless clean cards | No enclosing frame |

### Where each style applies

| Renderer | Behavior |
| :--- | :--- |
| In-game script | Screen-level `border:` only. `none` removes the frame, `ascii` uses the text renderer with `+----+` frames, and every other style draws the standard sprite frame. A panel-level `border:` is parsed but ignored. |
| `helmv.ps1` | All styles, at screen and panel level. |

`border_color:`, `border_gradient:`, `title_align:` and `title_color:` work at screen and panel level in both renderers. The LCD previews in these docs come from the in-game renderer, so they show one frame for every style except `ascii` and `none`.

---

## 2. Positional Titles (`title_align:`)

Titles can be positioned anywhere along the panel's top frame:

```helm
@panel POWER at(0, 0, 1, 1) title: "SYSTEM RESERVES" title_align: center
```

- **`left`** (default): Aligns title to the top-left edge (`┌─ [ POWER ] ────────────┐`).
- **`center`**: Automatically calculates exact middle alignment (`┌──────── [ POWER ] ────────┐`).
- **`right`**: Aligns title flush against the top-right frame (`┌──────────── [ POWER ] ─┐`).

---

## 3. Independent Title Typography Colors (`title_color:`)

Decouple your title text color from the panel's border color for improved readability and high-tech contrast:

```helm
@panel RADAR at(0, 0, 1, 1) title: "TARGET RADAR" border_color: #1e293b title_color: #f43f5e
```

- In this example, the structural border is subtle dark slate (`#1e293b`), while the title glows in bright tactical red (`#f43f5e`).

---

## 4. Multi-Stop Border Gradients (`border_gradient(...)`)

Apply dynamic multi-color gradients across panel and screen borders:

```helm
@screen CommandDeck
border_gradient(#38bdf8, #818cf8, #c084fc)
```

In sprite mode (`MySprite`), the quad renderer computes linear interpolation across border vertices. In terminal mode (`helmv.ps1`), ANSI 24-bit truecolor or 256-color palettes map each border segment along the gradient curve.

---

## 5. Reusable Templates (`layouts/templates.txt`)

You can define reusable color schemes, border frames, and styling presets in `layouts/templates.txt` and reference them using `@use`:

```helm
@use template("military_hud")
```

This keeps your display layouts clean and consistent across your entire fleet.
