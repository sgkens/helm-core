# Steam Community Guide source

This directory is the reviewable source for the English **HELM Core — Complete Guide**. It complements `WORKSHOP.md`: the Workshop page is the concise install/release description, while the Guide teaches setup, layout design and troubleshooting.

Steam publication is intentionally manual. In the Space Engineers Community Hub, open **Guides**, choose **Create a Guide**, select English and the appropriate modding/scripting categories, then paste the files listed in `manifest.json` as ordered sections. Preview every section and code block before publishing. Record the published URL in this file and in the docs once available.

The section-size check uses an 8,000-character safety limit because Steam's authoring UI has historically enforced per-section limits. Run `bun run content:check` before copying content.

Published guide: not yet published

Images and branding should use existing assets from `brand/` where accepted by Steam. Never commit downloaded Steam credentials or session data.
