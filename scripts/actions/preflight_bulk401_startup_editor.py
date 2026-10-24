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


def select_references(data):
    """Use Unity's emitted modular API set, never recursively glob legacy DLLs."""
    roster_path = Path(__file__).with_name('bulk389_editor_reference_roster.json')
    roster = json.loads(roster_path.read_text('utf-8'))
    names = roster['references']
    if len(names) != 210 or len(set(names)) != len(names) or len({Path(n).name for n in names}) != len(names):
        raise ValueError('Invalid or duplicate Editor reference roster')
    if 'Managed/UnityEngine.dll' in names or 'Managed/UnityEditor.dll' in names:
        raise ValueError('Legacy monolithic Editor references are forbidden')
    references = [data / n for n in names]
    if any(not r.is_file() or not r.resolve().is_relative_to(data.resolve()) for r in references):
        raise ValueError('Exact Editor reference roster incomplete')
    return references


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
    references = select_references(data)
    if not runtime.is_file() or not compiler.is_file() or len(references) < 50:
        raise ValueError('Exact Editor compiler/reference closure missing')
    sources = sorted((root / 'Assets/Editor').rglob('*.cs'))
    if {s.name for s in sources} != {'GodsPVZIOSBuild.cs', 'GodsPVZPackageReferenceMigrator.cs', 'GodsPVZStartupAudit.cs'}:
        raise ValueError('Unexpected Editor helper source inventory')
    stub = out / 'GodsPVZPackageReferenceMigrator.cs'
    stub.write_text(STUB, 'utf-8')
    rows = []
    legacy = data / 'Managed/UnityEngine.dll'
    if not legacy.is_file():
        raise ValueError('Legacy reference required for the CS0433 regression control')
    for name, selected in [('baseline', sources), ('candidate', [s for s in sources if s.name != stub.name] + [stub]), ('legacy_duplicate_negative', sources)]:
        rsp = out / (name + '.rsp')
        arguments = ['/nostdlib', '/target:library', '/langversion:9.0', '/define:UNITY_EDITOR', '/preferreduilang:en-US', '/utf8output',
                     '/out:"' + str(out / (name + '.dll')) + '"']
        arguments += ['/reference:"' + str(ref) + '"' for ref in references]
        if name == 'legacy_duplicate_negative':
            arguments += ['/reference:"' + str(legacy) + '"']
        arguments += ['"' + str(src) + '"' for src in selected]
        rsp.write_text('\n'.join(arguments) + '\n', 'utf-8')
        proc = subprocess.run([str(runtime), 'exec', str(compiler), '/noconfig', '@' + str(rsp)], capture_output=True, text=True, encoding='utf-8', errors='replace')
        (out / (name + '.log')).write_text(proc.stdout + proc.stderr, 'utf-8')
        output = proc.stdout + proc.stderr
        passed = (proc.returncode != 0 and 'error CS0433:' in output and "'Debug'" in output) if name == 'legacy_duplicate_negative' else proc.returncode == 0
        rows.append(dict(stage=name, exit_code=proc.returncode, expectation='REJECT_CS0433' if name == 'legacy_duplicate_negative' else 'COMPILE_SUCCESS', expectation_met=passed,
                         sources=[dict(path=str(s), sha256=hashlib.sha256(s.read_bytes()).hexdigest()) for s in selected]))
    result = dict(scope='EXACT_EDITOR_HELPER_API_COMPILATION_ONLY', reference_count=len(references), results=rows,
                  references=[dict(path=r.relative_to(data).as_posix(), sha256=hashlib.sha256(r.read_bytes()).hexdigest()) for r in references],
                  unity_started=False, game_runtime_tested=False)
    (out / 'HELPER-COMPILATION.json').write_text(json.dumps(result, indent=2) + '\n', 'utf-8')
    if not all(r['expectation_met'] for r in rows):
        raise SystemExit('Editor helper preflight failed; see compilation logs')
    print('EXACT_EDITOR_HELPERS_COMPILE_PASS')


if __name__ == '__main__':
    main()
