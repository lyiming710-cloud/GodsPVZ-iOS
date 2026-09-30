"""Run the fail-closed typed CIL verifier over a whole IL dump."""
import json
import sys

import verify_native19_types as V


def load(path):
    return json.load(open(path))


def verify_all(dump):
    data = dump if isinstance(dump, dict) else load(dump)
    types = data['types']
    out = {}
    for m in data['methods']:
        tok = m['token']
        try:
            V.verify(m, types)
            out[tok] = None
        except V.InvalidIL as e:
            out[tok] = str(e)
        except Exception as e:  # noqa
            out[tok] = 'CRASH %s: %s' % (type(e).__name__, e)
    return out


if __name__ == '__main__':
    base = {}
    prev = None
    for path in sys.argv[1:]:
        r = verify_all(path)
        base[path] = r
        n_fail = sum(1 for v in r.values() if v)
        print('%s : methods=%d  verify-FAIL=%d' % (path.split('/')[-1], len(r), n_fail))
        if prev is not None:
            fixed = sorted(t for t in r if prev[t] and not r[t])
            broke = sorted(t for t in r if not prev[t] and r[t])
            still = sorted(t for t in r if prev[t] and r[t])
            print('   transitions vs previous dump:')
            print('     FAIL -> PASS : %s' % (fixed or 'none'))
            print('     PASS -> FAIL : %s' % (broke or 'none'))
            print('     still failing: %d' % len(still))
        prev = r
