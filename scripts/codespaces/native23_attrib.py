"""Native23 C++ diagnostic attribution.

Hard rules (the bug this file fixes):
  * clang logs in .validation/native22-full2 were produced from the CANARY tree
    (see .validation/native22_scan2.sh SRC=...), NOT from the xcode-native18 tree.
  * Both the CodeGen method-pointer table and the C++ source lines therefore MUST
    come from the canary tree. Mixing them silently shifts every line number.
  * Every printed result carries the source file's sha256 so attribution can be
    audited against .validation/native22/input/cpp-manifest.json.
"""
import hashlib
import json
import os
import re
import sys

ROOT = '/workspaces/GodsPVZ-native19'
CANARY = ROOT + '/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp'
LOGDIR = ROOT + '/.validation/native22-full2'
MANIFEST = ROOT + '/.validation/native22/input/cpp-manifest.json'

DEF_RE = re.compile(r'IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR[^\n]*?\b([A-Za-z0-9_]+)\s*\(')
ERR_RE = re.compile(r'^([^:]+\.(?:cpp|c)):(\d+):(\d+): error: (.*)$')
SYM_RE = re.compile(r'^\s*([A-Za-z0-9_]+)\s*,\s*$')


def sha256(p):
    h = hashlib.sha256()
    with open(p, 'rb') as f:
        for chunk in iter(lambda: f.read(1 << 20), b''):
            h.update(chunk)
    return h.hexdigest()


def load_manifest():
    if os.path.exists(MANIFEST):
        return json.load(open(MANIFEST))
    return {}


def method_pointer_table():
    """IL2CPP symbol -> metadata token, from s_methodPointers[] index."""
    path = os.path.join(CANARY, 'GodsPVZRuntime1_CodeGen.c')
    lines = open(path, encoding='utf-8-sig').read().splitlines()
    start = next(i for i, l in enumerate(lines) if 's_methodPointers[' in l)
    end = next(i for i in range(start, len(lines)) if lines[i].startswith('}'))
    declared = int(re.search(r'\[(\d+)\]', lines[start]).group(1))
    table, stats = {}, {'declared': declared, 'body': end - start - 2, 'null': 0, 'dup': 0}
    for pos, l in enumerate(lines[start + 2:end]):
        entry = l.strip().rstrip(',')
        if entry == 'NULL':
            stats['null'] += 1
            continue
        tok = '0x%08X' % (0x06000000 + pos + 1)
        if entry in table:
            stats['dup'] += 1
            continue
        table[entry] = tok
    stats['symbols'] = len(table)
    return table, stats


def build_def_index(files):
    """symbol -> (file, def_line, end_line). Extent = until a line equal to '}'."""
    index = {}
    for name in files:
        path = os.path.join(CANARY, name)
        src = open(path, encoding='utf-8-sig', errors='replace').read().splitlines()
        n = len(src)
        defs = []
        for i, l in enumerate(src, 1):
            if 'IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR' in l:
                m = DEF_RE.search(l)
                if m:
                    defs.append((m.group(1), i))
        for sym, start_line in defs:
            end_line = n
            for j in range(start_line, n):
                if src[j] == '}':
                    end_line = j + 1
                    break
            sym_suffix = None
            # prefer suffix match so Plant_Awake_mXXXX maps to unique symbol
            index.setdefault(sym, []).append((name, start_line, end_line))
    return index


def main(tokens):
    man = load_manifest()
    table, stats = method_pointer_table()
    print('[provenance] CodeGen.c sha256 = %s' % sha256(os.path.join(CANARY, 'GodsPVZRuntime1_CodeGen.c'))[:16])
    print('[provenance] manifest entry  = %s' % (man.get('GodsPVZRuntime1_CodeGen.c', {}).get('sha256', 'N/A')[:16]))
    print('[table] declared=%d body=%d null=%d dup=%d symbols=%d' % (
        stats['declared'], stats['body'], stats['null'], stats['dup'], stats['symbols']))
    print()

    want = set(t.upper() for t in tokens)
    tok2sym = {}
    for sym, tok in table.items():
        if tok.upper() in want:
            tok2sym.setdefault(tok.upper(), []).append(sym)

    files = sorted(n for n in os.listdir(CANARY) if n.endswith('.cpp'))
    print('[index] scanning %d cpp files for definitions...' % len(files))
    defs = build_def_index(files)
    print('[index] %d definitions indexed' % len(defs))

    # load diagnostics once
    diags = {}
    for lg in sorted(os.listdir(LOGDIR)):
        if not lg.endswith('.log'):
            continue
        base = lg[:-4]
        for line in open(os.path.join(LOGDIR, lg), errors='replace'):
            m = ERR_RE.match(line)
            if m and os.path.basename(m.group(1)) == base:
                diags.setdefault(base, []).append((int(m.group(2)), int(m.group(3)), m.group(4)))
    total = sum(len(v) for v in diags.values())
    print('[diag] %d error lines across %d logs' % (total, len(diags)))
    print()

    for token in tokens:
        syms = tok2sym.get(token.upper(), [])
        print('=' * 100)
        print('TOKEN %s   il2cpp symbols: %s' % (token, syms))
        hits = []
        for sym in syms:
            for (fname, sline, eline) in defs.get(sym, []):
                for (ln, col, msg) in diags.get(fname, []):
                    if sline <= ln <= eline:
                        hits.append((fname, sline, eline, ln, col, msg, sym))
        hits.sort(key=lambda t: (t[0], t[3]))
        print('  diagnostics inside recovered bodies: %d' % len(hits))
        shown_files = set()
        for (fname, sline, eline, ln, col, msg, sym) in hits:
            if fname not in shown_files:
                real = sha256(os.path.join(CANARY, fname))
                known = man.get(fname, {}).get('sha256')
                flag = 'MATCHES-MANIFEST' if real == known else 'DRIFT!! %s != %s' % (real[:12], str(known)[:12])
                print('  [file] %s sha256=%s lines=%d  %s' % (fname, real[:16], man.get(fname, {}).get('lines', -1), flag))
                shown_files.add(fname)
            print('  [fn] %s  body lines %d..%d' % (sym, sline, eline))
            src = open(os.path.join(CANARY, fname), encoding='utf-8-sig', errors='replace').read().splitlines()
            print('  --- %s:%d:%d  %s' % (fname, ln, col, msg))
            for i in range(max(1, ln - 6), min(len(src), ln + 2) + 1):
                mark = '>>' if i == ln else '  '
                print('   %s %6d | %s' % (mark, i, src[i - 1][:160]))
            print()
        print()


if __name__ == '__main__':
    main(sys.argv[1:] or [])
