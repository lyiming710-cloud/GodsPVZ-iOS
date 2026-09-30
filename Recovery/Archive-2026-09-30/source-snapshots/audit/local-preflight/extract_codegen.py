from pathlib import Path
import io,lzma,tarfile,json,hashlib
R=Path(__file__).resolve().parent;cache=R.parent/'stage9-native4/inputs/editor-parts'
paths=sorted(cache.glob('Unity-China-2022.3.44f1c1.tar.xz.part-*'));assert len(paths)==15
class Parts(io.RawIOBase):
 def __init__(self):self.i=0;self.f=paths[0].open('rb')
 def readable(self):return True
 def readinto(self,b):
  n=self.f.readinto(b)
  if n:return n
  self.f.close();self.i+=1
  if self.i==len(paths):return 0
  self.f=paths[self.i].open('rb');return self.readinto(b)
out=R/'exact-editor-tools';out.mkdir(exist_ok=True);records=[]
with lzma.LZMAFile(io.BufferedReader(Parts()),'rb') as xz,tarfile.open(fileobj=xz,mode='r|') as tar:
 for m in tar:
  n=m.name.lstrip('./')
  if not m.isfile():continue
  relative=None
  if n.startswith('Editor/Data/il2cpp/build/deploy/'):
   relative='codegen/'+n[len('Editor/Data/il2cpp/build/deploy/'):]
  elif n.startswith('Editor/Data/Managed/UnityEngine/') and n.endswith('.dll'):
   relative='resolver/'+Path(n).name
  elif n.startswith('Editor/Data/MonoBleedingEdge/lib/mono/unityaot-macos/') and n.endswith('.dll'):
   relative='aot-resolver/'+Path(n).name
  if relative is None:continue
  p=out/relative;p.parent.mkdir(parents=True,exist_ok=True);b=tar.extractfile(m).read();p.write_bytes(b)
  records.append({'path':n,'output':relative,'size':len(b),'sha256':hashlib.sha256(b).hexdigest()})
(out/'extraction-manifest.json').write_text(json.dumps(records,indent=2),encoding='utf-8')
print('EXTRACTED',len(records),'files',sum(r['size'] for r in records),'bytes')
