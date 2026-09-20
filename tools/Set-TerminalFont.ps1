<#
.SYNOPSIS
    Reusable terminal font management utility — load, activate, and restore fonts
    within a PowerShell session without permanent system installation.

.DESCRIPTION
    Set-TerminalFont.ps1 provides a font management API for PowerShell scripts
    that need to temporarily change the active terminal font — for visual fidelity,
    TUI simulation accuracy, or Nerd Font glyph support.

    SUPPORT MATRIX
    ──────────────
    Host                       | Runtime Switch | Method                     | Notes
    ─────────────────────────────────────────────────────────────────────────────────────
    ConHost (classic PS window) | ✅  SUPPORTED  | SetCurrentConsoleFontEx    | P/Invoke, session-scoped only
    Windows Terminal (wt.exe)   | ⚠️  FRAGILE   | settings.json hot-patch    | Not implemented here; see docs
    VSCode Integrated Terminal  | ❌  NONE       | N/A                        | Change font in workspace settings
    Kitty (Linux/macOS)         | ❌  NONE       | N/A (size only via kitty@) | Font face not runtime-switchable
    Alacritty                   | ❌  NONE       | Live config reload only     | Edit alacritty.toml manually
    WezTerm                     | ❌  NONE*      | Lua user-var hook           | Requires pre-configured wezterm.lua
    iTerm2 (macOS)              | ❌  NONE       | N/A                        | No escape sequence for font face
    tmux / screen               | ❌  NONE       | Outer terminal must be set  | Multiplexer: cannot reach host font
    SSH remote session          | ❌  NONE       | Local terminal controls it  | Change font in your local client

    * WezTerm CAN do this with a custom Lua hook — see WEZTERM_SETUP.md when added.

    BUILT-IN FONT CATALOGUE (PRIMARY DEFAULTS)
    ──────────────────────────────────────────
    Key             | Face Name                   | File                                  | Purpose
    ────────────────────────────────────────────────────────────────────────────────────
    JetBrainsMono   | JetBrainsMono NF            | JetBrainsMonoNerdFont-Regular.ttf     | Primary dev monospace + NF glyphs
    CaskaydiaCove   | CaskaydiaCove NF            | CaskaydiaCoveNerdFont-Regular.ttf     | Cascadia Code NF — clean modern mono
    NerdFontSymbols | Symbols Nerd Font           | SymbolsNerdFont-Regular.ttf           | Symbols-only fallback glyph font

    SE SIMULATION FONTS (SPACE ENGINEERS VISUAL PARITY)
    ─────────────────────────────────────────────────────
    Key             | Face Name                   | Purpose
    ────────────────────────────────────────────────────
    SEMonospace     | BigBlue Terminal            | Closest match to SE "Monospace"/"Debug" font (IBM 8x14 EGA/VGA bitmap)
    SEStandard      | Share Tech Mono             | Closest match to SE "white_shadow" default proportional LCD font

    USAGE (dot-source)
    ──────────────────
    # Import into any script:
    . "$PSScriptRoot/tools/Set-TerminalFont.ps1"

    # Set font by catalogue key (uses fonts/ subdirectory):
    Set-TerminalFont -Key JetBrainsMono

    # Load two fonts (primary text + symbols fallback):
    Set-TerminalFont -Key JetBrainsMono -SecondaryKey NerdFontSymbols

    # Set font by explicit TTF path:
    Set-TerminalFont -Path "C:/Fonts/MyFont.ttf" -FaceName "My Font"

    # Get current terminal host info:
    Get-TerminalHostInfo

    # Restore original font on cleanup:
    Restore-TerminalFont

    FONT FILE SETUP
    ───────────────
    Download Nerd Font TTF files from https://github.com/ryanoasis/nerd-fonts/releases
    and place them in: tools/fonts/
      - JetBrainsMonoNerdFont-Regular.ttf
      - CaskaydiaCoveNerdFont-Regular.ttf
      - SymbolsNerdFont-Regular.ttf
    Or use -DownloadFonts to fetch them automatically.

.NOTES
    This file is designed to be dot-sourced into other scripts.
    When run directly (not dot-sourced), it prints the terminal host info and exits.

