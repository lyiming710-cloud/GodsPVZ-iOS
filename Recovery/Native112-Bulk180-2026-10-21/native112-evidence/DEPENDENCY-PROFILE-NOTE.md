# Native111 inherited dependency blocker

2026-10-21. Source-data inspection only; no local candidate or test execution.

AGY reports Run 39456112419 produced Bulk168 candidate `d7026d0121a4d3360dfa102ccb0e5999388faa75f95d18ac49f0c486ef4e3a16` and stopped on unresolved public `AnimatorStateInfo.get_normalizedTime` at new `04B1 IL_0059`.

Keep the failure. Do not classify it as proof that the game source's public getter is semantically wrong.

The authoritative original PC `04B1` machine code calls GetCurrentAnimatorStateInfo at `0x180372896`, copies the returned 36-byte value to its local state, and calls shared function `0x1802FB140` at `0x1803728BA`. Reading original bytes at that function yields a scalar load from `[rcx+0xC]` and return. Its address is shared with other structs' getters, so an address-only method name is ambiguous. The fourth 32-bit field of AnimatorStateInfo is m_NormalizedTime, and this is the value subsequently used for the UI fill remainder and Animator.Play.

The historical 24,064-byte AnimationModule (SHA-256 `057323dc2cadf8b9e609aca40703a42f4bf55326cb678d247062dcb881ca17ae`) retains nine private AnimatorStateInfo fields but no methods on that type. A local complete reference (161,792 bytes, SHA-256 `e514adf83b0ba4887fb073e30d2e05e1b35aded593e32c2ebe8b15cfb5b5b75e`) contains the matching nine-field layout and public instance `Single get_normalizedTime()` with real CIL reading private m_NormalizedTime.

This establishes a legitimate public API and native use. It does not alone establish that the larger local reference is a compatible iOS Player runtime module. The claim that the original game never used animation state time is contradicted by the original call/dataflow. An earlier stripping input that had lost this getter call is a plausible explanation, pending dependency lineage evidence.

AGY is assigned to establish same-version genuine Player provenance, profile/configuration closure, and either use that independent complete profile or re-run the normal Unity linker with the repaired references preserved. Never add a fabricated getter, bypass member checks, read another type's private fields, use unsafe offsets, substitute a constant, or remove animation behavior. Never silently overwrite the historical stripped profile. Retain prior and new runs separately with their input hashes.

Bulk180 source inherits the unchanged legal public getter and the explicit unresolved-profile status. No formal promotion is inherited.
