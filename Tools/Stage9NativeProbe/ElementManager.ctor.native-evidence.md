# ElementManager::.ctor PC native authority

Read-only prefetch for the constructor reached by `Zombie::.ctor`. Do not mutate unless runtime/coverage explicitly promotes this MethodDef.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- Source GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Managed input: runtime-qualified SkillProgress v2 candidate `2b2fdf165076ff9f48c577b84fb4c62346b78c925e46c6bff5f8ec6a79f67ce5`
- Managed MethodDef: `ElementManager::.ctor()`
- Token: `0x06000129`
- RID: `297`
- Managed RVA: `0x17298`
- Managed code size: `7`
- Managed locals: `1` (`ElementManager`)
- Managed fingerprint SHA256: `134d4b2e67692dbb2dd40e04c587467ea1f7b471f8e4fda36284551fdced2a16`
- Private external field refs: `0`
- Method-pointer table entry VA: `0x181B836A0`
- PC native target: `0x18030F300`
- Native leaf bytes: `33 d2 e9 69 2e ff ff`
- Native leaf length: `7`
- Native leaf SHA256: `43d959b8441e91f2d30f23b57b247ca5579b3d82158bc36e35d95ea54ca33092`

Native semantics are exactly the managed body: clear IL2CPP MethodInfo argument state and tail-call `System.Object::.ctor`. The native target is deduplicated and shared by several empty constructors, so the managed RID/token above is required for attribution.

Conclusion: this constructor is already structurally healthy on `2b2f...`; it is evidence only, not a recovery target.
