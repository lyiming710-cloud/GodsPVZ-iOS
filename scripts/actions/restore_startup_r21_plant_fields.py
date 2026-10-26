"""Restore only the 94 proved original serialized fields in nine Plant owners.
No DLL edits, runtime state injection, new assets, or listener substitutions.
"""
from pathlib import Path
import argparse,hashlib,json,math,re

EXPECTED='75160a3233f1e1ab13a44f712efb4b47fdeb9b57bd38fa25f180276bb7b50e15'
PLAN_SHA256='82e46ff8f080a25bb837cd557692ed586da1c7e42d22f2c45401c99992ba3e5b'
sha=lambda b:hashlib.sha256(b).hexdigest()
def require(v,message):
 if not v:raise ValueError(message)

def scalar(v):
 if isinstance(v,str):return json.dumps(v,ensure_ascii=False)
 if v is None:return 'null'
 if isinstance(v,bool):return '1'if v else'0'
 if isinstance(v,float):require(math.isfinite(v),'Nonfinite original value needs a separate encoding proof')
 require(isinstance(v,(int,float)),'Unsupported scalar');return repr(v)

def emit(v,indent=2):
 lines=[];prefix=' '*indent
 if isinstance(v,dict):
  for key,value in v.items():
   require(re.fullmatch(r'[A-Za-z_][A-Za-z_0-9]*',key)is not None,'Unexpected field spelling')
   if isinstance(value,dict)and'fileID'in value:
    require(set(value)in[{'fileID'},{'fileID','guid','type'}],'Unexpected PPtr shape');lines.append(prefix+key+': {'+', '.join(k+': '+scalar(x)for k,x in value.items())+'}')
   elif isinstance(value,(dict,list)):
    if not value:lines.append(prefix+key+(': []'if isinstance(value,list)else': {}'))
    else:lines.append(prefix+key+':');lines.extend(emit(value,indent+2))
   else:lines.append(prefix+key+': '+scalar(value))
 elif isinstance(v,list):
  for value in v:
   if isinstance(value,dict)and'fileID'in value:lines.append(prefix+'- {'+', '.join(k+': '+scalar(x)for k,x in value.items())+'}')
   elif isinstance(value,(dict,list)):
    lines.append(prefix+'-');lines.extend(emit(value,indent+2))
   else:lines.append(prefix+'- '+scalar(value))
 else:raise ValueError('Container required')
 return lines

def patch(data,row):
 text=data.decode('utf-8');heads=list(re.finditer(r'(?m)^--- !u!(\d+) &(-?\d+)[^\r\n]*\r?$',text));selected=[(k,h)for k,h in enumerate(heads)if int(h[1])==114 and int(h[2])==row['source_owner']];require(len(selected)==1,'Unique proved Plant owner required');k,h=selected[0];start=h.end();end=heads[k+1].start()if k+1<len(heads)else len(text);body=text[start:end]
 require(sha(body.encode())==row['source_owner_body_sha256'],'Owner differs from the captured empty source; refuse to overwrite fields')
 require(re.search(r'(?m)^  m_Script: \{fileID: 313814881, guid: 60b84a767c46ced9aacc93f38da4d2f3, type: 3\}',body)is not None,'Wrong game owner script')
 require(len(row['fields'])==row['original_field_count']==94 and not any(k.startswith('m_')for k in row['fields']),'Exactly 94 original game fields required')
 newline='\r\n'if'\r\n'in body else'\n';extra=newline.join(emit(row['fields']))+newline;require(body.endswith(newline),'Original document terminator required');out=(text[:end]+extra+text[end:]).encode();require(out[:len(text[:end].encode())]==text[:end].encode(),'Prefix changed')
 # Independent undo proves that only the appended owner fields changed.
 at=len(text[:end].encode());restored=out[:at]+out[at+len(extra.encode()):];require(restored==data,'Cannot undo append byte-for-byte');return out

def inventory(root):return {p.relative_to(root).as_posix():sha(p.read_bytes())for p in sorted(root.rglob('*'))if p.is_file()}

def restore(project,plan_path,output):
 root=Path(project).resolve();assets=root/'Assets';out=Path(output).resolve();require((root/'ProjectSettings/ProjectVersion.txt').is_file(),'Disposable Unity project required');plugin=assets/'Plugins/GodsPVZRuntime1.dll';require(sha(plugin.read_bytes())==EXPECTED,'Wrong candidate');plan_bytes=Path(plan_path).read_bytes();require(sha(plan_bytes)==PLAN_SHA256,'Unreviewed or changed restoration plan');plan=json.loads(plan_bytes);require(plan['candidate_sha256']==EXPECTED and plan['planned_prefabs']==9 and plan['planned_field_entries']==846 and not plan['quarantined'],'Complete original plan required');require(plan['scope']=='SOURCE_PLAN_ONLY_94_ORIGINAL_FIELDS_PER_PROVED_PLANT_OWNER_NOT_GAMEPLAY_PASS','Wrong plan scope')
 out.mkdir(parents=True,exist_ok=False);before=inventory(assets);prepared=[]
 for sprite in plan['sprite_proofs']:
  path=root/sprite['source_path'];require(path.resolve().is_relative_to(assets)and not path.is_symlink(),'Unsafe Sprite path');require(sha(path.read_bytes())==sprite['source_asset_sha256'],'Previously mapped Sprite source changed');require(sha(path.with_suffix(path.suffix+'.meta').read_bytes())==sprite['source_meta_sha256'],'Previously mapped Sprite GUID/meta changed')
 for row in plan['plans']:
  wanted=row['source_path'].lower();matches=[p for p in assets.rglob('*.prefab')if p.relative_to(root).as_posix().lower()==wanted];require(len(matches)==1,'Unique original resource path required');target=matches[0];require(target.resolve().is_relative_to(assets)and not target.is_symlink(),'Unsafe prefab path');data=target.read_bytes();after=patch(data,row);prepared.append((row,target,data,after))
 # Validate every owner before making any source write.
 records=[]
 for row,target,data,after in prepared:
  target.write_bytes(after);require(target.read_bytes()==after,'Readback differs');rel=target.relative_to(root).as_posix();backup=out/'before'/target.name;backup.parent.mkdir(exist_ok=True);backup.write_bytes(data);result=out/'after'/target.name;result.parent.mkdir(exist_ok=True);result.write_bytes(after);records.append(dict(file=rel,owner=row['source_owner'],before_sha256=sha(data),after_sha256=sha(after),fields_added=94,bindings=len(row['reference_bindings']),source_owner_before_sha256=row['source_owner_body_sha256']))
 after=inventory(assets);changed=[p for p in before if before[p]!=after.get(p)];require(set(before)==set(after)and set(changed)=={p.relative_to(assets).as_posix()for _,p,_,_ in prepared},'Other Assets changed');require(sha(plugin.read_bytes())==EXPECTED,'Game DLL changed')
 report=dict(scope='NINE_ORIGINAL_PLANT_PREFAB_FIELD_RESTORATION_SOURCE_ONLY_NOT_GAMEPLAY_PASS',candidate_sha256=EXPECTED,plan_sha256=sha(Path(plan_path).read_bytes()),changed_assets=changed,fields_added=846,other_asset_bytes_unchanged=True,game_dll_unchanged=True,records=records,unity_import_accepted=False,gameplay_accepted=False)
 (out/'SOURCE-RESTORATION.json').write_bytes((json.dumps(report,indent=2)+'\n').encode());print(json.dumps(report))

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--plan',required=True);p.add_argument('--output',required=True);a=p.parse_args();restore(a.project,a.plan,a.output)
