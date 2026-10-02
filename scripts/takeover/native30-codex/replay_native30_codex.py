"""Rebuild and check the corrected batch from its complete evidence ZIP.
This replays CIL isolation and CLR fixtures, not Unity, Xcode, or a device.
Requires Python 3 and .NET SDK/runtime 10; no network restore dependencies.
"""
from pathlib import Path,PurePosixPath
import sys,json,hashlib,zipfile,subprocess
archive=Path(sys.argv[1]).resolve();out=Path(sys.argv[2]).resolve()
if out.exists():raise SystemExit('Replay requires a fresh output directory')
out.mkdir(parents=True)
with zipfile.ZipFile(archive) as z:
 names=z.namelist()
 if len(names)!=len(set(names)):raise SystemExit('Duplicate ZIP members')
 manifest=json.loads(z.read('MANIFEST.json'))
 if set(names)!=set(manifest)|{'MANIFEST.json'}:raise SystemExit('Manifest inventory mismatch')
 for name,row in manifest.items():
  p=PurePosixPath(name)
  if p.is_absolute() or '..' in p.parts or '\\' in name:raise SystemExit('Unsafe archive member')
  data=z.read(name)
  if len(data)!=row['bytes'] or hashlib.sha256(data).hexdigest()!=row['sha256']:raise SystemExit('Member hash mismatch: '+name)
  target=out/name;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(data)
cecil=out/'support/Mono.Cecil.dll';parent=out/'inputs/native29-parent.dll'
def run(args,name,expected=0):
 p=subprocess.run(args,capture_output=True,text=True);(out/(name+'.log')).write_text(p.stdout+p.stderr)
 if p.returncode!=expected:raise SystemExit((name,p.returncode,p.stdout+p.stderr[-3000:]))
for project in ['Patcher','Audit','RuntimeFixture']:
 run(['dotnet','build',str(out/'sources/takeover/native30'/project/(project+'.csproj')),'-c','Release','-o',str(out/'replay-built'/project),'-p:MonoCecilPath='+str(cecil)],project+'-build')
for i in [1,2]:run(['dotnet',str(out/'replay-built/Patcher/Patcher.dll'),str(parent),str(out/('rebuilt'+str(i)+'.dll')),str(out/('rebuilt'+str(i)+'.json'))],'rebuild'+str(i))
if (out/'rebuilt1.dll').read_bytes()!=(out/'rebuilt2.dll').read_bytes() or (out/'rebuilt1.dll').read_bytes()!=(out/'work/candidate1.dll').read_bytes():raise SystemExit('Candidate reproduction mismatch')
run(['dotnet',str(out/'replay-built/Audit/Audit.dll'),str(parent),str(out/'rebuilt1.dll'),str(out/'rebuilt1.json'),str(out/'replay-isolation.json')],'isolation')
fixture=out/'replay-built/RuntimeFixture/RuntimeFixture.dll'
run(['dotnet',str(fixture),str(out/'rebuilt1.dll'),str(out/'replay-runtime.json')],'runtime')
for mode in ['fault_emit','fault_invoke']:
 run(['dotnet',str(fixture),str(out/'rebuilt1.dll'),str(out/(mode+'-replay.json')),mode],mode,2)
 r=json.loads((out/(mode+'-replay.json')).read_text())
 assert r['tool_errors']==1 and not r['gate_pass'] and not r['mutations'][0]['Detected']
positive=out/'work/positive-only'
run(['dotnet','build',str(positive/'RuntimeFixture.csproj'),'-c','Release','-o',str(out/'replay-built/positive-only'),'-p:MonoCecilPath='+str(cecil)],'positive-build')
neg=[]
for name in ['old-return','old-unity-null','actual-sunfall-wrong-position']:
 rpath=out/(name+'-replay.json');run(['dotnet',str(out/'replay-built/positive-only/RuntimeFixture.dll'),str(out/'inputs'/(name+'.dll')),str(rpath)],name,2)
 r=json.loads(rpath.read_text());assert r['positive_failures']>0 and r['tool_errors']==0 and not r['gate_pass'];neg.append({'name':name,'positive_failures':r['positive_failures']})
r=json.loads((out/'replay-runtime.json').read_text())
result={'archive_sha256':hashlib.sha256(archive.read_bytes()).hexdigest(),'members_verified':len(manifest),'candidate_reproduced_exactly':True,'runtime':{k:r[k] for k in ['methods','positive_cases','positive_failures','negative_controls','tool_errors','gate_pass']},'actual_dll_negatives':neg,'scope':'CIL/CLR batch replay; not full compiler/Unity/Xcode/device acceptance'}
(out/'ARCHIVE-REPLAY.json').write_text(json.dumps(result,indent=2));print(json.dumps(result))
