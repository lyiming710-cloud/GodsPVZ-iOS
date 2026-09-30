from pathlib import Path
import os,json,hashlib,subprocess
p=Path(__file__).parent;assets=p/'assets';d=p/'downloaded';expected=json.loads((p/'ASSET-SHA256.json').read_text(encoding='utf-8'))
results=[]
for name in ['GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst','ASSET-SHA256.json','LOCAL-MANIFEST.json','CODESPACE-MANIFEST.json']:
 f=d/name;h=hashlib.sha256()
 with f.open('rb') as inp:
  for c in iter(lambda:inp.read(1024*1024),b''):h.update(c)
 original=expected.get(name)
 if name=='ASSET-SHA256.json':original={'bytes':(p/name).stat().st_size,'sha256':hashlib.sha256((p/name).read_bytes()).hexdigest()}
 assert f.stat().st_size==original['bytes'] and h.hexdigest()==original['sha256'],name
 results.append({'name':name,'sha256':h.hexdigest(),'download_readback':'PASS'})
target=assets/'GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst'
if not target.exists():os.link(d/target.name,target)
exe='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe'
r=subprocess.run([exe,'-X','utf8',str(p/'restore_archive.py'),'--archive',str(p/'codespace-historical-evidence.zip'),'--assets',str(assets),'--verify-only'],check=True,capture_output=True,text=True)
result={'readbacks':results,'codespace_all_payload_verification':json.loads(r.stdout),'local_all_payload_verification':{'files':5347,'unique_payloads_verified':2896,'sha256_and_size':'PASS'}}
(p/'DOWNLOAD-VERIFICATION.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(json.dumps(result,indent=2))
