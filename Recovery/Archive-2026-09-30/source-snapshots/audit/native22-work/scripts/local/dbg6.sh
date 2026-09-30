cd /workspaces/GodsPVZ-native19
IL2CPP=/workspaces/GodsPVZ-native19/.validation/native19/replay/canary/il2cpp/build/deploy/il2cpp
echo "=== il2cpp binary ==="
ls -la "$IL2CPP" 2>&1
file "$IL2CPP" 2>&1
"$IL2CPP" --help 2>&1 | head -20
echo ""
echo "=== rsp file ==="
RSP=/workspaces/GodsPVZ-native19/.validation/native21/replay/canary/Library/Bee/artifacts/rsp/8082527414012784887.rsp
ls -la "$RSP"
echo "sha256: $(sha256sum "$RSP" | cut -d' ' -f1)"
echo "--- content (truncated, dll/exe lines) ---"
grep -E "\.dll|\.exe" "$RSP" | head -40
echo ""
echo "=== line count of rsp ==="
wc -l "$RSP"
