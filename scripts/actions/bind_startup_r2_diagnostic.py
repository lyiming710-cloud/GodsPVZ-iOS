"""Bind the diagnostic child without inheriting parent compiler/runtime PASS."""
from pathlib import Path
import argparse
import hashlib
import json
import zipfile

PARENT = '6de4db6c94b80a43876aea060b573ed60c1fa7170cff485103d2c808b94762ae'
CANDIDATE = '81815f178e1de70e1e2e5088afd856bb5292801c3ccdc1ffc60b9c24ba223823'

def bind(output, source):
    out=Path(output).resolve()
    repo=Path(__file__).resolve().parents[2]
    checkpoint=repo/'Recovery/Startup-R2-2026-10-24'
    evidence=out/'evidence'
    target=out/'candidate/Assembly-CSharp-bulk401.dll'
    if hashlib.sha256(target.read_bytes()).hexdigest()!=PARENT:
        raise ValueError('Exact separately qualified parent input required')
    parent_provenance=evidence/'PROVENANCE.json'
    parent=json.loads(parent_provenance.read_text('utf-8'))
    if parent['outputs']['unlinked']['sha256']!=PARENT or parent['source_commit']!=source:
        raise ValueError('Same-run parent provenance binding failed')
    if (evidence/'PARENT-BULK401-PROVENANCE.json').exists():
        raise ValueError('Do not overwrite an earlier diagnostic binding')
    isolation=json.loads((checkpoint/'ISOLATION.json').read_text('utf-8'))
    flow=json.loads((checkpoint/'FROZEN-STACK-FLOW.json').read_text('utf-8'))
    for report in (isolation,flow):
        if report['candidate_sha256']!=CANDIDATE:raise ValueError('Wrong child evidence')
    if isolation['status']!='PASS_SCOPED_ISOLATION_ONLY' or isolation['restored_marker_rows']!=111:
        raise ValueError('Scoped isolation evidence missing')
    if flow['passed']!=40 or flow['total']!=40:
        raise ValueError('Scoped flow evidence missing')
    archive=checkpoint/'STARTUP-R2-CANDIDATE.zip'
    with zipfile.ZipFile(archive) as z:
        if z.namelist()!=['STARTUP-R2.dll'] or z.testzip() is not None:
            raise ValueError('Unexpected candidate archive')
        data=z.read('STARTUP-R2.dll')
    if len(data)!=17996800 or hashlib.sha256(data).hexdigest()!=CANDIDATE:
        raise ValueError('Diagnostic candidate bytes differ')
    parent_provenance.rename(evidence/'PARENT-BULK401-PROVENANCE.json')
    target.write_bytes(data)
    report={
        'status':'STARTUP_R2_DIAGNOSTIC_NOT_COMPILER_OR_GAMEPLAY_QUALIFIED',
        'source_commit':source,
        'parent_sha256':PARENT,
        'candidate_sha256':CANDIDATE,
        'outputs':{'unlinked':{'sha256':CANDIDATE}},
        'restored_original_markers':111,
        'new_body_targets':40,
        'fixed_width_field_read_sites':66,
        'remaining_private_field_sites':77,
        'first_45_method_type_flow_failures_preserved':5,
        'parent_compiler_results_apply_to_child':False,
        'child_unity_export_performed':False,
        'child_cpp_compilation_performed':False,
        'child_device_execution_performed':False,
        'diagnostic_scope':'Original schema and actual MainMenu Editor startup only',
    }
    parent_provenance.write_text(json.dumps(report,indent=2)+'\n',encoding='utf-8')
    inspection=evidence/'candidate-inspection.txt'
    if inspection.exists():inspection.rename(evidence/'PARENT-BULK401-candidate-inspection.txt')
    inspection.write_text(json.dumps(report,indent=2)+'\n',encoding='utf-8')
    for name in ['ISOLATION.json','FROZEN-STACK-FLOW.json']:
        (evidence/name).write_bytes((checkpoint/name).read_bytes())
    return report

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--output',required=True);p.add_argument('--source',required=True)
    a=p.parse_args();print(json.dumps(bind(a.output,a.source)))
