set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native4-ios"
E="$OUT/evidence"
UNITY="$(cat "$OUT/unity-path.txt")"
PROJECT="$(cat "$OUT/project-path.txt")"
"$UNITY" -batchmode -nographics -quit -projectPath "$PROJECT" -executeMethod GodsPVZPackageReferenceMigrator.RunBatch -logFile "$E/package-migration.log"
MARKER="$PROJECT/Library/GodsPVZ.package-reference-migration.done"
PLUGIN="$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
test -f "$MARKER"
grep -q '^resolved=67/67$' "$MARKER"
GOT="$(sha256sum "$PLUGIN" | awk '{print $1}')"
echo "b001_workcopy_sha256=$GOT" | tee "$E/b001-workcopy.txt"
[ "$GOT" = "$B001_SHA256" ] || exit 59

