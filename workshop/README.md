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

`Script.cs` is copied from the minified release build. `description.bbcode.txt` contains the complete Workshop page extracted from `WORKSHOP.md`; to use that full page, paste it through Owner Controls and publish with `-PreserveWorkshopDescription`. `release-notes.txt` contains the change note sent with the update, generated from commit subjects since the previous stable SemVer tag unless `-WorkshopReleaseNotesPath` is supplied. SteamCMD updates `helm-core.vdf` with the assigned `publishedfileid` when it creates the item. The entire `build/` directory is ignored by Git but must be retained locally so later runs update the same item.

For the first publication, publish the local script through Space Engineers' Programmable Block **Edit → Browse Scripts** screen to assign its in-game script type. Then pass that item's ID with `-SteamItemId` or set `STEAM_WORKSHOP_ITEM_ID` for automated updates. Omitting the ID (or passing `0`) lets SteamCMD create an item, but this upload does not assign its in-game script category. Accept the Workshop legal agreement if Steam requests it.

Steam Guard codes are not accepted by `Publish-Release.ps1` or read from the environment. SteamCMD prompts for the current code after the build and staging phases, immediately before it uploads the item.

Every upload updates a compact BBCode description derived from the introduction and Quick Start sections of `WORKSHOP.md`. Steam renders BBCode directly; no description-format field is needed. This replaces any existing full page. Pass `-PreserveWorkshopDescription` to omit the VDF description field on existing-item updates and retain a page maintained through Steam Owner Controls. New items always receive the compact description. Valve's KeyValues parser limits a single value to less than 4096 UTF-8 bytes, so the complete exported `description.bbcode.txt` must be pasted through the Steam web editor.

Only the first fenced BBCode block under **Steam Workshop Description** is exported. The remaining Markdown is a repository appendix (DSL reference, layout catalogue and block matrix), linked from the full page rather than embedded. The full page stays within the 8,000-character publishing guard. Changes to the introduction and Quick Start upload automatically with the compact page. To maintain the full page instead, publish with `-PreserveWorkshopDescription` to regenerate the export, then copy that export into Steam's Edit Description page.

## Category and discoverability

The item must use Steam Workshop `Type: IngameScript`. Do not use `Mod category: Script`; that is a different kind of Space Engineers content. Create the item once from the in-game Programmable Block **Edit → Browse Scripts** screen, then pass that item's ID to `Publish-Release.ps1` for subsequent automated updates. Confirm the type on the Workshop item page.

Normal Workshop tags are assigned through Steam's `SetItemTags` API. `kvtags` are separate custom metadata: naming a key `Type` or `Tags` does not set the visible category or browsing tags. This publisher does not assign normal tags, and Valve's documented SteamCMD VDF example does not supply a normal-tag field. Do not rely on an unverified `tags` or `type` field to repair an untyped item. A publisher using `SetItemTags` can update an existing item's tags without creating a new item. Owner Controls are used here for editing the description; a category-editing option has not been verified.

Keep LCD, telemetry, dashboard, cockpit and monitoring terms in the description for text search. See [Valve's tag API](https://partner.steamgames.com/doc/api/ISteamUGC#SetItemTags), [Valve's SteamCMD guide](https://partner.steamgames.com/doc/features/workshop/implementation#SteamCmdIntegration), and [Keen's script publisher source](https://github.com/KeenSoftwareHouse/SpaceEngineers/blob/master/Sources/Sandbox.Game/Engine/Networking/MySteamWorkshop.cs). Use `CATEGORIES.md` to check the published item.
