#!/bin/bash
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp
XC=/workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
RG=/workspaces/GodsPVZ-native19/.validation/native22/regen-baseline/cpp

echo "=== counts (cpp only) ==="
echo "canary=$(ls $CAN/*.cpp 2>/dev/null | wc -l)  xcode=$(ls $XC/*.cpp 2>/dev/null | wc -l)  regen=$(ls $RG/*.cpp 2>/dev/null | wc -l)"
echo ""

echo "=== regen vs canary ==="
same=0; diff=0; missing=0
for f in $(ls "$RG"/*.cpp); do
  b=$(basename "$f")
  if [ -f "$CAN/$b" ]; then
    if cmp -s "$f" "$CAN/$b"; then same=$((same+1)); else diff=$((diff+1)); echo "  DIFF $b"; fi
  else missing=$((missing+1)); echo "  MISSING_IN_CANARY $b"; fi
done
echo "same=$same diff=$diff missing=$missing"
echo ""

echo "=== regen vs xcode (first 12 diffs) ==="
s=0; d=0; m=0; n=0
for f in $(ls "$RG"/*.cpp); do
  b=$(basename "$f")
  if [ -f "$XC/$b" ]; then
    if cmp -s "$f" "$XC/$b"; then s=$((s+1)); else d=$((d+1)); n=$((n+1)); [ $n -le 12 ] && echo "  DIFF $b"; fi
  else m=$((m+1)); [ $m -le 5 ] && echo "  MISSING_IN_XCODE $b"; fi
done
echo "same=$s diff=$d missing_in_xcode=$m"
echo ""

echo "=== line counts for the disputed file ==="
for t in "$RG" "$CAN" "$XC"; do
  f="$t/GodsPVZRuntime1__8.cpp"
  if [ -f "$f" ]; then
    echo "$(basename $(dirname $f))-tree: $(wc -l < "$f") lines, V_19 line = $(grep -n 'V_19 = ((float)(L_7&((intptr_t)0)))' "$f" | head -1 | cut -d: -f1)"
  fi
done
