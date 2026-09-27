# ElementManager::CreateNewElements<T> — direct PC-native generic audit

Status: **generic-sharing semantics established; recovery remains experimental; no production promotion**.

Authority is the original GodsPVZ 1.0.2 PC x86-64 IL2CPP release.

## Locked identity

- package SHA256: `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- `GameAssembly.dll` SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- `global-metadata.dat` SHA256: `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- managed method: `System.Void ElementManager::CreateNewElements<T>(T host)`
- MethodDef: `0x06000126`, local RID `294`
- declaring TypeDef: `ElementManager`, `0x0200002C`
- one unconstrained generic parameter `T`
- current damaged managed body: 1761 bytes / 506 IL instructions / 78 locals / 0 EH
- native12 inspection run: `36317617881`

The ordinary Assembly-CSharp MethodDef body is a Cpp2IL reconstruction containing fake native-pointer arithmetic and unresolved native calls. The PC generic-sharing tables, not that body, are authoritative for recovery.

## Actual managed callers

The exact candidate metadata contains only two constructed calls:

- `Plant::Start()` MethodDef `0x06000349` -> `CreateNewElements<Plant>(Plant)`
- `Zombie::Start()` MethodDef `0x0600041C` -> `CreateNewElements<Zombie>(Zombie)`

The involved managed TypeDefs are:

- `Plant`: `0x02000078`, RID 120
- `Zombie`: `0x02000084`, RID 132
- `ElementType`: `0x0200002A`
- `Element`: `0x0200002B`

`ElementType` currently has one named value, `fire_ice = 0`, but the original method dynamically enumerates `Enum.GetValues(typeof(ElementType))`; recovery must preserve that behavior rather than hard-code zero.

## Global-metadata mapping

For the Assembly-CSharp image, the previously locked image `typeStart` is `5088`. `CreateNewElements<T>` resolves to global method-definition index `42059`.

Four PC `Il2CppMethodSpec` records reference method-definition index `42059`:

| MethodSpec | class inst | method inst | resolved argument |
|---:|---:|---:|---|
| 64763 | -1 | 85 | `System.Object` reference-sharing |
| 64764 | -1 | 3607 | `Unity.IL2CPP.Metadata.__Il2CppFullySharedGenericType` |
| 74043 | -1 | 2453 | `Plant` |
| 74044 | -1 | 2547 | `Zombie` |

The concrete generic-inst identities are independently recoverable from the original `genericInst` table. Inst 2453 points at an `IL2CPP_TYPE_CLASS` record with global type-definition index 5207; `5207 - 5088 = 119`, which corresponds to managed RID 120 / `Plant` `0x02000078`. Inst 2547 points at global type-definition index 5219; `5219 - 5088 = 131`, corresponding to RID 132 / `Zombie` `0x02000084`.

The PC generic-method table uses 16-byte entries. For this target:

- MethodSpec 64763 -> generic-method-table entry 57710 -> generic method-pointer index 57553
- MethodSpec 64764 -> generic-method-table entry 57711 -> generic method-pointer index 57554
- concrete Plant/Zombie MethodSpecs do not own separate pointer entries; they share the generic implementations

The second 32-bit field of the generic-method-table entry is the generic method-pointer index. This layout is cross-checked against the previously locked `Damage::CalculateAD<T>` mapping.

## Exact PC native bodies

### Reference-sharing implementation — used by the real Plant/Zombie calls

- generic pointer index: `57553`
- native VA: `0x180439680`
- exact `.pdata`: `0x180439680–0x180439AB8`
- length: 1080 bytes
- unwind RVA: `0x1A9A698`
- exact SHA256: `191078e9a5434bd3f7d89c411ac132388ebef9799f154acf5889f9541291227e`

### Fully-shared value-type implementation

- generic pointer index: `57554`
- native VA: `0x1804390C0`
- exact `.pdata`: `0x1804390C0–0x18043967F`
- length: 1471 bytes
- unwind RVA: `0x1A9A6E4`
- exact SHA256: `64a719753c31e448656803a777561aeaf34e1197475ff6a4c5ce9b4c66627cb6`

The longer fully-shared body contains the expected generic value loading/boxing machinery. The only actual managed callers are reference types Plant and Zombie, so the reference-sharing implementation is the primary behavioral authority; the fully-shared body is retained as generic-definition corroboration.

## Exact observable semantics

The reference-sharing PC body at `0x180439680` establishes this behavior:

```text
zombie = host as Zombie;
plant = host as Plant;

IEnumerator it = Enum.GetValues(typeof(ElementType)).GetEnumerator();
try
{
    while (it.MoveNext())
    {
        ElementType type = (ElementType)it.Current;
        Element element = new Element(type, 0.0f, null, true);
        element.damage = null;
        element.manager = this;
        elements.Add(element);
    }
}
finally
{
    (it as IDisposable)?.Dispose();
}
```

Important details visible directly in native code:

1. `ElementManager` instance fields are written in declaration/runtime order:
   - `[this+0x10]` = `zombie`
   - `[this+0x18]` = `plant`
   - `[this+0x20]` = `elements`
2. A null host naturally leaves both typed fields null; there is no early return.
3. The two runtime type tests are independent `as`-style tests. They are not an `if/else` chain.
4. The method calls `Enum.GetValues(typeof(ElementType))` and then `System.Array::GetEnumerator()`; it does not hard-code the current sole enum value.
5. The enumerator is checked with `IEnumerator.MoveNext()` and its `Current` is unboxed as `ElementType`.
6. `Element::.ctor` is MethodDef `0x0600011C` with exact managed signature `Element::.ctor(ElementType,System.Single,Damage,System.Boolean)`.
7. Constructor arguments in the PC call are exactly: current enum value, `0.0f`, `null`, `true`.
8. Immediately after construction, native code explicitly clears `Element::damage` (`0x04000145`) again and assigns `Element::manager` (`0x04000144`) to the current manager.
9. `elements` is dereferenced and the new `Element` is appended using the normal `List<Element>` add path. A null `elements` therefore preserves normal managed null-reference behavior.
10. The loop-exit native path performs the equivalent of `(enumerator as IDisposable)?.Dispose()` before returning. That cleanup must be preserved.

The managed references already present in the candidate include:

- `System.Enum::GetValues(System.Type)` MemberRef `0x0A00007D`
- `System.Array::GetEnumerator()` MemberRef `0x0A0000EB`
- `System.Collections.IEnumerator::get_Current()` MemberRef `0x0A00015D`
- `System.Collections.IEnumerator::MoveNext()` MemberRef `0x0A000161`
- `System.IDisposable::Dispose()` MemberRef `0x0A0000CF`
- `System.Collections.Generic.List<Element>::Add(Element)` MemberRef `0x0A0000C8`

## Recovery rule

Replace only MethodDef `0x06000126`, preserving its generic signature and unconstrained `T`. Express the two host casts with managed boxing + `isinst`, preserve dynamic enum enumeration, constructor arguments, the explicit field writes, normal `List<Element>.Add`, and the `IDisposable` finally cleanup. Require deterministic materialization, unchanged MVID/counts, semantic isolation of all non-target MethodDefs, and direct post-Linker IL2CPP removal before any full Unity rebuild.
