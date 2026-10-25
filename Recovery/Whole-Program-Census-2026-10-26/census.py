"""Read-only whole-assembly census with candidate-bound evidence and honest gaps.

This inventories every managed body. It does not infer universal game behavior
from compilation, warning strings, tiny bodies, or historical test counts.
"""
from pathlib import Path
import bisect,collections,copy,csv,hashlib,importlib.util,json,re,struct,subprocess,sys
O=Path(__file__).resolve().parent;W=O.parent;ROOT=W.parents[1];Q=W/'startup-r19-batch100';P=Q/'shared-candidate9'
sys.dont_write_bytecode=True;sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile,pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64,CS_GRP_JUMP,CS_GRP_RET
from capstone.x86_const import X86_OP_IMM,X86_OP_MEM,X86_REG_RCX
DIGEST='0ba5bc7ec9603d14a1c0bde6d784d232aa78efaa1700304e75e4f2b56ef7dec2'
MARKER=re.compile(r'Invalid instruction:|Not implemented instruction:|Unmanaged memory load|Method not found @|Cpp2IL|NoteDecompilerIssue',re.I)
TERMINAL={'ret','throw','rethrow','endfinally','endfilter','jmp'}
def sha(b):return hashlib.sha256(b).hexdigest()
def js(p,j):Path(p).write_text(json.dumps(j,indent=2,ensure_ascii=True)+'\n',encoding='utf-8')
def read(p):return json.loads(Path(p).read_bytes())
def load(name,p):
 spec=importlib.util.spec_from_file_location(name,p);m=importlib.util.module_from_spec(spec);spec.loader.exec_module(m);return m
def cfg(method,code_size):
 ins=method['instructions'];lookup={i['offset']:i for i in ins};invalid=[];edges={};seeds=[ins[0]['offset']] if ins else []
 if len(lookup)!=len(ins) or (ins and ins[0]['offset']!=0):invalid.append(dict(kind='DUPLICATE_OR_MISSING_ENTRY'))
 for h in method['handlers']:
  for k in ['handlerStart','filterStart']:
   if isinstance(h.get(k),int):seeds.append(h[k])
  for k in ['tryStart','tryEnd','handlerStart','handlerEnd','filterStart']:
   v=h.get(k)
   if isinstance(v,int) and v not in lookup and not (k in ['tryEnd','handlerEnd'] and v==code_size):invalid.append(dict(kind='EH_BOUNDARY_NOT_AN_INSTRUCTION',boundary=k,offset=v))
  for a,b in [('tryStart','tryEnd'),('handlerStart','handlerEnd')]:
   if not isinstance(h.get(a),int) or not isinstance(h.get(b),int) or not 0<=h[a]<h[b]<=code_size:invalid.append(dict(kind='INVALID_EH_RANGE',start=h.get(a),end=h.get(b)))
 for k,i in enumerate(ins):
  at=i['offset'];op=i['opcode'];stem=op.split('.')[0];fall=[ins[k+1]['offset']] if k+1<len(ins) else [code_size];v=i['operand']
  if op in TERMINAL:targets=[]
  elif stem in ['br','leave']:targets=[v['target']]
  elif stem in ['brtrue','brfalse','beq','bne','bge','bgt','ble','blt']:targets=[v['target']]+fall
  elif op=='switch':targets=v['targets']+fall
  else:targets=fall
  for t in targets:
   if t not in lookup and t!=code_size:invalid.append(dict(kind='BRANCH_TARGET_NOT_AN_INSTRUCTION',source=at,target=t))
  if not 0<=at<code_size:invalid.append(dict(kind='INSTRUCTION_OUTSIDE_BODY',offset=at))
  edges[at]=targets
 reachable=set();pending=seeds[:]
 while pending:
  at=pending.pop()
  if at in reachable:continue
  reachable.add(at);pending+=edges.get(at,[])
 return dict(reachable=reachable,invalid=invalid,falls_through_end=code_size in reachable)
