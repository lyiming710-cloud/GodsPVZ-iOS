"""Linux Clang syntax preflight with exact CI-exported Unity headers; not Apple acceptance."""
from pathlib import Path
import subprocess,json,sys,concurrent.futures
root=Path(__file__).resolve().parents[2]
export=root/'.validation/xcode-native18/expanded/xcode/Il2CppOutputProject'
lib=export/'IL2CPP/libil2cpp';cpp=Path(sys.argv[1]) if len(sys.argv)>1 else export/'Source/il2cppOutput'
out=Path(sys.argv[2]) if len(sys.argv)>2 else root/'.validation/native19/cpp-preflight';out.mkdir(parents=True,exist_ok=True)
assert (lib/'pch/pch-cpp.hpp').exists(),lib
includes=[cpp,lib/'pch',lib,export/'IL2CPP/external/baselib/Include',export/'IL2CPP/external/baselib/Platforms/Linux/Include']
common=['clang++','-std=c++11','-fsyntax-only','-ferror-limit=0','-Wno-tautological-compare','-Wno-unused-value','-Wno-invalid-noreturn','-DRUNTIME_IL2CPP','-DIL2CPP_MONO_DEBUGGER_DISABLED','-DIL2CPP_DEBUG=0','-DNDEBUG','-DBASELIB_INLINE_NAMESPACE=il2cpp_baselib']+['-I'+str(p) for p in includes]
def run(p):
 r=subprocess.run(common+[str(p)],stdout=subprocess.PIPE,stderr=subprocess.STDOUT,text=True)
 (out/(p.name+'.log')).write_text(r.stdout)
 return {'file':p.name,'exit':r.returncode,'errors':[l for l in r.stdout.splitlines() if ': error:' in l]}
files=sorted(cpp.glob('GodsPVZRuntime1*.cpp'))
if not files:raise SystemExit('FAIL: no GodsPVZRuntime1 translation units found in '+str(cpp))
with concurrent.futures.ThreadPoolExecutor(max_workers=2) as pool:
 results=list(pool.map(run,files))
(out/'results.json').write_text(json.dumps(results,indent=2))
(out/'invocation.json').write_text(json.dumps({'source':str(cpp),'headers':str(lib),'command':common,'files':len(files)},indent=2))
for r in results:print(r['file'],r['exit'],len(r['errors']),*r['errors'][:2],sep=' | ',flush=True)
print('LINUX_SYNTAX_TOTAL',sum(len(r['errors']) for r in results),flush=True)
raise SystemExit(any(r['exit'] for r in results))
