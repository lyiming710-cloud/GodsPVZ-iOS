"""Native23 -- CONCRETE EXECUTION behaviour test for the repaired methods.

WHAT THIS IS
------------
This is NOT a type checker.  It is a concrete CIL interpreter that actually
*runs* the method bodies -- the damaged body and the repaired body -- on
generated object graphs, and records what happens: which branches are taken,
which calls are made, what is thrown, and, above all, **what kind of runtime
value is actually sitting on the stack at each repaired site**.

WHY THE LAST QUESTION IS THE ONE THAT MATTERS
---------------------------------------------
The repair replaces the constant `ldc.i4 0` by `ldnull`.  Measured on a real
CLR (.NET 10.0.12, see behave/cal) the two forms are *indistinguishable* at a
`ceq`: `ref(null) vs 0` and `ref(null) vs null` both give 1, and even a WRONG
flip (`int 0 vs null`) gives 1 as well.  So a runtime trace comparison between
the damaged and the repaired body can never validate these flips.  Anyone who
presented that comparison as "behaviour testing" would be doing exactly what
the user warned against.

What IS observable by execution is different: at the moment the constant is
pushed, what is the *runtime kind* of the value it is compared against?  If it
is a reference, `ldnull` is the right constant and `ldc.i4 0` is a mis-typed
zero.  If it is an integer, the repair is wrong.  Crucially this is decided by
*actual data flow* -- through locals that may be wrongly declared, through
merge points, through values that were stored earlier -- not by re-reading the
same declaration the provenance analysis already read.

THE HONEST PART
---------------
Values that the method never writes itself (fields of `this`, arguments,
results of calls to code we do not have) are UNKNOWN to us.  They are
initialised from their declared signature.  For those, execution reproduces the
static evidence rather than adding to it, and the script says so: each site
records the ORIGIN of the observed value, and `origin=init` means "this came
from a declaration we assumed, not from something the method computed".
"""
import collections
import json
import random
import sys

ROOT = '/workspaces/GodsPVZ-native19/.validation/native22'

INTS = {'System.Boolean', 'System.Char', 'System.Byte', 'System.SByte',
        'System.Int16', 'System.UInt16', 'System.Int32', 'System.UInt32'}

BASE_WORLDS = [
    # name,          ref fields null, numbers zero, call returns null
    ('A-nullref',    True,  True,  False),
    ('B-objref',     False, False, False),
    ('C-nullret',    True,  True,  True),
    ('D-objref-nz',  False, False, True),
    ('E-objref-z',   False, True,  False),
    ('F-nullref-nz', True,  False, False),
]

# branch policy for conditions whose value is unknown to us:
#   0 / 1     -> always take that direction
#   'r<int>'  -> pseudo-random, different seed per run, so different paths get
#                explored without pretending we know the answer
BRANCH_POLICIES = [0, 1, 'r1', 'r2', 'r3', 'r4']

WORLDS = [(n, rf, nz, cr, br)
          for (n, rf, nz, cr) in BASE_WORLDS for br in BRANCH_POLICIES]

STEP_BUDGET = 100000


# ---------------------------------------------------------------- type utils
def norm(t, types):
    if t in INTS or types.get(t, {}).get('enumType'):
        return 'I4'
    if t in ('System.Int64', 'System.UInt64'):
        return 'I8'
    if t in ('System.IntPtr', 'System.UIntPtr'):
        return 'I'
    if t in ('System.Single', 'System.Double'):
        return 'F'
    return t


def kind_of(t, types):
    n = norm(t, types)
    if n in ('I4', 'I'):
        return 'I4'
    if n == 'I8':
        return 'I8'
    if n == 'F':
        return 'F'
    if t == 'null' or t.endswith('[]'):
        return 'REF'
    if t in types and types[t].get('value'):
        return 'VAL'
    return 'REF'


def lit_value(x):
    a = x.get('operand')
    if isinstance(a, dict):
        for k in ('value', 'int', 'operand'):
            if k in a:
                return a[k]
        return None
    return a


def idx_of(x, default=None):
    a = x.get('operand')
    if isinstance(a, dict) and 'index' in a:
        return a['index']
    op = x['opcode']
    if '.' in op:
        try:
            return int(op.rsplit('.', 1)[1])
        except ValueError:
            return default
    return default


# ------------------------------------------------------------- the machine
class Stop(Exception):
    pass


