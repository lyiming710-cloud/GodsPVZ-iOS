"""Keep earlier transport receipts; explicitly admit only the reviewed test IPA."""
from pathlib import Path
import argparse,hashlib,json
import bind_startup_r28_diagnostic as transport
from admit_startup_r28_test_ipa import validate as admit

def validate_checkpoint():
 admission=admit();game,sdk,prior=transport.validate_checkpoint()
 assert hashlib.sha256(game).hexdigest()==admission['candidate_sha256']
 assert hashlib.sha256(sdk).hexdigest()==admission['sdk_sha256']
 return dict(scope='EXACT_R28_UNSIGNED_DIAGNOSTIC_BUILD_ONLY_NOT_PRODUCTION_PROMOTION',
             admission=admission,earlier_restricted_transport_proof=prior)

def bind(output,source):
 proof=validate_checkpoint();transport.bind(output,source)
 out=Path(output).resolve();folder=out/'evidence';path=folder/'PROVENANCE.json'
 previous=folder/'PARENT-R28-DIAGNOSTIC-TRANSPORT-PROVENANCE.json'
 assert not previous.exists();result=json.loads(path.read_bytes());path.rename(previous)
 result.update(status='FIXED_R28_UNSIGNED_DIAGNOSTIC_IPA_BUILD_ADMISSION',
               diagnostic_ipa_build_allowed=True,ipa_promotion_allowed=False,production_promotion_allowed=False,
               whole_game_accepted=False,proof=proof)
 for name in ['PROVENANCE.json','candidate-inspection.txt']:(folder/name).write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8')
 return result

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--validate-only',action='store_true');p.add_argument('--output');p.add_argument('--source');a=p.parse_args()
 if a.validate_only:result=validate_checkpoint()
 else:
  if not(a.output and a.source):p.error('Explicit output and workflow source required')
  result=bind(a.output,a.source)
 print(json.dumps(result))
