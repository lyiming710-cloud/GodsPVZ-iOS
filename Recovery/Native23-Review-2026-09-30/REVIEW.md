# Native23 全面独立评估

> 此报告保留推送前的审查快照。“GitHub 尚未发布”“没有提交或推送”描述审查执行当时的状态；本次仓库更新已发布两个 Native23 检查点与审查资料。候选验收结论不变。

日期：2026-09-30。审查人：Codex。审查对象：Codespace `glowing-train-p7j9gp74q6jwc76v6`、仓库 `/workspaces/GodsPVZ-native19` 的 Native23 源码、候选、报告、工具及生成结果。

**结论：有可重复的编译修复进展；Batch1 六个修复站点可作为受限候选保留，严格原始字节隔离条件仍未满足。Batch2 / Batch2a 不能作为已经证明高还原或可靠排除污染的恢复基线验收。当前不具备启动完整 IPA 构建的条件。**

本轮完成了实际候选重建、独立读取 DLL、实际 IL2CPP 重转译、全量 Clang 预检、原 PC unwind/机器码核对、完整候选 CIL 的有限运行测试及对验证器的反例测试。没有修改对方补丁源码、候选、Git 分支或共享 canary，没有提交、推送或启动 GitHub Actions。审查新增文件均在独立 review 目录。

## 1. 版本与输入身份

Codespace 本地提交链经读取确认：

```text
09dd564962f6b83b3175194db97d95cb7e5346b9
  -> 232273a851538a73eaa9c6837efd091f063ebd62
  -> f1a96517d21826f0de9efb579b481dee4a9863b0
```

所在分支 `repair/codex-native19-ipa`，审查前后 `git status --porcelain` 为空；两个新提交直接继承 handoff。`.github/workflows` 相对 09dd564 无差异。GitHub 已发布的同名分支经 API 复核仍指向 09dd564，两个 Native23 提交没有发布到该分支。GitHub 最近运行中没有 Native23 的 IPA 构建结果；Native18 Run 36505731509 的最终结论仍为 failure。

| 输入 / 候选 | SHA256 |
|---|---|
| Native21 linked baseline | `0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f` |
| Batch1，3 方法 / 6 编辑 | `4d970f5ed7e63dd42815c3c843ce51918328ff790fa5d1eb77e2a80e6f3b0a1b` |
| Batch2，377 方法 / 1623 编辑 | `1d8e743df3407b3769f50885a55556ae380525595b4284ad223795af0aa22e5d` |
| Batch2a，249 方法 / 865 编辑 | `6e8caffc6188d1f93a72d999f955056836c6cc263e21cda8cad49222191d3c79` |
| 原 PC GameAssembly.dll | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |

从 f1a9651 的 RepairNative23 源码重新编译工具，每个候选各构建两次，六个输出都逐字节复现上述三个候选哈希。基线、编辑清单、工具源码、全部参与转换的程序集哈希及命令均保存。

**活动 canary 的 GodsPVZRuntime1.dll 实际仍为 Batch2 的 `1d8e743d...`，不是 Batch2a。** 此为审查前已有状态，审查没有改动。Git 工作区干净不能证明被忽略的构建目录输入正确。下次复用该 canary 必须显式选择并核对输入哈希，不能把它的结果称为 Batch2a。

## 2. 实际 DLL 隔离性

独立 C# Inspector 使用 Mono.Cecil 与 System.Reflection.Metadata / PEReader，读取程序集身份、metadata 表计数、类型/字段/方法声明、局部变量、指令、解析后的调用引用、分支与 switch 目标、EH 边界及原始 IL bytes。跳转规范化为指令索引，避免仅因 offset 移动产生伪差异；原始字节单独计数。

| 比较 | 逻辑变化方法 | 变化指令 | 逻辑相同的非目标方法体 | 计划外逻辑变化 | 计划外 raw IL 变化 |
|---|---:|---:|---:|---:|---:|
| baseline → Batch1 | 3 | 6 | 2294 | 0 | **18** |
| Batch1 → Batch2 | 377 | 1623 | 1920 | 0 | 0 |
| Batch1 → Batch2a | 249 | 865 | 2048 | 0 | 0 |

三组均保留 320 TypeDef、2317 MethodDef、2802 Field，2297 方法体；MVID、程序集/模块身份、读取的类型和字段声明、方法签名与属性均保持一致，没有 MethodDef 重排，没有计划外局部变量或 EH/分支逻辑变化，也没有发现悬空分支、switch 或 EH 引用。

