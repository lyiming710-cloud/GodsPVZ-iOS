from pathlib import Path
import sys,re,subprocess,json,hashlib,py_compile
sys.path.insert(0,str(Path(__file__).parent/'python-deps'))
import yaml
R=Path(__file__).resolve().parents[1]/'native15-work'
p=R/'.github/workflows/stage9-native17-real-ios-xcode-gate.yml';s=p.read_text();doc=yaml.load(s,Loader=yaml.BaseLoader)
assert doc['on']['push']['branches']==['repair/codespace-native14-validation-2026-09-27']
assert doc['jobs']['export']['needs']=='qualification'
assert doc['permissions']=={'contents':'read','actions':'read'}
assert doc['jobs']['export']['env']['TEST_DLL_SHA256']=='74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2'
bash=Path.home()/'.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/bin/bash.exe'
checks=0;embedded=0
for job in doc['jobs'].values():
 for step in job['steps']:
  if 'run' not in step:continue
  code=re.sub(r'\$\{\{.*?\}\}','EXPRESSION',step['run'])
  subprocess.run([str(bash),'--noprofile','--norc','-n'],input=code,text=True,check=True,capture_output=True)
  checks+=1
  for body in re.findall(r"<<'PY'\n(.*?)\nPY(?:\n|$)",code,re.S):compile(body,step.get('name','embedded'),'exec');embedded+=1
assert 'native6' not in s and 'formal_native4' not in s
assert s.count('candidate-assembly-name.txt')>=3
py_compile.compile(str(R/'scripts/codespaces/package_native17.py'),doraise=True)
git=Path.home()/'.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
rel='Recovery/Native17-2026-09-28/validation/'
def blob(n):return subprocess.check_output([str(git),'-C',str(R),'show','HEAD:'+rel+n])
h=json.loads(blob('SHA256.json'));assert all(hashlib.sha256(blob(n)).hexdigest()==v for n,v in h.items())
print('YAML_PARSE_PASS; BASH_SYNTAX_PASS',checks,'; EMBEDDED_PYTHON_PASS',embedded,'; NATIVE17_ARCHIVE_HASH_PASS',len(h))
