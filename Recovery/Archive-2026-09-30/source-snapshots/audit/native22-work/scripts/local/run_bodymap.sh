cd /workspaces/GodsPVZ-native19/.validation/native22
time python3 native23_body_map.py > bodymap.out.txt 2>&1
echo "rc=$?"
cat bodymap.out.txt
