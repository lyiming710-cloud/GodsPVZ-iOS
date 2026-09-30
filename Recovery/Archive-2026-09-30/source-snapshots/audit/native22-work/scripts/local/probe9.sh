cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
f=GodsPVZRuntime1_CodeGen.c
echo "=== $f ($(wc -l < $f) lines, $(wc -c < $f) bytes)"
echo "=== first 30 lines"
head -30 "$f"
echo ""
echo "=== grep counts"
grep -c 'm[0-9A-F]\{8,\}' "$f"
echo ""
echo "=== lines that look like pointer array entries: first 5"
grep -n 'm[0-9A-F]\{8,\}' "$f" | head -5
echo ""
echo "=== last 3"
grep -n 'm[0-9A-F]\{8,\}' "$f" | tail -3
echo ""
echo "=== structure keywords"
grep -n 'MethodInfo\|s_methodPointers\|methodMetadata\|CodeGenModule\|rgctx\|Il2CppGenericMethodPointer' "$f" | head -20
