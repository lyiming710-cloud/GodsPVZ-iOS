from pathlib import Path
import ast
import datetime
import hashlib
import json
import shutil
import subprocess
import zipfile

Q = Path(__file__).resolve().parent
repo = Q.parents[1] / 'audit/repo-native113'
D = repo / 'Recovery/Bulk389-Qualification-2026-10-23'
src = Q / 'all-c-39999937442'
out = D / 'all-generated-c-39999937442'
out.mkdir(exist_ok=False)
report = json.loads((src / 'ALL-C-REPORT.json').read_text())
rows = json.loads((src / 'ALL-C-RESULTS.json').read_text())
controls = json.loads((src / 'C-COMPILER-CONTROLS.json').read_text())
assert report['c_files'] == report['attempted_c_files'] == len(rows) == 66
assert report['failed_c_files'] == report['error_diagnostics'] == 0
assert all(r['exit_code'] == 0 for r in rows)
assert [r['exit_code'] for r in controls] == [0, 1] and all(r['expectation_met'] for r in controls)
sources = Q / 'complete-generated-sources/xcode/Il2CppOutputProject/Source/il2cppOutput'
assert {p.name for p in sources.glob('*.c')} == {r['file'] for r in rows}
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
for row in rows:
    file = sources / row['file']
    assert file.stat().st_size == row['bytes'] and sha(file) == row['sha256']
    text = (src / 'logs' / (row['file'] + '.log')).read_text('utf-8')
    assert ': error:' not in text and ': fatal error:' not in text and 'warning:' not in text
run = json.loads(subprocess.check_output([r'C:\Program Files\GitHub CLI\gh.exe', 'run', 'view', '39999937442', '-R', 'lyiming710-cloud/GodsPVZ-iOS', '--json', 'status,conclusion,headSha,jobs,url']))
assert run['conclusion'] == 'success' and run['headSha'] == report['audit_source_commit']
run['observed_at_utc'] = datetime.datetime.now(datetime.timezone.utc).isoformat()
(out / 'RUN-FINAL.json').write_bytes((json.dumps(run, indent=2) + '\n').encode())
for name in ('ALL-C-REPORT.json', 'C-COMPILER-CONTROLS.json', 'CLANG-C-FLAGS.json'):
    shutil.copy2(src / name, out / name)
items = []
archive = out / 'ALL-C-EVIDENCE.zip'
with zipfile.ZipFile(archive, 'x', zipfile.ZIP_DEFLATED, compresslevel=6) as z:
    for file in sorted(src.rglob('*')):
        if file.is_file():
            b = file.read_bytes()
            relative = file.relative_to(src).as_posix()
            z.writestr(relative, b)
            items.append(dict(path=relative, bytes=len(b), sha256=hashlib.sha256(b).hexdigest()))
with zipfile.ZipFile(archive) as z:
    assert z.testzip() is None
    for item in items:
        b = z.read(item['path'])
        assert len(b) == item['bytes'] and hashlib.sha256(b).hexdigest() == item['sha256']
(out / 'EVIDENCE-MANIFEST.json').write_bytes((json.dumps(dict(archive=archive.name, entries=items), indent=2) + '\n').encode())
shutil.copy2(__file__, out / Path(__file__).name)
shutil.copy2(Q / 'GENERIC-METHOD-NAME-CANDIDATES.json', D / 'all-generated-cpp-39998642128/GENERIC-METHOD-NAME-CANDIDATES.json')
cpp_folder = D / 'all-generated-cpp-39998642128'
cpp_report = json.loads((cpp_folder / 'ALL-TU-REPORT.json').read_text())
inventory = json.loads((Q / 'all-cpp-39998642128/all-cpp-audit/ALL-CPP-INVENTORY.json').read_text())
generics_count = sum(r['file'].startswith('Generics') for r in inventory)
generic_methods_count = sum(r['file'].startswith('GenericMethods') for r in inventory)
assert (generics_count, generic_methods_count) == (75, 18)
cpp_run = json.loads((cpp_folder / 'RUN-FINAL.json').read_text())
cpp_step = next(s for j in cpp_run['jobs'] for s in j['steps'] if s['name'].startswith('Audit every generated CPP'))
c_step = next(s for j in run['jobs'] for s in j['steps'] if s['name'].startswith('Compile all 66'))
duration = lambda step: (datetime.datetime.fromisoformat(step['completedAt'].replace('Z', '+00:00')) - datetime.datetime.fromisoformat(step['startedAt'].replace('Z', '+00:00'))).total_seconds()
combined = dict(scope='FROZEN_EXPORT_ALL_GENERATED_C_AND_CPP_SYNTAX_CENSUS_ONLY', export_run=39988665189,
                cpp=dict(run_id=39998642128, attempted=259, passed=256, failed=3, error_diagnostics=38, check_step_seconds=duration(cpp_step)),
                c=dict(run_id=39999937442, attempted=66, passed=66, failed=0, error_diagnostics=0, check_step_seconds=duration(c_step)),
                total_generated_sources=325, total_failed_sources=3, total_passed_sources=322,
                generics_named_cpp=75, generic_methods_named_cpp=18, total_generic_cpp=93,
                historical_generic_count_75_excluded_GenericMethods_name_only=True,
                all_325_source_hashes_recomputed_from_exact_archive=True,
                compiler_controls_passed=6, xcode_clean='PASS', full_unity_rerun=False, full_xcode_build=False,
                linked=False, game_semantics='NOT_PROVEN', ipa=None,
                proposed_method_name_candidates=12, exact_clr_and_native_binding='PENDING',
                next_actions=['Bind the ten generated name families/overload candidates to exact managed definitions and original native evidence',
                              'Repair managed bodies in a new explicitly scoped revision; do not edit generated C++',
                              'Regenerate/check C++ independently for any changed candidate before another Unity export'])
