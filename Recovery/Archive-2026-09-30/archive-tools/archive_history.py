from pathlib import Path
import os,subprocess,json
p=Path(__file__).parent;w=p.parent/'native23-publication';git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
env=dict(os.environ);env['PATH']='C:/Program Files/GitHub CLI'+os.pathsep+env.get('PATH','')
base=[git,'-C',str(w),'-c','credential.helper=','-c','credential.helper=!gh auth git-credential']
refs=subprocess.check_output(base+['for-each-ref','--format=%(refname) %(objectname)'],text=True,env=env)
(p/'GIT-REFS.txt').write_text(refs,encoding='utf-8')
objects=subprocess.check_output(base+['rev-list','--objects','--all','--missing=print'],text=True,env=env)
(p/'GIT-OBJECTS.txt').write_text(objects,encoding='utf-8')
missing=[x[1:] for x in objects.splitlines() if x.startswith('?')]
(p/'HISTORY-SCOPE.json').write_text(json.dumps({'references':refs.splitlines(),'missing_promisor_objects':len(missing),'bundle_base':'09dd564962f6b83b3175194db97d95cb7e5346b9','bundle_tips':['refs/heads/docs/native23-review-2026-09-30','refs/heads/repair/codex-native22-ipa'],'note':'Complete incremental bundle of all locally preserved Native22/Native23/review commits after 09dd564, with 09dd564 prerequisite. Earlier commits remain in the GitHub origin; original local stage snapshots are preserved in the file archives. This bundle is not a standalone full-history clone.'},indent=2),encoding='utf-8')
with (p/'history-build.log').open('w',encoding='utf-8') as log:
 r=subprocess.run(base+['bundle','create',str(p/'project-history.bundle'),'refs/heads/docs/native23-review-2026-09-30','refs/heads/repair/codex-native22-ipa','^09dd564962f6b83b3175194db97d95cb7e5346b9'],stdout=log,stderr=subprocess.STDOUT,env=env)
print('BUNDLE_EXIT',r.returncode,flush=True)
if r.returncode:print((p/'history-build.log').read_text(encoding='utf-8')[-4000:]);raise SystemExit(r.returncode)
print('BUNDLE_BYTES',(p/'project-history.bundle').stat().st_size)
r=subprocess.run(base+['bundle','verify',str(p/'project-history.bundle')],capture_output=True,text=True,env=env)
(p/'history-verify.txt').write_text(r.stdout+r.stderr,encoding='utf-8');print(r.stdout+r.stderr);raise SystemExit(r.returncode)
