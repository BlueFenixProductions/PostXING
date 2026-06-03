# Deploy + launch PostXING on a connected Android device. Called by the `android` /
# `android:clean` package.json scripts (android:clean passes -Clean).
#
#   bun android        -> preflight, install the embedded APK OVER the existing app (so in-app
#                         settings / the SecureStorage PAT / the SAF folder grant survive), launch
#   bun android:clean  -> uninstall first (RESETS that in-app data) + wipe obj/bin for a from-scratch
#                         APK; use if a deploy ever still shows the blank PhoenixBlue screen
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
# Stability + settings: a stale install that previously used fast deploy can leave a .__override__
# dir that renders a blank colorPrimary screen. We build embedded (no fast deploy) and install OVER
# the existing app, which stays blank-screen-free AND preserves app data -- so in-app settings, the
# SecureStorage PAT, and the SAF folder grant survive a redeploy (a plain `adb uninstall` would wipe
# all of that, which is why settings used to reset on every deploy). `bun android:clean` is the
# escape hatch: it uninstalls (resetting that data) + wipes obj/bin for a from-scratch APK, for the
# rare case a blank screen still appears. We also resolve adb strictly (no silent skip), require
# exactly one authorized device, pin every adb call AND the MAUI deploy to that serial via
# ANDROID_SERIAL, and run a post-deploy health check (process alive + crash vs blank).
#
# ASCII-only on purpose: Windows PowerShell 5.1 misdecodes BOM-less UTF-8.
param([switch]$Clean)

$ErrorActionPreference = 'Stop'
$pkg = 'net.bluefenix.postxing'

# --- Resolve adb (required). A missing adb is exactly what used to make the pre-uninstall silently
#     no-op and deploy over stale state, so we fail loudly instead of warning and continuing. ---
$adb = @(
    "$env:ANDROID_HOME\platform-tools\adb.exe",
    "$env:ANDROID_SDK_ROOT\platform-tools\adb.exe",
    "${env:ProgramFiles(x86)}\Android\android-sdk\platform-tools\adb.exe",
    "$env:ProgramFiles\Android\android-sdk\platform-tools\adb.exe",
    "$env:LOCALAPPDATA\Android\Sdk\platform-tools\adb.exe"
) | Where-Object { $_ -and (Test-Path $_) } | Select-Object -First 1
if (-not $adb) {
    $onPath = Get-Command adb -ErrorAction SilentlyContinue
    if ($onPath) { $adb = $onPath.Source }
}
if (-not $adb) {
    Write-Host 'ERROR: adb.exe not found. Looked under ANDROID_HOME, ANDROID_SDK_ROOT,'
    Write-Host '       Program Files (x86)\Android\android-sdk, Program Files\Android\android-sdk,'
    Write-Host '       LOCALAPPDATA\Android\Sdk, and PATH. Install the Android SDK platform-tools'
    Write-Host '       or set ANDROID_HOME, then retry.'
    exit 1
}

# Script-scope serial; Invoke-Adb (defined below) reads it and pins every call with -s.
$serial = $null

# Run adb without letting a non-zero exit or native-stderr abort the script. In Windows PowerShell
# 5.1, `2>&1` on a native exe wraps stderr lines as NativeCommandError which, under -Stop, can
# throw; we drop to -Continue around the call, capture combined output as text for diagnostics, and
# throw ourselves only on a real failure (unless -AllowFail).
function Invoke-Adb {
    # Pass adb args as an explicit array, e.g. Invoke-Adb @('logcat','-d'). Do NOT use
    # ValueFromRemainingArguments here: PowerShell binds dash-prefixed items (-d, -p, -f) as
    # parameters and silently drops them, which turned `logcat -d` (dump + exit) into `logcat`
    # (stream forever) and hung the script.
    param([string[]]$AdbArgs, [switch]$AllowFail)
    $prev = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    if ($serial) { $out = & $adb -s $serial @AdbArgs 2>&1 | Out-String }
    else         { $out = & $adb @AdbArgs 2>&1 | Out-String }
    $code = $LASTEXITCODE
    $ErrorActionPreference = $prev
    if ($code -ne 0 -and -not $AllowFail) {
        throw "adb $($AdbArgs -join ' ') failed (exit $code): $($out.Trim())"
    }
    return $out
}

# --- Preflight: require exactly one authorized device, then pin to it. ---
$prev = $ErrorActionPreference
$ErrorActionPreference = 'Continue'
$devOut = & $adb devices 2>&1 | Out-String
$ErrorActionPreference = $prev
$authorized = @()
$other = @()
foreach ($line in ($devOut -split "`n")) {
    $t = $line.Trim()
    if (-not $t) { continue }
    if ($t -match '^List of devices') { continue }
    if ($t -match '^\*') { continue }   # "* daemon started ..." chatter
    $cols = $t -split "\s+"
    if ($cols.Count -ge 2) {
        if ($cols[1] -eq 'device') { $authorized += $cols[0] }
        else { $other += "$($cols[0]) ($($cols[1]))" }
    }
}
if ($authorized.Count -eq 0) {
    Write-Host 'ERROR: no authorized Android device found.'
    if ($other.Count -gt 0) { Write-Host ('       Detected but not usable: ' + ($other -join ', ')) }
    Write-Host '       Plug in the Pixel 7, enable USB debugging, and accept the "Allow USB debugging"'
    Write-Host '       prompt on the phone. Verify with: adb devices'
    exit 1
}
if ($authorized.Count -gt 1) {
    Write-Host ('ERROR: more than one authorized device attached: ' + ($authorized -join ', '))
    Write-Host '       Disconnect the extras so the deploy targets exactly one device.'
    exit 1
}
$serial = $authorized[0]
$env:ANDROID_SERIAL = $serial   # adb AND the adb calls MAUI's -t:Run makes both honor this
Write-Host "Target device: $serial"

