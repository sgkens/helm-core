using namespace System.Text.RegularExpressions
using namespace System.Collections.Generic

<#
.SYNOPSIS
Calculates the next SemVer version from Git history using Conventional Commits.

.DESCRIPTION
Finds the nearest reachable stable SemVer tag and scans only commits made
after that tag.

The highest Conventional Commit change determines the next version:

    BREAKING CHANGE / !  -> MAJOR
    feat                  -> MINOR
    fix / perf            -> PATCH
    everything else       -> no bump

The selected bump is applied ONCE, regardless of how many commits of that
type exist.

Examples:

    Existing tag: 1.2.3

    fix: a
    fix: b
        -> 1.2.4

    feat: a
    feat: b
    fix: c
        -> 1.3.0

    feat: a
    fix: b
    refactor!: c
        -> 2.0.0

Only exact stable SemVer tags are considered release tags:

    1.2.3
    v1.2.3

Tags such as these are ignored:

    1.2
    1.2.3-beta
    v1.2.3-rc.1
    release-1.2.3
    01.2.3

If no release tag exists, the baseline is 0.0.0.

Therefore:

    first feat:            -> 0.1.0
    first fix:             -> 0.0.1
    first breaking change: -> 1.0.0
    docs/chore only:       -> 0.0.0

In an existing repository, historical versions are NOT recalculated.
The nearest existing SemVer tag is treated as authoritative and only
commits after that tag affect the calculated version.

Shallow repositories are permitted when a valid release tag is reachable.
If no release tag can be found in a shallow checkout, the function stops
rather than silently calculating a potentially incorrect version.

.PARAMETER MinorTypes
Conventional Commit types that trigger a MINOR release.

Defaults to:

    feat

.PARAMETER PatchTypes
Conventional Commit types that trigger a PATCH release.

Defaults to:

    fix
    perf

Note that Conventional Commits itself explicitly maps fix to PATCH and
feat to MINOR. `perf` is included here as a project policy and can be
removed:

    -PatchTypes @('fix')

.EXAMPLE
Get-SemVer

.EXAMPLE
(Get-SemVer).Version

.EXAMPLE
Get-SemVer |
    Select-Object Version, LastTag, CommitsScanned, BumpType

.EXAMPLE
Get-SemVer -PatchTypes @('fix')

.OUTPUTS
PSCustomObject

Properties:

    Version
    LastTag
    CommitsScanned
    BumpType

.NOTES
Expected Conventional Commit examples:

    feat(parser): add new parser

    fix: correct null handling

    feat!: replace public API

    feat(api): replace public API

    BREAKING CHANGE: old API has been removed

Other commit types such as:

    build
    chore
    ci
    docs
    refactor
    style
    test

do not trigger a release unless they contain a breaking-change marker.
#>

