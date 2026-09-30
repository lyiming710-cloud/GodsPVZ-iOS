cd /workspaces/GodsPVZ-native19/.validation/native22
python3 native23_attrib.py 0x06000348 0x060003F7 0x060003FD 0x06000451 0x06000338 0x06000339 > attrib.out.txt 2>&1
echo "rc=$?"
head -20 attrib.out.txt
echo "..."
wc -l attrib.out.txt
