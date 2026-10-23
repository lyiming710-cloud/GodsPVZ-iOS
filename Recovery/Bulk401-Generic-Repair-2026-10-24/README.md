# Bulk401 generic repairs ¡ª 2026-10-24

Current R4's 38 Apple compiler diagnostics in three generated files bind to eleven exact managed definitions. `SwfList<T>.Push` is a twelfth damaged required dependency. This revision constructs all 401 scoped methods directly from Native93 `b0742241¡­`, preserving the prior 389 method bodies byte for byte. It does not modify generated C++ or the frozen checkers.

Original native generic binding is recovered through CodeRegistration `0x1815e88c0`, MetadataRegistration `0x1818c6d00`, the MethodSpec table, and original Assembly-CSharp RGCTX records. The Map wrapper's indirect call resolves to the untouched four-argument generic overload 0x06000290. The generic address/field/signature evidence and source are in BULK401-SOURCE.zip.

The repair restores real list/dictionary calls and correctly bound generic `!0/!1` arguments; Pop reads its value before clearing the slot to default(T); Push restores allocation/copy/store and mutation order; item bounds are unsigned; Unity GetComponent retains the `op_Implicit` check and optional AddComponent. Add calls the repaired public Push instead of illegally accessing another class's private fields.

Reopened isolation: 1916 non-target methods unchanged, all prior 389 repaired bodies identical, original Native93 slots preserved, six MemberRefs/five local signatures/one MethodSpec appended. Two builds produce the same bytes. The independently executed member-access checker reports no rejected or unresolved reference among 401 targets.

The existing static checker cannot model these generic receiver contexts and reference boxing precisely. GENERIC-STATIC-FIRST.json preserves its failures. They are not reported as PASS. Closed-context and actual CLR testing remain pending at this checkpoint.

The new manual-only `bulk401-generic-compiler-check.yml` regenerates C++ for the real damaged R4 as a negative control and for the new fixed candidate, then compiles every generated CPP and metadata C with no error limit. This is a Linux compiler precheck: it does not activate Unity or export/build/package an IPA. Full export remains blocked for the known-bad R4; main is unchanged.

No gameplay semantic qualification or IPA is claimed.
