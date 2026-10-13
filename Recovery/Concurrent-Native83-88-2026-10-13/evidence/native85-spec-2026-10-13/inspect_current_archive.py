"""Read existing parent82 archive metadata only; does not extract/execute DLL."""
from pathlib import Path
import sys,zipfile,hashlib,json
sys.dont_write_bytecode=True
R=Path(__file__).resolve().parents[2];O=Path(__file__).resolve().parent
sys.path.insert(0,str(R/'audit/stage9-native4/python-deps'))
import dnfile
p=R/'audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip'
z=zipfile.ZipFile(p);matches=[]
result={'archive':str(p),'archive_sha256':hashlib.sha256(p.read_bytes()).hexdigest(),'matching_dlls':[]}
for n in z.namelist():
 if not n.lower().endswith('.dll'):continue
 b=z.read(n);s=hashlib.sha256(b).hexdigest()
 if s!='48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf':continue
 d=dnfile.dnPE(data=b);t=d.net.mdtables
 owners={i.row_index:str(x.TypeNamespace)+'.'+str(x.TypeName) for x in t.TypeDef.rows for i in x.MethodList}
 item={'member':n,'sha256':s,'method_specs':[],'methods':[],'fields':[],'initial_generic_parameters':[]}
 for rid,x in enumerate(t.MethodSpec.rows,1):
  method=x.Method.row;index=x.Method.row_index
  owner=owners.get(index,'') if x.Method.table.name=='MethodDef' else ''
  if str(method.Name)=='Initial' and owner=='.Projectile':
   sig=x.Instantiation.value
   name=None
   if len(sig)>=4 and sig[:3]==bytes.fromhex('0a0112'):
    k=sig[3];value=k if k<128 else ((k&63)<<8)|sig[4] if k<192 else ((k&31)<<24)|(sig[4]<<16)|(sig[5]<<8)|sig[6]
    if value&3==0:
     tr=t.TypeDef.rows[(value>>2)-1];name=str(tr.TypeNamespace)+'.'+str(tr.TypeName)
   item['method_specs'].append({'token':hex(0x2B000000+rid),'method_definition':hex(0x06000000+index),'signature':method.Signature.value.hex(),'instantiation':sig.hex(),'generic_argument_name':name})
 for rid,x in enumerate(t.MethodDef.rows,1):
  if owners[rid] in ['.Damage','.Element'] or (owners[rid]=='.Projectile' and str(x.Name)=='Initial'):item['methods'].append({'token':hex(0x6000000+rid),'owner':owners[rid],'name':str(x.Name),'signature':x.Signature.value.hex(),'flags':str(x.Flags)})
 for rid,x in enumerate(t.GenericParam.rows,1):
  if x.Owner.table.name!='MethodDef' or x.Owner.row_index!=0x3ED:continue
  constraints=[]
  for c in t.GenericParamConstraint.rows:
   if c.Owner.row_index!=rid:continue
   ct=c.Constraint.row;constraints.append({'table':c.Constraint.table.name,'rid':c.Constraint.row_index,'name':str(getattr(ct,'TypeNamespace',''))+'.'+str(getattr(ct,'TypeName',''))})
  item['initial_generic_parameters'].append({'token':hex(0x2A000000+rid),'name':str(x.Name),'number':x.Number,'flags':str(x.Flags),'constraints':constraints})
 for x in t.TypeDef.rows:
  owner=str(x.TypeNamespace)+'.'+str(x.TypeName)
  if owner not in ['.Damage','.Zombie','.Board']:continue
  for fi in x.FieldList:
   f=fi.row
   if str(f.Name) not in ['damagePoint','camp','sourceZombie','board','animationSprites','ID','fZ','projectileManager']:continue
   item['fields'].append({'token':hex(0x4000000+fi.row_index),'owner':owner,'name':str(f.Name),'signature':f.Signature.value.hex(),'flags':str(f.Flags)})
 result['matching_dlls'].append(item)
(O/'CURRENT-METADATA.json').write_text(json.dumps(result,indent=2),encoding='utf-8')
print(json.dumps(result,indent=2))
