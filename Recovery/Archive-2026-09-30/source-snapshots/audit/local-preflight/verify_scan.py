from pathlib import Path
import json
R=Path(__file__).resolve().parent
def read(name):return json.loads((R/name).read_text(encoding='utf-8-sig'))
b={m['token']:m for m in read('baseline.json')['results']}
n={m['token']:m for m in read('native4.json')['results']}
control=read('valid-control.json')
assert all(not m['errors'] and not m['unsupported'] for m in control['results']), 'valid compiled control unexpectedly flagged'
assert any(e['category']=='stack_merge' for e in b['0x0600039F']['errors'])
assert any(e['category']=='nonempty_exit' for e in b['0x0600039F']['errors'])
assert any(e['category']=='local_type' and 'PTR:' in e['detail'] for e in b['0x06000267']['errors'])
tokens=['0x060001A5','0x060001B2','0x060001E0','0x06000248','0x06000267','0x0600039F']
assert all(not n[t]['errors'] and not n[t]['unsupported'] for t in tokens)
assert any(e['category']=='decompiler_placeholder' for e in n['0x060005BE']['hints'])
assert any(e['category']=='struct_primitive_operation' and 'Color' in e['detail'] for e in n['0x060006E3']['errors'])
assert any(e['category']=='struct_primitive_operation' and 'Vector3' in e['detail'] for e in n['0x06000498']['errors'])
report={'baseline_two_known_failures_reproduced':True,'native4_color_and_vector_failures_reproduced':True,'repaired_six_methods_clear':True,'valid_compiled_control_bodies':len(control['results']),'valid_control_findings':0,'remaining_native4_flagged_methods':sum(bool(m['errors']) for m in n.values()),'status':'PASS','limit':'Remaining flags require manual audit; this is not a full ECMA type verifier or a Unity/IL2CPP result.'}
(R/'scan-validation.json').write_text(json.dumps(report,indent=2),encoding='utf-8');print(json.dumps(report,indent=2))
