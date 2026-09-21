<#
.SYNOPSIS
    Tests and builds the HELM Space Engineers Programmable Block release script.

.DESCRIPTION
    Runs the C# unit test suite (HelmCore.cs + Tests.cs via csc/mono), then combines
    Program.cs and HelmCore.cs into HelmCombined.cs and produces a minified
    production-ready build (HelmCombined.min.cs) strictly maintaining the Space
    Engineers Programmable Block 100,000-character hard ceiling.

    Credentials and environment are never required by this script. It is safe to run
    fully offline and in CI with no secrets.

.PARAMETER NoTest
    Skip the unit test phase and go straight to build. Not recommended for release builds.

.PARAMETER NoMinify
    Skip minification. Produces only HelmCombined.cs (the human-readable combined source).

.PARAMETER BudgetSoftLimit
    Character count at which a budget warning is emitted (default 99500).

.PARAMETER BudgetHardLimit
    Character count at which the build is aborted as over-budget (default 100000).

.EXAMPLE
    # Full test + build + minify (default)
    pwsh ./Build-Release.ps1

.EXAMPLE
    # Build only, skip tests
    pwsh ./Build-Release.ps1 -NoTest

.EXAMPLE
    # Test + build, no minification
    pwsh ./Build-Release.ps1 -NoMinify

.OUTPUTS
    HelmCombined.cs         — Human-readable combined source (reference / MDK build)
    HelmCombined.min.cs     — Minified PB-ready build (paste into Programmable Block)
#>

[CmdletBinding()]
param(
    [switch]$NoTest,
    [switch]$NoMinify,
    [int]$BudgetSoftLimit = 99500,
    [int]$BudgetHardLimit = 100000
)

$ErrorActionPreference = 'Stop'
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

# ---------------------------------------------------------------------------
# Header
# ---------------------------------------------------------------------------
Write-Host ""
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host "   HELM — Build-Release Pipeline" -ForegroundColor Cyan
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host ""

$stepTotal = if ($NoMinify) { 3 } else { 4 }
$step = 0

# ---------------------------------------------------------------------------
# STEP 1: Unit Tests
# ---------------------------------------------------------------------------
if (-not $NoTest) {
    $step++
    Write-Host "  [$step/$stepTotal] Unit Tests" -ForegroundColor Yellow

    $testBin    = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "test_helm_build.exe")
    $corePath   = Join-Path $ScriptDir "HelmCore.cs"
    $testsPath  = Join-Path $ScriptDir "Tests.cs"

    # Prefer csc on PATH; fall back to mcs (Mono on some distros)
    $compiler = if (Get-Command csc -ErrorAction SilentlyContinue) { "csc" }
                elseif (Get-Command mcs -ErrorAction SilentlyContinue) { "mcs" }
                else { throw "No C# compiler found on PATH (csc or mcs required)." }

    Write-Host "      Compiler : $compiler" -ForegroundColor DarkGray
    Write-Host "      Output   : $testBin"  -ForegroundColor DarkGray

    $compileRes = & bash -c "$compiler -out:`"$testBin`" `"$corePath`" `"$testsPath`"" 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Error "Test compilation failed:`n$compileRes"
        exit 1
    }

    $runRes = & bash -c "mono `"$testBin`"" 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Error "Unit tests failed:`n$runRes"
        exit 1
    }

    Write-Host "      $($runRes.Trim())" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "  [--] Unit Tests  (skipped via -NoTest)" -ForegroundColor DarkGray
    Write-Host ""
}

# ---------------------------------------------------------------------------
# STEP 2: Combine Program.cs + HelmCore.cs → HelmCombined.cs
# ---------------------------------------------------------------------------
$step++
Write-Host "  [$step/$stepTotal] Combine" -ForegroundColor Yellow

$programContent = Get-Content (Join-Path $ScriptDir "Program.cs")   -Raw
$coreContent    = Get-Content (Join-Path $ScriptDir "HelmCore.cs")  -Raw

# The Programmable Block editor compiles the pasted text as the BODY of
#   partial class Program : MyGridProgram { <text> }
# with the game's usings already in place. The text therefore has no `using`, no `namespace` and no
# class declaration, and every helper type must be nested inside Program.

