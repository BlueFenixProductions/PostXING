# PostXING 4.0 [Memento Mori](https://chris.pelatari.com/meditations)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4.svg)](https://dotnet.microsoft.com/)
[![Platform: Windows](https://img.shields.io/badge/platform-Windows-0078D6.svg)](#requirements)
[![Platform: Android](https://img.shields.io/badge/platform-Android-3DDC84.svg)](#requirements)

A **Windows / Android .NET 10 MAUI** Markdown blog editor built around a **Markdown + GitHub + static-site-generator** workflow. PostXING 4.0 is a greenfield rewrite that carries the spirit of the original 2007 WinForms blog editor into a modern stack.

Published by **Blue Fenix Productions LLC** · app identity `net.bluefenix.postxing`.

---

## What it does

- **Write posts in Markdown** in a focused, distraction-free editor (Hack monospace, blank-canvas by default).
- **Local files are first-class.** Pick a Local Posts Folder in Settings, edit, and save straight to disk. Commit and push with whatever git client you like — CLI, GitHub Desktop, VSCodium, etc.
- **YAML front matter** is seeded for you through a title prompt (title, author, date, draft, tags, description) and parsed back on open.
- **Live preview** rendered with [Markdig](https://github.com/xoofx/markdig) in a WebView2 surface.
- **Optional GitHub integration** via the `gh` CLI (not Octokit) for power users — drafts and the publish flow run through the GitHub gateway once you've authenticated.

### Flat folder convention

Posts and drafts live in two flat top-level folders, in your local folder or your GitHub repo:

```
drafts/{slug}.md                  # work-in-progress
posts/{yyyy-MM-dd}-{slug}.md      # published (date computed at publish time)
```

These names aren't configurable — no year/month subdirectories.

---

## Requirements

- **Windows 10 19041+ / Windows 11** (single TFM `net10.0-windows10.0.19041.0`)
- **.NET SDK 10.0.300** (pinned in `global.json`)
- **MAUI Windows workload**: `dotnet workload install maui-windows`
- **[Bun](https://bun.sh/)** (or npm) for the dev/build scripts
- **`gh` CLI** — *optional*, only needed for the GitHub publish path (`gh auth login` once, or paste a PAT via the in-app terminal)

---

## Getting started

These all work no-args from the repo root:

```powershell
dotnet run        # build + launch the app
dotnet build      # build the App csproj + its referenced libs
bun xunit         # run the full test suite (npm run xunit works too)
bun run build     # full Release build over the whole graph, version-stamped (CI parity)
```

### Day-to-day dev loop

```powershell
bun dev           # fast: self-clean + launch the already-built Debug app (no rebuild)
bun dev:build     # self-clean + version-stamp + incremental build, then launch
bun bump          # regenerate .version only (date-based version + hotfix counter)
```

Use `bun dev:build` after you change C#, XAML, or editor assets. `bun dev` is for an instant relaunch when nothing was rebuilt; it also force-kills any stale `PostXING.App` that would otherwise lock the binaries in `bin\`.

> **Note:** the test script is `xunit`, not `test` — `bun test` is a Bun built-in that would shadow the script. Likewise use `bun run build`, not bare `bun build` (which hits Bun's bundler).

---

## Project layout

The runnable MAUI app sits at the repo root so plain `dotnet run` finds it; libraries live under `src/`, tests under `tests/`.

```
PostXING.App.csproj         net10.0-windows10.0.19041.0 — the runnable MAUI app
MauiProgram.cs, AppShell.xaml, Views/, ViewModels/, Services/, Resources/

src/
  PostXING.Core/            domain (Slug, FrontMatter, Post, PostHandle, PostSource, SiteConfig…)
  PostXING.GitHub/          IGitHubGateway + GhCliGitHubGateway + GitHubPublishService
  PostXING.Markdown/        YAML front-matter parser + Markdig renderer + preview renderer
  PostXING.ViewModels/      MVVM view models (extracted so they're unit-testable off the MAUI TFM)

tests/
  PostXING.Core.Tests/
  PostXING.GitHub.Tests/    uses InMemoryGitHubGateway; gh CLI not invoked
  PostXING.Markdown.Tests/
  PostXING.ViewModels.Tests/

solution/PostXING4.slnx     full solution graph (used by the bun scripts + CI)
```

The solution file lives under `solution/` (not the root) so that `dotnet run` **and** `dotnet build` both work no-args at the root without an MSB1011 collision.

---

## Tech & conventions

- **.NET 10 MAUI**, unpackaged WinUI 3 (`WindowsPackageType=None`).
- **MVVM** with `CommunityToolkit.Mvvm` (pinned at 8.4, field-syntax `[ObservableProperty]`).
- **GitHub via `gh` CLI shell-out**, not Octokit — no PAT storage in-app.
- **TDD discipline** for new features: red → green → refactor. xUnit + Shouldly + NSubstitute + coverlet.
- **Central package versions** in `Directory.Packages.props`; shared build props in `Directory.Build.props`.
- **Version stamping** is date-based (`version.mjs` → gitignored `.version`); the `bun` build scripts pass it to MSBuild. Plain `dotnet build`/`run` use the csproj defaults.

---

## Branch model

| Branch          | Role |
| --------------- | ---- |
| `main`          | production — PRs from `stage` only, linear history, branch-protected |
| `stage`         | pre-production integration — PRs from `develop`, coverage gate |
| `develop`       | active integration branch — feature branches merge here |
| `post/<slug>-<date>` | short-lived per-post branches created by the app |

Branch protection is live on `main` and `stage`: both require a PR plus the `Build + Test` CI check before merge. CI (GitHub Actions, Windows runner) fires on the gate branches; run it on `develop` on demand with `gh workflow run CI --ref develop`.

---

## License

[MIT](LICENSE) © 2026 Blue Fenix Productions LLC
