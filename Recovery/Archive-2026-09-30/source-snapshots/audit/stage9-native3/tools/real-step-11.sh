set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
E="$OUT/evidence"
UNITY="$(cat "$OUT/unity-path.txt")"
PROJECT="$(cat "$OUT/project-path.txt")"
PLUGIN="$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
set +e
"$UNITY" -batchmode -nographics -quit -projectPath "$PROJECT" -logFile "$E/import-after-native3.log"
RC=$?
set -e
echo "$RC" > "$E/import-after-native3-exit.txt"
[ "$RC" -eq 0 ] || exit 63
if grep -Eiq '\berror CS[0-9]+\b|Scripts have compiler errors|Compilation failed' "$E/import-after-native3.log"; then exit 64; fi
GOT="$(sha256sum "$PLUGIN" | awk '{print $1}')"
echo "$GOT" > "$E/test-dll-after-reimport.sha256"
[ "$GOT" = "$TEST_DLL_SHA256" ] || exit 65

