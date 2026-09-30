cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
echo "=== line 14105 exactly:"
awk 'NR==14105{print NR": "$0}' GodsPVZRuntime1__8.cpp
echo "=== line 14107 exactly:"
awk 'NR==14107{print NR": "$0}' GodsPVZRuntime1__8.cpp
echo "=== where is the V_19 expression?"
grep -n 'V_19 = ((float)(L_7' GodsPVZRuntime1__8.cpp | head -3
echo "=== where is V_20 expression?"
grep -n 'V_20 = ((float)(L_8' GodsPVZRuntime1__8.cpp | head -3
echo ""
echo "=== file mtime / size"
ls -la GodsPVZRuntime1__8.cpp
ls -la ../../../../../*/../*/*/GodsPVZRuntime1__8.cpp 2>/dev/null | head -3
echo ""
echo "=== other copies of this file on disk"
find /workspaces/GodsPVZ-native19 -name 'GodsPVZRuntime1__8.cpp' 2>/dev/null
