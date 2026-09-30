from pathlib import Path
import json, hashlib, subprocess
root=Path(__file__).parent/'native19-work'
evidence=Path(__file__).parent/'native19-evidence'
archive=root/'Recovery/Native19-2026-09-29'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
fixture=json.loads((evidence/'fixture/result.json').read_text())
for name,value in fixture['source_sha256'].items():
    assert sha(root/name)==value,(name,sha(root/name),value)
for source,dest in [('fixture/result.json','clr-result.json'),('fixture/fixture.log','clr-fixture.log')]:
    (archive/'validation'/dest).write_bytes((evidence/source).read_bytes().replace(b'\r\n',b'\n'))
manifest=archive/'SHA256.json'
old=json.loads(manifest.read_text())
paths={root/name for name in old}
paths.update(p for p in archive.rglob('*') if p.is_file() and p!=manifest)
result={p.relative_to(root).as_posix():sha(p) for p in sorted(paths)}
manifest.write_bytes((json.dumps(result,indent=2)+'\n').encode())
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
subprocess.run([git,'-C',str(root),'add','--sparse','Recovery/Native19-2026-09-29','scripts/takeover/PatcherNative19'],check=True,stdout=subprocess.DEVNULL,stderr=subprocess.PIPE)
r=subprocess.run([git,'-C',str(root),'diff','--cached','--check'],capture_output=True)
assert r.returncode==0,r.stdout.decode()[:3000]
for name,value in result.items():
    staged=subprocess.check_output([git,'-C',str(root),'show',':'+name])
    assert hashlib.sha256(staged).hexdigest()==value,('staged mismatch',name)
print('ARCHIVE_AND_STAGED_HASHES_PASS',len(result))
