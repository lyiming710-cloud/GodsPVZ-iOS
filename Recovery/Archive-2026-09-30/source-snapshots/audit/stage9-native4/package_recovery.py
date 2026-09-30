from pathlib import Path
import json,hashlib,zipfile
R=Path(__file__).resolve().parent;repo=R.parent/'stage9-native3/repository';files={}
sources=('README.md','SpecBuilder.cs','Gate.cs','Harness.cs','Rehost.cs','build-and-verify.ps1','prepare_ci_inputs.py','dump_managed.ps1','verify_stack.py','verify_spec_readback.py','extract_native.py','map_native.py','inspect_new_native.py','input-locks.json')
for n in sources:files[n]=R/n
for folder in ('evidence','specification','reopened','cloud-qualified'):
 for p in (R/folder).rglob('*'):
  if p.is_file():files[p.relative_to(R).as_posix()]=p
files['candidate/Assembly-CSharp-native4-six-method.dll']=R/'candidate/Assembly-CSharp-native4-six-method.dll'
files['ReferenceAssemblies/UnityEngine.AnimationModule.dll']=R/'resolver/UnityEngine.AnimationModule.dll'
files['native-evidence/animation-resolver-provenance.json']=R/'evidence/animation-resolver-provenance.json'
for n in ('stage9-native4-six-method-static-gate.yml','stage9-native4-real-ios-xcode-gate.yml'):
 files['workflows/'+n]=repo/'.github/workflows'/n
manifest={n:{'size':p.stat().st_size,'sha256':hashlib.sha256(p.read_bytes()).hexdigest()} for n,p in sorted(files.items())}
out=R/'GodsPVZ-Stage9.1-native4-six-method-recovery.zip'
with zipfile.ZipFile(out,'w',zipfile.ZIP_DEFLATED,compresslevel=9) as z:
 for n,p in sorted(files.items()):z.write(p,n)
 z.writestr('bundle-manifest.json',json.dumps(manifest,indent=2))
with zipfile.ZipFile(out) as z:assert z.testzip() is None
sha=hashlib.sha256(out.read_bytes()).hexdigest();out.with_suffix('.sha256').write_text(sha+'  '+out.name+'\n')
print(out,'files',len(files),'bytes',out.stat().st_size,'sha256',sha)
