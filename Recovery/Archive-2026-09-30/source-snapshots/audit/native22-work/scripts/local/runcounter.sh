cd /workspaces/GodsPVZ-native19/.validation/native22
export PYTHONPATH=/workspaces/GodsPVZ-native19/.validation/native22:/workspaces/GodsPVZ-native19/scripts/codespaces
python3 /workspaces/GodsPVZ-native19/scripts/codespaces/native23_counter.py > /workspaces/GodsPVZ-native19/.validation/native22/counter.log 2>&1
echo "counter rc=$?"
cat /workspaces/GodsPVZ-native19/.validation/native22/counter.log
