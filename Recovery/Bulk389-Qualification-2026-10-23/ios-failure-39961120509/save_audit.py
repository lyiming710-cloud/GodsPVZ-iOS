from pathlib import Path
import ast
import datetime
import hashlib
import json
import re
import shutil
import subprocess
import sys

Q = Path(__file__).resolve().parent
base = Q.parents[1]
repo = base / 'audit/repo-native113'
D = repo / 'Recovery/Bulk389-Qualification-2026-10-23'
dst = D / 'ios-failure-39961120509'
dst.mkdir(exist_ok=False)
gh = r'C:\Program Files\GitHub CLI\gh.exe'
run = json.loads(subprocess.check_output([gh, 'run', 'view', '39961120509', '-R', 'lyiming710-cloud/GodsPVZ-iOS', '--json', 'status,conclusion,headSha,url,jobs']))
assert run['status'] == 'completed' and run['conclusion'] == 'failure'
run['observed_at_utc'] = datetime.datetime.now(datetime.timezone.utc).isoformat()
(dst / 'RUN-FINAL.json').write_text(json.dumps(run, indent=2) + '\n', 'utf-8')
shutil.copytree(Q / 'unity-evidence', dst / 'unity-evidence')
shutil.copytree(Q / 'helper-api-check-r2', dst / 'helper-api-check', ignore=shutil.ignore_patterns('*.dll'))
for name in ('check_helpers_local.py', 'update_workflow.py', 'save_audit.py'):
    shutil.copy2(Q / name, dst / name)
control = subprocess.check_output([sys.executable, str(repo / 'scripts/actions/check_bulk389_pipeline.py')])
report = json.loads(control)
assert report['passed'] == 17
(dst / 'PIPELINE-CONTROLS.json').write_text(json.dumps(report, indent=2) + '\n', 'utf-8')
proc = subprocess.run([str(base / 'audit/stage9-native4/tools/actionlint.exe'), '-shellcheck=', '-pyflakes=', str(repo / '.github/workflows/bulk389-real-ios-ipa.yml')], capture_output=True)
assert proc.returncode == 0, proc.stdout + proc.stderr
(dst / 'actionlint.log').write_bytes(proc.stdout + proc.stderr)
sys.path.insert(0, str(base / 'audit/bulk388-qualification-2026-10-23/yaml-deps'))
import yaml
workflow = repo / '.github/workflows/bulk389-real-ios-ipa.yml'
document = yaml.load(workflow.read_text('utf-8'), Loader=yaml.BaseLoader)
jobs = document['jobs']
assert set(jobs) == {'conversion_binding', 'apple_preflight', 'export', 'build_ipa'}
assert jobs['export']['needs'] == ['conversion_binding', 'apple_preflight']
steps = jobs['export']['steps']
names = [s.get('name', '') for s in steps]
assert names.index('Prepare software graphics for original 8192 TMP atlas') < names.index('Acquire Personal seat')
assert names.index('Compile both helper states and verify licensing CLI before activation') < names.index('Acquire Personal seat')
embedded = 0
for job in jobs.values():
    for step in job['steps']:
        for block in re.findall(r"<<'PY'\n(.*?)\nPY(?:\n|$)", step.get('run', ''), re.S):
            ast.parse(block)
            embedded += 1
scripts = ['prepare_bulk389_unity.py', 'preflight_bulk389_editor.py', 'bulk389_export_binding.py', 'check_bulk389_pipeline.py', 'package_bulk389_ipa.py']
for name in scripts:
    ast.parse((repo / 'scripts/actions' / name).read_text('utf-8'))
