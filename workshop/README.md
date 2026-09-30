# Steam Workshop publishing

This directory contains source assets and generated staging files for the HELM Steam Workshop item.

## Source asset

Add `preview.jpg` here before publishing. It must be a JPEG, at least 512 by 288 pixels, and under 1 MB. The release script uses it as Steam's item preview and copies it to `build/content/thumb.jpg` for the Space Engineers script browser.

## Generated layout

Running `Publish-Release.ps1 -Steam` creates:

```text
workshop/
  preview.jpg
  build/
    content/
      Script.cs
      thumb.jpg
    description.bbcode.txt
    release-notes.txt
    helm-core.vdf
```

`Script.cs` is copied from the minified release build. `description.bbcode.txt` contains the complete Workshop page extracted from `WORKSHOP.md`; paste it into the Workshop page through Owner Controls. `release-notes.txt` contains the change note sent with the update, generated from commit subjects since the previous stable SemVer tag unless `-WorkshopReleaseNotesPath` is supplied. SteamCMD updates `helm-core.vdf` with the assigned `publishedfileid` when it creates the item. The entire `build/` directory is ignored by Git but must be retained locally so later runs update the same item.

For the first publish, omit `-SteamItemId` (or pass `0`). After the upload, open the reported Workshop URL, accept the Workshop legal agreement if Steam requests it, and confirm that the item is categorized as an in-game script. For publishing from another machine, pass the existing ID with `-SteamItemId` or set `STEAM_WORKSHOP_ITEM_ID`.

Steam Guard codes are not accepted by `Publish-Release.ps1` or read from the environment. SteamCMD prompts for the current code after the build and staging phases, immediately before it uploads the item.

New items receive a compact description derived from the introduction and Quick Start sections of `WORKSHOP.md`. Existing-item updates omit the VDF description field to preserve the full page entered through Steam Owner Controls. Valve's KeyValues parser limits a single value to less than 4096 UTF-8 bytes, so paste the complete exported `description.bbcode.txt` through the Steam web editor.

Only the first fenced BBCode block under **Steam Workshop Description** is exported. The remaining Markdown is a repository appendix (DSL reference, layout catalogue and block matrix), linked from the full page rather than embedded. The full page stays within the 8,000-character publishing guard. After changing it, publish to regenerate the export, then copy that export into Steam's Edit Description page; an automated item update intentionally does not replace your full description.

## Category and discoverability

The item must use Steam Workshop `Type: IngameScript`. Do not use `Mod category: Script`; that is a different kind of Space Engineers content. SteamCMD's `workshop_build_item` supports arbitrary `kvtags`, but not the normal Workshop tags used by Space Engineers' Type filter. Create the item once from the in-game Programmable Block **Browse Scripts** screen, then pass that item's ID to `Publish-Release.ps1` for subsequent automated updates. Confirm the type after every first publication.

Use the checklist in `CATEGORIES.md` when reviewing the item in Steam Owner Controls. Search terms belong naturally in the title and full description rather than as unsupported VDF metadata.
