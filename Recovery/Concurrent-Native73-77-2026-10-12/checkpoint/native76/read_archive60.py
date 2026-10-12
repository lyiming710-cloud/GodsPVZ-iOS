from pathlib import Path
import zipfile,json
p=Path('audit/native60-gemini-tests-2026-10-12')
d=json.loads((p/'MANIFEST-v5.json').read_text())
with zipfile.ZipFile(p/'Archive-v5.zip')as z:
 print('FILES',len(z.namelist()),[f for f in z.namelist()if any(t in f for t in ('runner','master','runtime','Cecil','process','deps','attempt'))])
 print('MANIFEST_KEYS',list(d))
 print('EXTERNAL',json.dumps(d.get('external_inputs',{}),indent=2))
for r in json.loads(Path('audit/stage9-native3/evidence/native-method-map.json').read_text()):
 if r['va']=='0x18131f760':print('OBJECT_OP',r)
