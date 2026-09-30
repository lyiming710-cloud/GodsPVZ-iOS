set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native4-ios"
E="$OUT/evidence"
UNITY="$(cat "$OUT/unity-path.txt")"
PROJECT="$(cat "$OUT/project-path.txt")"
PLUGIN="$PROJECT/Assets/Plugins/Assembly-CSharp.dll"
[ "$(sha256sum "$PLUGIN" | awk '{print $1}')" = "$TEST_DLL_SHA256" ] || exit 68
set +e
xvfb-run -a -s '-screen 0 1280x720x24' "$UNITY" -batchmode -quit -force-glcore -buildTarget iOS -projectPath "$PROJECT" -executeMethod GodsPVZIOSBuild.BuildIOS -logFile "$E/ios-export.log" > "$E/ios-export.stdout" 2> "$E/ios-export.stderr"
RC=$?
set -e
echo "$RC" > "$E/ios-export-exit.txt"
[ "$RC" -eq 0 ] || exit 69
test -d "$OUT/xcode"

