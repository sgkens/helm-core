#!/usr/bin/env pwsh
<#
.SYNOPSIS
    HELM Telemetry Layout Tester & Custom Data Export Cmdlet.
.DESCRIPTION
    Validates, renders, and tests Space Engineers HELM DSL configurations.
    Provides offline simulation, runtime performance diagnostics, and clipboard export
    for instant deployment to Space Engineers Programmable Block Custom Data.
.PARAMETER Preset
    Built-in layout preset to test or export.
.PARAMETER Path
    Path to a custom DSL layout file (.txt or .helm).
.PARAMETER LayoutText
    Raw DSL layout text passed directly.
.PARAMETER CopyToClipboard
    Copies the layout DSL text to the system clipboard for immediate in-game pasting.
.PARAMETER ShowStats
    Displays PB instruction and sprite allocation statistics.
.PARAMETER ShowSprites
    Displays visual graphic sprite rendering preview.
.PARAMETER StrictSEAscii
    Renders using Space Engineers strict Monospace font glyphs.
.PARAMETER Boot
    Runs boot screen diagnostic sequence.
.PARAMETER Frames
    Number of frames to simulate (default: 60).
.PARAMETER Interactive
    Runs interactive loop until stopped.
.PARAMETER PassThru
    Passes the parsed configuration object to the PowerShell pipeline.
.EXAMPLE
    Test-HelmLayout -Preset thruster_balance -ShowStats -ShowSprites
.EXAMPLE
    .\Test-HelmLayout.ps1 -Preset bridge_matrix -CopyToClipboard
.EXAMPLE
    Test-HelmLayout -Path ./layouts/default/battery_monolith.txt -StrictSEAscii
#>

