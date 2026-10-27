"""Admit one finitely checked R22 child for Linux Editor diagnosis only.

Parent Apple results are retained as parent evidence. A new child Apple check
is a separate gate for export/IPA; it is never inferred from this admission.
"""
from pathlib import Path
import argparse,hashlib,json,zipfile
import bind_startup_r21_diagnostic as parent
require=parent.require;sha=parent.sha
CANDIDATE='15e0150337a14f1613321385af2d9b1997abd1bc255d56abe4a4ce2c5f7ac2cf'
CHECKPOINT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R22-Board112-2026-10-27'
TARGETS={'0x060000E4','0x060000E7','0x060002A8','0x060002AB','0x060002C2'}

def validate_checkpoint(root=CHECKPOINT):
 root=Path(root).resolve();manifest=json.loads((root/'MANIFEST.json').read_bytes());names={p.relative_to(root).as_posix()for p in root.rglob('*')if p.is_file()and p!=root/'MANIFEST.json'}
 require(len(manifest)==len({r['file']for r in manifest})and names=={r['file']for r in manifest},'R22 checkpoint inventory mismatch')
 for r in manifest:
  p=root/r['file'];require(p.resolve().is_relative_to(root)and not p.is_symlink(),'Unsafe R22 evidence path');b=p.read_bytes();require(len(b)==r['bytes']and sha(b)==r['sha256'],'R22 evidence bytes differ: '+r['file'])
 read=lambda name:json.loads((root/name).read_bytes())
 construction=read('CONSTRUCTION.json');require(construction['candidate_sha256']==CANDIDATE and construction['parent_sha256']==parent.CANDIDATE and construction['new_unique_repair_positions']==112 and {r['token']for r in construction['changes']}==TARGETS,'Wrong R22 source scope')
 repeat=read('REPEATED-CONSTRUCTION.json');require(repeat['candidate_sha256']==CANDIDATE and repeat['two_fresh_constructions_identical'],'R22 repeated construction missing')
 isolation=read('ISOLATION.json');require(isolation['candidate_sha256']==CANDIDATE and isolation['parent_sha256']==parent.CANDIDATE and isolation['pass_']and isolation['changed_methods']==5 and isolation['unchanged_methoddefs']==2312,'R22 method boundary differs')
 require(isolation['old_code_slots_preserved']and isolation['old_metadata_rows_and_heaps_preserved']and isolation['method_flags_and_section_permissions_unchanged']and len(isolation['negative_controls'])==3 and all(r['rejected']for r in isolation['negative_controls']),'R22 old field/metadata/permission boundary failed')
 frozen=read('FROZEN-RAW.json');require(frozen['candidate_sha256']==CANDIDATE and frozen['checker_sha256']==parent.parent.CHECKER and frozen['total']==2297,'Frozen type checker identity differs');found={r['token']:r for r in frozen['results']};require(all(found[t]['status']=='PASS'for t in TARGETS),'A child target failed type/stack census')
 require(read('MEMBER-EXIT.json')['exit_code']==0,'R22 member access failed');jit=read('EXACT-JIT.json');require(jit['candidate_sha256']==CANDIDATE and jit['counts']=={'JIT_PREPARED_NO_EXECUTION':5},'R22 JIT preparation missing')
 native=read('NATIVE-COMPARE.json');require(native['candidate_sha256']==CANDIDATE and native['records']==128 and native['caller_byte_checks']==5 and not native['differences']and not native['callback_errors'],'R22 finite normal caller comparison failed')
 require(len(native['exception_limits'])==3 and all(r['status']=='NATIVE_WINDOWS_EXCEPTION_UNWIND_NOT_EXECUTED'and r['managed_finally_observed']for r in native['exception_limits']),'Preserve native exception limitation')
 controls=read('PHYSICAL-CONTROLS.json');require(controls['candidate_sha256']==CANDIDATE and controls['controls']==controls['rejected']==8 and controls['process_failures_counted_as_semantic_controls']==0,'Physical semantic controls incomplete')
 linux=read('ACTUAL-COMPILER-40452784098.json');require(linux['candidate_sha256']==CANDIDATE and linux['run_id']==40452784098 and linux['source_commit']=='ba78423afb2cefbde8e66d73cb4d1b92c0de34a0'and linux['pass_']and linux['conversion_exit']==0 and linux['raw_generated_sources_rehashed']==linux['raw_source_logs_rechecked']==1029 and linux['raw_errors']==0,'Fresh exact child Linux compiler/raw evidence missing')
 progress=read('PROGRESS.json');require(progress['candidate_sha256']==CANDIDATE and progress['qualified_methods']==0 and not progress['whole_game_accepted']and not progress['ipad_acceptance']and progress['ipa']is None,'Diagnostic must retain unresolved product gates')
 with zipfile.ZipFile(root/'STARTUP-R22-BOARD112-CANDIDATE.zip')as z:
  require(set(z.namelist())=={'STARTUP-R22-BOARD112.dll','MANIFEST.json'}and z.testzip()is None,'Wrong candidate ZIP');data=z.read('STARTUP-R22-BOARD112.dll');require(json.loads(z.read('MANIFEST.json'))==[dict(file='STARTUP-R22-BOARD112.dll',bytes=len(data),sha256=sha(data))],'Candidate inner byte manifest differs')
 require(len(data)==23656960 and sha(data)==CANDIDATE,'Wrong exact R22 game image')
 return data,dict(targets=sorted(TARGETS),normal_native_comparisons=128,managed_finally_cases=3,native_exception_unwind_executed=False,physical_semantic_controls=8,fresh_linux_compiler_run=40452784098,child_apple_result='SEPARATE_GATE_NOT_INFERRED_OR_REQUIRED_FOR_LINUX_EDITOR_EXECUTION',ipa_promotion_allowed=False)

