#!/bin/bash
set -x
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary
IL2CPP=/workspaces/GodsPVZ-native19/.validation/native19/replay/canary/il2cpp/build/deploy/il2cpp
W=/workspaces/GodsPVZ-native19/.validation/native22
OUT=$W/regen-baseline
LOG=$W/regen-baseline.log

echo "=== custom il2cpp root dir ==="
ls -d "$CAN/il2cpp" 2>&1
ls "$CAN/il2cpp" 2>&1 | head -20
echo ""

rm -rf "$OUT"; mkdir -p "$OUT/cpp" "$OUT/data" "$OUT/symbols"

cd "$CAN" || exit 1
ARGS=$(cat "$W/regen-args.txt")
export PROJECT_DIR="$CAN"

nohup "$IL2CPP" --convert-to-cpp \
  --custom-il2-cpp-root="$CAN/il2cpp" \
  $ARGS \
  --generatedcppdir="$OUT/cpp" \
  --symbols-folder="$OUT/symbols" \
  --emit-null-checks \
  --enable-array-bounds-check \
  --dotnetprofile=unityaot-macos \
  --data-folder="$OUT/data" \
  > "$LOG" 2>&1 &

echo "started pid=$!"
sleep 10
echo "=== log size: $(stat -c%s "$LOG") ==="
tail -20 "$LOG"
echo "=== cpp files so far: $(ls "$OUT/cpp" 2>/dev/null | wc -l) ==="
