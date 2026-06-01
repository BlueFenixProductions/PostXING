# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

**PostXING 4.0** — a greenfield **Windows-first** .NET 10 MAUI rewrite of the spirit of the original 2007 WinForms blog editor. Targets a Markdown + GitHub + static-site-generator workflow. Published by **Blue Fenix Productions LLC**; reverse-DNS app identity is `net.bluefenix.postxing`. As of **2026-05-30** the App also targets **Android** (multi-target `net10.0-windows10.0.19041.0;net10.0-android`) and runs on a Pixel 7 / CalyxOS — see **Android notes** below.

This repository previously held a recovered decompilation of PostXING v2 from 2007. That work was shelved on 2026-05-28 and the decompiled tree was removed from the rewrite line; the history remains in git up to commit `ca2ab1c`, and the legacy decompiled code now lives on the `px-decompiled` branch. Do not revive the decompilation on `develop` / `main`.

## Scope (hard)

- **Windows-first, plus Android.** The App multi-targets `net10.0-windows10.0.19041.0;net10.0-android` — the original Windows-only rule was lifted on 2026-05-30 when the Android port landed. Still **no** macCatalyst/iOS/Linux. Android divergences go through `OnPlatform` (XAML) / `#if ANDROID` (C#) and must leave Windows behavior unchanged; don't add other-platform conditionals "for someday." See **Android notes**.
- **Local files are first-class. `gh` CLI is optional/advanced.** The default user flow is: pick a Local Posts Folder in Settings, edit, save to disk, commit/push with whatever git client they prefer (CLI, GitHub Desktop, VSCodium, etc.). The in-app `gh` terminal (route `terminal`) and the gateway-backed Publish flow are present for power users, but the app must remain useful without `gh` installed.
- **GitHub integration: `gh` CLI on desktop, in-process HTTP on Android — never Octokit.** Both implement the same `IGitHubGateway`. Desktop: `src/PostXING.GitHub/GhCliGitHubGateway.cs` shells out to `gh`; the user runs `gh auth login` once (or pastes a PAT via the in-app terminal) and the app inherits the credential — no PAT storage on desktop. Android can't shell out, so `HttpGitHubGateway` calls the GitHub REST API directly over `HttpClient`, authenticating with a PAT kept in `SecureStorage` (`IGitHubTokenStore`). No `Octokit` package reference on either head.
- **YAGNI.** Don't add abstractions, alternative backends, future-platform hooks, or "we might want X later" scaffolding. If the user wants a new capability, they'll ask.

## Run / build / test from the repo root

From the repo root:

```powershell
dotnet run -f net10.0-windows10.0.19041.0   # launch the Windows app (-f required: multi-target)
dotnet build               # builds both TFMs (Windows + Android) of the App + its libs
bun dev                    # fast: self-clean + launch the built Windows app, no rebuild (npm run dev too)
bun dev:build              # self-clean + version-stamp + incremental Windows build, then launch (after code/asset edits)
bun android                # build + deploy + launch on a connected Android device (USB debugging on)
bun run build              # full slnx Release build, version-stamped (CI parity; npm run build too)
bun xunit                  # runs all tests via the slnx       (npm run xunit also works)
bun bump                   # regenerate .version only (date-based version + hotfix counter)
```

`dotnet run` no longer works **no-args** now that the App multi-targets — it needs `-f net10.0-windows10.0.19041.0` (or use `bun dev` for the Windows loop, `bun android` for the phone). Plain `dotnet build` compiles **both** heads, so it's slower; `bun dev`/`dev:build` build Windows only.

The test script is named `xunit`, **not** `test`, on purpose: `bun test` is a Bun built-in that runs Bun's own JS/TS test runner and ignores the `package.json` script, so a `test` script would silently never run under `bun`. `bun xunit` (no collision) wraps `dotnet test`. Don't rename it back to `test`.

`bun run build` / `npm run build` runs `scripts/build.ps1`, which stamps the version (regenerates `.version` and passes it to MSBuild — see **Version stamping** below) and then runs `dotnet build solution/PostXING4.slnx -c Release` over the whole graph (App + libs + tests). It's what you want for CI parity, and it's what the GitHub Actions workflow runs (`bun run build` + `bun run xunit`). Use `bun run build`, **not** bare `bun build` — `build` is a Bun builtin (the bundler, errors with "Missing entrypoints"), the same collision behind the `test` → `xunit` rename above. It deliberately routes through a `.ps1` rather than calling `dotnet` inline so the `.version` read behaves identically under `bun` and `npm` (cmd.exe, which `npm` uses on Windows, has no `$(...)` command substitution).

`bun dev` and `bun dev:build` both run `scripts/dev.ps1`, which first force-kills any leftover `PostXING.App`. A stale instance — from a closed terminal or a Ctrl-C that left the GUI process alive — locks the apphost `.exe`/`.dll`s in `bin\`, so the next build or launch can't overwrite them and fails (commonly exit `58`). After the kill, **`bun dev` launches the already-built Debug app directly** for a near-instant start (no rebuild), while **`bun dev:build` stamps the version then runs an incremental `dotnet build`** — use it after you change C#, XAML, or the editor's `index.html` (assets only land in `bin\` on a build). Plain `dotnet run` neither self-cleans nor is fast: it rebuilds the whole MAUI app every launch (~20–50s here), so prefer the `bun` scripts. The `dev.ps1` / `build.ps1` scripts are ASCII-only by the `.ps1` rule; if one ever needs a non-ASCII char, use a `[char]` escape.

### Version stamping

`version.mjs` computes a date-based version and writes it to `.version` at the repo root: a 4-part `major.minor.patch.build` string where `major` is `4` (the 4.x line), `minor` is the current year minus 2022, `patch` is month + zero-padded day (e.g. May 29 -> `529`), and `build` is a monotonic hotfix counter that `++`s off the previous `.version`. `.version` is **gitignored** and regenerated on every build, so the counter never churns committed files — which also means it resets to `1` on a fresh clone or if `.version` is deleted.

`scripts/build.ps1` and `scripts/dev.ps1 -Build` regenerate `.version`, then split it and pass the pieces to `dotnet build`: `-p:Version=` (the full 4-part, for assembly/file metadata), `-p:ApplicationDisplayVersion=` (`major.minor.patch` — this is what `AppInfo.VersionString` returns and what the About page shows), and `-p:ApplicationVersion=` (the build number, `AppInfo.BuildString`). `bun bump` (`node version.mjs`) regenerates `.version` without building.

Plain `dotnet build` / `dotnet run` at the root do **not** stamp — they build with the csproj defaults (`ApplicationDisplayVersion` `4.0.0`, `ApplicationVersion` `1`). Use the `bun` scripts when you want the real version in the binary.

### Why this layout

The .NET CLI errors with MSB1011 if there are both a `.csproj` and a `.slnx` in the CWD, except `dotnet run` which has special csproj-finding logic. To make `dotnet run` AND `dotnet build` both work no-args at the root, the slnx had to move to `solution/PostXING4.slnx`. The `package.json` scripts wrap the slnx-required commands (`dotnet test`, full-graph builds) so `bun xunit` and `bun dev` work transparently.

Don't move the slnx back to root — it breaks `dotnet build` and `dotnet run`. Don't remove the `<Compile Remove="src\**" />` block in `PostXING.App.csproj` — without it the root csproj sweeps up sibling project sources.

## CI / explicit slnx invocations

For CI scripts or when you want the full-graph build/test explicitly:

```powershell
dotnet restore solution/PostXING4.slnx
dotnet build   solution/PostXING4.slnx -c Release
dotnet test    solution/PostXING4.slnx -c Release
```

These explicit invocations build with the csproj default version — they don't run `version.mjs`, so use `bun build` when you want a version-stamped binary.

Requires the `maui-windows` and `maui-android` workloads: `dotnet workload install maui-windows maui-android`.

## Project layout

```
PostXING.App.csproj     net10.0-windows10.0.19041.0;net10.0-android — the runnable MAUI app
MauiProgram.cs, App.xaml, AppShell.xaml, Views/, Platforms/{Windows,Android}/, Properties/, Services/, Resources/
src/
  PostXING.Core/       net10.0 — domain (Slug, FrontMatter, Post, PostHandle, PostSource, SiteConfig, PublishState)
  PostXING.GitHub/     net10.0 — IGitHubGateway + GhCliGitHubGateway + InMemoryGitHubGateway + GitHubPublishService + GitCliStatusService
  PostXING.Markdown/   net10.0 — YamlFrontMatterParser + MarkdigRenderer + PreviewRenderer
  PostXING.ViewModels/ net10.0 — the MVVM view models, extracted so they unit-test off the MAUI TFM
tests/
  PostXING.Core.Tests/
  PostXING.GitHub.Tests/      (uses InMemoryGitHubGateway; gh CLI not invoked)
  PostXING.Markdown.Tests/
  PostXING.ViewModels.Tests/
```

The runnable App project sits at the repo root so plain `dotnet run` finds it. Libraries stay under `src/`, tests under `tests/`.

`PostHandle.Source` (`New` / `LocalFile` / `GitHub`) is the dispatch point: anything that "saves a post" or "knows where this post lives" must branch on it. App-layer code that papers over the source distinction usually means a regression to GitHub-only thinking.

`PostXING.App.Tests` is intentionally not present — no test project should need a MAUI TFM. ViewModels were extracted to the `PostXING.ViewModels` net10.0 project, so `PostXING.ViewModels.Tests` covers them without one (e.g. `EditorViewModelTests`).

Central package versions in `Directory.Packages.props`. Shared build properties in `Directory.Build.props`. SDK pinned in `global.json` to 10.0.300.

## Flat folder convention

Posts and drafts live in two flat top-level folders inside the user's local folder or the GitHub repo. These names are not configurable; don't introduce `DraftsFolder` / `PostsFolder` settings or year/month subdirectories.

- `drafts/{slug}.md` — work-in-progress. For local: written by `SaveAsync` when the editor's `PostHandle.Source` is `New`. For GitHub: written by `GitHubPublishService.SaveDraftAsync` directly to the integration branch (no PR).
- `posts/{yyyy-MM-dd}-{slug}.md` — published. The date is the publish date computed at publish time (not the post's `CreatedAt`), the slug is from `FrontMatter.Title` at publish time.

`OpenPostViewModel` lists both folders and tags entries `local` / `github draft` / `github post`.

## UI conventions

- **Editor body** uses Hack-Regular at 14pt with a 2px margin. Hack TTFs live in `Resources/Fonts/` (Regular + Bold, MIT/OFL from `source-foundry/Hack`) and are registered in `MauiProgram.ConfigureFonts`. Don't switch to a system font without asking.
- **`OpenPostPage` is the Shell root (the home screen).** The app launches into the Open list, not a blank editor. `AppShell.xaml` declares the single `ShellContent` as `OpenPostPage` (route `open`); `EditorPage` is a pushed route (`Routing.RegisterRoute("editor", …)`). Selecting a post or tapping `new` on the home screen pushes the editor on top; the editor's `open` is `GoToAsync("..")`, which pops back home. Don't restore `EditorPage` as the root — that's the old "launch into a new post" default that was deliberately replaced.
- **No persistent chrome.** The only navigation surface is a thin status bar at the bottom of each page. On `EditorPage`: New / Open / Settings / Save / Publish (Save and Publish hidden until `IsDirty`). On the `OpenPostPage` home screen: `new` (→ blank editor + title prompt) / `settings`.
- **New posts go through a full-window title-prompt overlay** driven by `EditorViewModel.ShowTitlePrompt`. The editor starts empty; the prompt seeds the YAML frontmatter (title, author, date, draft, tags, description) and an H1 heading from the title. Don't auto-seed frontmatter without going through the prompt — the blank-canvas cure depends on it.
- **The gh terminal page** (`Views/GhTerminalPage.xaml`, route `terminal`) is intentionally dark + monospace + green-on-black. It accepts a `gh` subcommand and an optional stdin block (used for `auth login --with-token`). It's the canonical in-app PAT entry point **on desktop**; don't add a Settings PAT field there. Android is the exception: with no `gh` terminal, Settings has a `[ github account ]` section that pastes a token straight into `SecureStorage` (see **Android notes**).
- **Inter-page post handoff** uses `IPendingPostBox` (singleton). On select, `OpenPostViewModel` fires `PostOpened` (the page calls `box.Put`) then `EditorRequested` (the page calls `GoToAsync("editor")`); `EditorPage.OnAppearing` takes the post. The `new` path fires `EditorRequested` with an empty box, so the editor falls through to its title-prompt overlay. Don't refactor to Shell query strings or `MessagingCenter` — the box is simple and untyped-route-friendly.

## Android notes

The Android head landed 2026-05-30 (`net10.0-android`). Key divergences from Windows:

- **UI chrome.** The desktop bottom status bar and `MenuBar` don't fit a phone. On Android `Shell.NavBarIsVisible` is true and page actions live in the Material top-app-bar **overflow menu** (`ToolbarItems`, `Order=Secondary`); the bottom bar is hidden via `OnPlatform`, and the title-prompt overlay is mobile-sized the same way. The in-app GitHub path **is live** on Android (via `HttpGitHubGateway`): the GitHub repo/target-branch config and a PAT-based `[ github account ]` section show in Settings, and Save (commit) / Publish (PR) / Merge are in the editor overflow. Only the genuinely CLI-bound surfaces stay hidden there — the `gh` terminal and the local-clone sync chip (no `gh`/`git` binary); `GitCliStatusService` still degrades gracefully (catches the missing-binary exception).
- **Editor bridge is the sharp edge.** MAUI's `HybridWebView` renders `Resources/Raw/editor/index.html`, but its **JS↔host raw-message bridge does not work on Android** in MAUI 10.0.20: `window.HybridWebView` is never injected, so `SendRawMessage`/`RawMessageReceived` never fire. And `EvaluateJavaScriptAsync`'s awaited Task **hangs during page load** (its *return value* works once the page is stable). So `EditorPage` (`#if ANDROID`):
  - **host→JS:** fires `EvaluateJavaScriptAsync("…setTextB64(b64)…")` **without awaiting** (the JS still runs); seeds on a short retry loop, no readiness handshake. Base64 survives MAUI's URL-encode round-trip.
  - **JS→host (edit-sync):** on Save, `EditorViewModel.SyncBeforeSaveAsync` → `EditorPage` calls `getText()` via `EvaluateJavaScriptAsync` (return works post-load) and JSON-decodes the result into `RawMarkdown`.
  - Don't try to use the HybridWebView raw-message bridge here — use those `EvaluateJavaScriptAsync` patterns. `BridgeLog` mirrors to logcat tag `PXBRIDGE` (`adb logcat -s PXBRIDGE`).
- **Deploy.** `bun android` (`scripts/android.ps1`) builds a **clean embedded** APK (`-p:EmbedAssembliesIntoApk=true`) and `-t:Run`s it. Stale MAUI fast-deploy state shows a **blank PhoenixBlue screen**; if you hit that, `adb uninstall net.bluefenix.postxing` then redeploy. adb ships with the Android SDK (`…\Android\android-sdk\platform-tools\adb.exe`), not on PATH.
- **Storage + GitHub are live on Android.** SAF scoped storage (`SafFolderPicker` + `SafLocalPostStore`, persisted tree URI) backs the Local Posts Folder, and the in-process `HttpGitHubGateway` + a SecureStorage PAT give the phone the full open / save-as-commit / publish-PR / merge loop against the blog repo. `FileSystemSettingsStore` uses `FileSystem.AppDataDirectory` on Android for settings.

## Branch model

- `main` — production. Operator-push only. PRs from `stage` only. Linear history required.
- `stage` — pre-production integration. PRs from `develop`. Coverage gate.
- `develop` — active integration branch (formerly `px-modernized`). Feature branches merge here.
- `px-decompiled` — frozen snapshot of the recovered 2007 decompilation (the pre-rewrite legacy code). Reference only; not in the px4 build / PR pipeline.
- `post/<slug>-<date>` — short-lived branches created by the app for individual blog posts via `GitHubPublishService`.

The app never pushes directly to `stage` or `main`.

**Branch protection is live on `main` and `stage`** (since 2026-05-29). Both require a PR plus the `Build + Test` CI check before merge; `main` also requires linear history. `enforce_admins` is off, so the operator/admin can still push or fast-forward directly (the escape hatch used for the px4 square-one promotion) — everyone else, and normal flow, goes through PRs (`develop` → `stage` → `main`). Force-pushes and deletions are blocked on both; a true force-rewrite needs `allow_force_pushes` toggled off first. CI fires only on these two gate branches (push + PR); check `develop` on demand with `gh workflow run CI --ref develop`.

## Identity / packaging

- `ApplicationId` = `net.bluefenix.postxing`
- `<Company>` = `Blue Fenix Productions LLC`
- `WindowsPackageType=None` — unpackaged WinUI 3. Switch to MSIX and add `Platforms/Windows/Package.appxmanifest` when ready to sign and ship.
- MSIX `Publisher` (when MSIX is enabled) = `CN=Blue Fenix Productions LLC` — must match the full subject DN of the code-signing cert byte-for-byte once issued.

## Hard rules

- **Windows + Android.** The Windows-only rule was lifted for **Android** on 2026-05-30; don't add macCatalyst/iOS/Linux. **No Octokit, LibGit2Sharp, or GitLab adapters** — but note the deliberate Android carve-out: since Android can't shell out to `gh`/`git`, its GitHub path is an **in-process `HttpClient`** implementation of `IGitHubGateway` (`HttpGitHubGateway`) — neither a shell-out nor Octokit. Only the `gh` terminal and the local-clone sync chip stay hidden on Android (no `gh`/`git` binary). If the platform set changes again, the user will say so.
- **Don't add speculative abstractions or "future maybe" code paths.** YAGNI is enforced; left-in dead code is worse than no code.
- **Never propose uninstalling any Visual Studio install, Build Tools install, or VS-installer-managed component.**
- **`.ps1` files written via the Write tool must be ASCII-only.** Windows PowerShell 5.1 misdecodes BOM-less UTF-8; if a script needs a non-ASCII character, use a `[char]` escape.
- **Do not fabricate confident-sounding .NET / Windows internals lore.** The user has 25 years of C# and an ASP MVP from ~2004-2007. "I don't know, want me to check?" is the right move.
- **Don't recommend blanket reformats with `dotnet format` or CSharpier without asking.**
- **Don't migrate `[ObservableProperty]` fields to partial properties.** `CommunityToolkit.Mvvm` is pinned at 8.4, which emits `MVVMTK0045` advising the partial-property syntax for WinRT AOT compat — but 8.4's source generator does *not* synthesize the partial property bodies, so the migration breaks the build. The MVVMTK0045 suppression in `.editorconfig` is deliberate. Revisit only after bumping the package to ≥ 8.5.
- **TDD discipline for new feature work** — red → green → refactor. Tests live in `tests/`. Frameworks: xUnit 2.9.x, Shouldly (not FluentAssertions — Xceed Community License), NSubstitute, coverlet.

## Known gaps

The GitHub **Save (commit)**, **Publish (open PR)**, and **Merge** paths are now wired and live on both heads — Android via `HttpGitHubGateway`, desktop via `gh`. This supersedes the former stubs documented here (the `SaveCommand` GitHub branch, the unsubscribed `PublishConfirmationRequested` modal, and `GetFileShaAsync` returning null). Remaining gaps:

- **Device-flow auth isn't built.** Android auth is PAT-paste only (Settings → `[ github account ]`), pending a registered Blue Fenix GitHub OAuth App `client_id`.
- **Publish leaves the source draft in place.** When `posts/{yyyy-MM-dd}-{slug}.md` is published, the originating `drafts/{slug}.md` isn't deleted — `IGitHubGateway` has no delete-file op yet.

## License notes on excluded packages

Considered and rejected:
- `FluentAssertions` >= 8 — Xceed Community License → use `Shouldly`
- `MediatR` >= 12.4 — paid for commercial use → plain DI handlers
- `AutoMapper` >= 14 — paid for commercial use → hand-write mappings
- `Moq` — SponsorLink concern → `NSubstitute`
- `Octokit` — rejected on scope grounds, not licensing. The GitHub backend is the `gh` CLI on desktop and a hand-rolled `HttpClient` gateway (`HttpGitHubGateway`) on Android — still no `Octokit` reference.

## Pickup

This file is the canonical pickup document. The active branch is `develop`. After a fresh clone (and `dotnet workload install maui-windows maui-android`), all of these should succeed:

```powershell
dotnet run -f net10.0-windows10.0.19041.0   # launch the Windows app (-f required: multi-target)
dotnet build                    # App (both heads) + libs
bun xunit   # or npm run xunit  # full slnx test pass
bun run build  # or npm run build  # full slnx build
bun android                     # build + deploy to a connected Android device
```

If any of those fail, that is the first thing to fix — the layout (App csproj at root, slnx under `solution/`, `package.json` scripts) is load-bearing and documented in **Run / build / test from the repo root** above.
