"""Print the IL context around A-family flip candidates so a human can eyeball
that each one really is a null check (and not, say, a genuine integer zero)."""
import json
import sys

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'
CAND = json.load(open(ROOT + '/out/afam2-candidates.json'))
DATA = json.load(open(ROOT + '/work/all-methods.batch1.json'))
BODIES = {m['token']: m for m in DATA['methods']}


def desc(op):
    o = op['operand']
    if o is None:
        return ''
    if isinstance(o, dict):
        return o.get('identity', json.dumps(o, ensure_ascii=False))
    return json.dumps(o, ensure_ascii=False)


def main(tokens):
    for tok in tokens:
        tok = '0x' + tok.lower().replace('0x', '').upper().rjust(8, '0')
        c = CAND.get(tok)
        b = BODIES.get(tok)
        if c is None or b is None:
            print('!! %s not a candidate (%s/%s)' % (tok, c is not None, b is not None))
            continue
        marks = {f['offset'] for f in c['flips']}
        rem = {(r['offset']): r['msg'] for r in c['removed']}
        print('=' * 100)
        print('%s  %s   [%s]  flips=%d' % (tok, c['name'], c['status'], len(c['flips'])))
        print('  owner=%s  locals=%s' % (c['owner'],
                                         ', '.join('%d:%s' % (i, t) for i, t in enumerate(b.get('locals', []))) or '-'))
        print('  clashes removed by the flips:')
        for o, m in sorted(rem.items()):
            print('     IL_%04X  %s' % (o, m))
        print('  clashes still remaining: %d' % len(c['remaining']))
        for r in c['remaining'][:4]:
            print('     IL_%04X  %s' % (r['offset'], r['msg'][:110]))
        print('  clang: %s  (%s)' % (c['clang_file'], len(c['clang'])))
        for m in c['clang'][:4]:
            print('     %s' % m[:120])
        print('  IL (>>> marks a flip):')
        for x in b['instructions']:
            tag = '>>>' if x['offset'] in marks else '   '
            print('    %s IL_%04X  %-14s %s' % (tag, x['offset'], x['opcode'], desc(x)))
        print()


if __name__ == '__main__':
    main(sys.argv[1:] or [])
