cd /workspaces/GodsPVZ-native19/.validation/native22
python3 - <<'PY'
import struct, subprocess
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
    raise ValueError('unmapped')
pdata = [s for s in sections if s[0]=='.pdata'][0]
ext={}
for o in range(pdata[3], pdata[3]+pdata[4]-11, 12):
    b,e,u=struct.unpack_from('<III',data,o)
    if b: ext[image_base+b]=(image_base+e)
def dis(va):
    end=ext.get(va)
    if end is None: return 'NO PDATA EXTENT'
    blob=data[v2o(va):v2o(end-1)+1]
    Path('/tmp/x.bin').write_bytes(blob)
    r=subprocess.run(['objdump','-D','-b','binary','-m','i386:x86-64','--adjust-vma=0x%x'%va,'/tmp/x.bin'],capture_output=True,text=True)
    return r.stdout
for va in [0x18046dc80, 0x18024fef0, 0x18024f360, 0x180250150, 0x180cb86f0]:
    print('='*90)
    print('TARGET 0x%x  extent -> 0x%x (%d bytes)' % (va, ext.get(va,0), (ext.get(va,0)-va) if ext.get(va) else -1))
    print(dis(va))
PY
