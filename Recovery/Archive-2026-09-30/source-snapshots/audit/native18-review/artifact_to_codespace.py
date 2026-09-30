import subprocess,urllib.request,urllib.error,json,sys
gh='C:/Program Files/GitHub CLI/gh.exe'; artifact=int(sys.argv[1])
meta=json.loads(subprocess.check_output([gh,'api',f'repos/lyiming710-cloud/GodsPVZ-iOS/actions/artifacts/{artifact}']))
token=subprocess.check_output([gh,'auth','token'],text=True).strip()
class NoRedirect(urllib.request.HTTPRedirectHandler):
 def redirect_request(self,*a,**k):return None
request=urllib.request.Request(f'https://api.github.com/repos/lyiming710-cloud/GodsPVZ-iOS/actions/artifacts/{artifact}/zip',headers={'Authorization':'Bearer '+token,'User-Agent':'GodsPVZ-independent-recovery'})
try: urllib.request.build_opener(NoRedirect).open(request,timeout=60);raise RuntimeError('expected artifact redirect')
except urllib.error.HTTPError as e:
 if e.code!=302:raise RuntimeError('GitHub artifact API status '+str(e.code)) from None
 url=e.headers['Location']
del token
script='''import urllib.request,pathlib,hashlib,json,zipfile,subprocess
meta=META
p=pathlib.Path('/workspaces/GodsPVZ-native19/.validation/xcode-native18');p.mkdir(parents=True,exist_ok=True)
z=p/'artifact.zip'
with urllib.request.urlopen(URL,timeout=300) as r,z.open('wb') as f:
 while chunk:=r.read(1024*1024):f.write(chunk)
digest=hashlib.file_digest(z.open('rb'),'sha256').hexdigest()
assert 'sha256:'+digest==meta['digest'],(digest,meta['digest'])
(p/'artifact-meta.json').write_text(json.dumps(meta,indent=2))
with zipfile.ZipFile(z) as f:
 for n in f.namelist():assert not pathlib.PurePosixPath(n).is_absolute() and '..' not in pathlib.PurePosixPath(n).parts
 f.extractall(p)
print('ARTIFACT_VERIFIED',meta['id'],z.stat().st_size,digest,flush=True)
archive=next(p.glob('*.tar.zst'));dest=p/'expanded';dest.mkdir(exist_ok=True)
subprocess.run(['tar','--zstd','-xf',str(archive),'-C',str(dest)],check=True)
print('XCODE_HEADERS_READY',dest,flush=True)
'''.replace('META',repr(meta)).replace('URL',repr(url))
r=subprocess.run([gh,'codespace','ssh','-c','glowing-train-p7j9gp74q6jwc76v6','--','python3 -'],input=script.encode(),stdout=subprocess.PIPE,stderr=subprocess.PIPE)
print(r.stdout.decode(errors='replace'))
if r.returncode:print('Remote download/extraction failed, exit',r.returncode) # signed URL must not leak through traceback
sys.exit(r.returncode)
