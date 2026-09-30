cd /workspaces/GodsPVZ-native19/.validation/native22
python3 native23_afam_nat.py 0x06000338 0x060000D9 > /tmp/n.txt 2>&1
echo rc=$?
sed -n '1,90p' /tmp/n.txt
