from pathlib import Path
import json,hashlib,zipfile,re,shutil,collections,time
p=Path(__file__).parent;root=p.parents[1];work=p.parent/'native23-publication';dest=work/'Recovery/Archive-2026-09-30';dest.mkdir(parents=True,exist_ok=True)
rows=json.loads((p/'inventory-files.json').read_text(encoding='utf-8'))
codeext={'.py','.cs','.csproj','.sh','.ps1','.mjs','.js','.ts','.yml','.yaml','.props','.targets','.sln','.mermaid'}
secrets=re.compile(rb'(?:gh[pousr]_[A-Za-z0-9]{30,}|github_pat_[A-Za-z0-9_]{30,}|-----BEGIN (?:RSA |EC |OPENSSH )?PRIVATE KEY-----)')
def reason(rel):
 s=rel.lower()
 if rel.startswith('.workbuddy/') or rel=='audit/_net-adapters.txt':return 'Personal assistant memory or machine diagnostics outside project scope'
 if Path(rel).name=='.git':return 'Machine-specific worktree pointer; refs/history archived separately'
 if rel.startswith('audit/stage9-native4/inputs/editor-parts/'):return 'Redownloadable Unity Editor installer cache; not game source'
 if rel.startswith('audit/local-preflight/dotnet-runtime'):return 'Redownloadable .NET runtime cache'
 if rel.startswith('audit/local-preflight/exact-editor-tools/') or rel.startswith('audit/local-preflight/windows-codegen/'):return 'Unity/.NET tool distribution cache; retain validation commands and outputs'
 if '/tools/actionlint' in s:return 'Redownloadable third-party executable'
 if Path(rel).name in {'archive-transport.txt','checkpoints-transport.txt'}:return 'Base64 transport duplicate; decoded bytes archived'
 if any(t in s for t in ('license-activate','license-return')):return 'Tool activation/return logs excluded'
 if Path(rel).suffix.lower() in {'.ulf','.pfx','.p12','.key'} or Path(rel).name=='.env':return 'Credential or license material'
 return None
def category(rel):
 if rel.startswith(('GodsPVZ_1.0.2.zip','1.0.2Android/','.inspect/')) or rel=='Assembly-CSharp-59bb-native2.dll':return '01-original-inputs'
 if rel.startswith('build/'):return '09-invalid-early-output'
 if '/handoff-' in rel or '/Handoff' in rel or Path(rel).suffix in {'.md','.txt'} and 'HANDOFF' in rel.upper():return '07-handoffs-and-reports'
 if '/actions-' in rel or '/real-unity/' in rel:return '06-ci-evidence'
 if Path(rel).suffix.lower() in codeext:return '02-code-and-workflows'
 if Path(rel).suffix.lower() in {'.dll','.pdb'} and 'resolver' not in rel.lower():return '04-baselines-and-experimental-candidates'
 if 'native23' in rel:return '05-native23-independent-review'
 return '03-historical-data-and-evidence'
manifest={'schema':1,'date':'2026-09-30','origin':'local project workspace','original_relative_paths':True,'acceptance':'Preserved history does not imply approval. Native23 Batch2/Batch2a remain experimental; no valid IPA is present. The 8 KB early IPA is explicitly invalid.','layout':'objects/<sha256> deduplicates exact bytes; files maps every preserved original path to its object or standalone asset','files':[],'excluded':[],'excluded_directories':json.loads((p/'inventory.json').read_text(encoding='utf-8'))['excluded_dirs']}
seen=set();counts=collections.Counter();browse=[];zpath=p/'local-project-history.zip'
with zipfile.ZipFile(zpath,'w',zipfile.ZIP_DEFLATED,compresslevel=6,allowZip64=True) as z:
 for row in rows:
  rel=row['path'];why=reason(rel)
  if why:manifest['excluded'].append(dict(row,reason=why));continue
  f=root/rel;size=f.stat().st_size
  if size!=row['bytes']:raise RuntimeError('Source changed during archive: '+rel)
  h=hashlib.sha256()
  with f.open('rb') as inp:
   for chunk in iter(lambda:inp.read(1024*1024),b''):h.update(chunk)
  digest=h.hexdigest();cat=category(rel);record=dict(row,sha256=digest,category=cat,object='objects/'+digest)
  if size<2000000 and f.suffix.lower() in codeext|{'.txt','.md','.json','.log','.xml','.config'}:
   if secrets.search(f.read_bytes()):raise RuntimeError('Credential scan blocked '+rel)
  if rel=='GodsPVZ_1.0.2.zip':record['asset']='GodsPVZ_1.0.2.zip';record.pop('object')
  elif rel=='1.0.2Android/GodsPVZ_1.0.2_Android.apk':record['asset']='GodsPVZ_1.0.2_Android.apk';record.pop('object')
  elif digest not in seen:
   z.write(f,'objects/'+digest);seen.add(digest)
  manifest['files'].append(record);counts[cat]+=1
  if f.suffix.lower() in codeext and size<2000000:
   target=dest/'source-snapshots'/rel;target.parent.mkdir(parents=True,exist_ok=True);shutil.copyfile(f,target);browse.append({'path':'source-snapshots/'+rel,'sha256':digest,'original':rel})
 z.writestr('MANIFEST.json',json.dumps(manifest,ensure_ascii=False,indent=2))
(p/'LOCAL-MANIFEST.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2),encoding='utf-8')
(dest/'LOCAL-MANIFEST.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2),encoding='utf-8')
(dest/'SOURCE-INDEX.json').write_text(json.dumps(browse,ensure_ascii=False,indent=2),encoding='utf-8')
with zipfile.ZipFile(zpath) as z:assert z.testzip() is None
summary={'files':len(manifest['files']),'unique_objects':len(seen),'excluded_files':len(manifest['excluded']),'archived_source_bytes':sum(r['bytes'] for r in manifest['files']),'zip_bytes':zpath.stat().st_size,'browseable_source_files':len(browse),'categories':dict(counts)}
(p/'local-summary.json').write_text(json.dumps(summary,indent=2),encoding='utf-8');print(json.dumps(summary,indent=2),flush=True)
