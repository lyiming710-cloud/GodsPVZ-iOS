echo "=== CodeGenModule file in il2cppOutput"
ls /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput | grep -i -E 'module|register|metadata' | head -20
echo ""
echo "=== all cpp basenames (non-shard)"
ls /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput/*.cpp | xargs -n1 basename | grep -v -E '^(GodsPVZRuntime1|Generics|GenericMethods)' | head -40
echo ""
echo "=== prior rounds: any token<->cpp-symbol map?"
find /workspaces/GodsPVZ-native19 -maxdepth 5 -name '*.json' 2>/dev/null | xargs grep -l 'm[0-9A-F]\{8,\}' 2>/dev/null | head -10
echo ""
echo "=== look for scripts that map symbol -> token"
grep -rl 'method pointers\|methodPointers\|token_of\|sym2tok\|symbol.*token' /workspaces/GodsPVZ-native19/scripts /workspaces/GodsPVZ-native19/.validation --include=*.py 2>/dev/null | head -10
echo ""
echo "=== any GODSPVZ evidence with 'RID' and 'm' mapping"
grep -rn "0x06000000 + \|0x06000000+" /workspaces/GodsPVZ-native19/scripts /workspaces/GodsPVZ-native19/Tools 2>/dev/null | head -10
