# Damage::CalculateAD<T> — direct PC generic-native audit

Status: **generic native semantics established for recovery work; no production promotion**.

Authority is the original GodsPVZ 1.0.2 PC x86-64 IL2CPP release.

## Locked identity

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- managed method: `System.Single Damage::CalculateAD<T>(T host, System.Single attackPoint, System.Single defensePoint, DamageAttribute damageAttribute)`
- Assembly-CSharp MethodDef: `0x06000111`
- global metadata type-definition index for `Damage`: `5125`
- global metadata method-definition index for `CalculateAD`: `42038`

The current damaged managed body retains the intended scalar tail but contains Cpp2IL-generated fake pointer/type operations in the generic type test. It is the direct IL2CPP blocker after native11.

## Generic MethodSpec mapping

The original MetadataRegistration is `0x1818C6D00`. Its relevant locked tables are:

- generic-inst count `0x161B` (`5659`), table `0x181679AE0`
- generic-method-functions count `0xEF60` (`61280`), table `0x1817D7700`
- method-spec count `0x1242D` (`74797`), table `0x181684EF0`

The CodeRegistration at `0x1815E88C0` contains `0xEEBE` (`61118`) generic method pointers at `0x181760110`.

For global method-definition index `42038`, the method-spec table contains:

| MethodSpec index | class inst | method inst | resolved generic argument |
|---:|---:|---:|---|
| `64720` | `-1` | `85` | shared reference representation (`System.Object`) |
| `64721` | `-1` | `3607` | `Unity.IL2CPP.Metadata.__Il2CppFullySharedGenericType` |
| `74012` | `-1` | `2547` | `Zombie` (shares the reference-type implementation and therefore has no independent generic-function row) |

The generic-method-functions table maps the first two specs as follows:

| MethodSpec | genericMethodPointer index | invoker index | adjustor thunk | native VA |
|---:|---:|---:|---:|---:|
| `64720` | `57514` | `9751` | `-1` | `0x180434460` |
| `64721` | `57515` | `9757` | `-1` | `0x180434280` |

This independently explains why the method must be audited through the generic tables rather than `Assembly-CSharp_CodeGenModule.methodPointers[RID-1]`.

## Exact native extents

The two generic shared implementations have normal non-chained `.pdata` records:

- reference-shared: `0x180434460–0x180434520`, 192 bytes, SHA256 `a31476d0aeae5eacae566ddf469ae9a78df098381a45c68fc13de786daf020dd`
- fully-shared value-type: `0x180434280–0x180434453`, 467 bytes, SHA256 `755675ec4a32581a56792b5412fb37861b86e1546236700eb2a5a48b7776b9b4`

The longer fully-shared body contains IL2CPP generic-data copying/boxing helpers. Those helpers implement the same source-level type test for a value-type representation; they are not additional damage arithmetic.

## Direct native semantics

The reference-shared body directly establishes the observable algorithm. The fully-shared body reaches the same arithmetic after IL2CPP's generic boxing/type machinery.

Equivalent managed semantics are:

```text
if (damageAttribute == DamageAttribute.real) // numeric value 4
    return attackPoint;

object boxedHost = (object)host;
if (boxedHost == null)
    return attackPoint;
if (!(boxedHost is Zombie))
    return attackPoint;

if (defensePoint < attackPoint)
    return (attackPoint - defensePoint) + defensePoint * 0.1f;

return attackPoint * 0.1f;
```

Native evidence:

- both native bodies compare `damageAttribute` with immediate `4` before the host/type logic;
- reference-shared checks a null host before the `Zombie` class-hierarchy test;
- the class test uses the `Zombie` TypeInfo used by the managed body's surviving `ldtoken Zombie` path;
- constant `0x1815A7C08` is IEEE-754 `0.1f`;
- when `defensePoint < attackPoint`, the native sequence computes `attackPoint - defensePoint + defensePoint * 0.1f`;
- otherwise it computes `attackPoint * 0.1f`;
- non-`Zombie` hosts return `attackPoint` unchanged.

The damaged managed tail independently preserves the same comparison and arithmetic, while its generic type-test region is Cpp2IL pointer-lifting debris. This agreement is corroboration, not the authority for the native mapping.

## Recovery constraint

Patch only MethodDef `0x06000111`. Use legal generic CIL (`box T`, null test, `isinst Zombie`) instead of attempting to preserve the fake unmanaged-pointer operations. Preserve a single scalar float return merge, MVID/counts, and all non-target method semantics. Start from the exact native11 candidate so all prior target closures remain intact. Require deterministic double materialization and direct post-Linker IL2CPP target closure before any full Unity/iOS run.
