"""Native22 triage: split the 360-method watch list into three buckets.

Buckets (per the route-B decision):
  V  verifier-capability shortfall - the only reported deficiency comes from the
     checker's own modelling limits. Says nothing about the IL.
  H  heuristic hit, unconfirmed - flagged only by the phantom-local heuristic.
  D  type-level defect confirmed - at least one hard typed-verification failure
     that no complete C# compiler could have emitted, or a C++ diagnostic.
Also cross-references C++ diagnostics by decoding the IL2CPP `_m<RID>` suffix.

Read-only. Writes out/classification.json and a console summary.
"""
import bisect
import collections
import json
import re
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent))
from verify_native19_types import verify, InvalidIL  # noqa: E402

ROOT = Path('/workspaces/GodsPVZ-native19')
NB = ROOT / '.validation/native22'
OUT = NB / 'out'

# ---------------------------------------------------------------- failure families
TOOLGAP_PREFIXES = (
    'unsupported opcode ',
    'unsupported EH ',
)
TOOLGAP_EXACT = {
    'fallthrough outside method',
    'invalid EH range',
    'invalid EH boundary tryStart', 'invalid EH boundary tryEnd',
    'invalid EH boundary handlerStart', 'invalid EH boundary handlerEnd',
    'declared MaxStack too small',
}
FAMILY_RULES = [
    ('toolgap:unsupported-opcode', 'unsupported opcode '),
    ('toolgap:unsupported-eh', 'unsupported EH '),
    ('toolgap:maxstack', 'declared MaxStack too small'),
    ('toolgap:eh-boundary', 'invalid EH'),
    ('toolgap:fallthrough', 'fallthrough outside method'),
    ('type:merge-height', 'merge height '),
    ('type:merge-types', 'merge types '),
    ('type:receiver', 'receiver '),
    ('type:not-assignable', ' is not assignable to '),
    ('type:numeric-operands', 'invalid numeric operands '),
    ('type:shift', 'invalid shift'),
    ('type:unary', 'invalid unary operand'),
    ('type:conversion', 'invalid numeric conversion '),
    ('type:comparison', 'invalid comparison '),
    ('type:branch-comparison', 'invalid branch comparison '),
    ('type:stack-underflow', 'stack underflow'),
    ('type:array-access', 'invalid array access '),
    ('type:array-store', 'array store '),
    ('type:elem-mismatch', 'primitive element mismatch'),
    ('type:reference-cast', 'reference cast applied to '),
    ('type:unbox', 'unbox on non-reference'),
    ('type:bitwise-float', 'bitwise floating operand'),
    ('type:ldlen', 'ldlen on '),
    ('type:ldelem-ref', 'ldelem.ref on '),
    ('type:invalid-condition', 'invalid condition '),
    ('type:nonempty-leave-stack', 'nonempty stack at leave'),
    ('type:leave-from-finally', 'leave from finally'),
    ('type:endfinally', 'invalid endfinally'),
    ('type:throw', 'throw requires reference'),
    ('type:return-stack', 'invalid return stack or EH region'),
    ('type:branch-region', 'branch across protected region boundary'),
    ('type:switch-region', 'switch crosses protected region boundary'),
]


def family_of(msg: str) -> str:
    tail = msg.split(': ', 1)[1] if ': ' in msg else msg
    for name, needle in FAMILY_RULES:
        if needle in tail:
            return name
    return 'other:' + tail[:70]


def is_toolgap(fam: str, exc: BaseException) -> bool:
    if isinstance(exc, (KeyError, ValueError, IndexError)):
        return True          # checker crashed on an unmodelled construct
    return fam.startswith('toolgap:')


# ---------------------------------------------------------------- phantom locals
def locnum(op, o):
    if op.count('.') >= 1 and op.split('.')[-1].isdigit():
        return int(op.split('.')[-1])
    return o['index'] if isinstance(o, dict) else None


def phantom_locals(m):
    ins, nl = m['instructions'], len(m['locals'])
    written, addr = set(), set()
    for x in ins:
        op = x['opcode']
        if op.startswith('stloc'):
            n = locnum(op, x['operand'])
            if n is not None:
                written.add(n)
        elif op.startswith('ldloca'):
            n = locnum(op, x['operand'])
            if n is not None:
                addr.add(n)
    strict = set()
    for x in ins:
        op = x['opcode']
        if op.startswith('ldloc') and not op.startswith('ldloca'):
            n = locnum(op, x['operand'])
            if n is not None and n < nl and n not in written and n not in addr:
                strict.add(n)
    return sorted(strict)


