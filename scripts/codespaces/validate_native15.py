#!/usr/bin/env python3
"""Native15 two-target experiment, with pinned native14 positive control."""
from pathlib import Path
import hashlib, json, runpy, subprocess, time, os
root=Path(__file__).resolve().parents[2]
base=root/'.validation/native15';base.mkdir(parents=True,exist_ok=True)
report={'status':'RUNNING','full_unity_export':False,'production_promotion':False}
(base/'latest-result.json').write_text(json.dumps(report))
started=time.monotonic()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
try:
    # Rebuilds native13/14 from the pinned original linked seed and verifies both
    # previous failure sets before this experiment is evaluated.
    g=runpy.run_path(str(root/'scripts/codespaces/validate_native14.py'))
    work=g['work'];run=g['run'];replay=g['replay']
    project=root/'scripts/takeover/PatcherNative15/PatcherNative15.csproj'
    unlinked=root/'.validation/native14/Assembly-CSharp-native14-elements.dll'
    assert sha(unlinked)=='281b7a20d3cc0719b0087be389f10a16e966a85c93bd5f43ef72fd54eaa01614'
    outputs={}
    for label,source,args in [('unlinked',unlinked,[]),('linked',g['linked14'],['linked'])]:
        first=work/f'native15-{label}.dll';second=work/f'native15-{label}-repeat.dll'
        for output,suffix in [(first,''),(second,'-repeat')]:
            run(['dotnet','run','--project',str(project),'--',str(source),str(output)]+args,f'native15-{label}{suffix}.log',env=dict(os.environ,GODSPVZ_RESOLVER=str(g['M'])))
        assert first.read_bytes()==second.read_bytes(),label+' materialization not deterministic'
        outputs[label]={'path':str(first),'input_sha256':sha(source),'sha256':sha(first)}
    result=replay('native15',Path(outputs['linked']['path']))
    targets={"System.Collections.Generic.List`1<Grid> Map::RandomGet_Grid_TestPlace(System.Int32,T,System.Int32,System.Int32)","System.Void VFXAnimationEvent::Binding(T,System.Boolean,System.Single,System.String)"}
    assert targets<=set(g['b']['methods']),'native14 positive control missing targets'
    assert not (targets&set(result['methods'])),'native15 target remains an IL2CPP failure'
    report.update(status='NATIVE15_DIRECT_CONVERSION_PASS' if result['exit']==0 else 'NATIVE15_TARGET_CLOSURE_PASS',native14=g['b'],native15=result,outputs=outputs,work_directory=str(work),elapsed_seconds=round(time.monotonic()-started,2),source_commit=subprocess.check_output(['git','rev-parse','HEAD'],cwd=root,text=True).strip())
    (work/'native15-result.json').write_text(json.dumps(report,indent=2)+'\n')
except Exception as e:
    report.update(status='FAILED',error=str(e))
    raise
finally:
    (base/'latest-result.json').write_text(json.dumps(report,indent=2)+'\n')
    print(json.dumps(report,indent=2),flush=True)
