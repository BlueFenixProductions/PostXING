# PostXING 4.0 — Handoff (2026-05-30, late session)

Working pickup note. `CLAUDE.md` is the canonical project doc (read its **Android notes**
section first). This file rolls forward only; for the in-flight design that drove the
SAF + git work, see `~/.claude/plans/can-maui-be-deployed-tranquil-badger.md`.

## Repo / branch state

- **Remote:** `github.com/BlueFenixProductions/PostXING` (`origin`).
- **Local main checkout:** `C:\Users\Chris\Documents\GitHub\PostXING-decompiled` (folder name is
  a holdover from the shelved decompilation; the project is "PostXING 4.0"). On `develop`.
- **Worktree** (where Android feature work happens): `C:\Users\Chris\Documents\GitHub\PostXING-android`
  on `feat/android-target`. Pattern: edit there → commit → `git rebase develop` (cheap, the
  Android-only follow-up commits keep ff-merging cleanly) → in the main checkout
  `git merge --ff-only feat/android-target && git push origin develop`. Both checkouts are
  clean and at the same tip.
- **develop tip = origin/develop tip = `8a37246`** (pushed). All session work is on develop.
- **`main`/`stage`** are branch-protected (PR + `Build + Test` check); `develop` is the
  integration branch. CI auto-fires only on main/stage; trigger on develop on demand with
  `gh workflow run CI --ref develop`.
- **Device:** Pixel 7 / CalyxOS, USB debugging on. Currently running 4.4.530.9
  (versionCode 40530009) — the develop tip stamped + redeployed at end of session.

## Done this session (all on develop)

| Item | Commit | State |
|---|---|---|
| HANDOFF.md introduced | `176fc64` | starting handoff for the SAF + git follow-ups |
| **A — native Android SAF storage** | `87a50cc` | `IFolderPicker` + `SafFolderPicker` (ACTION_OPEN_DOCUMENT_TREE + persistable Read\|Write grant via `MainActivity.OnActivityResult`) + `SafLocalPostStore` (DocumentsContract + ContentResolver, no `DocumentFile` package) + DI branching in `MauiProgram` + Settings "pick folder..." button. `AppSettings.IsLocalConfigured` relaxed from `Directory.Exists` to non-empty so SAF `content://` URIs qualify. **Verified on Pixel 7 for a Termux-managed folder**; non-Termux trees untested. |
| **D — publish flow + GH draft-save + local git ops** | `d77d027` | EditorPage `PublishConfirmationRequested` → action-sheet modal (Windows-only) → `EditorViewModel.ConfirmPublishAsync` → `GitHubPublishService.PublishAsync`. `IGitHubGateway.GetFileShaAsync` is real on the gateway + used by the service (`gh api .../contents/{path}?ref=... --jq .sha`; 404 → null). New thin `GitHubPublishService.SaveToBranchAsync(site, path, content, msg)` so SaveAsync's GH branch writes back to the opened path (works for drafts/ and posts/). `IGitStatusService` grows `CommitAsync/PushAsync/PullAsync` + new `GitOperationResult` record; sync chip on Editor + Open pages is actionable (commit & push / commit / push / pull / refresh). EditorViewModel ctor 6→7 deps, locking test updated. **113/113 xunit pass** (+8 ops tests). |
| **Android bugfixes pass 1** | `11b8e04` | (a) `MainActivity` gets `WindowSoftInputMode = SoftInput.AdjustResize`. (b) `Services/PreviewStyles` switched from `File.ReadAllText(AppContext.BaseDirectory + …)` to `FileSystem.OpenAppPackageFileAsync("preview/{file}")` so the github-markdown CSS actually loads on Android (was returning empty → preview body rendered unreadable black-on-dark). (c) `EditorPage.PreviewRequested` calls `SyncEditorTextBeforeSaveAsync()` on Android so the preview sees current typing, not the seed. |
| **Android editor pass 2** (IME composition) | `7cf0532` | `Resources/Raw/editor/index.html`: contenteditable changed `true` → `plaintext-only`; track `compositionstart`/`compositionend` and skip rehighlight while composing (rehighlight was aborting IME dictation mid-utterance + dropping Enter). Voice-to-text now commits the full utterance. |
| **Android editor pass 3** (viewport sizing) | `8a37246` | `#editor` height now follows `window.visualViewport.height` via a CSS custom property `--vv-h` updated from JS. Independent of WebView AdjustResize quirks. `scrollCaretIntoView()` is wrapped in `requestAnimationFrame` and falls back to the containing element's rect when the collapsed range's own rect is `(0,0,0,0)` (an Android WebView quirk at end-of-line). |

## Open verifies (operator-driven)

These all rely on either a real GitHub repo, a phone, or both. None can be exercised from
the test suite.

- **SAF — non-Termux folder trees.** Pick a primary-external folder (e.g. `Documents/`,
  `Downloads/`) and confirm New → type → Save → reopen still persists. The hand-rolled
  `Intent.ActionOpenDocumentTree` + `MainActivity.OnActivityResult` routing works for the
  Termux provider; if a system DocumentsProvider doesn't return through that path, the
  next reasonable move is MAUI's own `Microsoft.Maui.Storage.FolderPicker` (which uses
  AndroidX `RegisterForActivityResult` under the hood). **Caveat:** `FolderPicker` is not
  present in MAUI 10.0.20 — I tried that route and the build errored with
  `CS0103: The name 'FolderPicker' does not exist`. So either wait for a MAUI bump, or
  switch the hand-rolled picker to `ComponentActivity.RegisterForActivityResult` directly.
