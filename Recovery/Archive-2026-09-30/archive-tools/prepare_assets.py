from pathlib import Path
import json,hashlib,shutil,zipfile
p=Path(__file__).parent;root=p.parents[1];d=p.parent/'native23-publication'/'Recovery/Archive-2026-09-30'
def sha(f):
 h=hashlib.sha256()
 with f.open('rb') as inp:
  for chunk in iter(lambda:inp.read(1024*1024),b''):h.update(chunk)
 return h.hexdigest()
metadata=json.loads((p/'remote-package-result.json').read_text(encoding='utf-8'))
assert sha(p/'codespace-historical-evidence.zip')==metadata['sha256']
remote=json.loads((p/'CODESPACE-MANIFEST.json').read_text(encoding='utf-8'))
index=json.loads((d/'SOURCE-INDEX.json').read_text(encoding='utf-8'));index=[x for x in index if x.get('origin')!='codespace']
with zipfile.ZipFile(p/'codespace-historical-evidence.zip') as z:
 assert z.testzip() is None
 for r in remote['files']:
  if Path(r['path']).suffix in {'.py','.sh','.cs','.csproj','.md','.yml','.yaml'} and r['bytes']<2000000 and 'object' in r:
   b=z.read(r['object']);assert hashlib.sha256(b).hexdigest()==r['sha256'];rel='source-snapshots/codespace/native19/'+r['path'];target=d/rel;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(b)
   index.append({'path':rel,'sha256':r['sha256'],'original':'codespace/native19/'+r['path'],'origin':'codespace'})
(d/'SOURCE-INDEX.json').write_text(json.dumps(index,ensure_ascii=False,indent=2),encoding='utf-8')
base='https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/download/recovery-archive-2026-09-30/'
files=[root/'GodsPVZ_1.0.2.zip',root/'1.0.2Android/GodsPVZ_1.0.2_Android.apk']+[p/n for n in ['local-project-history.zip','codespace-historical-evidence.zip','project-history.bundle','LOCAL-MANIFEST.json','CODESPACE-MANIFEST.json','HISTORY-SCOPE.json','GIT-REFS.txt','restore_archive.py']]
manifest={f.name:{'bytes':f.stat().st_size,'sha256':sha(f),'url':base+f.name} for f in files}
x=metadata['export'];manifest[x['asset']]={'bytes':x['bytes'],'sha256':x['sha256'],'url':base+x['asset']}
(p/'ASSET-SHA256.json').write_text(json.dumps(manifest,indent=2),encoding='utf-8');shutil.copyfile(p/'ASSET-SHA256.json',d/'ASSET-SHA256.json')
sumtext='\n'.join(v['sha256']+'  '+k for k,v in manifest.items())+'\n'+sha(p/'ASSET-SHA256.json')+'  ASSET-SHA256.json\n'
(p/'SHA256SUMS.txt').write_text(sumtext,encoding='utf-8');shutil.copyfile(p/'SHA256SUMS.txt',d/'SHA256SUMS.txt')
(p/'asset-paths.json').write_text(json.dumps({f.name:str(f) for f in files}|{'ASSET-SHA256.json':str(p/'ASSET-SHA256.json'),'SHA256SUMS.txt':str(p/'SHA256SUMS.txt')},indent=2),encoding='utf-8')
print(json.dumps({'source_snapshots':len(index),'assets':len(manifest)+2,'archive_MB':sum(x['bytes'] for x in manifest.values())/1e6},indent=2))
