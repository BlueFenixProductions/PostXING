# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

**PostXING 4.0** — a greenfield .NET 10 MAUI rewrite of the spirit of the original 2007 WinForms blog editor. Targets a Markdown + GitHub + static-site-generator workflow rather than the original XML-RPC MetaWeblog flow. Published by **Blue Fenix Productions LLC**; reverse-DNS app identity is `net.bluefenix.postxing`.

This repository previously held a recovered decompilation of PostXING v2 from 2007. That work was shelved on 2026-05-28 and the entire `src/`, `bin-staged/`, `refasm/`, and net20 build infrastructure were removed; the history of that effort remains in git up to commit `ca2ab1c`. Do not attempt to revive the decompilation — the goal here is the new product.

## Branch model

- `main` — production. Operator-push only. PRs from `stage` only. Linear history required.
- `stage` — pre-production integration. PRs from `px-modernized`. Coverage gate.
- `px-modernized` — active integration branch (analogous to "develop" in the standard three-tier model). Feature branches merge here.
- `post/<slug>-<date>` — short-lived branches created by the app itself for individual blog posts (see `GitHubPublishService`).

The MAUI app never pushes directly to `stage` or `main`.

## Build

```powershell
dotnet restore PostXING4.slnx
dotnet build PostXING4.slnx -c Release
dotnet test PostXING4.slnx -c Release --filter "Category!=Integration"
```

The MAUI App (`src/PostXING.App`) requires the `maui-windows` and `maccatalyst` workloads. Check with `dotnet workload list`; install with `dotnet workload install maui` (meta-workload) or the individual `maui-windows` / `maccatalyst` workloads.

## Project layout

```
src/
  PostXING.Core/       net10.0 — domain model, no framework deps
  PostXING.GitHub/     net10.0 — IGitHubGateway, Octokit adapter, publish workflow
  PostXING.Markdown/   net10.0 — YAML front matter + Markdig rendering
  PostXING.App/        net10.0-windows + net10.0-maccatalyst — MAUI presentation
tests/
  PostXING.Core.Tests/
  PostXING.GitHub.Tests/
  PostXING.Markdown.Tests/
  (PostXING.App.Tests — TODO. ViewModels currently live in PostXING.App which
   targets MAUI TFMs; a plain net10.0 test project cannot reference it. Two
   ways to unblock: (a) extract ViewModels into a new PostXING.ViewModels
   net10.0 project, or (b) target PostXING.App.Tests to net10.0-windows10.0.x.
   Prefer (a) — keeps ViewModels framework-free.)
```

Central package versions in `Directory.Packages.props`. Shared build properties in `Directory.Build.props`. SDK pinned in `global.json` to .NET 10.

## Identity / packaging

- `ApplicationId` = `net.bluefenix.postxing` (Windows + macCatalyst share the unversioned reverse-DNS identity)
- `<Company>` = `Blue Fenix Productions LLC`
- MSIX `Publisher` = `CN=Blue Fenix Productions LLC` (placeholder — must be replaced with the full subject DN of the actual code-signing cert before shipping; CA-issued CN+O+L+S+C must match byte-for-byte)
- Apple Developer Team = Blue Fenix Productions LLC (organization account, D-U-N-S required)

## Hard rules

- **Never propose uninstalling any Visual Studio install, Build Tools install, or VS-installer-managed component.** Carry-over from the prior project, still applies.
- **`.ps1` files written via the Write tool must be ASCII-only.** Windows PowerShell 5.1 misdecodes BOM-less UTF-8; if a script needs a non-ASCII character, use a `[char]` escape.
- **Do not fabricate confident-sounding .NET / Windows internals lore.** The user has 25 years of C# and an ASP MVP from ~2004-2007. "I don't know, want me to check?" is the right move.
- **Don't recommend blanket reformats with `dotnet format` or CSharpier without asking** — partial formatting churn is worse than no formatting. The repo opts into CSharpier on build; one-shot mass formatting passes should still be confirmed.
- **TDD discipline** — for new feature work, the convention is red -> green -> refactor. Tests live in `tests/`. Frameworks: xUnit v3 (or v2 if v3 isn't GA on your toolchain), Shouldly (not FluentAssertions — the latter relicensed to Xceed Community in 2024 and is no longer OSI), NSubstitute, Verify.Xunit, coverlet.

## License notes on excluded packages

These were considered and **deliberately rejected** during scaffolding (re-check before adding):
- `FluentAssertions` >= 8 — Xceed Community License, restricts commercial use -> use `Shouldly`
- `MediatR` >= 12.4 — paid for commercial use -> use plain DI handlers
- `AutoMapper` >= 14 — paid for commercial use -> hand-write mappings
- `Moq` — SponsorLink telemetry concern -> use `NSubstitute`

## Pickup

This file is the canonical pickup document. The session that pivoted the repo (2026-05-28) deleted the entire decompiled v2 tree and bootstrapped the .NET 10 MAUI scaffold on `px-modernized`. The next session should be able to run `dotnet restore PostXING4.slnx` cleanly; if not, that is the first thing to fix.
