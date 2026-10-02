from pathlib import Path
import json,hashlib,struct,sys
p=Path(sys.argv[1]);fields=json.loads((p/'native-fields.json').read_text());assert hashlib.sha256((p/'native-fields.json').read_bytes()).hexdigest()=='8273b66712f7fd64732a4fed64c453c2e71934d2cab329937f9dadaf57eef110';assert hashlib.sha256((p/'GameAssembly.dll').read_bytes()).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
proof=[]
for owner,name,offset in [('EnemySelecter','board',0x20),('Board','boardConfig',0x28),('DeviceManager','board',0x20),('Grid','device_top',0x68),('Grid','device_sheath',0x60),('Grid','device_common',0x50),('Grid','device_bottom',0x48),('Frame','Labels',0x10),('SwfClipController','_clip',0x20),('SwfClip','_sequence',0x90),('PurpleFlowerLogic','_idleSequences',0),('Vector3','x',0x10),('Vector3','y',0x14),('Vector3','z',0x18)]:
 rows=[r for r in fields if r['type']==owner and r['name']==name];assert len(rows)==1 and rows[0]['offset']==offset;proof.append(rows[0])
data=(p/'GameAssembly.dll').read_bytes();nt=struct.unpack_from('<I',data,60)[0];base=struct.unpack_from('<Q',data,nt+48)[0];ns,opt=struct.unpack_from('<H',data,nt+6)[0],struct.unpack_from('<H',data,nt+20)[0]
def read(va):
 for i in range(ns):
  start=nt+24+opt+i*40;size,rva,rawlen,raw=struct.unpack_from('<IIII',data,start+8)
  if rva<=va-base<rva+rawlen:return data[raw+va-base-rva:raw+va-base-rva+4]
 raise AssertionError('Unmapped constant')
constants=[]
for va,bits,value in [(0x1815A7B0C,'0000e041',28.0),(0x1815A7B18,'00006442',57.0)]:
 raw=read(va);assert raw.hex()==bits and struct.unpack('<f',raw)[0]==value;constants.append({'va':hex(va),'bits':bits,'float':value})
r={'field_map_sha256':hashlib.sha256((p/'native-fields.json').read_bytes()).hexdigest(),'fields':proof,'constants':constants,'vector_field_offsets':'Original boxed IL2CPP offsets include the 16-byte object header; native return buffer contains x/y/z at 0/4/8.'};Path(sys.argv[2]).write_text(json.dumps(r,indent=2));print(json.dumps(r))
