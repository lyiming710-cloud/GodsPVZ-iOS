#!/usr/bin/env python3
"""Native17 exact-seed replay; target closure is not a full Unity export."""
from pathlib import Path
import hashlib, json, runpy, subprocess, time, os
root=Path(__file__).resolve().parents[2]
base=root/'.validation/native17';base.mkdir(parents=True,exist_ok=True)
report={'status':'RUNNING','full_unity_export':False,'production_promotion':False}
(base/'latest-result.json').write_text(json.dumps(report))
started=time.monotonic()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
try:
    previous=runpy.run_path(str(root/'scripts/codespaces/validate_native16.py'))
    g=previous['g'];work=previous['work'];run=previous['run'];replay=previous['replay']
    expected={'unlinked':'51a749bde92ec75e53dbeed5d35d083ba3616d30f4e4fd756af48a0bd3aed1a5','linked':'7576209f5c31c673434f95975d5e57cd8140f2b16f5608ddb8a521e6e721eadc'}
    outputs={}
    for label,args in [('unlinked',[]),('linked',['linked'])]:
        source=Path(previous['outputs'][label]['path']);assert sha(source)==expected[label],label+' native16 differs'
        first=work/f'native17-{label}.dll';second=work/f'native17-{label}-repeat.dll'
        for output,suffix in [(first,''),(second,'-repeat')]:
            run(['dotnet','run','--project',str(root/'scripts/takeover/PatcherNative17'),'--',str(source),str(output)]+args,f'native17-{label}{suffix}.log',env=dict(os.environ,GODSPVZ_RESOLVER=str(g['M'])))
        assert first.read_bytes()==second.read_bytes(),label+' nondeterministic'
        outputs[label]={'path':str(first),'input_sha256':sha(source),'sha256':sha(first)}
    result=replay('native17',Path(outputs['linked']['path']))
    targets={'System.Void FTRuntime.Internal.SwfList`1::AssignTo(System.Collections.Generic.List`1<T>)'}
    assert set(previous['result']['methods'])==targets,'native16 positive control differs'
    assert not targets.intersection(result['methods']),'native17 target still fails'
    report.update(status='NATIVE17_DIRECT_CONVERSION_PASS' if result['exit']==0 else 'NATIVE17_TARGET_CLOSURE_PASS',native16=previous['result'],native17=result,outputs=outputs,work_directory=str(work),elapsed_seconds=round(time.monotonic()-started,2),source_commit=subprocess.check_output(['git','rev-parse','HEAD'],cwd=root,text=True).strip())
    (work/'native17-result.json').write_text(json.dumps(report,indent=2)+'\n')
except Exception as e:
    report.update(status='FAILED',error=str(e));raise
finally:
    (base/'latest-result.json').write_text(json.dumps(report,indent=2)+'\n');print(json.dumps(report,indent=2),flush=True)
