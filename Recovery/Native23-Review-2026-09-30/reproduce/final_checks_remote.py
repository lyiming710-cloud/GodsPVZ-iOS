from pathlib import Path
import json,re,collections,bisect,hashlib,subprocess
p=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review');w=p.parent/'native22';report={}
for tag in ('baseline','batch1','batch2','batch2a'):
 cpp=p/tag/'cpp';cg=(cpp/'GodsPVZRuntime1_CodeGen.c').read_text(encoding='utf-8-sig');m=re.search(r's_methodPointers\[(\d+)\]\s*=\s*\{(.*?)\n\};',cg,re.S)
 assert m, 'no pointer table';raw=[x.strip() for x in m.group(2).split(',') if x.strip()];assert len(raw)==int(m.group(1))==2317
 symbol_to_tokens=collections.defaultdict(list)
 for idx,sym in enumerate(raw):
  if sym not in ('NULL','0'):symbol_to_tokens[sym].append('0x%08X'%(0x06000001+idx))
 mapped={};totalcpp=0;outside=0;header=0
 for f in sorted(cpp.glob('*.cpp')):
  text=f.read_text(encoding='utf-8-sig',errors='replace');lines=text.splitlines();regions=[]
  for idx,line in enumerate(lines):
   match=re.match(r'^IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR[^\n]*?\b([A-Za-z0-9_]+)\s*\(',line)
   if not match or idx+1>=len(lines) or lines[idx+1]!='{':continue
   end=next((j for j in range(idx+1,len(lines)) if lines[j]=='}'),None)
   if end is not None and match.group(1) in symbol_to_tokens:
    regions.append((idx+1,end+1,match.group(1)))
    for tok in symbol_to_tokens[match.group(1)]:mapped[tok]={'symbol':match.group(1),'file':f.name,'start':idx+1,'end':end+1,'errors':[]}
  starts=[r[0] for r in regions]
  for line in (p/tag/'clang'/(f.name+'.log')).read_text(errors='replace').splitlines():
   mm=re.match(r'^(.+?):(\d+):(\d+): (?:fatal )?error: (.*)',line)
   if not mm:continue
   if Path(mm.group(1)).name!=f.name:header+=1;continue
   totalcpp+=1;ln=int(mm.group(2));ix=bisect.bisect_right(starts,ln)-1
   if ix<0 or not regions[ix][0]<=ln<=regions[ix][1]:outside+=1;continue
   for tok in symbol_to_tokens[regions[ix][2]]:mapped[tok]['errors'].append({'line':ln,'col':int(mm.group(3)),'message':mm.group(4)})
 (p/(tag+'-method-diagnostics.json')).write_text(json.dumps(mapped,indent=2))
 report[tag]={'pointer_table_size':len(raw),'mapped_method_tokens':len(mapped),'methods_with_errors':sum(bool(v['errors']) for v in mapped.values()),'cpp_errors':totalcpp,'other_included_file_errors':header,'errors_outside_mapped_method':outside,'batch1_targets':{t:mapped[t] for t in ('0x06000339','0x06000348','0x0600034C')}}
for tag in ('batch1','batch2','batch2a'):
 inp='baseline' if tag=='batch1' else 'batch1';a=json.loads((p/(inp+'-method-diagnostics.json')).read_text());b=json.loads((p/(tag+'-method-diagnostics.json')).read_text())
 report[tag]['methods_new_errors']=[t for t in b if not a.get(t,{}).get('errors') and b[t]['errors']]
 report[tag]['methods_error_count_increased']=[t for t in b if len(b[t]['errors'])>len(a.get(t,{}).get('errors',[]))]
# Inspect every raw IL difference in the 18 logically unchanged batch1 methods.
aa=json.loads((p/'baseline-independent-inspect.json').read_text());bb=json.loads((p/'batch1-independent-inspect.json').read_text());a={m['token']:m for m in aa['methods']};b={m['token']:m for m in bb['methods']}
rawdiff=[]
for t in a:
 if a[t]['rawIL']!=b[t]['rawIL'] and a[t]['body']==b[t]['body']:
  x,y=bytes.fromhex(a[t]['rawIL']),bytes.fromhex(b[t]['rawIL']);d=[i for i in range(min(len(x),len(y))) if x[i]!=y[i]]
  rawdiff.append({'token':t,'name':a[t]['name'],'length_equal':len(x)==len(y),'byte_offsets':d,'equal_normalized_body':True})
report['non_target_raw_changes']=rawdiff
report['final_identity']={'head':subprocess.check_output(['git','rev-parse','HEAD'],cwd=p.parent.parent,text=True).strip(),'git_status':subprocess.check_output(['git','status','--porcelain'],cwd=p.parent.parent,text=True),'candidate_hashes':{tag:hashlib.sha256((w/'work'/(tag+'.dll')).read_bytes()).hexdigest() for tag in ('batch1','batch2','batch2a')},'shared_canary_hash':hashlib.sha256((p.parent/'native21/replay/canary/Library/Bee/artifacts/iOS/ManagedStripped/GodsPVZRuntime1.dll').read_bytes()).hexdigest()}
(p/'final-checks.json').write_text(json.dumps(report,indent=2));print(json.dumps({k:v for k,v in report.items() if k not in ('non_target_raw_changes',)},indent=2))
