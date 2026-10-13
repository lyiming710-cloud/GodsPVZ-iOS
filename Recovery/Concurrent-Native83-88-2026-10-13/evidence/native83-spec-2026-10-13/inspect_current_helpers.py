"""Inspect current archived parent constructor CIL as source, never execute it."""
from pathlib import Path
import sys,json,zipfile,hashlib
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
archive=R/'audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip';member='candidates/native82/candidate1.dll'
with zipfile.ZipFile(archive)as z:data=z.read(member)
pe=dnfile.dnPE(data=data);T=pe.net.mdtables
owners={m.row_index:t for t in T.TypeDef.rows for m in t.MethodList}
fieldowners={f.row_index:t for t in T.TypeDef.rows for f in t.FieldList}
inventory=json.loads((R/'audit/native83-codex-2026-10-13/metadata-current.json').read_text())
refnames={int(r['token'],16):r['name']for r in inventory['refs']}
for t in inventory['internals']:
 for f in t['fields']:refnames[int(f['token'],16)]=f['name']
def name(tok):
 if tok in refnames:return refnames[tok]
 tab=tok>>24;i=tok&0xffffff
 if tab==6:
  t=owners[i];m=T.MethodDef.rows[i-1];return str(t.TypeName)+'::'+str(m.Name)+' / sig '+m.Signature.value.hex()
 if tab==4:
  t=fieldowners[i];f=T.Field.rows[i-1];return str(t.TypeName)+'::'+str(f.Name)+' / sig '+f.Signature.value.hex()
 if tab==10:return str(T.MemberRef.rows[i-1].Name)+' / sig '+T.MemberRef.rows[i-1].Signature.value.hex()
 if tab==0x70:return str(pe.net.user_strings.get(i).value)
 return hex(tok)
out={'archive':str(archive.relative_to(R)),'member':member,'parent_sha256':hashlib.sha256(data).hexdigest(),'inventory_sha256':inventory['input_sha256'],'scope':'read actual helper CIL only, no verifier or runtime test','helpers':[]}
for tok in [0x060000e0,0x060000f5,0x060000fd,0x06000117,0x060002a4]:
 m=T.MethodDef.rows[(tok&0xffffff)-1];b=CilMethodBody(CilMethodBodyReaderBytes(pe.get_data(m.Rva,4096)));h=b.header_size
 r={'token':hex(tok),'name':name(tok),'rva':hex(m.Rva),'code_size':b.code_size,'max_stack':b.max_stack,'local_sig':str(b.local_var_sig_tok),'eh_count':len(b.exception_handlers),'body_bytes':pe.get_data(m.Rva,h+b.code_size).hex(),'il':[]}
 for i in b.instructions:
  op=str(i.opcode);a=i.operand;v=None if a is None else str(a)
  if hasattr(a,'value'):v={'token':hex(a.value),'identity':name(a.value)}
  elif isinstance(a,int)and(op.startswith('br')or op.startswith('leave')or op.startswith('bge')or op.startswith('blt')):v=a-h
  r['il'].append({'offset':i.offset-h,'opcode':op,'operand':v})
 out['helpers'].append(r)
print(json.dumps(out,indent=2))
