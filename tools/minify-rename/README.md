# minify-rename

Shortens the names HELM declares in the shipped script, so more features fit under the Programmable Block's 100,000 character limit.

## What is renamed

Locals, parameters, fields, methods, properties, nested types and enum members that are declared in HELM's own code. The renaming is symbol based (Roslyn), so every use changes together with its declaration.

Never renamed: names that belong to the game API, `Program`, `Main` and `Save` (the game calls these), anything that overrides or implements an external member, and the wrapper text around the script.

## How it is checked

| Check | Where | What it proves |
| :--- | :--- | :--- |
| Compile | `tools/minify-rename` | The renamed script compiles with no errors before it is written |
| `tools/spritedump/CombinedCheck`, `tools/pb-check` | CI and local | The renamed script compiles as C# 6 in the game's wrapper, against the stand-in and against the game's own assemblies |
| `tools/minify-check` | CI | The whitespace-minified script, before renaming, has the same tokens as `HelmCombined.cs` |
| `tools/rename-check/Test-RenameParity.ps1` | CI and local | Source and renamed script draw the same sprites and echo the same lines on all presets, including value changes, alerts, `scan` and `boot` |

## Reading a name from the game

Game error messages and stack traces show the short names. `Build-Release.ps1` writes `HelmCombined.map.txt` (kind, original name, short name, number of uses); it is also an artifact of the CI build job. To build without renaming, run `./Build-Release.ps1 -NoRename`.

## Usage

Run from the repository root (`Build-Release.ps1` does this):

```bash
dotnet build tools/spritedump/SeShim -c Release
dotnet run --project tools/minify-rename -c Release -- <in.cs> tools/spritedump/SeShim/bin/Release/net8.0/SeShim.dll <out.cs> --map map.txt
```
