<#
.SYNOPSIS
    Publishes a HELM release locally — builds, packages, uploads to GitLab Generic
    Packages, and (optionally) deploys to the Steam Workshop via SteamCMD.

.DESCRIPTION
    Full local release workflow:
      1. Test & Build   — delegates to Build-Release.ps1 (unit tests + combine + minify)
      2. Package        — creates a versioned .zip archive from build outputs
      3. GitLab Upload  — uploads the archive to GitLab Generic Packages API
      4. Steam Workshop — (optional, -Steam) invokes SteamCMD to update the Workshop item

    All credentials can be supplied as parameters or read from environment variables.
    The script explicitly documents which ENV variable maps to which parameter so CI
    pipelines and local shells both work without code changes.

    Credential precedence: explicit parameter > environment variable > prompt (if
    -Prompt is set) > error.

.PARAMETER Version
    Release version string (e.g. "v1.0.0", "v1.1.0-beta"). Used for the archive
    filename, GitLab package version, and Workshop changenote.
    ENV: HELM_VERSION

.PARAMETER SkipBuild
    Skip the Build-Release.ps1 phase. Use if you have already built and just want
    to package + publish.

.PARAMETER NoBudgetCheck
    Skip the character budget gate. Not recommended for Workshop releases.

.PARAMETER OutDir
    Directory to write the release archive into. Defaults to the script directory.

.PARAMETER GitLabProjectId
    GitLab numeric project ID (find in Settings → General).
    ENV: GITLAB_PROJECT_ID  /  CI_PROJECT_ID

.PARAMETER GitLabApiUrl
    GitLab API base URL. Defaults to https://gitlab.com/api/v4.
    ENV: CI_API_V4_URL  /  GITLAB_API_URL

.PARAMETER GitLabToken
    Personal Access Token or CI Job Token with api / write_package_registry scope.
    ENV: GITLAB_TOKEN  /  CI_JOB_TOKEN

.PARAMETER GitLabPackageName
    Name of the Generic Package to publish under (default: "helm-core").
    ENV: HELM_PACKAGE_NAME

.PARAMETER NoGitLab
    Skip the GitLab Generic Package upload step entirely.

.PARAMETER Steam
    Enable the Steam Workshop deploy step via SteamCMD.

.PARAMETER SteamUser
    Steam account username (Workshop item owner).
    ENV: STEAM_USERNAME

.PARAMETER SteamPass
    Steam account password.
    ENV: STEAM_PASSWORD

.PARAMETER SteamGuard
    Steam Guard 2FA code (if required). Rotate before each deploy.
    ENV: STEAM_GUARD_CODE

.PARAMETER SteamItemId
    Steam Workshop item ID to update. Leave empty ("0") for a first-time publish.
    ENV: STEAM_WORKSHOP_ITEM_ID

.PARAMETER SteamCmdPath
    Path to steamcmd executable or shell script.
    Defaults: Linux/macOS → "steamcmd" (on PATH), Windows → "steamcmd.exe"
    ENV: STEAMCMD_PATH

.PARAMETER Prompt
    If a required credential is missing and -Prompt is set, Read-Host is used
    instead of throwing an error. Useful for interactive local use.

.EXAMPLE
    # Full release — reads creds from ENV, publishes to GitLab, skips Steam
    pwsh ./Publish-Release.ps1 -Version "v1.0.0"

.EXAMPLE
    # Full release including Steam Workshop deploy
    pwsh ./Publish-Release.ps1 -Version "v1.0.0" -Steam

