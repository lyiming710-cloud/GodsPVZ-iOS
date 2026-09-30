from pathlib import Path
import subprocess,shutil
root=Path(__file__).resolve().parents[2]
work=root/'.validation/native17/fixture';work.mkdir(parents=True,exist_ok=True)
def run(cmd):subprocess.run(cmd,cwd=root,check=True,timeout=120)
run(['dotnet','build','scripts/codespaces/Native17Fixture','-o',str(work/'original')])
source=work/'original/Native17Fixture.dll';out=work/'patched/Native17Fixture.dll'
out.parent.mkdir(exist_ok=True)
run(['dotnet','run','--project','scripts/takeover/PatcherNative17','--',str(source),str(out),'fixture'])
shutil.copy2(work/'original/Native17Fixture.runtimeconfig.json',out.parent)
run(['dotnet',str(out)])
