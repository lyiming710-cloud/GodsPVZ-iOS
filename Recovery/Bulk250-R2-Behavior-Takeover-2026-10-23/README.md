# Bulk250-R2 受控 CLR 验证接管检查点，2026-10-23

用户要求在 Gemini 额度耗尽期间由 Codex 接手验证。本检查点扩充实际候选 CIL 的受控 CLR 执行，不改游戏候选 DLL、不改补丁器、不推进正式验收计数。

## 已执行的结果和边界

输入仍为 16,573,440 字节的 Bulk250-R2 candidate1.dll，SHA-256：

    4732b0c1ee64be4224649ee43c88ddd805f5e1ea461e43ecf52febd7a756f903

本地完整运行见 local-gate-v5-final.json、local-gate-v5-final-logs/。系统为 Windows x64，实际 CLR 为 .NET 10.0.13。候选在运行前后均核对哈希。

随后在 [GitHub Actions 39934578072](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39934578072) 对提交 09b57a9fe87d2cf4c81cc86c6e793091aaaeed19 真实复现。Ubuntu 24.04.5 LTS x64、实际 CLR .NET 10.0.13 下同样为 154 个用例、16 个被检出变异体、10 个拒绝控制，工作流成功。原始产物保存在 ci-39934578072/；CI-REVIEW.json 核对了 43 个日志文件的大小与哈希、全部发布源码哈希及逐项结果。

CROSS-PLATFORM-COMPARISON.json 对照两个已执行报告：154 个用例和 16 个变异体的完整观测行均一致，10 个实际方法输入和 16 个变异体输入的 canonical CIL 哈希均一致。跨平台一致仍然只属于受控 CLR 范围，不增加原版 native 行为证据。

| 原来 v4 的这批 50 个目标 | 当前检查点 |
| --- | --- |
| 有受控 CLR 执行记录 2 个 | 8 个 |
| 尚未执行 26 个 | 20 个 |
| 声明缺少引擎环境 22 个 | 22 个，未将探测或静态阅读算成执行 |
| 辅助 getter 实际执行 2 个 | 仍为 2 个，不重复算进 50 个目标 |
| 正式原版行为验收 | 本轮不更新 |

154 个用例实际执行并通过；16 个内存 CIL 变异体实际执行并被检出；10 个独立进程的拒绝控制通过。进程退出码为零仍需检查逐项结果、精确目标集合、用例数、变异体数、依赖与执行二进制哈希、以及未执行/不支持状态。

这里 PASS / methods_genuinely_passed 是继承的字段名，含义严格限定为受控依赖 CLR fixture 通过。它不表示原版机器码语义等价，也不是全程序集、Unity、Apple Clang、Xcode、iOS 或 IPA 资格。

## 本轮新增执行的六个目标

| Token | 方法 | 主要观测 |
| --- | --- | --- |
| 0x06000287 | Map.GetMapGrids | 展平顺序、重复与 null、双层 finally、集合失效 |
| 0x0600060A | BoardEntry.FindKey | 来自真实候选的六个后缀字面量、must/select 组合、首个匹配、清理 |
| 0x060004BF | Save.CompletenessTest | Reload 调用、排序和稳定性、整数边界、回调及异常清理 |
| 0x06000187 | DeviceManager.GetBoardEntryData | 三属性复制、33/34/35 分流、选择标志、int.ToString 键、float32 顺序 |
| 0x0600026F | ZombieManager.GetBoardEntryData | 0/6/32 分流、0 分支两次独立 GetValue、保持 Def、异常清理 |
| 0x060001A8 | EnemySelecter.FillWave | 预算、/3、每十波奖励、筛选顺序、权重、重复引用、回调时序、溢出 |

前两个既有目标为 CreateNewEnemy / DisplayBan_device；既有辅助 getter 为 BoardConfig.get_Map / Device.get_AnimationSprites。主程序从哈希锁定的候选读取 CIL，经显式类型与成员适配后用 DynamicMethod 在真实 CLR 执行，不用手写方法实现代替候选方法体。

GetBoardEntryData 的参数真实类型为 int32，候选调用 System.Int32.ToString；不要把它错认成 DeviceType / ZombieType 枚举 ToString。

## 修复的是测试器，游戏候选没有改变

继承的 41046f8 在两个 Actions 运行中失败，最近的 39908405432 在生成完整报告前抛 Sequence contains no matching element。根因是排序变异器寻找 ble，候选实际为 IL_0081 bge。本轮先在本地复现原失败，再按偏移和原 opcode 锁定该比较，并改为 ble 制作真实错误变异体。

