# Build, test and release

Install web dependencies once with `bun run setup`. `bun run build` builds HELM Toolbox first and then the complete documentation site into `public/`. Use `bun run docs:serve` for the combined local site or the namespaced root scripts for focused work.

Before merging a functional change, run:

```bash
bun run content:check
bun run helm-toolbox:test
bun run build
mcs -out:/tmp/test_helm.exe HelmCore.cs Tests.cs
mono /tmp/test_helm.exe
```

`pwsh ./Build-Release.ps1` is the release-oriented path. CI additionally checks the Programmable Block character budget, combined compilation, runtime scope, rename/minification parity, packages and Pages output.

Pushing a tag named `vMAJOR.MINOR.PATCH-release` creates a GitLab Release after the Generic Package archive is uploaded. Plain SemVer tags such as `v0.0.0` do not create a release; the corresponding explicit release tag is `v0.0.0-release`.

Before tagging, update `CHANGELOG.md` and add `release-notes/MAJOR.MINOR.PATCH.md`. The publisher loads the notes matching the tag and adds the ZIP download and Steam Workshop links to the release body. It fails if the notes are missing or empty, or if the expected `<package-name>-<tag>.zip` is absent from the registry. Every package file is attached as a release asset with a permanent `/packages/<filename>` download path; the ZIP also contains the changelog and release notes. Packaging downloads the build artifacts and waits for all release checks before uploading.

The Workshop link defaults to item `3810372634`, read from the local publishing manifest when this release workflow was configured. Override it with the CI/CD variable `STEAM_WORKSHOP_ITEM_ID` or the publisher's `-WorkshopItemId` parameter. For local publishing, `-ReleaseNotesPath` can select another Markdown file. Run `pwsh -NoProfile -File tools/tests/Test-GitLabRelease.ps1` to verify the publisher against mocked API responses without creating a live release.

## Generated artifacts

- `HelmCombined.cs` and `HelmCombined.min.cs` are assembled release outputs.
- `public/` is the combined site output.
- Toolbox data, vendor assets and WASM output are rebuilt by `helm-toolbox/package.json`.
- Documentation example frames are produced through `tools/spritedump/`.

Do not hide a failed gate by editing generated output. Fix its source or generator, then rebuild. Releases use the root `VERSION`; keep user-facing version references derived from it wherever possible.
