cd /workspaces/GodsPVZ-native19/.validation/native22
echo "===== PARITY GATE ====="
time python3 -u native23_multicheck.py work/all-methods.batch1.json 2>&1 | tail -20
echo "rc=${PIPESTATUS[0]}"
