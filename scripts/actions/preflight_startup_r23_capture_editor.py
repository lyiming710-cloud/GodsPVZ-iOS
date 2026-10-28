"""Compile report capture with the actual exact Editor APIs before license/setup."""
from pathlib import Path
import argparse,hashlib,json,subprocess
from instrument_startup_r20_build_helper import instrument

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--editor',required=True);p.add_argument('--prior',required=True);a=p.parse_args()
    data=Path(a.editor).resolve().parent/'Data';prior=Path(a.prior).resolve();root=Path(__file__).resolve().parents[2]
    original=root/'Recovery/Startup-R3-2026-10-24/ios-support/Assets/Editor/GodsPVZIOSBuild.cs';capture=Path(__file__).parent/'GodsPVZBuildReportCapture.cs';caseaudit=Path(__file__).parent/'GodsPVZAssetCaseAudit.cs';interaction=Path(__file__).parent/'GodsPVZInteractionAuditR23.cs'
    derived=prior/'GodsPVZIOSBuild-Instrumented.cs';derived.write_bytes(instrument(original.read_bytes()))
    lines=(prior/'candidate.rsp').read_text('utf-8').splitlines();found=0;args=[]
    for line in lines:
        if line.startswith('/out:'):args.append('/out:"'+str(prior/'build-capture.dll')+'"')
        elif line.strip('"').endswith('/Assets/Editor/GodsPVZIOSBuild.cs'):
            found+=1;args.append('"'+str(derived)+'"')
        else:args.append(line)
    assert found==1,'Exactly one original build helper must be replaced in preflight input'
    serialization=Path(__file__).parent/'GodsPVZSerializedReferenceProbeR23.cs'
    gameplay=Path(__file__).parent/'GodsPVZGameplayAuditR23.cs'
    args+=['"'+str(capture)+'"','"'+str(caseaudit)+'"','"'+str(interaction)+'"','"'+str(serialization)+'"','"'+str(gameplay)+'"'];rsp=prior/'build-capture.rsp';rsp.write_text('\n'.join(args)+'\n','utf-8')
    proc=subprocess.run([str(data/'NetCoreRuntime/dotnet'),'exec',str(data/'DotNetSdkRoslyn/csc.dll'),'/noconfig','@'+str(rsp)],capture_output=True)
    (prior/'build-capture.log').write_bytes(proc.stdout+proc.stderr)
    result=dict(scope='EXACT_EDITOR_CAPTURE_ASSET_NAMES_INTERACTION_SERIALIZATION_AND_GAMEPLAY_PROBE_API_COMPILATION_ONLY',exit_code=proc.returncode,sources=[dict(file=p.name,sha256=hashlib.sha256(p.read_bytes()).hexdigest()) for p in [original,derived,capture,caseaudit,interaction,serialization,gameplay]],unity_started=False,old_error_gate_changed=False)
    (prior/'BUILD-CAPTURE-HELPER-COMPILATION.json').write_text(json.dumps(result,indent=2)+'\n','utf-8');print(json.dumps(result));raise SystemExit(proc.returncode)
