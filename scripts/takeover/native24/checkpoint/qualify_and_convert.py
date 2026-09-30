from pathlib import Path
import subprocess,json,hashlib,shutil,os,zipfile,time
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');dst=Path('/tmp/godspvz-native24-codespace-2026-10-01')
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
data={t:json.loads((dst/(t+'-metadata.json')).read_text()) for t in ('baseline','historical','candidate')}
b,g,c=(data[t] for t in ('baseline','historical','candidate'));targets={'0x06000339','0x06000348','0x0600034C'}
assert c==g,'Complete independent metadata inspection differs from historical candidate'
for k in ('assembly','module','mvid','types'):assert b[k]==c[k],k
assert not c['defects'] and not b['defects']
bm={m['token']:m for m in b['methods']};cm={m['token']:m for m in c['methods']};assert set(bm)==set(cm)
raw_changed=[];normalized_changed=[];declaration_changed=[];body_sites=[]
for t,m in bm.items():
 n=cm[t]
 if m['rawIL']!=n['rawIL']:raw_changed.append(t)
 for k in ('token','name','attributes','impl','signature'):
  if m[k]!=n[k]:declaration_changed.append({'token':t,'field':k})
 if m['body']!=n['body']:
  normalized_changed.append(t)
  x,y=m['body'],n['body']
  for k in ('init','max','locals','eh'):assert x[k]==y[k],(t,k)
  assert len(x['instructions'])==len(y['instructions'])
  for index,(ii,jj) in enumerate(zip(x['instructions'],y['instructions'])):
   if ii!=jj:body_sites.append({'token':t,'instruction_index':index,'before':ii,'after':jj})
assert set(normalized_changed)==targets and len(body_sites)==6 and not declaration_changed
non_target_raw=[t for t in raw_changed if t not in targets]
report={'candidate_sha256':sha(dst/'candidate-1.dll'),'historical_sha256':sha(dst/'review/batch1.dll'),'repeat_sha256':sha(dst/'candidate-2.dll'),'full_file_equal_historical':(dst/'candidate-1.dll').read_bytes()==(dst/'review/batch1.dll').read_bytes(),'complete_inspection_equal_historical':True,'methods':len(bm),'method_bodies':sum(x['body'] is not None for x in bm.values()),'target_methods':normalized_changed,'sites':body_sites,'non_target_methods_normalized_unchanged':len(bm)-len(targets),'non_target_bodies_normalized_unchanged':sum(m['body'] is not None for t,m in bm.items() if t not in targets),'non_target_raw_changed':non_target_raw,'table_differences':{k:{'baseline':v,'candidate':c['tableCounts'][k]} for k,v in b['tableCounts'].items() if v!=c['tableCounts'][k]},'mvid_types_fields_and_method_declarations_unchanged':True,'non_target_raw_bytes_unchanged_claim':not non_target_raw,'qualification':'Pinned six-site materialization/isolation validated; whole method/game fidelity and all-C++ clearance NOT granted'}
assert report['candidate_sha256']==report['historical_sha256']==report['repeat_sha256']=='4d970f5ed7e63dd42815c3c843ce51918328ff790fa5d1eb77e2a80e6f3b0a1b'
(dst/'ISOLATION.json').write_text(json.dumps(report,indent=2));print(json.dumps({k:v for k,v in report.items() if k!='sites'},indent=2),flush=True)
support=old/'.validation/native23-independent-full-review/baseline/managed';status=json.loads((dst/'review/status.json').read_text());expected=status['stages']['baseline']['assemblies']
for name,want in expected.items():assert sha(support/name)==want,name
assert sha(support/'Unity.VisualScripting.Core.exe')=='16cabf56b65cd1e6c09338d39af27dce42377dc4d986c42cdfd9f91cc496a4fb'
conversion={'stages':{},'profile':'unityaot-macos exact historical qualifier replay, not Unity iOS export','compiler':subprocess.check_output(['clang++','--version'],text=True).splitlines()[0],'support':{p.name:{'bytes':p.stat().st_size,'sha256':sha(p)} for p in support.iterdir() if p.suffix in ('.dll','.exe')},'original_worktree_unchanged':subprocess.check_output(['git','-C',str(old),'status','--porcelain'],text=True)}
(dst/'INPUT-SUPPORT-LOCK.json').write_text(json.dumps(conversion['support'],indent=2))
original=json.loads((old/'.validation/native23-independent-full-review/baseline-conversion-command.json').read_text())
for tag,dll in [('baseline',dst/'inputs/baseline.dll'),('candidate',dst/'candidate-1.dll')]:
 stage=dst/('fresh-'+tag);stage.mkdir(exist_ok=True);managed=stage/'managed';shutil.copytree(support,managed,dirs_exist_ok=True);shutil.copyfile(dll,managed/'GodsPVZRuntime1.dll')
 cmd=[]
 for arg in original:
  if arg.startswith('--assembly='):arg='--assembly='+str(managed/Path(arg.split('=',1)[1]).name)
  elif arg.startswith('--generatedcppdir='):arg='--generatedcppdir='+str(stage/'cpp')
  elif arg.startswith('--symbols-folder='):arg='--symbols-folder='+str(stage/'symbols')
  elif arg.startswith('--data-folder='):arg='--data-folder='+str(stage/'data')
  cmd.append(arg)
 for name in ('cpp','symbols','data'):(stage/name).mkdir(exist_ok=True)
 (dst/(tag+'-fresh-command.json')).write_text(json.dumps(cmd,indent=2))
 started=time.time()
 with (dst/(tag+'-fresh-il2cpp.log')).open('w') as log:r=subprocess.run(cmd,cwd=old/'.validation/native21/replay/canary',env=dict(os.environ,PROJECT_DIR=str(old/'.validation/native21/replay/canary')),stdout=log,stderr=subprocess.STDOUT)
 assert r.returncode==0,(tag,(dst/(tag+'-fresh-il2cpp.log')).read_text()[-3000:])
 previous=old/'.validation/native23-independent-full-review'/('baseline' if tag=='baseline' else 'batch1')
 generated={p.name:sha(p) for p in (stage/'cpp').iterdir() if p.is_file()};historical={p.name:sha(p) for p in (previous/'cpp').iterdir() if p.is_file()}
 assert len(list((stage/'cpp').glob('*.cpp')))==264
 diffs=[name for name in sorted(set(generated)|set(historical)) if generated.get(name)!=historical.get(name)]
 conversion['stages'][tag]={'exit':r.returncode,'seconds':time.time()-started,'input_sha256':sha(dll),'cpp_TUs':264,'files_generated':len(generated),'historical_generated_file_differences':diffs,'all_generated_hashes_equal':not diffs,'metadata_sha256':sha(stage/'data/Metadata/global-metadata.dat'),'historical_metadata_sha256':sha(previous/'data/Metadata/global-metadata.dat'),'generated_file_hashes':generated}
 (dst/'FRESH-CONVERSION.json').write_text(json.dumps(conversion,indent=2));print('FRESH_CONVERSION',tag,'PASS','historical_diffs',len(diffs),flush=True)
 assert not diffs and conversion['stages'][tag]['metadata_sha256']==conversion['stages'][tag]['historical_metadata_sha256'],tag+' replay identities differ'
print('FRESH_REPLAY_IDENTITIES_PASS',flush=True)
