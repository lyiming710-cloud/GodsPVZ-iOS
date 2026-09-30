import subprocess,re
from pathlib import Path
R=Path(__file__).resolve().parent
repo=R/'repository'
s=subprocess.check_output(['git','show','366f50330263853ed9d3383b9609a074a042e34a:.github/workflows/stage9-native2-real-ios-xcode-gate.yml'],cwd=repo,text=True)
s=s.replace('native2','native3').replace('branches: [stage9-local-audit-tools]','branches: [stage9-native3-codex-recovery]')
s=s.replace('Assembly-CSharp-59bb-native3.dll','Assembly-CSharp-native3-four-method.dll')
replacements={
 'TEST_DLL_SHA256':'72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc',
 'TEST_CANDIDATE_RUN_ID':'36213547473',
 'TEST_CANDIDATE_ARTIFACT_ID':'10896577104',
 'TEST_CANDIDATE_ARTIFACT_SHA256':'c531edfb29e2348dfddb3df257cbee8a61417082758398980d32c25e0645b630',
}
for k,v in replacements.items():s=re.sub(r"(?m)^      "+k+r": '[^']+'$",f"      {k}: '{v}'",s)
s=re.sub(r'(?m)^      SEMANTIC_GATE_[^\n]+\n','',s)
s=re.sub(r'(?m)^          semantic_gate_(run|artifact_id|artifact_sha256)=.*\n','',s)
s=s.replace("      TEST_CANDIDATE_RUN_ID:","      TEST_CANDIDATE_HEAD: 'bf1c953eafeaf11bc29d9bca66ae4975d0e58779'\n      TEST_CANDIDATE_RUN_ID:")
preflight='''import os,json,hashlib,urllib.request,urllib.error,zipfile,io,shutil
from pathlib import Path
E=os.environ
out=Path(E['RUNNER_TEMP'])/'stage9-native3-ios'
base='https://api.github.com/repos/'+E['GITHUB_REPOSITORY']+'/'
class NoRedirect(urllib.request.HTTPRedirectHandler):
    def redirect_request(self,*args,**kwargs): return None
def api(path):
    req=urllib.request.Request(base+path,headers={'Authorization':'Bearer '+E['GH_TOKEN'],'Accept':'application/vnd.github+json','User-Agent':'native3-real-gate'})
    try:
        with urllib.request.build_opener(NoRedirect).open(req,timeout=90) as r: return r.read()
    except urllib.error.HTTPError as e:
        if e.code!=302: raise
        with urllib.request.urlopen(e.headers['Location'],timeout=90) as r: return r.read()
run=json.loads(api('actions/runs/'+E['TEST_CANDIDATE_RUN_ID']))
assert run['status']=='completed' and run['conclusion']=='success'
assert run['head_sha']==E['TEST_CANDIDATE_HEAD']
assert run['path']=='.github/workflows/stage9-native3-four-method-static-gate.yml'
meta=json.loads(api('actions/artifacts/'+E['TEST_CANDIDATE_ARTIFACT_ID']))
assert str(meta['workflow_run']['id'])==E['TEST_CANDIDATE_RUN_ID']
assert meta['workflow_run']['head_sha']==E['TEST_CANDIDATE_HEAD'] and not meta['expired']
assert meta['name']=='Stage9.1-native3-four-method-qualified'
b=api('actions/artifacts/'+E['TEST_CANDIDATE_ARTIFACT_ID']+'/zip')
assert hashlib.sha256(b).hexdigest()==E['TEST_CANDIDATE_ARTIFACT_SHA256']
qualified=out/'qualified';qualified.mkdir()
with zipfile.ZipFile(io.BytesIO(b)) as z:
    for n in z.namelist():
        target=(qualified/n).resolve()
        assert target.is_relative_to(qualified.resolve())
    z.extractall(qualified)
cand=qualified/'candidate/Assembly-CSharp-native3-four-method.dll'
assert hashlib.sha256(cand.read_bytes()).hexdigest()==E['TEST_DLL_SHA256']
summary=dict(x.split('=',1) for x in (qualified/'evidence/GATE-SUMMARY.txt').read_text().splitlines())
expected={'baseline_sha256':'18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd','candidate_sha256':E['TEST_DLL_SHA256'],'scope':'060001A5,060001B2,060001E0,06000248','static_gate':'PASS','semantic_targets':'24/24','semantic_resolve_keys':'23/23','non_target_semantic_diffs':'0','orphan_generics':'0','target_stack_verification':'4/4','negative_stack_controls':'4/4','clr_harness_assertions':'31'}
for k,v in expected.items(): assert summary[k]==v,(k,summary.get(k))
for name in ('spec-stack.log','reopened-stack.log'):
    d=json.loads((qualified/'evidence'/name).read_text(encoding='utf-8-sig'))
    assert len(d['results'])==4 and all(x['status']=='PASS' and x['checked']==x['instructions'] for x in d['results'])
    assert len(d['negative_controls'])==4 and all(x['rejected'] for x in d['negative_controls'])
lines=(qualified/'evidence/semantic-gate.log').read_text().splitlines()
for marker in ('METHODDEF=2317/2317','FIELDDEF=2802/2802','TYPEDEF=320/320','METHOD_IDENTITY_DRIFT=0/0','FIELD_IDENTITY_DRIFT=0/0','TYPE_IDENTITY_DRIFT=0/0','CHANGED_METHODS=4/4','NON_TARGET_METHOD_SEMANTIC_DIFFS=0/0','PRESERVED_060001DE=1/1','PRESERVED_0600017F=1/1','ORPHAN_GENERIC_HITS=0/0','SEMANTIC_TARGETS_ACCOUNTED=24/24','SEMANTIC_RESOLVE_KEYS=23/23','TRANSFORM_REPAIR_SITES=3/3','TARGET_UNRESOLVED_REFS=0/0','NATIVE3_LOCAL_SEMANTIC_GATE_PASS'):
    assert marker in lines,marker
assert (qualified/'evidence/spec-readback.log').read_text().strip()=='SPEC_READBACK=4/4; ALL_REFERENCE_USE_DELTAS=EXACT_MATCH'
assert 'NATIVE3_CLR_HARNESS_PASS assertions=31' in (qualified/'evidence/clr-harness.log').read_text().splitlines()
shutil.copy2(cand,out/'candidate'/cand.name)
shutil.copytree(qualified/'evidence',out/'evidence/static-source')
(out/'evidence/candidate-provenance.json').write_text(json.dumps({'run_id':run['id'],'head_sha':run['head_sha'],'artifact_id':meta['id'],'artifact_zip_sha256':E['TEST_CANDIDATE_ARTIFACT_SHA256'],'candidate_sha256':E['TEST_DLL_SHA256'],'static_gate':'PASS'},indent=2))
print('NATIVE3_QUALIFIED_INPUT_PREFLIGHT_PASS')
'''
block='''      - name: Verify exact four-method candidate and complete static evidence
        shell: bash
        env:
          GH_TOKEN: ${{ github.token }}
        run: |
          set -euo pipefail
          python3 - <<'PY'
'''+''.join('          '+l+'\n' for l in preflight.splitlines())+'''          PY

'''
start=s.index('      - name: Download static-qualified')
end=s.index('      - name: Download exact cached R3 archive')
s=s[:start]+block+s[end:]
dest=repo/'.github/workflows/stage9-native3-real-ios-xcode-gate.yml'
dest.write_text(s,encoding='utf-8')
(R/'real_preflight.py').write_text(preflight,encoding='utf-8')
print(dest)
