"""Static native86 source/metadata inspection only. Executes no assembly or native method."""
from pathlib import Path
import hashlib, importlib.util, json, re, struct, sys, zipfile
sys.dont_write_bytecode=True
ROOT=Path(__file__).resolve().parents[2];OUT=Path(__file__).resolve().parent
sys.path.insert(0,str(ROOT/'audit/stage9-native4/python-deps'))
import capstone,dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
from dncil.clr.token import Token
pp=ROOT/'audit/stage9-native3/repository/Tools/Stage9NativeProbe/pc_method_probe.py'
ss=importlib.util.spec_from_file_location('probe86',pp);probe=importlib.util.module_from_spec(ss);sys.modules[ss.name]=probe;ss.loader.exec_module(probe)
native=(ROOT/'audit/stage9-native3/inputs/GameAssembly.dll').read_bytes();meta=(ROOT/'audit/stage9-native3/inputs/global-metadata.dat').read_bytes()
sha=lambda b:hashlib.sha256(b).hexdigest()
assert sha(native)=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
assert sha(meta)=='ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9'
pe=probe.PE(native);read=lambda va,n:native[pe.va_to_offset(va):pe.va_to_offset(va)+n]
nq=lambda va:struct.unpack('<Q',read(va,8))[0];u=lambda at:struct.unpack_from('<I',meta,at)[0]
so,to,ts,mo,io,isz=u(24),u(160),u(164),u(48),u(168),u(172)
st=lambda ix:meta[so+ix:meta.index(0,so+ix)].decode('utf-8')
types=[]
for i in range(ts//88):
 p=to+i*88;types.append({'index':i,'name':st(u(p)),'namespace':st(u(p+4)),'method_start':u(p+36),'methods':struct.unpack_from('<H',meta,p+64)[0]})
images=[]
for i in range(isz//40):
 p=io+i*40;images.append({'name':st(u(p)),'type_start':u(p+8),'type_count':u(p+12)})
def mdef(index):
 p=mo+index*36;t=types[u(p+4)]
 return {'global_method_index':index,'owner':t['namespace']+'.'+t['name'],'name':st(u(p)),'token':f'0x{u(p+24):08X}',
         'flags':hex(struct.unpack_from('<H',meta,p+28)[0]),'parameter_count':struct.unpack_from('<H',meta,p+34)[0],'return_type_index':u(p+8)}
def offset_va(off):
 return next(pe.image_base+s.va+off-s.raw_offset for s in pe.sections if s.raw_offset<=off<s.raw_offset+s.raw_size)
def module_pointer_table(name):
 candidates=set()
 for hit in re.finditer(re.escape(name.encode()+b'\0'),native):
  va=offset_va(hit.start())
  for ref in re.finditer(re.escape(struct.pack('<Q',va)),native):
   off=ref.start()
   if off+24>len(native):continue
   count,ptr=struct.unpack_from('<QQ',native,off+8)
   if not (0<count<100000):continue
   try:read(ptr,count*8)
   except ValueError:continue
   if ptr<pe.image_base:continue
   candidates.add((count,ptr,offset_va(off)))
 assert len(candidates)==1,(name,candidates)
 count,ptr,module=next(iter(candidates))
 return {'name':name,'module_va':hex(module),'method_count':count,'pointer_table_va':hex(ptr)},ptr
reg=0x1818C6D00;assert nq(reg+80)==len(types)
result={'scope':'Static evidence only; no candidate construction, tests, native/CIL execution, build, or existing-file edits.',
        'original_pe_sha256':sha(native),'original_metadata_sha256':sha(meta),'metadata_registration_va':hex(reg)}
slots=[]
for va in (0x181BBC628,0x181BB86F0,0x181BB8770,0x181BB8718):
 enc=nq(va);usage=(enc>>29)&7;ix=(enc&0x1fffffff)>>1;item={'slot':hex(va),'encoded':hex(enc),'usage':usage,'index':ix}
 if usage==5:
  ln,off=struct.unpack_from('<II',meta,u(8)+8*ix);b=meta[u(16)+off:u(16)+off+ln];item.update({'literal':b.decode('utf-8'),'utf8_hex':b.hex()})
 elif usage==6:
  di,ci,mi=struct.unpack('<iii',read(nq(reg+72)+12*ix,12));item.update(mdef(di));item.update({'class_inst_index':ci,'method_inst_index':mi})
  if ci>=0:
   inst=nq(nq(reg+24)+8*ci);argc=struct.unpack('<I',read(inst,4))[0];argv=nq(inst+8);aa=[]
   for j in range(argc):
    data,bits=struct.unpack('<QI',read(nq(argv+8*j),12));kind=(bits>>16)&255;a={'kind':hex(kind),'data':hex(data)}
    if kind in (0x11,0x12) and data<len(types):a['type']=types[data]
    aa.append(a)
   item['class_inst_arguments']=aa
 slots.append(item)
result['original_metadata_slots']=slots
result['window_jump_table']=[{'Q':i,'target_va':hex(pe.image_base+struct.unpack('<I',read(0x1803B056C+i*4,4))[0])} for i in range(6)]
srcpath=ROOT/'audit/native30-continuation-2026-10-02/native55-native.json';src=json.loads(srcpath.read_text(encoding='utf-8'))
result['native55_source_sha256']=sha(srcpath.read_bytes());result['native_bodies']=[]
pdata=pe.section('.pdata');runtime=[]
for off in range(pdata.raw_offset,pdata.raw_offset+pdata.raw_size-11,12):
 lo,hi,un=struct.unpack_from('<III',native,off)
 if lo and hi:runtime.append((pe.image_base+lo,pe.image_base+hi,pe.image_base+un))
for row in src['methods']:
 if row['token'] not in ('0x060006DF','0x060006FE'):continue
 lo,hi=int(row['start'],16),int(row['end'],16);blob=read(lo,hi-lo)
 assert sha(blob)==row['native_sha256'] and nq(probe.DEFAULT_TABLE_VA+((int(row['token'],16)&0xffffff)-1)*8)==lo
 parts=[{'start':hex(r[0]),'end':hex(r[1]),'unwind_flags':read(r[2],1)[0]>>3} for r in runtime if lo<=r[0]<hi]
 assert [(int(x['start'],16),int(x['end'],16)) for x in parts]==[tuple(x) for x in row['pdata_parts']]
 result['native_bodies'].append({'token':row['token'],'name':row['method'],'start':hex(lo),'end':hex(hi),'sha256':sha(blob),'matches_original_pointer_and_slice':True,'boundary_scope':row['boundary_scope'],'pdata_parts':parts})
 (OUT/(row['token']+'-native55.asm')).write_text(row['disassembly'],encoding='utf-8')

dis=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64)
result['independent_original_helper_mappings']=[]
table_cache={}
for owner,names in [('TMP_Text',{'get_textInfo'}),('TMP_LinkInfo',{'GetLinkID'}),('TMP_TextUtilities',{'FindIntersectingLink'}),('PointerEventData',{'get_position'}),('String',{'op_Equality'})]:
 tt=next(t for t in types if t['name']==owner and (owner!='String' or t['namespace']=='System'))
 image=next(i for i in images if i['type_start']<=tt['index']<i['type_start']+i['type_count'])
 if image['name'] not in table_cache:table_cache[image['name']]=module_pointer_table(image['name'])
 descriptor,ptr=table_cache[image['name']]
 for index in range(tt['method_start'],tt['method_start']+tt['methods']):
  m=mdef(index)
  if m['name'] not in names:continue
  rid=int(m['token'],16)&0xffffff;va=nq(ptr+8*(rid-1));item={**m,'image':image['name'],'codegen_module':descriptor,'native_va':hex(va)}
  if m['name'] in ('get_textInfo','get_position'):
   rf=next((r for r in runtime if r[0]==va),None)
   if rf:hi=rf[1];bound='exact .pdata'
   else:
    rr=[]
    for ins in dis.disasm(read(va,128),va):
     rr.append(ins)
     if ins.mnemonic=='ret':break
    assert rr and rr[-1].mnemonic=='ret'
    hi=rr[-1].address+rr[-1].size;bound='leaf through first ret, exact getter instructions shown'
   blob=read(va,hi-va);item.update({'end':hex(hi),'boundary':bound,'sha256':sha(blob),'disassembly':[f'{i.address:016x}: {i.bytes.hex():<32} {i.mnemonic} {i.op_str}' for i in dis.disasm(blob,va)]})
  result['independent_original_helper_mappings'].append(item)
fields=json.loads((ROOT/'audit/stage9-native3/evidence/native-fields.json').read_text())
fieldset={'Vector2':{'x','y'},'TextLink':{'TMP_Text','linkIndex'},'Window_Q':{'Q','info0_int'},'MainUIController':{'<instance>k__BackingField','savesManager','savesUIController'},
          'SavesUIController':{'playerSaveList'},'SavesUIController_PlayerSave':{'savesManager','playerSaveID'},'PointerEventData':{'<position>k__BackingField'},'TMP_Text':{'m_textInfo'},'TMP_TextInfo':{'linkInfo'}}
result['original_fields']=[]
for f in fields:
 if f['type'] not in fieldset or f['name'] not in fieldset[f['type']]:continue
 type_ptr=nq(nq(reg+56)+8*f['type_index']);data,bits=struct.unpack('<QI',read(type_ptr,12))
 result['original_fields'].append({**f,'field_attributes_from_type_bits':hex(bits&0xffff)})

archive=ROOT/'audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip';member='candidates/native82/candidate1.dll'
with zipfile.ZipFile(archive) as z:parent=z.read(member)
assert sha(parent)=='48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf'
clr=dnfile.dnPE(data=parent);tables=clr.net.mdtables;owner_m={};owner_f={}
for td in tables.TypeDef.rows:
 name=(str(td.TypeNamespace)+'.' if str(td.TypeNamespace) else '')+str(td.TypeName)
 for ix in td.MethodList:owner_m[ix.row_index]=name
 for ix in td.FieldList:owner_f[ix.row_index]=name
def tokenid(tok):
 table,rid=tok>>24,tok&0xffffff
 if table==6:
  row=tables.MethodDef.rows[rid-1];return {'token':f'0x{tok:08X}','owner':owner_m[rid],'name':str(row.Name),'signature_hex':row.Signature.value.hex()}
 if table==4:
  row=tables.Field.rows[rid-1];return {'token':f'0x{tok:08X}','owner':owner_f[rid],'name':str(row.Name),'signature_hex':row.Signature.value.hex()}
 if table==10:
  row=tables.MemberRef.rows[rid-1];return {'token':f'0x{tok:08X}','name':str(row.Name),'signature_hex':row.Signature.value.hex()}
 return {'token':f'0x{tok:08X}'}
current={'parent_sha256':sha(parent),'archive_path':str(archive),'archive_sha256':sha(archive.read_bytes()),'member':member,'methods':[],'fields':[],'external_member_bindings':[]}
for rid,row in enumerate(tables.MethodDef.rows,1):
 if rid not in (0x6df,0x6fe,0x636,0x63a,0x23c,0x238,0x700,0x701,0x702):continue
 item={**tokenid(0x06000000+rid),'public':row.Flags.mdPublic,'private':row.Flags.mdPrivate,'static':row.Flags.mdStatic,'rva':row.Rva}
 if row.Rva and rid in (0x636,0x6df):
  body=read_method_body_from_bytes(clr.get_data(row.Rva));item['body_sha256']=sha(body.get_bytes());item['raw_body_hex']=body.get_bytes().hex()
  item['instructions']=[]
  for ins in body.instructions:
   op=ins.operand
   if isinstance(op,Token):
    if op.value>>24==0x70:op={'token':f'0x{op.value:08X}','literal':clr.net.user_strings.get(op.value&0xffffff).value}
    else:op=tokenid(op.value)
   elif isinstance(op,int) and str(ins.opcode.name).startswith(('br','beq','bne')):op={'target_cil_offset':op-body.header_size}
   elif op is not None:op=str(op)
   item['instructions'].append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode.name),'operand':op})
 current['methods'].append(item)
