from pathlib import Path
import json,sys,collections,subprocess,os,types,copy,hashlib
sys.dont_write_bytecode=True
repo=Path('/workspaces/GodsPVZ-native19');base=repo/'.validation/native22';dst=repo/'.validation/native23-independent-full-review'
sys.path.insert(0,str(base));sys.path.insert(1,str(repo/'scripts/codespaces'))
import native23_prov as P, native23_afam3 as C, native23_verify as V
result={}
local_source=P.__file__;tracked=repo/'scripts/codespaces/native23_prov.py'
result['provenance_source']={'executed':local_source,'sha256':hashlib.sha256(Path(local_source).read_bytes()).hexdigest(),'equals_tracked':Path(local_source).read_bytes()==tracked.read_bytes(),'equals_commit':tracked.read_bytes()==subprocess.check_output(['git','show','HEAD:scripts/codespaces/native23_prov.py'],cwd=repo)}
T={'Owner':{'baseType':'System.Object'},'GameObject':{'baseType':'System.Object'},'System.Object':{},'System.Int32':{'value':True},'System.Boolean':{'value':True},'System.String':{},'System.Void':{'value':True}}
def body(name,ops,locals=(),args=()):
 return {'token':'0x06000001','name':name,'owner':'Owner','hasThis':False,'args':list(args),'locals':list(locals),'ret':'System.Void','handlers':[],'maxStack':8,'instructions':[{'offset':2*n,'opcode':op,'operand':arg} for n,(op,arg) in enumerate(ops)]}
field={'owner':'Owner','type':'GameObject','identity':'GameObject Owner::f'}
cases=[
body('invalid local receiver laundered as FIELD',[('ldc.i4.1',None),('stloc',{'index':0}),('ldloc',{'index':0}),('ldfld',field),('ldc.i4.0',None),('ceq',None),('pop',None),('ret',None)],locals=['Owner']),
body('invalid starg laundered as ARG',[('ldc.i4.1',None),('starg',{'index':0}),('ldarg',{'index':0}),('ldc.i4.0',None),('ceq',None),('pop',None),('ret',None)],args=['GameObject']),
body('invalid call argument laundered as FIELD',[('ldc.i4.1',None),('call',{'owner':'Owner','hasThis':False,'args':['System.String'],'ret':'Owner','name':'GetOwner','identity':'Owner Owner::GetOwner(System.String)'}),('ldfld',field),('ldc.i4.0',None),('ceq',None),('pop',None),('ret',None)]),
body('invalid merge retains accepted predecessor',[('ldarg',{'index':0}),('brtrue',{'target':8}),('ldarg',{'index':1}),('br',{'target':10}),('ldc.i4.1',None),('ldfld',field),('ldc.i4.0',None),('ceq',None),('pop',None),('ret',None)],args=['System.Boolean','Owner'])]
result['counterexamples']=[{'case':d['name'],'errors':P.verify_prov(d,T),'solver':C.solve(d,T)} for d in cases]
source=Path(local_source).read_text()
instrumented=source.replace("        if opname.startswith('ld'):","        if opname.startswith('ld'):")
needle="            if opname.startswith('ld'):\n                kind = 'SFIELD'"
assert needle in source
instrumented=source.replace(needle,"            if opname.startswith('ld'):\n                TRACE.append({'offset':x['offset'],'opcode':opname,'receiver':rp})\n                kind = 'SFIELD'")
ns={'__name__':'review_instrumented','TRACE':[]};exec(compile(instrumented,'review-provenance-instrumentation','exec'),ns)
data=json.loads((dst/'all-methods.batch1.json').read_text());methods={m['token']:m for m in data['methods']}
accepted=json.loads((base/'out/afam3-accepted.json').read_text());lower=[];counts=collections.Counter();rx=collections.Counter();error_count=0
for tok,rec in accepted.items():
 m=methods[tok];ns['TRACE'].clear();errs=ns['verify_prov'](m,data['types']);tr=collections.defaultdict(set)
 for t in ns['TRACE']:tr[t['offset']].add(t['receiver'])
 ix={x['offset']:i for i,x in enumerate(m['instructions'])}
 for s in rec['sites']:
  counts[s['prov_class']]+=1;k=ix[s['flip_offset']];prev=m['instructions'][k-1]
  if prev['opcode'] in ('ldfld','ldflda'):
   receivers=tr[prev['offset']]
   for r in receivers:rx[(r or '<none>').split(':')[0]]+=1
   if any(r and r.startswith('LOCAL:') for r in receivers):
    lower.append({'token':tok,'name':m['name'],'site':s['flip_offset'],'field_load':prev['offset'],'receiver_provenance':sorted(receivers),'method_existing_errors':len(errs)})
