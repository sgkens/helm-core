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

## Generated artifacts

- `HelmCombined.cs` and `HelmCombined.min.cs` are assembled release outputs.
- `public/` is the combined site output.
- Toolbox data, vendor assets and WASM output are rebuilt by `helm-toolbox/package.json`.
- Documentation example frames are produced through `tools/spritedump/`.

Do not hide a failed gate by editing generated output. Fix its source or generator, then rebuild. Releases use the root `VERSION`; keep user-facing version references derived from it wherever possible.
