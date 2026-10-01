from pathlib import Path
import json,hashlib,subprocess,difflib,re
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native26-2026-10-01';prev=w/'.validation/native25-2026-10-01';new=n/'fresh/cpp';old=prev/'fresh/cpp'
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
parent=json.loads((prev/'CPP-METHODS.json').read_text());current=json.loads((n/'CPP-METHODS.json').read_text());changed=[]
for t,m in current['methods'].items():
 if t=='0x060000D9':continue
 b=parent['methods'][t];a=(old/b['file']).read_text().splitlines()[b['start']-1:b['end']];c=(new/m['file']).read_text().splitlines()[m['start']-1:m['end']]
 if a!=c:changed.append({'token':t,'parent':b['symbol'],'candidate':m['symbol']})
assert not changed,changed
conversion=json.loads((n/'IL2CPP-RESULT.json').read_text())
for name in conversion['different_files_from_native25']:(n/(name+'.diff')).write_text('\n'.join(difflib.unified_diff((old/name).read_text().splitlines(),(new/name).read_text().splitlines(),fromfile='native25/'+name,tofile='native26/'+name,n=3))+'\n')
flags=json.loads((n/'CLANG-FLAGS.json').read_text());rows=[]
for tag,folder in [('parent',old),('candidate',new)]:
 args=[a.replace(str(new),str(folder)) for a in flags]
 with (n/(tag+'-metadatausage-clang.log')).open('w') as log:rc=subprocess.run(args+['-x','c++',str(folder/'Il2CppMetadataUsage.c')],stdout=log,stderr=subprocess.STDOUT).returncode
 text=(n/(tag+'-metadatausage-clang.log')).read_text(errors='replace');rows.append({'input':tag,'exit':rc,'errors':len(re.findall(r'\berror:',text)),'sha256':sha(folder/'Il2CppMetadataUsage.c')})
assert all(x['exit']==0 and x['errors']==0 for x in rows)
report={'non_target_mapped_cpp_methods_equal':len(current['methods'])-1,'differing_non_target_methods':changed,'metadata_usage_c_extra_checks':rows,'global_metadata_equal_parent':conversion['metadata_sha256']==json.loads((prev/'IL2CPP-RESULT.json').read_text())['metadata_sha256']}
(n/'CPP-ISOLATION.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
