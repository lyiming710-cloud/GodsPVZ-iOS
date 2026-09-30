"""Repository-scoped GitHub API using the already configured Git credential helper.
Never prints or writes credentials; redirect downloads omit Authorization.
"""
import sys,json,subprocess,urllib.request,urllib.error
from pathlib import Path
REPO='lyiming710-cloud/GodsPVZ-iOS'
def request(path,method='GET',data=None):
 p=subprocess.run(['git','credential','fill'],input='protocol=https\nhost=github.com\npath='+REPO+'.git\n\n',text=True,capture_output=True,check=True)
 c=dict(l.split('=',1) for l in p.stdout.splitlines() if '=' in l)
 token=c.get('password');assert token,'credential unavailable'
 headers={'Authorization':'Bearer '+token,'Accept':'application/vnd.github+json','X-GitHub-Api-Version':'2022-11-28','User-Agent':'Codex-GodsPVZ-recovery'}
 b=None if data is None else json.dumps(data).encode()
 if b is not None:headers['Content-Type']='application/json'
 req=urllib.request.Request('https://api.github.com/repos/'+REPO+'/'+path,method=method,data=b,headers=headers)
 with urllib.request.urlopen(req,timeout=45) as r:
  b=r.read();return json.loads(b) if b else {'status':r.status}
if __name__=='__main__':
 mode=sys.argv[1]
 if mode=='get':
  d=request(sys.argv[2]);Path(sys.argv[3]).write_text(json.dumps(d,indent=2));print('saved',sys.argv[3])
 elif mode=='dispatch':print(request('actions/workflows/'+sys.argv[2]+'/dispatches','POST',{'ref':sys.argv[3],'inputs':json.loads(sys.argv[4]) if len(sys.argv)>4 else {}}))
 else:raise SystemExit('unsupported mode')
