"""Original native/metadata source reading only; no application execution or tests."""
from pathlib import Path
import struct,json,hashlib,sys,re
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
pe=pefile.PE(str(R/'audit/stage9-native3/inputs/GameAssembly.dll'));base=pe.OPTIONAL_HEADER.ImageBase
b=(R/'audit/stage9-native3/inputs/global-metadata.dat').read_bytes();raw=pe.__data__
u=lambda data,o=0:struct.unpack_from('<I',data,o)[0]
q=lambda data,o=0:struct.unpack_from('<Q',data,o)[0]
native=lambda va,n:pe.get_data(va-base,n)
nq=lambda va:q(native(va,8))
so,to,mo,po=u(b,24),u(b,160),u(b,48),u(b,88)
st=lambda i:b[so+i:b.index(0,so+i)].decode('utf-8')
types=json.loads((R/'audit/stage9-native3/evidence/native-types.json').read_text())
fields=json.loads((R/'audit/stage9-native3/evidence/native-fields.json').read_text())
maps=json.loads((R/'audit/stage9-native3/evidence/native-method-map.json').read_text())
reg=0x1818c6d00;typeTable=nq(reg+56);instTable=nq(reg+24);specTable=nq(reg+72)
def inst(ptr):
 if not ptr:return None
 count=u(native(ptr,4));args=nq(ptr+8)
 return {'ptr':hex(ptr),'count':count,'args':[typeptr(nq(args+k*8))for k in range(count)]}
def typeptr(ptr):
 data=nq(ptr);bits=u(native(ptr+8,4));kind=(bits>>16)&255
 r={'ptr':hex(ptr),'data':hex(data),'bits':hex(bits),'kind':hex(kind),'attrs':hex(bits&65535),'byref':bool(bits&(1<<30))}
 if kind in (0x11,0x12):r['typedef']=types[data]
 if kind in (0x1,0x2,0x8,0xc,0xe,0x1c):r['primitive_name']={1:'System.Void',2:'System.Boolean',8:'System.Int32',12:'System.Single',14:'System.String',28:'System.Object'}[kind]
 if kind==0x15:r['generic']={'class_ptr':hex(data),'base':typeptr(nq(data)),'class_inst':inst(nq(data+8)),'method_inst':inst(nq(data+16))}
 if kind==0x13:r['generic_parameter_index']=data
 return r
def typ(i):return dict(typeptr(nq(typeTable+i*8)),index=i)
def method(index):
 p=mo+index*36
 name,owner,ret,rpt,param,gc,token,flags,iflags,slot,count=struct.unpack_from('<IIIIiiIHHHH',b,p)
 t=types[owner]
 return {'index':index,'definition_offset':hex(p),'owner':t['namespace']+'.'+t['name'] if t['namespace'] else t['name'],'name':st(name),'token':hex(token),'flags':hex(flags),'impl_flags':hex(iflags),'slot':slot,'return_type':typ(ret),'parameters':[{'name':st(u(b,po+12*(param+k))),'type':typ(u(b,po+12*(param+k)+8))}for k in range(count)]}
def ms(i):
 m,c,g=struct.unpack('<iii',native(specTable+i*12,12))
 return {'index':i,'record_address':hex(specTable+i*12),'method_index':m,'class_inst_index':c,'method_inst_index':g,'method':method(m),'class_inst':inst(nq(instTable+c*8))if c>=0 else None,'method_inst':inst(nq(instTable+g*8))if g>=0 else None}
cells=[]
for va in [0x181bc3460,0x181bc34b8,0x181bc3510,0x181ba51b8,0x181babe78,0x181bb1fc8,0x181ba2018,0x181ba4278,0x181bb6478]:
 val=nq(va);kind=(val>>29)&7;idx=(val>>1)&0xfffffff
 r={'cell':hex(va),'raw_bytes':native(va,8).hex(),'encoded':hex(val),'kind':kind,'source_index':idx}
 if kind==6:r['method_spec']=ms(idx)
 if kind in (1,2):r['type']=typ(idx)
 cells.append(r)
