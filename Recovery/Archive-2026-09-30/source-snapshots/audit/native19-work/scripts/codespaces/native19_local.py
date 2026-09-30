"""Isolated Codespaces replay: never overwrites the Native18 working directory."""
from pathlib import Path
import os,shutil,subprocess,json,hashlib,shlex
root=Path(__file__).resolve().parents[2]
old=Path(os.environ.get('NATIVE19_SOURCE_REPLAY','/workspaces/GodsPVZ-native14-check/.validation/native14/replay-edye4slp')).resolve()
work=root/'.validation/native19/replay';work.mkdir(parents=True,exist_ok=True)
(work/'candidate.json').write_text(json.dumps({'status':'RUNNING','ipa_exported':False}))
cecil=root/'Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll';cecil.parent.mkdir(parents=True,exist_ok=True)
if not cecil.exists():
 source_repo=Path(subprocess.check_output(['git','-C',str(old),'rev-parse','--show-toplevel'],text=True).strip())
 shutil.copy2(source_repo/'Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll',cecil)
if not (work/'canary').exists():shutil.copytree(old/'canary',work/'canary')
C=work/'canary';M=C/'Library/Bee/artifacts/iOS/ManagedStripped'
expected={'unlinked':'74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2','linked':'9bf2d7a033632061e8e71d32c70312c3c234298649495afc384d0dfe7120a125'}
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
manifest=dict(l.split('=',1) for l in (C/'MANIFEST.txt').read_text().splitlines() if '=' in l)
rsp=C/'Library/Bee/artifacts/rsp'/manifest['rsp_basename']
rsp_text=rsp.read_text().replace(str(old/'canary'),str(C))
output=C/'Library/Bee/artifacts/iOS/il2cppOutput'
options=dict(a.split('=',1) for a in shlex.split(rsp_text) if a.startswith('--') and '=' in a)
for key,path in {'--generatedcppdir':output/'cpp','--symbols-folder':output/'cpp/Symbols','--data-folder':output/'data','--profiler-output-file':C/'Library/Bee/artifacts/il2cpp_conv_wdxc.traceevents'}.items():
 assert Path(options[key]).resolve()==path.resolve(),f'output path escapes isolated replay: {key}'
assert str(old) not in rsp_text,'stale replay path'
rsp.write_text(rsp_text)
for kind in ['unlinked','linked']:
 source=work/f'native17-{kind}.dll'
 if not source.exists():shutil.copy2(old/source.name,source)
 assert sha(source)==expected[kind],f'{kind} input hash mismatch'
 cmd=['dotnet','run','--project',str(root/'scripts/takeover/PatcherNative19'),'--',str(source),str(work/f'native19-{kind}.dll')]+(['linked'] if kind=='linked' else [])
 with (work/f'patch-{kind}.log').open('w') as f:r=subprocess.run(cmd,cwd=root,env=dict(os.environ,GODSPVZ_RESOLVER=str(M)),stdout=f,stderr=subprocess.STDOUT)
 print('PATCH',kind,r.returncode,flush=True)
 if r.returncode:print((work/f'patch-{kind}.log').read_text());raise SystemExit(r.returncode)
 repeat=work/f'native19-{kind}-repeat.dll';repeat_cmd=list(cmd);repeat_cmd[repeat_cmd.index(str(work/f'native19-{kind}.dll'))]=str(repeat)
 with (work/f'patch-{kind}-repeat.log').open('w') as f:subprocess.run(repeat_cmd,cwd=root,env=dict(os.environ,GODSPVZ_RESOLVER=str(M)),stdout=f,stderr=subprocess.STDOUT,check=True,timeout=180)
 assert sha(repeat)==sha(work/f'native19-{kind}.dll'),f'{kind} output is not deterministic'
 for verifier in ['verify_native19_types.py','test_native19_types.py','test_native19_die_cfg.py']:
  with (work/f'{kind}-{verifier}.log').open('w') as f:
   r=subprocess.run(['python3',str(root/'scripts/codespaces'/verifier),str(work/f'native19-{kind}.dll.targets.json')],stdout=f,stderr=subprocess.STDOUT)
  if r.returncode:print((work/f'{kind}-{verifier}.log').read_text());raise SystemExit(r.returncode)
 print('DETERMINISM_AND_TYPES',kind,'PASS',flush=True)
shutil.copy2(work/'native19-linked.dll',M/'GodsPVZRuntime1.dll')
if output.exists():shutil.rmtree(output)
(output/'cpp/Symbols').mkdir(parents=True);(output/'data').mkdir()
with (work/'il2cpp.log').open('w') as f:r=subprocess.run([str(C/'il2cpp/build/deploy/il2cpp'),'--custom-il2-cpp-root='+str(C/'il2cpp'),'@'+str(rsp)],cwd=C,stdout=f,stderr=subprocess.STDOUT)
print('IL2CPP',r.returncode,flush=True)
if r.returncode:print((work/'il2cpp.log').read_text()[-8000:])
if not r.returncode:
 files=list((output/'cpp').glob('GodsPVZRuntime1*.cpp'))
 assert len(files)>=10,'IL2CPP returned success without generating expected game C++'
 assert (output/'data/Metadata/global-metadata.dat').is_file(),'metadata missing'
 (work/'candidate.json').write_text(json.dumps({'status':'CONVERSION_ONLY','sources':expected,'outputs':{k:sha(work/f'native19-{k}.dll') for k in expected},'cpp_files':len(files),'cpp_root':str(output/'cpp'),'rsp_sha256':sha(rsp)},indent=2))
 print('GENERATED_CPP_FILES',len(files),flush=True)
raise SystemExit(r.returncode)
