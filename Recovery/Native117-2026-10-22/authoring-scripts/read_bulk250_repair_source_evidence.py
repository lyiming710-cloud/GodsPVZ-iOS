"""Read immutable source bytes and metadata for authoring; no target/test execution."""
from pathlib import Path
import sys, json, hashlib
sys.dont_write_bytecode = True
R = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(R/'audit/stage9-native3/python-deps'))
import dnfile, pefile
from capstone import Cs, CS_ARCH_X86, CS_MODE_64
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
OUT=R/'audit/native116-bulk250-r1-codex-2026-10-22'
OUT.mkdir(exist_ok=True)
P=R/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'
G=R/'audit/offline-test-handoff-2026-10-14/payload/inputs/original-pc/GameAssembly.dll'
m=dnfile.dnPE(str(P)); t=m.net.mdtables; pe=pefile.PE(str(G)); base=pe.OPTIONAL_HEADER.ImageBase
maps=json.loads((R/'audit/offline-test-handoff-2026-10-14/payload/inputs/maps/native-method-map.json').read_text('utf-8-sig'))
dec=Cs(CS_ARCH_X86,CS_MODE_64)
def native(start,end):
    b=pe.get_data(start-base,end-start)
    return dict(start=hex(start),end=hex(end),bytes=b.hex(),sha256=hashlib.sha256(b).hexdigest(),instructions=[dict(address=hex(i.address),bytes=i.bytes.hex(),mnemonic=i.mnemonic,operand=i.op_str) for i in dec.disasm(b,start)])
proof=[]
for rid,owner in [(0x1E5,'ParticlesManager'),(0x158,'BoardConfig')]:
    z=t.MethodDef.rows[rid-1]; body=CilMethodBody(CilMethodBodyReaderBytes(m.get_data(z.Rva,128)))
    start=int(next(x['va'] for x in maps if int(x['token'],16)==0x06000000+rid and x['type']==owner),16)
    extent=next((e for e in pe.DIRECTORY_ENTRY_EXCEPTION if base+e.struct.BeginAddress==start),None)
    end=base+extent.struct.EndAddress if extent else min(int(x['va'],16) for x in maps if int(x['va'],16)>start)
    proof.append(dict(token=f'0x{0x06000000+rid:08X}',owner=owner,name=str(z.Name),signature=z.Signature.value.hex(),public=bool(z.Flags.mdPublic),cil_header_size=body.header_size,cil_operand_offsets_include_header=True,cil=[dict(offset=i.offset-body.header_size,opcode=str(i.opcode),operand=str(i.operand)) for i in body.instructions],native=native(start,end),extent_source='pdata' if extent else 'next distinct method pointer'))
refs=[]
for rid in [0x233,0x23A,0x27F,0x304,0x306,0x307]:
    z=t.MemberRef.rows[rid-1]; owner=z.Class.row
    refs.append(dict(token=f'0x{0x0A000000+rid:08X}',owner_table=z.Class.table.name,owner_rid=z.Class.row_index,owner_name=str(getattr(owner,'TypeName','')),owner_signature=getattr(getattr(owner,'Signature',None),'value',b'').hex(),name=str(z.Name),signature=z.Signature.value.hex()))
orig=json.loads((R/'audit/native116-codex-2026-10-22/ORIGINAL-SOURCE.json').read_text('utf-8'))
cell=next(x for x in orig['cells'] if int(x['address'],16)==0x181BB0258)
string=dict(token='0x7000AE84',text=m.net.user_strings.get(0xAE84).value,original_cell=cell)
data=dict(scope='IMMUTABLE_SOURCE_READ_ONLY_NO_PROJECT_TESTS_OR_CANDIDATES',parent_sha256=hashlib.sha256(P.read_bytes()).hexdigest(),gameassembly_sha256=hashlib.sha256(G.read_bytes()).hexdigest(),getter_evidence=proof,member_references=refs,device_canbear_argument=string,distance_helper=native(0x1802FB1B0,0x1802FB272),runtime_methods_not_executed=True)
(OUT/'SOURCE-API-EVIDENCE.json').write_text(json.dumps(data,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
for p in proof:
    print(p['token'],p['owner'],p['name'],p['signature'],'public',p['public'])
    for i in p['cil']:print(i)
    for i in p['native']['instructions']:print(i['address'],i['mnemonic'],i['operand'])
print('Device.CanBear string',string)
