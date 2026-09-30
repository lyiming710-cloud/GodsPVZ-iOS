# Native24：Codespaces 恢复验证与六字节隔离补丁

2026-10-01，恢复执行位置为 `glowing-train-p7j9gp74q6jwc76v6`。本检查点完成了旧云环境离线时未闭环的补丁输出、完整声明和原始字节隔离验证，并独立重新执行 IL2CPP、CLR fixture 和全部 C++ 预检。**尚无合格未签名 IPA；本轮没有提交 workflow dispatch，没有执行持证 Unity 导入、真实 iOS 导出、Apple 编译、链接或设备测试。**

## 分支与输入边界

新工作树 `/workspaces/GodsPVZ-native24-codespace`，分支 `repair/codex-native24-codespace`，起点固定为 `919db42b7c08ac7686078558c03770b7a68ac2b6`（Native24 暂停交接），继承其 `08d5eb4` 历史恢复快照。旧工作树 `/workspaces/GodsPVZ-native19` 保持 `f1a96517d21826f0de9efb579b481dee4a9863b0` 且干净；原主工作树和 main 未修改。

旧云实例的完整日志及未提交 Cecil patcher 依然无法访问。此处是独立重新实现和重新运行，不是取回了那些文件，也不能据此解释不可访问旧补丁器的哈希失败。

本次支持程序集实际存在于旧 Codespace 的锁定 managed 目录。重新找回并归档了真实 `Unity.VisualScripting.Core.exe`：330240 字节，SHA-256 `16cabf56b65cd1e6c09338d39af27dce42377dc4d986c42cdfd9f91cc496a4fb`。它是转换输入，不能因为扩展名为 `.exe` 而当作可重建工具排除。完整 DLL 和 EXE 输入清单见 `validation/INPUT-SUPPORT-LOCK.json`。

## 候选与确切修改

输入 `GodsPVZRuntime1.dll` SHA-256：

`0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f`

严格只物化 Native23 Batch1 已有六站点规格，不包含 Batch2/Batch2a 或 254 个来源支持站点的批量晋级：

| MethodDef | 方法 | 原始 IL offset | 修改 |
|---|---|---|---|
| `0x06000348` | `Plant.Awake` | 7 | 将 Animator 泛型 GetComponent 的 Component 引用改为已有 GameObject 引用 |
| `0x06000348` | `Plant.Awake` | 35 | Component.get_transform 改为已有 GameObject.get_transform |
| `0x0600034C` | `Plant.Update` | 225 | 同上；Camera 的 Component.get_transform 保持不变 |
| `0x06000339` | `Device.TryPlacing` | 6、58 | 两处 `ldc.i4 0` 改为 `ldnull` 并各保留四个 NOP |
| `0x06000339` | `Device.TryPlacing` | 161 | 精确 new NRE→store V5→load V5→ret 路径改为 throw |

六站点的历史 PC native 证据和完整规格入口为本目录 `reference/NATIVE23-BATCH1-REPORT.md`（按历史原文保存；其旧归属计数和行为表述以当前独立审查为准）、Native23 独立审查与历史 Release 的完整审查档案。此处保留已有有限站点结论；没有重新把全部方法的行为判为通过，更没有整游戏行为验收。

新 `RepairNative24` 有两个输出后端：

- **Cecil 历史重放**：两次输出都与历史 Batch1 整文件逐字节一致，SHA-256 `4d970f5ed7e63dd42815c3c843ce51918328ff790fa5d1eb77e2a80e6f3b0a1b`。仅三个目标的规范化 IL 改变，但是 Cecil 的引用 token 重排还改变了 18 个非目标原始 IL；不能称原始字节全部隔离。
- **首选 raw PE 后端**：不调用 AssemblyDefinition.Write，重用基线中已存在的正确 MethodRef/MethodSpec token，保持文件长度、元数据、MVID、RVA、方法头、分支与 EH offset。零常量原本为五字节，`ldnull + 4 nop` 仍占五字节，零操作数字节本来就是零，因此最终整个文件仅六个字节改变。两次输出 SHA-256 均为 `8300c14ebac3b45cc00397c69cd72d4517366587acbe081b6e8b5a98f8fdedf9`。

