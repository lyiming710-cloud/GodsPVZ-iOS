"""Verify and restore hash-addressed project archives; Python standard library only."""
from pathlib import Path,PurePosixPath
import argparse,json,hashlib,zipfile,shutil
p=argparse.ArgumentParser();p.add_argument('--archive',type=Path,required=True);p.add_argument('--manifest',type=Path);p.add_argument('--assets',type=Path,required=True);p.add_argument('--destination',type=Path);p.add_argument('--verify-only',action='store_true');a=p.parse_args()
if not a.verify_only and not a.destination:p.error('--destination is required for restoration')
def digest(stream):
 h=hashlib.sha256();size=0
 for c in iter(lambda:stream.read(1024*1024),b''):h.update(c);size+=len(c)
 return h.hexdigest(),size
def safe_path(base,rel):
 pp=PurePosixPath(rel)
 if pp.is_absolute() or '..' in pp.parts or any(':' in q for q in pp.parts):raise ValueError('Unsafe archived path: '+rel)
 base=base.resolve();target=(base/Path(*pp.parts)).resolve();target.relative_to(base);return target
with zipfile.ZipFile(a.archive) as z:
 m=json.loads(a.manifest.read_text(encoding='utf-8')) if a.manifest else json.loads(z.read('MANIFEST.json'))
 verified={}
 for r in m['files']:
  key=r.get('object') or ('asset:'+r['asset'])
  if key not in verified:
   if 'object' in r:
    with z.open(r['object']) as inp:got=digest(inp)
   else:
    with safe_path(a.assets,r['asset']).open('rb') as inp:got=digest(inp)
   assert got==(r['sha256'],r['bytes']),(r['path'],got)
   verified[key]=got
  assert verified[key]==(r['sha256'],r['bytes']),r['path']
 if not a.verify_only:
  for r in m['files']:
   out=safe_path(a.destination,r['path']);out.parent.mkdir(parents=True,exist_ok=True)
   if out.exists():
    with out.open('rb') as inp:got=digest(inp)
    if got!=(r['sha256'],r['bytes']):raise FileExistsError('Refusing to overwrite different file: '+str(out))
    continue
   inp=z.open(r['object']) if 'object' in r else safe_path(a.assets,r['asset']).open('rb')
   with inp,out.open('wb') as dst:shutil.copyfileobj(inp,dst,1024*1024)
print(json.dumps({'files':len(m['files']),'unique_payloads_verified':len(verified),'restored':not a.verify_only,'sha256_and_size':'PASS'}))
