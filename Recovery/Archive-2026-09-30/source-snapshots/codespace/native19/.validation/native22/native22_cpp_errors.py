"""For each pilot token print every clang diagnostic plus the offending C++ lines."""
import re
import sys
from pathlib import Path

ROOT = Path('/workspaces/GodsPVZ-native19')
LOG = ROOT / '.validation/native22-full2'
CPP = ROOT / '.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput'


def method_pointer_table():
    lines = (CPP / 'GodsPVZRuntime1_CodeGen.c').read_text(encoding='utf-8-sig').splitlines()
    start = next(i for i, l in enumerate(lines) if 's_methodPointers[' in l)
    end = next(i for i in range(start, len(lines)) if lines[i].startswith('}'))
    return {tuple_sort(lines[i].strip().rstrip(',')): i - start - 2 for i in range(start + 2, end)}


def tuple_sort(s):
    return s


def main(tokens):
    lines = (CPP / 'GodsPVZRuntime1_CodeGen.c').read_text(encoding='utf-8-sig').splitlines()
    start = next(i for i, l in enumerate(lines) if 's_methodPointers[' in l)
    end = next(i for i in range(start, len(lines)) if lines[i].startswith('}'))
    table = {}
    for pos, l in enumerate(lines[start + 2:end]):
        entry = l.strip().rstrip(',')
        if entry and entry != 'NULL':
            table.setdefault(entry, []).append('0x%08X' % (0x06000000 + pos + 1))
    want = set(t.upper() for t in tokens)
    by_token = {}
    for sym, toks in table.items():
        for t in toks:
            if t.upper() in want:
                by_token.setdefault(t.upper(), []).append(sym)

    err_re = re.compile(r'^([^:]*\.(?:cpp|c)):(\d+):(\d+): error: (.*)$')
    for token in tokens:
        syms = by_token.get(token.upper(), [])
        print('#' * 100)
        print('TOKEN %s  IL2CPP symbols: %s' % (token, syms))
        found = []
        for log in sorted(LOG.glob('*.log')):
            src_path = CPP / log.name[:-4]
            src = src_path.read_text(encoding='utf-8-sig', errors='replace').splitlines() if src_path.exists() else []
            for line in log.read_text(errors='replace').splitlines():
                m = err_re.match(line)
                if not m or Path(m.group(1)).name != log.name[:-4]:
                    continue
                # enclosing function
                owner = None
                for i in range(int(m.group(2)) - 1, 0, -1):
                    if i <= len(src) and ('IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR' in src[i - 1]
                                          or src[i - 1].startswith('inline ')):
                        mm = re.search(r'\b([\w]+_m[0-9A-Fa-f]{8,})\s*\(', src[i - 1])
                        if mm:
                            owner = mm.group(1)
                            break
                if owner in syms:
                    found.append((log.name, int(m.group(2)), int(m.group(3)), m.group(4), src_path, src))
        print('diagnostics: %d' % len(found))
        for name, ln, col, msg, src_path, src in found:
            print()
            print('  --- %s:%d:%d  %s' % (name, ln, col, msg))
            lo, hi = max(1, ln - 8), min(len(src), ln + 3)
            for i in range(lo, hi + 1):
                mark = '>>' if i == ln else '  '
                print('   %s %5d | %s' % (mark, i, src[i - 1][:150]))


if __name__ == '__main__':
    main(sys.argv[1:])
