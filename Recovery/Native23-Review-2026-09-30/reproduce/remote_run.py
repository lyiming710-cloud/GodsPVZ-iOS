from pathlib import Path
import subprocess,sys
script=Path(sys.argv[1]);dest=Path(sys.argv[2]);dest.parent.mkdir(parents=True,exist_ok=True)
with dest.open('wb') as f:
    r=subprocess.run(['C:/Program Files/GitHub CLI/gh.exe','codespace','ssh','-c','glowing-train-p7j9gp74q6jwc76v6','--','python3 -'],input=script.read_bytes(),stdout=f,stderr=subprocess.STDOUT)
print('REMOTE_EXIT',r.returncode,'OUTPUT',dest,'BYTES',dest.stat().st_size)
if r.returncode:print(dest.read_text(encoding='utf-8',errors='replace')[-6000:])
raise SystemExit(r.returncode)
