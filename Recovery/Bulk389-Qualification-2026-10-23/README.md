**最新：真实流程 39961120509 已失败于重新导入的 CS2001 日志门；未进入 Xcode，无 IPA。已做扩大检查并修复构建工作副本切换，候选 DLL 未改。见 [扩大审查记录](ios-failure-39961120509/README.md)。后文启动时的观测按历史保留。**

# Bulk389 R4：候选集成与静态验证检查点，2026-10-23

**最新进展：R4 已通过 [Linux 编译诊断 39957635669](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39957635669)，14 个游戏 TU 全部零错误；[真实 Unity → Xcode → 未签名 IPA 流程 39961120509](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39961120509) 已启动，尚未回收 IPA。以下保留完整验证层次与历史失败。**

本轮已从固定 Native93 构造实际候选，并修复集成检查暴露的问题。最终候选包含 **389 个目标方法**（Bulk388 加一个有原版证据的公开伴随入口）；两次构造逐字节一致。独立回读、源发射器绑定、389 方法静态类型检查及成员访问检查均在各自有限范围内通过。**这不是完整游戏行为、Unity、Xcode 或 IPA 验收。**

- 唯一构造父 DLL：`b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162`，16,033,792 字节。
- 最终 R4 候选：`ef1992326d1ee494a389bc2c78bdfab131721434c0b08ab5f41dbc3e506c4e48`。准确大小和工具哈希见 `CURRENT-RESULT.json`。
- 文件名仍继承 `Bulk388.csproj` / `Bulk388.dll`；**实际写入范围以 R4 的 389 项 implementation plan 为准**。
- 候选取自 `BULK389-R4-CANDIDATE.zip`；请先校验 `DELIVERY-MANIFEST.json`，再校验解压 DLL 的完整哈希。

## 实际完成的验证

| 门 | 本轮结果 | 限制 |
| --- | --- | --- |
| 元数据列宽边界 | 32 个正常用例，7 个输入拒绝，8 个独立损坏输出均被检出 | 合成表数值编码，不是游戏执行；包含框架支持的表，不含旧式 AssemblyProcessor/OS 表 |
| 构造重复性 | 两份 R4 DLL 逐字节一致 | 确定性不等于正确性 |
| 回读隔离 | 37,512 个既有数值列保持原值；1,928 个非目标方法保持原样 | 其中 1,908 个有方法体，20 个零 RVA；目标旧槽位也未覆盖 |
| 附加元数据 | Blob 66,518 字节，所有需要的 Blob 索引扩为四字节；201 个 finally 区域 | 精确堆前缀、追加内容、行身份和结构已检查，行为仍需独立门 |
| 源码与候选绑定 | 389 个实际方法 IL 与相应编译后源发射器输出一致 | 源发射器不是行为 oracle |
| 旧版本继承 | R6 的 350 个原始方法体／头／EH 与 R4 完全相同 | 继承的是字节，不能继承不存在的行为资格 |
| 静态类型 | 389 / 389 | 有限检查器，不是完整 ECMA 验证或 CLR 游戏执行 |
| 成员访问 | 0 拒绝、0 未解析；未改变既有可见性 | 不替代 protected receiver 等全部规范检查 |
| 检查器反例 | 11 个静态模型反例、1 个实际私有调用字节变体、8 个隔离变体、2 个构造拒绝控制通过 | 不与游戏行为变异体混计 |
| 新入口受控行为 | 184 个用例：真实原版 PC 机器码 vs 最终候选实际 CIL，轨迹及字段结果一致 | 仅 `Projectile.Collision_Wall` 正常非空路径；依赖内部为受控回调，空指针／异常路径未执行 |

新入口行为测试的有符号范围变体产生 100 处分歧，漏掉 audio/particle 调用的变体产生 184 处分歧。二者是从实际 CIL 解码后修改的模型，不是完整游戏变体。正例执行的是候选实际 CIL，oracle 执行的是原版 VA `0x18037b090` 的原始 84 字节，并仅替换外部调用目的地。运行环境是 Windows x64 / .NET 10.0.13；Linux 没有执行这个 Windows 专用 oracle。见 `COLLISION-NATIVE-CIL.json`。

## 修正内容与范围