def bind(output,source):
 data,proof=validate_checkpoint();parent.bind(output,source);out=Path(output).resolve();e=out/'evidence';target=out/'candidate/Assembly-CSharp-bulk401.dll';require(sha(target.read_bytes())==parent.CANDIDATE,'Parent output differs');provenance=e/'PROVENANCE.json';backup=e/'PARENT-R21-PROVENANCE.json';require(not backup.exists(),'Do not overwrite parent evidence');provenance.rename(backup);target.write_bytes(data)
 result=dict(status='STARTUP_R22_RESTRICTED_LINUX_EDITOR_AND_COMPLEX_GAMEPLAY_DIAGNOSTIC_ONLY',source_commit=source,parent_sha256=parent.CANDIDATE,candidate_sha256=CANDIDATE,outputs={'unlinked':{'sha256':CANDIDATE}},authored_positions=112,changed_methods=5,proof=proof,parent_compiler_results_apply_to_child=False,child_unity_gameplay='NOT_YET_OBSERVED',child_apple_compilation='SEPARATE_PENDING_GATE',whole_game_semantics='NOT_PROVEN',child_device_execution_performed=False,ipa_promotion_allowed=False)
 provenance.write_bytes((json.dumps(result,indent=2)+'\n').encode());(e/'candidate-inspection.txt').write_bytes((json.dumps(result,indent=2)+'\n').encode())
 for name in['ISOLATION.json','EXACT-JIT.json','PROGRESS.json','ACTUAL-COMPILER-40452784098.json']:(e/('R22-'+name)).write_bytes((CHECKPOINT/name).read_bytes())
 return result

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--output');p.add_argument('--source');p.add_argument('--validate-only',action='store_true');a=p.parse_args()
 if a.validate_only:
  data,proof=validate_checkpoint();print(json.dumps(dict(candidate_sha256=sha(data),bytes=len(data),proof=proof)))
 else:require(bool(a.output and a.source),'Output/source required');print(json.dumps(bind(a.output,a.source)))
