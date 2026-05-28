# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

**PostXING 4.0** — a greenfield **Windows-only** .NET 10 MAUI rewrite of the spirit of the original 2007 WinForms blog editor. Targets a Markdown + GitHub + static-site-generator workflow. Published by **Blue Fenix Productions LLC**; reverse-DNS app identity is `net.bluefenix.postxing`.

This repository previously held a recovered decompilation of PostXING v2 from 2007. That work was shelved on 2026-05-28 and the entire decompiled tree was removed; the history remains in git up to commit `ca2ab1c`. Do not attempt to revive the decompilation.

## Scope (hard)

- **Windows-only.** Single TFM `net10.0-windows10.0.19041.0`. No macCatalyst, no iOS, no Android. Don't add cross-platform conditionals "for someday."
- **GitHub integration: `gh` CLI shell-out, not Octokit.** See `src/PostXING.GitHub/GhCliGitHubGateway.cs`. The `gh` binary is the runtime dependency; the user runs `gh auth login` once and the app inherits the credential. No PAT storage in-app, no `Octokit` package reference.
- **YAGNI.** Don't add abstractions, alternative backends, future-platform hooks, or "we might want X later" scaffolding. If the user wants a new capability, they'll ask.

## Run

```powershell
dotnet run
```

That's it — from the repo root. The runnable App csproj lives at the repo root (`PostXING.App.csproj`); the .NET CLI picks it up automatically. Launch profile `PostXING` is wired in `Properties/launchSettings.json` for F5 in VS / Rider / VS Code C# Dev Kit.

The libraries `Compile Remove="src\**"` exclude is in the App csproj precisely so this layout works — the root project doesn't sweep up sibling project trees. Don't remove that exclude block.

## Build / Test

```powershell
dotnet restore PostXING4.slnx
dotnet build PostXING4.slnx -c Release
dotnet test PostXING4.slnx -c Release
```

Requires `maui-windows` workload: `dotnet workload install maui-windows`.

## Project layout

```
PostXING.App.csproj     net10.0-windows10.0.19041.0 — the runnable MAUI app
MauiProgram.cs, App.xaml, AppShell.xaml, ViewModels/, Views/, Platforms/, Properties/
src/
  PostXING.Core/       net10.0 — domain (Slug, FrontMatter, Post, SiteConfig, PublishState)
  PostXING.GitHub/     net10.0 — IGitHubGateway + GhCliGitHubGateway + InMemoryGitHubGateway + GitHubPublishService
  PostXING.Markdown/   net10.0 — YamlFrontMatterParser + MarkdigRenderer
tests/
  PostXING.Core.Tests/
  PostXING.GitHub.Tests/      (uses InMemoryGitHubGateway; gh CLI not invoked)
  PostXING.Markdown.Tests/
```

The runnable App project sits at the repo root so plain `dotnet run` finds it. Libraries stay under `src/`, tests under `tests/`.

`PostXING.App.Tests` is intentionally not present. If/when ViewModel unit tests are wanted, extract ViewModels to a `PostXING.ViewModels` net10.0 project first so the tests don't need a MAUI TFM.

Central package versions in `Directory.Packages.props`. Shared build properties in `Directory.Build.props`. SDK pinned in `global.json` to 10.0.300.

## Branch model

- `main` — production. Operator-push only. PRs from `stage` only. Linear history required.
- `stage` — pre-production integration. PRs from `px-modernized`. Coverage gate.
- `px-modernized` — active integration branch. Feature branches merge here.
- `post/<slug>-<date>` — short-lived branches created by the app for individual blog posts via `GitHubPublishService`.

The app never pushes directly to `stage` or `main`.

## Identity / packaging

- `ApplicationId` = `net.bluefenix.postxing`
- `<Company>` = `Blue Fenix Productions LLC`
- `WindowsPackageType=None` — unpackaged WinUI 3. Switch to MSIX and add `Platforms/Windows/Package.appxmanifest` when ready to sign and ship.
- MSIX `Publisher` (when MSIX is enabled) = `CN=Blue Fenix Productions LLC` — must match the full subject DN of the code-signing cert byte-for-byte once issued.

## Hard rules

- **Windows-only and `gh` CLI only.** Don't propose Octokit, LibGit2Sharp, GitLab adapters, or Mac/Linux ports. If the constraint changes, the user will say so.
- **Don't add speculative abstractions or "future maybe" code paths.** YAGNI is enforced; left-in dead code is worse than no code.
- **Never propose uninstalling any Visual Studio install, Build Tools install, or VS-installer-managed component.**
- **`.ps1` files written via the Write tool must be ASCII-only.** Windows PowerShell 5.1 misdecodes BOM-less UTF-8; if a script needs a non-ASCII character, use a `[char]` escape.
- **Do not fabricate confident-sounding .NET / Windows internals lore.** The user has 25 years of C# and an ASP MVP from ~2004-2007. "I don't know, want me to check?" is the right move.
- **Don't recommend blanket reformats with `dotnet format` or CSharpier without asking.**
- **TDD discipline for new feature work** — red → green → refactor. Tests live in `tests/`. Frameworks: xUnit 2.9.x, Shouldly (not FluentAssertions — Xceed Community License), NSubstitute, coverlet.

## License notes on excluded packages

Considered and rejected:
- `FluentAssertions` >= 8 — Xceed Community License → use `Shouldly`
- `MediatR` >= 12.4 — paid for commercial use → plain DI handlers
- `AutoMapper` >= 14 — paid for commercial use → hand-write mappings
- `Moq` — SponsorLink concern → `NSubstitute`
- `Octokit` — rejected on scope grounds, not licensing. The chosen GitHub backend is the `gh` CLI.

## Pickup

This file is the canonical pickup document. The repo is on `px-modernized`. `dotnet restore PostXING4.slnx` and `dotnet run --project src/PostXING.App` should both succeed cleanly; if not, that is the first thing to fix.