1. `Board.Update_BoardKey` 用受约束的枚举 `ToString` 调用保留精确 receiver 类型。
2. `Zombie.ZC_IVH_Skill1_Boxing_step3` 的错误 `List<object>.Remove` 绑定改为原版读取的 `Transform.get_localScale`。
3. `SeedChooserScreen.Determine` 的模板克隆改用已有 `Instantiate<Card_Choose>` 实例，避免传到 `Instantiate<Device>`。
4. `TMP_TextEventHandler.LateUpdate` 的 sibling event locals 使用实际共有的封闭 UnityEvent 基类，word/line/link 调用使用正确的现有封闭 Invoke 引用。
5. `VertexShakeB.MoveNext` 写回自己的 `<>2__current`，不再写 ShakeA 的字段。
6. `SkillManager.Function22` 使用现有公开 `Plant.AnimationSprites()`，其 managed getter 与原版 native 都只读对应字段；不放宽私有字段权限。
7. `Almanac_PlantDetail.SetLevelUpPreview` 追加一个公开整数 `System.Math.Clamp` MemberRef，避免跨类型调用私有 ThrowMinMaxException。
8. `SkillManager.Updata10` 调用明确修复的公开 `Projectile.Collision_Wall()`，替代跨类型私有调用。其原版 native 中内联路径与该入口相同。
9. 新增伴随目标 `0x060003E9 Projectile.Collision_Wall`：按原版 unsigned 区间判断恢复，保留 explode → audio/particle → destroy 顺序；没有修改私有 audio/particle 方法体。

R3 的目标总数由 388 增至 389、非目标总数由 1,929 降为 1,928，均已在独立范围检查中显式核对。它不是静默扩大既有白名单。`SOURCE-REVISION-MATRIX.json` 列出具体字节差异。冻结的既有检查器未被改动；本轮新隔离器增加了跨列宽的数值比对，正负控制与失败记录均保留。

## 历史失败与当前待办

首份 Bulk388 候选只有 383 / 388 静态类型通过，并有 6 处非法成员访问；第二份为 387 / 388，仍有 2 处非法访问。第三版才达到上述结果。完整历史日志在 `COMPLETE-LOCAL-CHECKPOINT.zip` 中，不能只看最后一份报告。

测试工具的失败也保留了：首版合成夹具包含框架不支持的旧表而被拒绝；数值回读脚本一次因重复解析过慢被中止，随后缓存计数后有界完成；旧追加器的首个超限输入先触发 checked overflow，较小的超限输入才到达指定 `No heap index widening` 保护；新行为夹具首次编译缺 Cecil 引用，补齐已认证的原库后才执行。以上均未改写成早先就通过。

最后完整编译仍是 **2026-10-22 的 Bulk350-R6 / run 39790796339**：38 个去重报错方法，1,616 条诊断，14/16 个游戏 TU 失败。R4 尚不能把这些数字扣成零。新的 `bulk389-root-compiler-check.yml` 只做固定 SHA 候选的 Linux IL2CPP 转换和全部生成游戏 TU 的 Clang18 检查，不运行 Unity Editor，不做真实 Xcode，也不导出 IPA。它复用与 R6 相同 Git blob 的 portable compiler engine，强制检查候选和 AnimationModule 哈希，编译失败保持非零退出。

后续先读新编译的完整日志并独立归属、去重剩余方法；结合原版继续行为门与异常／引擎集成门，满足进入持证 Unity 和 macOS 构建的条件后再走真实导出。当前 50 方法 Bulk250-R2 CLR 队列的 14 通过／14 待执行／22 上下文缺项属于另一候选，保持原口径，不与本轮 389 相加。

## 文件使用

- `CURRENT-RESULT.json` 是本检查点权威状态；`source/` 内旧的 SOURCE_ONLY / untested 注释和历史清单保留生成时语境，不代表本轮最新结论。
- `COMPLETE-LOCAL-CHECKPOINT.zip` 包含 711 项核心本地路径：各次候选、R1/R2/R3 源码、二进制、完整日志、独立验证器、边界夹具及回读结果。`CHECKPOINT-CONTENTS.json` 逐项记录字节与 SHA-256，已重新解压读取核对。它在 CI 发布准备前封存；CI 文件以仓库 `.github/workflows/` 和 `scripts/actions/check_bulk389_compiler.py` 为准。
- 恢复 ZIP 至工作区 `audit/bulk388-qualification-2026-10-23/`；保留 Native93、原版 GameAssembly 和历史辅助依赖的原路径，或审阅后调整输入路径，禁止把旧日志路径当作新一次执行证明。
- 本轮 `run_recorded.py` 的 `artifacts` 字段记录初始冻结补丁器；R2/R3/R4 的实际 argv、源码、构建输出、候选、SOURCE-BINDING 报告及完整包另有精确身份记录，不能用旧 `artifacts` 字段冒充 R4 运行身份。
- 原版输入只读，补丁只从 Native93 构造；R6 仅作对照。请勿在旧候选上叠补丁、修改冻结验证器、删除 finally、弱化负控制或把回调内部未证明的效果标成已证明。


## 提交前追加的 R4 元数据审查

R3 方法指令的静态类型与成员访问虽通过，尚未覆盖未引用的新 MemberRef。新增检查器首版也出现了 false accept：Cecil 会为损坏的同模块签名补出 GenericParameter 占位对象，不能据其内存 Count 判断原始声明。改用 System.Reflection.Metadata 直接读取真实 GenericParam 表后，R3 的 `0x0A00051B` 被拒绝：非泛型 WordSelectionEvent 上错误引用了 !0/!1/!2。