# Body of `partial class Program : MyGridProgram { ... }` in Program.cs. The file ends with the class
# brace and then the namespace brace.
$classMatch  = [regex]::Match($programContent, '(?m)^\s*partial class Program\b')
if (-not $classMatch.Success) { throw "Program.cs: 'partial class Program' declaration not found" }
$classOpen   = $programContent.IndexOf('{', $classMatch.Index)
$nsClose     = $programContent.LastIndexOf('}')
$classClose  = $programContent.LastIndexOf('}', $nsClose - 1)
$progBody    = $programContent.Substring($classOpen + 1, $classClose - $classOpen - 1).Trim("`r", "`n")

# Code between "#if HELM_TOOLS" and "#endif" is for the test tools only and is not shipped.
$toolsBlock = '(?ms)^[ \t]*#if HELM_TOOLS[^\n]*\n.*?^[ \t]*#endif[^\n]*\n'
$progBody = [regex]::Replace([string]$progBody, $toolsBlock, '')

# Stamp the version from the VERSION file into the script (shown in the console header). An empty file keeps 0.0.0.
$verFile = [System.IO.Path]::Combine($ScriptDir, 'VERSION')
if ([System.IO.File]::Exists($verFile)) {
    $ver = [System.IO.File]::ReadAllText($verFile).Trim().TrimStart('v', 'V')
    if ($ver -match '^[0-9A-Za-z.+\-]+$') {
        $progBody = ([string]$progBody).Replace('HELM_VERSION = "0.0.0"', 'HELM_VERSION = "' + $ver + '"')
    }
}

# Body inside `namespace HelmCore { ... }` in HelmCore.cs (types nested into Program below)
$startNs = $coreContent.IndexOf("namespace HelmCore")
if ($startNs -lt 0) { throw "HelmCore.cs: 'namespace HelmCore' not found" }
$openBrace  = $coreContent.IndexOf('{', $startNs)
$lastBrace  = $coreContent.LastIndexOf('}')
$coreBody   = $coreContent.Substring($openBrace + 1, $lastBrace - $openBrace - 1).Trim("`r", "`n")

$coreBody = [regex]::Replace([string]$coreBody, $toolsBlock, '')

$combined = @"
// ==============================================================================
// HELM: High-Efficiency Lightweight Monitor for Space Engineers
// Paste into the Programmable Block editor (replaces everything in it).
// This is the body of Program: no usings, namespace or class declaration.
// ==============================================================================

$progBody

    // ==========================================================================
    // HELM CORE LIBRARY: DSL Parser, Layout, Color Engine, Aggregator
    // ==========================================================================

$coreBody
"@

$combinedPath = Join-Path $ScriptDir "HelmCombined.cs"
[System.IO.File]::WriteAllText($combinedPath, $combined, [System.Text.Encoding]::UTF8)
$charCount = $combined.Length

Write-Host "      HelmCombined.cs  $charCount chars ($([Math]::Round($charCount / 1000, 1)) k)" -ForegroundColor Green
Write-Host ""

