# Native19 recovery checkpoint — IPA not exported

Branch: `repair/codex-native19-ipa`, based on Antigravity commit `64b86ed725c40e8e00cecd7a917f72bbed37777e`.
Native18 was independently reviewed and rejected as a semantic acceptance result. See [the review](NATIVE18-REVIEW.md).
Native19 reconstructs from the pinned **Native17 DLLs**, not the faulty Native18 DLL.

## Result and exact scope

- 15 target methods; all pass the fail-closed typed CIL verifier's supported subset on linked and unlinked outputs.
- Eight typed negative controls reject wrong Vector3 arguments, wrong List receivers, numeric locals, reference/float comparisons, MaxStack, EH exits and unsupported instructions.
- 36 CLR assertions exercise six shared emitters with Unity/helper doubles. This is not a Unity runtime test.
- `Zombie.Die` actual emitted CIL passes 7,424 state combinations against an independent oracle derived from PC branches, plus three semantic negative controls. The interpreter uses explicit helper doubles; it is not CLR/device acceptance.
- Linked and unlinked candidates each reproduce byte-for-byte across two independent patch writes.
- 2,282 non-target MethodBodies retain the same **normalized** instructions, referenced identities, locals and EH after reopening. This does not claim raw file-byte identity after Cecil metadata rewriting.
- IL2CPP conversion exits 0 and produces 21 game C++ translation units plus metadata in the isolated replay directory.
- Linux Clang finds **zero diagnostics associated with the 15 target method definitions**. The complete 21 game translation units still produce **6,273 diagnostics associated with 781 methods**. This count is diagnostic lines, not independent defects.
- Whole-game C++ qualification is **FAIL**. No Native19 full Unity/macOS workflow has been dispatched and no IPA exists for this candidate. Other generated assemblies, Apple linking, packaging and device behavior remain unaccepted.

## Input and output locks

| File | SHA256 |
|---|---|
| Native17 unlinked input | `74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2` |
| Native17 linked input | `9bf2d7a033632061e8e71d32c70312c3c234298649495afc384d0dfe7120a125` |
| Native19 unlinked candidate | `c022fb10f30c4363ba27cc48a4508029f57c76e079729e4aa753cd7e188dd3de` |
| Native19 linked candidate | `2df30387140871d1211467ccbfc790789d588d68fa131974c37c23e11f0dc873` |
| Original PC GameAssembly.dll | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| Original PC global-metadata.dat | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |

The exact header bundle is GitHub run `36505731509`, artifact `11008315616`, ZIP SHA256 `ba30763dcb6b225f27e1b415787a725d2f9bd868793f3014dd083542e23c7b1e`.

## Corrections beyond Native18

| Method/token | Native-backed correction |
|---|---|
| `PreviousPosition` / `0x06000418` | Return the actual Vector3 field. |
| `Start_PreviousPosition` / `0x0600041E` | Store `(fX, fY, animationY-fY)`; preserve required null faults. |
| `Update_Move` / `0x06000427` | Logical movement and TestPosition precede object movement; animation Y includes fZ. |
| `Update_PreviousPosition` / `0x06000429` | Apply root/height deltas to existing UI offsets and update previous height. |
| `ArmBroken` / `0x0600042F` | Correct Vector3 arguments and float comparison; restore the missing List<string> enumeration, Current and Dispose/finally. |
| `Ashe` / `0x06000430` | Damage text Y is `fZ+fY+134`; preserve HP callback and hot-nut/death dispatch. |
| `CheckZombieWin` / `0x06000432` | Correct reference-null encoding. |
| `CreateStartPrePath` / `0x06000433` | Restore both Clear operations, BoardConfig constructor argument, shortest successful endpoint selection and failed-path null assignment. |
| `CreatParticles` / `0x06000434` | Pass the shadow position as a Vector3 value. |
| `DestroyZombie` / `0x06000436` | Correct integer indices, reference element loads and closed List<Zombie>.Remove signature. |
| `Die` / `0x06000437` | Restore both jump tables and unsigned range tests, float armor comparisons, ID17 particle `(fX,fZ+fY,0)`, and separate isDied/ashes stores. Preserve one charred-position read. |
| `Device/ICEUIController.Update` / `0x06000342` | Four Vector3 value arguments for particles/audio. |
| `Device/ICEUIController.Broken` / `0x06000343` | Two Vector3 value arguments for particles/audio. |
| `Project.Start` / `0x060003BF` | Restore `Vector3.one * size`, including the non-equal/NaN path. |
| `Project.LoopAddAnimation` / `0x060003C0` | Restore pre-order SpriteRenderer collection and recursive IEnumerator traversal with IDisposable finally. |

The native extracts retain raw addresses and instructions. Generic/shared native pointer labels can be aliases; for example the shared Dispose pointer must not be identified from its nearest unrelated method name alone.

## Reproduction in the current Codespace

Working directory: `/workspaces/GodsPVZ-native19`. Old checkout `/workspaces/GodsPVZ-native14-check` remains at 64b86ed and its tracked files are unchanged.

```bash
python3 scripts/codespaces/test_native19_fixture.py
python3 scripts/codespaces/native19_local.py
python3 scripts/codespaces/qualify_native19_cpp.py
```

The last command intentionally exits nonzero while the known whole-game compilation failures remain. Its JSON reports target-level and whole-game results separately.
Set `NATIVE19_SOURCE_REPLAY` to reuse a different pinned Native17 replay. Cecil and Unity's canary runtime/dependencies must already be restored. The current script is a replay driver, not a complete clean-machine bootstrap.

Two validation defects were discovered and corrected during development:

1. An empty C++ input directory previously produced a false zero-error result. The preflight now refuses empty input; qualification additionally requires the 21 expected game units and unchanged generated hashes.
2. A copied IL2CPP response file retained absolute paths into the old replay. Early Native19 conversion output therefore went to that old ignored C++ cache. The old DLL stayed at Native18 linked hash `7e04eb13515e204a8638a300cea3b6628d67266ab37be7a59c3e11c9bbe39543`; its C++ cache was regenerated from that unchanged input. New replay paths are rewritten and checked before conversion. All archived Native19 acceptance evidence comes from the corrected isolated replay.

## Next work

`NEXT-BATCH-PROPOSALS.json` contains **read-only proposals**, not applied changes: 73 complete methods / 112 encoding edits (84 reference-null, 14 floating-zero, 14 matching struct-value arguments). Each proposed method passes the supported type checker after the edits and contains none of the recognized unresolved-native hints. This is a conservative triage filter, not proof that these methods reproduce original gameplay.

Methods with unresolved native loads, jumps, missing calls, unsupported type flows or remaining verifier errors are excluded from that batch. Do not fix them through blanket pointer casts, global exception-return replacement, string deletion, or dummy returns.

Continue in separate explicitly enumerated repair batches, run typed/negative/behavior checks, regenerate IL2CPP, and compile all game units locally. Only dispatch full export/macOS build when the known local compilation gate closes. Then compile the remaining generated code on Apple Clang, package a real arm64 Unity app, verify the archive and publish the actual IPA artifact.

`scripts/codespaces/package_native19_ipa.py` is prepared to verify a real Xcode product, metadata, bundle ID, architecture, ZIP CRC and SHA256, and refuses stale packaging output. It has not run on macOS and is not yet connected to a new workflow. The two missing JavaScript packagers in the old Native18 workflow must not be treated as a working packaging stage.
