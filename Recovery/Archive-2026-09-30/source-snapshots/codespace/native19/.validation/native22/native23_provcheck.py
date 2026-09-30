"""Parity gate for native23_prov.verify_prov against native23_multi.verify_multi.

The provenance layer must be OBSERVATIONALLY INVISIBLE: for every method the two
oracles must report exactly the same clashes, in the same order.  Only then may
the provenance labels attached to those clashes be used to decide which sites are
independently determined.

Note this is a SECOND, independent gate from native23_multicheck.py.  That one
proves the multi oracle is not weaker than the single-error oracle; this one
proves adding provenance did not perturb the multi oracle.  Neither of them says
anything about whether a type used in an inference is trustworthy -- that is what
the provenance labels themselves are for.
"""
import sys

import native23_multi as M
import native23_prov as P

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'


def main():
    dump = sys.argv[1] if len(sys.argv) > 1 else ROOT + '/work/all-methods.batch1.json'
    import json
    data = json.load(open(dump))
    types = data['types']
    bad = 0
    tot = 0
    for m in data['methods']:
        e1 = [(e['offset'], e['msg']) for e in M.verify_multi(m, types)]
        e2 = [(e['offset'], e['msg']) for e in P.verify_prov(m, types)]
        tot += len(e1)
        if e1 != e2:
            bad += 1
            if bad <= 5:
                print('MISMATCH %s' % m['token'])
                print('   multi: %s' % e1[:3])
                print('   prov : %s' % e2[:3])
    print('methods: %d   clashes: %d   mismatches: %d'
          % (len(data['methods']), tot, bad))
    print('RESULT: %s' % ('PARITY OK' if bad == 0 else 'PARITY BROKEN -- do not use'))
    return 1 if bad else 0


if __name__ == '__main__':
    sys.exit(main())
