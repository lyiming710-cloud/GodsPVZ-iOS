"""Read complete reference helper CIL bodies, without loading or executing CIL."""
from pathlib import Path
import sys,json,hashlib
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
results=[]
for rel in ['audit/stage9-native3/resolver/UnityEngine.CoreModule.dll','audit/stage9-native3/resolver/mscorlib.dll']:
 path=R/rel;pe=dnfile.dnPE(str(path));tables=pe.net.mdtables
 owners={mi.row_index:ti for ti,t in enumerate(tables.TypeDef.rows,1)for mi in t.MethodList}
 fields={fi.row_index:ti for ti,t in enumerate(tables.TypeDef.rows,1)for fi in t.FieldList}
 nested={r.NestedClass.row_index:r.EnclosingClass.row_index for r in tables.NestedClass.rows}
 def typename(i):
  t=tables.TypeDef.rows[i-1];prefix=typename(nested[i])+'/'if i in nested else(str(t.TypeNamespace)+'.'if str(t.TypeNamespace)else'')
  return prefix+str(t.TypeName)
 def resolve(tok):
  t=tok>>24;i=(tok&0xffffff)
  if t==6:return typename(owners[i])+'::'+str(tables.MethodDef.rows[i-1].Name)
  if t==4:return typename(fields[i])+'::'+str(tables.Field.rows[i-1].Name)
  if t==10:
   r=tables.MemberRef.rows[i-1];return {'name':str(r.Name),'class_table':r.Class.table.name,'class_row':r.Class.row_index,'signature':r.Signature.value.hex()}
  return hex(tok)
 r={'path':rel,'bytes':path.stat().st_size,'sha256':hashlib.sha256(path.read_bytes()).hexdigest(),'mvid':str(tables.Module.rows[0].Mvid),'helpers':[]}
 wanted={'UnityEngine.Renderer':['get_material','GetMaterial','IsPersistent'],'UnityEngine.GameObject':['GetComponent'],'System.Collections.Generic.List`1':['GetEnumerator','get_Item'],'System.Collections.Generic.List`1/Enumerator':['.ctor','Dispose','get_Current','MoveNext','MoveNextRare']}
 for ti,t in enumerate(tables.TypeDef.rows,1):
  owner=typename(ti)
  if owner not in wanted:continue
  for mi in t.MethodList:
   m=mi.row;name=str(m.Name)
   if name not in wanted[owner]:continue
   if owner=='UnityEngine.GameObject'and name=='GetComponent'and not(m.Signature.value[0]&16):continue
   out={'owner':owner,'token':hex(0x06000000+mi.row_index),'name':name,'rva':hex(m.Rva),'signature':m.Signature.value.hex(),'public':m.Flags.mdPublic,'private':m.Flags.mdPrivate,'static':m.Flags.mdStatic,'impl_flags':str(m.ImplFlags)}
   if m.Rva:
    body=CilMethodBody(CilMethodBodyReaderBytes(pe.get_data(m.Rva,4096)));h=body.header_size
    out.update({'header_size':h,'code_size':body.code_size,'max_stack':body.max_stack,'local_sig':str(body.local_var_sig_tok),'body_bytes':pe.get_data(m.Rva,h+body.code_size).hex(),'eh_count':len(body.exception_handlers),'il':[]})
    for i in body.instructions:
     op=str(i.opcode);operand=i.operand;v=None if operand is None else str(operand)
     if hasattr(operand,'value'):v={'token':hex(operand.value),'resolved':resolve(operand.value)}
     elif isinstance(operand,int)and(op.startswith('br')or op.startswith('bge')or op.startswith('blt')or op.startswith('bne')or op.startswith('beq')or op.startswith('leave')):v=operand-h
     out['il'].append({'offset':i.offset-h,'opcode':op,'operand':v})
   r['helpers'].append(out)
 results.append(r)
print(json.dumps(results,indent=2))
