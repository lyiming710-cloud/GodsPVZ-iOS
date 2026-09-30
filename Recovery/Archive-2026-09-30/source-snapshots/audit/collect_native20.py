from pathlib import Path
import subprocess,base64,json
destination=Path(__file__).parent/'native20-evidence';destination.mkdir(exist_ok=True)
remote=r'''
from pathlib import Path
import json,base64
root=Path('/workspaces/GodsPVZ-native19/.validation/native20')
patterns=['replay/candidate.json','replay/*.log','replay/native20-*.dll.targets*','replay/native20-*.dll.edits.json','replay/native20-*.dll.before.json','replay/native20-*.dll','replay/all-methods.integer-audit.json','cpp-final73/results.json','cpp-final73/method-errors.json','cpp-final73/invocation.json','cpp-final73/qualification.json']
files={}
for pattern in patterns:
 for p in root.glob(pattern):
  if p.is_file():files[p.relative_to(root).as_posix()]=base64.b64encode(p.read_bytes()).decode()
print(json.dumps(files))
'''
r=subprocess.run(['C:/Program Files/GitHub CLI/gh.exe','codespace','ssh','-c','glowing-train-p7j9gp74q6jwc76v6','--','python3 -'],input=remote.encode(),stdout=subprocess.PIPE,check=True)
files=json.loads(r.stdout)
for name,data in files.items():
 p=(destination/name).resolve();assert p.is_relative_to(destination.resolve());p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(base64.b64decode(data))
print('COLLECTED',len(files),'files',destination)
