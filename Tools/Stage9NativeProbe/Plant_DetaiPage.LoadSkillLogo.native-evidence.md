# Plant_DetaiPage.LoadSkillLogo PC native authority

Source authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Method: `Plant_DetaiPage.LoadSkillLogo()`
- Managed token: `0x06000668`
- RID: `1640`
- Assembly-CSharp CodeGenModule pointer-table entry: `0x181B86098`
- Native method pointer: `0x1803A41D0`
- `.pdata` function range: `0x1803A41D0-0x1803A444E`
- Native function size: `638` bytes
- Native slice SHA256: `cc2dbd31e0ffb8e304825a7cfa00fb876f9ae902b01cf4192d6fd031ed59a3bf`

## Recovered semantics

The PC method first applies UnityEngine.Object truthiness to `this.plant`.

When `plant` is live:

1. Read `this.p_skill` and `this.plant`.
2. Read `p_skill.ID`, `p_skill.name`, and `plant.ID`.
3. Compute the ResourceManager skill-logo id as `p_skill.ID + (plant.ID * 3)`.
   - Native sequence: `lea ecx,[r14+rdi*2]` followed by `add ecx,edi`.
4. Call `ResourceManager.LoadSkillLogo(id, p_skill.name)` and retain the returned `Sprite`.
5. Enumerate every `UnityEngine.UI.Image` in `this.skillLogo` and assign the returned sprite to `Image.sprite`.

When `plant` is not live:

1. Enumerate every `UnityEngine.UI.Image` in `this.skillLogo`.
2. Assign `null` to every `Image.sprite`.

The native code uses the normal `List<Image>.Enumerator` lowering, including `MoveNext` and enumerator cleanup. For CLR reconstruction an index loop over the same stable `skillLogo` list is semantically equivalent here: the loop body only calls `Image.sprite` and cannot mutate the list. This avoids reusing the damaged generic-enumerator metadata in the recovered managed body while preserving the observable PC behavior and exception behavior for null list/image references.

## Damaged managed fingerprint on runtime-qualified 273d base

Input DLL SHA256: `273ddef385775d169aa88d63a505aeb2b99aab4b8c32204a4929175310e2969b`

- token `0x06000668`
- RID `1640`
- managed RVA `0x8BEF4`
- managed code size `440`
- locals `21`
- semantic fingerprint `1e5c936e7d9caf9cb11a02f4ebfe813130db2de66fe305207415005e71453c37`
- runtime failure: `InvalidProgramException: Invalid IL code in Plant_DetaiPage:LoadSkillLogo (): IL_005f: stloc 6`
- damaged instruction: integer `plant.ID * 2` is stored into local 6 typed `System.Object`; subsequent arithmetic also mixes `System.Object` and `Int32`.
- damaged generic enumeration contains placeholder `Method not found @1808197F0` and incorrect `List<object>.Enumerator` locals.

## Runtime promotion evidence

Exact-R3 corrected-lifecycle run `34833389193` / job `103941653511` on candidate `273d...` proved the previous `Plant_DetaiPage.Initialize(Plant)` blocker is gone and promoted `LoadSkillLogo()` as the next first causal Board-path blocker:

`Plant_DetaiPage.LoadSkillLogo -> Plant_DetaiPage.Initialize -> Board.Awake -> BoardManager.LoadBoard -> PrepareUIController.GameStart`.

Do not substitute later Dialogue/Update failures until this blocker is runtime-cleared.