compile_result = json.loads((dst / 'helper-api-check/RESULT.json').read_text('utf-8'))
assert [r['exit'] for r in compile_result['results']] == [0, 0, 1]
assert compile_result['results'][2]['cs2001']
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
log = (Q / 'unity-evidence/import-after-bulk389.log').read_text('utf-8')
assert (Q / 'unity-evidence/import-after-bulk389-exit.txt').read_text().strip() == '0'
errors = re.findall(r'(?m)^.*\berror CS\d+.*$', log)
assert len(errors) == 1 and 'CS2001' in errors[0]
assert (Q / 'unity-evidence/license-return-exit.txt').read_text().strip() == '28'
summary = dict(scope='BROAD_BUILD_PIPELINE_AUDIT_NOT_GAME_SEMANTIC_ACCEPTANCE', failed_run=39961120509,
               primary_failure=dict(unity_exit=0, compiler_error_lines=errors, failure='STALE_SOURCE_IN_CACHED_BEE_RESPONSE_FILE_REJECTED_BY_LOG_GATE'),
               cleanup_failure=dict(exit=28, message='ULF file not found', github_step_green_was_not_return_success=True, historical_seat_return='NOT_CONFIRMED'),
               actual_candidate_sha256=sha(base / 'audit/bulk388-qualification-2026-10-23/revision-r4/candidates/candidate1.dll'),
               game_dll_modified_this_audit=False, helper_compile_results=compile_result['results'],
               helper_compiler_limit='Local .NET 10 Roslyn against preserved Unity/Mono references; exact Linux Editor compiler is now a CI pre-activation gate',
               workflow_checks=dict(actionlint_exit=proc.returncode, shellcheck_executed=False, embedded_python_parsed=embedded, python_scripts_parsed=len(scripts), workflow_sha256=sha(workflow)),
               controls_passed=report['passed'],
               existing_asset_risks=['Case-colliding R3 animation/sprite/texture names reported by Unity; preserved, visual selection unverified', 'Headless shader fallback warnings retained; no claim of rendering acceptance'],
               historical_reference_issue='Local 73990144-byte Native18 Xcode tar.zst failed tar listing (truncated); not used as input to the new build',
               real_unity_rerun='NOT_STARTED_AT_AUDIT_ARCHIVE_CREATION', xcode='NOT_RUN', ipa=None)
