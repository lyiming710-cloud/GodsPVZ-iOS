from pathlib import Path
import re,json,hashlib,collections
p=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review');w=p.parent/'native22'
res={}
for tag,old in [('baseline',w/'regen-baseline/cpp'),('batch1',w/'regen-batch1/cpp'),('batch2',w/'regen-batch2/cpp'),('batch2a',None)]:
 allword=0;col=0;extra=[];counts={};drift=[];source_kind=collections.Counter();headers=[]
 for f in sorted((p/tag/'clang').glob('*.log')):
  s=f.read_text(errors='replace');counts[f.name]=0
  for line in s.splitlines():
   if re.search(r'\berror:',line):
    allword+=1
    if re.match(r'^.+:\d+:\d+:\s+(fatal )?error:',line):
     col+=1;counts[f.name]+=1
     filename=line.split(':',1)[0];source_kind[Path(filename).suffix]+=1
     if not filename.endswith(('.cpp','.c')) and len(headers)<8:headers.append(line)
    elif len(extra)<12:extra.append(line)
 if old:
  for q in (p/tag/'cpp').glob('*.cpp'):
   r=old/q.name
   if not r.exists() or hashlib.sha256(q.read_bytes()).digest()!=hashlib.sha256(r.read_bytes()).digest():drift.append(q.name)
 res[tag]={'all_error_strings':allword,'source_diagnostics':col,'diagnostics_by_extension':dict(source_kind),'header_error_examples':headers,'non_source_error_strings_examples':extra,'counts_by_file':counts,'generated_cpp_byte_drift_from_archived':drift}
(p/'diagnostics.json').write_text(json.dumps(res,indent=2))
print(json.dumps({t:{k:v for k,v in d.items() if k!='counts_by_file'} for t,d in res.items()},indent=2))
