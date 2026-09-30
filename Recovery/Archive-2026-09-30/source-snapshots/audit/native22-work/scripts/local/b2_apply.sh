set -x
cd /workspaces/GodsPVZ-native19/.validation/native22
W=/workspaces/GodsPVZ-native19/.validation/native22/work
export GODSPVZ_RESOLVER=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped

python3 native23_afam_edits.py

dotnet bin23/RepairNative23.dll "$W/batch1.dll" "$W/batch2.dll" \
  edits-batch2.json "$W/batch2.report.txt"
echo "patch rc=$?"
sha256sum "$W/batch1.dll" "$W/batch2.dll"
wc -l "$W/batch2.report.txt"
grep -c '!!' "$W/batch2.report.txt" || true
