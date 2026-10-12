from pathlib import Path
import json
d=json.loads(Path('audit/native76-codex-2026-10-12/metadata-current.json').read_text())
for key in ('input_sha256','targets','specs'):
 print(key,json.dumps(d.get(key),indent=2))
for t in d['internals']:
 if t['name']in ('MainUIController','SavesUIController','SavesUIController_PlayerSave','Device_boardEditor','PreDevice','DeviceData','Device'):
  print('FIELDS',t['name'],json.dumps(t['fields']))
  print('GETTERS',[m for m in t['methods']if 'get_'in m['name']])
print('BoardGetter',[v for v in d['getters']if v['token']=='0x06000169'])
