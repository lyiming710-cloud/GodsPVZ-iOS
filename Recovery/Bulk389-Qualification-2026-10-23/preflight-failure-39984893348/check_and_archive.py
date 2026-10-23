from pathlib import Path
import ast
import datetime
import hashlib
import json
import os
import re
import shutil
import subprocess
import sys
import tempfile

Q = Path(__file__).resolve().parent
base = Q.parents[1]
repo = base / 'audit/repo-native113'
D = repo / 'Recovery/Bulk389-Qualification-2026-10-23'
out = D / 'preflight-failure-39984893348'
out.mkdir(exist_ok=False)
shutil.copytree(Q / 'evidence', out / 'evidence')
sys.path.insert(0, str(repo / 'scripts/actions'))
from preflight_bulk389_editor import select_references
roster = json.loads((repo / 'scripts/actions/bulk389_editor_reference_roster.json').read_text())
with tempfile.TemporaryDirectory() as tmp:
    data = Path(tmp)
    for relative in roster['references']:
        file = data / relative
        file.parent.mkdir(parents=True, exist_ok=True)
        file.write_bytes(b'PATH_SELECTION_FIXTURE_ONLY')
    for name in ('UnityEditor.dll', 'UnityEngine.dll', 'arbitrary/UnityOther.dll'):
        file = data / 'Managed' / name
        file.parent.mkdir(parents=True, exist_ok=True)
        file.write_bytes(b'MUST_NOT_SELECT_LEGACY_OR_UNLISTED_FILE')
    selected = select_references(data)
    assert len(selected) == 210
    assert {p.relative_to(data).as_posix() for p in selected} == set(roster['references'])
    (data / roster['references'][0]).unlink()
    try:
        select_references(data)
    except ValueError as e:
        assert 'incomplete' in str(e)
    else:
        raise AssertionError('Missing required reference accepted')
for name in ('preflight_bulk389_editor.py', 'check_bulk389_helper_tools.py'):
    ast.parse((repo / 'scripts/actions' / name).read_text('utf-8'))
sys.path.insert(0, str(base / 'audit/bulk388-qualification-2026-10-23/yaml-deps'))
import yaml
paths = [repo / '.github/workflows' / f for f in ('bulk389-real-ios-ipa.yml', 'bulk389-editor-helper-preflight.yml')]
embedded = 0
for path in paths:
    doc = yaml.load(path.read_text('utf-8'), Loader=yaml.BaseLoader)
    if path.name == 'bulk389-real-ios-ipa.yml':
        assert 'push' not in doc['on']
        assert 'editor_helper_preflight' in doc['jobs']['export']['needs']
    for job in doc['jobs'].values():
        for step in job.get('steps', []):
            for block in re.findall(r"<<'PY'\n(.*?)\nPY(?:\n|$)", step.get('run', ''), re.S):
                ast.parse(block)
                embedded += 1
proc = subprocess.run([str(base / 'audit/stage9-native4/tools/actionlint.exe'), '-shellcheck=', '-pyflakes='] + list(map(str, paths)), capture_output=True)
assert proc.returncode == 0, proc.stdout + proc.stderr
(out / 'actionlint.log').write_bytes(proc.stdout + proc.stderr)
run = json.loads(subprocess.check_output([r'C:\Program Files\GitHub CLI\gh.exe', 'run', 'view', '39984893348', '-R', 'lyiming710-cloud/GodsPVZ-iOS', '--json', 'status,conclusion,headSha,jobs,url']))
assert run['conclusion'] == 'failure'
run['observed_at_utc'] = datetime.datetime.now(datetime.timezone.utc).isoformat()
(out / 'RUN-FINAL.json').write_bytes((json.dumps(run, indent=2) + '\n').encode())
bad_rsp = (out / 'evidence/baseline.rsp').read_text('utf-8')
bad_refs = re.findall(r'(?m)^/reference:"[^"]*/Editor/Data/([^"]+)"$', bad_rsp)
assert len(bad_refs) == 342 and 'Managed/UnityEngine.dll' in bad_refs
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
result = dict(scope='PREFLIGHT_TOOL_DEFECT_AUDIT', failed_run=39984893348, game_dll_changed=False,
              error='CS0433_DUPLICATE_UNITY_TYPES_FROM_RECURSIVE_REFERENCE_GLOB',
              responsibility='NEW_PREFLIGHT_TOOL_BUG_INTRODUCED_BY_CODEX',
              failed_reference_count=len(bad_refs), corrected_reference_count=210,
              corrected_roster_source_run=39961120509, original_bad_script_commit='3cccc3e91170fb9c3b2badc7433f6f8079039b8e',
              prior_17_fixtures_did_not_cover_real_editor_reference_selection=True,
              source_selector_checks=['EXACT_ROSTER_SELECTED', 'LEGACY_AND_ARBITRARY_FILES_IGNORED', 'MISSING_REQUIRED_FILE_REJECTED'],
              actual_linux_roslyn_repair='PENDING_INDEPENDENT_PREFLIGHT',
              new_real_compilation_negative_control='ADD_ACTUAL_LEGACY_UNITYENGINE_DLL_MUST_REJECT_CS0433_DEBUG',
              full_export_auto_push_disabled=True, unity_activation_in_failed_run='NOT_ATTEMPTED',
              unity_import='NOT_RUN', xcode='NOT_RUN', ipa=None,
              candidate_sha256=sha(base / 'audit/bulk388-qualification-2026-10-23/revision-r4/candidates/candidate1.dll'),
              actionlint_exit=proc.returncode, shellcheck_executed=False, embedded_python_parsed=embedded)
