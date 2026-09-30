from pathlib import Path
import json,base64,hashlib,zipfile,io
p=Path(__file__).resolve().parent
head,encoded=(p/'archive-transport.txt').read_bytes().split(b'\n',1)
meta=json.loads(head);data=base64.b64decode(encoded,validate=True)
assert len(data)==meta['bytes'];assert hashlib.sha256(data).hexdigest()==meta['sha256']
dest=p/'full-review-evidence.zip';dest.write_bytes(data)
out=p/'evidence';out.mkdir(exist_ok=True)
with zipfile.ZipFile(io.BytesIO(data)) as z:
 for info in z.infolist():
  target=(out/info.filename).resolve()
  assert target.is_relative_to(out.resolve()),info.filename
 z.extractall(out)
man=json.loads((out/'archive-manifest.json').read_text())
for name,record in man.items():
 raw=(out/name).read_bytes();assert len(raw)==record['bytes'];assert hashlib.sha256(raw).hexdigest()==record['sha256'],name
(p/'DOWNLOAD-VERIFIED.json').write_text(json.dumps(dict(meta,verified_members=len(man)),indent=2),encoding='utf-8')
analysis=json.loads((out/'review/analysis.json').read_text());iso=json.loads((out/'review/independent-isolation.json').read_text());final=json.loads((out/'review/final-checks.json').read_text())
print(json.dumps({'archive':meta,'verified_members':len(man),'local_receiver_methods':len({r['token'] for r in analysis['actual_accepted_analysis']['all_direct_local_receiver_sites']}),'rows':iso[0]['tables_before'],'methods_with_errors':{t:final[t]['methods_with_errors'] for t in ('baseline','batch1','batch2','batch2a')}},indent=2))
