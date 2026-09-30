cd /workspaces/GodsPVZ-native19
A=.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp
B=.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
echo "files canary=$(ls $A/*.cpp | wc -l)  xcode=$(ls $B/*.cpp | wc -l)"
echo ""
echo "=== sha256 differences among common *.cpp files (limit 20) ==="
cd "$A"
for f in *.cpp; do
  if [ -f "/workspaces/GodsPVZ-native19/$B/$f" ]; then
    h1=$(sha256sum "$f" | cut -c1-16)
    h2=$(sha256sum "/workspaces/GodsPVZ-native19/$B/$f" | cut -c1-16)
    if [ "$h1" != "$h2" ]; then echo "DIFF $f  $h1 $h2"; fi
  else
    echo "MISSING_IN_XCODE $f"
  fi
done | head -20
echo ""
echo "=== total differing: ==="
cd /workspaces/GodsPVZ-native19
cnt=0
cd "$A"
for f in *.cpp; do
  if [ -f "/workspaces/GodsPVZ-native19/$B/$f" ]; then
    h1=$(sha256sum "$f" | cut -c1-16)
    h2=$(sha256sum "/workspaces/GodsPVZ-native19/$B/$f" | cut -c1-16)
    [ "$h1" != "$h2" ] && cnt=$((cnt+1))
  fi
done
echo "differing common cpp files: $cnt"
