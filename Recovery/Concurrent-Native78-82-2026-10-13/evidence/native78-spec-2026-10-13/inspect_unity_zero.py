"""Inspect real Unity reference getter CIL as source data; no execution of CIL."""
from pathlib import Path
import sys,json,hashlib
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
results=[]
for rel in ['audit/stage9-native3/resolver/UnityEngine.CoreModule.dll','audit/local-preflight/exact-editor-tools/resolver/UnityEngine.CoreModule.dll']:
 p=R/rel;data=p.read_bytes();d=dnfile.dnPE(str(p))
 result={'path':rel,'bytes':len(data),'sha256':hashlib.sha256(data).hexdigest(),'mvid':str(d.net.mdtables.Module.rows[0].Mvid),'assembly_name':str(d.net.mdtables.Assembly.rows[0].Name)}
 for ti,t in enumerate(d.net.mdtables.TypeDef.rows,1):
  if str(t.TypeName)!='Vector3' or str(t.TypeNamespace)!='UnityEngine':continue
  result['type']={'token':hex(0x02000000+ti),'public':t.Flags.tdPublic}
  for mid in t.MethodList:
   m=mid.row
   if str(m.Name)!='get_zero':continue
   body=CilMethodBody(CilMethodBodyReaderBytes(d.get_data(m.Rva,128)))
   h=body.header_size
   result['getter']={'token':hex(0x06000000+mid.row_index),'rva':hex(m.Rva),'public':m.Flags.mdPublic,'static':m.Flags.mdStatic,'signature_hex':m.Signature.value.hex(),'header_size':h,'code_size':body.code_size,'max_stack':body.max_stack,'local_sig':str(body.local_var_sig_tok),'local_sig_hex':d.net.mdtables.StandAloneSig.rows[(body.local_var_sig_tok.value&0xffffff)-1].Signature.value.hex(),'body_bytes':d.get_data(m.Rva,h+body.code_size).hex(),'il':[{'offset':i.offset-h,'opcode':str(i.opcode),'operand':str(i.operand-h if isinstance(i.operand,int) and str(i.opcode).startswith('br') else i.operand)} for i in body.instructions],'eh_count':len(body.exception_handlers)}
  for fi in t.FieldList:
   f=fi.row
   if str(f.Name)=='zeroVector':result['field']={'token':hex(0x04000000+fi.row_index),'private':f.Flags.fdPrivate,'static':f.Flags.fdStatic,'readonly':f.Flags.fdInitOnly,'signature_hex':f.Signature.value.hex()}
 results.append(result)
print(json.dumps(results,indent=2))
