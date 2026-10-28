"""Fail closed on exact R24 finite evidence. This is compiler admission only."""
from pathlib import Path
import hashlib,json,zipfile
CANDIDATE='4b5349946ee57d368002cdc1c358334efd43d4b9764657556e057148934927f3'
PARENT='1d244776ee85083c2ed298c4f749f96b6c5ae820a78d50b0ce47480691775122'
TARGETS={'0x06000228','0x06000229','0x0600022B','0x06000234','0x060003A4'}
ROOT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R24-Actor130-2026-10-28'
sha=lambda b:hashlib.sha256(b).hexdigest()
def validate(root=ROOT):
 root=Path(root).resolve();entries=json.loads((root/'MANIFEST.json').read_bytes());names={p.relative_to(root).as_posix()for p in root.rglob('*')if p.is_file()and p.name!='MANIFEST.json'}
 assert names=={r['file']for r in entries}and len(entries)==len(names),'Checkpoint inventory changed'
 for r in entries:
  path=root/r['file'];assert path.resolve().is_relative_to(root)and not path.is_symlink();data=path.read_bytes();assert len(data)==r['bytes']and sha(data)==r['sha256'],r['file']
 read=lambda n:json.loads((root/n).read_bytes())
 c=read('CONSTRUCTION.json');assert c['candidate_sha256']==CANDIDATE and c['parent_sha256']==PARENT and c['new_unique_repair_positions']==130 and c['new_unique_methods']==5 and {x['token']for x in c['changes']}==TARGETS
 assert len({(x['method'],x['native_va'])for x in c['repair_sites']})==130
 repeat=read('REPEATED-CONSTRUCTION.json');assert repeat['candidate_sha256']==CANDIDATE and repeat['two_fresh_constructions_identical']
 iso=read('ISOLATION.json');assert iso['candidate_sha256']==CANDIDATE and iso['parent_sha256']==PARENT and iso['pass_']and iso['changed_methods']==5 and iso['unchanged_methoddefs']==2312
 assert all(iso[k]for k in ['old_code_slots_preserved','old_metadata_rows_and_heaps_preserved','method_flags_and_section_permissions_unchanged'])and len(iso['negative_controls'])==3 and all(x['rejected']for x in iso['negative_controls'])
 frozen=read('FROZEN-RAW.json');assert frozen['candidate_sha256']==CANDIDATE and frozen['checker_sha256']=='2bb0a8c229a63fd33269433759a587ff551c6fde19669bafc506033df9b63d14'and frozen['total']==2297
 by={x['token']:x for x in frozen['results']};assert all(by[t]['status']=='PASS'for t in TARGETS)
 assert read('MEMBER-EXIT.json')['exit_code']==0
 jit=read('EXACT-JIT.json');assert jit['candidate_sha256']==CANDIDATE and jit['counts']=={'JIT_PREPARED_NO_EXECUTION':5}
 native=read('NATIVE-COMPARE.json');assert native['candidate_sha256']==CANDIDATE and native['counts']=={'ZERO_DIFFERENCE':138,'UNSUPPORTED_ORIGINAL_WINDOWS_SEH_EXCEPTION_UNWIND':2}
 assert len(native['rows'])==140 and all(not x.get('differences')for x in native['rows'])
 controls=read('PHYSICAL-CONTROLS.json');assert controls['candidate_sha256']==CANDIDATE and controls['controls']==6 and controls['all_detected']and len(controls['results'])==6
 assert all(x['detected']and x['normal_execution_state_differences']>0 and x['frozen_type_status']=='PASS'for x in controls['results'])
 progress=read('PROGRESS.json');assert progress['qualified_methods']==0 and not progress['whole_game_accepted']and not progress['ipad_acceptance']and progress['ipa']is None and progress['unity_run']is None
 with zipfile.ZipFile(root/'STARTUP-R24-ACTOR130-CANDIDATE.zip')as z:
  assert z.testzip()is None and set(z.namelist())=={'STARTUP-R24-ACTOR130.dll','MANIFEST.json'};data=z.read('STARTUP-R24-ACTOR130.dll');assert json.loads(z.read('MANIFEST.json'))==[dict(file='STARTUP-R24-ACTOR130.dll',bytes=len(data),sha256=sha(data))]
 assert sha(data)==CANDIDATE
 return dict(scope='R24_FIXED_CANDIDATE_COMPILER_ADMISSION_ONLY_NOT_UNITY_OR_IPA',candidate_sha256=CANDIDATE,positions=130,normal_native_cases=138,unmodeled_native_unwind=2,whole_game_pass=False,ipa=None)
if __name__=='__main__':print(json.dumps(validate()))
