"""Bind current R20 to real Editor diagnostics, preserving open whole-game gates."""
from pathlib import Path
import argparse,hashlib,json,zipfile
PARENT='6de4db6c94b80a43876aea060b573ed60c1fa7170cff485103d2c808b94762ae'
CANDIDATE='d887b370c95c2966dc40d80648d85c286bb42d85004c32d095f64f004e095b37'
CHECKER='2bb0a8c229a63fd33269433759a587ff551c6fde19669bafc506033df9b63d14'
CHECKPOINT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R20-Sites104-2026-10-26'
def sha(data):return hashlib.sha256(data).hexdigest()
def require(condition,message):
    if not condition:raise ValueError(message)
def validate_completed_ipa_if_present(ipa, artifact_checkpoint=None):
    """An exported test IPA is not device/semantic acceptance or a UI test."""
    if ipa is None:return None
    root=Path(artifact_checkpoint or CHECKPOINT.parent/'Startup-R20-IPA-2026-10-26').resolve()
    rows=json.loads((root/'MANIFEST.json').read_bytes())
    names={p.relative_to(root).as_posix() for p in root.rglob('*') if p.is_file() and p!=root/'MANIFEST.json'}
    require(len(rows)==len({r['file'] for r in rows}) and names=={r['file'] for r in rows},'Actual IPA evidence inventory mismatch')
    for row in rows:
        path=root/row['file'];require(path.resolve().is_relative_to(root) and not path.is_symlink(),'Unsafe actual IPA evidence path')
        data=path.read_bytes();require(len(data)==row['bytes'] and sha(data)==row['sha256'],'Actual IPA evidence bytes differ: '+row['file'])
    raw=json.loads((root/'INDEPENDENT-IPA-READBACK.json').read_bytes());run=json.loads((root/'RUN.json').read_bytes())
    require(raw['run_id']==run['databaseId']==40263428409 and raw['source_commit']==run['headSha']=='c22560ed9e025f04d4e58bc38ba0f23df8b9feb4' and run['status']=='completed' and run['conclusion']=='success','Actual IPA run/source binding differs')
    require(raw['candidate_sha256']==CANDIDATE and raw['unity_errors']==raw['actual_apple_errors']==0 and raw['actual_xcode_build_succeeded'] and raw['packaged_metadata_exactly_equals_actual_export'],'Actual IPA build/readback missing')
    require(ipa['sha256']==raw['ipa_sha256']=='f7334b1b330cdc7dc2cbeeb0c9851c8c233bc0fc923c69e6de36f38aeda56c75' and ipa['bytes']==raw['ipa_bytes']==192091143,'Wrong prior test IPA identity')
    require(ipa['unsigned'] and ipa['platform']=='physical_iOS' and ipa['architecture']=='arm64','Wrong prior test IPA platform/signature')
    require(not raw['device_execution'] and not raw['physical_audio'] and not raw['full_game_semantic_acceptance'],'Prior IPA must not imply unperformed device/game acceptance')
    return dict(run=40263428409,ipa_sha256=ipa['sha256'],scope='PRIOR_TEST_PACKAGE_ONLY_NOT_CURRENT_INTERACTION_RESULTS')

