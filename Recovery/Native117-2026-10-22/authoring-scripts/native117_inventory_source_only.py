"""Read immutable metadata and worklist for source authoring; never execute a target."""
from pathlib import Path
import json, sys
sys.dont_write_bytecode=True
ROOT=Path(__file__).resolve().parents[1]
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
registry=read(ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v17-2026-10-22.json')
maps=read(ROOT/'audit/offline-test-handoff-2026-10-14/payload/inputs/maps/native-method-map.json')
vas=sorted({int(m['va'],16) for m in maps if int(m['va'],16)})
import bisect
bytoken={f"0x{int(m['token'],16):08X}":m for m in maps if 'Assembly-CSharp' in str(m.get('image',''))}
print('MAP_KEYS',maps[0].keys(),'STATES',sorted({m['state'] for m in registry['methods']}))
rows=[]
for m in registry['methods']:
    if m['state']=='SOURCE_WRITTEN_PENDING_VERIFICATION':continue
    n=bytoken.get(m['token'])
    if not n:continue
    va=int(n['va'],16);i=bisect.bisect_right(vas,va)
    rows.append(dict(token=m['token'],name=n['type']+'::'+n['name'],start=va,end=vas[i],size=vas[i]-va,category=m['primary_category'],state=m['state']))
rows.sort(key=lambda m:(m['size'],m['token']))
out=ROOT/'audit/native117-codex-2026-10-22';out.mkdir(exist_ok=True)
(out/'UNWRITTEN-SOURCE-QUEUE.json').write_text(json.dumps(rows,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
for m in rows:print(m['token'],m['size'],m['name'],m['category'])
