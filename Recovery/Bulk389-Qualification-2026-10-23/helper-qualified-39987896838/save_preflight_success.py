from pathlib import Path
import datetime
import hashlib
import json
import shutil
import subprocess

Q = Path(__file__).resolve().parent
repo = Q.parents[1] / 'audit/repo-native113'
D = repo / 'Recovery/Bulk389-Qualification-2026-10-23'
gh = r'C:\Program Files\GitHub CLI\gh.exe'
run_id = 39987896838
run = json.loads(subprocess.check_output([gh, 'run', 'view', str(run_id), '-R', 'lyiming710-cloud/GodsPVZ-iOS', '--json', 'status,conclusion,headSha,jobs,url']))
assert run['headSha'] == '8e69548d48b2d7caf32c44359efbaeb5fce40728'
assert run['status'] == 'completed' and run['conclusion'] == 'success', 'Actual helper qualification is not complete/pass'
run['observed_at_utc'] = datetime.datetime.now(datetime.timezone.utc).isoformat()
download = Q / ('preflight-' + str(run_id))
assert not download.exists()
subprocess.run([gh, 'run', 'download', str(run_id), '-R', 'lyiming710-cloud/GodsPVZ-iOS', '-n', f'Bulk389-EXACT-EDITOR-HELPER-PREFLIGHT-{run_id}-1', '-D', str(download)], check=True)
report = json.loads((download / 'HELPER-COMPILATION.json').read_text())
rows = report['results']
assert report['reference_count'] == len(report['references']) == 210
assert [(r['stage'], r['exit_code']) for r in rows[:2]] == [('baseline', 0), ('candidate', 0)]
assert rows[2]['stage'] == 'legacy_duplicate_negative' and rows[2]['exit_code'] != 0
assert all(r['expectation_met'] for r in rows)
assert 'error CS0433:' in (download / 'legacy_duplicate_negative.log').read_text()
assert all('error CS' not in (download / (name + '.log')).read_text() for name in ('baseline', 'candidate'))
assert all(r['path'] not in ('Managed/UnityEngine.dll', 'Managed/UnityEditor.dll') for r in report['references'])
tool_manifest = json.loads((download / 'HELPER-TOOLS-MANIFEST.json').read_text())
assert tool_manifest['source_editor_sha256'] == '0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14'
help_text = (download / 'licensing-client-help.txt').read_text()
assert '--deactivate-all' in help_text and '--activate-all' in help_text
dst = D / ('helper-qualified-' + str(run_id))
shutil.copytree(download, dst)
(dst / 'RUN-FINAL.json').write_bytes((json.dumps(run, indent=2) + '\n').encode())
shutil.copy2(__file__, dst / Path(__file__).name)
(dst / 'README.md').write_bytes(('真实 Linux Editor 自带 Roslyn 验证：原迁移 helper 和惰性 helper 两套均 exit 0；真实加入旧整体 UnityEngine.dll 的反例 exit 非零并命中 CS0433。所用 210 个引用逐文件哈希、确切 Editor 工具子集清单及原客户端 --help 均在本目录。此门没有激活账号、运行 Unity 或测试游戏。\n').encode())
current = json.loads((D / 'CURRENT-RESULT.json').read_text('utf-8'))
current['editor_helper_preflight_fix'] = dict(state='ACTUAL_LINUX_EDITOR_ROSLYN_PASS_WITH_CS0433_NEGATIVE_CONTROL', run_id=run_id, source_commit=run['headSha'], references=210, full_export_auto_push_disabled=True)
(D / 'CURRENT-RESULT.json').write_bytes((json.dumps(current, indent=2, ensure_ascii=False) + '\n').encode())
p = D / 'README.md'
p.write_bytes(('**独立修复验证已通过：'+run['url']+'。真实 Linux Roslyn 两套 helper 均成功，重复旧 DLL 反例真实触发 CS0433；游戏与 IPA 尚未因此验收。**\n\n'+p.read_text('utf-8')).encode())
p = repo / 'Recovery/REMAINING-WORK-2026-10-23.md'
text = p.read_text('utf-8')
text += '\n2026-10-24：39984893348 的新增预检查因递归引用旧整体 UnityEngine.dll 报 CS0433。工具引用选择已修正；独立运行 '+str(run_id)+' 的真实 Linux Roslyn 两套编译及重复引用反例通过。接下来恢复真实 Unity 导出；候选仍为原 R4，无 IPA。\n'
p.write_bytes(text.encode())
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
manifest = json.loads((D / 'DELIVERY-MANIFEST.json').read_text('utf-8'))
manifest['entries'] = [dict(path=p.relative_to(D).as_posix(), bytes=p.stat().st_size, sha256=sha(p)) for p in sorted(D.rglob('*')) if p.is_file() and p.name != 'DELIVERY-MANIFEST.json']
manifest['external_git_paths'] = [dict(path=e['path'], bytes=(repo/e['path']).stat().st_size, sha256=sha(repo/e['path'])) for e in manifest['external_git_paths']]
(D / 'DELIVERY-MANIFEST.json').write_bytes((json.dumps(manifest, indent=2, ensure_ascii=False) + '\n').encode())
print('ACTUAL_EDITOR_HELPER_QUALIFICATION_READBACK_PASS', [(r['stage'], r['exit_code']) for r in rows])
