cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
echo "=== count occurrences of each variant (whole tree) ==="
for s in Component_GetComponent_TisAnimator GameObject_GetComponent_TisAnimator; do
  n=$(grep -ho "$s" *.cpp *.c 2>/dev/null | wc -l)
  echo "$s : $n"
done
echo ""
echo "=== files containing either ==="
grep -l '_GetComponent_TisAnimator' *.cpp 2>/dev/null | head
echo ""
echo "=== one sample line each ==="
grep -h 'Component_GetComponent_TisAnimator' *.cpp 2>/dev/null | head -2
grep -h 'GameObject_GetComponent_TisAnimator' *.cpp 2>/dev/null | head -2
echo ""
echo "=== definitions (line starting with type, followed by sym and '(' then '{') ==="
grep -hn 'Animator_t8A52E42AE54F76681838FE9E632683EF3952E883\* [A-Za-z]*_GetComponent_TisAnimator' *.cpp 2>/dev/null | head -4
