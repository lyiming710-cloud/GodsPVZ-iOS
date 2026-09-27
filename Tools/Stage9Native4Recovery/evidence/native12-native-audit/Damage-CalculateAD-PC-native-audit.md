# Damage::CalculateAD<T> — direct PC-native generic audit

Status: **generic-sharing semantics established; recovery remains experimental; no production promotion**.

Authority is the original GodsPVZ 1.0.2 PC x86-64 IL2CPP release.

## Locked identity

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- managed method: `System.Single Damage::CalculateAD<T>(T host,System.Single attackPoint,System.Single defensePoint,DamageAttribute damageAttribute)`
- MethodDef: `0x06000111`, local RID `273`
- declaring TypeDef: `Damage`, `0x02000025`
- generic parameter: one unconstrained type parameter `T`
- current damaged managed body: 326 bytes

The ordinary Assembly-CSharp method-pointer slot for local RID 273 is null. This is expected for this generic definition; recovery must not invent a normal RID-to-native mapping.

## Global-metadata mapping

For metadata v31, the Assembly-CSharp image is image index 7:

- image name: `Assembly-CSharp.dll`
- `typeStart = 5088`
- `typeCount = 319` in the IL2CPP image definition

The `Damage` global type-definition record is index 5125 and has global `methodStart = 42032`. `Damage::CalculateAD<T>` is the seventh method on that type, therefore its global method-definition index is `42038`.

The PC `Il2CppMetadataRegistration` at `0x1818C6D00` contains:

- `genericInstsCount = 5659`, generic-inst pointer table `0x181679AE0`
- `genericMethodTableCount = 61280`, table `0x1817D7700`
- `methodSpecsCount = 74797`, table `0x181684EF0`

Three MethodSpec records reference global method-definition index 42038:

| MethodSpec index | class inst | method inst index | resolved generic argument |
|---:|---:|---:|---|
| 64720 | -1 | 85 | `System.Object` reference-sharing instantiation |
| 64721 | -1 | 3607 | `Unity.IL2CPP.Metadata.__Il2CppFullySharedGenericType` value-sharing instantiation |
| 74012 | -1 | 2547 | concrete `Zombie` instantiation |

MethodSpec 64720 maps through generic-method-table entry 57671 to generic method-pointer index 57514. MethodSpec 64721 maps through entry 57672 to generic method-pointer index 57515. The concrete Zombie MethodSpec shares one of these generic implementations rather than owning a separate ordinary MethodDef pointer.

The `Il2CppCodeRegistration` at `0x1815E88C0` reports `genericMethodPointersCount = 61118`, pointer array `0x181760110`.

## Exact native generic-sharing bodies

### Reference-sharing implementation

- generic pointer index: `57514`
- native VA: `0x180434460`
- `.pdata`: `0x180434460–0x180434520`
- length: 192 bytes
- exact SHA256: `a31476d0aeae5eacae566ddf469ae9a78df098381a45c68fc13de786daf020dd`

### Fully-shared value-type implementation

- generic pointer index: `57515`
- native VA: `0x180434280`
- `.pdata`: `0x180434280–0x180434453`
- length: 467 bytes
- exact SHA256: `755675ec4a32581a56792b5412fb37861b86e1546236700eb2a5a48b7776b9b4`

The longer fully-shared body contains the expected generic-value load/boxing machinery. After that generic machinery, both native implementations implement the same observable rule.

## Direct PC-native semantics

`DamageAttribute.real` is enum value `4`. The exact observable semantics are equivalent to:

```text
if (damageAttribute == DamageAttribute.real)
    return attackPoint;

if (host == null)
    return attackPoint;

if (!(host is Zombie))
    return attackPoint;

if (defensePoint < attackPoint)
    return (attackPoint - defensePoint) + defensePoint * 0.1f;

return attackPoint * 0.1f;
```

The reference-sharing body at `0x180434460` makes this especially explicit:

- compare `damageAttribute` with integer 4; equal returns the original attack value;
- null-test the generic host; null returns attack unchanged;
- perform the IL2CPP runtime type test against the `Zombie` class; failure returns attack unchanged;
- compare defense and attack;
- for `defense < attack`, compute `attack - defense + defense * 0.1f`;
- otherwise compute `attack * 0.1f`.

The fully-shared value-type body at `0x180434280` performs generic-value copying/boxing before the same Zombie type test and the same floating-point branches. This confirms that a typed managed reconstruction should express the host checks as `box T` + null/type test rather than attempting to preserve Cpp2IL's fake native-pointer locals.

The `0.1f` constant used by both bodies is the same PC constant loaded from the original image.

## Current managed corruption

The native10 inspection run `36310886375` locked the target as:

- MethodDef `0x06000111`
- generic parameter `T` with no constraints
- parameters: `T host`, `float attackPoint`, `float defensePoint`, `DamageAttribute damageAttribute`

The damaged body contains Cpp2IL artifacts such as fake unmanaged-memory strings, `IntPtr` locals, type-handle comparisons, and synthesized pointer arithmetic. The useful scalar tail (`attack-defense + defense*0.1` / `attack*0.1`) survived, but the generic host/type test is not valid managed CIL and is the source of the direct IL2CPP blocker.

## Recovery rule

Replace only MethodDef `0x06000111`. Preserve the generic method signature and generic parameter. Express host null/type semantics with managed generic boxing and `isinst Zombie`; preserve the exact branch order and float formulas above. Require deterministic materialization, non-target isolation, and direct post-Linker IL2CPP removal before any full Unity rebuild.
