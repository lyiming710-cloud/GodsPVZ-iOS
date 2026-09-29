"""Native21 pinned type-encoding batch; isolated outputs, shared read-only tools."""
from pathlib import Path
import os,shutil,subprocess,json,hashlib,shlex
root=Path(__file__).resolve().parents[2]
old=root/'.validation/native20/replay';work=root/'.validation/native21/replay';work.mkdir(parents=True,exist_ok=True)
(work/'candidate.json').write_text(json.dumps({'status':'RUNNING','ipa_exported':False}))
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
expected={'unlinked':'7d2b3661f3e4aa3ba27b986c233d65c920e4c439851eae1788ae58323cc1d910','linked':'f561d48a353f1747b77260f9db69acabc1bb4860f46d4ffc447d81764c55efa9'}
C=work/'canary';oldC=old/'canary'
if not C.exists():
    C.mkdir()
    # The compiler and support assemblies are read-only. The managed input and
    # every generated output are independent copies; no writable hard links.
    (C/'il2cpp').symlink_to(oldC/'il2cpp',target_is_directory=True)
    shutil.copy2(oldC/'MANIFEST.txt',C/'MANIFEST.txt')
    shutil.copytree(oldC/'Library/Bee/artifacts/iOS/ManagedStripped',C/'Library/Bee/artifacts/iOS/ManagedStripped')
M=C/'Library/Bee/artifacts/iOS/ManagedStripped'
manifest=dict(l.split('=',1) for l in (C/'MANIFEST.txt').read_text().splitlines() if '=' in l)
rsp=C/'Library/Bee/artifacts/rsp'/manifest['rsp_basename'];rsp.parent.mkdir(parents=True,exist_ok=True)
text=(oldC/'Library/Bee/artifacts/rsp'/manifest['rsp_basename']).read_text().replace(str(oldC),str(C))
options=dict(a.split('=',1) for a in shlex.split(text) if a.startswith('--') and '=' in a)
output=C/'Library/Bee/artifacts/iOS/il2cppOutput'
for key in ['--generatedcppdir','--symbols-folder','--data-folder','--profiler-output-file']:
    assert Path(options[key]).resolve().is_relative_to(C.resolve()),(key,options[key])
assert str(oldC) not in text;rsp.write_text(text)
proposal=root/'Recovery/Native21-2026-09-29/PROPOSALS.json'
assert sha(proposal)=='0e74832cb1f63d68acf182beff071e6bb4f58b466e05583fd9e64ad9f7c07cc5'
for kind in expected:
    source=old/f'native20-{kind}.dll';assert sha(source)==expected[kind]
    target=work/f'native21-{kind}.dll'
    cmd=['dotnet','run','--project',str(root/'scripts/takeover/PatcherNative21'),'--',str(source),str(target),str(proposal),kind]
    for repeat in [False,True]:
        current=list(cmd)
        if repeat:current[current.index(str(target))]=str(target)+'.repeat'
        with (work/f'patch-{kind}{"-repeat" if repeat else ""}.log').open('w') as f:
            r=subprocess.run(current,cwd=root,env=dict(os.environ,GODSPVZ_RESOLVER=str(M)),stdout=f,stderr=subprocess.STDOUT)
        if r.returncode:print((work/f'patch-{kind}{"-repeat" if repeat else ""}.log').read_text());raise SystemExit(r.returncode)
    assert sha(target)==sha(Path(str(target)+'.repeat')),'nondeterministic candidate'
    subprocess.run(['python3',str(root/'scripts/codespaces/test_native21_types.py'),str(target)+'.targets.json'],check=True)
    print('PATCH_DETERMINISM_TYPES_PASS',kind,sha(target),flush=True)
shutil.copy2(work/'native21-linked.dll',M/'GodsPVZRuntime1.dll')
if output.exists():
    assert output.resolve().is_relative_to(work.resolve());shutil.rmtree(output)
(output/'cpp/Symbols').mkdir(parents=True);(output/'data').mkdir()
with (work/'il2cpp.log').open('w') as f:
    r=subprocess.run([str(C/'il2cpp/build/deploy/il2cpp'),'--custom-il2-cpp-root='+str(C/'il2cpp'),'@'+str(rsp)],cwd=C,stdout=f,stderr=subprocess.STDOUT)
print('IL2CPP',r.returncode,flush=True)
if r.returncode:print((work/'il2cpp.log').read_text()[-8000:]);raise SystemExit(r.returncode)
files=list((output/'cpp').glob('GodsPVZRuntime1*.cpp'));assert len(files)==21
assert (output/'data/Metadata/global-metadata.dat').is_file()
(work/'candidate.json').write_text(json.dumps({'status':'CONVERSION_ONLY','sources':expected,'outputs':{k:sha(work/f'native21-{k}.dll') for k in expected},'cpp_files':len(files),'cpp_root':str(output/'cpp'),'rsp_sha256':sha(rsp)},indent=2))
print('GENERATED_CPP_FILES',len(files),flush=True)
