from pathlib import Path
import json,hashlib,sys
p=Path(sys.argv[1]);fields=json.loads((p/'native-fields.json').read_text());assert hashlib.sha256((p/'native-fields.json').read_bytes()).hexdigest()=='8273b66712f7fd64732a4fed64c453c2e71934d2cab329937f9dadaf57eef110';assert hashlib.sha256((p/'GameAssembly.dll').read_bytes()).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
proof=[]
for owner,name,offset in [('Banks','image_SunBank',0x48),('ProjectManager','moneyBank_Coin',0x40),('ProjectManager','moneyBank_Diamond',0x58)]:
 rows=[r for r in fields if r['type']==owner and r['name']==name];assert len(rows)==1 and rows[0]['offset']==offset;proof.append(rows[0])
r={'field_map_sha256':hashlib.sha256((p/'native-fields.json').read_bytes()).hexdigest(),'fields':proof};Path(sys.argv[2]).write_text(json.dumps(r,indent=2));print(json.dumps(r))
