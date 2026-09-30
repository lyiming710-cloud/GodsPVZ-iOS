cd /workspaces/GodsPVZ-native19
CAN=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary
echo "=== disk ==="
df -h /workspaces | tail -2
echo ""
echo "=== ManagedStripped dir ==="
ls -la "$CAN/Library/Bee/artifacts/iOS/ManagedStripped" 2>&1 | head -15
echo "count: $(ls "$CAN/Library/Bee/artifacts/iOS/ManagedStripped" 2>/dev/null | wc -l)"
echo ""
echo "=== GodsPVZRuntime1.dll identity ==="
G="$CAN/Library/Bee/artifacts/iOS/ManagedStripped/GodsPVZRuntime1.dll"
ls -la "$G" 2>&1
sha256sum "$G" 2>&1
echo "(candidate.json sources.unlinked = 7d2b3661f3e4aa3ba27b986c233d65c920e4c439851eae1788ae58323cc1d910)"
echo "(candidate.json sources.linked   = f561d48a353f1747b77260f9db69acabc1bb4860f46d4ffc447d81764c55efa9)"
echo ""
echo "=== other candidate input dlls in replay dir ==="
sha256sum "$CAN/../native21-linked.dll" "$CAN/../native21-unlinked.dll" 2>&1
echo ""
echo "=== full rsp (folded) ==="
RSP="$CAN/Library/Bee/artifacts/rsp/8082527414012784887.rsp"
tr ' ' '\n' < "$RSP" | grep -v '^--assembly=' | head -40
echo ""
echo "--- non-assembly flags ---"
tr ' ' '\n' < "$RSP" | grep '^-' | grep -v '^--assembly=' | head -40
