from pathlib import Path
import json,subprocess,hashlib,time
p=Path(__file__).parent;gh='C:/Program Files/GitHub CLI/gh.exe';repo='lyiming710-cloud/GodsPVZ-iOS';tag='recovery-archive-2026-09-30'
def api():return json.loads(subprocess.check_output([gh,'api','repos/'+repo+'/releases'],text=True))
release=next(x for x in api() if x['tag_name']==tag);existing={x['name']:x for x in release['assets']}
files=json.loads((p/'asset-paths.json').read_text(encoding='utf-8'));status=[]
for name,path in files.items():
 if name in existing:continue
 print('UPLOADING',name,flush=True);record={'name':name,'state':'uploading'};status.append(record);(p/'upload-remaining-status.json').write_text(json.dumps(status,indent=2))
 subprocess.run([gh,'release','upload',tag,path,'-R',repo],check=True)
 record['state']='uploaded';(p/'upload-remaining-status.json').write_text(json.dumps(status,indent=2));print('UPLOADED',name,flush=True)
