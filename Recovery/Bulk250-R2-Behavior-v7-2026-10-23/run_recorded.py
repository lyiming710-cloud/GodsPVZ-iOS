"""Run one bounded process, retaining argv, full streams, exit and artifact identities."""
from pathlib import Path
import argparse,hashlib,json,os,subprocess,time

BASE=Path(__file__).resolve().parent
ROOT=BASE.parents[1]
p=argparse.ArgumentParser();p.add_argument('--name',required=True);p.add_argument('--timeout',type=float,default=45)
p.add_argument('command',nargs=argparse.REMAINDER);a=p.parse_args()
cmd=a.command[1:] if a.command[:1]==['--'] else a.command
if not cmd:raise ValueError('Command required')
name=a.name
if any(ch not in 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_' for ch in name):raise ValueError('Simple log name only')
out=BASE/'logs';out.mkdir(exist_ok=True)
paths={k:out/f'{name}.{k}' for k in ['argv.json','stdout.log','stderr.log','result.json']}
if any(x.exists() for x in paths.values()):raise FileExistsError('Never overwrite execution evidence')
env=os.environ.copy();env.update(DOTNET_CLI_TELEMETRY_OPTOUT='1',DOTNET_NOLOGO='1',DOTNET_DbgEnableMiniDump='0')
paths['argv.json'].write_text(json.dumps({'argv':cmd,'cwd':str(ROOT),'timeout_seconds':a.timeout},indent=2)+'\n',encoding='utf-8')
start=time.monotonic()
try:
 r=subprocess.run(cmd,cwd=ROOT,env=env,capture_output=True,timeout=a.timeout)
 state='PROCESS_COMPLETED';exitcode=r.returncode;stdout=r.stdout;stderr=r.stderr
except subprocess.TimeoutExpired as e:
 state='TIMED_OUT_NOT_A_PASS';exitcode=None;stdout=e.stdout or b'';stderr=e.stderr or b''
paths['stdout.log'].write_bytes(stdout);paths['stderr.log'].write_bytes(stderr)
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
artifacts=sorted((BASE/'source').glob('*.cs'))+[BASE/'source/Fixture.csproj',BASE/'source/Mono.Cecil.dll',BASE/'source/bin/Release/net10.0/Fixture.dll']
record={'state':state,'exit_code':exitcode,'elapsed_seconds':time.monotonic()-start,
 'host_platform':os.name,'artifacts':[{'path':str(x.relative_to(ROOT)),'sha256':sha(x)} for x in artifacts if x.exists()],
 'streams':{k:{'bytes':paths[k].stat().st_size,'sha256':sha(paths[k])} for k in ['stdout.log','stderr.log']}}
paths['result.json'].write_text(json.dumps(record,indent=2)+'\n',encoding='utf-8')
print(json.dumps(record));print(stdout.decode('utf-8',errors='replace')[-3000:]);print(stderr.decode('utf-8',errors='replace')[-3000:])
raise SystemExit(0 if state=='PROCESS_COMPLETED' and exitcode==0 else 1)
