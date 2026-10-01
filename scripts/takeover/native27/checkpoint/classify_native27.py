"""Native27 diagnostics taxonomy and conservative source-supported repair queue.
This creates review records and in-memory trials, never writes a candidate DLL.
"""
from pathlib import Path
import json,collections,re,hashlib,sys,copy,time
w=Path('/workspaces/GodsPVZ-native24-codespace');base=w/'.validation/native26-2026-10-01';out=w/'.validation/native27-2026-10-01';out.mkdir(exist_ok=True)
sys.path.insert(0,str(w/'scripts/codespaces'));import verify_native19_types as V
sys.path.insert(0,str(w/'scripts/takeover/native24'));import provenance as P
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
mapping=json.loads((base/'CPP-METHODS.json').read_text());typed={m['token']:m for m in json.loads((base/'TYPED-RESULTS.json').read_text())};data=json.loads((base/'all-methods.json').read_text());types=data['types'];methods={m['token']:m for m in data['methods']}
def cpp_class(msg):
 if 'comparison between pointer and integer' in msg:return 'reference_integer_comparison'
 if 'il2cpp_array_size_t' in msg:return 'array_index_type'
 if re.search(r'(Vector[234]|Rect|Quaternion|Color|Matrix4x4)_t',msg):return 'value_struct_shape'
 if 'no matching function' in msg or 'no viable overloaded' in msg or 'cannot initialize a parameter' in msg:return 'call_argument_or_generic'
 if 'integer to pointer' in msg or 'pointer to integer' in msg or 'cast from pointer to smaller type' in msg:return 'numeric_reference_mixing'
 if 'member reference' in msg or 'no member named' in msg:return 'receiver_member_shape'
 if 'return object' in msg or 'returned value' in msg:return 'return_type'
 if 'assigning to' in msg:return 'assignment_type'
 if 'operands to binary' in msg or 'to unary expression' in msg:return 'arithmetic_type'
 if 'cast' in msg or 'conversion' in msg:return 'cast_type'
 return 'other'
def typed_class(error):
 msg=re.split(r' IL_[0-9A-Fa-f]+: ',error,maxsplit=1)[-1]
 if 'unsupported' in msg:return 'verifier_unsupported'
 if any(s in msg for s in ['stack underflow','merge','EH','branch','outside method','MaxStack']):return 'control_stack_or_eh'
 if 'invalid comparison' in msg:return 'comparison_type'
 if 'is not assignable to' in msg:
  a,b=msg.split(' is not assignable to',1)
  if '&' in a:return 'managed_pointer_as_value'
  if 'List`1' in a or 'Enumerator' in a or 'List`1' in b:return 'generic_binding'
  if b.strip()=='System.Object' and a in ['I4','I8','I','F']:return 'numeric_to_object_local'
  return 'assignment_or_argument_type'
 if 'receiver' in msg:return 'receiver_or_generic_binding'
 if 'numeric' in msg or 'conversion' in msg:return 'arithmetic_or_conversion'
 return 'other'
counts=collections.Counter();category_methods=collections.defaultdict(set);records=[]
for token,m in mapping['methods'].items():
 fam=collections.Counter(cpp_class(e['message']) for e in m['errors']);counts.update(fam)
 for cat in fam:category_methods[cat].add(token)
 if m['errors']:
  body=methods[token];markers=[i['offset'] for i in body['instructions'] if i['opcode']=='ldstr' and isinstance(i['operand'],str) and i['operand'].startswith(('Unmanaged memory load:','Unmanaged memory store:','Unmanaged memory read:','Method not found @','Unknown call target operand:'))]
  records.append({'token':token,'method':body['name'],'cpp_file':m['file'],'cpp_errors':len(m['errors']),'cpp_symptom_categories':dict(fam),'first_typed_error':typed[token].get('error'),'typed_category':typed_class(typed[token].get('error','')) if typed[token]['status']=='FAIL' else 'PASS','locals':len(body['locals']),'object_locals':body['locals'].count('System.Object'),'instructions':len(body['instructions']),'handlers':len(body['handlers']),'unmanaged_placeholder_offsets':markers,'generic_object_locals':sum('System.Object>' in t for t in body['locals']),'root_cause_proven':False})
