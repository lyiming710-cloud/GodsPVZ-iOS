# ZombieInfo::.ctor PC native authority

Read-only prefetch; do not mutate unless later runtime/coverage explicitly promotes this MethodDef.

- PC GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Token `0x06000152`, RID `338`
- Managed RVA `0x18730`, damaged code size `89`, locals `5`
- Managed fingerprint `4050d7219be77f3a38d71ee97ce8ac4f26b3619c90fdc638aab92cb0eda36769`
- Pointer entry `0x181B837E8`
- PC native `0x1803251F0–0x180325261`, 113 bytes
- Native SHA256 `4ccabbc3bb2ebdc0f06c97337a2473d0855c91c94c5c753635bf8e167ed90c56`

PC semantics are compact and unambiguous: `healthPoint=270f`, `attackPoint=100f`, `speedRating=3`, allocate `enemyRequirements = new bool[10]`, `painterID=-1`, then call the `System.Object` base constructor. The current managed body ends in a fake `Method not found @180302170`; the PC target at that location is the normal base-constructor path, not game-specific behavior.

If promoted, rebuild exactly this one constructor with typed CLR array allocation and the existing base constructor; preserve all field metadata and do not add guards.
