from pathlib import Path
import urllib.request, json, zipfile
root=Path(__file__).resolve().parent
requests={
 'repo-tree.json':'https://api.github.com/repos/lyiming710-cloud/GodsPVZ-iOS/git/trees/e40bdb0bc54dd5fc6e48d919387376f3e62e4c58?recursive=1',
 'mono.cecil.0.11.6.nupkg':'https://api.nuget.org/v3-flatcontainer/mono.cecil/0.11.6/mono.cecil.0.11.6.nupkg',
 'Il2CppMetadata.cs':'https://raw.githubusercontent.com/Perfare/Il2CppDumper/master/Il2CppDumper/Il2Cpp/MetadataClass.cs',
}
for name,url in requests.items():
 try:
  with urllib.request.urlopen(urllib.request.Request(url,headers={'User-Agent':'Codex-local-audit'}),timeout=25) as r: b=r.read()
  (root/'sources'/name).write_bytes(b)
  print(name,len(b))
  if name.endswith('.nupkg'):
   with zipfile.ZipFile(root/'sources'/name) as z:
    for p in ['lib/net40/Mono.Cecil.dll','lib/netstandard2.0/Mono.Cecil.dll']:
     dst=root/'tools'/p;dst.parent.mkdir(parents=True,exist_ok=True);dst.write_bytes(z.read(p))
 except Exception as e: print(name,type(e).__name__,str(e))
