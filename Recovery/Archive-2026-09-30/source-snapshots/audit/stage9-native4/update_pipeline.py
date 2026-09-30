from pathlib import Path
import json
R=Path(__file__).resolve().parent
p=R/'build-and-verify.ps1';s=p.read_text()
s=s.replace('native3-four-method','native4-six-method').replace('NATIVE3_PYTHON','NATIVE4_PYTHON').replace('72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc','11c1a9648ea67cb0ac66b84ddde50cb4ef46d24152f67515a99afb526a2d4b4e')
s=s.replace('evidence/reference-use-deltas.tsv >','evidence/reference-use-deltas.tsv inputs/native3-prior.dll >')
s=s.replace('scope=060001A5,060001B2,060001E0,06000248','scope=060001A5,060001B2,060001E0,06000248,06000267,0600039F')
s=s.replace('target_stack_verification=4/4','target_stack_verification=6/6').replace('negative_stack_controls=4/4','negative_stack_controls=6/6').replace('clr_harness_assertions=31','clr_harness_assertions=74').replace('NATIVE3_FOUR_METHOD','NATIVE4_SIX_METHOD')
s=s.replace("'static_gate=PASS'","'static_gate=PASS','preserved_native3_methods=4/4'")
p.write_text(s)
p=R/'Gate.cs';p.write_text(p.read_text().replace('NATIVE3_LOCAL','NATIVE4_LOCAL'))
p=R/'input-locks.json';d=json.loads(p.read_text());d['candidate_sha256']='11c1a9648ea67cb0ac66b84ddde50cb4ef46d24152f67515a99afb526a2d4b4e';d['prior_candidate_sha256']='72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc';p.write_text(json.dumps(d,indent=2)+'\n')
p=R/'prepare_ci_inputs.py';s=p.read_text().replace('GodsPVZ-native3-static-gate','GodsPVZ-native4-static-gate')
needle=" # NuGet integrity pinned to the package used locally."
insert=""" z=artifact(10896577104,'native3-prior.zip','c531edfb29e2348dfddb3df257cbee8a61417082758398980d32c25e0645b630');extract_sha(z,'72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc',R/'inputs/native3-prior.dll')
 module=R/'ReferenceAssemblies/UnityEngine.AnimationModule.dll'
 proof=json.loads((R/'native-evidence/animation-resolver-provenance.json').read_text())
 b=module.read_bytes();assert hashlib.sha256(b).hexdigest()==proof['module_sha256']
 (R/'resolver'/module.name).write_bytes(b)
"""
s=s.replace(needle,insert+needle);p.write_text(s)
