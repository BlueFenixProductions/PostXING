# Dev launcher for PostXING. Called by the `dev` / `dev:build` package.json scripts.
#   (no switch)  -> kill stragglers, launch the already-built app (fast, no build)
#   -Build       -> kill stragglers, incremental build, then launch
# ASCII-only on purpose: Windows PowerShell 5.1 misdecodes BOM-less UTF-8.
param([switch]$Build)

$ErrorActionPreference = 'Stop'
$exe = 'bin/Debug/net10.0-windows10.0.19041.0/win-x64/PostXING.App.exe'

# A leftover instance locks the apphost exe/dlls and breaks the next build/launch.
Get-Process PostXING.App -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue

if ($Build) {
    dotnet build -v q
    if ($LASTEXITCODE -ne 0) { Write-Host 'Build failed - not launching.'; exit $LASTEXITCODE }
}

if (Test-Path $exe) {
    & $exe
} else {
    Write-Host 'No build found. Run: bun dev:build'
    exit 1
}
