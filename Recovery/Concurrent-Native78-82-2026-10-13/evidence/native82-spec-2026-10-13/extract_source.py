"""Static PE, IL2CPP metadata, and resolver CIL source extraction only."""
from pathlib import Path
import sys, struct, json, re, hashlib, importlib.util
sys.dont_write_bytecode = True
ROOT=Path(__file__).resolve().parents[2]
OUT=Path(__file__).resolve().parent
sys.path.insert(0,str(ROOT/'audit/stage9-native4/python-deps'))
import capstone, dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
from dncil.clr.token import Token
sp=importlib.util.spec_from_file_location('probe82',ROOT/'audit/stage9-native3/repository/Tools/Stage9NativeProbe/pc_method_probe.py')
probe=importlib.util.module_from_spec(sp);sys.modules[sp.name]=probe;sp.loader.exec_module(probe)
raw=(ROOT/'audit/stage9-native3/inputs/GameAssembly.dll').read_bytes()
b=(ROOT/'audit/stage9-native3/inputs/global-metadata.dat').read_bytes()
sha=lambda x:hashlib.sha256(x).hexdigest()
assert sha(raw)=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
assert sha(b)=='ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9'
pe=probe.PE(raw);base=pe.image_base
def nat(v,n):
 o=pe.va_to_offset(v);return raw[o:o+n]
def nq(v):return struct.unpack('<Q',nat(v,8))[0]
def u(v,o=0):return struct.unpack_from('<I',v,o)[0]
def offset_va(o):
 for sec in pe.sections:
  if sec.raw_offset<=o<sec.raw_offset+sec.raw_size:return base+sec.va+o-sec.raw_offset
 return None
def st(i):
 o=u(b,24)+i;return b[o:b.index(0,o)].decode('utf-8')
