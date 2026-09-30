set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
E="$OUT/evidence"
ROOT="$OUT/editor-root"
MOD="$OUT/ios-module/UnitySetup-iOS-Support-for-Editor-2022.3.44f1.tar.xz"
test -f "$MOD"
GOT="$(sha256sum "$MOD" | awk '{print $1}')"
echo "$GOT" > "$E/ios-module-sha256.txt"
[ "$GOT" = "$IOS_MODULE_SHA256" ] || exit 54
xz -t "$MOD"
tar -xJf "$MOD" -C "$ROOT"
UNITY="$(cat "$OUT/unity-path.txt")"
V="$($UNITY -version 2>&1 | grep -Eo '2022\.3\.44f1c1|2022\.3\.44f1' | head -n1 || true)"
[ "$V" = "$UNITY_VERSION" ] || exit 55
rm -rf "$OUT/ios-module"

