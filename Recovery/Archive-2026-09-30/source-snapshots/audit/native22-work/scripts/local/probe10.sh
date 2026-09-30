cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
echo "=== s_methodPointers array head"
sed -n '2282,2292p' GodsPVZRuntime1_CodeGen.c
echo ""
echo "=== array tail"
awk 'NR>=2282 && /^};/{print NR": "$0; exit}' GodsPVZRuntime1_CodeGen.c
echo ""
echo "=== count lines between array start and };"
awk 'NR>2282 && /^}/{print NR; exit}' GodsPVZRuntime1_CodeGen.c
