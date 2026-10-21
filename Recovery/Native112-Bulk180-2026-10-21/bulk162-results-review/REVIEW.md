# Bulk162 已有结果的详细复核

日期：2026-10-21（Asia/Shanghai）

本轮只读取现有 GitHub 运行记录、归档、DLL 字节、JSON 和日志，并对已保存的清单及生成源码做文本对照。没有构建候选、运行冻结验证器、执行游戏 CIL、运行 IL2CPP 或 Clang，也没有启动 Codespace。

复核对象：

- Bulk152 对照运行：[39416885984](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39416885984)。
- Bulk162 运行：[39443473339](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39443473339)，提交 `b2f89eb3caf60d7c6c2a36f486bed048023c467c`。GitHub 实际结论为 **failure**。
- 详细原始结果根目录：`../native-bulk162-r1-codex-2026-10-20/verification-actions-39443473339/Bulk162-Linux-evidence-39443473339-1/bulk162-run/`。

## 结论

本批有可定位的编译修复进展：新增的 10 个目标方法不再出现在诊断归属表中，合计减少 25 条诊断；剩余报错方法从 235 减至 225。归档的生成源码未显示方法丢失或游戏编译单元漏编译。

但“静态门全部通过”不能原样接受：**堆栈下溢负控制因 fixture 的字段名错误而发生 KeyError，包装器把退出码 1 当成了检测成功。该项为 INVALID_CONTROL_TOOL_CRASH，必须撤回通过。** 这不直接否定其他独立通过项，也不表示候选已被证明有下溢；它表示这项负控制没有提供声称的证据。

全部 18 个游戏 C++ 编译单元仍返回失败，合计 4,477 条诊断。本批新增方法的原版行为等价尚无相应执行证据。本轮不能晋级为 Unity、Apple Clang、Xcode、iOS 或 IPA 通过。

## 1. 确认的负控制缺陷

原始证据：

- `results/neg_control_bad_stack_dump.json`
- `results/static-invocation-06.argv.json`
- `results/static-invocation-06.stderr.log`
- `results/static-invocation-06.exitcode.txt`

第一个变异方法是 `0x0600000B Administrator::ResetLevelPassState()`，开头两条记录为：

```json
[
  {"offset": 0, "op": "pop", "operand": null},
  {"offset": 0, "opcode": "ldsfld", "operand": "..."}
]
```

真实 schema 需要 `opcode`，生成器却插入了 `op`，并且重复使用 offset 0。原始 stderr 的终点为：

```text
File ".../tools/frozen_verify_types.py", line 61, in verify
    ... op=x['opcode'] ...
KeyError: 'opcode'
```

stdout 为空，退出码为 1。这次执行没有产出这份负控制的 `.typed.json`，没有绑定目标 token 的下溢诊断。

运行对应脚本 `scripts/actions/static_gates.py` 第 815 行构造了上述错误输入，第 819 行只断言返回码为 1，第 820 行就打印 `Negative control (stack underflow rejection): PASS.`。所以异常退出被错误计为预期拒绝。

已将证据告知 AGY：保留原误接受材料；修复 fixture 和包装器；使用有效 schema、明确目标以及一致的 offset/控制流；要求实际下溢诊断并排除 Traceback/KeyError。冻结验证器保持不改。重跑属于 AGY 的验证任务，本轮未执行。

## 2. 可保留的构建与隔离证据

本轮重读两个已保存候选的字节并计算哈希，两份均为：

- 大小：16,523,264 字节。
- SHA-256：`089cddb15cc5f2d543576c4935e5c861a082a5853118c15f65a4d36e40116da8`。

原始构建记录将 Native93 `b0742241...72e2162` 作为父文件，Bulk152 仅作为比较对象。两份既有候选字节一致，支持记录中的确定性构建结论；本轮没有重新构建。

`CANDIDATE-PREPARATION-MANIFEST.json` 记录与 Bulk152 比较 2,317 个方法体，差异恰为新增的 10 个目标，其余 2,307 个相同。

`INDEPENDENT-ISOLATION-REPORT.json` 记录：162 个目标、2,155 个非目标、30 个 finally 目标，以及 14 个分别命中特定校验原因的隔离负控制。14 项中的 EH padding、TypeSpec、两类 MethodSpec、#US 及越界表行修改均有具体拒绝原因；不能因另一项下溢负控制损坏而抹掉这些独立证据。

以上隔离结论属于**已读到的验证器执行记录**，不是本轮重新运行独立隔离审计的结果。

两个“拒绝覆盖已存在输出 / 拒绝错误父文件”记录的退出码均为预期的 3。

## 3. 类型、可见性和引用解析的实际范围

| 已保存结果 | 实际内容 | 可以支持的范围 |
|---|---|---|
| 冻结类型检查全量输出 | 155 PASS，7 FAIL | 保留失败原文，不能写冻结版 162/162 通过 |
| 支持范围的子集输出 | 155 PASS | 此检查器支持的 155 个目标 |
| 扩展副本输出 | 162 PASS | 扩展模型下的类型流检查，不等于行为验收 |
| MemberAccess 正样本 | 162 目标，0 rejected，0 unresolved | 可见性解析范围内通过 |
| MemberAccess 私有字段负控制 | 1 rejected，0 unresolved | 此特定越权变体被拒绝 |
| Eager MemberRef | 1,170 条，0 unresolved，70 条追加引用解析完成 | 引用解析完成，不等于全部 ECMA-335 或运行时正确 |
| 下溢负控制 | KeyError，exit 1 | **无效；不计通过** |

冻结类型检查保留的 7 个失败是：

