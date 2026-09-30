from pathlib import Path
import sys, zipfile, hashlib, json
ROOT=Path(__file__).resolve().parent
sys.path.insert(0,str(ROOT/'python-deps'))
import pefile
from capstone import Cs, CS_ARCH_X86, CS_MODE_64
source=ROOT.parents[1]
out=ROOT/'evidence'; out.mkdir(exist_ok=True)
inputs=ROOT/'inputs'; inputs.mkdir(exist_ok=True)
expected={'GameAssembly.dll':'9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d','global-metadata.dat':'ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9'}
manifest={}
with zipfile.ZipFile(source/'GodsPVZ_1.0.2.zip') as z:
 for name,sha in expected.items():
  matches=[x for x in z.namelist() if x.endswith('/'+name) or x==name]; assert len(matches)==1,matches
  b=z.read(matches[0]); actual=hashlib.sha256(b).hexdigest(); assert actual==sha,(name,actual)
  (inputs/name).write_bytes(b); manifest[name]={'zip_member':matches[0],'sha256':actual,'size':len(b)}
pe=pefile.PE(str(inputs/'GameAssembly.dll')); base=pe.OPTIONAL_HEADER.ImageBase
md=Cs(CS_ARCH_X86,CS_MODE_64); md.detail=True
targets={'GetZombieUnderMouse':(480,0x18031ee10),'CreateEnemySelecter':(434,0x180316ab0),'PassLevel':(584,0x18033fde0),'ZombieSelect_ctor':(421,0x180325270)}
for name,(rid,va) in targets.items():
 ptr=int.from_bytes(pe.get_data(0x181b82d60-base+(rid-1)*8,8),'little'); assert ptr==va,(name,hex(ptr))
 f=next(e.struct for e in pe.DIRECTORY_ENTRY_EXCEPTION if e.struct.BeginAddress==va-base)
 code=pe.get_data(f.BeginAddress,f.EndAddress-f.BeginAddress)
 lines=[f'{name} RID={rid} VA={va:#x} END={base+f.EndAddress:#x} SIZE={len(code)} UNWIND={base+f.UnwindData:#x}']
 for ins in md.disasm(code,va):
  lines.append(f'{ins.address:016X}  {ins.bytes.hex():24s} {ins.mnemonic:10s} {ins.op_str}')
 (out/(name+'.native.txt')).write_text('\n'.join(lines)+'\n')
 manifest[name]={'rid':rid,'va':hex(va),'end':hex(base+f.EndAddress),'length':len(code),'unwind_rva':hex(f.UnwindData),'code_sha256':hashlib.sha256(code).hexdigest()}
(out/'native-input-manifest.json').write_text(json.dumps(manifest,indent=2))
print(json.dumps(manifest,indent=2))
