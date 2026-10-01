"""Verify complete Native28 archive and rebuild only its standalone CLR fixture in a new directory."""
from pathlib import Path
import json,hashlib,subprocess,zipfile,sys
archive=Path(sys.argv[1]).resolve();out=Path(sys.argv[2]).resolve();assert not out.exists(),'Use a new output directory'
with zipfile.ZipFile(archive) as z:
 manifest=json.loads(z.read('MANIFEST.json'));assert set(z.namelist())==set(manifest['files'])|{'MANIFEST.json'}
 for name,row in manifest['files'].items():
  raw=z.read(name);assert len(raw)==row['bytes'] and hashlib.sha256(raw).hexdigest()==row['sha256'],name
 out.mkdir(parents=True);build=out/'build';build.mkdir()
 for member,dest in [('tools/RuntimeFixture/Program.cs',build/'Program.cs'),('tools/RuntimeFixture/RuntimeFixture.csproj',build/'RuntimeFixture.csproj'),('fixture-support/Mono.Cecil.dll',out/'Mono.Cecil.dll'),('work/native28-final1.dll',out/'candidate.dll')]:dest.write_bytes(z.read(member))
 expected=json.loads(z.read('work/QUALIFICATION.json'))['candidate_sha256']
def run(args,name,exit=0):
 with (out/(name+'.log')).open('w') as f:rc=subprocess.run(args,stdout=f,stderr=subprocess.STDOUT).returncode
 assert rc==exit,(name,rc,(out/(name+'.log')).read_text()[-3000:])
run(['dotnet','build',str(build/'RuntimeFixture.csproj'),'-c','Release','-o',str(out/'bin'),'-p:MonoCecilPath='+str(out/'Mono.Cecil.dll')],'build')
reports={}
for mode in ['normal','fault_emit','fault_invoke']:
 run(['dotnet',str(out/'bin/RuntimeFixture.dll'),str(out/'candidate.dll'),str(out/(mode+'.json'))]+([] if mode=='normal' else [mode]),mode,0 if mode=='normal' else 2)
 r=json.loads((out/(mode+'.json')).read_text());reports[mode]={k:r[k] for k in ['methods','positive_cases','positive_failures','negative_controls','tool_errors','gate_pass']}
 if mode=='normal':assert r['positive_cases']==854 and r['positive_failures']==0 and r['negative_controls']==28 and r['tool_errors']==0 and r['gate_pass']
 else:assert not r['gate_pass'] and r['tool_errors']==1 and not r['mutations'][0]['Detected']
assert hashlib.sha256((out/'candidate.dll').read_bytes()).hexdigest()==expected
report={'archive_sha256':hashlib.sha256(archive.read_bytes()).hexdigest(),'verified_members':len(manifest['files']),'candidate_sha256':expected,'reports':reports,'scope':'Complete archive hash validation and ordinary source-copy CLR fixture rebuild; not a new IL2CPP/Apple build'};(out/'REPLAY.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
