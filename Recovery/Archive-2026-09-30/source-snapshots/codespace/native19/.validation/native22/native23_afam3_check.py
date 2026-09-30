"""Validate the ACCEPT-only subset as a standalone batch (batch 2a).

Applying FEWER flips than the greedy set could in principle change the picture,
so nothing is assumed:

  C1  no clash may be ADDED relative to the unpatched body (a subset of flips
      cannot add, but we measure it instead of arguing it);
  C2  every ACCEPT flip must still remove the clash it claims to remove, when
      applied together with only the other ACCEPT flips;
  C3  every ACCEPT flip must still be individually necessary inside that subset;
  C4  report how many QUARANTINE clashes deliberately remain unfixed.
"""
import copy
import json
import sys

import native23_afam as A
import native23_prov as P

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'


def key(e):
    return (e['offset'], e['msg'])


def main():
    dump = sys.argv[1] if len(sys.argv) > 1 else ROOT + '/work/all-methods.batch1.json'
    data = json.load(open(dump))
    types = data['types']
    acc = json.load(open(ROOT + '/out/afam3-accepted.json'))
    qua = json.load(open(ROOT + '/out/afam3-quarantine.json'))
    bodies = {m['token']: m for m in data['methods']}

    added_total = 0
    not_removing = []
    not_necessary = []
    n_sites = 0
    for tok, v in acc.items():
        b = bodies[tok]
        offs = [s['flip_offset'] for s in v['sites']]
        idx = {}
        for k, x in enumerate(b['instructions']):
            idx[x['offset']] = k
        positions = [idx[o] for o in offs]
        E0 = {key(e) for e in P.verify_prov(b, types)}
        full = copy.deepcopy(b)
        for i in positions:
            full['instructions'][i]['opcode'] = 'ldnull'
            full['instructions'][i]['operand'] = None
        Ef = {key(e) for e in P.verify_prov(full, types)}
        added_total += len(Ef - E0)
        if Ef - E0:
            for x in sorted(Ef - E0)[:3]:
                print('  ADDED %s IL_%04X %s' % (tok, x[0], x[1][:80]))
        # C2 / C3
        for off, i in zip(offs, positions):
            n_sites += 1
            without = copy.deepcopy(b)
            for j in positions:
                if j == i:
                    continue
                without['instructions'][j]['opcode'] = 'ldnull'
                without['instructions'][j]['operand'] = None
            Ew = {key(e) for e in P.verify_prov(without, types)}
            if not (Ew - Ef):
                not_necessary.append((tok, off))
            # the clash this site claims to remove must be absent from Ef
            want = [s for s in v['sites'] if s['flip_offset'] == off][0]
            if (want['clash_offset'], want['clash_msg']) in Ef:
                not_removing.append((tok, off))
    print('=== C1 clashes ADDED by the ACCEPT-only subset ===')
    print('   %d' % added_total)
    print('=== C2 sites that no longer remove their claimed clash ===')
    print('   %d   %s' % (len(not_removing), not_removing[:5]))
    print('=== C3 sites that are NOT individually necessary inside the subset ===')
    print('   %d   %s' % (len(not_necessary), not_necessary[:5]))
    print()
    print('sites checked: %d' % n_sites)
    print('quarantined sites deliberately left unfixed: %d (in %d methods)'
          % (sum(len(x['sites']) for x in qua.values()), len(qua)))
    print('methods touched by the ACCEPT-only subset: %d' % len(acc))
    ok = (added_total == 0 and not not_removing and not not_necessary)
    print('RESULT: %s' % ('ACCEPT-ONLY SUBSET IS SELF-CONSISTENT' if ok else 'PROBLEM'))


if __name__ == '__main__':
    main()
