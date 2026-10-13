"""Original PE/metadata/CLR reading only; no target runtime or tests."""
from pathlib import Path
import sys,struct,json,re,hashlib,importlib.util
sys.dont_write_bytecode=True
R=Path(__file__).resolve().parents[2];O=Path(__file__).resolve().parent
sys.path.insert(0,str(R/'audit/stage9-native4/python-deps'))
import capstone,dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
from dncil.clr.token import Token
s=importlib.util.spec_from_file_location('probe85',R/'audit/stage9-native3/repository/Tools/Stage9NativeProbe/pc_method_probe.py');probe=importlib.util.module_from_spec(s);sys.modules[s.name]=probe;s.loader.exec_module(probe)
raw=(R/'audit/stage9-native3/inputs/GameAssembly.dll').read_bytes();b=(R/'audit/stage9-native3/inputs/global-metadata.dat').read_bytes();pe=probe.PE(raw);base=pe.image_base
sha=lambda x:hashlib.sha256(x).hexdigest()
assert sha(raw)=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
assert sha(b)=='ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9'
def nat(v,n):
 p=pe.va_to_offset(v);return raw[p:p+n]
def nq(v):return struct.unpack('<Q',nat(v,8))[0]
def u(x,p=0):return struct.unpack_from('<I',x,p)[0]
def st(i):
 p=u(b,24)+i;return b[p:b.index(0,p)].decode('utf-8')
