"""Fail-closed typed worklist verifier for the emitted native3 CIL subset.
Checks every reachable instruction, merge, field owner, call argument/receiver,
value/reference/pointer types, EH entry/exit and declared max stack. Not a whole-assembly ECMA verifier.
"""
import json,re,sys,copy
from pathlib import Path
R=Path(__file__).resolve().parent
parents={}
for l in (R/'evidence/managed-identities.txt').read_text(encoding='utf-8-sig').splitlines():
 if l.startswith('TYPE '):
  name,base=l.split('] ',1)[1].split(' base=');parents[name]=base
parents.update({'UnityEngine.MonoBehaviour':'UnityEngine.Behaviour','UnityEngine.Behaviour':'UnityEngine.Component','UnityEngine.Component':'UnityEngine.Object','UnityEngine.GameObject':'UnityEngine.Object','UnityEngine.Object':'System.Object'})
ints={'System.Int32','System.Boolean','System.Byte','System.SByte','System.Int16','System.UInt16','System.UInt32','ChallengeType','BoardEntryType'}
def norm(t):return 'I4' if t in ints else 'F' if t in ['System.Single','System.Double'] else t
def isref(t):return t not in {'I4','F','System.Void'} and not t.endswith('&') and '/Enumerator<' not in t and t!='UnityEngine.Vector3'
def assign(actual,expected):
 actual,expected=norm(actual),norm(expected)
 if actual==expected:return True
 if actual=='null':return isref(expected)
 if expected=='System.Object' and isref(actual):return True
 seen=set()
 while actual in parents and actual not in seen:
  seen.add(actual);actual=parents[actual]
  if actual==expected:return True
 return False
def instantiate(t,owner):
 if '!0' not in t:return t
 assert '<' in owner,'unbound generic '+t
 return t.replace('!0',owner[owner.index('<')+1:-1])
class VerificationError(Exception):pass
def verify(d):
 ins=d['instructions'];by={x['offset']:i for i,x in enumerate(ins)};states={};work=[];peak=0;visited=set()
 def error(i,msg):raise VerificationError(f"{d['name']} IL_{ins[i]['offset']:04X}: {msg}")
 def queue(i,stack):
  stack=tuple(map(norm,stack))
  if i in states:
   old=states[i]
   if len(old)!=len(stack):error(i,f'merge height {old} != {stack}')
   merged=[]
   for a,b in zip(old,stack):
    if a==b:merged.append(a)
    elif a=='null' and isref(b):merged.append(b)
    elif b=='null' and isref(a):merged.append(a)
    else:error(i,f'merge type {a} != {b}')
   if tuple(merged)==old:return
   stack=tuple(merged)
  states[i]=stack;work.append(i)
 def region(off):
  r=[]
  for n,h in enumerate(d['handlers']):
   if h['tryStart']<=off<h['tryEnd']:r.append(('try',n))
   if h['handlerStart']<=off<h['handlerEnd']:r.append(('handler',n))
  return r
 for h in d['handlers']:
  if h['kind']!='Finally':raise VerificationError('unsupported EH '+h['kind'])
  for k in ['tryStart','tryEnd','handlerStart','handlerEnd']:
   if h[k] not in by:raise VerificationError('EH boundary not an instruction')
  if not h['tryStart']<h['tryEnd']<=h['handlerStart']<h['handlerEnd']:raise VerificationError('invalid EH ranges')
  queue(by[h['handlerStart']],[])
 queue(0,[])
 while work:
  i=work.pop();x=ins[i];s=list(states[i]);visited.add(i);op=x['opcode'];a=x.get('operand');successors=None
  def pop(expected=None):
   if not s:error(i,'stack underflow')
   t=s.pop()
   if expected is not None and not assign(t,expected):error(i,f'type {t} is not assignable to {expected}')
   return t
  def push(t):s.append(norm(t))
  def receiver(owner):
   t=pop();wanted=owner+'&' if '/Enumerator<' in owner or owner=='UnityEngine.Vector3' else owner
   if not assign(t,wanted):error(i,f'wrong declaring type: {t}, expected {wanted}')
  if op=='nop':pass
  elif op=='ldnull':push('null')
  elif op.startswith('ldc.i4'):push('I4')
  elif op in ['ldc.r4','ldc.r8']:push('F')
  elif op.startswith('ldarg.'):
   n=int(op.rsplit('.',1)[1]);push(d['owner'] if n==0 else d['args'][n-1])
  elif op=='ldloc':push(d['locals'][a['index']])
  elif op=='ldloca':push(d['locals'][a['index']]+'&')
  elif op=='stloc':pop(d['locals'][a['index']])
  elif op=='dup':t=pop();push(t);push(t)
  elif op=='pop':pop()
  elif op in ['ldfld','ldflda']:
   receiver(a['owner']);push(a['type']+('&' if op=='ldflda' else ''))
  elif op=='stfld':pop(a['type']);receiver(a['owner'])
  elif op in ['call','callvirt','newobj']:
   for p in reversed(a['args']):pop(instantiate(p,a['owner']))
   if op!='newobj' and a['hasThis']:receiver(a['owner'])
   if op=='newobj':push(a['owner'])
   elif a['ret']!='System.Void':push(instantiate(a['ret'],a['owner']))
  elif op in ['add','sub','mul']:
   v=pop();w=pop()
   if v!=w or v not in ['I4','F']:error(i,f'invalid numeric operands {w}, {v}')
   push(v)
  elif op in ['conv.i4','conv.r4']:
   if pop() not in ['I4','F']:error(i,'invalid conversion')
   push('I4' if op=='conv.i4' else 'F')
  elif op in ['cgt.un','cgt','ceq']:
   v=pop();w=pop()
   if v!=w:error(i,f'incompatible comparison {w}, {v}')
   push('I4')
  elif op=='newarr':pop('I4');push(a['type']+'[]')
  elif op.startswith('ldelem'):
   pop('I4');arr=pop()
   if not arr.endswith('[]'):error(i,'not an array: '+arr)
   el=arr[:-2]
   if op=='ldelema':
    if el!=a['type']:error(i,'ldelema type mismatch')
    push(el+'&')
   elif op=='ldelem.i4':
    if norm(el)!='I4':error(i,'ldelem.i4 element mismatch')
    push('I4')
   elif op=='ldelem.u1':
    if el not in ['System.Boolean','System.Byte']:error(i,'ldelem.u1 element mismatch')
    push('I4')
   else:error(i,'unsupported opcode '+op)
  elif op.startswith('stelem.'):
   val=pop();pop('I4');arr=pop()
   if not arr.endswith('[]') or not assign(val,arr[:-2]):error(i,'array store mismatch')
  elif op=='ldind.i4':
   t=pop()
   if t!='System.Int32&':error(i,'ldind.i4 pointer mismatch')
   push('I4')
  elif op=='stind.i4':pop('I4');pop('System.Int32&')
  elif op in ['br','brtrue','brfalse','beq','bne.un','blt','bgt','bge','leave']:
   dest=by.get(a['target'])
   if dest is None:error(i,'branch not an instruction boundary')
   if op in ['brtrue','brfalse']:
    t=pop()
    if t=='F' or '/Enumerator<' in t:error(i,'invalid branch condition')
   elif op not in ['br','leave']:
    v=pop();w=pop()
    if v!=w or v not in ['I4','F']:error(i,f'incompatible branch comparison {w}, {v}')
   if op=='leave':
    if any(t=='handler' for t,n in region(x['offset'])):error(i,'leave from finally')
    if s:error(i,'nonempty stack at leave')
   elif region(x['offset'])!=region(a['target']):error(i,'ordinary branch across EH boundary')
   successors=[dest] if op in ['br','leave'] else [dest,i+1]
  elif op=='endfinally':
   if s:error(i,'nonempty finally exit')
   if not any(t=='handler' for t,n in region(x['offset'])):error(i,'endfinally outside handler')
   successors=[]
  elif op=='ret':
   if d['ret']!='System.Void':pop(d['ret'])
   if s:error(i,'nonempty return stack')
   if region(x['offset']):error(i,'ret in protected region')
   successors=[]
  else:error(i,'unsupported opcode '+op)
  peak=max(peak,len(s))
  if successors is None:successors=[i+1]
  for j in successors:
   if j>=len(ins):error(i,'fallthrough past method end')
   queue(j,s)
 if peak>d['maxStack']:raise VerificationError('declared maxstack too small')
 if len(visited)!=len(ins):raise VerificationError('unreachable instructions: '+str(sorted(set(range(len(ins)))-visited)))
 return {'method':d['name'],'instructions':len(ins),'checked':len(visited),'max_stack':peak,'eh':len(d['handlers']),'status':'PASS'}
