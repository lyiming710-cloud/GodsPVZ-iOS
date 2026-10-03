from pathlib import Path
import json,hashlib,zipfile,subprocess,sys,os,struct
archive=Path(sys.argv[1]);root=Path(sys.argv[2]);assert not root.exists();root.mkdir(parents=True)
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
with zipfile.ZipFile(archive) as z:
 manifest=json.loads(z.read('MANIFEST.json'));assert set(z.namelist())==set(manifest)|{'MANIFEST.json'}
 for name,row in manifest.items():
  p=(root/name).resolve();assert root.resolve()in p.parents and not Path(name).is_absolute()
  data=z.read(name);assert len(data)==row['bytes']and hashlib.sha256(data).hexdigest()==row['sha256'];p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(data)
src=root/'sources/takeover/native56';work=root/'work';out=root/'replay';out.mkdir();support=root/'support/managed';cecil=root/'support/Mono.Cecil.dll';core=support/'UnityEngine.CoreModule.dll'
original=(root/'original/GameAssembly.dll').read_bytes();nt=struct.unpack_from('<I',original,60)[0];base=struct.unpack_from('<Q',original,nt+48)[0];ns=struct.unpack_from('<H',original,nt+6)[0];opt=struct.unpack_from('<H',original,nt+20)[0]
slot=0x181BBAB00;rva=slot-base;hits=[]
for i in range(ns):
 sh=nt+24+opt+40*i;start,size,raw=struct.unpack_from('<III',original,sh+12)
 if start<=rva and rva+8<=start+size:hits.append(raw+rva-start)
assert len(hits)==1;enc=struct.unpack_from('<Q',original,hits[0])[0];assert (enc>>29)&7==5 and enc&1;idx=(enc&0x1FFFFFFF)>>1;meta=(root/'original/global-metadata.dat').read_bytes();tab,tab_bytes,text,text_bytes=struct.unpack_from('<IIII',meta,8);assert idx<tab_bytes//8;size,offset=struct.unpack_from('<II',meta,tab+8*idx);assert offset+size<=text_bytes;literal=meta[text+offset:text+offset+size].decode();assert literal=='isAttacking';proof=json.loads((work/'ORIGINAL-ANIMATOR-LITERAL.json').read_text());assert proof['value']==literal and proof['encoded']==hex(enc) and proof['index']==idx
def run(args,name,expected=0,env=None):
 p=subprocess.run(args,capture_output=True,text=True,timeout=90,env=env);(out/(name+'.log')).write_text(p.stdout+p.stderr);assert p.returncode==expected,(name,p.returncode,p.stdout+p.stderr)
for project in ['Patcher','Audit','RuntimeFixture','MemberAccess','Mutate','Read']:
 run(['dotnet','build',str(src/project/(project+'.csproj')),'-c','Release','-o',str(out/project),'-p:MonoCecilPath='+str(cecil),'-p:UnityCorePath='+str(core)],project+'-build')
parent=root/'inputs/native55a-parent.dll'
for i in [1,2]:run(['dotnet',str(out/'Patcher/Patcher.dll'),str(parent),str(out/f'candidate{i}.dll'),str(out/f'patch{i}.json')],f'patch{i}')
candidate=out/'candidate1.dll';assert sha(candidate)==sha(out/'candidate2.dll')==sha(work/'candidate1.dll')=='fbcded9c104193cd40df6a0f7e76c3dc8e543dd845cced71cfaf74c052f91b02'
run(['dotnet',str(out/'Audit/Audit.dll'),str(parent),str(candidate),str(out/'patch1.json'),str(out/'ISOLATION.json')],'isolation')
run(['dotnet',str(out/'MemberAccess/MemberAccess.dll'),str(candidate),str(support),str(out/'patch1.json'),str(out/'MEMBER-ACCESS.json')],'member-access')
run(['dotnet',str(out/'MemberAccess/MemberAccess.dll'),str(root/'diagnostic/native55/candidate1.dll'),str(support),str(root/'diagnostic/native55/patch1.json'),str(out/'REJECTED-MEMBER-ACCESS.json')],'private-member-negative',2)
private=json.loads((out/'REJECTED-MEMBER-ACCESS.json').read_text());assert private['rejected']==2 and private['unresolved']==0
run(['dotnet',str(out/'Read/Read.dll'),str(candidate),str(core),str(out/'CIL-AND-PUBLIC-CORE-PROOF.json')],'public-core-proof')
assert (out/'CIL-AND-PUBLIC-CORE-PROOF.json').read_bytes()==(work/'CIL-AND-PUBLIC-CORE-PROOF.json').read_bytes()
run(['dotnet',str(root/'tools/exporter/PatcherNative19.dll'),str(candidate),str(out/'all-methods.json'),'export-all'],'fresh-CIL-export',env=dict(os.environ,GODSPVZ_RESOLVER=str(support)))
data=json.loads((out/'all-methods.json').read_text());assert data==json.loads((work/'all-methods.json').read_text())
sys.path.insert(0,str(root/'sources/frozen-native40'));import verify_types as V
assert sha(root/'sources/frozen-native40/verify_types.py')=='5175b68ae0cb9366a493499b566a7430dc1d8986d81cf17be91e0640974dd47a'
typed=[]
for m in data['methods']:
 try:typed.append(V.verify(m,data['types']))
 except V.InvalidIL as e:typed.append({'token':m['token'],'status':'FAIL','error':str(e)})
