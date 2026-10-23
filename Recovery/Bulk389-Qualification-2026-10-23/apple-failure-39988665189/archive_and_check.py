from pathlib import Path
import ast
import datetime
import hashlib
import json
import shutil
import subprocess
import sys

Q = Path(__file__).resolve().parent
base = Q.parents[1]
repo = base / 'audit/repo-native113'
D = repo / 'Recovery/Bulk389-Qualification-2026-10-23'
out = D / 'apple-failure-39988665189'
shutil.copytree(Q / 'unity-evidence-fast', out / 'unity-evidence')
gh = r'C:\Program Files\GitHub CLI\gh.exe'
run = json.loads(subprocess.check_output([gh, 'run', 'view', '39988665189', '-R', 'lyiming710-cloud/GodsPVZ-iOS', '--json', 'status,conclusion,headSha,jobs,url']))
assert run['conclusion'] == 'failure'
assert next(j for j in run['jobs'] if j['name'] == 'export')['conclusion'] == 'success'
assert next(j for j in run['jobs'] if j['name'] == 'build_ipa')['conclusion'] == 'failure'
run['observed_at_utc'] = datetime.datetime.now(datetime.timezone.utc).isoformat()
(out / 'RUN-FINAL.json').write_bytes((json.dumps(run, indent=2) + '\n').encode())
assert (out / 'unity-evidence/license-return-exit.txt').read_text().strip() == '0'
observed = json.loads((out / 'OBSERVED-APPLE-ERRORS.json').read_text())
assert observed['unique_source_error_sites'] == 19 and observed['compiler_error_limit_hit']
ast.parse((repo / 'scripts/actions/audit_bulk389_all_generated_cpp.py').read_text('utf-8'))
proc = subprocess.run([str(base / 'audit/stage9-native4/tools/actionlint.exe'), '-shellcheck=', '-pyflakes=',
                       str(repo / '.github/workflows/bulk389-reuse-export-all-cpp.yml'), str(repo / '.github/workflows/bulk389-real-ios-ipa.yml')], capture_output=True)
assert proc.returncode == 0, proc.stdout + proc.stderr
(out / 'actionlint.log').write_bytes(proc.stdout + proc.stderr)
result = dict(scope='REAL_UNITY_EXPORT_PASS_APPLE_BUILD_FAILURE_AUDIT', run_id=39988665189,
              export_source_commit=run['headSha'], unity_import_and_export='PASS', legacy_migrator_transition='PASS',
              licensing_deactivate_exit=0, ipa=None, candidate_changed=False,
              apple_error_source='Generics__63.cpp', observed_unique_source_error_sites_lower_bound=19,
              apple_error_limit_hit=True, historical_diagnostics_repeated_in_exception_deduplicated=True,
              separate_xcode_clean_failure=True,
              coverage_gap='GodsPVZRuntime1 filename prefix excludes game generic method implementations in Generics shards',
              historical_unchecked_742_labelled_system_was_inaccurate=True,
              next_gate='ALL generated CPP, physical-iOS Apple Clang syntax, ferror-limit=0, continue all units',
              next_gate_unity_started=False, next_gate_xcode_full_build=False,
              reused_export_archive_sha256='93d32bb83a6d93b1c9d8a490bf057db7cfe2e35dad1219ab4924b6ec38538e04',
              all_cpp_audit='PENDING_ACTUAL_EXECUTION', excluded_scope='Generated .c files, native runtime/ObjC/linking are not covered by CPP syntax checks')
