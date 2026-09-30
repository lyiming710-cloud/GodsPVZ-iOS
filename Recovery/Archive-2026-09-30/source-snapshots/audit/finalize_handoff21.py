from pathlib import Path
import hashlib,json,re,subprocess
root=Path(__file__).parent/'native19-work';sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
for stage in [20,21]:
    out=root/f'Recovery/Native{stage}-2026-09-29';r=json.loads((out/'RESULT.json').read_text())
    body=f'''# Native{stage} — paused handoff, no IPA

Read [the complete Chinese handoff](../HANDOFF-2026-09-29-Native21.md) first.

- Targets: {r['targets']}; per-candidate reverted-defect controls: {r['typed_negative_controls_per_candidate']}.
- Reachable supported typed CIL passes; {r['unreachable_instructions']} unreachable instructions are reported, not semantically certified.
- Two independent writes produce identical bytes for each linked/unlinked candidate.
- Normalized non-target MethodBodies unchanged in memory and after reopen: {r['normalized_non_targets']}. This is not raw byte isolation.
- Real IL2CPP exit 0; 21 generated game translation units.
- Target-associated Clang diagnostics: 0. Whole-game diagnostics: {r['cpp_diagnostics']}; associated methods: {r['cpp_associated_methods']}.
- Whole-game C++ qualification: FAIL. No full workflow, Apple acceptance, IPA or device validation.
- Individual original-native semantic equivalence remains unproven for every target. This batch is a constrained type repair, not full gameplay acceptance.

Candidate binaries are preserved in `candidates/`. `validation/replay/` contains before/after CIL, edit maps, negative controls, deterministic patch logs and real conversion log. `validation/cpp-final{r['targets']}/` contains whole-game errors, source hashes and actual compiler invocation.

| Candidate | SHA256 |
|---|---|
| unlinked | `{r['candidate']['unlinked']}` |
| linked | `{r['candidate']['linked']}` |

Reproduce in the already bootstrapped Codespace:

```bash
cd /workspaces/GodsPVZ-native19
python3 scripts/codespaces/native{stage}_local.py
python3 scripts/codespaces/qualify_native{stage}_cpp.py
```

The qualification command currently exits nonzero because compilation fails. Preserve that failure. These scripts are not a clean-machine bootstrap.
'''
    (out/'README.md').write_bytes(body.encode())
current=root/'Recovery/HANDOFF_CURRENT.md';s=current.read_text()
notice='> **2026-09-29 最新暂停交接：请先读 [Native21 总交接](HANDOFF-2026-09-29-Native21.md) 和 [接手任务说明](TAKEOVER-PROMPT-Native21.md)。Native20/21 已完成当前验证并归档，全量 C++ 仍失败，没有 IPA。下方 Native19 与 2026-09-18 内容保留作历史记录，不代表最新候选。**\n\n'
if notice not in s:s=s.replace('\n\n','\n\n'+notice,1)
current.write_bytes(s.encode())
ignore=root/'.gitignore';s=ignore.read_text();s=s.replace('\n# Native20 local build products\nscripts/takeover/PatcherNative20/bin/\nscripts/takeover/PatcherNative20/obj/\n','\n');ignore.write_bytes(s.encode())
for stage in [20,21]:
    out=root/f'Recovery/Native{stage}-2026-09-29'
    paths=[p for p in out.rglob('*') if p.is_file() and p.name!='SHA256.json']
    paths += [p for p in (root/f'scripts/takeover/PatcherNative{stage}').glob('*') if p.is_file()]
    paths += [root/f'scripts/codespaces/{name}' for name in [f'native{stage}_local.py',f'test_native{stage}_types.py',f'qualify_native{stage}_cpp.py']]
    paths += list((root/'scripts/takeover/PatcherNative19').glob('*.cs'))
    paths += [root/'scripts/codespaces/verify_native19_types.py',root/'scripts/codespaces/cpp_syntax_preflight.py',root/'scripts/codespaces/analyze_cpp_failures.py']
    if stage==20:paths.append(root/'Recovery/Native19-2026-09-29/NEXT-BATCH-PROPOSALS.json')
    else:paths.append(root/'scripts/codespaces/audit_integer_locals.py')
    manifest={p.relative_to(root).as_posix():sha(p) for p in sorted(set(paths))}
    (out/'SHA256.json').write_bytes((json.dumps(manifest,indent=2)+'\n').encode())
handoff_files=[root/'Recovery/HANDOFF-2026-09-29-Native21.md',root/'Recovery/TAKEOVER-PROMPT-Native21.md',root/'Recovery/Historical-Handoff-2026-09-27.md',current]
(root/'Recovery/HANDOFF-Native21-SHA256.json').write_bytes((json.dumps({p.relative_to(root).as_posix():sha(p) for p in handoff_files},indent=2)+'\n').encode())
print('HANDOFF_MANIFESTS_WRITTEN')
