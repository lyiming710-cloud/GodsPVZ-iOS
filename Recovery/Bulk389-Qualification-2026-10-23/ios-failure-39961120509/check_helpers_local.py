from pathlib import Path
import hashlib
import json
import subprocess
import sys

out = Path(__file__).resolve().parent
base = out.parents[1]
repo = base / 'audit/repo-native113'
sys.path.insert(0, str(repo / 'scripts/actions'))
from prepare_bulk389_unity import STUB
dst = out / 'helper-api-check-r2'
dst.mkdir(exist_ok=False)
support = repo / 'Recovery/Bulk389-Qualification-2026-10-23/ios-support/Assets/Editor'
refs = sorted((base / 'audit/local-preflight/exact-editor-tools/aot-resolver').glob('*.dll'))
refs += sorted((base / 'audit/local-preflight/exact-editor-tools/resolver').glob('*.dll'))
compiler = base / '.local/dotnet10/sdk/10.0.402/Roslyn/bincore/csc.dll'
dotnet = base / '.local/dotnet10/dotnet.exe'
assert len(refs) > 100
stub = dst / 'GodsPVZPackageReferenceMigrator.cs'
stub.write_text(STUB, 'utf-8')
results = []
for name, sources in [
    ('baseline', [support / 'GodsPVZIOSBuild.cs', support / 'GodsPVZPackageReferenceMigrator.cs']),
    ('candidate', [support / 'GodsPVZIOSBuild.cs', stub]),
    ('deleted_source_negative', [support / 'GodsPVZIOSBuild.cs', dst / 'missing/GodsPVZPackageReferenceMigrator.cs'])]:
    args = ['/noconfig', '/nostdlib', '/target:library', '/langversion:9.0', '/define:UNITY_EDITOR', '/preferreduilang:en-US', '/utf8output', '/out:"' + str(dst / (name + '.dll')) + '"']
    args += ['/r:"' + str(r) + '"' for r in refs]
    args += ['"' + str(s) + '"' for s in sources]
    rsp = dst / (name + '.rsp')
    rsp.write_text('\n'.join(args), 'utf-8')
    cmd = [str(dotnet), 'exec', str(compiler), '@' + str(rsp)]
    proc = subprocess.run(cmd, capture_output=True, text=True, encoding='utf-8', errors='replace')
    text = proc.stdout + proc.stderr
    (dst / (name + '.log')).write_text(text, 'utf-8')
    results.append(dict(name=name, exit=proc.returncode, cs2001='error CS2001:' in text))
result = dict(scope='LOCAL_ROSLYN_COMPILATION_AGAINST_PRESERVED_UNITY_AND_MONO_REFERENCES_ONLY',
              results=results, references=[dict(path=str(r.relative_to(base)), sha256=hashlib.sha256(r.read_bytes()).hexdigest()) for r in refs],
              editor_launched=False, compiler_runtime='LOCAL_DOTNET_10_NOT_LINUX_EDITOR_COMPILER')
(dst / 'RESULT.json').write_text(json.dumps(result, indent=2) + '\n', 'utf-8')
print(results)
assert results[0]['exit'] == results[1]['exit'] == 0
assert results[2]['exit'] != 0 and results[2]['cs2001']
