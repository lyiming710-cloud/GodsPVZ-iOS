cd /workspaces/GodsPVZ-native19
echo "=== csproj ==="
cat scripts/takeover/PatcherNative19/*.csproj
echo ""
echo "=== Program.cs line count ==="
wc -l scripts/takeover/PatcherNative19/Program.cs
echo ""
echo "=== first 80 lines ==="
head -80 scripts/takeover/PatcherNative19/Program.cs
echo ""
echo "=== grep for the json emission / command line args ==="
grep -n "args\[" scripts/takeover/PatcherNative19/Program.cs | head -20
grep -n "JsonSerializer\|WriteAllText\|Serialize" scripts/takeover/PatcherNative19/Program.cs | head -20
