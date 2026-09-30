"""Record complete game-shard diagnostics even when Clang rejects the candidate."""
from pathlib import Path
import subprocess,json,hashlib,re
root=Path(__file__).resolve().parents[2];base=root/'.validation/native20';replay=base/'replay';out=base/'cpp-final73';out.mkdir(exist_ok=True)
candidate=json.loads((replay/'candidate.json').read_text());assert candidate['status']=='CONVERSION_ONLY'
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
assert sha(replay/'native20-linked.dll')==candidate['outputs']['linked']
cpp=Path(candidate['cpp_root']);before={p.name:sha(p) for p in cpp.glob('GodsPVZRuntime1*.cpp')};assert len(before)==candidate['cpp_files']==21
with (base/'cpp-final73.log').open('w') as f:r=subprocess.run(['python3',str(root/'scripts/codespaces/cpp_syntax_preflight.py'),str(cpp),str(out)],cwd=root,stdout=f,stderr=subprocess.STDOUT)
subprocess.run(['python3',str(root/'scripts/codespaces/analyze_cpp_failures.py'),str(out)],cwd=root,check=True,stdout=subprocess.DEVNULL)
assert before=={p.name:sha(p) for p in cpp.glob('GodsPVZRuntime1*.cpp')},'generated C++ changed during compilation'
results=json.loads((out/'results.json').read_text());analysis=json.loads((out/'method-errors.json').read_text());spec=json.loads((replay/'native20-linked.dll.targets.json').read_text());definitions=[]
for p in cpp.glob('GodsPVZRuntime1*.cpp'):
 for line in p.read_text(encoding='utf-8-sig').splitlines():
  if line.startswith('IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR') and not line.rstrip().endswith(';'):
   m=re.search(r'\b(\w+_m[0-9A-F]+)\s*\(',line)
   if m:definitions.append((m[1],re.findall(r'([\w:*]+)\s+___\d+_',line)))
targets=[]
for d in spec['methods']:
 prefix=d['owner'].rsplit('/',1)[-1].rsplit('.',1)[-1]+'_'+d['name'].split('::')[1].split('(')[0].replace('.','_')+'_m'
 primitive={'System.Boolean':'bool','System.Int32':'int32_t','System.UInt32':'uint32_t','System.Single':'float','System.Double':'double','System.String':'String_t*','System.Char':'Il2CppChar','System.Int64':'int64_t','System.UInt64':'uint64_t'}
 matches=[name for name,args in definitions if name.startswith(prefix) and len(args)==len(d['args']) and all(want not in primitive or actual==primitive[want] for actual,want in zip(args,d['args']))];assert len(matches)==1,(d['name'],matches)
 errors=[e for e in analysis['records'] if e['method']==matches[0]]
 targets.append({'token':d['token'],'method':d['name'],'cpp_symbol':matches[0],'diagnostics':len(errors)})
summary={'status':'FULL_GAME_CPP_PASS' if r.returncode==0 else 'FULL_GAME_CPP_FAIL','scope':'21 GodsPVZRuntime1 translation units; Linux Clang with exact CI Unity headers; not Apple linker/device acceptance','candidate':candidate['outputs'],'diagnostics':sum(len(x['errors']) for x in results),'associated_methods':analysis['methods'],'target_methods':targets,'generated_sha256':before}
(out/'qualification.json').write_text(json.dumps(summary,indent=2));print(json.dumps({k:v for k,v in summary.items() if k not in ('target_methods','generated_sha256')},indent=2));print('TARGET_CPP_DIAGNOSTICS',sum(t['diagnostics'] for t in targets),'TARGETS',len(targets))
raise SystemExit(r.returncode)
