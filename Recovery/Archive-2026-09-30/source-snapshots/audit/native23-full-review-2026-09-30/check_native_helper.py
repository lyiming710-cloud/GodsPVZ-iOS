from pathlib import Path
import sys,json,struct
base=Path(__file__).resolve().parents[1];sys.path.insert(0,str(base/'stage9-native4/python-deps'))
import pefile,capstone
data=(base/'stage9-native4/inputs/GameAssembly.dll').read_bytes();pe=pefile.PE(data=data);ib=pe.OPTIONAL_HEADER.ImageBase
d=pe.OPTIONAL_HEADER.DATA_DIRECTORY[3];off=pe.get_offset_from_rva(d.VirtualAddress);ranges=[struct.unpack_from('<III',data,i) for i in range(off,off+d.Size-11,12)]
methodmap=json.loads((base/'stage9-native4/evidence/native-method-map.json').read_text());cs=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64);cs.detail=True;rows=[]
for va in (0x18046dc80,0x18042e9f0,0x180250150):
 row={'start':hex(va),'range':None,'instructions':[]};entry=next((e for e in ranges if e[0]+ib==va),None)
 size=entry[1]-entry[0] if entry else 128;row['range']=[hex(va),hex(va+size)];o=pe.get_offset_from_rva(va-ib)
 for i in cs.disasm(data[o:o+size],va):
  rec={'address':hex(i.address),'op':i.mnemonic,'args':i.op_str}
  if i.mnemonic=='call' and i.operands[0].type==capstone.x86.X86_OP_IMM:rec['mapped']=[m for m in methodmap if int(m['va'],16)==i.operands[0].imm]
  row['instructions'].append(rec)
 rows.append(row)
(Path(__file__).parent/'native-helpers-independent.json').write_text(json.dumps(rows,indent=2))
print(json.dumps([{'start':r['start'],'calls':[i for i in r['instructions'] if i['op']=='call']} for r in rows],indent=2))
