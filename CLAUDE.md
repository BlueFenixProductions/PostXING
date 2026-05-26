# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

A recovered build of **PostXING v2**, a 2007 WinForms blog editor, reconstructed from compiled DLLs via ILSpy decompilation. The source under `src/` is *decompiler output that has been coaxed back into compiling*, not the original source the author shipped. Treat odd-looking constructs (synthesized field names, helper classes named `<>c__DisplayClass…`, unused locals, redundant casts) as decompiler residue rather than design intent.

The end goal is a project that compiles, runs, and can then be incrementally cleaned up. See `HANDOFF.md` for the most recent session state — it is the canonical pickup document and is updated each session.

## Build

The repo is SDK-style csprojs targeting `net20` (.NET Framework 2.0). **No .NET 2.0 runtime is required on the build host** — reference assemblies are supplied via the `Microsoft.NETFramework.ReferenceAssemblies` meta-package (auto-pulled by the SDK for any net20 TFM) plus hand-extracted DLLs under `refasm/net20/` for assemblies that NuGet package does not cover (notably `System.Deployment`, `System.Web`, `System.Design`, `System.Xml`).

There is no solution file. Build the entry-point project; the others get pulled in by reference:

```powershell
dotnet build src\PostXING\PostXING.csproj
```

### The NuGet restore quirk

On hosts with restrictive `packageSourceMapping` enabled globally, restore fails with `NU1100: Unable to resolve 'Microsoft.NETFramework.ReferenceAssemblies'`. The repo-root `nuget.config` works around this by registering the repo root itself as a local NuGet source and claiming the `Microsoft.NETFramework.ReferenceAssemblies*` pattern. **`net20ref.nupkg` (the `Microsoft.NETFramework.ReferenceAssemblies.net20` package) must exist at the repo root** for restore to succeed — it is gitignored as a refetchable input, so a fresh clone needs it dropped back in before the first build.

### Reference assemblies are gitignored

`refasm/` is in `.gitignore` as "re-fetchable decompiler inputs". A fresh clone will not build until that directory is repopulated with .NET 2.0 reference DLLs (hand-extracted from a `Microsoft.NETFramework.ReferenceAssemblies.net20` install or from a machine with the .NET 2.0 SDK targeting pack).

### Staged binaries

`bin-staged/` holds the original compiled DLLs that decompilation came from. Inter-project references in the csprojs point at these DLLs (e.g. `PostXING.csproj` references `bin-staged/PostXING.Components.dll`) rather than at the sibling projects directly. This was the bootstrapping move: it lets each project compile against a *known-good* binary of its dependencies while their source is still being made to compile. Once everything compiles cleanly, this should migrate to `<ProjectReference>` and `bin-staged/` can go away — but until then, **do not delete `bin-staged/`** and be aware that fixing a compile error in, say, `PostXING.Components` does not propagate to consumers until you rebuild it and either let the next build pick up the new output or update the HintPath.

## Project layout and dependency graph

Six csproj files under `src/`, all `net20`, all WinForms (`Microsoft.NET.Sdk.WindowsDesktop`) except `PostXING.Components` which is plain `Microsoft.NET.Sdk`:

- **`PostXING.Components`** — leaf. References `System.Web`, `System.Xml`. Plain class library.
- **`PostXING.Controls`** — WinForms controls. References Components (via bin-staged), plus third-party `TabControlEX`, `CodeHtmler`, `Genghis`, `System.Design`. Embeds `.bmp` resources (MozBar, navigation icons) with explicit `LogicalName` mappings — these matter for runtime resource resolution; don't rename without updating both sides.
- **`PostXING.Extensibility`** — plugin SPI. References Components + Controls.
- **`PostXING.MetaBlogProvider`** — XML-RPC blog backend. References Components, Controls, Extensibility, plus `CookComputing.XmlRpc`.
- **`PostXING`** — the WinForms app (`OutputType=WinExe`, `app.ico`). References all four libraries above plus `Genghis`, `edtftpnet-1.1.8`, `blogExtension`, `System.Deployment`, `System.Xml`. Empty `<RootNamespace />` — folder names under `src/PostXING/` (`PostXING.Forms`, `PostXING.Dialogs`, `PostXING.NavigationPages`, etc.) are organizational; namespaces come from the source files themselves.
- **`blogExtension`** — small helper. Currently declares `<TargetFramework>net10</TargetFramework>` which is almost certainly a decompilation artifact (should likely be `net20`); flag this if you touch it.

Third-party dependencies live as opaque DLLs in `bin-staged/` (`Genghis`, `CodeHtmler`, `CookComputing.XmlRpc`, `TabControlEX`, `edtftpnet-1.1.8`). These were not decompiled and are referenced as-is.

## Repo-specific conventions and gotchas

- **All csprojs set `LangVersion=14.0`** despite targeting `net20`. This is intentional — it lets the decompiled source use modern C# syntactic sugar (expression-bodied members, `out var`, etc.) without changing the TFM. Don't "fix" this by lowering `LangVersion` to match the framework.
- **`GenerateAssemblyInfo=False`** is set everywhere because the decompiled source already contains `[assembly: …]` attributes; letting the SDK generate them would cause duplicates.
- **`CheckForOverflowUnderflow=False`** is set everywhere to match the original build's checked-arithmetic behavior.
- **`AllowUnsafeBlocks=True`** is set everywhere even though most projects don't use `unsafe` — decompiler precaution.
- `.resx` files at `src/PostXING/PostXING.Forms.EditorForm.resx` (etc.) live alongside their containing folder rather than inside it. The MSBuild defaults pick them up by filename matching; don't restructure without checking that the resource discovery still works.

## Hard rules (from prior session memory)

- **Never propose uninstalling any Visual Studio install, Build Tools install, or VS-installer-managed component** on any machine. Hard rule. The .NET Framework Dev/Targeting Packs older than 4.8 are an exception that was already authorized and executed; do not generalize from that.
- **`.ps1` files written via the Write tool must be ASCII-only.** Windows PowerShell 5.1 misdecodes BOM-less UTF-8 and the resulting parser errors are misleading. If the script needs a non-ASCII character, use a `[char]` escape.
- **Do not fabricate confident-sounding .NET / Windows internals lore.** The user has 25 years of C# and an ASP MVP from ~2004-2007; they will catch invented dependency claims. "I don't know, want me to check?" is the right move.
- **Code is not "done" until the linter/formatter has run with `--fix`.** For `.ps1`, that also means a parse check. Don't recommend C# formatters (dotnet format, etc.) without asking first — this codebase's formatting is decompiler-shaped and a blanket reformat would create enormous noise.

## Files at the repo root worth knowing about

- `HANDOFF.md` — session-handoff log. Read first if returning to this project; update at end of session.
- `Dockerfile` — record of a jettisoned Windows-container build path. Image no longer exists; the file is kept for history. The forward path is a Hyper-V Win11 VM, not Docker.
- `cleanup-europa.ps1`, `cleanup-fx-old.ps1` — one-shot host cleanup scripts already executed against the build host. **History, not for re-execution as-is.** `cleanup-europa.ps1` has a known bug where the generic uninstall-flag-soup fallback breaks on SQL Server's `setup.exe`.
- `ilspy-*.log` — empty log files from the original ILSpy decompilation pass. Gitignored.