types=json.loads((ROOT/'audit/stage9-native3/evidence/native-types.json').read_text())
fields=json.loads((ROOT/'audit/stage9-native3/evidence/native-fields.json').read_text())
reg=0x1818C6D00
result={'scope':'Static source inspection only. No candidate generation, tests, builds, or CIL/native execution.', 'inputs':{'GameAssembly_sha256':sha(raw),'metadata_sha256':sha(b)},'registration':[{'offset':i,'value':hex(nq(reg+i))} for i in range(0,128,8)]}
def image_of(ti):
 io,sz=u(b,168),u(b,172)
 for k in range(sz//40):
  p=io+40*k;start,count=u(b,p+8),u(b,p+12)
  if start<=ti<start+count:return st(u(b,p))
def td(i):
 p=u(b,160)+i*88;t=types[i]
 return dict(t,image=image_of(i),byvalTypeIndex=u(b,p+8),parentTypeIndex=u(b,p+16),flags=hex(u(b,p+28)),bitfield=hex(u(b,p+80)))
def typ(i):
 p=nq(0x1818EDEB0+8*i);v=nat(p,16);d=struct.unpack_from('<Q',v)[0];bits=u(v,8);kind=(bits>>16)&255
 t={'index':i,'pointer':hex(p),'bytes':v.hex(),'data':d,'kind':hex(kind),'byref':bool(bits&(1<<30))}
 if kind in (0x11,0x12):t['typedef']=td(d)
 return t
def meth(i):
 p=u(b,48)+i*36
 name,owner,ret,rpt,ps,gc,token,flags,iflags,slot,count=struct.unpack_from('<IIIIiiIHHHH',b,p)
 return {'index':i,'record_offset':hex(p),'owner':types[owner]['namespace']+'.'+types[owner]['name'],'image':image_of(owner),'name':st(name),'token':hex(token),'return':typ(ret),'parameters':[{'name':st(u(b,u(b,88)+12*(ps+j))),'type':typ(u(b,u(b,88)+12*(ps+j)+8))} for j in range(count)],'genericContainerIndex':gc,'flags':hex(flags),'implflags':hex(iflags),'slot':slot}
def inst(i):
 if i<0:return None
 p=nq(nq(reg+24)+i*8);n=u(nat(p,16));argv=nq(p+8)
 return {'index':i,'pointer':hex(p),'argc':n,'argv':hex(argv),'args':[typ_index_ptr(nq(argv+8*j)) for j in range(n)]}
type_ptr_lookup={nq(0x1818EDEB0+8*i):i for i in range(nq(reg+48))}
def typ_index_ptr(p):
 return typ(type_ptr_lookup[p]) if p in type_ptr_lookup else {'unmapped_type_ptr':hex(p),'bytes':nat(p,16).hex()}
def spec(i):
 p=nq(reg+72)+i*12;m,c,g=struct.unpack('<iii',nat(p,12))
 return {'index':i,'record_pointer':hex(p),'bytes':nat(p,12).hex(),'methodDefinition':meth(m),'classInst':inst(c),'methodInst':inst(g)}
def cell(va):
 v=nq(va);kind=(v>>29)&7;i=(v&0x1fffffff)>>1
 r={'va':hex(va),'bytes':nat(va,8).hex(),'value':hex(v),'kind':kind,'index':i}
 if kind in (1,2):r['type']=typ(i)
 if kind==6:r['methodSpec']=spec(i)
 if kind==3:r['methodDefinition']=meth(i)
 if kind==5:
  le,off=struct.unpack_from('<II',b,u(b,8)+8*i);literal=b[u(b,16)+off:u(b,16)+off+le]
  r['literal']={'length':le,'bytes':literal.hex(),'text':literal.decode('utf-8')}
 return r
cells=[0x181BA5818,0x181BA5338,0x181BB0500,0x181BA3130,0x181BBFCF0,0x181BA40D8,0x181BA1CB8,0x181BB74C8,0x181BBC038,0x181BCE6F0]
result['cells']=[cell(v) for v in cells]
result['fields']=[dict(f,registered_type=typ(f['type_index'])) for f in fields if f['type'] in ['TMP_TextEventHandler','SwfManager']]
source=json.loads((ROOT/'audit/native30-continuation-2026-10-02/native55-native.json').read_text())
result['native55_sha256']=sha((ROOT/'audit/native30-continuation-2026-10-02/native55-native.json').read_bytes())
result['targets']=[]
targets=set()
for row in source['methods']:
 if row['token'] not in ['0x0600087D','0x06000734']:continue
 lo,hi=int(row['start'],16),int(row['end'],16);rb=nat(lo,hi-lo)
 assert nq(0x181B82D60+8*((int(row['token'],16)&0xffffff)-1))==lo
 assert sha(rb)==row['native_sha256']
 result['targets'].append({'token':row['token'],'method':row['method'],'start':row['start'],'end':row['end'],'byte_count':len(rb),'sha256':sha(rb),'pdata_parts':row['pdata_parts'],'metadata':next(meth(types[ti]['methodStart']+j) for ti in [5316,5379] for j in range(types[ti]['methods']) if meth(types[ti]['methodStart']+j)['token']==hex(int(row['token'],16))), 'existing_locals_reference_only':row['managed']['locals']})
 (OUT/(row['token']+'-native55.asm')).write_text(row['disassembly'],encoding='utf-8')
 for match in re.finditer(r'call\s+0x([0-9a-f]+)',row['disassembly']):targets.add(int(match.group(1),16))
result['module_matches']=[]
for im in range(u(b,172)//40):
 p=u(b,168)+40*im;name=st(u(b,p));start,count=u(b,p+8),u(b,p+12)
 if name not in ['mscorlib.dll','UnityEngine.CoreModule.dll','UnityEngine.UIModule.dll','Assembly-CSharp.dll']:continue
 mods=[]
 for mt in re.finditer(re.escape(name.encode()+b'\0'),raw):
  va=offset_va(mt.start())
  if va is None:continue
  for ref in re.finditer(re.escape(struct.pack('<Q',va)),raw):
   n,ptr=struct.unpack_from('<QQ',raw,ref.start()+8)
   if 10<n<100000 and base<ptr<base+0x3000000:mods.append((ref.start(),n,ptr))
 assert len(mods)==1,(name,mods)
 off,n,ptr=mods[0]
 for ti in range(start,start+count):
  for j in range(types[ti]['methods']):
   m=meth(types[ti]['methodStart']+j);rid=int(m['token'],16)&0xffffff
   va=nq(ptr+8*(rid-1))
   if va in targets or (name=='mscorlib.dll' and m['name']=='IsSubclassOf'):
    result['module_matches'].append({'module_name':name,'module_va':hex(offset_va(off)),'module_file_offset':off,'pointer_count':n,'pointer_table':hex(ptr),'va':hex(va),'method':m})
result['generic_method_rows']=[]
result['generic_table_prefix']=nat(nq(reg+40),128).hex()
for c in result['cells']:
 if c['kind']!=6:continue
 si=c['index'];matches=[]
 for j in range(nq(reg+32)):
  p=nq(reg+40)+16*j;sm,mi,inv,adj=struct.unpack('<iiii',nat(p,16))
  if sm==si:matches.append({'row':j,'pointer':hex(p),'methodSpecIndex':sm,'methodPointerIndex':mi,'invokerIndex':inv,'adjustorIndex':adj})
 result['generic_method_rows'].append({'cell':c['va'],'methodSpecIndex':si,'matches':matches})
definitions={36978:0x18046D9C0,36969:0x18046DC80,37000:0x18046D280,37122:0x1804A0420}
result['canonical_generic_method_rows']=[]
pointer_bases={}
for j in range(nq(reg+32)):
 p=nq(reg+40)+16*j;sm,mi,inv,adj=struct.unpack('<iiii',nat(p,16))
 if sm<0 or sm>=nq(reg+64):continue
 md,ci,gi=struct.unpack('<iii',nat(nq(reg+72)+12*sm,12))
 if md not in definitions:continue
 row={'row':j,'record_pointer':hex(p),'methodSpec':spec(sm),'methodPointerIndex':mi,'invokerIndex':inv,'adjustorIndex':adj,'expected_native':hex(definitions[md])}
 ptrrefs=[]
 for hit in re.finditer(re.escape(struct.pack('<Q',definitions[md])),raw):
  for sec in pe.sections:
   if sec.raw_offset<=hit.start()<sec.raw_offset+sec.raw_size:
    va=base+sec.va+hit.start()-sec.raw_offset;table=va-8*mi
    ptrrefs.append({'pointer_cell':hex(va),'candidate_table_base':hex(table)})
    pointer_bases.setdefault(table,set()).add(md)
    break
 row['pointer_occurrences']=ptrrefs;result['canonical_generic_method_rows'].append(row)
result['generic_pointer_table_candidates']=[{'base':hex(k),'matched_method_definitions':sorted(v)} for k,v in pointer_bases.items() if len(v)>=3]
assert len(result['generic_pointer_table_candidates'])==1
gptr=int(result['generic_pointer_table_candidates'][0]['base'],16)
for r in result['canonical_generic_method_rows']:
 r['original_pointer_cell']=hex(gptr+8*r['methodPointerIndex'])
 r['original_native_pointer']=hex(nq(gptr+8*r['methodPointerIndex']))
result['generic_pointer_table_references']=[]
for hit in re.finditer(re.escape(struct.pack('<Q',gptr)),raw):
 for sec in pe.sections:
  if sec.raw_offset<=hit.start()<sec.raw_offset+sec.raw_size:
   va=base+sec.va+hit.start()-sec.raw_offset
   result['generic_pointer_table_references'].append({'va':hex(va),'surrounding_qwords':[{'offset':i,'value':hex(nq(va+i))} for i in range(-24,80,8)]});break
ds=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64)
result['original_null_exception_fragments']=[]
for va,size in [(0x180250150,10),(0x180209500,23),(0x1802094E0,20),(0x1802078E0,100)]:
 rb=nat(va,size)
 result['original_null_exception_fragments'].append({'va':hex(va),'size':size,'scope':'bounded source fragment only','sha256':sha(rb),'disassembly':[f'{x.address:016x}: {x.bytes.hex():<32} {x.mnemonic} {x.op_str}' for x in ds.disasm(rb,va)]})
result['exception_factory_strings']=[{'va':hex(v),'text':nat(v,100).split(b'\0',1)[0].decode('ascii')} for v in [0x1819A7AE8,0x1819A7998]]
result['unmapped_native_helpers']=[]
pdata=pe.section('.pdata')
for va in sorted((targets-{int(m['va'],16) for m in result['module_matches']})|{0x1804C3750}):
 boundary=None
 for p in range(pdata.raw_offset,pdata.raw_offset+pdata.raw_size-11,12):
  lo,hi,unwind=struct.unpack_from('<III',raw,p)
  if base+lo==va:boundary=(base+lo,base+hi);break
 if boundary is None:boundary=(va,va+7) if va==0x1804C3750 else (va,va+256)
 rb=nat(boundary[0],boundary[1]-boundary[0])
 result['unmapped_native_helpers'].append({'start':hex(va),'end':hex(boundary[1]),'exact_pdata':va!=0x1804C3750 and boundary[1]!=va+256,'boundary_scope':'leaf body through first ret' if va==0x1804C3750 else 'exact pdata' if boundary[1]!=va+256 else 'context bytes only','bytes':rb.hex(),'sha256':sha(rb),'disassembly':[f'{x.address:016x}: {x.bytes.hex():<32} {x.mnemonic} {x.op_str}' for x in ds.disasm(rb,va)]})
resolver=ROOT/'audit/stage9-native3/resolver/UnityEngine.CoreModule.dll'
clr=dnfile.dnPE(str(resolver));tables=clr.net.mdtables
owners={i.row_index:str(t.TypeNamespace)+'.'+str(t.TypeName) for t in tables.TypeDef.rows for i in t.MethodList}
def identity(token):
 tb,rid=token>>24,token&0xffffff
 if tb==6:
  r=tables.MethodDef.rows[rid-1];return {'token':hex(token),'owner':owners[rid],'name':str(r.Name),'signature':r.Signature.value.hex()}
 if tb==10:
  r=tables.MemberRef.rows[rid-1];o=r.Class.row;return {'token':hex(token),'owner':str(getattr(o,'TypeNamespace',''))+'.'+str(getattr(o,'TypeName','')),'name':str(r.Name),'signature':r.Signature.value.hex()}
 if tb==4:
  r=tables.Field.rows[rid-1];return {'token':hex(token),'name':str(r.Name),'signature':r.Signature.value.hex()}
 return {'token':hex(token)}
result['resolver']={'path':str(resolver),'sha256':sha(resolver.read_bytes()),'methods':[]}
for rid,r in enumerate(tables.MethodDef.rows,1):
 if (owners[rid],str(r.Name)) not in [('UnityEngine.Object','FindObjectOfType'),('UnityEngine.GameObject','AddComponent'),('UnityEngine.GameObject','GetComponent'),('UnityEngine.GameObject','GetComponentInParent'),('UnityEngine.Object','op_Implicit'),('UnityEngine.Object','op_Inequality'),('UnityEngine.Object','CompareBaseObjects')]:continue
 item=dict(identity(0x06000000+rid),flags=str(r.Flags),rva=r.Rva)
 if r.Rva:
  body=read_method_body_from_bytes(clr.get_data(r.Rva));item.update(code_size=body.code_size,max_stack=body.max_stack,body_sha256=sha(body.get_bytes()),body_bytes=body.get_bytes().hex(),instructions=[])
  for ins in body.instructions:
   op=ins.operand
   if isinstance(op,Token):op=identity(op.value)
   elif op is not None:op=str(op)
   item['instructions'].append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode.name),'operand':op})
 result['resolver']['methods'].append(item)
