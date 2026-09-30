from pathlib import Path
import os,json,subprocess,sys,collections
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');d=Path('/tmp/godspvz-native24-codespace-2026-10-01');support=old/'.validation/native23-independent-full-review/baseline/managed'
exporter=old/'.validation/native22/bin/PatcherNative19.dll'
assert exporter.is_file(),str(exporter)
sys.path.insert(0,str(w/'scripts/codespaces'));import verify_native19_types as V
result={'scope':'Existing fail-closed typed instruction-subset verifier; E1 only, not full ECMA or game semantics','sets':{}}
for tag,dll in [('baseline',d/'inputs/baseline.dll'),('raw',d/'raw-1.dll')]:
 output=d/(tag+'-typed-export.json')
 p=subprocess.run(['dotnet',str(exporter),str(dll),str(output),'export-all'],env=dict(os.environ,GODSPVZ_RESOLVER=str(support)),capture_output=True,text=True)
 assert p.returncode==0,p.stdout+p.stderr
 data=json.loads(output.read_text());rows=[]
 for method in data['methods']:
  try:row=V.verify(method,data['types'])
  except V.InvalidIL as e:row={'method':method['name'],'token':method['token'],'status':'FAIL','error':str(e)}
  rows.append(row)
 (d/(tag+'-TYPED-RESULTS.json')).write_text(json.dumps(rows,indent=2));result['sets'][tag]={'count':len(rows),'pass':sum(r['status']=='PASS' for r in rows),'fail':sum(r['status']=='FAIL' for r in rows),'targets':[r for r in rows if r['token'] in ['0x06000339','0x06000348','0x0600034C']]}
b={r['token']:r for r in json.loads((d/'baseline-TYPED-RESULTS.json').read_text())};a={r['token']:r for r in json.loads((d/'raw-TYPED-RESULTS.json').read_text())}
result['pass_to_fail']=[t for t in b if b[t]['status']=='PASS' and a[t]['status']=='FAIL'];result['fail_to_pass']=[t for t in b if b[t]['status']=='FAIL' and a[t]['status']=='PASS']
assert not result['pass_to_fail'];assert len(result['fail_to_pass'])==3
result['generated_file_differences']=json.loads((d/'RAW-CONVERSION.json').read_text())['differing_files_from_cecil_backend']
(d/'TYPED-STATUS.json').write_text(json.dumps(result,indent=2));print(json.dumps(result,indent=2))
