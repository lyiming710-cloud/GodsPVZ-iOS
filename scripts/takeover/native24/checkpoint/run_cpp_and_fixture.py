from pathlib import Path
import subprocess,json,hashlib,os,concurrent.futures,re,shutil,sys,time
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');dst=Path('/tmp/godspvz-native24-codespace-2026-10-01');review=old/'.validation/native23-independent-full-review'
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
result={'scope':'Actual fresh IL2CPP source; Linux Clang18, not Apple compilation','sets':{},'fixture':{}}
fixture=w/'scripts/takeover/native24/RuntimeFixture';fixture.mkdir(exist_ok=True)
shutil.copyfile(review/'runtime-fixture/Program.cs',fixture/'Program.cs')
project=(review/'runtime-fixture/Runtime.csproj').read_text().replace('/workspaces/GodsPVZ-native19/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll','$(MonoCecilPath)')
(fixture/'Runtime.csproj').write_text(project)
with (dst/'fixture-build.log').open('w') as log:r=subprocess.run(['dotnet','build',str(fixture/'Runtime.csproj'),'-c','Release','-o',str(dst/'fixture-bin'),'-p:MonoCecilPath='+str(old/'Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll')],stdout=log,stderr=subprocess.STDOUT)
assert r.returncode==0,(dst/'fixture-build.log').read_text()[-2500:]
with (dst/'fixture.log').open('w') as log:r=subprocess.run(['dotnet',str(dst/'fixture-bin/Runtime.dll'),str(dst/'candidate-1.dll'),str(dst/'RUNTIME-FIXTURE.json')],stdout=log,stderr=subprocess.STDOUT)
assert r.returncode==0,(dst/'fixture.log').read_text()[-2500:]
result['fixture']=json.loads((dst/'RUNTIME-FIXTURE.json').read_text());(dst/'CPP-FIXTURE-STATUS.json').write_text(json.dumps(result,indent=2))
sys.path.insert(0,str(w/'scripts/takeover/native24'));import cpp_method_map as C
for tag in ['baseline','candidate']:
 stage=dst/('fresh-'+tag);cpp=stage/'cpp';logs=stage/'clang';logs.mkdir(exist_ok=True)
 flags=json.loads((review/'baseline-clang-flags.json').read_text())
 flags=[a.replace(str(review/'baseline/cpp'),str(cpp)) for a in flags]
 (dst/(tag+'-fresh-clang-flags.json')).write_text(json.dumps(flags,indent=2))
 def compile(source):
  log=logs/(source.name+'.log')
  with log.open('w') as f:r=subprocess.run(flags+[str(source)],stdout=f,stderr=subprocess.STDOUT)
  text=log.read_text(errors='replace');return {'file':source.name,'sha256':sha(source),'exit':r.returncode,'errors':len(re.findall(r'\berror:',text)),'fatal':len(re.findall(r'fatal error:',text))}
 start=time.time();units=sorted(cpp.glob('*.cpp'))
 with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:tests=list(pool.map(compile,units))
 manifest={r['file']:r for r in tests};mapping=C.attribute(cpp,logs,manifest)
 (dst/(tag+'-CPP-METHODS.json')).write_text(json.dumps(mapping,indent=2));(dst/(tag+'-CLANG-RESULTS.json')).write_text(json.dumps(tests,indent=2))
 result['sets'][tag]={'TUs':len(tests),'failed':sum(x['exit']!=0 for x in tests),'errors':sum(x['errors'] for x in tests),'fatal':sum(x['fatal'] for x in tests),'seconds':time.time()-start,'mapped':mapping['summary'],'six_site_target_methods':{t:mapping['methods'].get(t) for t in ['0x06000339','0x06000348','0x0600034C']}}
 (dst/'CPP-FIXTURE-STATUS.json').write_text(json.dumps(result,indent=2));print(tag,json.dumps({k:v for k,v in result['sets'][tag].items() if k!='six_site_target_methods'}),flush=True)
print('FIXTURE_PASS',result['fixture']['positive_cases'],'MUTANTS',result['fixture']['mutants_detected'],flush=True)
