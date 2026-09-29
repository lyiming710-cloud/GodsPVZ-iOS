# Native21 — paused handoff, no IPA

Read [the complete Chinese handoff](../HANDOFF-2026-09-29-Native21.md) first.

- Targets: 38; per-candidate reverted-defect controls: 99.
- Reachable supported typed CIL passes; 11 unreachable instructions are reported, not semantically certified.
- Two independent writes produce identical bytes for each linked/unlinked candidate.
- Normalized non-target MethodBodies unchanged in memory and after reopen: 2259. This is not raw byte isolation.
- Real IL2CPP exit 0; 21 generated game translation units.
- Target-associated Clang diagnostics: 0. Whole-game diagnostics: 6032; associated methods: 676.
- Whole-game C++ qualification: FAIL. No full workflow, Apple acceptance, IPA or device validation.
- Individual original-native semantic equivalence remains unproven for every target. This batch is a constrained type repair, not full gameplay acceptance.

Candidate binaries are preserved in `candidates/`. `validation/replay/` contains before/after CIL, edit maps, negative controls, deterministic patch logs and real conversion log. `validation/cpp-final38/` contains whole-game errors, source hashes and actual compiler invocation.

| Candidate | SHA256 |
|---|---|
| unlinked | `5c34c8b20787d1ea56932ab636dede404eb352f6597de7959b93f44cd4546842` |
| linked | `0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f` |

Reproduce in the already bootstrapped Codespace:

```bash
cd /workspaces/GodsPVZ-native19
python3 scripts/codespaces/native21_local.py
python3 scripts/codespaces/qualify_native21_cpp.py
```

The qualification command currently exits nonzero because compilation fails. Preserve that failure. These scripts are not a clean-machine bootstrap.