function Get-SemVer {

    [CmdletBinding()]
    [OutputType([PSCustomObject])]
    param (
        [Parameter()]
        [ValidateNotNull()]
        [string[]]$MinorTypes = @(
            'feat'
        ),

        [Parameter()]
        [ValidateNotNull()]
        [string[]]$PatchTypes = @(
            'fix',
            'perf'
        )
    )

    process {
        try {
            # ---------------------------------------------------------------------
            # Git availability
            # ---------------------------------------------------------------------

            $gitCommand = Get-Command git -ErrorAction SilentlyContinue

            if (-not $gitCommand) {
                throw 'Git is not installed or is not available on PATH.'
            }


            # ---------------------------------------------------------------------
            # Validate repository
            # ---------------------------------------------------------------------

            & git rev-parse --is-inside-work-tree 2>$null | Out-Null

            if ($LASTEXITCODE -ne 0) {
                throw 'The current directory is not inside a Git repository.'
            }


            # ---------------------------------------------------------------------
            # Normalise bump-type configuration
            # ---------------------------------------------------------------------

            $minorTypeSet = [HashSet[string]]::new(
                [StringComparer]::OrdinalIgnoreCase
            )

            $patchTypeSet = [HashSet[string]]::new(
                [StringComparer]::OrdinalIgnoreCase
            )

            foreach ($item in $MinorTypes) {
                if ([string]::IsNullOrWhiteSpace($item)) {
                    throw 'MinorTypes cannot contain an empty value.'
                }

                [void]$minorTypeSet.Add($item.Trim())
            }

            foreach ($item in $PatchTypes) {
                if ([string]::IsNullOrWhiteSpace($item)) {
                    throw 'PatchTypes cannot contain an empty value.'
                }

                $type = $item.Trim()

                if ($minorTypeSet.Contains($type)) {
                    throw "Commit type '$type' cannot exist in both MinorTypes and PatchTypes."
                }

                [void]$patchTypeSet.Add($type)
            }


            # ---------------------------------------------------------------------
            # Handle a repository with no commits
            # ---------------------------------------------------------------------

            & git rev-parse --verify HEAD 2>$null | Out-Null

            if ($LASTEXITCODE -ne 0) {
                return [PSCustomObject]@{
                    Version        = '0.0.0'
                    LastTag        = $null
                    CommitsScanned = 0
                    BumpType       = 'none'
                }
            }


            # ---------------------------------------------------------------------
            # SemVer tag definition
            #
            # Accepted:
            #
            #   1.2.3
            #   v1.2.3
            #
            # Intentionally excludes prerelease/build metadata.
            # ---------------------------------------------------------------------

            $semVerTagPattern =
                '^(?:v)?' +
                '(?<major>0|[1-9]\d*)\.' +
                '(?<minor>0|[1-9]\d*)\.' +
                '(?<patch>0|[1-9]\d*)$'


            # ---------------------------------------------------------------------
            # Find nearest reachable VALID SemVer tag.
            #
            # git describe is preferable to simply sorting tag names because we
            # want the release tag nearest to HEAD in Git history.
            #
            # Git's --match uses glob patterns rather than regular expressions,
            # so candidates are validated afterward against the strict SemVer
            # regular expression.
            #
            # If a candidate looks vaguely version-like but is invalid, exclude
            # it and ask Git for the next nearest candidate.
            # ---------------------------------------------------------------------

            $describeArgs = @(
                'describe',
                '--tags',
                '--abbrev=0',
                '--match', '[0-9]*.[0-9]*.[0-9]*',
                '--match', 'v[0-9]*.[0-9]*.[0-9]*'
            )

            $lastTag = $null
            $tagMatch = $null

            while ($true) {
                $candidate = & git @describeArgs 2>$null

                if ($LASTEXITCODE -ne 0 -or -not $candidate) {
                    break
                }

                # git describe should return one value, but explicitly collapse
                # native output to a scalar for predictable behaviour.
                $candidate = ([string]$candidate).Trim()

                $candidateMatch = [Regex]::Match(
                    $candidate,
                    $semVerTagPattern,
                    [RegexOptions]::CultureInvariant
                )

                if ($candidateMatch.Success) {
                    $lastTag = $candidate
                    $tagMatch = $candidateMatch

                    break
                }

                # Candidate matched Git's glob but is not a valid stable SemVer
                # tag. Ignore it and continue looking backward.
                $describeArgs += @(
                    '--exclude',
                    $candidate
                )
            }


            # ---------------------------------------------------------------------
            # Determine starting version
            # ---------------------------------------------------------------------

            if ($lastTag) {
                [uint64]$major = $tagMatch.Groups['major'].Value
                [uint64]$minor = $tagMatch.Groups['minor'].Value
                [uint64]$patch = $tagMatch.Groups['patch'].Value

                $range = "$lastTag..HEAD"
            }
            else {
                [uint64]$major = 0
                [uint64]$minor = 0
                [uint64]$patch = 0

                $range = $null


                # -------------------------------------------------------------
                # Protect CI from incomplete shallow history.
                #
                # "No tag" in a shallow checkout does NOT prove that the repo
                # has never had a release.
                # -------------------------------------------------------------

                $isShallow = & git rev-parse --is-shallow-repository 2>$null

                if (
                    $LASTEXITCODE -eq 0 -and
                    ([string]$isShallow).Trim() -eq 'true'
                ) {
                    throw 'No reachable stable SemVer tag was found in this shallow repository. Fetch tags and complete history before calculating a version.'
                }
            }


            # ---------------------------------------------------------------------
            # Retrieve commit hashes.
            #
            # Do NOT parse `git log -z` directly through PowerShell. Native
            # command output is represented as PowerShell strings/records and
            # relying on NUL-delimited native output is unnecessarily fragile.
            #
            # Instead, obtain one commit SHA per line and retrieve each complete
            # commit message separately.
            # ---------------------------------------------------------------------

            $logArgs = @(
                'log',
                '--no-color',
                '--format=%H'
            )

            if ($range) {
                $logArgs += $range
            }

            $commitHashes = @(
                & git @logArgs
            )

            if ($LASTEXITCODE -ne 0) {
                throw "git log failed with exit code $LASTEXITCODE."
            }

            $commitHashes = @(
                $commitHashes |
                    Where-Object {
                        -not [string]::IsNullOrWhiteSpace($_)
                    }
            )


            # ---------------------------------------------------------------------
            # Conventional Commit patterns
            #
            # Subject:
            #
            #   type: description
            #   type(scope): description
            #   type!: description
            #   type(scope)!: description
            #
            # Require ": " rather than allowing malformed "feat:thing".
            # ---------------------------------------------------------------------

            $subjectPattern =
                '^(?<type>[A-Za-z][A-Za-z0-9-]*)' +
                '(?:\((?<scope>[^()\r\n]+)\))?' +
                '(?<breaking>!)?: (?<description>\S.*)$'

            # Conventional Commits recognises both:
            #
            #   BREAKING CHANGE:
            #   BREAKING-CHANGE:
            #
            # Case-insensitive matching is intentionally tolerant here. Missing
            # a real breaking release is more dangerous than accepting a
            # differently-cased marker.
            $breakingPattern =
                '(?m)^BREAKING(?: |-)?CHANGE:\s+\S'


            # ---------------------------------------------------------------------
            # Determine highest required bump.
            #
            # IMPORTANT:
            #
            # We do NOT increment versions inside this loop.
            #
            # Multiple fixes still mean one PATCH release.
            # Multiple features still mean one MINOR release.
            # Any breaking change overrides every lower bump.
            # ---------------------------------------------------------------------

            [int]$bumpRank = 0

            # 0 = none
            # 1 = patch
            # 2 = minor
            # 3 = major

            foreach ($hash in $commitHashes) {
                $messageLines = @(
                    & git show `
                        '--no-patch' `
                        '--no-color' `
                        '--format=%B' `
                        $hash
                )

                if ($LASTEXITCODE -ne 0) {
                    throw "Unable to read commit '$hash'."
                }

                $message = $messageLines -join "`n"

                if ([string]::IsNullOrWhiteSpace($message)) {
                    continue
                }

                $lines = $message -split '\r?\n', 2

                $subject = $lines[0]

                $body = if ($lines.Count -gt 1) {
                    $lines[1]
                }
                else {
                    ''
                }

                $subjectMatch = [Regex]::Match(
                    $subject,
                    $subjectPattern,
                    [RegexOptions]::CultureInvariant
                )

                $bangBreaking =
                    $subjectMatch.Success -and
                    $subjectMatch.Groups['breaking'].Success

                $footerBreaking = [Regex]::IsMatch(
                    $body,
                    $breakingPattern,
                    [RegexOptions]::IgnoreCase -bor
                    [RegexOptions]::CultureInvariant
                )


                # -------------------------------------------------------------
                # MAJOR
                # -------------------------------------------------------------

                if ($bangBreaking -or $footerBreaking) {
                    $bumpRank = 3

                    # Nothing outranks a breaking change.
                    break
                }


                # A malformed/non-Conventional subject does not participate in
                # normal feat/fix classification.
                if (-not $subjectMatch.Success) {
                    continue
                }

                $type = $subjectMatch.Groups['type'].Value


                # -------------------------------------------------------------
                # MINOR
                # -------------------------------------------------------------

                if ($minorTypeSet.Contains($type)) {
                    if ($bumpRank -lt 2) {
                        $bumpRank = 2
                    }

                    continue
                }


                # -------------------------------------------------------------
                # PATCH
                # -------------------------------------------------------------

                if ($patchTypeSet.Contains($type)) {
                    if ($bumpRank -lt 1) {
                        $bumpRank = 1
                    }
                }
            }


            # ---------------------------------------------------------------------
            # Apply the selected bump EXACTLY ONCE.
            # ---------------------------------------------------------------------

            $bumpApplied = switch ($bumpRank) {
                3 {
                    $major++
                    $minor = 0
                    $patch = 0

                    'major'
                }

                2 {
                    $minor++
                    $patch = 0

                    'minor'
                }

                1 {
                    $patch++

                    'patch'
                }

                default {
                    'none'
                }
            }


            # ---------------------------------------------------------------------
            # Output
            # ---------------------------------------------------------------------

            return [PSCustomObject]@{
                Version        = "$major.$minor.$patch"
                LastTag        = $lastTag
                CommitsScanned = $commitHashes.Count
                BumpType       = $bumpApplied
            }
        }
        catch {
            $PSCmdlet.ThrowTerminatingError($_)
        }
    }

}
