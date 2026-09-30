from pathlib import Path
import difflib,json
d=Path('/tmp/godspvz-native24-codespace-2026-10-01');a=(d/'fresh-candidate/cpp/GodsPVZRuntime1__6.cpp').read_text().splitlines();b=(d/'fresh-raw/cpp/GodsPVZRuntime1__6.cpp').read_text().splitlines();delta='\n'.join(difflib.unified_diff(a,b,fromfile='Cecil/GodsPVZRuntime1__6.cpp',tofile='Raw/GodsPVZRuntime1__6.cpp',n=4))+'\n';(d/'RAW-VS-CECIL-CPP.diff').write_text(delta);print(delta)
for name in ['baseline','candidate','raw']:
 data=json.loads((d/(name+'-CPP-METHODS.json')).read_text());print(name,{t:data['methods'].get(t) for t in ['0x06000339','0x06000348','0x0600034C']})
