from pathlib import Path
import os,json,subprocess,hashlib,shutil,re
p=Path(__file__).parent;w=p.parent/'native23-publication';d=w/'Recovery/Archive-2026-09-30';git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe';gh='C:/Program Files/GitHub CLI/gh.exe'
env=dict(os.environ);env['PATH']='C:/Program Files/GitHub CLI'+os.pathsep+env.get('PATH','')
def run(a,**kw):return subprocess.run(a,check=True,env=env,**kw)
def out(a):return subprocess.check_output(a,text=True,env=env)
old='77fb194e59ea420b480620001a2d30b3d8b9adad';assert out([git,'-C',str(w),'rev-parse','HEAD']).strip()==old
remote=json.loads(out([gh,'api','repos/lyiming710-cloud/GodsPVZ-iOS/branches/repair%2Fcodex-native19-ipa']))['commit']['sha'];assert remote==old,remote
assert json.loads((p/'RELEASE-VERIFICATION.json').read_text())['server_size_digest_and_uploaded_state']=='PASS'
assert json.loads((p/'DOWNLOAD-VERIFICATION.json').read_text())['codespace_all_payload_verification']['sha256_and_size']=='PASS'
for name in ['RELEASE-VERIFICATION.json','DOWNLOAD-VERIFICATION.json']:
 shutil.copyfile(p/name,d/name)
for f in p.glob('*.py'):shutil.copyfile(f,d/'archive-tools'/f.name)
index=json.loads((d/'SOURCE-INDEX.json').read_text(encoding='utf-8'));assert len(index)==2494
for r in index:assert hashlib.sha256((d/r['path']).read_bytes()).hexdigest()==r['sha256'],r['path']
for link in re.findall(r'\]\(([^)]+)\)',(d/'README.md').read_text(encoding='utf-8')):
 if link.startswith('https:'):continue
 assert (d/link).exists(),link
checks={f.relative_to(d).as_posix():{'bytes':f.stat().st_size,'sha256':hashlib.sha256(f.read_bytes()).hexdigest()} for f in d.rglob('*') if f.is_file() and f.name!='INDEX-SHA256.json'}
(d/'INDEX-SHA256.json').write_text(json.dumps(checks,ensure_ascii=False,indent=2),encoding='utf-8')
paths=['README.md','Recovery/HANDOFF_CURRENT.md','Recovery/STATUS.md','Recovery/Archive-2026-09-30']
run([git,'-C',str(w),'add','--sparse','--']+paths)
staged=out([git,'-C',str(w),'diff','--cached','--name-only']).splitlines()
assert all(x in paths[:3] or x.startswith(paths[3]+'/') for x in staged),staged
# Imported snapshots retain original whitespace; only newly authored entry documents use diff whitespace checks.
run([git,'-C',str(w),'diff','--cached','--check','--']+paths[:3]+[paths[3]+'/README.md'])
tree_lines=out([git,'-C',str(w),'ls-files','--stage','--',paths[3]+'/source-snapshots']).splitlines()
tree={x.split('\t',1)[1]:x.split()[1] for x in tree_lines}
for r in index:
 b=(d/r['path']).read_bytes();raw=hashlib.sha1(('blob '+str(len(b))+'\0').encode()+b).hexdigest();assert tree[paths[3]+'/'+r['path']]==raw,r['path']
with (p/'archive-commit.log').open('w',encoding='utf-8') as log:
 run([git,'-C',str(w),'commit','-m','docs(recovery): classify and archive original inputs, historical source and validation evidence'],stdout=log,stderr=subprocess.STDOUT)
sha=out([git,'-C',str(w),'rev-parse','HEAD']).strip()
run([git,'-C',str(w),'-c','credential.helper=','-c','credential.helper=!gh auth git-credential','push','origin','HEAD:refs/heads/repair/codex-native19-ipa'])
actual=json.loads(out([gh,'api','repos/lyiming710-cloud/GodsPVZ-iOS/branches/repair%2Fcodex-native19-ipa']))['commit']['sha'];assert actual==sha
tree_remote=json.loads(out([gh,'api','repos/lyiming710-cloud/GodsPVZ-iOS/git/trees/'+sha+'?recursive=1']));assert not tree_remote.get('truncated')
published={x['path']:x for x in tree_remote['tree']}
for path in staged:assert path in published,path
for path,blob in tree.items():assert published[path]['sha']==blob,path
result={'commit':sha,'parent':old,'branch':'repair/codex-native19-ipa','files_published':len(staged),'source_snapshots_verified':len(index),'remote_tree_and_blob_verification':'PASS','source_files_changed':False,'workflow_dispatch':False}
(p/'ARCHIVE-PUBLICATION.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(json.dumps(result,indent=2))
