from pathlib import Path
import subprocess,json,os
dst=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review')
source='__INSPECTOR_SOURCE__'
project=dst/'inspector';project.mkdir(exist_ok=True)
(project/'Program.cs').write_text(source)
(project/'Inspector.csproj').write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net9.0</TargetFramework></PropertyGroup><ItemGroup><Reference Include="Mono.Cecil" HintPath="/workspaces/GodsPVZ-native19/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll" /></ItemGroup></Project>')
code=r'''
from pathlib import Path
import json,subprocess,os,time,collections
dst=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review');src=dst.parent/'native22'
with (dst/'inspector-build.log').open('w') as f: rc=subprocess.run(['dotnet','build',str(dst/'inspector/Inspector.csproj'),'-c','Release','-o',str(dst/'inspector-bin')],stdout=f,stderr=subprocess.STDOUT).returncode
assert rc==0,'inspector build failed'
exe=dst/'inspector-bin/Inspector.dll'
for tag,dll in [('baseline',src/'input/GodsPVZRuntime1.baseline.dll')]+[(t,src/'work'/(t+'.dll')) for t in ('batch1','batch2','batch2a')]:
 subprocess.run(['dotnet',str(exe),str(dll),str(dst/(tag+'-independent-inspect.json'))],check=True)
subprocess.run(['dotnet',str(exe),'probe',str(dst/'patcher-bin/RepairNative23.dll'),str(dst/'patcher-swap-probe.json')],check=True)
data={t:json.loads((dst/(t+'-independent-inspect.json')).read_text()) for t in ('baseline','batch1','batch2','batch2a')}
results=[]
for inp,out,ed in [('baseline','batch1','batch1'),('batch1','batch2','batch2'),('batch1','batch2a','batch2a')]:
 a,b=data[inp],data[out];aa={m['token']:m for m in a['methods']};bb={m['token']:m for m in b['methods']}
 edits=json.loads((src/('edits-'+ed+'.json')).read_text())['edits'];planned={e['token'].upper().replace('0X','0x') for e in edits}
 changed=[t for t in aa if aa[t]['body']!=bb[t]['body']]; raw=[t for t in aa if aa[t]['rawIL']!=bb[t]['rawIL']]
 metadata=['assembly','module','mvid','types']
 diffs=[]
 for t in changed:
  x,y=aa[t]['body'],bb[t]['body']
  if len(x['instructions'])!=len(y['instructions']):diffs.append({'token':t,'lengthDiff':True});continue
  for k,(u,v) in enumerate(zip(x['instructions'],y['instructions'])):
   if u!=v:diffs.append({'token':t,'index':k,'before':u,'after':v})
 r={'input':inp,'output':out,'methods':len(aa),'bodies':sum(m['body'] is not None for m in aa.values()),'planned_methods':len(planned),'changed_methods':len(changed),'unchanged_bodies':sum(m['body'] is not None and m['body']==bb[t]['body'] for t,m in aa.items()),'unexpected_changed':sorted(set(changed)-planned),'planned_unchanged':sorted(planned-set(changed)),'raw_IL_changed':len(raw),'raw_IL_changed_non_targets':sorted(set(raw)-planned),'identity_equal':{k:a[k]==b[k] for k in metadata},'tables_before':a['tableCounts'],'tables_after':b['tableCounts'],'method_metadata_drift':[t for t in aa if {k:v for k,v in aa[t].items() if k not in ('rawIL','body')}!={k:v for k,v in bb[t].items() if k not in ('rawIL','body')}],'defects':b['defects'],'instruction_differences':len(diffs),'diffs':diffs}
 results.append(r)
(dst/'independent-isolation.json').write_text(json.dumps(results,indent=2))
print('INSPECT_DONE')
'''
(dst/'inspect-driver.py').write_text(code)
with (dst/'inspect-driver.log').open('w') as f:p=subprocess.Popen(['python3',str(dst/'inspect-driver.py')],stdout=f,stderr=subprocess.STDOUT,start_new_session=True)
print(json.dumps({'pid':p.pid}))
