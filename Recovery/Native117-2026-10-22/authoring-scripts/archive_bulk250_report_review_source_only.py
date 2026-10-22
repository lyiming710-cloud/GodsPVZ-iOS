"""Preserve and describe existing AGY reports. No verifier, candidate or test run."""
from pathlib import Path
import json,hashlib,shutil
R=Path(__file__).resolve().parents[1]
S=R/'audit/bulk250-evidence-39662784115'
E=R/'audit/native116-bulk250-r1-codex-2026-10-22'
O=E/'prior-run-39662784115';O.mkdir(exist_ok=True)
files=['bulk250-run/results/construction1.json','bulk250-run/results/construction2.json','bulk250-run/results/BASELINE-CANDIDATE-COMPARISON.json','bulk250-run/results/INDEPENDENT-ISOLATION-REPORT.json','bulk250-run/results/static-invocation-04.stdout.log','bulk250-run/results/static-invocation-04.stderr.log','bulk250-run/results/static-invocation-04.exitcode.txt','bulk250-restore-receipt.json']
manifest=[]
for f in files:
    p=S/f;q=O/p.name;shutil.copyfile(p,q)
    manifest.append(dict(path=f,archived_as=q.relative_to(E).as_posix(),bytes=p.stat().st_size,sha256=hashlib.sha256(p.read_bytes()).hexdigest()))
fails=[]
for l in (S/'bulk250-run/results/static-invocation-04.stdout.log').read_text('utf-8-sig').splitlines():
    if l.startswith('{'):
        try:x=json.loads(l)
        except ValueError:continue
        if x.get('status')=='FAIL':fails.append(x)
body=dict(schema='previous50-report-review-v1',scope='READ_EXISTING_REPORTS_ONLY_NOT_REEXECUTION',run_id=39662784115,run_url='https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39662784115',reviewed_by_primary=True,previous_batch_acceptance='FAIL',reported_candidate_sha256='ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8',recorded_type_flow_failures=fails,additional_member_access_failure={'token':'0x060003F4','basis':'AGY final msg_8ac420d61d4e403fbdd281203e2155f9','saved_gate_invocation_and_output':'REQUEST_FULL_ARCHIVE_FROM_AGY'},delivery_inconsistency={'erroneous_receipt_candidate_sha256':'8ef0f2bc62658bd7a4bdeb2cb25a10cadaa9f5418e8f97910f7ac1b9604823b1','construction_and_comparison_candidate_sha256':'ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8','status':'RECONCILE_RECEIPT_ARCHIVE_MANIFEST_CANDIDATE_IDS'},bulk250_r1='SOURCE_REPAIRS_UNTESTED',bulk300_testing='HOLD_UNTIL_PREVIOUS_BATCH_ACCEPTANCE_REVIEW',new_formal_acceptance=0,files=manifest)
(E/'PRIOR-BATCH-RESULT-REVIEW.json').write_text(json.dumps(body,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
