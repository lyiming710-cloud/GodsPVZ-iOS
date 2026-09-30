from pathlib import Path
import json,re
R=Path(__file__).resolve().parent;repo=R.parent/'stage9-native3/repository'
run=json.loads((R/'evidence/static-run.json').read_text());assert run['conclusion']=='success' and run['status']=='completed'
artifact=json.loads((R/'evidence/static-artifacts.json').read_text())['artifacts'];assert len(artifact)==1
a=artifact[0];assert a['name']=='Stage9.1-native4-six-method-qualified' and a['workflow_run']['id']==run['id']
s=(repo/'.github/workflows/stage9-native3-real-ios-xcode-gate.yml').read_text()
s=s.replace('native3','native4').replace('NATIVE3','NATIVE4').replace('four-method','six-method').replace('four methods','six methods')
s=s.replace('branches: [stage9-native4-codex-recovery]','branches: [stage9-native3-codex-recovery]')
for key,value in {'TEST_DLL_SHA256':'11c1a9648ea67cb0ac66b84ddde50cb4ef46d24152f67515a99afb526a2d4b4e','TEST_CANDIDATE_HEAD':run['head_sha'],'TEST_CANDIDATE_RUN_ID':str(run['id']),'TEST_CANDIDATE_ARTIFACT_ID':str(a['id']),'TEST_CANDIDATE_ARTIFACT_SHA256':a['digest'].split(':')[1]}.items():
 s=re.sub(r"(?m)^      "+key+r": '[^']+'$",f"      {key}: '{value}'",s)
s=s.replace("'scope':'060001A5,060001B2,060001E0,06000248'","'scope':'060001A5,060001B2,060001E0,06000248,06000267,0600039F'")
s=s.replace("'target_stack_verification':'4/4'","'target_stack_verification':'6/6'").replace("'negative_stack_controls':'4/4'","'negative_stack_controls':'6/6'").replace("'clr_harness_assertions':'31'","'clr_harness_assertions':'74','preserved_native3_methods':'4/4'")
s=s.replace("len(d['results'])==4","len(d['results'])==6").replace("len(d['negative_controls'])==4","len(d['negative_controls'])==6").replace('CHANGED_METHODS=4/4','CHANGED_METHODS=6/6').replace('SPEC_READBACK=4/4','SPEC_READBACK=6/6').replace('assertions=31','assertions=74')
s=s.replace("'TARGET_UNRESOLVED_REFS=0/0'","'TARGET_UNRESOLVED_REFS=0/0','PRESERVED_NATIVE3_060001A5=1/1','PRESERVED_NATIVE3_060001B2=1/1','PRESERVED_NATIVE3_060001E0=1/1','PRESERVED_NATIVE3_06000248=1/1'")
(repo/'.github/workflows/stage9-native4-real-ios-xcode-gate.yml').write_text(s)
pre=re.search(r"          python3 - <<'PY'\n(.*?)          PY\n",s,re.S).group(1)
(R/'real_preflight.py').write_text('\n'.join(line[10:] for line in pre.splitlines())+'\n')
print('workflow bound to',run['id'],a['id'],run['head_sha'])
