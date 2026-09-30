cd /workspaces/GodsPVZ-native19/scripts/codespaces/RepairNative23
dotnet build -c Release -o /workspaces/GodsPVZ-native19/.validation/native22/bin23 2>&1 | tail -5
cd /workspaces/GodsPVZ-native19/.validation/native22
export REPAIR_DEBUG=1
export GODSPVZ_RESOLVER=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped
dotnet bin23/RepairNative23.dll work/stage0-batch1.dll work/batch1.dll edits-batch1.json work/batch1.report.txt
