from pathlib import Path
import json,hashlib,zipfile,subprocess,sys,os,struct
archive=Path(sys.argv[1]);root=Path(sys.argv[2]);assert not root.exists();root.mkdir(parents=True)
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
with zipfile.ZipFile(archive)as z:
 manifest=json.loads(z.read('MANIFEST.json'));assert len(set(z.namelist()))==len(z.namelist()) and set(z.namelist())==set(manifest)|{'MANIFEST.json'}
 for name,row in manifest.items():
  path=root/name;assert path.resolve().is_relative_to(root.resolve());data=z.read(name);assert len(data)==row['bytes'] and hashlib.sha256(data).hexdigest()==row['sha256'];path.parent.mkdir(parents=True,exist_ok=True);path.write_bytes(data)
src=root/'sources/takeover/native57';work=root/'work';out=root/'replay';out.mkdir();support=root/'support/managed';cecil=root/'support/Mono.Cecil.dll';core=support/'UnityEngine.CoreModule.dll'
raw=(root/'original/GameAssembly.dll').read_bytes();assert hashlib.sha256(raw).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d';nt=struct.unpack_from('<I',raw,60)[0];base=struct.unpack_from('<Q',raw,nt+48)[0];ns=struct.unpack_from('<H',raw,nt+6)[0];opt=struct.unpack_from('<H',raw,nt+20)[0]
def pe_off(va,length):
 hits=[];rva=va-base
 for i in range(ns):
  sh=nt+24+opt+40*i;start,size,off=struct.unpack_from('<III',raw,sh+12)
  if start<=rva and rva+length<=start+size:hits.append(off+rva-start)
 assert len(hits)==1;return hits[0]
enc=struct.unpack_from('<Q',raw,pe_off(0x181baa888,8))[0];assert (enc>>29)&7==5 and enc&1;idx=(enc&0x1FFFFFFF)>>1;meta=(root/'original/global-metadata.dat').read_bytes();assert hashlib.sha256(meta).hexdigest()=='ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9';tab,tab_bytes,text,text_bytes=struct.unpack_from('<IIII',meta,8);assert idx<tab_bytes//8;size,offset=struct.unpack_from('<II',meta,tab+8*idx);assert offset+size<=text_bytes;literal=meta[text+offset:text+offset+size].decode();assert literal=='PoleCommander.speed';proof=json.loads((work/'ORIGINAL-POLE-LITERAL.json').read_text());assert proof['value']==literal and proof['encoded']==hex(enc) and proof['index']==idx
start=pe_off(0x180302170,3);assert raw[start:start+3].hex()=='c20000'
e=json.loads((work/'NATIVE-EVIDENCE.json').read_text());assert len(e['methods'])==8
for m in e['methods']:
 va=int(m['start'],16);length=int(m['end'],16)-va;off=pe_off(va,length);assert hashlib.sha256(raw[off:off+length]).hexdigest()==m['native_sha256']
def run(args,name,expected=0,env=None):
 p=subprocess.run(args,capture_output=True,text=True,timeout=90,env=env);(out/(name+'.log')).write_text(p.stdout+p.stderr);assert p.returncode==expected,(name,p.returncode,p.stdout+p.stderr)
for project in ['Patcher','Audit','RuntimeFixture','MemberAccess','Mutate','Read']:
 run(['dotnet','build',str(src/project/(project+'.csproj')),'-c','Release','-o',str(out/project),'-p:MonoCecilPath='+str(cecil),'-p:UnityCorePath='+str(core)],project+'-build')
parent=root/'inputs/native56-parent.dll'
for i in [1,2]:run(['dotnet',str(out/'Patcher/Patcher.dll'),str(parent),str(out/f'candidate{i}.dll'),str(out/f'patch{i}.json')],f'patch{i}')
candidate=out/'candidate1.dll';assert sha(candidate)==sha(out/'candidate2.dll')==sha(work/'candidate1.dll')=='6e2aa50c2127dc07e4d7f20bcb40faf28c1958db0d843a20ac9db0f17fd8ea12'
run(['dotnet',str(out/'Audit/Audit.dll'),str(parent),str(candidate),str(out/'patch1.json'),str(out/'ISOLATION.json')],'isolation')
run(['dotnet',str(out/'MemberAccess/MemberAccess.dll'),str(candidate),str(support),str(out/'patch1.json'),str(out/'MEMBER-ACCESS.json')],'member-access')
run(['dotnet',str(out/'MemberAccess/MemberAccess.dll'),str(root/'diagnostic/native55/candidate1.dll'),str(support),str(root/'diagnostic/native55/patch1.json'),str(out/'REJECTED-MEMBER-ACCESS.json')],'private-member-negative',2)
private=json.loads((out/'REJECTED-MEMBER-ACCESS.json').read_text());assert private['rejected']==2 and private['unresolved']==0
run(['dotnet',str(out/'Read/Read.dll'),str(candidate),str(core),str(out/'CIL-AND-PUBLIC-CORE-PROOF.json')],'public-core-proof');assert (out/'CIL-AND-PUBLIC-CORE-PROOF.json').read_bytes()==(work/'CIL-AND-PUBLIC-CORE-PROOF.json').read_bytes()
run(['dotnet',str(root/'tools/exporter/PatcherNative19.dll'),str(candidate),str(out/'all-methods.json'),'export-all'],'fresh-CIL-export',env=dict(os.environ,GODSPVZ_RESOLVER=str(support)))
data=json.loads((out/'all-methods.json').read_text());assert data==json.loads((work/'all-methods.json').read_text());sys.path.insert(0,str(root/'sources/frozen-native40'));import verify_types as V
assert sha(root/'sources/frozen-native40/verify_types.py')=='5175b68ae0cb9366a493499b566a7430dc1d8986d81cf17be91e0640974dd47a'
typed=[]
for m in data['methods']:
 try:typed.append(V.verify(m,data['types']))
 except V.InvalidIL as e:typed.append({'token':m['token'],'status':'FAIL','error':str(e)})
