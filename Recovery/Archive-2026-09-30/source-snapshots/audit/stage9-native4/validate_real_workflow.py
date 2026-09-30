import re,os,json,subprocess
from pathlib import Path
R=Path(__file__).resolve().parent
s=(R.parent/'stage9-native3/repository/.github/workflows/stage9-native4-real-ios-xcode-gate.yml').read_text()
env=dict(re.findall(r"^      (\w+): '([^']*)'$",s,re.M))
env.update(RUNNER_TEMP=str(R/'preflight-test'),GITHUB_REPOSITORY='lyiming710-cloud/GodsPVZ-iOS')
os.environ.update(env)
out=Path(env['RUNNER_TEMP'])/'stage9-native4-ios'
for d in ('candidate','evidence'): (out/d).mkdir(parents=True,exist_ok=True)
pre=(R/'real_preflight.py').read_text();a,b=pre.split("run=json.loads(api(",1)
scope={};exec(a,scope)
def local_api(path):
 if path.startswith('actions/runs/'):return (R/'evidence/static-run.json').read_bytes()
 if path.endswith('/zip'):return (R/'inputs/cloud-qualified.zip').read_bytes()
 return json.dumps(json.loads((R/'evidence/static-artifacts.json').read_text())['artifacts'][0]).encode()
scope['api']=local_api
exec("run=json.loads(api("+b,scope)
blocks=[]
for match in re.finditer(r'^        run: \|\n((?:          .*\n|\n)+)',s,re.M):
 block='\n'.join(x[10:] if x.startswith('          ') else x for x in match.group(1).splitlines())+'\n'
 block=re.sub(r'\$\{\{.*?\}\}','placeholder',block)
 blocks.append(block)
 for py in re.findall(r"<<'PY'\n(.*?)\nPY",block,re.S):compile(py,'embedded-python','exec')
for i,b in enumerate(blocks): (R/'tools'/f'real-step-{i}.sh').write_text(b)
print('preflight artifact replay PASS; embedded Python syntax PASS; shell blocks',len(blocks))
