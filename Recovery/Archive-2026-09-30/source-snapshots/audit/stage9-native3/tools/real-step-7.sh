set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
E="$OUT/evidence"
UNITY="$(cat "$OUT/unity-path.txt")"
CLIENT="$(dirname "$UNITY")/Data/Resources/Licensing/Client/Unity.Licensing.Client"
set +e
"$CLIENT" --activate-all --include-personal --username "$UNITY_EMAIL" --password "$UNITY_PASSWORD" > "$E/license-activate.log" 2>&1
RC=$?
set -e
echo "$RC" > "$E/license-activate-exit.txt"
[ "$RC" -eq 0 ] || exit 56
touch "$OUT/seat-acquired.flag"

