"""Read original native/metadata facts only; no tests or recovered application execution."""
from pathlib import Path
import json,struct,sys,hashlib,re
R=Path(__file__).resolve().parents[2]
source=R/'audit/native80-spec-2026-10-13/inspect_original.py'
# Reuse the previously inspectible read-only original PE/metadata helpers.
exec(source.read_text().split('cells=[]',1)[0])
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
md=Cs(CS_ARCH_X86,CS_MODE_64);md.detail=True
from capstone.x86 import X86_OP_MEM,X86_OP_IMM,X86_REG_RIP
out={'source_hashes':{p.name:hashlib.sha256(p.read_bytes()).hexdigest()for p in [R/'audit/stage9-native3/inputs/GameAssembly.dll',R/'audit/stage9-native3/inputs/global-metadata.dat']},'read_only_parser_dependency':{'path':str(source.relative_to(R)),'sha256':hashlib.sha256(source.read_bytes()).hexdigest()},'methods':[],'fields':[],'types':[],'cells':[],'float_constants':[],'native_bodies':[],'helper_native':[],'independent_code_module_mapping':[]}
selected={'PathDataEditor':['InsertNewPath','LoadData'],'Path_enemyEditor':['LoadData'],'EnemyPath':['.ctor'],'HoleTeleport':['.ctor'],'Hide':['.ctor'],'Buff':['.ctor'],'AttackRange':['.ctor'],'DeviceEvent':['.ctor'],'BuffManager':['AddBuff'],'GlobalStaticVars':['AudioVolume','CreateAudioAtPoint'],'Object':['Instantiate'],'Transform':['SetParent','get_position'],'Component':['get_transform'],'Camera':['get_main'],'List`1':['Insert','.ctor']}
for t in types:
 if t['name']not in selected:continue
 if t['name']=='List`1'and t['namespace']!='System.Collections.Generic':continue
 tp=to+t['index']*88
 parent=u(b,tp+16)
 out['types'].append(dict(t,parent_type=typ(parent)if parent!=0xffffffff else None))
 for j in range(t['methods']):
  m=method(t['methodStart']+j)
  if m['name']not in selected[t['name']]:continue
  out['methods'].append(dict(m,native_map=[x for x in maps if int(x['token'],16)==int(m['token'],16)and x['type']==t['name']]))
for f in fields:
 if(f['type']in ['PathDataEditor','Path_enemyEditor','Enemy_boardEditor','HoleTeleport','DeviceEvent','Hide','Buff','AttackRange'])or(f['type']=='Enemy'and f['name']in ['gridX','row','myPath'])or(f['type']=='Zombie'and f['name']=='buffManager')or(f['type']=='ResourceManager'and f['name']=='zombieClips'):
  out['fields'].append(dict(f,decoded_type=typ(f['type_index'])))
cell_addresses=[0x181b9e828,0x181bb3df0,0x181baa508,0x181bb7db8,0x181bb0500,0x181ba2018,0x181ba35d0,0x181ba4278,0x181bb6478]
# Complete bodies of named original constructors, plus original generic helper fragments.
for va,label in [(0x18031c900,'Hide.ctor(bool,Zombie)'),(0x180310bb0,'Buff.ctor()'),(0x1803299e0,'EnemyPath.ctor(int,int,float,BoardConfig)'),(0x1803002a0,'AttackRange.ctor()')]:
 e=next(e for e in pe.DIRECTORY_ENTRY_EXCEPTION if e.struct.BeginAddress<=va-base<e.struct.EndAddress)
 end=base+e.struct.EndAddress;data=native(va,end-va);ins=[]
 for i in md.disasm(data,va):
  r={'address':hex(i.address),'bytes':i.bytes.hex(),'mnemonic':i.mnemonic,'operands':i.op_str}
  for op in i.operands:
   if op.type==X86_OP_MEM and op.mem.base==X86_REG_RIP:
    target=i.address+i.size+op.mem.disp;r['rip_target']=hex(target)
    if i.mnemonic=='lea'and base<=target<base+pe.OPTIONAL_HEADER.SizeOfImage:cell_addresses.append(target)
  ins.append(r)
 out['helper_native'].append({'label':label,'start':hex(va),'end':hex(end),'sha256':hashlib.sha256(data).hexdigest(),'instructions':ins})
