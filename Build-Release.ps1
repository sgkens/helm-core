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

# Strip `using HelmCore;` from Program.cs (namespace is inlined below)
$combinedProgram = $programContent -replace 'using HelmCore;\r?\n', ''

# Extract the body inside `namespace HelmCore { ... }`
$startNs = $coreContent.IndexOf("namespace HelmCore")
if ($startNs -ge 0) {
    $openBrace  = $coreContent.IndexOf('{', $startNs)
    $lastBrace  = $coreContent.LastIndexOf('}')
    $coreBody   = $coreContent.Substring($openBrace + 1, $lastBrace - $openBrace - 1).Trim()
} else {
    $coreBody = $coreContent
}

# Strip the closing brace of `namespace IngameScript` — we're splicing inside it
$lastProgBrace = $combinedProgram.LastIndexOf('}')
$progBody      = $combinedProgram.Substring(0, $lastProgBrace).TrimEnd()

$combined = @"
// ==============================================================================
// HELM: High-Efficiency Lightweight Monitor for Space Engineers
// Single-file combined distribution for Programmable Block
// ==============================================================================

$progBody

    // ==========================================================================
    // HELM CORE LIBRARY: DSL Parser, Layout, Color Engine, Aggregator
    // ==========================================================================

$coreBody
}
"@

$combinedPath = Join-Path $ScriptDir "HelmCombined.cs"
[System.IO.File]::WriteAllText($combinedPath, $combined, [System.Text.Encoding]::UTF8)
$charCount = $combined.Length

Write-Host "      HelmCombined.cs  $charCount chars ($([Math]::Round($charCount / 1000, 1)) k)" -ForegroundColor Green
Write-Host ""

# ---------------------------------------------------------------------------
# STEP 3: Minify → HelmCombined.min.cs
# ---------------------------------------------------------------------------
if (-not $NoMinify) {
    $step++
    Write-Host "  [$step/$stepTotal] Minify" -ForegroundColor Yellow

    # Token-aware C# comment stripper (preserves verbatim string literals)
    $source = Get-Content $combinedPath -Raw
    $sb     = New-Object System.Text.StringBuilder($source.Length)
    $len    = $source.Length
    $i      = 0
    $inString       = $false
    $isVerbatim     = $false
    $inLineComment  = $false
    $inBlockComment = $false

    while ($i -lt $len) {
        $c    = $source[$i]
        $next = if ($i + 1 -lt $len) { $source[$i + 1] } else { [char]0 }

        if ($inLineComment) {
            if ($c -eq "`n") {
                $inLineComment = $false
                [void]$sb.Append("`n")
            }
            $i++; continue
        }

        if ($inBlockComment) {
            if ($c -eq '*' -and $next -eq '/') { $inBlockComment = $false; $i += 2; continue }
            $i++; continue
        }

        if ($inString) {
            [void]$sb.Append($c)
            if ($isVerbatim) {
                if ($c -eq '"' -and $next -eq '"') { [void]$sb.Append($next); $i += 2; continue }
                elseif ($c -eq '"') { $inString = $false; $isVerbatim = $false }
            } else {
                if ($c -eq '\') {
                    if ($next -ne [char]0) { [void]$sb.Append($next); $i += 2; continue }
                } elseif ($c -eq '"') { $inString = $false }
            }
            $i++; continue
        }

        if ($c -eq '@' -and $next -eq '"') {
            $inString = $true; $isVerbatim = $true
            [void]$sb.Append('@"'); $i += 2; continue
        }
        if ($c -eq '"') {
            $inString = $true; $isVerbatim = $false
            [void]$sb.Append('"'); $i++; continue
        }
        if ($c -eq '/' -and $next -eq '/') { $inLineComment  = $true; $i += 2; continue }
        if ($c -eq '/' -and $next -eq '*') { $inBlockComment = $true; $i += 2; continue }

        [void]$sb.Append($c)
        $i++
    }

    # Strip blank lines (preserve verbatim string content)
    $lines    = $sb.ToString().Split("`n")
    $outLines = [System.Collections.Generic.List[string]]::new()
    $inVerb   = $false

    foreach ($line in $lines) {
        $trimmed = $line.Trim()
        if ($inVerb) {
            $outLines.Add($line)
            $quotes = ($line -replace '[^"]', '').Length
            if ($quotes % 2 -eq 1) { $inVerb = $false }
        } else {
            if ($trimmed.Length -gt 0) {
                $outLines.Add($trimmed)
                if ($trimmed.Contains('@"')) {
                    $idx   = $trimmed.IndexOf('@"')
                    $rest  = $trimmed.Substring($idx + 2)
                    $qcnt  = ($rest -replace '[^"]', '').Length
                    if ($qcnt % 2 -ne 1) { $inVerb = $true }
                }
            }
        }
    }

    $minified     = [string]::Join("`n", $outLines)
    $minPath      = Join-Path $ScriptDir "HelmCombined.min.cs"
    [System.IO.File]::WriteAllText($minPath, $minified, [System.Text.Encoding]::UTF8)
    $minCharCount = $minified.Length
    $headroom     = $BudgetHardLimit - $minCharCount

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
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host "   Build complete." -ForegroundColor Cyan
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host ""
