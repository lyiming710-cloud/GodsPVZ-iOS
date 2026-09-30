#!/usr/bin/env python3
"""Restore pinned inputs and run the native14 diagnostic in any checkout."""
from pathlib import Path
import hashlib, json, os, shutil, subprocess, sys, zipfile

ROOT = Path(__file__).resolve().parents[2]
CACHE = ROOT / '.validation/native14'
CACHE.mkdir(parents=True, exist_ok=True)
(CACHE / 'latest-result.json').write_text(json.dumps({'status': 'RUNNING', 'full_unity_export': False, 'production_promotion': False}))
def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()
def run(args, **kwargs):
    subprocess.run(args, cwd=ROOT, check=True, timeout=600, **kwargs)
for tool in ('gh', 'dotnet', 'python3', 'bash'):
    if not shutil.which(tool):
        raise SystemExit('Required tool missing: ' + tool)
artifacts = [
    (10928998261, 'native13', '5643737ef1300dfe7e7d11228d5dba895fe8b30dfe3119783b8c3153124953b2'),
    (10924263662, 'seed', 'ec652fbedfbd75fd4616aa18986ee26e2791b60ff3dbd099ecf9d52b2342b0b0'),
    (10925548819, 'supplement', '38f488d1e77d7972745f049f1b2a5462c89719e3cbab67f12423c2aa1145887b'),
]
for ident, name, digest in artifacts:
    path = CACHE / (name + '.zip')
    if not path.exists():
        part = path.with_suffix('.partial')
        with part.open('wb') as stream:
            run(['gh', 'api', f'repos/lyiming710-cloud/GodsPVZ-iOS/actions/artifacts/{ident}/zip'], stdout=stream)
        if sha(part) != digest:
            raise SystemExit(name + ' downloaded archive hash mismatch')
        part.replace(path)
    if sha(path) != digest:
        raise SystemExit(name + ' cached archive hash mismatch')
    print(name + ' ARCHIVE_HASH_PASS', flush=True)
source = CACHE / 'input/candidate/Assembly-CSharp-native13-selectorb.dll'
source.parent.mkdir(parents=True, exist_ok=True)
with zipfile.ZipFile(CACHE / 'native13.zip') as archive:
    hits = [n for n in archive.namelist() if n.endswith('/Assembly-CSharp-native13-selectorb.dll')]
    if len(hits) != 1:
        raise SystemExit('Expected exactly one native13 candidate')
    payload = archive.read(hits[0])
    if hashlib.sha256(payload).hexdigest() != 'fbc42f136d28916d155b1876139c0b3b19994bdb4bf24aca7eacd8c7556a62b4':
        raise SystemExit('native13 candidate hash mismatch')
    source.write_bytes(payload)
recovery = ROOT / 'Tools/Stage9Native4Recovery'
package = recovery / 'inputs/mono.cecil.nupkg'
cecil = recovery / 'tools/lib/netstandard2.0/Mono.Cecil.dll'
corlib = recovery / 'resolver/mscorlib.dll'
lock = json.loads((recovery / 'input-locks.json').read_text())
def dependencies_valid():
    if not all(p.exists() for p in (package, cecil, corlib)):
        return False
    if sha(package) != lock['cecil_nupkg_sha256'] or sha(corlib) != '4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be':
        return False
    with zipfile.ZipFile(package) as archive:
        return cecil.read_bytes() == archive.read('lib/netstandard2.0/Mono.Cecil.dll')
if not dependencies_valid():
    env = dict(os.environ)
    if not env.get('GH_TOKEN'):
        env['GH_TOKEN'] = subprocess.check_output(['gh', 'auth', 'token'], text=True).strip()
    run([sys.executable, str(recovery / 'prepare_ci_inputs.py')], env=env)
    if not dependencies_valid():
        raise SystemExit('Pinned dependency validation failed')
with (CACHE / 'materialize.log').open('w') as log:
    run(['bash', 'scripts/takeover/materialize_native14.sh', str(source), str(CACHE / 'Assembly-CSharp-native14-elements.dll')], stdout=log, stderr=subprocess.STDOUT)
candidate = CACHE / 'Assembly-CSharp-native14-elements.dll'
if sha(candidate) != '281b7a20d3cc0719b0087be389f10a16e966a85c93bd5f43ef72fd54eaa01614':
    raise SystemExit('Unlinked native14 differs from the reviewed output')
run([sys.executable, 'scripts/codespaces/validate_native14.py'])
