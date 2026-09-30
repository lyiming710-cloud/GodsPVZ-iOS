from pathlib import Path
import json,hashlib,re,subprocess
root=Path(__file__).parent/'native19-work';git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
paths=['.gitignore','Recovery/HANDOFF_CURRENT.md','Recovery/HANDOFF-2026-09-29-Native21.md','Recovery/HANDOFF-Native21-SHA256.json','Recovery/Historical-Handoff-2026-09-27.md','Recovery/TAKEOVER-PROMPT-Native21.md','Recovery/Native20-2026-09-29','Recovery/Native21-2026-09-29','scripts/takeover/PatcherNative20','scripts/takeover/PatcherNative21']
paths += ['scripts/codespaces/'+n for n in ['audit_integer_locals.py','native20_local.py','native21_local.py','qualify_native20_cpp.py','qualify_native21_cpp.py','test_native20_types.py','test_native21_types.py']]
subprocess.run([git,'-C',str(root),'add','--sparse',*paths],check=True,capture_output=True)
r=subprocess.run([git,'-C',str(root),'diff','--cached','--check'],capture_output=True);assert r.returncode==0,r.stdout.decode()[:3000]
count=0
for name in ['Recovery/Native19-2026-09-29/SHA256.json','Recovery/Native20-2026-09-29/SHA256.json','Recovery/Native21-2026-09-29/SHA256.json','Recovery/HANDOFF-Native21-SHA256.json']:
    for path,want in json.loads((root/name).read_text()).items():
        data=subprocess.check_output([git,'-C',str(root),'show',':'+path]);assert hashlib.sha256(data).hexdigest()==want,('staged hash mismatch',path);count+=1
names=subprocess.check_output([git,'-C',str(root),'diff','--cached','--name-only'],text=True).splitlines()
for name in names:
    p=root/name
    if p.suffix=='.dll':continue
    data=p.read_text(encoding='utf-8')
    assert not re.search(r'gh[pousr]_[A-Za-z0-9]{25,}|github_pat_[A-Za-z0-9_]{30,}|-----BEGIN (?:RSA |OPENSSH |EC )?PRIVATE KEY-----',data),('secret-like data',name)
for stage in [19,20,21]:
    suffix='cpp-qualification.json' if stage==19 else f'cpp-final{73 if stage==20 else 38}/qualification.json'
    q=json.loads((root/f'Recovery/Native{stage}-2026-09-29/validation'/suffix).read_text())
    assert sum(m['diagnostics'] for m in q['target_methods'])==0
print('HANDOFF_STAGED_CHECK_PASS',len(names),'changed files',count,'hash assertions')
