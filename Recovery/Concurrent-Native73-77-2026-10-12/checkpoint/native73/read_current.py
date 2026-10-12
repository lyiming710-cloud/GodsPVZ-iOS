from pathlib import Path
import json,zipfile
p=Path('audit/native73-codex-2026-10-12')
if(p/'metadata-current.json').exists():
    d=json.loads((p/'metadata-current.json').read_text())
    print('TARGETS',json.dumps(d['targets'],indent=2))
    print('REFS',json.dumps(d['refs'],indent=2))
    print('IL',json.dumps(d['getters'],indent=2))
    for t in d['internals']:
        print('INTERNAL',t['name'],json.dumps([m for m in t['methods']if any(x in m['name']for x in('SetEvent','get_Instance','149'))]))
        print('FIELDS',json.dumps([m for m in t['fields']if any(x in m['name']for x in('f','blockable','particle','instance','plant'))]))
with zipfile.ZipFile('audit/native60-gemini-tests-2026-10-12/Archive-v4.zip')as z:
    for name in ('sources/Native60Fixture.cs','reports/FIXTURE_REPORT.json'):
        lines=z.read(name).decode().splitlines()
        print(name,'\n'.join(lines[-95:]))