Batch1 有 18 个非目标方法 raw IL 改变，但规范化后的 opcode、引用身份、局部变量及分支/EH 相同。这与 Cecil 重写调用引用的 metadata 编号一致。它不是已发现的 18 个语义破坏，却足以否定“非目标方法 100% 原始字节不变”。若严格 raw 隔离仍是验收条件，需要保留原引用编号或采用保持 metadata 的写入方式，并重新测量；不能用规范化隔离替代字节隔离。

证据：`evidence/review/independent-isolation.json`、四份 `*-independent-inspect.json`、`final-checks.json` 的 `non_target_raw_changes`。

## 3. 真实转换与全量 C++ 预检

在独立 `/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review` 为四组分别复制 ManagedStripped 输入，锁定主 DLL，实际重跑 IL2CPP，每组生成 264 个 `.cpp`；每个 TU 的 compiler exit、源码哈希和完整诊断日志单独保存。总计 **4 次转换、1056 次 Clang TU 预检**。

工具环境：dotnet SDK 10.0.401 / runtime 10.0.12，Ubuntu Clang 18.1.3；使用相同 Unity IL2CPP 工具和归档 libil2cpp / baselib 头文件，`-std=c++11 -fsyntax-only -ferror-limit=0`，Linux baselib 平台头。转换 profile 为原复测使用的 `unityaot-macos`。这是 Codespace 的转译及语法/类型预检，不是 Apple Clang、iOS 链接、Unity 场景运行或 IPA 打包。

| 组别 | IL2CPP exit | TU 数 | 失败 TU | `.cpp` error | `.h` error | 完整 error 总数 | 含 `.cpp` 错误的方法 |
|---|---:|---:|---:|---:|---:|---:|---:|
| baseline | 0 | 264 | 24 | 6006 | 64 | 6070 | 676 |
| Batch1 | 0 | 264 | 24 | 6001 | 64 | 6065 | 673 |
| Batch2 | 0 | 264 | 23 | 4368 | 64 | 4432 | 612 |
| Batch2a | 0 | 264 | 23 | **5136** | 64 | **5200** | **651** |

基线、Batch1、Batch2 的 264 个新生成 CPP 均与对应旧归档逐字节相同。对方的 6006、6001、4368 可以按“只统计 .cpp 报错”口径复现；64 条头文件报错不应在完整构建报告中漏报。它们主要位于 il2cpp-codegen-common.h 的模板操作，包含指针/整数非法运算；应追踪导致模板实例化失败的调用方 IL，不能直接修改 Unity 头文件来屏蔽。

Batch2a 是本轮新补测的独立结果，比 Batch1 少 865 条 `.cpp` error，确有编译改善。四组均未通过全量预检。按独立方法归属映射，三组修复相对各自父候选均没有新出现错误的方法或方法错误数增长；这不等于没有行为回归。Batch1 三个目标的生成函数体中原 5 条错误全部消失。

## 4. Fail-closed 类型门与实际行为测试

对实际 DLL 重新导出的 2297 个方法运行已有 fail-closed 指令子集验证器：

| 组别 | 全程序集验证失败方法 | 相对父输入 FAIL→PASS | PASS→FAIL |
|---|---:|---:|---:|
| baseline | 872 | — | — |
| Batch1 | 869 | 3 | 0 |
| Batch2 | 858 | 11 | 0 |
| Batch2a | 864 | **5** | 0 |

验证器没有 crash，但不是完整 ECMA-335 验证器。Batch2 的 377 个触及方法里仅 11 个整体通过；Batch2a 的 249 个里仅 5 个整体通过，另外 244 个仍不通过。6 个 Batch1 编辑分别单独回退，6/6 重新触发类型失败。原套件的 10 个规则反例也能复现通过；它们没有覆盖下文的污染传播反例。

新写的 RuntimeFixture 从实际 Batch1 DLL 读取完整 `Device.TryPlacing` 和 `Plant.Awake` CIL，通过 ILGenerator 发射执行，仅将引用绑定到显式定义的 Board/Grid/GameObject/Plant 辅助函数替身。没有重新写一个“看起来合理”的 C# 版本冒充候选。

- TryPlacing：16 场景，覆盖 board null 抛 NRE、grid null 返回 false、拒绝放置、允许放置，并验证坐标（含 Int32 边界）与 GetGrid→CanPlacing→Placing 调用顺序。
- Awake：8 场景，验证 Animator 写入、List.Clear、GameObject transform 获取、LoopAddAnimation 次序，各 receiver 为 null，以及辅助调用抛异常后的副作用前缀。
- 24/24 通过。两个类型合法但行为错误的 mutant（吞掉 Placing 副作用、从错误 GameObject 取 Animator）均被行为断言识别。

