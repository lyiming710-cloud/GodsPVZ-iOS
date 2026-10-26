"""Restore six proven-null prefab links using exact root-relative component identities.

Only numeric fileID values in the owner document are patched. No prefab rewrite,
runtime field injection, DLL edit, listener replacement or null bypass occurs.
"""
from pathlib import Path
import argparse,hashlib,json,re
FIELDS={'zombiePortrait':('Portrait','UnityEngine.UI.Image'),'questionMark':('QMark','UnityEngine.UI.Image'),'checkMark':('CMark','UnityEngine.UI.Image'),'eliteMark':('EMark','UnityEngine.UI.Image'),'bossMark':('BMark','UnityEngine.UI.Image'),'button':('Portrait','UnityEngine.UI.Button')}
def sha(b):return hashlib.sha256(b).hexdigest()
def require(v,message):
 if not v:raise ValueError(message)
def inventory(root):return {p.relative_to(root).as_posix():sha(p.read_bytes()) for p in sorted(root.rglob('*')) if p.is_file()}
def patch(data,plan):
 require(plan['scope']=='SIX_ORIGINAL_ZOMBIE_WINDOW_SERIALIZED_COMPONENT_REFERENCE_BINDINGS_ONLY' and plan['owner_type']=='Almanac_ZombieWindow' and plan['root_name']=='ZombieWindow','Wrong binding plan scope')
 require({x['field']:(x['relative_gameobject_path'],x['component_type']) for x in plan['entries']}==FIELDS and len(plan['entries'])==6,'Wrong original binding identities')
 text=data.decode('utf-8');heads=list(re.finditer(r'(?m)^--- !u!(\d+) &(-?\d+)(?:[^\r\n]*)\r?$',text));require(heads,'Unity YAML documents required');docs={}
 for k,h in enumerate(heads):
  anchor=int(h[2]);require(anchor not in docs,'Duplicate YAML local fileID');docs[anchor]=dict(kind=int(h[1]),body=text[h.end():heads[k+1].start() if k+1<len(heads) else len(text)],start=h.end(),end=heads[k+1].start() if k+1<len(heads) else len(text))
 def ref(body,name):
  m=re.search(r'(?m)^  '+re.escape(name)+r': \{fileID: (-?\d+)\}[^\r\n]*\r?$',body);require(m is not None,'Missing local reference '+name);return int(m[1])
 gos={};transforms={}
 for anchor,d in docs.items():
  if d['kind']==1:
   m=re.search(r'(?m)^  m_Name: (.+?)\r?$',d['body']);require(m is not None,'GameObject name missing');gos[anchor]=m[1].strip('"\'')
  if d['kind'] in [4,224]:transforms[anchor]=(ref(d['body'],'m_GameObject'),ref(d['body'],'m_Father'))
 roots=[(tid,gid) for tid,(gid,parent) in transforms.items() if parent==0 and gos.get(gid)=='ZombieWindow'];require(len(roots)==1,'Unique original prefab root required');root_tid,root_gid=roots[0]
 def relative(tid):
  if tid==root_tid:return ''
  gid,parent=transforms[tid];require(parent in transforms,'Transform ancestry leaves prefab');prefix=relative(parent);return (prefix+'/' if prefix else '')+gos[gid]
 bypath={}
 for tid,(gid,parent) in transforms.items():
  path=relative(tid);require(path not in bypath,'Ambiguous root-relative hierarchy path');bypath[path]=gid
 owners=[d for d in docs.values() if d['kind']==114 and ref(d['body'],'m_GameObject')==root_gid and all(re.search(r'(?m)^  '+re.escape(f)+r':',d['body']) for f in FIELDS)];require(len(owners)==1,'Unique original field-bearing owner required');owner=owners[0];body=owner['body'];changes=[]
 for row in plan['entries']:
  field=row['field'];path=row['relative_gameobject_path'];require(path in bypath,'Missing original child '+path);gid=bypath[path];kind=row['component_type']
  markers=['m_Sprite:','m_Type:','m_PreserveAspect:'] if kind=='UnityEngine.UI.Image' else ['m_OnClick:','m_Interactable:']
  matches=[(anchor,d) for anchor,d in docs.items() if d['kind']==114 and ref(d['body'],'m_GameObject')==gid and all(re.search(r'(?m)^  '+re.escape(m),d['body']) for m in markers)];require(len(matches)==1,'Ambiguous/missing original '+kind+' at '+path);target=matches[0][0]
  require(re.search(r'(?m)^  m_Script: \{fileID: -?\d+, guid: [0-9a-f]{32}, type: 3\}',matches[0][1]['body']) is not None,'Typed script reference missing at '+path)
  pattern=r'(?m)^(  '+re.escape(field)+r': \{fileID: )(-?\d+)(\}[^\r\n]*\r?)$';hits=list(re.finditer(pattern,body));require(len(hits)==1,'Unique serialized field missing '+field);old=int(hits[0][2]);require(old in [0,target],'Refuse to replace a different non-null reference '+field)
  if old!=target:body,n=re.subn(pattern,lambda m:m[1]+str(target)+m[3],body);require(n==1,'Unexpected reference substitution count')
  changes.append(dict(field=field,relative_gameobject_path=path,component_type=kind,before_file_id=old,after_file_id=target,changed=old!=target))
 patched=(text[:owner['start']]+body+text[owner['end']:]).encode('utf-8');undo=body
 for row in changes:
  if row['changed']:undo,n=re.subn(r'(?m)^(  '+re.escape(row['field'])+r': \{fileID: )'+str(row['after_file_id'])+r'(\}[^\r\n]*\r?)$',lambda m:m[1]+str(row['before_file_id'])+m[2],undo);require(n==1,'Cannot independently undo field patch')
 require((text[:owner['start']]+undo+text[owner['end']:]).encode('utf-8')==data,'Unrelated prefab bytes changed')
 return patched,changes