for va,label,size in [(0x180302170,'shared no-op Object.ctor',1),(0x18030f300,'DeviceEvent.ctor thunk to Object.ctor',7),(0x18084d9c0,'List<T>.Insert shared body fragment',200),(0x1804a0690,'Object.Instantiate<Path_enemyEditor> shared fragment',200)]:
 data=native(va,size)
 out['helper_native'].append({'label':label,'start':hex(va),'bytes':size,'sha256':hashlib.sha256(data).hexdigest(),'scope':'source fragment; named generic closure is fixed by original passed MethodInfo usage cell','instructions':[{'address':hex(i.address),'bytes':i.bytes.hex(),'mnemonic':i.mnemonic,'operands':i.op_str}for i in md.disasm(data,va)]})
for va in sorted(set(cell_addresses)):
 val=nq(va);kind=(val>>29)&7;idx=(val>>1)&0xfffffff
 if not(val&1)or kind==0:continue
 r={'cell':hex(va),'encoded':hex(val),'bytes':native(va,8).hex(),'kind':kind,'source_index':idx}
 if kind==6:r['method_spec']=ms(idx)
 elif kind in [1,2]:r['type']=typ(idx)
 elif kind==5:
  lo,ld=u(b,8),u(b,16);ln,di=struct.unpack_from('<II',b,lo+8*idx)
  r['literal']={'record_offset':hex(lo+8*idx),'data_offset':hex(ld+di),'bytes':b[ld+di:ld+di+ln].hex(),'text':b[ld+di:ld+di+ln].decode('utf-8')}
 out['cells'].append(r)
for va in [0x1815a7ac8,0x1815a7acc]:out['float_constants'].append({'va':hex(va),'bytes':native(va,4).hex(),'value':struct.unpack('<f',native(va,4))[0]})
D=json.loads((R/'audit/native30-continuation-2026-10-02/native55-native.json').read_text())
for m in D['methods']:
 if m['token']not in ['0x06000056','0x06000119']:continue
 data=b''.join(native(a,z-a)for a,z in m['pdata_parts'])
 parts=[]
 for a,z in m['pdata_parts']:
  e=next(e for e in pe.DIRECTORY_ENTRY_EXCEPTION if e.struct.BeginAddress<=a-base<e.struct.EndAddress)
  ud=native(e.struct.UnwindData+base,4);parts.append({'start':hex(a),'end':hex(z),'unwind_address':hex(base+e.struct.UnwindData),'unwind_header_bytes':ud.hex(),'unwind_flags':ud[0]>>3})
 out['native_bodies'].append({'token':m['token'],'start':m['start'],'end':m['end'],'bytes':len(data),'sha256':hashlib.sha256(data).hexdigest(),'recorded_sha256':m['native_sha256'],'parts':parts,'instructions':[{'address':hex(i.address),'bytes':i.bytes.hex(),'mnemonic':i.mnemonic,'operands':i.op_str}for i in md.disasm(data,int(m['start'],16))]})
# Independently derive ordinary concrete call pointers from original image strings/tables.
for image in ['UnityEngine.CoreModule.dll','Assembly-CSharp.dll']:
 modules=[]
 for match in re.finditer(re.escape(image.encode()+b'\0'),raw):
  va=pe.get_rva_from_offset(match.start())+base
  for ref in re.finditer(re.escape(struct.pack('<Q',va)),raw):
   a=ref.start();cnt=q(raw,a+8);ptr=q(raw,a+16)
   if 100<cnt<100000 and base<ptr<base+pe.OPTIONAL_HEADER.SizeOfImage:modules.append((pe.get_rva_from_offset(a)+base,cnt,ptr))
 if len(modules)!=1:raise ValueError((image,modules))
 module,cnt,ptr=modules[0]
 for m in out['methods']:
  if not any(x['image']==image for x in m['native_map']):continue
  rid=int(m['token'],16)&0xffffff
  out['independent_code_module_mapping'].append({'image':image,'module':hex(module),'pointer_table':hex(ptr),'token':m['token'],'owner':m['owner'],'name':m['name'],'va':hex(nq(ptr+8*(rid-1)))})
print(json.dumps(out,indent=2,ensure_ascii=False))
