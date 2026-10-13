from pathlib import Path
import subprocess,hashlib,zipfile
p=Path('/tmp/godspvz-native77-restored-2026-10-13');p.mkdir(exist_ok=True)
z=p/'godspvz-native73-77-pending-2026-10-12.zip'
if not z.exists():subprocess.run(['gh','release','download','native73-77-pending-2026-10-12','--repo','lyiming710-cloud/GodsPVZ-iOS','--pattern',z.name,'--dir',str(p)],check=True)
if hashlib.sha256(z.read_bytes()).hexdigest()!='7efd9c78c6eca2f7cc9eedd71dae59fd60dc84c57e3fbbca353c3dd58ca10be7':raise RuntimeError('Pinned archive mismatch')
with zipfile.ZipFile(z)as a:a.extractall(p/'snapshot')
f=p/'snapshot/candidates/native77/candidate1.dll'
print('parent',f,f.stat().st_size,hashlib.sha256(f.read_bytes()).hexdigest())
