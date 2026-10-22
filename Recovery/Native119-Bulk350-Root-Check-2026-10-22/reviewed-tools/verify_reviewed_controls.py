"""Sensitivity controls for separately reviewed static checker; not runtime behavior tests."""
from pathlib import Path
import json,importlib.util,copy,hashlib,sys
v=Path(__file__).resolve().parent;f=v/'reviewed-tools/verify_types_reviewed.py'
spec=importlib.util.spec_from_file_location('reviewed',f);m=importlib.util.module_from_spec(spec);spec.loader.exec_module(m)
d=json.loads((v/'r5-reviewed-targets.json').read_text('utf-8'));results=[]
for a in d['methods']:
    try:result=m.verify(a,d['types'])
    except Exception as e:result=dict(token=a['token'],method=a['name'],status='FAIL',error=str(e))
    results.append(result)
controls=[]
def negative(name,tok,change):
    a=copy.deepcopy(next(x for x in d['methods'] if x['token']==tok));types=copy.deepcopy(d['types']);change(a,types)
    try:m.verify(a,types);ok=False;why='NOT_REJECTED'
    except m.InvalidIL as e:ok=True;why=str(e)
    except Exception as e:ok=False;why='VERIFIER_ERROR '+str(e)
    controls.append(dict(name=name,rejected=ok,reason=why))
def instruction(a,op):return next(x for x in a['instructions'] if x['opcode']==op)
negative('wrong_constrained_type','0x06000088',lambda a,t:instruction(a,'constrained.')['operand'].update(type='System.Int32'))
negative('constrained_call_wrong_return','0x06000088',lambda a,t:next(x for x in a['instructions'] if x['opcode']=='callvirt' and x['operand']['owner']=='System.Enum')['operand'].update(ret='System.Int32'))
negative('branch_bypasses_prefix','0x06000088',lambda a,t:a['instructions'][0].update(opcode='br',operand={'target':instruction(a,'constrained.')['offset']+6}))
def remove_interfaces(a,t):
    for name in t:
        if name.startswith('TMPro.Examples.TextMeshProFloatingText/<DisplayTextMeshProFloatingText>'):t[name]['interfaces']=[]
negative('missing_real_coroutine_interfaces','0x06000798',remove_interfaces)
def wrong_interface(a,t):
    next(x for x in a['instructions'] if x['opcode']=='callvirt' and x['operand'].get('name')=='StartCoroutine')['operand']['args']=['System.IFormatProvider']
negative('unimplemented_interface','0x06000798',wrong_interface)
negative('non_type_ldtoken','0x06000069',lambda a,t:instruction(a,'ldtoken').update(operand={'method':'NotATypeHandle'}))
negative('declared_stack_underflow','0x060004A9',lambda a,t:a['instructions'][0].update(opcode='pop',operand=None))
negative('maxstack_header_too_small','0x060004A9',lambda a,t:a.update(maxStack=0))
report=dict(scope='SCOPED_STATIC_TYPE_FLOW_WITH_REVIEWED_ENUM_CONSTRAINED_AND_INTERFACE_SUPPORT_ONLY',candidate_sha256=hashlib.sha256((v/'r5-candidate.dll').read_bytes()).hexdigest(),checker_sha256=hashlib.sha256(f.read_bytes()).hexdigest(),passed=sum(x['status']=='PASS' for x in results),total=len(results),negative_controls=controls,negative_controls_all_rejected=all(x['rejected'] for x in controls),results=results,behavior_qualification=False)
(v/'r5-REVIEWED-STATIC-TYPES.json').write_text(json.dumps(report,indent=2)+'\n','utf-8')
print({k:x for k,x in report.items() if k not in ['results','negative_controls']});print(controls);print([x for x in results if x['status']!='PASS'])
sys.exit(report['passed']!=report['total'] or not report['negative_controls_all_rejected'])
