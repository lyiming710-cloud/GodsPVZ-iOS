from pathlib import Path
import json,re,hashlib,subprocess,copy,sys
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native27-2026-10-01';base=w/'.validation/native26-2026-10-01'
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
assert sha(base/'native26-final1.dll')=='affd9276454e70d6f39d9e28144ba2006021e818b1a617ed517eea90741ca878'
all_e=json.loads((n/'NATIVE-BATCH-EVIDENCE.json').read_text());selected=all_e['methods'][2:14];assert len(selected)==12
null_throws={m['token']:(m['recipe']['null_sites'][0]['offset'],m['recipe']['throw_sites'][0]['offset']) for m in selected}
review=[]
for m in selected:
 asm=m['disassembly'];ins=m['managed_instructions'];recipe=m['recipe'];assert m['exception_helper_calls']==1 and len(recipe['null_sites'])==len(recipe['throw_sites'])==1
 assert not any(i['opcode']=='ldstr' for i in ins)
 terminal=re.search(r'([0-9a-f]+):\s+(?:[0-9a-f]{2} )+\s*call\s+0x180250150\s*\n\s*([0-9a-f]+):.*int3',asm);assert terminal
 site={'token':m['token'],'method':m['method'],'native_start':m['start'],'native_end':m['end'],'native_sha256':m['native_sha256'],'null_guard_terminal':'0x'+terminal[1],'exception_helper':'0x180250150','helper_identity_scope':'Historically audited NRE helper; current call+int3 verified, no fabricated MethodDef','recipe':recipe,'verdict':'APPROVED_CALL_CONTRACT','behavior_scope':'Native outer method contract; helper internals not newly recovered'}
 if 'SwfManager::get_' in m['method']:
  field=0x20 if 'get_clipCount' in m['method'] else 0x28
  assert re.search(r'mov\s+0x%x\(%%rbx\),%%rcx'%field,asm) and 'test   %rcx,%rcx' in asm
  jump=re.search(r'je\s+(0x[0-9a-f]+)',asm);assert jump and jump[1]==site['null_guard_terminal']
  assert 'jmp' in asm and '0x18065baa0' in asm
  site.update({'native_field_offset':field,'non_null_target':'0x18065baa0','non_null_contract':'Read this list then return existing SwfAssocList<T>.Count; no fallback to zero'})
 else:
  assert m['body_bytes']==114 and recipe['null_sites'][0]['reference_provenance']=='ARG:0'
  assert 'test   %rcx,%rcx' in asm;guard=re.search(r'je\s+(0x[0-9a-f]+)',asm);assert guard and guard[1]==site['null_guard_terminal']
  business=[i['operand'] for i in ins if i['opcode']=='call' and i['operand']['owner']=='FTRuntime.SwfClipController'];assert len(business)==1;action=business[0];expected={'Play':'0x1803c68a0','GotoAndStop':'0x1803c6240','GotoAndPlay':'0x1803c6030' if len(action['args'])==2 else '0x1803c60d0'}[action['name']]
  calls=[c['va'] for c in m['calls']];assert calls==[expected,'0x18024fef0','0x180250100','0x18030f300',{'FTRuntime.Yields.SwfWaitStopPlaying':'0x1803cbd20','FTRuntime.Yields.SwfWaitRewindPlaying':'0x1803cb7f0','FTRuntime.Yields.SwfWaitStopOrRewindPlaying':'0x1803cba50','FTRuntime.Yields.SwfWaitPlayStopped':'0x1803cb470'}[m['method'].split(' ')[0]],'0x180250150']
  assert 'mov    %rdi,%rdx' in asm and 'mov    %rbx,%rcx' in asm and 'mov    %rbx,%rax' in asm and 'xor    %edx,%edx' in asm
  # RCX receiver, RDX first arg and R8 second arg pass through unchanged before the action call. R8/R9 is only zeroed for MethodInfo.
  before=asm.split('call   '+expected)[0];assert not re.search(r'(?:mov|xor).*%(?:edx|rdx)',before);assert len(action['args'])==1 or not re.search(r'(?:mov|xor).*%r8[db]?',before)
  sub=next(i['operand'] for i in ins if i['opcode']=='call' and i['operand']['name']=='Subscribe');ctor=next(i for i in ins if i['opcode']=='newobj' and i['operand']['owner'].startswith('FTRuntime.Yields'));idx=ins.index(ctor);assert ins[idx-1]['opcode']=='ldnull' and ctor['operand']['owner']==sub['owner']
  site.update({'action_call':expected,'action_CIL':action['identity'],'subscribe_call':calls[-2],'subscribe_CIL':sub['identity'],'allocation':'0x180250100','constructor_shared_call':'0x18030f300','contract':'Controller physical null -> NRE. Otherwise action(original args), allocate declared wait, shared ctor(null), Subscribe(original controller), return allocated wait rather than Subscribe result','allocation_type_scope':'Wait owner inferred from matching existing CIL ctor and native Subscribe receiver; class pointer not independently decoded'})
 review.append(site)
report={'parent_sha256':all_e['input_sha256'],'native_inputs':{k:v for k,v in all_e.items() if k.endswith('sha256')},'approved_methods':review,'quarantined_known_missing_call':[{'token':m['token'],'reason':'PC calls UnityEvent<string,int,int>.Invoke at 0x180A66460; current IL discards Method not found placeholder and omits event invocation'} for m in all_e['methods'][:2]],'deferred_methods':[m['token'] for m in all_e['methods'][14:]],'scope':'Per-method native contract review, not standalone game behavior proof'}
(n/'NATIVE-REVIEW.json').write_text(json.dumps(report,indent=2));print(json.dumps({'approved':len(review),'null_sites':len(review),'throw_sites':len(review),'deferred':len(report['deferred_methods'])}))
# The previous filter is retained as negative evidence: both lost-Invoke methods passed its closure gate.
assert all(m['recipe']['type_trial']['status']=='PASS' for m in all_e['methods'][:2])
neg={'missing_call_type_pass_does_not_authorize_patch':True,'missing_call_prefix_quarantined':all(i['operand'].startswith('Method not found @') for m in all_e['methods'][:2] for i in m['managed_instructions'] if i['opcode']=='ldstr'),'historical_filter_missed_methods':['0x0600073A','0x0600073B']}
assert all(v for k,v in neg.items() if isinstance(v,bool));(n/'FILTER-NEGATIVE-CONTROLS.json').write_text(json.dumps(neg,indent=2))
