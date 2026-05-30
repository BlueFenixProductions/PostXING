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
    node version.mjs
    if ($LASTEXITCODE -ne 0) { Write-Host 'version.mjs failed - not launching.'; exit $LASTEXITCODE }
    # .version is major.minor.patch.build; split into the MSBuild properties.
    $full = (Get-Content '.version' -Raw).Trim()
    $parts = $full.Split('.')
    $display = ($parts[0..2] -join '.')
    $appver = $parts[3]
    # Windows-only for the fast dev loop: the App now multi-targets (...;net10.0-android), and a
    # bare build would compile both heads. Use scripts/android.ps1 (bun android) for the phone.
    dotnet build -f net10.0-windows10.0.19041.0 -v q -p:Version=$full -p:ApplicationDisplayVersion=$display -p:ApplicationVersion=$appver
    if ($LASTEXITCODE -ne 0) { Write-Host 'Build failed - not launching.'; exit $LASTEXITCODE }
}

if (Test-Path $exe) {
    & $exe
} else {
    Write-Host 'No build found. Run: bun dev:build'
    exit 1
}
