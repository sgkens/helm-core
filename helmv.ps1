#!/usr/bin/env pwsh
<#
.SYNOPSIS
    HELM Visualize (helmv) — High-Performance TUI Telemetry Visualizer & Interactive Console Simulator.
.DESCRIPTION
    Renders HELM DSL configurations in terminal using pure ANSI escape codes.
    Follows the strict Padding Rule: calculates visual character lengths before encoding
    to completely eliminate screen tear and ensure perfect border alignment.
    Supports -CustomPreset (inline DSL string or path to a .ps1/.txt file), modular border styles
    (-Border), positional title alignment (-TitleAlign), and Space Engineers hardware screen simulation (-ScreenProfile).
#>

[CmdletBinding(DefaultParameterSetName = 'Builtin')]
param(
    [Parameter(ParameterSetName = 'Builtin', Position = 0)]
    [ValidateSet('rover', 'airlock', 'bridge', 'hazard_demo', 'mining', 'fighter', 'cargo', 'monolith',
                 'bridge_matrix', 'cargo_overview', 'battery_monolith', 'reactor_monolith', 'cargo_monolith',
                 'tactical_defense', 'thruster_balance', 'split_flight_deck', 'refinery_monolith',
                 'jump_drive_spool', 'medical_life_support', 'rover_cockpit', 'airlock_security',
                 'mining_rig', 'fighter_hud', 'hazard_alert', 'assembler_monolith', 'dynamic_telemetry',
                 'planetary_base', 'command_carrier', 'survival_essentials', 'deep_space_explorer',
                 'mining_fleet_coordinator', 'atmospheric_lander',
                 'single_line', 'power_only', 'cargo_minimal', 'corner_gauge_cluster', 'all_gauges_showcase', 'long_labels', 'numeric_extremes', 'border_styles_gallery', 'title_alignment',
                 'template_stamp', 'multi_screen_one_pb', 'grid_4x4', 'panel_spans', 'transparent_bg', 'gradient_stress', 'units_all', 'boot_custom', 'alert_states', 'hangar_bay', 'solar_farm', 'piston_crane', 'docking_control', 'hydrogen_plant', 'sensor_security', 'trading_station', 'missing_binds', 'template_nested', 'dynamic_strings', 'background_lab', 'cargo_contents', 'fighter_ammo', 'startup_banner', 'projector_welder', 'rover_suspension', 'top_contents', 'tank_fullscreen', 'tank_trio', 'vertical_bars', 'bar_sizes', 'combo_dashboard')]
    [string]$Preset = 'bridge',

    [Parameter(ParameterSetName = 'Custom', Mandatory = $true)]
    [string]$CustomPreset,

    [Parameter()]
    [switch]$Boot,

    [Parameter()]
    [switch]$RedAlert,

    [Parameter()]
    [double]$BootDuration,

    [Parameter()]
    [int]$Frames = 60,

    [Parameter()]
    [ValidateSet('1x', '10x', '100x', 'Custom')]
    [string]$UpdateFrequency = '10x',

    [Parameter()]
    [int]$IntervalMs,

    [Parameter()]
    [switch]$Interactive,

    [Parameter()]
    [switch]$StrictSEAscii,

    [Parameter()]
    [switch]$DampingLerp = $true,

    [Parameter()]
    [switch]$ShowSprites,

    [Parameter()]
    [switch]$ShowStats,

    [Parameter()]
    [switch]$SingleColumn,

    [Parameter()]
    [switch]$MultiColumn,

    [Parameter()]
    [ValidateSet('Graphic', 'Ascii', 'Auto')]
    [string]$RenderMode = 'Auto',

    # ── Telemetry Value Formatting: Percent vs. Measurement Units ─────────
    # Measurement: Formats values as engineering units (MW, GW, kW, kL, m/s, km, etc.)
    # Percent:     Forces standard percentage readout (xx.x%) across all gauges
    [Parameter()]
    [Alias('Measurements', 'Units', 'UseUnits')]
    [switch]$Measurement,

    [Parameter()]
    [Alias('Percentage', 'PercentMode')]
    [switch]$Percent,

    # ── Space Engineers Hardware Screen Simulation ─────────────────────────
    # Simulates exact aspect ratios, pixel grids, and industrial bezels:
    #   TextPanel     (512x512, 1:1 Standard Wall LCD)
    #   WideLCD       (1024x512, 2:1 Ultra-Wide Screen)
    #   CockpitSmall  (512x256, Compact Instrument / Cockpit HUD)
    #   CornerLCD     (512x128, Slanted Corner LCD strip)
    #   CockpitCorner (256x128, Mini Gauge / Indicator HUD)
    #   Auto          (Infers best aspect ratio from screen layout & profile)
    [Parameter()]
    [ValidateSet('Auto', 'TextPanel', 'WideLCD', 'CockpitSmall', 'CornerLCD', 'CockpitCorner', 'Off')]
    [string]$ScreenProfile = 'Auto',

    # ── Font mode: Space Engineers visual simulation ─────────────────────
    # Requires tools/Set-TerminalFont.ps1 and font files in tools/fonts/.
    # Only active on supported terminal hosts (ConHost on Windows).
    # Monospace  → BigBlue Terminal (closest to SE Monospace/Debug bitmap font)
    # Standard   → Share Tech Mono  (closest to SE white_shadow default LCD font)
    # JetBrains  → JetBrains Mono Nerd Font + Symbols NF (developer default pair)
    # Cascadia   → CaskaydiaCove Nerd Font  + Symbols NF
    # Off        → no font switching (default, preserves terminal settings)
    [Parameter()]
    [ValidateSet('Monospace', 'Standard', 'JetBrains', 'Cascadia', 'Off')]
    [string]$FontMode = 'Off',

    [Parameter()]
    [ValidateRange(8, 72)]
    [int]$FontSize = 16,

    # ── Modular Border & Alignment CLI Overrides ──────────────────────────
    [Parameter()]
    [ValidateSet('default', 'heavy', 'double', 'single', 'rounded', 'minimal', 'ascii', 'graphic', 'none')]
    [string]$Border = 'default',

    [Parameter()]
    [ValidateSet('default', 'left', 'center', 'right')]
    [string]$TitleAlign = 'default',

    [Parameter()]
    [Alias('h', '?')]
    [switch]$Help
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Show-HelmvHelp {
    $e = [char]0x1B
    $b1 = "$e[38;5;51m"   # Bright cyan
    $b2 = "$e[38;5;45m"   # Cyan-blue
    $b3 = "$e[38;5;39m"   # Sky blue
    $b4 = "$e[38;5;33m"   # Royal blue
    $b5 = "$e[38;5;27m"   # Deep blue
    $b6 = "$e[38;5;141m"  # Violet accent

    $cTitle   = "$e[1m$e[38;5;51m"
    $cSec     = "$e[1m$e[38;5;214m" # Amber gold
    $cParam   = "$e[1m$e[38;5;48m"  # Mint green
    $cVal     = "$e[38;5;147m"      # Soft lilac
    $cDesc    = "$e[38;5;252m"      # Light gray
    $cDim     = "$e[38;5;243m"      # Dim gray
    $cBorder  = "$e[38;5;239m"      # Dark border gray
    $cAccent  = "$e[38;5;39m"       # Cyan accent
    $cReset   = "$e[0m"

    Write-Host ""
    Write-Host "${b1}  ██╗  ██╗███████╗██╗     ███╗   ███╗██╗   ██╗"
    Write-Host "${b2}  ██║  ██║██╔════╝██║     ████╗ ████║██║   ██║"
    Write-Host "${b3}  ███████║█████╗  ██║     ██╔████╔██║██║   ██║"
    Write-Host "${b4}  ██╔══██║██╔══╝  ██║     ██║╚██╔╝██║╚██╗ ██╔╝"
    Write-Host "${b5}  ██║  ██║███████╗███████╗██║ ╚═╝ ██║ ╚████╔╝ "
    Write-Host "${b6}  ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝     ╚═╝  ╚═══╝  ${cReset}"
    Write-Host "${cBorder}  ─────────────────────────────────────────────────────────────────────────────${cReset}"
    Write-Host "  ${cAccent}» ${cTitle}HELM VISUALIZE ${cDim}(helmv)${cAccent} • SPACE ENGINEERS TELEMETRY TUI SIMULATOR & DESIGNER «${cReset}"
    Write-Host "  ${cDim}Tear-Free Terminal Rendering • 32 Presets • Hardware Screen Bezels • 0-GC Profiling${cReset}"
    Write-Host "${cBorder}  ─────────────────────────────────────────────────────────────────────────────${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}SYNOPSIS${cReset}"
    Write-Host "    ${cParam}pwsh ./helmv.ps1${cReset} [${cVal}-Preset${cReset} <name>] [${cVal}Options...${cReset}]"
    Write-Host "    ${cParam}pwsh ./helmv.ps1${cReset} ${cVal}-CustomPreset${cReset} <file|dsl_string> [${cVal}Options...${cReset}]"
    Write-Host "    ${cParam}pwsh ./helmv.ps1${cReset} ${cVal}-Help${cReset} | ${cVal}-h${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}SIMULATION & TIMING OPTIONS${cReset}"
    Write-Host "    ${cParam}-Interactive${cReset}                  ${cDesc}Run continuous live loop with interactive keyboard controls${cReset}"
    Write-Host "    ${cParam}-Frames${cReset} ${cVal}<int>${cReset}               ${cDesc}Number of frames to render before exiting (default: 60)${cReset}"
    Write-Host "    ${cParam}-UpdateFrequency${cReset} ${cVal}<freq>${cReset}      ${cDesc}Match in-game PB cadence: ${cVal}1x${cDesc} (60Hz), ${cVal}10x${cDesc} (6Hz, default), ${cVal}100x${cDesc} (0.6Hz)${cReset}"
    Write-Host "    ${cParam}-IntervalMs${cReset} ${cVal}<int>${cReset}            ${cDesc}Direct frame cadence override in milliseconds${cReset}"
    Write-Host "    ${cParam}-Boot${cReset}                        ${cDesc}Play cinematic multi-stage boot sequence & liquid calibration sweep${cReset}"
    Write-Host "    ${cParam}-BootDuration${cReset} ${cVal}<sec>${cReset}         ${cDesc}Duration of the boot sequence in seconds (default: 2.0)${cReset}"
    Write-Host "    ${cParam}-DampingLerp${cReset}                ${cDesc}Toggle aerospace fluid damping lerp smoothing (default: `$true)${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}SPACE ENGINEERS HARDWARE SCREEN PROFILES (-ScreenProfile)${cReset}"
    Write-Host "    ${cParam}TextPanel${cReset}     ${cDim}(512x512, 1:1)${cReset}  ${cDesc}Standard Large/Small Grid wall LCD with heavy industrial mounts ${cBorder}[#]${cReset}"
    Write-Host "    ${cParam}WideLCD${cReset}       ${cDim}(1024x512, 2:1)${cReset} ${cDesc}Dual-column ultra-wide bridge screen with bridge mounting clamps ${cBorder}[x]${cReset}"
    Write-Host "    ${cParam}CockpitSmall${cReset}  ${cDim}(512x256, 2:1)${cReset}  ${cDesc}Compact fighter instrument HUD with avionics flight clamps ${cBorder}[+]${cReset}"
    Write-Host "    ${cParam}CornerLCD${cReset}     ${cDim}(512x128, 4:1)${cReset}  ${cDesc}Slanted corner LCD strip with angled structural brackets ${cBorder}// \\\\${cReset}"
    Write-Host "    ${cParam}CockpitCorner${cReset} ${cDim}(256x128, 2:1)${cReset}  ${cDesc}Mini annunciator / indicator gauge HUD with console mounts ${cBorder}|> <|${cReset}"
    Write-Host "    ${cParam}Auto${cReset}          ${cDim}(Inferred)${cReset}      ${cDesc}Automatically infer optimal aspect ratio from DSL screen layout (default)${cReset}"
    Write-Host "    ${cParam}Off${cReset}           ${cDim}(Headless)${cReset}      ${cDesc}Virtual telemetry stream without hardware mounting bezels${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}RENDER MODES & BORDER CUSTOMIZATION${cReset}"
    Write-Host "    ${cParam}-RenderMode${cReset} ${cVal}<mode>${cReset}          ${cDesc}Visual engine: ${cVal}Graphic${cDesc} (modern UTF-8), ${cVal}Ascii${cDesc} (SE vanilla), ${cVal}Auto${cReset}"
    Write-Host "    ${cParam}-StrictSEAscii${cReset}              ${cDesc}Force strict vanilla Space Engineers Monospace font glyphs (+---+, #, -)${cReset}"
    Write-Host "    ${cParam}-Border${cReset} ${cVal}<style>${cReset}             ${cDesc}Modular border style: ${cVal}heavy${cDesc}, ${cVal}double${cDesc}, ${cVal}single${cDesc}, ${cVal}rounded${cDesc}, ${cVal}minimal${cDesc}, ${cVal}ascii${cDesc}, ${cVal}none${cReset}"
    Write-Host "    ${cParam}-TitleAlign${cReset} ${cVal}<align>${cReset}         ${cDesc}Position titles along panel header frame: ${cVal}left${cDesc}, ${cVal}center${cDesc}, ${cVal}right${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}TELEMETRY UNITS & MEASUREMENTS (-Measurement / -Percent)${cReset}"
    Write-Host "    ${cParam}-Measurement${cReset} ${cDim}(-Units)${cReset}     ${cDesc}Format values as real engineering units: ${cVal}MW${cDesc}, ${cVal}GW${cDesc}, ${cVal}kW${cDesc}, ${cVal}kL${cDesc}, ${cVal}m/s${cDesc}, ${cVal}km${cDesc}, ${cVal}kN${cDesc}, ${cVal}rds${cDesc}, ${cVal}°${cDesc}, ${cVal}kPa${cDesc}, ${cVal}rpm${cReset}"
    Write-Host "    ${cParam}-Percent${cReset}                    ${cDesc}Force standard percentage readout (${cVal}xx.x%${cDesc}) across all gauges${cReset}"
    Write-Host "    ${cDim}Alignment Guarantee: Dynamic 3-phase width balancing guarantees zero border tears across all unit lengths.${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}LAYOUT GEOMETRY & SPRITE TELEMETRY${cReset}"
    Write-Host "    ${cParam}-SingleColumn${cReset}               ${cDesc}Stack panel cards vertically (41-character width)${cReset}"
    Write-Host "    ${cParam}-MultiColumn${cReset}                ${cDesc}Render cards in side-by-side grid rows (84-character width)${cReset}"
    Write-Host "    ${cParam}-ShowSprites${cReset}                ${cDesc}Display real-time VRage MySprite allocations & in-place mutation telemetry${cReset}"
    Write-Host "    ${cParam}-ShowStats${cReset}                  ${cDesc}Display PB instruction budget analysis (<0.1% normal, <1.0% redraw)${cReset}"
    Write-Host "    ${cParam}-FontMode${cReset} ${cVal}<mode>${cReset}            ${cDesc}Load SE bitmap fonts: ${cVal}Monospace${cDesc}, ${cVal}Standard${cDesc}, ${cVal}JetBrains${cDesc}, ${cVal}Cascadia${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}INTERACTIVE RUNTIME HOTKEYS (-Interactive)${cReset}"
    Write-Host "    ${cParam}[M]${cReset} ${cDesc}Switch to Measurements mode${cReset}    ${cParam}[P]${cReset} ${cDesc}Switch to Percentages mode${cReset}"
    Write-Host "    ${cParam}[U]${cReset} ${cDesc}Cycle active unit mode${cReset}         ${cParam}[Tab]${cReset} ${cDesc}Toggle Units / Percent${cReset}"
    Write-Host "    ${cParam}[Ctrl+C]${cReset} ${cDesc}Cleanly exit simulator and restore terminal state${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}BUILT-IN PRESETS (32 AVAILABLE)${cReset}"
    Write-Host "    ${cAccent}Flight & Cockpits:${cReset}   ${cVal}fighter${cReset}, ${cVal}fighter_hud${cReset}, ${cVal}split_flight_deck${cReset}, ${cVal}rover${cReset}, ${cVal}rover_cockpit${cReset}, ${cVal}atmospheric_lander${cReset}"
    Write-Host "    ${cAccent}Bases & Stations:${cReset}    ${cVal}bridge${cReset}, ${cVal}bridge_matrix${cReset}, ${cVal}command_carrier${cReset}, ${cVal}planetary_base${cReset}, ${cVal}deep_space_explorer${cReset}"
    Write-Host "    ${cAccent}Power & Monoliths:${cReset}   ${cVal}battery_monolith${cReset}, ${cVal}reactor_monolith${cReset}, ${cVal}monolith${cReset}, ${cVal}thruster_balance${cReset}"
    Write-Host "    ${cAccent}Industry & Mining:${cReset}   ${cVal}mining${cReset}, ${cVal}mining_rig${cReset}, ${cVal}mining_fleet_coordinator${cReset}, ${cVal}refinery_monolith${cReset}, ${cVal}assembler_monolith${cReset}"
    Write-Host "    ${cAccent}Cargo & Logistics:${cReset}   ${cVal}cargo${cReset}, ${cVal}cargo_monolith${cReset}, ${cVal}cargo_overview${cReset}, ${cVal}survival_essentials${cReset}"
    Write-Host "    ${cAccent}Security & Life:${cReset}     ${cVal}airlock${cReset}, ${cVal}airlock_security${cReset}, ${cVal}medical_life_support${cReset}, ${cVal}tactical_defense${cReset}, ${cVal}hazard_alert${cReset}"
    Write-Host "    ${cAccent}Advanced & API:${cReset}      ${cVal}dynamic_telemetry${cReset}, ${cVal}jump_drive_spool${cReset}, ${cVal}hazard_demo${cReset}"
    Write-Host ""

    Write-Host "  ${cSec}EXAMPLES${cReset}"
    Write-Host "    ${cDim}# 1. Preview atmospheric lander with engineering measurements & small cockpit bezel:${cReset}"
    Write-Host "    ${cParam}pwsh ./helmv.ps1${cReset} -Preset atmospheric_lander -ScreenProfile CockpitSmall -Measurement"
    Write-Host ""
    Write-Host "    ${cDim}# 2. Interactive bridge command console with live unit hotkeys:${cReset}"
    Write-Host "    ${cParam}pwsh ./helmv.ps1${cReset} -Preset command_carrier -ScreenProfile WideLCD -Interactive"
    Write-Host ""
    Write-Host "    ${cDim}# 3. Test battery monolith in fighter mini-HUD with rounded borders:${cReset}"
    Write-Host "    ${cParam}pwsh ./helmv.ps1${cReset} -Preset battery_monolith -ScreenProfile CockpitCorner -Border rounded"
    Write-Host ""
    Write-Host "    ${cDim}# 4. Strict SE Monospace ASCII mode with PB hardware sprite profiling:${cReset}"
    Write-Host "    ${cParam}pwsh ./helmv.ps1${cReset} -Preset planetary_base -RenderMode Ascii -ShowSprites -ShowStats"
    Write-Host ""
    Write-Host "${cBorder}  ─────────────────────────────────────────────────────────────────────────────${cReset}"
    Write-Host "  ${cDim}HELM Core: Zero-Allocation Retained Virtual Buffer Display Engine for Space Engineers.${cReset}"
    Write-Host ""
}

if ($Help) {
    Show-HelmvHelp
    exit 0
}

# -------------------------------------------------------------------------
# Font Mode — load and activate terminal font via Set-TerminalFont utility
# Runs before HelmCore compile so the correct font is active for the entire
# demo session. Restore-TerminalFont is registered in a finally block below.
# -------------------------------------------------------------------------
$script:FontModeActive = $false
if ($FontMode -ne 'Off') {
    $fontUtil = Join-Path $scriptDir "tools/Set-TerminalFont.ps1"
    if (Test-Path $fontUtil) {
        . $fontUtil   # dot-source to import functions

        $fontKey       = $null
        $fontSecondary = $null

        switch ($FontMode) {
            'Monospace'  { $fontKey = 'SEMonospace';   $fontSecondary = $null              }
            'Standard'   { $fontKey = 'SEStandard';    $fontSecondary = $null              }
            'JetBrains'  { $fontKey = 'JetBrainsMono'; $fontSecondary = 'NerdFontSymbols'  }
            'Cascadia'   { $fontKey = 'CaskaydiaCove'; $fontSecondary = 'NerdFontSymbols'  }
        }

        $setArgs = @{ Key = $fontKey; Size = [short]$FontSize }
        if ($fontSecondary) { $setArgs['SecondaryKey'] = $fontSecondary }

        $script:FontModeActive = Set-TerminalFont @setArgs
        if (-not $script:FontModeActive) {
            Write-Host "  [font] Host does not support runtime font switching — continuing with current font." -ForegroundColor DarkGray
        }
    } else {
        Write-Host "  [font] tools/Set-TerminalFont.ps1 not found — -FontMode ignored." -ForegroundColor DarkGray
    }
}

# -------------------------------------------------------------------------
# Compile or Load HelmCore Assembly (.NET First Rule)
# -------------------------------------------------------------------------
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
if ([string]::IsNullOrEmpty($scriptDir)) { $scriptDir = [System.IO.Directory]::GetCurrentDirectory() }
$helmCoreCs = [System.IO.Path]::Combine($scriptDir, "HelmCore.cs")
$tempDll = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "HelmCore.dll")