result['actual_accepted_analysis']={'methods':len(accepted),'sites':sum(counts.values()),'provenance_counts':dict(counts),'direct_field_receiver_observations':dict(rx),'accepted_direct_local_receiver_sites':len(lower),'examples':lower[:25],'all_direct_local_receiver_sites':lower,'note':'Instrumented original analyzer; counts direct predecessor field loads only, not a replacement proof. LOCAL dependency is unproved rather than necessarily wrong.'}
typed={t:V.verify_all(str(dst/('all-methods.'+t+'.json'))) for t in ('baseline','batch1','batch2','batch2a')}
result['typed']={t:{'methods':len(v),'fails':sum(bool(x) for x in v.values()),'crashes':sum(bool(x) and x.startswith('CRASH') for x in v.values())} for t,v in typed.items()}
for t in ('batch1','batch2','batch2a'):
 inp='baseline' if t=='batch1' else 'batch1'
 result['typed'][t]['fail_to_pass']=[k for k in typed[t] if typed[inp][k] and not typed[t][k]]
 result['typed'][t]['pass_to_fail']=[k for k in typed[t] if not typed[inp][k] and typed[t][k]]
json.dump(typed,open(dst/'typed-method-results.json','w'),indent=2)
neg=[]
b0=json.loads((dst/'all-methods.baseline.json').read_text());b1=data
by0={m['token']:m for m in b0['methods']};by1={m['token']:m for m in b1['methods']}
for edit in json.loads((base/'edits-batch1.json').read_text())['edits']:
 tok=edit['token'];bm=by0[tok];cm=copy.deepcopy(by1[tok]);k=next(i for i,x in enumerate(bm['instructions']) if x['offset']==edit['offset'])
 # Preserve candidate branch/offset references while reverting this opcode/operand.
 cm['instructions'][k]['opcode']=bm['instructions'][k]['opcode'];cm['instructions'][k]['operand']=bm['instructions'][k]['operand']
 outcome=V.verify_all({'methods':[cm],'types':b1['types']})[tok]
 neg.append({'edit':edit,'failure':outcome})
result['batch1_single_revert_negatives']=neg
# Actual baseline instruction contract test: attempting to replace a ret with ldnull must be rejected.
candidate=next(m for m in b0['methods'] if m['instructions'][-1]['opcode']=='ret' and not m['handlers'])
bad={'edits':[{'token':candidate['token'],'offset':candidate['instructions'][-1]['offset'],'action':'to_ldnull'}]}
(dst/'wrong-shape-edits.json').write_text(json.dumps(bad))
with (dst/'wrong-shape-patch.log').open('w') as f:rc=subprocess.run(['dotnet',str(dst/'patcher-bin/RepairNative23.dll'),str(base/'input/GodsPVZRuntime1.baseline.dll'),str(dst/'wrong-shape.dll'),str(dst/'wrong-shape-edits.json')],stdout=f,stderr=subprocess.STDOUT).returncode
result['wrong_shape_contract_probe']={'expected':'reject with no file','exit':rc,'written':(dst/'wrong-shape.dll').exists(),'token':candidate['token'],'old_opcode':'ret','requested_action':'to_ldnull'}
# Re-execute the committed counter suites, retaining actual process exit separately from text.
env=dict(os.environ,PYTHONDONTWRITEBYTECODE='1',PYTHONPATH=str(base))
for script in ('native23_counter.py','native23_provcheck.py','native23_afam3_check.py'):
 with (dst/(script+'.log')).open('w') as f:rc=subprocess.run(['python3',str(repo/'scripts/codespaces'/script)],cwd=base,env=env,stdout=f,stderr=subprocess.STDOUT).returncode
 result.setdefault('original_suite_exits',{})[script]=rc
(dst/'analysis.json').write_text(json.dumps(result,indent=2))
print(json.dumps({'analysis_done':True,'typed':result['typed'],'direct_local_receiver_sites':len(lower)}))
