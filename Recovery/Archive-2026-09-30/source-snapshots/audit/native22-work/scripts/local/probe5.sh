echo "=== native22-full-tu tree summary"
ls /workspaces/GodsPVZ-native19/.validation/native22-full-tu | sed 's/.*\.//' | sort | uniq -c
echo ""
echo "=== native22-full2 tree"
ls /workspaces/GodsPVZ-native19/.validation/native22-full2 | head -30
echo ""
echo "=== sample log content"
head -30 /workspaces/GodsPVZ-native19/.validation/native22-full-tu/GodsPVZRuntime1__1.cpp.log
echo ""
echo "=== locate il2cppOutput dirs"
find /workspaces/GodsPVZ-native19 -maxdepth 7 -type d -name 'il2cppOutput' 2>/dev/null
echo ""
echo "=== count cpp files in each"
for d in $(find /workspaces/GodsPVZ-native19 -maxdepth 7 -type d -name 'il2cppOutput' 2>/dev/null); do echo "$d : $(ls $d | wc -l)"; done
