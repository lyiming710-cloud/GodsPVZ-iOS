from pathlib import Path
import json
p=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review')
d=json.loads((p/'status.json').read_text())
print(json.dumps({'finished':d.get('finished'),'stages':{k:{a:b for a,b in v.items() if a in ('reproducible','il2cpp_exit','cpp_count','clang')} for k,v in d['stages'].items()}},indent=2))
for n in ('driver.log','inspect-driver.log','inspector-build.log','analysis.json','patcher-swap-probe.json'):
 if (p/n).exists():
  if n=='analysis.json':
   a=json.loads((p/n).read_text());print(json.dumps({'typed':a['typed'],'actual_accepted_analysis':{k:v for k,v in a['actual_accepted_analysis'].items() if 'sites' not in k and k!='examples'},'direct_local_receiver_sites':a['actual_accepted_analysis']['accepted_direct_local_receiver_sites'],'wrong_shape':a['wrong_shape_contract_probe']},indent=2))
  else:print(n,(p/n).read_text()[-2000:])
if (p/'independent-isolation.json').exists():
 dd=json.loads((p/'independent-isolation.json').read_text())
 print(json.dumps([{k:v for k,v in a.items() if k not in ('tables_before','tables_after','diffs')} for a in dd],indent=2))
