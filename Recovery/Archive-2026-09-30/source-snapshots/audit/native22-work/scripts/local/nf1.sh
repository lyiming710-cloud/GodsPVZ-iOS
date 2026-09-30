cd /workspaces/GodsPVZ-native19/.validation/native22
python3 native22_native_probe.py 0x06000348 0x0600034C 0x06000339 0x06000443 > nf.out.txt 2>&1
echo "rc=$?  bytes=$(stat -c%s nf.out.txt)"
cat nf.out.txt
