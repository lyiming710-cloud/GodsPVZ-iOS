set -u
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary
W=/workspaces/GodsPVZ-native19/.validation/native22
echo "=== alive ==="
date
echo "=== current input dll ==="
sha256sum $CAN/Library/Bee/artifacts/iOS/ManagedStripped/GodsPVZRuntime1.dll
echo "=== W tree (depth 2) ==="
find $W -maxdepth 2 | sort | head -60
echo "=== W/work ==="
ls -la $W/work 2>/dev/null | head -40
echo "=== regen-batch1 cpp count ==="
ls $W/regen-batch1/cpp/*.cpp 2>/dev/null | wc -l
echo "=== retest clang logs ==="
ls $W/retest/clang/*.log 2>/dev/null | wc -l
echo "=== out ==="
ls -la $W/out | head -20