# ---------------------------------------------------------------- cpp mapping
def method_pointer_table(cpp_dir):
    """s_methodPointers[] is indexed by MethodDef RID-1, so it is the exact
    IL2CPP symbol -> MetadataToken map for this image."""
    src = cpp_dir / 'GodsPVZRuntime1_CodeGen.c'
    text = src.read_text(encoding='utf-8-sig').splitlines()
    start = next(i for i, l in enumerate(text) if 's_methodPointers[' in l)
    end = next(i for i in range(start, len(text)) if text[i].startswith('}'))
    declared = int(re.search(r'\[(\d+)\]', text[start])[1])
    body = text[start + 2:end]
    table = {}
    stats = {'declared': declared, 'body_lines': len(body), 'null': 0, 'dup': 0}
    for pos, line in enumerate(body):
        entry = line.strip().rstrip(',')
        if entry == 'NULL':
            stats['null'] += 1
            continue
        token = '0x%08X' % (0x06000000 + pos + 1)
        if entry in table:
            stats['dup'] += 1
            continue
        table[entry] = token
    stats['symbols'] = len(table)
    return table, stats


def cpp_load():
    """Map every clang diagnostic to a MetadataToken via s_methodPointers."""
    pass
    pass
    log_dir = ROOT / '.validation/native22-full2'
    cpp_dir = (ROOT / '.validation/xcode-native18/expanded/xcode'
               '/Il2CppOutputProject/Source/il2cppOutput')
    table, tstats = method_pointer_table(cpp_dir)
    agg = collections.defaultdict(lambda: {'errors': 0, 'first': None, 'files': set()})
    unresolved = 0
    generic = 0
    total_errors = 0
    for log_file in sorted(log_dir.glob('*.log')):
        stem = log_file.name[:-4]
        src = cpp_dir / stem
        if not src.exists():
            continue
        locations, names = [], []
        for i, line in enumerate(src.read_text(encoding='utf-8-sig').splitlines(), 1):
            if line.startswith('IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR') and not line.rstrip().endswith(';'):
                mt = re.search(r'\b([\w]+_m[0-9A-Fa-f]{8,})\s*\(', line)
                if mt:
                    locations.append(i)
                    names.append(mt.group(1))
        for line in log_file.read_text(errors='replace').splitlines():
            mt = re.search(r'^([^:]*\.(?:cpp|c)):(\d+):(\d+): error: (.*)$', line)
            if not mt:
                continue
            if Path(mt.group(1)).name != stem:
                continue                       # belongs to an included header
            total_errors += 1
            idx = bisect.bisect_right(locations, int(mt.group(2))) - 1
            if idx < 0:
                unresolved += 1
                continue
            symbol = names[idx]
            token = table.get(symbol)
            if token is None:
                generic += 1
                continue
            rec = agg[token]
            rec['errors'] += 1
            rec['files'].add(stem)
            if rec['first'] is None:
                rec['first'] = mt.group(4)
    out = {tok: {'errors': v['errors'], 'first': v['first'], 'files': sorted(v['files'])}
           for tok, v in agg.items()}
    return out, cpp_dir, (f's_methodPointers {tstats}; '
                          f'{total_errors} clang error lines; mapped={sum(v["errors"] for v in out.values())}; '
                          f'not-in-pointer-table={generic}; outside-body={unresolved}')


