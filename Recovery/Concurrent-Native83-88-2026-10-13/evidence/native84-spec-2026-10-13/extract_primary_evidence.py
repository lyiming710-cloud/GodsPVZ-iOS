"""Static byte/metadata extraction only; never executes native bodies or assembly CIL."""
from pathlib import Path
import hashlib, importlib.util, json, struct, sys, zipfile
sys.dont_write_bytecode=True
ROOT=Path(__file__).resolve().parents[2]
OUT=Path(__file__).resolve().parent
sys.path.insert(0,str(ROOT/'audit/stage9-native4/python-deps'))
import capstone, dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
from dncil.clr.token import Token
probe_path=ROOT/'audit/stage9-native3/repository/Tools/Stage9NativeProbe/pc_method_probe.py'
spec=importlib.util.spec_from_file_location('probe84',probe_path)
probe=importlib.util.module_from_spec(spec);sys.modules[spec.name]=probe;spec.loader.exec_module(probe)
native=(ROOT/'audit/stage9-native3/inputs/GameAssembly.dll').read_bytes()
meta=(ROOT/'audit/stage9-native3/inputs/global-metadata.dat').read_bytes()
sha=lambda b:hashlib.sha256(b).hexdigest()
assert sha(native)=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
assert sha(meta)=='ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9'
pe=probe.PE(native)
read=lambda va,n:native[pe.va_to_offset(va):pe.va_to_offset(va)+n]
nq=lambda va:struct.unpack('<Q',read(va,8))[0]
u=lambda at:struct.unpack_from('<I',meta,at)[0]
so,to,ts,mo=u(24),u(160),u(164),u(48)
st=lambda ix:meta[so+ix:meta.index(0,so+ix)].decode('utf-8')
types=[]
for i in range(ts//88):
 p=to+i*88;types.append({'index':i,'name':st(u(p)),'namespace':st(u(p+4)),'field_start':u(p+32),'fields':struct.unpack_from('<H',meta,p+68)[0]})
def methoddef(index):
 p=mo+index*36;t=types[u(p+4)]
 return {'global_method_index':index,'owner':t['namespace']+'.'+t['name'],'name':st(u(p)),'token':f'0x{u(p+24):08X}',
         'flags':hex(struct.unpack_from('<H',meta,p+28)[0]),'parameter_count':struct.unpack_from('<H',meta,p+34)[0]}
reg=0x1818C6D00
assert nq(reg+80)==len(types) and nq(reg+96)==len(types)
result={'scope':'Read-only source/PE/metadata/MethodBody analysis only; no candidate construction, CIL/native-body execution, tests, builds, or acceptance.',
        'original_pe_sha256':sha(native),'original_metadata_sha256':sha(meta),'metadata_registration_va':hex(reg)}
result['original_metadata_slots']=[]
for va in (0x181BA2018,0x181BA4278,0x181BB6478,0x181BD1A40,0x181BC6F38):
 encoded=nq(va);usage=(encoded>>29)&7;index=(encoded&0x1fffffff)>>1
 item={'slot':hex(va),'encoded':hex(encoded),'usage':usage,'index':index}
 if usage==5:
  length,off=struct.unpack_from('<II',meta,u(8)+8*index);b=meta[u(16)+off:u(16)+off+length]
  item.update({'literal':b.decode('utf-8'),'utf8_hex':b.hex()})
 elif usage==6:
  definition,ci,mi=struct.unpack('<iii',read(nq(reg+72)+12*index,12));item.update(methoddef(definition));item.update({'class_inst_index':ci,'method_inst_index':mi})
  if ci>=0:
   inst=nq(nq(reg+24)+8*ci);count=struct.unpack('<I',read(inst,4))[0];argv=nq(inst+8);args=[]
   for j in range(count):
    p=nq(argv+8*j);data,bits=struct.unpack('<QI',read(p,12));kind=(bits>>16)&255
    a={'kind':hex(kind),'data':hex(data),'bits':hex(bits)}
    if kind in (0x11,0x12) and data<len(types):a['definition']=types[data]
    args.append(a)
   item['class_inst_arguments']=args
 elif usage==1:
  ptr=nq(nq(reg+56)+8*index);data,bits=struct.unpack('<QI',read(ptr,12));kind=(bits>>16)&255
  item.update({'type_pointer':hex(ptr),'type_kind':hex(kind),'type_data':hex(data),'type_bits':hex(bits)})
  if kind in (0x11,0x12) and data<len(types):item['definition']=types[data]
 result['original_metadata_slots'].append(item)
result['mouse_jump_table']=[{'ItemType':i,'target_va':hex(pe.image_base+struct.unpack('<I',read(0x18031D9D4+4*i,4))[0])} for i in range(7)]
enum=next(t for t in types if t['name']=='ItemType')
defaults={}
for off in range(u(64),u(64)+u(68),12):
 field_index,type_index,data_index=struct.unpack_from('<iii',meta,off);defaults[field_index]=(type_index,data_index)
result['original_item_type_constants']=[]
for j in range(enum['fields']):
 fi=enum['field_start']+j;p=u(96)+12*fi
 if fi not in defaults:continue
 ti,di=defaults[fi];encoded=meta[u(72)+di]
 # These original v31 enum Int32 defaults are one-byte nonnegative encoded
 # signed values (0,2,4,...). Their byte boundaries are the original defaults'
 # successive data indexes; do not read an uncompressed four-byte integer.
 assert encoded<128 and encoded&1==0
 value=encoded>>1
 result['original_item_type_constants'].append({'name':st(u(p)),'token':f'0x{u(p+8):08X}','value':value,'encoded_default_byte':hex(encoded),'field_global_index':fi,'default_data_index':di})

src_path=ROOT/'audit/native30-continuation-2026-10-02/native55-native.json'
src=json.loads(src_path.read_text(encoding='utf-8'));result['native55_source_sha256']=sha(src_path.read_bytes());result['native_bodies']=[]
pdata=pe.section('.pdata');rf=[]
for off in range(pdata.raw_offset,pdata.raw_offset+pdata.raw_size-11,12):
 lo,hi,unwind=struct.unpack_from('<III',native,off)
 if lo and hi:rf.append((pe.image_base+lo,pe.image_base+hi,pe.image_base+unwind))
for row in src['methods']:
 if row['token'] not in ('0x060001C8','0x060002CE'):continue
 start,end=int(row['start'],16),int(row['end'],16);blob=read(start,end-start)
 assert sha(blob)==row['native_sha256'] and nq(probe.DEFAULT_TABLE_VA+((int(row['token'],16)&0xffffff)-1)*8)==start
 r=next(x for x in rf if x[0]==start)
 parts=[{'start':hex(x[0]),'end':hex(x[1]),'unwind':hex(x[2]),'unwind_flags':read(x[2],1)[0]>>3} for x in rf if start<=x[0]<end]
 assert [(int(x['start'],16),int(x['end'],16)) for x in parts]==[tuple(x) for x in row['pdata_parts']]
 result['native_bodies'].append({'token':row['token'],'name':row['method'],'start':hex(start),'end':hex(end),'sha256':sha(blob),'matches_original_pointer_and_slice':True,'boundary_scope':row['boundary_scope'],'pdata_parts':parts})
 (OUT/(row['token']+'-native55.asm')).write_text(row['disassembly'],encoding='utf-8')
maps=json.loads((ROOT/'audit/stage9-native3/evidence/native-method-map.json').read_text())
targets={0x1803265E0,0x18031D500,0x180394350,0x180326190,0x18037E320,0x18031D9F0,0x180320FE0,0x18030F500,0x1812E27C0,0x18031AEA0,0x18031B230,0x18131BAD0,0x1812E68F0,0x18132C500,0x180326700,0x18131F870,0x18131F900,0x18131E940,0x1803100F0}
result['original_call_mapping']=[m for m in maps if int(m['va'],16) in targets]
fields=json.loads((ROOT/'audit/stage9-native3/evidence/native-fields.json').read_text())
field_targets={'MouseManager':{'board','handItemType','plantSkillCrosshairs','skillOnHandOwnerPlant','propOnHand'},'Board':{'isFailed','gamePause','menuOpen','pause','onDetailPage','pauseTemp'},'Plant':{'attackRangeUIController'},'ResourceManager':{'boardClips'},'BoardManager':{'<Instance>k__BackingField'}}
result['original_fields']=[f for f in fields if f['type'] in field_targets and f['name'] in field_targets[f['type']]]
dis=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64)
result['original_native_helpers']=[]
for va in (0x180825710,0x1803100F0):
 r=next(x for x in rf if x[0]==va);blob=read(va,r[1]-va)
 result['original_native_helpers'].append({'start':hex(va),'end':hex(r[1]),'sha256':sha(blob),'disassembly':[f'{i.address:016x}: {i.bytes.hex():<32} {i.mnemonic} {i.op_str}' for i in dis.disasm(blob,va)]})

