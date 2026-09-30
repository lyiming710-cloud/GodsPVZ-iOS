cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
echo "=== definitions containing GetComponent_TisAnimator (Animator type) ==="
grep -n 'Animator_t8A52E42AE54F76681838FE9E632683EF3952E883 (' *.cpp | head
echo ""
echo "=== search all files for both symbols and print bodies"
python3 - <<'PY'
import re, pathlib
root = pathlib.Path('.')
targets = {}
for p in root.glob('*.cpp'):
    txt = p.read_text(encoding='utf-8-sig', errors='replace')
    for m in re.finditer(r'^(\w+_GetComponent_TisAnimator_t[0-9A-Fa-f]+_m[0-9A-Fa-f]+)(_gshared)?\s*\(', txt, re.M):
        start = txt.rfind('\n', 0, m.start()) + 1
        end = txt.find('\n}\n', m.start())
        targets[(m.group(1), p.name)] = txt[start:end+3]
for (sym, f), body in sorted(targets.items()):
    print('#'*90)
    print('SYM', sym, 'FILE', f, 'len', len(body))
    print(body[:1800])
PY
