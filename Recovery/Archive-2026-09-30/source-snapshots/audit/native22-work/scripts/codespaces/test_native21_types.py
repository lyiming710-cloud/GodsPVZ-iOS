"""Every retyped local and every encoding edit must reject its reverted defect."""
import copy,json,sys,hashlib
from pathlib import Path
from verify_native19_types import verify,InvalidIL
root=Path(__file__).resolve().parents[2]
path=Path(sys.argv[1]);data=json.loads(path.read_text());before=json.loads(Path(str(path).replace('.targets.json','.before.json')).read_text())
plans=json.loads((root/'Recovery/Native21-2026-09-29/PROPOSALS.json').read_text())['eligible']
edits=json.loads(Path(str(path).replace('.targets.json','.edits.json')).read_text())
assert len(data['methods'])==38 and len(edits)==9 and sum(len(p['locals']) for p in plans)==90
methods={d['token']:d for d in data['methods']};originals={d['token']:d for d in before['methods']};results=[];negative=[]
def reject(d,label):
    try:verify(d,data['types'])
    except InvalidIL as error:negative.append({'token':d['token'],'change':label,'rejected':True,'error':str(error)})
    else:raise AssertionError(('reverted defect was accepted',d['name'],label))
for plan in plans:
    d=methods[plan['token']];results.append(verify(d,data['types']))
    changed={e['index']:e for e in plan['locals']}
    assert len(d['locals'])==len(originals[d['token']]['locals'])
    for index,(old,new) in enumerate(zip(originals[d['token']]['locals'],d['locals'])):
        if index in changed:
            assert old==changed[index]['before'] and new==changed[index]['after']
            bad=copy.deepcopy(d);bad['locals'][index]=old;reject(bad,{'local':index})
        else:assert old==new,('unplanned local change',d['name'],index)
for edit in edits:
    d=copy.deepcopy(methods[edit['token']]);ins=d['instructions'][edit['index']]
    assert ins['opcode']==edit['after']['opcode']
    ins['opcode']=edit['before']['opcode'];ins['operand']=edit['before']['operand'];reject(d,{'instruction':edit['index']})
report={'status':'PASS','scope':'integer local and encoding constraints across reachable CIL; not native equivalence','targets':len(results),'negative_controls':len(negative),'unreachable':sum(r['unreachable'] for r in results),'source_sha256':hashlib.sha256(path.read_bytes()).hexdigest(),'results':results,'negatives':negative}
path.with_suffix('.typed-negative.json').write_text(json.dumps(report,indent=2))
print(json.dumps({k:v for k,v in report.items() if k not in ('results','negatives')},indent=2))
