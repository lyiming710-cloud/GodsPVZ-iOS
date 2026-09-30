cd /workspaces/GodsPVZ-native19/.validation/native22
echo "=== public functions in verify_native19_types.py ==="
grep -n "^def \|^class \|^    def " verify_native19_types.py | head -40
echo ""
echo "=== how native22_classify.py calls it ==="
grep -n "verify\|verify_native19\|import" native22_classify.py | head -20
