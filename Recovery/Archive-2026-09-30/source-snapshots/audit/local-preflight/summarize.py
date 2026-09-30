import json,collections
from pathlib import Path
R=Path(__file__).resolve().parent
lines=[]
for name in ['baseline','native4']:
 d=json.loads((R/(name+'.json')).read_text());bad=[m for m in d['results'] if m['errors']];hints=[m for m in d['results'] if m['hints']]
 line=f"{name}: bodies={d['methodBodies']} methods_with_errors={len(bad)} methods_with_hints={len(hints)} unsupported_methods={sum(bool(m['unsupported']) for m in d['results'])}"
 lines.append(line);lines.append('error categories '+str(dict(collections.Counter(e['category'] for m in bad for e in m['errors']))))
 for m in d['results']:
  if m['token'] in ['0x0600039F','0x06000267','0x060005BE']:
   lines.append(m['token']+' '+m['name']+'\n'+json.dumps({'errors':m['errors'],'hints':m['hints']},ensure_ascii=False))
 if name=='native4':
  rows=['token\tmethod\terror_count\tcategories\tfirst_error']
  for m in bad:rows.append('\t'.join([m['token'],m['name'],str(len(m['errors'])),','.join(sorted({e['category'] for e in m['errors']})),m['errors'][0]['offset']+' '+m['errors'][0]['detail']]))
  (R/'native4-findings.tsv').write_text('\n'.join(rows)+'\n')
  lines.append('first remaining errors:')
  lines.extend(m['token']+' '+m['name']+' '+json.dumps(m['errors'][:2]) for m in bad[:12])
(R/'summary.txt').write_text('\n'.join(lines)+'\n');print('\n'.join(lines))