R4 将这个新增引用的 TypeSpec `0x1B0000B5` 改为原有基类对应的封闭 UnityEvent<string,int,int>。218 个新增引用全部重新检查，解析与原始泛型参数数量检查 0 失败；R3 的失败输入同时构成已检出的反例。此处修的是本轮新建检查器；冻结历史检查器未改。记录 32 因新检查器当时构建失败，实际上执行了旧二进制，**不采信**其零错误结论；记录 35 才是真正原始表行检查，检出 1 个错误。完整输出与首次漏报代码均保留。

R4 与 R3 的所有 2,297 个有 RVA 方法体逐字节相同；R4 已重新执行构造两次、独立隔离、源绑定、静态类型、成员访问、正负控制及 184 个实际 native/CIL 用例。状态以本目录 CURRENT-RESULT 及各报告中的 R4 SHA 为准，R3 不是正式可晋级候选。

新增与变更的本地证据在 R4-AND-REFERENCE-AUDIT-SUPPLEMENT.zip。先解压 COMPLETE-LOCAL-CHECKPOINT.zip，再覆盖解压该补充包，可恢复最终工具和全部修订；两个清单逐文件记录 SHA 并已实际回读。核心原 ZIP 继续保留 R1–R3 的当时状态。CI 准备脚本是当时生成器，真正执行配置以仓库 workflow 与 check_bulk389_compiler.py 固定的 R4 哈希为准。

32 个边界用例执行于首次编译的 WidthAwareTables；R4 的该源文件逐字节未改。R4 实际候选的每个旧列值与堆内容另外独立回读，没有把前一个补丁器的测试伪称为新二进制执行。


## 本轮完整 Linux 编译结果已回收

[Actions 39957635669](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39957635669) 在源码提交 1cc0761d6378c8d00e4d9c67e9c322eba6e52614 上成功结束。R4 SHA 一致，IL2CPP exit=0；此次生成 756 个 C++ 文件，游戏部分为 14 个 TU，14/14 均 Clang18 语法检查 exit=0、error=0。映射方法仍是 2,267，与 R6 相同。旧 38 个报错方法在这个 Linux 门下归零；源码分片从 16 降为 14，不是少检查两个现有文件。

独立回读了全部 14 个原始日志，并在生成源码压缩包中逐一核验源文件 SHA 和 global-metadata SHA。只编译了全部游戏 TU，没有宣称其余 742 个系统 C++ 也通过，更没有宣称 Apple 链接、设备执行或全方法行为通过。原始报告、日志、逐方法映射和独立回读保存于 compiler-39957635669/。生成全 C++ 压缩包 152,296,115 字节，超过 Git 单文件限制，单独保存到本轮 Release，不塞入 Git blob。

下一流程为 bulk389-real-ios-ipa.yml：把这个固定候选绑定到同次运行的包，经精确 Unity 2022.3.44f1c1 重新导出，再由 macOS-15 / Xcode16.4 构建 ARM64 未签名应用。保留失败日志、归还 Personal 席位，账号日志仅上传脱敏副本。历史 helper 来自固定 64b86ed 提交并独立放在 ios-support/；不替换仓库原 Assets/Editor 工具。历史 workflow 引用的两个 IPA 脚本不存在，已改用新标准库打包器，检查主程序及 UnityFramework 的物理 iOS ARM64、无代码签名／provision、ZIP CRC，以及 IPA metadata 与本次 Xcode 导出逐字节一致。该流程是否真正完成，以后续运行实际产物为准。


实际 iOS 流程已经启动：[39961120509](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39961120509)，固定代码提交 `6c33aa4ea57fec1162897026c3ad0e26f00209a6`。`REAL-IOS-RUN-OBSERVATION.json` 是带时间戳的观测，不能当成未来的最终状态。生成 C++、核心历史归档、R4 补充、候选及编译日志已保存至 [本轮预发布归档](https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/tag/native121-bulk389-r4-compiler-clean-2026-10-23)，6 个资产共 209,086,594 字节，服务器 SHA-256 digest 均与本地匹配（未重复下载 152 MB 资产）。Release 内清单固定于提交 6c33aa4 的归档时刻；仓库后续清单还包含运行回执。最后的审查／编译归属／workflow 准备工具与回执在 POST-COMPILER-TOOLS-AND-RECEIPTS.zip 内，逐文件再次解压校验。

范围补充：归档原 plan 的 explicit_companion_targets 缓存标量仍为 6，实际 companion_tokens 列表已为 7；构造／回读的 389 项范围均包含新入口。SCOPE-COUNT-CORRECTION.json 给出当前 381 + 7 + 1 = 389 的准确分解，保留原输入以免改写执行历史。
