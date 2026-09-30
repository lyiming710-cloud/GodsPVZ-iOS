"""Which native VA does a GetComponent<T> call reach, grouped by the owner the
recovered IL claims? Methods with no C++ diagnostic are treated as having the
correct owner, since a wrong owner fails to compile (see the Plant::Awake case).
"""
import collections
import json
import struct
import subprocess
from pathlib import Path

NB = Path('/workspaces/GodsPVZ-native19/.validation/native22')
PC = NB / 'pcnative'
TABLE_VA = 0x181B82D60

data = (PC / 'GameAssembly.dll').read_bytes()
pe_off = struct.unpack_from('<I', data, 0x3C)[0]
coff = pe_off + 4
_m, nsec, _t, _s, _ns, opt_size, _c = struct.unpack_from('<HHIIIHH', data, coff)
opt = coff + 20
IMAGE_BASE = struct.unpack_from('<Q', data, opt + 24)[0]
SECTIONS = []
sec_off = opt + opt_size
for i in range(nsec):
    o = sec_off + i * 40
    name = data[o:o + 8].split(b'\0', 1)[0].decode('ascii', 'replace')
    vsize, va, rsize, roff = struct.unpack_from('<IIII', data, o + 8)
    SECTIONS.append((name, va, max(vsize, rsize), roff, rsize))


def v2o(va):
    rva = va - IMAGE_BASE
    for n, va0, span, roff, rsize in SECTIONS:
        if va0 <= rva < va0 + span:
            d = rva - va0
            if d >= rsize:
                raise ValueError('tail')
            return roff + d
    raise ValueError('unmapped')


EXTENT = {}
for n, va0, span, roff, rsize in SECTIONS:
    if n == '.pdata':
        for o in range(roff, roff + rsize - 11, 12):
            b, e, u = struct.unpack_from('<III', data, o)
            if b:
                EXTENT[IMAGE_BASE + b] = IMAGE_BASE + e


def calls_of(va):
    end = EXTENT.get(va)
    if end is None:
        return None
    blob = data[v2o(va):v2o(end - 1) + 1]
    Path('/tmp/sl.bin').write_bytes(blob)
    r = subprocess.run(['objdump', '-D', '-b', 'binary', '-m', 'i386:x86-64',
                        '--adjust-vma=0x%x' % va, '/tmp/sl.bin'],
                       capture_output=True, text=True)
    out = []
    for line in r.stdout.splitlines():
        parts = line.split('\t')
        if len(parts) < 3:
            continue
        insn = parts[2]
        if not (insn.startswith('call') or insn.startswith('jmp')):
            continue
        tok = insn.split()[-1]
        if tok.startswith('0x'):
            out.append(int(tok, 16))
    return out


cls = json.loads((NB / 'out' / 'classification.json').read_text())
methods = {m['token']: m for m in cls['methods']}
bodies = json.loads((NB / 'out' / 'all-methods.json').read_text())
by_tok = {m['token']: m for m in bodies['methods']}


def receiver_type_of(b, op_idx):
    """Best-effort static type of the receiver on the stack at op_idx."""
    stack = []
    for x in b['instructions'][:op_idx]:
        op = x['opcode']
        o = x['operand'] or {}
        if op == 'ldarg.0' and b['hasThis']:
            stack.append(b['owner'])
        elif op.startswith('ldarg') and not op.startswith('ldarga'):
            ix = o.get('index') if isinstance(o, dict) else int(op.split('.')[-1])
            args = ([b['owner']] if b['hasThis'] else []) + b['args']
            stack.append(args[ix] if ix < len(args) else '?')
        elif op.startswith('ldloc') and not op.startswith('ldloca'):
            ix = o.get('index') if isinstance(o, dict) else int(op.split('.')[-1])
            ls = b['locals']
            stack.append(ls[ix] if ix < len(ls) else '?')
        elif op in ('ldfld', 'ldsfld'):
            stack.append(o.get('type'))
        elif op == 'pop':
            if stack:
                stack.pop()
        elif op.startswith('stloc'):
            if stack:
                stack.pop()
    return stack[-1] if stack else None


for want in ('UnityEngine.GameObject', 'UnityEngine.Component'):
    print('#' * 96)
    print('### recovered IL owner = %s' % want)
    groups = collections.defaultdict(list)
    sample_tokens = []
    for tok, b in by_tok.items():
        for i, x in enumerate(b['instructions']):
            o = x['operand'] or {}
            if isinstance(o, dict) and o.get('name') == 'GetComponent' and o.get('owner') == want:
                m = methods.get(tok, {})
                key = 'cpp-error' if 'cpp' in m else 'cpp-clean'
                if len(sample_tokens) < 400:
                    groups[key].append(tok)
                break
    for key in ('cpp-clean', 'cpp-error'):
        toks = groups[key][:14]
        print('-- %s : %d candidates (sampling %d)' % (key, len(groups[key]), len(toks)))
        for tok in toks:
            rid = int(tok, 16) & 0xFFFFFF
            try:
                tgt = struct.unpack_from('<Q', data, v2o(TABLE_VA + (rid - 1) * 8))[0]
                cs = calls_of(tgt)
            except ValueError:
                continue
            if cs is None:
                print('   %s (no pdata extent)' % tok)
                continue
            # report only targets that look like small non-method helpers are noise; print all unique
            print('   %s  VA 0x%x  calls: %s' % (tok, tgt, ' '.join(hex(c) for c in cs[:8])))
