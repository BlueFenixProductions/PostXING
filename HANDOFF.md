# PostXING 4.0 — Handoff (2026-06-05)

Working pickup note. `CLAUDE.md` is the canonical project doc (read its **Android notes** and
**Hard rules** first). This file **rolls forward only**: the "Done this session" + "Repo/branch
state" sections reflect the latest session; the "Durable knowledge" / "Architecture notes" below
accumulate and are kept reconciled with CLAUDE.md. Rolled forward from the 2026-05-30 Android-port
handoff (its per-commit session log is now git history; its still-load-bearing knowledge is kept
below, with the blank-screen note corrected per commit `72552d8`).

## Repo / branch state

- **Remote:** `github.com/BlueFenixProductions/PostXING` (`origin`).
- **Main checkout:** `C:\Users\Chris\Documents\GitHub\PostXING` — on `develop`. (Supersedes the old
  `PostXING-decompiled` / `PostXING-android` two-folder layout in the 2026-05-30 note.)
- **This session's worktree:** `C:\Users\Chris\Documents\GitHub\PostXING\.claude\worktrees\practical-ptolemy-ce845a`
  on `claude/practical-ptolemy-ce845a` (same tip as develop).
- **develop tip = `5a037c9` = `origin/develop` — pushed and in sync** (operator-pushed 2026-06-05).
- **`main`/`stage`** are branch-protected (PR + `Build + Test` check); `develop` is the integration
  branch. CI auto-fires only on main/stage; run on develop on demand:
  `gh workflow run CI --ref develop`.
- **Device (Pixel 7 / CalyxOS):** not touched this session; current installed build unverified.

## Done this session (2026-06-04) — issue #10 "default content folder = blog"

Implemented, tested, **merged to develop and pushed to origin** (fast-forward `72552d8 → 5a037c9`; `origin/develop` = `5a037c9`).

| Item | Commit | State |
|---|---|---|
| **#10 — first-run personal-defaults seed** | `cfb6810` | Pre-fills Settings on a fresh install / after a wipe (content folder=`blog`, owner/repo, integration branch=`main`, author) **without committing personal values to the public repo**. Mechanism: gitignored `defaults.local.json` at repo root → conditionally embedded (`EmbeddedResource` + `Exists` guard in `PostXING.App.csproj`) → `FileSystemSettingsStore.LoadAsync` seeds `Current` from it when no `settings.json` exists, else neutral `AppSettings.Default`. Parser `SettingsSeed.ParseOrDefault` (net10.0, unit-tested). Committed source stays neutral; no XAML change. 6 files. |
| **Local permission allowlist** | `5a037c9` | `.claude/settings.local.json` expanded (you asked to include it in the merge). |

**Verification:** TDD red→green; full suite **169 passed / 0 failed** (Core 7 · Markdown 66 · GitHub 47 · ViewModels 49); Windows build clean; Android head compiles (slnx test AOT pass); seed embed byte-verified in `PostXING.App.dll` (resource name + `vitepress.chris.pelatari.com` / `blog` present).

**Live-verified 2026-06-05:** app boots clean on the seed branch (shell-wrapped `bun dev`); the new `LoadAsync` seed path runs during App construction with no crash/deadlock.

## Next steps (immediate)

_Status 2026-06-05: items 1 and 2 are DONE (operator pushed origin/develop; app boots clean on the seed branch via shell-wrapped `bun dev`). Kept below as the runbook._

1. **Push develop → origin — DONE** (was ahead 2): `git -C C:/Users/Chris/Documents/GitHub/PostXING push origin develop`. No CI gate on develop, so it's a quiet publish.
2. **Live app run, the right way** (also the only way to *see* the seeded fields):
   ```powershell
   Rename-Item "$env:APPDATA\PostXING\settings.json" settings.json.bak   # force the seed branch
   bun dev                                                               # Settings should show blog / ChrisPelatari / main / Chris Pelatari
   # close the app, then restore:
   Rename-Item "$env:APPDATA\PostXING\settings.json.bak" settings.json
   ```
   NB: do **not** launch bun via `Start-Process bun` — on Windows `bun` is a shim, so it fails with
   *"%1 is not a valid Win32 application."* Run `bun dev` directly in a shell.
3. *(optional)* Add a short CLAUDE.md note on the `defaults.local.json` seed mechanism (today it's
   self-documented only via `defaults.local.example.json` + the `.gitignore` comment).

## Carried-forward open verifies (from 2026-05-30 — status UNCONFIRMED this session)

Operator-driven; none exercisable from the test suite. Re-confirm whether still open before acting.

- **SAF non-Termux folder trees.** Pick a primary-external folder (`Documents/`, `Downloads/`) →
  New → type → Save → reopen still persists. The hand-rolled `ActionOpenDocumentTree` +
  `MainActivity.OnActivityResult` works for the Termux provider; if a system DocumentsProvider
  doesn't route back, switch to `ComponentActivity.RegisterForActivityResult` (MAUI's own
  `FolderPicker` was absent in 10.0.20 — `CS0103`).
- **Android editor viewport (pass 3).** Long body past the IME stays visible while typing; Enter
  makes a real newline; cursor follows. Trace: `adb logcat -s PXBRIDGE` + `window.PostXING.getDiag()`.
- **Windows publish flow against a real repo.** Draft → Publish → "open PR only" → a real PR on the
  configured Owner/Repo vs `DevelopBranch`; status line `PR #N opened on post/<slug>-<date>`.
- **Windows sync chip on a real git repo.** Local Posts Folder = a real repo → chip → commit & push
  via `GitCliStatusService.CommitAsync/PushAsync`; pull is `--ff-only` (refuses diverged auto-merge).

