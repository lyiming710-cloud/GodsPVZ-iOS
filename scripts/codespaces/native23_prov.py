"""Multi-error typed CIL oracle WITH PROVENANCE TRACKING.

WHY THIS EXISTS (the question that was asked)
---------------------------------------------
`native23_multi.verify_multi` records every type clash in a body.  But once a body
already contains a clash, the abstract stack it keeps walking with may be built on
a type that is itself wrong:

  * a local may be declared `Board` when the real local was an int;
  * a merge point may have joined two incompatible types by arbitrarily picking
    one side, so everything downstream inherits a type nobody vouched for;
  * the result of an instruction that already clashed is definitionally suspect.

If we then report "I4 vs Board" and conclude "the literal must be ldnull", we may
be laundering a wrong *declaration* into a wrong *conclusion*.  "The first error
matches the single-error oracle" does NOT address this: it only proves we did not
miss the first clash.  It says nothing about whether the types we keep inferring
with are trustworthy.

WHAT THIS MODULE ADDS
---------------------
A second, parallel stack of PROVENANCE labels.  Every value on the abstract stack
remembers where its type came from:

    FIELD   : ldfld  -- the type comes from a field signature in metadata
    SFIELD  : ldsfld -- ditto, static
    ARG     : ldarg  -- the type comes from the method signature
    THIS    : ldarg.0
    CALLRET : call/callvirt/newobj -- the type comes from a resolved signature
    STRING  : ldstr
    NULL    : ldnull
    LOCAL   : ldloc  -- the type comes from a LOCALSIG declaration nobody vouches for
    ELEM    : ldelem* -- depends on an array whose element type may itself be wrong
    CONV    : box / castclass / isinst / unbox.any
    MERGE   : this value came from a control-flow merge whose two sides disagreed
    TAINTED : this value is the result of an instruction that already clashed,
              or was produced through a receiver that was already MERGE/TAINTED

Propagation rules (all conservative):
  * a clash marks the value it produces TAINTED (and `fail()` sets the pending
    provenance to TAINTED so the nominal result inherits it);
  * a merge whose two sides are not the *same* type is MERGE, even when the oracle
    can still join them by assignability;
  * a field/argument/call return whose RECEIVER is MERGE or TAINTED is demoted to
    TAINTED, because the signature tells us the type but not that we reached the
    right object.

Only provenance classes that are backed by a signature that exists in metadata and
does not depend on a local declaration or a merge are considered *independently
determined*.  Everything else is quarantined.  See native23_afam3.py.
"""
import json

INTS = {'System.Boolean', 'System.Char', 'System.Byte', 'System.SByte',
        'System.Int16', 'System.UInt16', 'System.Int32', 'System.UInt32'}

# provenance classes that a caller may treat as independently determined
TRUSTED = ('FIELD', 'SFIELD', 'ARG', 'THIS', 'CALLRET', 'STRING', 'NULL', 'NEWOBJ')


def prov_class(p):
    return p.split(':', 1)[0]


