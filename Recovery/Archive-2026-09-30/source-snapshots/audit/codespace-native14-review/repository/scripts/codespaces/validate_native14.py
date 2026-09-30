#!/usr/bin/env python3
"""Replay the pinned native13 control and native14 target inside a Codespace.
Requires verified seed/supplement ZIPs and prepared Cecil/full resolver.
This is a post-Linker diagnostic, not a full Unity export.
"""
from pathlib import Path
import hashlib, json, os, re, shutil, subprocess, tempfile, zipfile, time
root=Path(__file__).resolve().parents[2]
base=root/'.validation/native14'
base.mkdir(parents=True,exist_ok=True)
started=time.monotonic()
work=Path(tempfile.mkdtemp(prefix='replay-',dir=base))
def sha(p): return hashlib.sha256(p.read_bytes()).hexdigest()
def run(args, log, **kw):
    with (work/log).open('w') as f:
        r=subprocess.run(args,cwd=root,stdout=f,stderr=subprocess.STDOUT,timeout=600,**kw)
    if r.returncode: raise RuntimeError(f'{log}: exit {r.returncode}; see {work/log}')
for name,want in [('seed','ec652fbedfbd75fd4616aa18986ee26e2791b60ff3dbd099ecf9d52b2342b0b0'),('supplement','38f488d1e77d7972745f049f1b2a5462c89719e3cbab67f12423c2aa1145887b')]:
    z=base/(name+'.zip')
    assert sha(z)==want, name+' archive hash mismatch'
    with zipfile.ZipFile(z) as f: f.extractall(work/name)
C=work/'seed'; S=work/'supplement'
manifest=dict(line.split('=',1) for line in (C/'MANIFEST.txt').read_text().splitlines() if '=' in line)
assert manifest['status']=='NATIVE7_POST_LINKER_CANARY_SEED'
M=C/'Library/Bee/artifacts/iOS/ManagedStripped'
D=M/'GodsPVZRuntime1.dll'
assert sha(D)==manifest['stripped_godspvzruntime1_sha256']
assert sha(S/'il2cpp/libil2cpp/libil2cpp.icalls')=='85644eb6ebaacf203340a1ed357efa9e6802d447974fdecffe196a25ea440bd4'
(C/'il2cpp/build').mkdir(parents=True)
shutil.move(C/'il2cpp-deploy',C/'il2cpp/build/deploy')
shutil.copytree(S/'il2cpp/libil2cpp',C/'il2cpp/libil2cpp')
exe=C/'il2cpp/build/deploy/il2cpp'; exe.chmod(0o755)
rsp=C/'Library/Bee/artifacts/rsp'/manifest['rsp_basename']
rsp.write_text(Path(str(rsp)+'.template').read_text().replace('__PROJECT_ROOT__',str(C)))
# Reuse exactly the checked-in workflow's donor compile and patch chain.
yml=(root/'.github/workflows/stage9-native13-direct-il2cpp-canary.yml').read_text()
steps={}
for block in re.split(r'^      - ',yml,flags=re.M)[1:]:
    if not block.startswith('name: '): continue
    name=block.splitlines()[0][6:]
    match=re.search(r'^        run: \|\n((?:          .*\n|\n)*)',block,re.M)
    if match: steps[name]='\n'.join(line[10:] if line.startswith('          ') else line for line in match[1].splitlines())+'\n'
env=dict(os.environ,RUNNER_TEMP=str(work),GITHUB_WORKSPACE=str(root))
name='Reconstruct linked native13 from exact stripped native7'
steps[name]=chr(10).join(('bash '+line if line.startswith('scripts/takeover/patch_native13_linked.sh ') else line) for line in steps[name].splitlines() if not line.startswith('chmod +x '))
source_paths=[p for p in (root/'scripts/takeover').rglob('*') if p.suffix in ('.cs','.csproj','.sh') and not any(x in p.parts for x in ('bin','obj'))]
source_hashes={str(p.relative_to(root)):sha(p) for p in source_paths}

