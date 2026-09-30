from pathlib import Path
import subprocess,json,hashlib,os
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');dst=Path('/tmp/godspvz-native24-codespace-2026-10-01');dst.mkdir(exist_ok=True)
v=w/'.validation';v.mkdir(exist_ok=True);link=v/'native24'
if not link.exists():link.symlink_to(dst,target_is_directory=True)
assert link.resolve()==dst
result={'commands':[],'reads':{},'versions':{}}
for cmd in [['dotnet','--version'],['dotnet','--list-runtimes'],['clang++','--version']]:
 r=subprocess.run(cmd,capture_output=True,text=True);result['versions'][' '.join(cmd)]=r.stdout
for args,name in [(['python3','scripts/takeover/native24/prepare_review.py','--work',str(dst)],'prepare'),(['python3','scripts/takeover/native24/reproduce_findings.py',str(dst/'historical-counterexamples.json')],'counterexamples'),(['python3','scripts/takeover/native24/test_provenance.py',str(dst/'source-controls.json')],'controls'),(['python3','scripts/takeover/native24/audit_provenance.py','--output',str(dst/'provenance-sites.json')],'sites')]:
 with (dst/(name+'.log')).open('w') as f:r=subprocess.run(args,cwd=w,stdout=f,stderr=subprocess.STDOUT)
 result['commands'].append({'argv':args,'exit':r.returncode,'log':str(dst/(name+'.log')),'tail':(dst/(name+'.log')).read_text()[-1800:]})
 if r.returncode:break
for rel in ['scripts/takeover/native24/audit_provenance.py','scripts/takeover/native24/provenance.py','scripts/takeover/native24/cpp_method_map.py','scripts/takeover/PatcherNative23/Program.cs','scripts/takeover/PatcherNative23/RepairNative23.csproj']:
 q=w/rel
 if q.exists():result['reads'][rel]=q.read_text()
result['batch1_edits']=json.loads((dst/'inputs/edits-batch1.json').read_text()) if (dst/'inputs/edits-batch1.json').exists() else None
support=old/'.validation/native23-independent-full-review/baseline/managed';result['support']={p.name:{'bytes':p.stat().st_size,'sha256':hashlib.sha256(p.read_bytes()).hexdigest()} for p in support.iterdir() if p.is_file() and p.suffix in ('.dll','.exe')}
result['tool_paths']=[str(p) for p in [old/'.validation/native19/replay/canary/il2cpp/build/deploy/il2cpp',old/'.validation/native21/replay/canary/il2cpp',old/'.validation/native23-independent-full-review/inspector-bin/Inspector.dll',old/'.validation/native23-independent-full-review/runtime-fixture/Program.cs'] if p.exists()]
(dst/'replay-summary.json').write_text(json.dumps(result,indent=2));print(json.dumps(result,indent=2))