.LINK
    https://www.nerdfonts.com/
    https://www.jetbrains.com/lp/mono/
    https://int10h.org/oldschool-pc-fonts/ (BigBlue Terminal)
#>

# ---------------------------------------------------------------------------
# Guard: prevent re-definition if already dot-sourced
# ---------------------------------------------------------------------------
if (Get-Variable -Name 'HelmFontUtilLoaded' -Scope Script -ErrorAction SilentlyContinue) { return }
$script:HelmFontUtilLoaded = $true

# ---------------------------------------------------------------------------
# Module-scope state
# ---------------------------------------------------------------------------
$script:OriginalFontState   = $null
$script:LoadedFontPaths     = [System.Collections.Generic.List[string]]::new()
$script:FontsDir            = Join-Path $PSScriptRoot "fonts"

# ---------------------------------------------------------------------------
# Built-in font catalogue
# Key → @{ FaceName; FileName; Description; License; Url }
# ---------------------------------------------------------------------------
$script:FontCatalogue = [ordered]@{

    # ── Primary Nerd Font defaults ────────────────────────────────────────
    JetBrainsMono = @{
        FaceName    = 'JetBrainsMono NF'
        FileName    = 'JetBrainsMonoNerdFont-Regular.ttf'
        Description = 'JetBrains Mono Nerd Font Regular — primary developer monospace with full NF glyph set'
        License     = 'SIL OFL 1.1'
        Url         = 'https://github.com/ryanoasis/nerd-fonts/releases/latest/download/JetBrainsMono.zip'
        NfRelease   = 'JetBrainsMono'
    }
    CaskaydiaCove = @{
        FaceName    = 'CaskaydiaCove NF'
        FileName    = 'CaskaydiaCoveNerdFont-Regular.ttf'
        Description = 'Cascadia Code Nerd Font (CaskaydiaCove NF) — clean modern monospace with NF glyphs'
        License     = 'SIL OFL 1.1'
        Url         = 'https://github.com/ryanoasis/nerd-fonts/releases/latest/download/CascadiaCode.zip'
        NfRelease   = 'CascadiaCode'
    }
    NerdFontSymbols = @{
        FaceName    = 'Symbols Nerd Font'
        FileName    = 'SymbolsNerdFont-Regular.ttf'
        Description = 'Nerd Fonts Symbols Only — supplemental icon/glyph font for terminal fallback'
        License     = 'SIL OFL 1.1'
        Url         = 'https://github.com/ryanoasis/nerd-fonts/releases/latest/download/NerdFontsSymbolsOnly.zip'
        NfRelease   = 'NerdFontsSymbolsOnly'
    }

    # ── Space Engineers visual simulation fonts ───────────────────────────
    SEMonospace = @{
        FaceName    = 'BigBlue Terminal'
        FileName    = 'BigBlue_Terminal_437TT.TTF'
        Description = 'BigBlue Terminal — closest real-world match to SE Monospace/Debug bitmap font (IBM 8x14 EGA/VGA)'
        License     = 'CC BY-SA 4.0'
        Url         = 'https://int10h.org/oldschool-pc-fonts/download/'
        NfRelease   = $null
    }
    SEStandard = @{
        FaceName    = 'Share Tech Mono'
        FileName    = 'ShareTechMono-Regular.ttf'
        Description = 'Share Tech Mono — closest match to SE white_shadow default proportional LCD font'
        License     = 'SIL OFL 1.1'
        Url         = 'https://fonts.google.com/specimen/Share+Tech+Mono'
        NfRelease   = $null
    }
}

