# Steam Community Guide source

This directory is the reviewable source for the English **HELM Core — Complete Guide**. It complements `WORKSHOP.md`: the Workshop page provides installation, quick-start, commands, troubleshooting and maintained external links, while the Guide provides a longer ordered tutorial when a separate Steam Community Guide is useful.

Steam publication is intentionally manual. In the Space Engineers Community Hub, open **Guides**, choose **Create a Guide**, select English and the appropriate modding/scripting categories, then paste the files listed in `manifest.json` as ordered sections. Preview every section and code block before publishing. Record the published URL in this file and in the docs once available.

The section-size check uses an 8,000-character safety limit because Steam's authoring UI has historically enforced per-section limits. Run `bun run content:check` before copying content.

Published guide: not yet published

Images and branding should use existing assets from `brand/` where accepted by Steam. Never commit downloaded Steam credentials or session data.
