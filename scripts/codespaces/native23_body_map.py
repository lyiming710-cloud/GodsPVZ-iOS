"""Authoritative map: metadata token -> recovered IL2CPP body -> clang diagnostics.

Everything is read from the CANARY tree (the one .validation/native22_scan2.sh compiled).
Source file sha256 values are cross-checked against input/cpp-manifest.json so that a
stale tree can never silently masquerade as current again.
"""
import hashlib
import json
import os
import re
import collections

ROOT = '/workspaces/GodsPVZ-native19'
CANARY = ROOT + '/.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp'
LOGDIR = ROOT + '/.validation/native22-full2'
MANIFEST = ROOT + '/.validation/native22/input/cpp-manifest.json'
OUT = ROOT + '/.validation/native22/out/body-map.json'

DEF_RE = re.compile(r'IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR[^\n]*?\b([A-Za-z0-9_]+)\s*\(')
ERR_RE = re.compile(r'^([^:]+\.(?:cpp|c)):(\d+):(\d+): error: (.*)$')


def sha256(p):
    h = hashlib.sha256()
    with open(p, 'rb') as f:
        for chunk in iter(lambda: f.read(1 << 20), b''):
            h.update(chunk)
    return h.hexdigest()


def method_pointer_table():
    path = os.path.join(CANARY, 'GodsPVZRuntime1_CodeGen.c')
    lines = open(path, encoding='utf-8-sig').read().splitlines()
    start = next(i for i, l in enumerate(lines) if 's_methodPointers[' in l)
    end = next(i for i in range(start, len(lines)) if lines[i].startswith('}'))
    table = {}
    for pos, l in enumerate(lines[start + 2:end]):
        entry = l.strip().rstrip(',')
        if entry and entry != 'NULL':
            table.setdefault(entry, []).append('0x%08X' % (0x06000000 + pos + 1))
    return table


def main():
    man = json.load(open(MANIFEST))
    table = method_pointer_table()
    sym2tok = {}
    for sym, toks in table.items():
        sym2tok[sym] = toks

    files = sorted(n for n in os.listdir(CANARY) if n.endswith('.cpp'))
    print('scanning %d cpp files' % len(files))

    # per-file: definitions (symbol -> line) and diagnostics (line -> list)
    diags = {}
    for lg in sorted(os.listdir(LOGDIR)):
        if not lg.endswith('.log'):
            continue
        base = lg[:-4]
        for line in open(os.path.join(LOGDIR, lg), errors='replace'):
            m = ERR_RE.match(line)
            if m and os.path.basename(m.group(1)) == base:
                diags.setdefault(base, {}).setdefault(int(m.group(2)), []).append(
                    (int(m.group(3)), m.group(4)))

    result = {}
    drift = []
    for name in files:
        path = os.path.join(CANARY, name)
        real = sha256(path)
        known = man.get(name, {}).get('sha256')
        if known is not None and real != known:
            drift.append(name)
        src = open(path, encoding='utf-8-sig', errors='replace').read().splitlines()
        n = len(src)
        positions = []
        for i, l in enumerate(src, 1):
            if 'IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR' in l:
                m = DEF_RE.search(l)
                if m:
                    positions.append((m.group(1), i))
        fdiags = diags.get(name, {})
        if not positions and not fdiags:
            continue
        for k, (sym, sline) in enumerate(positions):
            end_line = positions[k + 1][1] - 1 if k + 1 < len(positions) else n
            for j in range(sline, min(end_line, n) + 1):
                if src[j - 1] == '}':
                    end_line = j
                    break
            for tok in sym2tok.get(sym, []):
                errs = []
                for ln in sorted(fdiags):
                    if sline <= ln <= end_line:
                        for (col, msg) in fdiags[ln]:
                            errs.append({'line': ln, 'col': col, 'msg': msg})
                result[tok] = {'symbol': sym, 'file': name, 'start': sline, 'end': end_line,
                               'file_sha256': real, 'errors': errs}
    print('tokens mapped: %d' % len(result))
    print('tokens with >=1 error: %d' % sum(1 for v in result.values() if v['errors']))
    print('source drift vs manifest: %s' % (drift if drift else 'none'))
    json.dump(result, open(OUT, 'w'))
    print('written: %s (%d bytes)' % (OUT, os.path.getsize(OUT)))

    fam = collections.Counter()
    for v in result.values():
        for e in v['errors']:
            m = e['msg']
            if 'no matching function for call to' in m:
                fam['call-signature'] += 1
            elif 'assigning to' in m:
                fam['assign-type'] += 1
            elif 'invalid operands to binary expression' in m:
                fam['invalid-operands'] += 1
            elif 'comparison between pointer and integer' in m:
                fam['ptr-vs-int'] += 1
            elif 'C-style cast' in m:
                fam['cast-not-allowed'] += 1
            elif 'member reference base type' in m or 'no member named' in m:
                fam['member-access'] += 1
            else:
                fam['other: ' + m[:60]] += 1
    print()
    print('diagnostic families (all mapped bodies):')
    for k, c in fam.most_common(20):
        print('  %6d  %s' % (c, k))


if __name__ == '__main__':
    main()
