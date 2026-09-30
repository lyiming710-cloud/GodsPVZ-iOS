cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
python3 - <<'PY'
import re, pathlib, collections
root = pathlib.Path('.')
seen = {}
for p in sorted(root.glob('*.cpp')):
    txt = p.read_text(encoding='utf-8-sig', errors='replace')
    for m in re.finditer(r'inline Animator_t[0-9A-Fa-f]+ (\w+_GetComponent_TisAnimator_t[0-9A-Fa-f]+_m[0-9A-Fa-f]+) \(([^)]*)\)\s*\n\{', txt):
        sym, args = m.group(1), m.group(2)
        if sym in seen:
            continue
        start = m.end()
        end = txt.find('\n}\n', start)
        seen[sym] = (p.name, args, txt[start:end], m.start())
for sym, (f, args, body, pos) in sorted(seen.items()):
    print('#'*96)
    print('FILE %s at char %d' % (f, pos))
    print('SYM  %s' % sym)
    print('ARGS %s' % args)
    print('BODY (%d chars):' % len(body))
    print('\n'.join(body.splitlines()[:40]))
PY