needed={'Plant':['SetBrightness'],'ProjectManager':['PlayAudio_Click'],'GlobalStaticVars':['AudioVolume','CreateAudioAtPoint'],'Renderer':['GetMaterial','get_material'],'Material':['SetFloat'],'Random':['Range'],'Camera':['get_main'],'Component':['get_transform'],'Transform':['get_position'],'GameObject':['GetComponent'],'Enumerator':['Dispose','MoveNext','get_Current'],'List`1':['GetEnumerator','get_Item']}
methods=[]
for t in types:
 if t['name'] not in needed:continue
 for j in range(t['methods']):
  m=method(t['methodStart']+j)
  if m['name'] not in needed[t['name']]:continue
  if t['name'] in ['Enumerator','List`1'] and t['namespace']!='System.Collections.Generic':continue
  match=[x for x in maps if int(x['token'],16)==int(m['token'],16)and x['type']==t['name']]
  methods.append(dict(m,native_map=match))
nativeBodies=[]
D=json.loads((R/'audit/native30-continuation-2026-10-02/native55-native.json').read_text())
for m in D['methods']:
 if m['token'] not in ['0x060003A0','0x06000206']:continue
 data=b''.join(native(a,z-a)for a,z in m['pdata_parts'])
 nativeBodies.append({'token':m['token'],'start':m['start'],'end':m['end'],'parts':m['pdata_parts'],'bytes':len(data),'sha256':hashlib.sha256(data).hexdigest(),'recorded_sha256':m['native_sha256']})
enum=next(t for t in types if t['name']=='ProjectType')
defaultOffset,defaultSize,dataOffset=u(b,64),u(b,68),u(b,72)
defaults=[]
for i in range(defaultSize//12):
 fi,ty,di=struct.unpack_from('<iii',b,defaultOffset+i*12)
 if enum['fieldStart']<=fi<enum['fieldStart']+enum['fields']:
  f=fields[next(j for j,f in enumerate(fields)if f['typeIndex']==enum['index']and int(f['token'],16)==u(b,u(b,96)+fi*12+8))]
  defaults.append({'field':f,'type':typ(ty),'data_index':di,'data_address':hex(dataOffset+di),'first_encoded_byte':hex(b[dataOffset+di]),'note':'Version 31 default I4 values use compressed storage; branch numeric behavior is derived from native switch, not a fixed-width read here.'})
floatConstants=[{'va':hex(va),'bytes':native(va,4).hex(),'value':struct.unpack('<f',native(va,4))[0]}for va in [0x1815a7a10,0x1815a7a14,0x1815a7ac4,0x1815a7abc,0x1815a7ad0]]
out={'source_hashes':{p.name:hashlib.sha256(p.read_bytes()).hexdigest()for p in [R/'audit/stage9-native3/inputs/GameAssembly.dll',R/'audit/stage9-native3/inputs/global-metadata.dat']},'registration':{'address':hex(reg),'type_count':nq(reg+48),'types':hex(typeTable),'generic_inst_count':nq(reg+16),'generic_insts':hex(instTable),'method_spec_count':nq(reg+64),'method_specs':hex(specTable)},'native_bodies':nativeBodies,'cells':cells,'fields':[dict(f,decoded_type=typ(f['type_index']))for f in fields if(f['type']=='Plant'and f['name']in ['animationSprites','Mat_Brightness'])or(f['type']=='Project'and f['name']=='projectType')or(f['type']=='ResourceManager'and f['name']=='projectClips')],'methods':methods,'project_type_constants':defaults,'float_constants':floatConstants,'playaudio_switch':[{'project_type':i,'target':hex(base+u(native(0x180323a00+i*4,4)))}for i in range(9)]}
# Re-derive needed concrete methods from original code-generation module pointers.
independent=[]
for image in ['UnityEngine.CoreModule.dll','Assembly-CSharp.dll']:
 modules=[]
 for match in re.finditer(re.escape(image.encode()+b'\0'),raw):
  va=pe.get_rva_from_offset(match.start())+base
  for ref in re.finditer(re.escape(struct.pack('<Q',va)),raw):
   a=ref.start();cnt=q(raw,a+8);ptr=q(raw,a+16)
   if 100<cnt<100000 and base<ptr<base+pe.OPTIONAL_HEADER.SizeOfImage:modules.append((pe.get_rva_from_offset(a)+base,cnt,ptr))
 if len(modules)!=1:raise ValueError((image,modules))
 module,cnt,ptr=modules[0]
 for m in methods:
  if not any(x['image']==image for x in m['native_map']):continue
  rid=int(m['token'],16)&0xffffff
  independent.append({'image':image,'module':hex(module),'pointer_count':cnt,'pointer_table':hex(ptr),'owner':m['owner'],'name':m['name'],'token':m['token'],'native':hex(nq(ptr+8*(rid-1)))})
out['independent_code_module_mapping']=independent
print(json.dumps(out,indent=2,ensure_ascii=False))