assert typed==json.loads((work/'TYPED-RESULTS.json').read_text());assert sum(m['status']=='PASS'for m in typed)==1618
run(['clang++','-std=c++17','-O0',str(src/'native57_oracle.cpp'),'-o',str(out/'native-oracle')],'oracle-build')
p=subprocess.run([str(out/'native-oracle'),str(root/'original/GameAssembly.dll')],capture_output=True,timeout=60);assert p.returncode==0,p.stderr;(out/'NATIVE-ORACLE.tsv').write_bytes(p.stdout);(out/'NATIVE-ORACLE.stderr').write_bytes(p.stderr);assert p.stdout==(work/'NATIVE-ORACLE.tsv').read_bytes(),'Native replay must be byte-identical'
run(['dotnet',str(out/'RuntimeFixture/RuntimeFixture.dll'),str(candidate),str(out/'NATIVE-ORACLE.tsv'),str(out/'RUNTIME-FIXTURE.json')],'fixture');fixture=json.loads((out/'RUNTIME-FIXTURE.json').read_text());assert fixture['gate_pass'] and fixture['positive_cases']==32768 and fixture['negative_controls']==12 and fixture['tool_errors']==0
semantic=['enemy-branch','board-index','copy-num','die-return','heal-y','cool-state','pole-jump','rotate-angle'];controls=[]
for mode in semantic+['old-heap','locals','section-flags','body-padding','max-stack','non-target']:
 mutant=out/(mode+'.dll');run(['dotnet',str(out/'Mutate/Mutate.dll'),str(candidate),str(mutant),mode],mode+'-write')
 if mode in semantic:
  run(['dotnet',str(out/'RuntimeFixture/RuntimeFixture.dll'),str(mutant),str(out/'NATIVE-ORACLE.tsv'),str(out/(mode+'-fixture.json')),'positive-only'],mode+'-fixture',2);r=json.loads((out/(mode+'-fixture.json')).read_text());assert r['positive_failures']>0 and r['tool_errors']==0
 report=json.loads((out/'patch1.json').read_text());report['candidate_sha256']=sha(mutant)
 if mode=='locals':report['methods'][0]['local_sig']=report['methods'][4]['local_sig']
 if mode=='max-stack':report['methods'][0]['new_max']=9
 changed=mutant.read_bytes()
 for row in report['methods']:row['code_sha256']=hashlib.sha256(changed[row['header']+12:row['header']+12+row['new_code_size']]).hexdigest()
 forged=out/(mode+'-forged.json');forged.write_text(json.dumps(report));must=out/(mode+'-must-not-pass.json');run(['dotnet',str(out/'Audit/Audit.dll'),str(parent),str(mutant),str(forged),str(must)],mode+'-audit',2);assert not must.exists();controls.append(mode)
result={'archive_members_verified':len(manifest),'candidate_reproduced_exactly':True,'candidate_sha256':sha(candidate),'fresh_CIL_export_and_frozen_all_method_verdicts_equal':True,'typed_pass':1618,'typed_fail':679,'native_recording_replayed_byte_identically':True,'DieTest_executes_original_native_and_candidate_CIL_Healed_transitively':True,'original_shared_empty_base_helper_executed_unchanged':True,'original_pole_literal_decoded_independently':literal,'actual_CIL_cases':32768,'emitted_bad_variants_detected':12,'actual_bad_DLLs_detected':8,'forged_scope_controls_detected':len(controls),'member_visibility_positive_pass':True,'private_UI_field_sites_rejected':2,'historical_regressions':'32 suites passed before archive; not rerun by this replay','full_IL2CPP_and_all_TU_gate_pending':True,'ipa_produced':False};(root/'ARCHIVE-REPLAY.json').write_text(json.dumps(result,indent=2));print(json.dumps(result))
