"""Append the 115 proved original serialized fields in 26 empty Zombie owners.
Includes the original cross-prefab Animator reference; never rewrites it to a
more plausible self reference. No game code, runtime fields or new assets.
"""
from pathlib import Path
import argparse,json,re
from restore_startup_r23_plant_fields import EXPECTED,ORIGINAL_PLAN_CANDIDATE,sha,require,emit,inventory
PLAN_SHA256='7ce35a7d8cf476845104dec8d45713b06096398b443f84002e963f9d4274a2ad'

def patch(data,row):
 text=data.decode('utf-8');heads=list(re.finditer(r'(?m)^--- !u!(\d+) &(-?\d+)[^\r\n]*\r?$',text));selected=[(i,h)for i,h in enumerate(heads)if int(h[1])==114 and int(h[2])==row['source_owner']];require(len(selected)==1,'Unique proved Zombie owner required');i,h=selected[0];at=h.end();end=heads[i+1].start()if i+1<len(heads)else len(text);body=text[at:end]
 require(sha(body.encode())==row['source_owner_body_sha256'],'Zombie owner differs from the captured empty document')
 require(re.search(r'(?m)^  m_Script: \{fileID: 855556878, guid: 60b84a767c46ced9aacc93f38da4d2f3, type: 3\}',body)is not None,'Wrong Zombie script identity')
 require(len(row['fields'])==row['original_field_count']==115 and not any(k.startswith('m_')for k in row['fields']),'Exactly 115 original game fields required')
 newline='\r\n'if'\r\n'in body else'\n';require(body.endswith(newline),'Document terminator missing');extra=newline.join(emit(row['fields']))+newline;out=(text[:end]+extra+text[end:]).encode();offset=len(text[:end].encode());require(out[:offset]+out[offset+len(extra.encode()):]==data,'Cannot undo fields byte-for-byte');return out

def restore(project,plan_path,output):
 root=Path(project).resolve();assets=root/'Assets';out=Path(output).resolve();require((root/'ProjectSettings/ProjectVersion.txt').is_file(),'Disposable Unity project required');plugin=assets/'Plugins/GodsPVZRuntime1.dll';require(sha(plugin.read_bytes())==EXPECTED,'Wrong candidate');b=Path(plan_path).read_bytes();require(sha(b)==PLAN_SHA256,'Unreviewed Zombie plan');plan=json.loads(b);require(plan['scope']=='SOURCE_PLAN_ONLY_115_ORIGINAL_FIELDS_PER_PROVED_ZOMBIE_OWNER_NOT_GAMEPLAY_PASS'and plan['candidate_sha256']==ORIGINAL_PLAN_CANDIDATE and plan['owner_script_file_id']==855556878 and plan['planned_prefabs']==26 and plan['planned_field_entries']==2990 and not plan['quarantined'],'Complete original Zombie plan required')
 out.mkdir(exist_ok=False,parents=True);before=inventory(assets);prepared=[]
 for row in plan['plans']:
  matches=[p for p in assets.rglob('*.prefab')if p.relative_to(root).as_posix().lower()==row['source_path'].lower()];require(len(matches)==1,'Unique source Zombie resource required');p=matches[0];require(p.resolve().is_relative_to(assets)and not p.is_symlink(),'Unsafe Zombie path');require(sha(p.with_suffix('.prefab.meta').read_bytes())==row['source_meta_sha256'],'Zombie prefab meta/GUID changed');data=p.read_bytes();prepared.append((row,p,data,patch(data,row)))
  for binding in row['reference_bindings']:
   if not binding.get('original_cross_prefab_reference_preserved'):continue
   target=root/binding['target_source_path'];require(target.resolve().is_relative_to(assets)and not target.is_symlink(),'Unsafe original cross-prefab target');require(sha(target.with_suffix('.prefab.meta').read_bytes())==binding['target_meta_sha256'],'Original cross-prefab target GUID changed')
 records=[]
 for row,p,data,after in prepared:
  q=out/'before'/p.name;q.parent.mkdir(exist_ok=True);q.write_bytes(data);p.write_bytes(after);require(p.read_bytes()==after,'Zombie byte readback differs');q=out/'after'/p.name;q.parent.mkdir(exist_ok=True);q.write_bytes(after);records.append(dict(file=p.relative_to(root).as_posix(),owner=row['source_owner'],before_sha256=sha(data),after_sha256=sha(after),fields_added=115,reference_bindings=len(row['reference_bindings'])))
 after=inventory(assets);changed=[p for p in before if before[p]!=after.get(p)];require(set(before)==set(after)and set(changed)=={p.relative_to(assets).as_posix()for _,p,_,_ in prepared},'Other Assets changed');require(sha(plugin.read_bytes())==EXPECTED,'Game DLL changed');report=dict(scope='TWENTY_SIX_ORIGINAL_ZOMBIE_PREFAB_FIELD_RESTORATION_SOURCE_ONLY_NOT_GAMEPLAY_PASS',candidate_sha256=EXPECTED,plan_sha256=PLAN_SHA256,changed_assets=changed,fields_added=2990,other_asset_bytes_unchanged=True,game_dll_unchanged=True,original_cross_prefab_animator_preserved=True,records=records,actual_unity_import_accepted=False,gameplay_accepted=False)
 (out/'SOURCE-RESTORATION.json').write_bytes((json.dumps(report,indent=2)+'\n').encode());print(json.dumps(report))

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--plan',required=True);p.add_argument('--output',required=True);a=p.parse_args();restore(a.project,a.plan,a.output)
