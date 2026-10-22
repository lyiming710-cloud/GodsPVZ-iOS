"""Log a real process invocation, stdout/stderr and exit; never translate failure to PASS."""
import json,os,pathlib,subprocess,sys,time
root=pathlib.Path(__file__).resolve().parents[2]
out=pathlib.Path(__file__).resolve().parent/'logs';out.mkdir(exist_ok=True)
name=sys.argv[1];args=sys.argv[2:]
env=os.environ.copy();env.update(DOTNET_CLI_TELEMETRY_OPTOUT='1',DOTNET_SKIP_FIRST_TIME_EXPERIENCE='1',DOTNET_NOLOGO='1',DOTNET_CLI_HOME=str(root/'.local/dotnet-state'),PYTHONUTF8='1')
(out/(name+'.argv.json')).write_text(json.dumps(args,ensure_ascii=False,indent=2)+'\n','utf-8')
t=time.monotonic()
with (out/(name+'.stdout.log')).open('wb') as so,(out/(name+'.stderr.log')).open('wb') as se:
    p=subprocess.run(args,cwd=root,env=env,stdout=so,stderr=se)
(out/(name+'.exitcode.txt')).write_text(str(p.returncode)+'\n','utf-8')
print(json.dumps(dict(step=name,exit=p.returncode,seconds=round(time.monotonic()-t,3))))
print((out/(name+'.stdout.log')).read_text('utf-8',errors='replace')[-9000:])
print((out/(name+'.stderr.log')).read_text('utf-8',errors='replace')[-4000:])
sys.exit(p.returncode)