- `0069/006A/006B/008D`：`ldtoken` 模型不支持。
- `08BD/08BE`：`ldftn` 模型不支持。
- `01FB`：模型不能确认 `CultureInfo -> IFormatProvider` 可赋值。

扩展副本为 `ldtoken`、`ldftn` 和上述接口关系提供了补充。原始冻结失败与扩展通过应并列保留。下溢变体的问题在 fixture/包装器，不能通过放宽冻结工具解决。

## 4. 19 → 18 个编译单元的覆盖解释

本轮读取两份生成源码 tar 以及各自的 `GAME-TU-RESULTS.json`：

- Bulk152：760 个 `.cpp`，19 个游戏单元；19 个全部有记录。
- Bulk162：759 个 `.cpp`，18 个游戏单元；18 个全部有记录。
- 每条编译记录的文件大小和 SHA-256 与归档中的对应源码一致。
- 两版 `s_methodPointers` 均有 2,317 个槽位，槽位中的符号逐项相同。
- 两版均定位到相同的 2,267 个 MethodDef，没有新增或缺失 token。
- 原 `GodsPVZRuntime1__18.cpp` 的 `0x06000901–0x0600090D` 共 13 个方法，全部转移到新 `GodsPVZRuntime1__17.cpp`。

因此，现有归档支持**生成器重新分片**的解释，没有发现生成后漏掉游戏单元编译。这个结论来自保存的源码/回执对照，并不意味着所有 759 个非游戏及游戏 C++ 文件都经过了编译；实际 Clang 阶段只编译 `GodsPVZRuntime1*.cpp` 游戏单元。

完整数据见 [GENERATED-SOURCE-COVERAGE.json](GENERATED-SOURCE-COVERAGE.json)。

## 5. 编译错误减少的具体归属

| Token | 方法 | Bulk152 诊断 | Bulk162 诊断 |
|---|---|---:|---:|
| `0x060000B9` | PlantAnimationEvent.PC_SW_CreateCircle | 1 | 0 |
| `0x0600049A` | Zombie.ZC_LadderSaboteursBoom | 3 | 0 |
| `0x0600049D` | Zombie.ZC_NutBoom | 3 | 0 |
| `0x060005D2` | BoardPreview.CreateDevicePreview_Device | 2 | 0 |
| `0x060005D4` | BoardPreview.CreateEnemyPreview_Zombie | 3 | 0 |
| `0x06000711` | Zombie_Select.Initialize | 2 | 0 |
| `0x060007AC` | TMPro_InstructionOverlay.Awake | 1 | 0 |
| `0x060007AF` | TMP_ExampleScript_01.Awake | 4 | 0 |
| `0x060007B2` | TMP_FrameRateCounter.Awake | 1 | 0 |
| `0x060007B5` | TMP_FrameRateCounter.Set_FrameCounter_Position | 5 | 0 |
| 合计 | 10 个方法 | 25 | 0 |

总数从 4,502 降为 4,477，报错方法从 235 降为 225。退出报错集合的恰好是这 10 个方法，没有新增报错方法；其余仍报错方法的诊断条数不变。

方法归属算法为 `heuristic_next_definition`，不是 AST/函数体花括号平衡解析。因此这里只承认“当前诊断归属下不再报错”，不将其描述为逐方法独立编译成功或原版行为等价。两次均有 58 条包含文件诊断，不能忽略模板/头文件调用链的影响。

完整记录见 [COMPILER-DIAGNOSTIC-DELTA.json](COMPILER-DIAGNOSTIC-DELTA.json)。

## 6. 行为证据缺口与最终产品状态

`N109-FIXTURE-REPORT.json` 的 768 个正例分属 `0115`、`011A`、`01D3` 三个目标，每个 256 例；5 个变异体均被报告检出。这是特定旧 fixture 的回归结果，不是 Native110 新增 10 个方法的原版 native/CIL 行为对照，也不是 162 个方法的全量行为回归。

报告已明确声明 `isolated_target_with_stubbed_dependencies`、真实 `01D3 -> 01E0 -> 0677` 交互链 `UNPROVEN`、`formal_promotion_allowed=false`。这些限制必须继续保留。

IL2CPP 转换退出 0 是真实已有记录。后续 Linux Clang 18.1.3 使用 `-fsyntax-only`、`-ferror-limit=0` 对 18 个游戏单元检查，18 个全部退出 1，没有 fatal error 截断记录；失败仍有 4,477 条诊断。它不构成 macOS Apple Clang 编译、Xcode 链接、IPA 打包或真机执行证据。

## 7. 报告口径修正

- 实际支持程序集输入清单是 **263 个（含游戏 DLL）**，不是报告文字中的 311 个。两版名单一致，除游戏 DLL 外其余 262 个文件的哈希均一致。
- 实际 `dotnet --version` 输出是 **10.0.402**；应与配置请求的 10.0.401 区分。
- `MANIFEST-v1.json` 的 `Native152-Takeover-Verification` 和 `bulk152_current_measured` 是沿用的旧标签；新报告中的 4,477/225/18 属于 Bulk162，不能再当作另一份 Bulk152 测量。
- `108/108 attributed clean` 只覆盖旧名单，且 108 个方法所在单元全部失败，不能表述为 108 个方法的完整编译或正式行为通过。

## 8. 后续处理

已向 `GodsPVZ — AGY Flash 协作验证` 对话发送上述负控制缺陷及覆盖对照结果。AGY 继续既定 Bulk168 验证，同时修正 fixture 与报告；根代理维持源码修复分工。本轮未改历史证据或冻结验证器，未触发新工作流。

本目录的 `inspect_existing_results.py` 只读取归档、文本与 JSON 并输出对照清单，没有编译、导入或调用项目验证器。所有历史输入保持原位。
