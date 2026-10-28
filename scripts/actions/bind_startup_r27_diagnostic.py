"""Admit exact R27 game + already engine-tested SDK for fresh Editor diagnosis.

The R26 path is immutable transport and parent evidence. Neither its Linux
compiler evidence nor its real-engine SDK probe proves this new game caller.
"""
from pathlib import Path
import argparse,hashlib,json,zipfile
import bind_startup_r26_diagnostic as bootstrap
import validate_startup_r27_checkpoint as game
require=bootstrap.require;sha=lambda b:hashlib.sha256(b).hexdigest()
CANDIDATE=game.CANDIDATE;SDK=bootstrap.SDK

def validate_checkpoint():
 proof=game.validate();parent,sdk,prior=bootstrap.validate_checkpoint()
 require(sha(parent)==game.PARENT and sha(sdk)==SDK,'Parent game/SDK transport differs')
 observed=json.loads((bootstrap.CHECKPOINT/'ACTUAL-UNITY-40709087573.json').read_bytes())
 require(observed['sdk_sha256']==SDK and observed['sdk_real_engine']=='PASS'and observed['sdk_engine_checks']==10 and observed['actual_loaded_sdk_copies_rehashed']==2,'Actual parent SDK engine proof missing')
 require(observed['gameplay_pass']is False and observed['gameplay_status']=='ACTUAL_GAME_LOSS_OBSERVED','Retain parent gameplay failure')
 with zipfile.ZipFile(game.ROOT/'STARTUP-R27-LIFECYCLE123-CANDIDATE.zip')as z:data=z.read('STARTUP-R27-LIFECYCLE123.dll')
 require(sha(data)==CANDIDATE,'Wrong child bytes')
 return data,sdk,dict(child_scoped=proof,parent_transport=prior,prior_sdk_real_engine_run=40709087573,
                     current_game_linux_compiler=False,current_game_apple_compiler=False,
                     current_game_real_unity=False,ipa_promotion_allowed=False)

def bind(output,source):
 data,sdk,proof=validate_checkpoint();bootstrap.bind(output,source);out=Path(output).resolve();e=out/'evidence';target=out/'candidate/Assembly-CSharp-bulk401.dll'
 require(sha(target.read_bytes())==bootstrap.CANDIDATE,'Parent R26 image differs')
 require(sha((out/'recovered-sdk/Unity.VisualScripting.Core.dll').read_bytes())==SDK,'SDK transport differs')
 provenance=e/'PROVENANCE.json';backup=e/'PARENT-R26-TRANSPORT-PROVENANCE.json';require(not backup.exists(),'Keep prior transport receipt');provenance.rename(backup);target.write_bytes(data)
 result=dict(status='STARTUP_R27_RESTRICTED_REAL_EDITOR_DIAGNOSIS_ONLY',source_commit=source,candidate_sha256=CANDIDATE,sdk_sha256=SDK,outputs={'unlinked':{'sha256':CANDIDATE}},new_game_methods=2,new_native_operation_positions=123,proof=proof,whole_game_semantics='NOT_PROVEN',child_device_execution_performed=False,ipa_promotion_allowed=False)
 for name in ['PROVENANCE.json','candidate-inspection.txt']:(e/name).write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8')
 return result

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--validate-only',action='store_true');p.add_argument('--output');p.add_argument('--source');a=p.parse_args()
 if a.validate_only:
  b,s,proof=validate_checkpoint();print(json.dumps(dict(candidate_sha256=sha(b),sdk_sha256=sha(s),proof=proof)))
 else:require(bool(a.output and a.source),'Output and source required');print(json.dumps(bind(a.output,a.source)))
