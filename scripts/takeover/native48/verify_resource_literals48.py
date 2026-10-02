"""Verify original PC resource literal slots using PE sections and v31 literal metadata."""
from pathlib import Path
import hashlib,struct,json,sys
native=Path(sys.argv[1]).read_bytes();meta=Path(sys.argv[2]).read_bytes()
assert hashlib.sha256(native).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
assert hashlib.sha256(meta).hexdigest()=='ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9'
nt=struct.unpack_from('<I',native,60)[0];base=struct.unpack_from('<Q',native,nt+48)[0];count=struct.unpack_from('<H',native,nt+6)[0];opt=struct.unpack_from('<H',native,nt+20)[0]
magic,version,table,tablebytes,data,databytes=struct.unpack_from('<IIIIII',meta,0);assert magic==0xfab11baf and version==31
assert table+tablebytes<=len(meta) and data+databytes<=len(meta)
rows=[]
for slot,expected in [(6471534032, 'jsons'), (6471538544, 'DeviceData'), (6471538720, 'DeviceInfo'), (6471600568, 'ZombieData'), (6471600736, 'ZombieInfo'), (6471426504, '.json'), (6471441864, '缺失装置信息文件'), (6471429000, '已生成装置信息文件模板'), (6471441768, '缺失僵尸信息文件'), (6471428904, '已生成缺失的僵尸信息文件模板')]:
 rva=slot-base;at=None
 for i in range(count):
  sh=nt+24+opt+40*i;va,size,raw=struct.unpack_from('<III',native,sh+12)
  if va<=rva and rva+8<=va+size:at=raw+rva-va;break
 assert at is not None and at+8<=len(native)
 encoded=struct.unpack_from('<Q',native,at)[0];usage=(encoded>>29)&7;index=(encoded&0x1fffffff)>>1
 assert usage==5 and encoded&1 and index<tablebytes//8
 length,offset=struct.unpack_from('<II',meta,table+index*8);assert offset+length<=databytes
 literal=meta[data+offset:data+offset+length].decode('utf8');assert literal==expected
 rows.append(dict(slot=hex(slot),encoded=hex(encoded),index=index,literal=literal))
result={'GameAssembly_sha256':hashlib.sha256(native).hexdigest(),'global_metadata_sha256':hashlib.sha256(meta).hexdigest(),'version':version,'literals':rows}
if len(sys.argv)>3:Path(sys.argv[3]).write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
print(json.dumps(result,ensure_ascii=False))
