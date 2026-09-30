set -x
cd /workspaces/GodsPVZ-native19/.validation/native22
W=/workspaces/GodsPVZ-native19/.validation/native22/work
mkdir -p "$W"
export GODSPVZ_RESOLVER=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped
cp -p input/GodsPVZRuntime1.baseline.dll "$W/stage0-batch1.dll"
dotnet bin23/RepairNative23.dll "$W/stage0-batch1.dll" "$W/batch1.dll" \
  /workspaces/GodsPVZ-native19/.validation/native22/edits-batch1.json \
  "$W/batch1.report.txt"
echo "rc=$?"
sha256sum input/GodsPVZRuntime1.baseline.dll "$W/batch1.dll"
