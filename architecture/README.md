# HELM Core architecture

This directory explains how HELM Core is assembled and records decisions that should survive individual implementations. Start with [System overview](system-overview.md), then follow the document that matches the area you are changing.

| Document | Scope |
| --- | --- |
| [System overview](system-overview.md) | Repository boundaries and end-to-end data flow. |
| [Runtime and rendering](runtime-rendering.md) | Programmable Block lifecycle, telemetry and retained sprites. |
| [DSL and generated data](dsl-and-data.md) | Parser stages and source-of-truth rules. |
| [Build, test and release](build-test-release.md) | Local commands, CI gates and generated artifacts. |
| [Decision records](decisions/README.md) | How to document durable architectural choices. |

The public user documentation belongs in `docs/`. These files are maintainer-facing and should describe the implementation as it exists, not proposed features.