types=json.loads((R/'audit/stage9-native3/evidence/native-types.json').read_text());fields=json.loads((R/'audit/stage9-native3/evidence/native-fields.json').read_text());maps=json.loads((R/'audit/stage9-native3/evidence/native-method-map.json').read_text())
reg=0x1818C6D00;typetable=0x1818EDEB0
ptrindex={nq(typetable+i*8):i for i in range(nq(reg+48))}
def image(ti):
 for k in range(u(b,172)//40):
  p=u(b,168)+40*k;a,z=u(b,p+8),u(b,p+12)
  if a<=ti<a+z:return st(u(b,p))
def td(i):
 p=u(b,160)+88*i
 return dict(types[i],image=image(i),byvalTypeIndex=u(b,p+8),parentTypeIndex=u(b,p+16),flags=hex(u(b,p+28)),bitfield=hex(u(b,p+80)))
def typ(i):
 p=nq(typetable+8*i);v=nat(p,16);d=struct.unpack_from('<Q',v)[0];bits=u(v,8);kind=(bits>>16)&255
 t={'index':i,'pointer':hex(p),'bytes':v.hex(),'data':d,'kind':hex(kind),'byref':bool(bits&(1<<29)),'attrs':hex(bits&65535),'bits':hex(bits)}
 if kind in (0x11,0x12,0x1c):t['typedef']=td(d)
 if kind==0x15:
  definition=nq(d);ci,mi=struct.unpack('<QQ',nat(d+8,16));t['genericClass']={'pointer':hex(d),'definition_type':typ(ptrindex[definition]) if definition in ptrindex else {'pointer':hex(definition)},'classInst':inst_ptr(ci) if ci else None,'methodInst':inst_ptr(mi) if mi else None}
 return t
def inst_ptr(p):
 n=u(nat(p,16));argv=nq(p+8)
 return {'pointer':hex(p),'argc':n,'args':[typ(ptrindex[nq(argv+8*j)]) if nq(argv+8*j) in ptrindex else {'pointer':hex(nq(argv+8*j))} for j in range(n)]}
def inst(i):return None if i<0 else dict(inst_ptr(nq(nq(reg+24)+8*i)),index=i)
def method(i):
 p=u(b,48)+i*36;name,owner,ret,rpt,ps,gc,token,flags,iflags,slot,n=struct.unpack_from('<IIIIiiIHHHH',b,p)
 return {'index':i,'owner':types[owner]['namespace']+'.'+types[owner]['name'],'image':image(owner),'name':st(name),'token':hex(token),'return':typ(ret),'parameters':[{'name':st(u(b,u(b,88)+12*(ps+j))),'type':typ(u(b,u(b,88)+12*(ps+j)+8))} for j in range(n)],'genericContainer':gc,'flags':hex(flags),'implflags':hex(iflags),'slot':slot}
def spec(i):
 p=nq(reg+72)+12*i;m,c,g=struct.unpack('<iii',nat(p,12));return {'index':i,'pointer':hex(p),'bytes':nat(p,12).hex(),'definition':method(m),'classInst':inst(c),'methodInst':inst(g)}
def cell(v):
 q=nq(v);k=(q>>29)&7;i=(q&0x1fffffff)>>1;r={'va':hex(v),'bytes':nat(v,8).hex(),'value':hex(q),'kind':k,'index':i}
 if k in (1,2):r['type']=typ(i)
 if k==6:r['spec']=spec(i)
 if k==3:r['method']=method(i)
 if k==5:
  n,z=struct.unpack_from('<II',b,u(b,8)+8*i);s=b[u(b,16)+z:u(b,16)+z+n];r['literal']={'length':n,'bytes':s.hex(),'text':s.decode('utf-8')}
 return r
result={'scope':'Static original input/metadata/full resolver source only; no tests, native/CIL execution, builds, candidates or existing file edits.','inputs':{'GameAssembly_sha256':sha(raw),'metadata_sha256':sha(b)},'cells':[cell(v) for v in [0x181B9A928,0x181B9E168,0x181BA2018,0x181BBB820,0x181BCF088,0x181BA9DE0,0x181BC27E8]]}
src=R/'audit/native30-continuation-2026-10-02/native55-native.json';rows=json.loads(src.read_text())['methods'];result['native55_sha256']=sha(src.read_bytes());result['targets']=[];calls=set()
for row in rows:
 if row['token'] not in ['0x06000443','0x0600047D']:continue
 a,z=int(row['start'],16),int(row['end'],16);v=nat(a,z-a)
 assert nq(0x181B82D60+8*((int(row['token'],16)&0xffffff)-1))==a
 assert sha(v)==row['native_sha256']
 result['targets'].append({'token':row['token'],'name':row['method'],'start':row['start'],'end':row['end'],'bytes':len(v),'sha256':sha(v),'pdata_parts':row['pdata_parts'],'reference_locals_only':row['managed']['locals']})
 (O/(row['token']+'-native55.asm')).write_text(row['disassembly'],encoding='utf-8')
 calls.update(int(m.group(1),16) for m in re.finditer(r'call\s+0x([0-9a-f]+)',row['disassembly']))
result['method_maps']=[]
for rec in maps:
 if int(rec['va'],16) not in calls:continue
 ms=[method(types[ti]['methodStart']+j) for ti,t in enumerate(types) if t['name']==rec['type'] and image(ti)==rec['image'] for j in range(t['methods']) if method(types[ti]['methodStart']+j)['token']==rec['token']]
 for m in ms:
  if rec['image']=='Assembly-CSharp.dll':assert nq(0x181B82D60+8*((int(rec['token'],16)&0xffffff)-1))==int(rec['va'],16)
  result['method_maps'].append(dict(rec,metadata=m))
result['zombie_damage_metadata']=[method(types[5219]['methodStart']+j) for j in range(types[5219]['methods']) if method(types[5219]['methodStart']+j)['token'] in ['0x6000443','0x600047d']]
result['field_maps']=[dict(f,registered_type=typ(f['type_index'])) for f in fields if (f['type']=='Zombie' and f['name'] in ['board','fZ','ID','camp','animationSprites']) or (f['type']=='Damage' and f['offset'] in [16,20,88,104]) or (f['type']=='Board' and f['offset']==248) or (f['type']=='Vector3' and f['name'] in ['x','y','z','zeroVector'])]
result['switches']=[]
for va,start,count in [(0x180360F04,3,20),(0x18036918C,6,17)]:
 entries=list(struct.unpack('<'+'I'*count,nat(va,count*4)))
 result['switches'].append({'table_va':hex(va),'bytes':nat(va,count*4).hex(),'base_id':start,'entries':[{'id':start+j,'target':hex(base+p)} for j,p in enumerate(entries)]})
result['float32_constants']=[{'va':hex(v),'bytes':nat(v,4).hex(),'bits':hex(u(nat(v,4))),'float32':struct.unpack('<f',nat(v,4))[0]} for v in [0x1815A7BE4,0x1815A7BE8,0x1815A7D10,0x1815A7C44,0x1815A7BE0,0x1815A7A08,0x1815A7B04,0x1815A7B40,0x1815A7D68,0x1815A7A40]]
result['generic_canonical_rows']=[]
generic_definitions={c['spec']['definition']['index'] for c in result['cells'] if c['kind']==6}
for j in range(nq(reg+32)):
 p=nq(reg+40)+16*j;si,mi,iv,ad=struct.unpack('<iiii',nat(p,16));md,ci,gi=struct.unpack('<iii',nat(nq(reg+72)+12*si,12))
 if md not in generic_definitions:continue
 result['generic_canonical_rows'].append({'row':j,'record':hex(p),'spec':spec(si),'methodPointerIndex':mi,'pointer_cell':hex(0x181760110+8*mi),'native_pointer':hex(nq(0x181760110+8*mi)),'invokerIndex':iv,'adjustorIndex':ad})
ds=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64);pdata=pe.section('.pdata');result['generic_native_helpers']=[]
for va in sorted(calls-{int(m['va'],16) for m in result['method_maps']}):
 if va in [0x18024FEF0,0x180250100,0x180250150,0x1802501E0,0x18024F360]:continue
 end=None
 for p in range(pdata.raw_offset,pdata.raw_offset+pdata.raw_size-11,12):
  a,z,uw=struct.unpack_from('<III',raw,p)
  if base+a==va:end=base+z;break
 if end is None:end=va+512
 v=nat(va,end-va);result['generic_native_helpers'].append({'start':hex(va),'end':hex(end),'exact_pdata':end!=va+512,'sha256':sha(v),'disassembly':[f'{x.address:016x}: {x.bytes.hex():<32} {x.mnemonic} {x.op_str}' for x in ds.disasm(v,va)]})
