from pathlib import Path
import sys,struct,json,hashlib
R=Path(__file__).resolve().parents[1];sys.path.insert(0,str(R/'stage9-native4/python-deps'))
import pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
src=R/'stage9-native4/inputs'; out=R/'native15-evidence';out.mkdir(exist_ok=True)
data=(src/'GameAssembly.dll').read_bytes();meta=(src/'global-metadata.dat').read_bytes()
assert hashlib.sha256(data).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
assert hashlib.sha256(meta).hexdigest()=='ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9'
pe=pefile.PE(data=data);base=pe.OPTIONAL_HEADER.ImageBase
def n(va,k):return pe.get_data(va-base,k)
def q(va):return struct.unpack('<Q',n(va,8))[0]
def u(buf,p):return struct.unpack_from('<I',buf,p)[0]
reg=0x1818C6D00;code=0x1815E88C0
for name,addr in [('metadata',reg),('code',code)]:
 print(name,[(i,hex(q(addr+i)),hex(q(addr+i+8))) for i in range(0,128,16)])
so=u(meta,24);mo=u(meta,48)
def st(i):return meta[so+i:meta.index(0,so+i)].decode()
types=json.loads((R/'stage9-native4/evidence/native-types.json').read_text())
targets={}
for tn,mn in [('ElementManager','CreateNewElements'),('VFXAnimationEvent','Binding'),('Map','RandomGet_Grid_TestPlace'),('VFXAnimationEvent','SetSorting'),('SwfAssocList`1','Remove'),('SwfList`1','UnorderedRemoveAt'),('SwfList`1','AssignTo')]:
 t=next(x for x in types if x['name']==tn)
 for idx in range(t['methodStart'],t['methodStart']+t['methods']):
  p=mo+36*idx
  if st(u(meta,p))==mn:targets[idx]={'type':tn,'name':mn,'token':hex(u(meta,p+24))}
print('targets',targets)
spec_count=q(reg+64);spec_ptr=q(reg+72);table_count=q(reg+32);table_ptr=q(reg+40)
ptr_count=q(code+16);ptrs=q(code+24)
specs={i:struct.unpack('<iii',n(spec_ptr+12*i,12)) for i in range(spec_count)}
selected={i:s for i,s in specs.items() if s[0] in targets}
print('specs',selected)
result=[]
for i in range(table_count):
 vals=struct.unpack('<iiii',n(table_ptr+16*i,16))
 if vals[0] not in selected:continue
 spec=selected[vals[0]];va=q(ptrs+8*vals[1]); target=targets[spec[0]]
 entry=next(e for e in pe.DIRECTORY_ENTRY_EXCEPTION if e.struct.BeginAddress==va-base)
 end=base+entry.struct.EndAddress;raw=n(va,end-va);ins=list(Cs(CS_ARCH_X86,CS_MODE_64).disasm(raw,va))
 assert sum(x.size for x in ins)==len(raw)
 row=dict(target,global_method=spec[0],method_spec=vals[0],class_inst=spec[1],method_inst=spec[2],table_entry=i,pointer_index=vals[1],va=hex(va),end=hex(end),size=len(raw),sha256=hashlib.sha256(raw).hexdigest())
 print(row);result.append(row)
 (out/f'{target["type"]}-{va:x}.asm').write_text('\n'.join(f'{x.address:x} {x.mnemonic} {x.op_str}' for x in ins))
(out/'mapping.json').write_text(json.dumps(result,indent=2))
for inst in [85,3607,2393,2453,4899]:
 ip=q(q(reg+24)+8*inst);argc=q(ip);argv=q(ip+8)
 print('INST',inst,[(hex(q(q(argv+8*j))),hex(q(q(argv+8*j)+8))) for j in range(argc)])
for va in [0x181bb1fc8,0x181b9cf08]:
 usage=q(va);ti=(usage&0x1fffffff)>>1;tp=q(q(reg+56)+8*ti);definition=q(tp)
 print('SLOT',hex(va),hex(usage),'type_index',ti,'definition',definition,'name',types[definition]['name'])
