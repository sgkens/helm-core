# Steam Workshop category checklist

The HELM Workshop item must be published as a Space Engineers in-game script and checked for discoverability after its first upload.

## Required type

- Type: `IngameScript`
- Do not select Mod category: `Script`

Space Engineers assigns the reliable item type when the initial item is published from a Programmable Block's **Edit → Browse Scripts** screen. Use SteamCMD only after recording that item's published file ID.

This release VDF does not assign normal tags. `kvtags` metadata cannot assign the visible type or browsing tags. To repair an existing untyped item while retaining its ID, use a publisher that calls Steam's `SetItemTags` API. A category-editing option in Steam Owner Controls has not been verified.

## Description search terms

The Workshop title and description already use these relevant terms for text search:

- LCD dashboard
- cockpit display
- telemetry
- Programmable Block
- in-game script
- battery, power, hydrogen, oxygen and cargo
- gauges, bars, dials and status screens
- layout editor and preview

Do not add unrelated terms. Confirm the item appears when browsing Space Engineers Workshop items with `Type: IngameScript`.