raw 输出不同于历史 Cecil 输出是明确选择保持布局的结果。独立解析证明目标方法在删除 NOP、映射分支和 EH 索引后与历史 Batch1 相同；生成的全部文件只有 `GodsPVZRuntime1__6.cpp` 不同，差异仅为 Device.TryPlacing 的 IL 标签编号。完整 diff 已归档。

## 真实执行结果与结论边界

| 检查 | 本轮结果 | 不代表什么 |
|---|---|---|
| 历史输入恢复 | 审查 ZIP 中 1259 个文件哈希全部核验 | 不代表所有历史恢复 DLL 都已验收 |
| 保守来源门 | 11 个控制通过；1623 站点→254 来源支持、1369 隔离；225 LOCAL receiver 全部隔离 | 254 个不是行为通过或可批量修复 |
| 补丁器结构/签名控制 | 19/19，错误输入和错误 manifest CLI 均拒绝且无输出 | EH 五指针测试只检查引用身份，不是有效 EH 行为执行 |
| 六站点逐一撤回 | 6/6 均被 typed verifier 检出 | 属于 E1 反例，不是行为证据 |
| raw 全程序集隔离 | 2314 个非目标 MethodDef 不变，其中 2294 个有方法体，原始 IL、规范化内容、完整声明不变；整文件仅六个字节改变 | 不是游戏行为一致证明 |
| E1 全量类型检查 | 2297 个方法体：1425→1428 通过；872→869 失败；三个目标全部 FAIL→PASS；PASS→FAIL=0 | fail-closed 指令子集，不是完整 ECMA-335 验证器 |
| 实际 CLR fixture | .NET 10.0.12，24/24 用例，2 个行为错误变体被检出 | 只执行 Device.TryPlacing、Plant.Awake 的候选 CIL，调用绑定显式 helper doubles；没有 Plant.Update、Unity fake-null 或游戏场景测试 |
| 实际 IL2CPP | 基线、Cecil、raw 三次 exit 0，每次 264 个 C++ TU | 历史 `unityaot-macos` qualifier 在 Linux 的重放，不是持证 Unity iOS 导出 |
| 历史生成重放 | 基线和 Cecil 的全部生成文件及 global-metadata 与历史各自哈希相同 | 不能把转换 exit 0 当作 C++ 编译通过 |
| 真实全量 Linux Clang | 三组共 792 次编译，见下表 | 不是 Apple Clang、链接、IPA 或真机验收 |

编译器为 **Ubuntu Clang 18.1.3**，与旧云 Native24 的 Clang19 区分；每一组均使用同一编译器、同一宏与锁定的 Native18 Xcode 导出头文件。源文件 SHA 与日志严格配对，方法归属仅用定义范围，不能拿另一棵 Xcode 源码树套行号。

| 本轮新转换输入 | 总错误（含头文件） | 生成 CPP 错误 | 失败 TU | 含错误方法 |
|---|---:|---:|---:|---:|
| 锁定 baseline | 6070 | 6006 | 24/264 | 676 |
| Cecil Batch1 重放 | 6065 | 6001 | 24/264 | 673 |
| raw 六字节输出 | 6065 | 6001 | 24/264 | 673 |

64 条头文件错误计入总数，生成 CPP 中另有 38 条错误位于游戏方法定义之外。三个目标方法均零编译错误；整包还没有通过编译门，不能启动长 Unity/Xcode 流程后才发现这些已知问题。

## 归档结构与恢复

- `validation/`：关键输入锁、来源控制、隔离、实际 fixture、typed 结果汇总、三组转换和全量编译结果及精确方法归属。
- [完整证据 ZIP](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/download/native24-codespace-2026-10-01/native24-codespace-2026-10-01-evidence.zip)（Release 资产，86,621,209 字节；SHA 见 `EVIDENCE-ARCHIVE.json`）：完整机器日志、所有 TU 编译日志、独立 metadata/declarations/typed 导出、输入 DLL、真实支持 DLL/EXE、六个撤回变体、候选、C++ 内容与各阶段映射、工具源码和复现脚本。
- `SHA256.json`：归档文件与关键证据的哈希。
- `scripts/takeover/native24/RepairNative24/`：完整 Cecil/raw 后端与声明审计源码，portable MonoCecilPath 属性。
- `scripts/takeover/native24/RuntimeFixture/`：真实 CLR fixture 源码，保持明确的 helper double 范围。
- `scripts/takeover/native24/IndependentInspector/`：独立 PEReader/Cecil 检查器。
- `scripts/takeover/native24/checkpoint/`：本轮复现脚本和安全恢复工具。执行路径/编译器参数见原始 command JSON；这些记录包含 Codespace 的路径，迁移时必须显式重映射，而不是假定其他环境具备原目录。

