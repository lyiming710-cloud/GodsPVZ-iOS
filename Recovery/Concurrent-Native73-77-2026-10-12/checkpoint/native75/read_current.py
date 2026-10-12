from pathlib import Path
import json
d=json.loads(Path('audit/native75-codex-2026-10-12/metadata-current.json').read_text())
for key in ('input_sha256','targets','refs','getters'):
 print(key,json.dumps(d[key],indent=2))
print('FIELD',[f for t in d['internals']if t['name']=='Almanac_PlantDetail'for f in t['fields']if 'animtion_'in f['name']])
