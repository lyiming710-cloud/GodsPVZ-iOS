from pathlib import Path
import json,hashlib
A=Path(__file__).resolve().parents[1];R=A/'native15-work';E=R/'Recovery/Native16-2026-09-28';E.mkdir(parents=True,exist_ok=True)
rows=[r for r in json.loads((A/'native15-evidence/mapping.json').read_text()) if r['name'] in ('SetSorting','Remove','UnorderedRemoveAt')]
(E/'mapping.json').write_text(json.dumps(rows,indent=2)+'\n',newline='\n')
for r in rows:
 n=r['type']+'-'+r['va'][2:]+'.asm';(E/n).write_text((A/'native15-evidence'/n).read_text(),newline='\n')
s=(A/'local-preflight/audit_generic_remaining.py').read_text().replace("R=Path(__file__).resolve().parents[1];", "if len(sys.argv)!=2:raise SystemExit('usage: extract_native.py <audit-root-containing-stage9-native4>')\nR=Path(sys.argv[1]).resolve();")
(E/'extract_native.py').write_text(s,newline='\n')
s=(A/'native15-evidence/native16-annotations.txt').read_text(encoding='utf-8')
(E/'native-annotations.txt').write_text(s,encoding='utf-8',newline='\n')
v=R/'Recovery/Native15-2026-09-28/validation';manifest=json.loads((v/'SHA256.json').read_text())
assert all(hashlib.sha256((v/n).read_bytes()).hexdigest()==h for n,h in manifest.items())
print('NATIVE15_ARCHIVE_HASHES_PASS',len(manifest))

