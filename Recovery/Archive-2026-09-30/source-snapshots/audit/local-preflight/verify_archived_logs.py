from pathlib import Path
import subprocess,json,hashlib
root=Path(__file__).resolve().parents[1]/'native15-work'
git=Path.home()/'.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
for stage in (15,16):
 rel=f'Recovery/Native{stage}-2026-09-28/validation/'
 def blob(n):return subprocess.check_output([str(git),'-C',str(root),'show','HEAD:'+rel+n])
 manifest=json.loads(blob('SHA256.json'))
 for n,h in manifest.items():assert hashlib.sha256(blob(n)).hexdigest()==h,(stage,n)
 print(f'NATIVE{stage}_COMMITTED_ARCHIVE_HASH_PASS',len(manifest))