def constant(i):
 op=i['opcode'];v=i['operand']
 if op=='ldnull':return ('null',0)
 if op in ['ldc.i4','ldc.i4.s','ldc.i8']:return ('integer',v)
 if op=='ldc.i4.m1':return ('integer',-1)
 if re.fullmatch(r'ldc\.i4\.[0-8]',op):return ('integer',int(op[-1]))
 return None
def ordinary_pointer(rid,pointers):
 return pointers[rid-1] if 1<=rid<=len(pointers) else None
def classify_constant(method,native):
 ins=[i for i in method['instructions'] if i['opcode']!='nop'];raw=bytes.fromhex(native.get('straight_leaf_hex') or '')
 if len(ins)==1 and ins[0]['opcode']=='ret' and method['ret']=='System.Void' and raw in [bytes.fromhex('c3'),bytes.fromhex('c20000'),bytes.fromhex('6690c3')]:return dict(status='DIRECT_NATIVE_EMPTY_MATCH',cil_constant=None,native_constant=None)
 v=constant(ins[0]) if len(ins)==2 and ins[-1]['opcode']=='ret' else None
 if not v:return None
 n=None
 if raw in [bytes.fromhex('31c0c3'),bytes.fromhex('33c0c3')]:n=0
 elif raw==bytes.fromhex('32c0c3') and method['ret']=='System.Boolean':n=0
 elif len(raw)==6 and raw[0]==0xb8 and raw[-1]==0xc3:n=struct.unpack_from('<i',raw,1)[0]
 elif len(raw)==3 and raw[0]==0xb0 and raw[-1]==0xc3 and method['ret']=='System.Boolean':n=raw[1]
 if n is None:return dict(status='TINY_BODY_REQUIRES_NATIVE_REVIEW',cil_constant=v,native_constant=None)
 if method['ret'] not in ['System.Boolean','System.Int32','System.UInt32'] and v[0]!='null':return dict(status='NATIVE_RETURN_ABI_NOT_CLASSIFIED',cil_constant=v,native_constant=n)
 # A full zeroing EAX instruction also zeros RAX, including a null reference.
 if v[0]=='null' and raw not in [bytes.fromhex('31c0c3'),bytes.fromhex('33c0c3')]:return dict(status='NATIVE_RETURN_ABI_NOT_CLASSIFIED',cil_constant=v,native_constant=n)
 return dict(status='DIRECT_NATIVE_CONSTANT_MATCH' if v[1]==n else 'DIRECT_NATIVE_CONSTANT_MISMATCH',cil_constant=v,native_constant=n)