archive=ROOT/'audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip'
member='candidates/native82/candidate1.dll'
with zipfile.ZipFile(archive) as z:parent=z.read(member)
assert sha(parent)=='48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf'
clr=dnfile.dnPE(data=parent);t=clr.net.mdtables;mo_owner={};fo_owner={}
for td in t.TypeDef.rows:
 name=(str(td.TypeNamespace)+'.' if str(td.TypeNamespace) else '')+str(td.TypeName)
 for ix in td.MethodList:mo_owner[ix.row_index]=name
 for ix in td.FieldList:fo_owner[ix.row_index]=name
def tokenid(tok):
 table,rid=tok>>24,tok&0xffffff
 if table==6:
  r=t.MethodDef.rows[rid-1];return {'token':f'0x{tok:08X}','owner':mo_owner[rid],'name':str(r.Name),'signature_hex':r.Signature.value.hex()}
 if table==4:
  r=t.Field.rows[rid-1];return {'token':f'0x{tok:08X}','owner':fo_owner[rid],'name':str(r.Name),'signature_hex':r.Signature.value.hex()}
 if table==10:
  r=t.MemberRef.rows[rid-1];return {'token':f'0x{tok:08X}','name':str(r.Name),'signature_hex':r.Signature.value.hex()}
 return {'token':f'0x{tok:08X}'}
