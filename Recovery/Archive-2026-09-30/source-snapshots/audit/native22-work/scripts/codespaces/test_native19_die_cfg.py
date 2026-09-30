"""Execute the actual exported Die CIL CFG against an independent PC-native oracle.

This is an IL interpreter fixture with explicit Unity/helper call doubles, not CLR
or device acceptance. Unsupported instructions/calls fail rather than being skipped.
Native authority: 0x18035E7E0-0x18035ED68 and tables at 0x18035ED74/0x18035ED8C.
"""
import json,itertools,sys,copy
from pathlib import Path

def state(case):
 id,noResidue,ashe,died,ashes,dying,plant,normal,disabled=case
 return dict(kind='zombie',ID=id,isDied=died,ashes=ashes,isDying=dying,armor1Point=2.,armor2Point=3.,destroyTicking=9.,fX=11.,fY=23.,fZ=7.,
             animator={'kind':'animator'},animationGroup={'kind':'animation'},sortingGroup={'kind':'sorting'},transform={'kind':'root','position':(101.,202.,303.)})

def oracle(case):
 id,noResidue,ashe,died,ashes,dying,plant,normal,disabled=case;z=state(case);events=[]
 if id==17:
  events=[['position','particle',[11.,30.,0.]],['audio'],['drop'],['destroy']]
 elif id==18:
  z['isDied']=True
  if not disabled:events.append(['drop'])
  events.append(['trigger','GoDie'])
 else:
  special=id in (0,1,2,3,4,5,10,14,15,16,23) or plant
  suppress=(died or ashes) or (dying if special else died)
  if not ashe:
   z['isDied']=True
   if not suppress:events.append(['drop'])
   events.extend([['armor2',noResidue],['armor1',noResidue]])
   if noResidue:events.append(['destroy'])
   else:
    if normal or plant:events.append(['group',0])
    events.append(['trigger','GoDie'])
  else:
   if suppress and dying or plant:
    events.append(['speed',0.]);z['destroyTicking']=3.
   else:events.extend([['position','charred',[101.,202.,303.]],['link_zombie'],['layer_name','fixture'],['layer_id',7],['enabled',False],['active',False]])
   z['isDied']=True;z['ashes']=True
   if not suppress:events.append(['drop'])
 return events,{k:z[k] for k in ('isDied','ashes','destroyTicking')}

