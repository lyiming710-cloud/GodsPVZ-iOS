"""Read actual stored managed CIL as source data; never execute it."""
from pathlib import Path
import sys,zipfile,json,hashlib,struct
sys.dont_write_bytecode=True
R=Path(__file__).resolve().parents[2];O=Path(__file__).resolve().parent
sys.path.insert(0,str(R/'audit/stage9-native4/python-deps'))
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
from dncil.clr.token import Token
sha=lambda x:hashlib.sha256(x).hexdigest()
archive=R/'audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip'
z=zipfile.ZipFile(archive);raw=z.read('candidates/native82/candidate1.dll')
assert sha(raw)=='48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf'
result={'scope':'Actual parent82 DLL and resolver CIL source only; no execution, tests, builds or extraction of DLL.', 'parent82_sha256':sha(raw),'assemblies':[]}
for name,data in [('parent82',raw),('full_core',(R/'audit/stage9-native3/resolver/UnityEngine.CoreModule.dll').read_bytes()),('editor_core',(R/'audit/local-preflight/exact-editor-tools/resolver/UnityEngine.CoreModule.dll').read_bytes()),('full_mscorlib',(R/'audit/stage9-native3/resolver/mscorlib.dll').read_bytes())]:
 d=dnfile.dnPE(data=data);t=d.net.mdtables
 mo={i.row_index:str(x.TypeNamespace)+'.'+str(x.TypeName) for x in t.TypeDef.rows for i in x.MethodList};fo={i.row_index:str(x.TypeNamespace)+'.'+str(x.TypeName) for x in t.TypeDef.rows for i in x.FieldList}
 def identity(token):
  tb,ri=token>>24,token&0xffffff
  if tb==6:
   x=t.MethodDef.rows[ri-1];return {'token':hex(token),'owner':mo[ri],'name':str(x.Name),'signature':x.Signature.value.hex()}
  if tb==4:
   x=t.Field.rows[ri-1];return {'token':hex(token),'owner':fo[ri],'name':str(x.Name),'signature':x.Signature.value.hex()}
  if tb==10:
   x=t.MemberRef.rows[ri-1];c=x.Class.row;o=str(getattr(c,'TypeNamespace',''))+'.'+str(getattr(c,'TypeName',''));return {'token':hex(token),'owner':o,'name':str(x.Name),'signature':x.Signature.value.hex(),'class_table':x.Class.table.name,'class_rid':x.Class.row_index}
  if tb==1:
   x=t.TypeRef.rows[ri-1];return {'token':hex(token),'name':str(x.TypeNamespace)+'.'+str(x.TypeName)}
  if tb==2:
   x=t.TypeDef.rows[ri-1];return {'token':hex(token),'name':str(x.TypeNamespace)+'.'+str(x.TypeName)}
  if tb==27:return {'token':hex(token),'type_spec':t.TypeSpec.rows[ri-1].Signature.value.hex()}
  if tb==43:
   x=t.MethodSpec.rows[ri-1];mt=6 if x.Method.table.name=='MethodDef' else 10
   return {'token':hex(token),'method':identity((mt<<24)+x.Method.row_index),'instantiation':x.Instantiation.value.hex()}
  if tb==112:
   x=d.net.user_strings.get(ri);return {'token':hex(token),'text':str(x.value) if x else None}
  return {'token':hex(token)}
 item={'name':name,'sha256':sha(data),'methods':[],'fields':[],'enum_constants':[]}
 for ri,x in enumerate(t.MethodDef.rows,1):
  selected=(name=='parent82' and ((mo[ri]=='.ParticlesManager' and str(x.Name)=='get_instance') or (mo[ri]=='.GlobalStaticVars' and ri in [0x138,0x13E]))) or (name in ['full_core','editor_core'] and ((mo[ri]=='UnityEngine.Random' and str(x.Name) in ['Range','RandomRangeInt']) or (mo[ri]=='UnityEngine.Vector3' and str(x.Name)=='.ctor' and x.Signature.value.hex()=='2003010c0c0c'))) or (name=='full_mscorlib' and mo[ri]=='System.Collections.Generic.List`1' and str(x.Name)=='get_Item')
  if not selected:continue
  m=dict(identity(0x06000000+ri),flags=str(x.Flags),impl_flags=str(x.ImplFlags),rva=x.Rva)
  if x.Rva:
   body=read_method_body_from_bytes(d.get_data(x.Rva));m.update(body_bytes=body.get_bytes().hex(),body_sha256=sha(body.get_bytes()),code_size=body.code_size,max_stack=body.max_stack,instructions=[])
   for ins in body.instructions:
    op=ins.operand
    if isinstance(op,Token):op=identity(op.value)
    elif isinstance(op,int) and str(ins.opcode.name).startswith(('br','beq','bne','bge','bgt','ble','blt','leave')):op={'target_offset':op-body.header_size}
    elif op is not None:op=str(op)
    m['instructions'].append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode.name),'operand':op})
  item['methods'].append(m)
 if name=='parent82':
  for ri,x in enumerate(t.Field.rows,1):
   if (fo[ri]=='.ResourceManager' and str(x.Name) in ['dialogueClips','particleClips']) or (fo[ri]=='.ParticlesManager' and str(x.Name)=='<instance>k__BackingField'):
    item['fields'].append(dict(identity(0x04000000+ri),flags=str(x.Flags)))
  for c in t.Constant.rows:
   if c.Parent.table.name!='Field':continue
   ri=c.Parent.row_index
   if fo[ri] not in ['.DialogueAudioType','.ParticleState']:continue
   v=c.Value.value;item['enum_constants'].append({'owner':fo[ri],'field':str(c.Parent.row.Name),'token':hex(0x04000000+ri),'constant_type':c.Type,'bytes':v.hex(),'int32':struct.unpack('<i',v)[0] if len(v)==4 else None})
  item['relevant_type_specs']=[{'token':hex(0x1B000000+ri),'signature':x.Signature.value.hex()} for ri,x in enumerate(t.TypeSpec.rows,1)]
  item['audio_type_refs']=[{'token':hex(0x01000000+ri),'name':str(x.TypeNamespace)+'.'+str(x.TypeName)} for ri,x in enumerate(t.TypeRef.rows,1) if str(x.TypeName) in ['AudioClip','Vector3']]
  item['audio_get_item']=[dict(identity(0x0A000000+ri),declaring_type_spec=identity(0x1B000000+x.Class.row_index)) for ri,x in enumerate(t.MemberRef.rows,1) if str(x.Name)=='get_Item' and x.Class.table.name=='TypeSpec' and x.Signature.value.hex()=='2001130008']
 result['assemblies'].append(item)
inventory=R/'audit/native86-codex-2026-10-13/metadata-current.json';iv=json.loads(inventory.read_text());result['current_inventory']={'path':str(inventory),'sha256':sha(inventory.read_bytes()),'described_dll_sha256':iv['input_sha256'],'targets':[x for x in iv['targets'] if x['token'] in ['0x06000199','0x06000386']],'selected_refs':[x for x in iv['refs'] if x['name'] in ['System.Int32 UnityEngine.Random::Range(System.Int32,System.Int32)','System.Single UnityEngine.Random::Range(System.Single,System.Single)','System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)']]}
(O/'MANAGED-SOURCE-EVIDENCE.json').write_text(json.dumps(result,indent=2,ensure_ascii=False),encoding='utf-8');print(json.dumps(result,indent=2,ensure_ascii=True))
