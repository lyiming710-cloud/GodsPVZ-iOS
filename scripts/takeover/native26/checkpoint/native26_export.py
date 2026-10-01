from pathlib import Path
import subprocess,os,json
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');n=w/'.validation/native26-2026-10-01';r=w/'.validation/native25-2026-10-01/native24-restored'
resources={p.name:p.read_text() for p in [Path('/sys/fs/cgroup/memory.max'),Path('/sys/fs/cgroup/memory.current'),Path('/sys/fs/cgroup/memory.events')] if p.exists()};print('RESOURCES',json.dumps(resources),flush=True)
with (n/'typed-export-retry.log').open('w') as log:p=subprocess.run(['dotnet',str(old/'.validation/native22/bin/PatcherNative19.dll'),str(n/'native26-final1.dll'),str(n/'all-methods.json'),'export-all'],env=dict(os.environ,GODSPVZ_RESOLVER=str(r/'managed-support')),stdout=log,stderr=subprocess.STDOUT)
print(json.dumps({'exit':p.returncode,'output_bytes':(n/'all-methods.json').stat().st_size if (n/'all-methods.json').exists() else None,'log':(n/'typed-export-retry.log').read_text()[-3000:]},indent=2));assert p.returncode==0
