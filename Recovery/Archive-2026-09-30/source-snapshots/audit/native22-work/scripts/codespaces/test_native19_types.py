"""Mutate actual candidate method specifications, and require typed rejection."""
import copy,json,sys
from pathlib import Path
from verify_native19_types import verify,InvalidIL
path=Path(sys.argv[1]);data=json.loads(path.read_text());types=data['types'];cases=[]
def method(name):return copy.deepcopy(next(d for d in data['methods'] if '::'+name+'(' in d['name']))
def add(label,d):cases.append((label,d))
d=method('Update_Move');i=next(n for n,x in enumerate(d['instructions']) if isinstance(x['operand'],dict) and x['operand'].get('name')=='set_position');d['instructions'][i-1]['opcode']='ldloca';add('Vector3 pointer supplied to by-value parameter',d)
d=method('DestroyZombie');d['locals'][10]='System.Object';add('object used as numeric loop local',d)
d=method('DestroyZombie');x=next(x for x in d['instructions'] if isinstance(x['operand'],dict) and x['operand'].get('name')=='Remove');x['operand']['owner']='System.Collections.Generic.List`1<System.Object>';add('List<object> receiver on List<Zombie>',d)
d=method('CheckZombieWin');x=next(x for n,x in enumerate(d['instructions'][:-1]) if x['opcode']=='ldnull' and d['instructions'][n+1]['opcode']=='ceq');x['opcode']='ldc.i4.0';add('reference compared with integer zero',d)
d=method('ArmBroken');i=next(n for n,x in enumerate(d['instructions']) if isinstance(x['operand'],dict) and x['operand'].get('identity','').startswith('System.Single Zombie::armor2Point'));d['instructions'][i+1]['opcode']='ldc.i4.0';add('float compared with integer zero',d)
d=method('Update_Move');d['maxStack']=0;add('undersized declared MaxStack',d)
d=method('LoopAddAnimation');x=next(x for x in d['instructions'] if x['opcode']=='leave');x['opcode']='br';add('branch exits try without finally',d)
d=method('PreviousPosition');d['instructions'][0]['opcode']='unrecognized';add('unsupported instruction fails closed',d)
results=[]
for label,d in cases:
 try:verify(d,types)
 except InvalidIL as e:results.append({'case':label,'rejected':True,'reason':str(e)})
 else:raise AssertionError('mutant accepted: '+label)
path.with_suffix('.negative.json').write_text(json.dumps(results,indent=2));print('TYPED_NEGATIVE_CONTROLS_PASS cases='+str(len(results)))
