from pathlib import Path
import json,hashlib,collections
R=Path(__file__).resolve().parent
report=[]
for p in sorted((R/'specification').glob('*.spec.json')):
 a=json.loads(p.read_text());b=json.loads((R/'reopened'/p.name).read_text())
 for d in [a,b]:
  d.pop('dependency',None);d.pop('maxStack',None)
  for i in d['instructions']:i.pop('native',None)
 assert a==b,p.name
 report.append({'method':a['name'],'instruction_exact_match':True})
assert len(report)==6
# Lock complete observed use deltas, then later gate executions must match this reviewed manifest.
delta=R/'evidence/reference-use-deltas.tsv'
locked=R/'specification/expected-reference-use-deltas.tsv'
assert locked.exists(),'review and freeze use delta manifest before running'
assert delta.read_text().splitlines()==locked.read_text().splitlines(),'reference use deltas changed'
(R/'evidence/spec-readback-verification.json').write_text(json.dumps({'readback':report,'use_delta_manifest_sha256':hashlib.sha256(locked.read_bytes()).hexdigest(),'status':'PASS'},indent=2))
print('SPEC_READBACK=6/6; ALL_REFERENCE_USE_DELTAS=EXACT_MATCH')
