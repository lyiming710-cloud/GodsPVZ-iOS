# HF5 — Zombie.Start + Zombie.LoopAddAnimation native recovery

Date: 2026-09-08

## Scope

HF5 is an incremental patch over the audited HF4 DLL. It changes exactly two managed MethodDefs:

- `Zombie.Start()` — original RID 1052, token `0x0600041C`, PC native `0x1803695F0`
- `Zombie.LoopAddAnimation(Transform)` — original RID 1055, token `0x0600041F`, PC native `0x180365BA0`

Final HF5 DLL SHA-256:

`58fe2001b6d9df986a08a930e60ea936fbf9392d82532f2e815bda63557d5e3a`

HF4 input SHA-256:

`2abbc9eb02b93bcd0178091bbc38ca450b6162875df9dcb55e874d5b9af9f0a6`

## Native-backed behavior — Zombie.Start

PC x86-64 establishes the following ordering, retained by HF5:

1. Resolve `animationGroup.transform` and call `Zombie.LoopAddAnimation`.
2. Get and store `Animator`.
3. For IDs `{0,2,4,5,6,7,8,9,11,12,14,15,19,20,21,22}`, set Animator integer `Group` to `Random.Range(0,2)`.
4. Set Animator float `Speed` from `GetRandenAnimationSpeedMagnification()`.
5. Call `elementManager.CreateNewElements<Zombie>(this)`.
6. Read `this.transform.position` separately for `fX` and `fY`.
7. Compute `fZ` from `animationGroup.transform.position.y - this.transform.position.y`.
8. Write `previousPosition.x`, then `.y`, then recompute and write `.z` from animation-group Y minus previousPosition Y.
9. Call `SetUIs()`.
10. If `board != null`, get `ZombieManager.BoardEntryData` and apply HP/attack/defense/element-resistance scaling in native order.
11. If armor1 exists, apply HP/defense/toughness scaling to armor1.
12. If armor2 exists, apply HP/defense/toughness scaling to armor2.
13. Call `Start_Characteristic()`.
14. If `prePath == null || prePath.Count == 0`, call `CreateStartPrePath()`.

HF5 intentionally preserves the multiple independent Transform/position reads rather than collapsing them into a single cached Vector3.

## Native-backed behavior — Zombie.LoopAddAnimation

PC x86-64 shows:

1. `parent.GetComponent<SpriteRenderer>()`.
2. Unity Object truth test; if valid, add `parent.gameObject` to `animationSprites`.
3. Enumerate child transforms through `Transform.GetEnumerator()` / `IEnumerator`.
4. Recursively call `Zombie.LoopAddAnimation(child)` for each child.
5. Preserve the native enumerator cleanup semantics using a `try/finally`, `isinst IDisposable`, and conditional `Dispose()`.

This is intentionally stricter than the earlier behavior-equivalent Plant helper, which did not reproduce the finally/dispose shape.

## Validation

### Patcher

HF5 patcher workflow run `34177036273` completed successfully.

Artifact digest:

`sha256:af7aa42055e3bc25b269ed1af1a1b0970c76906bcc56f35bc0ef5070cb35cc16`

Patcher reopen verification:

- `Zombie.Start`: 229 IL instructions, 859 bytes
- `Zombie.LoopAddAnimation`: 41 IL instructions, 151 bytes, exactly one finally handler

### Independent Mono.Cecil

Recovery auditor commit: `3431792dbf29a83c6c041944fb755abb797d4230`

Audit workflow run: `34177457311`

Both OPEN1 and OPEN2 independently reopen the final HF5 DLL and validate:

- `Zombie.Awake`: 239 IL
- `Zombie.Start`: 229 IL
- `Zombie.LoopAddAnimation`: 41 IL
- `Zombie.InjuryStatusUpdate_Body`: 334 IL

as well as the previously guarded Plant/Card/MouseManager methods.

### Independent ILSpy

ILSpyCmd `11.0.0.9375` was run with the recovered Unity dependency directory as `--referencepath`.

- `Zombie.Start` token `0x0600041C`: stderr 0, decompiler warnings 0
- `Zombie.LoopAddAnimation` token `0x0600041F`: stderr 0, decompiler warnings 0

A prior no-referencepath type-wide decompile produced `Unknown result type` annotations. Repeating the exact same DLL decompile with the Unity dependency reference path removed all such annotations; the DLL hash did not change. Therefore those annotations were resolver-environment artifacts, not invalid HF5 IL.

### Semantic isolation

Full IL was exported for HF4 and HF5. After normalizing method RVAs and physical `.data` labels, the entire assembly diff contains exactly two hunks:

1. `Zombie.Start`
2. `Zombie.LoopAddAnimation`

No other MethodDef changes semantically.

## Confidence

- `Zombie.Start`: **Exact**
- `Zombie.LoopAddAnimation`: **Exact**

The classification is based on PC native control/data flow, constants, call ordering, independent Cecil reopening, independent ILSpy decompilation, and whole-assembly semantic isolation.
