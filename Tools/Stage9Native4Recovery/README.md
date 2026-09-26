# Stage9.1 native4：新增两方法恢复

用户在 native3 真实门禁失败后明确要求直接推进下一步。本轮仍从唯一可信 `18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd` 生成候选；保留 native3 四方法实现，新增 `0600039F` 与 `06000267`，合计六个 MethodBody。`72fe4526…1343bc` 只作为前四方法的语义对照，不作为生成输入。

## 输入失败与证据

真实门 Run `36213730164` 第 23 步报出 `Plant.SetAnimationState_Planting` 的 `Invalid global variables count` 和 `SunManager.SetMaxSunFallTime` 的 `Cannot get stack type for Enumerator`。两个方法在 18e44 与 native3 正文一致。本轮不更改已经结束的工作流或工件。

原 PC ZIP、GameAssembly、metadata 哈希与 native3 相同；每个方法的 MethodDef RID、CodeGenModule 指针、`.pdata` 边界、机器码哈希见 `evidence/native-input-manifest.json`。完整反汇编、字段偏移、常量、字符串和调用映射已保存。

## Plant.SetAnimationState_Planting：0600039F

Native `180358310–1803583EE`，222 字节。

1. `[Plant+70] attackRangeUIController.CollapseView()`；null 到 `1803583E8` 抛异常。
2. `[Plant+178] shadow.SetActive(true)`；null 同样抛异常。
3. `[Plant+168] animator.SetFloat("speed", [Plant+C0] updateRate)`；null 抛异常。
4. ID 4 或 49：`attackable=false`，返回。
5. 其余 ID：`attackable=true`。ID 2 额外读取 animator、设置 `state=1`、调用 `SetTrigger("ExplodeTrigger")`。

原 managed 方法除分支残栈外，还把前三步改成 null 时跳过，并丢失了整个 ID 2 分支。新 body 是依据原版完整重建，不是单独删除一个 `ldarg.0`。

字符串通过 metadata usage 编码独立解码：slot `181BCADF0` = `a0003e6b` → literal index 7989 → `speed`；slot `181BC2AD8` = `a00010bb` → index 2141 → `ExplodeTrigger`。Animator 原 PC AnimationModule 89 个 method pointers 位于 `181CBCF50`，确认 `SetFloat(string,float)` / `SetTrigger(string)` 对应调用地址。SetFloatString / SetTriggerString 与公开包装方法共享 native 指针，managed 调用使用基线已有公开签名。

## SunManager.SetMaxSunFallTime：06000267

Native `180340990–180340B80`，496 字节。

- boardConfig 为 null 时直接返回并保留旧值，这是原版明确的正常返回分支。
- 使用 `boardConfig.map`（+30）及其 `senarioState`（+18）；第二个 Map 参数没有读取。
- 场景 0 → 8，8 → 16，9 → 12，10 → 24，其余（包括 1）→ 0。浮点常量已从原版 `.rdata` 读取验证。
- 枚举 `boardConfig.boardEntries`（+68）；仅当 `must || select` 才处理词条。
- 类型 2：累加自身一次，即翻倍；类型 3：设为正零；其他类型不改变时间。
- 完整 `Enumerator<BoardEntry>`、MoveNext/Current 与 finally Dispose。原版 normal/exceptional Dispose 地址在 `180340B11–180340B3B`，异常继续抛出。
- 只有成功遍历和 Dispose 后才写 `[SunManager+2C] sunFallTimeMax`；null map/list/entry 或遍历异常都保留原输出。

## 验证和限制

新候选：`candidate/Assembly-CSharp-native4-six-method.dll`。

SHA256：`11c1a9648ea67cb0ac66b84ddde50cb4ef46d24152f67515a99afb526a2d4b4e`。

六方法规格共 599 条 CIL；新增 Plant 48 条、Sun 88 条。实际 DLL 回读执行相同类型化栈检查，加入本次两种损伤的负例，总计六个负例。CLR harness 移植候选的实际六个 MethodBody，共 74 项断言，包括旧 31 项、新场景分支、词条组合、调用顺序、触发器字符串、null 异常和写回时机。

语义门仍核验 2317 MethodDef、2802 FieldDef、320 TypeDef、24/24 历史引用、23/23 Resolve keys、3/3 transform sites、无孤立泛型和非目标方法零语义变化；额外逐方法比较 native3 原四个 body 保持不变。

验证范围是六个方法的实现，不代表整个游戏可运行。`AttackRangeUIController.CollapseView` 的基线仍有 GetEnumerator/Dispose 占位和未初始化的枚举器；它是原版调用的独立依赖，本轮保留其正文，并记录为后续语义审计项。CLR harness 对 Unity 与这个 UI 依赖使用测试替身，因此通过测试不能证明 UI 依赖本身已修复。

只有完整静态门通过后才运行一次真实 Unity 门；导出、Xcode 审计与工件上传全部成功后才进入 macOS。生产分支和原 audit 分支保持冻结。

## 当前状态

本地完整静态门通过：六方法栈检查、六个负例、74 项 CLR 断言、原四方法保留检查、非目标方法零变化，以及目标引用全部 Resolve。精确 Editor AnimationModule 已从原始 15 个分片合并校验后的 Unity China 归档提取，归档 SHA 仍为 `0008115c…9a1fe14`；精确路径 `Editor/Data/Managed/UnityEngine/UnityEngine.AnimationModule.dll`，文件 161792 字节，SHA256 `e514adf83b0ba4887fb073e30d2e05e1b35aded593e32c2ebe8b15cfb5b5b75e`。模块作为哈希锁定的静态解析依赖保存，不注入游戏 candidate 或 Unity 工程。
