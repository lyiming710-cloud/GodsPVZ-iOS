"""Add one report observation; do not edit the original helper's error decisions."""
from pathlib import Path
import argparse,hashlib,json

ORIGINAL_SHA='a6b85524d8b0789f2255448271243fe6cb6f7f158bdbac3a1b1977279b8b5445'
MARKER=b'        Debug.Log($"GodsPVZ iOS export summary: result={report.summary.result}'

def instrument(data):
    assert hashlib.sha256(data).hexdigest()==ORIGINAL_SHA,'Exact unchanged original build helper required'
    assert data.count(MARKER)==1 and b'GodsPVZBuildReportCapture' not in data
    assert b'if (report.summary.result != BuildResult.Succeeded || report.summary.totalErrors != 0)' in data
    ending=b'\r\n' if b'\r\n' in data else b'\n'
    inserted=b'        GodsPVZBuildReportCapture.Record(report);'+ending
    out=data.replace(MARKER,inserted+MARKER)
    assert out.replace(inserted,b'',1)==data
    return out

def install(source,project,evidence):
    source=Path(source);project=Path(project);evidence=Path(evidence);original=source.read_bytes();new=instrument(original)
    build=project/'Assets/Editor/GodsPVZIOSBuild.cs';capture=project/'Assets/Editor/GodsPVZBuildReportCapture.cs';build.parent.mkdir(parents=True,exist_ok=True)
    if build.exists():assert build.read_bytes()==original,'Unexpected project build helper before report instrumentation'
    build.write_bytes(new);capture.write_bytes((Path(__file__).parent/'GodsPVZBuildReportCapture.cs').read_bytes());evidence.mkdir(parents=True,exist_ok=True)
    receipt=dict(scope='ONE_REPORT_OBSERVATION_ORIGINAL_GATE_BYTES_PRESERVED',original_sha256=hashlib.sha256(original).hexdigest(),instrumented_sha256=hashlib.sha256(new).hexdigest(),capture_sha256=hashlib.sha256(capture.read_bytes()).hexdigest(),old_bytes_exact_after_removing_one_inserted_call=True,errors_ignored=False,game_candidate_changed=False)
    (evidence/'build-report-instrumentation.json').write_text(json.dumps(receipt,indent=2)+'\n','utf-8');return receipt

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--source',required=True);p.add_argument('--project',required=True);p.add_argument('--evidence',required=True);a=p.parse_args();print(json.dumps(install(a.source,a.project,a.evidence)))
