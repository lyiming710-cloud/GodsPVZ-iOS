> **当前暂停（用户指示）**：云环境返回 `409 Conflict environment_offline`。已停止测试与构建，本轮没有创建或启动新工作流。已保存工具源码、观察摘要与交接；本机完整日志和未提交补丁器仍待环境恢复后归档。六站点补丁输出与历史 Batch1 的原始哈希比较失败，完整规范化/raw 差异尚未确认；不得晋级游戏候选。

> **2026-09-30 Native24 接手检查点**：独立工作从固定 `08d5eb4eb039fd5db95b8caea740470697a6ad96` 开始，保存于 `repair/codex-native24-verification`。来源门已修复并通过 11 个控制，1623 个历史站点重算为 254 个有限 E2 支持、1369 个隔离，225 个已知 LOCAL receiver 站点无一获准。三组 792 次 Clang19 全量复编译完成，Batch2a 仍有 5200 条错误、23 个失败 TU；没有本轮 Unity/Xcode 或 IPA。补丁器的独立 DLL 再读与 raw/规范化隔离因云环境 `environment_offline` 尚未完成。详见 [本轮证据与继续入口](Native24-2026-09-30/README.md)。该检查点不晋级游戏候选。


> **2026-09-30 项目分类归档**：原版输入、当前/历史代码、实验候选、验证证据与 Xcode 工程已整理。入口：[Recovery/Archive-2026-09-30](Archive-2026-09-30/README.md)。归档不是 IPA 发布；验收结论仍以 Native23 独立审查为准。

# GodsPVZ 1.0.2 高保真恢复 / iOS 移植 — CURRENT HANDOFF

> **2026-09-30 最新进度：Native23 全面独立审查已完成，先读 [审查与当前状态](Native23-Review-2026-09-30/README.md)。有真实编译改善，但 Batch2a 全量预检仍有 5200 条错误、23 个失败 TU，没有 IPA。Batch1 六个站点可保留为受限候选；Batch2/2a 未通过高还原验收。来源追踪、方法归属、字节隔离和补丁器约束需先纠正。此前 Native21 交接继续保留为项目历史与输入依据。**

> **2026-09-29 最新暂停交接：请先读 [Native21 总交接](HANDOFF-2026-09-29-Native21.md) 和 [接手任务说明](TAKEOVER-PROMPT-Native21.md)。Native20/21 已完成当前验证并归档，全量 C++ 仍失败，没有 IPA。下方 Native19 与 2026-09-18 内容保留作历史记录，不代表最新候选。**

> 2026-09-29 Native19 更新：本分支最新恢复状态见 [Native19 检查点](Native19-2026-09-29/README.md)。15 个目标的方法级检查已通过，但全游戏 C++ 预检仍失败，尚无 IPA。以下 2026-09-18 内容保留为历史输入与 sealed baseline 记录，不代表当前候选的编译或交付状态。

快照日期：2026-09-18
仓库：`lyiming710-cloud/GodsPVZ-iOS`
当前工作分支：`stage9-admin-start-static`

2026-09-18 已清理 Administrator.Start 专用旧交接、R2/旧 First Import 状态、2026-09-16 handoff manifest/provenance/SHA 快照及旧 trigger marker；原始 HF/Recovery 审计证据保留。当前唯一交接入口是本文件。

## 1. 当前项目结论

正式 sealed high-fidelity baseline 仍然是 **HF55**：

`dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`

HF55 不因 Stage9.1 的开发候选、Unity import、scene smoke 或后续 iOS export 自动晋级 HF56。除非新的 residual MethodDef 同时满足既定 native-backed HF gate，否则不要开启 HF56。

Stage9.1 已经通过当前 exact-R3 Unity import/compile、package-reference migration、二次 reopen 稳定性以及 `MainMenu` / `Board` scene-load smoke。当前唯一应继续使用的 Stage9.1 runtime-qualified development DLL SHA256：

`047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`

早期输入/候选 `26064265fdba4e9b501f0c3312260503fdde43b9e111072c19dd39891e5ca022` 仅作为历史链路证据保留，**不得再当作当前最终 Stage9.1 DLL**。

## 2. Administrator.Start / runtime 闭环

目标历史 MethodDef：

- `Administrator.Start()`
- MethodDef `0x06000004`
- RID 4

此前针对 `List<GameObject>.GetEnumerator()` / Cecil generic MemberRef、`get_Item !0`、foreach enumerator metadata 和 `BoardManager` accessor 路径的调查属于已完成的诊断链，不再是当前 blocker。

