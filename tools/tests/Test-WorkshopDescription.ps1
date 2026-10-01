$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent

# Load only these pure helpers; do not run the publishing pipeline.
$tokens = $null
$parseErrors = $null
$ast = [System.Management.Automation.Language.Parser]::ParseFile(
    (Join-Path $repository 'Publish-Release.ps1'), [ref]$tokens, [ref]$parseErrors)
if ($parseErrors.Count) { throw "Publisher parse failed: $parseErrors" }
$helperNames = @('Get-WorkshopDescriptionBody', 'Get-WorkshopDescription', 'ConvertTo-VdfString')
$helpers = $ast.FindAll({ param($node)
    $node -is [System.Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -in $helperNames
}, $true)
if ($helpers.Count -ne $helperNames.Count) { throw 'Workshop helpers not found.' }
foreach ($helper in $helpers) { . ([scriptblock]::Create($helper.Extent.Text)) }

function Assert-Workshop {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

$fixturePath = Join-Path ([System.IO.Path]::GetTempPath()) ("helm-workshop-" + [guid]::NewGuid() + '.md')
try {
    $description = Get-WorkshopDescription (Join-Path $repository 'WORKSHOP.md')
    Assert-Workshop ($description.Contains('HELM turns LCDs')) 'Introduction missing.'
    Assert-Workshop ($description.Contains('[h2]Quick Start[/h2]')) 'Quick Start missing.'
    Assert-Workshop (-not $description.Contains('[h2]Key Features[/h2]')) 'Feature list should be omitted from compact description.'
    Assert-Workshop (-not $description.Contains('[h2]Minimal Display Example[/h2]')) 'Quick Start extraction included the next section.'
    $vdf = ConvertTo-VdfString $description 'Workshop description'
    Assert-Workshop ([System.Text.Encoding]::UTF8.GetByteCount($vdf) -lt 4096) 'Description exceeds the SteamCMD token limit.'

    # Plain headings must work with either newline style and a final Quick Start.
        foreach ($newline in @("`n", "`r`n")) {
            $fixture = @('## Steam Workshop Description', '```text', '[h1]HELM[/h1]', 'Introduction',
                "[h2]Key Features[/h2]", 'Features',
                "[h2]Quick Start[/h2]", 'Install HELM', '```') -join $newline
            [System.IO.File]::WriteAllText($fixturePath, $fixture)
            $compact = Get-WorkshopDescription $fixturePath
            Assert-Workshop ($compact.Contains('Introduction') -and $compact.Contains('Install HELM')) 'Plain headings or end-of-block extraction failed.'
            Assert-Workshop (-not $compact.Contains('Features')) 'Compact description includes features.'
        }
    [System.IO.File]::WriteAllText($fixturePath, "## Steam Workshop Description`n" + '```text' + "`nIntroduction`n" + '```')
    $rejected = $false
    try { Get-WorkshopDescription $fixturePath | Out-Null }
    catch {
        if ($_.Exception.Message -notlike '*Workshop introduction or Quick Start section not found*') { throw }
        $rejected = $true
    }
    Assert-Workshop $rejected 'Missing sections were not rejected.'
    Write-Host 'Workshop description checks passed: current page, plain headings, LF/CRLF, end-of-block Quick Start, missing sections, and VDF byte limit.'
}
finally { Remove-Item $fixturePath -ErrorAction SilentlyContinue }
