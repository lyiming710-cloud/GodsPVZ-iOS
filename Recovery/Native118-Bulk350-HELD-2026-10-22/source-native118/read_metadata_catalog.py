"""Read immutable metadata for source binding; no build or target invocation."""
from pathlib import Path
import json,sys
sys.dont_write_bytecode=True
ROOT=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile
md=dnfile.dnPE(str(ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).net.mdtables
d={'types':[{'token':f'0x{(tab<<24)|i:08X}','name':str(t.TypeNamespace)+'.'+str(t.TypeName)} for tab,table in [(1,md.TypeRef),(2,md.TypeDef)] for i,t in enumerate(table.rows,1)],
 'typespecs':[{'token':f'0x{0x1B000000|i:08X}','blob':t.Signature.value.hex()} for i,t in enumerate(md.TypeSpec.rows,1)],
 'methodspecs':[{'token':f'0x{0x2B000000|i:08X}','method':f'0x{((6 if t.Method.table.name=="MethodDef" else 10)<<24)|t.Method.row_index:08X}','blob':t.Instantiation.value.hex()} for i,t in enumerate(md.MethodSpec.rows,1)]}
(Path(__file__).resolve().parent/'METADATA-CATALOG.json').write_text(json.dumps(d,indent=2)+'\n',encoding='utf-8')
print(json.dumps(d['typespecs']))
