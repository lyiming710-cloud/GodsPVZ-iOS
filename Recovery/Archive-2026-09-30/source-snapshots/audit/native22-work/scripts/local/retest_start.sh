set -x
cd /workspaces/GodsPVZ-native19
CAN=.validation/native21/replay/canary
W=.validation/native22
MANAGED=$CAN/Library/Bee/artifacts/iOS/ManagedStripped/GodsPVZRuntime1.dll
OUT=$W/regen-batch1
mkdir -p "$W/retest"

# preserve the pre-repair input at the real path too
cp -p "$MANAGED" "$W/retest/ManagedStripped.pre-batch1.dll"
sha256sum "$W/retest/ManagedStripped.pre-batch1.dll"

cp -p "$W/work/batch1.dll" "$MANAGED"
sha256sum "$MANAGED"
date +%s > "$W/retest/started"
echo READY