(dst / 'AUDIT-RESULT.json').write_text(json.dumps(summary, indent=2) + '\n', 'utf-8')
readme = '''# 真实 Unity 失败后的扩大检查

39961120509 已失败，无 IPA。Unity 本身重新导入退出 0，但首次 Bee 编译响应文件仍引用被工作流删除的迁移脚本，产生一次 CS2001；下一轮内部重新编译成功并未抹除这条错误，严格日志门正确拦截。未将 grep 放宽以掩盖错误。

另发现许可证归还步骤假绿：--return-ulf 退出 28，ULF 文件不存在。Personal 激活实际使用 entitlement，不能据绿色步骤宣称席位已归还。本轮历史席位归还状态保留 NOT_CONFIRMED。

修复限于一次性重建工程和构建脚本，R4 候选仍为 ef1992326d1ee494a389bc2c78bdfab131721434c0b08ab5f41dbc3e506c4e48。完成 67/67 迁移且 B001、原迁移脚本、候选哈希均正确后，原路径写入无自动初始化/无 DLL 改写功能的迁移脚本占位类，保留 .meta GUID，清理仅 Library/Bee 与 Library/ScriptAssemblies；保留导入的纹理等资产缓存。候选注入后及导出后再次核对 SHA。

本地用保留的真实 Unity/Mono API 引用编译两套 helper：原迁移状态、惰性占位状态均 exit 0；删除源文件反例 exit 1 且重现 CS2001。该本地 Roslyn 不是 Linux Editor 自带编译器；新 CI 在获取许可证前，使用还原 Editor 自带 Roslyn、runtime、4.8 API 编译两状态，若失败就先停。

17 项流程回归检查通过。覆盖迁移标记/父与候选/原脚本哈希、保留路径/GUID/资产缓存、导出绑定及错误候选/工作流/C++ 改写的拒绝；实际 IPA Python 校验代码在合成 Mach-O/ZIP 上验证合法样本和 x86_64、模拟器平台、签名、错误文件角色、不同 metadata、嵌套签名反例。Apple ditto 在这些 fixture 中是明确的测试替身，未执行 Xcode，也没有生成可交付游戏 IPA。

Xcode 16.4 / iphoneos18.5 / ditto 先由独立轻量 Mac job 验证。8192 纹理软件 GL 检查也提前。新导出收据绑定本轮代码、候选、游戏 C++、PBX 和 metadata；Mac 编译前、打包前校验，包内 metadata 与本次导出逐字节比较。签名检查覆盖整个包。许可证 CLI --help 必须有 --deactivate-all，清理使用当前机器的订阅停用命令并严格检查退出码。命令支持与实际停用仍由下一次 CI 观测，不冒充已执行。

完整失败日志、最终 job 状态、本地编译源码/响应文件/引用文件哈希、反例结果和 actionlint 结果均在本目录。actionlint 未调用 shellcheck；YAML 和所有内嵌 Python 已解析。原 R3 资源大小写冲突及 headless shader 警告保留为资源/视觉验收风险。另有本地历史 Native18 Xcode 归档截断，仅为不使用的参考文件，不混入新导出。

这轮检查证明流程边界与拒绝规则，不能替代 Unity/Xcode、389 方法完整语义或真机验收。
'''
(dst / 'README.md').write_text(readme, 'utf-8')
result = json.loads((D / 'CURRENT-RESULT.json').read_text('utf-8'))
result['unity'] = 'BASELINE_IMPORT_AND_67_67_MIGRATION_PASS_CANDIDATE_REIMPORT_LOG_GATE_FAILED_CS2001'
result['real_unity_xcode_ipa_workflow'] = dict(run_id=39961120509, code_commit=run['headSha'], url=run['url'], status_observed='completed', conclusion_observed='failure', observed_at_utc=run['observed_at_utc'])
result['ios_failure_audit'] = dict(path='ios-failure-39961120509/AUDIT-RESULT.json', controls=17, game_dll_unchanged=True, next_real_run='PENDING')
(D / 'CURRENT-RESULT.json').write_text(json.dumps(result, indent=2, ensure_ascii=False) + '\n', 'utf-8')
p = D / 'README.md'
text = p.read_text('utf-8')
text = '**最新：真实流程 39961120509 已失败于重新导入的 CS2001 日志门；未进入 Xcode，无 IPA。已做扩大检查并修复构建工作副本切换，候选 DLL 未改。见 [扩大审查记录](ios-failure-39961120509/README.md)。后文启动时的观测按历史保留。**\n\n' + text
p.write_text(text, 'utf-8')
p = repo / 'Recovery/REMAINING-WORK-2026-10-23.md'
text = p.read_text('utf-8').replace('实际流程 39961120509 已启动，待真实运行产物', '39961120509 重新导入的 CS2001 日志门失败；扩大流程审查 17 检查通过，待修复后重跑')
p.write_text(text, 'utf-8')
manifest = json.loads((D / 'DELIVERY-MANIFEST.json').read_text('utf-8'))
for name in scripts:
    path = 'scripts/actions/' + name
    if not any(e['path'] == path for e in manifest['external_git_paths']):
        manifest['external_git_paths'].append(dict(path=path))
manifest['entries'] = [dict(path=p.relative_to(D).as_posix(), bytes=p.stat().st_size, sha256=sha(p)) for p in sorted(D.rglob('*')) if p.is_file() and p.name != 'DELIVERY-MANIFEST.json']
manifest['external_git_paths'] = [dict(path=e['path'], bytes=(repo/e['path']).stat().st_size, sha256=sha(repo/e['path'])) for e in manifest['external_git_paths']]
(D / 'DELIVERY-MANIFEST.json').write_text(json.dumps(manifest, indent=2, ensure_ascii=False) + '\n', 'utf-8')
print('BROAD_AUDIT_SAVED', report['passed'], len(errors), 'license_return_exit=28')
