from pathlib import Path
import subprocess,os,json,sys,hashlib,re
d=Path('/tmp/godspvz-native24-codespace-2026-10-01');old=Path('/workspaces/GodsPVZ-native19');w=Path('/workspaces/GodsPVZ-native24-codespace');report=json.loads((d/'raw-1-report.json').read_text());original=(d/'inputs/baseline.dll').read_bytes();candidate=(d/'raw-1.dll').read_bytes()
sys.path.insert(0,str(w/'scripts/codespaces'));import verify_native19_types as V
controls=d/'site-controls';controls.mkdir(exist_ok=True);results=[]
for n,site in enumerate(report['rows']):
 mutated=bytearray(candidate);at=site['fileOffset'];size=len(bytes.fromhex(site['beforeBytes']));mutated[at:at+size]=original[at:at+size];dll=controls/(str(n)+'.dll');dll.write_bytes(mutated);out=controls/(str(n)+'-all.json')
 r=subprocess.run(['dotnet',str(old/'.validation/native22/bin/PatcherNative19.dll'),str(dll),str(out),'export-all'],env=dict(os.environ,GODSPVZ_RESOLVER=str(old/'.validation/native23-independent-full-review/baseline/managed')),capture_output=True,text=True);assert r.returncode==0,r.stdout+r.stderr
 data=json.loads(out.read_text());method=next(m for m in data['methods'] if m['token']==site['token'])
 try:V.verify(method,data['types']);raise AssertionError('Site rollback undetected')
 except V.InvalidIL as e:error=str(e)
 (controls/(str(n)+'-target.json')).write_text(json.dumps({'method':method,'types':data['types']},indent=2));results.append({'site':site,'sha256':hashlib.sha256(mutated).hexdigest(),'detected':True,'error':error});out.unlink()
a=(d/'fresh-candidate/cpp/GodsPVZRuntime1__6.cpp').read_text();b=(d/'fresh-raw/cpp/GodsPVZRuntime1__6.cpp').read_text();assert re.sub(r'\bIL_[0-9a-f]+\b','IL_LABEL',a)==re.sub(r'\bIL_[0-9a-f]+\b','IL_LABEL',b)
data={'six_site_rollbacks_detected':len(results),'results':results,'generated_cpp_only_IL_label_offsets_differ':True,'scope':'Rollback typed controls are E1; CLR behavioral mutants are recorded separately'};(d/'RAW-SITE-CONTROLS.json').write_text(json.dumps(data,indent=2));print(json.dumps(data,indent=2))
