cd /workspaces/GodsPVZ-native19/.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput
python3 - <<'PY'
import re, collections
lines = open('GodsPVZRuntime1_CodeGen.c', encoding='utf-8-sig').read().splitlines()
start = next(i for i,l in enumerate(lines) if 's_methodPointers[' in l)
end   = next(i for i in range(start, len(lines)) if lines[i].startswith('}'))
body  = lines[start+2:end]
print('declared size   :', re.search(r'\[(\d+)\]', lines[start])[1])
print('body lines      :', len(body))
blank = [i for i,l in enumerate(body) if not l.strip()]
print('blank lines     :', len(blank))
sym = re.compile(r'^\t([\w]+_m[0-9A-Fa-f]{8,}),$')
good, bad = [], []
for i,l in enumerate(body):
    m = sym.match(l)
    (good if m else bad).append((i,l))
print('matching lines  :', len(good))
print('non-matching    :', len(bad))
for i,l in bad[:15]:
    print('   [%d] %r' % (i, l[:120]))
names = [sym.match(l)[1] for l,_ in good]
dups = [n for n,c in collections.Counter(names).items() if c>1]
print('duplicate symbols:', len(dups))
for d in dups[:10]: print('   ', d, collections.Counter(names)[d])
PY
