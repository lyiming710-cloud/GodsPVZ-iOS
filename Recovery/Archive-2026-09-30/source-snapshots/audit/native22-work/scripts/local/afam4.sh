cd /workspaces/GodsPVZ-native19/.validation/native22
nohup python3 -u native23_afam2.py work/all-methods.batch1.json out/body-map.new.json > afam2.out.txt 2>&1 &
echo "started pid=$!"
sleep 30
tail -3 afam2.out.txt 2>/dev/null
echo "(partial)"
