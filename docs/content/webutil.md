---
title: "Browser Layout Tool (webutil)"
description: "Write, preview, save and exchange HELM Custom Data in a browser using the real script renderer."
order: 9
section: "Tooling"
badge: "Alpha"
---

Webutil renders HELM Custom Data with the same compiled C# parser and sprite renderer used by the Programmable Block script. It is useful for checking a layout before opening Space Engineers. [Open webutil](/webutil/).

## Basic Workflow

1. Pick a starter layout and an LCD profile.
2. Edit the Custom Data pane. The preview updates after a short delay and shows the parser's real errors and warnings.
3. Search the block palette. Select a telemetry field to insert a compatible gauge line at the editor cursor.
4. Adjust the generated telemetry sliders to inspect low, half-full and full states.
5. Copy or export the result, paste it into the Programmable Block's Custom Data, and run `reload`.

The screen selector appears when the text contains more than one `@screen`. Its options come from the parsed layout, not a separate browser model.

## Projects, Files and Share Links

The current editor text is autosaved locally. A named project stores the text and selected screen profile in the browser. Saved projects do not leave the device.

- **Save** updates the project named beside the project picker.
- **Import** reads a `.txt` or `.helm.txt` file into the editor.
- **Export** downloads Custom Data as a plain text file.
- **Share** copies a URL whose fragment contains the Custom Data. The fragment is decoded in the recipient's browser and is not sent to the site server.

Before using an export in game, verify every block and group name. On an empty Programmable Block, run `starter` once; after replacing its Custom Data, run `reload`.

## Preview Fidelity

The parser, layout calculations, gauge drawing, conditions, colours, surface sizes and emitted sprite list come from the real script compiled to WebAssembly. The browser only converts those sprites to SVG and draws the selected Space Engineers LCD bezel.

Font widths remain approximate because the game measures its own font atlas. The tool cannot display pixels drawn by another Programmable Block script; HELM can only read another LCD's text through `line1`, `line2` and later fields.

## Keyboard and Motion

Press `?` outside an input to open the shortcut sheet. `Ctrl+S` saves a named project, `Ctrl+O` opens a text file, and `Ctrl+Shift+E` exports. The shortcut sheet also has a motion control that pauses simulated time. The page follows the operating system's reduced-motion preference on first load.

Every palette action is a button and can be reached without dragging. Full visual canvas editing, panel dragging and gauge reordering remain planned work.

## Local Development

From the repository root:

```shell
cd docs && bun run css:build && cd ../webutil
bun install
bun run build
bun run serve
```

Open `http://127.0.0.1:8891/`. See `webutil/README.md` for the generated data, WebAssembly and vendor-asset layout.
