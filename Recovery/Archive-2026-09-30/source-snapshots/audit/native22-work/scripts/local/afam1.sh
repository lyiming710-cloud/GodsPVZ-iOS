cd /workspaces/GodsPVZ-native19/.validation/native22
nohup python3 -u native23_afam.py work/all-methods.batch1.json out/body-map.new.json > afam.out.txt 2>&1 &
echo "started pid=$!"
sleep 20
echo "--- partial ---"
tail -5 afam.out.txt 2>/dev/null
