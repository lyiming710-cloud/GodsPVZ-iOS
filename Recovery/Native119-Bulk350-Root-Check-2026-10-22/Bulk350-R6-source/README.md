# Bulk350 root R6

R5 was rejected by real IL2CPP Run39789146252: 0x0600057E used callvirt for a static VisualScripting AddComponent<SortingGroup>(Object). This confirms a false accept in the prior scoped checker. R6 fixes that one opcode, and the checker now rejects the actual R5 CIL. R5, the old checker and CI failure are retained as evidence. The R5->R6 DLL difference is exactly one byte (0x6F->0x28); all other bytes are identical.

R6 candidate SHA256: ef95567ce714146b2f7aed1a92e98798023090bb47757f6c5b377f9ce6d9c0f4. Existing scoped350-method type-flow and access checks pass after the added static-callvirt rejection rule. Isolation rerun is stored separately. Behavioral and platform qualification remain pending. The final37 Native119 sources are not included in this350-method candidate.
