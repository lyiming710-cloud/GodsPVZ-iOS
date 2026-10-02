"""Offline batch replay: Python 3, .NET 10, Linux x64 Clang required.
Re-executes pinned original PC leaf bodies; no Unity/Xcode/device acceptance.
"""
from pathlib import Path,PurePosixPath
import sys,json,hashlib,zipfile,subprocess
archive=Path(sys.argv[1]).resolve();out=Path(sys.argv[2]).resolve()
if out.exists():raise SystemExit('Fresh output directory required')
out.mkdir(parents=True)
with zipfile.ZipFile(archive) as z:
 names=z.namelist();manifest=json.loads(z.read('MANIFEST.json'))
 assert len(names)==len(set(names)) and set(names)==set(manifest)|{'MANIFEST.json'}
 for name,row in manifest.items():
  p=PurePosixPath(name);assert not p.is_absolute() and '..' not in p.parts and '\\' not in name
  data=z.read(name);assert len(data)==row['bytes'] and hashlib.sha256(data).hexdigest()==row['sha256'],name
  target=out/name;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(data)
def run(args,name,expected=0):
 p=subprocess.run(args,capture_output=True,text=True);(out/(name+'.log')).write_text(p.stdout+p.stderr)
 if p.returncode!=expected:raise SystemExit((name,p.returncode,(p.stdout+p.stderr)[-5000:]))
cecil=out/'support/Mono.Cecil.dll';parent=out/'inputs/native40-parent.dll'
for project in ['Patcher','Audit','RuntimeFixture']:
 run(['dotnet','build',str(out/'sources/takeover/native41'/project/(project+'.csproj')),'-c','Release','-o',str(out/'replay-built'/project),'-p:MonoCecilPath='+str(cecil)],project+'-build')
for i in [1,2]:run(['dotnet',str(out/'replay-built/Patcher/Patcher.dll'),str(parent),str(out/f'rebuilt{i}.dll'),str(out/f'rebuilt{i}.json')],f'build-candidate{i}')
assert (out/'rebuilt1.dll').read_bytes()==(out/'rebuilt2.dll').read_bytes()==(out/'work/candidate1.dll').read_bytes()
run(['dotnet',str(out/'replay-built/Audit/Audit.dll'),str(parent),str(out/'rebuilt1.dll'),str(out/'rebuilt1.json'),str(out/'replay-isolation.json')],'isolation')
for source,tsv in [('native41_oracle','NATIVE-ORACLE.tsv')]:
 exe=out/source
 run(['clang++','-std=c++17','-O0',str(out/'sources/takeover/native41'/(source+'.cpp')),'-o',str(exe)],source+'-build')
 p=subprocess.run([str(exe),str(out/'original/GameAssembly.dll')],capture_output=True)
 (out/(source+'.stderr')).write_bytes(p.stderr);assert p.returncode==0
 (out/tsv).write_bytes(p.stdout);assert p.stdout==(out/'work'/tsv).read_bytes(),tsv
run(['dotnet',str(out/'replay-built/RuntimeFixture/RuntimeFixture.dll'),str(out/'rebuilt1.dll'),str(out/'NATIVE-ORACLE.tsv'),str(out/'replay-runtime.json')],'runtime')
neg=[]
for mutant in sorted((out/'work').glob('*-semantic-negative.dll')):
 report=out/(mutant.stem+'.json');run(['dotnet',str(out/'replay-built/RuntimeFixture/RuntimeFixture.dll'),str(mutant),str(out/'NATIVE-ORACLE.tsv'),str(report),'positive-only'],mutant.stem,2)
 r=json.loads(report.read_text());assert r['positive_failures']>0 and r['tool_errors']==0 and not r['gate_pass'];neg.append({'dll':mutant.name,'positive_failures':r['positive_failures']})
assert len(neg)==4
scope=[]
for mutant in sorted((out/'work').glob('*-scope-negative.dll')):
 report=out/(mutant.stem+'-must-not-pass.json');run(['dotnet',str(out/'replay-built/Audit/Audit.dll'),str(parent),str(mutant),str(out/'work/patch1.json'),str(report)],mutant.stem,2);assert not report.exists();scope.append(mutant.name)
assert len(scope)==5
audit=json.loads((out/'supplement/GENERATED-METADATA-AUDIT.json').read_text())
expected=out/'expected-literals.json';expected.write_text(json.dumps(audit['removed_literals']))
run([sys.executable,str(out/'sources/audit_generated_metadata.py'),str(out/'supplement/native39-generated-metadata.dat'),str(out/'supplement/native40-generated-metadata.dat'),str(expected)],'generated-metadata-audit')
r=json.loads((out/'replay-runtime.json').read_text());assert r['positive_cases']==8228 and r['negative_controls']==26 and r['tool_errors']==0 and r['gate_pass']
result={'archive_sha256':hashlib.sha256(archive.read_bytes()).hexdigest(),'members_verified':len(manifest),'candidate_reproduced_exactly':True,'original_native_state_oracles_reproduced_exactly_with_explicit_preconditions':True,'runtime':{k:r[k] for k in ['methods','positive_cases','positive_failures','negative_controls','tool_errors','gate_pass']},'actual_dll_negatives':neg,'append_scope_negatives':scope,'generated_metadata_audit_pass':True,'scope':'Batch CIL/CLR with original editor deletion callers, recorded native dependencies and public CLR List.Remove under explicit layouts/equality preconditions; no Unity/Xcode/device acceptance'}
(out/'ARCHIVE-REPLAY.json').write_text(json.dumps(result,indent=2));print(json.dumps(result))
