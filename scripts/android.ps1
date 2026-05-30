# Deploy + launch PostXING on a connected Android device. Called by the `android` package.json
# script. Requires USB debugging on and the device authorized (adb devices shows it).
#
# EmbedAssembliesIntoApk=true builds a self-contained APK instead of using MAUI fast deploy --
# stale fast-deploy state leaves a blank colorPrimary screen on device, so we avoid it. -t:Run
# installs the APK and starts the activity.
#
# ASCII-only on purpose: Windows PowerShell 5.1 misdecodes BOM-less UTF-8.
$ErrorActionPreference = 'Stop'

dotnet build PostXING.App.csproj -t:Run -f net10.0-android -c Debug -p:EmbedAssembliesIntoApk=true
exit $LASTEXITCODE
