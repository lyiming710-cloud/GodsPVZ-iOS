"""Native23 batch 2 (second attempt) -- enumerate the "integer literal zero used
where a reference is required" family with a MULTI-ERROR verifier.

See native23_afam.py for the rule statement and native23_multi.py for the oracle.
The first attempt stalled at 10 methods purely because the oracle only ever
reported one error per method; with every clash visible, the greedy loop can keep
going inside methods that also carry unrelated damage.

ACCEPTANCE FOR ONE FLIP (all must hold, else the flip is never made):
  F1  It removes at least one recorded clash.
  F2  Every clash it removes has the shape "I4 vs <reference type>"
      (invalid comparison / invalid branch comparison / not assignable /
       receiver incompatible).  See afam_other().
  F3  It adds no clash that was not there before.
FINAL ACCEPTANCE FOR ONE METHOD:
  M1  >= 1 flip survived minimality.
  M2  No clash was added anywhere (guaranteed by F3 applied incrementally).
  M3  Every surviving flip is individually necessary: putting that single
      literal back makes at least one clash return.
"""
import collections
import copy
import json
import re
import sys

import verify_native19_types as V
import native23_multi as M
import native23_afam as A

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'
MAXZ = 60


def key(e):
    return (e['offset'], e['msg'])


def errs(b, types):
    return M.verify_multi(b, types)


def is_afam(e, types):
    if e['offset'] < 0:
        return False
    other = A.afam_other(e['msg'] or '')
    return other is not None and A.is_ref(other, types)


def solve(b, types, maxsteps=60):
    base = errs(b, types)
    if not base:
        return {'status': 'ALREADY-CLEAN', 'flips': [], 'base': [], 'final': base}
    zeros = [i for i, x in enumerate(b['instructions']) if A.is_zero_lit(x)]
    if len(zeros) > MAXZ:
        return {'status': 'TOO-MANY-ZEROS', 'flips': [], 'base': base, 'final': base}
    cur = copy.deepcopy(b)
    Ecur = [key(e) for e in base]
    flips = []
    removed_all = []
    for _ in range(maxsteps):
        best = None
        for i in zeros:
            if i in flips:
                continue
            trial = copy.deepcopy(cur)
            trial['instructions'][i]['opcode'] = 'ldnull'
            trial['instructions'][i]['operand'] = None
            E1 = [key(e) for e in errs(trial, types)]
            s1 = set(E1)
            s0 = set(Ecur)
            added = [e for e in E1 if e not in s0]
            removed = [e for e in Ecur if e not in s1]
            if added or not removed:
                continue
            if not all(is_afam({'offset': o, 'msg': m}, types) for o, m in removed):
                continue
            if best is None or len(removed) > len(best[1]):
                best = (i, removed, E1)
        if best is None:
            break
        i, removed, E1 = best
        flips.append(i)
        removed_all.extend(removed)
        cur['instructions'][i]['opcode'] = 'ldnull'
        cur['instructions'][i]['operand'] = None
        Ecur = E1
    if not flips:
        return {'status': 'NO-FLIP', 'flips': [], 'base': base, 'final': Ecur}
    # ---- M3 minimality / necessity ----
    keep = []
    for i in flips:
        trial = copy.deepcopy(b)
        for j in flips:
            if j == i:
                continue
            trial['instructions'][j]['opcode'] = 'ldnull'
            trial['instructions'][j]['operand'] = None
        E = [key(e) for e in errs(trial, types)]
        if set(E) - set(Ecur):          # a clash came back -> necessary
            keep.append(i)
    flips = keep
    if not flips:
        return {'status': 'ALL-UNNECESSARY', 'flips': [], 'base': base, 'final': Ecur}
    final = copy.deepcopy(b)
    for j in flips:
        final['instructions'][j]['opcode'] = 'ldnull'
        final['instructions'][j]['operand'] = None
    Ef = errs(final, types)
    return {'status': 'PASS' if not Ef else 'PARTIAL',
            'flips': flips, 'base': base, 'final': Ef,
            'removed': sorted(set(removed_all))}


def main():
    dump = sys.argv[1] if len(sys.argv) > 1 else ROOT + '/work/all-methods.batch1.json'
    bm_path = sys.argv[2] if len(sys.argv) > 2 else ROOT + '/out/body-map.new.json'
    data = json.load(open(dump))
    types = data['types']
    bm = json.load(open(bm_path))

    out = {}
    status = collections.Counter()
    tot_base = tot_final = 0
    for m in data['methods']:
        r = solve(m, types)
        status[r['status']] += 1
        tot_base += len(r.get('base') or [])
        tot_final += len(r.get('final') or [])
        if r['status'] not in ('PASS', 'PARTIAL'):
            continue
        out[m['token']] = {
            'name': m['name'],
            'owner': m.get('owner'),
            'status': r['status'],
            'flips': [{'offset': m['instructions'][i]['offset'],
                       'opcode': m['instructions'][i]['opcode'],
                       'operand': m['instructions'][i].get('operand')}
                      for i in r['flips']],
            'removed': [{'offset': o, 'msg': mm} for o, mm in r['removed']],
            'remaining': [{'offset': e['offset'], 'msg': e['msg']} for e in r['final']],
            'clang': [e['msg'] for e in bm.get(m['token'], {}).get('errors', [])],
            'clang_file': bm.get(m['token'], {}).get('file'),
        }
    print('methods scanned: %d' % len(data['methods']))
    for k, c in status.most_common():
        print('   %-18s %d' % (k, c))
    print()
    print('clashes recorded by the multi-error oracle: %d -> %d (%+d)'
          % (tot_base, tot_final, tot_final - tot_base))
    print()
    print('methods repaired: %d   total flips: %d'
          % (len(out), sum(len(v['flips']) for v in out.values())))
    print('   whole-method clean after repair (PASS) : %d'
          % sum(1 for v in out.values() if v['status'] == 'PASS'))
    print('   still carries other damage (PARTIAL)   : %d'
          % sum(1 for v in out.values() if v['status'] == 'PARTIAL'))
    print('   clang A-family lines inside these bodies: %d'
          % sum(1 for v in out.values() for x in v['clang']
                if 'comparison between pointer and integer' in x))
    print()
    print('flip-count distribution:')
    d = collections.Counter(len(v['flips']) for v in out.values())
    for k in sorted(d):
        print('   %d flip(s): %d methods' % (k, d[k]))
    print()
    print('top declaring classes (first 25):')
    c = collections.Counter((v['owner'] or '?').split('.')[-1].split('/')[0]
                            for v in out.values())
    for k, n in c.most_common(25):
        print('   %-36s %d' % (k, n))
    json.dump(out, open(ROOT + '/out/afam2-candidates.json', 'w'), indent=1)
    print()
    print('written: %s' % (ROOT + '/out/afam2-candidates.json'))


if __name__ == '__main__':
    main()
