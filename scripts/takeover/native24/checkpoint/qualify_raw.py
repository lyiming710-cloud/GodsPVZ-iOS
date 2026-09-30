from pathlib import Path
import subprocess,json,hashlib,shutil,os,time,concurrent.futures,re,sys
dst=Path('/tmp/godspvz-native24-codespace-2026-10-01');old=Path('/workspaces/GodsPVZ-native19');w=Path('/workspaces/GodsPVZ-native24-codespace');review=old/'.validation/native23-independent-full-review'
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
def canonical(body):
 if body is None:return None
 live=[i for i,x in enumerate(body['instructions']) if x['op']!='nop'];ids={v:i for i,v in enumerate(live)}
 def idx(v):
  if v<0:return v
  return next((ids[i] for i in live if i>=v),len(live))
 def operand(x):
  if isinstance(x,dict) and 'branch' in x:return {'branch':idx(x['branch'])}
  if isinstance(x,dict) and 'branches' in x:return {'branches':[idx(i) for i in x['branches']]}
  return x
 return {**body,'instructions':[{'op':body['instructions'][i]['op'],'operand':operand(body['instructions'][i]['operand'])} for i in live],'eh':[{**h,**{k:idx(h[k]) for k in ['ts','te','hs','he','filter']}} for h in body['eh']]}
b=json.loads((dst/'baseline-metadata.json').read_text());r=json.loads((dst/'raw-metadata.json').read_text());g=json.loads((dst/'candidate-metadata.json').read_text());targets={'0x06000339','0x06000348','0x0600034C'}
for k in ['assembly','module','mvid','tableCounts','types']:assert b[k]==r[k],k
assert not r['defects'];bm={m['token']:m for m in b['methods']};rm={m['token']:m for m in r['methods']};gm={m['token']:m for m in g['methods']};assert bm.keys()==rm.keys()
for t,m in bm.items():
 n=rm[t]
 for k in ['token','name','attributes','impl','signature']:assert m[k]==n[k],(t,k)
 if t not in targets:assert m==n,('NON_TARGET_DIFF',t)
 else:assert canonical(n['body'])==canonical(gm[t]['body']),('TARGET_SEMANTIC_SHAPE_DIFF',t)
declarations=[json.loads((dst/(x+'-declarations.json')).read_text()) for x in ['baseline','cecil','raw']];assert declarations[0]==declarations[1]==declarations[2],'declarations'
before=(dst/'inputs/baseline.dll').read_bytes();after=(dst/'raw-1.dll').read_bytes();repeat=(dst/'raw-2.dll').read_bytes();assert after==repeat and len(before)==len(after)
diff=[i for i,(a,b) in enumerate(zip(before,after)) if a!=b];report=json.loads((dst/'raw-1-report.json').read_text());assert diff==report['bytesChanged'] and len(diff)==6
allowed=set()
for site in report['rows']:allowed.update(range(site['fileOffset'],site['fileOffset']+len(bytes.fromhex(site['beforeBytes']))))
assert set(diff)<=allowed
fixture=json.loads((dst/'RAW-RUNTIME-FIXTURE.json').read_text());assert fixture['positive_cases']==24 and fixture['mutants_detected']
isolation={'input_sha256':sha(dst/'inputs/baseline.dll'),'output_sha256':sha(dst/'raw-1.dll'),'repeat_sha256':sha(dst/'raw-2.dll'),'changed_bytes':diff,'sites':report['rows'],'file_length_unchanged':True,'metadata_and_headers_byte_unchanged':True,'all_declarations_equal':True,'non_target_methoddefs_exact':len(bm)-3,'non_target_bodies_raw_and_normalized_exact':sum(m['body'] is not None for t,m in bm.items() if t not in targets),'target_bodies_equal_historical_cecil_modulo_nops':True,'padding_nops':8,'fixture_cases':24,'fixture_limit':'helper doubles for Device.TryPlacing and Plant.Awake; not Unity fake-null, Plant.Update or full game','qualification':'six-site limited candidate only; no whole-game fidelity or compile clearance'}
(dst/'RAW-ISOLATION.json').write_text(json.dumps(isolation,indent=2));print('RAW_ISOLATION_PASS',json.dumps(isolation),flush=True)
support=review/'baseline/managed';lock=json.loads((dst/'INPUT-SUPPORT-LOCK.json').read_text())
for name,value in lock.items():assert sha(support/name)==value['sha256'],name
stage=dst/'fresh-raw';stage.mkdir(exist_ok=True);managed=stage/'managed';shutil.copytree(support,managed,dirs_exist_ok=True);shutil.copyfile(dst/'raw-1.dll',managed/'GodsPVZRuntime1.dll')
original=json.loads((review/'baseline-conversion-command.json').read_text());cmd=[]
for arg in original:
 if arg.startswith('--assembly='):arg='--assembly='+str(managed/Path(arg.split('=',1)[1]).name)
 elif arg.startswith('--generatedcppdir='):arg='--generatedcppdir='+str(stage/'cpp')
 elif arg.startswith('--symbols-folder='):arg='--symbols-folder='+str(stage/'symbols')
 elif arg.startswith('--data-folder='):arg='--data-folder='+str(stage/'data')
 cmd.append(arg)
