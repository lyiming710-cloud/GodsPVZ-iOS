from pathlib import Path
import hashlib,json,zipfile
R=Path(__file__).resolve().parent
files={}
for n in ('README.md','SpecBuilder.cs','Gate.cs','Harness.cs','Rehost.cs','build-and-verify.ps1','dump_managed.ps1','input-locks.json','prepare_ci_inputs.py','verify_spec_readback.py','verify_stack.py','extract_native.py','map_native.py','check_evidence.py','triage_new_blockers.ps1'):
 files[n]=R/n
for folder in ('specification','reopened','evidence','candidate','cloud-qualified','sources'):
 for p in (R/folder).rglob('*'):
  if not p.is_file():continue
  if folder=='sources' and p.suffix not in ('.txt','.md'):continue
  if folder=='candidate' and p.name!='Assembly-CSharp-native3-four-method.dll':continue
  files[p.relative_to(R).as_posix()]=p
for n in ('stage9-native3-four-method-static-gate.yml','stage9-native3-real-ios-xcode-gate.yml'):
 files['workflows/'+n]=R/'repository/.github/workflows'/n
manifest={n:{'sha256':hashlib.sha256(p.read_bytes()).hexdigest(),'size':p.stat().st_size} for n,p in sorted(files.items())}
out=R/'GodsPVZ-Stage9.1-native3-four-method-recovery.zip'
with zipfile.ZipFile(out,'w',zipfile.ZIP_DEFLATED,compresslevel=9) as z:
 for n,p in sorted(files.items()):z.write(p,n)
 z.writestr('bundle-manifest.json',json.dumps(manifest,indent=2))
with zipfile.ZipFile(out) as z:assert z.testzip() is None
sha=hashlib.sha256(out.read_bytes()).hexdigest()
(R/'GodsPVZ-Stage9.1-native3-four-method-recovery.sha256').write_text(sha+'  '+out.name+'\n')
print('bundle',out,'files',len(files),'bytes',out.stat().st_size,'sha256',sha)
