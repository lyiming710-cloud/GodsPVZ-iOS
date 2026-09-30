
from pathlib import Path
import subprocess, json, shutil, hashlib, os, shlex, concurrent.futures, re, time
repo=Path('/workspaces/GodsPVZ-native19'); src=repo/'.validation/native22'; dst=repo/'.validation/native23-independent-full-review'
def sha(p): return hashlib.sha256(Path(p).read_bytes()).hexdigest()
def save(name,data): (dst/name).write_text(json.dumps(data,indent=2))
def run(args,log,**kw):
    with open(dst/log,'w') as f: return subprocess.run(args,stdout=f,stderr=subprocess.STDOUT,**kw).returncode
result={'start':time.time(),'head':subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip(),'stages':{}}
save('status.json',result)
project=dst/'patcher'; project.mkdir(exist_ok=True)
for n in ('Program.cs','RepairNative23.csproj'): shutil.copy2(repo/'scripts/codespaces/RepairNative23'/n,project/n)
result['sources']={p.name:sha(p) for p in project.iterdir() if p.is_file()}
rc=run(['dotnet','build',str(project/'RepairNative23.csproj'),'-c','Release','-o',str(dst/'patcher-bin')],'patcher-build.log')
assert rc==0, 'patcher build failed'
can=repo/'.validation/native21/replay/canary'
managed=can/'Library/Bee/artifacts/iOS/ManagedStripped'
env=dict(os.environ,GODSPVZ_RESOLVER=str(managed),PYTHONDONTWRITEBYTECODE='1')
for tag, inp, edits, claimed in (
    ('batch1',src/'input/GodsPVZRuntime1.baseline.dll',src/'edits-batch1.json',src/'work/batch1.dll'),
    ('batch2',dst/'batch1.dll',src/'edits-batch2.json',src/'work/batch2.dll'),
    ('batch2a',dst/'batch1.dll',src/'edits-batch2a.json',src/'work/batch2a.dll')):
    hashes=[]
    for repeat in (1,2):
        out=dst/(tag+('.dll' if repeat==1 else '-repeat.dll'))
        rc=run(['dotnet',str(dst/'patcher-bin/RepairNative23.dll'),str(inp),str(out),str(edits),str(dst/(tag+f'-edits-{repeat}.txt'))],tag+f'-patch-{repeat}.log',env=env)
        assert rc==0,tag+' patch failed'
        hashes.append(sha(out))
    result['stages'][tag]={'input':sha(inp),'edits':sha(edits),'output':hashes,'claimed':sha(claimed),'reproducible':hashes[0]==hashes[1]==sha(claimed)}
    assert result['stages'][tag]['reproducible'],tag+' mismatch'
    save('status.json',result)
for tag, dll in [('baseline',src/'input/GodsPVZRuntime1.baseline.dll')]+[(t,dst/(t+'.dll')) for t in ('batch1','batch2','batch2a')]:
    rc=run(['dotnet',str(src/'bin/PatcherNative19.dll'),str(dll),str(dst/('all-methods.'+tag+'.json')),'export-all'],tag+'-export.log',env=env)
    assert rc==0,tag+' export failed'
    result['stages'].setdefault(tag,{})['export_sha']=sha(dst/('all-methods.'+tag+'.json'))
    save('status.json',result)
il2cpp=repo/'.validation/native19/replay/canary/il2cpp/build/deploy/il2cpp'
xinc=repo/'.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP'
args=shlex.split((src/'regen-args.txt').read_text())
for tag,dll in [('baseline',src/'input/GodsPVZRuntime1.baseline.dll')]+[(t,dst/(t+'.dll')) for t in ('batch1','batch2','batch2a')]:
    out=dst/tag; out.mkdir(exist_ok=True); mm=out/'managed'; shutil.copytree(managed,mm,dirs_exist_ok=True)
    shutil.copy2(dll,mm/'GodsPVZRuntime1.dll')
    for n in ('cpp','data','symbols'): (out/n).mkdir(exist_ok=True)
    aa=[]
    for a in args:
        if a.startswith('--assembly='):
            pp=Path(a.split('=',1)[1]); assert (mm/pp.name).exists(),pp.name
            aa.append('--assembly='+str(mm/pp.name))
    cmd=[str(il2cpp),'--convert-to-cpp','--custom-il2-cpp-root='+str(can/'il2cpp')]+aa+['--generatedcppdir='+str(out/'cpp'),'--symbols-folder='+str(out/'symbols'),'--emit-null-checks','--enable-array-bounds-check','--dotnetprofile=unityaot-macos','--data-folder='+str(out/'data')]
    save(tag+'-conversion-command.json',cmd)
    rc=run(cmd,tag+'-il2cpp.log',cwd=can,env=dict(os.environ,PROJECT_DIR=str(can)))
    stage=result['stages'].setdefault(tag,{})
    stage['il2cpp_exit']=rc; stage['input_dll']=sha(mm/'GodsPVZRuntime1.dll'); stage['assemblies']={p.name:sha(p) for p in mm.glob('*.dll')}
    cpp=list(sorted((out/'cpp').glob('*.cpp'))); stage['cpp_count']=len(cpp)
    save('status.json',result)
    assert rc==0 and len(cpp)==264,tag+' IL2CPP incomplete'
    logs=out/'clang';logs.mkdir(exist_ok=True)
    flags=['clang++','-std=c++11','-fsyntax-only','-ferror-limit=0','-Wno-tautological-compare','-Wno-unused-value','-Wno-invalid-noreturn','-DRUNTIME_IL2CPP','-DIL2CPP_MONO_DEBUGGER_DISABLED','-DIL2CPP_DEBUG=0','-DNDEBUG','-DBASELIB_INLINE_NAMESPACE=il2cpp_baselib']
    for p in (out/'cpp',xinc/'libil2cpp/pch',xinc/'libil2cpp',xinc/'external/baselib/Include',xinc/'external/baselib/Platforms/Linux/Include'):flags.append('-I'+str(p))
    save(tag+'-clang-flags.json',flags)
    def clang(p):
        log=logs/(p.name+'.log')
        with log.open('w') as f:rr=subprocess.run(flags+[str(p)],stdout=f,stderr=subprocess.STDOUT).returncode
        txt=log.read_text(errors='replace')
        return {'file':p.name,'sha256':sha(p),'exit':rr,'errors':len(re.findall(r'\berror:',txt)),'fatal':len(re.findall(r'fatal error:',txt))}
    with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool: tests=list(pool.map(clang,cpp))
    save(tag+'-clang-results.json',tests)
    stage['clang']={'total':len(tests),'failed':sum(x['exit']!=0 for x in tests),'errors':sum(x['errors'] for x in tests),'fatal':sum(x['fatal'] for x in tests),'infrastructure_failures':[x for x in tests if x['exit'] and x['errors']==0]}
    stage['metadata_sha']=sha(out/'data/Metadata/global-metadata.dat'); stage['finished']=time.time()
    save('status.json',result)
result['finished']=time.time(); result['git_status_after']=subprocess.check_output(['git','status','--porcelain'],cwd=repo,text=True)
save('status.json',result)
