"""Verify actual serialized targets and reject each individually reverted edit."""
import copy,json,sys,hashlib
from pathlib import Path
from verify_native19_types import verify,InvalidIL
path=Path(sys.argv[1]);data=json.loads(path.read_text())
edits=json.loads(Path(str(path).replace('.targets.json','.edits.json')).read_text())
assert len(data['methods'])==73 and len(edits)==112
methods={d['token']:d for d in data['methods']};results=[];negative=[]
for d in data['methods']:
    r=verify(d,data['types']);results.append(r)
for e in edits:
    original=methods[e['token']];d=copy.deepcopy(original);ins=d['instructions'][e['index']]
    assert ins['opcode']==e['after']['opcode'],(d['name'],ins,e)
    ins['opcode']=e['before']['opcode'];ins['operand']=e['before']['operand']
    try:verify(d,data['types'])
    except InvalidIL as error:negative.append({'token':d['token'],'index':e['index'],'rejected':True,'error':str(error)})
    else:raise AssertionError(('reverted edit was accepted',e))
report={'status':'PASS','scope':'typed reachable CIL and one-reversion-per-edit negative controls; not native equivalence','targets':len(results),'negative_controls':len(negative),'unreachable':sum(r['unreachable'] for r in results),'source_sha256':hashlib.sha256(path.read_bytes()).hexdigest(),'results':results,'negatives':negative}
path.with_suffix('.typed-negative.json').write_text(json.dumps(report,indent=2))
print(json.dumps({k:v for k,v in report.items() if k not in ('results','negatives')},indent=2))
