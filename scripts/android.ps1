# Deploy + launch PostXING on a connected Android device. Called by the `android` package.json
# script. Requires USB debugging on and the device authorized (adb devices shows it).
#
# Version-stamps like dev.ps1 -Build so the About page shows the real version (a bare build would
# leave the csproj defaults 4.0.0 / build 1). On Android, ApplicationVersion IS the versionCode and
# must be a monotonic integer or a reinstall fails as a downgrade. The date-based .version parts
# give that without churn: versionCode = minor*10,000,000 + patch*1,000 + build
# (e.g. 4.4.530.19 -> 40,530,019), always increasing and well under Android's 2^31 ceiling.
#
# EmbedAssembliesIntoApk=true builds a self-contained APK instead of using MAUI fast deploy --
# stale fast-deploy state leaves a blank colorPrimary screen on device, so we avoid it. -t:Run
# installs the APK and starts the activity.
#
# ASCII-only on purpose: Windows PowerShell 5.1 misdecodes BOM-less UTF-8.
$ErrorActionPreference = 'Stop'

node version.mjs
if ($LASTEXITCODE -ne 0) { Write-Host 'version.mjs failed - not deploying.'; exit $LASTEXITCODE }

# .version is major.minor.patch.build; split into the MSBuild properties.
$full = (Get-Content '.version' -Raw).Trim()
$parts = $full.Split('.')
$display = ($parts[0..2] -join '.')
$versionCode = ([int]$parts[1] * 10000000) + ([int]$parts[2] * 1000) + [int]$parts[3]

dotnet build PostXING.App.csproj -t:Run -f net10.0-android -c Debug -p:EmbedAssembliesIntoApk=true -p:Version=$full -p:ApplicationDisplayVersion=$display -p:ApplicationVersion=$versionCode
exit $LASTEXITCODE
