"""Fail closed on exact R28 finite evidence. This is restricted Editor diagnosis admission only."""
from pathlib import Path
import hashlib,json,zipfile
CANDIDATE='9081408a2226a79e23991355108540a0260a3966a500cecf4524f3cba9dfb364'
PARENT='eef6c35a2dceb0c38d1032837204abaff56f3a4c4d029ca47d76ff496fd69293'
TARGETS={'0x060003A8','0x060003AB','0x060004E1','0x060004E2'}
ROOT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R28-Attack115-2026-10-29'
sha=lambda b:hashlib.sha256(b).hexdigest()
def validate(root=ROOT):
 root=Path(root).resolve();entries=json.loads((root/'MANIFEST.json').read_bytes());names={p.relative_to(root).as_posix()for p in root.rglob('*')if p.is_file()and p.name!='MANIFEST.json'}
 assert names=={r['file']for r in entries}and len(entries)==len(names),'Checkpoint inventory changed'
 for r in entries:
  path=root/r['file'];assert path.resolve().is_relative_to(root)and not path.is_symlink();data=path.read_bytes();assert len(data)==r['bytes']and sha(data)==r['sha256'],r['file']
 read=lambda n:json.loads((root/n).read_bytes())
 c=read('CONSTRUCTION.json');assert c['candidate_sha256']==CANDIDATE and c['parent_sha256']==PARENT and c['new_unique_repair_positions']==115 and c['new_unique_methods']==4 and {x['token']for x in c['changes']}==TARGETS
 assert len({(x['method'],x['native_va'])for x in c['repair_sites']})==115
 repeat=read('REPEATED-CONSTRUCTION.json');assert repeat['candidate_sha256']==CANDIDATE and repeat['two_fresh_constructions_identical']
 iso=read('ISOLATION.json');assert iso['candidate_sha256']==CANDIDATE and iso['parent_sha256']==PARENT and iso['pass_']and iso['changed_methods']==4 and iso['unchanged_methoddefs']==2313
 assert all(iso[k]for k in ['old_code_slots_preserved','old_metadata_rows_and_heaps_preserved','method_flags_and_section_permissions_unchanged'])and len(iso['negative_controls'])==3 and all(x['rejected']for x in iso['negative_controls'])
 frozen=read('FROZEN-RAW.json');assert frozen['candidate_sha256']==CANDIDATE and frozen['checker_sha256']=='2bb0a8c229a63fd33269433759a587ff551c6fde19669bafc506033df9b63d14'and frozen['total']==2297
 by={x['token']:x for x in frozen['results']};assert all(by[t]['status']=='PASS'for t in TARGETS)
 assert read('MEMBER-EXIT.json')['exit_code']==0
 jit=read('EXACT-JIT.json');assert jit['candidate_sha256']==CANDIDATE and jit['counts']=={'JIT_PREPARED_NO_EXECUTION':4}
 native=read('NATIVE-COMPARE.json');assert native['candidate_sha256']==CANDIDATE and native['counts']=={'ZERO_DIFFERENCE':320}
 assert len(native['rows'])==320 and all(not x.get('differences')for x in native['rows'])
 from collections import Counter
 assert dict(Counter(x['status']for x in native['rows']))==native['counts']
 assert all(x['new_eh']is None for x in c['changes'])
 parent=read('ACTUAL-PARENT-DIFFERENCES.json');assert parent['parent_sha256']==PARENT and parent['candidate_sha256']==CANDIDATE and parent['different_cases']==54 and {x['token']for x in parent['rows']}==TARGETS and all(x['state_changed']for x in parent['rows'])
 controls=read('PHYSICAL-CONTROLS.json');assert controls['candidate_sha256']==CANDIDATE and controls['controls']==6 and controls['all_detected']and len(controls['results'])==6
 assert all(x['detected']and x['normal_execution_state_differences']>0 and x['frozen_type_status']=='PASS'for x in controls['results'])
 progress=read('PROGRESS.json');assert progress['qualified_methods']==0 and not progress['whole_game_accepted']and not progress['ipad_acceptance']and progress['ipa']is None and progress['unity_run']is None
 with zipfile.ZipFile(root/'STARTUP-R28-ATTACK115-CANDIDATE.zip')as z:
  assert z.testzip()is None and set(z.namelist())=={'STARTUP-R28-ATTACK115.dll','MANIFEST.json'};data=z.read('STARTUP-R28-ATTACK115.dll');assert json.loads(z.read('MANIFEST.json'))==[dict(file='STARTUP-R28-ATTACK115.dll',bytes=len(data),sha256=sha(data))]
 assert sha(data)==CANDIDATE
 return dict(scope='R28_FIXED_CANDIDATE_EDITOR_DIAGNOSIS_ADMISSION_ONLY_NOT_UNITY_OR_IPA',candidate_sha256=CANDIDATE,positions=115,normal_native_cases=320,unmodeled_native_unwind=0,whole_game_pass=False,ipa=None)
if __name__=='__main__':print(json.dumps(validate()))
