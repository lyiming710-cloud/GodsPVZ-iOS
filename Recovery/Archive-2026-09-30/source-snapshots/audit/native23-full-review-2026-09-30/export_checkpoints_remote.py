from pathlib import Path
import subprocess,json,hashlib,base64,sys
repo=Path('/workspaces/GodsPVZ-native19');out=repo/'.validation/native23-independent-full-review/native23-checkpoints.bundle'
head=subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip()
status=subprocess.check_output(['git','status','--porcelain'],cwd=repo,text=True)
assert head=='f1a96517d21826f0de9efb579b481dee4a9863b0',head
assert status=='',status
subprocess.run(['git','bundle','create',str(out),'09dd564962f6b83b3175194db97d95cb7e5346b9..HEAD'],cwd=repo,check=True,stdout=subprocess.DEVNULL)
raw=out.read_bytes();print(json.dumps({'head':head,'bytes':len(raw),'sha256':hashlib.sha256(raw).hexdigest()}),flush=True)
sys.stdout.buffer.write(base64.b64encode(raw))
