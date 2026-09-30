"""Native23 batch 2 -- enumerate and repair the "integer literal zero used where a
reference is required" family (clang: `comparison between pointer and integer`).

WHY THIS FILE EXISTS
--------------------
Batch 1 proved, on `Device::TryPlacing`, that the recovery pipeline emits
`ldc.i4 0` where the original IL had `ldnull`: PC native does `test reg,reg; je`
(a null check), the typed verifier reports `invalid comparison Board, I4`, and
clang reports a pointer/integer comparison on the same body.  Batch 1 fixed 2 such
sites by hand.  There are ~2194 diagnostics in this family, so a hand-written
per-method spec does not scale.  This script turns that single proven
transformation into a *fail-closed mechanical rule*.

THE RULE (every condition must hold, otherwise the method is abandoned untouched)
---------------------------------------------------------------------------------
R1  The typed verifier must fail on this method, and the failure must have one of
    these exact shapes, where <X> is a *reference* type (class / interface /
    array / string / System.Object -- i.e. `ref()` in verify_native19_types):
        invalid comparison <X>, I4          invalid comparison I4, <X>
        invalid branch comparison <X>, I4   invalid branch comparison I4, <X>
        I4 is not assignable to <X>
        receiver I4 incompatible with <X>
    Rationale: `ceq`/`beq`/`stfld`/`stloc`/`call` between a reference and I4 is
    not legal CIL under any interpretation, so one of the two operands is wrong.
R2  There must be EXACTLY ONE integer-zero-literal instruction in the method
    whose replacement by `ldnull` makes that reported failure disappear.
    Zero candidates -> abandon.  More than one -> abandon (ambiguous).
R3  The replacement must not create a new failure in which `null` is compared or
    assigned against I4/I8/I/F (that would mean we broke a genuine integer zero).
R4  Greedy fixpoint until the method PASSES.  Any step that is ambiguous or
    produces no progress abandons the method entirely.
R5  Minimality: after PASS, each flip is removed again on its own; any flip whose
    removal still passes is dropped (it was not necessary).

WHAT THIS IS NOT
----------------
It is not proof of runtime behaviour.  It is a type-level restoration whose
correctness rests on (a) the strict typed verifier, (b) the per-flip necessity
(revert) proof, (c) PC-native sampling of the null-check pattern, and (d) the
clang re-scan showing the pointer/integer diagnostics disappear.  Behaviour is
reported separately from compilation, as agreed.
"""
import collections
import copy
import json
import re
import sys

import verify_native19_types as V

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'

ERR_AT = re.compile(r'IL_([0-9A-Fa-f]{4}): (.*)$')

CMP_RE = re.compile(r'^invalid comparison (.+?), (.+)$')
BCMP_RE = re.compile(r'^invalid branch comparison (.+?), (.+)$')
ASG_RE = re.compile(r'^(.+?) is not assignable to (.+)$')
RCV_RE = re.compile(r'^receiver (.+?) incompatible with (.+?)(?: \(.*\))?$')


def is_ref(t, types):
    return (t == 'null' or t.endswith('[]') or
            (t in types and not types[t].get('value')
             and not types[t].get('byref') and not types[t].get('unbound')))


def afam_other(msg):
    """Return the non-I4 side of an I4-vs-something type clash, else None."""
    for r in (CMP_RE, BCMP_RE):
        m = r.match(msg)
        if m:
            a, b = m.group(1), m.group(2)
            if a == 'I4' and b != 'I4':
                return b
            if b == 'I4' and a != 'I4':
                return a
            return None
    m = ASG_RE.match(msg)
    if m and m.group(1) == 'I4':
        return m.group(2)
    m = RCV_RE.match(msg)
    if m and m.group(1) == 'I4':
        return m.group(2)
    return None


def lit_value(x):
    a = x.get('operand')
    if isinstance(a, dict):
        for k in ('value', 'int', 'operand'):
            if k in a:
                return a[k]
        return None
    return a


def is_zero_lit(x):
    op = x['opcode']
    if op == 'ldc.i4.0':
        return True
    if op in ('ldc.i4', 'ldc.i4.s', 'ldc.i8'):
        return lit_value(x) == 0
    return False


def verr(b, types):
    try:
        V.verify(b, types)
        return None
    except V.InvalidIL as e:
        return str(e)
    except Exception as e:  # noqa
        return 'CRASH %s: %s' % (type(e).__name__, e)


def locate(err):
    m = ERR_AT.search(err or '')
    if not m:
        return None, None
    return int(m.group(1), 16), m.group(2)


