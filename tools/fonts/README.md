# tools/fonts/

This directory holds TTF font files used by [`Set-TerminalFont.ps1`](../Set-TerminalFont.ps1)
for runtime terminal font switching in `helmv.ps1` and other HELM scripts.

Font files are **not committed to the repository** (`.gitignore` excludes `*.ttf`, `*.TTF`).
Use the install helper to download them automatically, or place them here manually.

---

## Quick Install (PowerShell)

```powershell
# From the repo root — downloads all three Nerd Font defaults
. "./tools/Set-TerminalFont.ps1"
Install-HelmFonts
```

Or download individual fonts:

```powershell
Install-HelmFonts -Keys JetBrainsMono, NerdFontSymbols
Install-HelmFonts -Keys SEMonospace
```

---

## Required Font Files

| Catalogue Key    | Face Name               | File                                   | Source                                                |
| :--------------- | :---------------------- | :------------------------------------- | :---------------------------------------------------- |
| `JetBrainsMono`  | JetBrainsMono NF        | `JetBrainsMonoNerdFont-Regular.ttf`    | [Nerd Fonts — JetBrainsMono](https://github.com/ryanoasis/nerd-fonts/releases/latest/download/JetBrainsMono.zip) |
| `CaskaydiaCove`  | CaskaydiaCove NF        | `CaskaydiaCoveNerdFont-Regular.ttf`    | [Nerd Fonts — CascadiaCode](https://github.com/ryanoasis/nerd-fonts/releases/latest/download/CascadiaCode.zip) |
| `NerdFontSymbols`| Symbols Nerd Font       | `SymbolsNerdFont-Regular.ttf`          | [Nerd Fonts — Symbols Only](https://github.com/ryanoasis/nerd-fonts/releases/latest/download/NerdFontsSymbolsOnly.zip) |
| `SEMonospace`    | BigBlue Terminal        | `BigBlue_Terminal_437TT.TTF`           | [int10h.org — Oldschool PC Fonts](https://int10h.org/oldschool-pc-fonts/download/) |
| `SEStandard`     | Share Tech Mono         | `ShareTechMono-Regular.ttf`            | [Google Fonts — Share Tech Mono](https://fonts.google.com/specimen/Share+Tech+Mono) |

---

## Font Licences

| Font                     | Licence     | Notes                                   |
| :----------------------- | :---------- | :-------------------------------------- |
| JetBrains Mono NF        | SIL OFL 1.1 | Free, open source                       |
| CaskaydiaCove NF         | SIL OFL 1.1 | Free, open source                       |
| Nerd Fonts Symbols Only  | SIL OFL 1.1 | Free, open source                       |
| BigBlue Terminal         | CC BY-SA 4.0| Free, attribution required              |
| Share Tech Mono          | SIL OFL 1.1 | Free, open source                       |

All fonts here are free to use and redistribute. The CC BY-SA 4.0 licence on BigBlue Terminal
requires attribution if you redistribute it — credit [int10h.org](https://int10h.org).

---

## Terminal Host Support

| Host                      | Runtime Switch | Notes                                                        |
| :------------------------ | :------------: | :----------------------------------------------------------- |
| **ConHost** (classic PS)  | ✅ Supported   | Full P/Invoke via `SetCurrentConsoleFontEx` — session only   |
| **Windows Terminal**      | ❌ Not impl.   | Font managed via `settings.json` — change manually           |
| **VSCode Terminal**       | ❌ None        | Change `terminal.integrated.fontFamily` in settings          |
| **Kitty**                 | ❌ None        | Set `font_family` in `kitty.conf`                            |
| **Alacritty**             | ❌ None        | Set `font.normal.family` in `alacritty.toml`                 |
| **WezTerm**               | ❌ None*       | Lua `user-var-changed` hook required in `wezterm.lua`        |
| **iTerm2**                | ❌ None        | Change via Profiles → Text → Font                            |
| **tmux / SSH**            | ❌ None        | Font is controlled by the outer/local terminal client        |

*WezTerm Lua hook docs will be added as `WEZTERM_SETUP.md` in a future release.
