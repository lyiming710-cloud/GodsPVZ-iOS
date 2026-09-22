# Stage9.1 native-confirmed exception-path recovery gate

Status: `AUDIT_ONLY_BLOCKED_PENDING_LOCAL_EXECUTION`

This checkpoint covers only the three exception-return scan hits for which the repair checklist records an original-PC native sample showing an exception-helper terminal path followed by `int3`. It does **not** promote the other 154 scan hits.

## Scope and priority

1. `L001 / 0x0600008C / System2.FindGrid(int,int)` — P0 core grid logic.
2. `L002 / 0x060000D4 / AttackRange.TestInCircles_Position(Vector3)` — P0 core combat-range logic.
3. `L156 / 0x06000903 / FlashTools.Examples.PurpleFlowerLogic.GetRandomIdleSequence(...)` — P2 example/demo code.

The exact recovered tail patterns and native addresses are locked in `Recovery/Stage9.1-native-confirmed-exception-paths-v1.json`.

## Evidence boundary

The saved native samples prove that the corresponding original-PC exceptional tails terminate through an exception helper and `int3`, while the recovered managed IL constructs an exception object and routes it to `ret`. This is enough to classify the recovered tail as semantically wrong.

It is **not** enough to assume that every other exception-object-return scan hit is wrong. Those entries remain `native-unconfirmed` until separately mapped and classified.

HF16 independently maps native `0x1802FFAE0` to `AttackRange.TestInCircles_Position(Vector3)` and shows that `AttackRange.TestInRange<T>` calls it for Circle/Mixed range dispatch. This establishes gameplay relevance and identity, but the exception-tail classification still comes from the saved native sample.

## Required ordering

Do not mix this logic repair into the currently blocked MemberRef investigation.

1. Complete the all-24 MemberRef local gate on exact `b07254d5...f720`.
2. Pass real Unity/IL2CPP far enough to establish that MemberRef normalization does not regress the build.
3. Audit L001/L002 control-flow safety on that exact work copy.
4. Produce a separate logic-repair candidate and whole-method semantic diff.
5. Reopen with Cecil/ILSpy and run Unity/IL2CPP plus gameplay regression.
6. Handle L156 only after core gameplay paths are stable unless it independently becomes a build blocker.

## Read-only audit gate

`Tools/Stage9LogicExceptionAudit` is intentionally read-only. For each locked target it must verify:

- exact MethodDef token and return type;
- exact `newobj exception::.ctor -> stloc -> ldloc -> ret` tail at the recorded IL offsets;
- `stloc` and `ldloc` refer to the same local;
- no external branch/switch enters the `stloc` or `ldloc` instruction;
- all incoming edges to the trailing `ret` are reported, because a normal path may share that `ret`;
- EH boundaries touching the candidate rewrite region are reported and any boundary on the rewrite instructions blocks automatic repair.

A successful read-only result is `STAGE9_EXCEPTION_PATH_AUDIT_READY`; it does not authorize a write by itself.

## Candidate rewrite rule after audit passes

Do not patch raw file offsets. Locate methods and instructions through Cecil using the locked MethodDef tokens and revalidate the complete tail before modification.

The managed-observable exceptional path must become a true throw/exception terminal path, not a returned exception object. The exact IL rewrite encoding is deliberately **not locked yet** because it depends on the audit result for shared `ret` predecessors and EH boundaries. Preserve any shared normal `ret` and normal predecessor stack behavior.

After a candidate is generated, require:

- exact input SHA gate;
- per-method before/after IL manifest;
- no method other than the explicitly selected target set changes semantically;
- MethodDef remains 2317;
- no new MemberRef/orphan-generic regression;
- Cecil reopen and ILSpy readback succeed;
- stack/EH/branch analysis passes;
- real UnityLinker/IL2CPP passes;
- core scene tests cover grid lookup and circle-range behavior, including normal paths.

## Non-bulk rule

The remaining 154 static hits must stay unchanged unless their corresponding original native branch is independently inspected. Similar-looking `newobj -> stloc -> ldloc -> ret` sequences are not sufficient evidence for bulk conversion to `throw`.
