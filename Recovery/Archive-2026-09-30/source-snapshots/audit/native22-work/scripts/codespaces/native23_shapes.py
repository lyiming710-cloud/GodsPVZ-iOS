"""Shape census of the candidate sites: which IL shape does each repaired site
have, for the ACCEPTED and the QUARANTINED sets.

This matters because the real-CLR calibration (behave/cal) showed that only
SOME shapes are behaviourally discriminating:

  * `... ; <zero> ; ceq`        -> CLR cannot tell the two constants apart
  * `... ; <zero> ; call`       -> CLR throws InvalidProgramException on the
                                   damaged form, so it IS observable
"""
import collections
import json
import re

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'


def main():
    data = json.load(open(ROOT + '/work/all-methods.batch1.json'))
    by_tok = {m['token']: m for m in data['methods']}
    for name in ('afam3-accepted', 'afam3-quarantine'):
        sites = json.load(open(ROOT + '/out/%s.json' % name))
        shape = collections.Counter()
        fam = collections.Counter()
        n = 0
        for tok, info in sites.items():
            body = by_tok.get(tok)
            if body is None:
                continue
            by = {x['offset']: i for i, x in enumerate(body['instructions'])}
            for s in info['sites']:
                n += 1
                i = by.get(s['flip_offset'])
                msg = s.get('clash_msg') or ''
                m = re.match(r'^(\w+)', msg)
                fam[m.group(1) if m else '?'] += 1
                if i is None:
                    shape['<unmapped>'] += 1
                    continue
                prev = body['instructions'][i-1]['opcode'] if i > 0 else '<entry>'
                nxt = body['instructions'][i+1]['opcode'] if i+1 < len(body['instructions']) else '<end>'
                shape['%s ; <zero> ; %s' % (prev, nxt)] += 1
        print('=== %s : %d sites in %d methods ===' % (name, n, len(sites)))
        print('  -- clash message family --')
        for k, v in fam.most_common():
            print('     %-14s %d' % (k, v))
        print('  -- IL shape around the zero literal --')
        for k, v in shape.most_common(12):
            print('     %-30s %d' % (k, v))
        print()


if __name__ == '__main__':
    main()
