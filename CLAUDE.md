# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

**PostXING 4.0** — a greenfield **Windows-only** .NET 10 MAUI rewrite of the spirit of the original 2007 WinForms blog editor. Targets a Markdown + GitHub + static-site-generator workflow. Published by **Blue Fenix Productions LLC**; reverse-DNS app identity is `net.bluefenix.postxing`.

This repository previously held a recovered decompilation of PostXING v2 from 2007. That work was shelved on 2026-05-28 and the entire decompiled tree was removed; the history remains in git up to commit `ca2ab1c`. Do not attempt to revive the decompilation.

## Scope (hard)

- **Windows-only.** Single TFM `net10.0-windows10.0.19041.0`. No macCatalyst, no iOS, no Android. Don't add cross-platform conditionals "for someday."
- **Local files are first-class. `gh` CLI is optional/advanced.** The default user flow is: pick a Local Posts Folder in Settings, edit, save to disk, commit/push with whatever git client they prefer (CLI, GitHub Desktop, VSCodium, etc.). The in-app `gh` terminal (route `terminal`) and the gateway-backed Publish flow are present for power users, but the app must remain useful without `gh` installed.
- **GitHub integration: `gh` CLI shell-out, not Octokit.** See `src/PostXING.GitHub/GhCliGitHubGateway.cs`. The `gh` binary is the runtime dependency for the GitHub path; the user runs `gh auth login` once (or pastes a PAT via the in-app terminal) and the app inherits the credential. No PAT storage in-app, no `Octokit` package reference.
- **YAGNI.** Don't add abstractions, alternative backends, future-platform hooks, or "we might want X later" scaffolding. If the user wants a new capability, they'll ask.

## Run / build / test from the repo root

All four of these work no-args from the root:

```powershell
dotnet run                 # launches the App
dotnet build               # builds the App csproj + its referenced libs
bun dev                    # alias for `dotnet run`           (npm run dev also works)
bun test                   # runs all tests via the slnx       (npm run test also works)
```

`bun build` / `npm run build` runs `dotnet build solution/PostXING4.slnx -c Release`, which builds the whole graph (App + libs + tests) and is what you want for CI parity.

### Why this layout

The .NET CLI errors with MSB1011 if there are both a `.csproj` and a `.slnx` in the CWD, except `dotnet run` which has special csproj-finding logic. To make `dotnet run` AND `dotnet build` both work no-args at the root, the slnx had to move to `solution/PostXING4.slnx`. The `package.json` scripts wrap the slnx-required commands (`dotnet test`, full-graph builds) so `bun test` and `bun dev` work transparently.

Don't move the slnx back to root — it breaks `dotnet build` and `dotnet run`. Don't remove the `<Compile Remove="src\**" />` block in `PostXING.App.csproj` — without it the root csproj sweeps up sibling project sources.

## CI / explicit slnx invocations

For CI scripts or when you want the full-graph build/test explicitly:

```powershell
dotnet restore solution/PostXING4.slnx
dotnet build   solution/PostXING4.slnx -c Release
dotnet test    solution/PostXING4.slnx -c Release
```

Requires `maui-windows` workload: `dotnet workload install maui-windows`.

## Project layout

```
PostXING.App.csproj     net10.0-windows10.0.19041.0 — the runnable MAUI app
MauiProgram.cs, App.xaml, AppShell.xaml, ViewModels/, Views/, Platforms/, Properties/, Services/, Resources/Fonts/
src/
  PostXING.Core/       net10.0 — domain (Slug, FrontMatter, Post, PostHandle, PostSource, SiteConfig, PublishState)
  PostXING.GitHub/     net10.0 — IGitHubGateway + GhCliGitHubGateway + InMemoryGitHubGateway + GitHubPublishService
  PostXING.Markdown/   net10.0 — YamlFrontMatterParser + MarkdigRenderer
tests/
  PostXING.Core.Tests/
  PostXING.GitHub.Tests/      (uses InMemoryGitHubGateway; gh CLI not invoked)
  PostXING.Markdown.Tests/
```

The runnable App project sits at the repo root so plain `dotnet run` finds it. Libraries stay under `src/`, tests under `tests/`.

`PostHandle.Source` (`New` / `LocalFile` / `GitHub`) is the dispatch point: anything that "saves a post" or "knows where this post lives" must branch on it. App-layer code that papers over the source distinction usually means a regression to GitHub-only thinking.

`PostXING.App.Tests` is intentionally not present. If/when ViewModel unit tests are wanted, extract ViewModels to a `PostXING.ViewModels` net10.0 project first so the tests don't need a MAUI TFM.

