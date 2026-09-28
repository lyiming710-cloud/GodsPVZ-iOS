# Full Unity gate preflight

Codespace checks against proposed-full-workflow.yml at source 95cd0be passed:

- YAML parse and dependency/permission/input assertions.
- 19 shell bodies parsed by bash -n; no workflow step executed by this check.
- Four embedded Python bodies compiled successfully.
- Native17 package required direct-conversion exit 0 and the exact linked and
  unlinked hashes, then inspected the unlinked candidate's actual assembly name.
- Actual assembly name: **GodsPVZRuntime1**. The full export audit reads this
  checked metadata instead of incorrectly requiring Assembly-CSharp.cpp.
- Candidate packaging: NATIVE17_CONVERSION_QUALIFIED_PACKAGE_PASS.

The activated workflow is byte-identical to the checked proposed workflow.
Qualification rebuilds from pinned native13/seed artifacts and rechecks prior
controls, 80 CLR assertions across native15/16/17, deterministic candidates,
and zero-error IL2CPP conversion before producing the same-run candidate artifact.
Export checks candidate SHA256 and provenance source_commit against GITHUB_SHA.
It then uses the preserved R3, baseline migration, packages, exact Unity China
Editor and iOS module to perform a fresh Unity import and iOS Xcode export.

This records preflight evidence only. The Actions run is the authority for full
export status; Xcode compilation, IPA packaging and device validation are later
acceptance stages. Production remains unchanged.
