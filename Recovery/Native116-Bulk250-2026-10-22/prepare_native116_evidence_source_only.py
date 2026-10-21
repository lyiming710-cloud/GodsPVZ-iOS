"""Prepare read-only source evidence. Does not construct, run or test a candidate."""
from pathlib import Path
import json,re
ROOT=Path(__file__).resolve().parents[1]
out=ROOT/'audit/native116-codex-2026-10-22'
q=json.loads((out/'UNWRITTEN-SOURCE-QUEUE.json').read_text(encoding='utf-8'))
selected=[m for m in q if '<' not in m['name'] and m['size']<=1104]
ranges={int(m['token'],16):(m['start'],m['end']) for m in selected}
source=(ROOT/'audit/native114-codex-2026-10-21/read_source_evidence.py').read_text(encoding='utf-8')
source=re.sub(r'^ranges = .*$', 'ranges = '+repr(ranges),source,flags=re.M)
(out/'read_source_evidence.py').write_text(source,encoding='utf-8')
ledger={'schema':'next50-authoring-ledger-v1','date':'2026-10-22','start_aggregate':200,'start_direct':194,
 'required_new_unique_method_sources':50,'completed_new_unique_method_sources':0,
 'next_concentrated_test_at_aggregate':250,'selected_source_input_tokens':[m['token'] for m in selected],
 'new_source_tokens':[],'source_state':'AUTHORING','current_scope_testing_owner':'AGY',
 'new_scope_testing':'DEFERRED_UNTIL_50_NEW_METHOD_SOURCES','root_project_tests_executed':False,
 'codespace':'SHUTDOWN_DO_NOT_RESTART','formal_promotions':0}
(ROOT/'audit/NEXT50-AFTER-BULK200-2026-10-22.json').write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print('Selected source inputs',len(selected))
