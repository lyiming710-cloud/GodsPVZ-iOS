from pathlib import Path
import subprocess,json,hashlib,sys,time
p=Path(__file__).parent;root=p.parents[1];gh='C:/Program Files/GitHub CLI/gh.exe';repo='lyiming710-cloud/GodsPVZ-iOS';tag='recovery-archive-2026-09-30'
r=subprocess.run([gh,'release','view',tag,'-R',repo,'--json','id,isDraft'],capture_output=True,text=True)
if r.returncode:
 subprocess.run([gh,'release','create',tag,'-R',repo,'--draft','--prerelease','--latest=false','--target','77fb194e59ea420b480620001a2d30b3d8b9adad','--title','GodsPVZ recovery source and evidence archive · 2026-09-30','--notes-file',str(p/'RELEASE-NOTES.md')],check=True)
else:assert json.loads(r.stdout)['isDraft'],r.stdout
status=[]
for f in [root/'GodsPVZ_1.0.2.zip',root/'1.0.2Android/GodsPVZ_1.0.2_Android.apk']:
 h=hashlib.sha256()
 with f.open('rb') as inp:
  for c in iter(lambda:inp.read(1024*1024),b''):h.update(c)
 record={'name':f.name,'bytes':f.stat().st_size,'sha256':h.hexdigest(),'status':'uploading'};status.append(record)
 (p/'upload-originals-status.json').write_text(json.dumps(status,indent=2));print('UPLOADING',f.name,flush=True)
 subprocess.run([gh,'release','upload',tag,str(f),'-R',repo],check=True)
 record['status']='uploaded';(p/'upload-originals-status.json').write_text(json.dumps(status,indent=2));print('UPLOADED',f.name,flush=True)
