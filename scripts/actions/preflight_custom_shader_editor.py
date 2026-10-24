"""Add the custom-shader helper to the exact preflight's post-migration source set."""
from pathlib import Path
import argparse
import hashlib
import json
import subprocess

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--editor',required=True);p.add_argument('--prior',required=True);p.add_argument('--source',required=True);a=p.parse_args()
    data=Path(a.editor).resolve().parent/'Data';prior=Path(a.prior).resolve();source=Path(a.source).resolve()
    lines=(prior/'candidate.rsp').read_text('utf-8').splitlines()
    if len([line for line in lines if line.startswith('/out:')])!=1:
        raise ValueError('Exact prior candidate compilation response required')
    args=[('/out:"'+str(prior/'custom-shaders.dll')+'"') if line.startswith('/out:') else line for line in lines]
    args+=['"'+str(source)+'"']
    rsp=prior/'custom-shaders.rsp';rsp.write_text('\n'.join(args)+'\n','utf-8')
    proc=subprocess.run([str(data/'NetCoreRuntime/dotnet'),'exec',str(data/'DotNetSdkRoslyn/csc.dll'),'/noconfig','@'+str(rsp)],capture_output=True,text=True)
    (prior/'custom-shaders.log').write_text(proc.stdout+proc.stderr,'utf-8')
    result=dict(scope='EXACT_EDITOR_ADDITIONAL_SHADER_HELPER_API_COMPILATION_ONLY',exit_code=proc.returncode,source_sha256=hashlib.sha256(source.read_bytes()).hexdigest(),unity_started=False)
    (prior/'CUSTOM-SHADER-HELPER-COMPILATION.json').write_text(json.dumps(result,indent=2)+'\n','utf-8')
    print(json.dumps(result));raise SystemExit(proc.returncode)
