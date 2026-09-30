#!/bin/bash
W=/workspaces/GodsPVZ-native19/.validation/native22
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary
OUT=$W/regen-baseline
echo "=== process alive? ==="
pgrep -a -f "il2cpp --convert-to-cpp" | head -3
echo "(none above means finished)"
echo ""
echo "=== log (tail 15) ==="
tail -15 "$W/regen-baseline.log"
echo ""
echo "=== generated counts ==="
echo "cpp: $(ls "$OUT/cpp"/*.cpp 2>/dev/null | wc -l)  total files: $(ls "$OUT/cpp" 2>/dev/null | wc -l)"
echo "canary cpp: $(ls "$CAN/Library/Bee/artifacts/iOS/il2cppOutput/cpp"/*.cpp 2>/dev/null | wc -l)"
echo ""
echo "=== sizes ==="
du -sh "$OUT/cpp" 2>/dev/null
grep -c . "$W/regen-baseline.log" 2>/dev/null && echo "bytes: $(stat -c%s "$W/regen-baseline.log")"
