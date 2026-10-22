"""Freeze source deliveries and copy immutable evidence; no project execution."""
from pathlib import Path
import json,hashlib,shutil
R=Path(__file__).resolve().parents[1]
E=R/'audit/native116-bulk250-r1-codex-2026-10-22';S=R/'audit/native-bulk250-r1-codex-2026-10-22';N=R/'audit/native117-codex-2026-10-22'
for f in ['SOURCE-API-EVIDENCE.json','SOURCE-REPAIR-CHANGELOG.json','PRIOR-BATCH-RESULT-REVIEW.json']:shutil.copyfile(E/f,S/f)
shutil.copyfile(E/'SOURCE-API-EVIDENCE.json',N/'SUPPLEMENTAL-SOURCE-API-EVIDENCE.json')
tokens=['0x06000163','0x060004BF','0x06000297','0x06000384','0x06000514','0x0600035B']
p=E/'original-native-and-current-cil';p.mkdir(exist_ok=True)
for token in tokens:
    for suffix in ['-current-il.txt','-original.asm']:shutil.copyfile(R/'audit/native116-codex-2026-10-22'/(token+suffix),p/(token+suffix))
def freeze(d):
    files=[dict(path=p.relative_to(d).as_posix(),bytes=p.stat().st_size,sha256=hashlib.sha256(p.read_bytes()).hexdigest()) for p in sorted(d.rglob('*')) if p.is_file() and p.name!='SOURCE-MANIFEST.json']
    data=dict(schema='source-only-manifest-v1',date='2026-10-22',state='SOURCE_ONLY_UNBUILT_UNEXECUTED',candidate=None,root_executed_project_tests=False,files=files)
    (d/'SOURCE-MANIFEST.json').write_text(json.dumps(data,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
freeze(E);freeze(S)
