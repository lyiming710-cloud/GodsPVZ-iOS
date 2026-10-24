from pathlib import Path
import hashlib
import importlib.util
import json
import re
import shutil
import zipfile

W=Path(__file__).resolve().parent;R=W.parents[1]/'audit/repo-native113'
spec=importlib.util.spec_from_file_location('installer',R/'scripts/actions/prepare_custom_shaders_r1.py')
installer=importlib.util.module_from_spec(spec);spec.loader.exec_module(installer)
base=W/'shader-r1-validation/workcopy-fixtures'
base.mkdir(exist_ok=True)

def digest(root):
    return {p.relative_to(root).as_posix():hashlib.sha256(p.read_bytes()).hexdigest() for p in root.rglob('*') if p.is_file()}

def fixture(name):
    path=base/name
    if path.exists():
        raise ValueError('Preserve prior fixture: '+name)
    project=path/'project';(project/'ProjectSettings').mkdir(parents=True)
    (project/'ProjectSettings/ProjectVersion.txt').write_bytes(b'm_EditorVersion: 2022.3.44f1c1\n')
    with zipfile.ZipFile(W/'R3-STARTUP-TEXT-40067634181.zip') as z:
        for filename in ['Unlit_GameUnitShader.shader','Shader Graphs_Brightness.shader']:
            for suffix in ('','.meta'):
                out=project/'Assets/Shader'/(filename+suffix);out.parent.mkdir(exist_ok=True,parents=True)
                out.write_bytes(z.read('project-text/Assets/Shader/'+filename+suffix))
    untouched=project/'Assets/Plugins/GodsPVZRuntime1.dll';untouched.parent.mkdir()
    untouched.write_bytes(b'CONTROLLED_UNTOUCHED_PLUGIN')
    (project/'Assets/keep.mat').write_bytes(b'CONTROLLED_UNTOUCHED_SERIALIZED_REFERENCE\n')
    return project,path/'evidence'

rows=[]
project,evidence=fixture('positive')
before=digest(project);result=installer.install(project,evidence);after=digest(project)
changed=[n for n in before if before[n]!=after[n]]
added=sorted(set(after)-set(before))
assert sorted(changed)==['Assets/Shader/Shader Graphs_Brightness.shader','Assets/Shader/Unlit_GameUnitShader.shader']
assert added==sorted('Assets/Shader/'+n+s for n in ['RecoveredBrightness.hlsl','RecoveredGameUnit.hlsl'] for s in ('','.meta'))
rows.append(dict(name='positive-exact-workcopy-isolation',pass_=True,changed=changed,added=added,unchanged_files=len(before)-2))

for name in ['dummy-source-altered','shader-guid-altered','include-already-exists','include-guid-collision','source-manifest-mismatch']:
    project,evidence=fixture(name)
    original_source=installer.SOURCE
    if name=='dummy-source-altered':
        (project/'Assets/Shader/Unlit_GameUnitShader.shader').write_bytes(b'wrong shader')
    elif name=='shader-guid-altered':
        p=project/'Assets/Shader/Unlit_GameUnitShader.shader.meta';p.write_bytes(p.read_bytes().replace(b'dce507989fabaa040b1384154c02f6ed',b'00000000000000000000000000000000'))
    elif name=='include-already-exists':
        (project/'Assets/Shader/RecoveredGameUnit.hlsl').write_bytes(b'DO NOT OVERWRITE')
    elif name=='include-guid-collision':
        m=json.loads((installer.SOURCE/'SOURCE-MANIFEST.json').read_text());(project/'Assets/collision.meta').write_bytes(('guid: '+m['includes'][0]['guid']+'\n').encode())
    else:
        copied=project.parent/'source';shutil.copytree(installer.SOURCE/'Shaders',copied/'Shaders')
        shutil.copyfile(installer.SOURCE/'SOURCE-MANIFEST.json',copied/'SOURCE-MANIFEST.json')
        (copied/'Shaders/RecoveredBrightness.hlsl').write_bytes(b'bad source')
        installer.SOURCE=copied
    before=digest(project)
    try:
        installer.install(project,evidence)
        raise AssertionError('Invalid fixture was accepted: '+name)
    except ValueError as error:
        assert digest(project)==before
        rows.append(dict(name=name,pass_=True,rejected=str(error),no_project_mutation=True))
    finally:
        installer.SOURCE=original_source

states=json.loads((W/'custom-shader-evidence/ORIGINAL-PROPERTIES-AND-STATES.json').read_text())
for name in ['Unlit/GameUnitShader','Shader Graphs/Brightness']:
    pc=next(s for s in states if s['origin']=='PC' and s['shader']==name)
    android=next(s for s in states if s['origin']=='ANDROID' and s['shader']==name)
    assert pc['properties']==android['properties']
    for p,a in zip(pc['subshaders'][0]['passes'],android['subshaders'][0]['passes']):
        assert p['state']==a['state'] and p['tags']==a['tags']
    rows.append(dict(name=name+'-pc-android-property-pass-state-agreement',pass_=True))

bindings=[]
for variant in range(16):
    asm=(W/f'custom-shader-evidence/original-dxbc/Shader_Graphs_Brightness-program{5+variant}.asm.txt').read_text()
    slot=int(re.search(r'mul o2.xyzw, v4.xyzw, cb0\[(\d+)\]',asm).group(1))
    assert slot==129+3*variant.bit_count()
    bindings.append(dict(variant=variant,renderer_color_register=slot))
report=dict(scope='EXACT_INPUT_INSTALLATION_ISOLATION_AND_ORIGINAL_SHADER_STATE_CHECKS',pass_=all(r['pass_'] for r in rows),checks=rows,all_original_vs_renderer_bindings=bindings)
(W/'shader-r1-validation/WORKCOPY-ISOLATION.json').write_bytes((json.dumps(report,indent=2)+'\n').encode())
print(json.dumps(report,indent=2))
