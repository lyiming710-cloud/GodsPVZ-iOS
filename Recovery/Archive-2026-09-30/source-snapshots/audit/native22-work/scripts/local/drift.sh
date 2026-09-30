cd /workspaces/GodsPVZ-native19
echo "=== Plant_Awake body line in each copy (expect 2545 if same as scanned) ==="
for f in \
  ".validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput/GodsPVZRuntime1__7.cpp" \
  ".validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp/GodsPVZRuntime1__7.cpp" \
  ".validation/native19/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp/GodsPVZRuntime1__7.cpp" ; do
  if [ -f "$f" ]; then
    n=$(grep -n 'L_1 = Component_GetComponent_TisAnimator' "$f" | head -1 | cut -d: -f1)
    echo "  $n  $f"
  fi
done
echo ""
echo "=== Projectile V_19 line in each copy of __8.cpp (expect 14105) ==="
for f in \
  ".validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput/GodsPVZRuntime1__8.cpp" \
  ".validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp/GodsPVZRuntime1__8.cpp" \
  ".validation/native20/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp/GodsPVZRuntime1__8.cpp" \
  ".validation/native19/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp/GodsPVZRuntime1__8.cpp" ; do
  if [ -f "$f" ]; then
    n=$(grep -n 'V_19 = ((float)(L_7' "$f" | head -1 | cut -d: -f1)
    tot=$(wc -l < "$f")
    echo "  V_19@$n  total=$tot  $f"
  fi
done
echo ""
echo "=== which dir does full2 scan reference? look for any invocation record"
ls -la .validation/native22-full2/*.log | head -3
grep -m1 'GodsPVZ-native19' .validation/native22-full2/GodsPVZRuntime1__8.cpp.log | head -2
echo "=== does any full2 log record absolute paths?"
head -3 .validation/native22-full2/GodsPVZRuntime1__8.cpp.log
