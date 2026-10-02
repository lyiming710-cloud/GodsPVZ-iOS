from pathlib import Path
import json, os, subprocess, hashlib, shutil, time, re, sys, concurrent.futures

w = Path('/workspaces/GodsPVZ-native24-codespace')
old = Path('/workspaces/GodsPVZ-native19')
prev = w / '.validation/native28-2026-10-01'
prev29 = w / '.validation/native52-2026-10-02'
n = w / '.validation/native53-2026-10-02'
r = w / '.validation/native25-2026-10-01/native24-restored'
candidate_dll = n / 'candidate1.dll'

stage = n / 'fresh'
stage.mkdir(exist_ok=True)

def sha(p): return hashlib.sha256(Path(p).read_bytes()).hexdigest()

managed = stage / 'managed'
shutil.copytree(r / 'managed-support', managed, dirs_exist_ok=True)
shutil.copyfile(candidate_dll, managed / 'GodsPVZRuntime1.dll')

cmd = json.loads((r / 'work/raw-fresh-command.json').read_text())
new_cmd = []
for a in cmd:
    if a.startswith('--assembly='):
        new_cmd.append('--assembly=' + str(managed / Path(a.split('=', 1)[1]).name))
    elif a.startswith('--generatedcppdir='):
        new_cmd.append('--generatedcppdir=' + str(stage / 'cpp'))
    elif a.startswith('--symbols-folder='):
        new_cmd.append('--symbols-folder=' + str(stage / 'symbols'))
    elif a.startswith('--data-folder='):
        new_cmd.append('--data-folder=' + str(stage / 'data'))
    else:
        new_cmd.append(a)

for name in ['cpp', 'symbols', 'data']:
    (stage / name).mkdir(exist_ok=True)

(n / 'IL2CPP-COMMAND.json').write_text(json.dumps(new_cmd, indent=2))
start = time.time()
print("Starting IL2CPP conversion...", flush=True)

with (n / 'il2cpp.log').open('w') as log:
    p = subprocess.run(new_cmd, cwd=old / '.validation/native21/replay/canary', env=dict(os.environ, PROJECT_DIR=str(old / '.validation/native21/replay/canary')), stdout=log, stderr=subprocess.STDOUT)
assert p.returncode == 0, (n / 'il2cpp.log').read_text()[-3000:]

generated = {p.name: sha(p) for p in (stage / 'cpp').iterdir() if p.is_file()}
parent_hashes = json.loads((prev29 / 'IL2CPP-RESULT.json').read_text())['generated_file_hashes']
units = sorted((stage / 'cpp').glob('*.cpp'))
assert len(units) == 264, f"Expected 264 units, got {len(units)}"

diff_files = [k for k in sorted(set(generated) | set(parent_hashes)) if generated.get(k) != parent_hashes.get(k)]
conversion = {
    'exit': 0,
    'seconds': time.time() - start,
    'profile': 'Historical unityaot-macos qualifier on Linux; not Unity iOS export',
    'candidate_sha256': sha(candidate_dll),
    'generated_file_hashes': generated,
    'different_files_from_parent': diff_files,
    'metadata_sha256': sha(stage / 'data/Metadata/global-metadata.dat')
}
(n / 'IL2CPP-RESULT.json').write_text(json.dumps(conversion, indent=2))
print('IL2CPP_PASS', json.dumps({k: v for k, v in conversion.items() if k != 'generated_file_hashes'}), flush=True)

flags = json.loads((r / 'work/raw-fresh-clang-flags.json').read_text())
flags = [a.replace('/tmp/godspvz-native24-codespace-2026-10-01/fresh-raw/cpp', str(stage / 'cpp')) for a in flags]
(n / 'CLANG-FLAGS.json').write_text(json.dumps(flags, indent=2))
logs = stage / 'clang'
logs.mkdir(exist_ok=True)

def compile_tu(p):
    log_file = logs / (p.name + '.log')
    with log_file.open('w') as log:
        rc = subprocess.run(flags + [str(p)], stdout=log, stderr=subprocess.STDOUT).returncode
    text = log_file.read_text(errors='replace')
    return {
        'file': p.name,
        'sha256': sha(p),
        'exit': rc,
        'errors': len(re.findall(r'\berror:', text)),
        'fatal': len(re.findall(r'fatal error:', text))
    }

