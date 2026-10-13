from pathlib import Path
import hashlib,json,zipfile,io,ast,base64
p=Path('audit/native60-integrator-acceptance-2026-10-13')
raw=Path('audit/native60-gemini-tests-2026-10-12/Archive-v6.zip').read_bytes()
h=hashlib.sha256(raw).hexdigest()
if h!='0b21024f659fdab0caca461b48bde50975af2cfd71c95e8da6ee594466e4fb95':raise RuntimeError('Evidence changed before freezing; review new version')
dest=p/'Native60-reviewed-evidence-0b21024f.zip'
if dest.exists() and dest.read_bytes()!=raw:raise RuntimeError('Frozen evidence differs')
dest.write_bytes(raw)
z=zipfile.ZipFile(io.BytesIO(raw))
items=['binaries/native60-oracle','sources/native60_oracle.cpp','sources/Native60Fixture.cs','evidence/candidate_repro.dll','evidence/NATIVE-ORACLE.tsv','reports/strict-result-v3.json','reports/ISOLATION_REPORT.json','reports/TYPED-VERIFIER-REPORT.json','reports/MEMBER-ACCESS.json','reports/FIXTURE_REPORT.json','reports/HISTORICAL_35_REGRESSIONS_REPORT.json','runners/master_n60_v3.py','logs/master_n60_v3.log','logs/fixture_v3.stdout','logs/fixture_v3.stderr','docs/EXTERNAL-DEPENDENCIES.md']
members={n:{'bytes':len(z.read(n)),'sha256':hashlib.sha256(z.read(n)).hexdigest()}for n in items}
tree=ast.parse(z.read('runners/master_n60_v3.py').decode())
embedded=[]
for n in ast.walk(tree):
 if isinstance(n,ast.Call) and isinstance(n.func,ast.Attribute) and n.func.attr=='b64decode' and len(n.args)==1 and isinstance(n.args[0],ast.Constant) and isinstance(n.args[0].value,str):
  b=base64.b64decode(n.args[0].value);embedded.append({'runner_line':n.lineno,'bytes':len(b),'sha256':hashlib.sha256(b).hexdigest()})
receipt={'status':'APPROVED_PREBUILD_SCOPE','review_date':'2026-10-13','accepted_batch':60,'qualified_unique_methods_since_native53':42,'pending_unique_implemented_methods_through_native82':70,'accepted_plus_pending_implementation_count':112,'parent_sha256':'d198bb3f63a21d95d708a66546b3b12529d52aee810508712db6e32f43b021c0','candidate_sha256':'4e1b743d87db3d78816b37e0a114b105e59b0f25fcbe6cb90e0797378d188b35','targets':['0x0600046D','0x060005B9'],'evidence_archive':{'name':dest.name,'bytes':len(raw),'sha256':h,'members':len(z.namelist())},'reviewed_members':members,'embedded_source_identities':embedded,'test_executor_thread':'01a12e9a-49e8-71c1-acf0-a634ac5215af','test_executor':'AGY Gemini 3.8 Flash','integrator_acceptance_tests_executed':False,'full_IL2CPP_conversion_passed':False,'all_TU_gate_passed':False,'ipa_produced':False}
(p/'ACCEPTANCE.json').write_text(json.dumps(receipt,indent=2))
print(json.dumps({'frozen_archive_sha256':h,'accepted_candidate':receipt['candidate_sha256'],'qualified':42,'root_tests_executed':False},indent=2))
