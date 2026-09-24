---
title: "Browser Layout Tool (webutil)"
description: "Write, preview, save and exchange HELM Custom Data in a browser using the real script renderer."
order: 9
section: "Tooling"
badge: "Alpha"
---

Webutil renders HELM Custom Data with the same compiled C# parser and sprite renderer used by the Programmable Block script. It is useful for checking a layout before opening Space Engineers. [Open webutil](/webutil/).

## Basic Workflow

1. Pick one of the layout presets and an LCD profile. The picker includes every layout in `layouts/default/` and groups them by the same categories as the preset gallery.
2. Edit the Custom Data pane. Prism highlights the HELM DSL with the same grammar and selected code theme as the docs; the preview updates after a short delay and shows the parser's real errors and warnings.
3. Search the block palette. Select a telemetry field or component to insert its example at the editor cursor. Item and component examples also add any missing `@bind` aliases near the top of Custom Data without changing existing binds. Replace the suggested block/group names with the names used in your grid.
4. Adjust the generated telemetry sliders to inspect low, half-full and full states.
5. Copy or export the result, paste it into the Programmable Block's Custom Data, and run `reload`.

Drag the divider above Custom Data to resize the editor and preview together. The SVG display scales into the remaining preview space. The divider also accepts the Up and Down arrow keys, while **Expand editor** provides a quick full-height editing view and restores the preview when selected again. Webutil remembers the chosen height in the browser.

The screen selector appears when the text contains more than one `@screen`. Its options come from the parsed layout, not a separate browser model.

## Visual Configuration Inspector

The right-hand inspector keeps Custom Data as the source of truth while making the common layout work form-driven:

- **Base** creates or selects a screen and edits its LCD output, surface, profile, grid, font, spacing, border and colours. **New** starts from a valid screen, panel, bind and component instead of an unexplained blank preview.
- **Panel** adds and selects panels, then edits their title, grid position and span, orientation, border and colours.
- **Component** selects a `line:` in the current panel and edits its label, gauge, bind reference, flat or gradient colours, size, direction, unit, label colour and track colour.
- **Telemetry** contains the simulated values used only by the preview.

Choose a panel before inserting from Blocks, Items or Components. Webutil puts the new line in that panel, adds a missing `@bind` where needed, and opens the Component inspector for immediate adjustment.

Each control names the HELM directive it changes, and every change remains visible in the highlighted Custom Data pane. The inspector rewrites only the selected directive or line: comments, templates, conditional colour rules and unknown future directives pass through unchanged. Complex custom `line:` syntax that cannot be read safely remains in the text and is marked read-only in the inspector, so the visual editor never flattens it.

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

For self-hosting, run `bun run build` at the repository root and serve the resulting `public/` directory. WebUtil intentionally remains beside the documentation because it shares the docs CSS, syntax theme, renderer assets and generated reference sources.
