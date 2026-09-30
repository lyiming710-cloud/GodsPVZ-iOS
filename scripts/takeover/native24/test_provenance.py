"""Discriminating controls for the new conservative source gate."""
import copy
import json
import pathlib
import sys
sys.dont_write_bytecode = True
import provenance as P
import reproduce_findings as R


def solve(d):
    previous = R.C.P
    try:
        R.C.P = P
        return R.C.solve(d, R.TYPES)
    finally:
        R.C.P = previous


def accepted(result):
    return result is not None and any(x['verdict'] == 'ACCEPT' for x in result['recs'])


def main():
    tests = []
    for d in R.pollution_cases():
        result = solve(d)
        tests.append({'case': d['name'], 'accepted': accepted(result), 'result': result,
                      'errors': P.verify_prov(d, R.TYPES)})
        assert not accepted(result), d['name']
    # Stores are actually tracked: loading the changed argument never recreates an ARG label.
    direct = R.body('direct field positive', [('ldarg', {'index': 0}), ('ldfld', R.field),
                    ('ldc.i4.0', None), ('ceq', None), ('pop', None), ('ret', None)], args=['Owner'])
    arg = R.body('direct argument positive', [('ldarg', {'index': 0}), ('ldc.i4.0', None),
                 ('ceq', None), ('pop', None), ('ret', None)], args=['GameObject'])
    static = R.body('static field positive', [('ldsfld', R.field), ('ldc.i4.0', None),
                    ('ceq', None), ('pop', None), ('ret', None)])
    cleanlocal = R.body('clean local remains unproved', [('ldarg', {'index': 0}),
                 ('stloc', {'index': 0}), ('ldloc', {'index': 0}), ('ldfld', R.field),
                 ('ldc.i4.0', None), ('ceq', None), ('pop', None), ('ret', None)],
                 locals=['Owner'], args=['Owner'])
    integer = R.body('real integer zero', [('ldarg', {'index': 0}), ('ldc.i4.0', None),
                      ('ceq', None), ('pop', None), ('ret', None)], args=['System.Int32'])
    for d in [direct, arg, static, cleanlocal, integer]:
        result = solve(d)
        want = d in [direct, arg, static]
        assert accepted(result) == want, d['name']
        tests.append({'case': d['name'], 'accepted': accepted(result), 'expected': want, 'result': result})
    # Error in the first actual argument must survive a later valid argument pop.
    multiple = R.body('bad earlier call argument', [('ldc.i4.1', None), ('ldstr', 'good'),
        ('call', {'owner': 'Owner', 'hasThis': False, 'args': ['System.String', 'System.String'],
                  'ret': 'Owner', 'name': 'GetOwner', 'identity': 'GetOwner(String,String)'}),
        ('ldfld', R.field), ('ldc.i4.0', None), ('ceq', None), ('pop', None), ('ret', None)])
    result = solve(multiple)
    assert not accepted(result)
    tests.append({'case': multiple['name'], 'accepted': accepted(result), 'result': result})
    # Invalid EH input receives no automatic source proof.
    eh = copy.deepcopy(direct); eh['handlers'] = [{'kind': 'Catch', 'tryStart': 0,
                       'tryEnd': 2, 'handlerStart': 2, 'handlerEnd': 12}]
    result = solve(eh)
    assert not accepted(result)
    tests.append({'case': 'EH not modeled', 'accepted': accepted(result), 'result': result})
    target = pathlib.Path(sys.argv[1])
    target.write_text(json.dumps({'scope': 'E2 only, not E1/E5/E6', 'tests': tests, 'passed': len(tests)}, indent=2)+'\n')
    print(f'PASS: {len(tests)} source controls including 4 independently reviewed negatives')


if __name__ == '__main__':
    main()
