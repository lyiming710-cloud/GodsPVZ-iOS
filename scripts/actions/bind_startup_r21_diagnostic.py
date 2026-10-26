"""Exact R21 caller/compiler proof, retained parent proof, restricted actual UI audit."""
from pathlib import Path
import argparse,hashlib,json,zipfile
import bind_startup_r20_diagnostic as parent
require=parent.require;sha=parent.sha
CANDIDATE='75160a3233f1e1ab13a44f712efb4b47fdeb9b57bd38fa25f180276bb7b50e15'
CHECKPOINT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R21-UI139-2026-10-27'
def validate_checkpoint(root=CHECKPOINT):
 root=Path(root).resolve();rows=json.loads((root/'MANIFEST.json').read_bytes());names={p.relative_to(root).as_posix() for p in root.rglob('*') if p.is_file() and p!=root/'MANIFEST.json'};require(len(rows)==len({r['file'] for r in rows}) and names=={r['file'] for r in rows},'R21 checkpoint inventory mismatch')
 for r in rows:
  p=root/r['file'];require(p.resolve().is_relative_to(root) and not p.is_symlink(),'Unsafe R21 evidence path');b=p.read_bytes();require(len(b)==r['bytes'] and sha(b)==r['sha256'],'R21 evidence byte mismatch '+r['file'])
 def read(name):return json.loads((root/name).read_bytes())
 construction=read('CONSTRUCTION.json');targets={r['token'] for r in construction['changes']};require(construction['candidate_sha256']==CANDIDATE and construction['parent_sha256']==parent.CANDIDATE and construction['new_unique_repair_positions']==139 and len(targets)==7,'R21 construction scope differs')
 start=read('SHARED-TEST-START.json');require(start['candidate_sha256']==CANDIDATE and start['repeat_identical'],'R21 repeated construction missing')
 isolation=read('ISOLATION.json');require(isolation['candidate_sha256']==CANDIDATE and isolation['pass_'] and isolation['changed_methods']==7 and isolation['unchanged_methoddefs']==2310,'R21 target isolation missing');require(isolation['old_code_slots_preserved'] and isolation['old_metadata_rows_and_heaps_preserved'] and isolation['method_flags_and_section_permissions_unchanged'],'Old method/metadata/permissions changed')
 frozen=read('FROZEN-WITH-INTERFACE-FACTS.json');require(frozen['candidate_sha256']==CANDIDATE and frozen['checker_sha256']==parent.CHECKER and frozen['counts']=={'PASS':2227,'FAIL_OR_UNSUPPORTED':70},'Wrong frozen checker/census');found={r['token']:r for r in frozen['results']};require(all(found[t]['status']=='PASS' for t in targets),'A current target failed the frozen type check')
 require(read('MEMBER-EXIT.json')['exit_code']==0,'R21 member audit failed');jit=read('EXACT-JIT.json');require(jit['candidate_sha256']==CANDIDATE and jit['counts']=={'JIT_PREPARED_NO_EXECUTION':7},'R21 JIT preparation missing')
 for name,count in [('BUTTON-NATIVE-COMPARE.json',300),('AUDIO-NATIVE-COMPARE.json',44)]:
  r=read(name);require(r['candidate_sha256']==CANDIDATE and r['records']==count and not r['differences'] and not r['callback_errors'],'R21 finite native profile failed '+name)
 controls=read('PHYSICAL-CONTROLS.json');require(controls['candidate_sha256']==CANDIDATE and controls['controls']==controls['rejected']==15 and controls['process_failures_counted_as_semantic_controls']==0,'Physical semantic controls incomplete')
 linux=read('ACTUAL-COMPILER-40361033961.json');require(linux['candidate_sha256']==CANDIDATE and linux['run_id']==40361033961 and linux['source_commit']=='9b85377470b73517dba3f592c8597955635413cd' and linux['pass_'] and linux['raw_generated_sources_rehashed']==linux['raw_source_logs_rechecked']==1029,'Fresh exact Linux census missing')
 apple=read('ACTUAL-APPLE-40363183752.json');require(apple['pass_'] and apple['actual']['candidate_sha256']==CANDIDATE and apple['actual']['source_conversion_run']==40361033961 and apple['run_id']==40363183752 and apple['source_records_rechecked']==apple['raw_source_logs_rechecked']==1029 and apple['damaged_control_raw_errors']==34,'Fresh exact Apple census or raw damaged control missing')
 current=read('PROGRESS.json');require(current['candidate_sha256']==CANDIDATE and current['qualified_methods']==0 and not current['ipad_acceptance'] and not current['whole_game_accepted'] and current['ipa'] is None,'R21 diagnostic must preserve pending product/device gates')
 with zipfile.ZipFile(root/'STARTUP-R21-UI139-CANDIDATE.zip') as z:
  require(set(z.namelist())=={'STARTUP-R21-UI139.dll','MANIFEST.json'} and z.testzip() is None,'Unexpected R21 ZIP contents');data=z.read('STARTUP-R21-UI139.dll');require(json.loads(z.read('MANIFEST.json'))==[dict(file='STARTUP-R21-UI139.dll',bytes=len(data),sha256=sha(data))],'R21 inner byte manifest mismatch')
 require(len(data)==23218688 and sha(data)==CANDIDATE,'Wrong R21 candidate bytes')
 return data,dict(targets=sorted(targets),finite_normal_records=344,physical_semantic_controls=15,linux_run=40361033961,apple_run=40363183752,whole_type_counts=frozen['counts'],qualified_methods=0,whole_game_accepted=False,ipad_acceptance=False)
def bind(output,source):
 data,proof=validate_checkpoint();out=Path(output).resolve();target=out/'candidate/Assembly-CSharp-bulk401.dll';e=out/'evidence'
 # The existing immutable package and R20 evidence are validated independently.
 # Their old compiler observations are never attributed to this child.
 parent_result=parent.bind(output,source);provenance=e/'PROVENANCE.json';require(sha(target.read_bytes())==parent.CANDIDATE,'Parent chain output differs');backup=e/'PARENT-R20-PROVENANCE.json';require(not backup.exists(),'Do not overwrite parent proof');provenance.rename(backup);target.write_bytes(data)
 result=dict(status='STARTUP_R21_RESTRICTED_REAL_UI_DIAGNOSTIC_ONLY',source_commit=source,parent_sha256=parent.CANDIDATE,candidate_sha256=CANDIDATE,outputs={'unlinked':{'sha256':CANDIDATE}},authored_positions=139,changed_methods=7,finite_normal_comparisons=344,actual_child_compiler_proofs=proof,parent_compiler_results_apply_to_child=False,whole_game_semantics='NOT_PROVEN',failed_or_unsupported_whole_type_entries_preserved=70,child_device_execution_performed=False,ipa_promotion_allowed=False)
 provenance.write_bytes((json.dumps(result,indent=2)+'\n').encode());(e/'candidate-inspection.txt').write_bytes((json.dumps(result,indent=2)+'\n').encode())
 for name in ['ISOLATION.json','EXACT-JIT.json','PROGRESS.json','ACTUAL-COMPILER-40361033961.json','ACTUAL-APPLE-40363183752.json']:(e/('R21-'+name)).write_bytes((CHECKPOINT/name).read_bytes())
 return result
if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--output');p.add_argument('--source');p.add_argument('--validate-only',action='store_true');a=p.parse_args()
 if a.validate_only:
  data,proof=validate_checkpoint();print(json.dumps(dict(candidate_sha256=sha(data),bytes=len(data),proof=proof)))
 else:require(bool(a.output and a.source),'Output/source required');print(json.dumps(bind(a.output,a.source)))
