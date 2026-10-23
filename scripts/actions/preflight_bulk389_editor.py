"""Compile the full helper set and post-migration helper set before seat acquisition.

Uses this exact restored Editor's own Roslyn, runtime and .NET 4.8 references;
does not start Unity or acquire a license. This is API/compilation validation,
not an Editor import or game runtime test.
"""
from pathlib import Path
import argparse
import hashlib
import json
import subprocess
from prepare_bulk389_unity import STUB


def main():
    p = argparse.ArgumentParser()
    p.add_argument('--editor', required=True)
    p.add_argument('--project', required=True)
    p.add_argument('--output', required=True)
    a = p.parse_args()
    data = Path(a.editor).resolve().parent / 'Data'
    root = Path(a.project).resolve()
    out = Path(a.output).resolve()
    out.mkdir(parents=True, exist_ok=False)
    runtime = data / 'NetCoreRuntime/dotnet'
    compiler = data / 'DotNetSdkRoslyn/csc.dll'
    api = data / 'UnityReferenceAssemblies/unity-4.8-api'
    references = sorted(api.glob('*.dll')) + sorted((api / 'Facades').glob('*.dll'))
    references += sorted((data / 'Managed').rglob('Unity*.dll'))
    if not runtime.is_file() or not compiler.is_file() or len(references) < 50:
        raise ValueError('Exact Editor compiler/reference closure missing')
    sources = sorted((root / 'Assets/Editor').rglob('*.cs'))
    if {s.name for s in sources} != {'GodsPVZIOSBuild.cs', 'GodsPVZPackageReferenceMigrator.cs'}:
        raise ValueError('Unexpected Editor helper source inventory')
    stub = out / 'GodsPVZPackageReferenceMigrator.cs'
    stub.write_text(STUB, 'utf-8')
    rows = []
    for name, selected in [('baseline', sources), ('candidate', [s for s in sources if s.name != stub.name] + [stub])]:
        rsp = out / (name + '.rsp')
        arguments = ['/noconfig', '/nostdlib', '/target:library', '/langversion:9.0', '/define:UNITY_EDITOR', '/preferreduilang:en-US', '/utf8output',
                     '/out:"' + str(out / (name + '.dll')) + '"']
        arguments += ['/reference:"' + str(ref) + '"' for ref in references]
        arguments += ['"' + str(src) + '"' for src in selected]
        rsp.write_text('\n'.join(arguments) + '\n', 'utf-8')
        proc = subprocess.run([str(runtime), 'exec', str(compiler), '@' + str(rsp)], capture_output=True, text=True, encoding='utf-8', errors='replace')
        (out / (name + '.log')).write_text(proc.stdout + proc.stderr, 'utf-8')
        rows.append(dict(stage=name, exit_code=proc.returncode,
                         sources=[dict(path=str(s), sha256=hashlib.sha256(s.read_bytes()).hexdigest()) for s in selected]))
    result = dict(scope='EXACT_EDITOR_HELPER_API_COMPILATION_ONLY', reference_count=len(references), results=rows,
                  unity_started=False, game_runtime_tested=False)
    (out / 'HELPER-COMPILATION.json').write_text(json.dumps(result, indent=2) + '\n', 'utf-8')
    if any(r['exit_code'] for r in rows):
        raise SystemExit('Editor helper preflight failed; see compilation logs')
    print('EXACT_EDITOR_HELPERS_COMPILE_PASS')


if __name__ == '__main__':
    main()
