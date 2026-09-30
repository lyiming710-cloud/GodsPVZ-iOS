from pathlib import Path
import json
p=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review');w=p.parent/'native22'
out={}
for tag,path in [('baseline','body-map.json'),('batch1','body-map.new.json'),('batch2','body-map.b2.json')]:
 mine=json.loads((p/(tag+'-method-diagnostics.json')).read_text());old=json.loads((w/'out'/path).read_text());miss=[t for t in mine if mine[t]['errors'] and not old.get(t,{}).get('errors')]
 out[tag]={'missing_error_methods':len(miss),'missing':[{**mine[t],'token':t,'old_record':old.get(t)} for t in miss],'named_old_zero_probes':{t:{'ours':mine.get(t),'old':old.get(t)} for t in ('0x060003F7','0x060003FD','0x06000451')}}
(p/'attribution-comparison.json').write_text(json.dumps(out,indent=2))
print(json.dumps({t:{'missing_error_methods':v['missing_error_methods'],'examples':v['missing'][:2],'named_old_zero_probes':v['named_old_zero_probes']} for t,v in out.items()},indent=2))
