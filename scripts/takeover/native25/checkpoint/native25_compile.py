from pathlib import Path
import json,os,subprocess,hashlib,shutil,time,re,sys,concurrent.futures
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');n=w/'.validation/native25-2026-10-01';r=n/'native24-restored';stage=n/'fresh';stage.mkdir(exist_ok=True)
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
for name,record in json.loads((r/'work/INPUT-SUPPORT-LOCK.json').read_text()).items():assert sha(r/'managed-support'/name)==record['sha256'],name
managed=stage/'managed';shutil.copytree(r/'managed-support',managed,dirs_exist_ok=True);shutil.copyfile(n/'native25-final1.dll',managed/'GodsPVZRuntime1.dll')
cmd=json.loads((r/'work/raw-fresh-command.json').read_text());new=[]
for a in cmd:
 if a.startswith('--assembly='):a='--assembly='+str(managed/Path(a.split('=',1)[1]).name)
 elif a.startswith('--generatedcppdir='):a='--generatedcppdir='+str(stage/'cpp')
 elif a.startswith('--symbols-folder='):a='--symbols-folder='+str(stage/'symbols')
 elif a.startswith('--data-folder='):a='--data-folder='+str(stage/'data')
 new.append(a)
for name in ['cpp','symbols','data']:(stage/name).mkdir(exist_ok=True)
(n/'IL2CPP-COMMAND.json').write_text(json.dumps(new,indent=2));start=time.time()
with (n/'il2cpp.log').open('w') as log:p=subprocess.run(new,cwd=old/'.validation/native21/replay/canary',env=dict(os.environ,PROJECT_DIR=str(old/'.validation/native21/replay/canary')),stdout=log,stderr=subprocess.STDOUT)
assert p.returncode==0,(n/'il2cpp.log').read_text()[-3000:]
generated={p.name:sha(p) for p in (stage/'cpp').iterdir() if p.is_file()};parent=json.loads((r/'GENERATED-SOURCE-MAP.json').read_text())['raw'];units=sorted((stage/'cpp').glob('*.cpp'));assert len(units)==264
conversion={'exit':0,'seconds':time.time()-start,'profile':'Historical unityaot-macos qualifier on Linux; not Unity iOS export','candidate_sha256':sha(n/'native25-final1.dll'),'generated_file_hashes':generated,'different_files_from_native24':[k for k in sorted(set(generated)|set(parent)) if generated.get(k)!=parent.get(k)],'metadata_sha256':sha(stage/'data/Metadata/global-metadata.dat')};(n/'IL2CPP-RESULT.json').write_text(json.dumps(conversion,indent=2));print('IL2CPP_PASS',json.dumps({k:v for k,v in conversion.items() if k!='generated_file_hashes'}),flush=True)
flags=json.loads((r/'work/raw-fresh-clang-flags.json').read_text());flags=[a.replace('/tmp/godspvz-native24-codespace-2026-10-01/fresh-raw/cpp',str(stage/'cpp')) for a in flags];(n/'CLANG-FLAGS.json').write_text(json.dumps(flags,indent=2));logs=stage/'clang';logs.mkdir(exist_ok=True)
def compile(p):
 with (logs/(p.name+'.log')).open('w') as log:rc=subprocess.run(flags+[str(p)],stdout=log,stderr=subprocess.STDOUT).returncode
 text=(logs/(p.name+'.log')).read_text(errors='replace');return {'file':p.name,'sha256':sha(p),'exit':rc,'errors':len(re.findall(r'\berror:',text)),'fatal':len(re.findall(r'fatal error:',text))}
start=time.time()
with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:results=list(pool.map(compile,units))
(n/'CLANG-RESULTS.json').write_text(json.dumps(results,indent=2));sys.path.insert(0,str(w/'scripts/takeover/native24'));import cpp_method_map as C
mapping=C.attribute(stage/'cpp',logs,{row['file']:row for row in results});(n/'CPP-METHODS.json').write_text(json.dumps(mapping,indent=2));previous=json.loads((r/'work/raw-CPP-METHODS.json').read_text());regressions=[];improvements=[]
for t,m in mapping['methods'].items():
 now=len(m['errors']);before=len(previous['methods'].get(t,{}).get('errors',[]))
 if now>before:regressions.append({'token':t,'before':before,'after':now})
 if now<before:improvements.append({'token':t,'before':before,'after':now})
assert not regressions,regressions;assert not mapping['methods']['0x06000338']['errors'];assert [m['token'] for m in improvements]==['0x06000338']
report={'compiler':subprocess.check_output(['clang++','--version'],text=True).splitlines()[0],'scope':'Linux syntax-only with locked Native18 export headers; not Apple build/link','TUs':len(results),'failed':sum(x['exit']!=0 for x in results),'errors':sum(x['errors'] for x in results),'fatal':sum(x['fatal'] for x in results),'seconds':time.time()-start,'mapping':mapping['summary'],'pass_to_fail_methods':regressions,'improved_methods':improvements,'target':mapping['methods']['0x06000338'],'cumulative_targets':{t:mapping['methods'][t] for t in ['0x06000339','0x06000348','0x0600034C']}}
(n/'CPP-STATUS.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2),flush=True)
