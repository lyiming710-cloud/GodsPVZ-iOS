from pathlib import Path
import subprocess,hashlib,zipfile,json
p=Path('/tmp/godspvz-native72-restored-2026-10-12')
p.mkdir(exist_ok=True)
z=p/'godspvz-native72-pending-2026-10-12.zip'
if not z.exists():
    subprocess.run(['gh','release','download','native72-pending-2026-10-12','--repo','lyiming710-cloud/GodsPVZ-iOS','--pattern',z.name,'--dir',str(p)],check=True)
h=hashlib.sha256(z.read_bytes()).hexdigest()
if h!='76831fae19bfc91c1ae5b254034920cf5e9283416eb3d045d4132f8b8f23b39e':raise RuntimeError('Pinned parent archive mismatch')
with zipfile.ZipFile(z)as a:
    print('members',a.namelist())
    a.extractall(p/'snapshot')
for f in p.rglob('candidate1.dll'):
    print('candidate',f,len(f.read_bytes()),hashlib.sha256(f.read_bytes()).hexdigest())
