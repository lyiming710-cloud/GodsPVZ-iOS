"""Usage: python extract_native.py GameAssembly.dll native-method-map.json [python-deps]."""
from pathlib import Path
import sys,json,hashlib
if len(sys.argv)>3:sys.path.insert(0,sys.argv[3])
import pefile,capstone
raw=Path(sys.argv[1]).read_bytes();digest=hashlib.sha256(raw).hexdigest()
assert digest=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
methods=json.loads(Path(sys.argv[2]).read_text());own=[m for m in methods if m['image']=='Assembly-CSharp.dll' and m.get('va')]
targets={0x418,0x41e,0x427,0x429,0x42f,0x430,0x432,0x433,0x434,0x436,0x437,0x342,0x343,0x3bf,0x3c0}
pe=pefile.PE(data=raw);ib=pe.OPTIONAL_HEADER.ImageBase;dis=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64);out=Path(__file__).parent/'native';out.mkdir(exist_ok=True)
for m in own:
 if int(m['token'],16)&0xffffff not in targets:continue
 start=int(m['va'],16);end=min(int(x['va'],16) for x in own if int(x['va'],16)>start)
 # Use the next pointer in the same managed image, not the next member of the
 # same class: the latter can run through unrelated intervening classes.
 lines=[json.dumps(m),'PC_SHA256='+digest,f'DISASSEMBLY_RANGE=0x{start:X}..0x{end:X} (next same-image method pointer; exclusive end)']
 for i in dis.disasm(pe.get_data(start-ib,end-start),start):lines.append(f'{i.address:016x} {i.mnemonic:9} {i.op_str}'.rstrip())
 name=(m['type']+'.' if m['type']!='Zombie' else '')+m['name']+'.native.txt'
 (out/name).write_bytes(('\n'.join(lines)+'\n').encode())
print('NATIVE_EXTRACTS',len(targets))
