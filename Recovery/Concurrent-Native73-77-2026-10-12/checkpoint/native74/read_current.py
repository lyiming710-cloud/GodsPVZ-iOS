from pathlib import Path
import json
p=Path('audit/native74-codex-2026-10-12/metadata-current2.json')
d=json.loads(p.read_text())
for key in ('input_sha256','targets','specs','external_fields'):
 print(key,json.dumps(d.get(key),indent=2))
for t in d['internals']:
 if t['name']in ('BoardManager','BoardPreview','SettlementUIController'):
  print('FIELDS',t['name'],json.dumps(t['fields']))
print('GETTERS',json.dumps(d['getters'],indent=2))