当前 authoritative exact-R3 natural runtime：

- run：`35237045119`
- final DLL SHA256：`047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`
- `INVALID_IL_TOTAL=0`
- `Administrator.Start` anomaly count = 0
- `Administrator.Update` anomaly count = 0
- `BGMVolume` anomaly count = 0
- SkillProgress 五阶段全部到达 / PASS

因此不要再把旧 runtime 中的 `Administrator.Update = 269 Invalid IL` 或早期 `List<GameObject>.GetEnumerator()` MissingMethodException 当成当前状态。

## 3. Stage9.1 package-script 67/67 闭包

67 个 package-script reference 已全部得到 exact package/GUID evidence。

authoritative run：

- workflow run：`35238895838`
- result：`resolved=67/67`
- artifact：`HF46-package-reference-evidence`
- artifact ID：`10505616227`
- artifact digest：`sha256:77c956492fe4690f9e3369979738ac745e7a283d2d7cd9b234abc7ac5baf8820`

关键 pinned package provenance：

- `com.unity.2d.animation@9.1.1`
- `com.unity.render-pipelines.core@14.0.11`
- `com.unity.render-pipelines.universal@14.0.11`
- `com.unity.textmeshpro@3.0.6`
- `com.unity.ugui@1.0.0`

不要重新用模糊类名匹配替代现有 67/67 exact GUID 证据。

## 4. Package dependency closure

authoritative dependency-closure run：

- workflow run：`35239319822`
- `resolved_count=10`
- `override_count=4`
- `unresolved_count=0`
- artifact：`Stage9.1-package-dependency-closure`
- artifact ID：`10505060075`
- artifact digest：`sha256:e7a949235772cab30c9465071f91daf76d9313ebe05812aae7c632b470ebdeee`
- artifact size：`416089243` bytes

不要重新解析成未经锁定的浮动 package 版本。

## 5. Pinned Unity package bundle

authoritative pinned bundle run：

- workflow run：`35239547918`
- artifact：`Stage9.1-pinned-unity-packages`
- artifact ID：`10505237349`
- artifact digest：`sha256:e94aaa2c0a85832ef9e0263dbd925d9d3f8a71803e199867f876ae223ec440c0`
- artifact size：`157887283` bytes

## 6. Unity 固定环境

后续所有 Unity/iOS gate 固定使用：

- Unity：`2022.3.44f1c1`
- exact Editor concat SHA256：`0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14`
- IL2CPP metadata layout：`31.1`
- Assembly-CSharp MethodDef total invariant：`2317`
- 正式 HF managed baseline：HF55
- Stage9.1 runtime-qualified development DLL：`047054e0...d43ee`
- package references：67/67 exact GUID evidence
- dependency closure：run `35239319822`
- pinned package bundle：run `35239547918`

原 PC x86-64 IL2CPP 仍是 gameplay 语义第一权威来源，Android native/metadata 为第二独立来源；Cpp2IL / ILSpy 只能作为 attribution / managed reconstruction 辅助。

## 7. 正式 Unity import / compile — PASS

authoritative formal import run：`35298445986`

workflow：`.github/workflows/stage9-current-baseline-formal-import.yml`

artifact：

- `Stage9.1-CURRENT-BASELINE-FORMAL-UNITY-IMPORT`
- artifact ID：`10529117757`
- artifact digest：`sha256:3fb767760d99489c23ebd47083e58771f66b08b3e75bef9387f7d1adc16703a9`

最终 gate：`CURRENT_BASELINE_FORMAL_IMPORT_PASS`

关键结果：

- Unity exit code：`0`
- compile errors：`0`
- package errors：`0`
- assembly collisions：`0`
- serialization errors：`0`
- missing-script errors：`0`
- fatal errors：`0`
- license errors：`0`
- final DLL SHA256：严格等于 `047054e0...d43ee`
- `packages-lock.json`：存在
- package migration marker：PASS，`resolved=67/67`
- remaining old recovered-package refs：`0`

早期 run `35297584050` 只因 workflow 未预建 `Assets/Editor/` 而在 Unity 前失败；run `35297727652` 的 Unity import 已成功，但首启动 `delayCall` 未在 `-quit` 前写 marker。两者都不是项目/程序集/package blocker。后续改为显式 `GodsPVZPackageReferenceMigrator.RunBatch` 后正式 PASS。

## 8. Stability / MainMenu / Board scene smoke — PASS

authoritative run：`35299209733`

