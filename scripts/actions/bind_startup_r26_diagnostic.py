"""Admit one exact R25 game + separately recovered SDK for Editor diagnosis.

R23 remains an immutable transport/bootstrap path. Its results do not establish
the behavior of this game/SDK pair. This binder cannot promote an IPA build.
"""
from pathlib import Path
import argparse,hashlib,json,zipfile
import bind_startup_r23_diagnostic as bootstrap
import validate_startup_r25_checkpoint as game
require=bootstrap.require;sha=lambda b:hashlib.sha256(b).hexdigest()
CANDIDATE=game.CANDIDATE;SDK='51f72bcd146c5571f642f3c4149f0545eb704fd325d83eb0f7c4ca2deec14c2b'
CHECKPOINT=Path(__file__).resolve().parents[2]/'Recovery/Startup-R26-SDK-Helpers-2026-10-28'
def validate_checkpoint(root=CHECKPOINT):
 gameproof=game.validate();root=Path(root).resolve();rows=json.loads((root/'MANIFEST.json').read_bytes());paths={p.relative_to(root).as_posix()for p in root.rglob('*')if p.is_file()and p!=root/'MANIFEST.json'};require(paths=={r['file']for r in rows}and len(rows)==len(paths),'SDK evidence inventory differs')
 for r in rows:
  p=root/r['file'];require(p.resolve().is_relative_to(root)and not p.is_symlink(),'SDK path escapes');b=p.read_bytes();require(sha(b)==r['sha256']and len(b)==r['bytes'],'Changed SDK evidence: '+r['file'])
 read=lambda n:json.loads((root/n).read_bytes());c=read('CONSTRUCTION.json');require(c['candidate_sha256']==SDK and c['parent_sha256']=='dc1b51ccb332d73e52642f6a50fe0694661e51b8eb2ff7ca25381f1ab877c8f9'and {int(x['token'],16)for x in c['changes']}=={0x06000903,0x06000904},'Wrong SDK body scope')
 iso=read('ISOLATION.json');require(iso['candidate_sha256']==SDK and iso['pass_']and iso['unchanged_methoddefs']==4933 and not iso['game_assembly_edited']and iso['only_section_permission_change']=='CLEAR_DISCARDABLE_ON_EXTENDED_LAST_SECTION','SDK isolation fails')
 strict=read('FROZEN-RAW.json');require(strict['candidate_sha256']==SDK and len(strict['results'])==2 and all(x['status']=='PASS'for x in strict['results']),'SDK strict check fails');require(read('MEMBER-EXIT.json')['exit_code']==0,'SDK member access fails')
 r=read('ACTUAL-CIL-SOURCE-COMPARE.json');require(r['candidate_sha256']==SDK and r['cases']==36 and r['differences']==0 and all(x['same']for x in r['rows']),'SDK source conformance fails');require(read('CONTROL-STUBS-OBSERVED.json')['old_stub_different_cases']==32,'Actual default-return defect not detected')
 n=read('NATIVE-GAMEOBJECT-COMPARE.json');require(n['candidate_sha256']==SDK and n['cases']==16 and n['unique_input_shapes']==8 and n['differences']==0 and n['generic_addcomponent_native_execution']=='UNSUPPORTED_NO_DIRECT_METHOD_POINTER','Preserve generic native limitation')
 repeat=read('REPEATED-CONSTRUCTION.json');require(repeat['candidate_sha256']==SDK and repeat['two_fresh_constructions_identical'],'SDK repeat construction differs')
 progress=read('PROGRESS.json');require(progress['game_candidate_sha256']==CANDIDATE and not progress['whole_game_accepted']and not progress['ipad_acceptance']and progress['qualified_methods']==0 and progress['ipa']is None,'Product claims precede actual evidence')
 compiler=json.loads((game.ROOT/'ACTUAL-COMPILER-40645188536.json').read_bytes());require(compiler['candidate_sha256']==CANDIDATE and compiler['source_commit']=='3bdd8311a2b0e87138f44710fd3831b8cb1775ff'and compiler['pass_']and compiler['raw_generated_sources_rehashed']==compiler['raw_source_logs_rechecked']==1029 and compiler['raw_errors']==0,'Fresh exact R25 game compiler readback missing')
 with zipfile.ZipFile(game.ROOT/'STARTUP-R25-PLANT123-CANDIDATE.zip')as z:data=z.read('STARTUP-R25-PLANT123.dll')
 with zipfile.ZipFile(root/'RECOVERED-VISUALSCRIPTING-CORE.zip')as z:require(z.testzip()is None,'Broken SDK archive');sdk=z.read('Unity.VisualScripting.Core.dll');require(json.loads(z.read('MANIFEST.json'))==[dict(file='Unity.VisualScripting.Core.dll',bytes=len(sdk),sha256=SDK)],'SDK inner manifest differs')
 require(sha(data)==CANDIDATE and sha(sdk)==SDK,'Fixed image pair differs')
 return data,sdk,dict(game=gameproof,fresh_game_linux_run=40645188536,sdk_source_cases=36,actual_old_stub_different_cases=32,sdk_native_getter_unique_inputs=8,sdk_generic_native_execution=False,real_sdk_engine_probe_required=True,sdk_recovered_image_in_prior_linux_compiler=False,ipa_promotion_allowed=False)
def bind(output,source):
 data,sdk,proof=validate_checkpoint();bootstrap.bind(output,source);out=Path(output).resolve();e=out/'evidence';target=out/'candidate/Assembly-CSharp-bulk401.dll';require(sha(target.read_bytes())==bootstrap.CANDIDATE,'R23 transport image differs');provenance=e/'PROVENANCE.json';backup=e/'PARENT-R23-TRANSPORT-PROVENANCE.json';require(not backup.exists(),'Transport provenance exists');provenance.rename(backup);target.write_bytes(data);s=out/'recovered-sdk';s.mkdir();(s/'Unity.VisualScripting.Core.dll').write_bytes(sdk)
 result=dict(status='STARTUP_R26_RESTRICTED_REAL_EDITOR_GAME_AND_SDK_DIAGNOSIS_ONLY',source_commit=source,candidate_sha256=CANDIDATE,sdk_sha256=SDK,outputs={'unlinked':{'sha256':CANDIDATE}},new_r24_r25_game_methods=8,new_r24_r25_native_positions=253,sdk_changed_methods=2,proof=proof,whole_game_semantics='NOT_PROVEN',child_device_execution_performed=False,ipa_promotion_allowed=False)
 for name in['PROVENANCE.json','candidate-inspection.txt']:(e/name).write_text(json.dumps(result,indent=2)+'\n','utf-8')
 return result
if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--validate-only',action='store_true');p.add_argument('--output');p.add_argument('--source');a=p.parse_args()
 if a.validate_only:
  b,s,proof=validate_checkpoint();print(json.dumps(dict(candidate_sha256=sha(b),sdk_sha256=sha(s),proof=proof)))
 else:require(bool(a.output and a.source),'Output and source required');print(json.dumps(bind(a.output,a.source)))