current={'archive_path':str(archive),'archive_sha256':sha(archive.read_bytes()),'member':member,'parent_sha256':sha(parent),'method_declarations':[],'fields':[],'item_type_constants':[]}
method_tokens={0x169,0x16e,0x1c8,0x1c9,0x1ca,0x1e1,0x2b8,0x2ba,0x2bd,0x2ce,0x401,0x5be,0x138,0x13d}
for rid,row in enumerate(t.MethodDef.rows,1):
 if rid not in method_tokens:continue
 item={**tokenid(0x06000000+rid),'public':row.Flags.mdPublic,'private':row.Flags.mdPrivate,'static':row.Flags.mdStatic,'rva':row.Rva}
 if rid==0x169:
  body=read_method_body_from_bytes(clr.get_data(row.Rva));item.update({'raw_body_hex':body.get_bytes().hex(),'body_sha256':sha(body.get_bytes()),'instructions':[]})
  for ins in body.instructions:
   op=ins.operand
   if isinstance(op,Token):op=tokenid(op.value)
   elif isinstance(op,int) and str(ins.opcode.name).startswith('br'):op={'target_cil_offset':op-body.header_size}
   elif op is not None:op=str(op)
   item['instructions'].append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode.name),'operand':op})
 if rid==0x2ce:
  body=read_method_body_from_bytes(clr.get_data(row.Rva));item['existing_user_string_tokens']=[]
  for ins in body.instructions:
   if str(ins.opcode.name)=='ldstr':
    tok=ins.operand.value;text=clr.net.user_strings.get(tok&0xffffff).value
    item['existing_user_string_tokens'].append({'cil_offset':ins.offset-body.header_size,'token':f'0x{tok:08X}','literal':text})
 current['method_declarations'].append(item)