workflow：`.github/workflows/stage9-current-baseline-scene-smoke.yml`

artifact：

- `Stage9.1-CURRENT-BASELINE-SCENE-SMOKE`
- artifact ID：`10529316787`
- artifact digest：`sha256:43313c564abc3a149c08562e96c3d4cf6e78185b38d2be80fcbe4e1c9ed65ffe`

稳定性结果：

- first import：compile/missing-script/serialization/exception 均 `0`
- first import 初始化阶段 `shader_not_found=52`
- immediate second reopen：`shader_not_found=0`
- second reopen compile/missing-script/serialization/exception 均 `0`
- `second_blocking_zero=true`

场景加载结果：

- `Assets/Scenes/MainMenu.unity`：loaded=true，root=3，GameObjects=3，component slots=9，missing scripts=0，missing shaders=0
- `Assets/Scenes/Board.unity`：loaded=true，root=3，GameObjects=3，component slots=9，missing scripts=0，missing shaders=0
- scene smoke overall：PASS
- generated package lock dependency count：`47`
- 15 个预期 embedded package：全部版本/source/depth 校验通过，problems=[]

注意：当前 scene smoke 是 Editor scene-load/serialization 层的确定性 gate，不等价于完整玩家交互 gameplay PlayMode/device validation。

## 9. 当前唯一正确下一阶段

**下一主线：iOS Build Support preflight → unsigned iOS Xcode export。**

已创建 preflight workflow：

`.github/workflows/stage9-ios-module-preflight.yml`

当前 preflight run：`35315541247`。

该 run 只检查 preserved exact Unity China `2022.3.44f1c1` Editor 是否已经包含 `Editor/Data/PlaybackEngines/iOSSupport`；不改 gameplay DLL、不改 HF baseline。

后续执行顺序：

1. 等待 `35315541247` 的 iOS module probe 结论。
2. 若 exact Editor 已含 iOS Build Support：基于当前 exact-R3 + `047054e0...d43ee` + locked packages 重建项目，执行 `GodsPVZIOSBuild.BuildIOS`，产出 unsigned Xcode project。
3. 若缺 iOS Build Support：只补齐与 **同一 Unity 2022.3.44f1c1** 匹配的 iOS module，并锁定来源/SHA；不得用其他 Unity 版本替代。
4. Xcode project export 后审计 IL2CPP 产物、scene list、bundle id、架构、target OS、native plugin/asset copy 和 generated Xcode project 完整性。
5. 再进入 macOS/Xcode unsigned device build / IPA packaging 路径；没有 macOS/Xcode 时，不把仅导出的 Xcode project 宣称为可安装 IPA。
6. 只有新的 runtime evidence 明确证明 residual managed reconstruction damage 时，才重新打开具体 MethodDef 的 native-backed recovery。

## 10. 当前不要再做的事情

- 不要重新修 `Administrator.Start()`，除非新的 Unity/runtime evidence 明确回归到该方法。
- 不要重新以旧 `26064265...` DLL 作为当前 candidate。
- 不要重复 generic `List<GameObject>` MemberRef / Enumerator 猜测性试验。
- 不要把 Stage9.1 development candidate 自动写成 HF56。
- 不要添加无 original-native evidence 的 gameplay fallback/wrapper。
- 不要添加 catch/try-catch 来吞掉恢复错误。
- 不要为了“能编译”而改变原版 gameplay 行为。
- 不要重新选择浮动 package 版本替代已固定的 package bundle / dependency closure。
- 不要再把 Unity import/compile 或 MainMenu/Board scene-load 当成未完成 blocker。

## 11. 固定原始文件身份

- PC ZIP SHA256：`2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC GameAssembly SHA256：`9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC metadata SHA256：`ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Android APK SHA256：`428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`
- Android arm64 `libil2cpp.so` SHA256：`cfa13d53d7c3e218221a90c5fb615a012339393774af3160ff1c17f3826c61a6`
- Android metadata SHA256：`e7a4412e3af25da3ba2c806d3c691ec68d00c1e7915d8fb6843c3a82422f5005`

## 12. 一句话状态

**HF55 仍是 formal sealed baseline；Stage9.1 final development DLL `047054e0...d43ee` 已通过 exact-R3 natural runtime、67/67 package closure、正式 Unity import/compile、显式 package migration、二次 reopen、MainMenu/Board scene-load smoke；当前主线已推进到 exact Unity 2022.3.44f1c1 的 iOS Build Support preflight 与 unsigned Xcode export。**