# ---------------------------------------------------------------------------
# Comment and whitespace removal for C# source
#
# String, verbatim, interpolated and char literals are copied untouched. Comments are dropped. At each run of
# whitespace or comments in code a space is kept only where removing it could join two tokens into one
# (identifier next to identifier, two operator characters, a digit before '.', anything next
# to '?' or ':'). Lines are wrapped after ';', '{' or '}' once they pass $WrapAt characters so
# the in-game editor never receives one very long line. tools/minify-check proves the result
# has the same token stream as the source.
# ---------------------------------------------------------------------------
function Get-LiteralEnd {
    param([string]$s, [int]$i)
    $n = $s.Length
    $verbatim = $false; $interp = $false
    $j = $i
    while ($j -lt $n -and ($s[$j] -eq '@' -or $s[$j] -eq '$')) {
        if ($s[$j] -eq '@') { $verbatim = $true } else { $interp = $true }
        $j++
    }
    if ($j -ge $n) { return -1 }
    $q = $s[$j]
    if ($q -eq "'") {
        if ($j -ne $i) { return -1 }
        $j++
        while ($j -lt $n -and $s[$j] -ne "'") { if ($s[$j] -eq '\') { $j++ }; $j++ }
        return $j + 1
    }
    if ($q -ne '"') { return -1 }
    $j++
    $depth = 0
    while ($j -lt $n) {
        $ch = $s[$j]
        if ($verbatim) {
            if ($ch -eq '"') {
                if ($j + 1 -lt $n -and $s[$j + 1] -eq '"') { $j += 2; continue }
                if ($depth -eq 0) { return $j + 1 }
            }
        } else {
            if ($ch -eq '\' -and $depth -eq 0) { $j += 2; continue }
            if ($ch -eq '"' -and $depth -eq 0) { return $j + 1 }
        }
        if ($interp) {
            if ($ch -eq '{') {
                if ($depth -eq 0 -and $j + 1 -lt $n -and $s[$j + 1] -eq '{') { $j += 2; continue }
                $depth++
            } elseif ($ch -eq '}') {
                if ($depth -gt 0) { $depth-- }
                elseif ($j + 1 -lt $n -and $s[$j + 1] -eq '}') { $j += 2; continue }
            } elseif ($depth -gt 0 -and ($ch -eq '"' -or $ch -eq "'")) {
                $e = Get-LiteralEnd $s $j
                if ($e -gt 0) { $j = $e; continue }
            }
        }
        $j++
    }
    return $n
}

function Get-TriviaEnd {
    param([string]$s, [int]$i)
    $n = $s.Length
    while ($i -lt $n) {
        $c = $s[$i]
        if ([char]::IsWhiteSpace($c)) { $i++; continue }
        if ($c -eq '/' -and $i + 1 -lt $n) {
            if ($s[$i + 1] -eq '/') {
                $e = $s.IndexOf("`n", $i)
                if ($e -lt 0) { return $n }
                $i = $e; continue
            }
            if ($s[$i + 1] -eq '*') {
                $e = $s.IndexOf('*/', $i + 2)
                if ($e -lt 0) { return $n }
                $i = $e + 2; continue
            }
        }
        break
    }
    return $i
}

function Compress-CSharpWhitespace {
    param([string]$Source, [int]$WrapAt = 160)
    $structural = '{}()[];,'
    $operators  = '+-*/%&|^!~<>=?.:'
    $sb  = New-Object System.Text.StringBuilder($Source.Length)
    $len = $Source.Length
    $col = 0
    $i   = 0
    while ($i -lt $len) {
        $c = $Source[$i]

        if ($c -eq '"' -or $c -eq "'" -or $c -eq '@' -or $c -eq '$') {
            $e = Get-LiteralEnd $Source $i
            if ($e -gt 0) {
                $seg = $Source.Substring($i, $e - $i)
                [void]$sb.Append($seg)
                $nl = $seg.LastIndexOf("`n")
                if ($nl -ge 0) { $col = $seg.Length - $nl - 1 } else { $col += $seg.Length }
                $i = $e
                continue
            }
        }

        # "private" is the default for members of a class, so it is dropped, except before get/set (private set).
        if ($c -eq 'p' -and $i + 8 -lt $len -and [string]::CompareOrdinal($Source, $i, 'private', 0, 7) -eq 0 -and
            ($i -eq 0 -or -not ([char]::IsLetterOrDigit($Source[$i - 1]) -or $Source[$i - 1] -eq '_')) -and [char]::IsWhiteSpace($Source[$i + 7])) {
            $k = $i + 7
            while ($k -lt $len -and [char]::IsWhiteSpace($Source[$k])) { $k++ }
            $e = $k
            while ($e -lt $len -and [char]::IsLetter($Source[$e])) { $e++ }
            $next = $Source.Substring($k, $e - $k)
            if ($next -ne 'set' -and $next -ne 'get' -and $next -ne 'init' -and $next -ne 'protected') {
                $i += 7
                continue
            }
        }

        $t = if ([char]::IsWhiteSpace($c) -or $c -eq '/') { Get-TriviaEnd $Source $i } else { $i }
        if ($t -gt $i) {
            $i = $t
            if ($i -ge $len -or $sb.Length -eq 0) { continue }
            $p = $sb[$sb.Length - 1]
            $n = $Source[$i]
            $keep = $false
            if ($p -eq "`n") {
                $keep = $false
            } elseif ($structural.IndexOf($p) -ge 0 -or $structural.IndexOf($n) -ge 0) {
                $keep = $false
            } elseif ($p -eq '?' -or $p -eq ':' -or $n -eq '?' -or $n -eq ':') {
                $keep = $true
            } elseif (([char]::IsLetterOrDigit($p) -or $p -eq '_' -or $p -eq '@' -or $p -eq '$' -or $p -eq '"' -or $p -eq "'") -and
                      ([char]::IsLetterOrDigit($n) -or $n -eq '_' -or $n -eq '@' -or $n -eq '$' -or $n -eq '"' -or $n -eq "'")) {
                $keep = $true
            } elseif ($operators.IndexOf($p) -ge 0 -and $operators.IndexOf($n) -ge 0) {
                $keep = $true
            } elseif ([char]::IsDigit($p) -and $n -eq '.') {
                $keep = $true
            }
            if ($keep) { [void]$sb.Append(' '); $col++ }
            continue
        }

        [void]$sb.Append($c)
        $col++
        $i++
        if ($col -ge $WrapAt -and ($c -eq ';' -or $c -eq '{' -or $c -eq '}')) {
            [void]$sb.Append("`n")
            $col = 0
        }
    }
    return $sb.ToString()
}

# ---------------------------------------------------------------------------
# STEP 3: Minify → HelmCombined.min.cs
# ---------------------------------------------------------------------------
if (-not $NoMinify) {
    $step++
    Write-Host "  [$step/$stepTotal] Minify" -ForegroundColor Yellow

    $minified = Compress-CSharpWhitespace (Get-Content $combinedPath -Raw)
    $minPath      = Join-Path $ScriptDir "HelmCombined.min.cs"
    [System.IO.File]::WriteAllText($minPath, $minified, [System.Text.Encoding]::UTF8)
    $minCharCount = $minified.Length
    $headroom     = $BudgetHardLimit - $minCharCount

    # Wrap the body the way the game does, for the compile check (tools/spritedump/CombinedCheck).
    $wrapped = "using System;using System.Collections;using System.Collections.Generic;using System.Linq;using System.Text;" +
               "using Sandbox.ModAPI.Ingame;using VRage.Game.GUI.TextPanel;using VRageMath;" +
               "namespace IngameScript{public partial class Program:MyGridProgram{`n" + $minified + "`n}}"
    $wrapPath = Join-Path (Join-Path (Join-Path (Join-Path $ScriptDir "tools") "spritedump") "CombinedCheck") "Wrapped.g.cs"
    [System.IO.File]::WriteAllText($wrapPath, $wrapped, (New-Object System.Text.UTF8Encoding($false)))

    Write-Host "      HelmCombined.min.cs  $minCharCount chars ($([Math]::Round($minCharCount / 1000, 1)) k)" -ForegroundColor Green

    if ($minCharCount -ge $BudgetHardLimit) {
        Write-Host ""
        Write-Host "  !!! HARD BUDGET BREACH !!!" -ForegroundColor Red
        Write-Host "      $minCharCount chars >= $BudgetHardLimit chars (Space Engineers PB ceiling)" -ForegroundColor Red
        Write-Host "      The minified build WILL FAIL inside the Programmable Block." -ForegroundColor Red
        exit 1
    } elseif ($minCharCount -ge $BudgetSoftLimit) {
        Write-Host "      Budget : $([Math]::Round(($minCharCount / $BudgetHardLimit) * 100, 1))% used  ($headroom chars remaining)  ⚠ approaching ceiling" -ForegroundColor Yellow
    } else {
        Write-Host "      Budget : $([Math]::Round(($minCharCount / $BudgetHardLimit) * 100, 1))% used  ($headroom chars remaining)" -ForegroundColor Cyan
    }
    Write-Host ""
}

# ---------------------------------------------------------------------------
# STEP 4: Summary
# ---------------------------------------------------------------------------
$step++
Write-Host "  [$step/$stepTotal] Summary" -ForegroundColor Yellow
Write-Host "      Unminified  : $charCount chars   → HelmCombined.cs"    -ForegroundColor White
if (-not $NoMinify) {
    Write-Host "      Minified    : $minCharCount chars   → HelmCombined.min.cs" -ForegroundColor White
}
Write-Host ""

# Compile the shipped script against the game's own assemblies (tools/pb-check). Needs a Space Engineers
# install, so it runs only where SE_BIN64 points at the game's Bin64 folder; CI cannot carry the game.
if (-not $NoMinify) {
    if ($env:SE_BIN64) {
        Write-Host "  [pb-check] Compiling against the game's assemblies" -ForegroundColor Yellow
        & ([System.IO.Path]::Combine($ScriptDir, 'tools', 'pb-check', 'Test-PbApi.ps1'))
        if ($LASTEXITCODE -ne 0) { throw "pb-check failed: the game would reject HelmCombined.min.cs." }
    } else {
        Write-Host "  [pb-check] Skipped: set SE_BIN64 to the game's Bin64 folder to compile against the real API." -ForegroundColor DarkYellow
    }
    Write-Host ""
}

Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host "   Build complete." -ForegroundColor Cyan
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host ""
