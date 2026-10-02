from pathlib import Path
import json,hashlib,sys
p=Path(sys.argv[1]);fields=json.loads((p/'native-fields.json').read_text());assert hashlib.sha256((p/'native-fields.json').read_bytes()).hexdigest()=='8273b66712f7fd64732a4fed64c453c2e71934d2cab329937f9dadaf57eef110'
assert hashlib.sha256((p/'GameAssembly.dll').read_bytes()).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
expected=[('DeviceManager','deviceList',0x040001D6,0x30),('PlantManager','plants',0x040002B1,0x30),('ZombieManager','zombieList',0x04000362,0x38),('Board','plantManager',0x040003D4,0xE8),('Board','zombieManager',0x040003D5,0xF0),('Board','deviceManager',0x040003D8,0x108),('LawnApp','savesManager',0x04000166,0x28),('SavesManager','playerSave',0x04000310,0x10),('GlobalStaticVars','gLawnApp',0x0400015E,0),('Save','almanac_DeviceLock',0x0400063F,0x88),('Save','almanac_ZombieLock',0x0400063E,0x80),('Almanac_DeviceHome','deviceWindowList',0x040006E1,0x20),('Almanac_ZombieHome','zombieWindowList',0x0400075D,0x20)]
proof=[]
for owner,name,token,offset in expected:
 rows=[r for r in fields if r['type']==owner and r['name']==name];assert len(rows)==1;f=rows[0];assert int(f['token'],16)==token and f['offset']==offset;proof.append(f)
r={'field_map_sha256':hashlib.sha256((p/'native-fields.json').read_bytes()).hexdigest(),'fields':proof};Path(sys.argv[2]).write_text(json.dumps(r,indent=2));print(json.dumps(r))
