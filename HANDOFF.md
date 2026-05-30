# PostXING 4.0 — Handoff (2026-05-30)

In-flight follow-ups after the Android port. This is a working pickup note; `CLAUDE.md` is the
canonical project doc (it has an **Android notes** section worth reading first). The full design
for the two pending items lives in the plan at
`~/.claude/plans/can-maui-be-deployed-tranquil-badger.md`.

## Repo / branch state

- **Remote:** `github.com/BlueFenixProductions/PostXING` (`origin`).
- **Local main checkout:** `C:\Users\Chris\Documents\GitHub\PostXING-decompiled` (folder name is a
  holdover from the shelved decompilation; the project is "PostXING 4.0"). On `develop`.
- **Worktree** (where the Android feature work happens): `C:\Users\Chris\Documents\GitHub\PostXING-android`
  on branch `feat/android-target`. The pattern this session: `git -C <worktree> reset --hard develop`,
  do the work, commit, `git -C <main> merge feat/android-target` (fast-forward), repeat.
- **develop:** local tip `a49aed3`; **`origin/develop` is one behind at `efe1b90`** — the SAF-foundation
  commit hasn't been pushed. Push with `git -C <main> push origin develop` to keep CI current.
- `main`/`stage` are branch-protected (PR + `Build + Test` check); `develop` is the integration branch.

## Done + merged this arc

| Item | Commit | State |
|---|---|---|
| Android version stamping + monotonic versionCode | `e6ea789` | `bun android` stamps; device shows versionName=4.4.530, versionCode=40530002 (monotonic) |
| Blue-screen fix (fast-deploy) | `efe1b90` | `android.ps1` uninstalls before deploy; device renders reliably |
| CI Android build | (config on develop) | **CI green on develop** (real run: workloads + Android compile + tests, 6m6s) |
| SAF foundation: `ILocalPostStore.CreateAsync` | `a49aed3` | path/URI construction moved into the store; 105 tests green, both heads compile |

## Pending work

### A — native Android SAF (the deferred big one)

Foundation is in place: `ILocalPostStore` now has `CreateAsync(folder, relativePath, contents) -> opaque id`,
and `LocalPostFile.Id` is opaque (filesystem path on desktop, SAF document URI on Android). The desktop
`Services/FileSystemLocalPostStore.cs` implements all four methods. `EditorViewModel.SaveAsync` (New
branch) calls `CreateAsync` — no more `Path.Combine` that would mangle a `content://` URI.

To finish (operator chose **native SAF, no new package**):
1. **`IFolderPicker`** (in `PostXING.ViewModels`): `Task<string?> PickFolderAsync()` → folder id
   (path on Windows, persisted tree-URI string on Android, null on cancel).
2. **Android picker** (`Platforms/Android`): `Intent(Intent.ActionOpenDocumentTree)` via
   `StartActivityForResult` + a `TaskCompletionSource` wired through `MainActivity.OnActivityResult`
   (or AndroidX `RegisterForActivityResult`); then **`ContentResolver.TakePersistableUriPermission(uri, Read|Write)`**
   so the grant survives restarts. Return `uri.ToString()`.
3. **`SafLocalPostStore : ILocalPostStore`** (`#if ANDROID`): use `Android.Provider.DocumentsContract`
   + `ContentResolver` (NOT `DocumentFile` — that needs the `Xamarin.AndroidX.DocumentFile` package):
   - `List(treeUri)`: query children under `drafts/`/`posts/` via `DocumentsContract.BuildChildDocumentsUriUsingTree`; return doc URIs as `Id`.
   - `ReadAsync(docUri)`: `ContentResolver.OpenInputStream`.
   - `WriteAsync(docUri)`: `ContentResolver.OpenOutputStream(uri, "wt")`.
   - `CreateAsync(treeUri, "drafts/x.md", ...)`: find/create the subfolder dir doc, `DocumentsContract.CreateDocument("text/markdown", "x.md")`, write, return the new doc URI.
   - **Verify these DocumentsContract signatures against the installed .NET-Android binding — don't fabricate.**
4. **DI** (`MauiProgram.cs`): `#if ANDROID` → `SafLocalPostStore` + Android `IFolderPicker`; else the desktop store (+ a Windows picker, or keep the typed Entry).
5. **Settings UI** (`Views/SettingsPage.xaml`): Android "Pick folder" button → `SettingsViewModel.PickFolderCommand`
   (calls `IFolderPicker`, stores into `LocalFolder`); keep the typed Entry on Windows via `OnPlatform`.
6. **Verify on device:** pick folder (grant SAF) → New post → type → Save → reopen the app → draft persists and reopens. Windows local-folder save unchanged.

`OpenPostViewModel.RefreshAsync`/`SelectAsync` already pass the opaque `Id` through, so they need no change once `List` returns document URIs.

