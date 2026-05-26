# PostXING modernization - session handoff (2026-05-20, after resume)

This file is the canonical resume-from-here document. Read this first; supporting context lives in the memory files (see "Memory" at the bottom).

## Status at handoff

iCloud-the-application cleanup is complete. The user-data folder `C:\Users\Chris\iCloudDrive` was deleted explicitly via robocopy /MIR + Remove-Item this session (the prior handoff's prediction that reboot's `MoveFileEx(MOVEFILE_DELAY_UNTIL_REBOOT)` finalization would clear the user-data folder was wrong — the delayed delete the MSI queued only covered the application directories under Program Files, not the user folder). Build inputs are in place. Next concrete action is running the dotnet build and working through whatever compiler errors land.

## What's done

- **iCloud uninstall complete**: no services (iCloud*, APSDaemon, Bonjour, Apple*), no Appx (per-user or provisioned), no registry uninstall entries, all `Program Files\iCloud` and `Common Files\Apple` dirs gone. `C:\ProgramData\Apple Computer` and `C:\ProgramData\Apple Inc` removed this session as the final cleanup. The `cldflt` driver is still running but that's the generic Windows CloudFilter mini-filter, not iCloud-specific (OneDrive uses it too).
- **`C:\Users\Chris\iCloudDrive` removed**: user authorized after confirming multi-machine redundancy of the contents. Used the robocopy /MIR-from-empty-source technique because the tree contained a `node_modules` checkout (deep paths). One CloudFilter placeholder residue survived — see "Known residue" below.
- **Build inputs staged**:
  - `nuget.config` at repo root, registering the repo root itself as a local NuGet source and claiming the `Microsoft.NETFramework.ReferenceAssemblies*` pattern to bypass europa's global `packageSourceMapping` rule.
  - `net20ref.nupkg` (the `Microsoft.NETFramework.ReferenceAssemblies.net20` package) at the repo root.
  - `refasm/net20/` populated with hand-extracted ref DLLs (System.Deployment, System.Xml, System.Web, System.Design, etc.).
  - `bin-staged/` holds the original compiled DLLs that decompilation came from; inter-project references in the csprojs point at these.
- **CLAUDE.md written** (2026-05-20, this session) — architecture + build + repo-specific gotchas for future Claude Code sessions in this directory.
- **Memory migrated** from the iCloud-pathed project key (`C--Users-Chris-iCloudDrive-Documents-GitHub-PostXING-decompiled/`) to the local-pathed key (`C--Users-Chris-Documents-GitHub-PostXING-decompiled/`) under `~/.claude/projects/`, so memory auto-loads now that the working directory is the local path.

## Immediate next step

1. **Run the build**: `dotnet build src\PostXING\PostXING.csproj`. The first restore will pull `Microsoft.NETFramework.ReferenceAssemblies` from the local source via `nuget.config` then attempt to compile. Expect a wall of decompiler-residue compile errors. Read the first batch, group by class (decompiler artifact vs. missing reference vs. genuine source bug), surface to user, don't blanket-fix.

3. **Optional**: Microsoft OpenSSH Server install on europa (`Add-WindowsCapability -Online -Name OpenSSH.Server~~~~0.0.1.0` + `Start-Service sshd` + scope firewall to Tailscale interface). Currently not installed; RDP covers the headless-access need so this is nice-to-have, not blocking.

## Known residue

A `0x9000601a` (`IO_REPARSE_TAG_CLOUD`) CloudFilter placeholder owned by `com.apple.CloudDocs` is wedged at `C:\Users\Chris\iCloudDrive\Documents\GitHub\PostXING-decompiled`. Three empty parent directories (`iCloudDrive`, `iCloudDrive\Documents`, `iCloudDrive\Documents\GitHub`) survive because that placeholder blocks them. `fsutil reparsepoint delete` returns "Error 395: Access to the cloud file is denied" — the `cldflt` driver is holding the placeholder against a provider registration that's already gone from `SyncRootManager`, so neither side can release it. Clearing it cleanly would require either reinstalling iCloud (so the provider can claim and properly unregister the placeholder) or a deeper Windows-internals operation. It's three empty dir entries with no actual data; cosmetic. Don't waste another session on it unless the user asks.

## Open / loose ends