def execute(d,case):
 id,noResidue,ashe,died,ashes,dying,plant,normal,disabled=case
 z=state(case);stack=[];locals=[None if ('System.' not in t or t in ('System.Object','System.String')) else 0 for t in d['locals']];args=[z,noResidue,ashe];events=[]
 by={i['offset']:n for n,i in enumerate(d['instructions'])};pc=0
 def call(m,p,receiver):
  owner=m['owner'];name=m['name']
  if owner=='Zombie':
   if name=='IsPlantZombie':return plant
   if name=='IsNormalZombie':return normal
   if name=='IsDisabled':return disabled
   if name=='DropLootPiece':events.append(['drop']);return
   if name=='DestroyZombie':events.append(['destroy']);return
   if name in ('Drop_Armor1','Drop_Armor2'):events.append(['armor'+name[-1],bool(p[0])]);return
  if name=='get_transform':return receiver.setdefault('transform',{'kind':receiver['kind'],'position':(0.,0.,0.)})
  if name=='get_position':return receiver['position']
  if name=='set_position':receiver['position']=p[0];events.append(['position',receiver['kind'],list(p[0])]);return
  if owner=='ParticlesManager' and name=='CreatNewParticle':return {'kind':'particle'}
  if owner=='ResourceManager' and name=='GetZombie_charred':return {'kind':'charred'}
  if owner=='UnityEngine.Object' and name=='Instantiate':return {'kind':'charred'}
  if name=='AddComponent':return {'kind':'sorting'}
  if owner=='UnityEngine.Vector3' and name=='.ctor':return tuple(p)
  if name=='get_Item':return receiver[p[0]]
  if name=='CreateAudioAtPoint':events.append(['audio']);return {'kind':'audio'}
  if name=='Range' and owner=='UnityEngine.Random':return 0
  if name=='get_sortingLayerName':return 'fixture'
  if name=='get_sortingLayerID':return 7
  if name=='set_sortingLayerName':events.append(['layer_name',p[0]]);return
  if name=='set_sortingLayerID':events.append(['layer_id',p[0]]);return
  if name=='set_enabled':events.append(['enabled',bool(p[0])]);return
  if name=='SetActive':events.append(['active',bool(p[0])]);return
  if name=='set_speed':events.append(['speed',p[0]]);return
  if name=='SetInteger':events.append(['group',p[1]]);return
  if name=='SetTrigger':events.append(['trigger',p[0]]);return
  raise AssertionError('unsupported call '+m['identity'])
 for step in range(5000):
  x=d['instructions'][pc];op=x['opcode'];a=x['operand'];nextpc=pc+1
  if op=='nop':pass
  elif op=='ldnull':stack.append(None)
  elif op=='ldstr':stack.append(a)
  elif op.startswith('ldc.i4'):
   stack.append(a if op in ('ldc.i4','ldc.i4.s') else -1 if op=='ldc.i4.m1' else int(op.rsplit('.',1)[1]))
  elif op in ('ldc.i8','ldc.r4','ldc.r8'):stack.append(a)
  elif op.startswith('ldarg'):
   index=a['index'] if isinstance(a,dict) else int(op.rsplit('.',1)[1]);stack.append(args[index])
  elif op.startswith('ldloc'):stack.append(locals[a['index']])
  elif op.startswith('stloc'):locals[a['index']]=stack.pop()
  elif op in ('ldfld','stfld'):
   name=a['identity'].split('::')[-1]
   if op=='ldfld':stack.append(stack.pop()[name])
   else:
    value=stack.pop();receiver=stack.pop()
    if a['type']=='System.Boolean':value=bool(int(value)&255)
    receiver[name]=value
    if name=='zombie':events.append(['link_zombie'])
  elif op=='ldsfld':
   name=a['identity'].split('::')[-1]
   if name=='<instance>k__BackingField':stack.append({'kind':'particles'})
   elif name=='particleClips':stack.append([{'kind':'clip'}]*20)
   else:raise AssertionError('unsupported static '+name)
  elif op in ('add','sub','ceq','cgt','cgt.un','clt'):
   b=stack.pop();c=stack.pop()
   if op=='add':v=c+b
   elif op=='sub':v=c-b
   elif op=='ceq':v=c==b
   elif op=='cgt':v=c>b
   elif op=='cgt.un':v=(c&0xffffffff)>(b&0xffffffff)
   else:v=c<b
   if op in ('add','sub') and isinstance(v,int):v=(v+2**31)%2**32-2**31
   stack.append(v)
  elif op=='pop':stack.pop()
  elif op=='switch':
   index=stack.pop()
   if 0<=index<len(a['targets']):nextpc=by[a['targets'][index]]
  elif op in ('br','brtrue','brfalse'):
   go=True if op=='br' else bool(stack.pop())==(op=='brtrue')
   if go:nextpc=by[a['target']]
  elif op in ('call','callvirt','newobj'):
   n=len(a['args']);p=stack[-n:] if n else []
   if n:del stack[-n:]
   receiver=stack.pop() if op!='newobj' and a['hasThis'] else None
   value=call(a,p,receiver)
   if op=='newobj' or a['ret']!='System.Void':stack.append(value)
  elif op=='ret':
   assert not stack,'nonempty return'
   return events,{k:z[k] for k in ('isDied','ashes','destroyTicking')}
  else:raise AssertionError('unsupported opcode '+op)
  pc=nextpc
 raise AssertionError('loop bound exceeded')

def main():
 path=Path(sys.argv[1]);d=next(d for d in json.loads(path.read_text())['methods'] if 'Zombie::Die(' in d['name']);count=0
 ids=[-2147483648,-1]+list(range(26))+[2147483647]
 for id in ids:
  for flags in itertools.product((False,True),repeat=8):
   case=(id,)+flags;want=oracle(case);got=execute(d,case)
   assert got==want,json.dumps({'case':case,'want':want,'got':got})
   count+=1
 mutants=[]
 broken=copy.deepcopy(d);store=next(n for n,x in enumerate(broken['instructions']) if x['opcode']=='stfld' and x['operand']['identity']=='System.Boolean Zombie::ashes')
 for x in broken['instructions'][store-2:store+1]:x['opcode']='nop';x['operand']=None
 mutants.append(('omitted ashes store',broken,(0,False,True,False,False,False,False,False,False)))
 broken=copy.deepcopy(d)
 for x in broken['instructions']:
  if x['opcode']=='switch':x['operand']['targets']=[x['operand']['targets'][1]]*6
 mutants.append(('missing native jump-table cases',broken,(0,False,False,False,False,True,False,False,False)))
 broken=copy.deepcopy(d);load=next(n for n,x in enumerate(broken['instructions']) if x['opcode']=='ldfld' and x['operand']['identity']=='System.Single Zombie::fZ')
 broken['instructions'][load-1]['opcode']='nop';broken['instructions'][load-1]['operand']=None;broken['instructions'][load]['opcode']='ldc.r4';broken['instructions'][load]['operand']=0.
 mutants.append(('lost particle height offset',broken,(17,False,False,False,False,False,False,False,False)))
 for label,broken,case in mutants:assert execute(broken,case)!=oracle(case),'accepted semantic mutant '+label
 result={'status':'PASS','cases':count,'negative_controls':[label for label,_,_ in mutants],'scope':'actual Die CIL CFG with explicit helper doubles; native-derived independent oracle','native_va':'0x18035E7E0'}
 path.with_suffix('.die-cfg.json').write_text(json.dumps(result,indent=2));print('DIE_NATIVE_CFG_MATRIX_PASS cases='+str(count))
if __name__=='__main__':main()
