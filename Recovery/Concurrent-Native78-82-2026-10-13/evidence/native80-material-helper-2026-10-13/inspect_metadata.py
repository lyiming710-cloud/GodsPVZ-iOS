"""Read exact renderer and NativeConditional metadata without executing application CIL."""
from pathlib import Path
import sys,json,hashlib
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
results=[]
for rel in ['audit/stage9-native3/resolver/UnityEngine.CoreModule.dll','audit/stage9-native4/resolver/UnityEngine.CoreModule.dll','audit/local-preflight/exact-editor-tools/resolver/UnityEngine.CoreModule.dll','audit/local-preflight/exact-editor-tools/resolver/UnityEngine.SharedInternalsModule.dll']:
 path=R/rel;pe=dnfile.dnPE(str(path));T=pe.net.mdtables
 owners={m.row_index:i for i,t in enumerate(T.TypeDef.rows,1)for m in t.MethodList}
 def tn(i):
  t=T.TypeDef.rows[i-1];return str(t.TypeNamespace)+'.'+str(t.TypeName)
 def attr_ctor(x):
  if x.table.name=='MemberRef':
   r=x.row;c=r.Class.row;return {'owner':str(getattr(c,'TypeNamespace',''))+'.'+str(getattr(c,'TypeName','')),'name':str(r.Name),'signature':r.Signature.value.hex()}
  if x.table.name=='MethodDef':return {'owner':tn(owners[x.row_index]),'name':str(x.row.Name),'signature':x.row.Signature.value.hex()}
  return {'table':x.table.name,'row':x.row_index}
 def attrs(table,index):return [{'constructor':attr_ctor(a.Type),'blob':a.Value.value.hex()}for a in T.CustomAttribute.rows if a.Parent.table.name==table and a.Parent.row_index==index]
 def body(m):
  if not m.Rva:return {'rva':0,'internal_call':m.ImplFlags.miInternalCall,'body':None}
  b=CilMethodBody(CilMethodBodyReaderBytes(pe.get_data(m.Rva,2048)));h=b.header_size
  return {'rva':hex(m.Rva),'code_size':b.code_size,'body_bytes':pe.get_data(m.Rva,h+b.code_size).hex(),'il':[{'offset':i.offset-h,'opcode':str(i.opcode),'operand':str(i.operand)}for i in b.instructions]}
 out={'path':rel,'bytes':path.stat().st_size,'sha256':hashlib.sha256(path.read_bytes()).hexdigest(),'mvid':str(T.Module.rows[0].Mvid),'types':[]}
 for ti,t in enumerate(T.TypeDef.rows,1):
  fullname=tn(ti)
  if fullname not in ['UnityEngine.Renderer','UnityEngine.Bindings.NativeConditionalAttribute']:continue
  tr={'name':fullname,'token':hex(0x02000000+ti),'attributes':attrs('TypeDef',ti),'methods':[],'properties':[]}
  for mi in t.MethodList:
   m=mi.row;n=str(m.Name)
   if fullname=='UnityEngine.Renderer'and n not in ['IsPersistent','get_material','GetMaterial']:continue
   tr['methods'].append({'name':n,'token':hex(0x06000000+mi.row_index),'flags':str(m.Flags),'impl_flags':str(m.ImplFlags),'signature':m.Signature.value.hex(),'custom_attributes':attrs('MethodDef',mi.row_index),**body(m)})
  for pm in T.PropertyMap.rows:
   if pm.Parent.row_index!=ti:continue
   for pi in pm.PropertyList:
    p=pi.row
    if fullname=='UnityEngine.Renderer'and str(p.Name)!='material':continue
    tr['properties'].append({'name':str(p.Name),'token':hex(0x17000000+pi.row_index),'attributes':attrs('Property',pi.row_index)})
  out['types'].append(tr)
 refids={i for i,r in enumerate(T.MemberRef.rows,1)if str(getattr(r.Class.row,'TypeName',''))=='NativeConditionalAttribute'}
 out['native_conditional_attribute_count']=sum(a.Type.table.name=='MemberRef'and a.Type.row_index in refids for a in T.CustomAttribute.rows)
 results.append(out)
print(json.dumps(results,indent=2))
