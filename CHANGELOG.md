# Changelog

## 0.39.0

### Added

- Rotating display pages sharing an output, configured with `rotate:` and a common interval.
- Dynamic binding freshness through `ttl:`, `.age`, and `.fresh`, plus HELM messages received over unicast.
- HELM Toolbox undo and redo, with grouped typing, selection restoration, and bounded edit history.
- Documentation discovery through a sitemap, robots file, and `llms.txt`.
- GitLab releases for `vMAJOR.MINOR.PATCH-release` tags, with versioned release notes, ZIP assets, and a Steam Workshop link.
- Workshop preview assets and publishing guidance.

### Changed

- IGC message processing drains a bounded backlog during scheduled runs and ignores non-finite numeric payloads.
- Rotating pages redraw when selected and respect their refresh intervals after switching.
- Workshop publishing stages script content and change notes while preserving an existing item's full page description.
- Release packaging retrieves build artifacts and waits for the compilation, budget, scope, renderer, and minification checks.
- Release ZIPs include the changelog and release notes; custom package names are supported in job artifacts.
- Updated the DSL, gauges, inter-script API, getting-started documentation, and Steam guide.

### Validation

- Added coverage for rotating pages, dynamic freshness, edit history, documentation discovery, and GitLab release publishing.

Earlier release history is available in the [GitLab tags](https://gitlab.com/sgkens/helm-core/-/tags).
