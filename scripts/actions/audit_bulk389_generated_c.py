"""Check every generated metadata C file in the same frozen export, without Unity."""
from pathlib import Path
import argparse
import concurrent.futures
import json
import os
import subprocess
from audit_bulk389_all_generated_cpp import compile_flags, run_one, sha, EXPORT_HEAD
from bulk389_export_binding import validate


def main():
    p = argparse.ArgumentParser()
    p.add_argument('--xcode-dir', required=True)
    p.add_argument('--output-dir', required=True)
    a = p.parse_args()
    root = Path(a.xcode_dir).resolve()
    out = Path(a.output_dir).resolve()
    out.mkdir(parents=True, exist_ok=False)
    (out / 'logs').mkdir()
    validate(root, EXPORT_HEAD)
    flags = compile_flags(root)
    flags[0] = subprocess.check_output(['xcrun', '--find', 'clang'], text=True).strip()
    flags = [f for f in flags if f not in ('-frtti', '-fexceptions', '-stdlib=libc++')]
    flags[flags.index('-std=c++11')] = '-std=c11'
    flags[flags.index('-x') + 1] = 'c'
    (out / 'CLANG-C-FLAGS.json').write_text(json.dumps(flags, indent=2) + '\n', 'utf-8')
    results = []
    for name, code, success in [
        ('valid_c_mode', '#ifdef __cplusplus\n#error Must_compile_as_C\n#endif\nint f(void){return 1;}', True),
        ('invalid_c_identifier', 'int f(void){return deliberately_undefined_identifier;}', False),
    ]:
        source = out / (name + '.c')
        source.write_text(code + '\n', 'utf-8')
        result = run_one(source, flags, root, out)
        result['expectation_met'] = (result['exit_code'] == 0) if success else (result['exit_code'] != 0 and result['error_count'] > 0)
        results.append(result)
    (out / 'C-COMPILER-CONTROLS.json').write_text(json.dumps(results, indent=2) + '\n', 'utf-8')
    if not all(r['expectation_met'] for r in results):
        raise ValueError('C parser/target controls failed')
    sources = sorted((root / 'Il2CppOutputProject/Source/il2cppOutput').glob('*.c'))
    if len(sources) != 66:
        raise ValueError('Frozen export C inventory changed')
    with concurrent.futures.ThreadPoolExecutor(max_workers=min(3, os.cpu_count() or 1)) as pool:
        rows = list(pool.map(lambda source: run_one(source, flags, root, out), sources))
    if len(rows) != len(sources) or {r['file'] for r in rows} != {s.name for s in sources}:
        raise ValueError('C coverage incomplete')
    report = dict(scope='ALL_GENERATED_METADATA_C_APPLE_ARM64_C11_SYNTAX_ONLY', export_run=39988665189,
                  export_source_commit=EXPORT_HEAD, audit_source_commit=os.environ['GITHUB_SHA'],
                  c_files=len(sources), attempted_c_files=len(rows), failed_c_files=sum(r['exit_code'] != 0 for r in rows),
                  error_diagnostics=sum(r['error_count'] for r in rows), warning_diagnostics=sum(r['warning_count'] for r in rows),
                  flags_derived_from_observed_cpp_command_with_explicit_c_frontend=True,
                  error_limit=0, unity_started=False, il2cpp_regenerated=False, linked=False, ipa=None)
    (out / 'ALL-C-RESULTS.json').write_text(json.dumps(rows, indent=2) + '\n', 'utf-8')
    (out / 'ALL-C-REPORT.json').write_text(json.dumps(report, indent=2) + '\n', 'utf-8')
    print(json.dumps(report, indent=2), flush=True)
    raise SystemExit(1 if report['failed_c_files'] else 0)


if __name__ == '__main__':
    main()
