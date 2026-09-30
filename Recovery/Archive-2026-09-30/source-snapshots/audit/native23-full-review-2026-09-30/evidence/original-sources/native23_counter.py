"""Native23 -- counterexample / regression suite for the repair rule.

A rule that "replaces an integer zero by ldnull" is only trustworthy if it also
*refuses* the cases where that replacement is wrong.  This suite builds minimal
method bodies, runs the real rule on them, and asserts the outcome.

Two kinds of case:
  MUST-ACCEPT   a genuine reference null-check written with the wrong constant
  MUST-REFUSE   a genuine integer zero, a non-zero literal, an already-correct
                body, or a value whose type we cannot vouch for

The MUST-REFUSE set is the important one: each of these is a shape a sloppier
rule would silently corrupt.
"""
import json
import sys

import native23_afam as A
import native23_afam3 as C
import native23_prov as P

TYPES = {
    'GameObject': {'baseType': 'System.Object'},
    'Board': {'baseType': 'System.Object'},
    'Owner': {'baseType': 'System.Object'},
    'System.Object': {},
    'System.Int32': {'value': True},
    'System.Boolean': {'value': True},
    'System.Void': {'value': True},
    'System.String': {},
}


class B(object):
    def __init__(self, name, locals_, args=(), ret='System.Void', hasthis=True):
        self.d = {'token': '0x06TEST%02d' % B.n, 'name': name,
                  'owner': 'Owner', 'args': list(args), 'locals': list(locals_),
                  'ret': ret, 'hasThis': hasthis, 'handlers': [],
                  'maxStack': 8, 'instructions': []}
        B.n += 1

    n = 0

    def i(self, op, operand=None):
        self.d['instructions'].append(
            {'opcode': op, 'offset': len(self.d['instructions']) * 2,
             'operand': operand})
        return self

    def fld(self, op, t, name='f'):
        return self.i(op, {'owner': 'Owner', 'type': t, 'name': name})

    def ldloc(self, k):
        return self.i('ldloc', {'index': k})

    def stloc(self, k):
        return self.i('stloc', {'index': k})

    def zero(self):
        return self.i('ldc.i4.0')

    def done(self):
        return self.d


def ref_nullcheck_via_field():
    b = B('TRUE-NULLCHECK-FIELD', ['System.Boolean'])
    b.i('ldarg.0').fld('ldfld', 'GameObject').zero().i('ceq').stloc(0).i('ret')
    return b.done(), 'ACCEPT', 'FIELD'


def ref_nullcheck_via_arg():
    b = B('TRUE-NULLCHECK-ARG', ['System.Boolean'], args=['GameObject'])
    b.i('ldarg', {'index': 1}).zero().i('ceq').stloc(0).i('ret')
    return b.done(), 'ACCEPT', 'ARG'


def ref_nullcheck_via_sfield():
    b = B('TRUE-NULLCHECK-SFIELD', ['System.Boolean'])
    b.fld('ldsfld', 'GameObject').zero().i('ceq').stloc(0).i('ret')
    return b.done(), 'ACCEPT', 'SFIELD'


def int_zero_compare():
    b = B('INT-ZERO-COMPARE', ['System.Boolean'])
    b.i('ldarg.0').fld('ldfld', 'System.Int32').zero().i('ceq').stloc(0).i('ret')
    return b.done(), 'NO-FLIP', None


def nonzero_literal():
    b = B('NONZERO-LITERAL', ['System.Boolean'])
    b.i('ldarg.0').fld('ldfld', 'GameObject').i('ldc.i4', {'value': 3})
    b.i('ceq').stloc(0).i('ret')
    return b.done(), 'NO-FLIP', None


