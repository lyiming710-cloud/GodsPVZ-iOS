# Custom Shader R1 — original-backed restoration, 2026-10-24

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

## First real Unity readback: 40091379142

All 76 OpenGLCore/Metal stage compilations succeeded, the physical broken-fragment control was rejected, and the asset gate had no unresolved custom dummy shaders. Pixel readback failed: GameUnit expected color omitted the original Linear-project Color-property upload conversion; Brightness selected the error fallback in the pre-Play probe; uninitialized URP is the leading explanation to verify, not a proven game-side pipeline defect. The original GraphicsSettings already binds the preserved pipeline asset GUID 22c2bf3291e4d8f4db5d66a87a6f8123. The next probe requires normal MainMenu Play Mode and records the actual configured pipeline/instance/tag; it may not substitute a pipeline or force the tag. Shader sources remain unchanged. Full first-run evidence, binary programs, pixel images and exact helper are retained in FIRST-UNITY-RENDER-FAILURE-40091379142.zip. The first failure is retained as FAIL.
