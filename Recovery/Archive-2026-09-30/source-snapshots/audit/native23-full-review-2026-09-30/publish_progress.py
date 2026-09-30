from pathlib import Path
import subprocess,json,hashlib,os,re
review=Path(__file__).resolve().parent;work=review.parent/'native23-publication';dest=work/'Recovery/Native23-Review-2026-09-30'
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe';gh='C:/Program Files/GitHub CLI/gh.exe'
env=dict(os.environ);env['PATH']='C:/Program Files/GitHub CLI'+os.pathsep+env.get('PATH','')
def call(args,**kw):return subprocess.run(args,check=True,env=env,**kw)
manifest={str(f.relative_to(dest)).replace('\\','/'):{'bytes':f.stat().st_size,'sha256':hashlib.sha256(f.read_bytes()).hexdigest()} for f in sorted(dest.rglob('*')) if f.is_file() and f.name!='SHA256.json'}
(dest/'SHA256.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2),encoding='utf-8')
for rel,record in manifest.items():
 data=(dest/rel).read_bytes();assert len(data)==record['bytes'] and hashlib.sha256(data).hexdigest()==record['sha256']
 assert record['bytes']<100*1024*1024
assert manifest['full-review-evidence.zip']['sha256']=='b7565f28d68a91620d42f9b6d154d7943beefe9a5932ece290eadf93fe994aed'
for name in ('README.md','REVIEW.md'):
 pp=dest/name
 for link in re.findall(r'\]\(([^)]+)\)',pp.read_text(encoding='utf-8')):
  if link.startswith(('http://','https://')) or '#' in link:continue
  assert (pp.parent/link).exists(),(name,link)
parent=subprocess.check_output([git,'-C',str(work),'rev-parse','HEAD'],text=True).strip();assert parent=='f1a96517d21826f0de9efb579b481dee4a9863b0'
remote=json.loads(subprocess.check_output([gh,'api','repos/lyiming710-cloud/GodsPVZ-iOS/branches/repair%2Fcodex-native19-ipa'],text=True))['commit']['sha']
assert remote=='09dd564962f6b83b3175194db97d95cb7e5346b9',remote
paths=['README.md','Recovery/HANDOFF_CURRENT.md','Recovery/STATUS.md','Recovery/Native23-Review-2026-09-30']
call([git,'-C',str(work),'add','--sparse','--']+paths)
staged=subprocess.check_output([git,'-C',str(work),'diff','--cached','--name-only'],text=True).splitlines()
assert len(staged)==51,len(staged)
assert all(p in paths[:3] or p.startswith(paths[3]+'/') for p in staged)
call([git,'-C',str(work),'diff','--cached','--check'])
call([git,'-C',str(work),'commit','-m','docs(recovery): publish Native23 independent review and verified progress'])
sha=subprocess.check_output([git,'-C',str(work),'rev-parse','HEAD'],text=True).strip()
call([git,'-C',str(work),'-c','credential.helper=','-c','credential.helper=!gh auth git-credential','push','origin','HEAD:refs/heads/repair/codex-native19-ipa'])
actual=json.loads(subprocess.check_output([gh,'api','repos/lyiming710-cloud/GodsPVZ-iOS/branches/repair%2Fcodex-native19-ipa'],text=True))['commit']['sha'];assert actual==sha,actual
# Verify the published manifest and archive SHA from GitHub's Git database.
root=json.loads(subprocess.check_output([gh,'api',f'repos/lyiming710-cloud/GodsPVZ-iOS/git/trees/{sha}?recursive=1'],text=True));assert not root.get('truncated')
prefix='Recovery/Native23-Review-2026-09-30/';tree={x['path']:x for x in root['tree']}
for pp in staged:assert pp in tree,pp
archive=tree[prefix+'full-review-evidence.zip'];data=(dest/'full-review-evidence.zip').read_bytes()
blob=hashlib.sha1(('blob '+str(len(data))+'\0').encode()+data).hexdigest();assert blob==archive['sha']
checks=json.loads(subprocess.check_output([gh,'api','repos/lyiming710-cloud/GodsPVZ-iOS/contents/'+prefix+'SHA256.json?ref='+sha],text=True))
import base64
published=json.loads(base64.b64decode(checks['content']));assert published==manifest
result={'branch':'repair/codex-native19-ipa','commit':sha,'parent':parent,'prior_remote':remote,'published_files':len(staged),'archive_sha256':hashlib.sha256(data).hexdigest(),'archive_git_blob':blob,'manifest_readback_verified':True,'archive_git_blob_verified':True,'url':'https://github.com/lyiming710-cloud/GodsPVZ-iOS/commit/'+sha,'worktree':str(work)}
(review/'PUBLICATION.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(json.dumps(result,indent=2))