.EXAMPLE
    # Provide credentials explicitly
    pwsh ./Publish-Release.ps1 -Version "v1.0.0" `
        -GitLabProjectId "12345678" `
        -GitLabToken "glpat-xxxxxxxxxxxx" `
        -Steam `
        -SteamUser "mybot" -SteamPass "hunter2" -SteamGuard "AB123" `
        -SteamItemId "3456789012"

.EXAMPLE
    # Skip build (already built), skip GitLab, deploy Steam only
    pwsh ./Publish-Release.ps1 -Version "v1.0.0" -SkipBuild -NoGitLab -Steam -Prompt
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    # Release identity
    [Parameter(Mandatory = $false)]
    [string]$Version = $env:HELM_VERSION,

    # Build control
    [switch]$SkipBuild,
    [switch]$NoBudgetCheck,
    [string]$OutDir = $PSScriptRoot,

    # GitLab Generic Package upload
    [string]$GitLabProjectId = $(if ($env:GITLAB_PROJECT_ID)   { $env:GITLAB_PROJECT_ID }
                                  elseif ($env:CI_PROJECT_ID)  { $env:CI_PROJECT_ID }),
    [string]$GitLabApiUrl    = $(if ($env:CI_API_V4_URL)       { $env:CI_API_V4_URL }
                                  elseif ($env:GITLAB_API_URL) { $env:GITLAB_API_URL }
                                  else                         { 'https://gitlab.com/api/v4' }),
    [string]$GitLabToken     = $(if ($env:CI_JOB_TOKEN)        { $env:CI_JOB_TOKEN }
                                  elseif ($env:GITLAB_TOKEN)   { $env:GITLAB_TOKEN }),
    [string]$GitLabPackageName = $(if ($env:HELM_PACKAGE_NAME) { $env:HELM_PACKAGE_NAME }
                                    else                       { 'helm-core' }),
    [switch]$NoGitLab,

    # Steam Workshop deploy (opt-in)
    [switch]$Steam,
    [string]$SteamUser   = $env:STEAM_USERNAME,
    [string]$SteamPass   = $env:STEAM_PASSWORD,
    [string]$SteamGuard  = $env:STEAM_GUARD_CODE,
    [string]$SteamItemId = $(if ($env:STEAM_WORKSHOP_ITEM_ID) { $env:STEAM_WORKSHOP_ITEM_ID } else { '0' }),
    [string]$SteamCmdPath = $(if ($env:STEAMCMD_PATH) { $env:STEAMCMD_PATH }
                               elseif ($IsWindows)     { 'steamcmd.exe' }
                               else                    { 'steamcmd' }),

    # Interactive credential prompting (local use)
    [switch]$Prompt
)

$ErrorActionPreference = 'Stop'
$ScriptDir = $PSScriptRoot

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------
function Resolve-Credential {
    param(
        [string]$Value,
        [string]$EnvName,
        [string]$Label,
        [switch]$Mandatory,
        [switch]$Prompt
    )
    if ($Value) { return $Value }
    $fromEnv = [System.Environment]::GetEnvironmentVariable($EnvName)
    if ($fromEnv) { return $fromEnv }
    if ($Prompt) { return Read-Host "  Enter $Label" }
    if ($Mandatory) { throw "Missing required credential: $Label (parameter or ENV $EnvName)" }
    return $null
}

function Write-Step {
    param([string]$Label, [int]$Num, [int]$Total)
    Write-Host ""
    Write-Host "  [$Num/$Total] $Label" -ForegroundColor Yellow
}

function Write-Ok   { param([string]$Msg) Write-Host "      $Msg" -ForegroundColor Green  }
function Write-Info { param([string]$Msg) Write-Host "      $Msg" -ForegroundColor Cyan   }
function Write-Warn { param([string]$Msg) Write-Host "      $Msg" -ForegroundColor Yellow }

# ---------------------------------------------------------------------------
# Resolve Version
# ---------------------------------------------------------------------------
if (-not $Version) {
    # Fall back to the repository VERSION file when neither -Version nor HELM_VERSION is set.
    $versionFile = Join-Path $ScriptDir "VERSION"
    if (Test-Path $versionFile) { $Version = ([System.IO.File]::ReadAllText($versionFile)).Trim() }
}
if (-not $Version) {
    if ($Prompt) { $Version = Read-Host "  Enter version (e.g. v1.0.0)" }
    else         { throw "Version is required. Pass -Version, set ENV HELM_VERSION, or put it in the VERSION file." }
}
# Normalize: strip leading 'v' for GitLab package version (re-add for archive name)
$versionClean  = $Version.TrimStart('v')
$versionTag    = if ($Version.StartsWith('v')) { $Version } else { "v$Version" }
$archiveName   = "helm-core-${versionTag}.zip"
$archivePath   = Join-Path $OutDir $archiveName

# Determine step count based on active stages
$totalSteps = 2  # package + summary always present
if (-not $SkipBuild)  { $totalSteps++ }
if (-not $NoGitLab)   { $totalSteps++ }
if ($Steam)           { $totalSteps++ }
$step = 0

# ---------------------------------------------------------------------------
# Header
# ---------------------------------------------------------------------------
Write-Host ""
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host "   HELM — Publish-Release Pipeline" -ForegroundColor Cyan
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host "      Version  : $versionTag" -ForegroundColor White
Write-Host "      Archive  : $archiveName" -ForegroundColor White
Write-Host "      GitLab   : $(if ($NoGitLab) { 'skipped' } else { "Project $GitLabProjectId" })" -ForegroundColor White
Write-Host "      Steam    : $(if ($Steam) { "Item $SteamItemId" } else { 'skipped' })" -ForegroundColor White
Write-Host ""

# ---------------------------------------------------------------------------
# STEP: Test & Build
# ---------------------------------------------------------------------------
if (-not $SkipBuild) {
    $step++
    Write-Step "Test & Build" $step $totalSteps

    $buildScript = Join-Path $ScriptDir "Build-Release.ps1"
    if (-not (Test-Path $buildScript)) {
        throw "Build-Release.ps1 not found at $buildScript"
    }

    $buildArgs = @()
    if ($NoBudgetCheck) { $buildArgs += '-NoMinify' }

    Write-Info "Invoking Build-Release.ps1 $($buildArgs -join ' ')"
    & $buildScript @buildArgs
    if ($LASTEXITCODE -and $LASTEXITCODE -ne 0) {
        throw "Build-Release.ps1 exited with code $LASTEXITCODE"
    }
    Write-Ok "Build complete."
} else {
    Write-Host "  [--] Test & Build  (skipped via -SkipBuild)" -ForegroundColor DarkGray
}

# ---------------------------------------------------------------------------
# STEP: Package — create versioned .zip archive
# ---------------------------------------------------------------------------
$step++
Write-Step "Package" $step $totalSteps

# Files to include in the release archive
$includeItems = @(
    (Join-Path $ScriptDir "HelmCombined.cs"),
    (Join-Path $ScriptDir "HelmCombined.min.cs"),
    (Join-Path $ScriptDir "layouts"),
    (Join-Path $ScriptDir "WORKSHOP.md"),
    (Join-Path $ScriptDir "README.md"),
    (Join-Path $ScriptDir "Build-Release.ps1"),
    (Join-Path $ScriptDir "Publish-Release.ps1"),
    (Join-Path $ScriptDir "Test-HelmLayout.ps1"),
    (Join-Path $ScriptDir "helmv.ps1")
)

# Ensure OutDir exists
if (-not (Test-Path $OutDir)) { New-Item -ItemType Directory -Force -Path $OutDir | Out-Null }

# Remove stale archive
if (Test-Path $archivePath) {
    Remove-Item $archivePath -Force
    Write-Warn "Removed existing archive: $archiveName"
}

# Build archive using System.IO.Compression (no external zip required)
Add-Type -AssemblyName System.IO.Compression.FileSystem

$zipStream = [System.IO.File]::Open($archivePath, [System.IO.FileMode]::Create)
$archive   = [System.IO.Compression.ZipArchive]::new($zipStream, [System.IO.Compression.ZipArchiveMode]::Create, $false)

function Add-ToZip {
    param($Archive, [string]$SourcePath, [string]$ZipRelBase)
    $item = Get-Item $SourcePath -ErrorAction SilentlyContinue
    if (-not $item) { Write-Warn "  Skipped (not found): $SourcePath"; return }
    if ($item.PSIsContainer) {
        foreach ($child in Get-ChildItem $item.FullName -Recurse -File) {
            $rel     = $child.FullName.Substring($item.Parent.FullName.Length).TrimStart([System.IO.Path]::DirectorySeparatorChar, '/')
            $entry   = $Archive.CreateEntry($rel, [System.IO.Compression.CompressionLevel]::Optimal)
            $entStream = $entry.Open()
            $fs      = [System.IO.File]::OpenRead($child.FullName)
            $fs.CopyTo($entStream)
            $fs.Dispose(); $entStream.Dispose()
        }
    } else {
        $rel       = $item.Name
        $entry     = $Archive.CreateEntry($rel, [System.IO.Compression.CompressionLevel]::Optimal)
        $entStream = $entry.Open()
        $fs        = [System.IO.File]::OpenRead($item.FullName)
        $fs.CopyTo($entStream)
        $fs.Dispose(); $entStream.Dispose()
    }
}

foreach ($item in $includeItems) { Add-ToZip -Archive $archive -SourcePath $item }
$archive.Dispose()
$zipStream.Dispose()

$archiveSize = (Get-Item $archivePath).Length
Write-Ok "Created: $archiveName  ($([Math]::Round($archiveSize / 1024, 1)) KB)"
Write-Info "Path: $archivePath"

# ---------------------------------------------------------------------------
# STEP: GitLab Generic Package Upload
# ---------------------------------------------------------------------------
if (-not $NoGitLab) {
    $step++
    Write-Step "GitLab Generic Package Upload" $step $totalSteps

    # Resolve credentials
    if (-not $GitLabProjectId) {
        $GitLabProjectId = Resolve-Credential -Value $GitLabProjectId -EnvName 'GITLAB_PROJECT_ID' `
            -Label 'GitLab Project ID' -Mandatory -Prompt:$Prompt
    }
    if (-not $GitLabToken) {
        $GitLabToken = Resolve-Credential -Value $GitLabToken -EnvName 'GITLAB_TOKEN' `
            -Label 'GitLab Token (glpat-...)' -Mandatory -Prompt:$Prompt
    }

    $apiBase = $GitLabApiUrl.TrimEnd('/')
    $isCiToken = [bool]($env:CI_JOB_TOKEN -and $GitLabToken -eq $env:CI_JOB_TOKEN)
    $authHeader = if ($isCiToken) { 'JOB-TOKEN' } else { 'PRIVATE-TOKEN' }

    $uploadUrl = "$apiBase/projects/$GitLabProjectId/packages/generic/$GitLabPackageName/$versionClean/$archiveName"

    Write-Info "Package : $GitLabPackageName @ $versionClean"
    Write-Info "URL     : $uploadUrl"
    Write-Info "Auth    : $authHeader"

    $headers = @{
        $authHeader = $GitLabToken
        'Content-Type' = 'application/octet-stream'
    }

    if ($PSCmdlet.ShouldProcess($uploadUrl, "PUT archive")) {
        try {
            $response = Invoke-RestMethod -Uri $uploadUrl -Method Put -Headers $headers -InFile $archivePath
            Write-Ok "Uploaded: $archiveName → GitLab Generic Package"
            if ($response) { Write-Info "Response: $($response | ConvertTo-Json -Compress)" }
        } catch {
            Write-Host ""
            Write-Host "  !!! GitLab upload failed !!!" -ForegroundColor Red
            Write-Host "      $_" -ForegroundColor Red
            throw
        }
    }
}

# ---------------------------------------------------------------------------
# STEP: Steam Workshop Deploy (optional, -Steam)
# ---------------------------------------------------------------------------
if ($Steam) {
    $step++
    Write-Step "Steam Workshop Deploy" $step $totalSteps

    # Resolve Steam credentials
    $SteamUser  = Resolve-Credential -Value $SteamUser  -EnvName 'STEAM_USERNAME'         -Label 'Steam username'      -Mandatory -Prompt:$Prompt
    $SteamPass  = Resolve-Credential -Value $SteamPass  -EnvName 'STEAM_PASSWORD'         -Label 'Steam password'      -Mandatory -Prompt:$Prompt
    $SteamGuard = Resolve-Credential -Value $SteamGuard -EnvName 'STEAM_GUARD_CODE'       -Label 'Steam Guard code'    -Prompt:$Prompt
    $SteamItemId = if ($SteamItemId) { $SteamItemId } else { '0' }

    if (-not (Get-Command $SteamCmdPath -ErrorAction SilentlyContinue)) {
        throw "SteamCMD not found at '$SteamCmdPath'. Install SteamCMD or set ENV STEAMCMD_PATH."
    }

    # Stage workshop content folder
    $workshopContent = Join-Path $OutDir "workshop_content"
    if (Test-Path $workshopContent) { Remove-Item $workshopContent -Recurse -Force }
    New-Item -ItemType Directory -Force -Path $workshopContent | Out-Null

    $minCs = Join-Path $ScriptDir "HelmCombined.min.cs"
    if (-not (Test-Path $minCs)) { throw "HelmCombined.min.cs not found — run without -SkipBuild first." }

    Copy-Item $minCs (Join-Path $workshopContent "HelmCombined.cs")
    Copy-Item (Join-Path $ScriptDir "layouts") $workshopContent -Recurse
    Copy-Item (Join-Path $ScriptDir "WORKSHOP.md") $workshopContent

    # Steam Workshop preview: PNG, JPG or GIF. A missing file must stop the publish, not skip it.
    $previewSrc = Join-Path (Join-Path $ScriptDir "brand") "preview.png"
    if (-not (Test-Path $previewSrc)) { throw "Workshop preview not found: $previewSrc (add a PNG, ideally 512x512 or larger)." }
    Copy-Item $previewSrc (Join-Path $workshopContent "preview.png")

    # Generate VDF manifest
    $vdfPath = Join-Path ([System.IO.Path]::GetTempPath()) "helm_workshop.vdf"
    $workshopContentAbs = (Resolve-Path $workshopContent).Path
    $previewAbs = Join-Path $workshopContentAbs "preview.png"

    Set-Content -Path $vdfPath -Encoding UTF8 -Value @"
"workshopitem"
{
    "appid"            "244850"
    "publishedfileid"  "$SteamItemId"
    "contentfolder"    "$($workshopContentAbs.Replace('\','/'))"
    "previewfile"      "$($previewAbs.Replace('\','/'))"
    "visibility"       "0"
    "title"            "HELM: High-Efficiency Lightweight Monitor"
    "changenote"       "Release $versionTag"
}
"@

    Write-Info "Workshop item  : $SteamItemId"
    Write-Info "Content folder : $workshopContent"
    Write-Info "VDF manifest   : $vdfPath"

    if ($PSCmdlet.ShouldProcess("Steam Workshop Item $SteamItemId", "SteamCMD workshop_build_item")) {
        $steamArgs = @(
            "+login", $SteamUser, $SteamPass
        )
        if ($SteamGuard) { $steamArgs += $SteamGuard }
        $steamArgs += @("+workshop_build_item", $vdfPath, "+quit")

        & $SteamCmdPath @steamArgs
        if ($LASTEXITCODE -ne 0) {
            throw "SteamCMD exited with code $LASTEXITCODE — check Steam credentials and try again."
        }
        Write-Ok "Workshop deploy complete."
        if ($SteamItemId -ne '0') {
            Write-Info "URL: https://steamcommunity.com/sharedfiles/filedetails/?id=$SteamItemId"
        }
    }
}

# ---------------------------------------------------------------------------
# Summary
# ---------------------------------------------------------------------------
$step++
Write-Step "Summary" $step $totalSteps
Write-Info "Version  : $versionTag"
Write-Info "Archive  : $archiveName  ($([Math]::Round($archiveSize / 1024, 1)) KB)"
if (-not $NoGitLab) {
    Write-Info "GitLab   : $GitLabPackageName @ $versionClean  (Project $GitLabProjectId)"
}
if ($Steam) {
    Write-Info "Steam    : Workshop Item $SteamItemId"
}
Write-Host ""
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host "   Release $versionTag published." -ForegroundColor Cyan
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host ""