- The user mentioned eventually wanting to delete local copies of the older VDB / Velociraptor / Velsys productions (65 csprojs targeting v4.0). All on personal GitHub. Not urgent — they want to take a manual inventory because some of those projects use old `[ModuleInitializer]`-style workarounds (the "secret passcodes" the user used pre-C#-9). Don't touch unless explicitly asked.
- The cleanup scripts at the repo root (`cleanup-europa.ps1`, `cleanup-fx-old.ps1`) had a known bug: the generic `/S /silent /quiet /qn /uninstall` flag-soup fallback breaks on SQL Server's `setup.exe`. History, not for re-execution as-is. If reused on a future box, the SQL Server step needs SQL-specific syntax (`/Action=Uninstall /QUIET /IAcceptSQLServerLicenseTerms`).
- `blogExtension.csproj` has `<TargetFramework>net10</TargetFramework>` which is almost certainly a decompiler artifact (should likely be `net20`). Flag if you touch it.

## State pointers

- **Repo (canonical)**: `C:\Users\Chris\Documents\GitHub\PostXING-decompiled` (no longer iCloud-pathed)
- **Dockerfile**: `<repo>\Dockerfile` — history only, built once into `postxing-build:latest`, image is gone with Docker
- **Ref-assembly NuGet (staged)**: `<repo>\net20ref.nupkg` — gitignored, required at repo root for restore
- **Hand-extracted ref DLLs**: `<repo>\refasm\net20\` — gitignored, required for build
- **Decompiled binary staging**: `<repo>\bin-staged\` — committed, do not delete
- **Cleanup scripts at repo root** (history, not for re-execution as-is): `cleanup-europa.ps1`, `cleanup-fx-old.ps1`
- **CLAUDE.md**: `<repo>\CLAUDE.md` — architecture + build instructions for future Claude Code sessions

## Memory

Project memory now lives at `C:\Users\Chris\.claude\projects\C--Users-Chris-Documents-GitHub-PostXING-decompiled\memory\` (local path; auto-loads):

- `user_workstations.md` — itachi (M4 Mac, daily) vs europa (Win11, mothballed); user is a 25-yr C# vet, MVP in ASP roughly 2004-2007; MSDN-era VL keys in reserve; ARM Win11 VMs on itachi off the table
- `project_postxing_modernization.md` — PostXING v2 is a 2007 WinForms blog editor decompiled via ILSpy 2026-05-18; soul work; net20 + NuGet ref-assemblies plan, no .NET 2.0 install
- `project_build_environment.md` — refreshed this session: Docker path tried and jettisoned, current path is host-native `dotnet build` on europa, forward path is Hyper-V Win11 VM (not yet built)
- `feedback_diagnostic_style.md` — concise, patient, ask before guessing, don't contradict without evidence
- `feedback_no_confident_bullshit.md` — hard rule, no fabricating .NET / Windows internals lore
- `feedback_powershell_ascii_only.md` — `.ps1` files written via Write tool must be ASCII-only
- `feedback_destructive_ops_authorized.md` — new this session: when user authorizes deleting a personal-data folder, one safety surface then act; they keep multi-machine copies of that kind of folder

Global memory at `C:\Users\Chris\.claude\memory\`:

- `feedback_pre_done_linter_check.md` — code is not "done" until linter/formatter has run with `--fix`; for `.ps1` this also means a parse check; don't recommend C# formatters without asking
- `feedback_no_uninstall_visual_studio.md` — HARD RULE, never propose uninstalling any Visual Studio version, Build Tools, or VS-installer-managed component

## Lessons captured this session (for posterity)

- **MSI uninstaller's `MoveFileEx(MOVEFILE_DELAY_UNTIL_REBOOT)` scope is narrower than expected.** It covered iCloud's *application* directories under Program Files, not the user-data folder `C:\Users\Chris\iCloudDrive`. Reboot finalized those but the user-data folder needed an explicit `Remove-Item`.
- **Windows CloudFilter placeholders can outlive their provider's `SyncRootManager` registration.** Once both sides are gone-but-not-cleanly-unregistered, `fsutil reparsepoint delete` fails with Error 395 and no userland tool can clear it. The clean path is to never let the unregister flow get half-finished — uninstall sequence matters.
- **`d----l` mode in `Get-ChildItem -Force` is the early-warning sign** of a reparse point / cloud placeholder hiding in an apparently-mundane directory tree.
- **robocopy /MIR from an empty source is the path-length-safe wipe technique** the prior handoff already named, and it worked here (`node_modules` checkout cleared without complaint) — except for the reparse-point endpoint, which it correctly refused to chase.