class Machine(object):
    def __init__(self, body, types, world, flips, rng=None):
        self.ins = body['instructions']
        self.by = {x['offset']: i for i, x in enumerate(self.ins)}
        self.types = types
        self.body = body
        self.w = world
        self.rng = rng
        self.flips = flips            # instruction indices that are repair sites
        self.heap = []
        self.stack = []
        self.locals = []
        self.args = []
        self.events = []
        self.sites = {}               # site index -> {'kind':..,'origin':..}
        self.status = 'RUNNING'

    # ---- value constructors -------------------------------------------
    def dv(self, t, origin='init'):
        """default/init value of declared type t under the current world"""
        k = kind_of(t, self.types)
        if k == 'REF':
            return ('REF', None if self.w[1] else ('O', t), origin)
        if k == 'VAL':
            return ('VAL', ('S', t), origin)
        if k == 'F':
            return ('F', 0.0 if self.w[2] else 1.0, origin)
        return (k, 0 if self.w[2] else 1, origin)

    def callret(self, t):
        k = kind_of(t, self.types)
        if k == 'REF':
            return ('REF', None if self.w[3] else ('O', t), 'callret')
        if k == 'VAL':
            return ('VAL', ('S', t), 'callret')
        if k == 'F':
            return ('F', None, 'callret')
        return (k, None, 'callret')          # unknown number

    def new_heap_obj(self, t):
        self.heap.append({})
        return ('REF', len(self.heap) - 1, 'newobj')

    # ---- stack ---------------------------------------------------------
    def push(self, v):
        self.stack.append(v)

    def pop(self):
        if not self.stack:
            raise Stop('UNDERFLOW')
        return self.stack.pop()

    def is_null(self, v):
        return v[0] == 'REF' and v[1] is None

    def truthy(self, v):
        """returns True / False / None(unknown)"""
        if v[0] == 'REF':
            return not self.is_null(v)
        if v[2] is None or v[1] is None:
            return None
        if v[0] in ('I4', 'I8'):
            return v[1] != 0
        if v[0] == 'F':
            return v[1] != 0.0
        return None

    def zeroish(self, v):
        """for the mixed ref-vs-integer comparison: is this value the zero one"""
        if v[0] == 'REF':
            return self.is_null(v)
        if v[0] in ('I4', 'I8', 'F'):
            if v[1] is None:
                return None
            return v[1] == 0
        return None

    # ---- execution ------------------------------------------------------
    def run(self):
        pc = 0
        steps = 0
        try:
            while True:
                if pc >= len(self.ins):
                    raise Stop('FELL-OFF-END')
                steps += 1
                if steps > STEP_BUDGET:
                    raise Stop('BUDGET')
                x = self.ins[pc]
                op = x['opcode']
                a = x.get('operand')
                nxt = pc + 1
                if pc in self.flips:
                    top = self.stack[-1] if self.stack else None
                    self.sites.setdefault(pc, []).append(
                        (top[0] if top else '<empty>',
                         top[2] if top else '<empty>'))

                base = op[:-2] if op.endswith('.s') else op
                if op == 'nop':
                    pass
                elif op == 'ldnull':
                    self.push(('REF', None, 'const'))
                elif op == 'ldstr':
                    self.push(('REF', ('O', 'System.String'), 'ldstr'))
                elif op.startswith('ldc.i4'):
                    v = 0 if op == 'ldc.i4.0' else lit_value(x)
                    self.push(('I4', v, 'const'))
                elif op == 'ldc.i8':
                    self.push(('I8', lit_value(x), 'const'))
                elif op in ('ldc.r4', 'ldc.r8'):
                    self.push(('F', lit_value(x), 'const'))
                elif op == 'ldtoken':
                    self.push(('VAL', ('S', 'System.RuntimeTypeHandle'), 'ldtoken'))
                elif op.startswith('ldarg'):
                    i = idx_of(x, 0)
                    self.push(self.args[i])
                elif op.startswith('starg'):
                    i = idx_of(x, 0)
                    self.args[i] = self.pop()
                elif op.startswith('ldloc'):
                    i = idx_of(x, 0)
                    if op == 'ldloca':
                        self.push(('REF', ('A', i), 'addr'))
                    else:
                        self.push(self.locals[i])
                elif op.startswith('stloc'):
                    i = idx_of(x, 0)
                    self.locals[i] = self.pop()
                elif op == 'dup':
                    v = self.pop()
                    self.push(v)
                    self.push(v)
                elif op == 'pop':
                    self.pop()
                elif op in ('ldfld', 'stfld'):
                    ftype = a['type']
                    fname = a.get('name') or a.get('identity') or '?'
                    if op == 'stfld':
                        val = self.pop()
                    recv = self.pop()
                    if self.is_null(recv):
                        raise Stop('NRE-receiver')
                    if op == 'ldfld':
                        if recv[0] == 'REF' and isinstance(recv[1], int):
                            obj = self.heap[recv[1]]
                            if fname not in obj:
                                obj[fname] = self.dv(ftype)
                            self.push(obj[fname])
                        else:
                            self.push(self.dv(ftype))
                elif op in ('ldsfld', 'stsfld'):
                    if op == 'stsfld':
                        self.pop()
                    else:
                        self.push(self.dv(a['type']))
                elif op in ('call', 'callvirt', 'newobj'):
                    nargs = len(a.get('args') or [])
                    for _ in range(nargs):
                        self.pop()
                    if op != 'newobj' and a.get('hasThis'):
                        recv = self.pop()
                        if op == 'callvirt' and self.is_null(recv):
                            raise Stop('NRE-callvirt')
                    self.events.append('CALL:' + str(a.get('name') or a.get('owner') or '?'))
                    if op == 'newobj':
                        self.push(self.new_heap_obj(a['owner']))
                    elif a.get('ret') not in (None, 'System.Void'):
                        self.push(self.callret(a['ret']))
                elif op in ('add', 'sub', 'mul', 'div', 'div.un', 'rem', 'rem.un',
                            'and', 'or', 'xor', 'shl', 'shr', 'shr.un'):
                    b = self.pop()
                    c = self.pop()
                    k = b[0] if b[0] in ('I4', 'I8', 'F') else c[0]
                    self.push((k, None, 'arith'))
                elif op in ('neg', 'not'):
                    c = self.pop()
                    self.push((c[0], None, 'arith'))
                elif op.startswith('conv.'):
                    c = self.pop()
                    s = op.split('.')[1]
                    k = 'F' if s in ('r4', 'r8', 'r') else 'I8' if s in ('i8', 'u8') \
                        else 'I4' if s in ('i', 'u') else 'I4'
                    self.push((k, None, 'conv'))
                elif op in ('ceq', 'cgt', 'cgt.un', 'clt', 'clt.un'):
                    b = self.pop()
                    c = self.pop()
                    self.push(('I4', self.cmp(op, c, b), 'cmp'))
                elif op in ('castclass', 'isinst'):
                    self.pop()
                    self.push(('REF', ('O', a['type']), 'cast'))
                elif op == 'box':
                    self.pop()
                    self.push(('REF', ('O', 'System.Object'), 'box'))
                elif op == 'unbox.any':
                    self.pop()
                    self.push(self.dv(a['type'], 'unbox'))
                elif op == 'newarr':
                    self.pop()
                    self.heap.append({'#arr': 4})
                    self.push(('REF', len(self.heap) - 1, 'newarr'))
                elif op == 'ldlen':
                    self.pop()
                    self.push(('I4', None, 'ldlen'))
                elif op.startswith(('ldelem', 'stelem')):
                    if op.startswith('stelem'):
                        self.pop()
                    self.pop()
                    arr = self.pop()
                    if self.is_null(arr):
                        raise Stop('NRE-array')
                    if op.startswith('ld'):
                        self.push(self.dv(a['type'], 'ldelem'))
                elif op == 'br':
                    nxt = self.by[a['target']]
                elif op in ('brtrue', 'brfalse'):
                    c = self.pop()
                    t = self.truthy(c)
                    if t is None:
                        brm = self.w[4]
                        if isinstance(brm, str):
                            t = bool(self.rng.getrandbits(1))
                        else:
                            t = bool(brm)
                    take = t if op == 'brtrue' else (not t)
                    self.events.append('BR:%04X:%s' % (x['offset'], 'T' if take else 'F'))
                    nxt = self.by[a['target']] if take else pc + 1
                elif op == 'switch':
                    self.pop()
                    nxt = self.by[a['targets'][0]]
                elif op == 'ret':
                    if self.body.get('ret') not in (None, 'System.Void'):
                        self.pop()
                    raise Stop('RET')
                elif op == 'throw':
                    self.pop()
                    raise Stop('THROW')
                else:
                    raise Stop('UNSUPPORTED:' + op)
                pc = nxt
        except Stop as e:
            self.status = str(e)
        except (IndexError, KeyError, TypeError) as e:
            self.status = 'ABORT:%s:%s' % (type(e).__name__, e)
        return self

    def cmp(self, op, c, b):
        """ceq/cgt/... on runtime values.

        Reproduces the behaviour measured on the real CLR (behave/cal):
          * ref vs ref        -> identity
          * num vs num        -> numeric
          * ref vs integer 0  -> compares null-ness against zero-ness, which is
                                 EXACTLY what CoreCLR does, and is why the two
                                 forms of the constant are indistinguishable.
        """
        if c[0] == 'REF' and b[0] == 'REF':
            if self.is_null(c) and self.is_null(b):
                return 1
            if self.is_null(c) or self.is_null(b):
                return 0
            if isinstance(c[1], int) and isinstance(b[1], int):
                return 1 if c[1] == b[1] else 0
            return None
        if c[0] != 'REF' and b[0] != 'REF':
            if c[1] is None or b[1] is None:
                return None
            a1, a2 = c[1], b[1]
            if op == 'ceq':
                return 1 if a1 == a2 else 0
            if op == 'clt':
                return 1 if a1 < a2 else 0
            if op == 'cgt':
                return 1 if a1 > a2 else 0
            return None
        # mixed:
        z1, z2 = self.zeroish(c), self.zeroish(b)
        if z1 is None or z2 is None:
            return None
        return 1 if z1 == z2 else 0

    def setup(self):
        types = self.types
        args = ([('REF', ('O', self.body['owner']), 'this')] if self.body.get('hasThis')
                else [])
        for t in self.body.get('args') or []:
            args.append(self.dv(t))
        self.args = args
        self.locals = [self.dv(t) for t in (self.body.get('locals') or [])]
        return self


