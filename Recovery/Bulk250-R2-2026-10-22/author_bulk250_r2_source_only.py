"""Read immutable source evidence, preserve prior reports and author two source repairs.

Never construct a candidate, run a verifier/fixture/compiler or execute a game method.
"""
from pathlib import Path
import json,sys,hashlib,shutil
sys.dont_write_bytecode=True
R=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile,pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
S=R/'audit/native-bulk250-r1-codex-2026-10-22'
O=R/'audit/native-bulk250-r2-codex-2026-10-22'
E=R/'audit/native116-bulk250-r2-codex-2026-10-22'
read=lambda p:json.loads(p.read_text('utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,ensure_ascii=False,indent=2)+'\n')
O.mkdir(exist_ok=False);E.mkdir(exist_ok=False)
for f in read(S/'SOURCE-MANIFEST.json')['files']:
    p=S/f['path'];data=p.read_bytes()
    if sha(data)!=f['sha256']:raise ValueError('Frozen source delivery differs: '+f['path'])
    q=O/f['path'];q.parent.mkdir(parents=True,exist_ok=True);q.write_bytes(data)
parent=R/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'
game=R/'audit/offline-test-handoff-2026-10-14/payload/inputs/original-pc/GameAssembly.dll'
m=dnfile.dnPE(str(parent));md=m.net.mdtables;pe=pefile.PE(str(game));base=pe.OPTIONAL_HEADER.ImageBase
maps=read(R/'audit/offline-test-handoff-2026-10-14/payload/inputs/maps/native-method-map.json')
dec=Cs(CS_ARCH_X86,CS_MODE_64)
proof=[]
for rid,owner,field in [(0x158,'BoardConfig',0x198),(0x303,'Device',0x488)]:
    row=md.MethodDef.rows[rid-1];b=CilMethodBody(CilMethodBodyReaderBytes(m.get_data(row.Rva,128)))
    start=int(next(x['va'] for x in maps if int(x['token'],16)==0x06000000+rid and x['type']==owner),16)
    extent=next((e for e in pe.DIRECTORY_ENTRY_EXCEPTION if base+e.struct.BeginAddress==start),None)
    end=base+extent.struct.EndAddress if extent else min(int(x['va'],16) for x in maps if int(x['va'],16)>start)
    raw=pe.get_data(start-base,end-start);fr=md.Field.rows[field-1]
    p=dict(token=f'0x{0x06000000+rid:08X}',owner=owner,name=str(row.Name),signature=row.Signature.value.hex(),public=bool(row.Flags.mdPublic),cil_header_size=b.header_size,cil=[dict(offset=i.offset-b.header_size,opcode=str(i.opcode),operand=str(i.operand)) for i in b.instructions],field=dict(token=f'0x{0x04000000+field:08X}',name=str(fr.Name),signature=fr.Signature.value.hex(),private=bool(fr.Flags.fdPrivate)),native=dict(start=hex(start),end=hex(end),bytes=raw.hex(),sha256=sha(raw),extent_source='pdata' if extent else 'next distinct method pointer',instructions=[dict(address=hex(i.address),bytes=i.bytes.hex(),mnemonic=i.mnemonic,operand=i.op_str) for i in dec.disasm(raw,start)]))
    proof.append(p)
dump(E/'PUBLIC-GETTER-SOURCE-EVIDENCE.json',dict(scope='IMMUTABLE_CIL_METADATA_NATIVE_READ_ONLY',parent_sha256=sha(parent.read_bytes()),gameassembly_sha256=sha(game.read_bytes()),methods=proof,candidate_constructed=False,project_tests_executed=False))
for p in proof:
    print(p['token'],p['owner'],p['name'],'public',p['public'])
    print(p['cil'])
    print(p['native']['instructions'])
changes=[]
p=O/'N116.MethodBodies.cs';s=p.read_text('utf-8')
for method,token,old,new,n in [('N116_CreateEnemy','0x0600001C','.Field(0x04000198)','.Virtual(0x06000158)',2),('N116_DisplayBannedDevice','0x06000183','.Field(0x04000488)','.Virtual(0x06000303)',1)]:
    a=s.index('    private static byte[] '+method+'(');z=s.find('\n    private static ',a+1);z=len(s) if z<0 else z
    body=s[a:z]
    if body.count(old)!=n:raise ValueError((method,'unexpected source shape'))
    after=body.replace(old,new);s=s[:a]+after+s[z:]
    changes.append(dict(token=token,emitter=method,source_file='N116.MethodBodies.cs',sites=n,old_source_sha256=sha(body.encode()),new_source_sha256=sha(after.encode()),old_instruction=old,new_instruction=new,reason='Use existing public accessor whose immutable CIL/native body returns the same field; preserve read count, receiver evaluation, null failure and callback order.'))
write(p,s)
plan=read(O/'IMPLEMENTATION-PLAN.json');r1tokens=plan['revision_changed_method_tokens'];tokens=[x['token'] for x in changes]
plan.update(schema='bulk250-r2-source-implementation-v1',state='SOURCE_REPAIRS_WRITTEN_PENDING_AGY_EXECUTION',source_revision='Bulk250-R2',candidate=None,new_unique_source_methods_this_turn=0,new_qualified_methods=0,new_accepted_methods=0,root_executed_project_tests=False,previous_revision_changed_method_tokens=r1tokens,revision_changed_method_tokens=tokens,revision_changed_method_count=2,cumulative_revision_changed_method_tokens=r1tokens+tokens,cumulative_revision_changed_method_count=10,source_predecessor=S.relative_to(R).as_posix(),comparison_to_bulk250_r1={'candidate_sha256':'dceb44b834889ec6ebcd36baaadbf0b2062fc41982f3d3b64144461a99338996','expected_changed_bodies':2,'expected_unchanged_bodies':2315,'not_executed':True},comparison_to_identified_bulk250={'candidate_sha256':'ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8','expected_changed_bodies':10,'expected_unchanged_bodies':2307,'not_executed':True},comparison_to_identified_bulk200={'candidate_sha256':'667e032fe7512f471bcc891c80fc81fd22d27e0ce0ae675bb226f7bbe0e00bc0','expected_changed_bodies':52,'expected_unchanged_bodies':2265,'not_executed':True},previous_batch_gate='R1_MEMBER_ACCESS_FAIL_RETEST_R2',next_bulk300_execution='HOLD_UNTIL_PREVIOUS_BATCH_REVIEW',metadata_changed_by_revision=False)
dump(O/'IMPLEMENTATION-PLAN.json',plan)
dump(O/'SOURCE-LINEAGE.json',dict(schema='bulk250-r2-source-lineage-v1',parent_sha256=plan['parent_sha256'],candidate_parent_is_native93=True,source_predecessor=S.relative_to(R).as_posix(),r1_source_manifest_sha256=sha((S/'SOURCE-MANIFEST.json').read_bytes()),r1_candidate_sha256='dceb44b834889ec6ebcd36baaadbf0b2062fc41982f3d3b64144461a99338996',expected_total_method_bodies=250,new_revision_tokens=tokens,cumulative_revision_tokens=r1tokens+tokens,metadata_changed=False,tests_inherited=False,new_acceptance_inherited=False,candidate=None))
dump(E/'SOURCE-REPAIR-CHANGELOG-R2.json',dict(schema='bulk250-r2-source-repairs-v1',state='SOURCE_ONLY_NOT_TESTED',changed_existing_method_sources=2,replaced_sites=3,changes=changes,metadata_changes=0,candidate_constructed=False,project_tests_executed=False,new_formal_acceptance=0))
# Existing result reading/archival only; no verifier or compiler is called.
reports=S/'verification-actions-39703773021/bulk250-r1-run/results'
names=['construction1.json','construction2.json','BASELINE-CANDIDATE-COMPARISON.json','INDEPENDENT-ISOLATION-REPORT.json','MEMBER-ACCESS-180METHODS-AUTHENTIC.json','CONTROL-MEMBER-ACCESS-REJECT-PRIVATE-PREPAREUI-INSTANCE.json','EAGER-MEMBERREF-BULK250-AUTHENTIC.json']
out=E/'prior-run-39703773021';out.mkdir();entries=[]
for name in names:
    p=reports/name;shutil.copyfile(p,out/name);entries.append(dict(path=name,bytes=p.stat().st_size,sha256=sha(p.read_bytes())))
access=read(reports/'MEMBER-ACCESS-180METHODS-AUTHENTIC.json')
dump(E/'PRIOR-BATCH-RESULT-REVIEW-R1.json',dict(schema='bulk250-r1-result-review-v1',scope='EXISTING_REPORT_READ_ONLY_NOT_REEXECUTION',run_id=39703773021,reported_candidate_sha256='dceb44b834889ec6ebcd36baaadbf0b2062fc41982f3d3b64144461a99338996',previous_batch_acceptance='FAIL_MEMBER_ACCESS',member_access_report={k:access[k] for k in ['pass','target_methods','rejected','unresolved']},rejected_members=[x for x in access['members'] if x.get('allowed') is False],reported_type_flow_fixed=True,reported_clang={'game_tus':18,'errors':3463,'error_methods':137,'clean_methods':2130,'included_file_errors':44,'not_independently_reexecuted':True},report_claim_corrections=['v18 source registry has294 direct sources and87 remaining, not137;137 belongs to the tested250-source scope','Equality of error-method count and unwritten-source count does not establish token-set equality or250-target cleanliness','2130+137=2267 is a different denominator from2317 managed MethodDefs; explain unmapped/nonemitted methods and included-file attribution','Native/CIL behavior remains separate; no new formal semantic acceptance in this review'],required_next_reports=['Candidate-bound250-target vs failing-method token intersection and explicit unmapped/ambiguous cases','Observable normal/exception/native-vs-CIL cases for the preceding50 with genuine negative controls','Full baseline-pass/member-access-mutant-fail control forR2'],bulk300_testing='HOLD',files=entries))
for name in ['PUBLIC-GETTER-SOURCE-EVIDENCE.json','SOURCE-REPAIR-CHANGELOG-R2.json','PRIOR-BATCH-RESULT-REVIEW-R1.json']:shutil.copyfile(E/name,O/name)
print(json.dumps(dict(scope='SOURCE_AUTHORING_AND_PRIOR_REPORT_READING_ONLY',new_revision_methods=2,new_unique_method_sources=0,total_method_sources=250,candidate_constructed=False,project_tests_executed=False)))