### D — git interaction (Windows-only; hidden on Android)

The GitHub **backend is fully built + tested**; the gap is UI wiring + one stub. Operator wants all three:

- **D1 Publish flow:** `EditorPage.xaml.cs` subscribe to `vm.PublishConfirmationRequested` (mirror the
  `PreviewRequested` wiring) → confirm modal w/ auto-merge → new `EditorViewModel.ConfirmPublishAsync(autoMerge)`
  builds `SiteConfig` (settings Owner/Repo/DevelopBranch) + `Post` (FrontMatter/Slug/body) and calls
  `GitHubPublishService.PublishAsync(post, site, RawMarkdown, autoMerge)` (signature confirmed). Surface
  `PublishState` in `SaveStatus`. **Inject `GitHubPublishService` into `EditorViewModel`** (6 deps → 7) and
  update the `Ctor_takes_only_the_six_real_dependencies` locking test in `EditorViewModelTests`.
- **D2 draft-save + open + GetFileShaAsync:** `EditorViewModel.SaveAsync` `PostSource.GitHub` branch →
  `GitHubPublishService.SaveDraftAsync(post, site, RawMarkdown)` (currently a "use the gh terminal" stub).
  Implement the sha: add `IGitHubGateway.GetFileShaAsync(owner, repo, branch, path)` (`gh api
  repos/{o}/{r}/contents/{path}?ref={b} --jq .sha`) in `GhCliGitHubGateway` + `InMemoryGitHubGateway`, and
  use it in `GitHubPublishService.GetFileShaAsync` (replace `return null`, lines ~77-81). Open-from-GitHub is
  already wired in `OpenPostViewModel.SelectAsync` (fetches via `GetFileContentAsync`, `PostHandle.FromGitHubPath`).
- **D3 local git:** extend `GitCliStatusService` (or a sibling) with `CommitAsync/PushAsync/PullAsync`,
  reusing its `MakeProcessRunner` pattern (`git -C <folder> …`, `GIT_TERMINAL_PROMPT=0`). UI: make the sync
  chip actionable (action sheet: commit-with-message / push / pull) on `EditorPage` + `OpenPostPage`; refresh
  the chip after each.

Gate all D behind `#if WINDOWS` / `OnPlatform` — no `gh`/`git` CLI on Android.

## Hard-won knowledge (don't relearn)

- **MAUI HybridWebView's JS↔host bridge is dead on Android (10.0.20):** `window.HybridWebView` never
  injects; `SendRawMessage`/`RawMessageReceived` never fire. Host→JS uses **fire-and-forget
  `EvaluateJavaScriptAsync`** (the JS runs even though the awaited Task hangs); JS→host (edit-sync) uses
  `getText()` via `EvaluateJavaScriptAsync` **return**, which works once the page is stable (hangs *during*
  load). See `Views/EditorPage.xaml.cs` `#if ANDROID` and the CLAUDE.md Android notes.
- **Fast-deploy gremlin:** a stale `.__override__` dir leaves a blank PhoenixBlue screen. `bun android`
  (`scripts/android.ps1`) now `adb uninstall`s first + builds embedded (`-p:EmbedAssembliesIntoApk=true`).
  If you ever see blue: `adb uninstall net.bluefenix.postxing` and redeploy.
- **Android versionCode** is `minor*10_000_000 + patch*1_000 + build` (monotonic, < 2^31), computed by
  `android.ps1` from `.version`.
- **adb** is at `C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe` (not on PATH).
  Bridge trace → logcat tag `PXBRIDGE` (`adb logcat -s PXBRIDGE`).
- **Multi-target:** `dotnet run` needs `-f net10.0-windows10.0.19041.0`; `bun dev`/`dev:build` build
  Windows-only; `bun android` deploys the phone; `bun run build` is the full-graph (both heads) CI build.

## Build / run / test / deploy

```powershell
dotnet run -f net10.0-windows10.0.19041.0   # Windows app
bun dev          # fast Windows relaunch (no rebuild)
bun dev:build    # Windows incremental build + launch (after edits)
bun android      # clean build + deploy to the connected Pixel 7 (USB debugging on)
bun run build    # full slnx Release, both heads (CI parity)
bun xunit        # 105 tests
gh workflow run CI --ref develop   # run CI on demand (CI auto-fires only on main/stage)
```

## Pointers

- `CLAUDE.md` — canonical project doc (Android notes, branch model, hard rules).
- `~/.claude/plans/can-maui-be-deployed-tranquil-badger.md` — full A/B/C/D design.
- homelab-topology journal: `journal/2026-05-30-postxing-android-port.md`.
- Memory: `project-android-port`, `reference-android-hybridwebview-bridge`.
