#!/usr/bin/env python3
"""Native16 exact-seed replay; target closure is not a full Unity export."""
from pathlib import Path
import hashlib, json, runpy, subprocess, time, os
root=Path(__file__).resolve().parents[2]
base=root/'.validation/native16';base.mkdir(parents=True,exist_ok=True)
report={'status':'RUNNING','full_unity_export':False,'production_promotion':False}
(base/'latest-result.json').write_text(json.dumps(report))
started=time.monotonic()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
try:
    previous=runpy.run_path(str(root/'scripts/codespaces/validate_native15.py'))
    g=previous['g'];work=previous['work'];run=previous['run'];replay=previous['replay']
    expected={'unlinked':'3416340aa16234f853f0d50e13c34b4ef382d1233c351ef3b3fcd918abed5d97','linked':'542370a6b40adb7df92fa3bfa7384f867a035b29f3685152ac9a3bc809f64259'}
    outputs={}
    for label,args in [('unlinked',[]),('linked',['linked'])]:
        source=Path(previous['outputs'][label]['path']);assert sha(source)==expected[label],label+' native15 differs'
        first=work/f'native16-{label}.dll';second=work/f'native16-{label}-repeat.dll'
        for output,suffix in [(first,''),(second,'-repeat')]:
            run(['dotnet','run','--project',str(root/'scripts/takeover/PatcherNative16'),'--',str(source),str(output)]+args,f'native16-{label}{suffix}.log',env=dict(os.environ,GODSPVZ_RESOLVER=str(g['M'])))
        assert first.read_bytes()==second.read_bytes(),label+' nondeterministic'
        outputs[label]={'path':str(first),'input_sha256':sha(source),'sha256':sha(first)}
    result=replay('native16',Path(outputs['linked']['path']))
    targets={'System.Void FTRuntime.Internal.SwfAssocList`1::Remove(T)','System.Void VFXAnimationEvent::SetSorting(ParticleState,T)'}
    assert set(previous['result']['methods'])==targets,'native15 positive control differs'
    assert not targets.intersection(result['methods']),'native16 target still fails'
    assert not any('SwfList`1::UnorderedRemoveAt' in s for s in result['methods']),'dependent target fails'
    report.update(status='NATIVE16_DIRECT_CONVERSION_PASS' if result['exit']==0 else 'NATIVE16_TARGET_CLOSURE_PASS',native15=previous['result'],native16=result,outputs=outputs,work_directory=str(work),elapsed_seconds=round(time.monotonic()-started,2),source_commit=subprocess.check_output(['git','rev-parse','HEAD'],cwd=root,text=True).strip())
    (work/'native16-result.json').write_text(json.dumps(report,indent=2)+'\n')
except Exception as e:
    report.update(status='FAILED',error=str(e));raise
finally:
    (base/'latest-result.json').write_text(json.dumps(report,indent=2)+'\n');print(json.dumps(report,indent=2),flush=True)
