"""Archive source-only metadata, getter CIL and original helper bytes; executes no target."""
from pathlib import Path
import sys,json,hashlib
sys.dont_write_bytecode=True
P=Path(__file__).resolve().parent;R=P.parents[1]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile,pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
parent=R/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'
pc=R/'audit/offline-test-handoff-2026-10-14/payload/inputs/original-pc/GameAssembly.dll'
md=dnfile.dnPE(str(parent));native=pefile.PE(str(pc));base=native.OPTIONAL_HEADER.ImageBase
dec=Cs(CS_ARCH_X86,CS_MODE_64);rows=md.net.mdtables.MethodDef.rows
owners={m.row_index:t for t in md.net.mdtables.TypeDef.rows for m in t.MethodList}
getters=[]
for token in [0x06000636,0x06000158,0x0600086A,0x06000830]:
    rid=token&0xFFFFFF;m=rows[rid-1]
    body=CilMethodBody(CilMethodBodyReaderBytes(md.get_data(m.Rva,512)))
    getters.append(dict(token=f'0x{token:08X}',owner=str(owners[rid].TypeName),name=str(m.Name),signature=m.Signature.value.hex(),public=bool(m.Flags.mdPublic),rva=hex(m.Rva),cil=body.raw_bytes[body.header_size:body.header_size+body.code_size].hex(),instructions=[dict(op=str(i.opcode),operand=str(i.operand)) for i in body.instructions]))
helpers=[]
for label,start,size in [('AnimatorStateInfoFullPathHash',0x1808E3A80,6),('MainUIInstance',0x1802FB0C0,48)]:
    data=native.get_data(start-base,size)
    helpers.append(dict(name=label,start=hex(start),bytes=data.hex(),sha256=hashlib.sha256(data).hexdigest(),instructions=[dict(address=hex(i.address),mnemonic=i.mnemonic,operands=i.op_str) for i in dec.disasm(data,start)]))
generics=[]
for token in [0x060003EF,0x060003ED,0x060000C4,0x060000C6]:
    rid=token&0xFFFFFF;m=rows[rid-1]
    generics.append(dict(token=f'0x{token:08X}',owner=str(owners[rid].TypeName),name=str(m.Name),signature=m.Signature.value.hex(),evidence_state='METADATA_DEFINITION_ONLY_NATIVE_CALLSITE_INSTANTIATION_NOT_PROVEN'))
out=dict(scope='SOURCE_METADATA_AND_ORIGINAL_BYTES_ONLY_NO_PROJECT_TESTS',parent_sha256=hashlib.sha256(parent.read_bytes()).hexdigest(),original_pc_sha256=hashlib.sha256(pc.read_bytes()).hexdigest(),getters=getters,helpers=helpers,generic_definitions=generics,main_ui_class_cell='0x181BAC570',main_ui_cell_note='Compare original Window_I.CreatNewSave callsite and helper; no private backing-field access in new source',animator_public_api_source='https://raw.githubusercontent.com/Unity-Technologies/UnityCsReference/2022.3/Modules/Animation/ScriptBindings/Animator.bindings.cs',animator_note='get_fullPathHash returns m_FullPath. Native helper reads value-type +8. Dummy support DLL omits methods and is not runtime API acceptance.')
(P/'SOURCE-API-EVIDENCE.json').write_text(json.dumps(out,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print(json.dumps(dict(scope=out['scope'],getters=len(getters),generic_definitions=len(generics))))
