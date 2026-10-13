from pathlib import Path
import base64

payload={n:base64.b64encode(Path(f'audit/native{n}-codex-2026-10-13/Patcher{n}.cs').read_bytes()).decode() for n in range(83,87)}
script='''from pathlib import Path
import base64,json,hashlib,subprocess,zipfile,shutil
w=Path('/workspaces/GodsPVZ-native24-codespace')
base=Path('/tmp/godspvz-native82-restored-2026-10-13');base.mkdir(exist_ok=True)
def run(args):
 r=subprocess.run(args,cwd=w,capture_output=True,text=True)
 if r.returncode:raise RuntimeError(r.stdout+r.stderr)
 return r.stdout
expected_zip='50823009a158de06a8e40056491d0fd0189fadcac87738abc250bc398de0d282'
archive=base/'godspvz-native78-82-pending-2026-10-13.zip'
if not archive.exists():run(['gh','release','download','native78-82-pending-2026-10-13','--repo','lyiming710-cloud/GodsPVZ-iOS','--pattern',archive.name,'--dir',str(base)])
if hashlib.sha256(archive.read_bytes()).hexdigest()!=expected_zip:raise RuntimeError('Immutable Release archive identity differs')
with zipfile.ZipFile(archive)as z:
 raw=z.read('candidates/native82/candidate1.dll')
if hashlib.sha256(raw).hexdigest()!='48b9e89e941c041d6c11c4064ad05d9384a1a4dd51d5f8d3fad9192cbda5adbf':raise RuntimeError('Pinned original generated Native82 input differs')
parent=base/'candidate1.dll';parent.write_bytes(raw)
'''+ 'payload='+repr(payload)+'\n'+'''expected={83:'d1727135f7fcb7141a62ad5bbe0e2342e6d25efb97d25b59ae223bb26fcd9975',84:'b3284d01f0a57c9d22f6297741e96a831874cb0a584af2e165b98c42def84587',85:'ba1b6ae5cd5b27b89797c16484cab5a6eb52157deab0bd4251e5fefe1aa562b0'}
files={'inputs/native82-parent.dll':parent};batches=[]
cecil=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
for n in range(83,87):
 d=Path(f'/tmp/godspvz-native{n}-codex-2026-10-13');d.mkdir(exist_ok=True);s=d/'Patcher';s.mkdir(exist_ok=True)
 (s/'Program.cs').write_bytes(base64.b64decode(payload[n]));(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
 out=d/('candidate3.dll' if n==84 else 'candidate1.dll');note=d/('generation3.json' if n==84 else 'generation1.json')
 if out.exists() or note.exists():raise RuntimeError('Fresh rebuild outputs required')
 r=subprocess.run(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(d/'bin'),'-p:MonoCecilPath='+str(cecil)],capture_output=True,text=True)
 (d/'rebuild-build.log').write_text(r.stdout+r.stderr)
 if r.returncode:raise RuntimeError(r.stdout+r.stderr)
 r=subprocess.run(['dotnet',str(d/'bin/Patcher.dll'),str(parent),str(out),str(note)],capture_output=True,text=True)
 (d/'rebuild-generation.log').write_text(r.stdout+r.stderr)
 if r.returncode:raise RuntimeError(r.stdout+r.stderr)
 data=json.loads(note.read_text());actual=hashlib.sha256(out.read_bytes()).hexdigest()
 if data['candidate_sha256']!=actual or (n in expected and actual!=expected[n]):raise RuntimeError('Generated artifact identity differs from earlier checkpoint')
 batches.append({'batch':n,**data})
 for f in d.rglob('*'):
  if f.is_file() and 'obj' not in f.parts:files[f'build/native{n}/{f.relative_to(d).as_posix()}']=f
 parent=out
 checkpoint=w/'.validation/native83-86-2026-10-13-checkpoints';checkpoint.mkdir(exist_ok=True)
 # Immediately keep one compressed batch on the persistent /workspaces volume.
 zip_path=checkpoint/f'native{n}-generated.zip'
 if zip_path.exists():raise RuntimeError('Fresh persistent batch archive required')
 with zipfile.ZipFile(zip_path,'w',zipfile.ZIP_DEFLATED,compresslevel=6)as z:
  for rel,f in files.items():
   if rel.startswith(f'build/native{n}/'):z.write(f,rel)
 print(json.dumps({'batch':n,'candidate_sha256':actual,'persistent_archive':str(zip_path),'persistent_sha256':hashlib.sha256(zip_path.read_bytes()).hexdigest()}),flush=True)
dest=Path('/tmp/godspvz-native83-86-build-checkpoint-2026-10-13.zip')
if dest.exists():raise RuntimeError('Fresh joint checkpoint required')
manifest={rel:{'bytes':f.stat().st_size,'sha256':hashlib.sha256(f.read_bytes()).hexdigest()}for rel,f in files.items()}
with zipfile.ZipFile(dest,'w',zipfile.ZIP_DEFLATED,compresslevel=6)as z:
 for rel,f in files.items():z.write(f,rel)
 z.writestr('MANIFEST.json',json.dumps(manifest,indent=2));z.writestr('GENERATED.json',json.dumps(batches,indent=2))
receipt={'path':str(dest),'bytes':dest.stat().st_size,'sha256':hashlib.sha256(dest.read_bytes()).hexdigest(),'batches':batches,'acceptance_tests_executed':False}
print(json.dumps(receipt,indent=2))
'''
Path('audit/native86-codex-2026-10-13/rebuild_remote.py').write_text(script)
