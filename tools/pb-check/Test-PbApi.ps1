#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Compiles HelmCombined.min.cs against Space Engineers' own assemblies, the way Check Code does.
.DESCRIPTION
    Wraps the script as the body of "partial class Program : MyGridProgram" with the game's default usings,
    compiles it as C# 6 against the game's DLLs, and runs the MDK analyzers, which apply the game's list of
    prohibited types (MDK01). A wrong member name on a block (CS1061) or a banned type is reported here
    instead of in the game. Needs a Space Engineers install; CI cannot carry the game's files.
.PARAMETER GameBin
    The game's Bin64 folder. Defaults to the SE_BIN64 environment variable. A Windows path
    (G:\...\Bin64) is accepted from WSL.
.EXAMPLE
    ./tools/pb-check/Test-PbApi.ps1 -GameBin 'G:\Applications\Steam\steamapps\common\SpaceEngineers\Bin64'
#>
[CmdletBinding()]
param(
    [string]$GameBin = $env:SE_BIN64
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$ToolDir  = $PSScriptRoot
$RepoRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($ToolDir, '..', '..'))
$Script   = [System.IO.Path]::Combine($RepoRoot, 'HelmCombined.min.cs')

if ([string]::IsNullOrWhiteSpace($GameBin)) {
    throw "Set -GameBin or the SE_BIN64 environment variable to the Space Engineers Bin64 folder."
}
# From WSL/Linux a Windows path is mapped to /mnt/<drive>/...
if (-not $IsWindows -and $GameBin -match '^([A-Za-z]):[\\/](.*)$') {
    $GameBin = '/mnt/' + $Matches[1].ToLowerInvariant() + '/' + ($Matches[2] -replace '\\', '/')
}
if (-not [System.IO.File]::Exists([System.IO.Path]::Combine($GameBin, 'Sandbox.Game.dll'))) {
    throw "Sandbox.Game.dll not found in '$GameBin'. Point -GameBin at the game's Bin64 folder."
}
if (-not [System.IO.File]::Exists($Script)) {
    throw "HelmCombined.min.cs not found. Run ./Build-Release.ps1 first."
}

# The game's default usings for Programmable Block scripts.
$usings = 'using Sandbox.Game.EntityComponents;using Sandbox.ModAPI.Ingame;using Sandbox.ModAPI.Interfaces;' +
          'using SpaceEngineers.Game.ModAPI.Ingame;using System.Collections.Generic;using System.Collections;' +
          'using System.Linq;using System.Text;using System;using VRage.Collections;using VRage.Game.Components;' +
          'using VRage.Game.GUI.TextPanel;using VRage.Game.ModAPI.Ingame.Utilities;using VRage.Game.ModAPI.Ingame;' +
          'using VRage.Game.ObjectBuilders.Definitions;using VRage.Game;using VRage;using VRageMath;'
$body = [System.IO.File]::ReadAllText($Script).TrimStart([char]0xFEFF)
$wrapped = $usings + "`nnamespace IngameScript{public partial class Program:MyGridProgram{`n" + $body + "`n}}"
[System.IO.File]::WriteAllText([System.IO.Path]::Combine($ToolDir, 'Wrapped.g.cs'), $wrapped, (New-Object System.Text.UTF8Encoding($false)))

$output = & dotnet build ([System.IO.Path]::Combine($ToolDir, 'PbCheck.csproj')) --nologo --no-incremental "-p:SE_BIN64=$GameBin" 2>&1
$exit = $LASTEXITCODE
$problems = $output | Where-Object { $_ -match ': (error|warning) ' } |
    ForEach-Object { ($_ -replace '^.*Wrapped\.g\.cs', 'Program') -replace '\s+\[.*\.csproj\]$', '' } | Sort-Object -Unique

if ($exit -eq 0 -and -not $problems) {
    Write-Host "pb-check: OK. HelmCombined.min.cs compiles against the game's assemblies with no prohibited APIs." -ForegroundColor Green
    exit 0
}
Write-Host "pb-check: FAILED" -ForegroundColor Red
$problems | ForEach-Object { Write-Host "  $_" }
exit 1
