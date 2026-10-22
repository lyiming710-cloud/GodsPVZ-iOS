# Native120 / Bulk388 source checkpoint

2026-10-23. **Source only, uncompiled, unexecuted. No new candidate, tests, workflow dispatch, IPA or qualification.**

The author completed the newly discovered `0x060003F9 Projectile.SetSortingGroup` source from original PC native instructions. The historical 381 direct worklist entries, six companions and this additional method now have 388 source proposals. Native119's previous 37 sources are now composed into the aggregate source; they have still not been inserted into a constructed candidate DLL.

`source388/` contains the complete standalone patcher source project and explicit scope/metadata plans. It inherits actual corrected Bulk350 R6 source. The old short-index metadata writer remains unchanged. A separate full-column writer source handles the expected 65,075 → 66,497 byte #Blob transition; it is not a weakened guard. Its source declares 388 target bodies, 1,929 non-target MethodDefs and 201 finally regions. These are input requirements to verify, not observed PASS counts.

`Native120-source-evidence/` contains the original native disassembly, managed source readout, field offsets and the new emitter. An initial ambiguous-token read from another image is retained as explicitly withdrawn; active evidence pins image Assembly-CSharp.dll and native VA 0x18037C5E0. The original 37 methods' evidence remains in the earlier Native119-Bulk350 recovery directory in this branch history.

Read `source388/VERIFY-NEXT.md` for the independent verification backlog. The current AGY verifier remains assigned to correcting Bulk250-R2's behavior runner. Its prior 28-PASS report is rejected as candidate behavior evidence. This source checkpoint does not hand off a new execution task or widen the formal accepted scope.

The last actually built candidate is Bulk350 R6 (SHA ef95567ce714146b2f7aed1a92e98798023090bb47757f6c5b377f9ce6d9c0f4), from historical Actions 39790796339: IL2CPP exit 0, Clang still failed in 14/16 game TUs. No new conversion or compilation result is claimed here.

The authoring scripts are archived for provenance and assume the original workspace/audit directory layout. To build the future patcher, the self-contained source388/Bulk388.csproj does not need those authoring scripts. SOURCE-MANIFEST.json lists the delivered source bytes; it is an identity inventory, not a correctness certificate.
