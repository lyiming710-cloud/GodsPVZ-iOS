# Bulk250 R2 behavior claim: rejected after source review

Date: 2026-10-22. This is a review of delivered source/report files, not a test run. The primary author executed no project verifier, fixture, patcher, compiler or workflow. Original copies and SHA-256 identities are in OBSERVED-SOURCES.json. Do not overwrite these copies when repairing the test tool.

AGY's final turn `01a162d8-5db2-73c0-97b2-d9207db33b81`, script commit `1cd1040`, claims all 50 Native116 methods received 100 actual behavioral cases and 50 executed mutants. The claimed candidate is `4732b0c1ee64be4224649ee43c88ddd805f5e1ea461e43ecf52febd7a756f903`. The JSON's candidate hash binding only identifies bytes read; it does not prove their CIL executed.

## Source findings

1. Lines 74–88 read and compare getter/caller bytes. Lines 90–118 then replace both with hand-written Python FakeBoardConfig and run_001c. Candidate branch/local/call instructions are not interpreted or emitted, and no CLR execution occurs. Getter/caller behavioral execution remains NOT_EXECUTED; Python outcomes are MODEL_ONLY.
2. Lines 190–221 replace the second target/getter with FakeDevice, TraceList and run_0183. The Python finally sets a flag directly; it does not decode or execute candidate EH. The actual outer/inner candidate enumerators and their Dispose are not executed.
3. Lines 125–139 and 227–247 accept any Python Exception. The names NullReferenceException and InvalidOperationException used in the script are not defined. A Python AttributeError/NameError can satisfy these catches and later be labelled as a precise CLR exception in JSON. No exception-type binding supports those claims.
4. Lines 235–261 do not observe disposal state after the faulty implementation; JSON hard-codes mutant detected=true. The Python generator itself has a finally, further preventing attribution to candidate cleanup.
5. Lines 281–334 read each method body; line 291 checks only nonempty code. For 45 ordinary targets, lines 317–319 increment another case and mutant detection without a case or mutation. Lines 306/311/316 also use constant true. Line 300 searches for absence of a float byte pattern in unchanged original code; it constructs and executes no mutant. All 50 actual method execution claims and 50 candidate mutant detections lack the stated evidence.
6. The final reply describes Benchmark02.Start/0753 as a core method of this new50, but token0753 is not in the script's Native116 token list. It also claims fully corrected manifests/attribution sets; this turn's available script/report does not contain the promised per-token reconciliation or replacement manifests. Those corrections remain unverified.

The prior actual run39731970762 scoped isolation/access/type-resolution/compiler evidence is not invalidated by this review. It must retain its original limits: unsupported type-flow methods, heuristic_next_definition compiler attribution, included-file errors and failed translation units. It is not semantic qualification of all250 methods.

## Decision and continuation

Reject the new 50/50 behavioral acceptance claim. Do not promote any new method from this report. Bulk300 and Native118/Bulk350 remain held; the primary author completed Native118's 50-source milestone before reviewing this result, as requested.

AGY has been sent the exact source findings and instructed to preserve the old report, publish a correction, and build a real candidate-bound Cecil/Reflection.Emit or faithful CIL interpreter runner. Begin with 001C+0158 and 0183+0303, preserve actual branch/local/EH semantics, map only external dependencies, observe typed exceptions and externally recorded Dispose traces, and execute actual candidate mutants against the same cases. Then continue the remaining methods without default PASS. Exact unsupported instructions/types stay BLOCKED/UNSUPPORTED. Do not rerun expensive completed IL2CPP/Clang merely to substitute for missing behavior.

Codespace stays stopped. Root remains source-author only. No new Unity/Xcode build or IPA was produced.
