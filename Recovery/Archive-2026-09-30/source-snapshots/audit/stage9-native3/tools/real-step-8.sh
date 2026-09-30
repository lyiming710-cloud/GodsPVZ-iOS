set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
E="$OUT/evidence"
UNITY="$(cat "$OUT/unity-path.txt")"
PROJECT="$(cat "$OUT/project-path.txt")"
set +e
"$UNITY" -batchmode -nographics -quit -projectPath "$PROJECT" -logFile "$E/import-before-migration.log"
RC=$?
set -e
echo "$RC" > "$E/import-before-migration-exit.txt"
[ "$RC" -eq 0 ] || exit 57
if grep -Eiq '\berror CS[0-9]+\b|Scripts have compiler errors|Compilation failed' "$E/import-before-migration.log"; then exit 58; fi

