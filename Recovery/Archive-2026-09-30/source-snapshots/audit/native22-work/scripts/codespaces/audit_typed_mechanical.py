"""Read-only proposals for narrow, type-preserving CIL encoding corrections.

No DLL writes. A method is eligible only if the entire reachable body passes the
typed subset verifier after proposed edits and contains no unresolved native hint.
This does not certify native semantic equivalence of otherwise damaged methods.
"""
import json,sys,re,copy,collections,hashlib
from pathlib import Path
from verify_native19_types import verify,InvalidIL
path=Path(sys.argv[1]);data=json.loads(path.read_text());types=data['types'];eligible=[];blocked=[];clean=[]
for original in data['methods']:
 d=copy.deepcopy(original);changes=[];issue=None
 hints=[x['operand'] for x in d['instructions'] if x['opcode']=='ldstr' and isinstance(x['operand'],str) and x['operand'].startswith(('Unmanaged memory load','Indirect jump','Method not found','Not implemented instruction'))]
 if hints:blocked.append({'token':d['token'],'method':d['name'],'reason':'unresolved native reconstruction','hints':hints});continue
 for attempt in range(24):
  try:verify(d,types);break
  except (InvalidIL,KeyError,ValueError,IndexError) as e:
   issue=str(e);match=re.search(r' IL_([0-9A-F]+):',issue)
   if not match:break
   index=next(n for n,x in enumerate(d['instructions']) if x['offset']==int(match[1],16));x=d['instructions'][index]
   if not index:break
   prior=d['instructions'][index-1]
   replacement=None;reason=None
   if x['opcode'] in ('ceq','cgt.un','cgt','clt','clt.un') and prior['opcode'] in ('ldc.i4.0','ldc.i4','ldc.i4.s') and (prior['opcode']=='ldc.i4.0' or prior['operand']==0):
    types_match=re.search(r'invalid comparison (.*), I4$',issue)
    if types_match:
     other=types_match[1]
     if other=='F':replacement=('ldc.r4',0.);reason='float zero in numeric comparison'
     elif x['opcode'] in ('ceq','cgt.un') and other in types and not types[other].get('value') and not types[other].get('byref') and not types[other].get('unbound'):replacement=('ldnull',None);reason='null in reference comparison'
   if x['opcode'] in ('call','callvirt','newobj') and x['operand']['args'] and prior['opcode'] in ('ldloca','ldloca.s','ldflda'):
    want=x['operand']['args'][-1]
    if want in ('UnityEngine.Vector2','UnityEngine.Vector3','UnityEngine.Vector4','UnityEngine.Quaternion','UnityEngine.Color'):
     actual=d['locals'][prior['operand']['index']] if prior['opcode'].startswith('ldloca') else prior['operand']['type']
     if actual==want and f'{want}& is not assignable to {want}' in issue:replacement=('ldloc' if prior['opcode'].startswith('ldloca') else 'ldfld',prior['operand']);reason='same struct value for by-value parameter'
   if replacement is None:break
   changes.append({'offset':prior['offset'],'before':copy.deepcopy(prior),'after':{'opcode':replacement[0],'operand':replacement[1]},'reason':reason})
   prior['opcode'],prior['operand']=replacement;issue=None
 else:issue='proposal limit exceeded'
 if issue:blocked.append({'token':d['token'],'method':d['name'],'reason':issue,'attempted_safe_edits':len(changes)})
 elif changes:eligible.append({'token':d['token'],'method':d['name'],'changes':changes})
 else:clean.append(d['token'])
result={'status':'READ_ONLY_PROPOSALS','source_spec_sha256':hashlib.sha256(path.read_bytes()).hexdigest(),'method_count':len(data['methods']),'already_typed_pass':len(clean),'eligible_methods':len(eligible),'eligible_edits':sum(len(m['changes']) for m in eligible),'eligible':eligible,'blocked':blocked}
path.with_suffix('.mechanical-audit.json').write_text(json.dumps(result,indent=2))
print(json.dumps({k:v for k,v in result.items() if k not in ('eligible','blocked')},indent=2))
