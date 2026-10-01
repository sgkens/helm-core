[CmdletBinding(SupportsShouldProcess)]
param (
    [Parameter(Mandatory = $true)]
    [string]$ProjectId,

    [Parameter(Mandatory = $true)]
    [ValidatePattern('^v(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)-release$')]
    [string]$Tag,

    [string]$PackageName = $(if ($env:HELM_PACKAGE_NAME) { $env:HELM_PACKAGE_NAME } else { 'helm-core' }),
    [string]$ProjectName = $env:CI_PROJECT_NAME,
    [string]$Namespace = $env:CI_PROJECT_NAMESPACE,
    [string]$ProjectUrl = $env:CI_PROJECT_URL,
    [string]$ReleaseNotesPath,
    [ValidatePattern('^[1-9][0-9]*$')]
    [string]$WorkshopItemId = $(if ($env:STEAM_WORKSHOP_ITEM_ID) { $env:STEAM_WORKSHOP_ITEM_ID } else { '3810372634' }),
    [string]$ApiUrl = $(if ($env:CI_API_V4_URL) { $env:CI_API_V4_URL } else { 'https://gitlab.com/api/v4' }),
    [string]$ApiToken = $(if ($env:CI_JOB_TOKEN) { $env:CI_JOB_TOKEN } else { $env:GITLAB_API_KEY })
)

$ErrorActionPreference = 'Stop'
$ApiUrl = $ApiUrl.TrimEnd('/')

if (-not $ApiToken) {
    throw 'A GitLab API token is required. Set CI_JOB_TOKEN or GITLAB_API_KEY.'
}

. "$PSScriptRoot/Request-GenericPackage.ps1"

# package-release strips only the leading v from a tag for its package version.
$packageVersion = $Tag -replace '^v', ''
$releaseVersion = $packageVersion -replace '-release$', ''
$isCiToken = [bool]$env:CI_JOB_TOKEN -and $ApiToken -eq $env:CI_JOB_TOKEN

if (-not $ReleaseNotesPath) {
    $ReleaseNotesPath = Join-Path $PSScriptRoot "../release-notes/$releaseVersion.md"
}
if (-not (Test-Path $ReleaseNotesPath -PathType Leaf)) {
    throw "Release notes not found: $ReleaseNotesPath. Add notes for $releaseVersion before tagging."
}
$description = [System.IO.File]::ReadAllText((Resolve-Path $ReleaseNotesPath).Path).Trim()
if (-not $description) { throw "Release notes are empty: $ReleaseNotesPath" }

Write-Host "Fetching $PackageName package assets for $packageVersion..."
$packageFiles = @(
    Request-GenericPackage `
        -ProjectId $ProjectId `
        -PackageName $PackageName `
        -PackageVersion $packageVersion `
        -NameSpace $Namespace `
        -projectName $ProjectName `
        -ProjectUrl $ProjectUrl `
        -ApiUrl $ApiUrl `
        -ApiKey $ApiToken `
        -ci:$isCiToken
)

if ($packageFiles.Count -eq 0) {
    throw "No Generic Package assets found for $PackageName version $packageVersion."
}

$archiveName = "$PackageName-$Tag.zip"
if (-not ($packageFiles | Where-Object { $_.file_name -ceq $archiveName })) {
    throw "Release archive $archiveName was not found in Generic Package version $packageVersion."
}

$assetLinks = @($packageFiles | ForEach-Object {
    [ordered]@{
        name = $_.file_name
        url = $_.download_url
        link_type = 'package'
        direct_asset_path = '/packages/' + $_.file_name
    }
})

$encodedProjectId = [Uri]::EscapeDataString($ProjectId)
$archiveUrl = "$ApiUrl/projects/$encodedProjectId/packages/generic/$([Uri]::EscapeDataString($PackageName))/$([Uri]::EscapeDataString($packageVersion))/$([Uri]::EscapeDataString($archiveName))"
$workshopUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=$WorkshopItemId"
$description += "`n`n## Downloads and installation`n`n- [Download $archiveName]($archiveUrl) (also attached under release assets).`n- [Subscribe on Steam Workshop]($workshopUrl).`n"

$release = [ordered]@{
    name = "HELM Core v$releaseVersion"
    tag_name = $Tag
    description = $description
    assets = [ordered]@{
        links = $assetLinks
    }
}

$headers = @{
    $(if ($isCiToken) { 'JOB-TOKEN' } else { 'PRIVATE-TOKEN' }) = $ApiToken
}
$releaseUrl = "$ApiUrl/projects/$encodedProjectId/releases"

if ($PSCmdlet.ShouldProcess($releaseUrl, "Create GitLab Release $Tag")) {
    $response = Invoke-RestMethod `
        -Uri $releaseUrl `
        -Method Post `
        -Headers $headers `
        -ContentType 'application/json' `
        -Body ($release | ConvertTo-Json -Depth 6)

    Write-Host "Created GitLab Release $($response.tag_name) with $($assetLinks.Count) asset link(s)."
    Write-Host $response._links.self
}