if (-not (Get-Module -Name "HelmCore" -ErrorAction SilentlyContinue) -and -not ("HelmCore.HelmParser" -as [type])) {
    if (-not [System.IO.File]::Exists($tempDll) -or ([System.IO.File]::GetLastWriteTime($helmCoreCs) -gt [System.IO.File]::GetLastWriteTime($tempDll)) -or
        ([System.IO.File]::GetLastWriteTime($MyInvocation.MyCommand.Path) -gt [System.IO.File]::GetLastWriteTime($tempDll))) {
        Write-Host "Compiling HelmCore.cs for offline simulation..." -ForegroundColor DarkGray
        # HELM_TOOLS keeps the template-file loader, which the game script does not ship (see Build-Release.ps1).
        $csc = Get-Command csc -ErrorAction SilentlyContinue
        if ($csc) {
            & csc -nologo -target:library -define:HELM_TOOLS -out:$tempDll $helmCoreCs | Out-Null
        } else {
            Add-Type -TypeDefinition ("#define HELM_TOOLS`n" + [System.IO.File]::ReadAllText($helmCoreCs)) -ReferencedAssemblies 'System.Linq'
        }
    }
    if ([System.IO.File]::Exists($tempDll) -and -not ("HelmCore.HelmParser" -as [type])) {
        Add-Type -Path $tempDll
    }
}

# Automatically load custom border & frame templates from layouts/templates.txt
$templatesTxt = [System.IO.Path]::Combine($scriptDir, "layouts", "templates.txt")
if ([System.IO.File]::Exists($templatesTxt) -and ("HelmCore.HelmParser" -as [type])) {
    $tContent = [System.IO.File]::ReadAllText($templatesTxt)
    if (-not [string]::IsNullOrWhiteSpace($tContent)) {
        [HelmCore.HelmParser]::LoadTemplateFile($tContent)
    }
}

# -------------------------------------------------------------------------
# ANSI & TUI Styling Definitions (Palette & UI Objects)
# -------------------------------------------------------------------------
$script:ESC = [char]27
$script:RedAlert = [bool]$RedAlert
$script:ANSI = @{
    Reset      = "$($script:ESC)[0m"
    Bold       = "$($script:ESC)[1m"
    Dim        = "$($script:ESC)[2m"
    ClearScreen= "$($script:ESC)[2J$($script:ESC)[H"
    HideCursor = "$($script:ESC)[?25l"
    ShowCursor = "$($script:ESC)[?25h"
}

function Get-AnsiRgb([byte]$r, [byte]$g, [byte]$b) {
    return "$($script:ESC)[38;2;${r};${g};${b}m"
}

function Convert-HelmColorToAnsi([string]$colorStr) {
    if ([string]::IsNullOrEmpty($colorStr)) { return "$($script:ESC)[38;2;220;225;235m" }
    $c = [HelmCore.HelmColor]::Parse($colorStr)
    if ($c.IsTransparent) { return $script:ANSI.Reset }
    return Get-AnsiRgb $c.R $c.G $c.B
}

function Get-InterpolatedAnsiColor([string[]]$stops, [double]$ratio) {
    if (-not $stops -or $stops.Length -eq 0) { return "$($script:ESC)[38;2;56;189;248m" }
    if ($stops.Length -eq 1) { return Convert-HelmColorToAnsi $stops[0] }

    $clampedRatio = [Math]::Max(0.0, [Math]::Min(1.0, $ratio))
    $scaled = $clampedRatio * ($stops.Length - 1)
    $idx1 = [int][Math]::Floor($scaled)
    $idx2 = [int][Math]::Min($stops.Length - 1, $idx1 + 1)
    $subRatio = $scaled - $idx1

    $c1 = [HelmCore.HelmColor]::Parse($stops[$idx1])
    $c2 = [HelmCore.HelmColor]::Parse($stops[$idx2])

    $r = [byte][Math]::Round($c1.R + ($c2.R - $c1.R) * $subRatio)
    $g = [byte][Math]::Round($c1.G + ($c2.G - $c1.G) * $subRatio)
    $b = [byte][Math]::Round($c1.B + ($c2.B - $c1.B) * $subRatio)

    return Get-AnsiRgb $r $g $b
}

