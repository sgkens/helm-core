# #4 — Documentation header badges

Source: [GitLab #4](https://gitlab.com/sgkens/helm-core/-/issues/4).
Updated: 2026-10-06.
Status: implemented locally; publication and user review pending.

## Report

The badges beneath breadcrumbs repeat page information and make the documentation header busy. Keep breadcrumbs and remove the header badges.

## Findings and change

[`docs/_includes/doc.njk`](../docs/_includes/doc.njk) rendered a section badge and optional page badge immediately beneath the breadcrumb. Removed that badge container. The title, description and breadcrumbs remain. Front-matter metadata remains available to other consumers.

## Verification — 2026-10-05

- Eleventy generated the documentation successfully.
- A Chromium check of `/docs/helm-toolbox/` found zero badges in the article header and retained `Docs → Tooling → HELM Toolbox` breadcrumbs and the page title.
- Documentation discovery validation passed for 14 pages.

The full content check failed on existing unbalanced BBCode in `WORKSHOP.md`; this is a separate validation failure. The change is part of the local [0.40.1 notes](../release-notes/0.40.1.md), not proof that the live site has been updated.

## Next verification

Check a published docs page on desktop and a narrow viewport. Confirm the header has no duplicate badges and breadcrumbs still identify the page.

## Follow-up log

- 2026-10-06: recorded implementation and test scope. No publication or user acceptance recorded.

- 2026-10-06: full content validation now passes after correcting the Workshop simulator link. Documentation site generation and discovery pass.
