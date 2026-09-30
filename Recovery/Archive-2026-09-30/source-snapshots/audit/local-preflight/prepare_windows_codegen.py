"""Create an isolated Windows-hosted copy of the exact managed IL2CPP tool.
Original extracted binaries are immutable. Use official .NET 6.0.18 Windows
runtime with SHA512 from Microsoft's release metadata, not Linux native hosts.
"""
from pathlib import Path
import json, urllib.request, hashlib, zipfile, shutil
R=Path(__file__).resolve().parent
metadata_url='https://builds.dotnet.microsoft.com/dotnet/release-metadata/6.0/releases.json'
meta=json.load(urllib.request.urlopen(metadata_url))
release=next(r for r in meta['releases'] if r.get('runtime',{}).get('version')=='6.0.18')
f=next(f for f in release['runtime']['files'] if f['rid']=='win-x64' and f['name'].endswith('.zip'))
cache=R/'dotnet-runtime-6.0.18-win-x64.zip'
if not cache.exists():
 print('Downloading',f['url'],flush=True)
 urllib.request.urlretrieve(f['url'],cache)
assert hashlib.sha512(cache.read_bytes()).hexdigest().lower()==f['hash'].lower()
runtime=R/'dotnet-runtime';runtime.mkdir(exist_ok=True)
with zipfile.ZipFile(cache) as z:z.extractall(runtime)
source=R/'exact-editor-tools/codegen';dest=R/'windows-codegen';dest.mkdir(exist_ok=True)
# Exclude platform-specific .NET assemblies and deps from the Linux deployment.
# Non-framework tool assemblies load beside il2cpp.dll; framework comes from dotnet.
for p in source.glob('*.dll'):
 if p.name.startswith(('System.','Microsoft.')) or p.name in ('mscorlib.dll','netstandard.dll','WindowsBase.dll'):continue
 shutil.copy2(p,dest/p.name)
config=json.loads((source/'il2cpp.runtimeconfig.json').read_text())
options=config['runtimeOptions'];options.pop('includedFrameworks')
options['framework']={'name':'Microsoft.NETCore.App','version':'6.0.18'}
(dest/'il2cpp.runtimeconfig.json').write_text(json.dumps(config,indent=2))
proof={'source_archive_sha256':'0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14','runtime_metadata_url':metadata_url,'runtime_file':f,'il2cpp_sha256':hashlib.sha256((source/'il2cpp.dll').read_bytes()).hexdigest(),'limit':'Experimental Windows host of exact managed tool; not a Unity Editor or full iOS build.'}
(R/'windows-codegen-provenance.json').write_text(json.dumps(proof,indent=2))
print('PREPARED',dest,flush=True)
