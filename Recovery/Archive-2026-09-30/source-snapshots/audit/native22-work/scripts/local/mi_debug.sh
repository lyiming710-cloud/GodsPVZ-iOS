cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import struct, json
from pathlib import Path
PC = Path('/workspaces/GodsPVZ-native19/.validation/native22/pcnative')
data = (PC/'GameAssembly.dll').read_bytes()
pe_off = struct.unpack_from('<I', data, 0x3C)[0]
coff = pe_off+4
_m, nsec, _t,_s,_ns, opt_size,_c = struct.unpack_from('<HHIIIHH', data, coff)
opt = coff+20
image_base = struct.unpack_from('<Q', data, opt+24)[0]
sections=[]
sec_off = opt+opt_size
for i in range(nsec):
    o=sec_off+i*40
    name=data[o:o+8].split(b'\0',1)[0].decode()
    vsize,va,rsize,roff=struct.unpack_from('<IIII',data,o+8)
    sections.append((name,va,max(vsize,rsize),roff,rsize))
def v2o(va):
    rva=va-image_base
    for n,va0,span,roff,rsize in sections:
        if va0<=rva<va0+span:
            d=rva-va0
            if d>=rsize: raise ValueError('tail')
            return roff+d
    raise ValueError('unmapped 0x%x'%va)
def cstr(va):
    if not va: return None
    try: o=v2o(va)
    except ValueError: return None
    e=data.find(b'\0',o)
    if e<0 or e-o>256: return None
    return data[o:e].decode('utf-8','replace')

slot=0x181ba4738
print('SLOT', hex(slot), 'contains', hex(struct.unpack_from('<Q',data,v2o(slot))[0]))
for cand_name, cand in [('slot', slot), ('deref', struct.unpack_from('<Q',data,v2o(slot))[0])]:
    print('--- try', cand_name, hex(cand))
    try:
        o=v2o(cand)
    except Exception as e:
        print('   unmapped', e); continue
    q=[struct.unpack_from('<Q',data,o+8*i)[0] for i in range(8)]
    print('   qwords:', [hex(x) for x in q])
    for nm_p in q[:6]:
        s=cstr(nm_p)
        if s: print('      field-ish string:', repr(s))
    # print every plausible cstr within first 64 bytes
    for i in range(8):
        s=cstr(q[i])
        print('      +%2d -> %-40s %r' % (i*8, hex(q[i]), (s or '')[:60]))
PY
