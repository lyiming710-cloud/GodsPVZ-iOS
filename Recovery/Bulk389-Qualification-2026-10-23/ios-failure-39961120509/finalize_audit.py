from pathlib import Path
import ast
import hashlib
import json
import shutil
import subprocess

Q = Path(__file__).resolve().parent
repo = Q.parents[1] / 'audit/repo-native113'
D = repo / 'Recovery/Bulk389-Qualification-2026-10-23'
dst = D / 'ios-failure-39961120509'
cli = Path(r'D:\Unity\Unity Hub\UnityLicensingClient_V1\Unity.Licensing.Client.exe')
p = subprocess.run([str(cli), '--help'], capture_output=True)
assert p.returncode == 0 and b'--deactivate-all' in p.stdout + p.stderr
(dst / 'licensing-client-help-local.txt').write_bytes(p.stdout + p.stderr)
shutil.copy2(__file__, dst / Path(__file__).name)
result = json.loads((dst / 'AUDIT-RESULT.json').read_text('utf-8'))
workflow = repo / '.github/workflows/bulk389-real-ios-ipa.yml'
actionlint = Q.parents[1] / 'audit/stage9-native4/tools/actionlint.exe'
proc = subprocess.run([str(actionlint), '-shellcheck=', '-pyflakes=', str(workflow)], capture_output=True)
assert proc.returncode == 0, proc.stdout + proc.stderr
(dst / 'actionlint.log').write_bytes(proc.stdout + proc.stderr)
result['workflow_checks']['workflow_sha256'] = hashlib.sha256(workflow.read_bytes()).hexdigest()
result['xcode_archive_preserved_even_if_cleanup_fails'] = True
result['local_cli_help_evidence'] = 'licensing-client-help-local.txt (Hub client 1.18.3; exact CI client support separately gated)'
(dst / 'AUDIT-RESULT.json').write_text(json.dumps(result, indent=2) + '\n', 'utf-8')
p = dst / 'README.md'
text = p.read_text('utf-8')
text += '\n成功导出的 Xcode 归档现在即使随后清理步骤失败也会上传，避免丢失可复用的长流程产物。本机 Hub licensing client 1.18.3 的 --help 已保存，仅证明本机选项；还原 CI Editor 的客户端会在激活前单独验证。第一次本地编译日志读取受 Windows GBK 解码影响而中止，该次未计通过；已在独立 helper-api-check-r2 目录用 UTF-8 重做，正式证据为本目录 helper-api-check。\n'
p.write_text(text, 'utf-8')
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
manifest = json.loads((D / 'DELIVERY-MANIFEST.json').read_text('utf-8'))
manifest['entries'] = [dict(path=p.relative_to(D).as_posix(), bytes=p.stat().st_size, sha256=sha(p)) for p in sorted(D.rglob('*')) if p.is_file() and p.name != 'DELIVERY-MANIFEST.json']
manifest['external_git_paths'] = [dict(path=e['path'], bytes=(repo/e['path']).stat().st_size, sha256=sha(repo/e['path'])) for e in manifest['external_git_paths']]
(D / 'DELIVERY-MANIFEST.json').write_text(json.dumps(manifest, indent=2, ensure_ascii=False) + '\n', 'utf-8')
print('AUDIT_FINALIZED', len(manifest['entries']), len(manifest['external_git_paths']))