## Durable knowledge (load-bearing — kept reconciled with CLAUDE.md)

- **#10 seed is SYNCHRONOUS on purpose.** `FileSystemSettingsStore.ReadEmbeddedSeed` +
  `SettingsSeed.ParseOrDefault` use sync I/O (`StreamReader.ReadToEnd`, `JsonSerializer.Deserialize`)
  because `LoadAsync` must stay synchronous — it's DI-resolved via a blocking
  `GetAwaiter().GetResult()` during `App` construction. An awaited async read there deadlocks (next
  bullet). Don't "async-ify" the seed path. `defaults.local.json` is **per-checkout** (gitignored):
  present in both the main develop checkout and this worktree; absent on a public clone / CI → neutral
  defaults.
- **Blank PhoenixBlue splash = startup DEADLOCK, not stale fast-deploy state.** The `.__override__`
  theory was a 2026-06-03 red herring (corrected in CLAUDE.md, commit `72552d8`). Cause: `ISettingsStore`
  is resolved via a blocking `LoadAsync().GetAwaiter().GetResult()`, and once `App`'s ctor took an
  `ISettingsStore` dep (to apply the saved theme) that resolve runs on the main thread during App
  construction — an awaited async read (`DeserializeAsync`) deadlocks whenever a `settings.json`
  already exists. Fix: `LoadAsync` is synchronous (`File.ReadAllText` + `Deserialize`). Tell: rendered
  on a fresh install (no file → early return), blanked once settings existed. **Diagnose a blue screen
  with `adb logcat -d --pid <pid>`** — a managed `FATAL EXCEPTION` points at startup code; mono loading
  then silence = a main-thread hang/deadlock like this one. `bun android` installs **over** the app
  (preserves in-app settings / PAT / SAF grant); `bun android:clean` uninstalls (resets them) and is
  only for genuine device-side stale state — read logcat before reaching for it.
- **MAUI HybridWebView JS↔host raw bridge is dead on Android (10.0.20):** `window.HybridWebView`
  never injects; `SendRawMessage`/`RawMessageReceived` never fire. Host→JS uses **fire-and-forget
  `EvaluateJavaScriptAsync`** (JS runs even though the awaited Task hangs during load); JS→host uses
  the **return value** of `EvaluateJavaScriptAsync("window.PostXING.getText()")` (safe once the page
  is stable). See `Views/EditorPage.xaml.cs` `#if ANDROID`; trace tag `PXBRIDGE`.
- **Contenteditable must be `plaintext-only` + composition-guarded.** Don't revert to
  `contenteditable="true"`: rehighlight must not touch `editor.innerHTML` while an IME composition is
  active (it was aborting dictation + dropping Enter). `compositionend` runs the deferred rehighlight.
- **Android `versionCode`** = `minor*10_000_000 + patch*1_000 + build` (monotonic, < 2^31), from
  `.version` by `scripts/android.ps1`.
- **adb** is at `C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe` (not on PATH).
- **Multi-target:** `dotnet run` needs `-f net10.0-windows10.0.19041.0`; `bun dev`/`dev:build` build
  Windows-only; `bun android` deploys the phone; `bun run build` is the full-graph (both heads) CI build.

## Architecture notes (durable)

- **Save-to-GitHub writes back to where the post came from.** `EditorViewModel` SaveAsync's GH branch
  uses `GitHubPublishService.SaveToBranchAsync(site, path, content, msg)` (writes to the opened
  `_handle.Identifier`), not `SaveDraftAsync` (which hard-codes `drafts/{slug}.md` and would re-route a
  `posts/` edit). `SaveDraftAsync` remains for a future "save as GitHub draft from New" flow.
- **`IGitStatusService` carries the git ops** (`CommitAsync/PushAsync/PullAsync` + `GitOperationResult`),
  not a sibling interface — same `GitRunner` shell-out. `git pull --ff-only` is deliberate (no silent
  diverged-merge); `git commit` "nothing to commit" is benign success (no red chip on a no-op).
- **Android editor `--vv-h`.** `#editor` height tracks `window.visualViewport.height` via a CSS custom
  property, because Android WebView doesn't shrink the layout viewport on AdjustResize, so `100vh` lies
  when the IME is open. `scrollCaretIntoView()` is `requestAnimationFrame`-wrapped with a rect fallback.

## Build / run / test / deploy

```powershell
dotnet run -f net10.0-windows10.0.19041.0   # Windows app (-f required: multi-target)
bun dev          # fast Windows relaunch (no rebuild)
bun dev:build    # Windows incremental build + launch (after edits)
bun android      # build + deploy OVER the app on the connected Pixel 7 (keeps settings)
bun android:clean# uninstall (RESETS settings) + wipe obj/bin — rare escape hatch
bun run build    # full slnx Release, both heads (CI parity)
bun xunit        # 169 tests
gh workflow run CI --ref develop   # CI on demand (auto-fires only on main/stage)
```

## Safety / cleanup (this session)

- App settings were moved aside to exercise the seed branch and **restored** — `%APPDATA%\PostXING\settings.json` is unchanged.
- Backups remain at `%TEMP%\postxing-merge-backup\` (`defaults.local.json`, `settings.local.json`, `appdata-settings.json`) — delete when satisfied.
- This `HANDOFF.md` is **untracked** (not committed to develop).

## Pointers

- `CLAUDE.md` — canonical project doc (Android notes, branch model, hard rules, version stamping).
- `~/.claude/plans/can-maui-be-deployed-tranquil-badger.md` — the A/B/C/D design behind the Android arc (A + D landed).
- homelab-topology journal: `journal/` (e.g. `2026-05-30-postxing-android-port.md`).
- Memories: `project-android-port`, `reference-android-hybridwebview-bridge`.