(D / 'COMBINED-GENERATED-SYNTAX-CENSUS.json').write_bytes((json.dumps(combined, indent=2) + '\n').encode())
(out / 'README.md').write_bytes('''# 66 个生成 C 文件已独立检查通过

同一冻结导出的全部 66 个 metadata C 源文件，用实际 Apple ARM64/iPhoneOS18.5 的 C11 前端检查，66/66 通过，错误/警告均为 0；正确 C 模式与未声明标识符反例分别 exit 0/1。根代理重新核验了 66 份日志，并从本地 SHA-256 锁定的完整导出直接重算每个源文件哈希，全部一致。实际检查步骤耗时 6 秒（不含排队、下载、解压）。没有 Unity/IL2CPP/完整 Xcode build/链接/IPA。

结合之前 C++ 门，325 个生成源文件均已实际做对应语言语法检查；322 个通过，3 个失败，38 条诊断集中在泛型代码。C++ 门检查步骤耗时 66 秒。注意历史 C++ 报告 generic_cpp_count=75 只计 Generics 前缀，另 18 个 GenericMethods 也已检查，总泛型 C++ 文件实际是 93；该字段显示口径已更正，未改变检查覆盖或重写历史报告。

完整证据在 ZIP；C 前端参数由真实 C++ 构建命令保留平台/宏/include 后切换为 C11，不将其冒充完整链接资格。两个独立检查和完整导出均保持手动调度，工具或文档小改动不会自动重复跑。
'''.encode())
p = cpp_folder / 'README.md'
p.write_bytes((p.read_text('utf-8') + '\n计数口径补充：75 是 Generics 命名前缀文件数，另有 18 个 GenericMethods 文件，均在 259/259 中实际检查，总泛型 C++ 文件为 93。旧运行原始 JSON 保留，后续工具显示计数已修正。\n').encode())
current = json.loads((D / 'CURRENT-RESULT.json').read_text('utf-8'))
current['all_generated_c_census'] = combined['c']
current['all_generated_cpp_census'].update(generics_named_cpp=75, generic_methods_named_cpp=18, total_generic_cpp=93, metadata_c_files_pending=0)
current['combined_generated_source_census'] = dict(attempted=325, passed=322, failed=3, error_diagnostics=38, all_source_hashes_recomputed=True, linked=False, ipa=None)
(D / 'CURRENT-RESULT.json').write_bytes((json.dumps(current, indent=2, ensure_ascii=False) + '\n').encode())
p = D / 'README.md'
p.write_bytes(('**独立生成代码预检完成：259 CPP + 66 C = 325 文件全部检查；322 通过、3 失败、38 条诊断，未重跑 Unity 或完整 Xcode build。CPP 检查 66 秒，C 检查 6 秒，不含下载排队。泛型文件为 75 Generics + 18 GenericMethods。见 COMBINED-GENERATED-SYNTAX-CENSUS.json。**\n\n' + p.read_text('utf-8')).encode())
p = repo / 'Recovery/REMAINING-WORK-2026-10-23.md'
p.write_bytes((p.read_text('utf-8') + '\n66 个生成 C 文件已由独立运行 39999937442 检查通过。结合 CPP 门，325 个生成源文件均已尝试；3 个泛型 CPP 仍有 38 条诊断。两类编译检查实际步骤分别 66 秒/6 秒，不含排队下载；没有重跑完整流程。接下来按精确 CLR/native 绑定修复这些泛型方法，不能用旧导出证明新 DLL。\n').encode())
proc = subprocess.run([str(Q.parents[1] / 'audit/stage9-native4/tools/actionlint.exe'), '-shellcheck=', '-pyflakes=',
                       str(repo / '.github/workflows/bulk389-reuse-export-all-cpp.yml'), str(repo / '.github/workflows/bulk389-reuse-export-generated-c.yml')], capture_output=True)
assert proc.returncode == 0, proc.stdout + proc.stderr
ast.parse((repo / 'scripts/actions/audit_bulk389_generated_c.py').read_text('utf-8'))
manifest = json.loads((D / 'DELIVERY-MANIFEST.json').read_text('utf-8'))
manifest['entries'] = [dict(path=p.relative_to(D).as_posix(), bytes=p.stat().st_size, sha256=sha(p)) for p in sorted(D.rglob('*')) if p.is_file() and p.name != 'DELIVERY-MANIFEST.json']
manifest['external_git_paths'] = [dict(path=e['path'], bytes=(repo/e['path']).stat().st_size, sha256=sha(repo/e['path'])) for e in manifest['external_git_paths']]
(D / 'DELIVERY-MANIFEST.json').write_bytes((json.dumps(manifest, indent=2, ensure_ascii=False) + '\n').encode())
cache = (repo / 'scripts/actions/__pycache__').resolve()
destination = (Q / 'pipeline-pycache').resolve()
workspace = Q.parents[1].resolve()
assert cache.is_relative_to(workspace) and destination.is_relative_to(workspace)
if cache.exists():
    assert not destination.exists()
    shutil.move(cache, destination)
print('ALL_325_GENERATED_SOURCE_READBACK_COMPLETE', combined['cpp'], combined['c'])