# Always stop the running app so the install + relaunch is clean. force-stop only kills the process;
# it does NOT touch app data, so settings survive.
Invoke-Adb @('shell','am','force-stop',$pkg) -AllowFail | Out-Null

if ($Clean) {
    # --- Clean redeploy (escape hatch). Uninstall wipes app data on purpose -- it clears any stale
    #     fast-deploy override state AND resets in-app settings / the PAT / the SAF folder grant --
    #     then wipe obj/bin so the APK is rebuilt from scratch. Use this if a blank screen appears. ---
    $listed = Invoke-Adb @('shell','pm','list','packages',$pkg) -AllowFail
    if ($listed -match [regex]::Escape("package:$pkg")) {
        Write-Host "Uninstalling existing $pkg (this RESETS in-app settings / PAT / folder grant) ..."
        Invoke-Adb @('uninstall',$pkg) | Out-Null
        $listed2 = Invoke-Adb @('shell','pm','list','packages',$pkg) -AllowFail
        if ($listed2 -match [regex]::Escape("package:$pkg")) {
            Write-Host "ERROR: $pkg is still installed after uninstall; aborting to avoid deploying over"
            Write-Host '       stale state. Disconnect/reconnect the device and retry.'
            exit 1
        }
        Write-Host 'Previous install removed.'
    } else {
        Write-Host 'No previous install present.'
    }
    Write-Host 'Clean: removing obj/bin for net10.0-android ...'
    foreach ($d in @('obj\Debug\net10.0-android', 'bin\Debug\net10.0-android')) {
        if (Test-Path $d) { Remove-Item -Recurse -Force $d }
    }
} else {
    # --- Default redeploy. Install the embedded APK OVER the existing app: no uninstall, so app data
    #     survives. -t:Run's `adb install -r` reinstalls in place (versionCode is monotonic, so it's
    #     an upgrade, never a downgrade), and the embedded APK creates no fast-deploy override state,
    #     so this stays blank-screen-free. Settings / PAT / SAF grant carry over the redeploy. ---
    Write-Host 'Installing over the existing app (settings preserved). bun android:clean for a full reset.'
}

node version.mjs
if ($LASTEXITCODE -ne 0) { Write-Host 'version.mjs failed - not deploying.'; exit $LASTEXITCODE }

# .version is major.minor.patch.build; split into the MSBuild properties.
$full = (Get-Content '.version' -Raw).Trim()
$parts = $full.Split('.')
$display = ($parts[0..2] -join '.')
$versionCode = ([int]$parts[1] * 10000000) + ([int]$parts[2] * 1000) + [int]$parts[3]

Invoke-Adb @('logcat','-c') -AllowFail | Out-Null   # fresh buffer so the post-deploy crash scan reflects THIS run
Write-Host "Building + deploying $full (versionCode $versionCode) to $serial ..."
dotnet build PostXING.App.csproj -t:Run -f net10.0-android -c Debug -p:EmbedAssembliesIntoApk=true -p:Version=$full -p:ApplicationDisplayVersion=$display -p:ApplicationVersion=$versionCode
$buildExit = $LASTEXITCODE
if ($buildExit -ne 0) { Write-Host "Build/deploy failed (exit $buildExit)."; exit $buildExit }

# --- Post-deploy health check: confirm the app is actually up; flag a crash vs a stale blank. ---
Start-Sleep -Seconds 3
$appPid = (Invoke-Adb @('shell','pidof',$pkg) -AllowFail).Trim()
# Scope the crash scan to our process so we don't trip on unrelated / historical AndroidRuntime
# lines elsewhere in the device's shared log buffer (it spans every app and survives reboots).
if ($appPid) {
    $firstPid = ($appPid -split '\s+')[0]
    $log = Invoke-Adb @('logcat','-d','--pid',$firstPid) -AllowFail
} else {
    $log = Invoke-Adb @('logcat','-d') -AllowFail
}
$crash = $log -match 'FATAL EXCEPTION'
$shot = Join-Path $env:TEMP 'px_deploy.png'
Invoke-Adb @('shell','screencap','-p','/sdcard/px_deploy.png') -AllowFail | Out-Null
Invoke-Adb @('pull','/sdcard/px_deploy.png',$shot) -AllowFail | Out-Null
Invoke-Adb @('shell','rm','-f','/sdcard/px_deploy.png') -AllowFail | Out-Null

Write-Host ''
if ($appPid) { Write-Host "OK: $pkg is running (pid $appPid)." }
else         { Write-Host "WARNING: $pkg does not appear to be running." }
if ($crash)  { Write-Host 'WARNING: a FATAL EXCEPTION / AndroidRuntime entry is in logcat - possible startup crash.' }
if ((-not $appPid) -or $crash) {
    Write-Host 'If the screen is blank or the app crashed:'
    Write-Host '  - from-scratch redeploy:   bun android:clean'
    Write-Host '  - editor bridge log:       adb logcat -s PXBRIDGE'
}
if (Test-Path $shot) { Write-Host "Screenshot saved: $shot" }
exit 0
