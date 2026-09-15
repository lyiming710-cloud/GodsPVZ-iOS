# BuffManager::.ctor PC native authority

Read-only prefetch for the constructor reached by `Zombie::.ctor`. Do not mutate unless runtime/coverage explicitly promotes this MethodDef.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- Source GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Managed input: runtime-qualified SkillProgress v2 candidate `2b2fdf165076ff9f48c577b84fb4c62346b78c925e46c6bff5f8ec6a79f67ce5`
- Managed MethodDef: `BuffManager::.ctor()`
- Token: `0x06000108`
- RID: `264`
- Managed RVA: `0x14E4E`
- Managed code size: `40`
- Managed locals: `0`
- Managed fingerprint SHA256: `095be03f22fd461e94299d6b82652518356e255b0dd31c6be2e9a289434df75c`
- Private external field refs: `0`
- Method-pointer table entry VA: `0x181B83598`
- PC native target: `0x180310630`
- `.pdata` range: `0x180310630–0x180310703`
- Native span length: `211`
- Native span SHA256: `842957427a349c056d3be1f1a6c49649c583dd523e336529d80f9883665a78c6`

PC native semantics:
1. Allocate a fresh generic `List<Buff>` and store it at object offset `+0x10` (`buffs`).
2. Allocate a fresh `List<Buff>` at `+0x18` (`buffs_toAdd`).
3. Allocate a fresh `List<Buff>` at `+0x20` (`buffs_toRemove`).
4. Tail-call `System.Object::.ctor`.

The current managed body expresses the same logic, but its three generic `List<Buff>..ctor()` MemberRefs must not be assumed CLR-safe merely because ILSpy can display them. If this MethodDef is ever promoted, recovery/qualification must apply the same exact Unity Mono corelib open-generic MethodDef + closed-host `Resolve()` rule used by BGM v2 and SkillProgress v2.
