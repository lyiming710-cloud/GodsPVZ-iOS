from pathlib import Path
import zipfile,hashlib,json,subprocess,base64,sys
p=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review');repo=p.parent.parent;w=p.parent/'native22'
files=[]
for f in p.iterdir():
 if f.is_file() and f.suffix in ('.json','.log','.py','.dll','.txt'):files.append((f,'review/'+f.name))
for tag in ('baseline','batch1','batch2','batch2a'):
 for f in (p/tag/'clang').glob('*.log'):files.append((f,'review/'+tag+'/clang/'+f.name))
 for f in (p/tag/'cpp').glob('GodsPVZRuntime1*'):
  if f.is_file():files.append((f,'review/'+tag+'/cpp/'+f.name))
for sub in ('patcher','inspector','runtime-fixture'):
 for f in (p/sub).iterdir():
  if f.is_file() and f.suffix in ('.cs','.csproj'):files.append((f,'review/'+sub+'/'+f.name))
for f in (repo/'scripts/codespaces').glob('native23_*.py'):files.append((f,'original-sources/'+f.name))
for f in (repo/'scripts/codespaces/RepairNative23').iterdir():
 if f.is_file():files.append((f,'original-sources/RepairNative23/'+f.name))
for f in w.glob('edits-batch*.json'):files.append((f,'inputs/'+f.name))
for name in ('afam3-accepted.json','afam3-quarantine.json','tiers.json','behave-sites.json'):
 f=w/'out'/name;files.append((f,'original-evidence/'+name))
files.append((w/'input/GodsPVZRuntime1.baseline.dll','inputs/baseline.dll'))
files.append((p.parent/'xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP/libil2cpp/codegen/il2cpp-codegen-common.h','headers/il2cpp-codegen-common.h'))
manifest={arc:{'bytes':f.stat().st_size,'sha256':hashlib.sha256(f.read_bytes()).hexdigest()} for f,arc in files}
(p/'archive-manifest.json').write_text(json.dumps(manifest,indent=2))
dest=p/'full-review-evidence.zip'
with zipfile.ZipFile(dest,'w',zipfile.ZIP_DEFLATED,compresslevel=6) as z:
 for f,arc in files:z.write(f,arc)
 z.write(p/'archive-manifest.json','archive-manifest.json')
meta={'file':dest.name,'bytes':dest.stat().st_size,'sha256':hashlib.sha256(dest.read_bytes()).hexdigest(),'members':len(files)+1}
(p/'archive-download-meta.json').write_text(json.dumps(meta,indent=2))
print(json.dumps(meta),flush=True)
sys.stdout.buffer.write(base64.b64encode(dest.read_bytes()))
