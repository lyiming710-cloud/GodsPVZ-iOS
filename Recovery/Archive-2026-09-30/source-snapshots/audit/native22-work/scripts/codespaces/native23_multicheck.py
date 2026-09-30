"""Parity gate for native23_multi.py.

The multi-error oracle must never be WEAKER than verify_native19_types.verify.
Concretely, for every method:
  * if the original verifier passes, the multi oracle must record zero clashes;
  * if the original verifier raises, the FIRST clash recorded here must reproduce
    the original message exactly (name + IL offset + text).
Any mismatch is reported and must be fixed before the multi oracle is used.
"""
import json
import sys

import verify_native19_types as V
import native23_multi as M

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'


def main():
    dump = sys.argv[1] if len(sys.argv) > 1 else ROOT + '/work/all-methods.batch1.json'
    data = json.load(open(dump))
    types = data['types']
    bad = 0
    n_pass = n_fail = 0
    extra = 0
    for m in data['methods']:
        try:
            V.verify(m, types)
            orig = None
        except V.InvalidIL as e:
            orig = str(e)
        except Exception as e:  # noqa
            orig = 'CRASH %s: %s' % (type(e).__name__, e)
        errs = M.verify_multi(m, types)
        if orig is None:
            n_pass += 1
            if errs:
                bad += 1
                extra += len(errs)
                if bad <= 5:
                    print('MISMATCH(orig PASS, multi found %d) %s %s'
                          % (len(errs), m['token'], errs[0]))
            continue
        n_fail += 1
        if not errs:
            bad += 1
            if bad <= 5:
                print('MISMATCH(orig FAIL, multi clean) %s : %s' % (m['token'], orig))
            continue
        e0 = errs[0]
        rebuilt = '%s IL_%04X: %s' % (m['name'], e0['offset'], e0['msg'])
        if rebuilt != orig and e0['msg'] != orig:
            bad += 1
            if bad <= 5:
                print('MISMATCH %s\n   orig : %s\n   multi: %s' % (m['token'], orig, rebuilt))
    print('methods: %d   orig-PASS=%d   orig-FAIL=%d' % (len(data['methods']), n_pass, n_fail))
    print('parity mismatches: %d   (extra clashes reported on orig-PASS methods: %d)' % (bad, extra))
    print('RESULT: %s' % ('PARITY OK' if bad == 0 else 'PARITY BROKEN -- do not use'))
    return 1 if bad else 0


if __name__ == '__main__':
    sys.exit(main())
