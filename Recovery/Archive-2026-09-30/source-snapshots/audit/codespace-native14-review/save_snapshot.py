from pathlib import Path
import subprocess, tarfile, io, hashlib, json
root = Path(__file__).resolve().parent
repo = root.parent/'stage9-native3/repository'
git = 'C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
commit = '6b73e710bb6495fb7fbbfbea43c90375c0aea730'
data = subprocess.check_output([git, '-c', 'core.autocrlf=false', 'archive', commit, '.gitignore', 'scripts/codespaces', 'scripts/takeover', 'Recovery/Codespace-Native14-2026-09-28'], cwd=repo)
dest = root/'repository'
dest.mkdir(exist_ok=True)
with tarfile.open(fileobj=io.BytesIO(data)) as archive:
    archive.extractall(dest, filter='data')
evidence = dest/'Recovery/Codespace-Native14-2026-09-28'
manifest = json.loads((evidence/'SHA256.json').read_text())
for name, digest in manifest.items():
    assert hashlib.sha256((evidence/name).read_bytes()).hexdigest() == digest, name
print('LOCAL_SNAPSHOT_AND_EVIDENCE_HASHES_PASS', commit, len(manifest))
