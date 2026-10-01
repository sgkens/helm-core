# Publish complete GitLab releases with notes and downloadable ZIPs

Release-tag pipelines previously used a one-line release description. They now load `release-notes/<version>.md`, include the package ZIP download and Steam Workshop links in the body, and attach package files with permanent release download paths. Publishing fails before creating a release if the notes are missing or empty, or the expected ZIP is absent.

Packaging now retrieves the generated scripts from `build-release`, waits for all release checks, and includes the changelog and release notes in the ZIP. Job artifact paths support the configured package name. The release job avoids downloading unrelated artifacts.

Adds the v0.39.0 changelog and release notes covering rotating pages, dynamic freshness and unicast messaging, Toolbox undo and redo, documentation discovery, and Workshop publishing updates. The Workshop link defaults to item `3810372634` and can be overridden with `STEAM_WORKSHOP_ITEM_ID`.

Fixes Workshop description extraction to accept the current plain headings without requiring Unicode symbols. Quick Start can also end at the fenced block boundary. This resolves the staging failure after the package upload.

## Validation

- Mocked publisher checks: Markdown body, ZIP and Workshop links, release asset paths, custom package names, and rejection of missing notes or archives.
- Workshop extraction checks against the current page, plain headings, LF/CRLF files, a final Quick Start section, missing sections, and SteamCMD's VDF byte limit.
- CI YAML parsing and dependency checks.
- Executed the CI packaging script locally and checked the ZIP contains both generated scripts, layouts, changelog, and release notes; registry upload was skipped.
- Repository content validation and Toolbox tests.

The checks use mocked API responses; live publication happens when the release tag pipeline runs. Add matching notes before each new release tag.
