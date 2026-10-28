"""Admit the post-runtime correction for Linux Editor diagnosis, never an IPA.

The R21 transport/bootstrap remains separate. The R22 wrong-ABI finite report is
explicitly insufficient and is not used as semantic admission evidence.
"""
from pathlib import Path
import argparse,json,zipfile
import bind_startup_r21_diagnostic as bootstrap
require=bootstrap.require;sha=bootstrap.sha
CANDIDATE='1d244776ee85083c2ed298c4f749f96b6c5ae820a78d50b0ce47480691775122'
PARENT='15e0150337a14f1613321385af2d9b1997abd1bc255d56abe4a4ce2c5f7ac2cf'
CHECKPOINT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R23-Runtime-Correction-2026-10-28'
TARGETS={'0x060002AB','0x06000309','0x0600030A'}
def validate_checkpoint(root=CHECKPOINT):
 root=Path(root).resolve();entries=json.loads((root/'MANIFEST.json').read_bytes());names={p.relative_to(root).as_posix()for p in root.rglob('*')if p.is_file()and p!=root/'MANIFEST.json'}
 require(len(entries)==len({r['file']for r in entries})and names=={r['file']for r in entries},'R23 checkpoint inventory differs')
 for r in entries:
  p=root/r['file'];require(p.resolve().is_relative_to(root)and not p.is_symlink(),'Unsafe evidence path');b=p.read_bytes();require(len(b)==r['bytes']and sha(b)==r['sha256'],'Changed R23 evidence: '+r['file'])
 read=lambda n:json.loads((root/n).read_bytes())
 construction=read('CONSTRUCTION.json');require(construction['candidate_sha256']==CANDIDATE and construction['parent_sha256']==PARENT and {r['token']for r in construction['changes']}==TARGETS and construction['additional_distinct_source_corrections']==10,'Wrong correction input/scope')
 require(construction['prior_completed_authoring_cohort']==dict(batch='R22',positions=112,methods=5),'Do not claim a new hundred-position batch')
 repeat=read('REPEATED-CONSTRUCTION.json');require(repeat['candidate_sha256']==CANDIDATE and repeat['two_fresh_constructions_identical'],'Candidate construction not repeatable')
 isolation=read('ISOLATION.json');require(isolation['candidate_sha256']==CANDIDATE and isolation['parent_sha256']==PARENT and isolation['pass_'] and isolation['changed_methods']==3 and isolation['unchanged_methoddefs']==2314,'Target isolation differs')
 require(isolation['old_code_slots_preserved']and isolation['old_metadata_rows_and_heaps_preserved']and isolation['method_flags_and_section_permissions_unchanged']and all(r['rejected']for r in isolation['negative_controls'])and len(isolation['negative_controls'])==3,'Old body/metadata/flags boundary failed')
 frozen=read('FROZEN-RAW.json');require(frozen['candidate_sha256']==CANDIDATE and frozen['checker_sha256']==bootstrap.parent.CHECKER and frozen['total']==2297,'Wrong strict checker');by={r['token']:r for r in frozen['results']};require(all(by[t]['status']=='PASS'for t in TARGETS),'Corrected target fails strict type flow')
 require(read('MEMBER-EXIT.json')['exit_code']==0,'Member access fails');jit=read('EXACT-JIT.json');require(jit['candidate_sha256']==CANDIDATE and jit['counts']=={'JIT_PREPARED_NO_EXECUTION':3},'Exact method JIT preparation differs')
 board=read('BOARD-NATIVE-COMPARE.json');require(board['candidate_sha256']==CANDIDATE and board['records']==128 and not board['differences']and not board['callback_errors'],'Corrected five-caller ABI/state comparison fails')
 require(len(board['exception_limits'])==3 and all(r['status']=='NATIVE_WINDOWS_EXCEPTION_UNWIND_NOT_EXECUTED'and r['managed_finally_observed']for r in board['exception_limits']),'Native SEH limitation must remain explicit')
 device=read('DEVICE-NATIVE-COMPARE.json');require(device['candidate_sha256']==CANDIDATE and device['cases']==124 and not device['differences']and not device['callback_errors'],'Device finite caller comparison fails')
 apis=read('ORIGINAL-DIRECT-API-IDENTITIES.json');require(any(r['owner']=='UnityEngine.Animator'and r['name']=='SetBool'and r['args']==2 for r in apis['resolved']['0x1812dba00']),'Exact original Animator ABI proof missing')
 controls=read('PHYSICAL-CONTROLS.json');require(controls['candidate_sha256']==CANDIDATE and controls['controls']==8 and controls['semantic_controls']==7 and controls['strict_type_controls']==1 and controls['all_detected'],'Scoped physical controls differ')
 require(all(r['normal_execution_differences']>0 for r in controls['results']if r['kind']=='SEMANTIC')and all(not r['stock_clr_rejected']for r in controls['results']if r['kind']=='STRICT_TYPE'),'Do not turn permissive CLR into an invented rejection')
 linux=read('ACTUAL-COMPILER-40612953895.json');require(linux['candidate_sha256']==CANDIDATE and linux['run_id']==40612953895 and linux['source_commit']=='8df1d5acb420070a973701d0f055bae5a09697a4'and linux['pass_']and linux['conversion_exit']==0 and linux['raw_generated_sources_rehashed']==linux['raw_source_logs_rechecked']==1029 and linux['raw_errors']==0,'Fresh exact correction compiler readback missing')
 progress=read('PROGRESS.json');require(progress['candidate_sha256']==CANDIDATE and progress['qualified_methods']==0 and not progress['whole_game_accepted']and not progress['ipad_acceptance']and progress['ipa']is None,'Retain real product gates')
 with zipfile.ZipFile(root/'STARTUP-R23-CORRECTION-CANDIDATE.zip')as z:
  require(set(z.namelist())=={'STARTUP-R23-CORRECTION.dll','MANIFEST.json'}and z.testzip()is None,'Candidate archive differs');data=z.read('STARTUP-R23-CORRECTION.dll');require(json.loads(z.read('MANIFEST.json'))==[dict(file='STARTUP-R23-CORRECTION.dll',bytes=len(data),sha256=sha(data))],'Candidate inner manifest differs')
 require(sha(data)==CANDIDATE and len(data)==24095232,'Wrong actual corrected image')
 return data,dict(normal_board_bank_comparisons=128,normal_device_comparisons=124,managed_finally_cases=3,native_exception_unwind_executed=False,old_r22_wrong_abi_report_used_for_admission=False,physical_semantic_controls=7,strict_type_controls=1,fresh_linux_compiler_run=40612953895,ipa_promotion_allowed=False)
