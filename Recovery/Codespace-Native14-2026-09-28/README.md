# Codespace native14 validation — 2026-09-28

**Target closure verified. Complete IL2CPP conversion still fails. No full Unity export or production promotion.**

## Reproduce

```bash
cd /workspaces/GodsPVZ-native14-check
python3 scripts/codespaces/run_native14_validation.py
```

This also works from another checkout path. Requires authenticated GitHub CLI access to this repository, Python 3, bash, a .NET SDK supporting net9.0 and the .NET 9 runtime. The entry point restores three hash-pinned archives, verifies cached data, prepares Cecil/full mscorlib, materializes native14 twice, reconstructs the linked native13 control, patches linked native14 twice, and invokes the exact Unity China IL2CPP converter for both inputs. Raw results live under `.validation/native14/`. Only a final `NATIVE14_CODESPACE_TARGET_CLOSURE_PASS` is a completed diagnostic.

## Infrastructure fixes

The previous linked chain rejected checkouts outside `/workspaces/GodsPVZ-iOS`. Native8/native9 Cecil references now use relative paths. Native8 normalizes a temporary source copy instead of rewriting tracked Program.cs. Downstream scripts use bash to invoke their predecessor, avoiding executable-bit mutations. Existing patch algorithms and all locked intermediate DLL hashes are preserved.

## Verified results

- GitHub native13 control: [36312525896](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36312525896).
- GitHub native14 materialization: [36318431640](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36318431640).
- GitHub native14 converter reference: [36318580594](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36318580594).
- Unlinked native14 SHA256: `281b7a20d3cc0719b0087be389f10a16e966a85c93bd5f43ef72fd54eaa01614`.
- Linked native13 control SHA256: `e7eaa90de7ed308b23efa9f20ef728e2f578523e8eea61a5547f97f85e908548`.
- Linked native14 SHA256: `4514a1e5f87d4cbbc896811669c47187dff1c46178a09ee694889cc05e3a0d96`, identical to GitHub.
- Control errors: `ElementManager.CreateNewElements<T>` and `Map.RandomGet_Grid_TestPlace<T>`.
- Native14 errors: `Map.RandomGet_Grid_TestPlace<T>` and `VFXAnimationEvent.Binding<T>`.
- Both converter processes exit 255. The diagnostic passes because the target error is removed with matching locked outputs and expected residual blockers; it does not claim the overall converter succeeded.
- Existing patchers verify non-target body isolation and unchanged MVID/counts. The runner checks takeover source hashes before/after replay.
- An earlier cached replay took 84.73 seconds; the final replay duration is in result.json.

Windows-side readback of the original PC GameAssembly independently confirmed the reference-sharing 1080-byte and fully-shared 1471-byte native14 extents, their recorded hashes, and complete instruction decoding. Generic table mappings and all earlier native7–native13 semantic claims were not independently re-audited in this infrastructure change.

## Next repair boundary

Map and VFX remain unrepaired. The later VFX error becoming visible alone is not proof of a regression. Obtain original PC generic-sharing mappings and complete native control flow before replacing either body. The adjacent metadata dumps are diagnostic input, not recovered C# specifications.

Use this direct diagnostic before an expensive Unity rebuild. A fresh UnityLinker pass, complete iOS export, Xcode build, IPA packaging and device behavior still require separate evidence.

Exact remaining targets: Map MethodDef `0x06000290` (421 instructions, 67 locals, 0 EH); VFXAnimationEvent.Binding MethodDef `0x060000C4` (104 instructions, 19 locals, 0 EH). Both have one unconstrained generic parameter. These counts describe damaged reconstructed IL, not a repair specification.

Repository log/dump copies normalize trailing whitespace only. Original raw output remains in the recorded Codespace replay directory.