[CmdletBinding(DefaultParameterSetName = 'Preset')]
param(
    [Parameter(ParameterSetName = 'Preset', Position = 0)]
    [ValidateSet('rover', 'airlock', 'bridge', 'hazard_demo', 'mining', 'fighter', 'cargo', 'monolith',
                 'bridge_matrix', 'cargo_overview', 'battery_monolith', 'reactor_monolith', 'cargo_monolith',
                 'tactical_defense', 'thruster_balance', 'split_flight_deck', 'refinery_monolith',
                 'jump_drive_spool', 'medical_life_support', 'rover_cockpit', 'airlock_security',
                 'mining_rig', 'fighter_hud', 'hazard_alert', 'assembler_monolith', 'dynamic_telemetry',
                 'planetary_base', 'command_carrier', 'survival_essentials', 'deep_space_explorer',
                 'mining_fleet_coordinator', 'atmospheric_lander',
                 'single_line', 'power_only', 'cargo_minimal', 'corner_gauge_cluster', 'all_gauges_showcase', 'long_labels', 'numeric_extremes', 'border_styles_gallery', 'title_alignment',
                 'template_stamp', 'multi_screen_one_pb', 'grid_4x4', 'panel_spans', 'transparent_bg', 'gradient_stress', 'units_all', 'boot_custom', 'alert_states', 'hangar_bay', 'solar_farm', 'piston_crane', 'docking_control', 'hydrogen_plant', 'sensor_security', 'trading_station')]
    [string]$Preset = 'bridge',

    [Parameter(ParameterSetName = 'Path', Mandatory = $true)]
    [Alias('FilePath', 'File')]
    [string]$Path,

    [Parameter(ParameterSetName = 'Raw', Mandatory = $true)]
    [string]$LayoutText,

    [Parameter()]
    [Alias('Clipboard', 'Clip')]
    [switch]$CopyToClipboard,

    [Parameter()]
    [switch]$ShowStats,

    [Parameter()]
    [switch]$ShowSprites,

    [Parameter()]
    [switch]$StrictSEAscii,

    [Parameter()]
    [switch]$Boot,

    [Parameter()]
    [double]$BootDuration,

    [Parameter()]
    [int]$Frames = 60,

    [Parameter()]
    [ValidateSet('1x', '10x', '100x', 'Custom')]
    [string]$UpdateFrequency = '10x',

    [Parameter()]
    [switch]$Interactive,

    [Parameter()]
    [switch]$MultiColumn,

    [Parameter()]
    [switch]$SingleColumn,

    [Parameter()]
    [Alias('Measurements', 'Units', 'UseUnits')]
    [switch]$Measurement,

    [Parameter()]
    [Alias('Percentage', 'PercentMode')]
    [switch]$Percent,

    [Parameter()]
    [switch]$PassThru
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Copy-ToSystemClipboard {
    param([string]$Text)
    try {
        Set-Clipboard -Value $Text -ErrorAction Stop
        Write-Host "✓ Copied DSL configuration to clipboard! (Ready to paste into PB Custom Data)" -ForegroundColor Cyan
        return
    } catch { }

    # Cross-platform clipboard fallback (Linux/macOS)
    if (Get-Command 'xclip' -ErrorAction SilentlyContinue) {
        $Text | xclip -selection clipboard
        Write-Host "✓ Copied to clipboard via xclip!" -ForegroundColor Cyan
    } elseif (Get-Command 'wl-copy' -ErrorAction SilentlyContinue) {
        $Text | wl-copy
        Write-Host "✓ Copied to clipboard via wl-copy!" -ForegroundColor Cyan
    } elseif (Get-Command 'pbcopy' -ErrorAction SilentlyContinue) {
        $Text | pbcopy
        Write-Host "✓ Copied to clipboard via pbcopy!" -ForegroundColor Cyan
    } else {
        Write-Warning "Clipboard command not found in this environment. Outputting DSL below."
    }
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$helmvScript = Join-Path $scriptDir "helmv.ps1"
$demoScript = $helmvScript

# Resolve DSL content
$dslContent = ""
$resolvedCustomPath = $null

if ($PSCmdlet.ParameterSetName -eq 'Path') {
    if (-not [System.IO.File]::Exists($Path)) {
        throw "Specified layout file not found: '$Path'"
    }
    $dslContent = [System.IO.File]::ReadAllText($Path)
    $resolvedCustomPath = $Path
} elseif ($PSCmdlet.ParameterSetName -eq 'Raw') {
    $dslContent = $LayoutText
} else {
    # Check if a layout file exists in layouts/default/<Preset>.txt
    $presetFile = Join-Path $scriptDir "layouts/default/$Preset.txt"
    if ([System.IO.File]::Exists($presetFile)) {
        $dslContent = [System.IO.File]::ReadAllText($presetFile)
        $resolvedCustomPath = $presetFile
    }
}

# Handle CopyToClipboard
if ($CopyToClipboard) {
    if ([string]::IsNullOrEmpty($dslContent)) {
        # Extract builtin DSL from helmv.ps1 if no file exists
        Write-Host "Extracting configuration for preset '$Preset'..." -ForegroundColor DarkGray
        $dslContent = & pwsh -NoProfile -Command "
            `$demoDir = '$scriptDir'
            & (Join-Path `$demoDir 'helmv.ps1') -Preset '$Preset' -Frames 0
        " 2>$null
    }
    if (-not [string]::IsNullOrEmpty($dslContent)) {
        Copy-ToSystemClipboard -Text $dslContent
    }
}

# Forward to the helmv.ps1 simulator
$forwardArgs = @{}
if ($resolvedCustomPath) {
    $forwardArgs['CustomPreset'] = $resolvedCustomPath
} elseif ($PSCmdlet.ParameterSetName -eq 'Raw') {
    $forwardArgs['CustomPreset'] = $LayoutText
} else {
    $forwardArgs['Preset'] = $Preset
}

if ($Boot) { $forwardArgs['Boot'] = $true }
if ($BootDuration -gt 0) { $forwardArgs['BootDuration'] = $BootDuration }
if ($ShowStats) { $forwardArgs['ShowStats'] = $true }
if ($ShowSprites) { $forwardArgs['ShowSprites'] = $true }
if ($StrictSEAscii) { $forwardArgs['StrictSEAscii'] = $true }
if ($Interactive) { $forwardArgs['Interactive'] = $true }
if ($MultiColumn) { $forwardArgs['MultiColumn'] = $true }
if ($SingleColumn) { $forwardArgs['SingleColumn'] = $true }
if ($Measurement) { $forwardArgs['Measurement'] = $true }
if ($Percent) { $forwardArgs['Percent'] = $true }
$forwardArgs['Frames'] = $Frames
$forwardArgs['UpdateFrequency'] = $UpdateFrequency

& $demoScript @forwardArgs

# Tab Completion Helpers for Block Types and Presets
if (Get-Command 'Register-ArgumentCompleter' -ErrorAction SilentlyContinue) {
    Register-ArgumentCompleter -CommandName 'Test-HelmLayout', 'Test-HelmLayout.ps1' -ParameterName 'Preset' -ScriptBlock {
        param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameters)
        $presets = @('rover', 'airlock', 'bridge', 'hazard_demo', 'mining', 'fighter', 'cargo', 'monolith',
                     'bridge_matrix', 'cargo_overview', 'battery_monolith', 'reactor_monolith', 'cargo_monolith',
                     'tactical_defense', 'thruster_balance', 'split_flight_deck', 'refinery_monolith',
                     'jump_drive_spool', 'medical_life_support', 'rover_cockpit', 'airlock_security',
                     'mining_rig', 'fighter_hud', 'hazard_alert', 'assembler_monolith', 'dynamic_telemetry',
                     'planetary_base', 'command_carrier', 'survival_essentials', 'deep_space_explorer',
                     'mining_fleet_coordinator', 'atmospheric_lander',
                     'single_line', 'power_only', 'cargo_minimal', 'corner_gauge_cluster', 'all_gauges_showcase', 'long_labels', 'numeric_extremes', 'border_styles_gallery', 'title_alignment')
        $presets | Where-Object { $_ -like "$wordToComplete*" } | ForEach-Object {
            [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
        }
    }
}
