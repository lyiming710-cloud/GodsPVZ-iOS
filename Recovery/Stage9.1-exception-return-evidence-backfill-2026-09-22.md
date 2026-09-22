# Stage9.1 exception-return evidence backfill — 2026-09-22

## Purpose

Cross-check the 157 Stage9.1 static exception-object-return hits (L001-L157) against the already archived high-fidelity native recovery chain HF3-HF55. This is an evidence classification pass only. It does not patch a DLL and does not promote any Stage9.1 candidate.

## Static-hit meaning

L001-L157 were selected because the recovered runtime contains suspicious tails of the form:

`newobj <Exception> -> stloc -> ldloc -> ret`

The pattern is not by itself proof that the original native code throws. A method is promoted from `static_hit` only when original-PC native evidence establishes the corresponding control-flow semantics.

## Direct formal-target cross-check

The archived HF3-HF55 native-backed target manifests/evidence were cross-checked by exact MethodDef token, not by method family, neighboring RID, caller/callee relationship, or similar method name.

Result:

- Direct exact-token overlap between the formal HF3-HF55 repair targets and L001-L157: **0**.
- Therefore none of the 157 Stage9.1 exception-return hits may be marked closed merely because it belongs to a subsystem previously recovered in HF3-HF55.
- This specifically prevents false closure from adjacent/sibling methods. Examples:
  - L003 `0x060000D7 AttackRange.TestInRects_Rect` is called by HF16 `0x060000D8 AttackRange.TestInRange<T>`, but L003 itself was not an HF16 formal target.
  - L059 `0x060002A5 EnemyPath.DistanceStatistics` is adjacent to HF49 `0x060002A3 EnemyPath.ArrivalTest`, but HF49 explicitly left `DistanceStatistics` as a residual candidate.
  - L070 `0x0600046A Zombie.Path_Finding` is called from multiple native-backed Zombie paths, but it is not HF50's `0x0600046B Zombie.Path_Test` and was not itself promoted by those HF stages.
  - L094 `0x06000704 Window_Q.PopupNewWindow(int,Transform,int)` and L095 `0x06000706 Window_Q.PopupNewWindow(int,Transform)` are siblings of HF53 target `0x06000705 Window_Q.PopupNewWindow(int,Transform,Board)`, not the same MethodDef.
  - L012 `0x0600013B GlobalStaticVars.CreateAudioAtPoint(AudioClip)` is not HF53 `0x0600013D` or HF34 `0x0600013E` overloads.

## Native-confirmed exception-tail set

Exactly three Stage9.1 hits currently have saved original-PC native samples demonstrating that the recovered direct exception-object return is semantically wrong:

1. L001 — `0x0600008C System2.FindGrid(int,int)` — core gameplay / grid — P0.
2. L002 — `0x060000D4 AttackRange.TestInCircles_Position(Vector3)` — core gameplay / range — P0.
3. L156 — `0x06000903 FlashTools.Examples.PurpleFlowerLogic.GetRandomIdleSequence(SwfClipController)` — example/demo code — P2.

For these three, the saved native sample terminates through the native exception helper / trap path instead of returning the constructed exception object as a normal method result. They remain candidate-only until the read-only Cecil control-flow audit confirms the replacement boundary is structurally safe.

## Existing HF evidence that raises priority without proving the tail

### L070 — Zombie.Path_Finding — `0x0600046A`

`Path_Finding` is reached by multiple already native-backed Zombie paths, including the formal Zombie update/attack/wake-up recovery chain. This establishes gameplay reachability and raises verification priority, but does **not** prove the internal L070 exception tail. Classification: `native_backed_call_reachable_only`, P0/P1 verification priority.

### L059 — EnemyPath.DistanceStatistics — `0x060002A5`

HF49 explicitly records `EnemyPath.DistanceStatistics` as a residual candidate requiring separate native closure while formally repairing `EnemyPath.ArrivalTest`. Classification: `explicit_hf_residual`, high pathing priority.

### L003 — AttackRange.TestInRects_Rect — `0x060000D7`

HF16 independently identifies this method as the native Rect primitive called by `AttackRange.TestInRange<T>`. That proves identity/call reachability, not the semantics of L003's own suspicious exception tail. Classification: `native_backed_callee_identity_only`, P0/P1 verification priority.

### L094/L095 — Window_Q PopupNewWindow overloads

HF53 formally recovered sibling overload `0x06000705 PopupNewWindow(int,Transform,Board)`. L094/L095 remain separate MethodDefs. Classification: `adjacent_native_sibling_only`, UI priority.

## Generic no-direct-pointer group

L146-L155 have original direct PC method pointer recorded as zero. They must not be validated with ordinary `RID-1 -> methodPointers[index]` attribution. They require shared generic attribution through MethodSpec/genericMethodFunctions and concrete callsites, analogous to the already formal shared-generic HF14/HF15/HF16/HF18 methodology.

## Priority for remaining native verification

P0/P1 gameplay first:

- L003-L009 AttackRange family.
- L014 BoardConfig grid position.
- L048-L059 zombie creation / A* / Grid / EnemyPath cluster.
- L060-L075 Device / Plant / Projectile / Zombie gameplay cluster.
- L083 AttackRangeUIController.GetAttackRange when needed by active damage/range paths.

Lower priority after gameplay closure:

- Save/Supplies/UI convenience methods.
- TMP examples and animation examples.
- FTRuntime helper/example surfaces unless active game callsites establish material gameplay reachability.
- L156 remains native-confirmed but P2 because it is FlashTools example code.

## Promotion rule

Do not bulk convert all 157 tails to `throw`.

For each promoted item require:

1. Exact MethodDef identity and original native attribution.
2. Native proof that the suspicious branch terminates as an exception path rather than a normal return.
3. Recovered-IL control-flow audit: exact constructor/local sequence, no unexpected external branch/switch entry, and no conflicting EH boundary.
4. Candidate patch only on a recognized work-copy hash; never modify the locked authoritative runtime artifact.
5. Cecil reopen / stack / EH validation.
6. Final Unity/IL2CPP regression gate before production promotion.

## Current status

- `native_confirmed_exception_tail`: L001, L002, L156.
- `formal_HF3_HF55_exact_target_overlap`: none.
- `explicit_hf_residual`: at least L059.
- `native_backed_call_reachable_only`: at least L070.
- Remaining items stay `unverified_static_hit` or a narrower evidence-only subclass until original-native closure exists.
- Production remains unchanged; this document is evidence-only on `stage9-local-audit-tools`.
