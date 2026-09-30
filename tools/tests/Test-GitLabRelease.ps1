$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$publisher = Join-Path $repository 'tools/Publish-GitLabRelease.ps1'
$notesPath = Join-Path ([System.IO.Path]::GetTempPath()) ("helm-release-notes-" + [guid]::NewGuid() + '.md')
$previousJobToken = $env:CI_JOB_TOKEN
$previousWorkshopId = $env:STEAM_WORKSHOP_ITEM_ID

function Assert-Release {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

# Intercept every HTTP request, including requests made from the package helper.
function Invoke-RestMethod {
    param($Uri, $Method, $Headers, $ContentType, $Body)
    Assert-Release ($Headers['JOB-TOKEN'] -eq 'test-job-token') 'Missing CI job token header.'
    if ($Method -eq 'Post') {
        $global:helmReleaseTestPosted = $Body | ConvertFrom-Json
        return @{ tag_name = $global:helmReleaseTestPosted.tag_name; _links = @{ self = 'https://example.invalid/release' } }
    }
    if ($Uri -match '/packages/42/package_files\?per_page=100$') {
        return ,$global:helmReleaseTestFiles
    }
    if ($Uri -eq "https://example.invalid/api/v4/projects/123/packages?package_name=$global:helmReleaseTestPackage&package_version=0.39.0-release&package_type=generic") {
        return ,@([pscustomobject]@{ id = 42 })
    }
    throw "Unexpected HTTP request: $Method $Uri"
}

function Set-TestPackage {
    param([string]$Name = 'helm-core', [switch]$WithoutZip)
    $global:helmReleaseTestPackage = $Name
    $global:helmReleaseTestPosted = $null
    $global:helmReleaseTestFiles = @(@{ package_id = 42; id = 8; file_name = 'checksums.txt' })
    if (-not $WithoutZip) {
        $global:helmReleaseTestFiles += @{ package_id = 42; id = 7; file_name = "$Name-v0.39.0-release.zip" }
    }
}

function Assert-Rejected {
    param([hashtable]$Parameters, [string]$ExpectedError)
    $rejected = $false
    try { & $publisher @Parameters }
    catch {
        Assert-Release ($_.Exception.Message -like "*$ExpectedError*") "Unexpected failure: $_"
        $rejected = $true
    }
    Assert-Release $rejected "Expected rejection: $ExpectedError"
    Assert-Release ($null -eq $global:helmReleaseTestPosted) 'A rejected release must not be posted.'
}

try {
    $env:CI_JOB_TOKEN = 'test-job-token'
    $env:STEAM_WORKSHOP_ITEM_ID = '3810372634'
    $parameters = @{
        ProjectId = '123'
        Tag = 'v0.39.0-release'
        ProjectUrl = 'https://example.invalid/helm'
        ApiUrl = 'https://example.invalid/api/v4'
    }
    Set-TestPackage
    & $publisher @parameters
    $posted = $global:helmReleaseTestPosted
    Assert-Release ($posted.tag_name -eq 'v0.39.0-release' -and $posted.name -eq 'HELM Core v0.39.0') 'Wrong release metadata.'
    $expectedNotes = [System.IO.File]::ReadAllText((Join-Path $repository 'release-notes/0.39.0.md')).Trim()
    Assert-Release ($posted.description.StartsWith($expectedNotes)) 'Versioned notes missing from release body.'
    Assert-Release ($posted.description.Contains('https://steamcommunity.com/sharedfiles/filedetails/?id=3810372634')) 'Workshop link missing.'
    Assert-Release ($posted.description.Contains('https://example.invalid/api/v4/projects/123/packages/generic/helm-core/0.39.0-release/helm-core-v0.39.0-release.zip')) 'ZIP download missing from body.'
    Assert-Release ($posted.assets.links.Count -eq 2) 'Package files were not attached.'
    $zip = $posted.assets.links | Where-Object name -eq 'helm-core-v0.39.0-release.zip'
    Assert-Release ($zip.url -eq 'https://example.invalid/helm/-/package_files/7/download') 'Wrong ZIP asset target.'
    Assert-Release ($zip.direct_asset_path -eq '/packages/helm-core-v0.39.0-release.zip') 'Permanent ZIP download path missing.'

    Set-TestPackage -Name 'custom-package'
    & $publisher @parameters -PackageName custom-package -WorkshopItemId 123456
    Assert-Release ($global:helmReleaseTestPosted.description.Contains('/custom-package/0.39.0-release/custom-package-v0.39.0-release.zip')) 'Custom package ZIP missing.'
    Assert-Release ($global:helmReleaseTestPosted.description.Contains('?id=123456')) 'Workshop override ignored.'

    Set-TestPackage
    & $publisher @parameters -WhatIf
    Assert-Release ($null -eq $global:helmReleaseTestPosted) 'WhatIf posted a release.'

    $parameters.ReleaseNotesPath = $notesPath
    Assert-Rejected $parameters 'Release notes not found'
    [System.IO.File]::WriteAllText($notesPath, " `n")
    Assert-Rejected $parameters 'Release notes are empty'
    $parameters.Remove('ReleaseNotesPath')
    Set-TestPackage -WithoutZip
    Assert-Rejected $parameters 'Release archive helm-core-v0.39.0-release.zip was not found'
    Write-Host 'GitLab release checks passed: notes, ZIP assets, Workshop link, overrides, WhatIf, and missing-input rejection.'
}
finally {
    $env:CI_JOB_TOKEN = $previousJobToken
    $env:STEAM_WORKSHOP_ITEM_ID = $previousWorkshopId
    Remove-Item $notesPath -ErrorAction SilentlyContinue
    Remove-Variable helmReleaseTestPosted, helmReleaseTestPackage, helmReleaseTestFiles -Scope Global -ErrorAction SilentlyContinue
}
