"""Compile the exact R28 export/report pair before acquiring a seat."""
from pathlib import Path
import argparse,hashlib,json,subprocess
from instrument_startup_r28_build_helper import instrument

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--editor',required=True);p.add_argument('--prior',required=True);a=p.parse_args()
 data=Path(a.editor).resolve().parent/'Data';prior=Path(a.prior).resolve();root=Path(__file__).resolve().parents[2]
 original=root/'Recovery/Startup-R3-2026-10-24/ios-support/Assets/Editor/GodsPVZIOSBuild.cs'
 capture=Path(__file__).with_name('GodsPVZBuildReportCaptureR28.cs')
 derived=prior/'GodsPVZIOSBuild-R28-Instrumented.cs';derived.write_bytes(instrument(original.read_bytes()))
 args=[];found=0
 for line in(prior/'candidate.rsp').read_text(encoding='utf-8').splitlines():
  if line.startswith('/out:'):args.append('/out:"'+str(prior/'r28-export-capture.dll')+'"')
  elif line.strip('"').endswith('/Assets/Editor/GodsPVZIOSBuild.cs'):found+=1;args.append('"'+str(derived)+'"')
  else:args.append(line)
 assert found==1
 args.append('"'+str(capture)+'"');rsp=prior/'r28-export-capture.rsp';rsp.write_text('\n'.join(args)+'\n',encoding='utf-8')
 proc=subprocess.run([str(data/'NetCoreRuntime/dotnet'),'exec',str(data/'DotNetSdkRoslyn/csc.dll'),'/noconfig','@'+str(rsp)],capture_output=True)
 (prior/'r28-export-capture.log').write_bytes(proc.stdout+proc.stderr)
 result=dict(scope='EXACT_EDITOR_EXPORT_HELPER_API_ONLY_NOT_UNITY_EXPORT',exit_code=proc.returncode,sources=[dict(file=q.name,sha256=hashlib.sha256(q.read_bytes()).hexdigest())for q in [original,derived,capture]],unity_started=False,game_changed=False)
 (prior/'R28-EXPORT-HELPER-COMPILATION.json').write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8');print(json.dumps(result));raise SystemExit(proc.returncode)
