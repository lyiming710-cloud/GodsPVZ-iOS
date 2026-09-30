cd /workspaces/GodsPVZ-native19/.validation
echo "=== tree mtimes ==="
stat -c '%y %n' native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput 2>&1
echo ""
echo "=== canary tree parents ==="
ls -la native21/replay 2>&1 | head -20
echo ""
echo "=== anything describing how canary was produced ==="
find native21/replay -maxdepth 2 -type f \( -name "*.log" -o -name "*.json" -o -name "*.md" -o -name "*.sh" \) -printf "%T@ %p %s\n" 2>/dev/null | sort -rn | head -25
echo ""
echo "=== the input assemblies: where is Assembly-CSharp.dll? ==="
find /workspaces/GodsPVZ-native19 -maxdepth 4 -name "Assembly-CSharp.dll" -printf "%T@ %p %s\n" 2>/dev/null | sort -rn | head -20
echo ""
echo "=== il2cpp conversion tooling present? ==="
find / -maxdepth 6 -name "il2cpp" -type f 2>/dev/null | head -5
find / -maxdepth 6 -name "UnityEditor*.dll" 2>/dev/null | head -3
echo ""
echo "=== repo Tools dir ==="
ls -la /workspaces/GodsPVZ-native19/Tools 2>&1 | head -30
