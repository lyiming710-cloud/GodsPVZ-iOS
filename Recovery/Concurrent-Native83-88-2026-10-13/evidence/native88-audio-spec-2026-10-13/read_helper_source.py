from pathlib import Path
import json
p=Path('audit/native88-audio-spec-2026-10-13');p.mkdir(exist_ok=True)
d=json.loads(Path('audit/native87-spec-2026-10-13/PRIMARY-EVIDENCE.json').read_text())
m=json.loads(Path('audit/native87-spec-2026-10-13/MANAGED-SOURCE-EVIDENCE.json').read_text())
helpers=[x for x in d['native_helpers'] if int(str(x['start']),0)==0x18031B350]
assembly=m['assemblies'][0]
method=[x for x in assembly['methods'] if int(str(x.get('token','0')),0)==0x0600013E]
print('HELPER FIELDS', [list(x) for x in method])
if helpers:print(helpers[0]['disassembly'])
if method:print(json.dumps(method[0],indent=2))
(p/'helper-source.json').write_text(json.dumps({'original_pe_sha256':'9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d','original_helpers':helpers,'parent82_methods':method,'scope':'Static source evidence only; no tests or application execution'},indent=2))
