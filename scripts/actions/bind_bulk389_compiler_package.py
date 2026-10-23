"""Bind a prior successful compiler run to the same-run candidate package.
This stage does not claim game behavioral qualification.
"""
from pathlib import Path
import argparse,json,hashlib,zipfile,shutil,subprocess,os
p=argparse.ArgumentParser();p.add_argument('--prior',required=True);p.add_argument('--output',required=True);a=p.parse_args()
repo=Path(__file__).resolve().parents[2];prior=Path(a.prior);out=Path(a.output);out.mkdir(parents=True,exist_ok=False)
expected='ef1992326d1ee494a389bc2c78bdfab131721434c0b08ab5f41dbc3e506c4e48'
support=repo/'Recovery/Bulk389-Qualification-2026-10-23/ios-support';manifest=json.loads((support/'MANIFEST.json').read_text())
assert manifest['source_commit']=='64b86ed725c40e8e00cecd7a917f72bbed37777e' and len(manifest['entries'])==3
for item in manifest['entries']:assert hashlib.sha256((support/item['path']).read_bytes()).hexdigest()==item['sha256']
run=json.loads(subprocess.check_output(['gh','api','repos/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39957635669']))
assert run['head_sha']=='1cc0761d6378c8d00e4d9c67e9c322eba6e52614' and run['conclusion']=='success'
r=json.loads((prior/'bulk389-linux/BULK389-COMPILER-REPORT.json').read_text());t=json.loads((prior/'bulk389-linux/results/GAME-TU-RESULTS.json').read_text())
assert r['candidate_sha256']==expected and r['il2cpp']['exit_code']==0 and len(t)==r['il2cpp']['game_tu_count']==14
assert all(x['exit_code']==x['error_count']==x['fatal_count']==0 for x in t)
with zipfile.ZipFile(repo/'Recovery/Bulk389-Qualification-2026-10-23/BULK389-R4-CANDIDATE.zip') as z:data=z.read('BULK389-R4-CANDIDATE.dll')
assert hashlib.sha256(data).hexdigest()==expected
(out/'Assembly-CSharp-bulk389.dll').write_bytes(data)
(out/'candidate-assembly-name.txt').write_text('GodsPVZRuntime1\n')
provenance=dict(status='BULK389_R4_LINUX_COMPILER_PASS',source_commit=os.environ['GITHUB_SHA'],prior_compiler_run=39957635669,prior_compiler_commit=run['head_sha'],linux_compiler=dict(exit=0,failed_game_tus=0,diagnostics=0,game_tus=14),outputs=dict(unlinked=dict(sha256=expected)),runtime_semantics='NOT_FORMALLY_QUALIFIED',device_execution='NOT_PERFORMED')
(out/'PROVENANCE.json').write_text(json.dumps(provenance,indent=2)+'\n');shutil.copy2(repo/'Recovery/Bulk389-Qualification-2026-10-23/CURRENT-RESULT.json',out/'candidate-inspection.txt');print('FIXED_COMPILER_CANDIDATE_BINDING_PASS')
