"""Read-only PE/metadata source inspection; no candidate generation or tests."""
from pathlib import Path
import struct, json, re, hashlib, sys
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import pefile
pe=pefile.PE(str(R/'audit/stage9-native3/inputs/GameAssembly.dll'))
base=pe.OPTIONAL_HEADER.ImageBase
b=(R/'audit/stage9-native3/inputs/global-metadata.dat').read_bytes()
raw=pe.__data__
def u(data,o=0):return struct.unpack_from('<I',data,o)[0]
def q(data,o=0):return struct.unpack_from('<Q',data,o)[0]
def native(va,n):return pe.get_data(va-base,n)
def nq(va):return q(native(va,8))
so,to,ts,fo,mo,po=u(b,24),u(b,160),u(b,164),u(b,96),u(b,48),u(b,88)
def st(i):return b[so+i:b.index(0,so+i)].decode('utf-8')
types=json.loads((R/'audit/stage9-native3/evidence/native-types.json').read_text())
pat=struct.pack('<Q',len(types)); candidates=[]
for match in re.finditer(re.escape(pat),raw):
 p=match.start()
 if raw[p+16:p+24]!=pat:continue
 ptr=q(raw,p+8)
 try:
  t=next(t for t in types if t['name']=='BoardConfig')
  fp=nq(ptr+8*t['index'])
  v=list(struct.unpack('<'+'i'*t['fields'],native(fp,t['fields']*4)))
  if v[:2]==[16,24]:candidates.append((pe.get_rva_from_offset(p)+base,ptr))
 except Exception:pass
if len(candidates)!=1:raise ValueError(candidates)
fieldpair,offsets=candidates[0]
types_count=nq(fieldpair-32); types_ptr=nq(fieldpair-24)
print('SOURCES',json.dumps({p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in [R/'audit/stage9-native3/inputs/GameAssembly.dll',R/'audit/stage9-native3/inputs/global-metadata.dat']}))
print('REGISTRATION',json.dumps({'fieldoffset_pair':hex(fieldpair),'fieldoffsets':hex(offsets),'types_count_pair':hex(fieldpair-32),'types_count':types_count,'types_table':hex(types_ptr),'registration_start':hex(fieldpair-80)}))
def td(i):
 t=types[i];p=to+i*88
 return dict(t,byvalTypeIndex=u(b,p+8),parentIndex=u(b,p+16),elementTypeIndex=u(b,p+20),flags=hex(u(b,p+28)),bitfield=hex(u(b,p+80)))
def typ(i):
 ptr=nq(types_ptr+i*8); v=native(ptr,16); data=q(v);bits=u(v,8);kind=(bits>>16)&255
 result={'index':i,'table_slot':hex(types_ptr+i*8),'type_ptr':hex(ptr),'bytes':v.hex(),'data':data,'bits':hex(bits),'kind':hex(kind),'byref':bool(bits&(1<<30))}
 if kind in (0x11,0x12):result['typedef']=td(data)
 return result
for va in [0x181B9E7A8,0x181BA95A8,0x181BA4EC8,0x181BBAB00,0x181BC27E8,0x181BB0500,0x181BBFCF0,0x181B9EC48]:
 val=nq(va); usage=(val>>29)&7;index=(val&0x1fffffff)>>1
 result={'cell':hex(va),'raw_bytes':native(va,8).hex(),'raw_value':hex(val),'usage_kind':usage,'source_index':index}
 if usage in (1,2):result['type']=typ(index)
 if usage==5:
  sl,sd=u(b,8),u(b,16);length,dataIndex=struct.unpack_from('<II',b,sl+8*index)
  literal=b[sd+dataIndex:sd+dataIndex+length]
  result['literal']={'record_offset':hex(sl+8*index),'data_offset':hex(sd+dataIndex),'length':length,'utf8_bytes':literal.hex(),'text':literal.decode('utf-8')}
 print('CELL',json.dumps(result,ensure_ascii=True))
for i in [15454,15470,14734,14735,19915,19919,16879,18640,11936,12427]:print('FIELD_TYPE',json.dumps(typ(i),ensure_ascii=True))
def method_at(t,j):
 p=mo+36*(t['methodStart']+j)
 name,owner,ret,rpt,param,gc,token,flags,iflags,slot,count=struct.unpack_from('<IIIIiiIHHHH',b,p)
 return {'index':t['methodStart']+j,'definition_offset':hex(p),'name':st(name),'owner':types[owner]['name'],'return_type':typ(ret),'parameter_start':param,'token':hex(token),'flags':hex(flags),'impl_flags':hex(iflags),'slot':slot,'parameter_count':count,'parameters':[{'name':st(u(b,po+12*(param+k))),'token':hex(u(b,po+12*(param+k)+4)),'type':typ(u(b,po+12*(param+k)+8))} for k in range(count)]}
for name,names in [('Type',['GetTypeFromHandle']),('Enum',['IsDefined']),('Plant',['BlockZombie']),('Zombie',['ResetAttackSpeed','TranToAttack']),('System2',['SetGridState']),('GridConfig_A',['SetGridState']),('Animator',['SetBool']),('Object',['op_Inequality']),('Debug',['Log']),('Vector3',['get_zero'])]:
 for t in types:
  if t['name']==name:
   for j in range(t['methods']):
    m=method_at(t,j)
    if m['name'] in names:print('METHOD',json.dumps(m,ensure_ascii=True))
print('INT32_TYPEDEF',json.dumps(td(198)))
# Independently map needed mscorlib and animation calls from original module tables.
io,isz=u(b,168),u(b,172)
for im in range(isz//40):
 p=io+40*im;name=st(u(b,p));start=u(b,p+8);count=u(b,p+12)
 if name not in ['mscorlib.dll','UnityEngine.AnimationModule.dll']:continue
 modules=[]
 for pos in (m.start() for m in re.finditer(re.escape(name.encode()+b'\0'),raw)):
  va=pe.get_rva_from_offset(pos)+base
  for ref in re.finditer(re.escape(struct.pack('<Q',va)),raw):
   a=ref.start();cnt=q(raw,a+8);ptr=q(raw,a+16)
   if 10<cnt<100000 and base<ptr<base+pe.OPTIONAL_HEADER.SizeOfImage:modules.append((pe.get_rva_from_offset(a)+base,cnt,ptr))
 if len(modules)!=1:raise ValueError((name,modules))
 module,cnt,ptr=modules[0]
 for t in types[start:start+count]:
  if t['name'] not in ['Type','Enum','Animator']:continue
  for j in range(t['methods']):
   m=method_at(t,j)
   if m['name'] not in ['GetTypeFromHandle','IsDefined','SetBool']:continue
   rid=int(m['token'],16)&0xffffff
   print('CALL_MAP',json.dumps({'image':name,'module':hex(module),'pointer_count':cnt,'pointer_table':hex(ptr),'token':m['token'],'owner':m['owner'],'name':m['name'],'va':hex(nq(ptr+8*(rid-1))),'signature':m}))
# Hash the bounded pdata bytes again from original PE; this is evidence inspection.
audit=json.loads((R/'audit/native30-continuation-2026-10-02/native55-native.json').read_text())
for m in audit['methods']:
 if m['token'] not in ['0x0600061D','0x06000483']:continue
 data=b''.join(native(a,z-a) for a,z in m['pdata_parts'])
 print('NATIVE_BODY',json.dumps({'token':m['token'],'start':m['start'],'end':m['end'],'byte_count':len(data),'sha256':hashlib.sha256(data).hexdigest(),'recorded_sha256':m['native_sha256']}))
