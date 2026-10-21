"""Read archived results and generated text only; never execute project/test code."""
from pathlib import Path
import collections
import hashlib
import json
import re
import tarfile
import zipfile

BASE = Path(__file__).resolve().parents[2]
OUT = Path(__file__).resolve().parent
OLD = BASE / 'audit/actions-migration-bulk152-2026-10-20/ACTIONS-RUN-39416885984-EVIDENCE.zip'
NEWROOT = BASE / 'audit/native-bulk162-r1-codex-2026-10-20/verification-actions-39443473339/Bulk162-Linux-evidence-39443473339-1'
NEWRUN = NEWROOT / 'bulk162-run'


def digest(data):
    return hashlib.sha256(data).hexdigest()


def read_generated(archive):
    selected = {}
    cpp_count = 0
    for member in archive:
        if not member.isfile():
            continue
        name = member.name.rsplit('/', 1)[-1]
        if name.endswith('.cpp'):
            cpp_count += 1
        if (re.fullmatch(r'GodsPVZRuntime1(?:__\d+)?\.cpp', name)
                or name == 'GodsPVZRuntime1_CodeGen.c'):
            data = archive.extractfile(member).read()
            selected[name] = {
                'archive_path': member.name,
                'bytes': len(data), 'sha256': digest(data),
                'text': data.decode('utf-8', errors='replace'),
            }
    return selected, cpp_count


def method_inventory(files):
    match = re.search(r's_methodPointers\[(\d+)\]\s*=\s*\{(.*?)\};',
                      files['GodsPVZRuntime1_CodeGen.c']['text'], re.S)
    pointers = [p.strip() for p in match[2].split(',') if p.strip()]
    by_symbol = collections.defaultdict(list)
    for i, sym in enumerate(pointers):
        if sym not in ('NULL', '0'):
            by_symbol[sym].append(f'0x{0x06000001+i:08X}')
    methods = {}
    for name, record in files.items():
        if not name.endswith('.cpp'):
            continue
        for lineno, line in enumerate(record['text'].splitlines(), 1):
            stripped = line.strip()
            if (stripped.startswith('IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR ')
                    and '(' in stripped and not stripped.endswith(';')):
                definition = re.search(r'IL2CPP_METHOD_ATTR\s+.*?([A-Za-z0-9_]+_m[A-Fa-f0-9]{40})\s*\(', line)
                if definition:
                    for token in by_symbol.get(definition[1], []):
                        methods[token] = {
                            'token': token, 'symbol': definition[1],
                            'file': name, 'line': lineno,
                        }
    return pointers, methods


def error_methods(report):
    return {m['token']: m for rows in report.values() for m in rows}


