from pathlib import Path
import subprocess,json,hashlib
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native28-2026-10-01';repo='lyiming710-cloud/GodsPVZ-iOS'
def run(args,cwd=w):
 r=subprocess.run(args,cwd=cwd,capture_output=True,text=True);assert r.returncode==0,r.stdout+r.stderr;return r.stdout.strip()
r=json.loads((n/'PUBLICATION-RESULT.json').read_text());head=r['head'];assert run(['git','rev-parse','HEAD'])==head and not run(['git','status','--porcelain'])
old=Path('/workspaces/GodsPVZ-native19');assert run(['git','rev-parse','HEAD'],old)=='f1a96517d21826f0de9efb579b481dee4a9863b0' and not run(['git','status','--porcelain'],old)
tree=json.loads(run(['gh','api','repos/'+repo+'/git/trees/'+head+'?recursive=1']));assert not tree['truncated'];entries={x['path']:x for x in tree['tree']};names=run(['git','diff-tree','--no-commit-id','--name-only','-r',head]).splitlines()
for name in names:assert entries[name]['sha']==run(['git','hash-object',name]),name
assert json.loads(run(['gh','api','repos/'+repo+'/git/ref/heads/repair/codex-native24-codespace']))['object']['sha']==head
assert json.loads(run(['gh','api','repos/'+repo+'/git/ref/heads/main']))['object']['sha']==r['main_before']
release=json.loads(run(['gh','api','repos/'+repo+'/releases/tags/'+r['archive']['tag']]));assert not release['draft'] and release['prerelease'] and release['target_commitish']==head;asset=next(x for x in release['assets'] if x['name']==r['archive']['file']);assert asset['size']==r['archive']['bytes'] and asset['digest']=='sha256:'+r['archive']['sha256']
runs=json.loads(run(['gh','api','repos/'+repo+'/actions/runs?head_sha='+head+'&per_page=100']));assert not runs['workflow_runs'];r.update({'all_changed_git_blobs_readback_verified':len(names),'remote_branch_verified':True,'published_release_verified':True,'new_and_old_worktrees_clean':True,'main_unchanged':True,'new_commit_workflow_runs':len(runs['workflow_runs'])});(n/'PUBLICATION-RESULT.json').write_text(json.dumps(r,indent=2));print(json.dumps(r,indent=2))
