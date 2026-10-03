from pathlib import Path
import json,hashlib,zipfile,subprocess,sys
archive=Path(sys.argv[1]);root=Path(sys.argv[2]);assert not root.exists();root.mkdir(parents=True)
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
with zipfile.ZipFile(archive) as z:
 manifest=json.loads(z.read('MANIFEST.json'));assert set(z.namelist())==set(manifest)|{'MANIFEST.json'}
 for name,row in manifest.items():
  p=(root/name).resolve();assert root.resolve()in p.parents and not Path(name).is_absolute()
  data=z.read(name);assert len(data)==row['bytes']and hashlib.sha256(data).hexdigest()==row['sha256'];p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(data)
src=root/'sources/takeover/native55a';work=root/'work';out=root/'replay';out.mkdir();support=root/'support/managed';cecil=root/'support/Mono.Cecil.dll';core=support/'UnityEngine.CoreModule.dll'
def run(args,name,expected=0):
 p=subprocess.run(args,capture_output=True,text=True,timeout=90);(out/(name+'.log')).write_text(p.stdout+p.stderr);assert p.returncode==expected,(name,p.returncode,p.stdout+p.stderr)
for project in ['Patcher','Audit','RuntimeFixture','MemberAccess']:
 run(['dotnet','build',str(src/project/(project+'.csproj')),'-c','Release','-o',str(out/project),'-p:MonoCecilPath='+str(cecil),'-p:UnityCorePath='+str(core)],project+'-build')
parent=root/'inputs/native54-parent.dll'
for i in [1,2]:run(['dotnet',str(out/'Patcher/Patcher.dll'),str(parent),str(out/f'candidate{i}.dll'),str(out/f'patch{i}.json')],f'patch{i}')
candidate=out/'candidate1.dll';assert sha(candidate)==sha(out/'candidate2.dll')==sha(work/'candidate1.dll')=='c329074e4f3de510d8b7a3dfd22bade3178578a159846acc6b9eb26389770a73'
run(['dotnet',str(out/'Audit/Audit.dll'),str(parent),str(candidate),str(out/'patch1.json'),str(out/'ISOLATION.json')],'isolation')
run(['dotnet',str(out/'MemberAccess/MemberAccess.dll'),str(candidate),str(support),str(out/'patch1.json'),str(out/'MEMBER-ACCESS.json')],'member-access')
run(['dotnet',str(out/'MemberAccess/MemberAccess.dll'),str(root/'diagnostic/native55/candidate1.dll'),str(support),str(root/'diagnostic/native55/patch1.json'),str(out/'REJECTED-MEMBER-ACCESS.json')],'member-access-negative',2)
bad=json.loads((out/'REJECTED-MEMBER-ACCESS.json').read_text());assert bad['rejected']==2 and bad['unresolved']==0
run(['clang++','-std=c++17','-O0',str(src/'native55_oracle.cpp'),'-o',str(out/'native-oracle')],'oracle-build')
p=subprocess.run([str(out/'native-oracle'),str(root/'original/GameAssembly.dll')],capture_output=True,timeout=60);assert p.returncode==0,p.stderr;(out/'NATIVE-ORACLE.tsv').write_bytes(p.stdout);(out/'NATIVE-ORACLE.stderr').write_bytes(p.stderr);assert p.stdout==(work/'NATIVE-ORACLE.tsv').read_bytes(),'Native record replay must be byte-identical'
run(['dotnet',str(out/'RuntimeFixture/RuntimeFixture.dll'),str(candidate),str(out/'NATIVE-ORACLE.tsv'),str(out/'RUNTIME-FIXTURE.json')],'fixture')
fixture=json.loads((out/'RUNTIME-FIXTURE.json').read_text());assert fixture['gate_pass']and fixture['positive_cases']==32768 and fixture['negative_controls']==13 and fixture['tool_errors']==0
writer=work/'mutant-writer';run(['dotnet','build',str(writer/'Writer.csproj'),'-o',str(out/'Writer'),'-p:MonoCecilPath='+str(cecil)],'writer-build');semantic=['audio-zero','range-selector','audio-pitch','skill-threshold','formation','device-color','zombie-color','page-offset'];controls=[]
for mode in semantic+['old-heap','locals','section-flags','body-padding','max-stack','non-target']:
 mutant=out/(mode+'.dll');run(['dotnet',str(out/'Writer/Writer.dll'),str(candidate),str(mutant),mode],mode+'-write')
 if mode in semantic:
  run(['dotnet',str(out/'RuntimeFixture/RuntimeFixture.dll'),str(mutant),str(out/'NATIVE-ORACLE.tsv'),str(out/(mode+'-fixture.json')),'positive-only'],mode+'-fixture',2);r=json.loads((out/(mode+'-fixture.json')).read_text());assert r['positive_failures']>0 and r['tool_errors']==0
 report=json.loads((out/'patch1.json').read_text());report['candidate_sha256']=sha(mutant)
 if mode=='audio-zero':report['methods'][0]['new_code_size']+=1
 if mode=='locals':report['methods'][0]['local_sig']=0x110001AA
 if mode=='max-stack':report['methods'][0]['new_max']=9
 data=mutant.read_bytes()
 for row in report['methods']:row['code_sha256']=hashlib.sha256(data[row['header']+12:row['header']+12+row['new_code_size']]).hexdigest()
 forged=out/(mode+'-forged.json');forged.write_text(json.dumps(report));must=out/(mode+'-must-not-pass.json');run(['dotnet',str(out/'Audit/Audit.dll'),str(parent),str(mutant),str(forged),str(must)],mode+'-audit',2);assert not must.exists();controls.append(mode)
result={'archive_members_verified':len(manifest),'candidate_reproduced_exactly':True,'candidate_sha256':sha(candidate),'native_recording_replayed_byte_identically':True,'native_zero_initializer_and_getter_executed_unchanged':True,'actual_CIL_cases':32768,'emitted_bad_variants_detected':13,'actual_bad_DLLs_detected':8,'forged_scope_controls_detected':len(controls),'member_visibility_positive_pass':True,'two_private_UI_field_sites_rejected':True,'full_IL2CPP_and_all_TU_gate_pending':True,'ipa_produced':False};(root/'ARCHIVE-REPLAY.json').write_text(json.dumps(result,indent=2));print(json.dumps(result))