for rid,row in enumerate(t.Field.rows,1):
 if fo_owner[rid] not in field_targets or str(row.Name) not in field_targets[fo_owner[rid]]:continue
 current['fields'].append({**tokenid(0x04000000+rid),'public':row.Flags.fdPublic,'private':row.Flags.fdPrivate,'static':row.Flags.fdStatic})
for constant in t.Constant.rows:
 if constant.Parent.table.name!='Field':continue
 rid=constant.Parent.row_index
 if fo_owner[rid]!='ItemType':continue
 row=t.Field.rows[rid-1];value=struct.unpack('<i',constant.Value.value)[0]
 current['item_type_constants'].append({'name':str(row.Name),'token':f'0x{0x04000000+rid:08X}','value':value,'bytes_le':constant.Value.value.hex()})
assert {x['name']:x['value'] for x in current['item_type_constants']}=={x['name']:x['value'] for x in result['original_item_type_constants']}
result['actual_current_parent_metadata']=current

resolver=ROOT/'audit/stage9-native3/resolver/mscorlib.dll';lib=dnfile.dnPE(str(resolver));lt=lib.net.mdtables
listdef=next(td for td in lt.TypeDef.rows if str(td.TypeNamespace)=='System.Collections.Generic' and str(td.TypeName)=='List`1')
idx=next(ix for ix in listdef.MethodList if str(ix.row.Name)=='get_Item');row=idx.row
body=read_method_body_from_bytes(lib.get_data(row.Rva))
result['resolver_list_get_item']={'path':str(resolver),'sha256':sha(resolver.read_bytes()),'token':f'0x{0x06000000+idx.row_index:08X}','public':row.Flags.mdPublic,'signature_hex':row.Signature.value.hex(),'raw_body_hex':body.get_bytes().hex(),'body_sha256':sha(body.get_bytes()),'instructions':[]}
for ins in body.instructions:
 op=ins.operand
 if isinstance(op,Token):
  tok=op.value;rid=tok&0xffffff
  if tok>>24==10:r=lt.MemberRef.rows[rid-1];op={'token':f'0x{tok:08X}','name':str(r.Name),'signature_hex':r.Signature.value.hex()}
  elif tok>>24==6:r=lt.MethodDef.rows[rid-1];op={'token':f'0x{tok:08X}','name':str(r.Name)}
  else:op={'token':f'0x{tok:08X}'}
 elif isinstance(op,int) and str(ins.opcode.name).startswith(('blt','br')):op={'target_cil_offset':op-body.header_size}
 elif op is not None:op=str(op)
 result['resolver_list_get_item']['instructions'].append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode.name),'operand':op})
inventory_path=ROOT/'audit/native83-codex-2026-10-13/metadata-current.json';inventory=json.loads(inventory_path.read_text());assert inventory['input_sha256']==sha(parent)
result['current_inventory']={'path':str(inventory_path),'sha256':sha(inventory_path.read_bytes()),'parent_sha256':inventory['input_sha256'],
 'targets':[x for x in inventory['targets'] if x['token'] in ('0x060001C8','0x060002CE')],
 'relevant_refs':[x for x in inventory['refs'] if x['token'] in ('0x0A000001','0x0A000004','0x0A000007','0x0A00000B','0x0A00000C','0x0A000010','0x0A000025','0x0A000037','0x0A000084','0x0A0000F2')]}
(OUT/'PRIMARY-EVIDENCE.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps({k:result[k] for k in ('scope','native_bodies','original_metadata_slots','mouse_jump_table','original_item_type_constants','actual_current_parent_metadata')},ensure_ascii=False,indent=2))
