cd /workspaces/GodsPVZ-native19/.validation/native22
python3 native23_afam_spec.py 0x060000D9 0x06000338 0x0600043B 0x06000263 > /tmp/s.txt 2>/tmp/s.err
echo "rc=$?"
echo "--- stderr ---"; cat /tmp/s.err | head -20
echo "--- stdout ---"; head -150 /tmp/s.txt
