from pathlib import Path
import struct,json,hashlib,sys
pc=Path(sys.argv[1]);raw=(pc/'GameAssembly.dll').read_bytes();fields=json.loads((pc/'native-fields.json').read_text());assert hashlib.sha256(raw).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
assert hashlib.sha256((pc/'native-fields.json').read_bytes()).hexdigest()=='8273b66712f7fd64732a4fed64c453c2e71934d2cab329937f9dadaf57eef110'
nt=struct.unpack_from('<I',raw,60)[0];base=struct.unpack_from('<Q',raw,nt+48)[0];count=struct.unpack_from('<H',raw,nt+6)[0];opt=struct.unpack_from('<H',raw,nt+20)[0];constants=[]
for va,bits in [(0x1815a7b14,0x42480000),(0x1815a7f80,0x430c0000),(0x1815a7fa4,0xc3520000)]:
 rva=va-base;offset=None
 for i in range(count):
  sh=nt+24+opt+40*i;v,size,start=struct.unpack_from('<III',raw,sh+12)
  if v<=rva and rva+4<=v+size:offset=start+rva-v;break
 assert offset is not None and offset+4<=len(raw);value=raw[offset:offset+4];assert struct.unpack('<I',value)[0]==bits
 constants.append({'va':hex(va),'bits':value.hex(),'uint32':bits,'float':struct.unpack('<f',value)[0]})
proof=[f for f in fields if f['type']=='Prop'and f['name']in ['mousePosition','propType','propImagex']or f['type']=='Card_Choose'and f['name']=='page'];assert len(proof)==4
for owner,name,token,offset in [('Prop','propType',0x0400057A,0x2c),('Prop','mousePosition',0x04000586,0x70),('Prop','propImagex',0x04000589,0x90),('Card_Choose','page',0x04000447,0x100)]:
 f=next(f for f in proof if f['type']==owner and f['name']==name);assert int(f['token'],16)==token and f['offset']==offset
r={'GameAssembly_sha256':hashlib.sha256(raw).hexdigest(),'constants':constants,'fields':proof};Path(sys.argv[2]).write_text(json.dumps(r,indent=2));print(json.dumps(r))
