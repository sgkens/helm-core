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
    Enable the Steam Workshop deploy step via SteamCMD. SteamCMD prompts for a
    Steam Guard code immediately before upload when the account requires one.

.PARAMETER SteamUser
    Steam account username (Workshop item owner).
    ENV: STEAM_USERNAME

.PARAMETER SteamPass
    Steam account password.
    ENV: STEAM_PASSWORD

.PARAMETER SteamItemId
    Steam Workshop item ID to update. Leave empty for a first-time publish. After
    creation, SteamCMD writes the new ID into workshop/build/helm-core.vdf and the
    script reuses it on later publishes.
    ENV: STEAM_WORKSHOP_ITEM_ID

.PARAMETER WorkshopDir
    Workshop source and staging directory. The source preview is expected at
    workshop/preview.jpg; generated content and the persistent VDF are written below
    workshop/build/. Defaults to the repository workshop directory.

.PARAMETER WorkshopPreviewPath
    JPEG preview uploaded for the Workshop item and copied to content/thumb.jpg for
    the Space Engineers script browser. Defaults to workshop/preview.jpg.

.PARAMETER WorkshopReleaseNotesPath
    Optional UTF-8 text file containing the Workshop change note for this release.
    When omitted, commit subjects since the previous stable SemVer tag are used.
    ENV: HELM_WORKSHOP_RELEASE_NOTES

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
        -SteamUser "mybot" -SteamPass "hunter2" `
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
    [string]$SteamItemId = $env:STEAM_WORKSHOP_ITEM_ID,
    [string]$SteamCmdPath = $(if ($env:STEAMCMD_PATH) { $env:STEAMCMD_PATH }
                               elseif ($IsWindows)     { 'steamcmd.exe' }
                               else                    { 'steamcmd' }),
    [string]$WorkshopDir = (Join-Path $PSScriptRoot 'workshop'),
    [string]$WorkshopPreviewPath = (Join-Path (Join-Path $PSScriptRoot 'workshop') 'preview.jpg'),
    [string]$WorkshopReleaseNotesPath = $env:HELM_WORKSHOP_RELEASE_NOTES,

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

function ConvertTo-VdfString {
    param(
        [AllowEmptyString()][string]$Value,
        [string]$Label = 'VDF value'
    )
    if ($null -eq $Value) { return '' }
    if ($Value.Contains('"')) {
        throw "$Label contains a double quote, which SteamCMD cannot safely parse in this VDF format."
    }
    $escaped = $Value.Replace('\', '\\')
    $byteCount = [System.Text.Encoding]::UTF8.GetByteCount($escaped)
    if ($byteCount -ge 4096) {
        throw "$Label is $byteCount UTF-8 bytes; SteamCMD KeyValues values must be smaller than 4096 bytes."
    }
    return $escaped
}

function Get-WorkshopDescriptionBody {
    param([string]$Path)
    $markdown = [System.IO.File]::ReadAllText($Path)
    $match = [regex]::Match($markdown, '(?ms)^## Steam Workshop Description.*?^```text[ \t]*\r?\n(?<body>.*?)\r?\n^```[ \t]*$')
    if (-not $match.Success) {
        throw "Workshop description fenced block not found in $Path"
    }
    return $match.Groups['body'].Value.Trim()
}

function Get-WorkshopDescription {
    param([string]$Path)
    $body = Get-WorkshopDescriptionBody $Path
    $featuresAt = $body.IndexOf('[h2]✦ Key Features[/h2]', [System.StringComparison]::Ordinal)
    $quickStart = [regex]::Match($body, '(?ms)^\[h2\]✦ Quick Start\[/h2\].*?(?=^\[h2\])')
    if ($featuresAt -lt 0 -or -not $quickStart.Success) {
        throw "Workshop introduction or Quick Start section not found in $Path"
    }

    # Valve KeyValues uses a 4096-byte token buffer. Keep the full description in
    # WORKSHOP.md for the Steam web editor, but generate a compact initial/update
    # description that SteamCMD can parse as one VDF value.
    $introduction = $body.Substring(0, $featuresAt).Trim()
    return ($introduction + "`n`n" + $quickStart.Value.Trim()).Replace('"', "'")
}

function Get-WorkshopChangeNote {
    param(
        [string]$RepositoryPath,
        [string]$ReleaseVersion,
        [string]$NotesPath
    )

    if ($NotesPath) {
        if (-not (Test-Path $NotesPath -PathType Leaf)) {
            throw "Workshop release-notes file not found: $NotesPath"
        }
        return [System.IO.File]::ReadAllText((Resolve-Path $NotesPath).Path).Trim()
    }

    $releaseNames = @($ReleaseVersion, $ReleaseVersion.TrimStart('v'), "v$($ReleaseVersion.TrimStart('v'))")
    $tags = @(& git -C $RepositoryPath tag --merged HEAD --sort=-version:refname)
    if ($LASTEXITCODE -ne 0) { throw 'Unable to inspect Git tags for Workshop release notes.' }
    $previousTag = $tags | Where-Object {
        $_ -match '^v?(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)$' -and $_ -notin $releaseNames
    } | Select-Object -First 1

    $range = if ($previousTag) { "$previousTag..HEAD" } else { 'HEAD' }
    $subjects = @(& git -C $RepositoryPath log --no-merges --format='%s' $range)
    if ($LASTEXITCODE -ne 0) { throw 'Unable to read Git history for Workshop release notes.' }
    $subjects = @($subjects | Where-Object { $_ -and $_ -notmatch '^(chore|ci|build)(\([^)]*\))?!?:' })

    $header = "HELM Core $ReleaseVersion"
    if ($previousTag) { $header += " (changes since $previousTag)" }
    if ($subjects.Count -eq 0) { return "$header`n`nRelease $ReleaseVersion." }

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add($header)
    $lines.Add('')
    foreach ($subject in $subjects) {
        $candidate = ($lines + "- $subject") -join "`n"
        if ([System.Text.Encoding]::UTF8.GetByteCount($candidate) -ge 3900) {
            $lines.Add('- Additional changes are listed in the GitLab release history.')
            break
        }
        $lines.Add("- $subject")
    }
    return ($lines -join "`n").Replace('"', "'")
}

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

