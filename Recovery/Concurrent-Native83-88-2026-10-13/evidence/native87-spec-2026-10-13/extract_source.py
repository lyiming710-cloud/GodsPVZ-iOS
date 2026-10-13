"""Static original PE/IL2CPP metadata inspection; no tests or execution."""
from pathlib import Path
import sys,struct,json,re,hashlib,importlib.util
sys.dont_write_bytecode=True
R=Path(__file__).resolve().parents[2];O=Path(__file__).resolve().parent
sys.path.insert(0,str(R/'audit/stage9-native4/python-deps'))
import capstone
s=importlib.util.spec_from_file_location('probe87',R/'audit/stage9-native3/repository/Tools/Stage9NativeProbe/pc_method_probe.py');probe=importlib.util.module_from_spec(s);sys.modules[s.name]=probe;s.loader.exec_module(probe)
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
reg=0x1818C6D00;table=0x1818EDEB0;ptrindex={nq(table+i*8):i for i in range(nq(reg+48))}
def image(ti):
 for k in range(u(b,172)//40):
  p=u(b,168)+40*k;a,z=u(b,p+8),u(b,p+12)
  if a<=ti<a+z:return st(u(b,p))
def td(i):
 p=u(b,160)+88*i;return dict(types[i],image=image(i),byvalTypeIndex=u(b,p+8),parentTypeIndex=u(b,p+16),elementTypeIndex=u(b,p+20),flags=hex(u(b,p+28)),bitfield=hex(u(b,p+80)))
def typ(i):
 p=nq(table+8*i);v=nat(p,16);d=struct.unpack_from('<Q',v)[0];bits=u(v,8);k=(bits>>16)&255
 t={'index':i,'pointer':hex(p),'bytes':v.hex(),'data':d,'kind':hex(k),'byref':bool(bits&(1<<29)),'attrs':hex(bits&65535),'bits':hex(bits)}
 if k in (0x11,0x12,0x1c):t['typedef']=td(d)
 if k==0x15:
  dt=nq(d);ci,mi=struct.unpack('<QQ',nat(d+8,16));t['genericClass']={'pointer':hex(d),'definition':typ(ptrindex[dt]),'classInst':inst_ptr(ci) if ci else None,'methodInst':inst_ptr(mi) if mi else None}
 return t
def inst_ptr(p):
 n=u(nat(p,16));argv=nq(p+8);return {'pointer':hex(p),'argc':n,'args':[typ(ptrindex[nq(argv+8*j)]) for j in range(n)]}
def inst(i):return None if i<0 else dict(inst_ptr(nq(nq(reg+24)+8*i)),index=i)
def method(i):
 p=u(b,48)+36*i;n,owner,ret,rpt,ps,gc,tok,flags,iflags,slot,num=struct.unpack_from('<IIIIiiIHHHH',b,p)
 return {'index':i,'owner':types[owner]['namespace']+'.'+types[owner]['name'],'image':image(owner),'name':st(n),'token':hex(tok),'return':typ(ret),'parameters':[{'name':st(u(b,u(b,88)+12*(ps+j))),'type':typ(u(b,u(b,88)+12*(ps+j)+8))} for j in range(num)],'genericContainer':gc,'flags':hex(flags),'implflags':hex(iflags),'slot':slot}
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
result={'scope':'Static original native/metadata source only; no tests, builds, target execution or candidate generation.','inputs':{'GameAssembly_sha256':sha(raw),'metadata_sha256':sha(b)},'cells':[cell(v) for v in [0x181BA2018,0x181BA4278,0x181BB0500,0x181BB6478,0x181BB1890,0x181BA7408]]}
src=R/'audit/native30-continuation-2026-10-02/native55-native.json';rows=json.loads(src.read_text())['methods'];result['native55_sha256']=sha(src.read_bytes());result['targets']=[];calls=set()
for row in rows:
 if row['token'] not in ['0x06000199','0x06000386']:continue
 a,z=int(row['start'],16),int(row['end'],16);v=nat(a,z-a)
 assert nq(0x181B82D60+8*((int(row['token'],16)&0xffffff)-1))==a and sha(v)==row['native_sha256']
 result['targets'].append({'token':row['token'],'name':row['method'],'start':row['start'],'end':row['end'],'bytes':len(v),'sha256':sha(v),'pdata_parts':row['pdata_parts'],'reference_locals_only':row['managed']['locals']})
 (O/(row['token']+'-native55.asm')).write_text(row['disassembly'],encoding='utf-8')
 calls.update(int(m.group(1),16) for m in re.finditer(r'(?:call|jmp)\s+0x([0-9a-f]+)',row['disassembly']))
extra=[m for m in maps if m['type']=='ParticlesManager' and m['name']=='get_instance']
calls.update(int(m['va'],16) for m in extra)
result['method_maps']=[]
for rec in maps:
 if int(rec['va'],16) not in calls:continue
 for ti,t in enumerate(types):
  if t['name']!=rec['type'] or image(ti)!=rec['image']:continue
  for j in range(t['methods']):
   m=method(t['methodStart']+j)
   if m['token']!=rec['token']:continue
   if rec['image']=='Assembly-CSharp.dll':assert nq(0x181B82D60+8*((int(rec['token'],16)&0xffffff)-1))==int(rec['va'],16)
   result['method_maps'].append(dict(rec,metadata=m))
result['target_metadata']=[method(types[ti]['methodStart']+j) for ti in [5154,5207] for j in range(types[ti]['methods']) if method(types[ti]['methodStart']+j)['token'] in ['0x6000199','0x6000386']]
result['fields']=[dict(f,registered_type=typ(f['type_index'])) for f in fields if (f['type']=='ResourceManager' and f['name'] in ['dialogueClips','particleClips']) or (f['type']=='ParticlesManager' and f['name']=='<instance>k__BackingField') or (f['type']=='Plant' and f['name']=='board')]
entries=struct.unpack('<IIIIIII',nat(0x18031453C,28));result['dialogue_switch']={'table_va':'0x18031453c','bytes':nat(0x18031453C,28).hex(),'entries':[{'enum_bits':i,'target':hex(base+x)} for i,x in enumerate(entries)]}
result['dialogue_enum']=td(5153);result['dialogue_enum']['fields']=[dict(f,registered_type=typ(f['type_index'])) for f in fields if f['typeIndex']==5153]
result['original_enum_defaults']=[]
result['original_potato_particle_enum']=[]
particle_type=next(t for t in types if t['name']=='ParticleState' and image(t['index'])=='Assembly-CSharp.dll')
for k in range(u(b,68)//12):
 p=u(b,64)+12*k;fi,ti,di=struct.unpack_from('<iii',b,p)
 if particle_type['fieldStart']<=fi<particle_type['fieldStart']+particle_type['fields']:
  fp=u(b,96)+12*fi;dp=u(b,72)+di
  if st(u(b,fp))=='PotatoBoom':result['original_potato_particle_enum'].append({'enum_type':td(particle_type['index']),'field':st(u(b,fp)),'token':hex(u(b,fp+8)),'field_index':fi,'type_index':ti,'data_index':di,'compressed_first_byte':b[dp],'decoded_nonnegative_i4':b[dp]>>1 if b[dp]<128 and not b[dp]&1 else None})
 if not types[5153]['fieldStart']<=fi<types[5153]['fieldStart']+types[5153]['fields']:continue
 fp=u(b,96)+12*fi;dp=u(b,72)+di
 result['original_enum_defaults'].append({'field_index':fi,'name':st(u(b,fp)),'type_index':ti,'data_index':di,'data_prefix_hex':b[dp:dp+8].hex(),'compressed_i4_first_byte':b[dp],'decoded_nonnegative_i4':b[dp]>>1 if b[dp]<128 and not b[dp]&1 else None})
result['float32_constants']=[{'va':hex(v),'bytes':nat(v,4).hex(),'bits':hex(u(nat(v,4))),'float32':struct.unpack('<f',nat(v,4))[0]} for v in [0x1815A7A10,0x1815A7BE0,0x1815A7C04,0x1815A7AC8,0x1815A7A14,0x1815A7AC4,0x1815A7AB4,0x1815A7AA0]]
result['generic_rows']=[];gdefs={c['spec']['definition']['index'] for c in result['cells'] if c['kind']==6}
result['list_index_throw_source']=[]
for ti,t in enumerate(types):
 if t['namespace']!='System' or t['name']!='ThrowHelper':continue
 for j in range(t['methods']):
  m=method(t['methodStart']+j)
  if m['name']!='ThrowArgumentOutOfRange_IndexException':continue
  rid=int(m['token'],16)&0xffffff
  result['list_index_throw_source'].append({'metadata':m,'module_pointer_table':'0x181c4e110','pointer_cell':hex(0x181C4E110+8*(rid-1)),'native_pointer':hex(nq(0x181C4E110+8*(rid-1)))})
for j in range(nq(reg+32)):
 p=nq(reg+40)+16*j;si,mi,iv,ad=struct.unpack('<iiii',nat(p,16));md,ci,gi=struct.unpack('<iii',nat(nq(reg+72)+12*si,12))
 if md not in gdefs:continue
 result['generic_rows'].append({'row':j,'record':hex(p),'spec':spec(si),'methodPointerIndex':mi,'pointer_cell':hex(0x181760110+8*mi),'native_pointer':hex(nq(0x181760110+8*mi)),'invokerIndex':iv,'adjustorIndex':ad})
ds=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64);pdata=pe.section('.pdata');result['native_helpers']=[]
helpervas={0x18031AEA0,0x18031B350,0x180825710,0x18130D310,0x18130D350}|{int(m['va'],16) for m in extra}
for va in sorted(helpervas):
 end=None
 for p in range(pdata.raw_offset,pdata.raw_offset+pdata.raw_size-11,12):
  a,z,uw=struct.unpack_from('<III',raw,p)
  if base+a==va:end=base+z;break
 if va==0x18031B350:
  end=min(int(m['va'],16) for m in maps if int(m['va'],16)>va)
 if end is None:
  mapped=[int(m['va'],16) for m in maps if int(m['va'],16)>va];end=min(mapped) if mapped else va+256
 v=nat(va,end-va)
 result['native_helpers'].append({'start':hex(va),'end':hex(end),'scope':'through next mapped pointer including all split pdata' if va==0x18031B350 else 'pdata or next pointer','sha256':sha(v),'bytes':v.hex(),'disassembly':[f'{x.address:016x}: {x.bytes.hex():<32} {x.mnemonic} {x.op_str}' for x in ds.disasm(v,va)]})
result['audio_module_call_maps']=[]
audio_targets={0x1812DBE00,0x1812DC410,0x1812DC4B0,0x1812DC340,0x1812DC460,0x1812DC300}
def offset_va(p):
 for sec in pe.sections:
  if sec.raw_offset<=p<sec.raw_offset+sec.raw_size:return base+sec.va+p-sec.raw_offset
 return None
name='UnityEngine.AudioModule.dll';mods=[]
for hit in re.finditer(re.escape(name.encode()+b'\0'),raw):
 va=offset_va(hit.start())
 if va is None:continue
 for ref in re.finditer(re.escape(struct.pack('<Q',va)),raw):
  count,ptr=struct.unpack_from('<QQ',raw,ref.start()+8)
  if 10<count<100000 and base<ptr<base+0x3000000:mods.append((offset_va(ref.start()),count,ptr))
assert len(mods)==1,mods
module,count,ptr=mods[0]
for ti,t in enumerate(types):
 if image(ti)!=name:continue
 for j in range(t['methods']):
  m=method(t['methodStart']+j);rid=int(m['token'],16)&0xffffff;va=nq(ptr+8*(rid-1))
  if va in audio_targets:result['audio_module_call_maps'].append({'module_va':hex(module),'pointer_count':count,'pointer_table':hex(ptr),'va':hex(va),'metadata':m})
result['random_icall_name']={'va':'0x181621750','text':nat(0x181621750,160).split(b'\0',1)[0].decode('ascii')}
(O/'PRIMARY-EVIDENCE.json').write_text(json.dumps(result,indent=2,ensure_ascii=False),encoding='utf-8');print(json.dumps(result,indent=2,ensure_ascii=True))
