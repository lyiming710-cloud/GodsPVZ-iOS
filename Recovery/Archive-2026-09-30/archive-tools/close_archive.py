from pathlib import Path
import subprocess,os,json,hashlib,shutil
p=Path(__file__).parent;w=p.parent/'native23-publication';d=w/'Recovery/Archive-2026-09-30';gh='C:/Program Files/GitHub CLI/gh.exe';git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe';env=dict(os.environ);env['PATH']='C:/Program Files/GitHub CLI'+os.pathsep+env.get('PATH','')
def out(a):return subprocess.check_output(a,text=True,env=env)
result=json.loads((p/'FINAL-ARCHIVE-RESULT.json').read_text());parent=result['tag_commit'];assert out([git,'-C',str(w),'rev-parse','HEAD']).strip()==parent
for n in ['FINAL-ARCHIVE-RESULT.json','ARCHIVE-PUBLICATION.json','FINAL-RELEASE-NOTES.md']:shutil.copyfile(p/n,d/n)
for f in p.glob('*.py'):shutil.copyfile(f,d/'archive-tools'/f.name)
readme=d/'README.md';body=readme.read_text(encoding='utf-8');note='\n> **发布完成**：归档于 '+result['published_at']+' 正式发布，13 个资产（1,220,069,545 字节）全部核对通过；tag 固定在 `'+parent+'`。完整发布结果见 [FINAL-ARCHIVE-RESULT.json](FINAL-ARCHIVE-RESULT.json)。`RELEASE-VERIFICATION.json` 是发布前草稿状态的验收快照，最终状态以上述发布记录为准。\n'
readme.write_text(body.split('\n',1)[0]+note+'\n'+body.split('\n',1)[1],encoding='utf-8')
checks={f.relative_to(d).as_posix():{'bytes':f.stat().st_size,'sha256':hashlib.sha256(f.read_bytes()).hexdigest()} for f in d.rglob('*') if f.is_file() and f.name!='INDEX-SHA256.json'}
(d/'INDEX-SHA256.json').write_text(json.dumps(checks,ensure_ascii=False,indent=2),encoding='utf-8')
subprocess.run([git,'-C',str(w),'add','--sparse','--','Recovery/Archive-2026-09-30'],check=True,env=env,capture_output=True)
with (p/'archive-close-commit.log').open('w',encoding='utf-8') as log:subprocess.run([git,'-C',str(w),'commit','-m','docs(recovery): record published archive and final integrity checks'],check=True,env=env,stdout=log,stderr=subprocess.STDOUT)
sha=out([git,'-C',str(w),'rev-parse','HEAD']).strip();subprocess.run([git,'-C',str(w),'-c','credential.helper=','-c','credential.helper=!gh auth git-credential','push','origin','HEAD:refs/heads/repair/codex-native19-ipa'],check=True,env=env)
remote=json.loads(out([gh,'api','repos/lyiming710-cloud/GodsPVZ-iOS/branches/repair%2Fcodex-native19-ipa']))['commit']['sha'];assert sha==remote;assert not out([git,'-C',str(w),'status','--porcelain']).strip()
result['latest_progress_commit']=sha;result['publication_worktree_clean']=True;(p/'COMPLETE.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(json.dumps(result,indent=2))
