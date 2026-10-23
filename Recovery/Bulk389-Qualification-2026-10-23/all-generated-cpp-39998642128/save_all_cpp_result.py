from pathlib import Path
import collections
import datetime
import hashlib
import json
import re
import shutil
import subprocess
import zipfile

Q = Path(__file__).resolve().parent
repo = Q.parents[1] / 'audit/repo-native113'
D = repo / 'Recovery/Bulk389-Qualification-2026-10-23'
src = Q / 'all-cpp-39998642128'
cpp = src / 'all-cpp-audit'
out = D / 'all-generated-cpp-39998642128'
out.mkdir(exist_ok=False)
inventory = json.loads((cpp / 'ALL-CPP-INVENTORY.json').read_text())
rows = json.loads((cpp / 'ALL-TU-RESULTS.json').read_text())
report = json.loads((cpp / 'ALL-TU-REPORT.json').read_text())
controls = json.loads((cpp / 'COMPILER-CONTROLS.json').read_text())
assert len(rows) == len(inventory) == report['source_cpp_count'] == report['compiled_cpp_count'] == 259
assert {r['file'] for r in rows} == {r['file'] for r in inventory}
assert len(controls) == 4 and all(c['expectation_met'] for c in controls)
assert [c['exit_code'] for c in controls] == [0, 1, 1, 1]
failures = [r for r in rows if r['exit_code'] != 0]
assert [r['file'] for r in failures] == ['GenericMethods__14.cpp', 'GenericMethods__16.cpp', 'Generics__63.cpp']
assert [r['error_count'] for r in failures] == [3, 1, 34]
assert sum(r['error_count'] for r in rows) == report['error_diagnostics'] == 38
raw_count = 0
for row in rows:
    text = (cpp / 'logs' / (row['file'] + '.log')).read_text('utf-8')
    errors = [line for line in text.splitlines() if re.search(r':\d+:\d+: (?:fatal )?error:', line)]
    assert len(errors) == row['error_count'], row['file']
    assert 'too many errors emitted' not in text
    raw_count += len(errors)
assert raw_count == 38
assert 'CLEAN SUCCEEDED' in (src / 'audit-evidence/xcode-clean.log').read_text('utf-8')
assert not report['unity_started'] and not report['il2cpp_regenerated'] and report['error_limit'] == 0
for row in failures:
    assert hashlib.sha256((cpp / 'failed-sources' / row['file']).read_bytes()).hexdigest() == row['sha256']
gh = r'C:\Program Files\GitHub CLI\gh.exe'
run = json.loads(subprocess.check_output([gh, 'run', 'view', '39998642128', '-R', 'lyiming710-cloud/GodsPVZ-iOS', '--json', 'status,conclusion,headSha,jobs,url']))
assert run['conclusion'] == 'failure' and run['headSha'] == report['audit_source_commit']
run['observed_at_utc'] = datetime.datetime.now(datetime.timezone.utc).isoformat()
(out / 'RUN-FINAL.json').write_bytes((json.dumps(run, indent=2) + '\n').encode())
for name in ('ALL-TU-REPORT.json', 'COMPILER-CONTROLS.json', 'CLANG-FLAGS.json'):
    shutil.copy2(cpp / name, out / name)
shutil.copy2(Q / 'EXPORTED-SOURCE-INVENTORY.json', out / 'EXPORTED-SOURCE-INVENTORY.json')
shutil.copy2(src / 'audit-evidence/xcode-clean.log', out / 'xcode-clean.log')
archive = out / 'ALL-CPP-EVIDENCE.zip'
items = []
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
attributed = json.loads((cpp / 'ATTRIBUTED-ERRORS.json').read_text())
families = collections.Counter(re.sub(r'_m[0-9A-Fa-f]{40}(?:_gshared)?$', '', r['generated_method']['symbol']) for r in attributed)
result = dict(scope='INDEPENDENT_READBACK_OF_ALL_GENERATED_CPP_SYNTAX_CENSUS', attempted_cpp=259, passed_cpp=256, failed_cpp=3,
              error_diagnostics=38, raw_logs_recounted=259, source_inventory_independently_listed_from_exact_archive=True,
              generic_cpp=75, compiler_controls_pass=4, generated_symbol_families=dict(families),
              clr_methoddef_mapping='PENDING', method_behavior='NOT_PROVEN', xcode_clean='PASS',
              c_metadata_files=66, c_metadata_check='SEPARATE_WORKFLOW_PENDING', linked=False, ipa=None)