start = time.time()
print(f"Starting clang++ compilation of {len(units)} TUs...", flush=True)
with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:
    results = list(pool.map(compile_tu, units))

(n / 'CLANG-RESULTS.json').write_text(json.dumps(results, indent=2))

sys.path.insert(0, str(w / 'scripts/takeover/native24'))
import cpp_method_map as C
mapping = C.attribute(stage / 'cpp', logs, {row['file']: row for row in results})
(n / 'CPP-METHODS.json').write_text(json.dumps(mapping, indent=2))

previous = json.loads((prev29 / 'CPP-METHODS.json').read_text())
regressions = []
improvements = []
for t, m in mapping['methods'].items():
    now = len(m['errors'])
    before = len(previous['methods'].get(t, {}).get('errors', []))
    if now > before:
        regressions.append({'token': t, 'before': before, 'after': now})
    if now < before:
        improvements.append({'token': t, 'before': before, 'after': now})

targets = {m['token'] for m in json.loads((n / 'patch1.json').read_text())['methods']}
assert not regressions, f"Regressions found: {regressions}"
assert all(not mapping['methods'][t]['errors'] for t in targets), "Errors remaining on targets!"
assert {m['token'] for m in improvements} == targets

report = {
    'compiler': subprocess.check_output(['clang++', '--version'], text=True).splitlines()[0],
    'scope': 'Linux syntax-only with locked Native18 export headers; not Apple build/link',
    'TUs': len(results),
    'failed': sum(x['exit'] != 0 for x in results),
    'errors': sum(x['errors'] for x in results),
    'fatal': sum(x['fatal'] for x in results),
    'seconds': time.time() - start,
    'mapping': mapping['summary'],
    'pass_to_fail_methods': regressions,
    'improved_methods': improvements,
    'targets': {t: mapping['methods'][t] for t in targets}
}
(n / 'CPP-STATUS.json').write_text(json.dumps(report, indent=2))
print("Clang report:", json.dumps({k: report[k] for k in ['compiler', 'scope', 'TUs', 'failed', 'errors', 'seconds', 'mapping']}, indent=2), flush=True)

changed = []
for t, m in mapping['methods'].items():
    if t in targets:
        continue
    b = previous['methods'][t]
    a = (prev29 / 'fresh/cpp' / b['file']).read_text().splitlines()[b['start']-1:b['end']]
    c = (stage / 'cpp' / m['file']).read_text().splitlines()[m['start']-1:m['end']]
    if a != c:
        changed.append(t)

assert not changed, 'Non-target C++ drift: '+repr(changed)
(n / 'CPP-DRIFT.json').write_text(json.dumps(changed, indent=2))
print('NON_TARGET_CPP_DRIFT count:', len(changed), flush=True)

metadata_checks = []
for tag, folder in [('parent', prev29 / 'fresh/cpp'), ('candidate', stage / 'cpp')]:
    args = [a.replace(str(stage / 'cpp'), str(folder)) for a in flags]
    meta_log = n / (tag + '-metadatausage-clang.log')
    with meta_log.open('w') as log:
        rc = subprocess.run(args + ['-x', 'c++', str(folder / 'Il2CppMetadataUsage.c')], stdout=log, stderr=subprocess.STDOUT).returncode
    text = meta_log.read_text(errors='replace')
    metadata_checks.append({'input': tag, 'exit': rc, 'errors': len(re.findall(r'\berror:', text)), 'sha256': sha(folder / 'Il2CppMetadataUsage.c')})

assert all(x['exit'] == 0 and x['errors'] == 0 for x in metadata_checks)
isolation = {
    'non_target_mapped_cpp_methods_equal': len(mapping['methods']) - len(targets),
    'differing_non_target_methods': changed,
    'metadata_usage_c_extra_checks': metadata_checks,
    'global_metadata_equal_parent': conversion['metadata_sha256'] == json.loads((prev29 / 'IL2CPP-RESULT.json').read_text())['metadata_sha256']
}
(n / 'CPP-ISOLATION.json').write_text(json.dumps(isolation, indent=2))
print('CPP_ISOLATION:', json.dumps(isolation, indent=2), flush=True)
print("ALL IL2CPP AND CLANG CHECKS COMPLETE!")

(n/"COMPILER-COMPLETE.json").write_text(json.dumps({"exit":0,"candidate_sha256":sha(candidate_dll)}))