report={'input_sha256':sha(base/'native26-final1.dll'),'scope':'Diagnostic symptom classification; counts do not establish independent root causes or semantic restoration','cpp_total':mapping['summary']['total_errors'],'cpp_attributed_errors':sum(counts.values()),'cpp_errors_outside_game_methods':mapping['summary']['cpp_errors_outside_game_methods'],'included_file_errors':mapping['summary']['included_file_errors'],'methods_with_cpp_errors':len(records),'typed_fail':sum(t['status']=='FAIL' for t in typed.values()),'cpp_categories':{cat:{'errors':n,'methods':len(category_methods[cat])} for cat,n in counts.most_common()},'typed_first_error_categories':dict(collections.Counter(typed_class(t['error']) for t in typed.values() if t['status']=='FAIL')),'method_flags':{'with_object_locals':sum(r['object_locals']>0 for r in records),'with_unmanaged_placeholders':sum(bool(r['unmanaged_placeholder_offsets']) for r in records),'with_EH':sum(r['handlers']>0 for r in records)},'records':records}
assert report['cpp_attributed_errors']+report['cpp_errors_outside_game_methods']+report['included_file_errors']==report['cpp_total']
(out/'CLASSIFICATION.json').write_text(json.dumps(report,indent=2)+'\n');print('CLASSIFICATION',json.dumps({k:v for k,v in report.items() if k!='records'}),flush=True)
def isref(t):return t=='null' or t.endswith('[]') or (t in types and not types[t].get('value') and not types[t].get('byref') and not types[t].get('unbound'))
def zero(i):return i['opcode']=='ldc.i4.0' or i['opcode'] in ['ldc.i4','ldc.i4.s'] and i['operand']==0
queue=[];quarantine=[];observed=collections.Counter();start=time.time()
for seq,record in enumerate(records):
 token=record['token'];m=methods[token]
 if 'reference_integer_comparison' not in record['cpp_symptom_categories']:continue
 trial=copy.deepcopy(m);changes=[];source_errors=[];stop_reason='no trusted zero-comparison site'
 for iteration in range(32):
  try:errors=P.verify_prov(trial,types)
  except Exception as e:stop_reason='provenance unsupported: '+str(e);break
  chosen=None
  for error in errors:
   if error['opcode'] not in ['ceq','cgt.un'] or not error['msg'].startswith('invalid comparison '):continue
   pair=error['msg'][len('invalid comparison '):].rsplit(', ',1)
   if len(pair)!=2:continue
   second,top=pair
   if top=='I4' and isref(second):prov=error.get('prov_second','?')
   elif second=='I4' and isref(top):prov=error.get('prov_top','?')
   else:continue
   observed[P.prov_class(prov)]+=1
   if P.prov_class(prov) not in P.TRUSTED:continue
   index=next(i for i,x in enumerate(trial['instructions']) if x['offset']==error['offset']);before=trial['instructions'][index-1] if index else None
   # A literal directly preceding the comparison is independently identified; no LOCAL argument inference.
   if not before or not zero(before):continue
   chosen=(index-1,error,prov);break
  if not chosen:break
  index,error,prov=chosen;inst=trial['instructions'][index];changes.append({'offset':inst['offset'],'opcode_before':inst['opcode'],'operand_before':inst['operand'],'comparison_offset':error['offset'],'reference_provenance':prov});source_errors.append(error);inst['opcode']='ldnull';inst['operand']=None
 else:stop_reason='iteration limit reached'
 if not changes:
  quarantine.append({'token':token,'method':m['name'],'reason':stop_reason});continue
 try:result=V.verify(trial,types)
 except V.InvalidIL as e:result={'status':'FAIL','error':str(e)}
 # Classifier candidates need whole-method type closure and supported CFG, plus no placeholder/EH.
 eligible=result['status']=='PASS' and not trial['handlers'] and not record['unmanaged_placeholder_offsets'] and result['unreachable']==0
 item={'token':token,'method':m['name'],'sites':changes,'cpp_errors_before':record['cpp_errors'],'type_trial':result,'source_observations':source_errors,'E5':'NOT_PROVED','E6':'NOT_PROVED','status':'SITE_NATIVE_REVIEW_REQUIRED' if eligible else 'QUARANTINE','reason':None if eligible else 'Whole-method closure/EH/placeholder/reachability guard did not pass'}
 (queue if eligible else quarantine).append(item)
 if seq%75==0:print('QUEUE_PROGRESS',seq,len(queue),flush=True)
result={'scope':'Conservative source-supported queue; no DLL emitted; native site proof required before construction','candidate_methods':len(queue),'candidate_sites':sum(len(x['sites']) for x in queue),'candidates':sorted(queue,key=lambda x:(len(x['sites']),x['type_trial']['instructions'],x['token'])),'quarantine':quarantine,'observed_reference_provenance':dict(observed),'seconds':time.time()-start}
(out/'BATCH-QUEUE.json').write_text(json.dumps(result,indent=2)+'\n');print('QUEUE',json.dumps({k:v for k,v in result.items() if k not in ['candidates','quarantine']}),flush=True);print('FIRST_CANDIDATES',json.dumps(result['candidates'][:24],indent=2),flush=True)
