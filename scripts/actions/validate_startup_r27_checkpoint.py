"""Fail closed on exact R27 finite evidence. This is restricted Editor diagnosis admission only."""
from pathlib import Path
import hashlib,json,zipfile
CANDIDATE='eef6c35a2dceb0c38d1032837204abaff56f3a4c4d029ca47d76ff496fd69293'
PARENT='9490c1845298fa5afda04fc8dfba915ce2a939eaebe96be83e3311b39557bc4c'
TARGETS={'0x060001DD','0x06000453'}
ROOT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R27-Lifecycle123-2026-10-28'
sha=lambda b:hashlib.sha256(b).hexdigest()
def validate(root=ROOT):
 root=Path(root).resolve();entries=json.loads((root/'MANIFEST.json').read_bytes());names={p.relative_to(root).as_posix()for p in root.rglob('*')if p.is_file()and p.name!='MANIFEST.json'}
 assert names=={r['file']for r in entries}and len(entries)==len(names),'Checkpoint inventory changed'
 for r in entries:
  path=root/r['file'];assert path.resolve().is_relative_to(root)and not path.is_symlink();data=path.read_bytes();assert len(data)==r['bytes']and sha(data)==r['sha256'],r['file']
 read=lambda n:json.loads((root/n).read_bytes())
 c=read('CONSTRUCTION.json');assert c['candidate_sha256']==CANDIDATE and c['parent_sha256']==PARENT and c['new_unique_repair_positions']==123 and c['new_unique_methods']==2 and {x['token']for x in c['changes']}==TARGETS
 assert len({(x['method'],x['native_va'])for x in c['repair_sites']})==123
 repeat=read('REPEATED-CONSTRUCTION.json');assert repeat['candidate_sha256']==CANDIDATE and repeat['two_fresh_constructions_identical']
 iso=read('ISOLATION.json');assert iso['candidate_sha256']==CANDIDATE and iso['parent_sha256']==PARENT and iso['pass_']and iso['changed_methods']==2 and iso['unchanged_methoddefs']==2315
 assert all(iso[k]for k in ['old_code_slots_preserved','old_metadata_rows_and_heaps_preserved','method_flags_and_section_permissions_unchanged'])and len(iso['negative_controls'])==3 and all(x['rejected']for x in iso['negative_controls'])
 frozen=read('FROZEN-RAW.json');assert frozen['candidate_sha256']==CANDIDATE and frozen['checker_sha256']=='2bb0a8c229a63fd33269433759a587ff551c6fde19669bafc506033df9b63d14'and frozen['total']==2297
 by={x['token']:x for x in frozen['results']};assert all(by[t]['status']=='PASS'for t in TARGETS)
 assert read('MEMBER-EXIT.json')['exit_code']==0
 jit=read('EXACT-JIT.json');assert jit['candidate_sha256']==CANDIDATE and jit['counts']=={'JIT_PREPARED_NO_EXECUTION':2}
 native=read('NATIVE-COMPARE.json');assert native['candidate_sha256']==CANDIDATE and native['counts']=={'ZERO_DIFFERENCE':268,'UNSUPPORTED_ORIGINAL_WINDOWS_SEH_EXCEPTION_UNWIND':1}
 assert len(native['rows'])==269 and all(not x.get('differences')for x in native['rows'])
 from collections import Counter
 assert dict(Counter(x['status']for x in native['rows']))==native['counts']
 regions=c['changes'][0]['new_eh'];assert isinstance(regions,list)and len(regions)==1 and regions[0]['kind']=='finally' and c['changes'][1]['new_eh']is None
 controls=read('PHYSICAL-CONTROLS.json');assert controls['candidate_sha256']==CANDIDATE and controls['controls']==6 and controls['all_detected']and len(controls['results'])==6
 assert all(x['detected']and x['normal_execution_state_differences']>0 and x['frozen_type_status']=='PASS'for x in controls['results'])
 progress=read('PROGRESS.json');assert progress['qualified_methods']==0 and not progress['whole_game_accepted']and not progress['ipad_acceptance']and progress['ipa']is None and progress['unity_run']is None
 with zipfile.ZipFile(root/'STARTUP-R27-LIFECYCLE123-CANDIDATE.zip')as z:
  assert z.testzip()is None and set(z.namelist())=={'STARTUP-R27-LIFECYCLE123.dll','MANIFEST.json'};data=z.read('STARTUP-R27-LIFECYCLE123.dll');assert json.loads(z.read('MANIFEST.json'))==[dict(file='STARTUP-R27-LIFECYCLE123.dll',bytes=len(data),sha256=sha(data))]
 assert sha(data)==CANDIDATE
 return dict(scope='R27_FIXED_CANDIDATE_EDITOR_DIAGNOSIS_ADMISSION_ONLY_NOT_UNITY_OR_IPA',candidate_sha256=CANDIDATE,positions=123,normal_native_cases=268,unmodeled_native_unwind=1,whole_game_pass=False,ipa=None)
if __name__=='__main__':print(json.dumps(validate()))
