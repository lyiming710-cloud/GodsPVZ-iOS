from pathlib import Path
import zipfile,json
for n in(60,61):
 p=Path(f'audit/native{n}-gemini-tests-2026-10-12')
 print('RECENT',n,[(f.name,f.stat().st_size)for f in sorted(p.iterdir(),key=lambda f:f.stat().st_mtime,reverse=True)[:16]if f.is_file()])
 for name in('FIXTURE_REPORT.json','strict-result-v3.json'):
  f=p/name
  if f.exists():
   d=json.loads(f.read_text());print(name,n,json.dumps({k:v for k,v in d.items()if k not in('historical_regressions',)},ensure_ascii=False)[:5500])
 f=p/'Archive-v6.zip'
 if f.exists():
  with zipfile.ZipFile(f)as z:
   print('V6_MEMBERS',len(z.namelist()),[x for x in z.namelist()if 'INPUT'in x.upper()or'DEPEND'in x.upper()])
   for name in z.namelist():
    if('DEPEND'in name.upper()or'INPUT'in name.upper())and name.endswith(('.json','.md')):print(name,z.read(name).decode()[:6000])