Central package versions in `Directory.Packages.props`. Shared build properties in `Directory.Build.props`. SDK pinned in `global.json` to 10.0.300.

## Flat folder convention

Posts and drafts live in two flat top-level folders inside the user's local folder or the GitHub repo. These names are not configurable; don't introduce `DraftsFolder` / `PostsFolder` settings or year/month subdirectories.

- `drafts/{slug}.md` — work-in-progress. For local: written by `SaveAsync` when the editor's `PostHandle.Source` is `New`. For GitHub: written by `GitHubPublishService.SaveDraftAsync` directly to the integration branch (no PR).
- `posts/{yyyy-MM-dd}-{slug}.md` — published. The date is the publish date computed at publish time (not the post's `CreatedAt`), the slug is from `FrontMatter.Title` at publish time.

`OpenPostViewModel` lists both folders and tags entries `local` / `github draft` / `github post`.

## UI conventions

- **Editor body** uses Hack-Regular at 14pt with a 2px margin. Hack TTFs live in `Resources/Fonts/` (Regular + Bold, MIT/OFL from `source-foundry/Hack`) and are registered in `MauiProgram.ConfigureFonts`. Don't switch to a system font without asking.
- **No persistent chrome.** The only navigation surface is a thin status bar at the bottom of `EditorPage`. New / Open / Settings / Save / Publish are tap-labels in that bar; Save and Publish are hidden until `IsDirty`.
- **New posts go through a full-window title-prompt overlay** driven by `EditorViewModel.ShowTitlePrompt`. The editor starts empty; the prompt seeds the YAML frontmatter (title, author, date, draft, tags, description) and an H1 heading from the title. Don't auto-seed frontmatter without going through the prompt — the blank-canvas cure depends on it.
- **The gh terminal page** (`Views/GhTerminalPage.xaml`, route `terminal`) is intentionally dark + monospace + green-on-black. It accepts a `gh` subcommand and an optional stdin block (used for `auth login --with-token`). It's the canonical in-app PAT entry point; don't add a Settings PAT field.
- **Inter-page post handoff** uses `IPendingPostBox` (singleton). `OpenPostPage` puts the selected `OpenedPost`; `EditorPage.OnAppearing` takes it. Don't refactor to Shell query strings or `MessagingCenter` — the box is simple and untyped-route-friendly.

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
- **Don't migrate `[ObservableProperty]` fields to partial properties.** `CommunityToolkit.Mvvm` is pinned at 8.4, which emits `MVVMTK0045` advising the partial-property syntax for WinRT AOT compat — but 8.4's source generator does *not* synthesize the partial property bodies, so the migration breaks the build. The MVVMTK0045 suppression in `.editorconfig` is deliberate. Revisit only after bumping the package to ≥ 8.5.
- **TDD discipline for new feature work** — red → green → refactor. Tests live in `tests/`. Frameworks: xUnit 2.9.x, Shouldly (not FluentAssertions — Xceed Community License), NSubstitute, coverlet.

## Known stubs (intentional, don't "fix")

- `EditorViewModel.SaveCommand` for `PostSource.GitHub` is a stub with a hint to use the gh terminal. The local-file path is real; the GitHub commit path is deliberately deferred until the UX is settled.
- `EditorViewModel.PublishCommand` raises `PublishConfirmationRequested` but no page subscribes. No PR is opened from the UI yet. `GitHubPublishService.PublishAsync` is fully implemented and tested at the service layer — what's missing is the publish modal, not the backend.
- `GitHubPublishService.GetFileShaAsync` returns `null` unconditionally with a `CA1822` suppression. Adding a real sha lookup is gated on the GitHub-save UI being wired.

## License notes on excluded packages

Considered and rejected:
- `FluentAssertions` >= 8 — Xceed Community License → use `Shouldly`
- `MediatR` >= 12.4 — paid for commercial use → plain DI handlers
- `AutoMapper` >= 14 — paid for commercial use → hand-write mappings
- `Moq` — SponsorLink concern → `NSubstitute`
- `Octokit` — rejected on scope grounds, not licensing. The chosen GitHub backend is the `gh` CLI.

## Pickup

This file is the canonical pickup document. The repo is on `px-modernized`. After a fresh clone (and `dotnet workload install maui-windows`), all four of these should succeed:

```powershell
dotnet run                      # launches the App
dotnet build                    # App + libs
bun test    # or npm run test   # full slnx test pass
bun build   # or npm run build  # full slnx build
```

If any of those fail, that is the first thing to fix — the layout (App csproj at root, slnx under `solution/`, `package.json` scripts) is load-bearing and documented in **Run / build / test from the repo root** above.
