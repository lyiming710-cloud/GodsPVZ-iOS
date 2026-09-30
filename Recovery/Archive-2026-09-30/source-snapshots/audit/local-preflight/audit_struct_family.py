from pathlib import Path
import sys,json,hashlib,struct
R=Path(__file__).resolve().parent;N=R.parent/'stage9-native4';out=R.parent/'stage9-native5/evidence';out.mkdir(parents=True,exist_ok=True)
sys.path.insert(0,str(N/'python-deps'))
import pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
path=N/'inputs/GameAssembly.dll';assert hashlib.sha256(path.read_bytes()).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
pe=pefile.PE(str(path));base=pe.OPTIONAL_HEADER.ImageBase;md=Cs(CS_ARCH_X86,CS_MODE_64)
functions=[(e.struct.BeginAddress,e.struct.EndAddress,e.struct.UnwindData) for e in pe.DIRECTORY_ENTRY_EXCEPTION]
def root(f,seen=None):
 seen=set() if seen is None else seen
 assert f not in seen,'Cyclic unwind chain'
 seen.add(f)
 head=pe.get_data(f[2],4)
 if head[0]>>3 & 4:
  parent=struct.unpack('<III',pe.get_data(f[2]+4+((head[2]+1)//2)*4,12))
  return root(parent,seen)
 return f
roots={f:root(f) for f in functions}
maps={int(m['token'],16):m for m in json.loads((N/'evidence/native-method-map.json').read_text()) if m['image']=='Assembly-CSharp.dll'}
targets=[m for m in json.loads((R/'native4.json').read_text(encoding='utf-8'))['results'] if any(e['category']=='struct_primitive_operation' for e in m['errors'])]
manifest=[]
for m in targets:
 token=int(m['token'],16);entry=maps[token];va=int(entry['va'],16)
 pointer=int.from_bytes(pe.get_data(0x181b82d60-base+((token&0xffffff)-1)*8,8),'little');assert pointer==va
 entryfn=next(f for f in functions if f[0]==va-base)
 fragments=sorted(f for f in functions if roots[f]==roots[entryfn])
 b=b''.join(pe.get_data(f[0],f[1]-f[0]) for f in fragments)
 lines=[m['token']+' '+m['name']+f' VA={va:#x} SIZE={len(b)} FRAGMENTS={len(fragments)}']
 for start,end,unwind in fragments:
  lines.append(f'FRAGMENT {base+start:#x}-{base+end:#x} unwind={unwind:#x}')
  decoded=list(md.disasm(pe.get_data(start,end-start),base+start))
  assert sum(i.size for i in decoded)==end-start,f'Incomplete disassembly {m["token"]} {start:x}-{end:x}, decoded={sum(i.size for i in decoded)}'
  lines.extend(f'{i.address:016X} {i.mnemonic:10s} {i.op_str}'.rstrip() for i in decoded)
 (out/(m['token']+'.native.txt')).write_text('\n'.join(lines)+'\n')
 manifest.append({'token':m['token'],'name':m['name'],'native_va':hex(va),'fragments':[{'start':hex(base+s),'end':hex(base+e),'unwind':hex(u)} for s,e,u in fragments],'size':len(b),'code_sha256':hashlib.sha256(b).hexdigest(),'scan_findings':[e for e in m['errors'] if e['category']=='struct_primitive_operation']})
(out/'struct-family-native-manifest.json').write_text(json.dumps(manifest,indent=2))
print('extracted',len(manifest),'native bodies')