# ------------------------------------------------------------------ driver
def run_body(body, types, world, flips):
    rng = random.Random('%s|%s' % (world[0], world[4]))
    m = Machine(body, types, world, set(flips), rng).setup().run()
    return m


def main():
    dump = sys.argv[1] if len(sys.argv) > 1 else ROOT + '/work/all-methods.batch1.json'
    acc_path = sys.argv[2] if len(sys.argv) > 2 else ROOT + '/out/afam3-accepted.json'
    data = json.load(open(dump))
    types = data['types']
    acc = json.load(open(acc_path))
    by_tok = {m['token']: m for m in data['methods']}

    site_verdict = {}
    trace_div = 0
    methods_run = 0
    status_hist = collections.Counter()
    origin_hist = collections.Counter()
    kind_hist = collections.Counter()
    per_method = {}

    for tok, info in acc.items():
        body = by_tok.get(tok)
        if body is None:
            continue
        methods_run += 1
        by = {x['offset']: i for i, x in enumerate(body['instructions'])}
        flips = [by[s['flip_offset']] for s in info['sites'] if s['flip_offset'] in by]
        if not flips:
            continue

        repaired = json.loads(json.dumps(body))
        for i in flips:
            repaired['instructions'][i]['opcode'] = 'ldnull'
            repaired['instructions'][i]['operand'] = None

        seen = {}
        diverged = False
        for world in WORLDS:
            md = run_body(body, types, world, flips)      # damaged
            mr = run_body(repaired, types, world, flips)  # repaired
            status_hist[md.status] += 1
            if md.events != mr.events:
                diverged = True
            for i, obs in md.sites.items():
                for k, o in obs:
                    seen.setdefault(i, set()).add((k, o))
        if diverged:
            trace_div += 1

        for i in flips:
            obs = seen.get(i)
            if not obs:
                v = 'UNREACHED'
            else:
                kinds = {k for k, _ in obs}
                if kinds <= {'REF'}:
                    v = 'REF-ONLY'
                elif 'REF' in kinds:
                    v = 'MIXED'
                else:
                    v = 'NON-REF'
                for k, o in obs:
                    kind_hist[k] += 1
                    origin_hist[o] += 1
            site_verdict[(tok, body['instructions'][i]['offset'])] = v
        per_method[tok] = {'name': info['name'], 'n_sites': len(flips)}

    vc = collections.Counter(site_verdict.values())
    print('=== concrete execution over the ACCEPTED methods ===')
    print('   methods executed                 : %d' % methods_run)
    print('   repair sites                     : %d' % len(site_verdict))
    print()
    print('=== verdict per site (runtime kind actually observed at the site) ===')
    for k, v in vc.most_common():
        print('   %-12s %d' % (k, v))
    print()
    print('=== runtime kinds observed ===')
    for k, v in kind_hist.most_common():
        print('   %-10s %d' % (k, v))
    print()
    print('=== ORIGIN of the observed value (init = assumed from a declaration) ===')
    for k, v in origin_hist.most_common():
        print('   %-10s %d' % (k, v))
    print()
    print('=== damaged vs repaired trace divergence ===')
    print('   methods whose event trace DIFFERED: %d' % trace_div)
    print()
    print('=== execution status of the damaged body (one per method x world) ===')
    for k, v in status_hist.most_common():
        print('   %-22s %d' % (k, v))

    out = {'per_method': per_method,
           'sites': {'%s@%04X' % (t, o): v for (t, o), v in site_verdict.items()}}
    json.dump(out, open(ROOT + '/out/behave-sites.json', 'w'), indent=1)
    print()
    print('written: out/behave-sites.json')


if __name__ == '__main__':
    main()
