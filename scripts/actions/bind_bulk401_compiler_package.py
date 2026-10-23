"""Bind the exact new candidate to both completed compiler gates and audits."""
from pathlib import Path
import argparse,json,hashlib,zipfile,shutil,subprocess,os
p=argparse.ArgumentParser();p.add_argument('--output',required=True);a=p.parse_args()
repo=Path(__file__).resolve().parents[2];root=repo/'Recovery/Bulk401-Generic-Repair-2026-10-24';out=Path(a.output)
expected='6de4db6c94b80a43876aea060b573ed60c1fa7170cff485103d2c808b94762ae'
linux=json.loads((root/'compiler-40012442171/BULK401-COMPILER-REPORT.json').read_text())
apple=json.loads((root/'apple-40014022885/APPLE-REPORT.json').read_text())
readback=json.loads((root/'COMPILER-READBACK.json').read_text());ar=json.loads((root/'APPLE-READBACK.json').read_text())
for r in [linux,apple,readback,ar]:assert r['candidate_sha256']==expected
assert linux['pass'] and linux['cpp']['attempted']==756 and linux['cpp']['failed']==linux['c']['failed']==0
assert apple['attempted']==1030 and apple['failed']==apple['diagnostics']==apple['unparsed_error_lines']==0
assert readback['cpp']==756 and readback['c']==274 and ar['logs_checked']==1030
for run_id,head in [(40012442171,'7e135883bd2781d056e7dd0d3050420831243b85'),(40014022885,'e2c3f98215718951358de8963a7e7d64bde1f2c6')]:
    run=json.loads(subprocess.check_output(['gh','api',f'repos/lyiming710-cloud/GodsPVZ-iOS/actions/runs/{run_id}']))
    assert run['head_sha']==head and run['conclusion']=='success' and run['status']=='completed'
isolation=json.loads((root/'ISOLATION.json').read_text());runtime=json.loads((root/'ACTUAL-CLR-NATIVE-ROWS.json').read_text())
native=json.loads((root/'ORIGINAL-NATIVE-SUMMARY.json').read_text());controls=json.loads((root/'ACTUAL-CLR-CONTROLS-FINAL.json').read_text())
for r in [isolation,runtime,native]:assert r['candidate_sha256']==expected
assert isolation['unchanged_non_target_methods']==1916 and isolation['inherited_389_raw_bodies_identical']
assert runtime['pass'] and runtime['assertions_executed']==269 and controls['all_matched']
assert native['cases']==139 and native['methods']==12 and native['differences']==0
with zipfile.ZipFile(root/'BULK401-CANDIDATE.zip') as z:data=z.read('BULK401-CANDIDATE.dll')
assert hashlib.sha256(data).hexdigest()==expected
out.mkdir(parents=True,exist_ok=False);(out/'Assembly-CSharp-bulk401.dll').write_bytes(data)
(out/'candidate-assembly-name.txt').write_text('GodsPVZRuntime1\n')
provenance=dict(status='BULK401_LINUX_AND_APPLE_CPP_PRECHECK_PASS',source_commit=os.environ['GITHUB_SHA'],
    linux_compiler=dict(exit=0,failed_game_tus=0,diagnostics=0,all_cpp=756,all_c=274),
    apple_compiler=dict(run_id=40014022885,all_generated=1030,failed=0,diagnostics=0),
    outputs=dict(unlinked=dict(sha256=expected)),native_controlled_cases=139,
    runtime_semantics='CONTROLLED_SCOPED_NOT_FORMALLY_QUALIFIED',device_execution='NOT_PERFORMED')
(out/'PROVENANCE.json').write_text(json.dumps(provenance,indent=2)+'\n')
shutil.copy2(root/'CURRENT-RESULT.json',out/'candidate-inspection.txt')
print('BULK401_FIXED_COMPILER_CANDIDATE_BINDING_PASS')
