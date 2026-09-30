"""Multi-error variant of verify_native19_types.verify.

WHY
---
verify_native19_types.verify raises on the FIRST type clash, so a method with five
independent problems only ever reports one.  That makes it impossible to see how
many of the "integer literal zero vs reference" clashes a body actually contains,
and it makes a greedy repair loop stall (Native23 batch 2 first attempt: only 10
of 869 failing methods could be driven to PASS).

WHAT IT DOES DIFFERENTLY
------------------------
Identical type lattice, identical rules, identical worklist order.  The only change
is that `fail` records (offset, message) and lets the walk continue instead of
raising, so one pass yields EVERY reachable clash.

PARITY REQUIREMENT (self-check, see native23_multicheck.py)
------------------------------------------------------------
For every method that the original verifier rejects, the FIRST error recorded here
must be byte-identical to the original's exception message.  If that ever stops
holding, this module is weaker than the oracle and must not be used.
"""
import json
import sys

INTS = {'System.Boolean', 'System.Char', 'System.Byte', 'System.SByte',
        'System.Int16', 'System.UInt16', 'System.Int32', 'System.UInt32'}


def verify_multi(d, types):
    ins = d['instructions']
    by = {x['offset']: i for i, x in enumerate(ins)}
    states = {}
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

    def fail(i, s):
        errs.append({'offset': ins[i]['offset'], 'msg': s,
                     'opcode': ins[i]['opcode']})

    def queue(i, s):
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
            for a, b in zip(old, s):
                if a == b:
                    joined.append(a)
                elif assign(a, b):
                    joined.append(b)
                elif assign(b, a):
                    joined.append(a)
                else:
                    fail(i, 'merge types %s vs %s' % (a, b))
                    return
            s = tuple(joined)
            if s == old:
                return
        states[i] = s
        todo.append(i)

    def regions(off):
        return {(k, n) for n, h in enumerate(d['handlers'])
                for k in ('try', 'handler') if h[k + 'Start'] <= off < h[k + 'End']}

    queue(0, [])
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
            queue(by[h['handlerStart']], [])
    if bad_eh:
        return errs

    while todo:
        i = todo.pop()
        x = ins[i]
        s = list(states[i])
        op = x['opcode']
        a = x['operand']
        succ = None

        def pop(want=None):
            if not s:
                fail(i, 'stack underflow')
                return 'UNKNOWN'
            t = s.pop()
            if want is not None and not assign(t, want):
                fail(i, '%s is not assignable to %s' % (t, want))
            return t

        def push(t):
            s.append(norm(t))

        def receiver(owner, write=False, call=False):
            t = pop()
            value = types.get(owner, {}).get('value')
            valid = ((t == owner + '&' or (not write and not call and assign(t, owner)))
                     if value else assign(t, owner))
            if not valid:
                fail(i, 'receiver %s incompatible with %s (write=%s, call=%s)'
                     % (t, owner, write, call))

        if op == 'nop':
            pass
        elif op == 'ldnull':
            push('null')
        elif op == 'ldstr':
            push('System.String')
        elif op.startswith('ldc.i4'):
            push('I4')
        elif op == 'ldc.i8':
            push('I8')
        elif op in ('ldc.r4', 'ldc.r8'):
            push('F')
        elif op.startswith(('ldarg', 'starg')):
            ix = a['index'] if isinstance(a, dict) else int(op.split('.')[-1])
            args = ([d['owner']] if d['hasThis'] else []) + d['args']
            t = args[ix]
            if op.startswith('starg'):
                pop(t)
            else:
                push(t + ('&' if op.startswith('ldarga') else ''))
        elif op.startswith(('ldloc', 'stloc')):
            ix = a['index'] if isinstance(a, dict) else int(op.split('.')[-1])
            t = d['locals'][ix]
            if op.startswith('stloc'):
                pop(t)
            else:
                push(t + ('&' if op.startswith('ldloca') else ''))
        elif op == 'dup':
            v = pop()
            push(v)
            push(v)
        elif op == 'pop':
            pop()
        elif op in ('ldfld', 'ldflda', 'stfld', 'ldsfld', 'ldsflda', 'stsfld'):
            if op in ('stfld', 'stsfld'):
                pop(a['type'])
            if op in ('ldfld', 'ldflda', 'stfld'):
                receiver(a['owner'], write=op in ('stfld', 'ldflda'))
            if op.startswith('ld'):
                push(a['type'] + ('&' if op.endswith('a') else ''))
        elif op in ('call', 'callvirt', 'newobj'):
            for t in reversed(a['args']):
                pop(t)
            if op != 'newobj' and a['hasThis']:
                receiver(a['owner'], call=True)
            if op == 'newobj':
                push(a['owner'])
            elif a['ret'] != 'System.Void':
                push(a['ret'])
        elif op in ('add', 'sub', 'mul', 'div', 'div.un', 'rem', 'rem.un',
                    'and', 'or', 'xor'):
            b = pop()
            c = pop()
            if b != c or b not in ('I4', 'I8', 'I', 'F'):
                fail(i, 'invalid numeric operands %s, %s' % (c, b))
            if op in ('and', 'or', 'xor') and b == 'F':
                fail(i, 'bitwise floating operand')
            push(b)
        elif op in ('shl', 'shr', 'shr.un'):
            b = pop()
            c = pop()
            if b not in ('I4', 'I') or c not in ('I4', 'I8', 'I'):
                fail(i, 'invalid shift')
            push(c)
        elif op in ('neg', 'not'):
            t = pop()
            if t not in ('I4', 'I8', 'I', 'F') or op == 'not' and t == 'F':
                fail(i, 'invalid unary operand')
            push(t)
        elif op.startswith('conv.'):
            t = pop()
            if t not in ('I4', 'I8', 'I', 'F'):
                fail(i, 'invalid numeric conversion ' + t)
            suffix = op.split('.')[1]
            push('F' if suffix in ('r4', 'r8', 'r') else
                 'I8' if suffix in ('i8', 'u8') else
                 'I' if suffix in ('i', 'u') else 'I4')
        elif op in ('ceq', 'cgt', 'cgt.un', 'clt', 'clt.un'):
            b = pop()
            c = pop()
            numeric = b == c and b in ('I4', 'I8', 'I', 'F')
            refs = ref(b) and ref(c) and (op == 'ceq' or op == 'cgt.un' and b == 'null')
            if not numeric and not refs:
                fail(i, 'invalid comparison %s, %s' % (c, b))
            push('I4')
        elif op in ('castclass', 'isinst'):
            t = pop()
            if not ref(t):
                fail(i, 'reference cast applied to ' + t)
            push(a['type'])
        elif op == 'box':
            pop(a['type'])
            push('System.Object')
        elif op == 'unbox.any':
            if not ref(pop()):
                fail(i, 'unbox on non-reference')
            push(a['type'])
        elif op == 'newarr':
            pop('I4')
            push(a['type'] + '[]')
        elif op == 'ldlen':
            t = pop()
            if not t.endswith('[]'):
                fail(i, 'ldlen on ' + t)
            push('I')
        elif op.startswith(('ldelem', 'stelem')):
            val = pop() if op.startswith('stelem') else None
            index = pop()
            arr = pop()
            if index not in ('I4', 'I') or not arr.endswith('[]'):
                fail(i, 'invalid array access %s[%s]' % (arr, index))
            el = arr[:-2]
            if val is not None:
                if not assign(val, el):
                    fail(i, 'array store %s into %s' % (val, el))
            elif op == 'ldelema':
                if el != a['type']:
                    fail(i, 'ldelema type mismatch')
                push(el + '&')
            elif op == 'ldelem.ref':
                if not ref(el):
                    fail(i, 'ldelem.ref on non-reference element')
                push(el)
            elif op == 'ldelem.any':
                if el != a['type']:
                    fail(i, 'ldelem.any type mismatch')
                push(el)
            else:
                if norm(el) not in ('I4', 'I8', 'I', 'F'):
                    fail(i, 'primitive element mismatch')
                push(el)
        elif op in ('initobj', 'ldobj', 'stobj'):
            if op == 'stobj':
                pop(a['type'])
            pop(a['type'] + '&')
            if op == 'ldobj':
                push(a['type'])
        elif op == 'switch':
            pop('I4')
            if any(regions(t) != regions(x['offset']) for t in a['targets']):
                fail(i, 'switch crosses protected region boundary')
            succ = [by[t] for t in a['targets']] + [i + 1]
        elif op.rstrip('.s') in ('br', 'brtrue', 'brfalse', 'leave') or \
                op.split('.')[0] in ('beq', 'bne', 'bgt', 'blt', 'bge', 'ble'):
            base = op[:-2] if op.endswith('.s') else op
            target = by[a['target']]
            if base in ('brtrue', 'brfalse'):
                t = pop()
                if not (t in ('I4', 'I8', 'I') or ref(t) or t.endswith('&')):
                    fail(i, 'invalid condition ' + t)
            elif base not in ('br', 'leave'):
                b = pop()
                c = pop()
                if not (b == c and b in ('I4', 'I8', 'I', 'F')
                        or ref(b) and ref(c) and base in ('beq', 'bne.un')):
                    fail(i, 'invalid branch comparison %s, %s' % (c, b))
            if base == 'leave':
                if s:
                    fail(i, 'nonempty stack at leave')
                if any(k == 'handler' for k, n in regions(x['offset'])):
                    fail(i, 'leave from finally')
            elif regions(x['offset']) != regions(a['target']):
                fail(i, 'branch across protected region boundary')
            succ = [target] if base in ('br', 'leave') else [target, i + 1]
        elif op == 'endfinally':
            if s or not any(k == 'handler' for k, n in regions(x['offset'])):
                fail(i, 'invalid endfinally')
            succ = []
        elif op == 'throw':
            if not ref(pop()):
                fail(i, 'throw requires reference')
            succ = []
        elif op == 'ret':
            if d['ret'] != 'System.Void':
                pop(d['ret'])
            if s or regions(x['offset']):
                fail(i, 'invalid return stack or EH region')
            succ = []
        else:
            fail(i, 'unsupported opcode ' + op)
            succ = []
        peak = max(peak, len(s))
        for target in succ if succ is not None else [i + 1]:
            queue(target, s)
    if peak > d['maxStack']:
        errs.append({'offset': -1, 'msg': 'declared MaxStack too small',
                     'opcode': '-'})
    return errs