for rid,row in enumerate(tables.Field.rows,1):
 if owner_f[rid] not in fieldset or str(row.Name) not in fieldset[owner_f[rid]]:continue
 current['fields'].append({**tokenid(0x04000000+rid),'public':row.Flags.fdPublic,'private':row.Flags.fdPrivate,'static':row.Flags.fdStatic})
for rid,row in enumerate(tables.MemberRef.rows,1):
 owner=row.Class.row
 if str(getattr(owner,'TypeName','')) not in ('Vector2','TMP_TextInfo','PointerEventData','TMP_Text','TMP_TextUtilities','TMP_LinkInfo'):continue
 current['external_member_bindings'].append({'token':f'0x{0x0A000000+rid:08X}','owner':str(getattr(owner,'TypeNamespace',''))+'.'+str(owner.TypeName),'name':str(row.Name),'signature_hex':row.Signature.value.hex()})
result['actual_current_parent_metadata']=current
inventory_path=ROOT/'audit/native86-codex-2026-10-13/metadata-current.json';inv=json.loads(inventory_path.read_text());assert inv['input_sha256']==sha(parent)
result['current_inventory']={'path':str(inventory_path),'sha256':sha(inventory_path.read_bytes()),'parent_sha256':inv['input_sha256'],
 'targets':[x for x in inv['targets'] if x['token'] in ('0x060006DF','0x060006FE')],
 'relevant_refs':[x for x in inv['refs'] if any(s in x['name'] for s in ('get_textInfo','FindIntersectingLink','GetLinkID','OpenURL','SavesUIController_PlayerSave>::get_','UnityEngine.Object::op_','PointerEventData','UnityEngine.Vector2::.ctor','UnityEngine.Vector3::.ctor','Component::get_gameObject'))],
 'external_fields':[x for x in inv['external_fields'] if any(s in x['name'] for s in ('TMP_TextInfo','TMP_Text::','Vector2::','Vector3::'))],
 'type_refs':[x for x in inv['type_refs'] if any(s in x['name'] for s in ('PointerEventData','Vector2','TMP_TextInfo','TMP_LinkInfo','TMP_Text'))]}
result['proven_list_public_bodies']=json.loads((ROOT/'audit/native81-spec-2026-10-13/PRIMARY-EVIDENCE.json').read_text())['resolver_list_bodies']
assert sha((ROOT/'audit/stage9-native3/resolver/mscorlib.dll').read_bytes())==result['proven_list_public_bodies']['sha256']
(OUT/'PRIMARY-EVIDENCE.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps({k:result[k] for k in ('scope','native_bodies','original_metadata_slots','window_jump_table','independent_original_helper_mappings')},ensure_ascii=False,indent=2))
