# Device::.ctor PC native authority and CLR adaptation note

Read-only prefetch. Do not mutate `Device::.ctor` unless later runtime/coverage explicitly promotes this MethodDef.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- PC GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Managed input used for attribution: runtime-qualified SkillProgress v2 candidate `2b2fdf165076ff9f48c577b84fb4c62346b78c925e46c6bff5f8ec6a79f67ce5`
- Managed MethodDef: `Device::.ctor()`
- Token: `0x0600033A`
- RID: `826`
- Managed RVA: `0x417FC`
- Managed damaged code size: `205`
- Managed locals: `6`
- Managed fingerprint SHA256: `d1461ae245199e90caa215f0234c18940fff64ed66cfbf5b215ab0b3928af6e0`
- Private external field refs: `1`
- Method-pointer table entry VA: `0x181B84728`
- PC native target: `0x18034CAD0–0x18034CC66`
- Native span length: `406` bytes
- Native span SHA256: `40738cc7ee1cd160439bb04745a2279b7509e202ccc6e91d9cd7e6888fd2f699`

## PC semantics

The original IL2CPP constructor initializes the object in this order:

1. `camp = Camp.plant` (`1`).
2. Association-grid default includes `-1` in the grid/default-initialization region around object offset `+0x68`.
3. `maxIcePoint = 6000f` around object offset `+0x8C`.
4. Allocate fresh `List<DeviceEvent>` instances for `deviceEvents`, `deviceEvents_toAdd`, and `deviceEvents_toRemove` (object offsets `+0xA8`, `+0xB0`, `+0xB8`).
5. Allocate a fresh `List<GameObject>` for `animationSprites` (object offset `+0xD8`).
6. `brightIntensity = 1f` (around `+0x100`).
7. Allocate the nested `Device.HPUIController` object, initialize its `maxHPEffect` backing field to `1f` at nested-object offset `+0x28`, run its base constructor, and assign it to the Device HP-UI field (around Device offset `+0x120`).
8. Run the `UnityEngine.MonoBehaviour` base constructor.

No defensive null guards or exception swallowing are present in the PC authority.

## Current managed corruption

The Cpp2IL-managed constructor expresses the same broad initialization, but after `new Device.HPUIController()` it emits an additional direct write to the nested type's private `maxHPEffect` field. Mono rejects this with:

`FieldAccessException: Field 'HPUIController:maxHPEffect' is inaccessible from method 'Device:.ctor ()'`

The nested `Device.HPUIController::.ctor` itself is structurally healthy and already performs:

```text
this.maxHPEffect = 1f;
System.Object::.ctor();
```

Therefore the PC native direct field store is the native/inlined form of the nested constructor's initialization. Cpp2IL incorrectly preserved both the nested constructor call and the inlined private-field store.

## Required CLR adaptation if promoted

If runtime/coverage later promotes `Device::.ctor`, preserve the PC final state by:

- constructing `Device.HPUIController` once through its own constructor;
- assigning that object to the Device field;
- **omitting the redundant outer direct write to the nested private `maxHPEffect` field**;
- preserving field metadata and visibility exactly;
- not making `maxHPEffect` public/internal;
- not adding reflection, guards, or exception swallowing.

Generic `List<T>` constructor MemberRefs must also be treated under the project-wide exact Unity Mono corelib rule: build/resolve them against the locked Unity `mscorlib` open generic MethodDef rather than trusting a Cpp2IL MemberRef merely because ILSpy can display it.