for name in ['cpp','symbols','data']:(stage/name).mkdir(exist_ok=True)
(dst/'raw-fresh-command.json').write_text(json.dumps(cmd,indent=2));start=time.time()
with (dst/'raw-fresh-il2cpp.log').open('w') as log:p=subprocess.run(cmd,cwd=old/'.validation/native21/replay/canary',env=dict(os.environ,PROJECT_DIR=str(old/'.validation/native21/replay/canary')),stdout=log,stderr=subprocess.STDOUT)
assert p.returncode==0,(dst/'raw-fresh-il2cpp.log').read_text()[-2000:]
generated={p.name:sha(p) for p in (stage/'cpp').iterdir() if p.is_file()};gold={p.name:sha(p) for p in (dst/'fresh-candidate/cpp').iterdir() if p.is_file()};cpp=list((stage/'cpp').glob('*.cpp'));assert len(cpp)==264
conversion={'exit':0,'profile':'unityaot-macos historical Linux qualifier; not Unity iOS export','seconds':time.time()-start,'TUs':len(cpp),'input_sha256':sha(dst/'raw-1.dll'),'metadata_sha256':sha(stage/'data/Metadata/global-metadata.dat'),'generated_file_hashes':generated,'differing_files_from_cecil_backend':[n for n in sorted(set(generated)|set(gold)) if generated.get(n)!=gold.get(n)]}
(dst/'RAW-CONVERSION.json').write_text(json.dumps(conversion,indent=2));print('RAW_CONVERSION_PASS','different generated files',len(conversion['differing_files_from_cecil_backend']),flush=True)
flags=json.loads((review/'baseline-clang-flags.json').read_text());flags=[a.replace(str(review/'baseline/cpp'),str(stage/'cpp')) for a in flags];(dst/'raw-fresh-clang-flags.json').write_text(json.dumps(flags,indent=2));logs=stage/'clang';logs.mkdir(exist_ok=True)
def compile(source):
 log=logs/(source.name+'.log')
 with log.open('w') as f:p=subprocess.run(flags+[str(source)],stdout=f,stderr=subprocess.STDOUT)
 text=log.read_text(errors='replace');return {'file':source.name,'sha256':sha(source),'exit':p.returncode,'errors':len(re.findall(r'\berror:',text)),'fatal':len(re.findall(r'fatal error:',text))}
start=time.time()
with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:results=list(pool.map(compile,sorted(cpp)))
sys.path.insert(0,str(w/'scripts/takeover/native24'));import cpp_method_map as C
mapping=C.attribute(stage/'cpp',logs,{x['file']:x for x in results});(dst/'raw-CPP-METHODS.json').write_text(json.dumps(mapping,indent=2));(dst/'raw-CLANG-RESULTS.json').write_text(json.dumps(results,indent=2))
gold_results=json.loads((dst/'candidate-CLANG-RESULTS.json').read_text());gold_by={x['file']:x for x in gold_results}
diffs=[{'file':x['file'],'raw':{k:x[k] for k in ['exit','errors','fatal']},'cecil':{k:gold_by[x['file']][k] for k in ['exit','errors','fatal']}} for x in results if any(x[k]!=gold_by[x['file']][k] for k in ['exit','errors','fatal'])]
summary={'compiler':subprocess.check_output(['clang++','--version'],text=True).splitlines()[0],'scope':'Linux Clang18; not Apple build','TUs':len(results),'errors':sum(x['errors'] for x in results),'failed':sum(x['exit']!=0 for x in results),'seconds':time.time()-start,'method_mapping':mapping['summary'],'error_count_changes_from_cecil_backend':diffs,'qualification':'Not all-C++ clear; full Unity/Xcode dispatch blocked by compile errors'}
(dst/'RAW-CPP-STATUS.json').write_text(json.dumps(summary,indent=2));print('RAW_CPP',json.dumps(summary),flush=True)
assert not diffs,'Raw backend compiler outcome regression/difference'
