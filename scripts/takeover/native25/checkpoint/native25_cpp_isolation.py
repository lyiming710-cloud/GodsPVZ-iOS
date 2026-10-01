from pathlib import Path
import json,hashlib,subprocess,re,difflib
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native25-2026-10-01';r=n/'native24-restored';new=n/'fresh/cpp';old=r/'generated/raw/cpp'
parent=json.loads((r/'work/raw-CPP-METHODS.json').read_text());current=json.loads((n/'CPP-METHODS.json').read_text());changed=[]
for t,m in current['methods'].items():
 if t=='0x06000338':continue
 before=parent['methods'][t];a=(old/before['file']).read_text().splitlines()[before['start']-1:before['end']];b=(new/m['file']).read_text().splitlines()[m['start']-1:m['end']]
 if a!=b:changed.append({'token':t,'parent':before['symbol'],'candidate':m['symbol']})
assert not changed,changed
for name in ['GodsPVZRuntime1__6.cpp','Il2CppMetadataUsage.c']:
 diff='\n'.join(difflib.unified_diff((old/name).read_text().splitlines(),(new/name).read_text().splitlines(),fromfile='native24/'+name,tofile='native25/'+name,n=3))+'\n';(n/(name+'.diff')).write_text(diff)
flags=json.loads((n/'CLANG-FLAGS.json').read_text());rows=[]
for tag,folder in [('parent',old),('candidate',new)]:
 f=[a.replace(str(new),str(folder)) for a in flags];log=n/(tag+'-metadatausage-clang.log')
 with log.open('w') as out:rc=subprocess.run(f+['-x','c++',str(folder/'Il2CppMetadataUsage.c')],stdout=out,stderr=subprocess.STDOUT).returncode
 text=log.read_text(errors='replace');rows.append({'input':tag,'exit':rc,'errors':len(re.findall(r'\berror:',text)),'sha256':hashlib.sha256((folder/'Il2CppMetadataUsage.c').read_bytes()).hexdigest()})
assert rows[0]['exit']==rows[1]['exit'] and rows[0]['errors']==rows[1]['errors']
report={'non_target_mapped_cpp_methods_equal':len(current['methods'])-1,'differing_non_target_methods':changed,'metadata_usage_c_extra_checks':rows,'global_metadata_equal_parent':hashlib.sha256((n/'fresh/data/Metadata/global-metadata.dat').read_bytes()).hexdigest()==json.loads((r/'work/RAW-CONVERSION.json').read_text())['metadata_sha256']}
(n/'CPP-ISOLATION.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