# ---------------------------------------------------------------------------
# Win32 P/Invoke definitions (Windows ConHost only)
# Compiled once per session via Add-Type
# ---------------------------------------------------------------------------
if ($IsWindows -and -not ('HelmFontApi.ConHostFont' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Runtime.InteropServices;

namespace HelmFontApi {
    [StructLayout(LayoutKind.Sequential)]
    public struct COORD {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CONSOLE_FONT_INFOEX {
        public uint   cbSize;
        public uint   nFont;
        public COORD  dwFontSize;
        public int    FontFamily;
        public int    FontWeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FaceName;
    }

    public static class ConHostFont {
        // ── Console font API ────────────────────────────────────────────
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool GetCurrentConsoleFontEx(
            IntPtr hConsoleOutput,
            bool   bMaximumWindow,
            ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetCurrentConsoleFontEx(
            IntPtr hConsoleOutput,
            bool   bMaximumWindow,
            ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);

        // ── GDI font resource API (loads TTF into system table for ConHost) ──
        // NOTE: AddFontResource (non-private) is required — ConHost.exe is a
        // separate process and cannot see FR_PRIVATE fonts loaded by pwsh.exe.
        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern int AddFontResource(string lpFileName);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool RemoveFontResource(string lpFileName);

        // ── Broadcast WM_FONTCHANGE so ConHost picks up newly registered font ──
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(
            IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public const int  STD_OUTPUT_HANDLE = -11;
        public const uint WM_FONTCHANGE     = 0x001D;
        public const int  HWND_BROADCAST    = 0xFFFF;

        // TMPF_TRUETYPE | TMPF_VECTOR | FF_DONTCARE
        public const int  FONT_FAMILY_TRUETYPE = 54;
        public const int  FONT_WEIGHT_NORMAL   = 400;

        public static bool GetFont(out CONSOLE_FONT_INFOEX info) {
            info = new CONSOLE_FONT_INFOEX();
            info.cbSize = (uint)Marshal.SizeOf(info);
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            return GetCurrentConsoleFontEx(handle, false, ref info);
        }

        public static bool SetFont(string faceName, short height, int weight = FONT_WEIGHT_NORMAL) {
            var info = new CONSOLE_FONT_INFOEX();
            info.cbSize     = (uint)Marshal.SizeOf(info);
            info.nFont      = 0;
            info.dwFontSize = new COORD { X = 0, Y = height };
            info.FontFamily = FONT_FAMILY_TRUETYPE;
            info.FontWeight = weight;
            info.FaceName   = faceName;
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            return SetCurrentConsoleFontEx(handle, false, ref info);
        }

        public static int RegisterFont(string path) {
            int added = AddFontResource(path);
            // Broadcast WM_FONTCHANGE so all windows (including ConHost) notice
            SendMessage((IntPtr)HWND_BROADCAST, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
            return added;
        }

        public static bool UnregisterFont(string path) {
            bool removed = RemoveFontResource(path);
            SendMessage((IntPtr)HWND_BROADCAST, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
            return removed;
        }
    }
}
'@ -Language CSharp
}

# ===========================================================================
# PUBLIC API
# ===========================================================================

# ---------------------------------------------------------------------------
# Get-TerminalHostInfo
#   Returns a PSCustomObject describing the current terminal host and whether
#   runtime font switching is supported.
# ---------------------------------------------------------------------------
function Get-TerminalHostInfo {
    [CmdletBinding()]
    [OutputType([PSCustomObject])]
    param()

    $info = [ordered]@{
        HostName          = 'Unknown'
        IsWindows         = ($IsWindows -or -not (Get-Variable IsWindows -ErrorAction SilentlyContinue))
        IsConHost         = $false
        Supported         = $false
        Method            = 'None'
        Notes             = ''
        KnownEnvMarkers   = @()
    }

    if ($info.IsWindows) {
        if ($env:WT_SESSION) {
            $info.HostName        = 'WindowsTerminal'
            $info.Supported       = $false
            $info.Method          = 'SettingsJsonPatch'
            $info.Notes           = 'Windows Terminal: runtime font switching not implemented. Edit settings.json → font.face.'
            $info.KnownEnvMarkers = @('WT_SESSION')
        } elseif ($env:TERM_PROGRAM -eq 'vscode') {
            $info.HostName        = 'VSCode'
            $info.Supported       = $false
            $info.Method          = 'None'
            $info.Notes           = 'VSCode integrated terminal: change font via settings.json → terminal.integrated.fontFamily.'
            $info.KnownEnvMarkers = @('TERM_PROGRAM=vscode')
        } elseif ($env:ConEmuPID) {
            $info.HostName        = 'ConEmu'
            $info.Supported       = $false
            $info.Method          = 'None'
            $info.Notes           = 'ConEmu: font managed by ConEmu settings. No runtime API.'
            $info.KnownEnvMarkers = @('ConEmuPID')
        } else {
            # Assume classic ConHost (powershell.exe or pwsh.exe blue window)
            $info.HostName        = 'ConHost'
            $info.IsConHost       = $true
            $info.Supported       = $true
            $info.Method          = 'SetCurrentConsoleFontEx'
            $info.Notes           = 'Classic Windows Console Host: P/Invoke SetCurrentConsoleFontEx — session-scoped, no system changes persist.'
        }
    } else {
        # Unix-like
        if ($env:TMUX) {
            $info.HostName        = "tmux"
            $info.Supported       = $false
            $info.Method          = 'None'
            $info.Notes           = 'tmux multiplexer: font must be changed on the outer terminal host.'
            $info.KnownEnvMarkers = @('TMUX')
        } elseif ($env:KITTY_PID -or $env:TERM -eq 'xterm-kitty') {
            $info.HostName        = 'Kitty'
            $info.Supported       = $false
            $info.Method          = 'None'
            $info.Notes           = 'Kitty: no runtime font face switching. Size only via "kitty @ set-font-size". Set font in kitty.conf.'
            $info.KnownEnvMarkers = @('KITTY_PID', 'TERM=xterm-kitty')
        } elseif ($env:WEZTERM_PANE) {
            $info.HostName        = 'WezTerm'
            $info.Supported       = $false
            $info.Method          = 'LuaUserVar'
            $info.Notes           = 'WezTerm: requires wezterm.lua user-var-changed hook. Emit: printf "\033]1337;SetUserVar=helm_font=<name>\007"'
            $info.KnownEnvMarkers = @('WEZTERM_PANE')
        } elseif ($env:ITERM_SESSION_ID) {
            $info.HostName        = 'iTerm2'
            $info.Supported       = $false
            $info.Method          = 'None'
            $info.Notes           = 'iTerm2: no escape sequence for runtime font face. Change via Profiles → Text → Font.'
            $info.KnownEnvMarkers = @('ITERM_SESSION_ID')
        } elseif ($env:TERM_PROGRAM -eq 'vscode') {
            $info.HostName        = 'VSCode'
            $info.Supported       = $false
            $info.Method          = 'None'
            $info.Notes           = 'VSCode integrated terminal: change font via settings.json → terminal.integrated.fontFamily.'
            $info.KnownEnvMarkers = @('TERM_PROGRAM=vscode')
        } elseif ($env:ALACRITTY_WINDOW_ID -or $env:TERM -eq 'alacritty') {
            $info.HostName        = 'Alacritty'
            $info.Supported       = $false
            $info.Method          = 'LiveConfigReload'
            $info.Notes           = 'Alacritty: edit alacritty.toml font.normal.family. Enable live_config_reload = true for hot-reload.'
            $info.KnownEnvMarkers = @('ALACRITTY_WINDOW_ID')
        } else {
            $info.HostName        = 'GenericUnix'
            $info.Supported       = $false
            $info.Method          = 'None'
            $info.Notes           = 'Generic Unix terminal: no standard runtime font-face switching. Set font in your terminal emulator preferences.'
        }

        if ($env:SSH_CLIENT -or $env:SSH_TTY) {
            $info.Notes           = "SSH remote session: $($info.Notes) Font is controlled by the LOCAL terminal client."
            $info.KnownEnvMarkers += 'SSH_CLIENT'
        }
    }

    return [PSCustomObject]$info
}

# ---------------------------------------------------------------------------
# Get-FontCatalogue
#   Lists all built-in fonts with availability status.
# ---------------------------------------------------------------------------
function Get-FontCatalogue {
    [CmdletBinding()]
    param(
        [switch]$CheckFiles
    )

    $rows = foreach ($key in $script:FontCatalogue.Keys) {
        $f = $script:FontCatalogue[$key]
        $path = Join-Path $script:FontsDir $f.FileName
        $avail = if ($CheckFiles) { Test-Path $path } else { $null }
        [PSCustomObject][ordered]@{
            Key         = $key
            FaceName    = $f.FaceName
            FileName    = $f.FileName
            FileFound   = $avail
            License     = $f.License
            Description = $f.Description
        }
    }
    return $rows
}

# ---------------------------------------------------------------------------
# Get-FontPath
#   Resolves the TTF path for a catalogue key. Checks tools/fonts/ first,
#   then system font directories.
# ---------------------------------------------------------------------------
function Get-FontPath {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Key
    )

    if (-not $script:FontCatalogue.Contains($key)) {
        throw "Unknown font key '$Key'. Valid keys: $($script:FontCatalogue.Keys -join ', ')"
    }

    $entry    = $script:FontCatalogue[$Key]
    $localPath = Join-Path $script:FontsDir $entry.FileName
    if (Test-Path $localPath) { return (Resolve-Path $localPath).Path }

    # Check system font directories
    $sysDirs = if ($IsWindows) {
        @("$env:WINDIR\Fonts", "$env:LOCALAPPDATA\Microsoft\Windows\Fonts")
    } elseif ($IsMacOS) {
        @("$env:HOME/Library/Fonts", "/Library/Fonts", "/System/Library/Fonts")
    } else {
        @("$env:HOME/.local/share/fonts", "$env:HOME/.fonts", "/usr/share/fonts", "/usr/local/share/fonts")
    }

    foreach ($dir in $sysDirs) {
        $candidate = Join-Path $dir $entry.FileName
        if (Test-Path $candidate) { return (Resolve-Path $candidate).Path }
    }

    return $null   # not found
}

# ---------------------------------------------------------------------------
# Install-HelmFonts
#   Downloads Nerd Font TTF files from GitHub releases into tools/fonts/.
#   Only downloads fonts that have a NfRelease key set in the catalogue.
# ---------------------------------------------------------------------------
function Install-HelmFonts {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [string[]]$Keys,           # specific keys; omit for all NF catalogue fonts
        [switch]$Force             # re-download even if file already exists
    )

    if (-not (Test-Path $script:FontsDir)) {
        New-Item -ItemType Directory -Force -Path $script:FontsDir | Out-Null
    }

    $targets = if ($Keys) { $Keys } else { $script:FontCatalogue.Keys | Where-Object { $script:FontCatalogue[$_].NfRelease } }

    foreach ($key in $targets) {
        $entry = $script:FontCatalogue[$key]
        if (-not $entry) { Write-Warning "Unknown font key '$key' — skipped."; continue }
        if (-not $entry.NfRelease) {
            Write-Warning "$key ($($entry.FaceName)): no NfRelease URL — download manually from $($entry.Url)"
            continue
        }

        $destFile = Join-Path $script:FontsDir $entry.FileName
        if ((Test-Path $destFile) -and -not $Force) {
            Write-Host "  $key : already present — $($entry.FileName)" -ForegroundColor DarkGray
            continue
        }

        # Download the release zip, extract the specific TTF
        $zipUrl  = "https://github.com/ryanoasis/nerd-fonts/releases/latest/download/$($entry.NfRelease).zip"
        $tmpZip  = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "$($entry.NfRelease).zip")
        $tmpDir  = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "nf_$($entry.NfRelease)")

        if ($PSCmdlet.ShouldProcess($zipUrl, "Download Nerd Font zip")) {
            Write-Host "  $key : downloading $($entry.NfRelease).zip ..." -ForegroundColor Yellow
            try {
                Invoke-WebRequest -Uri $zipUrl -OutFile $tmpZip -UseBasicParsing
                if (Test-Path $tmpDir) { Remove-Item $tmpDir -Recurse -Force }
                Expand-Archive -Path $tmpZip -DestinationPath $tmpDir -Force

                # Find the specific TTF in the extracted contents
                $found = Get-ChildItem $tmpDir -Filter $entry.FileName -Recurse | Select-Object -First 1
                if ($found) {
                    Copy-Item $found.FullName $destFile -Force
                    Write-Host "  $key : installed → $($entry.FileName)" -ForegroundColor Green
                } else {
                    # Try loose filename match (some releases use different casing)
                    $loose = Get-ChildItem $tmpDir -Recurse -Include "*.ttf","*.TTF" |
                             Where-Object { $_.Name -like "*Regular*" -and $_.Name -notlike "*Italic*" -and $_.Name -notlike "*Bold*" } |
                             Select-Object -First 1
                    if ($loose) {
                        Copy-Item $loose.FullName $destFile -Force
                        Write-Host "  $key : installed (loose match: $($loose.Name)) → $($entry.FileName)" -ForegroundColor Yellow
                    } else {
                        Write-Warning "  $key : '$($entry.FileName)' not found in zip. Download manually: $($entry.Url)"
                    }
                }
            } catch {
                Write-Warning "  $key : download failed — $_"
            } finally {
                if (Test-Path $tmpZip) { Remove-Item $tmpZip -Force -ErrorAction SilentlyContinue }
                if (Test-Path $tmpDir) { Remove-Item $tmpDir -Recurse -Force -ErrorAction SilentlyContinue }
            }
        }
    }
}

# ---------------------------------------------------------------------------
# Set-TerminalFont
#   Activates a terminal font for the current session.
#   Saves original font state so Restore-TerminalFont can undo the change.
#
#   On ConHost:  AddFontResource → SetCurrentConsoleFontEx
#   On others:   Prints a setup guide and returns $false
# ---------------------------------------------------------------------------
function Set-TerminalFont {
    [CmdletBinding(DefaultParameterSetName = 'ByKey')]
    [OutputType([bool])]
    param(
        # ── By catalogue key ────────────────────────────────────────────────
        [Parameter(ParameterSetName = 'ByKey', Position = 0)]
        [ValidateScript({ $script:FontCatalogue.Contains($_) })]
        [string]$Key = 'JetBrainsMono',

        # Optional secondary font to register (e.g., NerdFontSymbols for glyph fallback)
        [Parameter(ParameterSetName = 'ByKey')]
        [ValidateScript({ -not $_ -or $script:FontCatalogue.Contains($_) })]
        [string]$SecondaryKey,

        # ── By explicit path + face name ────────────────────────────────────
        [Parameter(ParameterSetName = 'ByPath', Mandatory)]
        [string]$Path,

        [Parameter(ParameterSetName = 'ByPath', Mandatory)]
        [string]$FaceName,

        [Parameter(ParameterSetName = 'ByPath')]
        [string]$SecondaryPath,

        # ── Common options ──────────────────────────────────────────────────
        [ValidateRange(8, 72)]
        [short]$Size = 16,           # font height in pixels for ConHost

        [ValidateSet('Normal', 'Bold')]
        [string]$Weight = 'Normal',

        [switch]$Quiet,              # suppress informational output
        [switch]$Force               # attempt even on unsupported hosts (no-op, just logs)
    )

    $hostInfo = Get-TerminalHostInfo

    # ── Unsupported host path ───────────────────────────────────────────────
    if (-not $hostInfo.Supported) {
        if (-not $Quiet) {
            Write-Host ""
            Write-Host "  ⚠  Terminal font switching not supported on $($hostInfo.HostName)" -ForegroundColor Yellow
            Write-Host "     $($hostInfo.Notes)" -ForegroundColor DarkGray

            # Print font suggestion for manual setup
            if ($PSCmdlet.ParameterSetName -eq 'ByKey' -and $script:FontCatalogue.Contains($Key)) {
                $entry = $script:FontCatalogue[$Key]
                Write-Host ""
                Write-Host "     For best visual accuracy, set your terminal font to:" -ForegroundColor Cyan
                Write-Host "       Face   : $($entry.FaceName)" -ForegroundColor White
                Write-Host "       File   : $($entry.FileName)" -ForegroundColor White
                Write-Host "       Source : $($entry.Url)" -ForegroundColor DarkGray
            }
            Write-Host ""
        }
        return $false
    }

    # ── ConHost: P/Invoke path ──────────────────────────────────────────────
    if ($hostInfo.IsConHost) {
        # Save original font state (once per session)
        if (-not $script:OriginalFontState) {
            $orig = New-Object HelmFontApi.CONSOLE_FONT_INFOEX
            [HelmFontApi.ConHostFont]::GetFont([ref]$orig) | Out-Null
            $script:OriginalFontState = $orig
            if (-not $Quiet) {
                Write-Host "  Saved original font: $($orig.FaceName) @ $($orig.dwFontSize.Y)px" -ForegroundColor DarkGray
            }
        }

        # Resolve primary font path
        $primaryPath = $null
        $primaryFace = $null

        if ($PSCmdlet.ParameterSetName -eq 'ByKey') {
            $primaryPath = Get-FontPath -Key $Key
            $primaryFace = $script:FontCatalogue[$Key].FaceName
            if (-not $primaryPath) {
                Write-Warning "Font file not found for key '$Key' ($primaryFace). Run: Install-HelmFonts -Keys $Key"
                Write-Warning "Attempting to use face name '$primaryFace' if already system-installed..."
            }
        } else {
            $primaryPath = $Path
            $primaryFace = $FaceName
        }

        # Register primary TTF with GDI system table
        if ($primaryPath -and (Test-Path $primaryPath)) {
            $added = [HelmFontApi.ConHostFont]::RegisterFont($primaryPath)
            if ($added -gt 0) {
                $script:LoadedFontPaths.Add($primaryPath)
                if (-not $Quiet) {
                    Write-Host "  Registered: $primaryFace ($([System.IO.Path]::GetFileName($primaryPath)))" -ForegroundColor DarkGray
                }
            } else {
                Write-Warning "AddFontResource returned 0 for '$primaryPath' — font may already be registered or path is invalid."
            }
        }

        # Register secondary font (symbols / glyph fallback)
        $secondPath = $null
        if ($PSCmdlet.ParameterSetName -eq 'ByKey' -and $SecondaryKey) {
            $secondPath = Get-FontPath -Key $SecondaryKey
            $secondFace = $script:FontCatalogue[$SecondaryKey].FaceName
        } elseif ($PSCmdlet.ParameterSetName -eq 'ByPath' -and $SecondaryPath) {
            $secondPath = $SecondaryPath
            $secondFace = "Secondary"
        }

        if ($secondPath -and (Test-Path $secondPath)) {
            $added2 = [HelmFontApi.ConHostFont]::RegisterFont($secondPath)
            if ($added2 -gt 0) {
                $script:LoadedFontPaths.Add($secondPath)
                if (-not $Quiet) {
                    Write-Host "  Registered: $secondFace ($([System.IO.Path]::GetFileName($secondPath)))" -ForegroundColor DarkGray
                }
            }
        } elseif ($secondPath) {
            Write-Warning "Secondary font not found: $secondPath"
        }

        # Activate the primary font via SetCurrentConsoleFontEx
        $fontWeight = if ($Weight -eq 'Bold') { 700 } else { 400 }
        $ok = [HelmFontApi.ConHostFont]::SetFont($primaryFace, $Size, $fontWeight)

        if ($ok) {
            if (-not $Quiet) {
                Write-Host "  Active font: $primaryFace @ $($Size)px  [$($hostInfo.Method)]" -ForegroundColor Green
            }
            return $true
        } else {
            $err = [System.Runtime.InteropServices.Marshal]::GetLastWin32Error()
            Write-Warning "SetCurrentConsoleFontEx failed (Win32 error $err). Font '$primaryFace' may not be recognized by ConHost."
            Write-Warning "Ensure the font is registered or installed. Try 'Install-HelmFonts' first."
            return $false
        }
    }

    return $false
}

# ---------------------------------------------------------------------------
# Restore-TerminalFont
#   Restores the original terminal font saved by Set-TerminalFont, and
#   unregisters any TTF files loaded during the session.
# ---------------------------------------------------------------------------
function Restore-TerminalFont {
    [CmdletBinding()]
    param(
        [switch]$Quiet
    )

    if (-not $IsWindows) { return }

    # Unregister all loaded font files from GDI system table
    foreach ($path in $script:LoadedFontPaths) {
        [HelmFontApi.ConHostFont]::UnregisterFont($path) | Out-Null
        if (-not $Quiet) {
            Write-Host "  Unregistered: $([System.IO.Path]::GetFileName($path))" -ForegroundColor DarkGray
        }
    }
    $script:LoadedFontPaths.Clear()

    # Restore original console font
    if ($script:OriginalFontState) {
        $orig = $script:OriginalFontState
        [HelmFontApi.ConHostFont]::SetFont($orig.FaceName, $orig.dwFontSize.Y, $orig.FontWeight) | Out-Null
        if (-not $Quiet) {
            Write-Host "  Restored font: $($orig.FaceName) @ $($orig.dwFontSize.Y)px" -ForegroundColor DarkGray
        }
        $script:OriginalFontState = $null
    }
}

# ---------------------------------------------------------------------------
# Show-TerminalFontStatus
#   Prints the terminal host detection results and font catalogue to the console.
# ---------------------------------------------------------------------------
function Show-TerminalFontStatus {
    [CmdletBinding()]
    param(
        [switch]$ShowCatalogue
    )

    $ESC = [char]27

    $hostInfo = Get-TerminalHostInfo
    $supportedColor = if ($hostInfo.Supported) { "${ESC}[32m" } else { "${ESC}[33m" }
    $reset = "${ESC}[0m"
    $dim   = "${ESC}[2m"
    $bold  = "${ESC}[1m"
    $cyan  = "${ESC}[36m"

    Write-Host ""
    Write-Host "  ${bold}Terminal Font Status${reset}"
    Write-Host "  $('─' * 50)"
    Write-Host "  Host      : ${cyan}$($hostInfo.HostName)${reset}"
    Write-Host "  Supported : ${supportedColor}$($hostInfo.Supported)${reset}"
    Write-Host "  Method    : $($hostInfo.Method)"
    Write-Host "  Notes     : ${dim}$($hostInfo.Notes)${reset}"
    Write-Host ""

    if ($IsWindows -and $hostInfo.IsConHost) {
        $current = New-Object HelmFontApi.CONSOLE_FONT_INFOEX
        [HelmFontApi.ConHostFont]::GetFont([ref]$current) | Out-Null
        Write-Host "  Current font : $($current.FaceName) @ $($current.dwFontSize.Y)px  weight=$($current.FontWeight)"
        Write-Host ""
    }

    if ($ShowCatalogue) {
        Write-Host "  ${bold}Font Catalogue${reset}"
        Write-Host "  $('─' * 50)"
        $catalogue = Get-FontCatalogue -CheckFiles
        foreach ($row in $catalogue) {
            $avail = if ($row.FileFound -eq $true) { "${ESC}[32m✓${reset}" }
                     elseif ($row.FileFound -eq $false) { "${ESC}[33m✗${reset}" }
                     else { "${ESC}[2m?${reset}" }
            Write-Host "  ${cyan}$($row.Key.PadRight(18))${reset} $avail  $($row.FaceName.PadRight(28)) $dim$($row.FileName)$reset"
        }
        Write-Host ""
        Write-Host "  ${dim}Run Install-HelmFonts to download missing Nerd Font TTF files.$reset"
        Write-Host ""
    }
}

# ===========================================================================
# STANDALONE EXECUTION (not dot-sourced)
# ===========================================================================
# When run directly (pwsh ./tools/Set-TerminalFont.ps1), print status and help.
if ($MyInvocation.InvocationName -ne '.' -and
    $MyInvocation.Line -notmatch '^\s*\.\s+') {

    Show-TerminalFontStatus -ShowCatalogue

    Write-Host "  Usage (dot-source into other scripts):"    -ForegroundColor Cyan
    Write-Host '    . "./tools/Set-TerminalFont.ps1"'
    Write-Host '    Set-TerminalFont -Key JetBrainsMono -SecondaryKey NerdFontSymbols'
    Write-Host '    Set-TerminalFont -Key SEMonospace'
    Write-Host '    Restore-TerminalFont'
    Write-Host '    Install-HelmFonts'
    Write-Host ""
}
