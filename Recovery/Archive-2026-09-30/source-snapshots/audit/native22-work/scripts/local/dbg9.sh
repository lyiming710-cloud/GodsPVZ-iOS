cd /workspaces/GodsPVZ-native19
echo "=== grep tools that write all-methods.json ==="
grep -rl "all-methods" --include=*.cs --include=*.py --include=*.sh --include=*.mjs . 2>/dev/null | head -20
echo ""
echo "=== PatcherNative19 source location ==="
find . -maxdepth 4 -type d -name "PatcherNative19*" -printf "%p\n" 2>/dev/null | head
echo ""
echo "=== scripts/codespaces listing ==="
ls scripts/codespaces | head -40
echo ""
echo "=== takeover listing ==="
ls scripts/takeover | head -40