def save(name, value):
    (OUT / name).write_text(json.dumps(value, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')


with zipfile.ZipFile(OLD) as archive:
    old_status = json.loads(archive.read('bulk152-run/results/CPP-STATUS.json'))
    old_tus = json.loads(archive.read('bulk152-run/results/GAME-TU-RESULTS.json'))
    old_errors = error_methods(json.loads(archive.read('bulk152-run/results/REMAINING-METHODS-BY-ROOTCAUSE.json')))
    old_support = json.loads(archive.read('bulk152-run/results/il2cpp_conversion/COHERENT-PLAYER-INPUT-ASSEMBLIES.json'))
    old_last_log = archive.read('bulk152-run/results/clang_logs/GodsPVZRuntime1__18.cpp.log').decode()
    with archive.open('bulk152-generated-sources.tar.gz') as nested:
        with tarfile.open(fileobj=nested, mode='r|gz') as generated:
            old_files, old_cpp_count = read_generated(generated)

with tarfile.open(NEWROOT / 'bulk162-generated-sources.tar.gz', mode='r|gz') as generated:
    new_files, new_cpp_count = read_generated(generated)

new_status = json.loads((NEWRUN / 'verifier_output/results/CPP-STATUS.json').read_text())
new_tus = json.loads((NEWRUN / 'verifier_output/results/GAME-TU-RESULTS.json').read_text())
new_errors = error_methods(json.loads((NEWRUN / 'verifier_output/results/REMAINING-METHODS-BY-ROOTCAUSE.json').read_text()))
new_support = json.loads((NEWRUN / 'verifier_output/results/il2cpp_conversion/COHERENT-PLAYER-INPUT-ASSEMBLIES.json').read_text())
old_pointers, old_methods = method_inventory(old_files)
new_pointers, new_methods = method_inventory(new_files)

inventories = {}
for label, files, tus, pointers, methods, cpp_count in [
        ('Bulk152', old_files, old_tus, old_pointers, old_methods, old_cpp_count),
        ('Bulk162', new_files, new_tus, new_pointers, new_methods, new_cpp_count)]:
    generated_tus = {n for n in files if n.endswith('.cpp')}
    logged_tus = {r['file'] for r in tus}
    hash_mismatches = []
    for row in tus:
        actual = files.get(row['file'])
        if actual is None or any(actual[k] != row[k] for k in ['bytes', 'sha256']):
            hash_mismatches.append(row['file'])
    inventories[label] = {
        'cpp_files_in_archive': cpp_count,
        'generated_game_tus': sorted(generated_tus),
        'logged_game_tus': sorted(logged_tus),
        'generated_but_not_logged': sorted(generated_tus-logged_tus),
        'logged_but_not_generated': sorted(logged_tus-generated_tus),
        'source_hash_or_size_mismatches': hash_mismatches,
        'pointer_slots': len(pointers),
        'null_pointer_tokens': [f'0x{0x06000001+i:08X}' for i,s in enumerate(pointers) if s in ('NULL','0')],
        'mapped_methods': len(methods),
        'files': {k: {x:v for x,v in r.items() if x!='text'} for k,r in files.items()},
    }

removed = sorted(set(old_errors)-set(new_errors))
added = sorted(set(new_errors)-set(old_errors))
changed = []
for token in sorted(set(old_errors)&set(new_errors)):
    if old_errors[token]['error_count'] != new_errors[token]['error_count']:
        changed.append({'token':token, 'symbol':new_errors[token]['symbol'],
                        'old':old_errors[token]['error_count'], 'new':new_errors[token]['error_count']})
new10 = ['0x060000B9','0x0600049A','0x0600049D','0x060005D2','0x060005D4',
         '0x06000711','0x060007AC','0x060007AF','0x060007B2','0x060007B5']
method_delta = {
    'scope': 'Existing generated text and archived compiler diagnostics only; heuristic attribution, not behavioral equivalence.',
    'old_error_count':old_status['total_errors_in_game_tus'],
    'new_error_count':new_status['total_errors_in_game_tus'],
    'old_errored_methods':len(old_errors), 'new_errored_methods':len(new_errors),
    'removed_error_methods':[{**old_errors[t],'new_error_count':0} for t in removed],
    'newly_errored_methods':[new_errors[t] for t in added],
    'still_errored_changed_counts':changed,
    'new10': [{'token':t,'symbol':new_methods.get(t,{}).get('symbol'),
               'old_error_count':old_errors.get(t,{}).get('error_count',0),
               'new_error_count':new_errors.get(t,{}).get('error_count',0),
               'old_definition':old_methods.get(t),'new_definition':new_methods.get(t)} for t in new10],
}

old_assemblies = {r['name']:r for r in old_support['files']}
new_assemblies = {r['name']:r for r in new_support['files']}
coverage = {
    'scope':'Read-only inventory comparison of saved source archives and saved compilation receipts.',
    'inventories':inventories,
    'mapped_tokens_missing_after':sorted(set(old_methods)-set(new_methods)),
    'mapped_tokens_added_after':sorted(set(new_methods)-set(old_methods)),
    'pointer_slot_symbols_changed':[{'token':f'0x{0x06000001+i:08X}','old':a,'new':b}
                                    for i,(a,b) in enumerate(zip(old_pointers,new_pointers)) if a!=b],
    'baseline_last_tu_method_relocations':[{**old_methods[t], 'new_file':new_methods.get(t,{}).get('file'),
                                         'new_line':new_methods.get(t,{}).get('line')}
                                        for t in sorted(old_methods) if old_methods[t]['file']=='GodsPVZRuntime1__18.cpp'],
    'support_assemblies':{
        'old_count':len(old_assemblies),'new_count':len(new_assemblies),
        'removed':sorted(set(old_assemblies)-set(new_assemblies)),
        'added':sorted(set(new_assemblies)-set(old_assemblies)),
        'changed': [{'name':n,'old':old_assemblies[n],'new':new_assemblies[n]}
                    for n in sorted(set(old_assemblies)&set(new_assemblies)) if old_assemblies[n]!=new_assemblies[n]],
    },
}
save('GENERATED-SOURCE-COVERAGE.json',coverage)
save('COMPILER-DIAGNOSTIC-DELTA.json',method_delta)
(OUT/'BASELINE-LAST-TU-CLANG.log').write_text(old_last_log,encoding='utf-8')

typed = {}
for p in sorted((NEWRUN/'results').glob('*.typed.json')):
    data = json.loads(p.read_text())
    typed[p.name] = {'methods':len(data), 'status_counts':dict(collections.Counter(x['status'] for x in data)),
                     'nonpass':[x for x in data if x['status']!='PASS']}
save('TYPING-RESULTS-READBACK.json',typed)

print(json.dumps({
    'inventories': {k:{x:v for x,v in d.items() if x not in ['files','null_pointer_tokens']} for k,d in inventories.items()},
    'method_sets': {k:coverage[k] for k in ['mapped_tokens_missing_after','mapped_tokens_added_after','pointer_slot_symbols_changed']},
    'last_tu_moved_methods':coverage['baseline_last_tu_method_relocations'],
    'support':coverage['support_assemblies'],
    'error_delta':method_delta,
},ensure_ascii=False,indent=2))
