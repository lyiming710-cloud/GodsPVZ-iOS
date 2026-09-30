from pathlib import Path
import json,subprocess,hashlib,os
repo=Path('/workspaces/GodsPVZ-native19');base=repo/'.validation/native22'
def cmd(args):
    r=subprocess.run(args,cwd=repo,stdout=subprocess.PIPE,stderr=subprocess.STDOUT,text=True)
    return {'exit':r.returncode,'output':r.stdout.strip()}
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
git={'head':cmd(['git','rev-parse','HEAD']),'branch':cmd(['git','branch','--show-current']),'status':cmd(['git','status','--short']),'log':cmd(['git','log','-5','--format=%H %s']),'agents':cmd(['git','ls-files','*AGENTS.md'])}
files={}
patterns=['work/*.dll','input/*.dll','retest*/ManagedStripped*.dll','edits*.json','out/*accepted*.json','out/*quarantine*.json','out/*tiers*.json','out/*sites*.json','out/afam2-candidates.json','logs/*','regen-batch*/**/global-metadata.dat']
for pattern in patterns:
    for p in base.glob(pattern):
        if p.is_file():files[p.relative_to(base).as_posix()]={'bytes':p.stat().st_size,'sha256':sha(p)}
source={}
for folder in [repo/'scripts/codespaces',base]:
    for p in folder.glob('native23_*.py'):
        source[str(p)]={'bytes':p.stat().st_size,'sha256':sha(p)}
for p in (repo/'scripts/codespaces/RepairNative23').glob('*'):
    if p.is_file():source[str(p)]={'bytes':p.stat().st_size,'sha256':sha(p)}
tree={p.name:[x.name for x in p.iterdir()][:35] for p in base.iterdir() if p.is_dir()}
cpp={str(p.relative_to(base)):len(list(p.glob('*.cpp'))) for p in base.glob('regen*/cpp')}
result={'git':git,'disk':cmd(['df','-h','/workspaces']),'tools':{k:cmd(v) for k,v in {'dotnet':['dotnet','--info'],'clang':['clang++','--version'],'python':['python3','--version']}.items()},'base':str(base),'tree':tree,'cpp':cpp,'files':files,'source':source}
print(json.dumps(result,indent=2))