def main():
    data = json.loads((OUT / 'all-methods.json').read_text())
    by_tok = {m['token']: m for m in data['methods']}
    cpp, cpp_base, cpp_note = cpp_load()

    rows = []
    fam_counter = collections.Counter()
    for m in data['methods']:
        rid = int(m['token'], 16) & 0xFFFFFF
        row = {
            'token': m['token'], 'rid': rid, 'name': m['name'],
            'owner': m['owner'], 'instructions': len(m['instructions']),
            'locals': m['locals'], 'verify': 'PASS', 'families': [], 'error': None,
            'phantom': phantom_locals(m),
        }
        try:
            verify(m, data['types'])
        except InvalidIL as e:
            fam = family_of(str(e))
            row['verify'] = 'FAIL'
            row['families'].append(fam)
            row['error'] = str(e)
            row['toolgap'] = is_toolgap(fam, e)
            fam_counter[fam] += 1
        except Exception as e:                                   # checker crash
            row['verify'] = 'CRASH'
            row['families'].append('toolgap:crash:' + type(e).__name__)
            row['error'] = f'{type(e).__name__}: {e}'
            row['toolgap'] = True
            fam_counter['toolgap:crash:' + type(e).__name__] += 1
        else:
            row['toolgap'] = False
        ce = cpp.get(m['token'])
        if ce:
            row['cpp'] = ce
        rows.append(row)

    # ---- bucket assignment
    for r in rows:
        hard = [f for f in r['families'] if not is_toolgap(f, None)]
        tool = [f for f in r['families'] if is_toolgap(f, None)]
        r['hard_families'] = hard
        r['toolgap_families'] = tool
        has_cpp = 'cpp' in r
        if hard:
            r['bucket'] = 'D'          # type-level defect (hard verifier failure)
        elif has_cpp:
            r['bucket'] = 'D'
        elif tool:
            r['bucket'] = 'V'          # only the checker itself failed
        elif r['phantom']:
            r['bucket'] = 'H'          # heuristic-only
        else:
            r['bucket'] = 'OK'

    # ---- the historic 360 watch list = strict phantom set
    watch = [r for r in rows if r['phantom']]
    buckets = collections.Counter(r['bucket'] for r in rows)
    watch_buckets = collections.Counter(r['bucket'] for r in watch)
    cpp_rows = [r for r in rows if 'cpp' in r]

    print('=== inputs')
    print('all-methods.json methods     :', len(rows))
    print('cpp diagnostics source       :', cpp_base.name, '-', cpp_note)
    print('cpp tokens resolved by table :', len(cpp))
    print('cpp tokens absent from export:', sum(1 for t in cpp if t not in by_tok))
    print('cpp-error methods in export  :', len(cpp_rows))
    print('cpp-error diagnostics total  :', sum(r['cpp']['errors'] for r in cpp_rows))
    print()
    print('=== verifier failure families (whole game body set)')
    for fam, n in fam_counter.most_common():
        tag = 'TOOLGAP' if fam.startswith('toolgap:') else 'HARD'
        print('  %-8s %5d  %s' % (tag, n, fam))
    print()
    print('=== bucket assignment over all %d methods' % len(rows))
    for k in ('OK', 'H', 'V', 'D'):
        print('  %s : %d' % (k, buckets[k]))
    print()
    print('=== the 360 watch list (STRICT phantom) re-bucketed: %d methods' % len(watch))
    for k in ('OK', 'H', 'V', 'D'):
        print('  %s : %d' % (k, watch_buckets.get(k, 0)))
    print()
    print('=== overlap: watch list vs cpp error methods')
    cpp_tokens = {r['token'] for r in cpp_rows}
    print('  cpp-error methods            :', len(cpp_tokens))
    print('  watch list ∩ cpp-error       :', sum(1 for r in watch if r['token'] in cpp_tokens))
    print()
    print('=== D-bucket top classes (all methods, not just watch list)')
    per = collections.Counter()
    tot = collections.Counter()
    for r in rows:
        tot[r['owner']] += 1
        if r['bucket'] == 'D':
            per[r['owner']] += 1
    for k, v in per.most_common(20):
        print('  %3d/%-4d %s' % (v, tot[k], k))
    print()
    print('=== H-bucket (heuristic-only) top classes')
    perh = collections.Counter(r['owner'] for r in rows if r['bucket'] == 'H')
    for k, v in perh.most_common(20):
        print('  %3d/%-4d %s' % (v, tot[k], k))
    print()

    focus = ('Plant', 'Zombie', 'Device', 'Board', 'Projectile', 'MouseManager')
    print('=== pilot-batch candidates: D-bucket, focus classes, by ascending size')
    cand = [r for r in rows if r['bucket'] == 'D' and r['owner'] in focus]
    cand.sort(key=lambda r: (r['instructions'], r['token']))
    print('  %-10s %-5s %-5s %-6s %s' % ('TOKEN', 'INS', 'CPP', 'PHAN', 'METHOD'))
    for r in cand[:45]:
        print('  %-10s %-5d %-5d %-6d %s' % (
            r['token'], r['instructions'], r['cpp']['errors'] if 'cpp' in r else 0,
            len(r['phantom']), r['name']))
    print('  ... %d candidates in focus classes total' % len(cand))

    (OUT / 'classification.json').write_text(json.dumps({
        'inputs': {'methods': len(rows), 'cpp_source': str(cpp_base), 'cpp_note': cpp_note},
        'families': dict(fam_counter),
        'buckets_all': dict(buckets),
        'buckets_watch': dict(watch_buckets),
        'methods': rows,
    }, indent=2))
    print()
    print('WROTE', OUT / 'classification.json')


if __name__ == '__main__':
    main()