msc=ROOT/'audit/stage9-native3/resolver/mscorlib.dll'
clr=dnfile.dnPE(str(msc));tables=clr.net.mdtables
owners={i.row_index:str(t.TypeNamespace)+'.'+str(t.TypeName) for t in tables.TypeDef.rows for i in t.MethodList}
result['mscorlib_resolver']={'path':str(msc),'sha256':sha(msc.read_bytes()),'methods':[]}
for rid,r in enumerate(tables.MethodDef.rows,1):
 if (owners[rid],str(r.Name)) not in [('System.Type','op_Equality'),('System.Type','IsSubclassOf'),('System.Object','GetType')]:continue
 item=dict(identity(0x06000000+rid),flags=str(r.Flags),rva=r.Rva)
 if r.Rva:
  body=read_method_body_from_bytes(clr.get_data(r.Rva));item.update(body_sha256=sha(body.get_bytes()),body_bytes=body.get_bytes().hex(),instructions=[])
  for ins in body.instructions:
   op=ins.operand
   if isinstance(op,Token):op=identity(op.value)
   elif op is not None:op=str(op)
   item['instructions'].append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode.name),'operand':op})
 result['mscorlib_resolver']['methods'].append(item)
inventory_path=ROOT/'audit/native80-codex-2026-10-13/metadata-current.json'
inventory=json.loads(inventory_path.read_text(encoding='utf-8'))
result['parent79_inventory']={'path':str(inventory_path),'sha256':sha(inventory_path.read_bytes()),'described_dll_sha256':inventory['input_sha256'],'targets':[t for t in inventory['targets'] if t['token'] in ['0x06000734','0x0600087D']],'internals':[t for t in inventory['internals'] if t['name'] in ['TMPro.TMP_TextEventHandler','FTRuntime.SwfManager']]}
(OUT/'PRIMARY-EVIDENCE.json').write_text(json.dumps(result,indent=2,ensure_ascii=False),encoding='utf-8')
print(json.dumps({k:v for k,v in result.items() if k not in ['unmapped_native_helpers','resolver','fields']},indent=2,ensure_ascii=True))
print('HELPERS '+json.dumps(result['unmapped_native_helpers'],indent=2))
