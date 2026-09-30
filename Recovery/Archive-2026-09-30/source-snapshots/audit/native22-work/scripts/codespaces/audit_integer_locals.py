"""Read-only candidates: object locals populated with integer values.

Only I4/I8 stores can propose Int32/Int64. Conflicting definitions or uses must
fail whole reachable-body verification. Native reconstruction hints are blocked.
No DLL is written by this analysis.
"""
import copy,json,re,sys,hashlib,collections
from pathlib import Path
from verify_native19_types import verify,InvalidIL
path=Path(sys.argv[1]);data=json.loads(path.read_text());types=data['types'];eligible=[];blocked=[]
for original in data['methods']:
    d=copy.deepcopy(original);changes=[];locals_changed=[];issue=None
    if any(x['opcode']=='ldstr' and isinstance(x['operand'],str) and x['operand'].startswith(('Unmanaged memory load','Indirect jump','Method not found','Not implemented instruction')) for x in d['instructions']):
        continue
    for attempt in range(100):
        try:checked=verify(d,types);break
        except (InvalidIL,KeyError,ValueError,IndexError) as error:
            issue=str(error);match=re.search(r' IL_([0-9A-F]+):',issue)
            if not match:break
            index=next(n for n,x in enumerate(d['instructions']) if x['offset']==int(match[1],16));x=d['instructions'][index]
            if x['opcode'].startswith('stloc'):
                local=x['operand']['index'] if isinstance(x['operand'],dict) else int(x['opcode'].split('.')[-1])
                match_type=re.search(r': (I4|I8) is not assignable to System.Object$',issue)
                if d['locals'][local]=='System.Object' and match_type:
                    new='System.Int32' if match_type[1]=='I4' else 'System.Int64'
                    locals_changed.append({'index':local,'before':'System.Object','after':new,'trigger_offset':x['offset']})
                    d['locals'][local]=new;issue=None;continue
            if not index:break
            prior=d['instructions'][index-1];replacement=None;reason=None
            if x['opcode'] in ('ceq','cgt.un','cgt','clt','clt.un') and prior['opcode'] in ('ldc.i4.0','ldc.i4','ldc.i4.s') and (prior['opcode']=='ldc.i4.0' or prior['operand']==0):
                t=re.search(r'invalid comparison (.*), I4$',issue)
                if t:
                    other=t[1]
                    if other=='F':replacement=('ldc.r4',0.);reason='float zero'
                    elif x['opcode'] in ('ceq','cgt.un') and other in types and not types[other].get('value') and not types[other].get('byref') and not types[other].get('unbound'):replacement=('ldnull',None);reason='reference null'
            if x['opcode'] in ('call','callvirt','newobj') and x['operand']['args'] and prior['opcode'] in ('ldloca','ldloca.s','ldflda'):
                want=x['operand']['args'][-1]
                if want in ('UnityEngine.Vector2','UnityEngine.Vector3','UnityEngine.Vector4','UnityEngine.Quaternion','UnityEngine.Color'):
                    actual=d['locals'][prior['operand']['index']] if prior['opcode'].startswith('ldloca') else prior['operand']['type']
                    if actual==want and f'{want}& is not assignable to {want}' in issue:replacement=('ldloc' if prior['opcode'].startswith('ldloca') else 'ldfld',prior['operand']);reason='same struct value'
            if replacement is None:break
            changes.append({'offset':prior['offset'],'before':copy.deepcopy(prior),'after':{'opcode':replacement[0],'operand':replacement[1]},'reason':reason})
            prior['opcode'],prior['operand']=replacement;issue=None
    else:issue='proposal limit exceeded'
    if issue:
        blocked.append({'token':d['token'],'method':d['name'],'reason':issue})
    elif locals_changed:
        # Revert one local at a time; every inferred type must be necessary.
        for edit in locals_changed:
            bad=copy.deepcopy(d);bad['locals'][edit['index']]=edit['before']
            try:verify(bad,types)
            except InvalidIL:pass
            else:raise AssertionError(('local negative control accepted',d['name'],edit))
        eligible.append({'token':d['token'],'method':d['name'],'locals':locals_changed,'changes':changes,'verification':checked})
result={'status':'READ_ONLY_PROPOSALS','source_sha256':hashlib.sha256(path.read_bytes()).hexdigest(),'eligible_methods':len(eligible),'locals':sum(len(m['locals']) for m in eligible),'instruction_edits':sum(len(m['changes']) for m in eligible),'eligible':eligible,'blocked':blocked}
path.with_suffix('.integer-audit.json').write_text(json.dumps(result,indent=2))
print(json.dumps({k:v for k,v in result.items() if k not in ('eligible','blocked')},indent=2))