(out / 'AUDIT-RESULT.json').write_bytes((json.dumps(result, indent=2) + '\n').encode())
shutil.copy2(__file__, out / Path(__file__).name)
readme = '''# 新增预检查自身的 CS0433 错误

这次由 Codex 新增的预检查写错引用选择造成，不能归咎于游戏修复又坏了。递归收集 Managed/Unity*.dll 扩为 342 个引用，混入根目录旧整体 UnityEngine.dll 与 UnityEditor.dll；真正 Unity 编译的引用使用 Managed/UnityEngine 子目录内的模块和转发程序集。Debug / JsonUtility 因而重复定义。两套辅助脚本编译都被此工具错误拦下，退出 70；许可证激活、Unity 导入、DLL 注入、iOS 导出和 Xcode 均未执行。

此前本地编译采用的是保留的扁平模块集合，没有根目录旧整体 DLL；17 项流程 fixture 也未覆盖真实 Editor 引用选择。它们不能支撑“Linux Editor 预检查已通过”。完整保留这次失败的 rsp / 编译日志 / helper 报告，明确归档为预检查工具缺陷。

修复：从 Unity 自己生成的编译 rsp 提取固定引用目录，去除仅与平台扩展有关且 helper 不使用的 4 项，保留 210 项 API 引用，禁止根目录旧整体 DLL，不再 glob。路径选择回归验证正确选择、忽略额外污染文件、缺必要引用时拒绝；这是路径 fixture，不冒充真实编译。真实 Linux Roslyn 另需执行两套成功编译，并真实加入旧整体 UnityEngine.dll 产生 CS0433，证明反例有效。

完整导出改为仅手动调度，且依赖独立 helper 资格检查。新独立工作流只恢复确切 Editor，不下载 R3 工程，不激活账号，不运行 Unity/Xcode；第一次恢复后只缓存 compiler/runtime/API/licensing client，以后逐文件 SHA-256 核验并复用。只有实际 Linux 检查通过后才启动完整导出，避免拿完整 12 分钟流程试预检查工具。

R4 DLL、389 方法源码、冻结验证器均不变；没有 IPA。真实预检查通过情况必须以新运行实际日志补充，不由本次代码审查推断。
'''
(out / 'README.md').write_bytes(readme.encode())
p = D / 'README.md'
p.write_bytes(('**更新：39984893348 因新增预检查错误引用旧整体 UnityEngine.dll 报 CS0433，未激活或导入 Unity。已修正引用名单，完整导出暂停自动推送触发，先单独验证预检查。详见 preflight-failure-39984893348/README.md。**\n\n' + p.read_text('utf-8')).encode())
current = json.loads((D / 'CURRENT-RESULT.json').read_text('utf-8'))
current['ios_rerun_observation'] = dict(run_id=39984893348, status='completed', conclusion='failure', failure='NEW_PREFLIGHT_CS0433_DUPLICATE_REFERENCES', observed_at_utc=run['observed_at_utc'])
current['editor_helper_preflight_fix'] = dict(state='PENDING_REAL_LINUX_COMPILATION', references=210, full_export_auto_push_disabled=True)
(D / 'CURRENT-RESULT.json').write_bytes((json.dumps(current, indent=2, ensure_ascii=False) + '\n').encode())
manifest = json.loads((D / 'DELIVERY-MANIFEST.json').read_text('utf-8'))
for name in ('scripts/actions/bulk389_editor_reference_roster.json', 'scripts/actions/check_bulk389_helper_tools.py', '.github/workflows/bulk389-editor-helper-preflight.yml'):
    if not any(e['path'] == name for e in manifest['external_git_paths']):
        manifest['external_git_paths'].append(dict(path=name))
manifest['entries'] = [dict(path=p.relative_to(D).as_posix(), bytes=p.stat().st_size, sha256=sha(p)) for p in sorted(D.rglob('*')) if p.is_file() and p.name != 'DELIVERY-MANIFEST.json']
manifest['external_git_paths'] = [dict(path=e['path'], bytes=(repo/e['path']).stat().st_size, sha256=sha(repo/e['path'])) for e in manifest['external_git_paths']]
(D / 'DELIVERY-MANIFEST.json').write_bytes((json.dumps(manifest, indent=2, ensure_ascii=False) + '\n').encode())
print('REFERENCE_SELECTOR_AND_WORKFLOW_CHECKS_PASS', embedded)