def validate_checkpoint(checkpoint=CHECKPOINT):
    root=Path(checkpoint).resolve();rows=json.loads((root/'MANIFEST.json').read_bytes())
    names={p.relative_to(root).as_posix() for p in root.rglob('*') if p.is_file() and p!=root/'MANIFEST.json'}
    require(len(rows)==len({r['file'] for r in rows}) and names=={r['file'] for r in rows},'Checkpoint inventory mismatch')
    for r in rows:
        p=root/r['file'];require(p.resolve().is_relative_to(root) and not p.is_symlink(),'Unsafe checkpoint path')
        data=p.read_bytes();require(len(data)==r['bytes'] and sha(data)==r['sha256'],'Checkpoint bytes differ: '+r['file'])
    def read(name):return json.loads((root/name).read_bytes())
    construction=read('CONSTRUCTION.json');targets={r['token'] for r in construction['changes']}
    require(construction['candidate_sha256']==CANDIDATE and construction['new_unique_repair_positions']==104 and len(targets)==8,'Wrong R20 construction scope')
    first=read('SHARED-TEST-START.json');require(first['repeat_identical'],'Threshold repeat proof missing')
    isolation=read('ISOLATION.json');require(isolation['candidate_sha256']==CANDIDATE and isolation['pass_'] and isolation['changed_methods']==8 and isolation['unchanged_methoddefs']==2309,'Raw isolation missing')
    require(isolation['old_code_slots_preserved'] and isolation['old_metadata_rows_and_heaps_preserved'] and isolation['method_flags_and_section_permissions_unchanged'],'Old slots/metadata/permissions changed')
    frozen=read('FROZEN-WITH-INTERFACE-FACTS.json');require(frozen['candidate_sha256']==CANDIDATE and frozen['checker_sha256']==CHECKER,'Wrong type checker/candidate')
    require(frozen['counts']=={'PASS':2227,'FAIL_OR_UNSUPPORTED':70},'Whole type failures must remain explicit')
    found={r['token']:r for r in frozen['results']};require(all(found[t]['status']=='PASS' for t in targets),'A target type check failed')
    member=read('MEMBER-EXIT.json');require(member['exit_code']==0,'Member access did not pass')
    jit=read('EXACT-JIT.json');require(jit['candidate_sha256']==CANDIDATE and jit['methods']==8 and jit['counts']=={'JIT_PREPARED_NO_EXECUTION':8},'Exact JIT preparation missing')
    normal=0
    for name,count in [('NATIVE-MANAGERS.json',552),('NATIVE-SAVE-UI.json',216)]:
        v=read(name);require(v['candidate_sha256']==CANDIDATE and v['normal_records']==count and not v['differences'] and not v['callback_errors'],'Native/CIL finite profile missing: '+name);normal+=count
    controls=read('RUNTIME-CONTROLS.json');require(controls['candidate_sha256']==CANDIDATE and controls['controls']==controls['detected']==8 and controls['process_failures']==0,'Physical runtime controls missing')
    linux=read('ACTUAL-COMPILER-40219580971.json');require(linux['candidate_sha256']==CANDIDATE and linux['pass_'] and linux['raw_generated_sources_rehashed']==linux['raw_source_logs_rechecked']==1029,'Exact Linux readback missing')
    apple=read('ACTUAL-APPLE-40220652002.json');require(apple['pass_'] and apple['actual']['candidate_sha256']==CANDIDATE and apple['actual']['source_conversion_run']==40219580971 and apple['source_records_rechecked']==apple['raw_source_logs_rechecked']==1029,'Exact Apple readback missing')
    replay=read('ACTUAL-APPLE-CONTROL-40221471927.json');require(replay['expectation_met'] and replay['raw_errors']==34 and replay['flags_identical_to_prior_damaged_control'],'Apple damaged-control raw replay missing')
    current=read('PROGRESS.json');require(current['candidate_sha256']==CANDIDATE and current['whole_type_counts']['FAIL_OR_UNSUPPORTED']==70 and current['qualified_methods']==0 and not current['ipad_acceptance'] and not current['whole_game_accepted'],'Diagnostic must preserve product hold')
    prior_test_ipa=validate_completed_ipa_if_present(current['ipa'])
    with zipfile.ZipFile(root/'STARTUP-R20-SITES104-CANDIDATE.zip') as z:
        require(set(z.namelist())=={'STARTUP-R20-SITES104.dll','MANIFEST.json'} and z.testzip() is None,'Unexpected candidate archive')
        data=z.read('STARTUP-R20-SITES104.dll');inside=json.loads(z.read('MANIFEST.json'))
        require(inside==[dict(file='STARTUP-R20-SITES104.dll',bytes=len(data),sha256=sha(data))],'Inner candidate manifest differs')
    require(len(data)==22777856 and sha(data)==CANDIDATE,'Wrong R20 candidate bytes')
    return data,dict(current=current,prior_test_ipa=prior_test_ipa,whole_type_counts=frozen['counts'],targets=sorted(targets),finite_normal_records=normal,
        linux_run=linux['run_id'],linux_source_commit=linux['source_commit'],apple_run=apple['run_id'],apple_source_commit=apple['actual']['apple_audit_source_commit'],apple_raw_control_run=replay['run_id'])
def bind(output,source):
    out=Path(output).resolve();e=out/'evidence';target=out/'candidate/Assembly-CSharp-bulk401.dll';provenance=e/'PROVENANCE.json'
    require(sha(target.read_bytes())==PARENT,'Exact separately qualified package parent required')
    p=json.loads(provenance.read_bytes());require(p['outputs']['unlinked']['sha256']==PARENT and p['source_commit']==source and p['status']=='BULK401_LINUX_AND_APPLE_CPP_PRECHECK_PASS','Same-run parent package binding failed')
    backup=e/'PARENT-BULK401-PROVENANCE.json';require(not backup.exists(),'Do not overwrite existing parent binding')
    data,proof=validate_checkpoint()
    # Do not mutate the disposable output until every check above has passed.
    provenance.rename(backup);target.write_bytes(data)
    result=dict(status='STARTUP_R20_RESTRICTED_REAL_EDITOR_DIAGNOSTIC_ONLY',source_commit=source,parent_sha256=PARENT,candidate_sha256=CANDIDATE,
        outputs={'unlinked':{'sha256':CANDIDATE}},authored_positions=104,changed_methods=8,finite_normal_comparisons=768,
        parent_compiler_results_apply_to_child=False,actual_child_compiler_proofs=proof,
        whole_game_semantics='NOT_PROVEN',failed_or_unsupported_whole_type_entries_preserved=70,
        child_unity_export_performed=False,child_device_execution_performed=False,ipa_promotion_allowed=False,
        diagnostic_scope='Actual Editor MainMenu startup, scene/prefab bindings, shader compilation and controlled render pixels')
    provenance.write_bytes((json.dumps(result,indent=2)+'\n').encode())
    inspection=e/'candidate-inspection.txt'
    if inspection.exists():inspection.rename(e/'PARENT-BULK401-candidate-inspection.txt')
    inspection.write_bytes((json.dumps(result,indent=2)+'\n').encode())
    for name in ['ISOLATION.json','EXACT-JIT.json','PROGRESS.json','ACTUAL-COMPILER-40219580971.json','ACTUAL-APPLE-40220652002.json','ACTUAL-APPLE-CONTROL-40221471927.json']:(e/('R20-'+name)).write_bytes((CHECKPOINT/name).read_bytes())
    return result
if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--output');p.add_argument('--source');p.add_argument('--validate-only',action='store_true');a=p.parse_args()
    if a.validate_only:
        data,proof=validate_checkpoint();print(json.dumps(dict(candidate_sha256=sha(data),bytes=len(data),proof=proof)))
    else:
        require(bool(a.output and a.source),'--output and --source required');print(json.dumps(bind(a.output,a.source)))
