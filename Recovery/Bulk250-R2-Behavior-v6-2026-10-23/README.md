# Bulk250-R2 受控 CLR 执行 v6，2026-10-23

本检查点接续 76a39e4 / v5。Gemini 额度耗尽期间继续由 Codex 执行，未向 Gemini 发送新任务。候选游戏 DLL、恢复补丁器、冻结验证器及 v5 归档均未修改。

## 实际结果

本地 Windows x64 / .NET 10.0.13 已完成 local-gate-v6-final.json 对应的真实执行：

随后 [GitHub Actions 39940181375](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39940181375) 对源码提交 8f037d8db165890930707704d0152f5507a228d3 在 Ubuntu 24.04.5 LTS x64 / .NET 10.0.13 上实际复现并成功。CI-REVIEW.json 绑定原始报告、55 个已核对大小和哈希的日志文件及发布源码哈希；原始下载产物在 ci-39940181375/。

CROSS-PLATFORM-COMPARISON.json 对照两个已执行报告，214 个用例观测行、27 个变异体观测行均完全一致；13 个执行方法输入及 27 个变异体输入的 canonical CIL 哈希也一致。这个一致性结论不包含原版 native 或 Unity 行为。

| 范围 | v5 | v6 |
| --- | --- | --- |
| 50 个目标内的受控 CLR 通过方法 | 8 | 11 |
| 尚未执行 | 20 | 17 |
| 仍声明缺少引擎上下文 | 22 | 22 |
| 实际执行的辅助 getter，单独计数 | 2 | 2 |
| 正向及边界用例 | 154 | 214 |
| 被实际执行并检出的语义变异体 | 16 | 27 |
| 独立进程拒绝控制 | 10 | 14 |

新增三个目标全部执行：

| Token | 方法 | 新用例 | 新变异体 |
| --- | --- | --- | --- |
| 0x0600027B | ZombieManager.KillAll | 11 | 4 |
| 0x060001A1 | Enemy..ctor(Enemy copy) | 27 | 3 |
| 0x060006BB | SeedChooserScreen.SetPrechooseList | 22 | 4 |

PASS / methods_genuinely_passed 是沿用字段，严格表示受控依赖 CLR fixture 通过。本轮没有新增原版 native 语义验收，没有更新正式合格计数，没有执行 Unity / IL2CPP / Xcode，也没有生成 IPA。

## 执行方式及保留的行为

仍以 SHA-256 为 4732b0c1ee64be4224649ee43c88ddd805f5e1ea461e43ecf52febd7a756f903、16,573,440 字节的 candidate1.dll 为唯一输入。Cecil 读取该候选的实际指令、类型和 EH，DynamicMethod 逐条发射并由真实 CLR JIT 执行。没有用手写的被测方法实现替代候选方法。

- KillAll：先遍历原列表收集存活对象，再对副本调用伤害；原列表在回调中清空或替换，不影响已捕获副本；后续对象被标记死亡仍按副本处理；重复引用被调用两次；每次创建独立 Damage，保留伤害值、属性和三个布尔标志；两轮枚举各自清理，异常不伪装为通过。
- Enemy 构造方法：在 RuntimeHelpers.GetUninitializedObject 创建的全新宿主对象上执行候选构造方法体，真实调用 BCL System.Object 构造器。候选初始化 entryKey、新路径列表、复制普通字段和 Vector3 值，并为每个源路径项调用受控 EnemyPath 构造器。验证路径对象身份、重复源引用产生不同新对象、isEnd 在构造回调后读取、源列表捕获与目标字段重新读取，以及 -0、NaN payload、Infinity 的浮点位模式。
- SetPrechooseList：只重置原先 chosen 的卡；先调用 Awake(null)，然后逐个请求遍历当前卡列表；不提前结束重复 ID 的匹配；Choose_Being 回调后重新读取 plantDetail，下一轮请求重新读取卡列表。测试 null、同一列表作输入、列表失效和回调异常下的三段 finally。

GetValue、随机选择、真实伤害处理、EnemyPath 构造器、Choose_Reset、Choose_Being、PlantDetail.Awake 等依赖仍使用受控适配器。它们的原版行为与副作用为 UNPROVEN。特别是 Choose_Reset 本身仍在未执行队列，不能因调用它的替身就算这个方法已验收。

DynamicMethod 会重新生成宿主 CIL 布局；这不验证候选原始 PE 的加载、MaxStack / EH 原始编码、Unity 泛型 AOT 或原版机器码。原始候选的隔离性和构建门仍须单独验证。

## 工具变化和拒绝控制

明确绑定新增字段、方法签名、值类型 Vector3、String.Empty 和受控构造器；保留既有的未知类型、成员、opcode 拒绝。扩展 finally 发射器以处理这三个方法的顺序及嵌套区间，检查实际 Instruction 身份、区间闭合与 endfinally 栈，不按偏移字符串重建控制流。

保留 v5 的 10 项拒绝控制，新增四项：交叉错误的 EH、未知字段、构造方法签名不匹配、指向方法体外 Instruction 的分支。14 项均由独立限时进程运行，预期错误种类或退出码不匹配就失败。

本轮最初缺少 SeedChooserScreen/PlantDetail 嵌套类型绑定时真实失败，日志 01-normal-v6 保留，未算通过。source-snapshots/initial-214-cases 保存扩充最后四项拒绝控制之前的已执行源码；最终执行源码在 source/，完整进程流和哈希在 local-gate-v6-final-logs/。

## 复现与后续队列

仓库根目录安装 .NET 10 SDK，下载固定 Release 资产后运行：

    gh release download bulk250-r2-candidate-2026-10-22 --repo lyiming710-cloud/GodsPVZ-iOS --pattern candidate1.dll --dir /tmp/bulk250-r2-v6-input
    python3 scripts/actions/test_behavior_bulk250_r2.py --candidate-dll /tmp/bulk250-r2-v6-input/candidate1.dll --output-json /tmp/bulk250-r2-v6-result.json --fixture-project scripts/actions/Bulk250R2BehaviorFixture.csproj

输出文件和日志目录必须是新路径。每个 fixture 子进程限时 30 秒；超时不能通过。脚本核对精确目标集合、每项观测、214 个用例、27 个被检出变异体、17 个待执行状态、22 个引擎缺项、执行二进制和 Cecil 哈希。

source/ 是本地实际执行源码，在线 scripts/actions/ 发布版本只规范文件名和 LF 换行；SOURCE-PUBLICATION-MAP.json 记录对应哈希。离线项目仍需 INPUT-LOCK.json 指定哈希的 Mono.Cecil.dll。归档不重复保存运行器构造的 16 MB 错误候选，保留生成规则、哈希和日志。

QUEUE.json 列出 17 个未执行目标和 22 个上下文缺项。下一组建议复用本轮类型与调用边界，处理 LoadTeam_Card、Choose_Reset、CanPlacing；实施前仍需读取实际候选方法，不能从建议直接判定可执行或通过。Bulk388 源码和历史 Bulk350 构建状态保持独立。
