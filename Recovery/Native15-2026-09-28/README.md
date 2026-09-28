# Native15: Map and VFX generic MethodBody reconstruction

Experimental only. No production promotion or full Unity export is claimed.

Codespace validation at source commit `796221ea58a0ecb77bab586d6e51f408aa61580d`
reported **NATIVE15_TARGET_CLOSURE_PASS** in 129.0 seconds. The emitted CIL passed
24 CLR fixture assertions, two stack negative controls, and non-target isolation
for 2295 MethodBodies. Unlinked and linked outputs each reproduced byte-for-byte.

- Unlinked native15 SHA256: `3416340aa16234f853f0d50e13c34b4ef382d1233c351ef3b3fcd918abed5d97`
- Linked native15 SHA256: `542370a6b40adb7df92fa3bfa7384f867a035b29f3685152ac9a3bc809f64259`
- IL2CPP exit: **255**, with newly exposed `FTRuntime.Internal.SwfAssocList<T>.Remove`
  and `VFXAnimationEvent.SetSorting<T>` errors. Both repaired target errors are gone.
- Full Unity Actions was not started because these known conversion failures remain.

Authority: original PC `GameAssembly.dll` SHA256
`9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
and metadata SHA256
`ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`.
Input is native14 derived from the locked 18e44 baseline; the runner checks the
unlinked and linked input hashes before patching. Only 0x06000290 and 0x060000C4
are replaced. 0x0600028F, the other generic Map overload, is not changed.

## Native identity

MetadataRegistration `0x1818C6D00`, CodeRegistration `0x1815E88C0`.
MethodSpecs have 12-byte records; generic method functions have 16-byte records.
`mapping.json` records all spec/pointer identities and exact .pdata body hashes.
The extractor cross-checks both already verified ElementManager shared bodies.

| Target | global definition | reference spec / pointer | reference VA / size | fully shared VA / size |
|---|---:|---|---|---|
| Map four-argument generic |42421|66524 / 58645|0x1804910B0 / 1312|0x180490B20 / 1424|
| VFX Binding |41961|69280 / 60832|0x1804C4AB0 / 250|0x1804C4920 / 388|

Concrete PC instantiations also exist: Map spec 74481 -> inst 2393 -> global
type 5206 (Device); Binding spec 74722 -> inst 2453 -> global type 5207 (Plant).
Reference-sharing inst 85 is Object; fully-shared inst 3607 is the IL2CPP shared
generic type. The bodies and fields corroborate these mappings.

## Binding specification

Reference body 0x1804C4AF9 reads the fifth native argument (string) from stack,
writes this+0x48 (`type`), then writes float duration to this+0x44. If the bool
parameter is false, return. Otherwise box/as Plant; a failed cast returns. On
success assign this+0x28 (`plant`), load Plant+0x1D0 (`particleSystems`), obtain
Component.gameObject and call List<GameObject>.Add. A null particle list throws
after get_gameObject is evaluated. There is no write to the `original` field,
no zombie/projectile assignment and no clearing of plant on the early returns.
The raw null/type tests are not Unity Object.op_Equality calls.

The type check's RIP slot 0x181BB1FC8 contains encoded usage 0x200083DB -> type
index 16877 -> global type 5207 Plant. The fully shared body at 0x1804C4A01 boxes
the unconstrained argument and corroborates the same writes and control flow.

## Map specification

1. Allocate result and eligible List<Grid>, in that order.
2. Enumerate Map.rows (+0x10) using List<Row>.Enumerator. Each row starts x at
   argument 3. Re-evaluate GetMapX at every inner-loop test and compare x with
   min(GetMapX(), unchecked(end + 1)). Read row.grids (+0x18)[x].
3. Log grid.gridX (+0x1C).ToString() + "," + grid.gridY (+0x20).ToString() before
   testing the host. Box/as Device. If non-null and Grid.CanPlacing(Device)
   returns true, append grid to eligible. Increment x and continue.
4. Dispose the row enumerator on both normal exit and exception. Native normal
   disposal is 0x1804913CA; exception cleanup begins 0x1804913E0.
5. If requested count > eligible.Count, return eligible directly. Otherwise
   allocate HashSet<int>; repeatedly add Random.Range(0, eligible.Count) until
   its Count reaches the requested count. Preserve duplicate draws and random
   consumption; do not replace with shuffling or sampling with replacement.
6. Enumerate the HashSet in its natural order, append eligible[index] to result,
   and dispose on both paths (0x18049154C / 0x18049155D). Return result.

The Device type check uses RIP slot 0x181B9CF08: 0x20006A91 -> type index13640 ->
global type5206. Grid.CanPlacing's native call is 0x180329AA0; GetMapX is
0x18032C3B0. Null rows/row/grids/grid retain ordinary managed exceptions. Zero
or negative requested count yields an empty result after traversal. When count
equals eligible.Count the original still samples; it does not return early.

## Validation

Run from repository root after the native14 bootstrap:

```sh
python3 scripts/codespaces/run_native14_validation.py
python3 scripts/codespaces/test_native15_fixture.py
python3 scripts/codespaces/validate_native15.py
```

The latter reruns native13/native14 positive controls, patches unlinked and
linked native14 twice independently, checks determinism, identity counts,
MVID and every non-target MethodBody, reopens the outputs, and executes the
exact Unity China IL2CPP converter on the linked candidate. Target closure,
full converter success, fresh Unity export and runtime fidelity are distinct
results. Check latest-result.json; do not infer success from script existence.

The patcher resolves framework methods only from pinned Unity directories. The
runner supplies `GODSPVZ_RESOLVER` pointing to the exact seed's ManagedStripped
directory for System.Core, which the small full resolver bundle does not carry.
Never fall back to the installed .NET framework for candidate materialization.
An explicit guard rejects a leaked System.Private.CoreLib assembly reference.

The synthetic CLR fixture uses stubs for surrounding game/Unity behavior and
executes the emitted target bodies on .NET 9. It tests branch behavior, casts,
argument/write order, range boundaries, duplicate random draws and exceptions;
it is not a Unity game/runtime qualification. Its Roslyn-generated nested-type
token order is normalized once with Cecil before token-based non-target checks.
Real candidate DLLs are never normalized this way. The patcher also checks stack
height over both target CFGs and finally handlers, including two deliberately
broken-stack negative controls, then repeats the check after reopening the DLL.
