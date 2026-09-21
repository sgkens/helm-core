#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Proves that the shipped, renamed script behaves exactly like the source it was built from.
.DESCRIPTION
    Runs Program.cs and HelmCore.cs (variant "source") and the shipped script in tools/spritedump/CombinedCheck/Wrapped.g.cs
    (variant "renamed") through their public entry points on every preset in layouts/default: the constructor, Main, run
    arguments, value changes and alerts, on fake blocks and screens. Every drawn sprite and every echoed line is written
    to a file per preset, and the two folders are compared. Run ./Build-Release.ps1 first.
.EXAMPLE
    ./tools/rename-check/Test-RenameParity.ps1
#>
[CmdletBinding()]
param(
    [string]$Work = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), 'helm-rename-parity')
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$Root    = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, '..', '..'))
$Project = [System.IO.Path]::Combine($PSScriptRoot, 'RenameCheck.csproj')
$Layouts = [System.IO.Path]::Combine($Root, 'layouts')
if (-not [System.IO.File]::Exists([System.IO.Path]::Combine($Root, 'tools', 'spritedump', 'CombinedCheck', 'Wrapped.g.cs'))) {
    throw "Wrapped.g.cs not found. Run ./Build-Release.ps1 first."
}

$dirs = @{}
foreach ($variant in 'source', 'renamed') {
    $out = [System.IO.Path]::Combine($Work, $variant)
    if ([System.IO.Directory]::Exists($out)) { [System.IO.Directory]::Delete($out, $true) }
    $dirs[$variant] = $out
    $log = & dotnet run --project $Project "-p:Variant=$variant" -- $Layouts $out 2>&1
    if ($LASTEXITCODE -ne 0) { throw "The $variant variant failed:`n$(($log | Select-Object -Last 15) -join "`n")" }
}

$differ = New-Object System.Collections.Generic.List[string]
$files = [System.IO.Directory]::GetFiles($dirs['source'], '*.txt')
$lines = 0
foreach ($f in $files) {
    $name = [System.IO.Path]::GetFileName($f)
    $a = [System.IO.File]::ReadAllText($f)
    $bPath = [System.IO.Path]::Combine($dirs['renamed'], $name)
    $b = if ([System.IO.File]::Exists($bPath)) { [System.IO.File]::ReadAllText($bPath) } else { $null }
    $lines += ($a.Split("`n")).Length
    if ($a -ne $b) { $differ.Add($name) }
}
if ($differ.Count -eq 0) {
    Write-Host "rename-parity: OK. $($files.Length) presets, $lines lines of sprites and echo identical." -ForegroundColor Green
    exit 0
}
Write-Host "rename-parity: FAILED. $($differ.Count) of $($files.Length) presets differ: $(($differ | Select-Object -First 8) -join ', ')" -ForegroundColor Red
Write-Host "  Compare the folders $($dirs['source']) and $($dirs['renamed'])."
exit 1