def verify_prov(d, types):
    ins = d['instructions']
    by = {x['offset']: i for i, x in enumerate(ins)}
    states = {}
    pstates = {}
    todo = []
    peak = 0
    errs = []

    def norm(t):
        if t in INTS or types.get(t, {}).get('enumType'):
            return 'I4'
        if t in ('System.Int64', 'System.UInt64'):
            return 'I8'
        if t in ('System.IntPtr', 'System.UIntPtr'):
            return 'I'
        if t in ('System.Single', 'System.Double'):
            return 'F'
        return t

    def ref(t):
        return (t == 'null' or t.endswith('[]') or
                (t in types and not types[t].get('value')
                 and not types[t].get('byref') and not types[t].get('unbound')))

    def assign(a, b):
        a, b = norm(a), norm(b)
        if a == b:
            return True
        if a == 'null':
            return ref(b)
        if b == 'System.Object' and ref(a):
            return True
        if a.endswith('[]') and b == 'System.Array':
            return True
        seen = set()
        while a in types and a not in seen:
            seen.add(a)
            a = types[a].get('baseType')
            if a == b:
                return True
        return False

    pending = ['?']       # provenance of the value most recently popped

    def fail(i, s):
        pending[0] = 'TAINTED'
        errs.append({'offset': ins[i]['offset'], 'msg': s,
                     'opcode': ins[i]['opcode']})

    def queue(i, s, ps):
        if not 0 <= i < len(ins):
            fail(min(i, len(ins) - 1) if ins else 0, 'fallthrough outside method')
            return
        s = tuple(map(norm, s))
        if i in states:
            old = states[i]
            if len(old) != len(s):
                fail(i, 'merge height %s vs %s' % (old, s))
                return
            joined = []
            jp = []
            for a, b, pa, pb in zip(old, s, pstates[i], ps):
                if a == b:
                    joined.append(a)
                    jp.append(pa if pa == pb else 'MERGE')
                elif assign(a, b):
                    joined.append(b)
                    jp.append('MERGE')
                elif assign(b, a):
                    joined.append(a)
                    jp.append('MERGE')
                else:
                    fail(i, 'merge types %s vs %s' % (a, b))
                    return
            s = tuple(joined)
            ps = tuple(jp)
            if s == old and ps == pstates[i]:
                return
        states[i] = s
        pstates[i] = tuple(ps)
        todo.append(i)

    def regions(off):
        return {(k, n) for n, h in enumerate(d['handlers'])
                for k in ('try', 'handler') if h[k + 'Start'] <= off < h[k + 'End']}

    queue(0, [], [])
    bad_eh = False
    for h in d['handlers']:
        if h['kind'] != 'Finally':
            fail(0, 'unsupported EH ' + h['kind'])
            bad_eh = True
            continue
        if not h['tryStart'] < h['tryEnd'] <= h['handlerStart'] < h['handlerEnd']:
            fail(0, 'invalid EH range')
            bad_eh = True
            continue
        for k in ('tryStart', 'tryEnd', 'handlerStart', 'handlerEnd'):
            if h[k] not in by:
                fail(0, 'invalid EH boundary ' + k)
                bad_eh = True
        if not bad_eh:
            queue(by[h['handlerStart']], [], [])
    if bad_eh:
        return errs

    while todo:
        i = todo.pop()
        x = ins[i]
        s = list(states[i])
        ps = list(pstates[i])
        opname = x['opcode']
        succ = None

        def pop(want=None):
            if not s:
                fail(i, 'stack underflow')
                return 'UNKNOWN'
            t = s.pop()
            p = ps.pop() if ps else 'TAINTED'
            if want is not None and not assign(t, want):
                fail(i, '%s is not assignable to %s' % (t, want))
                p = 'TAINTED'
            pending[0] = p
            return t

        def push(t, prov=None):
            s.append(norm(t))
            ps.append(prov if prov is not None else pending[0])

        def receiver(owner, write=False, call=False):
            """pop()s the receiver and leaves its provenance in pending[0], so the
            caller can demote the result when the object came from a polluted
            value (a signature tells us the type, not that we reached the right
            object)."""
            t = pop()
            value = types.get(owner, {}).get('value')
            valid = ((t == owner + '&' or (not write and not call and assign(t, owner)))
                     if value else assign(t, owner))
            if not valid:
                fail(i, 'receiver %s incompatible with %s (write=%s, call=%s)'
                     % (t, owner, write, call))

        a = x['operand']
        if opname == 'nop':
            pass
        elif opname == 'ldnull':
            push('null', 'NULL')
        elif opname == 'ldstr':
            push('System.String', 'STRING')
        elif opname.startswith('ldc.i4'):
            push('I4', 'CONST')
        elif opname == 'ldc.i8':
            push('I8', 'CONST')
        elif opname in ('ldc.r4', 'ldc.r8'):
            push('F', 'CONST')
        elif opname.startswith(('ldarg', 'starg')):
            ix = a['index'] if isinstance(a, dict) else int(opname.split('.')[-1])
            args = ([d['owner']] if d['hasThis'] else []) + d['args']
            t = args[ix]
            if opname.startswith('starg'):
                pop(t)
            else:
                push(t + ('&' if opname.startswith('ldarga') else ''),
                     'THIS' if (ix == 0 and d['hasThis']) else 'ARG:%d' % ix)
        elif opname.startswith(('ldloc', 'stloc')):
            ix = a['index'] if isinstance(a, dict) else int(opname.split('.')[-1])
            t = d['locals'][ix]
            if opname.startswith('stloc'):
                pop(t)
            else:
                push(t + ('&' if opname.startswith('ldloca') else ''),
                     'LOCAL:%d(%s)' % (ix, t))
        elif opname == 'dup':
            v = pop()
            p = pending[0]
            push(v, p)
            push(v, p)
        elif opname == 'pop':
            pop()
        elif opname in ('ldfld', 'ldflda', 'stfld', 'ldsfld', 'ldsflda', 'stsfld'):
            if opname in ('stfld', 'stsfld'):
                pop(a['type'])
            rp = None
            if opname in ('ldfld', 'ldflda', 'stfld'):
                receiver(a['owner'], write=opname in ('stfld', 'ldflda'))
                rp = pending[0]
            if opname.startswith('ld'):
                kind = 'SFIELD' if opname.startswith('ldsfld') else 'FIELD'
                prov = '%s:%s %s::%s' % (kind, a['type'], a['owner'],
                                         a.get('name') or a.get('identity') or '?')
                if rp is not None and prov_class(rp) in ('MERGE', 'TAINTED'):
                    prov = 'TAINTED(receiver %s)' % prov_class(rp)
                push(a['type'] + ('&' if opname.endswith('a') else ''), prov)
        elif opname in ('call', 'callvirt', 'newobj'):
            for t in reversed(a['args']):
                pop(t)
            rp = None
            if opname != 'newobj' and a['hasThis']:
                receiver(a['owner'], call=True)
                rp = pending[0]
            if opname == 'newobj':
                push(a['owner'], 'NEWOBJ:%s' % a['owner'])
            elif a['ret'] != 'System.Void':
                prov = 'CALLRET:%s' % a.get('identity', a['ret'])
                if rp is not None and prov_class(rp) in ('MERGE', 'TAINTED'):
                    prov = 'TAINTED(receiver %s)' % prov_class(rp)
                push(a['ret'], prov)
        elif opname in ('add', 'sub', 'mul', 'div', 'div.un', 'rem', 'rem.un',
                        'and', 'or', 'xor'):
            b = pop()
            c = pop()
            if b != c or b not in ('I4', 'I8', 'I', 'F'):
                fail(i, 'invalid numeric operands %s, %s' % (c, b))
            if opname in ('and', 'or', 'xor') and b == 'F':
                fail(i, 'bitwise floating operand')
            push(b, 'ARITH')
        elif opname in ('shl', 'shr', 'shr.un'):
            b = pop()
            c = pop()
            if b not in ('I4', 'I') or c not in ('I4', 'I8', 'I'):
                fail(i, 'invalid shift')
            push(c, 'ARITH')
        elif opname in ('neg', 'not'):
            t = pop()
            if t not in ('I4', 'I8', 'I', 'F') or opname == 'not' and t == 'F':
                fail(i, 'invalid unary operand')
            push(t, 'ARITH')
        elif opname.startswith('conv.'):
            t = pop()
            if t not in ('I4', 'I8', 'I', 'F'):
                fail(i, 'invalid numeric conversion ' + t)
            suffix = opname.split('.')[1]
            push('F' if suffix in ('r4', 'r8', 'r') else
                 'I8' if suffix in ('i8', 'u8') else
                 'I' if suffix in ('i', 'u') else 'I4', 'CONV')
        elif opname in ('ceq', 'cgt', 'cgt.un', 'clt', 'clt.un'):
            b = pop()
            pb = pending[0]
            c = pop()
            pc = pending[0]
            numeric = b == c and b in ('I4', 'I8', 'I', 'F')
            refs = ref(b) and ref(c) and (opname == 'ceq' or opname == 'cgt.un' and b == 'null')
            if not numeric and not refs:
                fail(i, 'invalid comparison %s, %s' % (c, b))
                errs[-1]['prov_top'] = pb
                errs[-1]['prov_second'] = pc
            push('I4', 'ARITH')
        elif opname in ('castclass', 'isinst'):
            t = pop()
            pp = pending[0]
            if not ref(t):
                fail(i, 'reference cast applied to ' + t)
            push(a['type'], 'CONV' if prov_class(pp) in TRUSTED else 'TAINTED(via %s)' % prov_class(pp))
        elif opname == 'box':
            pop(a['type'])
            push('System.Object', 'CONV')
        elif opname == 'unbox.any':
            if not ref(pop()):
                fail(i, 'unbox on non-reference')
            push(a['type'], 'CONV')
        elif opname == 'newarr':
            pop('I4')
            push(a['type'] + '[]', 'NEWARR')
        elif opname == 'ldlen':
            t = pop()
            if not t.endswith('[]'):
                fail(i, 'ldlen on ' + t)
            push('I', 'ARITH')
        elif opname.startswith(('ldelem', 'stelem')):
            val = pop() if opname.startswith('stelem') else None
            index = pop()
            arr = pop()
            pa = pending[0]
            if index not in ('I4', 'I') or not arr.endswith('[]'):
                fail(i, 'invalid array access %s[%s]' % (arr, index))
            el = arr[:-2]
            p = 'ELEM' if prov_class(pa) in TRUSTED else 'TAINTED(via %s)' % prov_class(pa)
            if val is not None:
                if not assign(val, el):
                    fail(i, 'array store %s into %s' % (val, el))
            elif opname == 'ldelema':
                if el != a['type']:
                    fail(i, 'ldelema type mismatch')
                push(el + '&', p)
            elif opname == 'ldelem.ref':
                if not ref(el):
                    fail(i, 'ldelem.ref on non-reference element')
                push(el, p)
            elif opname == 'ldelem.any':
                if el != a['type']:
                    fail(i, 'ldelem.any type mismatch')
                push(el, p)
            else:
                if norm(el) not in ('I4', 'I8', 'I', 'F'):
                    fail(i, 'primitive element mismatch')
                push(el, p)
        elif opname in ('initobj', 'ldobj', 'stobj'):
            if opname == 'stobj':
                pop(a['type'])
            pop(a['type'] + '&')
            if opname == 'ldobj':
                push(a['type'], 'CONV')
        elif opname == 'switch':
            pop('I4')
            if any(regions(t) != regions(x['offset']) for t in a['targets']):
                fail(i, 'switch crosses protected region boundary')
            succ = [by[t] for t in a['targets']] + [i + 1]
        elif opname.rstrip('.s') in ('br', 'brtrue', 'brfalse', 'leave') or \
                opname.split('.')[0] in ('beq', 'bne', 'bgt', 'blt', 'bge', 'ble'):
            base = opname[:-2] if opname.endswith('.s') else opname
            target = by[a['target']]
            if base in ('brtrue', 'brfalse'):
                t = pop()
                pt = pending[0]
                if not (t in ('I4', 'I8', 'I') or ref(t) or t.endswith('&')):
                    fail(i, 'invalid condition ' + t)
                    errs[-1]['prov_top'] = pt
            elif base not in ('br', 'leave'):
                b = pop()
                pb = pending[0]
                c = pop()
                pc = pending[0]
                if not (b == c and b in ('I4', 'I8', 'I', 'F')
                        or ref(b) and ref(c) and base in ('beq', 'bne.un')):
                    fail(i, 'invalid branch comparison %s, %s' % (c, b))
                    errs[-1]['prov_top'] = pb
                    errs[-1]['prov_second'] = pc
            if base == 'leave':
                if s:
                    fail(i, 'nonempty stack at leave')
                if any(k == 'handler' for k, n in regions(x['offset'])):
                    fail(i, 'leave from finally')
            elif regions(x['offset']) != regions(a['target']):
                fail(i, 'branch across protected region boundary')
            succ = [target] if base in ('br', 'leave') else [target, i + 1]
        elif opname == 'endfinally':
            if s or not any(k == 'handler' for k, n in regions(x['offset'])):
                fail(i, 'invalid endfinally')
            succ = []
        elif opname == 'throw':
            t = pop()
            pt = pending[0]
            if not ref(t):
                fail(i, 'throw requires reference')
                errs[-1]['prov_top'] = pt
            succ = []
        elif opname == 'ret':
            if d['ret'] != 'System.Void':
                pop(d['ret'])
            if s or regions(x['offset']):
                fail(i, 'invalid return stack or EH region')
            succ = []
        else:
            fail(i, 'unsupported opcode ' + opname)
            succ = []
        peak = max(peak, len(s))
        for tgt in succ if succ is not None else [i + 1]:
            queue(tgt, s, ps)
    if peak > d['maxStack']:
        errs.append({'offset': -1, 'msg': 'declared MaxStack too small', 'opcode': '-'})
    return errs
