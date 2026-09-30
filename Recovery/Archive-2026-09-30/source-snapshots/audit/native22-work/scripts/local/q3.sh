cd /workspaces/GodsPVZ-native19/.validation/native22
nohup python3 -u native23_afam3.py work/all-methods.batch1.json > afam3.out.txt 2>&1 &
echo started
sleep 40
cat afam3.out.txt
