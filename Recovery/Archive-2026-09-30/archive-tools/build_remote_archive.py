from pathlib import Path
import os,json,hashlib,zipfile,subprocess,time
root=Path('/workspaces/GodsPVZ-native19');out=Path('/tmp/godspvz-archive-2026-09-30');out.mkdir(exist_ok=True)
rows=json.loads(Path('/tmp/godspvz-archive-inventory.json').read_text())['files'] if Path('/tmp/godspvz-archive-inventory.json').exists() else []
if not rows:
 for top,dirs,names in os.walk(root/'.validation'):
  dirs[:]=[d for d in dirs if d not in {'bin','obj','__pycache__','node_modules','dotnet','runtime','canary'}]
  for n in names:
   p=Path(top)/n
   if not p.is_symlink():rows.append({'path':p.relative_to(root).as_posix(),'bytes':p.stat().st_size})
def reason(rel):
 if rel.startswith('.validation/xcode-native18/expanded/'):return 'Duplicate expanded Xcode project; preserved exact tar.zst export instead'
 if rel=='.validation/xcode-native18/artifact.zip':return 'Duplicate Actions download container; preserved its tar.zst export and artifact metadata'
 if any(s in rel.lower() for s in ('license-activate','license-return')):return 'Tool activation logs excluded'
 if rel.endswith(('.pdb','.dylib','.exe')) or '/bin23/' in rel or '/inspector-bin/' in rel or '/patcher-bin/' in rel:return 'Rebuildable tool output'
 return None
manifest={'schema':1,'origin':'codespace:glowing-train-p7j9gp74q6jwc76v6:/workspaces/GodsPVZ-native19','head':subprocess.check_output(['git','-C',str(root),'rev-parse','HEAD'],text=True).strip(),'files':[],'excluded':[],'layout':'objects/sha256 stores one copy per exact content; file rows restore original relative paths','acceptance':'Historical and diagnostic evidence only. Candidate archive membership is not semantic acceptance.'}
assert not subprocess.check_output(['git','-C',str(root),'status','--porcelain'],text=True).strip()
seen=set();export=None;zp=out/'codespace-historical-evidence.zip'
with zipfile.ZipFile(zp,'w',zipfile.ZIP_DEFLATED,compresslevel=6,allowZip64=True) as z:
 for row in rows:
  rel=row['path'];why=reason(rel)
  if why:manifest['excluded'].append(dict(row,reason=why));continue
  p=root/rel;h=hashlib.sha256()
  with p.open('rb') as f:
   for chunk in iter(lambda:f.read(1024*1024),b''):h.update(chunk)
  digest=h.hexdigest();record=dict(row,sha256=digest,object='objects/'+digest)
  if rel.endswith('GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst'):
   record['asset']='GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst';record.pop('object');export=dict(record,absolute=str(p))
  elif digest not in seen:
   z.write(p,'objects/'+digest);seen.add(digest)
  manifest['files'].append(record)
 z.writestr('MANIFEST.json',json.dumps(manifest,ensure_ascii=False,indent=2))
(out/'CODESPACE-MANIFEST.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2))
h=hashlib.sha256()
with zp.open('rb') as f:
 for c in iter(lambda:f.read(1024*1024),b''):h.update(c)
metadata={'archive':str(zp),'bytes':zp.stat().st_size,'sha256':h.hexdigest(),'files':len(manifest['files']),'objects':len(seen),'excluded':len(manifest['excluded']),'export':export,'head_after':subprocess.check_output(['git','-C',str(root),'rev-parse','HEAD'],text=True).strip()}
(out/'metadata.json').write_text(json.dumps(metadata,indent=2));print(json.dumps(metadata))
