"""Read and archive immutable native/support metadata evidence. Never execute target CIL."""
from pathlib import Path
import sys,json,hashlib,struct
sys.dont_write_bytecode=True
P=Path(__file__).resolve().parent;R=P.parents[1]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import pefile,dnfile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
pc=R/'audit/offline-test-handoff-2026-10-14/payload/inputs/original-pc/GameAssembly.dll'
pe=pefile.PE(str(pc));base=pe.OPTIONAL_HEADER.ImageBase;dec=Cs(CS_ARCH_X86,CS_MODE_64)
helpers=[]
for name,start,end in [('StringHash',0x1803CCF20,0x1803CCF75),('Clamp',0x18030E0E0,0x18030E181)]:
    raw=pe.get_data(start-base,end-start)
    helpers.append(dict(name=name,start=hex(start),end=hex(end),bytes=raw.hex(),sha256=hashlib.sha256(raw).hexdigest(),instructions=[dict(address=hex(i.address),bytes=i.bytes.hex(),mnemonic=i.mnemonic,operands=i.op_str) for i in dec.disasm(raw,start)]))
tables=[]
for name,addr,count in [('PlantAttackStartTry',0x180355D28,8)]:
    raw=pe.get_data(addr-base,count*4)
    tables.append(dict(name=name,address=hex(addr),bytes=raw.hex(),cases=[dict(case=i,target=hex(base+rva)) for i,rva in enumerate(struct.unpack('<'+'I'*count,raw))]))
support=R/'audit/offline-test-handoff-2026-10-14/payload/references/managed-support/Unity.TextMeshPro.dll'
sp=dnfile.dnPE(str(support));getter=next(m for t in sp.net.mdtables.TypeDef.rows if str(t.TypeName)=='TMP_Text' for mi in t.MethodList for m in [mi.row] if str(m.Name)=='get_textInfo')
body=CilMethodBody(CilMethodBodyReaderBytes(sp.get_data(getter.Rva,64)))
support_info=dict(path=support.relative_to(R).as_posix(),bytes=support.stat().st_size,sha256=hashlib.sha256(support.read_bytes()).hexdigest(),name='TMP_Text::get_textInfo',flags=str(getter.Flags),rva=hex(getter.Rva),signature=getter.Signature.value.hex(),instructions=[dict(opcode=str(i.opcode),operand=str(i.operand)) for i in body.instructions])
growth=pefile.PE(str(R/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).get_data(0xB0E78,20)
out=dict(scope='READ_ONLY_ORIGINAL_NATIVE_AND_SUPPORT_METADATA_NOT_TESTED',native_sha256=hashlib.sha256(pc.read_bytes()).hexdigest(),helpers=helpers,tables=tables,support_getter=support_info,growth_array=dict(field_token='0x04000AEF',rva='0xB0E78',bytes=growth.hex(),values=list(struct.unpack('<5i',growth))))
(P/'SUPPLEMENTAL-ORIGINAL-EVIDENCE.json').write_text(json.dumps(out,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print(json.dumps(dict(tables=tables,support=support_info,growth=out['growth_array']),ensure_ascii=True))