# -------------------------------------------------------------------------
# Built-In Presets: HELM DSL Configurations
# -------------------------------------------------------------------------
$Configs = @{
    'rover' = @"
@bind batt = battery("Rover Battery Bank")
@bind eng  = engine("Hydrogen Engine Aux")
@bind susp = suspension("Wheel Suspensions")
@bind ore  = cargo("Ore Hopper Hold")

@screen rover_dash
output: "Rover Cockpit LCD", 0
layout: grid(2, 2)
bg: #0b0f17
border_color: #38bdf8

@panel pwr at(0, 0, 1, 1) title: "POWERTRAIN [HV-4]"
line: "BATT" bar(batt.percent) gradient(red, yellow, green)
line: "ENGN" led(eng.percent) gradient(green, yellow, cyan)

@panel susp at(0, 1, 1, 1) title: "CHASSIS & BRAKES"
line: "SUSP" dots(susp.percent) flat(#38bdf8)
line: "TRAC" chevron(susp.percent) gradient(cyan, lime)

@panel cargo at(1, 0, 1, 2) title: "LOGISTICS MANIFEST"
line: "ORE " bar(ore.percent) gradient(green, yellow, red)
line: "AMMO" dots(batt.percent) flat(yellow)
line: "RATE" spark(ore.percent) flat(cyan)
line: "WARN" hazard(batt.percent) flat(red)
"@

    'airlock' = @"
@bind pres = airvent("Airlock Pressure Vent")
@bind in_d = door("Inner Security Bulkhead")
@bind out_d= door("Outer Airlock Gate")

@screen airlock_panel
output: "Airlock Control Screen", 0
layout: grid(1, 2)
bg: #090c10
border_color: #10b981

@panel atmo at(0, 0, 1, 1) title: "ATMOSPHERIC INTEGRITY"
line: "PRES" bar(pres.percent) gradient(red, yellow, cyan)
line: "VENT" dots(pres.percent) flat(lime)
line: "CYCL" chevron(pres.percent) flat(cyan)

@panel doors at(0, 1, 1, 1) title: "SECURITY ACCESS GATES"
line: "INNR" bar(in_d.percent) gradient(green, yellow, red)
line: "OUTR" bar(out_d.percent) gradient(red, yellow, green)
line: "LOCK" hazard(out_d.percent) flat(yellow)
"@

    'bridge' = @"
@bind main_pwr = group("Capacitor Array", "Main Reactors", agg: avg)
@bind o2_life  = airvent("Bridge Life Support")
@bind jump_sp  = jumpdrive("Hyper Jump Core")
@bind h2_prop  = group("Hydrogen Reserves", agg: min)

@screen fleet_bridge
output: "Bridge Main Multi-Matrix", 0
layout: grid(2, 2)
bg: #0a0d14
border_color: #2563eb

@panel pwr at(0, 0, 1, 1) title: "ENERGY CORE MATRIX"
line: "GRID" bar(main_pwr.percent) gradient(red, yellow, green)
line: "DRAIN" spark(main_pwr.percent) flat(cyan)

@panel nav at(1, 0, 1, 1) title: "PROPULSION DYNAMICS"
line: "H2  " bar(h2_prop.percent) gradient(red, yellow, cyan)
line: "THR " chevron(h2_prop.percent) gradient(blue, cyan, white)

@panel o2 at(0, 1, 1, 1) title: "LIFE SUPPORT SYSTEMS"
line: "O2  " bar(o2_life.percent) flat(cyan)
line: "VENT" dots(o2_life.percent) gradient(red, yellow, green)

@panel fsd at(1, 1, 1, 1) title: "WARP FLIGHT DECK"
line: "JUMP" led(jump_sp.percent) flat(magenta)
line: "WARN" hazard(jump_sp.percent) flat(yellow)
"@

    'hazard_demo' = @"
@bind reactor = reactor("Large Reactor Core")
@bind weapon  = warhead("Forward Torpedo Bay")
@bind vel     = thruster("Main Ion Thrusters")

@screen alert_hud
output: "Tactical HUD", 0
layout: grid(1, 2)
bg: #120909
border_color: #ef4444

@panel weapons at(0, 0, 1, 1) title: "TACTICAL DEFENSE [LIVE]"
line: "ARMD" hazard(weapon.percent) flat(yellow)
line: "CORE" hazard(reactor.percent) gradient(yellow, red)
line: "SPIN" chevron(vel.percent) gradient(cyan, lime, white)

@panel sensors at(0, 1, 1, 1) title: "SYSTEM CELL TELEMETRY"
line: "CELL" dots(reactor.percent) gradient(red, yellow, green)
line: "GRID" led(weapon.percent) gradient(green, yellow, red)
line: "TRAC" spark(vel.percent) flat(cyan)
"@

    'mining' = @"
@bind ore = cargo("Mining Ore Hold")
@bind drill = group("Drill Heads", agg: avg)

@screen mining_dash
output: "Drill Console LCD", 0
layout: grid(1, 2)
bg: #100f0a
border_color: #f59e0b

@panel hold at(0, 0, 1, 1) title: "CARGO CAPACITY"
line: "HOLD" bar(ore.percent) gradient(green, yellow, red)
line: "FLOW" spark(ore.percent) flat(cyan)

@panel drills at(0, 1, 1, 1) title: "DRILL ARRAY LOAD"
line: "HEAD" led(drill.percent) gradient(cyan, yellow, red)
line: "WARN" hazard(drill.percent) flat(yellow)
"@

    'fighter' = @"
@bind batt = battery("Fighter Battery")
@bind h2   = tank("H2 Fuel Tank")
@bind thr  = thruster("Ion Thrusters")

@screen fighter_cockpit
output: "Cockpit HUD", 0
layout: grid(1, 2)
bg: #05080c
border_color: #38bdf8

@panel flight at(0, 0, 1, 1) title: "PROPULSION DYNAMICS"
line: "PWR " bar(batt.percent) gradient(red, yellow, green)
line: "H2  " bar(h2.percent) gradient(red, yellow, cyan)
line: "VECT" chevron(thr.percent) gradient(blue, cyan, white)

@panel tactical at(0, 1, 1, 1) title: "SYSTEMS & CELLS"
line: "CELL" dots(batt.percent) flat(cyan)
line: "WARN" hazard(h2.percent) flat(yellow)
"@
}

# -------------------------------------------------------------------------
# Dynamic Telemetry Simulation Engine
# -------------------------------------------------------------------------
class TelemetrySim {
    [double]$Time = 0.0

    [double] GetValue([string]$bindName) {
        $t = $this.Time
        $name = $bindName.ToLowerInvariant()
        if ($name.Contains("batt"))   { return [Math]::Round(72.0 + 18.0 * [Math]::Sin($t * 0.4), 1) }
        if ($name.Contains("eng"))    { return [Math]::Round(55.0 + 35.0 * [Math]::Cos($t * 0.6), 1) }
        if ($name.Contains("susp"))   { return [Math]::Round(40.0 + 30.0 * [Math]::Sin($t * 1.2), 1) }
        if ($name.Contains("ore") -or $name.Contains("hold")) { return [Math]::Round((($t * 8.0) % 100.0), 1) }
        if ($name.Contains("drill"))  { return [Math]::Round(65.0 + 25.0 * [Math]::Sin($t * 1.4), 1) }
        if ($name.Contains("pres"))   { return [Math]::Round(98.0 - 15.0 * [Math]::Abs([Math]::Sin($t * 0.3)), 1) }
        if ($name.Contains("in_d"))   { return [Math]::Round(100.0 * [Math]::Max(0.0, [Math]::Sin($t * 0.5)), 1) }
        if ($name.Contains("out_d"))  { return [Math]::Round(100.0 * [Math]::Max(0.0, [Math]::Cos($t * 0.5)), 1) }
        if ($name.Contains("ammo"))   { return [Math]::Round(100.0 - (($t * 6.0) % 100.0), 1) }
        if ($name.Contains("gatling") -or $name.Contains("turret")) { return [Math]::Round(82.0 + 15.0 * [Math]::Sin($t * 1.8), 1) }
        if ($name.Contains("missile") -or $name.Contains("torp"))   { return [Math]::Round(70.0 - (($t * 4.0) % 60.0), 1) }
        if ($name.Contains("shield")) { return [Math]::Round(95.0 - 18.0 * [Math]::Abs([Math]::Sin($t * 0.8)), 1) }
        if ($name.Contains("target") -or $name.Contains("radar"))   { return 85.0 }
        if ($name.Contains("coolant")){ return [Math]::Round(90.0 - 12.0 * [Math]::Sin($t * 0.5), 1) }
        if ($name.Contains("rod") -or $name.Contains("trim") -or $name.Contains("pitch") -or $name.Contains("roll") -or $name.Contains("yaw")) {
            return [Math]::Round(50.0 + 35.0 * [Math]::Sin($t * 0.9), 1)
        }
        if ($name.Contains("flow") -or $name.Contains("rate")) {
            # Oscillates above and below 50% to cycle between CHG, STBY, DSC
            return [Math]::Round(50.0 + 40.0 * [Math]::Sin($t * 0.7), 1)
        }
        if ($name.Contains("contain") -or $name.Contains("magnet") -or $name.Contains("sort")) { return 100.0 }
        if ($name.Contains("scram") -or $name.Contains("eject"))   { return 0.0 }
        if ($name.Contains("status") -or $name.Contains("safety")) { return 90.0 }
        if ($name.Contains("reactor")){ return [Math]::Round(85.0 + 14.0 * [Math]::Sin($t * 0.8), 1) }
        if ($name.Contains("weapon")) { return [Math]::Round(92.0 + 8.0 * [Math]::Sin($t * 1.5), 1) }
        if ($name.Contains("vel") -or $name.Contains("thr")) { return [Math]::Round(65.0 + 30.0 * [Math]::Cos($t * 0.7), 1) }
        if ($name.Contains("refinery") -or $name.Contains("furnace") -or $name.Contains("smelt")) { return [Math]::Round(78.0 + 16.0 * [Math]::Sin($t * 0.6), 1) }
        if ($name.Contains("crush"))  { return [Math]::Round(60.0 + 30.0 * [Math]::Sin($t * 1.3), 1) }
        if ($name.Contains("slag"))   { return [Math]::Round(45.0 + 20.0 * [Math]::Cos($t * 0.8), 1) }
        if ($name.Contains("warp") -or $name.Contains("spool")) { return [Math]::Round((($t * 15.0) % 100.0), 1) }
        if ($name.Contains("cryo") -or $name.Contains("med"))   { return [Math]::Round(98.0 - 2.0 * [Math]::Abs([Math]::Sin($t * 0.2)), 1) }
        if ($name.Contains("jump"))   { return [Math]::Round((($t * 12.0) % 100.0), 1) }
        if ($name.Contains("h2"))     { return [Math]::Round(88.0 - (($t * 3.0) % 40.0), 1) }
        if ($name.Contains("o2"))     { return 99.4 }
        return [Math]::Round(50.0 + 40.0 * [Math]::Sin($t), 1)
    }

    [void] Step([double]$delta) {
        $this.Time += $delta
    }
}

# -------------------------------------------------------------------------
# Strict TUI Visual Gauge Renderer (Zero-Tear Padding Rule)
# Calculates character count BEFORE encoding to guarantee perfect right border alignment
# -------------------------------------------------------------------------
function Render-Gauge(
    [HelmCore.LineDef]$line,
    [double]$value,
    [int]$targetGaugeChars, # Strict visual length in characters
    [bool]$alertPhase,
    [bool]$strictSEAscii
) {
    # 1. Resolve active color
    $ansiCol = if ($line.Gradient -and $line.GradientColors.Length -gt 0) {
        $c = [HelmCore.Layout]::GradientLerp($line.GradientColors, $value)
        Get-AnsiRgb $c.R $c.G $c.B
    } elseif ($line.ColorRules -and $line.ColorRules.Count -gt 0) {
        $colorName = [HelmCore.Layout]::ResolveConditionalColor($line.ColorRules, $value)
        Convert-HelmColorToAnsi $colorName
    } else {
        Convert-HelmColorToAnsi $line.FlatColor
    }

    $colorName = if ($line.Gradient -and $line.GradientColors.Length -gt 0) {
        [HelmCore.Layout]::GradientColorAt($line.GradientColors, $value)
    } elseif ($line.ColorRules -and $line.ColorRules.Count -gt 0) {
        [HelmCore.Layout]::ResolveConditionalColor($line.ColorRules, $value)
    } else {
        $line.FlatColor
    }

    $isAlert = ($colorName -eq 'red' -or $colorName -eq '#ef4444')
    if ($isAlert -and $alertPhase) {
        $ansiCol = "$($script:ESC)[38;2;255;90;90m$($script:ANSI.Bold)"
    }

    $dimCol = "$($script:ESC)[38;2;45;55;72m"
    $pct = [Math]::Max(0.0, [Math]::Min(1.0, $value / 100.0))

    $sb = [System.Text.StringBuilder]::new()
    [int]$charCount = 0

    switch ($line.Kind) {
        ([HelmCore.LineKind]::Bar) {
            $filled = [int][Math]::Round($targetGaugeChars * $pct)
            if ($filled -gt $targetGaugeChars) { $filled = $targetGaugeChars }
            $unfilled = $targetGaugeChars - $filled
            $fillChar = if ($strictSEAscii) { '#' } else { [char]0x2588 }
            $unfillChar = if ($strictSEAscii) { '-' } else { [char]0x2500 }
            if ($line.Direction -eq 'up' -and $filled -gt 0) {
                $baseCount = [Math]::Max(0, $filled - 1)
                $meniscus = if ($strictSEAscii) { '^' } else { '~' }
                [void]$sb.Append($ansiCol)
                [void]$sb.Append([string]::new($fillChar, $baseCount))
                [void]$sb.Append("$($script:ANSI.Bold)$meniscus$($script:ANSI.Reset)$ansiCol")
                [void]$sb.Append($dimCol)
                [void]$sb.Append([string]::new($unfillChar, $unfilled))
            } else {
                [void]$sb.Append($ansiCol)
                [void]$sb.Append([string]::new($fillChar, $filled))
                [void]$sb.Append($dimCol)
                [void]$sb.Append([string]::new($unfillChar, $unfilled))
            }
            $charCount = $targetGaugeChars
        }

        ([HelmCore.LineKind]::LedBar) {
            # Each chiclet is 3 chars: [#] or [ ]
            $numChiclets = [int]($targetGaugeChars / 3)
            $litCount = [int][Math]::Floor($pct * $numChiclets)
            for ($s = 0; $s -lt $numChiclets; $s++) {
                if ($s -lt $litCount) {
                    [void]$sb.Append("${ansiCol}[#]")
                } else {
                    [void]$sb.Append("${dimCol}[ ]")
                }
            }
            $charCount = $numChiclets * 3
            # Pad any remainder to reach targetGaugeChars exactly
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Hazard) {
            # Each stripe is 2 chars: //
            $numStripes = [int]($targetGaugeChars / 2)
            $litCount = [int][Math]::Round($pct * $numStripes)
            for ($s = 0; $s -lt $numStripes; $s++) {
                if ($s -lt $litCount) {
                    $c = if ($s % 2 -eq 0) { $ansiCol } else { "$($script:ESC)[38;2;30;35;45m" }
                    [void]$sb.Append("${c}//")
                } else {
                    [void]$sb.Append("${dimCol}..")
                }
            }
            $charCount = $numStripes * 2
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Chevron) {
            # Each chevron is 2 chars: >>
            $numArrs = [int]($targetGaugeChars / 2)
            $litCount = [int][Math]::Round($pct * $numArrs)
            for ($s = 0; $s -lt $numArrs; $s++) {
                if ($s -lt $litCount) {
                    [void]$sb.Append("${ansiCol}>>")
                } else {
                    [void]$sb.Append("${dimCol}..")
                }
            }
            $charCount = $numArrs * 2
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Dots) {
            # Each dot is 2 chars: * or .
            $numDots = [int]($targetGaugeChars / 2)
            $litCount = [int][Math]::Round($pct * $numDots)
            for ($s = 0; $s -lt $numDots; $s++) {
                if ($s -lt $litCount) {
                    [void]$sb.Append("${ansiCol}* ")
                } else {
                    [void]$sb.Append("${dimCol}. ")
                }
            }
            $charCount = $numDots * 2
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Spark) {
            # Sparkline trend mini-graph (each column is 2 chars: block/glyph + space)
            $blocks = if ($strictSEAscii) {
                @('.', ':', ';', '=', '+', '*', '#', '%')
            } else {
                @([char]0x2581, [char]0x2582, [char]0x2583, [char]0x2584, [char]0x2585, [char]0x2586, [char]0x2587, [char]0x2588)
            }
            $numCols = [int]($targetGaugeChars / 2)
            [void]$sb.Append($ansiCol)
            for ($h = 0; $h -lt $numCols; $h++) {
                $hVal = [Math]::Max(0.0, [Math]::Min(100.0, $value + (([Math]::Sin($h + $value) * 15.0))))
                $idx = [int][Math]::Floor(($hVal / 100.0) * 7)
                [void]$sb.Append($blocks[$idx])
                [void]$sb.Append(' ')
            }
            $charCount = $numCols * 2
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Balance) {
            # Zero-centered bidirectional balance chart: <----[ 0 ]---->
            # 50% is dead center. < 50% extends left; > 50% extends right.
            # Total width: $targetGaugeChars (e.g. 18 chars = 8 left + 2 center + 8 right)
            $halfSpan = [int](($targetGaugeChars - 2) / 2)
            $leftTrackChar = if ($strictSEAscii) { '-' } else { [char]0x2500 }
            $rightTrackChar = if ($strictSEAscii) { '-' } else { [char]0x2500 }
            $fillChar = '<'
            $rightFillChar = '>'
            $centerGlyph = if ($strictSEAscii) { '||' } else { [char]0x2502 + [char]0x2502 }

            $dev = ($value - 50.0) / 50.0 # -1.0 to +1.0
            if ($dev -lt -1.0) { $dev = -1.0 }
            if ($dev -gt 1.0) { $dev = 1.0 }

            if ($dev -lt 0) {
                $fillCount = [int][Math]::Round([Math]::Abs($dev) * $halfSpan)
                if ($fillCount -gt $halfSpan) { $fillCount = $halfSpan }
                $padLeft = $halfSpan - $fillCount

                [void]$sb.Append($dimCol)
                [void]$sb.Append([string]::new($leftTrackChar, $padLeft))
                [void]$sb.Append($ansiCol)
                [void]$sb.Append([string]::new($fillChar, $fillCount))
                [void]$sb.Append("$($script:ANSI.Bold)$centerGlyph$($script:ANSI.Reset)")
                [void]$sb.Append($dimCol)
                [void]$sb.Append([string]::new($rightTrackChar, $halfSpan))
            } else {
                $fillCount = [int][Math]::Round($dev * $halfSpan)
                if ($fillCount -gt $halfSpan) { $fillCount = $halfSpan }
                $padRight = $halfSpan - $fillCount

                [void]$sb.Append($dimCol)
                [void]$sb.Append([string]::new($leftTrackChar, $halfSpan))
                [void]$sb.Append("$($script:ANSI.Bold)$centerGlyph$($script:ANSI.Reset)")
                [void]$sb.Append($ansiCol)
                [void]$sb.Append([string]::new($rightFillChar, $fillCount))
                [void]$sb.Append($dimCol)
                [void]$sb.Append([string]::new($rightTrackChar, $padRight))
            }
            $charCount = $halfSpan * 2 + 2
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Toggle) {
            # Pill slider: [====O] ON (10 chars) or [O====] OFF (11 chars), padded to targetGaugeChars
            $isOn = $value -ge 50.0
            $knobCol = if ($isOn) { "$($script:ESC)[38;2;34;197;94m$($script:ANSI.Bold)" } else { "$($script:ESC)[38;2;148;163;184m" }
            $trackCol = "$($script:ESC)[38;2;60;70;85m"
            if ($isOn) {
                [void]$sb.Append("${trackCol}[====${knobCol}O${trackCol}] ${knobCol}ON")
                $charCount = 10   # "[====O] ON"
            } else {
                [void]$sb.Append("${trackCol}[${knobCol}O${trackCol}====] ${knobCol}OFF")
                $charCount = 11   # "[O====] OFF" is one character longer than the ON state
            }
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Checkbox) {
            # Square checkbox: [X] ACTIVE or [ ] READY  (12 chars, padded to targetGaugeChars)
            $isChecked = $value -ge 50.0
            $boxCol = if ($isChecked) { "$($script:ESC)[38;2;56;189;248m$($script:ANSI.Bold)" } else { "$($script:ESC)[38;2;100;116;139m" }
            if ($isChecked) {
                [void]$sb.Append("${boxCol}[X] ACTIVE")
            } else {
                [void]$sb.Append("${boxCol}[ ] READY ")
            }
            $charCount = 10
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Status) {
            # Rounded badge: [● ONLINE ] or [◐ STANDBY] or [○ OFFLINE] (12 chars, padded to targetGaugeChars)
            $badgeCol = if ($value -ge 75.0) {
                "$($script:ESC)[38;2;34;197;94m$($script:ANSI.Bold)"
            } elseif ($value -ge 25.0) {
                "$($script:ESC)[38;2;234;179;8m$($script:ANSI.Bold)"
            } else {
                "$($script:ESC)[38;2;239;68;68m$($script:ANSI.Bold)"
            }
            $badgeText = if ($value -ge 75.0) {
                if ($strictSEAscii) { "[# ONLINE ]" } else { "[* ONLINE ]" }
            } elseif ($value -ge 25.0) {
                if ($strictSEAscii) { "[: STANDBY]" } else { "[: STANDBY]" }
            } else {
                if ($strictSEAscii) { "[. OFFLINE]" } else { "[. OFFLINE]" }
            }
            [void]$sb.Append("${badgeCol}${badgeText}")
            $charCount = 11
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Ammo) {
            # Ammo magazine: [▮▮▮▮▮▯▯▯] or [||||....] with percentage (padded to targetGaugeChars)
            $maxPips = [Math]::Max(4, [int]($targetGaugeChars - 6)) # reserve 6 chars for " 100%"
            $litPips = [int][Math]::Round($maxPips * ($pct))
            if ($litPips -gt $maxPips) { $litPips = $maxPips }
            $unlitPips = $maxPips - $litPips

            $pipFilledChar = if ($strictSEAscii) { '|' } else { [char]0x25AE }
            $pipUnfilledChar = if ($strictSEAscii) { '.' } else { [char]0x25AF }

            [void]$sb.Append("$($script:ESC)[38;2;60;70;85m[")
            [void]$sb.Append($ansiCol)
            [void]$sb.Append([string]::new($pipFilledChar, $litPips))
            [void]$sb.Append($dimCol)
            [void]$sb.Append([string]::new($pipUnfilledChar, $unlitPips))
            [void]$sb.Append("$($script:ESC)[38;2;60;70;85m]")

            $charCount = $maxPips + 2
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        ([HelmCore.LineKind]::Flow) {
            # Animated spinner & directional flow indicator: | ^^ CHG or | vv DSC or | == STBY
            # Frame index derived from value/simulation time
            $spinnersAscii = @('|', '/', '-', '\')
            $spinIdx = [int][Math]::Abs([Math]::Floor($value * 1.5))
            $spinChar = $spinnersAscii[$spinIdx % $spinnersAscii.Length]

            $isMachinery = ($line.Field -match 'producing|active|running|state') -or ($line.BindName -match 'assembler|refinery|drill|welder|grinder|factory|plant|ops|fab')
            $flowLabel = ""
            $flowCol = ""
            if ($isMachinery) {
                if ($value -gt 0) {
                    $flowLabel = "$spinChar RUN"
                    $flowCol = "$($script:ESC)[38;2;34;197;94m$($script:ANSI.Bold)"
                } else {
                    $flowLabel = "== IDLE"
                    $flowCol = "$($script:ESC)[38;2;148;163;184m"
                }
            } elseif ($value -gt 55.0) {
                $flowLabel = "$spinChar ^^ CHG"
                $flowCol = "$($script:ESC)[38;2;34;197;94m$($script:ANSI.Bold)"
            } elseif ($value -lt 45.0) {
                $flowLabel = "$spinChar vv DSC"
                $flowCol = "$($script:ESC)[38;2;239;68;68m$($script:ANSI.Bold)"
            } else {
                $flowLabel = "$spinChar == STBY"
                $flowCol = "$($script:ESC)[38;2;56;189;248m"
            }

            [void]$sb.Append("${flowCol}${flowLabel}")
            $charCount = $flowLabel.Length
            $rem = $targetGaugeChars - $charCount
            if ($rem -gt 0) {
                [void]$sb.Append([string]::new(' ', $rem))
                $charCount += $rem
            }
        }

        default {
            # Text line readout: padded to gauge chars
            $rawText = "$($value.ToString('F1'))%"
            [void]$sb.Append("${ansiCol}$($rawText.PadRight($targetGaugeChars))")
            $charCount = $targetGaugeChars
        }
    }

    [void]$sb.Append($script:ANSI.Reset)
    return @{
        RenderedGauge = $sb.ToString()
        VisibleChars  = $charCount
        ColorCode     = $ansiCol
    }
}

# -------------------------------------------------------------------------
# Load DSL Configuration
# -------------------------------------------------------------------------
$activeDsl = $null

if ($PSCmdlet.ParameterSetName -eq 'Custom') {
    if ([System.IO.File]::Exists($CustomPreset)) {
        $activeDsl = [System.IO.File]::ReadAllText($CustomPreset)
    } else {
        $activeDsl = $CustomPreset
    }
} else {
    if ($Configs.ContainsKey($Preset)) {
        $activeDsl = $Configs[$Preset]
    } else {
        # Check layouts/custom and layouts/default
        $candidates = @(
            [System.IO.Path]::Combine($scriptDir, "layouts", "custom", "${Preset}.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "battery_${Preset}.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "reactor_${Preset}.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "cargo_${Preset}.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_defense.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_balance.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_overview.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_monolith.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_matrix.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_cockpit.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_security.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_rig.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_hud.txt"),
            [System.IO.Path]::Combine($scriptDir, "layouts", "default", "${Preset}_alert.txt")
        )
        foreach ($c in $candidates) {
            if ([System.IO.File]::Exists($c)) {
                $activeDsl = [System.IO.File]::ReadAllText($c)
                break
            }
        }
        if ([string]::IsNullOrEmpty($activeDsl)) {
            $matchedFile = Get-ChildItem -Path @([System.IO.Path]::Combine($scriptDir, "layouts", "custom"), [System.IO.Path]::Combine($scriptDir, "layouts", "default")) -Filter "*${Preset}*.txt" -ErrorAction SilentlyContinue | Select-Object -First 1
            if ($matchedFile) {
                $activeDsl = [System.IO.File]::ReadAllText($matchedFile.FullName)
            }
        }
        if ([string]::IsNullOrEmpty($activeDsl)) {
            Write-Error "Could not resolve preset '$Preset' from builtin configurations or layouts directory."
            exit 1
        }
    }
}

$parsedConfig = [HelmCore.HelmParser]::Parse($activeDsl)

if ($parsedConfig.Errors.Count -gt 0) {
    Write-Error "HELM DSL Parse Errors:`n$([string]::Join("`n", $parsedConfig.Errors))"
    exit 1
}

$screen = $parsedConfig.Screens[0]
$sim = [TelemetrySim]::new()

# Calculate game-accurate refresh cadence
# Space Engineers base physics tick = 60 Hz (~16.66ms / tick)
# Update1:   1 tick   = ~16.6ms  (60 fps)
# Update10:  10 ticks = ~166ms   (6 fps)
# Update100: 100 ticks= ~1660ms  (0.6 fps)
[int]$effectiveIntervalMs = 0
if ($PSBoundParameters.ContainsKey('IntervalMs') -and $IntervalMs -gt 0) {
    $effectiveIntervalMs = $IntervalMs
} else {
    switch ($UpdateFrequency) {
        '1x'   { $effectiveIntervalMs = 17 }
        '10x'  { $effectiveIntervalMs = 166 }
        '100x' { $effectiveIntervalMs = 1660 }
        default{ $effectiveIntervalMs = [int]([Math]::Round($screen.RefreshTicks * 16.66)) }
    }
}
if ($effectiveIntervalMs -le 0) { $effectiveIntervalMs = 166 }

# -------------------------------------------------------------------------
# Strict TUI Visual Formatting Helpers (Zero-Tear Padding Rule)
# -------------------------------------------------------------------------
function Get-BorderChars([string]$borderStyle, [bool]$strictAscii) {
    if ($strictAscii -or $borderStyle -eq 'ascii') {
        return @{
            TL = '+'; TR = '+'; BL = '+'; BR = '+'
            H = '-'; V = '|'
        }
    }
    switch ($borderStyle) {
        'double' {
            return @{
                TL = [char]0x2554; TR = [char]0x2557; BL = [char]0x255A; BR = [char]0x255D
                H = [char]0x2550; V = [char]0x2551
            }
        }
        'single' {
            return @{
                TL = [char]0x250C; TR = [char]0x2510; BL = [char]0x2514; BR = [char]0x2518
                H = [char]0x2500; V = [char]0x2502
            }
        }
        'rounded' {
            return @{
                TL = [char]0x256D; TR = [char]0x256E; BL = [char]0x2570; BR = [char]0x256F
                H = [char]0x2500; V = [char]0x2502
            }
        }
        'minimal' {
            return @{
                TL = ' '; TR = ' '; BL = ' '; BR = ' '
                H = [char]0x2500; V = ' '
            }
        }
        'heavy' {
            return @{
                TL = [char]0x250F; TR = [char]0x2513; BL = [char]0x2517; BR = [char]0x251B
                H = [char]0x2501; V = [char]0x2503
            }
        }
        'none' {
            return @{
                TL = ' '; TR = ' '; BL = ' '; BR = ' '
                H = ' '; V = ' '
            }
        }
        default {
            # 'graphic' / default SE aesthetic
            return @{
                TL = '+'; TR = '+'; BL = '+'; BR = '+'
                H = '-'; V = '|'
            }
        }
    }
}

function Format-TuiHeader(
    [string]$title,
    [int]$cardWidth,
    [string]$borderCol,
    [string]$titleAlign = 'left',
    [string]$titleCol = '',
    [string]$borderStyle = 'graphic',
    [bool]$strictAscii = $false,
    [string[]]$borderGradient = $null
) {
    $chars = Get-BorderChars $borderStyle $strictAscii
    $titleTag = " [ ${title} ] "
    if ($titleTag.Length -gt ($cardWidth - 6)) {
        $maxInner = [Math]::Max(1, $cardWidth - 10)
        $titleTag = " [ $($title.Substring(0, $maxInner)) ] "
    }

    $tCol = if (-not [string]::IsNullOrEmpty($titleCol)) { Convert-HelmColorToAnsi $titleCol } else { "$($script:ESC)[1m$($script:ESC)[38;2;248;250;252m" }

    $remainingDashes = [Math]::Max(0, $cardWidth - 2 - $titleTag.Length)
    $dashLeft = 2
    $dashRight = [Math]::Max(0, $remainingDashes - $dashLeft)

    if ($titleAlign -eq 'center') {
        $dashLeft = [int][Math]::Floor($remainingDashes / 2)
        $dashRight = [Math]::Max(0, $remainingDashes - $dashLeft)
    } elseif ($titleAlign -eq 'right') {
        $dashRight = 2
        $dashLeft = [Math]::Max(0, $remainingDashes - $dashRight)
    }

    $leftH = [string]::new($chars.H, $dashLeft)
    $rightH = [string]::new($chars.H, $dashRight)

    $bColStart = $borderCol
    $bColEnd = $borderCol
    if ($borderGradient -and $borderGradient.Length -gt 1) {
        $bColStart = Convert-HelmColorToAnsi $borderGradient[0]
        $bColEnd = Convert-HelmColorToAnsi $borderGradient[$borderGradient.Length - 1]
    }

    return "${bColStart}$($chars.TL)${leftH}$($script:ANSI.Reset)${tCol}$($script:ANSI.Bold)${titleTag}$($script:ANSI.Reset)${bColEnd}${rightH}$($chars.TR)$($script:ANSI.Reset)"
}

function Format-TuiFooter(
    [int]$cardWidth,
    [string]$borderCol,
    [string]$borderStyle = 'graphic',
    [bool]$strictAscii = $false,
    [string[]]$borderGradient = $null
) {
    $chars = Get-BorderChars $borderStyle $strictAscii
    $bColStart = $borderCol
    $bColEnd = $borderCol
    if ($borderGradient -and $borderGradient.Length -gt 1) {
        $bColStart = Convert-HelmColorToAnsi $borderGradient[0]
        $bColEnd = Convert-HelmColorToAnsi $borderGradient[$borderGradient.Length - 1]
    }
    $hLine = [string]::new($chars.H, $cardWidth - 2)
    return "${bColStart}$($chars.BL)${bColEnd}${hLine}$($chars.BR)$($script:ANSI.Reset)"
}

function Format-TuiLine(
    [string]$text,
    [int]$contentWidth,
    [string]$color,
    [string]$borderCol,
    [string]$borderStyle = 'graphic',
    [bool]$strictAscii = $false
) {
    $chars = Get-BorderChars $borderStyle $strictAscii
    $cleanText = if ($text.Length -gt $contentWidth) {
        $text.Substring(0, $contentWidth)
    } else {
        $text.PadRight($contentWidth)
    }
    return "${borderCol}$($chars.V)$($script:ANSI.Reset) ${color}${cleanText}$($script:ANSI.Reset) ${borderCol}$($chars.V)$($script:ANSI.Reset)"
}

function Format-TelemetryValue(
    [HelmCore.LineDef]$line,
    [double]$value,
    [string]$unitMode = 'Auto'
) {
    if ($unitMode -eq 'Percent') {
        return "$($value.ToString('F1'))%"
    }

    # If explicit unit is defined in DSL line
    if (-not [string]::IsNullOrEmpty($line.Unit)) {
        return [HelmCore.Layout]::FormatDisplayValue($value, $line.Unit, $line.Format)
    }

    # If in Measurement mode, infer engineering units from bind/field/label
    if ($unitMode -eq 'Measurement') {
        $bind = if ($line.BindName) { $line.BindName.ToLowerInvariant() } else { "" }
        $fld  = if ($line.Field) { $line.Field.ToLowerInvariant() } else { "" }
        $lbl  = if ($line.Label) { $line.Label.ToLowerInvariant() } else { "" }
        $combined = "$bind $fld $lbl"

        if ($combined -match 'batt|power|pwr|energy|core') {
            $scaled = [Math]::Round($value * 0.24, 1)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "MW", "F1")
        }
        if ($combined -match 'reactor') {
            $scaled = [Math]::Round($value * 0.05, 2)
            if ($scaled -ge 1.0) {
                return [HelmCore.Layout]::FormatDisplayValue($scaled, "GW", "F2")
            }
            return [HelmCore.Layout]::FormatDisplayValue([Math]::Round($value * 0.5, 1), "MW", "F1")
        }
        if ($combined -match 'solar|wind') {
            $scaled = [Math]::Round($value * 1.6, 0)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "kW", "F0")
        }
        if ($combined -match 'h2|hydrogen|coolant') {
            $scaled = [Math]::Round($value * 50.0, 0)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "kL", "F0")
        }
        if ($combined -match 'o2|oxygen') {
            $scaled = [Math]::Round($value * 1.0, 1)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "kL", "F1")
        }
        if ($combined -match 'pres|vent|atmo') {
            $scaled = [Math]::Round($value * 1.013, 0)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "kPa", "F0")
        }
        if ($combined -match 'cargo|ore|inv|buffer|hold') {
            $scaled = [Math]::Round($value * 15.0, 0)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "kL", "F0")
        }
        if ($combined -match 'vel|speed') {
            $scaled = [Math]::Round($value * 1.04, 1)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "m/s", "F1")
        }
        if ($combined -match 'dist|range|radar|target') {
            if ($value * 25.0 -ge 1000.0) {
                $scaled = [Math]::Round($value * 0.025, 1)
                return [HelmCore.Layout]::FormatDisplayValue($scaled, "km", "F1")
            }
            $scaled = [Math]::Round($value * 25.0, 0)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "m", "F0")
        }
        if ($combined -match 'thr|thrust|engine') {
            $scaled = [Math]::Round($value * 4.8, 0)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "kN", "F0")
        }
        if ($combined -match 'ammo|gatling|turret|missile') {
            $scaled = [Math]::Round($value * 8.0, 0)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "rds", "F0")
        }
        if ($combined -match 'pitch|roll|yaw|trim') {
            $scaled = [Math]::Round(($value - 50.0) * 0.9, 1)
            $sign = if ($scaled -ge 0) { "+" } else { "" }
            return "${sign}$($scaled.ToString('F1'))°"
        }
        if ($combined -match 'susp|travel') {
            $scaled = [Math]::Round($value * 0.4, 1)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "cm", "F1")
        }
        if ($combined -match 'drill|assembler|refinery') {
            $scaled = [Math]::Round($value * 12.0, 0)
            return [HelmCore.Layout]::FormatDisplayValue($scaled, "rpm", "F0")
        }
    }

    # Default fallback: standard percentage
    return "$($value.ToString('F1'))%"
}

# ---------------------------------------------------------------------------
# Card backgrounds. The in-game script paints the screen background, then a card behind each panel of a multi-panel
# screen, then the contents. Here each card line gets one background colour: the panel's own colour, a value-driven
# colour (bg: color(...)), a vertical gradient across the card, the screen's colour, or the default card (white at
# 6/255 over the screen background). flash: and -RedAlert switch it to alert_bg on the alert phase.
# Not drawn: horizontal gradients, textures and the gutters between cards.
# ---------------------------------------------------------------------------
function ConvertTo-BgRgb([string]$color) {
    if ([string]::IsNullOrEmpty($color)) { return $null }
    $c = [HelmCore.HelmColor]::Parse($color)
    if ($c.IsTransparent) { return $null }
    return @([int]$c.R, [int]$c.G, [int]$c.B)
}

function Get-CardBgRgb($panel, $screen, $sim, [bool]$alertPhase, [double]$rowFrac) {
    $alertCol = if ([string]::IsNullOrEmpty($screen.AlertBg)) { '#5b0f0f' } else { $screen.AlertBg }
    $alertOn  = ($alertCol -ne 'none') -and $alertPhase
    if ($alertOn) {
        foreach ($src in @($panel, $screen)) {
            if ($src.FlashRules -and $src.FlashRules.Count -gt 0 -and $null -ne [HelmCore.Layout]::ResolveConditionalColor($src.FlashRules, $sim.GetValue($src.FlashBind))) {
                return (ConvertTo-BgRgb $alertCol)
            }
        }
        if ($script:RedAlert) { return (ConvertTo-BgRgb $alertCol) }
    }

    # The panel's own background, if it has one.
    if ($panel.BgRules -and $panel.BgRules.Count -gt 0) { return (ConvertTo-BgRgb ([HelmCore.Layout]::ResolveConditionalColor($panel.BgRules, $sim.GetValue($panel.BgBind)))) }
    if ($panel.BgGradient -and $panel.BgGradient.Length -gt 1) { $g = [HelmCore.Layout]::GradientLerp($panel.BgGradient, 100.0 * $rowFrac); return @([int]$g.R, [int]$g.G, [int]$g.B) }
    if (-not [string]::IsNullOrEmpty($panel.Background)) { return (ConvertTo-BgRgb $panel.Background) }

    # Otherwise the screen background, with the default card over it on a multi-panel screen.
    $base = @(10, 12, 16)
    if ($screen.BgRules -and $screen.BgRules.Count -gt 0) { $base = ConvertTo-BgRgb ([HelmCore.Layout]::ResolveConditionalColor($screen.BgRules, $sim.GetValue($screen.BgBind))) }
    elseif ($screen.BgGradient -and $screen.BgGradient.Length -gt 1) { $g = [HelmCore.Layout]::GradientLerp($screen.BgGradient, 100.0 * $rowFrac); $base = @([int]$g.R, [int]$g.G, [int]$g.B) }
    elseif (-not [string]::IsNullOrEmpty($screen.Background)) { $base = ConvertTo-BgRgb $screen.Background }
    if ($null -eq $base) { $base = @(0, 0, 0) }
    # panel_bg: is the only card; without it the screen background shows through as one colour.
    if (-not [string]::IsNullOrEmpty($screen.PanelBg)) { $solid = ConvertTo-BgRgb $screen.PanelBg; if ($null -ne $solid) { return $solid } }
    return $base
}

function Set-CardBg([System.Collections.Generic.List[string]]$lines, $panel, $screen, $sim, [bool]$alertPhase) {
    $n = $lines.Count
    $reset = "$($script:ESC)[0m"
    for ($i = 0; $i -lt $n; $i++) {
        $rgb = Get-CardBgRgb $panel $screen $sim $alertPhase ($i / [Math]::Max(1, $n - 1))
        if ($null -eq $rgb) { continue }
        $bg = "$($script:ESC)[48;2;$($rgb[0]);$($rgb[1]);$($rgb[2])m"
        $lines[$i] = $bg + ([string]$lines[$i]).Replace($reset, $reset + $bg) + $reset
    }
}

function Render-PanelCardLines(
    [HelmCore.PanelDef]$panel,
    [HelmCore.ScreenDef]$screen,
    [TelemetrySim]$sim,
    [System.Collections.Generic.Dictionary[string, double]]$displayValues,
    [bool]$dampingLerp,
    [bool]$alertPhase,
    [bool]$strictSEAscii,
    [int]$cardWidth,
    [object]$calibOverride = $null,
    [int]$maxLines = 0,
    [string]$unitMode = 'Auto'
) {
    $lines = [System.Collections.Generic.List[string]]::new()

    $pBCol = $panel.BorderColor
    if ([string]::IsNullOrEmpty($pBCol)) { $pBCol = $screen.BorderColor }
    $borderCol = Convert-HelmColorToAnsi $pBCol

    $titleText = $panel.Title
    if ([string]::IsNullOrEmpty($titleText)) { $titleText = "PANEL: $($panel.Name)" }
    if ($panel.Orientation -eq 'vertical' -and -not $titleText.StartsWith('[V]')) {
        $titleText = "[V] $titleText"
    }
    $borderStyle = if ($Border -ne 'default') { $Border }
                   elseif (-not [string]::IsNullOrEmpty($panel.Border)) { $panel.Border }
                   elseif (-not [string]::IsNullOrEmpty($screen.Border)) { $screen.Border }
                   else { 'graphic' }

    $bGrad = if ($panel.BorderGradientColors -and $panel.BorderGradientColors.Length -gt 0) { $panel.BorderGradientColors }
             elseif ($screen.BorderGradientColors -and $screen.BorderGradientColors.Length -gt 0) { $screen.BorderGradientColors }
             else { $null }

    $titleAlign = if ($TitleAlign -ne 'default') { $TitleAlign }
                  elseif (-not [string]::IsNullOrEmpty($panel.TitleAlign)) { $panel.TitleAlign }
                  elseif (-not [string]::IsNullOrEmpty($screen.TitleAlign)) { $screen.TitleAlign }
                  else { 'left' }

    $titleColor = if (-not [string]::IsNullOrEmpty($panel.TitleColor)) { $panel.TitleColor }
                  elseif (-not [string]::IsNullOrEmpty($screen.TitleColor)) { $screen.TitleColor }
                  else { '' }

    $chars = Get-BorderChars $borderStyle $strictSEAscii

    $lines.Add((Format-TuiHeader $titleText $cardWidth $borderCol $titleAlign $titleColor $borderStyle $strictSEAscii $bGrad))

    # ── Phase 1: Pre-calculate smoothed values & formatted measurement strings ──
    $renderedItems = [System.Collections.Generic.List[PSObject]]::new()
    $maxValLen = 0

    $lineIdx = 0
    foreach ($line in $panel.Lines) {
        if ($maxLines -gt 0 -and $lineIdx -ge $maxLines) {
            break
        }
        $lineIdx++

        $targetVal = if ($calibOverride -ne $null) { [double]$calibOverride } else { $sim.GetValue($line.BindName) }
        $key = "$($panel.Name)_$($line.Label)"

        # Aerospace fluid damping lerp
        $renderVal = $targetVal
        if ($dampingLerp) {
            if (-not $displayValues.ContainsKey($key)) {
                $displayValues[$key] = $targetVal
            } else {
                $cur = $displayValues[$key]
                $diff = $targetVal - $cur
                if ([Math]::Abs($diff) -gt 0.05) {
                    $cur += $diff * 0.35
                    $displayValues[$key] = $cur
                } else {
                    $displayValues[$key] = $targetVal
                }
            }
            $renderVal = $displayValues[$key]
        }

        # Format readout string (percent or engineering measurement)
        $valFormatted = Format-TelemetryValue $line $renderVal $unitMode
        if ($valFormatted.Length -gt $maxValLen) {
            $maxValLen = $valFormatted.Length
        }

        $renderedItems.Add([PSCustomObject]@{
            Line         = $line
            RenderVal    = $renderVal
            ValFormatted = $valFormatted
        })
    }

    # ── Phase 2: Compute laser-aligned column widths ──────────────────────
    $LABEL_WIDTH = if ($cardWidth -gt 50) { 14 } else { 10 }
    $minValWidth = if ($cardWidth -gt 50) { 8 } else { 7 }
    $maxValWidth = if ($cardWidth -gt 50) { 12 } else { 9 }
    $VAL_WIDTH   = [Math]::Max($minValWidth, [Math]::Min($maxValWidth, $maxValLen))
    $GAUGE_WIDTH = [Math]::Max(8, $cardWidth - 4 - $LABEL_WIDTH - $VAL_WIDTH - 2)

    # ── Phase 3: Render lines with strict tear-free formatting ────────────
    foreach ($item in $renderedItems) {
        $line = $item.Line
        $renderVal = $item.RenderVal
        $valRaw = $item.ValFormatted

        $gaugeRes = Render-Gauge $line $renderVal $GAUGE_WIDTH $alertPhase $strictSEAscii

        # Dynamic label interpolation
        $lbl = $line.Label
        if ($lbl -match '\{val\}|\{percent\}') {
            $lbl = $lbl.Replace('{percent}', "$([int][Math]::Round($renderVal))%").Replace('{val}', $renderVal.ToString('F1'))
        }

        # Strict visual character calculations
        $lblClean = if ($lbl.Length -gt $LABEL_WIDTH) { $lbl.Substring(0, $LABEL_WIDTH) } else { $lbl.PadRight($LABEL_WIDTH) }
        $valClean = if ($valRaw.Length -gt $VAL_WIDTH) { $valRaw.Substring(0, $VAL_WIDTH) } else { $valRaw.PadLeft($VAL_WIDTH) }

        $sb = [System.Text.StringBuilder]::new()
        [void]$sb.Append("${borderCol}$($chars.V)$($script:ANSI.Reset) ")
        [void]$sb.Append("$($script:ESC)[38;2;203;213;225m${lblClean}$($script:ANSI.Reset) ")
        [void]$sb.Append($gaugeRes.RenderedGauge)
        [void]$sb.Append(" ")
        [void]$sb.Append("$($gaugeRes.ColorCode)${valClean}$($script:ANSI.Reset)")
        [void]$sb.Append(" ${borderCol}$($chars.V)$($script:ANSI.Reset)")

        $lines.Add($sb.ToString())
    }

    # Add overflow line if panel has more lines than screen height limit
    if ($maxLines -gt 0 -and $panel.Lines.Count -gt $maxLines) {
        $hiddenCount = $panel.Lines.Count - $maxLines
        $overMsg = "[... +$hiddenCount lines (screen height limit) ...]"
        if ($overMsg.Length -gt ($cardWidth - 4)) {
            $overMsg = "[... +$hiddenCount lines ...]"
        }
        $sbOver = [System.Text.StringBuilder]::new()
        [void]$sbOver.Append("${borderCol}$($chars.V)$($script:ANSI.Reset) ")
        [void]$sbOver.Append("$($script:ANSI.Dim)$($overMsg.PadRight($cardWidth - 4))$($script:ANSI.Reset)")
        [void]$sbOver.Append(" ${borderCol}$($chars.V)$($script:ANSI.Reset)")
        $lines.Add($sbOver.ToString())
    }

    $lines.Add((Format-TuiFooter $cardWidth $borderCol $borderStyle $strictSEAscii $bGrad))
    Set-CardBg $lines $panel $screen $sim $alertPhase
    return $lines
}

# -------------------------------------------------------------------------
# Execution & Layout State
# -------------------------------------------------------------------------
# Fluid damping lerp state: line index -> smoothed DisplayValue
$displayValues = [System.Collections.Generic.Dictionary[string, double]]::new()

$CARD_WIDTH = 41

# -------------------------------------------------------------------------
# Active Value Unit Mode Resolver (-Measurement | -Percent | Auto)
# -------------------------------------------------------------------------
$activeUnitMode = if ($Measurement) { 'Measurement' } elseif ($Percent) { 'Percent' } else { 'Auto' }

# -------------------------------------------------------------------------
# Hardware Profile Resolver (-ScreenProfile TextPanel | WideLCD | ...)
# -------------------------------------------------------------------------
if ($RenderMode -eq 'Ascii') {
    $StrictSEAscii = $true
    if ($Border -eq 'default') { $Border = 'ascii' }
} elseif ($RenderMode -eq 'Graphic') {
    $StrictSEAscii = $false
    if ($Border -eq 'default' -and (-not [string]::IsNullOrEmpty($screen.Border)) -and $screen.Border -eq 'ascii') {
        $Border = 'graphic'
    }
} elseif ($RenderMode -eq 'Auto') {
    if (-not $StrictSEAscii -and $screen.Border -eq 'ascii') {
        $StrictSEAscii = $true
    }
}

$panels = $screen.Panels
if ($panels.Count -eq 0) {
    $panels = [System.Collections.Generic.List[HelmCore.PanelDef]]::new()
    $dummy = [HelmCore.PanelDef]::new()
    $dummy.Lines = $screen.Lines
    $dummy.Title = "MAIN MONITOR"
    $panels.Add($dummy)
}

$termWidth = try { [Console]::WindowWidth } catch { 80 }
if ($termWidth -le 0) { $termWidth = 80 }

# -------------------------------------------------------------------------
# Space Engineers Physical Surface Dimension & Bezel Resolver
# -------------------------------------------------------------------------
$activeScreenProfile = $ScreenProfile
if ($activeScreenProfile -eq 'Auto') {
    if ($screen.Profile -eq 'corner' -or $screen.OutputBlockName -like '*Corner*') {
        $activeScreenProfile = 'CornerLCD'
    } elseif ($screen.Profile -eq 'compact' -or $screen.OutputBlockName -like '*Cockpit*') {
        $activeScreenProfile = 'CockpitSmall'
    } elseif ($screen.Profile -eq 'wide' -or $screen.OutputBlockName -like '*Wide*' -or $screen.GridCols -ge 3 -or $panels.Count -gt 4) {
        $activeScreenProfile = 'WideLCD'
    } else {
        $activeScreenProfile = 'TextPanel'
    }
}

$seSurfaceSpecs = @{
    'TextPanel'     = @{ Name = "Large Grid Text Panel (1x1)"; Res = "512x512"; Aspect = "1:1";  Bezel = "HEAVY INDUSTRIAL" }
    'WideLCD'       = @{ Name = "Wide LCD Surface (2x1)";       Res = "1024x512"; Aspect = "2:1"; Bezel = "COMMAND BRIDGE" }
    'CockpitSmall'  = @{ Name = "Cockpit Instrument HUD";      Res = "512x256"; Aspect = "2:1";  Bezel = "AVIONICS FLIGHT CLAMP" }
    'CornerLCD'     = @{ Name = "Corner LCD Segment";          Res = "512x128"; Aspect = "4:1";  Bezel = "SLANTED BULKHEAD" }
    'CockpitCorner' = @{ Name = "Fighter Cockpit Mini-HUD";    Res = "256x128"; Aspect = "2:1";  Bezel = "COMPACT CONSOLE" }
    'Off'           = @{ Name = "Standard TUI Stream";         Res = "Virtual"; Aspect = "N/A";  Bezel = "NONE" }
}
$activeSpec = if ($seSurfaceSpecs.ContainsKey($activeScreenProfile)) { $seSurfaceSpecs[$activeScreenProfile] } else { $seSurfaceSpecs['TextPanel'] }

$maxLinesPerPanel = switch ($activeScreenProfile) {
    'CornerLCD'     { 2 }
    'CockpitCorner' { 2 }
    'CockpitSmall'  { 3 }
    default         { 0 }
}

$isWideProfile = ($activeScreenProfile -eq 'WideLCD' -or $activeScreenProfile -eq 'CornerLCD')
$multiColumn = ($MultiColumn -or ($isWideProfile -and $panels.Count -gt 1) -or ($panels.Count -gt 1 -and $termWidth -ge 84)) -and (-not $SingleColumn)

# Multi-column grid rows setup
$panelRows = [System.Collections.Generic.List[System.Collections.Generic.List[HelmCore.PanelDef]]]::new()
if ($multiColumn -and $panels.Count -ne 3) {
    for ($p = 0; $p -lt $panels.Count; $p += 2) {
        $r = [System.Collections.Generic.List[HelmCore.PanelDef]]::new()
        $r.Add($panels[$p])
        if (($p + 1) -lt $panels.Count) { $r.Add($panels[$p + 1]) }
        $panelRows.Add($r)
    }
}

$useAltBuffer = -not [Console]::IsOutputRedirected
Write-Host $script:ANSI.HideCursor -NoNewline
try {
    if ($useAltBuffer) {
        [Console]::Write("$($script:ESC)[?1049h")
        [Console]::Write("$($script:ESC)[2J")
    } else {
        Write-Host $script:ANSI.ClearScreen -NoNewline
    }

    $frameCount = if ($Interactive) { 99999 } else { $Frames }

    for ($frame = 0; $frame -lt $frameCount; $frame++) {
        $sim.Step(0.08)
        $alertPhase = ($frame % 8) -lt 4

        # Staggered Boot Sequence Animation (-Boot)
        if ($Boot -and $frame -lt 4) {
            $rawBannerLines = @(
                "  _    _ ______ _      __  __ ",
                " | |  | |  ____| |    |  \/  |",
                " | |__| | |__  | |    | \  / |  >> HELM SYSTEM INITIALIZATION <<",
                " |  __  |  __| | |    | |\/| |     Cold-Start Bus Power-On",
                " | |  | | |____| |____| |  | |     Physical Screens Detected: 1",
                " |_|  |_|______|______|_|  |_|     Status: ALL SYSTEMS NOMINAL"
            )

            # Resolve Gradient or Solid Color for Startup Logo
            $gradientStops = $screen.BootGradientColors
            if (-not $gradientStops -or $gradientStops.Length -eq 0) {
                if (-not [string]::IsNullOrEmpty($screen.BootColor)) {
                    $gradientStops = @($screen.BootColor)
                } else {
                    $gradientStops = @("cyan", "#38bdf8")
                }
            }

            $bSb = [System.Text.StringBuilder]::new()
            [void]$bSb.Append("$($script:ESC)[H")

            for ($bi = 0; $bi -lt $rawBannerLines.Count; $bi++) {
                $ratio = if ($rawBannerLines.Count -gt 1) { [double]$bi / ($rawBannerLines.Count - 1) } else { 0.0 }
                $lineColor = Get-InterpolatedAnsiColor $gradientStops $ratio
                [void]$bSb.AppendLine("${lineColor}$($rawBannerLines[$bi])$($script:ANSI.Reset)$($script:ESC)[K")
            }

            $bootMsg = if ($screen.BootMessage) { $screen.BootMessage } else { "HELM TELEMETRY KERNEL ONLINE" }
            [void]$bSb.AppendLine("$($script:ESC)[K")
            [void]$bSb.AppendLine("$($script:ANSI.Bold)$($script:ESC)[38;2;34;197;94m>> ${bootMsg}$($script:ANSI.Reset)$($script:ESC)[K")
            [void]$bSb.AppendLine("$($script:ESC)[38;2;148;163;184m>> Stagger Cadence: $($screen.BootStaggerTicks) ticks/screen | Duration: $($screen.BootDuration)s$($script:ANSI.Reset)$($script:ESC)[K")
            [void]$bSb.AppendLine("$($script:ESC)[K")

            # Progressive Trailing Diagnostic Boot Logs
            $effectiveLogs = $screen.GetEffectiveBootLogs()
            $logsToShow = [int][Math]::Ceiling((($frame + 1) / 4.0) * $effectiveLogs.Count)
            for ($li = 0; $li -lt $logsToShow; $li++) {
                $logText = $effectiveLogs[$li]
                $prefix = if ($li -eq ($effectiveLogs.Count - 1) -and $frame -ge 3) {
                    "$($script:ANSI.Bold)$($script:ESC)[38;2;34;197;94m[OK]$($script:ANSI.Reset)"
                } else {
                    "$($script:ESC)[38;2;56;189;248m[..]$($script:ANSI.Reset)"
                }
                [void]$bSb.AppendLine("${prefix} $($script:ESC)[38;2;203;213;225m${logText}$($script:ANSI.Reset)$($script:ESC)[K")
            }

            [void]$bSb.Append("$($script:ESC)[J")
            [Console]::Write($bSb.ToString())

            $totalBootSec = if ($PSBoundParameters.ContainsKey('BootDuration') -and $BootDuration -gt 0) {
                $BootDuration
            } else {
                $screen.BootDuration
            }
            if ($totalBootSec -le 0) { $totalBootSec = 2.5 }
            $stageDelayMs = [int][Math]::Max(50, [Math]::Round(($totalBootSec * 1000.0) / 4.0))
            [System.Threading.Thread]::Sleep($stageDelayMs)
            continue
        }

        # Liquid calibration sweep during boot
        $calibOverride = $null
        if ($Boot -and $frame -ge 4 -and $frame -le 7) {
            $calibOverride = [Math]::Round((($frame - 3) / 4.0) * 100.0, 1)
        }

        $buffer = [System.Text.StringBuilder]::new()
        [void]$buffer.Append("$($script:ESC)[H") # Zero-flicker cursor reset to home

        # Title Header
        $presetLabel = if ($PSCmdlet.ParameterSetName -eq 'Custom') { "Custom DSL" } else { $Preset }
        $rateLabel = "${UpdateFrequency} (${effectiveIntervalMs}ms)"
        $simTitle = if ($multiColumn) {
            "=== HELM CONSOLE TELEMETRY TUI SIMULATOR (SPACE ENGINEERS) ==="
        } else {
            "=== HELM TELEMETRY SIMULATOR ==="
        }
        $bootPhaseLabel = if ($calibOverride -ne $null) { " | Boot: CALIBRATION SWEEP (${calibOverride}%)" } else { "" }
        $unitModeLabel = switch ($activeUnitMode) {
            'Measurement' { "Measurements" }
            'Percent'     { "Percent (%)" }
            default       { "Auto" }
        }
        [void]$buffer.AppendLine("$($script:ESC)[1m$($script:ESC)[38;2;56;189;248m${simTitle}$($script:ANSI.Reset)$($script:ESC)[K")
        [void]$buffer.AppendLine("$($script:ESC)[38;2;148;163;184mPreset: $($script:ESC)[1m${presetLabel}$($script:ESC)[22m | Rate: $rateLabel | Units: $($script:ESC)[1m$($script:ESC)[38;2;56;189;248m${unitModeLabel}$($script:ANSI.Reset)$($script:ESC)[38;2;148;163;184m | Lerp: $(if ($DampingLerp){'0.35'}else{'OFF'})${bootPhaseLabel} | Frame: $frame$($script:ANSI.Reset)$($script:ESC)[K")
        [void]$buffer.AppendLine("$($script:ESC)[K")

        # -----------------------------------------------------------------
        # Space Engineers Hardware Screen Housing (Top Bezel)
        # -----------------------------------------------------------------
        $bezelActive = ($activeScreenProfile -ne 'Off')
        $totalFrameWidth = if ($multiColumn -or $activeScreenProfile -eq 'WideLCD') { 84 } else { $CARD_WIDTH }
        $bezelColor = "$($script:ESC)[38;2;100;116;139m"
        $accentColor = "$($script:ESC)[38;2;56;189;248m"

        $bracketL = "[#]"
        $bracketR = "[#]"
        $edgeH    = "="
        $subSepChar = "═"

        if ($StrictSEAscii) {
            $bracketL = "+-"
            $bracketR = "-+"
            $edgeH    = "-"
            $subSepChar = "-"
        } else {
            switch ($activeScreenProfile) {
                'WideLCD' {
                    $bracketL = "[x]"
                    $bracketR = "[x]"
                    $edgeH    = "="
                    $subSepChar = "═"
                }
                'CockpitSmall' {
                    $bracketL = "[+]"
                    $bracketR = "[+]"
                    $edgeH    = "="
                    $subSepChar = "─"
                }
                'CornerLCD' {
                    $bracketL = "//"
                    $bracketR = "\\\\"
                    $edgeH    = "="
                    $subSepChar = "─"
                }
                'CockpitCorner' {
                    $bracketL = "|>"
                    $bracketR = "<|"
                    $edgeH    = "="
                    $subSepChar = "─"
                }
                default {
                    # TextPanel
                    $bracketL = "[#]"
                    $bracketR = "[#]"
                    $edgeH    = "="
                    $subSepChar = "═"
                }
            }
        }

        $bWidth = [Math]::Max(10, $totalFrameWidth - $bracketL.Length - $bracketR.Length)
        $subSep = $bracketL + [string]::new($subSepChar, $bWidth) + $bracketR

        if ($bezelActive) {
            $housingTag = " [ SE HARDWARE: $($activeSpec.Name) | $($activeSpec.Res) ($($activeSpec.Aspect)) ] "
            if ($housingTag.Length -gt ($bWidth - 2)) {
                $housingTag = " [ $($activeSpec.Name) | $($activeSpec.Res) ] "
            }
            $dash1 = [Math]::Max(1, [int][Math]::Floor(($bWidth - $housingTag.Length) / 2))
            $dash2 = [Math]::Max(1, $bWidth - $housingTag.Length - $dash1)
            [void]$buffer.AppendLine("${bezelColor}${bracketL}$([string]::new($edgeH, $dash1))${accentColor}${housingTag}${bezelColor}$([string]::new($edgeH, $dash2))${bracketR}$($script:ANSI.Reset)$($script:ESC)[K")

            $specText = " [BEZEL: $($activeSpec.Bezel)] Surface Aspect: $($activeSpec.Aspect) | VRage Viewport: $($activeSpec.Res) px "
            $padEnd = [Math]::Max(1, $bWidth - $specText.Length)
            [void]$buffer.AppendLine("${bezelColor}${bracketL}${accentColor}${specText}$([string]::new(' ', $padEnd))${bracketR}$($script:ANSI.Reset)$($script:ESC)[K")
            [void]$buffer.AppendLine("${bezelColor}${subSep}$($script:ANSI.Reset)$($script:ESC)[K")
        }

        # -----------------------------------------------------------------
        # Render Panels
        # -----------------------------------------------------------------
        if ($panels.Count -eq 3 -and $multiColumn) {
            # 3-panel asymmetric layout (e.g. Rover cockpit: Left 2 stacked, Right 1 tall)
            $leftLines = [System.Collections.Generic.List[string]]::new()
            $c0 = Render-PanelCardLines $panels[0] $screen $sim $displayValues $DampingLerp $alertPhase $StrictSEAscii $CARD_WIDTH $calibOverride $maxLinesPerPanel $activeUnitMode
            $c1 = Render-PanelCardLines $panels[1] $screen $sim $displayValues $DampingLerp $alertPhase $StrictSEAscii $CARD_WIDTH $calibOverride $maxLinesPerPanel $activeUnitMode
            foreach ($l in $c0) { $leftLines.Add($l) }
            foreach ($l in $c1) { $leftLines.Add($l) }

            $rightCard = [System.Collections.Generic.List[string]]::new([string[]](Render-PanelCardLines $panels[2] $screen $sim $displayValues $DampingLerp $alertPhase $StrictSEAscii $CARD_WIDTH $calibOverride $maxLinesPerPanel $activeUnitMode))
            $pBCol2 = $panels[2].BorderColor
            if ([string]::IsNullOrEmpty($pBCol2)) { $pBCol2 = $screen.BorderColor }
            $bCol2 = Convert-HelmColorToAnsi $pBCol2

            while ($rightCard.Count -lt $leftLines.Count) {
                $rightCard.Insert($rightCard.Count - 1, (Format-TuiLine "" ($CARD_WIDTH - 4) "" $bCol2))
            }

            for ($i = 0; $i -lt $leftLines.Count; $i++) {
                [void]$buffer.AppendLine("$($leftLines[$i])  $($rightCard[$i])$($script:ESC)[K")
            }
        } elseif ($multiColumn) {
            # Multi-column grid rows (2 panels per row side-by-side)
            foreach ($row in $panelRows) {
                if ($row.Count -eq 1) {
                    $singleWidth = if ($activeScreenProfile -eq 'WideLCD') { 84 } else { $CARD_WIDTH }
                    $c = Render-PanelCardLines $row[0] $screen $sim $displayValues $DampingLerp $alertPhase $StrictSEAscii $singleWidth $calibOverride $maxLinesPerPanel $activeUnitMode
                    foreach ($l in $c) {
                        [void]$buffer.AppendLine("${l}$($script:ESC)[K")
                    }
                } else {
                    $c1 = [System.Collections.Generic.List[string]]::new([string[]](Render-PanelCardLines $row[0] $screen $sim $displayValues $DampingLerp $alertPhase $StrictSEAscii $CARD_WIDTH $calibOverride $maxLinesPerPanel $activeUnitMode))
                    $c2 = [System.Collections.Generic.List[string]]::new([string[]](Render-PanelCardLines $row[1] $screen $sim $displayValues $DampingLerp $alertPhase $StrictSEAscii $CARD_WIDTH $calibOverride $maxLinesPerPanel $activeUnitMode))

                    $pBCol1 = $row[0].BorderColor; if ([string]::IsNullOrEmpty($pBCol1)) { $pBCol1 = $screen.BorderColor }
                    $pBCol2 = $row[1].BorderColor; if ([string]::IsNullOrEmpty($pBCol2)) { $pBCol2 = $screen.BorderColor }
                    $bCol1 = Convert-HelmColorToAnsi $pBCol1
                    $bCol2 = Convert-HelmColorToAnsi $pBCol2

                    $maxL = [Math]::Max($c1.Count, $c2.Count)
                    while ($c1.Count -lt $maxL) {
                        $c1.Insert($c1.Count - 1, (Format-TuiLine "" ($CARD_WIDTH - 4) "" $bCol1))
                    }
                    while ($c2.Count -lt $maxL) {
                        $c2.Insert($c2.Count - 1, (Format-TuiLine "" ($CARD_WIDTH - 4) "" $bCol2))
                    }

                    for ($i = 0; $i -lt $maxL; $i++) {
                        [void]$buffer.AppendLine("$($c1[$i])  $($c2[$i])$($script:ESC)[K")
                    }
                }
            }
        } else {
            # Single-column stacked layout
            $sWidth = if ($panels.Count -eq 1 -and $activeScreenProfile -eq 'WideLCD') { 84 } else { $CARD_WIDTH }
            foreach ($panel in $panels) {
                $c = Render-PanelCardLines $panel $screen $sim $displayValues $DampingLerp $alertPhase $StrictSEAscii $sWidth $calibOverride $maxLinesPerPanel $activeUnitMode
                foreach ($l in $c) {
                    [void]$buffer.AppendLine("${l}$($script:ESC)[K")
                }
            }
        }

        # -----------------------------------------------------------------
        # Space Engineers Hardware Screen Housing (Bottom Bezel)
        # -----------------------------------------------------------------
        if ($bezelActive) {
            $footerTag = " [ VRAGE SPRITE BUFFER: 0-GC | REFRESH: ${rateLabel} ] "
            $bDash1 = [Math]::Max(1, [int][Math]::Floor(($bWidth - $footerTag.Length) / 2))
            $bDash2 = [Math]::Max(1, $bWidth - $footerTag.Length - $bDash1)
            [void]$buffer.AppendLine("${bezelColor}${subSep}$($script:ANSI.Reset)$($script:ESC)[K")
            [void]$buffer.AppendLine("${bezelColor}${bracketL}$([string]::new($edgeH, $bDash1))${accentColor}${footerTag}${bezelColor}$([string]::new($edgeH, $bDash2))${bracketR}$($script:ANSI.Reset)$($script:ESC)[K")
        }

        # -----------------------------------------------------------------
        # In-Game Hardware Sprite & Performance Telemetry Inspector (-ShowSprites, -ShowStats)
        # Displays exact VRage MySprite allocations, mutations, and PB instruction budgets
        # -----------------------------------------------------------------
        if ($ShowSprites -or $ShowStats) {
            [void]$buffer.AppendLine("$($script:ESC)[K")
            $spBorderCol = "$($script:ESC)[38;2;168;85;247m" # Purple sci-fi accent

            # Compute official HelmCore screen telemetry statistics
            $stats = [HelmCore.Layout]::ComputeStats($screen)
            $totalSprites = $stats.TotalAllocatedSprites
            $mutatedThisFrame = $stats.MutatedPerTick

            if ($multiColumn) {
                # Multi-column mode: card width spans 84 columns matching dual panels ($CARD_WIDTH * 2 + 2 = 41 * 2 + 2 = 84)
                $SP_WIDTH = 84
                $SP_INNER = $SP_WIDTH - 4 # 80 chars
                [void]$buffer.AppendLine((Format-TuiHeader "HELM-CORE TELEMETRY & HARDWARE SPRITE STATISTICS" $SP_WIDTH $spBorderCol) + "$($script:ESC)[K")

                # Each half is 38 chars; joined with " | " (3 chars) = 38 + 3 + 38 = 79 chars (+ 1 space = 80 chars)
                $s1_l = "Allocated MySprites: $totalSprites".PadRight(38)
                $s1_r = "Per-Tick Allocs:     0.000 KB (0-byte)".PadRight(39)
                $s2_l = "In-Place Mutated:    $mutatedThisFrame quads/tick".PadRight(38)
                $s2_r = "Vanilla Textures:    Square, Tri, Circle".PadRight(39)
                $s3_l = "Render Pipeline:     VRage GPU Direct".PadRight(38)
                $s3_r = "Execution Model:     Retained Virtual Buffer".PadRight(39)
                $s4_l = "Normal Instructions: $($stats.EstInstructionCountNormal) / 50k (<0.1%)".PadRight(38)
                $s4_r = "Redraw Instructions: $($stats.EstInstructionCountRedraw) / 50k (<1.0%)".PadRight(39)
                $s5_l = "Active Sub-Panels:   $($stats.PanelCount) | Lines: $($stats.LineCount)".PadRight(38)
                $s5_r = "Dynamic Fill Slots:  $($stats.DynamicFillSprites) | Tracks: $($stats.StaticTrackSprites)".PadRight(39)

                [void]$buffer.AppendLine((Format-TuiLine "$s1_l | $s1_r" $SP_INNER "$($script:ESC)[38;2;226;232;240m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "$s2_l | $s2_r" $SP_INNER "$($script:ESC)[38;2;56;189;248m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "$s3_l | $s3_r" $SP_INNER "$($script:ESC)[38;2;34;197;94m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "$s4_l | $s4_r" $SP_INNER "$($script:ESC)[38;2;251;191;36m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "$s5_l | $s5_r" $SP_INNER "$($script:ESC)[38;2;192;132;252m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiFooter $SP_WIDTH $spBorderCol) + "$($script:ESC)[K")
            } else {
                # Single-column mode: card width is exactly 41 columns matching single panel
                $SP_WIDTH = 41
                $SP_INNER = $SP_WIDTH - 4 # 37 chars
                [void]$buffer.AppendLine((Format-TuiHeader "HELM-CORE STATS (0-ALLOC)" $SP_WIDTH $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "Allocated:  $totalSprites MySprites" $SP_INNER "$($script:ESC)[38;2;226;232;240m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "Mutated:    $mutatedThisFrame quads/tick" $SP_INNER "$($script:ESC)[38;2;56;189;248m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "Allocs:     0.000 KB (Zero-Alloc)" $SP_INNER "$($script:ESC)[38;2;34;197;94m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "Normal PB:  $($stats.EstInstructionCountNormal) / 50k (<0.1%)" $SP_INNER "$($script:ESC)[38;2;251;191;36m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "Redraw PB:  $($stats.EstInstructionCountRedraw) / 50k (<0.9%)" $SP_INNER "$($script:ESC)[38;2;251;191;36m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "Structure:  $($stats.PanelCount) panels, $($stats.LineCount) lines" $SP_INNER "$($script:ESC)[38;2;192;132;252m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiLine "Textures:   SquareSimple, Tri, Circ" $SP_INNER "$($script:ESC)[38;2;148;163;184m" $spBorderCol) + "$($script:ESC)[K")
                [void]$buffer.AppendLine((Format-TuiFooter $SP_WIDTH $spBorderCol) + "$($script:ESC)[K")
            }
        }

        [void]$buffer.AppendLine("$($script:ESC)[K")
        $controlsTip = if ($Interactive) {
            "Controls: [M]easurements | [P]ercent | [U]nits toggle | Ctrl+C to exit"
        } else {
            "Press Ctrl+C to stop simulation."
        }
        [void]$buffer.AppendLine("$($script:ESC)[38;2;100;116;139m${controlsTip}$($script:ANSI.Reset)$($script:ESC)[K")
        [void]$buffer.Append("$($script:ESC)[J") # Clear remaining lines below the buffer

        [Console]::Write($buffer.ToString())

        # Check for interactive hotkey toggles (safe non-redirected console check)
        try {
            if ($Interactive -and -not [Console]::IsInputRedirected -and [Console]::KeyAvailable) {
                $keyInfo = [Console]::ReadKey($true)
                if ($keyInfo.Key -eq [ConsoleKey]::M) {
                    $activeUnitMode = 'Measurement'
                } elseif ($keyInfo.Key -eq [ConsoleKey]::P) {
                    $activeUnitMode = 'Percent'
                } elseif ($keyInfo.Key -eq [ConsoleKey]::U -or $keyInfo.Key -eq [ConsoleKey]::Tab -or $keyInfo.Key -eq [ConsoleKey]::Spacebar) {
                    $activeUnitMode = if ($activeUnitMode -eq 'Measurement') { 'Percent' } elseif ($activeUnitMode -eq 'Percent') { 'Auto' } else { 'Measurement' }
                }
            }
        } catch { }

        [System.Threading.Thread]::Sleep($effectiveIntervalMs)
    }
}
finally {
    Write-Host $script:ANSI.ShowCursor -NoNewline
    if ($useAltBuffer) {
        [Console]::Write("$($script:ESC)[?1049l")
    }
    # Restore original terminal font if font mode was active
    if ($script:FontModeActive -and (Get-Command Restore-TerminalFont -ErrorAction SilentlyContinue)) {
        Restore-TerminalFont -Quiet
    }
    Write-Host "`n$($script:ESC)[38;2;34;197;94mHELM TUI Simulation Finished.$($script:ANSI.Reset)"
}

