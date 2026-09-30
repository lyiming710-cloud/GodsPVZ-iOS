echo "=== dotnet ==="
which dotnet && dotnet --version
echo ""
echo "=== CecilPatch project ==="
ls -la /workspaces/GodsPVZ-native19/Tools/CecilPatch
echo ""
echo "=== its csproj ==="
find /workspaces/GodsPVZ-native19/Tools -maxdepth 2 -name "*.csproj" -o -maxdepth 2 -name "*.sln" | head
echo ""
echo "=== source files of the dumper used for all-methods.json ==="
ls -la /workspaces/GodsPVZ-native19/Tools/CecilPatch/*.cs 2>/dev/null | head -20
echo ""
echo "=== grep for a tool that emits methods json ==="
grep -rl "instructions" /workspaces/GodsPVZ-native19/Tools --include=*.cs 2>/dev/null | head -10
echo ""
echo "=== existing PatcherNative19 csproj ==="
find /workspaces/GodsPVZ-native19 -maxdepth 4 -name "*.csproj" -printf "%p\n" 2>/dev/null | head -10
