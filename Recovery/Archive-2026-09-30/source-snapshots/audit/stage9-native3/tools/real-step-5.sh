set -euo pipefail
OUT="$RUNNER_TEMP/stage9-native3-ios"
E="$OUT/evidence"
PARTS="$OUT/editor-parts"
ROOT="$OUT/editor-root"
mkdir -p "$ROOT"
mapfile -t FILES < <(find "$PARTS" -maxdepth 1 -type f -name 'Unity-China-2022.3.44f1c1.tar.xz.part-*' | sort)
[ "${#FILES[@]}" = '15' ] || exit 51
SHA="$({ for part in "${FILES[@]}"; do cat "$part"; done; } | sha256sum | awk '{print $1}')"
echo "$SHA" > "$E/editor-sha256.txt"
[ "$SHA" = "$EDITOR_SHA256" ] || exit 52
{
  for part in "${FILES[@]}"; do cat "$part"; rm -f "$part"; done
} | tar -xJf - -C "$ROOT"
rmdir "$PARTS" 2>/dev/null || true
UNITY="$ROOT/Editor/Unity"
chmod +x "$UNITY"
V="$($UNITY -version 2>&1 | grep -Eo '2022\.3\.44f1c1|2022\.3\.44f1' | head -n1 || true)"
echo "$V" > "$E/editor-version-before-ios-module.txt"
[ "$V" = "$UNITY_VERSION" ] || exit 53
printf '%s\n' "$UNITY" > "$OUT/unity-path.txt"

