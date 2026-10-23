from pathlib import Path
import subprocess,base64,hashlib,json
Q=Path(__file__).resolve().parent;R=Q.parents[1];repo=R/'audit/repo-native113';D=repo/'Recovery/Bulk389-Qualification-2026-10-23';ci=D/'ci';ci.mkdir(exist_ok=False)
gh=r'C:\Program Files\GitHub CLI\gh.exe';endpoint='repos/lyiming710-cloud/GodsPVZ-iOS/contents/Recovery/Native119-Bulk350-Root-Check-2026-10-22/ci/UnityEngine.AnimationModule.dll?ref=94eaf50d5d28d1b6acc655fe9d4e36d0a3a47cb0'
module=base64.b64decode(subprocess.check_output([gh,'api',endpoint,'--jq','.content']));assert hashlib.sha256(module).hexdigest()=='324d1e5ceef2b6fa1d0e3473ce820b451e41f397458afc46708a27aae0aef5e2';(ci/'UnityEngine.AnimationModule.dll').write_bytes(module)
t=(Q/'previous-check-compiler.py').read_text('utf-8').replace('ef95567ce714146b2f7aed1a92e98798023090bb47757f6c5b377f9ce6d9c0f4','9c8cefd7b348a254a7c41ec48d95d9c5a1047eb6e17ad6eb45eb03b7351ce1c1').replace('BULK350_R6','BULK389_R3').replace('BULK350-COMPILER-REPORT','BULK389-COMPILER-REPORT')
script=repo/'scripts/actions/check_bulk389_compiler.py';assert not script.exists();script.write_text(t,'utf-8')
workflow=(Q/'previous-compiler-workflow.yml').read_text('utf-8')
workflow=workflow.replace('Bulk350 R6 fixed-candidate','Bulk389 R3 fixed-candidate').replace('repair/native119-bulk350-root-check-2026-10-22','verify/codex-bulk250-r2-takeover-2026-10-23').replace('bulk350-root-compiler-check.yml','bulk389-root-compiler-check.yml').replace('check_bulk350_compiler.py','check_bulk389_compiler.py').replace('bulk350-r6-root-compiler','bulk389-r3-root-compiler')
workflow=workflow.replace('gh release download native119-bulk350-root-check-2026-10-22 --repo "$GITHUB_REPOSITORY" --pattern BULK350-R6-CANDIDATE.dll --dir "$RUNNER_TEMP/downloads"','python3 -m zipfile -e Recovery/Bulk389-Qualification-2026-10-23/BULK389-R3-CANDIDATE.zip "$RUNNER_TEMP/downloads"')
workflow=workflow.replace('ef95567ce714146b2f7aed1a92e98798023090bb47757f6c5b377f9ce6d9c0f4','9c8cefd7b348a254a7c41ec48d95d9c5a1047eb6e17ad6eb45eb03b7351ce1c1').replace('BULK350-R6-CANDIDATE.dll','BULK389-R3-CANDIDATE.dll').replace('Recovery/Native119-Bulk350-Root-Check-2026-10-22/ci/','Recovery/Bulk389-Qualification-2026-10-23/ci/').replace('bulk350-restore','bulk389-restore').replace('bulk350-linux','bulk389-linux').replace('bulk350-generated','bulk389-generated').replace('Bulk350-R5-compiler','Bulk389-R3-compiler')
path=repo/'.github/workflows/bulk389-root-compiler-check.yml';assert not path.exists();path.write_text(workflow,'utf-8')
print('CI_PREPARED not dispatched, frozen portable compiler engine unchanged')
