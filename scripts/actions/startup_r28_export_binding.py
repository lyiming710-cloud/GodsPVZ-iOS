"""Bind the fixed game/SDK and actual report to the complete fresh Xcode tree."""
from pathlib import Path,PurePosixPath
import argparse,hashlib,json,os
from validate_startup_r28_checkpoint import CANDIDATE
from admit_startup_r28_test_ipa import SDK

RECEIPT='startup-r28-export-binding.json'
REPORT='R28-actual-unity-build-report.json'
sha=lambda p:hashlib.sha256(Path(p).read_bytes()).hexdigest()

def report_valid(data):
 if not data.get('build_report_found')or data.get('result')!='Succeeded' or data.get('total_errors')!=0 or data.get('exception')or data.get('errors_ignored')is not False:raise ValueError('Actual successful Unity report with zero errors required')
 if not data.get('original_error_and_result_decisions_preserved')or not data.get('build_helper_api_alias_replaced'):raise ValueError('Unexpected report observer identity')
 if data.get('logs')or any(m['type']in ['Error','Exception','Assert']for s in data.get('steps',[])for m in s.get('messages',[])):raise ValueError('Actual Unity export logged errors')

def files(root):
 result={}
 for p in root.rglob('*'):
  if p.is_symlink():raise ValueError('Export must contain copied files, not symlinks')
  if p.is_file()and p!=root/RECEIPT:
   name=p.relative_to(root).as_posix()
   # Xcode CLI may add user interface state. It is never an input source.
   if name.startswith(('Unity-iPhone.xcodeproj/xcuserdata/','Unity-iPhone.xcodeproj/project.xcworkspace/xcuserdata/')):continue
   result[name]=p
 return result

def validate(xcode,workflow_head):
 xcode=Path(xcode).resolve();data=json.loads((xcode/RECEIPT).read_bytes())
 if data['candidate_sha256']!=CANDIDATE or data['sdk_sha256']!=SDK or data['workflow_head']!=workflow_head:raise ValueError('Export game/SDK/workflow binding mismatch')
 if data.get('production_promotion_allowed')is not False or data.get('whole_game_accepted')is not False:raise ValueError('Diagnostic export is not game qualification')
 rows=data['files'];actual=files(xcode);names={r['path']for r in rows}
 if len(rows)!=len(names)or names!=set(actual):raise ValueError('Export inventory changed or duplicated')
 required={'Data/Managed/Metadata/global-metadata.dat','Unity-iPhone.xcodeproj/project.pbxproj',REPORT}
 if not required<=names or not any(n.startswith('Il2CppOutputProject/Source/il2cppOutput/GodsPVZRuntime1')and n.endswith('.cpp')for n in names):raise ValueError('Incomplete Unity export')
 for r in rows:
  p=PurePosixPath(r['path'])
  if p.is_absolute()or'..'in p.parts:raise ValueError('Unsafe export receipt path')
  target=actual[r['path']]
  if target.stat().st_size!=r['bytes']or sha(target)!=r['sha256']:raise ValueError('Changed export file: '+r['path'])
 report_valid(json.loads((xcode/REPORT).read_bytes()))
 return data

def create(xcode,plugin,sdk,build_report,workflow_head):
 xcode=Path(xcode).resolve();receipt=xcode/RECEIPT
 if receipt.exists()or(xcode/REPORT).exists():raise ValueError('Never replace a previous export receipt/report')
 if sha(plugin)!=CANDIDATE or sha(sdk)!=SDK:raise ValueError('Unity changed the fixed game or SDK')
 report=Path(build_report).read_bytes();report_valid(json.loads(report));(xcode/REPORT).write_bytes(report)
 inventory=files(xcode)
 data=dict(scope='EXACT_R28_UNSIGNED_DIAGNOSTIC_UNITY_EXPORT_FULL_FILE_BINDING',candidate_sha256=CANDIDATE,sdk_sha256=SDK,workflow_head=workflow_head,
  files=[dict(path=n,bytes=p.stat().st_size,sha256=sha(p))for n,p in sorted(inventory.items())],
  production_promotion_allowed=False,whole_game_accepted=False,complex_level_status='ACTUAL_LOSS_RETAINED',device_execution='NOT_PERFORMED')
 receipt.write_text(json.dumps(data,indent=2)+'\n',encoding='utf-8')
 return validate(xcode,workflow_head)

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--xcode-dir',required=True);p.add_argument('--plugin');p.add_argument('--sdk');p.add_argument('--build-report');p.add_argument('--export-workflow-head',default=os.environ.get('GITHUB_SHA'));a=p.parse_args()
 if not a.export_workflow_head:p.error('Explicit export workflow head required')
 if a.plugin:
  if not(a.sdk and a.build_report):p.error('Exact SDK and actual report required for creation')
  result=create(a.xcode_dir,a.plugin,a.sdk,a.build_report,a.export_workflow_head)
 else:result=validate(a.xcode_dir,a.export_workflow_head)
 print(json.dumps(dict(scope=result['scope'],candidate_sha256=result['candidate_sha256'],sdk_sha256=result['sdk_sha256'],export_files=len(result['files']),whole_game_accepted=False)))
