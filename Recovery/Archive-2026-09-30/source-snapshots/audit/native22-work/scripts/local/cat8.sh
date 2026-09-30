cd /workspaces/GodsPVZ-native19/.validation/native22
echo "=== Plant::Update region 0x18035C650 .. 0x18035C6C0 ==="
awk '/^ *18035c6[0-9a-f]0:/,/^ *18035c6[0-9a-f]0:/' out/asm-0600034c.txt | head -0
grep -E "^ *(18035c6[4-9]|18035c[6][a-f])" out/asm-0600034c.txt | head -60
echo ""
echo "=== full context: every line from 0x18035C640 to 0x18035C6D0 ==="
python3 - <<'PY'
p='/workspaces/GodsPVZ-native19/.validation/native22/out/asm-0600034c.txt'
for line in open(p):
    import re
    m=re.match(r'^\s+([0-9a-f]+):',line)
    if m:
        a=int(m.group(1),16)
        if 0x18035c640<=a<=0x18035c6d5:
            print(line.rstrip())
PY
