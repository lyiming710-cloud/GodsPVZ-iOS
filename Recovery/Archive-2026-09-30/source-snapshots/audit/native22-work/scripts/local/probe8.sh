cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
echo "=== Assembly-CSharp related files"
ls | grep -i assembly | head
echo ""
for f in Assembly-CSharp_CodeGen.c Assembly-CSharp.cpp; do
  if [ -f "$f" ]; then echo "--- $f ($(wc -l < $f) lines)"; fi
done
echo ""
echo "=== look for s_methodPointers definition referencing Assembly-CSharp"
grep -ln 's_methodPointers' *.c 2>/dev/null | head
echo ""
if [ -f Assembly-CSharp_CodeGen.c ]; then
  echo "=== head of Assembly-CSharp_CodeGen.c"
  head -40 Assembly-CSharp_CodeGen.c
  echo "=== count of entries in s_methodPointers"
  awk '/static Il2CppMethodPointer  s_methodPointers/{f=1} f{print} /^};/{if(f)exit}' Assembly-CSharp_CodeGen.c | grep -c 'm[0-9A-F]\{8,\}'
  echo "=== first 3 and last 2 entries"
  awk '/static Il2CppMethodPointer  s_methodPointers/{f=1} f{print} /^};/{if(f)exit}' Assembly-CSharp_CodeGen.c | grep 'm[0-9A-F]\{8,\}' | head -3
  awk '/static Il2CppMethodPointer  s_methodPointers/{f=1} f{print} /^};/{if(f)exit}' Assembly-CSharp_CodeGen.c | grep 'm[0-9A-F]\{8,\}' | tail -2
fi
