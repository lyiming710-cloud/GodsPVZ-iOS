"""Print the authoritative MethodBody evidence for selected tokens.

Every datum below traces to the baseline dll
  sha256 0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f
via three enumerated hops (IL dump == fresh export; cpp == fresh conversion;
clang == canary tree hashes matched against input/cpp-manifest.json).
"""
import json
import os
import sys

import verify_native19_types as V

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'
BM = json.load(open(ROOT + '/out/body-map.json'))
DATA = json.load(open(ROOT + '/out/all-methods.json'))
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
        b = BODIES.get(tok)
        print('=' * 104)
        print('TOKEN %s' % tok)
        if b is None:
            print('  !! not present in IL dump')
            continue
        print('  name      : %s' % b['name'])
        print('  owner     : %s   hasThis=%s   ret=%s' % (b.get('owner'), b.get('hasThis'), b.get('ret')))
        print('  args      : %s' % json.dumps(b.get('args'), ensure_ascii=False))
        print('  maxStack  : %s' % b.get('maxStack'))
        print('  locals    : %s' % json.dumps(
            ['%d:%s' % (i, t) for i, t in enumerate(b.get('locals', []))], ensure_ascii=False))
        h = b.get('handlers') or []
        print('  handlers  : %s' % json.dumps(h, ensure_ascii=False))
        print()
        print('  IL:')
        for x in b['instructions']:
            print('    IL_%04X  %-14s %s' % (x['offset'], x['opcode'], desc(x)))
        print()
        try:
            err = None
            try:
                V.verify(b, DATA['types'])
            except V.InvalidIL as e:
                err = str(e)
            except Exception as e:  # noqa
                err = 'VERIFIER-CRASH %s: %s' % (type(e).__name__, e)
            print('  typed-verify: %s' % (err if err else 'PASS'))
        except Exception as e:
            print('  typed-verify: unavailable (%s)' % e)
        m = BM.get(tok)
        print()
        if not m:
            print('  cpp body  : <no IL2CPP symbol for this token>')
        else:
            print('  cpp symbol: %s' % m['symbol'])
            print('  cpp file  : %s  sha256=%s  body lines %d..%d' % (
                m['file'], m['file_sha256'][:16], m['start'], m['end']))
            ns = len(m['errors'])
            print('  cpp errors: %d' % ns)
            for e in m['errors']:
                print('     :%d:%d  %s' % (e['line'], e['col'], e['msg'][:150]))
        print()


if __name__ == '__main__':
    main(sys.argv[1:] or [])