def main():
 root=Path(sys.argv[1]) if len(sys.argv)>1 else R/'specification'
 docs=[json.loads(p.read_text()) for p in sorted(root.glob('*.spec.json'))]
 assert len(docs)==4
 results=[verify(d) for d in docs]
 # Mutation tests verify rejection of the historical bug classes, not just positive fixtures.
 byname={d['name']:d for d in docs}; enemy=next(d for d in docs if 'CreateEnemySelecter' in d['name']);zombie=next(d for d in docs if 'GetZombieUnderMouse' in d['name']);pas=next(d for d in docs if 'PassLevel' in d['name'])
 mutants=[]
 d=copy.deepcopy(enemy);i=next(i for i,x in enumerate(d['instructions']) if x['opcode']=='stfld' and x['operand']['identity']=='Board EnemySelecter::board');d['instructions'][i-3]['opcode']='nop';d['instructions'][i-3]['operand']=None;mutants.append(('missing stfld objref',d))
 d=copy.deepcopy(zombie);x=next(x for x in d['instructions'] if x['opcode']=='ldfld' and x['operand']['identity']=='ZombieManager Board::zombieManager');x['operand']['owner']='ZombieManager';mutants.append(('wrong declaring type',d))
 d=copy.deepcopy(pas);d['locals'][1]='System.Object';mutants.append(('value/reference enumerator mismatch',d))
 d=copy.deepcopy(zombie);x=next(x for x in d['instructions'] if x['opcode']=='ldflda');x['opcode']='ldfld';mutants.append(('Vector3 value instead of managed pointer',d))
 negative=[]
 for title,d in mutants:
  try:verify(d)
  except VerificationError as e:negative.append({'case':title,'rejected':True,'reason':str(e)})
  else:raise AssertionError('accepted mutant '+title)
 report={'scope':'four target methods; supported opcode subset; fail closed','results':results,'negative_controls':negative}
 (root/'stack-verification.json').write_text(json.dumps(report,indent=2))
 print(json.dumps(report,indent=2))
if __name__=='__main__':main()
