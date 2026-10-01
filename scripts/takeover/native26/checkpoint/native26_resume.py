from pathlib import Path
import json,hashlib,subprocess,sys,os,shutil,struct,gc
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');prev=w/'.validation/native25-2026-10-01';n=w/'.validation/native26-2026-10-01';r=prev/'native24-restored'
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
def run(args,name,env=None):
 with (n/(name+'.log')).open('w') as log:rc=subprocess.run(args,stdout=log,stderr=subprocess.STDOUT,env=env).returncode
 assert rc==0,(name,rc,(n/(name+'.log')).read_text()[-3000:])
fixture=json.loads((n/'RUNTIME-FIXTURE.json').read_text());assert fixture['positive_pass'] and fixture['mutants_detected']
b=json.loads((prev/'final-metadata.json').read_text());a=json.loads((n/'final-metadata.json').read_text());base=json.loads((r/'work/baseline-metadata.json').read_text());target='0x060000D9'
for k in ['assembly','module','mvid','tableCounts','types']:assert a[k]==b[k]==base[k],k
assert not a['defects'];bm={m['token']:m for m in b['methods']};am={m['token']:m for m in a['methods']};orig={m['token']:m for m in base['methods']};assert bm.keys()==am.keys()
for t,m in bm.items():
 if t!=target:assert m==am[t],('non-target difference',t)
 else:
  for k in ['token','name','attributes','impl','signature']:assert m[k]==am[t][k],k
cumulative={'0x06000339','0x06000348','0x0600034C','0x06000338',target}
for t,m in orig.items():
 if t not in cumulative:assert m==am[t],('cumulative difference',t)
before=(prev/'native25-final1.dll').read_bytes();after=(n/'native26-final1.dll').read_bytes();assert len(before)==len(after);patch=json.loads((n/'patch-final1.json').read_text());diff=[i for i,(x,y) in enumerate(zip(before,after)) if x!=y];assert diff==patch['positions']
run(['dotnet',str(prev/'declaration-bin/RepairNative24.dll'),'--declarations',str(n/'native26-final1.dll'),str(n/'declarations.json'),str(r/'managed-support')],'declarations');assert json.loads((n/'declarations.json').read_text())==json.loads((prev/'declarations.json').read_text())
counts={'incremental_non_target_methoddefs_exact':len(bm)-1,'incremental_non_target_bodies_raw_exact':sum(m['body'] is not None for t,m in bm.items() if t!=target),'cumulative_non_target_methoddefs_exact':len(bm)-5,'cumulative_non_target_bodies_raw_exact':sum(m['body'] is not None for t,m in bm.items() if t not in cumulative)}
del b,a,base,bm,am,orig;gc.collect()
sys.path.insert(0,str(w/'scripts/codespaces'));import verify_native19_types as V
data=json.loads((n/'all-methods.json').read_text());results=[]
for m in data['methods']:
 try:result=V.verify(m,data['types'])
 except V.InvalidIL as e:result={'token':m['token'],'method':m['name'],'status':'FAIL','error':str(e)}
 results.append(result)
(n/'TYPED-RESULTS.json').write_text(json.dumps(results,indent=2));previous={row['token']:row for row in json.loads((prev/'TYPED-RESULTS.json').read_text())};rows={row['token']:row for row in results};regressions=[t for t in rows if previous[t]['status']=='PASS' and rows[t]['status']=='FAIL'];improvements=[t for t in rows if previous[t]['status']=='FAIL' and rows[t]['status']=='PASS'];assert not regressions and improvements==[target]
fields_file=old/'.validation/native22/pcnative/native-fields.json';assert sha(fields_file)=='8273b66712f7fd64732a4fed64c453c2e71934d2cab329937f9dadaf57eef110';fields=json.loads(fields_file.read_text());selected=[f for f in fields if f['namespace']=='' and ((f['type']=='Device' and f['name'] in ['fX','fY','fW','fD'])or(f['type']=='AttackRange' and f['name']=='rangeType'))];assert {f['name']:f['offset'] for f in selected}=={'fX':56,'fY':60,'fW':72,'fD':76,'rangeType':56}
scope={'__name__':'native26_reference'};exec((w/'scripts/codespaces/native23_native.py').read_text(),scope);pc=old/'.validation/native22/pcnative/GameAssembly.dll';assert sha(pc)=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d';pe=scope['PE'](pc);offset=pe.va_to_offset(0x1815a7a08);constant=pe.data[offset:offset+4];assert struct.unpack('<f',constant)[0]==.5
evidence=json.loads((prev/'NATIVE-EVIDENCE.json').read_text());evidence['methods']=[m for m in evidence['methods'] if int(m['record']['token'],16)==0x060000D9];evidence.update({'field_offsets_scope':'Historical native field offset extraction, current locked file verified; not newly re-extracted','native_fields_sha256':sha(fields_file),'field_offsets':selected,'constant':{'va':'0x1815a7a08','bytes':constant.hex(),'value':.5}});(n/'NATIVE-EVIDENCE.json').write_text(json.dumps(evidence,indent=2));shutil.copyfile(prev/'native-060000D9.bin',n/'native-060000D9.bin');shutil.copyfile(prev/'native-060000D9.asm',n/'native-060000D9.asm')
report={'candidate_sha256':sha(n/'native26-final1.dll'),'repeat_sha256':sha(n/'native26-final2.dll'),'changed_bytes':len(diff),'incremental_non_target_methoddefs_exact':counts['incremental_non_target_methoddefs_exact'],'incremental_non_target_bodies_raw_exact':counts['incremental_non_target_bodies_raw_exact'],'cumulative_non_target_methoddefs_exact':counts['cumulative_non_target_methoddefs_exact'],'cumulative_non_target_bodies_raw_exact':counts['cumulative_non_target_bodies_raw_exact'],'complete_declarations_unchanged':True,'file_length_RVA_metadata_unchanged':True,'typed_pass':sum(x['status']=='PASS' for x in results),'typed_fail':sum(x['status']=='FAIL' for x in results),'pass_to_fail':regressions,'fail_to_pass':improvements,'target_typed':rows[target],'new_fixture_cases':255,'new_behavioral_mutants_detected':len(fixture['mutants']),'inherited_fixture_cases':70,'behavior_scope':fixture['scope']};(n/'QUALIFICATION.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