这为两个目标的有限方法行为提供独立证据，不证明辅助函数真实实现、Unity Object fake-null 行为、Plant.Update 的全部帧路径或整包游戏行为。仍没有真机/模拟器/Unity player 测试。

## 5. 原 PC native 核对

对原 PC GameAssembly 重新解析 .pdata 与 UNW_FLAG_CHAININFO；函数范围依据 unwind root 关联，而非地址连续。另用 capstone 独立解码指令并核对 native-method-map 和 native-fields。

- Plant.Awake `0x18034DEA0–0x18034DF58`，184 bytes。GameObject 字段偏移、Animator 写入、List.Clear 与 LoopAddAnimation 次序吻合。
- Plant.Update `0x18035C4D0–0x18035C8E3` 的八个片段全部属于同一 unwind root；本次八段判断正确。shadow 对应 GameObject.get_transform，Camera 对应 Component.get_transform；候选保留了 Camera 的调用归属。
- Device.TryPlacing `0x18034BE30–0x18034BEA9`，121 bytes。GetGrid→CanPlacing→Placing、两次引用判空及 board null 的不返回异常路径吻合。
- 进一步反汇编 `0x18046DC80` 与 `0x18042E9F0`：前者调用映射为 GameObject.GetComponentFastPath 的 `0x18131B3F0`，后者调用 Component.GetComponentFastPath 的 `0x181304010`，支持泛型调用的归属区分。
- Device.TestPlacing、AttackRange.TestInRange_Device、BoardInfoPage.SetLootList 的所检查函数范围也有独立 unwind 关联支持；**定位到方法不代表已完成所有编辑站点↔native 指令的映射**。

Batch1 六个具体修复方向有正面证据。没有据此授予所有三方法的完整行为等价或整个游戏高还原结论。

## 6. 必须纠正的审查发现

### F1 [P1] provenance 在 local、参数、调用和合流处丢失污染

`native23_prov.py:110–140,222–273` 中 `fail` 只改临时 pending；stloc 不记录存入来源，ldloc 再生成声明类型标签；starg 不持久化污染；ldfld 仅拒绝 MERGE/TAINTED receiver，会把 LOCAL receiver 的字段读取标成可信 FIELD；调用参数的失败可能被随后 pop/返回标签覆盖；不兼容合流返回而保留旧状态和结论。

对已提交且实际执行的源码，独立构造的四个污染反例（错误 local store、错误 starg、错误调用实参、不兼容合流）**四个都仍被 ACCEPT**。因此不是理论上的担忧，而是可复现的门禁错误。

对实际接受集合做只读追踪：865 站点 = FIELD 746、ARG 69、SFIELD 49、THIS 1；其中 **225 个 FIELD 站点，涉及 97 个方法，直接依赖 LOCAL receiver**。这与“经 local 中转不会洗白”的报告叙述不符。站点清单含 token、IL offset、字段读取 offset 和 local provenance。

这些 225 处是证据不足，不是已证明 225 处都修错；剩余 640 处也不能自动视为已通过，因为调用、参数及合流污染缺口仍存在。应实现持久化来源状态和保守合流、指令级污染传播，或先保守隔离尚未证明的依赖，再重算全体站点。不要只依据冲突减少或编译错误减少继续扩大批次。

### F2 [P1] 方法错误归属把声明当定义，覆盖真实错误

`native23_body_map.py:79–97` 把匹配到的声明和定义都加入 positions，并对同一 token 无条件覆盖 result。独立工具从 2317 项 CodeGen method pointer table 取得 symbol/token，只接受后续确有 `{` 的函数定义，再用真实闭合函数范围归属诊断。

原工具在基线、Batch1 各漏掉 **68 个**报错方法，在 Batch2 漏掉 **61 个**。正确数量是 **676→673→612**，而非 **608→605→551**；Batch2a 为 **651**。例如 Admin_system2_boardEdior.Initialize 的真实定义位于 GodsPVZRuntime1.cpp:5063–5486，有 16 条错误；原记录被 GodsPVZRuntime1__1.cpp 中的声明覆盖为零错误。

报告另举的 Projectile.SetDamage、Projectile ctor、Zombie.GetRandenAnimationSpeedMagnification 在本轮独立映射中确实也是零错误，不应反向误判这三个例子。需要修的是通用归属算法和由其产生的漏报数字。

