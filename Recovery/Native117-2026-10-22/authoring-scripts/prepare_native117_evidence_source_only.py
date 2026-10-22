from pathlib import Path
import json,re,shutil
R=Path.cwd();o=R/'audit/native117-codex-2026-10-22';p=R/'audit/native116-codex-2026-10-22'
q=json.loads((o/'UNWRITTEN-SOURCE-QUEUE.json').read_text('utf-8'))
chosen=q[:80]
s=(p/'read_source_evidence.py').read_text('utf-8')
s=re.sub(r'^ranges = .*$', 'ranges = '+repr({int(x['token'],16):(x['start'],x['end']) for x in chosen}),s,flags=re.M)
(o/'read_source_evidence.py').write_text(s,encoding='utf-8')
for f in ['source_view.py','read_metadata_catalog.py','read_native_jump_tables.py']:shutil.copyfile(p/f,o/f)
(R/'audit/NEXT50-AFTER-BULK250-2026-10-22.json').write_text(json.dumps({'schema':'next50-source-ledger-v1','predecessor':'audit/native-bulk250-codex-2026-10-22','target_new_unique_method_sources':50,'completed_new_unique_method_sources':0,'remaining_new_method_sources':50,'new_source_tokens':[],'root_executes_tests':False,'new_scope_testing':'DEFERRED_UNTIL_50_NEW_METHOD_SOURCES'},indent=2)+'\n',encoding='utf-8')