from pathlib import Path
import json,sys,copy,collections
w=Path('/workspaces/GodsPVZ-native24-codespace');base=w/'.validation/native26-2026-10-01';out=w/'.validation/native27-2026-10-01'
sys.path.insert(0,str(w/'scripts/codespaces'));import verify_native19_types as V
sys.path.insert(0,str(w/'scripts/takeover/native24'));import provenance as P
data=json.loads((base/'all-methods.json').read_text());types=data['types'];methods={m['token']:m for m in data['methods']};records=json.loads((out/'CLASSIFICATION.json').read_text())['records']
def isref(t):return t=='null' or t.endswith('[]') or(t in types and not types[t].get('value') and not types[t].get('byref') and not types[t].get('unbound'))
def zero(i):return i['opcode']=='ldc.i4.0' or i['opcode'] in ['ldc.i4','ldc.i4.s'] and i['operand']==0
def loc(i):return i['operand']['index'] if isinstance(i['operand'],dict) else int(i['opcode'].split('.')[-1])
queue=[];rejected=[];stats=collections.Counter()
for r in records:
 m=methods[r['token']];trial=copy.deepcopy(m);ins=trial['instructions'];throw_changes=[];null_changes=[]
 for index,i in enumerate(ins):
  if i['opcode']!='ret' or index<3:continue
  load=ins[index-1];store=ins[index-2];ctor=ins[index-3]
  if not(load['opcode'].startswith('ldloc') and not load['opcode'].startswith('ldloca') and store['opcode'].startswith('stloc') and loc(load)==loc(store)):continue
  ix=loc(load)
  if trial['locals'][ix]!='System.NullReferenceException' or ctor['opcode']!='newobj' or ctor['operand']['identity']!='System.Void System.NullReferenceException::.ctor()':continue
  # The sequence must be entered only at the ctor; no alternate jump can supply a different exception value.
  interiors={store['offset'],load['offset'],i['offset']};incoming=[]
  for j in ins:
   a=j['operand']
   if isinstance(a,dict):
    if a.get('target') in interiors:incoming.append(j['offset'])
    if any(t in interiors for t in a.get('targets',[])):incoming.append(j['offset'])
  if incoming:continue
  throw_changes.append({'offset':i['offset'],'opcode_before':'ret','opcode_after':'throw','ctor_offset':ctor['offset'],'exception_local':ix});i['opcode']='throw'
 for iteration in range(32):
  try:errors=P.verify_prov(trial,types)
  except Exception:break
  chosen=None
  for e in errors:
   if e['opcode'] not in ['ceq','cgt.un'] or not e['msg'].startswith('invalid comparison '):continue
   pair=e['msg'][len('invalid comparison '):].rsplit(', ',1)
   if len(pair)!=2:continue
   a,b=pair
   prov=e.get('prov_second','?') if b=='I4' and isref(a) else e.get('prov_top','?') if a=='I4' and isref(b) else '?'
   if P.prov_class(prov) not in P.TRUSTED:continue
   index=next(i for i,x in enumerate(ins) if x['offset']==e['offset'])
   if index and zero(ins[index-1]):chosen=(index-1,e,prov);break
  if chosen is None:break
  index,e,prov=chosen;i=ins[index];null_changes.append({'offset':i['offset'],'opcode_before':i['opcode'],'operand_before':i['operand'],'comparison_offset':e['offset'],'reference_provenance':prov});i['opcode']='ldnull';i['operand']=None
 if not throw_changes and not null_changes:continue
 try:result=V.verify(trial,types)
 except V.InvalidIL as e:result={'status':'FAIL','error':str(e)}
 eligible=result['status']=='PASS' and not m['handlers'] and not r['unmanaged_placeholder_offsets'] and result['unreachable']==0
 x={'token':r['token'],'method':m['name'],'null_sites':null_changes,'throw_sites':throw_changes,'cpp_errors_before':r['cpp_errors'],'type_trial':result,'E5':'NOT_PROVED','E6':'NOT_PROVED'}
 if eligible:queue.append(x)
 else:rejected.append(x)
 stats['trial_methods']+=1;stats['type_closed']+=result['status']=='PASS';stats['type_closed_with_placeholders']+=result['status']=='PASS' and bool(r['unmanaged_placeholder_offsets']);stats['type_closed_with_unreachable']+=result['status']=='PASS' and result['unreachable']>0
result={'scope':'Review-only in-memory recipes. Exception termination must be matched to native CFG per method; no global ret-to-throw and no DLL emitted','stats':dict(stats),'candidate_methods':len(queue),'candidate_null_sites':sum(len(m['null_sites']) for m in queue),'candidate_throw_sites':sum(len(m['throw_sites']) for m in queue),'candidates':sorted(queue,key=lambda m:(m['type_trial']['instructions'],m['token'])),'rejected':rejected};(out/'BATCH-RECIPE-QUEUE.json').write_text(json.dumps(result,indent=2));print(json.dumps({k:v for k,v in result.items() if k not in ['candidates','rejected']},indent=2));print(json.dumps(result['candidates'][:35],indent=2))