### F3 [P2] 补丁器缺少输入形状约束及完整目标重定向

`RepairNative23/Program.cs:96–97,192–203` 的 to_ldnull 没有核对原 opcode 是否是整型零；Swap 只更新单个 Instruction operand，没有更新 Instruction[] switch 和 EH 边界。

独立反射测试实际 Swap：单分支更新成功，switch 和 EH 引用仍指向已被移除指令。另在隔离输入中请求把 ret 替换为 ldnull，实际工具 exit=0 并写出文件，没有拒绝错误清单。

本次三个真实候选未发现悬空目标，不能说这个缺口已经损坏它们。但该工具不能支持“任何目标形状错误均不写出”或“控制流完全安全”的通用保证。还缺 input SHA、预期旧 opcode/operand/signature、重复编辑检查；SameShape 未核对返回类型/ExplicitThis/calling convention。具体三处 GameObject 调用当前没有发现这些签名异常。

### F4 [P2] E1/E4/E5 数字未按自身定义计算

`native23_tiers.py:83` 为所有候选无条件赋予 E1，却定义 E1 为整体 FAIL→PASS；实测 Batch2 只有 11、Batch2a 只有 5，不能写 377 已通过类型层。

RE_TEST 将 `test %al,%al`、`test %eax,%eax` 也算 pointer-width null check；RE_CMPQ 接受 `cmpq $0x20,(%rax)` 等非零比较。三例均能复现命中。E5 是三个 token 的硬编码名单，BoardInfoPage 的三个接受站点没有各自 native 地址证据。E4=362、E5=3 应撤回为待核查索引或用真正的站点/宽度/常量验证重算。

### F5 [P2] 通用 native extent 仅按连续地址合并

`native23_native.py:61–84` 丢弃 UnwindData，按地址连续合并，独立扫描找到 **101 个**其窗口包含另一不同已映射方法入口的情况。例如 Administrator.Start 吞入 Administrator.Update。

