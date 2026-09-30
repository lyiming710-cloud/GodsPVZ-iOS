set -x
cd /workspaces/GodsPVZ-native19
CAN=.validation/native21/replay/canary
W=.validation/native22
MANAGED=$CAN/Library/Bee/artifacts/iOS/ManagedStripped/GodsPVZRuntime1.dll
mkdir -p "$W/retest2"
cp -p "$MANAGED" "$W/retest2/ManagedStripped.pre-batch2.dll"
sha256sum "$W/retest2/ManagedStripped.pre-batch2.dll"
cp -p "$W/work/batch2.dll" "$MANAGED"
sha256sum "$MANAGED"
echo READY