新增严格签名验证还暴露 CloneMethod 丢失 DeclaringType：普通候选用例通过，但六个新增变异体均因测试器上下文缺失而失败。next-three-normal-v2.json 如实保存 6 个未检出及异常，不计通过。修复克隆的所属类型后，变异体才实际执行并被检出。克隆只存在内存中，不添加到候选模块的方法表，也不保存为游戏 DLL。

所有失败和过渡日志保留在 logs/ 和相应 JSON；12-six-targets-strict 的泛型返回值映射失败也保留。最终源码显式闭合 List<T>.Enumerator 返回值中的 !0，并拒绝未知类型、成员、opcode 和异常区间。

排序比较反转的变异体只用不同 ID 检出，不用重复 ID 运行它，以免该错误变异体对相等项无休止交换。进程有 30 秒上限，超时不能算 PASS。

## 仍未证明的依赖

- 新数据对象构造器、BoardEntry.GetValue 使用受控替身。GetValue 的配置读取及副作用没有在原版验证。
- UnityEngine.Object.op_Inequality 使用受控 destroyed/null 适配器；测试该调用的分支响应，不证明 Unity 原生假空语义。
- FillWave 的 ZombieSelectRandomGet / AddEnemy 使用可记录调用的受控替身，未证明随机分布、真实敌人创建和跨方法副作用。
- ReloadPlantSaveData、图形/Transform 等既有替身仍保留原有证据限制。
- List<T>.Enumerator 为宿主 BCL 的真实枚举器加 Dispose 记录包装，未宣称 Unity 原版运行库相同。
- 本轮没有执行原版 GameAssembly.dll oracle，没有重跑 IL2CPP / Unity / Xcode，没有生成 IPA。

Bulk388 的源码工作另在 source/native120-bulk388-2026-10-23，提交 60c05bd551b58114e8cf19137fdafed554334485，仍为源码检查点，不因本报告获得候选资格。Bulk350-R6 的历史编译失败也没有被本报告消除。

## 复现

在此修复分支的仓库根目录，安装 .NET 10 SDK，通过对应 Release 下载锁定候选：

    gh release download bulk250-r2-candidate-2026-10-22 --repo lyiming710-cloud/GodsPVZ-iOS --pattern candidate1.dll --dir /tmp/bulk250-r2-v5-input
    python3 scripts/actions/test_behavior_bulk250_r2.py --candidate-dll /tmp/bulk250-r2-v5-input/candidate1.dll --output-json /tmp/bulk250-r2-v5-result.json --fixture-project scripts/actions/Bulk250R2BehaviorFixture.csproj

运行器只构建一次，然后分别启动受限时长的独立进程。输出路径必须不存在，防止旧报告被当作新结果。脚本拒绝 Python 优化模式，因为它会禁用断言。在线项目引用 Mono.Cecil 0.11.6；执行时额外核对实际 Cecil DLL 哈希。离线 source/Fixture.csproj 需要在同目录放入 INPUT-LOCK.json 指定哈希的 Mono.Cecil.dll，二进制未重复塞进本源码档案。

10 个拒绝控制：断言失败、变异漏检、未知 getter opcode、非预期异常、故意错误的预期 SHA、未知新增 opcode、未知新增成员、未知 local、错误 EH，以及对正常入口提供一个末字节翻转的真实文件。后一个文件由脚本在新输出目录构造，原候选不变；归档保留生成方式、哈希和日志，不重复上传 16 MB 错误文件。

冻结验证器不变。新测试器不是独立 native oracle，不能用它的绿色输出代替高还原证据。

source/ 保存本地实际执行时的源码字节；scripts/actions/ 的发布源码按仓库惯例规范为 LF。SOURCE-PUBLICATION-MAP.json 记录文件名、换行和对应哈希，Linux CI 对发布后的源码重新编译并记录独立运行身份。MANIFEST.json 绑定本归档的文件字节，不用 Git 自动换行转换改写历史日志。

## 接下来与 Gemini 恢复额度后的交回点

QUEUE.json 列出全部 20 个未执行方法、22 个引擎上下文缺项，以及下一组建议目标 KillAll / Enemy..ctor / SetPrechooseList。cil/ 中对这三个方法仅作了只读 CIL 提取，未执行它们。

Gemini 当前因额度耗尽停止，本轮未向它继续发送任务。额度恢复后，从本检查点源码、local-gate-v5-final.json 与日志接手，优先独立审查候选绑定与受控调用边界，再扩充缺失方法。不要重新从“总数绿色”推定正式验收，不改候选输入和历史失败证据。
