from pathlib import Path
import sys,json,struct
R=Path(__file__).resolve().parent;N=R.parent/'stage9-native4';sys.path.insert(0,str(N/'python-deps'))
import pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
p=pefile.PE(str(N/'inputs/GameAssembly.dll'));base=p.OPTIONAL_HEADER.ImageBase;c=Cs(CS_ARCH_X86,CS_MODE_64)
maps=json.loads((N/'evidence/native-method-map.json').read_text())
for m in maps:
 if 0x180370300<=int(m['va'],16)<=0x180370700:print(m)
lines=[]
for e in p.DIRECTORY_ENTRY_EXCEPTION:
 f=e.struct
 if 0x370340<=f.BeginAddress<0x370600:
  head=p.get_data(f.UnwindData,4);version=head[0]&7;flags=head[0]>>3
  tail=p.get_data(f.UnwindData+4+((head[2]+1)//2)*4,12)
  lines.append(f'PData {base+f.BeginAddress:X}-{base+f.EndAddress:X} unwind={f.UnwindData:X} version={version} flags={flags} tail={tail.hex()}')
  for i in c.disasm(p.get_data(f.BeginAddress,f.EndAddress-f.BeginAddress),base+f.BeginAddress):lines.append(f'{i.address:016X} {i.mnemonic:10s} {i.op_str}'.rstrip())
(R/'evidence/LadderPlaceEnd.fragments.txt').write_text('\n'.join(lines)+'\n');print('\n'.join(lines))