# SteamCMD updates the VDF with the new publishedfileid after first creation. Keep
# that generated VDF under workshop/build and reuse its ID when no parameter or
# environment variable was supplied.
$workshopBuild = Join-Path $WorkshopDir 'build'
$workshopContent = Join-Path $workshopBuild 'content'
$workshopVdfPath = Join-Path $workshopBuild 'helm-core.vdf'
if ($Steam -and -not $SteamItemId -and (Test-Path $workshopVdfPath)) {
    $savedVdf = [System.IO.File]::ReadAllText($workshopVdfPath)
    $savedId = [regex]::Match($savedVdf, '"publishedfileid"\s+"(?<id>[0-9]+)"')
    if ($savedId.Success -and $savedId.Groups['id'].Value -ne '0') {
        $SteamItemId = $savedId.Groups['id'].Value
    }
}
if ($Steam -and -not $SteamItemId) { $SteamItemId = '0' }
if ($Steam -and $SteamItemId -notmatch '^[0-9]+$') {
    throw "Steam Workshop item ID must contain digits only: $SteamItemId"
}

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
    $isNewWorkshopItem = $SteamItemId -eq '0'

    if (-not (Get-Command $SteamCmdPath -ErrorAction SilentlyContinue)) {
        throw "SteamCMD not found at '$SteamCmdPath'. Install SteamCMD or set ENV STEAMCMD_PATH."
    }

    # Stage the exact folder consumed by the Space Engineers in-game script browser.
    # Keep the build directory and VDF so SteamCMD's first-publish item ID survives.
    if (-not (Test-Path $workshopBuild)) { New-Item -ItemType Directory -Force -Path $workshopBuild | Out-Null }
    if (Test-Path $workshopContent) { Remove-Item $workshopContent -Recurse -Force }
    New-Item -ItemType Directory -Force -Path $workshopContent | Out-Null

    $minCs = Join-Path $ScriptDir "HelmCombined.min.cs"
    if (-not (Test-Path $minCs)) { throw "HelmCombined.min.cs not found — run without -SkipBuild first." }

    if (-not (Test-Path $WorkshopPreviewPath -PathType Leaf)) {
        throw "Workshop preview not found: $WorkshopPreviewPath (add workshop/preview.jpg; JPEG, at least 512x288 and under 1 MB)."
    }
    $preview = Get-Item $WorkshopPreviewPath
    if ($preview.Extension -notin @('.jpg', '.jpeg')) {
        throw "Workshop preview must be a JPEG file: $WorkshopPreviewPath"
    }
    if ($preview.Length -ge 1MB) {
        throw "Workshop preview must be under 1 MB: $WorkshopPreviewPath"
    }

    Copy-Item $minCs (Join-Path $workshopContent 'Script.cs')
    Copy-Item $WorkshopPreviewPath (Join-Path $workshopContent 'thumb.jpg')

    # Generate the persistent VDF manifest. SteamCMD mutates publishedfileid in this
    # file after creating an item, so do not use a temporary path.
    $workshopContentAbs = (Resolve-Path $workshopContent).Path
    $previewAbs = (Resolve-Path $WorkshopPreviewPath).Path
    $workshopMarkdownPath = Join-Path $ScriptDir 'WORKSHOP.md'
    $fullDescription = Get-WorkshopDescriptionBody $workshopMarkdownPath
    if ($fullDescription.Length -gt 8000) {
        throw "Full Workshop description is $($fullDescription.Length) characters; keep the Steam web-editor copy at or below 8000 characters."
    }
    $description = Get-WorkshopDescription $workshopMarkdownPath
    $changeNote = Get-WorkshopChangeNote -RepositoryPath $ScriptDir -ReleaseVersion $versionTag -NotesPath $WorkshopReleaseNotesPath
    $contentVdf = ConvertTo-VdfString $workshopContentAbs 'Workshop content path'
    $previewVdf = ConvertTo-VdfString $previewAbs 'Workshop preview path'
    $descriptionVdf = ConvertTo-VdfString $description 'Workshop description'
    $titleVdf = ConvertTo-VdfString 'HELM: High-Efficiency Lightweight Monitor' 'Workshop title'
    $changeVdf = ConvertTo-VdfString $changeNote 'Workshop change note'

    $fullDescriptionPath = Join-Path $workshopBuild 'description.bbcode.txt'
    $releaseNotesPath = Join-Path $workshopBuild 'release-notes.txt'
    [System.IO.File]::WriteAllText($fullDescriptionPath, $fullDescription, [System.Text.UTF8Encoding]::new($false))
    [System.IO.File]::WriteAllText($releaseNotesPath, $changeNote, [System.Text.UTF8Encoding]::new($false))

    # Omitting description on updates preserves the full page entered in Steam's
    # Owner Controls. Only new items receive the compact bootstrap description.
    $descriptionLine = if ($isNewWorkshopItem) { '    "description"      "' + $descriptionVdf + '"' } else { '' }

    $vdf = @"
