from pathlib import Path
import json,hashlib,struct,sys
p=Path(sys.argv[1]);meta=Path(sys.argv[2]).read_bytes();fields=json.loads((p/'native-fields.json').read_text());data=(p/'GameAssembly.dll').read_bytes();assert hashlib.sha256(meta).hexdigest()=='ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9';assert hashlib.sha256(data).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d';assert hashlib.sha256((p/'native-fields.json').read_bytes()).hexdigest()=='8273b66712f7fd64732a4fed64c453c2e71934d2cab329937f9dadaf57eef110'
proof=[]
for owner,name,offset in [('Zombie','previousPosition',0x148),('Zombie','armor1Type',0x84),('Zombie','animationSprites',0x1B0),('Zombie','anim_Armor2Sprites',0x1B8),('Enemy','row',0x14),('Vector3','x',0x10),('Vector3','y',0x14),('Vector3','z',0x18)]:
 rows=[r for r in fields if r['type']==owner and r['name']==name];assert len(rows)==1 and rows[0]['offset']==offset;proof.append(rows[0])
nt=struct.unpack_from('<I',data,60)[0];base=struct.unpack_from('<Q',data,nt+48)[0];ns,opt=struct.unpack_from('<H',data,nt+6)[0],struct.unpack_from('<H',data,nt+20)[0]
def read(va,size):
 for i in range(ns):
  start=nt+24+opt+i*40;vsize,rva,rawlen,raw=struct.unpack_from('<IIII',data,start+8)
  if rva<=va-base and va-base+size<=rva+rawlen:return data[raw+va-base-rva:raw+va-base-rva+size]
 raise AssertionError('Unmapped original bytes')
constants=[]
for va,bits,value in [(0x1815A7B2C,'00000643',134.),(0x1815A7C70,'0000c842',100.),(0x1815A7D34,'00008f43',286.),(0x1815A7EA8,'00002a44',680.),(0x1815A7EB4,'00007544',980.),(0x1815A7EC0,'0000a0c1',-20.)]:
 raw=read(va,4);assert raw.hex()==bits and struct.unpack('<f',raw)[0]==value;constants.append({'va':hex(va),'bits':bits,'float':value})
table,tablebytes,text,textbytes=struct.unpack_from('<IIII',meta,8);literals=[]
for slot,name in [(0x181BA5E68,'anim_cone'),(0x181BA5E08,'anim_bucket'),(0x181BA5DA8,'anim_brick'),(0x181BCBEC8,'IceCube1'),(0x181BD4FC8,'LadderSaboteurs_helmet1')]:
 encoded=struct.unpack('<Q',read(slot,8))[0];assert (encoded>>29)&7==5 and encoded&1;index=(encoded&0x1FFFFFFF)>>1;assert index<tablebytes//8;length,offset=struct.unpack_from('<II',meta,table+8*index);assert offset+length<=textbytes;literal=meta[text+offset:text+offset+length].decode('utf8');assert literal==name;literals.append({'slot':hex(slot),'encoded':hex(encoded),'index':index,'literal':name})
targets=[hex(base+v)for v in struct.unpack('<6I',read(0x180360508,24))];assert targets==['0x18036048c','0x1803604a7','0x180360495','0x18036049e','0x1803604a7','0x1803604b0']
r={'GameAssembly_sha256':hashlib.sha256(data).hexdigest(),'global_metadata_sha256':hashlib.sha256(meta).hexdigest(),'fields':proof,'constants':constants,'original_strings':literals,'armor1_jump_table':{'va':'0x180360508','targets':targets,'enum_1_to_6_literals':['anim_cone','anim_bucket','anim_brick','IceCube1','anim_bucket','LadderSaboteurs_helmet1'],'default':'String.Empty'}};Path(sys.argv[3]).write_text(json.dumps(r,indent=2));print(json.dumps(r))
