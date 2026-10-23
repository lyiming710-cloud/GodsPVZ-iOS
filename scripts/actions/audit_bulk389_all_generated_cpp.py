"""Audit ALL generated C++ with the captured physical-iOS Apple compiler flags.

This reuses a frozen Unity export. It does not start Unity, regenerate C++, edit
IL, link the application or package an IPA. Failed units never stop the census.
"""
from pathlib import Path
import argparse
import concurrent.futures
import hashlib
import json
import os
import re
import shutil
import subprocess
import time
from bulk389_export_binding import validate

EXPORT_HEAD = 'bc27f5334334601fd56ed4bcd987af55dd84b6be'
OLD_ROOT = '/Users/runner/work/_temp/build/xcode'


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def compile_flags(xcode):
    repo = Path(__file__).resolve().parents[2]
    template = repo / 'Recovery/Bulk389-Qualification-2026-10-23/apple-failure-39988665189/APPLE-COMPILE-COMMAND.json'
    command = json.loads(template.read_text('utf-8'))
    compiler = subprocess.check_output(['xcrun', '--find', 'clang++'], text=True).strip()
    sdk = subprocess.check_output(['xcrun', '--sdk', 'iphoneos', '--show-sdk-path'], text=True).strip()
    if '/Xcode_16.4.app/' not in compiler or not sdk.endswith('/iPhoneOS18.5.sdk'):
        raise ValueError('Require exact Apple toolchain/SDK')
    flags = [compiler]
    i = 1
    while i < len(command):
        item = command[i]
        if item == '-o':
            i += 2
            continue
        if item in ('-c', '-fcolor-diagnostics', '-fembed-bitcode', '-g', '-O3', '-pipe') or item.endswith('/Generics__63.cpp'):
            i += 1
            continue
        if item == '-isysroot':
            flags.extend([item, sdk])
            i += 2
            continue
        flags.append(item.replace(OLD_ROOT, str(xcode)))
        i += 1
    flags.extend(['-fsyntax-only', '-ferror-limit=0', '-fno-color-diagnostics'])
    if any(OLD_ROOT in arg for arg in flags) or '-arch' not in flags or 'arm64' not in flags:
        raise ValueError('Compiler command not rebound to current export/arm64')
    return flags


def diagnostics(text):
    rows = []
    unparsed = []
    pattern = re.compile(r'^(.*?):(\d+):(\d+): (fatal error|error|warning): (.*)$')
    for line in text.splitlines():
        match = pattern.match(line)
        if match:
            rows.append(dict(file=match[1], line=int(match[2]), column=int(match[3]), severity=match[4], message=match[5]))
        elif ': error:' in line or ': fatal error:' in line:
            unparsed.append(line)
    return rows, unparsed


def method_ranges(path):
    lines = path.read_text('utf-8', errors='replace').splitlines()
    ranges = []
    for at, line in enumerate(lines):
        if line.startswith('IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR ') and not line.rstrip().endswith(';'):
            match = re.search(r'\b([A-Za-z0-9_]+_m[0-9A-Fa-f]{40}(?:_gshared)?)\s*\(', line)
            if not match:
                continue
            end = next((n + 1 for n in range(at + 1, len(lines)) if lines[n] == '}'), len(lines))
            comment = lines[at - 1].removeprefix('// ').strip() if at and lines[at - 1].startswith('// ') else None
            ranges.append(dict(start_line=at + 1, end_line=end, symbol=match[1], signature_comment=comment))
    return ranges


def run_one(path, flags, root, out):
    started = time.monotonic()
    log = out / 'logs' / (path.name + '.log')
    command = flags + [str(path)]
    try:
        with log.open('w', encoding='utf-8') as handle:
            proc = subprocess.run(command, cwd=root, stdout=handle, stderr=subprocess.STDOUT, timeout=240)
        exit_code = proc.returncode
    except subprocess.TimeoutExpired:
        with log.open('a', encoding='utf-8') as handle:
            handle.write('\nAUDIT_PROCESS_TIMEOUT_240_SECONDS\n')
        exit_code = 124
    rows, unparsed = diagnostics(log.read_text('utf-8', errors='replace'))
    return dict(file=path.name, bytes=path.stat().st_size, sha256=sha(path), exit_code=exit_code,
                error_count=sum(r['severity'] in ('error', 'fatal error') for r in rows),
                warning_count=sum(r['severity'] == 'warning' for r in rows),
                diagnostics=rows, unparsed_errors=unparsed, duration_seconds=round(time.monotonic() - started, 3))


def controls(flags, root, out):
    cases = [
        ('valid_pointer_and_integer', 'bool f(int* p, int i) { return p == nullptr && i == 0; }', True, None),
        ('pointer_compared_to_integer', 'bool f(int* p) { return p == (int)0; }', False, 'comparison between pointer and integer'),
        ('integer_compared_to_null', 'bool f(int i) { return i == nullptr; }', False, None),
        ('object_array_index', 'int f(int* a, void* i) { return a[static_cast<unsigned long>(i)]; }', False, 'static_cast'),
    ]
    results = []
    for name, code, expect_success, expected_error in cases:
        source = out / (name + '.cpp')
        source.write_text(code + '\n', 'utf-8')
        result = run_one(source, flags, root, out)
        passed = (result['exit_code'] == 0) if expect_success else (result['exit_code'] != 0 and result['error_count'] > 0)
        if expected_error:
            passed = passed and any(expected_error in r['message'] for r in result['diagnostics'])
        result['expected_success'] = expect_success
        result['expectation_met'] = passed
        results.append(result)
    (out / 'COMPILER-CONTROLS.json').write_text(json.dumps(results, indent=2) + '\n', 'utf-8')
    if not all(r['expectation_met'] for r in results):
        raise ValueError('Compiler controls failed; this audit is not qualified')


