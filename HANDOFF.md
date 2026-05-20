# PostXING modernization - session handoff (2026-05-19, late evening)

This file is the canonical resume-from-here document for the work we did in the long session that ended with europa being rebooted to release the iCloud CloudFilter driver state. Read this first; supporting context lives in the memory files (see "Memory" at the bottom).

## Status at handoff

The user is rebooting europa to clear the kernel-level CloudFilter mini-filter driver state that was blocking the final `C:\Users\Chris\iCloudDrive` deletion. The MSI uninstaller flagged that folder for `MoveFileEx(MOVEFILE_DELAY_UNTIL_REBOOT)` cleanup hours ago - we just spent a while not realizing the system had already told us reboot was the right move.

**Disk recovered this session**: 0.7 GB free at start -> ~36 GB free at last check. Most of the win came from Docker uninstall + iCloud Drive Archive cleanup + older SQL Server engine uninstalls via GUI. The .NET Framework Developer Pack / Targeting Pack / SDK uninstalls gave near-zero disk back because most bytes are shared components owned by Visual Studio - the entries are gone from Programs-and-Features but the files stayed.

## What's done

- **RDP from itachi -> europa**: working. The fix was updating the macOS RDP client (Microsoft renamed it to "Windows App" recently). No host-side changes needed; europa's RDP service had been listening fine the whole time.
- **Docker Desktop**: uninstalled. The `postxing-build:latest` image was built successfully against `mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022` but that path was jettisoned in favor of the Hyper-V VM direction. The Dockerfile is still in the repo as a record.
- **SQL Server**: 2022 retained for old prod maintenance; older majors uninstalled via Programs-and-Features GUI (the cleanup script's flag-soup approach broke on SQL Server's `setup.exe`).
- **iCloud for Windows**: uninstalled at the per-user MSI layer earlier in the session, then properly torn down at the provisioned-UWP layer via `Remove-AppxPackage -AllUsers` + `Remove-AppxProvisionedPackage -Online` later. The final folder deletion is pending the post-reboot CloudFilter driver release.
- **Silverlight**: uninstalled (manually before script ran).
- **.NET Framework Dev/Targeting/Multi-Targeting/SDK Packs older than 4.8**: all uninstalled via `cleanup-fx-old.ps1`. Kept: 4.8 SDK, 4.8 Targeting Pack, 4.8.1 SDK. **DO NOT** touch Visual Studio installs (hard rule, see memory).
- **PostXING repo**: copied from iCloud path to `C:\Users\Chris\Documents\GitHub\PostXING-decompiled`. The legacy iCloud copy has been deleted along with the rest of `C:\Users\Chris\iCloudDrive` (pending reboot finalization). Both PostXING and PostXING-decompiled exist on the user's personal GitHub - that's the redundancy.
- **First `dotnet build` attempt** on `src\PostXING\PostXING.csproj`: ran 6 seconds, failed at NuGet restore with `NU1100: Unable to resolve 'Microsoft.NETFramework.ReferenceAssemblies (>= 1.0.3)' for '.NETFramework,Version=v2.0'. PackageSourceMapping is enabled, the following source(s) were not considered: Microsoft Visual Studio Offline Packages, nuget.org.` - so the failure isn't structural; it's a NuGet PackageSourceMapping rule on europa preventing nuget.org from resolving that package. The user already has `net20ref.nupkg` (which is `Microsoft.NETFramework.ReferenceAssemblies.net20`) staged at the repo root for a local source fix.

## Immediate next step (post-reboot)