def bind(output,source):
 data,proof=validate_checkpoint();bootstrap.bind(output,source);out=Path(output).resolve();e=out/'evidence';target=out/'candidate/Assembly-CSharp-bulk401.dll';require(sha(target.read_bytes())==bootstrap.CANDIDATE,'Bootstrap transport image differs')
 provenance=e/'PROVENANCE.json';backup=e/'PARENT-R21-PROVENANCE.json';require(not backup.exists(),'Parent evidence already present');provenance.rename(backup);target.write_bytes(data)
 result=dict(status='STARTUP_R23_RESTRICTED_LINUX_EDITOR_COMPLEX_LEVEL_DIAGNOSTIC_ONLY',source_commit=source,parent_sha256=PARENT,candidate_sha256=CANDIDATE,outputs={'unlinked':{'sha256':CANDIDATE}},changed_methods=3,source_corrections=10,prior_authoring_cohort_positions=112,proof=proof,parent_compiler_results_apply_to_child=False,child_unity_gameplay='NOT_YET_OBSERVED',child_apple_compilation='SEPARATE_PENDING_GATE',whole_game_semantics='NOT_PROVEN',child_device_execution_performed=False,ipa_promotion_allowed=False,open_observed_defects=['Actual Device.InjuryStatusUpdate NRE cause','Loaded VisualScripting ComponentHolderProtocol default-return methods'])
 js=lambda p,v:p.write_bytes((json.dumps(v,indent=2)+'\n').encode())
 js(provenance,result);js(e/'candidate-inspection.txt',result);js(e/'PARENT-R22-CORRECTION-PROVENANCE.json',dict(candidate_sha256=PARENT,role='EXACT_CONSTRUCTION_PARENT_NOT_INHERITED_SEMANTIC_PROOF',actual_failed_run=40456165422,old_board_update_finite_claim_sufficient=False))
 for name in ['ISOLATION.json','EXACT-JIT.json','PROGRESS.json','ACTUAL-COMPILER-40612953895.json']:(e/('R23-'+name)).write_bytes((CHECKPOINT/name).read_bytes())
 return result
if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--output');p.add_argument('--source');p.add_argument('--validate-only',action='store_true');a=p.parse_args()
 if a.validate_only:
  data,proof=validate_checkpoint();print(json.dumps(dict(candidate_sha256=sha(data),bytes=len(data),proof=proof)))
 else:require(bool(a.output and a.source),'Output/source required');print(json.dumps(bind(a.output,a.source)))