def main():
    p = argparse.ArgumentParser()
    p.add_argument('--xcode-dir', required=True)
    p.add_argument('--output-dir', required=True)
    args = p.parse_args()
    root = Path(args.xcode_dir).resolve()
    out = Path(args.output_dir).resolve()
    out.mkdir(parents=True, exist_ok=False)
    (out / 'logs').mkdir()
    validate(root, EXPORT_HEAD)
    cpp = root / 'Il2CppOutputProject/Source/il2cppOutput'
    sources = sorted(cpp.glob('*.cpp'))
    if not sources or not (cpp / 'Generics__63.cpp').is_file():
        raise ValueError('Expected generated C++ inventory missing')
    inventory = [dict(file=s.name, bytes=s.stat().st_size, sha256=sha(s)) for s in sources]
    (out / 'ALL-CPP-INVENTORY.json').write_text(json.dumps(inventory, indent=2) + '\n', 'utf-8')
    flags = compile_flags(root)
    (out / 'CLANG-FLAGS.json').write_text(json.dumps(flags, indent=2) + '\n', 'utf-8')
    controls(flags, root, out)
    known = run_one(cpp / 'Generics__63.cpp', flags, root, out)
    if known['exit_code'] == 0 or not any('comparison between pointer and integer' in r['message'] for r in known['diagnostics']):
        raise ValueError('Failed to reproduce the actual Apple compiler blocker')
    results = [known]
    remaining = [s for s in sources if s.name != 'Generics__63.cpp']
    with concurrent.futures.ThreadPoolExecutor(max_workers=min(3, os.cpu_count() or 1)) as pool:
        futures = {pool.submit(run_one, s, flags, root, out): s for s in remaining}
        for future in concurrent.futures.as_completed(futures):
            result = future.result()
            results.append(result)
            if result['exit_code'] or len(results) % 25 == 0:
                print('ALL_CPP_PROGRESS', len(results), '/', len(sources), result['file'], 'exit', result['exit_code'], 'errors', result['error_count'], flush=True)
    results.sort(key=lambda r: r['file'])
    if {r['file'] for r in results} != {s.name for s in sources} or len(results) != len(sources):
        raise ValueError('Compile coverage inventory mismatch')
    failed_sources = out / 'failed-sources'
    failed_sources.mkdir()
    for result in results:
        if result['exit_code']:
            shutil.copy2(cpp / result['file'], failed_sources / result['file'])
    attributed = []
    for result in results:
        ranges = method_ranges(cpp / result['file']) if result['error_count'] else []
        for diag in result['diagnostics']:
            if diag['severity'] == 'warning':
                continue
            method = next((r for r in ranges if Path(diag['file']).name == result['file'] and r['start_line'] <= diag['line'] <= r['end_line']), None)
            attributed.append(dict(translation_unit=result['file'], diagnostic=diag, generated_method=method,
                                   methoddef_token='UNRESOLVED', signature_is_comment_not_semantic_proof=True))
    report = dict(scope='ALL_GENERATED_CPP_APPLE_ARM64_SYNTAX_ONLY', export_run=39988665189, export_source_commit=EXPORT_HEAD,
                  audit_source_commit=os.environ['GITHUB_SHA'], source_cpp_count=len(sources), compiled_cpp_count=len(results),
                  failed_cpp_count=sum(r['exit_code'] != 0 for r in results), error_diagnostics=sum(r['error_count'] for r in results),
                  unparsed_error_lines=sum(len(r['unparsed_errors']) for r in results),
                  generic_cpp_count=sum(s.name.startswith(('Generics', 'GenericMethods')) for s in sources),
                  generics_named_cpp_count=sum(s.name.startswith('Generics') for s in sources),
                  generic_methods_named_cpp_count=sum(s.name.startswith('GenericMethods') for s in sources),
                  excluded_generated_c_files=[s.name for s in sorted(cpp.glob('*.c'))],
                  error_limit=0, continue_after_failed_unit=True, unity_started=False, il2cpp_regenerated=False,
                  runtime_sources_or_xcode_objc_compiled=False, linked=False, ipa=None, game_semantics='NOT_PROVEN')
    for name, data in [('ALL-TU-RESULTS.json', results), ('ATTRIBUTED-ERRORS.json', attributed), ('ALL-TU-REPORT.json', report)]:
        (out / name).write_text(json.dumps(data, indent=2) + '\n', 'utf-8')
    print(json.dumps(report, indent=2), flush=True)
    raise SystemExit(1 if report['failed_cpp_count'] else 0)


if __name__ == '__main__':
    main()
