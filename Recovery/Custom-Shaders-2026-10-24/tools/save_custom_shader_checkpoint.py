from pathlib import Path
import hashlib
import json
import shutil
import zipfile

W=Path(__file__).resolve().parent
ROOT=W.parents[1]
REPO=ROOT/'audit/repo-native113'
D=REPO/'Recovery/Custom-Shaders-2026-10-24'
D.mkdir(exist_ok=True)

def row(name,raw):
    return dict(file=name,bytes=len(raw),sha256=hashlib.sha256(raw).hexdigest())

def write_json(path,data):
    path.write_bytes((json.dumps(data,ensure_ascii=False,indent=2)+'\n').encode())

def archive(path,files):
    files=list(files)
    assert len(files)==len({n for n,b in files})
    files=dict(files)
    assert 'MANIFEST.json' not in files
    files['MANIFEST.json']=(json.dumps([row(n,b) for n,b in sorted(files.items())],indent=2)+'\n').encode()
    with zipfile.ZipFile(path,'w',zipfile.ZIP_DEFLATED) as z:
        for name,raw in sorted(files.items()):
            z.writestr(name,raw)
    with zipfile.ZipFile(path) as z:
        assert z.testzip() is None
        manifest=json.loads(z.read('MANIFEST.json'))
        assert {r['file'] for r in manifest}==set(z.namelist())-{'MANIFEST.json'}
        for r in manifest:
            assert row(r['file'],z.read(r['file']))==r

with zipfile.ZipFile(W/'R3-STARTUP-TEXT-40067634181.zip') as z:
    shader_rows=[]
    for name,guid in [('Unlit_GameUnitShader.shader','dce507989fabaa040b1384154c02f6ed'),('Shader Graphs_Brightness.shader','c77d3165efc3124408daf752fe07f184')]:
        original=z.read('project-text/Assets/Shader/'+name)
        meta=z.read('project-text/Assets/Shader/'+name+'.meta')
        shader_rows.append(dict(file=name,guid=guid,original_dummy_sha256=hashlib.sha256(original).hexdigest(),original_meta_sha256=hashlib.sha256(meta).hexdigest()))
    pinned=['Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl','Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl','Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl']
    references={n:z.read('project-text/'+n) for n in pinned}
includes=[dict(file=n,guid=hashlib.sha256(('GodsPVZ/RecoveredShader/R1/'+n).encode()).hexdigest()[:32]) for n in ['RecoveredGameUnit.hlsl','RecoveredBrightness.hlsl']]
manifest=dict(scope='TWO_CUSTOM_SHADER_SOURCES_ONLY',files=[row('Shaders/'+p.name,p.read_bytes()) for p in sorted((D/'Shaders').iterdir()) if p.suffix in ('.shader','.hlsl')],shaders=shader_rows,includes=includes,
              original_shader_state_evidence_sha256=hashlib.sha256((W/'custom-shader-evidence/ORIGINAL-PROPERTIES-AND-STATES.json').read_bytes()).hexdigest(),
              pinned_reference_sources=[row(n,b) for n,b in references.items()])
write_json(D/'SOURCE-MANIFEST.json',manifest)

evidence=W/'custom-shader-evidence'
archive(D/'ORIGINAL-SHADER-EVIDENCE.zip',[(('SOURCE-OBJECT-MANIFEST.json' if p.name=='MANIFEST.json' else p.name),p.read_bytes()) for p in evidence.iterdir() if p.is_file()]+[('original-dxbc/'+p.name,p.read_bytes()) for p in (evidence/'original-dxbc').iterdir() if p.is_file()]+[('pinned-r3-reference/'+n,b) for n,b in references.items()])
tools=D/'tools';tools.mkdir(exist_ok=True)
for name in ['extract_pc_shader_dxbc.py','d3d11_shader_oracle.py','extract_custom_shader_evidence.py','extract_shader_programs.py','extract_shader_states.py','save_custom_shader_checkpoint.py','validate_shader_workcopy.py','local_shader_helper_api.py']:
    shutil.copyfile(W/name,tools/name)
shutil.copyfile(W/'shader-r1-validation/D3D11-ORACLE-RESULT.json',D/'D3D11-ORACLE-RESULT.json')
shutil.copyfile(W/'shader-r1-validation/WORKCOPY-ISOLATION.json',D/'WORKCOPY-ISOLATION.json')
shutil.copyfile(W/'FIRST-ORACLE-BINDING-FAILURE.zip',D/'FIRST-ORACLE-BINDING-FAILURE.zip')
validation=W/'shader-r1-validation'
archive(D/'LOCAL-EXECUTION-EVIDENCE.zip',[(p.relative_to(validation).as_posix(),p.read_bytes()) for p in validation.rglob('*') if p.is_file()])
current_path=D/'CURRENT-RESULT.json'
if not current_path.exists():
    write_json(current_path,dict(status='ACTUAL_PC_SHADER_RASTER_PASS_UNITY_METAL_AND_RENDER_PENDING',dll_sha256='752939d1ae69877ef457945f6e0dfe61c749e8d4657e720075adcbcc76cc2e7f',
                                original_pc_dxbc='EXECUTED_IN_WARP',controlled_raster_cases=328,nonempty_cases=258,physical_negative_controls=9,
                                unity_import='PENDING',metal_compile='PENDING',metal_device='NOT_RUN',game_pixel_equivalence='NOT_PROVEN',ipa=None))