def int_and_ref_in_one_method():
    """A genuine int zero test AND a genuine null-check in the same body.
    Only the second may be flipped."""
    b = B('MIXED-INT-AND-REF', ['System.Boolean', 'System.Boolean'])
    b.i('ldarg.0').fld('ldfld', 'System.Int32', 'n').zero().i('ceq').stloc(0)
    b.i('ldarg.0').fld('ldfld', 'GameObject', 'p').zero().i('ceq').stloc(1)
    b.i('ret')
    # instruction index 7 is the SECOND zero literal -- the one that follows the
    # GameObject field load.  Index 2 is the genuine integer zero and must stay.
    return b.done(), 'ACCEPT-AT', 7


def already_correct():
    b = B('ALREADY-CORRECT', ['System.Boolean'])
    b.i('ldarg.0').fld('ldfld', 'GameObject').i('ldnull').i('ceq').stloc(0)
    b.i('ret')
    return b.done(), 'NO-FLIP', None


def zero_into_int_local():
    b = B('ZERO-INTO-INT-LOCAL', ['System.Int32'])
    b.zero().stloc(0).i('ret')
    return b.done(), 'NO-FLIP', None


def via_local():
    """The reference reaches the comparison through a local, so its type rests
    on the localsig, which is exactly what we do not trust."""
    b = B('VIA-LOCAL', ['GameObject', 'System.Boolean'])
    b.i('ldarg.0').fld('ldfld', 'GameObject').stloc(0)
    b.ldloc(0).zero().i('ceq').stloc(1).i('ret')
    return b.done(), 'QUARANTINE', 'LOCAL'


def zero_into_ref_local():
    """Assignment family: `I4 is not assignable to GameObject`."""
    b = B('ZERO-INTO-REF-LOCAL', ['GameObject'])
    b.zero().stloc(0).i('ret')
    return b.done(), 'QUARANTINE', None


def main():
    cases = [ref_nullcheck_via_field(), ref_nullcheck_via_arg(),
             ref_nullcheck_via_sfield(), int_zero_compare(), nonzero_literal(),
             int_and_ref_in_one_method(), already_correct(),
             zero_into_int_local(), via_local(), zero_into_ref_local()]
    npass = nfail = 0
    for body, want, extra in cases:
        r = C.solve(body, TYPES)
        recs = r['recs'] if r else []
        verdicts = {x['verdict'] for x in recs}
        classes = {x['prov_class'] for x in recs}
        flips = sorted({x['flip_offset'] for x in recs})
        ok = False
        detail = ''
        if want == 'NO-FLIP':
            ok = not recs
            detail = 'flips=%d' % len(flips)
        elif want == 'ACCEPT':
            ok = verdicts == {'ACCEPT'} and extra in classes
            detail = 'verdicts=%s classes=%s' % (sorted(verdicts), sorted(classes))
        elif want == 'QUARANTINE':
            ok = bool(recs) and verdicts == {'QUARANTINE'}
            if extra:
                ok = ok and extra in classes
            detail = 'verdicts=%s classes=%s' % (sorted(verdicts), sorted(classes))
        elif want == 'ACCEPT-AT':
            idx = extra
            want_off = body['instructions'][idx]['offset']
            ok = (verdicts == {'ACCEPT'} and flips == [want_off])
            detail = 'flips=%s wanted=[%d] verdicts=%s' % (
                flips, want_off, sorted(verdicts))
        # nothing may be left broken
        if recs:
            applied = json.loads(json.dumps(body))
            for off in flips:
                for x in applied['instructions']:
                    if x['offset'] == off:
                        x['opcode'] = 'ldnull'
                        x['operand'] = None
            left = P.verify_prov(applied, TYPES)
            if left:
                ok = False
                detail += '  LEFTOVER=%s' % [e['msg'] for e in left]
        print('%-5s %-24s want=%-12s %s'
              % ('PASS' if ok else 'FAIL', body['name'], want, detail))
        npass += ok
        nfail += not ok
    print()
    print('counterexample suite: %d passed, %d failed' % (npass, nfail))
    return 1 if nfail else 0


if __name__ == '__main__':
    sys.exit(main())
