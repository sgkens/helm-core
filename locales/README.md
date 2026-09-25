# Localization policy

English (`en`) is HELM Core's source locale and fallback. Locale identifiers follow BCP 47-style casing, for example `de`, `fr`, `pt-BR` and `zh-CN`. Add a locale to `locales.json` only when its maintained catalog or publication source exists.

## What is translated

- Documentation prose and navigation.
- HELM Toolbox labels, help text, status messages and accessibility text.
- Steam Workshop and Community Guide publication copy.

Do not translate DSL directives, telemetry fields, template identifiers, command arguments, code, filenames or URLs. Keep code examples byte-for-byte functional.

## Contribution workflow

1. Open an issue naming the locale and a reviewer who understands the language.
2. Copy the complete English source. Do not silently machine-translate published text.
3. Translate prose without changing JSON keys, code tokens or links.
4. Run `bun run content:check`; missing English keys fail validation.
5. Have a fluent community reviewer approve clarity and Space Engineers terminology.

Documentation keeps stable English URLs. A future translated page uses `/docs/<locale>/...`; missing pages fall back to English. Toolbox selects `?lang=<locale>`, then the saved preference, then English. Steam supports one language per guide, so each published translation is maintained as a separate guide and links back to the English source.
