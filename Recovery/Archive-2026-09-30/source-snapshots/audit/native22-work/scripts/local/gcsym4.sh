cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
for pat in Component_GetComponent_TisAnimator GameObject_GetComponent_TisAnimator; do
  hit=$(grep -n "^inline Animator_t[0-9A-Fa-f]*\* $pat" *.cpp | head -1)
  echo "############ $pat"
  echo "HIT: $hit"
  f=${hit%%:*}; rest=${hit#*:}; ln=${rest%%:*}
  if [ -n "$f" ] && [ -n "$ln" ]; then
    sed -n "${ln},$((ln+42))p" "$f"
  fi
  echo ""
done
