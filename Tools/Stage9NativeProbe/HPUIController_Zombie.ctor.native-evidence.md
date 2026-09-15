# HPUIController_Zombie::.ctor PC native authority

Read-only prefetch for the constructor reached by `Zombie::.ctor`. Do not mutate unless runtime/coverage explicitly promotes this MethodDef.

Authority: original GodsPVZ 1.0.2 PC x86-64 IL2CPP `GameAssembly.dll`.

- Source GameAssembly SHA256: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- Managed input: runtime-qualified SkillProgress v2 candidate `2b2fdf165076ff9f48c577b84fb4c62346b78c925e46c6bff5f8ec6a79f67ce5`
- Managed MethodDef: `HPUIController_Zombie::.ctor()`
- Token: `0x06000629`
- RID: `1577`
- Managed RVA: `0x87384`
- Managed code size: `29`
- Managed locals: `1`
- Managed fingerprint SHA256: `a10615b5c9ae6e6f81833ee811b1c417c882cd759bdb0011029a3e4834496ed7`
- Private external field refs: `0`
- Method-pointer table entry VA: `0x181B85EA0`
- PC native target: `0x18039E260`
- Native leaf range used for hashing: `0x18039E260–0x18039E275`
- Native leaf length: `21`
- Native leaf SHA256: `e3bd26563031bfe359de37bb4a6e467109186ebd29b2b4e8c3508a511813cca8`

PC native semantics:
1. `maxHPEffect = 1.0f` at object offset `+0x50`.
2. `previewEffectTime = 0.3f` at object offset `+0x54`.
3. Tail-call `UnityEngine.MonoBehaviour::.ctor`.

This exactly matches the current managed body. Conclusion: already structurally healthy on `2b2f...`; evidence only.