readme=D/'README.md'
if not readme.exists():
    readme.write_bytes('''# Custom Shader R1 — original-backed restoration, 2026-10-24

The exact Startup R3 DLL stays unchanged (`752939d1…cc2e7f`). This checkpoint changes only two custom shader source assets and adds two local HLSL includes in a disposable Unity work copy. It does not claim a repaired IPA or device acceptance.

## Source and execution evidence

Original PC `resources.assets` Shader path IDs 871 (Brightness) and 886 (GameUnit) are bound by raw object SHA-256. The matching Android objects provide actual GLSL for the mobile path. Full properties/pass states are retained independently of UnityPy's generic export, which drops program code and blend state. The PC DXBC extractor validates each indexed entry and DXBC length/chunk boundary, then uses Microsoft's D3DDisassemble. All 38 original GPU programs and their disassembly are archived.

`D3D11-ORACLE-RESULT.json` records actual Windows D3D11 WARP execution of the original DXBC and the restored `.shader` vertex/fragment entry bodies plus exact shared HLSL. The adapter supplies controlled original constant registers, matrices, texture sampling and vertex declarations. 328 complete 32×32 float32 raster comparisons pass the declared tolerance; 258 write non-clear pixels. Coverage includes GameUnit skew/rotation, scale, UV, vertex/material color, exposure, alpha and hidden branch; all 16 Brightness light combinations; normals and forward; negative color and alpha-zero paths; both direct output and original alpha blending. The 9 physical wrong shader variants are rejected only when their baselines pass.

This is scoped shader execution evidence. Unity import/binding/pass selection, GLES/Metal precision and whole-game pixels require separate checks. Metal shader cross compilation is not Metal execution on an iPad.

## Preserved behavior

GameUnit ignores incoming vertex Z/W, uses radian angles, returns sampled RGB and zero alpha when `_IsVisible == -1`, and uses SrcAlpha/OneMinusSrcAlpha for both color and alpha. Brightness preserves its 3 original pass tags, per-pass alpha behavior and separate alpha blend (One/OneMinusSrcAlpha). Lit discards exactly zero alpha and clamps negative output; Forward does neither. Normals alpha uses texture alpha × `_Transparent`, independently of vertex alpha.

Original GUIDs stay byte-identical: GameUnit `dce507989fabaa040b1384154c02f6ed`; Brightness `c77d3165efc3124408daf752fe07f184`. Material/scene references and game DLL bytes are not changed by shader installation. The installer checks original dummy source/meta hashes and refuses overwritten inputs or include GUID collisions.

## Honest first failure and verifier correction

The first WARP fixture bound `_RendererColor` to one fixed constant register for all Brightness variants. Original DXBC uses 129+3×enabledLights, or 128 for Forward. This produced 194 differences; the old negative-control summary also lacked a baseline-pass prerequisite. `FIRST-ORACLE-BINDING-FAILURE.zip` preserves that failed tool, raw report and generated shaders. The corrected adapter is checked against every original VS disassembly; a mutant cannot pass its rejection gate when its baseline fails. No shader implementation was changed to accommodate the faulty adapter.

## Reproduction

On Windows x64 with Python 3.12 and system D3D11/D3DCompiler_47, unpack `ORIGINAL-SHADER-EVIDENCE.zip` into a new directory, then run:

```text
python tools/d3d11_shader_oracle.py --sources Shaders --original <unpacked>/original-dxbc --output <new-output-directory>
```

The extractor requires UnityPy 1.25.2 with its dependencies and the authoritative PC ZIP. `SOURCE-MANIFEST.json` binds all actual shader inputs. `LOCAL-EXECUTION-EVIDENCE.zip` retains generated fixture HLSL/DXBC, results, compiler logs and local Editor API compile evidence; its member manifest is exact. Legacy parser outputs quarantined earlier are excluded from accepted program evidence.

## Unity gate and remaining work

Registered workflow `startup-r2-editor-diagnostic.yml` retains the exact R3 candidate and original project/package/font/splash fixes. It now installs these sources only in the work copy, compiles the additional helper before license acquisition, requests 76 stage/platform/variant compilations (OpenGLCore and iOS Metal), reads 20 controlled real Unity renders, requires a deliberately broken fragment shader to fail, then repeats normal MainMenu Play Mode. Shader bytecode/images are uploaded separately from text sanitization so their bytes cannot be corrupted.

Read `CURRENT-RESULT.json` for live qualification state. Known remaining method/access defects remain open. MainMenu startup, controlled graphics, gameplay and actual iPad behavior are different acceptance scopes. Do not promote this checkpoint based on a green workflow name alone.
'''.encode())
rows=[row(p.relative_to(D).as_posix(),p.read_bytes()) for p in sorted(D.rglob('*')) if p.is_file() and p!=D/'MANIFEST.json']
write_json(D/'MANIFEST.json',rows)
print(json.dumps(dict(checkpoint=str(D),files=len(rows),source_files=len(manifest['files']))))
