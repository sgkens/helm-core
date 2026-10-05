# Issue investigation logs

Updated: 2026-10-06. These are local developer records; they do not change GitLab issue status. A local implementation or passing test is not release or user acceptance.

| Reference | Log | Current state |
| --- | --- | --- |
| GitLab #4 | [Documentation header badges](issue-004-doc-header-badges.md) | Implemented locally; publication and user review pending |
| GitLab #5 | [Toolbox animation and intermittent alerts](issue-005-toolbox-animation.md) | Local fixes pass Firefox/Chromium; user reproduction pending |
| GitLab #6 | [Corner LCD text sizing](issue-006-corner-lcd-text.md) | Implemented locally for 0.40.1; in-game acceptance pending |
| Release job 16937135867 | [Duplicate package assets](release-package-duplicates.md) | Fix committed locally; remote CI verification pending |
| New roadmap requests | [Backlog review, 6 October 2026](backlog-2026-10-06.md) | Guidance and four themed power screens implemented locally |

## Evidence

- [Toolbox animation console output](animation-issue.md): original user capture; retain verbatim.
- [In-game run 1](in-game-run-1-errors.md) and [in-game run 2](in-game-run-2-errors.md): historical compile-error captures.
- [Development backlog](../DEVTRACKER-PENDING.md#backlog): local roadmap, ignored by Git. The [shared backlog review](backlog-2026-10-06.md) preserves the new requests and investigation context for other developers.

## Adding information

Append a dated entry to the relevant log. Record what was observed, the exact environment and reproduction steps, evidence, and what remains unknown. Keep earlier results with their original scope; a later failure should not erase an earlier passing test.

For a browser report, include the URL, browser/version, served commit or build, preset or exact DSL, selected LCD, motion preference, actions taken, expected and actual behavior, and console/network output. For an in-game report, include the script version, block/surface type, Custom Data, commands and screenshots or errors.

Keep raw console output and screenshots as separate evidence files linked from the log. Label hypotheses as hypotheses. Mark a fix accepted only after the reported reproduction succeeds in the affected environment.
