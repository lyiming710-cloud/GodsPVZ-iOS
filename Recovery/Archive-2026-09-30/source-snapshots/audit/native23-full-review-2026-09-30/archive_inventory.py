from pathlib import Path
import os,json,collections
root=Path(__file__).resolve().parents[2]
out=root/'audit/project-archive-2026-09-30';out.mkdir(exist_ok=True)
prune={'.git','node_modules','__pycache__','.venv','python-deps','bin','obj','Library','Temp','Logs','UserSettings','PackageCache'}
skiproots={'native23-publication','project-archive-2026-09-30'}
rows=[];groups=collections.defaultdict(lambda:{'files':0,'bytes':0,'extensions':collections.Counter(),'largest':[]});excluded=collections.Counter()
for top,dirs,names in os.walk(root):
 rel=Path(top).relative_to(root)
 keep=[]
 for d in dirs:
  if d in prune or d in skiproots:excluded[d]+=1
  else:keep.append(d)
 dirs[:]=keep
 for n in names:
  f=Path(top)/n
  if f.is_symlink():continue
  stat=f.stat();rr=f.relative_to(root).as_posix()
  parts=Path(rr).parts;group='/'.join(parts[:2]) if parts[0]=='audit' else parts[0] if len(parts)>1 else 'root-files'
  row={'path':rr,'bytes':stat.st_size,'group':group,'extension':f.suffix.lower()}
  rows.append(row);g=groups[group];g['files']+=1;g['bytes']+=stat.st_size;g['extensions'][f.suffix.lower()]+=1;g['largest'].append(row)
for g in groups.values():g['largest']=sorted(g['largest'],key=lambda x:-x['bytes'])[:8];g['extensions']=dict(g['extensions'])
report={'root':str(root),'files':len(rows),'bytes':sum(x['bytes'] for x in rows),'excluded_dirs':dict(excluded),'groups':dict(groups)}
(out/'inventory.json').write_text(json.dumps(report,ensure_ascii=False,indent=2),encoding='utf-8')
(out/'inventory-files.json').write_text(json.dumps(rows,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps({'files':report['files'],'GB':round(report['bytes']/1e9,3),'groups':[{ 'name':k,'files':v['files'],'MB':round(v['bytes']/1e6,1),'largest':[(x['path'],round(x['bytes']/1e6,1)) for x in v['largest'][:3]]} for k,v in sorted(groups.items())],'excluded_dirs':dict(excluded)},ensure_ascii=False,indent=2))
