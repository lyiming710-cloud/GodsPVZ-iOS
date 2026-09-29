from pathlib import Path
import subprocess,shutil,json,hashlib
root=Path(__file__).resolve().parents[2];work=root/'.validation/native19/fixture';work.mkdir(parents=True,exist_ok=True)
logs=[]
def run(args):
 r=subprocess.run(args,cwd=root,stdout=subprocess.PIPE,stderr=subprocess.STDOUT,text=True,timeout=180)
 logs.append(r.stdout);print(r.stdout,end='');(work/'fixture.log').write_text('\n'.join(logs))
 r.check_returncode()
run(['dotnet','build','scripts/codespaces/Native19Fixture','-o',str(work/'original')])
out=work/'patched/Native19Fixture.dll';out.parent.mkdir(exist_ok=True)
run(['dotnet','run','--project','scripts/takeover/PatcherNative19','--',str(work/'original/Native19Fixture.dll'),str(out),'correction-fixture'])
shutil.copy2(work/'original/Native19Fixture.runtimeconfig.json',out.parent)
run(['dotnet',str(out)])
(work/'result.json').write_text(json.dumps({'status':'PASS','scope':'six shared emitters against CLR Unity/helper doubles','candidate_assembly_sha256':hashlib.sha256(out.read_bytes()).hexdigest(),'source_sha256':{str(p.relative_to(root)):hashlib.sha256(p.read_bytes()).hexdigest() for p in list((root/'scripts/takeover/PatcherNative19').glob('*.cs'))+[root/'scripts/codespaces/Native19Fixture/Program.cs']}},indent=2))
