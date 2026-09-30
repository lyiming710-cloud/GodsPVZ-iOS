set -euo pipefail
python3 - <<'PY'
import os,json,hashlib,urllib.request,urllib.error,zipfile,io,shutil
from pathlib import Path
E=os.environ
out=Path(E['RUNNER_TEMP'])/'stage9-native4-ios'
base='https://api.github.com/repos/'+E['GITHUB_REPOSITORY']+'/'
class NoRedirect(urllib.request.HTTPRedirectHandler):
    def redirect_request(self,*args,**kwargs): return None
def api(path):
    req=urllib.request.Request(base+path,headers={'Authorization':'Bearer '+E['GH_TOKEN'],'Accept':'application/vnd.github+json','User-Agent':'native4-real-gate'})
    try:
        with urllib.request.build_opener(NoRedirect).open(req,timeout=90) as r: return r.read()
    except urllib.error.HTTPError as e:
        if e.code!=302: raise
        with urllib.request.urlopen(e.headers['Location'],timeout=90) as r: return r.read()
run=json.loads(api('actions/runs/'+E['TEST_CANDIDATE_RUN_ID']))
assert run['status']=='completed' and run['conclusion']=='success'
assert run['head_sha']==E['TEST_CANDIDATE_HEAD']
assert run['path']=='.github/workflows/stage9-native4-six-method-static-gate.yml'
meta=json.loads(api('actions/artifacts/'+E['TEST_CANDIDATE_ARTIFACT_ID']))
assert str(meta['workflow_run']['id'])==E['TEST_CANDIDATE_RUN_ID']
assert meta['workflow_run']['head_sha']==E['TEST_CANDIDATE_HEAD'] and not meta['expired']
assert meta['name']=='Stage9.1-native4-six-method-qualified'
b=api('actions/artifacts/'+E['TEST_CANDIDATE_ARTIFACT_ID']+'/zip')
assert hashlib.sha256(b).hexdigest()==E['TEST_CANDIDATE_ARTIFACT_SHA256']
qualified=out/'qualified';qualified.mkdir()
with zipfile.ZipFile(io.BytesIO(b)) as z:
    for n in z.namelist():
        target=(qualified/n).resolve()
        assert target.is_relative_to(qualified.resolve())
    z.extractall(qualified)
cand=qualified/'candidate/Assembly-CSharp-native4-six-method.dll'
assert hashlib.sha256(cand.read_bytes()).hexdigest()==E['TEST_DLL_SHA256']
summary=dict(x.split('=',1) for x in (qualified/'evidence/GATE-SUMMARY.txt').read_text().splitlines())
expected={'baseline_sha256':'18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd','candidate_sha256':E['TEST_DLL_SHA256'],'scope':'060001A5,060001B2,060001E0,06000248,06000267,0600039F','static_gate':'PASS','semantic_targets':'24/24','semantic_resolve_keys':'23/23','non_target_semantic_diffs':'0','orphan_generics':'0','target_stack_verification':'6/6','negative_stack_controls':'6/6','clr_harness_assertions':'74','preserved_native3_methods':'4/4'}
for k,v in expected.items(): assert summary[k]==v,(k,summary.get(k))
for name in ('spec-stack.log','reopened-stack.log'):
    d=json.loads((qualified/'evidence'/name).read_text(encoding='utf-8-sig'))
    assert len(d['results'])==6 and all(x['status']=='PASS' and x['checked']==x['instructions'] for x in d['results'])
    assert len(d['negative_controls'])==6 and all(x['rejected'] for x in d['negative_controls'])
lines=(qualified/'evidence/semantic-gate.log').read_text().splitlines()
for marker in ('METHODDEF=2317/2317','FIELDDEF=2802/2802','TYPEDEF=320/320','METHOD_IDENTITY_DRIFT=0/0','FIELD_IDENTITY_DRIFT=0/0','TYPE_IDENTITY_DRIFT=0/0','CHANGED_METHODS=6/6','NON_TARGET_METHOD_SEMANTIC_DIFFS=0/0','PRESERVED_060001DE=1/1','PRESERVED_0600017F=1/1','ORPHAN_GENERIC_HITS=0/0','SEMANTIC_TARGETS_ACCOUNTED=24/24','SEMANTIC_RESOLVE_KEYS=23/23','TRANSFORM_REPAIR_SITES=3/3','TARGET_UNRESOLVED_REFS=0/0','PRESERVED_NATIVE3_060001A5=1/1','PRESERVED_NATIVE3_060001B2=1/1','PRESERVED_NATIVE3_060001E0=1/1','PRESERVED_NATIVE3_06000248=1/1','NATIVE4_LOCAL_SEMANTIC_GATE_PASS'):
    assert marker in lines,marker
assert (qualified/'evidence/spec-readback.log').read_text().strip()=='SPEC_READBACK=6/6; ALL_REFERENCE_USE_DELTAS=EXACT_MATCH'
assert 'NATIVE4_CLR_HARNESS_PASS assertions=74' in (qualified/'evidence/clr-harness.log').read_text().splitlines()
shutil.copy2(cand,out/'candidate'/cand.name)
shutil.copytree(qualified/'evidence',out/'evidence/static-source')
(out/'evidence/candidate-provenance.json').write_text(json.dumps({'run_id':run['id'],'head_sha':run['head_sha'],'artifact_id':meta['id'],'artifact_zip_sha256':E['TEST_CANDIDATE_ARTIFACT_SHA256'],'candidate_sha256':E['TEST_DLL_SHA256'],'static_gate':'PASS'},indent=2))
print('NATIVE4_QUALIFIED_INPUT_PREFLIGHT_PASS')
PY

