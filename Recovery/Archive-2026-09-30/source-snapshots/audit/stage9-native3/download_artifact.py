import hashlib,subprocess,urllib.request,urllib.error,sys
from pathlib import Path
class NoRedirect(urllib.request.HTTPRedirectHandler):
 def redirect_request(self,*args,**kwargs): return None
def download(artifact,dest,expected=None):
 p=subprocess.run(['git','credential','fill'],input='protocol=https\nhost=github.com\npath=lyiming710-cloud/GodsPVZ-iOS.git\n\n',text=True,capture_output=True,check=True)
 token=dict(l.split('=',1) for l in p.stdout.splitlines() if '=' in l)['password']
 req=urllib.request.Request(f'https://api.github.com/repos/lyiming710-cloud/GodsPVZ-iOS/actions/artifacts/{artifact}/zip',headers={'Authorization':'Bearer '+token,'Accept':'application/vnd.github+json','User-Agent':'Codex-recovery'})
 try:r=urllib.request.build_opener(NoRedirect).open(req,timeout=90)
 except urllib.error.HTTPError as e:
  if e.code!=302:raise
  r=urllib.request.urlopen(e.headers['Location'],timeout=90)
 with r: b=r.read()
 sha=hashlib.sha256(b).hexdigest()
 if expected:assert sha==expected,(sha,expected)
 Path(dest).write_bytes(b);print('saved',dest,'sha256',sha,'bytes',len(b))
if __name__=='__main__':download(*sys.argv[1:])
