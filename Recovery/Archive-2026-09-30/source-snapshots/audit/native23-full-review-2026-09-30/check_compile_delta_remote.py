from pathlib import Path
import re,json
dst=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review');src=dst.parent/'native22'
diff=[]
for p in sorted((dst/'batch1/clang').glob('*.log')):
 q=src/'retest/clang'/p.name
 if not q.exists():continue
 a=p.read_text(errors='replace');b=q.read_text(errors='replace')
 n=lambda s:len(re.findall(r'\berror:',s))
 if n(a)!=n(b):diff.append({'file':p.name,'review':n(a),'old':n(b),'review_error_lines':[x for x in a.splitlines() if 'error:' in x][:20],'old_error_lines':[x for x in b.splitlines() if 'error:' in x][:20]})
result={'total_extra':sum(x['review']-x['old'] for x in diff),'diff':diff}
(dst/'compile-count-delta.json').write_text(json.dumps(result,indent=2))
print(json.dumps(result,indent=2))
