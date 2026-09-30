> **用户要求暂停**：环境离线后，用户要求暂停测试推进并说明成果。本检查点保存已完成的工作；本轮没有新 Actions run 或 IPA。

# Native24 接手与验证修复 · 2026-09-30

当前没有合格 IPA，也未达到完整 Unity/Xcode 构建资格。本轮的确定成果是来源验证工具修复与独立复测；不将 E2 支持当作原版行为恢复。

起点固定为 `08d5eb4eb039fd5db95b8caea740470697a6ad96`。远端恢复分支经比较没有后续差异。本机独立工作区为 `/workspace/GodsPVZ-native24`，GitHub 保存分支为 `repair/codex-native24-verification`。main、原恢复分支和只读输入均未覆盖。

## 实际输入核对

实际输入根是 `/workspace/.cloud-setup/godspvz/game-inputs`。重算通过 4006 个 Git blob、13 个 Release 资产、本地 5347/Codespace 5576 个路径，以及独立审查 ZIP 中 1259 个文件。原 PC native、metadata、最早 managed `18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd` 和 Native21 linked `0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f` 的哈希均吻合。

Release 与仓库的 LOCAL-MANIFEST 原始哈希不同，但完整解析后的 JSON 相同，差异是序列化格式。按 Release 原始文件做了历史内容校验，未混用清单身份。R3 保持历史 preimport 身份，未被覆盖或作为当前游戏候选。

本实例实测 cpu.max 为 `200000 100000`，memory.max 为 `8589934592`，采用 2 个编译并发。不是按接入说明中的约 4 核/32GiB 运行。

## 重现与修复

旧来源工具的 local store、starg、call argument、merge 四个反例均实际误接受；旧具体解释器写入 7 后字段仍为 3。三例错误 pointer 正则、错误 ret→ldnull（exit=0/写文件）与旧 Swap 漏 switch/EH 也已重现。

新 [provenance.py](../../scripts/takeover/native24/provenance.py) 保存逐 CFG 状态的 locals/args 来源、调用污染和保守合流，并隔离未建模的 EH/别名写入。源码 SHA-256：

`8498a03a7ec3ee2589c9f42bc3a98cc02fa3972d6dca245c3ad7642a6382f393`

通过 11 个正负控制；三个直接字段/参数/静态字段正控制仍获得有限支持，真实整数零不改动。对固定 1623 个站点，254 个仅获 E2 来源支持、1369 个保持未证明；审查列出的 225 个 LOCAL receiver 站点全部隔离。旧 Batch2a 的 865 个站点中，仅同一 254 个保留有限支持，611 个隔离。没有据此生成或晋级游戏 DLL，E5/E6 未证明。

新 [C++ 映射](../../scripts/takeover/native24/cpp_method_map.py) 仅接受真实定义，使用平衡括号/函数体边界、严格源文件哈希和全 TU 日志；不会让后续原型覆盖定义。实际基线映射为 676 个报错方法，Batch2a 为 651 个。

本机 RepairNative24 构建、内存 branch/switch/五个 EH 槽和 return/ExplicitThis/calling convention 负例已运行；六站点输出与历史 Batch1 的原始哈希比较断言失败。该工具、输出和再读任务仍在本机，未获候选资格。必须先独立完成全部声明、方法体、非目标规范化与 raw IL 对比，不能假定只是 MemberRef 重编号。

旧不真实的行为解释器保持历史资料身份，不用于行为验收。实际 Batch1 DLL 完整 CIL 的 Device.TryPlacing/Plant.Awake CLR fixture 重跑 24/24，两种类型合法行为错误 mutant 被检出。辅助函数替身、Unity fake-null、Plant.Update 和全游戏行为仍未覆盖。

## 编译与权限

按哈希核验的历史生成 C++，用相同 Clang19、flags 和 Native18 导出头文件，真实检查 baseline/Batch1/Batch2a 各 264 个 TU，共 792 次。基线仍有 6070 条错误/24 个失败 TU，Batch2a 5200/23，均含 64 条头文件错误。这是新的复编译，不是新的 IL2CPP 转换或 Apple 编译。

完整逐 TU 日志已写入本机 `Recovery/Native24-2026-09-30/clang19-full-logs.tar.gz`（763164 字节），因环境断线尚不能上传或重新打开。其余本机 JSON、失败记录和候选同样需要环境恢复后核对及归档。[OBSERVED-EXECUTION.json](OBSERVED-EXECUTION.json) 如实区分已经观测的结果与未完成项。

全量重转换还缺 `Unity.VisualScripting.Core.exe`：CODESPACE-MANIFEST 将它当“Rebuildable tool output”排除了，但原转换命令明确包含它。不得删掉此依赖或用 dummy DLL 填补。已保存从既有、哈希锁定 Native7 seed 恢复该输入的脚本，但尚未执行。其身份仍须通过支持 DLL、重生成全部 C++ 与 metadata 对照才可接纳为 exact replay。

GitHub 读取和实际独立分支写入已成功；本机对不存在 ref 的 Actions dispatch 探针返回 422 而非权限拒绝，没有启动真实游戏构建。已重新核对历史 Mac 作业原始日志：Xcode16.4/iPhoneOS18.5，ExitCode=1、BUILD FAILED、unique_errors=19，是 Native17 历史诊断。没有使用其绿色状态声称游戏通过。本轮没有持证 Unity、Xcode、IPA 或设备测试。

## 复现

先准备已核验审查输入，再运行原工具反例、新来源正负控制和固定站点复算：

```sh
python scripts/takeover/native24/prepare_review.py --work .validation/native24
mkdir -p .validation/native24/evidence
python scripts/takeover/native24/reproduce_findings.py .validation/native24/evidence/historical-counterexamples.json
python scripts/takeover/native24/test_provenance.py .validation/native24/evidence/provenance-controls.json
python scripts/takeover/native24/audit_provenance.py --output .validation/native24/evidence/provenance-1623-sites.json
```

当前云环境明确返回 `409 Conflict environment_offline`，导致本机操作中断。用户随后要求在环境无法理想运行测试时暂停，已遵照暂停。没有创建或启动本轮新工作流，也未启动完整 IPA gate。

## 继续顺序

1. 用户要求暂停，当前不执行后续测试或构建。恢复工作后，先确认环境连接与资源，再保存实际命令、退出码与产物哈希；E2 来源支持仍不授予游戏资格。
2. 云环境恢复后，先核对本机未提交补丁器、两次 DLL、完整再读结果及 full-logs。本地 HEAD 仍在固定起点且有未提交成果，远端已追加工具/文档：先比较及保存普通副本，再协调分支；不要 hard reset 或覆盖未提交文件。
3. 补丁器输出严格做完整 MethodBody/metadata、非目标规范化语义、原始 bytes/token、branch/switch/EH 内存与重新开盘检查。哈希失败原因未解释前，不晋级。
4. 缺失输入核对后才重跑实际 IL2CPP，随后检查全部 TU。在独立六站点范围闭环后，逐个证明新的 MethodDef/IL/native CFG 站点；不能将 254 个 E2 支持站点直接批量改写。
5. 候选静态、native、行为资格真正闭环后，才建立/启动持证 Unity → macOS Xcode → 未签名 Payload IPA 的完整流程，并核验真实 Mach-O ARM64、资源、签名状态及下载 SHA-256。当前阶段未到达。
