from pathlib import Path

p = Path('Recovery/STATUS.md')
text = p.read_text(encoding='utf-8')

old_chain = '- **HF44 Zombie Movement Position Core — native-backed formal final — `fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`.**'
new_chain = '- HF44 `fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`\n- **HF45 FlagMeter Runtime Core — native-backed formal final — `a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`.**'
assert text.count(old_chain) == 1, 'HF44 chain-tail assertion failed'
assert '### HF45 — FlagMeter Runtime Core' not in text, 'HF45 section already exists'
text = text.replace(old_chain, new_chain, 1)

unity_marker = '## Unity reconstruction state'
assert text.count(unity_marker) == 1, 'Unity marker assertion failed'
hf45 = '''### HF45 — FlagMeter Runtime Core

Restores exactly two original native-backed MethodDefs:

- `0x06000617 FlagMeter.Update()` / RID 1559 / PC `0x18039D010`;
- `0x06000618 FlagMeter.UpdateMeter(int,int)` / RID 1560 / PC `0x18039CF30`.

Accepted behavior restores the original wave-meter runtime: single-precision `(theWave + 1) / wavesNum` progress, signed `% 10` flag activation, current/total text refresh, head-meter movement using `218f - progress * 436f`, and active-flag child vertical movement. Native COMISS unordered/NaN fall-through, List/index/null/Transform exception behavior, and all original constants are retained; no defensive iOS guards were added.

Formal validation:

- formal HF44 input was re-fetched from Drive after HF45 patcher publication and SHA-verified `fe5e73abead459121ea00c0f91be7e7abc8b3392eb924458b9c0c0352b444118`;
- patcher build head `289191fdf2fb9f30f2599833bd4b9b8de50fd6a0`, workflow `34489420443` PASS, artifact ID `10157078781`, SHA `21fbdb6a5814dd328941ac0ae36ec45de981b3617e3edcb0c678568d6bee0b67`;
- independent double patch -> byte-identical `a34feef56319f047a9f60caef0028db9592536b89fa484c41e371a826d6ccd5a`, both exit 0, stderr 0;
- Cecil reopen: `0x06000617` 68 IL / 184 bytes / 0 EH; `0x06000618` 31 / 74 / 0 EH; no Cpp2IL helper remains;
- fixed ILSpy 11.0.0.9375 + 56 refs: both target blocks exit 0, stderr 0, Cpp2IL/NotImplemented/invalid stack-type-comparison/warning-error markers 0;
- HF43 whole IL reproduced `26da408a5fe00508522d3a784338c4c50577ab1a78226d3c1929886e42760743`; HF44 whole IL reproduced `dbb10be399d1672a0927510a2fb05481783aabdfd56c9eb921d2bb9982ce2b1f`; HF45 whole IL `4d4ccd9b2c857ef9aceb0e18896e77cb6b493d9e654dc0a27dce2f5bfbd46668`;
- prior Drive HF43->HF44 semantic diff reproduced byte-for-byte at `c79688ce7518eb8fd5fe0cc697d2569ff39b4a189c004ceacd9460a09c430294`; HF44->HF45 semantic diff `a1508fbe29703cec50c81b8db85857fcdb2e1fda4abbdf24a1059b0fcda225e5`;
- MethodDef `2317 -> 2317`, distinct method-body RVAs `2139 -> 2139`, normalized non-method skeleton identical, changed exactly `0x06000617`, `0x06000618`;
- HF44 canonical MethodDef table reproduced byte-for-byte at `ab1557de0172766a51cc30dd7edae12d5a0cd29551cfb659a0e3641571afc790`; HF45 MethodDef table `784c0d85a4dd74015670c865493c2e08e324fc54c83541caace717e06300b50d`;
- permanent RecoveryAudit commit `bf54c77b625d818449d3a80a724a00ae2c0b95f6`, workflow `34489954734` PASS, artifact ID `10157301323`, SHA `c3d5aa29b9b2ef40803924de08fac48a1e66bf7062d8782e3ef990db8a9f06fc`; two independent auditor processes each passed internal OPEN1/OPEN2 over all retained targets and ended `RECOVERY_AUDIT_OK`;
- GitHub Evidence commit `5fd1e2bd56d68c69e2e63c6484f1ffeea9b07351`;
- Drive folder `1j1M9AyrsNcnHXtMg2sdQiBTuODSmLbUi`;
- cumulative DLL `19HEqSmIkz4PUt-SIhXWdmEwBy6UMu8so`;
- payload manifest `1Q-frWbl9L_rHsGTCR7yUOMeXOvjzdHwI`, SHA `26b9ffbc923460adc3c18d86fa3123f742407627570d92c0658e96ac88f1154f`; pre-closure provider readback exactly 20 files;
- Evidence-FINAL `1qYAlZSvqvAjrQFvfZPqS7_t7gxLqjx4-`, SHA `5caff93348f409767016a3cb9c80f4d307f85bff02c649e739c20ff3f7e887ff`;
- SHA256SUMS-FINAL `1thN5bR035bBNDFY5jAHrl2AwnnsM-eE2`, 21 entries, SHA `c64bd4b37fd7af6ae0bb5cf656a8f927243021e82304a8bf60f9962b1bcc9d3a`;
- final Google Drive provider readback exactly 22 files; FINAL pair was downloaded back from Drive and byte-hashed to the same recorded SHA values.

**HF45 formal acceptance: PASS. HF45 is now the only allowed formal input for any later cumulative HF stage.**

'''
text = text.replace(unity_marker, hf45 + unity_marker, 1)

gate = '## Decision gate — do not auto-open HF45'
assert text.count(gate) == 1, 'HF45 decision-gate assertion failed'
idx = text.index(gate)
new_gate = '''## Decision gate — do not auto-open HF46

Run a fresh residual active-path scan against the HF45 cumulative assembly. A later HF stage may open only if an original-PC-native-backed MethodDef simultaneously has concrete managed loss/mis-reconstruction, material gameplay reachability, and behaviorally closed native dependencies. Warning count, MethodDef adjacency, shared-stub xref centrality, or cosmetic decompiler quality alone are not gates.

`EnemyManager.TimeUpdate()` remains a candidate because it is directly reached from `EnemyManager.Update()`, but the newly repaired FlagMeter dependency does not by itself authorize patching it. Its remaining `TextWaveHealth` and broader huge-wave/final-particle Board/state dependencies must be closed from original native evidence first. `Zombie.SetUpdateRate()` and residual Projectile helper bodies remain unpromoted without independent live-call evidence.

If no remaining candidate satisfies the gates, stop HF managed recovery and proceed to 67/67 Unity/package validation, integrate the HF45 cumulative Assembly-CSharp recovery, compile and validate MainMenu/Board/gameplay paths, and only then perform necessary iOS adaptation.
'''
text = text[:idx] + new_gate
p.write_text(text, encoding='utf-8')
