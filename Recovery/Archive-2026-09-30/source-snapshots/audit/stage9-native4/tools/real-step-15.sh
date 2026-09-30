set -uo pipefail
OUT="$RUNNER_TEMP/stage9-native4-ios"
E="$OUT/evidence"
if [ -f "$OUT/seat-acquired.flag" ] && [ -f "$OUT/unity-path.txt" ]; then
  UNITY="$(cat "$OUT/unity-path.txt")"
  CLIENT="$(dirname "$UNITY")/Data/Resources/Licensing/Client/Unity.Licensing.Client"
  set +e
  "$CLIENT" --return-ulf > "$E/license-return.log" 2>&1
  RC=$?
  set -e
  echo "$RC" > "$E/license-return-exit.txt"
fi

