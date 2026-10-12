from pathlib import Path
import json,zipfile
d=json.loads(Path('audit/native77-codex-2026-10-12/metadata-current.json').read_text())
print('TARGETS',json.dumps(d['targets'],indent=2))
for t in d['internals']:
 if t['name']=='ZombieAnimationEvent':print(t['fields'])
print('GETTER',[m for m in d['getters']if m['token']=='0x060001E5'])
print('MOVE_IL',[m for m in d['getters']if m['token']=='0x060003D9'])
with zipfile.ZipFile('audit/native60-gemini-tests-2026-10-12/Archive-v5.zip')as z:
 print('DEPS',z.read('docs/EXTERNAL-DEPENDENCIES.md').decode())
