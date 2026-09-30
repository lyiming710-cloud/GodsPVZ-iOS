from pathlib import Path
import hashlib,json,zipfile,subprocess,re,shutil
R=Path(__file__).resolve().parent.parent
A=R/'audit';O=A/'handoff-2026-09-27';O.mkdir(exist_ok=True)
repo=A/'stage9-native3/repository'
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
source_ext={'.py','.cs','.csproj','.config','.gitignore','.ps1','.md','.json','.txt','.tsv','.log','.yml','.yaml','.sh','.sha256','.exe','.dll','.nupkg'}
files={};excluded=[];redactions=[]
def add(p,name=None):
 n=name or p.relative_to(R).as_posix()
 if any(x in p.name.lower() for x in ['license','activation']):
  excluded.append({'path':n,'reason':'license or activation material'});return
 if p.suffix.lower() not in source_ext:return
 b=p.read_bytes()
 if p.suffix.lower() not in {'.exe','.dll','.nupkg'}:
  try:s=b.decode('utf-8-sig')
  except UnicodeDecodeError:s=None
  if s is not None:
   patterns=[r'\bgh[pousr]_[A-Za-z0-9]{25,}\b',r'\bgithub_pat_[A-Za-z0-9_]{30,}\b',r'(?i)(Bearer\s+)[A-Za-z0-9._-]{25,}',r'(?i)(-password\s+)[^\s"\']+']
   for pat in patterns:
    s,c=re.subn(pat,'[REDACTED]',s)
    if c:redactions.append({'path':n,'count':c,'rule':pat})
   # Preserve original bytes if nothing was changed.
   if any(x['path']==n for x in redactions):b=s.encode('utf-8')
 files[n]=b
for version in ['stage9-native3','stage9-native4']:
 base=A/version
 for p in base.iterdir():
  if p.is_file():add(p)
 for d in ['candidate','evidence','specification','reopened','resolver','sources']:
  if (base/d).exists():
   for p in (base/d).rglob('*'):
    if p.is_file():add(p)
 for p in (base/'tools').glob('*'):
  if p.is_file() and p.name!='actionlint.exe':add(p)
for p in (A/'local-preflight').iterdir():
 if p.is_file():add(p)
add(A/'local-preflight/exact-editor-tools/extraction-manifest.json')
for p in (A/'stage9-native5').rglob('*'):
 if p.is_file():add(p)
add(A/'HANDOFF-2026-09-27.md','HANDOFF.md')
add(Path(__file__))
tracked=subprocess.check_output([git,'ls-tree','-r','--name-only','b873b8f75173dcc022c1a4de555bc21e78e6e40d'],cwd=repo,text=True).splitlines()
files['REMOTE-COMMIT-ALL-TRACKED-PATHS.txt']=('\n'.join(tracked)+'\n').encode()
assert (repo/'scripts/codespaces/Patcher/Program.cs').exists(),'Missing latest remote patcher source'
for name in tracked:
 p=repo/name
 if p.is_file():add(p,'remote-repository-b873b8f/'+name)
excluded.extend([{'path':p,'reason':'reproducible download/cache or duplicated build output; source identities retained'} for p in ['audit/*/inputs/editor-parts','audit/*/python-deps','audit/*/preflight-test','audit/*/cloud-qualified','audit/local-preflight/dotnet-runtime*','audit/local-preflight/windows-codegen','audit/local-preflight/exact-editor-tools/codegen','audit/local-preflight/exact-editor-tools/resolver','audit/local-preflight/exact-editor-tools/aot-resolver']])
def package(name,data):
 manifest=[{'path':k,'size':len(v),'sha256':hashlib.sha256(v).hexdigest()} for k,v in sorted(data.items())]
 with zipfile.ZipFile(O/name,'w',zipfile.ZIP_DEFLATED,compresslevel=9) as z:
  for k,v in sorted(data.items()):z.writestr(k,v)
  z.writestr('MANIFEST-SHA256.json',json.dumps(manifest,indent=2))
 with zipfile.ZipFile(O/name) as z:
  assert z.testzip() is None
  for m in manifest:assert hashlib.sha256(z.read(m['path'])).hexdigest()==m['sha256']
 (O/(name+'.manifest.json')).write_text(json.dumps(manifest,indent=2))
 return {'name':name,'files':len(manifest),'bytes':(O/name).stat().st_size,'sha256':hashlib.sha256((O/name).read_bytes()).hexdigest()}
files['EXCLUDED-FILES.json']=json.dumps(excluded,indent=2).encode()
files['REDACTIONS.json']=json.dumps(redactions,indent=2).encode()
results=[package('GodsPVZ-Stage9-Latest-2026-09-27.zip',files)]
inputs={}
for p in [R/'Assembly-CSharp-59bb-native2.dll',A/'stage9-native4/inputs/GameAssembly.dll',A/'stage9-native4/inputs/global-metadata.dat',A/'stage9-native4/inputs/59bb-historical.dll']:
 inputs[p.name]=p.read_bytes()
results.append(package('GodsPVZ-Stage9-Locked-Inputs-2026-09-27.zip',inputs))
shutil.copy2(A/'HANDOFF-2026-09-27.md',O/'HANDOFF.md')
(O/'UPLOAD-MANIFEST.json').write_text(json.dumps({'packages':results,'redacted_files':len(set(r['path'] for r in redactions))},indent=2))
print(json.dumps(results,indent=2))
