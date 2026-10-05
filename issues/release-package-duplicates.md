# Release publishing — Duplicate generic-package files

Source: [failed create-release job 16937135867](https://gitlab.com/sgkens/helm-core/-/jobs/16937135867).
Updated: 2026-10-06.
Status: fix committed locally as `cc6df6e`; remote CI verification pending.

## Failure and cause

The `v0.40.0-release` job discovered two files in package 71309121: file IDs 362137484 and 363077346. Both represented `helm-core-v0.40.0-release.zip`. Release creation rejected duplicate asset names and `/packages/helm-core-v0.40.0-release.zip` paths.

CI already used the helper: `.gitlab-ci.yml` → `tools/Publish-GitLabRelease.ps1` → `tools/Request-GenericPackage.ps1`. The file is named `Request-GenericPackage.ps1`, not `Request-GeniericPackage.ps1`.

The helper returned every upload, including repeated filenames. Its package query also relied on the API's default ascending creation order when selecting the first package.

## Fix and verification — 2026-10-05

[`Request-GenericPackage.ps1`](../tools/Request-GenericPackage.ps1) now requests packages by descending creation date, reads all file pages, and retains the newest upload per filename by creation date and numeric file ID.

[`Test-GitLabRelease.ps1`](../tools/tests/Test-GitLabRelease.ps1) passed mocked release tests, including duplicate ZIP uploads beyond the first page, timestamp ties, asset links, custom package names, WhatIf and missing-input rejection.

## Publishing state

The fix was committed as `cc6df6e` and the local `v0.40.0-release` tag was recreated at that commit. Remote tag deletion/push failed because `GITLAB_API_KEY` was unset and HTTPS credentials were unavailable. The last observed remote branch/tag remained at `4e6c238`; confirm current remote refs before continuing.

No successful rerun of release creation has been recorded here. Coordinate the pending 0.40.0 tag replacement with the locally prepared 0.40.1 patch rather than assuming either release is already published.

## Next verification

Run release publishing in authenticated CI against the duplicated registry files. Confirm exactly one asset per filename, the newest ZIP file ID, and a successful GitLab Release response.

## Follow-up log

- 2026-10-06: recorded the job evidence, committed fix and unresolved remote verification.