- **Android editor pass 3 verify.** Re-confirm: long body that pushes past the IME stays
  visible above the keyboard as you type; Enter creates a real new line; cursor follows.
  If it still drifts, `adb logcat -s PXBRIDGE *:E` and the in-memory diag ring in
  `index.html` (window.PostXING.getDiag()) are the trace surface.
- **Windows publish flow against a real GitHub repo.** Open a draft → tap Publish →
  pick "open PR only" → confirm a real PR shows up on the configured Owner/Repo against
  `DevelopBranch`. Status line should read `PR #N opened on post/<slug>-<date>`. Auto-merge
  variant waits up to 10 min for checks then squash-merges. The service has been there since
  pre-session; only the UI wiring is new.
- **Windows sync chip on the PostXING repo itself.** Set Local Posts Folder to a real git
  repo (e.g. this checkout), tap the chip → commit & push exercises the new
  `GitCliStatusService.CommitAsync/PushAsync` against `git -C <folder> …`. Pull uses
  `--ff-only` and refuses a diverged-history auto-merge.

## Architecture notes you'll want when you pick this up

### The Save-GH path opportunistically chose layering over the handoff's literal text

The pre-session handoff said `EditorViewModel.SaveAsync` GitHub branch should call
`GitHubPublishService.SaveDraftAsync(post, site, RawMarkdown)`. I went with a small new
`SaveToBranchAsync(site, path, content, msg)` instead because `SaveDraftAsync` hard-codes
`drafts/{slug}.md` from the post's title — saving a post opened from `posts/` would
silently re-route to `drafts/` (or rename on title change). `SaveToBranchAsync` writes
back to `_handle.Identifier` (the path the post was opened from), mirroring the
LocalFile branch's "write back to where it came from" behavior. `SaveDraftAsync` is
still around for a future "save as GitHub draft from New" flow.

### `IGitStatusService` grew ops methods, not a sibling interface

The handoff suggested "extend `GitCliStatusService` (or a sibling)". I added
`CommitAsync/PushAsync/PullAsync` directly to `IGitStatusService` rather than minting a
new interface — same `GitRunner` plumbing, same shell-out posture, no need for two
DI registrations. `git pull --ff-only` is deliberate: we refuse silent auto-merges of a
diverged history; the user resolves that in their git client. `git commit` exiting
non-zero with "nothing to commit" is treated as benign success (otherwise the chip
flashes red for a no-op).

### Android editor bridge — fully documented in CLAUDE.md and `reference-android-hybridwebview-bridge` memory

The IME composition issue (pass 2) added a real load-bearing rule on top: **do not touch
`editor.innerHTML` while a composition is active**. This is now baked into
`Resources/Raw/editor/index.html`'s `input` handler. The `compositionend` hook is what runs
the deferred rehighlight + post + scrollCaretIntoView.

The `--vv-h` visualViewport sizing (pass 3) is the same pattern most mobile-first editor
shells use; if you ever wonder why `100vh` "should work", the answer is that Android WebView
historically doesn't shrink the layout viewport on AdjustResize, so `100vh` lies about the
visible area when the IME is open. `visualViewport.height` does not lie.

## Hard-won knowledge (still load-bearing)

- **MAUI HybridWebView's JS↔host raw bridge is dead on Android (10.0.20):** `window.HybridWebView`
  never injects; `SendRawMessage` / `RawMessageReceived` never fire. Host→JS uses
  **fire-and-forget `EvaluateJavaScriptAsync`** (the JS runs even though the awaited Task hangs);
  JS→host uses `EvaluateJavaScriptAsync` **return value** of `window.PostXING.getText()`, which
  is safe once the page is stable (hangs *during* load). See `Views/EditorPage.xaml.cs`
  `#if ANDROID` and the CLAUDE.md Android notes.
- **Fast-deploy gremlin:** a stale `.__override__` dir leaves a blank PhoenixBlue screen.
  `bun android` (`scripts/android.ps1`) now `adb uninstall`s first + builds embedded
  (`-p:EmbedAssembliesIntoApk=true`). If you ever see blue: `adb uninstall net.bluefenix.postxing`
  and redeploy.
- **Android versionCode** is `minor*10_000_000 + patch*1_000 + build` (monotonic, < 2^31),
  computed by `android.ps1` from `.version`.
- **adb** is at `C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe` (not on
  PATH). Bridge trace → logcat tag `PXBRIDGE` (`adb logcat -s PXBRIDGE`).
- **Multi-target:** `dotnet run` needs `-f net10.0-windows10.0.19041.0`; `bun dev`/`dev:build`
  build Windows-only; `bun android` deploys the phone; `bun run build` is the full-graph
  (both heads) CI build.
- **Contenteditable must be `plaintext-only` + composition-guarded.** Don't revert to
  `contenteditable="true"` without re-thinking IME handling; the title-prompt seed and
  the rehighlight pipeline depend on plain-text editing.

## Build / run / test / deploy

```powershell
dotnet run -f net10.0-windows10.0.19041.0   # Windows app
bun dev          # fast Windows relaunch (no rebuild)
bun dev:build    # Windows incremental build + launch (after edits)
bun android      # clean build + deploy to the connected Pixel 7 (USB debugging on)
bun run build    # full slnx Release, both heads (CI parity)
bun xunit        # 113 tests
gh workflow run CI --ref develop   # run CI on demand (CI auto-fires only on main/stage)
```

## Pointers

- `CLAUDE.md` — canonical project doc (Android notes, branch model, hard rules).
- `~/.claude/plans/can-maui-be-deployed-tranquil-badger.md` — the A/B/C/D design that
  drove this arc; A and D are landed, B/C were not on the table this session.
- homelab-topology journal: `journal/2026-05-30-postxing-android-port.md`.
- Memory: `project-android-port`, `reference-android-hybridwebview-bridge`.
