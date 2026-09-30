from pathlib import Path
import json,re,bisect,collections,sys
root=Path(__file__).resolve().parents[2];out=Path(sys.argv[1]) if len(sys.argv)>1 else root/'.validation/native19/cpp-preflight'
cpp=Path(json.loads((out/'invocation.json').read_text())['source']) if (out/'invocation.json').exists() else root/'.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/Source/il2cppOutput'
records=[];counts=collections.Counter()
for result in json.loads((out/'results.json').read_text()):
 text=(cpp/result['file']).read_text(encoding='utf-8-sig').splitlines();locations=[];names=[]
 for i,line in enumerate(text,1):
  if line.startswith('IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR') and not line.rstrip().endswith(';'):
   match=re.search(r'\b([\w]+_m[0-9A-F]+)\s*\(',line)
   if match:locations.append(i);names.append(match.group(1))
 for error in result['errors']:
  match=re.search(r'\.cpp:(\d+):(\d+): error: (.*)',error)
  if not match:continue
  line=int(match[1]);index=bisect.bisect_right(locations,line)-1;method=names[index] if index>=0 else '<global>'
  category=match[3].split("'")[0].strip();counts[category]+=1
  records.append({'file':result['file'],'line':line,'method':method,'error':match[3]})
summary={'diagnostics':len(records),'methods':len(set(r['method'] for r in records)),'categories':counts.most_common(),'records':records}
(out/'method-errors.json').write_text(json.dumps(summary,indent=2))
print(json.dumps({k:v for k,v in summary.items() if k!='records'},indent=2))
