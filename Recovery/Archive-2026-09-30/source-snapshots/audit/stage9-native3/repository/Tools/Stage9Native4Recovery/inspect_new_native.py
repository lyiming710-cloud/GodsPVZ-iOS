from pathlib import Path
import sys,json,struct
R=Path(__file__).resolve().parent;sys.path.insert(0,str(R/'python-deps'))
import pefile
pe=pefile.PE(str(R/'inputs/GameAssembly.dll'));base=pe.OPTIONAL_HEADER.ImageBase
def data(va,n):return pe.get_data(va-base,n)
lines=[]
for va in [0x18035838d+0x1872a63,0x1803583c8+0x186a710]:
 q=struct.unpack('<Q',data(va,8))[0]
 lines.append(f'string slot {va:#x} raw {q:#x} usage={q>>29} index={q&0x1fffffff}')
md=(R/'inputs/global-metadata.dat').read_bytes()
u=lambda off:struct.unpack_from('<I',md,off)[0]
lines.append('metadata header '+repr([u(i) for i in range(0,40,4)]))
for va in [0x181bcadf0,0x181bc2ad8]:
 encoded=struct.unpack('<Q',data(va,8))[0];index=(encoded&0x1fffffff)>>1
 assert encoded>>29==5 and encoded&1 and index<u(12)//8
 length,offset=struct.unpack_from('<II',md,u(8)+index*8)
 literal=md[u(16)+offset:u(16)+offset+length].decode('utf-8')
 lines.append(f'literal {va:#x} encoded={encoded:#x} index={index} value={literal!r}')
for va in [0x180340a17+0x12670ed,0x180340a42+0x12671ae,0x180340a4c+0x1267198,0x180340a56+0x1267196]:
 lines.append(f'constant {va:#x} = {struct.unpack("<f",data(va,4))[0]}')
fields=json.loads((R/'evidence/native-fields.json').read_text())
for t,offsets in [('Plant',[0x70,0x178,0x168,0xc0,0x7c,0xb5,0x90]),('BoardConfig',[0x30,0x68]),('Map',[0x18]),('SunManager',[0x2c]),('BoardEntry',[0x10,0x18,0x19])]:
 for f in fields:
  if f['type']==t and f['offset'] in offsets:lines.append('field '+json.dumps(f))
methods=json.loads((R/'evidence/native-method-map.json').read_text())
for va in ['0x180394350','0x18131bad0','0x1812dbab0','0x1812dbc00']:
 for m in methods:
  if m['va']==va:lines.append('call '+json.dumps(m))
(R/'evidence/new-native-identities.txt').write_text('\n'.join(lines)+'\n');print('\n'.join(lines))