def controls():
 def i(o,op,v=None):return dict(offset=o,opcode=op,operand=v)
 def m(ins,handlers=None,ret='System.Void'):return dict(instructions=ins,handlers=handlers or [],ret=ret)
 results=[]
 def check(name,okay):
  assert okay,name;results.append(dict(name=name,status='EXPECTED_RESULT'))
 x=m([i(0,'br',dict(target=7)),i(5,'ldstr','Invalid instruction: dead'),i(6,'pop'),i(7,'ret')]);g=cfg(x,8);check('UNREACHABLE_MARKER_IS_NOT_REACHABLE',5 not in g['reachable'] and not g['invalid'])
 x=m([i(0,'switch',dict(targets=[7])),i(5,'ret'),i(7,'ldstr','Invalid instruction: switch'),i(8,'pop'),i(9,'ret')]);check('SWITCH_CASE_MARKER_IS_REACHABLE',7 in cfg(x,10)['reachable'])
 x=m([i(0,'ret'),i(1,'ldstr','Invalid instruction: handler'),i(2,'pop'),i(3,'endfinally')],[dict(tryStart=0,tryEnd=1,handlerStart=1,handlerEnd=4,filterStart=None)]);check('EH_HANDLER_ENTRY_IS_NOT_IGNORED',1 in cfg(x,4)['reachable'])
 x=m([i(0,'br',dict(target=2)),i(5,'ret')]);check('BAD_BRANCH_BOUNDARY_IS_REJECTED',bool(cfg(x,6)['invalid']))
 x=m([i(0,'ret')],[dict(tryStart=0,tryEnd=7,handlerStart=0,handlerEnd=1)]);check('BAD_EH_RANGE_IS_REJECTED',bool(cfg(x,1)['invalid']))
 check('REACHABLE_END_WITHOUT_TERMINATOR_IS_RECORDED',cfg(m([i(0,'ldstr','x'),i(5,'pop')]),6)['falls_through_end'])
 x=m([i(0,'ldc.i4',7),i(5,'ret')],ret='System.Int32');check('LEGITIMATE_CONSTANT_NOT_CALLED_BUG',classify_constant(x,dict(straight_leaf_hex='b807000000c3'))['status']=='DIRECT_NATIVE_CONSTANT_MATCH');check('WRONG_NATIVE_CONSTANT_IS_DETECTED',classify_constant(x,dict(straight_leaf_hex='b808000000c3'))['status']=='DIRECT_NATIVE_CONSTANT_MISMATCH')
 check('UNSUPPORTED_NATIVE_BODY_IS_NOT_PROMOTED',classify_constant(x,dict(straight_leaf_hex='e800000000c3'))['status']=='TINY_BODY_REQUIRES_NATIVE_REVIEW')
 check('ORIGINAL_EMPTY_RET_IMM_ZERO_IS_NOT_DAMAGE',classify_constant(m([i(0,'ret')]),dict(straight_leaf_hex='c20000'))['status']=='DIRECT_NATIVE_EMPTY_MATCH')
 check('ORIGINAL_NOP_RET_IS_NOT_DAMAGE',classify_constant(m([i(0,'ret')]),dict(straight_leaf_hex='6690c3'))['status']=='DIRECT_NATIVE_EMPTY_MATCH')
 check('NONEMPTY_NATIVE_CALL_NOT_ACCEPTED_AS_EMPTY',classify_constant(m([i(0,'ret')]),dict(straight_leaf_hex='e800000000c3')) is None)
 check('INJECTED_RID_NEVER_READS_BEYOND_NATIVE_TABLE',ordinary_pointer(2,[0x180302170]) is None)
 return dict(scope='THIRTEEN_CENSUS_FALSE_ACCEPT_FALSE_POSITIVE_CONTROLS_NOT_GAMEPLAY_PROOF',controls=results)
