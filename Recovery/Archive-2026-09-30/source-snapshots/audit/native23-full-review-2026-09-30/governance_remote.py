from pathlib import Path
import subprocess,json
repo=Path('/workspaces/GodsPVZ-native19');p=repo/'.validation/native23-independent-full-review'
res={}
for key,args in [('changes',['git','diff','--name-status','09dd564962f6b83b3175194db97d95cb7e5346b9..HEAD']),('ancestor',['git','merge-base','09dd564962f6b83b3175194db97d95cb7e5346b9','HEAD']),('log',['git','log','--format=%H %P | %an | %s','-3']),('workflow_changes',['git','diff','--name-only','09dd564962f6b83b3175194db97d95cb7e5346b9..HEAD','--','.github/workflows']),('status',['git','status','--porcelain'])]:
 r=subprocess.run(args,cwd=repo,capture_output=True,text=True);res[key]={'exit':r.returncode,'output':r.stdout}
(p/'governance.json').write_text(json.dumps(res,indent=2));print(json.dumps(res,indent=2))
