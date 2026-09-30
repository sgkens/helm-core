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

$assetLinks = @($packageFiles | ForEach-Object {
    [ordered]@{
        name = $_.file_name
        url = $_.download_url
        link_type = 'package'
    }
})

$release = [ordered]@{
    name = "HELM Core v$releaseVersion"
    tag_name = $Tag
    description = "HELM Core v$releaseVersion release."
    assets = [ordered]@{
        links = $assetLinks
    }
}

$headers = @{
    $(if ($isCiToken) { 'JOB-TOKEN' } else { 'PRIVATE-TOKEN' }) = $ApiToken
}
$releaseUrl = "$ApiUrl/projects/$ProjectId/releases"

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
