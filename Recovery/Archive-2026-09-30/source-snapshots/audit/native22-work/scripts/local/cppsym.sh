cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
echo "=== sample IL2CPP_EXTERN_C headers from GodsPVZRuntime1__1.cpp"
grep -n 'IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR' GodsPVZRuntime1__1.cpp | head -8
echo ""
echo "=== all distinct _m suffixes in that file"
grep -o '_m[0-9A-Fa-f]\+' GodsPVZRuntime1__1.cpp | sort -u | head -10
echo ""
echo "=== a few method signature lines"
grep -n '^\(.*\)\?IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR' GodsPVZRuntime1__1.cpp | head -3
sed -n '110,125p' GodsPVZRuntime1__1.cpp
echo ""
echo "=== for each log with errors show first 3 error lines (full2)"
cd /workspaces/GodsPVZ-native19/.validation/native22-full2
total=0
for f in *.log; do
  n=$(grep -cE '^[A-Za-z0-9_\\./]+\.(cpp|c):[0-9]+:[0-9]+: error: ' "$f" 2>/dev/null || true)
  total=$((total+n))
done
echo "regex-matched error lines: $total"
grep -hE '^[A-Za-z0-9_\\./]+\.(cpp|c):[0-9]+:[0-9]+: error: ' GodsPVZRuntime1__1.cpp.log 2>/dev/null | head -5
