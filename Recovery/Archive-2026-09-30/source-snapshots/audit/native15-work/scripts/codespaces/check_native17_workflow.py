"""Parse workflow, shell syntax, and embedded Python without executing steps."""
from pathlib import Path
import sys,re,subprocess,shutil
root=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(root/'.validation/workflow-deps'))
import yaml
p=Path(sys.argv[1]);s=p.read_text();d=yaml.load(s,Loader=yaml.BaseLoader)
assert d['on']['push']['branches']==['repair/codespace-native14-validation-2026-09-27']
assert d['jobs']['export']['needs']=='qualification'
assert d['permissions']=={'contents':'read','actions':'read'}
assert d['jobs']['export']['env']['TEST_DLL_SHA256']=='74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2'
assert 'native6' not in s and 'formal_native4' not in s
assert s.count('candidate-assembly-name.txt')>=3
shell=shutil.which('bash');assert shell,'bash needed for non-executing syntax check'
n=k=0
for job in d['jobs'].values():
 for step in job['steps']:
  if 'run' not in step:continue
  code=re.sub(r'\$\{\{.*?\}\}','EXPRESSION',step['run'])
  subprocess.run([shell,'--noprofile','--norc','-n'],input=code,text=True,check=True)
  n+=1
  for body in re.findall(r"<<'PY'\n(.*?)\nPY(?:\n|$)",code,re.S):compile(body,step.get('name','embedded'),'exec');k+=1
print('NATIVE17_WORKFLOW_STATIC_PASS shell_steps='+str(n)+' embedded_python='+str(k))