(out / 'AUDIT-RESULT.json').write_bytes((json.dumps(result, indent=2) + '\n').encode())
(out / 'README.md').write_bytes('''# 实际 Unity 导出成功后的 Apple 泛型编译错误

39988665189 的辅助检查、67/67 迁移、R4 注入、重新导入、真实 Unity iOS 导出及导出绑定均成功；停用命令退出 0。本轮 Xcode 工程已保存，不应再为检查这批错误重跑 Unity。

Apple 编译首先在 Generics__63.cpp 发现游戏相关 SwfList<T> 的指针/整数比较、指针窄化、object 数组下标及 Dictionary 实参类型错误。日志有 19 个去重源码诊断站点，编译器达到 20 条上限后停止；异常回显重复了一遍，不能算两批独立错误，更不能说总共只有 19/20 个错误。

根本漏项：旧 Linux 门筛选 GodsPVZRuntime1*.cpp，仅编译 14 个前缀文件；泛型定义/实例化落在 Generics__*.cpp。此前将其余 742 个未检查文件统称“系统 CPP”不准确，它们也包含游戏方法。14/14 只能证明这 14 个文件，不能推出整个程序集可编译。

还有单独的 CLEAN FAILED：工作流手工创建了 DerivedData 外的 Release-iphoneos 目录，Xcode 拒绝删除。产品目录改到 DerivedData/Build/Products/Release-iphoneos，移除手工 mkdir。独立工作流先用实际导出的 Xcode 工程执行 clean（不 build）核实该调整。

新检查复用此轮 SHA-256 固定的 350 MB 导出，使用原失败 Cmdline 的 Apple ARM64/iOS SDK 编译参数，去掉目标文件/优化/调试生成参数，加入 fsyntax-only 与 ferror-limit=0。先执行真实编译器正负控制，并要求重现 Generics__63.cpp 的真实错误，再对导出目录所有 .cpp 无文件名前缀筛选地执行检查；每个文件失败后继续，保存全量日志/文件哈希/退出码/错误归属，最终严格非零。方法注释仅供定位，不等于 MethodDef/native 语义证据。

此门不启动 Unity、IL2CPP 重新转换、Xcode 完整编译或 IPA 打包。生成的 .c、native runtime、ObjC 和链接仍是单独范围，不会包装成全项目通过。managed 修复必须回到真实 DLL 与原版证据，不能靠修改生成 C++ 或全局替换零/空值消除编译错误。
'''.encode())
shutil.copy2(__file__, out / Path(__file__).name)
current = json.loads((D / 'CURRENT-RESULT.json').read_text('utf-8'))
current['state'] = 'REAL_UNITY_EXPORT_PASS_APPLE_GENERIC_COMPILATION_FAIL'
current['unity'] = 'REAL_IMPORT_AND_IOS_XCODE_EXPORT_PASS'
current['xcode'] = 'BUILD_FAILED_IN_GENERICS_WITH_ERROR_LIMIT'
current['full_rerun_after_actual_helper_qualification'].update(status='completed', conclusion='failure', export='success', apple_build='failure', observed_at_utc=run['observed_at_utc'])
current['coverage_correction'] = dict(filename_prefix_check_is_not_whole_assembly=True, unchecked_generic_units_can_contain_game_methods=True, observed_source_error_sites_lower_bound=19, full_census_pending=True)
(D / 'CURRENT-RESULT.json').write_bytes((json.dumps(current, indent=2, ensure_ascii=False) + '\n').encode())
p = D / 'README.md'
p.write_bytes(('**最新纠正：39988665189 已成功真实 Unity 导出，Apple 在未被旧 14 文件检查覆盖的 Generics__63.cpp 中报错。泛型分片包含游戏方法；旧“742 个系统 CPP”表述不准确。现复用导出做全生成 CPP 清查，停止完整流程试错。见 apple-failure-39988665189/README.md。**\n\n' + p.read_text('utf-8')).encode())
p = repo / 'Recovery/REMAINING-WORK-2026-10-23.md'
text = p.read_text('utf-8')
text = text.replace('**已知游戏 C++ 报错在 Linux 预检查中已归零，真实 Unity / Apple / IPA 门仍需完成。**', '**旧 Linux 门仅覆盖 14 个文件名前缀 TU；真实 Unity 导出已成功，Apple 泛型分片编译失败。先复用导出完成全生成 CPP 清查，无 IPA。**')
text = text.replace('其余 742 个系统 CPP 未在此门编译', '其余 742 个生成 CPP 未检查，其中含游戏泛型方法，不能统称系统 CPP')
text += '\n2026-10-24 最新：39988665189 真实 Unity 导出成功；Apple 在 Generics__63.cpp 报至少 19 个去重源码诊断并触及错误上限。另修复 Xcode clean 产品目录问题。接下来只复用导出做独立全生成 CPP 检查，不重跑 Unity，也不直接再跑完整构建。\n'
p.write_bytes(text.encode())
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
manifest = json.loads((D / 'DELIVERY-MANIFEST.json').read_text('utf-8'))
for name in ('scripts/actions/audit_bulk389_all_generated_cpp.py', '.github/workflows/bulk389-reuse-export-all-cpp.yml'):
    if not any(e['path'] == name for e in manifest['external_git_paths']):
        manifest['external_git_paths'].append(dict(path=name))
manifest['entries'] = [dict(path=p.relative_to(D).as_posix(), bytes=p.stat().st_size, sha256=sha(p)) for p in sorted(D.rglob('*')) if p.is_file() and p.name != 'DELIVERY-MANIFEST.json']
manifest['external_git_paths'] = [dict(path=e['path'], bytes=(repo/e['path']).stat().st_size, sha256=sha(repo/e['path'])) for e in manifest['external_git_paths']]
(D / 'DELIVERY-MANIFEST.json').write_bytes((json.dumps(manifest, indent=2, ensure_ascii=False) + '\n').encode())
print('APPLE_FAILURE_AND_COVERAGE_CORRECTION_SAVED')
