from pathlib import Path
import sys,struct,re,json
sys.path.insert(0,'audit/stage9-native3/repository/Tools/Stage9NativeProbe')
from pc_method_probe import PE
raw=Path('audit/stage9-native3/inputs/GameAssembly.dll').read_bytes();pe=PE(raw)
b=Path('audit/stage9-native3/inputs/global-metadata.dat').read_bytes()
def u(o):return struct.unpack_from('<I',b,o)[0]
def st(i):return b[so+i:b.index(0,so+i)].decode()
def nq(va):return struct.unpack_from('<Q',raw,pe.va_to_offset(va))[0]
def offset_va(off):
    s=next(s for s in pe.sections if s.raw_offset<=off<s.raw_offset+s.raw_size)
    return pe.image_base+s.va+off-s.raw_offset
so=u(24);to=u(160);ts=u(164);mo=u(48);io=u(168);isz=u(172)
out=[]
for im in range(isz//40):
    p=io+40*im;name=st(u(p));start=u(p+8);count=u(p+12)
    if name!='UnityEngine.InputLegacyModule.dll':continue
    modules=[]
    for pos in re.finditer(re.escape(name.encode()+b'\0'),raw):
        va=offset_va(pos.start())
        for ref in re.finditer(re.escape(struct.pack('<Q',va)),raw):
            a=ref.start();cnt=struct.unpack_from('<Q',raw,a+8)[0];ptr=struct.unpack_from('<Q',raw,a+16)[0]
            if 1<cnt<10000 and pe.image_base<ptr<pe.image_base+0x3000000:
                try:pe.va_to_offset(ptr)
                except ValueError:continue
                modules.append((offset_va(a),cnt,ptr))
    print('MODULE',name,modules)
    if len(modules)!=1:raise RuntimeError('Ambiguous codegen module')
    module_va,cnt,ptr=modules[0]
    for ti in range(start,start+count):
        t=to+ti*88;tn=st(u(t));ns=st(u(t+4));ms=u(t+36);mc=struct.unpack_from('<H',b,t+64)[0]
        for j in range(mc):
            p=mo+36*(ms+j);token=u(p+24);rid=token&0xffffff;va=nq(ptr+8*(rid-1));method=st(u(p))
            if method in ('GetAxis','GetAxisRaw')or va==0x1813645C0:
                out.append({'image':name,'module_va':hex(module_va),'method_pointers_va':hex(ptr),'type':ns+'.'+tn,'name':method,'token':hex(token),'metadata_method_index':ms+j,'va':hex(va),'parameter_count':struct.unpack_from('<H',b,p+34)[0]})
Path('audit/native73-codex-2026-10-12/input-native-map.json').write_text(json.dumps(out,indent=2))
print(json.dumps(out,indent=2))
