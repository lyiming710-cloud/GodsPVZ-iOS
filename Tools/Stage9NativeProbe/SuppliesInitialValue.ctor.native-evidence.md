# SuppliesInitialValue::.ctor PC native authority

Read-only prefetch; do not mutate unless later runtime/coverage explicitly promotes this MethodDef.

- PC GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Token `0x0600025C`, RID `604`
- Managed RVA `0x2FAF8`, damaged code size `285`, locals `14`
- Managed fingerprint `f6908a722b3fb1fbea4ea2f3dcc62f0c1c8829c1160d198ccff1d50f03237f8e`
- Pointer entry `0x181B84038`
- PC native `0x180341320–0x18034146C`, 332 bytes
- Native SHA256 `3688ac9e8b1ca81c33bb128a816e2a5ab536fd6e48126a5e093749dfd964c6f8`

PC semantics: call the base `System.Object` constructor, allocate `suppliesInfos = new List<SuppliesInfo>()`, then for `i = 0; i < 512; i++` allocate `new SuppliesInfo(i)` and append it with normal `List<SuppliesInfo>.Add` semantics.

The current managed reconstruction exposes IL2CPP's internal list growth implementation (`_version`, `_items`, `_size`, `AddWithResize`) and even changes the temporary list to `List<object>`. Those are reconstruction artifacts. If promoted, restore the high-level typed `List<SuppliesInfo>.Add` loop using exact Unity Mono corelib generic MethodDefs/MemberRefs and require `Resolve()` against the locked corelib SHA before runtime.