resolver=R/'audit/stage9-native3/resolver/UnityEngine.CoreModule.dll';clr=dnfile.dnPE(str(resolver));tables=clr.net.mdtables
owners={i.row_index:str(t.TypeNamespace)+'.'+str(t.TypeName) for t in tables.TypeDef.rows for i in t.MethodList}
def identity(t):
 tb,ri=t>>24,t&0xffffff
 if tb==6:
  x=tables.MethodDef.rows[ri-1];return {'token':hex(t),'owner':owners[ri],'name':str(x.Name),'signature':x.Signature.value.hex()}
 if tb==4:
  x=tables.Field.rows[ri-1];return {'token':hex(t),'name':str(x.Name),'signature':x.Signature.value.hex()}
 return {'token':hex(t)}
result['resolver_vector3_zero']={'path':str(resolver),'sha256':sha(resolver.read_bytes()),'methods':[]}
for ri,x in enumerate(tables.MethodDef.rows,1):
 if owners[ri]!='UnityEngine.Vector3' or (str(x.Name)!='get_zero' and not (str(x.Name)=='.ctor' and x.Signature.value.hex()=='2003010c0c0c')):continue
 body=read_method_body_from_bytes(clr.get_data(x.Rva));m=dict(identity(0x6000000+ri),public=x.Flags.mdPublic,body_bytes=body.get_bytes().hex(),body_sha256=sha(body.get_bytes()),instructions=[])
 for ins in body.instructions:
  op=ins.operand
  if isinstance(op,Token):op=identity(op.value)
  elif op is not None:op=str(op)
  m['instructions'].append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode.name),'operand':op})
 result['resolver_vector3_zero']['methods'].append(m)
inventory_path=R/'audit/native83-codex-2026-10-13/metadata-current.json'
inventory=json.loads(inventory_path.read_text(encoding='utf-8'))
result['current_inventory']={'path':str(inventory_path),'sha256':sha(inventory_path.read_bytes()),'described_dll_sha256':inventory['input_sha256'],'targets':[x for x in inventory['targets'] if x['token'] in ['0x06000443','0x0600047D']],'selected_refs':[x for x in inventory['refs'] if x['name'] in ['System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)','UnityEngine.Vector3 UnityEngine.Vector3::get_zero()']],'internals':[x for x in inventory['internals'] if x['name'] in ['Zombie','Damage','Element','Projectile','Board','GlobalStaticVars']]}
(O/'PRIMARY-EVIDENCE.json').write_text(json.dumps(result,indent=2,ensure_ascii=False),encoding='utf-8')
print(json.dumps(result,indent=2,ensure_ascii=True))
