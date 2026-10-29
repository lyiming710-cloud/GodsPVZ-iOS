"""Admit this exact image for an unsigned diagnostic build, never production.

The real complex-level loss and profiler gaps remain failures/limits. Passing
this admission permits platform compilation, not a game-correctness claim.
"""
from pathlib import Path,PurePosixPath
import hashlib,json,re,tempfile,zipfile,tarfile,io
import validate_startup_r28_checkpoint as game
from read_startup_r28_attack_trace import read as read_attack

SDK='51f72bcd146c5571f642f3c4149f0545eb704fd325d83eb0f7c4ca2deec14c2b'
sha=lambda b:hashlib.sha256(b).hexdigest()

def validate(root=game.ROOT):
 root=Path(root);game.validate(root)
 actual=json.loads((root/'ACTUAL-UNITY-40794439817.json').read_bytes())
 assert actual['candidate_sha256']==game.CANDIDATE and actual['sdk_sha256']==SDK
 assert actual['run']['headSha']=='8a0c2b0823739e87f9dc55062afc4430a511469e'and actual['run']['conclusion']=='failure'
 assert actual['gameplay_status']=='ACTUAL_GAME_LOSS_OBSERVED'and not actual['gameplay_pass']and not actual['whole_level_completed']
 assert actual['raw_runtime_error_occurrences']==0 and not actual['runtime_errors']
 assert actual['ui_callback_checks']==25 and actual['ui_callback_check_result']=='PASS'
 assert actual['sdk_real_engine']=='PASS'and actual['actual_loaded_sdk_copies_rehashed']==2
 assert actual['onfield_plants']>=1 and actual['board_started']and actual['observed_game_seconds']>=40
 assert actual['unity_license_return_exit']==0 and actual['qualified_methods_added']==0
 with zipfile.ZipFile(root/'ACTUAL-UNITY-40794439817-FULL-FAILURE-EVIDENCE.zip')as z:
  assert z.testzip()is None
  manifest=json.loads(z.read('MANIFEST.json'));names={r['file']for r in manifest}
  assert len(names)==len(manifest)and set(z.namelist())==names|{'MANIFEST.json'}
  for r in manifest:
   path=PurePosixPath(r['file']);assert not path.is_absolute()and'..'not in path.parts
   b=z.read(r['file']);assert len(b)==r['bytes']and sha(b)==r['sha256']
  delivery=json.loads(z.read('gameplay/DELIVERY-MANIFEST.json'))
  for r in delivery['files']:
   b=z.read('gameplay/'+r['path']);assert len(b)==r['size']and sha(b)==r['sha256']
  play=json.loads(z.read('gameplay/GAMEPLAY-RESULT.json'))
  assert play['candidate_sha256']==game.CANDIDATE and play['workflow_head']==actual['run']['headSha']
  assert play['status']=='ACTUAL_GAME_LOSS_OBSERVED'and not play['complex_gameplay_pass']and not play['whole_level_completed']
  assert not play['runtime_errors']and all(c['pass']for c in play['checks'])
  assert play['plant_count_increased_after_input']and play['os_input_calibrated']and play['confirmed_after_os_input']
  assert not any(play[k]for k in ['game_fields_assigned','game_dll_changed','save_unlock_modified','level_data_modified','units_spawned_by_probe','unit_update_or_damage_called_by_probe'])
  with tempfile.TemporaryDirectory(prefix='r28-actual-attack-readback-')as d:
   for name in ['R28-ACTUAL-ATTACK-TRACE.json','ATTACK-RAW-PROFILER-FRAMES.jsonl']:(Path(d)/name).write_bytes(z.read('gameplay/'+name))
   attack=read_attack(d)
  assert attack==actual['independent_raw_attack_readback']and attack['plant_damage_call_stack_observed']
 compiler=json.loads((root/'ACTUAL-COMPILER-40794444003.json').read_bytes())
 assert compiler['candidate_sha256']==game.CANDIDATE and compiler['runtime_sdk_sha256']==SDK and compiler['source_commit']==actual['run']['headSha']
 assert compiler['pass_']and compiler['raw_errors']==0 and compiler['cpp']==755 and compiler['c']==274
 assert compiler['raw_source_logs_rechecked']==compiler['raw_generated_sources_rehashed']==1029
 with zipfile.ZipFile(root/'ACTUAL-LINUX-40794444003-REPORTS-AND-GAME-SOURCES.zip')as z:
  entries=sum([json.loads(z.read('startup-r28/'+n))for n in ['ALL-CPP-RESULTS.json','ALL-C-RESULTS.json']],[])
  lookup={r['file']:r for r in entries};assert len(lookup)==len(entries)==1029
  assert all(r['exit_code']==r['error_count']==0 for r in entries)
  logs=[]
  with tarfile.open(fileobj=io.BytesIO(z.read('startup-r28-evidence.tar.gz')))as t:
   for member in t:
    p=PurePosixPath(member.name)
    if member.isfile()and p.parent.as_posix()=='.validation/startup-r28/candidate/results/clang_logs'and p.name.endswith('.log'):
     text=t.extractfile(member).read().decode(errors='replace');name=p.name[:-4]
     assert name in lookup and not re.search(r': (?:fatal )?error:',text)
     assert text.count(': warning:')==lookup[name]['warning_count'];logs.append(name)
  assert len(logs)==len(set(logs))==1029 and set(logs)==set(lookup)
 return dict(scope='EXACT_R28_UNSIGNED_TEST_IPA_COMPILATION_ADMISSION_ONLY',candidate_sha256=game.CANDIDATE,sdk_sha256=SDK,
  test_ipa_build_allowed=True,production_promotion_allowed=False,whole_game_accepted=False,whole_level_accepted=False,
  actual_unity_run=40794439817,actual_linux_run=40794444003,actual_complex_level_status='LOSS_RETAINED',
  observed_plant_damage_call_stacks=1,profiler_frames_lost=162,damage_numeric_equivalence=False,ipad_tested=False,qualified_methods_added=0)

if __name__=='__main__':print(json.dumps(validate()))
