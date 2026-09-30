"""Extract exact Editor AnimationModule from the preserved, hash-verified split archive."""
from pathlib import Path
import sys,json,hashlib,zipfile,subprocess,urllib.request,urllib.error,concurrent.futures,lzma,tarfile,io
R=Path(__file__).resolve().parent
sys.path.insert(0,str(R.parent/'stage9-native3'))
from download_artifact import download
ART=json.loads((R/'evidence/editor-artifacts.json').read_text())['artifacts']
parts=sorted((a for a in ART if a['name'].startswith('Stage9.1-unity-china-editor-part-')),key=lambda a:a['name'])
assert len(parts)==15
cache=R/'inputs/editor-parts';cache.mkdir(exist_ok=True)
with zipfile.ZipFile(R.parent/'stage9-native3/inputs/resolver.zip') as z:
 report=json.loads(z.read('attempt-4/editor-managed-resolver/extraction-report.json'))
locks={x['name']:x for x in report['parts']}
def fetch(a):
 idx=a['name'][-2:];name='Unity-China-2022.3.44f1c1.tar.xz.part-'+idx;p=cache/name
 if p.exists() and hashlib.sha256(p.read_bytes()).hexdigest()==locks[name]['sha256']:return p
 archive=cache/(idx+'.zip');download(str(a['id']),str(archive),a['digest'].split(':')[1])
 with zipfile.ZipFile(archive) as z:
  names=[n for n in z.namelist() if n.endswith(name)];assert len(names)==1
  b=z.read(names[0]);assert len(b)==locks[name]['size'] and hashlib.sha256(b).hexdigest()==locks[name]['sha256'];p.write_bytes(b)
 archive.unlink();return p
with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool: paths=list(pool.map(fetch,parts))
class Parts(io.RawIOBase):
 def __init__(self):self.i=0;self.f=paths[0].open('rb')
 def readable(self):return True
 def readinto(self,b):
  n=self.f.readinto(b)
  if n:return n
  self.f.close();self.i+=1
  if self.i==len(paths):return 0
  self.f=paths[self.i].open('rb');return self.readinto(b)
h=hashlib.sha256()
for p in paths:
 with p.open('rb') as f:
  for block in iter(lambda:f.read(1024*1024),b''):h.update(block)
assert h.hexdigest()=='0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14'
with lzma.LZMAFile(io.BufferedReader(Parts()),'rb') as xz,tarfile.open(fileobj=xz,mode='r|') as tar:
 for member in tar:
  if member.name.lstrip('./')=='Editor/Data/Managed/UnityEngine/UnityEngine.AnimationModule.dll':
   b=tar.extractfile(member).read();out=R/'resolver/UnityEngine.AnimationModule.dll';out.write_bytes(b)
   proof={'archive_sha256':h.hexdigest(),'archive_path':member.name,'module_sha256':hashlib.sha256(b).hexdigest(),'module_size':len(b),'source_run':34668863583,'parts':[{'id':a['id'],'digest':a['digest'],'name':a['name']} for a in parts]}
   (R/'evidence/animation-resolver-provenance.json').write_text(json.dumps(proof,indent=2));print('EXACT_ANIMATION_RESOLVER',proof['module_sha256'],len(b));break
 else:raise Exception('Exact Editor module not found')