本轮明确纠正一个界限：**Plant.Update 的八段实际正确，Batch1 三个目标范围均正确**；通用提取器有缺陷不能用于否定这三个已独立取证的范围。仍需用 unwind root、方法入口及 CFG 共同确定所有自动抽取范围。[Microsoft x64 unwind 文档](https://learn.microsoft.com/en-us/cpp/build/exception-handling-x64?view=msvc-170)规定了链式展开信息的关联结构。

### F6 [P2] 所谓具体执行器不实现基本存储语义

`native23_behave.py:264–282,462` 对 stfld/stsfld 只弹栈而不更新存储；很多算术/分支/调用使用未知值或签名替身。独立对实际源码测试：Owner.n 初始为 3，执行写入 7 后正常 RET，字段仍是 3。观察轨迹只比较调用名和分支，不充分比较 heap、return、exception 和结束状态。

因此“真的执行 249 个方法、747 处已到达”应降级为路径探索统计，不是 CLR/IL2CPP 的真实游戏方法覆盖。原报告主动承认 init/this 并未增加独立证据是正确的，但不足以让这个工具成为行为门。

### F7 [P2] 构建状态、发布状态与严格隔离表述需分开

18 个非目标 raw IL 差异需如实报告。Codespace 本地提交与 GitHub 已发布提交不同；共享 canary 当前仍为包含 758 个后来隔离站点的 Batch2。以后每次执行必须在实际读入点验证候选哈希，而非依赖目录名或 Git clean。

上一轮“Batch2a 缺完整构建链”的证据缺口已由本轮补测消除：现在有 Batch2a 的真实 DLL diff、IL2CPP 和全量预检结果。**结果为失败，不是未测试，也不是已通过。**

## 7. 对另一 AI 回复逐项判定

| 声称 | 独立判定 |
|---|---|
| 真实 dotnet 的部分 ceq 示例无法区分 null / 整型零形态 | 可保留为该 runtime 上的有限负面测试结论；不能推广为所有运行时或 ECMA 类型规则 |
| 污染不会经 local / receiver / call / merge 洗白 | **否定，四个反例以及 225 个实际接受站点存在相应缺口** |
| 865 ACCEPT / 758 QUARANTINE | 集合和候选真实存在；分类数量正确，ACCEPT 的安全含义不成立 |
| 真正执行 249 方法、747 站点 | 工具不实现完整具体语义；降级为路径探索，不算运行时验收 |
| 反例套件 10/10 | 能复现；反例覆盖不足，新四个污染反例均误接受 |
| E1=377 / E4=362 / E5=3 | E1 与实际整体类型通过不符；E4/E5 自动证据有上述缺陷 |
| 提交、哈希、检查点存在 | Codespace 本地确认；未发布到 GitHub 同名分支 |
| Batch2 编译改善 / Batch1 三目标清错 | 可复现；方法报错数量因归属 bug 需纠正，完整总数还需加 64 条头文件 error |
| E6=0 | 对全体方法的完整行为验收保持未证明是正确态度；本轮有限 fixture 不升级为全方法/全游戏 E6 |

[ECMA-335 第六版](https://ecma-international.org/wp-content/uploads/ECMA-335_6th_edition_june_2012.pdf)的 III.3.21/III.3.45 与比较操作类型表约束 CIL 操作数及 null 形态。某个 JIT 接受并执行损坏 IL，不能代替标准正确性、IL2CPP 可编译性或与原 native 的行为等价。

## 8. 下一步顺序与验收边界

1. 保留原可信 handoff 和独立实验候选，修复 provenance 与方法诊断归属门，补齐负控制。先建立可信验证结果，再扩大修复。
2. 重算 1623 个站点；225 个 local receiver 依赖先逐一证明或隔离，同时检查 starg、call argument、merge、字段 receiver 链。不将另外 640 处直接宣告安全。
3. 补丁写入前锁定输入/清单哈希和旧指令形状，写入后独立重新读取；检查分支、switch、EH、metadata 与 raw IL 隔离。每个目标的所有副作用、异常、循环及调用语义均以原 native 为准。
4. 不依据反编译伪 C# 填充控制流；不添加原版没有的 defensive null-return，不全局 ret→throw，不仅以编译器接受为由改 object/int64/Vector3 参数。对结构体核对 by-value / byref、地址生命周期与调用签名，对泛型核对闭合实例和 !0/!!0 绑定。
5. 对复杂方法分别检查正常、空引用、拒绝条件、循环边界、异常路径、字段写入和调用次序；类型负例与行为负例分开，行为 oracle 必须能够识别类型合法但语义错误的 mutant。
6. 同一锁定候选先真正通过完整 IL2CPP 与 Clang 预检，包括头文件模板实例化诊断，再进入 Apple Clang / xcodebuild / 链接 / unsigned IPA gate，最终核对真正存在且可下载的 IPA。当前 5200 条错误、23 个失败 TU 尚不满足该前置条件。

审查结果不批准直接扩大 Batch2a，也没有把它部署到共享构建输入。下一阶段应先纠正验证工具与候选选择，继续原目标：高还原、未签名 IPA。

## 9. 证据与复现

- `full-review-evidence.zip`：从 Codespace 回传，28,851,145 bytes，SHA256 `b7565f28d68a91620d42f9b6d154d7943beefe9a5932ece290eadf93fe994aed`。1260 个 archive members；其中 1259 个文件的内容哈希全部按 archive-manifest 核对通过，另一个成员是 manifest 本身。
- `DOWNLOAD-VERIFIED.json`：下载/逐文件验证记录。
- `evidence/review/status.json`：六次 DLL 构建、四次转换、1056 次 Clang 检查汇总。
- `evidence/review/independent-isolation.json`：实际 DLL 的独立隔离检查。
- `evidence/review/analysis.json`：四个污染反例、225 站点清单、全量类型门与 6 个回退负例。
- `evidence/review/final-checks.json`、`attribution-comparison.json`：纠正后的方法错误归属、遗漏实例及最终工作区/候选身份。
- `evidence/review/runtime-fixture-results.json`：24 场景与两个行为 mutant 的真实运行结果。
- `evidence/review/patcher-swap-probe.json`、`wrong-shape-patch.log`：switch/EH 与错误清单的补丁器反例。
- `native-independent.json`、`native-helpers-independent.json`：原 PC unwind/反汇编独立核对。
- `evidence-tool-probes.json`：执行器字段存储错误、正则误判及 101 个跨方法窗口探针。
- `evidence/original-sources`：被审查的原工具源码快照。`Inspector.cs`、`RuntimeFixture.cs`、`start_full_remote.py`、`audit_analysis_remote.py`、`final_checks_remote.py` 为审查新增工具；新增工具源码及运行命令均归档。

未覆盖：全部游戏方法的 native 行为等价、完整 ECMA 验证、Apple 平台编译/链接、Unity player/真机场景行为、性能及存档兼容性。没有 IPA 产物。上述未覆盖项不能由“零新增类型错误”或“同一事件轨迹”替代。
