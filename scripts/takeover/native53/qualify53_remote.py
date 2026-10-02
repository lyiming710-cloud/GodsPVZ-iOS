from pathlib import Path
import subprocess,json,hashlib,sys,os
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native53-2026-10-02';prev=w/'.validation/native25-2026-10-01';candidate=n/'candidate1.dll';parent=w/'.validation/native52-2026-10-02/candidate1.dll'
def run(args,name,expected=0,env=None):
 p=subprocess.run(args,capture_output=True,text=True,env=env);(n/(name+'.log')).write_text(p.stdout+p.stderr);assert p.returncode==expected,(name,p.returncode,p.stdout+p.stderr);return p
run(['dotnet','/workspaces/GodsPVZ-native19/.validation/native22/bin/PatcherNative19.dll',str(candidate),str(n/'all-methods.json'),'export-all'],'export-all',env=dict(os.environ,GODSPVZ_RESOLVER=str(prev/'native24-restored/managed-support')))
run(['dotnet',str(prev/'inspector-bin/Inspector.dll'),str(candidate),str(n/'metadata.json')],'inspect')
run(['dotnet',str(prev/'declaration-bin/RepairNative24.dll'),'--declarations',str(candidate),str(n/'declarations.json'),str(prev/'native24-restored/managed-support')],'declarations')
run(['dotnet',str(prev/'declaration-bin/RepairNative24.dll'),'--declarations',str(parent),str(n/'parent-declarations.json'),str(prev/'native24-restored/managed-support')],'parent-declarations')
assert json.loads((n/'declarations.json').read_text())==json.loads((n/'parent-declarations.json').read_text())
sys.path.insert(0,str(w/'scripts/takeover/native40'));import verify_types as V
data=json.loads((n/'all-methods.json').read_text());out=[]
for m in data['methods']:
 try:out.append(V.verify(m,data['types']))
 except V.InvalidIL as e:out.append({'token':m['token'],'status':'FAIL','error':str(e)})
(n/'TYPED-RESULTS.json').write_text(json.dumps(out,indent=2));old={m['token']:m for m in json.loads((w/'.validation/native52-2026-10-02/TYPED-RESULTS.json').read_text())};targets={m['token'] for m in json.loads((n/'patch1.json').read_text())['methods']}
assert not [m for m in out if old[m['token']]['status']=='PASS' and m['status']!='PASS']
assert not [m for m in out if m['token'] in targets and m['status']!='PASS']

run(['dotnet','/workspaces/GodsPVZ-native19/.validation/native22/bin/PatcherNative19.dll',str(parent),str(n/'parent-all-methods.json'),'export-all'],'parent-export-all',env=dict(os.environ,GODSPVZ_RESOLVER=str(prev/'native24-restored/managed-support')))
pdata=json.loads((n/'parent-all-methods.json').read_text());parent_results=[]
for m in pdata['methods']:
 try:parent_results.append(V.verify(m,pdata['types']))
 except V.InvalidIL as e:parent_results.append({'token':m['token'],'status':'FAIL','error':str(e)})
(n/'PARENT-EXTENDED-TYPED-RESULTS.json').write_text(json.dumps(parent_results,indent=2))
parent_by={m['token']:m for m in parent_results}
assert all(m==parent_by[m['token']] for m in out if m['token'] not in targets),'Non-target typed result drift'
assert all(parent_by[m['token']]['status']=='PASS' for m in old.values() if m['status']=='PASS'),'Frozen pass regression in parent'
(n/'TYPED-NON-TARGET-ISOLATION.json').write_text(json.dumps({'methods':len(out)-len(targets),'equal':True,'frozen_pass_regression':False,'verifier_version':'Native40 managed-ref extension'},indent=2))

rows=[]
for label in ['native31','native32','native33','native34','native35','native36','native37','native38','native39','native40','native41','native42','native43','native44','native45','native46','native47','native48','native49','native50','native51','native52']:
 run(['dotnet',str(w/('.validation/'+label+'-2026-10-02/built/RuntimeFixture/RuntimeFixture.dll')),str(candidate),str(w/('.validation/'+label+'-2026-10-02/NATIVE-ORACLE.tsv')),str(n/(label+'.json'))],label);rows.append({'suite':label,'report':label+'.json'})
for name,exe in [('NATIVE30-CODEX',w/'.validation/native30-codex-2026-10-02/built/RuntimeFixture/RuntimeFixture.dll'),('NATIVE28',w/'.validation/native28-2026-10-01/fixture-bin/RuntimeFixture.dll'),('NATIVE27-VERIFIER',w/'.validation/native27-verifier-2026-10-01/fixture-bin/Native27Verifier.dll'),('NATIVE26',w/'.validation/native26-2026-10-01/fixture-bin/RuntimeFixture.dll'),('NATIVE25',prev/'fixture-bin/RuntimeFixture.dll'),('NATIVE24',prev/'previous-fixture-bin/Runtime.dll')]:
 run(['dotnet',str(exe),str(candidate),str(n/(name+'.json'))]+([str(w/'.validation/native27-2026-10-01/fixture-bin/RuntimeFixture.dll')] if name=='NATIVE27-VERIFIER' else []),name);rows.append({'suite':name,'report':name+'.json','sha256':hashlib.sha256((n/(name+'.json')).read_bytes()).hexdigest()})
(n/'REGRESSION.json').write_text(json.dumps({'pass':True,'suites':rows,'candidate_sha256':hashlib.sha256(candidate.read_bytes()).hexdigest()},indent=2))
for name,inp,outp in [('wrong-parent',candidate,n/'must-not-exist.dll'),('existing-output',parent,candidate)]:run(['dotnet',str(n/'built/Patcher/Patcher.dll'),str(inp),str(outp),str(n/(name+'.json'))],name,3)
assert not (n/'must-not-exist.dll').exists()
neg=[]
for mode in ['non-target','old-row']:
 outp=n/(mode+'-negative.dll');run(['dotnet',str(w/'.validation/native29-2026-10-01/negative-audit-bin/Mutate.dll'),str(candidate),str(outp),mode],mode+'-mutate')
 run(['dotnet',str(n/'built/Audit/Audit.dll'),str(parent),str(outp),str(n/'patch1.json'),str(n/(mode+'-must-not-pass.json'))],mode+'-audit-reject',2);assert not (n/(mode+'-must-not-pass.json')).exists();neg.append({'mode':mode,'audit_exit':2,'detected':True})
(n/'AUDIT-NEGATIVE.json').write_text(json.dumps(neg,indent=2))
print(json.dumps({'typed_pass':sum(m['status']=='PASS' for m in out),'typed_fail':sum(m['status']=='FAIL' for m in out),'target_typed_pass':len(targets),'declarations_equal':True,'regression_suites':rows,'audit_negatives':neg}))

e=json.loads((n/'NATIVE-EVIDENCE.json').read_text());e['candidate_input_sha256']=hashlib.sha256(parent.read_bytes()).hexdigest();e['diagnostics_baseline']='Native52 full compiler result; exact parent extraction';(n/'NATIVE-EVIDENCE.json').write_text(json.dumps(e,indent=2))
