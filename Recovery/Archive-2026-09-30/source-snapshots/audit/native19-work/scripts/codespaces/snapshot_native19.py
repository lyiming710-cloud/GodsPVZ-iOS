"""Fail closed when archiving a method-scope checkpoint; never labels it an IPA pass."""
from pathlib import Path
import json,hashlib,subprocess
root=Path(__file__).resolve().parents[2];base=root/'.validation/native19';replay=base/'replay'
def read(p):return json.loads(p.read_text())
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
candidate=read(replay/'candidate.json');cpp=read(base/'cpp-final15/qualification.json');fixture=read(base/'fixture/result.json')
assert candidate['status']=='CONVERSION_ONLY' and candidate['outputs']==cpp['candidate']
for kind,digest in candidate['outputs'].items():
 p=replay/f'native19-{kind}.dll';assert sha(p)==digest==sha(replay/f'native19-{kind}-repeat.dll')
 checks=read(replay/f'native19-{kind}.dll.targets.typed.json');assert len(checks)==15 and all(c['status']=='PASS' and c['unreachable']==0 for c in checks)
 negative=read(replay/f'native19-{kind}.dll.targets.negative.json');assert len(negative)==8 and all(c['rejected'] for c in negative)
 matrix=read(replay/f'native19-{kind}.dll.targets.die-cfg.json');assert matrix['status']=='PASS' and matrix['cases']==7424 and len(matrix['negative_controls'])==3
for name,digest in cpp['generated_sha256'].items():assert sha(Path(candidate['cpp_root'])/name)==digest,'C++ changed since preflight: '+name
assert len(cpp['target_methods'])==15 and all(m['diagnostics']==0 for m in cpp['target_methods'])
assert fixture['status']=='PASS'
for name,digest in fixture['source_sha256'].items():assert sha(root/name)==digest,'fixture source drift: '+name
result={'status':'METHOD_SCOPE_PASS_GAME_CPP_FAIL' if cpp['status']=='FULL_GAME_CPP_FAIL' else 'METHOD_SCOPE_AND_GAME_CPP_PASS','ipa_exported':False,'candidate':candidate['outputs'],'targets':15,'normalized_non_targets':2282,'cpp_diagnostics':cpp['diagnostics'],'cpp_methods_with_diagnostics':cpp['associated_methods'],'clr_assertions':36,'die_cfg_cases_per_candidate':7424,'typed_negative_controls_per_candidate':8,'semantic_negative_controls_per_candidate':3,'base_commit':subprocess.check_output(['git','rev-parse','HEAD'],cwd=root,text=True).strip()}
(base/'checkpoint.json').write_text(json.dumps(result,indent=2));print(json.dumps(result,indent=2))
