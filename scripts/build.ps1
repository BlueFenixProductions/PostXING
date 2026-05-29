# Release build for CI parity. Called by the `build` package.json script.
# Stamps the version: runs version.mjs to (re)generate .version, then reads it and
# passes the pieces as -p: properties to the full-graph slnx Release build.
# ASCII-only on purpose: Windows PowerShell 5.1 misdecodes BOM-less UTF-8.
$ErrorActionPreference = 'Stop'

node version.mjs
if ($LASTEXITCODE -ne 0) { Write-Host 'version.mjs failed.'; exit $LASTEXITCODE }

# .version is major.minor.patch.build; split into the MSBuild properties.
$full = (Get-Content '.version' -Raw).Trim()
$parts = $full.Split('.')
$display = ($parts[0..2] -join '.')
$appver = $parts[3]

dotnet build solution/PostXING4.slnx -c Release -p:Version=$full -p:ApplicationDisplayVersion=$display -p:ApplicationVersion=$appver
exit $LASTEXITCODE