def inspect(project,plan_path,output):
 root=Path(project).resolve();out=Path(output).resolve();require((root/'ProjectSettings/ProjectVersion.txt').is_file(),'Require reconstructed Unity project');plan=json.loads(Path(plan_path).read_bytes());matches=[p for p in (root/'Assets').rglob('*.prefab') if p.stem=='ZombieWindow'];require(len(matches)==1,'Unique source ZombieWindow prefab required');target=matches[0];require(target.resolve().is_relative_to(root/'Assets') and not target.is_symlink(),'Unsafe prefab path');before=target.read_bytes();out.mkdir(parents=True,exist_ok=True);receipt=out/'r21-original-zombie-bindings-source-only.json';require(not receipt.exists(),'Do not overwrite source inspection');(out/'r21-zombie-prefab-source-before-migration.prefab').write_bytes(before)
 try:after,changes=patch(before,plan)
 except Exception as e:
  (out/'r21-original-zombie-bindings-source-failure.json').write_bytes((json.dumps(dict(error=repr(e),input_sha256=sha(before),source_changed=False),indent=2)+'\n').encode());raise
 require(target.read_bytes()==before,'Read-only source inspection changed the file');result=dict(scope='SOURCE_PREFAB_BINDING_PARSER_PREFLIGHT_BEFORE_LICENSE_OR_EDITOR',target=target.relative_to(root).as_posix(),before_sha256=sha(before),would_change_sha256=sha(after),changes=changes,source_changed=False,unity_run=False);receipt.write_bytes((json.dumps(result,indent=2)+'\n').encode());return result
def repair(project,plan_path,output):
 root=Path(project).resolve();out=Path(output).resolve();require((root/'ProjectSettings/ProjectVersion.txt').is_file(),'Require disposable reconstructed Unity project');plan=json.loads(Path(plan_path).read_bytes());assets=root/'Assets';matches=[p for p in assets.rglob('*.prefab') if p.stem=='ZombieWindow'];require(len(matches)==1,'Unique source ZombieWindow prefab required');target=matches[0];require(target.resolve().is_relative_to(assets) and not target.is_symlink(),'Unsafe prefab source path');out.mkdir(parents=True,exist_ok=True);receipt=out/'r21-original-zombie-bindings.json';require(not receipt.exists(),'Do not overwrite binding evidence')
 before=target.read_bytes();meta=target.with_suffix('.prefab.meta').read_bytes();(out/'r21-zombie-prefab-before.prefab').write_bytes(before);original=inventory(assets)
 try:after,changes=patch(before,plan)
 except Exception as e:
  (out/'r21-original-zombie-bindings-patch-failure.json').write_bytes((json.dumps(dict(error=repr(e),input_sha256=sha(before),source_changed=False),indent=2)+'\n').encode());raise
 target.write_bytes(after);readback=target.read_bytes();require(readback==after,'Prefab byte readback differs');require(target.with_suffix('.prefab.meta').read_bytes()==meta,'Prefab GUID/meta changed');current=inventory(assets);changed=[p for p in original if original[p]!=current.get(p)];require(set(original)==set(current) and changed in [[],[target.relative_to(assets).as_posix()]],'Other asset bytes changed')
 (out/'r21-zombie-prefab-after.prefab').write_bytes(after);result=dict(scope=plan['scope'],project_asset_count=len(original),changed_asset_paths=changed,target=target.relative_to(root).as_posix(),before_sha256=sha(before),after_sha256=sha(after),meta_sha256=sha(meta),only_six_reference_values_permitted=True,all_other_prefab_bytes_preserved=True,all_other_asset_bytes_preserved=True,changes=changes,unity_import_or_game_acceptance=False)
 receipt.write_bytes((json.dumps(result,indent=2)+'\n').encode());return result
if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--plan',required=True);p.add_argument('--output',required=True);p.add_argument('--inspect-only',action='store_true');a=p.parse_args();print(json.dumps((inspect if a.inspect_only else repair)(a.project,a.plan,a.output)))
