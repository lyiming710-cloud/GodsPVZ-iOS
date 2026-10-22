"""Extract immutable source evidence; no candidate construction or target execution."""
from pathlib import Path
import json
import shutil

ROOT = Path(__file__).resolve().parents[1]
out = ROOT / 'audit/native120-codex-2026-10-23'
out.mkdir(exist_ok=True)
maps = json.loads((ROOT / 'audit/offline-test-handoff-2026-10-14/payload/inputs/maps/native-method-map.json').read_text('utf-8-sig'))
entry = next(m for m in maps if m['image'] == 'Assembly-CSharp.dll' and int(m['token'], 16) == 0x060003F9)
start = int(entry['va'], 16)
end = min(int(m['va'], 16) for m in maps if int(m['va'], 16) > start)
prior = out / 'ORIGINAL-SOURCE.json'
if prior.exists() and json.loads(prior.read_text('utf-8'))['methods'][0]['start'] != hex(start):
    withdrawn = out / 'withdrawn-ambiguous-token-read'
    withdrawn.mkdir(exist_ok=True)
    for name in ['CURRENT-SOURCE.json', 'ORIGINAL-SOURCE.json', '0x060003F9-original.asm', '0x060003F9-current-il.txt']:
        shutil.copyfile(out / name, withdrawn / name)
    (withdrawn / 'README.md').write_text('WITHDRAWN source read: MethodDef tokens are image-scoped. The first map match belonged to another image. No candidate or acceptance was produced. The active reader pins Assembly-CSharp.dll.\n', encoding='utf-8')
source = (ROOT / 'audit/native119-codex-2026-10-22/read_source_evidence.py').read_text('utf-8')
begin = source.index('ranges = {')
finish = source.index('\n', begin)
source = source[:begin] + f'ranges = {{0x060003F9: ({start}, {end})}}' + source[finish:]
(out / 'read_source_evidence.py').write_text(source, encoding='utf-8', newline='\n')
print(json.dumps({'source_reader': str(out / 'read_source_evidence.py'), 'start': hex(start), 'end': hex(end), 'scope': 'SOURCE_READER_AUTHORING_ONLY'}))