完整生成文件按 SHA-256 内容去重保存在 ZIP 的 `cpp-blobs/`，`GENERATED-SOURCE-MAP.json` 指定 baseline、candidate、raw 的全部文件。没有丢弃重复阶段；恢复脚本会重建三套普通文件并核验每个内容。ZIP 内 `MANIFEST.json` 列出每个归档成员的大小与 SHA。不要使用硬链接编辑原始参考输入。

```bash
# archive-sha 从本目录 EVIDENCE-ARCHIVE.json 读取；destination 必须不存在。
gh release download native24-codespace-2026-10-01 --repo lyiming710-cloud/GodsPVZ-iOS \
  --pattern native24-codespace-2026-10-01-evidence.zip --dir /tmp
cd /tmp
# 下方脚本路径需指向仓库内实际脚本；请勿假定 cd 后相对路径仍相同。
python3 /workspaces/GodsPVZ-native24-codespace/scripts/takeover/native24/checkpoint/restore_checkpoint.py \
  native24-codespace-2026-10-01-evidence.zip /tmp/native24-restored \
  --sha256 <archive-sha>

dotnet build /workspaces/GodsPVZ-native24-codespace/scripts/takeover/native24/RepairNative24/RepairNative24.csproj \
  -c Release -o /tmp/native24-patcher -p:MonoCecilPath=<locked-Mono.Cecil.dll>
export GODSPVZ_RESOLVER=<hash-verified-managed-support>
dotnet /tmp/native24-patcher/RepairNative24.dll --self-test /tmp/controls.json
dotnet /tmp/native24-patcher/RepairNative24.dll --raw \
  <baseline.dll> /tmp/native24-new.dll /tmp/raw-report.json
```

## 后续推进

继续以 raw 六字节输出为这个有限补丁的基准，保留 Cecil 后端供历史精确重放。余下 869 个 E1 失败与 673 个含 C++ 错误方法需要分批 native 取证和方法规格，不能把 254 个 E2 来源支持直接当成 E5/E6，更不能重新启用 Batch2/Batch2a 批量替换。

先从现有站点级 native 证据的 `Device.TestPlacing`、`AttackRange.TestInRange_Device`、`BoardInfoPage.SetLootList` 做精确清单：只覆盖已证实的站点，其他 LOCAL/污染来源继续隔离。复杂方法如 `Zombie.GetDamage` 存在缺失分支与反编译错误字符串，需完整 body 重建，不能机械删字符串或全局 ret→throw。

每一批顺序：原版 PC native/image/token/多段函数边界→站点 CFG 与调用证据→最小 MethodBody 规格→确定性补丁→声明和原始/规范化隔离→typed 及逐站点反例→真实 CLR/Unity 可执行测试并说明覆盖缺口→新 IL2CPP 产物→全部 TU 编译与确切方法归属。静态与实际平台门通过后才进入持证 Unity iOS 导出与 GitHub macOS-15/Xcode；最终目标仍是**未签名 IPA**，签名方案无需再次确认。

本轮没有本机 Unity 激活，也没有读取、导出或归档 Unity/GitHub 凭据。云端未上传旧成果仍单独标记缺失；新证据不可回填成旧运行的日志。

归档恢复工具已实际执行：1930 个归档成员全部核验，并重建三组各 330 个生成文件。源码中的新 net10 IndependentInspector 也已重编译，其实际独立解析与本次 raw 资格检查数据相同。原始档案中的 README 保留归档准备时的本地 ZIP 命名；使用当前页面的 Release 下载地址和恢复命令。