# Workflow calls this directory 'canary'.
(C).rename(work/'canary'); C=work/'canary'; M=C/'Library/Bee/artifacts/iOS/ManagedStripped'; D=M/'GodsPVZRuntime1.dll'
exe=C/'il2cpp/build/deploy/il2cpp'; rsp=C/'Library/Bee/artifacts/rsp'/manifest['rsp_basename']
rsp.write_text(Path(str(rsp)+'.template').read_text().replace('__PROJECT_ROOT__',str(C)))
for name,log in [('Compile typed Selector_B donor','donor.log'),('Reconstruct linked native13 from exact stripped native7','linked13.log')]:
    run(['bash','-c',steps[name]],log,env=env)
    print(name+' PASS',flush=True)
linked13=work/'GodsPVZRuntime1.native13.stripped.dll'
assert sha(linked13)=='e7eaa90de7ed308b23efa9f20ef728e2f578523e8eea61a5547f97f85e908548','linked native13 differs from GitHub control'
linked14=work/'GodsPVZRuntime1.native14.stripped.dll'
patcher=root/'scripts/takeover/PatcherNative14/PatcherNative14.csproj'
run(['dotnet','run','--project',str(patcher),'--',str(linked13),str(linked14),'linked'],'linked14.log')
second=work/'native14-repeat.dll'
run(['dotnet','run','--project',str(patcher),'--',str(linked13),str(second),'linked'],'linked14-repeat.log')
assert linked14.read_bytes()==second.read_bytes(),'linked14 nondeterministic'
def replay(label,dll):
    shutil.copy2(dll,D)
    output=C/'Library/Bee/artifacts/iOS/il2cppOutput'
    if output.exists(): shutil.rmtree(output)
    (output/'cpp/Symbols').mkdir(parents=True); (output/'data').mkdir()
    with (work/(label+'.log')).open('w') as f:
        r=subprocess.run([str(exe),'--custom-il2-cpp-root='+str(C/'il2cpp'),'@'+str(rsp)],cwd=C,stdout=f,stderr=subprocess.STDOUT,timeout=180)
    s=(work/(label+'.log')).read_text(errors='replace')
    methods=sorted(set(re.findall(r"IL2CPP error for method '([^']+)'",s)))
    if r.returncode and not methods: raise RuntimeError(label+' non-method failure; see '+str(work/(label+'.log')))
    result={'exit':r.returncode,'methods':methods,'input_sha256':sha(dll)}
    print(label+' '+json.dumps(result),flush=True)
    return result
a=replay('native13-control',linked13)
b=replay('native14',linked14)
target='System.Void ElementManager::CreateNewElements(T)'
assert target in a['methods'],'positive control missing target'
assert target not in b['methods'],'native14 target still fails'
expected_control={
 'System.Collections.Generic.List`1<Grid> Map::RandomGet_Grid_TestPlace(System.Int32,T,System.Int32,System.Int32)',target}
expected_next={
 'System.Collections.Generic.List`1<Grid> Map::RandomGet_Grid_TestPlace(System.Int32,T,System.Int32,System.Int32)',
 'System.Void VFXAnimationEvent::Binding(T,System.Boolean,System.Single,System.String)'}
assert set(a['methods'])==expected_control,'control differs from GitHub 36312525896'
assert set(b['methods'])==expected_next,'next blocker set differs from GitHub 36318580594'
assert sha(linked14)=='4514a1e5f87d4cbbc896811669c47187dff1c46178a09ee694889cc05e3a0d96','linked output differs from GitHub'
assert source_hashes=={str(p.relative_to(root)):sha(p) for p in source_paths},'tracked patcher source changed'

report={'status':'NATIVE14_CODESPACE_TARGET_CLOSURE_PASS','native13':a,'native14':b,'full_unity_export':False,'production_promotion':False,'work_directory':str(work),'elapsed_seconds':round(time.monotonic()-started,2),'source_hashes':source_hashes,'reference_run':36318580594,'source_commit':subprocess.check_output(['git','rev-parse','HEAD'],text=True).strip()}
(work/'result.json').write_text(json.dumps(report,indent=2)+'\n')
(base/'latest-result.json').write_text(json.dumps(report,indent=2)+'\n')
print(json.dumps(report,indent=2),flush=True)
