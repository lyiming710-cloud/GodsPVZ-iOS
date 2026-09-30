from pathlib import Path
import sys,json,struct
R=Path(__file__).resolve().parent;sys.path.insert(0,str(R/'python-deps'))
import pefile
p=pefile.PE(str(R/'inputs/GameAssembly.dll'));base=p.OPTIONAL_HEADER.ImageBase
maps=json.loads((R/'evidence/native-method-map.json').read_text())
targets=[0x18131f760,0x1813044d0,0x18131bf10,0x18131f870,0x18033aad0,0x180395070]
lines=[]
for va in targets:
 hits=[m for m in maps if int(m['va'],16)==va];assert len(hits)==1
 lines.append(str(hits[0]))
for va in [0x1815a7a08,0x1815a7a10]:
 lines.append(f'float @{va:#x} = {struct.unpack("<f",p.get_data(va-base,4))[0]}')
for name in ['PassLevel','CreateEnemySelecter','GetZombieUnderMouse']:
 f=next(e.struct for e in p.DIRECTORY_ENTRY_EXCEPTION if e.struct.BeginAddress+base==int(json.loads((R/'evidence/native-input-manifest.json').read_text())[name]['va'],16))
 r=f.UnwindData;hdr=p.get_data(r,4);flags=hdr[0]>>3;n=hdr[2];tail=r+4+((n+1)&~1)*2
 # Preserve raw unwind/handler data; do not infer language-specific scope from unwind flags alone.
 lines.append(f'{name} UNWIND flags={flags} codes={n} tail_RVA={tail:#x} raw_tail={p.get_data(tail,80).hex()}')
(R/'evidence/call-constant-unwind-evidence.txt').write_text('\n'.join(lines)+'\n')
print('\n'.join(lines))