1. Confirm `C:\Users\Chris\iCloudDrive` is gone after reboot. If still present (unlikely), one more `Remove-Item -LiteralPath 'C:\Users\Chris\iCloudDrive' -Recurse -Force` should now succeed.
2. **Unblock the PostXING build**: write a `nuget.config` at the repo root that adds a local source pointing at the directory containing `net20ref.nupkg`, plus a `packageSourceMapping` entry that allows `Microsoft.NETFramework.ReferenceAssemblies*` to be resolved from that local source. Without changes to europa's existing global mapping rules. Then re-run `dotnet build src\PostXING\PostXING.csproj` and start working through actual compiler errors.
3. **Hyper-V VM** for Win11 dev environment - the agreed forward path:
   - User has Win11 ISO + MSDN-era Volume Licensing keys ready
   - Hyper-V is already enabled (Docker Desktop's old engine used it)
   - VirtualBox was explicitly ruled out (fights with Hyper-V)
   - VM-on-itachi was explicitly ruled out (M4 Apple Silicon + Win11 ARM = path the user has burned out on)
   - **Critical**: install Tailscale inside the VM so it becomes its own tailnet node, reachable from itachi independent of europa's host
4. **Optional**: Microsoft OpenSSH Server install on europa (`Add-WindowsCapability -Online -Name OpenSSH.Server~~~~0.0.1.0` + `Start-Service sshd` + scope firewall to Tailscale interface). Currently not installed; RDP covers the headless-access need so this is nice-to-have, not blocking.

## Open / loose ends

- The user mentioned eventually wanting to delete local copies of the older VDB / Velociraptor / Velsys productions (65 csprojs targeting v4.0). All on personal GitHub. Not urgent - they want to take a manual inventory because some of those projects use old `[ModuleInitializer]`-style workarounds (the "secret passcodes" the user used pre-C#-9). Don't touch unless explicitly asked.
- The cleanup script (`cleanup-europa.ps1`) had a bug: its generic `/S /silent /quiet /qn /uninstall` flag-soup fallback breaks on SQL Server's `setup.exe`. If reused on a future box, the SQL Server step needs SQL-specific syntax (`/Action=Uninstall /QUIET /IAcceptSQLServerLicenseTerms`) rather than the soup.
- Other cleanup-target Apple stuff that may still be on disk after reboot: `Bonjour`, `Apple Mobile Device Support`, `Apple Software Update`. None were explicitly authorized for removal yet; surface them if the user wants to keep going.

## State pointers

- **Repo (canonical)**: `C:\Users\Chris\Documents\GitHub\PostXING-decompiled`
- **Dockerfile**: `<repo>\Dockerfile` (built once into `postxing-build:latest`, image is gone with Docker)
- **Ref-assembly NuGet (staged)**: `<repo>\net20ref.nupkg` - the `Microsoft.NETFramework.ReferenceAssemblies.net20` package
- **Hand-extracted ref DLLs**: `<repo>\refasm\net20\` (System.Deployment.dll, System.Xml.dll)
- **Decompiled binary staging**: `<repo>\bin-staged\`
- **Cleanup scripts at repo root** (history, not for re-execution as-is): `cleanup-europa.ps1`, `cleanup-fx-old.ps1`

## Memory

Project memory lives at `C:\Users\Chris\.claude\projects\C--Users-Chris-iCloudDrive-Documents-GitHub-PostXING-decompiled\memory\` (keyed to the original iCloud path):

- `user_workstations.md` - itachi (M4 Mac) is the daily driver, europa is a mothballed Win11 Pro box used only for Windows-specific tooling; user is a 25-yr C# vet, MVP in ASP roughly 2004-2007; MSDN-era Volume Licensing keys in reserve; ARM Win11 VMs on itachi are off the table; the StackOverflow-killed-the-listserv-community observation
- `project_postxing_modernization.md` - PostXING v2 is a 2007 WinForms blog editor decompiled via ILSpy on 2026-05-18; soul work, market evaporated when Microsoft open-sourced Windows Live Writer; net20 + NuGet ref assemblies plan, no .NET 2.0 install
- `project_build_environment.md` - Docker path described but jettisoned; this should be rewritten when the VM path is built (project memory is stale on this front - flag for next session)
- `feedback_diagnostic_style.md` - concise, patient, ask before guessing, don't contradict without evidence
- `feedback_no_confident_bullshit.md` - hard rule, do not fabricate plausible-sounding .NET / Windows internals lore; "I don't know, want me to check?" beats pattern completion
- `feedback_powershell_ascii_only.md` - `.ps1` files written via the Write tool must be ASCII-only; PowerShell 5.1 misdecodes BOM-less UTF-8 and parser errors are misleading

Global memory at `C:\Users\Chris\.claude\memory\`:

- `feedback_pre_done_linter_check.md` - code is not "done" until linter/formatter has run with `--fix`; for `.ps1` files this also means a parse check; don't recommend C# formatters without asking
- `feedback_no_uninstall_visual_studio.md` - HARD RULE, never propose uninstalling any Visual Studio version, Build Tools, or VS-installer-managed component on any machine

If the next session opens at this same iCloud-pathed project key (which it will, unless the working directory changes), memory auto-loads. The local `Documents\GitHub` path would not auto-load these memories; the migration `Copy-Item` one-liner is in the prior version of this file but the principle is: copy the entire `C--Users-Chris-iCloudDrive-Documents-GitHub-PostXING-decompiled` directory under `~/.claude/projects/` to a new directory keyed by the local path if you switch.

## Lessons captured during this session (for posterity)

- The user is sharper on .NET history than the model is - challenged confident-sounding claims about Windows-version dependencies and was right both times. The pattern was caught explicitly and saved to memory.
- The Microsoft Store / Settings / Programs-and-Features / Get-AppxPackage / DISM views of "what's installed" are not the same view; an MSI uninstall can succeed while a provisioned UWP package layer survives. iCloud was the worked example.
- Robocopy `/MIR` from an empty source is the path-length-safe way to wipe directories with recursive ASP.NET `_PublishedWebsites` damage.
- Cloud sync providers (iCloud, OneDrive) wire into the Windows CloudFilter mini-filter driver; uninstalling the user-mode service does not release kernel-mode references until reboot.
- The user's diagnostic preference is direct: one tight finding, no walls of commands, ask before guessing, own corrections without recovery theater.
