import os,urllib.request,urllib.error,zipfile,hashlib,json
from pathlib import Path
R=Path(__file__).resolve().parent
class NoRedirect(urllib.request.HTTPRedirectHandler):
 def redirect_request(self,*args,**kwargs):return None
def download(url,path,sha,auth=False):
 headers={'User-Agent':'GodsPVZ-native4-static-gate'}
 if auth:headers['Authorization']='Bearer '+os.environ['GH_TOKEN'];headers['Accept']='application/vnd.github+json'
 opener=urllib.request.build_opener(NoRedirect)
 try:r=opener.open(urllib.request.Request(url,headers=headers),timeout=90)
 except urllib.error.HTTPError as e:
  if e.code not in (301,302,303,307,308):raise
  r=urllib.request.urlopen(e.headers['Location'],timeout=90)
 with r:b=r.read()
 assert hashlib.sha256(b).hexdigest()==sha,path
 path.parent.mkdir(parents=True,exist_ok=True);path.write_bytes(b)
def artifact(id,name,sha):
 p=R/'inputs'/name;download(f'https://api.github.com/repos/lyiming710-cloud/GodsPVZ-iOS/actions/artifacts/{id}/zip',p,sha,True);return zipfile.ZipFile(p)
def extract_sha(z,sha,dest):
 hits=[z.read(n) for n in z.namelist() if n.endswith('.dll') and hashlib.sha256(z.read(n)).hexdigest()==sha];assert len(hits)==1;dest.write_bytes(hits[0])
if __name__=='__main__':
 z=artifact(10859981387,'baseline.zip','336d24ab200a56c6af1c3440cc09e249eceffa73485c2f77673babe79ae516e1');extract_sha(z,'18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd',R/'inputs/baseline-18e44.dll')
 z=artifact(10853467622,'59bb-lineage.zip','bf475675366e45da76ab89ee1bd258a362ebf391e1f136d535f1d2267f7717f6');extract_sha(z,'59bb8e0224787f369653bfdb91fadff0a2d09316e33060d04497b4cab5638c33',R/'inputs/59bb-historical.dll')
 z=artifact(10853191869,'resolver.zip','efb8ef74f996f2faac0e72d7cf4530dcd98783b1ac16c74cfeb3459b11184271');(R/'resolver').mkdir(exist_ok=True)
 for name,sha in {'mscorlib.dll':'4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be','UnityEngine.CoreModule.dll':'83192116b34abac2bb1797ec0e3943362be33b028b1eefd2e561d5fdc1c3312f'}.items():
  b=z.read('attempt-4/editor-managed-resolver/'+name);assert hashlib.sha256(b).hexdigest()==sha;(R/'resolver'/name).write_bytes(b)
 z=artifact(10896577104,'native3-prior.zip','c531edfb29e2348dfddb3df257cbee8a61417082758398980d32c25e0645b630');extract_sha(z,'72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc',R/'inputs/native3-prior.dll')
 module=R/'ReferenceAssemblies/UnityEngine.AnimationModule.dll'
 proof=json.loads((R/'native-evidence/animation-resolver-provenance.json').read_text())
 b=module.read_bytes();assert hashlib.sha256(b).hexdigest()==proof['module_sha256']
 (R/'resolver'/module.name).write_bytes(b)
 # NuGet integrity pinned to the package used locally.
 lock=json.loads((R/'input-locks.json').read_text())
 p=R/'inputs/mono.cecil.nupkg';download('https://api.nuget.org/v3-flatcontainer/mono.cecil/0.11.6/mono.cecil.0.11.6.nupkg',p,lock['cecil_nupkg_sha256'])
 with zipfile.ZipFile(p) as z:
  for name in ['lib/net40/Mono.Cecil.dll','lib/netstandard2.0/Mono.Cecil.dll']:
   target=R/'tools'/name;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(z.read(name))
 print('EXACT_NATIVE3_INPUTS_READY')
