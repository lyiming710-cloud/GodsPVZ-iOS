# Native20 — paused handoff, no IPA

Read [the complete Chinese handoff](../HANDOFF-2026-09-29-Native21.md) first.

- Targets: 73; per-candidate reverted-defect controls: 112.
- Reachable supported typed CIL passes; 47 unreachable instructions are reported, not semantically certified.
- Two independent writes produce identical bytes for each linked/unlinked candidate.
- Normalized non-target MethodBodies unchanged in memory and after reopen: 2224. This is not raw byte isolation.
- Real IL2CPP exit 0; 21 generated game translation units.
- Target-associated Clang diagnostics: 0. Whole-game diagnostics: 6175; associated methods: 714.
- Whole-game C++ qualification: FAIL. No full workflow, Apple acceptance, IPA or device validation.
- Individual original-native semantic equivalence remains unproven for every target. This batch is a constrained type repair, not full gameplay acceptance.

Candidate binaries are preserved in `candidates/`. `validation/replay/` contains before/after CIL, edit maps, negative controls, deterministic patch logs and real conversion log. `validation/cpp-final73/` contains whole-game errors, source hashes and actual compiler invocation.

| Candidate | SHA256 |
|---|---|
| unlinked | `7d2b3661f3e4aa3ba27b986c233d65c920e4c439851eae1788ae58323cc1d910` |
| linked | `f561d48a353f1747b77260f9db69acabc1bb4860f46d4ffc447d81764c55efa9` |

Reproduce in the already bootstrapped Codespace:

```bash
cd /workspaces/GodsPVZ-native19
python3 scripts/codespaces/native20_local.py
python3 scripts/codespaces/qualify_native20_cpp.py
```

The qualification command currently exits nonzero because compilation fails. Preserve that failure. These scripts are not a clean-machine bootstrap.
