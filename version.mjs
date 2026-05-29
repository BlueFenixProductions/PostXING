// Computes the build version and writes it to `.version` at the repo root.
// Run automatically by the build scripts (scripts/build.ps1 and scripts/dev.ps1 -Build);
// also runnable directly via `bun bump` / `npm run bump`.
//
// `.version` holds a single 4-part string:  major.minor.patch.build
//   major = 4                        the 4.x product line
//   minor = current year - 2022      Blue Fenix anniversary year (2026 -> 4)
//   patch = month + zero-padded day  e.g. May 29 -> "529", Jan 5 -> "105"
//   build = monotonic hotfix counter ++ each run (read back from the prior .version)
//
// The build scripts split this into the three MSBuild properties that matter:
//   ApplicationDisplayVersion = major.minor.patch  (AppInfo.VersionString -> About page)
//   ApplicationVersion        = build              (AppInfo.BuildString  -> "(build N)")
//   Version                   = the full 4-part    (assembly / file / informational)
//
// `.version` is gitignored: the counter lives only here so builds don't churn committed
// files, which means it resets to 1 on a fresh clone or if `.version` is deleted.
import fs from 'fs'

const VERSION_FILE = '.version'

const now = new Date()
const major = 4
const minor = now.getFullYear() - 2022
const month = now.getMonth() + 1
const day = String(now.getDate()).padStart(2, '0')
const patch = `${month}${day}`

// hotfix ++ : read the previous build number from .version, else start at 0.
let build = 0
if (fs.existsSync(VERSION_FILE)) {
  const prev = parseInt(fs.readFileSync(VERSION_FILE, 'utf8').trim().split('.')[3] ?? '', 10)
  if (Number.isFinite(prev)) build = prev
}
build += 1

const version = `${major}.${minor}.${patch}.${build}`
fs.writeFileSync(VERSION_FILE, version)
console.log(`Version stamped: ${version}`)
export default version
