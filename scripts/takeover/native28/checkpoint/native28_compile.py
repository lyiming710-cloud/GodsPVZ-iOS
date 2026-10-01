from pathlib import Path
import json,os,subprocess,hashlib,shutil,time,re,sys,concurrent.futures
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');prev=w/'.validation/native27-2026-10-01';n=w/'.validation/native28-2026-10-01';r=w/'.validation/native25-2026-10-01/native24-restored';stage=n/'fresh';stage.mkdir(exist_ok=True)
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
for name,record in json.loads((r/'work/INPUT-SUPPORT-LOCK.json').read_text()).items():assert sha(r/'managed-support'/name)==record['sha256'],name
managed=stage/'managed';shutil.copytree(r/'managed-support',managed,dirs_exist_ok=True);shutil.copyfile(n/'native28-final1.dll',managed/'GodsPVZRuntime1.dll')
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
generated={p.name:sha(p) for p in (stage/'cpp').iterdir() if p.is_file()};parent=json.loads((prev/'IL2CPP-RESULT.json').read_text())['generated_file_hashes'];units=sorted((stage/'cpp').glob('*.cpp'));assert len(units)==264
conversion={'exit':0,'seconds':time.time()-start,'profile':'Historical unityaot-macos qualifier on Linux; not Unity iOS export','candidate_sha256':sha(n/'native28-final1.dll'),'generated_file_hashes':generated,'different_files_from_native27':[k for k in sorted(set(generated)|set(parent)) if generated.get(k)!=parent.get(k)],'metadata_sha256':sha(stage/'data/Metadata/global-metadata.dat')};(n/'IL2CPP-RESULT.json').write_text(json.dumps(conversion,indent=2));print('IL2CPP_PASS',json.dumps({k:v for k,v in conversion.items() if k!='generated_file_hashes'}),flush=True)
flags=json.loads((r/'work/raw-fresh-clang-flags.json').read_text());flags=[a.replace('/tmp/godspvz-native24-codespace-2026-10-01/fresh-raw/cpp',str(stage/'cpp')) for a in flags];(n/'CLANG-FLAGS.json').write_text(json.dumps(flags,indent=2));logs=stage/'clang';logs.mkdir(exist_ok=True)
def compile(p):
 with (logs/(p.name+'.log')).open('w') as log:rc=subprocess.run(flags+[str(p)],stdout=log,stderr=subprocess.STDOUT).returncode
 text=(logs/(p.name+'.log')).read_text(errors='replace');return {'file':p.name,'sha256':sha(p),'exit':rc,'errors':len(re.findall(r'\berror:',text)),'fatal':len(re.findall(r'fatal error:',text))}
start=time.time()
with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:results=list(pool.map(compile,units))
(n/'CLANG-RESULTS.json').write_text(json.dumps(results,indent=2));sys.path.insert(0,str(w/'scripts/takeover/native24'));import cpp_method_map as C
mapping=C.attribute(stage/'cpp',logs,{row['file']:row for row in results});(n/'CPP-METHODS.json').write_text(json.dumps(mapping,indent=2));previous=json.loads((prev/'CPP-METHODS.json').read_text());regressions=[];improvements=[]
for t,m in mapping['methods'].items():
 now=len(m['errors']);before=len(previous['methods'].get(t,{}).get('errors',[]))
 if now>before:regressions.append({'token':t,'before':before,'after':now})
 if now<before:improvements.append({'token':t,'before':before,'after':now})
targets={m['token'] for m in json.loads((n/'NATIVE-REVIEW.json').read_text())['approved_methods']};assert not regressions,regressions;assert all(not mapping['methods'][t]['errors'] for t in targets);assert {m['token'] for m in improvements}==targets
report={'compiler':subprocess.check_output(['clang++','--version'],text=True).splitlines()[0],'scope':'Linux syntax-only with locked Native18 export headers; not Apple build/link','TUs':len(results),'failed':sum(x['exit']!=0 for x in results),'errors':sum(x['errors'] for x in results),'fatal':sum(x['fatal'] for x in results),'seconds':time.time()-start,'mapping':mapping['summary'],'pass_to_fail_methods':regressions,'improved_methods':improvements,'targets':{t:mapping['methods'][t] for t in targets},'cumulative_targets':{t:mapping['methods'][t] for t in ['0x06000339','0x06000348','0x0600034C','0x06000338','0x060000D9']}}
(n/'CPP-STATUS.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2),flush=True)

changed=[]
for t,m in mapping['methods'].items():
 if t in targets:continue
 b=previous['methods'][t];a=(prev/'fresh/cpp'/b['file']).read_text().splitlines()[b['start']-1:b['end']];c=(stage/'cpp'/m['file']).read_text().splitlines()[m['start']-1:m['end']]
 if a!=c:changed.append(t)
assert not changed,changed
metadata_checks=[]
for tag,folder in [('parent',prev/'fresh/cpp'),('candidate',stage/'cpp')]:
 args=[a.replace(str(stage/'cpp'),str(folder)) for a in flags]
 with (n/(tag+'-metadatausage-clang.log')).open('w') as log:rc=subprocess.run(args+['-x','c++',str(folder/'Il2CppMetadataUsage.c')],stdout=log,stderr=subprocess.STDOUT).returncode
 text=(n/(tag+'-metadatausage-clang.log')).read_text(errors='replace');metadata_checks.append({'input':tag,'exit':rc,'errors':len(re.findall(r'\berror:',text)),'sha256':sha(folder/'Il2CppMetadataUsage.c')})
assert all(x['exit']==0 and x['errors']==0 for x in metadata_checks)
isolation={'non_target_mapped_cpp_methods_equal':len(mapping['methods'])-12,'differing_non_target_methods':changed,'metadata_usage_c_extra_checks':metadata_checks,'global_metadata_equal_parent':conversion['metadata_sha256']==json.loads((prev/'IL2CPP-RESULT.json').read_text())['metadata_sha256']};assert isolation['global_metadata_equal_parent'];(n/'CPP-ISOLATION.json').write_text(json.dumps(isolation,indent=2));print('CPP_ISOLATION',json.dumps(isolation),flush=True)