def solve(b, types, maxsteps=40):
    cur = copy.deepcopy(b)
    flips = []
    trace = []
    for _ in range(maxsteps):
        err = verr(cur, types)
        if err is None:
            break
        off, msg = locate(err)
        if off is None:
            return {'status': 'STOP-UNLOCATED', 'flips': flips, 'trace': trace,
                    'err': err}
        other = afam_other(msg or '')
        if other is None or not is_ref(other, types):
            return {'status': 'STOP-NOT-AFAM', 'flips': flips, 'trace': trace,
                    'err': err}
        zeros = [i for i, x in enumerate(cur['instructions'])
                 if is_zero_lit(x) and i not in flips]
        cands = []
        for i in zeros:
            trial = copy.deepcopy(cur)
            trial['instructions'][i]['opcode'] = 'ldnull'
            trial['instructions'][i]['operand'] = None
            e2 = verr(trial, types)
            if e2 is None:
                cands.append((i, 'PASS', None))
                continue
            o2, m2 = locate(e2)
            if o2 is None or o2 == off:
                continue
            if 'null' in (m2 or '') and ('I4' in m2 or 'I8' in m2 or ' F' in m2):
                continue          # we manufactured a bad null -> reject
            cands.append((i, 'MOVED', (o2, m2)))
        best = [c for c in cands if c[1] == 'PASS']
        if best:
            if len(best) != 1:
                return {'status': 'AMBIGUOUS', 'flips': flips, 'trace': trace,
                        'err': err, 'ambig': [c[0] for c in best]}
            pick = best[0]
        else:
            if len(cands) != 1:
                return {'status': 'AMBIGUOUS', 'flips': flips, 'trace': trace,
                        'err': err, 'ambig': [c[0] for c in cands]}
            pick = cands[0]
        i = pick[0]
        flips.append(i)
        trace.append({'off': cur['instructions'][i]['offset'],
                      'err_before': err, 'outcome': pick[1],
                      'err_after': (pick[2][1] if pick[2] else None)})
        cur['instructions'][i]['opcode'] = 'ldnull'
        cur['instructions'][i]['operand'] = None
    else:
        return {'status': 'STOP-MAXSTEPS', 'flips': flips, 'trace': trace,
                'err': verr(cur, types)}

    # ---- R5 minimality: drop any flip that is not individually necessary ----
    changed = True
    while changed and flips:
        changed = False
        for i in list(flips):
            rest = [j for j in flips if j != i]
            trial = copy.deepcopy(b)
            for j in rest:
                trial['instructions'][j]['opcode'] = 'ldnull'
                trial['instructions'][j]['operand'] = None
            if verr(trial, types) is None:
                flips = rest
                changed = True
                break
    # verify the MINIMAL set (cur may still carry flips that were pruned away)
    final = copy.deepcopy(b)
    for j in flips:
        final['instructions'][j]['opcode'] = 'ldnull'
        final['instructions'][j]['operand'] = None
    if verr(final, types) is not None:
        return {'status': 'NOT-PASS', 'flips': flips, 'trace': trace}
    return {'status': 'PASS', 'flips': flips, 'trace': trace}


def main():
    dump = sys.argv[1] if len(sys.argv) > 1 else ROOT + '/work/all-methods.batch1.json'
    bm_path = sys.argv[2] if len(sys.argv) > 2 else ROOT + '/out/body-map.new.json'
    data = json.load(open(dump))
    types = data['types']
    bm = json.load(open(bm_path))

    # ---- show the operand encoding actually used for zero literals (audit aid)
    seen = collections.Counter()
    for m in data['methods']:
        for x in m['instructions']:
            if is_zero_lit(x):
                seen[x['opcode'] + ' | ' + json.dumps(x.get('operand'),
                                                      ensure_ascii=False)[:40]] += 1
    print('zero-literal encodings found (top 6):')
    for k, c in seen.most_common(6):
        print('   %-58s %d' % (k, c))
    print()

    out = {}
    status = collections.Counter()
    for m in data['methods']:
        if verr(m, types) is None:
            continue                      # already clean
        r = solve(m, types)
        status[r['status']] += 1
        if r['status'] != 'PASS' or not r['flips']:
            continue
        out[m['token']] = {
            'name': m['name'],
            'owner': m.get('owner'),
            'flips': [{'offset': m['instructions'][i]['offset'],
                       'opcode': m['instructions'][i]['opcode'],
                       'operand': m['instructions'][i].get('operand')}
                      for i in r['flips']],
            'trace': r['trace'],
            'clang': [e['msg'] for e in bm.get(m['token'], {}).get('errors', [])],
            'clang_file': bm.get(m['token'], {}).get('file'),
        }

    print('methods that failed typed verification: %d' % sum(status.values()))
    for k, c in status.most_common():
        print('   %-16s %d' % (k, c))
    print()
    print('SOLVED (PASS, >=1 necessary flip): %d methods, %d flips'
          % (len(out), sum(len(v['flips']) for v in out.values())))

    withclang = [t for t, v in out.items()
                 if any('comparison between pointer and integer' in m for m in v['clang'])]
    print('   of which the body also carries a clang pointer-vs-integer diagnostic: %d'
          % len(withclang))
    print('   clang A-family lines covered by these bodies: %d'
          % sum(1 for t, v in out.items() for m in v['clang']
                if 'comparison between pointer and integer' in m))
    print()
    print('flip-count distribution:')
    d = collections.Counter(len(v['flips']) for v in out.values())
    for k in sorted(d):
        print('   %d flip(s): %d methods' % (k, d[k]))
    print()
    print('top declaring classes:')
    c = collections.Counter((v['owner'] or '?').split('.')[-1].split('/')[0]
                            for v in out.values())
    for k, n in c.most_common(20):
        print('   %-34s %d' % (k, n))

    json.dump(out, open(ROOT + '/out/afam-candidates.json', 'w'), indent=1)
    print()
    print('written: %s' % (ROOT + '/out/afam-candidates.json'))


if __name__ == '__main__':
    main()
