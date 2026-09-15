# Zombie::.ctor PC native authority

Read-only prefetch. Do not mutate this MethodDef unless a later runtime/coverage gate explicitly promotes `Zombie::.ctor()` as the next causal gameplay blocker.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- Source ZIP SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Assembly-CSharp method pointer table VA: `0x181B82D60`
- Managed MethodDef: `Zombie::.ctor()`
- Token: `0x060004BB`
- RID: `1211`
- Managed RVA on SkillProgress-v2 cheap candidate: `0x00066EA4`
- Managed damaged code size: `457` bytes
- Managed damaged locals: `14`
- Managed fingerprint SHA256: `a74da4d5f1d38e62b1a0e1ff96e91ba09871fd054134263c926dd46a8068c1ab`
- Private external field refs: `0`
- Pointer-table entry VA: `0x181B85330`
- Native method VA: `0x1803741D0`
- `.pdata` exact range: `0x1803741D0–0x1803744DD`
- Native span length: `781` bytes
- Native span SHA256: `84d27db79e7cf21970718ac79287badd239f62af082e008e02fea7e6cef88cb0`

## Why the reconstructed managed constructor is invalid

The current reconstructed body contains several independent IL2CPP-to-CLR lowering failures. The first runtime-visible one is:

```text
IL_004D ldstr "Unmanaged memory load: [1815A7B70]"
IL_0052 pop
IL_0053 ldc.i4.0
IL_0054 conv.i
IL_0055 stfld UnityEngine.Color Zombie::color
```

This attempts to store a native-sized integer into a `UnityEngine.Color` value-type field and produces the observed `InvalidProgramException ... IL_0055: stfld`.

The same body also contains three fake `UnityEngine.Vector3::zeroVector` field loads plus unmanaged-memory scaffolding. These are reconstruction artifacts around IL2CPP native static data, not valid CLR field access.

## PC native constructor semantics

The original x86-64 IL2CPP constructor performs the following initialization sequence before tail-calling the Unity `MonoBehaviour` base constructor:

1. `attackPoint = 100f`.
2. `isStant = true`.
3. `isOnBoard = true`.
4. `camp = (Camp)2`.
5. Allocate and assign `ElementManager`.
6. `waitingTime = 3f`.
7. Allocate and assign `prePath = new List<EnemyPath>()`.
8. Allocate and assign `path = new List<EnemyPath>()`.
9. Copy Unity native `Vector3.zero` into `rSpeed`.
10. Copy Unity native `Vector3.zero` into `rDirection`.
11. Copy Unity native `Vector3.zero` into `previousPosition`.
12. Allocate and assign `animationSprites = new List<GameObject>()`.
13. Copy the 16-byte constant at PC VA `0x1815A7B70` into `color`. Direct binary decoding of this constant as four little-endian IEEE-754 floats is exactly `(1.0f, 1.0f, 1.0f, 1.0f)`; therefore the CLR reconstruction must use a typed white `Color`, not pointer/integer scaffolding.
14. `brightIntensity = 1f`.
15. `brightIntensity_Armor2 = 1f`.
16. Allocate and assign `hpUIController = new HPUIController_Zombie()`.
17. Allocate and assign `elementUIControllers = new List<ElementUIController>()`.
18. Allocate and assign `buffManager = new BuffManager()`.
19. `updateRate = 1f`.
20. Tail-call the `UnityEngine.MonoBehaviour` base constructor.

Relevant native stores/calls are within the locked `0x1803741D0–0x1803744DD` span. The three Vector3 copies read the Unity `Vector3` static storage rather than a managed field named `zeroVector`.

## Recovery constraints if later promoted

- Reconstruct only token `0x060004BB`.
- Preserve all 123 `Zombie` fields and their access metadata exactly.
- Use legal typed CLR construction for `Color(1,1,1,1)` and `Vector3.zero` semantics; do not expose or synthesize fake Unity private/static fields.
- For generic `List<T>` constructors, use exact Unity Mono corelib MethodDefs / resolvable generic MemberRefs rather than copying Cpp2IL generic references blindly.
- Do not add defensive null guards or exception swallowing.
- Preserve constructor ordering and the base-constructor call.
- Require semantic isolation, field-metadata isolation, exact-reference readback, generic MemberRef `Resolve()` where applicable, and a runtime/coverage gate that actually constructs Zombies before qualification.