(out / 'READBACK-RESULT.json').write_bytes((json.dumps(result, indent=2) + '\n').encode())
(out / 'README.md').write_bytes('''# 无 Unity 的独立全生成 C++ 检查已完成

复用 39988665189 的 SHA 锁定 Xcode 导出，实际 Apple ARM64 / iPhoneOS18.5 语法检查共 259/259 个生成 CPP，其中 75 个泛型分片。256 文件通过，3 文件失败，共 38 条源码编译诊断：GenericMethods__14.cpp = 3，GenericMethods__16.cpp = 1，Generics__63.cpp = 34。关闭错误上限并检查全部文件，没有首错停止。独立回读重数了全部 259 份原始日志，未发现被错误上限截断；全量清单与本地实际导出 tar 列表一致。38 是诊断条数，不是 38 个 managed 方法。

4 个实际编译器控制通过：正确指针/整数比较正常编译；指针对整型、整型对 nullptr、object 指针作为数组索引三个错误反例均被拒绝。真实 Generics__63.cpp 的原始阻断也先被重现。新产品目录位于 DerivedData 下的真实 xcodebuild clean 已成功；没有 build / Unity / IL2CPP 重跑 / 链接 / IPA。

全部逐文件源哈希、退出码、日志、归属和失败源 CPP 原文在 ALL-CPP-EVIDENCE.zip，逐文件 ZIP CRC/大小/SHA 已回读，见 EVIDENCE-MANIFEST.json。另列出的 66 个 metadata C 文件将用实际 C 前端单独检查，不能冒充已经覆盖。native runtime / ObjC / 链接及游戏语义仍属于后续门。
'''.encode())
shutil.copy2(__file__, out / Path(__file__).name)
current = json.loads((D / 'CURRENT-RESULT.json').read_text('utf-8'))
current['all_generated_cpp_census'] = dict(run_id=39998642128, cpp_attempted=259, cpp_passed=256, cpp_failed=3, error_diagnostics=38,
                                         generic_cpp=75, raw_logs_recounted=259, xcode_clean='PASS', unity_rerun=False, metadata_c_files_pending=66)
current['coverage_correction']['full_census_pending'] = False
(D / 'CURRENT-RESULT.json').write_bytes((json.dumps(current, indent=2, ensure_ascii=False) + '\n').encode())
p = D / 'README.md'
p.write_bytes(('**无 Unity 的独立 CPP 清查已完成：259/259（含 75 泛型），256 通过、3 失败、38 条诊断，全部日志独立重数；Xcode clean 目录调整真实通过。另 66 个生成 C 文件单独检查中。见 all-generated-cpp-39998642128/README.md。**\n\n' + p.read_text('utf-8')).encode())
p = repo / 'Recovery/REMAINING-WORK-2026-10-23.md'
p.write_bytes((p.read_text('utf-8') + '\n独立 CPP 清查 39998642128：复用导出，259/259 文件全部尝试，256 通过、3 失败，共 38 条诊断，75 泛型分片纳入；Xcode clean 调整真实通过。剩余方法须回到 CLR/native 证据修复；66 个 metadata C 文件单独检查，无 Unity 重跑、无 IPA。\n').encode())
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
manifest = json.loads((D / 'DELIVERY-MANIFEST.json').read_text('utf-8'))
for name in ('scripts/actions/audit_bulk389_generated_c.py', '.github/workflows/bulk389-reuse-export-generated-c.yml'):
    if not any(e['path'] == name for e in manifest['external_git_paths']):
        manifest['external_git_paths'].append(dict(path=name))
manifest['entries'] = [dict(path=p.relative_to(D).as_posix(), bytes=p.stat().st_size, sha256=sha(p)) for p in sorted(D.rglob('*')) if p.is_file() and p.name != 'DELIVERY-MANIFEST.json']
manifest['external_git_paths'] = [dict(path=e['path'], bytes=(repo/e['path']).stat().st_size, sha256=sha(repo/e['path'])) for e in manifest['external_git_paths']]
(D / 'DELIVERY-MANIFEST.json').write_bytes((json.dumps(manifest, indent=2, ensure_ascii=False) + '\n').encode())
print('ALL_CPP_CENSUS_READBACK_PASS', len(items), '259 checked / 3 failed / 38 diagnostics')