"workshopitem"
{
    "appid"            "244850"
    "publishedfileid"  "$SteamItemId"
    "contentfolder"    "$contentVdf"
    "previewfile"      "$previewVdf"
    "visibility"       "0"
    "title"            "$titleVdf"
$descriptionLine
    "changenote"       "$changeVdf"
}
"@
    [System.IO.File]::WriteAllText($workshopVdfPath, $vdf, [System.Text.UTF8Encoding]::new($false))

    Write-Info "Workshop item  : $(if ($isNewWorkshopItem) { 'new item' } else { $SteamItemId })"
    Write-Info "Content folder : $workshopContent"
    Write-Info "Preview        : $WorkshopPreviewPath"
    Write-Info "VDF manifest   : $workshopVdfPath"
    Write-Info "Full page      : $fullDescriptionPath"
    Write-Info "Release notes  : $releaseNotesPath"

    if ($PSCmdlet.ShouldProcess("Steam Workshop Item $(if ($isNewWorkshopItem) { 'new' } else { $SteamItemId })", "SteamCMD workshop_build_item")) {
        $steamArgs = @(
            "+login", $SteamUser, $SteamPass
        )
        $steamArgs += @("+workshop_build_item", $workshopVdfPath, "+quit")

        Write-Info "Starting SteamCMD; enter the current Steam Guard code if prompted."
        & $SteamCmdPath @steamArgs
        if ($LASTEXITCODE -ne 0) {
            throw "SteamCMD exited with code $LASTEXITCODE — review the SteamCMD output above for the specific login, VDF, or upload error."
        }

        # SteamCMD writes the assigned ID back into the VDF for a newly created item.
        $publishedVdf = [System.IO.File]::ReadAllText($workshopVdfPath)
        $publishedId = [regex]::Match($publishedVdf, '"publishedfileid"\s+"(?<id>[0-9]+)"')
        if (-not $publishedId.Success -or $publishedId.Groups['id'].Value -eq '0') {
            throw "SteamCMD completed but did not write a Workshop item ID to $workshopVdfPath"
        }
        $SteamItemId = $publishedId.Groups['id'].Value
        Write-Ok "Workshop deploy complete."
        Write-Info "URL: https://steamcommunity.com/sharedfiles/filedetails/?id=$SteamItemId"
        Write-Info "Paste the full page from $fullDescriptionPath through Workshop Owner Controls."
        Write-Warn "SteamCMD cannot set Space Engineers' normal Workshop type/category tags; confirm Type: IngameScript in the game or Owner Controls."
        if ($isNewWorkshopItem) {
            Write-Warn "For reliable Type: IngameScript assignment, create the item once from Space Engineers' Browse Scripts screen, then use its ID for future SteamCMD updates."
            Write-Warn "First publish: open the URL and accept the Workshop legal agreement if prompted."
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
