cd /workspaces/GodsPVZ-native19/.validation/native22
export PYTHONPATH=/workspaces/GodsPVZ-native19/.validation/native22:/workspaces/GodsPVZ-native19/scripts/codespaces
python3 /workspaces/GodsPVZ-native19/scripts/codespaces/native23_tiers.py > /workspaces/GodsPVZ-native19/.validation/native22/tiers.log 2>&1
echo "rc=$?"
cat /workspaces/GodsPVZ-native19/.validation/native22/tiers.log