assert typed==json.loads((work/'TYPED-RESULTS.json').read_text());assert sum(m['status']=='PASS'for m in typed)==1610
run(['clang++','-std=c++17','-O0',str(src/'native56_oracle.cpp'),'-o',str(out/'native-oracle')],'oracle-build')
p=subprocess.run([str(out/'native-oracle'),str(root/'original/GameAssembly.dll')],capture_output=True,timeout=60);assert p.returncode==0,p.stderr;(out/'NATIVE-ORACLE.tsv').write_bytes(p.stdout);(out/'NATIVE-ORACLE.stderr').write_bytes(p.stderr);assert p.stdout==(work/'NATIVE-ORACLE.tsv').read_bytes(),'Native replay must be byte-identical'
run(['dotnet',str(out/'RuntimeFixture/RuntimeFixture.dll'),str(candidate),str(out/'NATIVE-ORACLE.tsv'),str(out/'RUNTIME-FIXTURE.json')],'fixture')
fixture=json.loads((out/'RUNTIME-FIXTURE.json').read_text());assert fixture['gate_pass']and fixture['positive_cases']==40960 and fixture['negative_controls']==15 and fixture['tool_errors']==0
semantic=['bank-x','device-op','flicker-gain','sun-z','shadow-y','track-source','missile-unordered','zombie-reset','popup-index','bleep-zero'];controls=[]
for mode in semantic+['old-heap','locals','section-flags','body-padding','max-stack','non-target']:
 mutant=out/(mode+'.dll');run(['dotnet',str(out/'Mutate/Mutate.dll'),str(candidate),str(mutant),mode],mode+'-write')
 if mode in semantic:
  run(['dotnet',str(out/'RuntimeFixture/RuntimeFixture.dll'),str(mutant),str(out/'NATIVE-ORACLE.tsv'),str(out/(mode+'-fixture.json')),'positive-only'],mode+'-fixture',2);r=json.loads((out/(mode+'-fixture.json')).read_text());assert r['positive_failures']>0 and r['tool_errors']==0
 report=json.loads((out/'patch1.json').read_text());report['candidate_sha256']=sha(mutant)
 if mode=='bleep-zero':report['methods'][-1]['new_code_size']+=1
 if mode=='locals':report['methods'][0]['local_sig']=report['methods'][2]['local_sig']
 if mode=='max-stack':report['methods'][0]['new_max']=9
 raw=mutant.read_bytes()
 for row in report['methods']:row['code_sha256']=hashlib.sha256(raw[row['header']+12:row['header']+12+row['new_code_size']]).hexdigest()
 forged=out/(mode+'-forged.json');forged.write_text(json.dumps(report));must=out/(mode+'-must-not-pass.json');run(['dotnet',str(out/'Audit/Audit.dll'),str(parent),str(mutant),str(forged),str(must)],mode+'-audit',2);assert not must.exists();controls.append(mode)
old=work/'pre-header-correction/candidate1.dll'
run(['dotnet',str(root/'tools/exporter/PatcherNative19.dll'),str(old),str(out/'old-header-all-methods.json'),'export-all'],'old-header-export',env=dict(os.environ,GODSPVZ_RESOLVER=str(support)))
bad_data=json.loads((out/'old-header-all-methods.json').read_text());m=next(m for m in bad_data['methods']if m['token']=='0x0600066E')
try:V.verify(m,bad_data['types'])
except V.InvalidIL as e:assert str(e)=='declared MaxStack too small'
else:raise AssertionError('Underdeclared original raw method header must fail frozen checker')
run(['dotnet',str(out/'RuntimeFixture/RuntimeFixture.dll'),str(old),str(out/'NATIVE-ORACLE.tsv'),str(out/'OLD-HEADER-FIXTURE-LIMIT.json'),'positive-only'],'old-header-fixture-limit')
run(['dotnet',str(out/'Audit/Audit.dll'),str(parent),str(old),str(work/'pre-header-correction/patch1.json'),str(out/'old-header-must-not-pass.json')],'old-header-audit',2)
assert json.loads((out/'OLD-HEADER-FIXTURE-LIMIT.json').read_text())['positive_failures']==0
result={'archive_members_verified':len(manifest),'candidate_reproduced_exactly':True,'candidate_sha256':sha(candidate),'fresh_CIL_export_and_frozen_all_method_verdicts_equal':True,'typed_pass':1610,'typed_fail':687,'native_recording_replayed_byte_identically':True,'original_native_zero_initializer_and_getter_executed_unchanged':True,'original_animator_literal_decoded_independently':literal,'actual_CIL_cases':40960,'emitted_bad_variants_detected':15,'actual_bad_DLLs_detected':10,'forged_scope_controls_detected':len(controls),'member_visibility_positive_pass':True,'private_UI_field_sites_rejected':2,'old_underdeclared_header_rejected_by_frozen_gate':True,'old_header_still_passes_DynamicMethod_fixture_proving_limit':True,'historical_regressions':'31 suites passed before archive; not rerun by this replay','full_IL2CPP_and_all_TU_gate_pending':True,'ipa_produced':False};(root/'ARCHIVE-REPLAY.json').write_text(json.dumps(result,indent=2));print(json.dumps(result))
