cd /workspaces/GodsPVZ-native19
ls -la Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/ 2>&1 | head
echo "--- build ---"
cd scripts/codespaces/RepairNative23
dotnet build -c Release -o /workspaces/GodsPVZ-native19/.validation/native22/bin23 2>&1 | tail -25
