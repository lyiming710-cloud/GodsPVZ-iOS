set -e
cd /workspaces/GodsPVZ-native19
cp /dev/null /tmp/upload_nothing 2>/dev/null || true
echo "=== place classifier next to verifier"
ls -la .validation/native22/verify_native19_types.py
echo "=== invocation.json of full-tu"
cat .validation/native22-full-tu/invocation.json 2>/dev/null | head -c 800
echo ""
echo "=== results.json present?"
ls -la .validation/native22-full-tu/ | head -10
echo "=== count of cpp logs"
ls .validation/native22-full-tu/*.log 2>/dev/null | wc -l
