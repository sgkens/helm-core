---
title: "Localization and Translation"
description: "Language fallback, translation structure and the community review workflow."
order: 12
section: "Project"
---

English is HELM Core's source language and current published locale. The documentation, HELM Toolbox, Steam Guide and Workshop copy are structured for community translations, but a locale is not advertised until a fluent contributor has reviewed it.

## Behavior

- Existing English documentation URLs remain stable.
- A future translated page uses `/docs/<locale>/...`; when a page is missing, link to the English page rather than serving incomplete or machine-generated prose.
- HELM Toolbox reads `?lang=<locale>`, then the saved browser preference, then English. Missing message keys fall back to English.
- DSL directives, field and template names, commands, code, filenames and URLs are never translated.
- Steam declares one language per Guide, so each translation is published separately and links to the English source.

## Contributing a language

Read [`locales/README.md`](https://gitlab.com/sgkens/helm-core/-/blob/main/locales/README.md), open an issue naming the locale and reviewer, and copy the full English source. Preserve JSON keys and working code examples. Run:

```shell
bun run content:check
bun run build
```

Review the result for terminology, layout overflow, links, code integrity and untranslated prose. Machine translation can help a contributor draft privately, but it is not accepted as an unreviewed published locale.