def main():
 out=O/(sys.argv[1] if len(sys.argv)>1 else 'run1');out.mkdir(exist_ok=False)
 data=(Q/'candidate9.dll').read_bytes();assert sha(data)==DIGEST;pe=dnfile.dnPE(data=data);assert len(pe.net.mdtables.MethodDef.rows)==2317
 roster=[f'0x{0x6000000|rid:08X}' for rid,r in enumerate(pe.net.mdtables.MethodDef.rows,1) if r.Rva];assert len(roster)==2297
 exporter=ROOT/'audit/bulk250-r1-evidence-39702928368/bulk250-r1-run/tools_bin/CilExporter/bin/CilExporter.dll';frozen=ROOT/'audit/bulk350-root-verification-2026-10-22/reviewed-tools/verify_types_reviewed.py';raw_reader=ROOT/'audit/native111-publication-2026-10-20/repository/scripts/actions/static_gates.py'
 assert sha(exporter.read_bytes())=='0a9964ecd2cfa0f5f7e57ecb41b5809c6f305070285734a36cdebc2747798dc0';assert sha(frozen.read_bytes())=='2bb0a8c229a63fd33269433759a587ff551c6fde19669bafc506033df9b63d14'
 cmd=[str(ROOT/'.local/dotnet10/dotnet.exe'),str(exporter),str(Q/'candidate9.dll'),str(out/'ALL-CIL.json'),str(ROOT/'audit/bulk350-root-verification-2026-10-22/authentic-support'),','.join(roster)];p=subprocess.run(cmd,capture_output=True,timeout=180);(out/'export.stdout.log').write_bytes(p.stdout);(out/'export.stderr.log').write_bytes(p.stderr);assert p.returncode==0
 cil=read(out/'ALL-CIL.json');assert cil==read(P/'ALL-CIL.json'),'Fresh immutable export differs from prior candidate projection';assert {m['token'] for m in cil['methods']}==set(roster)
 reader=load('census_raw_method_reader',raw_reader);v=load('unchanged_frozen_census',frozen);js(out/'CONTROLS.json',controls())
 raw_types=[]
 for m in cil['methods']:
  try:r=v.verify(m,cil['types'])
  except Exception as e:r=dict(token=m['token'],method=m['name'],status='FAIL_OR_UNSUPPORTED',error=str(e))
  raw_types.append(r)
 assert raw_types==read(P/'FROZEN-RAW.json')['results'],'Fresh frozen results changed'
 facts=read(P/'INTERFACE-FACTS.json');assert facts['candidate_sha256']==DIGEST;enriched=copy.deepcopy(cil)
 nested={r.NestedClass.row_index:r.EnclosingClass.row_index for r in pe.net.mdtables.NestedClass.rows}
 def typedef(rid):
  r=pe.net.mdtables.TypeDef.rows[rid-1]
  return typedef(nested[rid])+'/'+str(r.TypeName) if rid in nested else (str(r.TypeNamespace)+'.' if str(r.TypeNamespace) else '')+str(r.TypeName)
 def typeref(rid):
  r=pe.net.mdtables.TypeRef.rows[rid-1]
  return typeref(r.ResolutionScope.row_index)+'/'+str(r.TypeName) if r.ResolutionScope.table and r.ResolutionScope.table.name=='TypeRef' else (str(r.TypeNamespace)+'.' if str(r.TypeNamespace) else '')+str(r.TypeName)
 # These exact InterfaceImpl edges have already been read from this same DLL.
 # Require the current table row identities, not an inferred assignability rule.
 for row in facts['rows']:
  md=pe.net.mdtables.InterfaceImpl.rows[row['interfaceimpl_row']-1];assert md.Interface.table.name in ['TypeDef','TypeRef'];assert row['owner']==typedef(md.Class.row_index);assert row['interface']==(typedef(md.Interface.row_index) if md.Interface.table.name=='TypeDef' else typeref(md.Interface.row_index))
  if row['applied_to_existing_owner']:
   entries=enriched['types'][row['owner']].setdefault('interfaces',[])
   if row['interface'] not in entries:entries.append(row['interface'])
 assert enriched==read(P/'ALL-CIL-WITH-INTERFACE-FACTS.json')
 typed=[]
 for m in enriched['methods']:
  try:r=v.verify(m,enriched['types'])
  except Exception as e:r=dict(token=m['token'],method=m['name'],status='FAIL_OR_UNSUPPORTED',error=str(e))
  typed.append(r)
 assert typed==read(P/'FROZEN-WITH-INTERFACE-FACTS.json')['results']
 js(out/'FRESH-TYPE-CENSUS.json',dict(candidate_sha256=DIGEST,checker_sha256=sha(frozen.read_bytes()),raw_counts=dict(collections.Counter(r['status'] for r in raw_types)),with_actual_interface_facts_counts=dict(collections.Counter(r['status'] for r in typed)),raw_results=raw_types,results=typed,first_issue_per_method_only=True))
 typed={r['token']:r for r in typed};jit_report=read(P/'jit-all/RESULT.json');assert jit_report['candidate_sha256']==DIGEST and len(jit_report['rows'])==2297;jit={r['token']:r for r in jit_report['rows']};assert set(jit)==set(roster)
 member_exit=read(P/'MEMBER-EXIT.json');member=read(P/'MEMBER-ACCESS-ALL.json');assert member_exit['candidate_sha256']==DIGEST and member_exit['exit_code']==0 and member['target_methods']==2297 and not member['rejected'] and not member['unresolved']
 registry=read(P/'METHOD-TEST-REGISTRY.json');assert registry['candidate_sha256']==DIGEST;runtime={r['token']:r for r in registry['rows']};changed=set(runtime)
 pcraw=(ROOT/'audit/offline-test-handoff-2026-10-14/payload/inputs/original-pc/GameAssembly.dll').read_bytes();assert sha(pcraw)=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d';pc=pefile.PE(data=pcraw,fast_load=True);pc.parse_data_directories(directories=[3]);base=pc.OPTIONAL_HEADER.ImageBase
 # Read the actual CodeGenModule declaration. The injected final Cpp2IL
 # helper is not an original method, even though its managed RID is valid.
 declared=[];needle=struct.pack('<Q',0x181b82d60);pos=0
 while True:
  a=pcraw.find(needle,pos)
  if a<0:break
  pos=a+1;begin=a-16;nameptr=struct.unpack_from('<Q',pcraw,begin)[0]
  try:nameat=pc.get_offset_from_rva(nameptr-base);name=pcraw[nameat:nameat+100].split(b'\0')[0].decode('ascii')
  except Exception:continue
  if name=='Assembly-CSharp.dll':declared.append(dict(module_name=name,file_offset=begin,method_pointer_count=struct.unpack_from('<I',pcraw,begin+8)[0],method_pointer_table='0x181b82d60',declaration_bytes=pcraw[begin:begin+24].hex()))
 assert len(declared)==1 and declared[0]['method_pointer_count']==2316
 native_count=declared[0]['method_pointer_count'];pointers=[int.from_bytes(pc.get_data(0x181b82d60-base+8*n,8),'little') for n in range(native_count)];assert all(not p or base<=p<base+pc.OPTIONAL_HEADER.SizeOfImage for p in pointers);unique=sorted(set(p for p in pointers if p));dis=Cs(CS_ARCH_X86,CS_MODE_64);dis.detail=True;js(out/'ORIGINAL-CODEGEN-MODULE.json',declared[0])
 def native_summary(rid):
  entry=ordinary_pointer(rid,pointers)
  if entry is None:return dict(entry=None,status='INJECTED_METHOD_OUTSIDE_ORIGINAL_CODEGEN_TABLE',instructions=0,straight_leaf_hex=None)
  if not entry:return dict(entry=None,status='NO_ORDINARY_NATIVE_POINTER_GENERIC_BINDING_REQUIRED')
  k=bisect.bisect_right(unique,entry);end=min(unique[k] if k<len(unique) else entry+4096,entry+8192);pending=[entry];ins={};calls=[];outside=[];indirect=[]
  while pending and len(ins)<1000:
   at=pending.pop()
   while entry<=at<end and at not in ins and len(ins)<1000:
    got=list(dis.disasm(pc.get_data(at-base,15),at,count=1))
    if not got:break
    i=got[0];ins[at]=i
    if i.mnemonic=='call':calls.append(hex(i.operands[0].imm) if i.operands[0].type==X86_OP_IMM else i.op_str)
    if i.group(CS_GRP_RET) or i.mnemonic in ['int3','ud2']:break
    if i.group(CS_GRP_JUMP):
     if i.operands[0].type==X86_OP_IMM:
      t=i.operands[0].imm
      if entry<=t<end:pending.append(t)
      else:outside.append(dict(source=hex(at),target=hex(t)))
     else:indirect.append(dict(source=hex(at),operand=i.op_str))
     if i.mnemonic=='jmp':break
    at+=i.size
  ordered=sorted(ins.values(),key=lambda x:x.address);straight=bool(ordered and ordered[0].address==entry and ordered[-1].mnemonic=='ret' and not calls and not outside and not indirect and all(a.address+a.size==b.address for a,b in zip(ordered,ordered[1:])))
  return dict(entry=hex(entry),scope='BOUNDED_NATIVE_CFG_LEAD_ONLY_NOT_GENERAL_EQUIVALENCE',instructions=len(ins),calls=sorted(set(calls)),external_tails=outside,indirect_jumps=indirect,limit_reached=len(ins)>=1000,straight_leaf_hex=b''.join(i.bytes for i in ordered).hex() if straight and len(ordered)<=5 else None,decoded_instructions=[dict(va=hex(i.address),bytes=i.bytes.hex(),opcode=i.mnemonic,operand=i.op_str) for i in ordered])
 rows=[];all_edges=[];unknown_calls=[];raw_bodies=[]
 for m in cil['methods']:
  token=m['token'];rid=int(token,16)&0xffffff;md=pe.net.mdtables.MethodDef.rows[rid-1];at=pe.get_offset_from_rva(md.Rva);info=reader.parse_raw_method_body_span(data,at);body=data[at+info[1]:at+info[1]+info[2]];assert len(body)==info[2]
  if 'il_sha256' in jit[token]:assert sha(body)==jit[token]['il_sha256'],('Cached JIT not bound to current body',token)
  graph=cfg(m,info[2]);issues=[];markers=[];small=[i for i in m['instructions'] if i['opcode']!='nop'];prefix=small
  while len(prefix)>=2 and prefix[0]['opcode']=='ldstr' and prefix[1]['opcode']=='pop' and isinstance(prefix[0]['operand'],str) and MARKER.search(prefix[0]['operand']):prefix=prefix[2:]
  for i in m['instructions']:
   if i['opcode']=='ldstr' and isinstance(i['operand'],str) and MARKER.search(i['operand']):markers.append(dict(offset=i['offset'],message=i['operand'],conservatively_reachable=i['offset'] in graph['reachable']))
   if i['opcode'] in ['call','callvirt','newobj','jmp','ldftn','ldvirtftn']:
    n=i['offset']+(2 if body[i['offset']]==0xfe else 1);ref=int.from_bytes(body[n:n+4],'little');target=ref
    if ref>>24==0x2b:
     ms=pe.net.mdtables.MethodSpec.rows[(ref&0xffffff)-1];target=(0x6000000|ms.Method.row_index) if ms.Method.table.name=='MethodDef' else 0
    if target>>24==6 and f'0x{target:08X}' in jit:all_edges.append(dict(caller=token,offset=i['offset'],kind=i['opcode'],target=f'0x{target:08X}',metadata_token=f'0x{ref:08X}'))
    else:unknown_calls.append(dict(caller=token,offset=i['offset'],kind=i['opcode'],owner=i['operand'].get('owner'),name=i['operand'].get('name'),metadata_token=f'0x{ref:08X}',scope='EXTERNAL_OR_MEMBERREF_DISPATCH_NOT_RESOLVED_IN_THIS_GRAPH'))
  if graph['invalid']:issues.append(dict(kind='STRUCTURAL_BRANCH_OR_EH_ERROR',classification='DEFINITE_STRUCTURE_FAILURE',details=graph['invalid']))
  if graph['falls_through_end']:issues.append(dict(kind='REACHABLE_LEXICAL_END',classification='STRUCTURE_REVIEW_REQUIRED',note='Conservative EH entry graph; this is not a game bug count'))
  if typed[token]['status']!='PASS':issues.append(dict(kind='FROZEN_TYPE_FAIL_OR_UNSUPPORTED',classification='FAIL_OR_UNSUPPORTED_RETAINED',error=typed[token].get('error')))
  if jit[token]['status']!='JIT_PREPARED_NO_EXECUTION':issues.append(dict(kind='EXACT_CLR_JIT_UNSUPPORTED_OR_FAILURE',classification=jit[token]['status'],error=jit[token].get('error')))
  reachable_markers=[r for r in markers if r['conservatively_reachable']]
  if reachable_markers:issues.append(dict(kind='REACHABLE_DECOMPILER_MARKER',classification='SEMANTIC_INVESTIGATION_LEAD',sites=reachable_markers))
  exceptions=[i['operand']['owner'] for i in m['instructions'] if i['opcode']=='newobj' and i['operand']['owner'] in ['System.NotImplementedException','System.NotSupportedException']]
  if exceptions:issues.append(dict(kind='EXPLICIT_NOT_IMPLEMENTED_OR_SUPPORTED_THROW',classification='MAY_BE_INTENTIONAL_OR_PLACEHOLDER_REQUIRES_NATIVE_EVIDENCE',types=exceptions))
  tiny=(len(prefix)==1 and prefix[0]['opcode']=='ret') or (len(prefix)==2 and prefix[-1]['opcode']=='ret' and constant(prefix[0]) is not None)
  native=None;constant_result=None
  if tiny:
   native=native_summary(rid);constant_result=classify_constant(dict(m,instructions=prefix),native)
   if constant_result and constant_result['status']=='DIRECT_NATIVE_CONSTANT_MISMATCH':issues.append(dict(kind='DIRECT_NATIVE_CONSTANT_MISMATCH',classification='DIRECT_NATIVE_LEAF_MISMATCH',details=constant_result))
   elif constant_result and constant_result['status'] in ['DIRECT_NATIVE_CONSTANT_MATCH','DIRECT_NATIVE_EMPTY_MATCH']:pass
   elif native.get('status')=='INJECTED_METHOD_OUTSIDE_ORIGINAL_CODEGEN_TABLE':pass
   elif len(prefix)==1 and prefix[0]['opcode']=='ret' and native.get('calls'):
    issues.append(dict(kind='EMPTY_CIL_WITH_NONEMPTY_NATIVE_CALLER',classification='CONFIRMED_BODY_GAP_PORTING_INTENT_AND_RUNTIME_IMPACT_REVIEW_REQUIRED',note='Actual CIL is empty; original native has calls. Do not silently approve a recovery stub or claim its device symptom without execution.'))
   else:issues.append(dict(kind='TINY_OR_EMPTY_BODY',classification='NATIVE_REVIEW_LEAD_NOT_AUTOMATIC_BUG',note='Original getters, empty callbacks and constants may be legitimate'))
  profile=runtime.get(token,{}).get('runtime_profile');raw_bodies.append(dict(token=token,rva=md.Rva,file_offset=at,code_size=info[2],header_size=info[1],body_span=info[0],raw_il_sha256=sha(body),max_stack_header=info[3],local_signature=info[4],eh_clause_count=info[7]))
  rows.append(dict(token=token,name=m['name'],owner=m['owner'],code_bytes=info[2],r19_target=token in changed,unity_lifecycle_name_candidate=bool(re.search(r'::(?:Awake|Start|Update|LateUpdate|FixedUpdate|OnEnable|OnDisable|OnDestroy)\(',m['name'])),frozen_status=typed[token]['status'],jit_status=jit[token]['status'],markers=markers,tiny_native_summary=native,tiny_constant_result=constant_result,current_r19_finite_profile=profile,historical_profile_binding='NOT_COMPREHENSIVELY_REBOUND_IN_THIS_CENSUS',issues=issues,issues_are_not_a_bug_count=True))
 # Internal metadata call edges form an underapproximation, not Unity dispatch.
 reverse=collections.defaultdict(list)
 for e in all_edges:reverse[e['target']].append(e)
 for row in rows:row['direct_internal_callers']=reverse[row['token']]
 findings=[r for r in rows if r['issues']];findings.sort(key=lambda r:(not r['unity_lifecycle_name_candidate'],not r['r19_target'],r['token']))
 js(out/'METHOD-BODIES.json',dict(candidate_sha256=DIGEST,methods=raw_bodies));js(out/'ALL-METHOD-STATUS.json',dict(candidate_sha256=DIGEST,methods=2297,rows=rows));js(out/'FINDINGS.json',dict(candidate_sha256=DIGEST,deduplicated_methods=len(findings),rows=findings,not_a_bug_count=True));js(out/'INTERNAL-CALL-GRAPH.json',dict(candidate_sha256=DIGEST,edges=all_edges,unresolved_or_external_calls=unknown_calls,virtual_unity_reflection_dispatch_complete=False));
 with (out/'FINDINGS.csv').open('w',encoding='utf-8-sig',newline='') as f:
  writer=csv.writer(f);writer.writerow(['token','name','R19_target','lifecycle_name_candidate','frozen','JIT','issue_categories','direct_internal_callers'])
  for r in findings:writer.writerow([r['token'],r['name'],r['r19_target'],r['unity_lifecycle_name_candidate'],r['frozen_status'],r['jit_status'],';'.join(i['kind'] for i in r['issues']),';'.join(e['caller'] for e in r['direct_internal_callers'])])
 category=collections.Counter(i['kind'] for r in rows for i in r['issues']);runtime_count=sum(bool(r['current_r19_finite_profile']) for r in rows);constant_counts=collections.Counter(r['tiny_constant_result']['status'] for r in rows if r['tiny_constant_result'])
 prior=ROOT/'audit/repo-native113/Recovery/Startup-R19-Batch100-2026-10-25';linux=read(prior/'ACTUAL-COMPILER-40195582520.json');apple=read(prior/'ACTUAL-APPLE-40197688048.json');assert linux['candidate_sha256']==DIGEST and apple['actual']['candidate_sha256']==DIGEST
 sources=[Q/'candidate9.dll',P/'ALL-CIL.json',P/'jit-all/RESULT.json',P/'MEMBER-EXIT.json',P/'MEMBER-ACCESS-ALL.json',P/'INTERFACE-FACTS.json',P/'METHOD-TEST-REGISTRY.json',P/'PENDING-RUNTIME-PROFILES.json',exporter,frozen,raw_reader,prior/'ACTUAL-COMPILER-40195582520.json',prior/'ACTUAL-APPLE-40197688048.json',O/'census.py']
 js(out/'INPUTS.json',[dict(path=p.relative_to(ROOT).as_posix(),bytes=p.stat().st_size,sha256=sha(p.read_bytes())) for p in sources])
 summary=dict(scope='CURRENT_EXACT_CANDIDATE_WHOLE_MANAGED_ASSEMBLY_READ_ONLY_CENSUS_NOT_EXHAUSTIVE_GAME_BUG_PROOF',candidate_sha256=DIGEST,methoddefs=2317,original_pc_method_pointer_count=native_count,injected_final_method_not_given_native_pointer=True,bodies=2297,fresh_export_identical_to_previous=True,fresh_raw_type_counts=dict(collections.Counter(r['status'] for r in raw_types)),fresh_type_counts_with_exact_interface_facts=dict(collections.Counter(r['status'] for r in typed.values())),exact_jit_reused_and_raw_il_hash_rechecked=dict(collections.Counter(r['jit_status'] for r in rows)),member_access_rejections=member['rejected'],member_access_unresolved=member['unresolved'],finding_categories=dict(category),deduplicated_finding_methods=len(findings),tiny_constant_categories=dict(constant_counts),marker_methods=sum(bool(r['markers']) for r in rows),marker_sites=sum(len(r['markers']) for r in rows),unreachable_marker_sites=sum(not x['conservatively_reachable'] for r in rows for x in r['markers']),iterator_reset_marker_methods=sum(bool(r['markers']) and '::System.Collections.IEnumerator.Reset(' in r['name'] for r in rows),other_marker_methods=sum(bool(r['markers']) and '::System.Collections.IEnumerator.Reset(' not in r['name'] for r in rows),direct_internal_call_edges=len(all_edges),unresolved_or_external_call_sites=len(unknown_calls),current_r19_finite_profile_methods=runtime_count,current_r19_pending_profiles=registry['pending_runtime_profiles'],other_method_histories='RETAINED_BUT_NOT_COMPREHENSIVELY_REBOUND_TO_CURRENT_CANDIDATE_HERE',current_candidate_linux_syntax_run=40195582520,current_candidate_apple_syntax_run=40197688048,current_candidate_generated_source_count=1029,current_candidate_new_unity_scene_execution=False,current_candidate_linked_player=False,current_candidate_ipa=None,ipad_startup_black_flicker_audio_issue='OPEN_DEVICE_ACCEPTANCE_REQUIRED',all_real_game_bugs_count=None,mutations_to_candidate_or_frozen_tools=0)
 js(out/'SUMMARY.json',summary);print(json.dumps(summary,indent=2))
if __name__=='__main__':main()
