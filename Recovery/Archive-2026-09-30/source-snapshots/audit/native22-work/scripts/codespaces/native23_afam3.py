"""Batch 2 re-derivation WITH PROVENANCE.

Re-runs the same greedy rule as native23_afam2.py, but records, for every single
flip, where the REFERENCE operand's type came from:

  * TRUSTED  -- FIELD / SFIELD / ARG / THIS / CALLRET / NEWOBJ / STRING / NULL:
                the type is written in a signature that exists in metadata
                (field signature, method signature, ctor signature).
  * QUARANTINE -- LOCAL / ELEM / CONV / ARITH / MERGE / TAINTED / anything
                derived from a value that already clashed: the type depends on a
                localsig declaration nobody vouches for, on a control-flow merge
                that had to join two disagreeing types, or on a polluted value.

Sites in the second group are NOT accepted merely because the conflict count
dropped.  They are written to a separate quarantine file for later evidence
work.  This is the answer to "when a method already has type conflicts, how do
you avoid continuing to infer with types that may be wrongly declared or
polluted by wrong data flow?" -- you do not try to; you refuse to conclude
anything from those sites.
"""
import collections
import copy
import json
import sys

import native23_afam as A
import native23_prov as P

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'
MAXZ = 60


def key(e):
    return (e['offset'], e['msg'])


def is_afam(e, types):
    if e['offset'] < 0:
        return False
    other = A.afam_other(e['msg'] or '')
    return other is not None and A.is_ref(other, types)


def classify(e, types):
    """Return (verdict, provenance) for the REFERENCE side of an I4-vs-reference
    clash.  The I4 side is by construction the literal we are considering, whose
    provenance is CONST, so the reference side is whichever operand is not CONST."""
    top, second = e.get('prov_top'), e.get('prov_second')
    cands = [p for p in (top, second) if p and P.prov_class(p) != 'CONST']
    if not cands:
        cands = [p for p in (top, second) if p]
    for p in cands:
        if P.prov_class(p) in P.TRUSTED and 'TAINTED' not in p:
            return 'ACCEPT', p
    for p in cands:
        return 'QUARANTINE', p
    return 'QUARANTINE', '<no provenance recorded>'


def solve(b, types, maxsteps=60):
    cur = copy.deepcopy(b)
    Ecur = P.verify_prov(cur, types)
    if not Ecur:
        return None
    kcur = [key(e) for e in Ecur]
    prov_of = {key(e): e for e in Ecur}
    zeros = [i for i, x in enumerate(b['instructions']) if A.is_zero_lit(x)]
    if len(zeros) > MAXZ:
        return None
    flips = []
    recs = []
    for _ in range(maxsteps):
        best = None
        for i in zeros:
            if i in flips:
                continue
            trial = copy.deepcopy(cur)
            trial['instructions'][i]['opcode'] = 'ldnull'
            trial['instructions'][i]['operand'] = None
            E1 = P.verify_prov(trial, types)
            s1 = {key(e) for e in E1}
            s0 = set(kcur)
            added = [e for e in E1 if key(e) not in s0]
            removed = [k for k in kcur if k not in s1]
            if added or not removed:
                continue
            if not all(is_afam(prov_of[k], types) for k in removed):
                continue
            if best is None or len(removed) > len(best[1]):
                best = (i, removed, E1)
        if best is None:
            break
        i, removed, E1 = best
        for k in removed:
            e = prov_of[k]
            verdict, prov = classify(e, types)
            recs.append({'flip_offset': cur['instructions'][i]['offset'],
                         'clash_offset': k[0], 'clash_msg': k[1],
                         'prov': prov, 'prov_class': P.prov_class(prov or ''),
                         'verdict': verdict})
        flips.append(i)
        cur['instructions'][i]['opcode'] = 'ldnull'
        cur['instructions'][i]['operand'] = None
        Ecur = E1
        kcur = [key(e) for e in Ecur]
        prov_of = {key(e): e for e in Ecur}
    if not recs:
        return None
    # necessity, unchanged from afam2
    keep = []
    Ef = {key(e) for e in Ecur}
    for i in flips:
        trial = copy.deepcopy(b)
        for j in flips:
            if j == i:
                continue
            trial['instructions'][j]['opcode'] = 'ldnull'
            trial['instructions'][j]['operand'] = None
        if {key(e) for e in P.verify_prov(trial, types)} - Ef:
            keep.append(i)
    kept_offsets = {b['instructions'][i]['offset'] for i in keep}
    recs = [r for r in recs if r['flip_offset'] in kept_offsets]
    if not recs:
        return None
    return {'recs': recs, 'n_flips': len(keep)}


def main():
    dump = sys.argv[1] if len(sys.argv) > 1 else ROOT + '/work/all-methods.batch1.json'
    data = json.load(open(dump))
    types = data['types']
    accepted, quarantine = {}, {}
    vcount = collections.Counter()
    pcount = collections.Counter()
    contested = collections.Counter()
    for m in data['methods']:
        r = solve(m, types)
        if r is None:
            continue
        recs = r['recs']
        # types named in ANY clash of this method = contested declarations
        cts = {A.afam_other(e['msg']) for e in P.verify_prov(m, types)
               if A.afam_other(e['msg'] or '')}
        for rec in recs:
            vcount[rec['verdict']] += 1
            pcount[rec['prov_class']] += 1
            other = A.afam_other(rec['clash_msg'])
            if other in cts:
                contested[(rec['verdict'], other in cts)] += 1
        acc = [x for x in recs if x['verdict'] == 'ACCEPT']
        qua = [x for x in recs if x['verdict'] == 'QUARANTINE']
        if acc:
            accepted[m['token']] = {'name': m['name'], 'owner': m.get('owner'),
                                    'sites': acc}
        if qua:
            quarantine[m['token']] = {'name': m['name'], 'owner': m.get('owner'),
                                      'sites': qua}
    print('=== verdicts over all candidate sites ===')
    for k, v in vcount.most_common():
        print('   %-12s %d' % (k, v))
    print()
    print('=== provenance class of the reference operand ===')
    for k, v in pcount.most_common():
        print('   %-12s %d' % (k, v))
    print()
    print('=== methods ===')
    print('   with >=1 ACCEPT site     : %d  (%d sites)'
          % (len(accepted), sum(len(v['sites']) for v in accepted.values())))
    print('   with only QUARANTINE     : %d  (%d sites)'
          % (len([t for t in quarantine if t not in accepted]),
             sum(len(v['sites']) for v in quarantine.values() if True)))
    print('   QUARANTINE sites inside methods that also have ACCEPT sites: %d'
          % sum(len(quarantine[t]['sites']) for t in quarantine if t in accepted))
    json.dump(accepted, open(ROOT + '/out/afam3-accepted.json', 'w'), indent=1)
    json.dump(quarantine, open(ROOT + '/out/afam3-quarantine.json', 'w'), indent=1)
    print()
    print('written: out/afam3-accepted.json / out/afam3-quarantine.json')


if __name__ == '__main__':
    main()
