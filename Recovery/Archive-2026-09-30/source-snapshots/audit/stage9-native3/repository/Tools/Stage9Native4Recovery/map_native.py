from pathlib import Path
import sys,struct,json,re
R=Path(__file__).resolve().parent;sys.path.insert(0,str(R/'python-deps'))
import pefile
pe=pefile.PE(str(R/'inputs/GameAssembly.dll')); base=pe.OPTIONAL_HEADER.ImageBase
b=(R/'inputs/global-metadata.dat').read_bytes()
def u(data,o=0):return struct.unpack_from('<I',data,o)[0]
def q(data,o=0):return struct.unpack_from('<Q',data,o)[0]
def native(va,n):return pe.get_data(va-base,n)
def nq(va):return q(native(va,8))
so=u(b,24);to=u(b,160);ts=u(b,164);fo=u(b,96);mo=u(b,48);io=u(b,168);isz=u(b,172)
def st(i):return b[so+i:b.index(0,so+i)].decode('utf-8')
assert ts%88==0
types=[]
for i in range(ts//88):
 p=to+i*88
 types.append(dict(index=i,name=st(u(b,p)),namespace=st(u(b,p+4)),fieldStart=u(b,p+32),methodStart=u(b,p+36),methods=struct.unpack_from('<H',b,p+64)[0],fields=struct.unpack_from('<H',b,p+68)[0],token=hex(u(b,p+84))))
print('types',len(types),'types_offset',hex(to))
# Locate the metadata registration pair fieldOffsetsCount, fieldOffsets, typeDefinitionsSizesCount.
raw=pe.__data__;pat=struct.pack('<Q',len(types));candidates=[]
for match in re.finditer(re.escape(pat),raw):
 p=match.start()
 if raw[p+16:p+24]!=pat:continue
 ptr=q(raw,p+8)
 try:
  values=[]
  for name in ['BoardConfig','BoardEntry','SavesManager']:
   t=next(t for t in types if t['name']==name); fp=nq(ptr+8*t['index']);v=list(struct.unpack('<'+'i'*t['fields'],native(fp,t['fields']*4)));values.append(v)
  if values[0][:2]==[16,24]:candidates.append((pe.get_rva_from_offset(p)+base,ptr,values))
 except Exception:pass
assert len(candidates)==1,candidates
reg,offsets,values=candidates[0];print('fieldOffsets pair',hex(reg),hex(offsets))
fields=[]
for t in types:
 if not t['fields']:continue
 fp=nq(offsets+8*t['index'])
 for j in range(t['fields']):
  p=fo+12*(t['fieldStart']+j)
  fields.append(dict(type=t['name'],namespace=t['namespace'],typeIndex=t['index'],name=st(u(b,p)),token=hex(u(b,p+8)),offset=struct.unpack('<i',native(fp+4*j,4))[0],type_index=u(b,p+4)))
(R/'evidence/native-fields.json').write_text(json.dumps(fields,indent=2))
(R/'evidence/native-types.json').write_text(json.dumps(types,indent=2))
# Derive CodeGenModule pointers by finding the exact image name string and references.
maps=[]
for im in range(isz//40):
 p=io+40*im; name=st(u(b,p)); start=u(b,p+8);count=u(b,p+12)
 if name not in ['Assembly-CSharp.dll','UnityEngine.CoreModule.dll','UnityEngine.AnimationModule.dll']:continue
 positions=[m.start() for m in re.finditer(re.escape(name.encode()+b'\0'),raw)]
 modules=[]
 for pos in positions:
  va=pe.get_rva_from_offset(pos)+base
  for ref in re.finditer(re.escape(struct.pack('<Q',va)),raw):
   a=ref.start(); cnt=q(raw,a+8);ptr=q(raw,a+16)
   if 0<cnt<10000 and base<ptr<base+pe.OPTIONAL_HEADER.SizeOfImage:modules.append((cnt,ptr))
 assert len(modules)==1,(name,modules)
 cnt,ptr=modules[0];print(name,cnt,hex(ptr))
 for t in types[start:start+count]:
  for j in range(t['methods']):
   # Version 31 method definition adds returnParameterToken; 36-byte layout.
   p=mo+36*(t['methodStart']+j);tok=u(b,p+24);rid=tok&0xffffff
   assert tok>>24==6,(t['name'],hex(tok),hex(p))
   va=nq(ptr+8*(rid-1))
   maps.append(dict(image=name,type=t['name'],name=st(u(b,p)),token=hex(tok),va=hex(va)))
(R/'evidence/native-method-map.json').write_text(json.dumps(maps,indent=2))
selected=['BoardConfig','BoardEntry','SavesManager','Save','MouseManager','ZombieManager','Zombie','Board','PrepareUIController','BoardPreview','EnemyManager','EnemySelecter','ZombieInfo','ZombieSelect','Plant','SunManager','Map','AttackRangeUIController']
lines=[]
for t in selected:
 lines.append('\n'+t)
 lines.extend(f"{f['token']} +0x{f['offset']:X} {f['name']}" for f in fields if f['type']==t)
(R/'evidence/target-field-offsets.txt').write_text('\n'.join(lines))
print('mapped methods',len(maps))
