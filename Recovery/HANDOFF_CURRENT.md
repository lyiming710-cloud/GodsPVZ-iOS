# GodsPVZ 1.0.2 高保真恢复 / iOS 移植 — CURRENT HANDOFF

快照日期：2026-09-18
仓库：`lyiming710-cloud/GodsPVZ-iOS`
当前工作分支：`stage9-admin-start-static`
替换旧交接文件前 HEAD：`43ac13c6ea6f848df6d99440be6a3a6f9557006e`
旧交接删除提交：`af3fc5ce9b5d118f00bbecb60b787b78cda53546`

## 1. 当前项目结论

正式 sealed high-fidelity baseline 仍然是 **HF55**：

`dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`

HF55 不因 Stage9.1 的开发候选、Unity 运行验证或 package closure 自动晋级 HF56。除非新的残余 MethodDef 同时满足既定 native-backed HF gate，否则不要开启 HF56。

Stage9.1 当前已经不再停留在 `Administrator.Start()` 恢复阶段。Administrator.Start、其后续 runtime 异常以及 package-script reference closure 已经闭环到可进入正式 Unity import/compile 的状态。

当前唯一应继续使用的 Stage9.1 最终开发 DLL candidate SHA256：

`047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`

早期输入/候选 `26064265fdba4e9b501f0c3312260503fdde43b9e111072c19dd39891e5ca022` 仅作为历史链路证据保留，**不得再当作当前最终 Stage9.1 DLL**。

## 2. Administrator.Start / runtime 闭环

目标历史 MethodDef：

- `Administrator.Start()`
- MethodDef `0x06000004`
- RID 4

此前针对 `List<GameObject>.GetEnumerator()` / Cecil generic MemberRef、`get_Item !0`、foreach enumerator metadata 和 `BoardManager` accessor 路径的调查属于已完成的诊断链，不再是当前执行 blocker。

当前 authoritative exact-R3 natural runtime：

- run：`35236388436`
- final DLL SHA256：`047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`
- `INVALID_IL_TOTAL=0`
- `Administrator.Start` anomaly count = 0
- `Administrator.Update` anomaly count = 0
- `BGMVolume` anomaly count = 0
- SkillProgress 五阶段全部到达 / PASS

因此不要再把旧 runtime 中的 `Administrator.Update = 269 Invalid IL` 或早期 `List<GameObject>.GetEnumerator()` MissingMethodException 当成当前状态。那些属于已淘汰 candidate 的历史失败。

## 3. Stage9.1 package-script 67/67 闭包

67 个 package-script reference 已全部得到 exact package/GUID evidence。

authoritative run：

- workflow run：`35238895838`
- job：`package-guid-evidence`
- result：`resolved=67/67`
- artifact：`HF46-package-reference-evidence`
- artifact ID：`10505616227`
- artifact digest：`sha256:77c956492fe4690f9e3369979738ac745e7a283d2d7cd9b234abc7ac5baf8820`

已固定的关键 package provenance 包括：

- `com.unity.2d.animation@9.1.1`
- `com.unity.render-pipelines.core@14.0.11`
- `com.unity.render-pipelines.universal@14.0.11`
- `com.unity.textmeshpro@3.0.6`
- `com.unity.ugui@1.0.0`

不要重新用模糊类名匹配替代现有 67/67 exact GUID 证据。

## 4. Package dependency closure

authoritative dependency-closure run：

- workflow run：`35239319822`
- job：`closure`
- result：`resolved_count=10`
- result：`override_count=4`
- result：`unresolved_count=0`
- artifact：`Stage9.1-package-dependency-closure`
- artifact ID：`10505060075`
- artifact digest：`sha256:e7a949235772cab30c9465071f91daf76d9313ebe05812aae7c632b470ebdeee`
- artifact size：`416089243` bytes

该 artifact 是进入 Unity import/compile 时的 dependency closure 输入之一。不要再次重新解析成一套未经锁定的 package 版本。

## 5. Pinned Unity package bundle

authoritative pinned bundle run：

- workflow run：`35239547918`
- job：`bundle`
- artifact：`Stage9.1-pinned-unity-packages`
- artifact ID：`10505237349`
- artifact digest：`sha256:e94aaa2c0a85832ef9e0263dbd925d9d3f8a71803e199867f876ae223ec440c0`
- artifact size：`157887283` bytes

工作分支上的提交 `43ac13c6ea6f848df6d99440be6a3a6f9557006e` 只用于 dispatch 已存在的 Stage9.1 pinned package bundle workflow；不要把该 commit 本身误当作新的 formal managed baseline。

## 6. Unity 固定环境

后续 Unity gate 固定使用：

- Unity：`2022.3.44f1c1`
- IL2CPP metadata layout：`31.1`
- Assembly-CSharp MethodDef total invariant：`2317`
- 正式 HF managed baseline：HF55
- Stage9.1 runtime-qualified development DLL：`047054e0...d43ee`
- package references：67/67 exact GUID evidence
- dependency closure：run `35239319822`
- pinned package bundle：run `35239547918`

原 PC x86-64 IL2CPP 仍是 gameplay 语义第一权威来源，Android native/metadata 为第二独立来源；Cpp2IL / ILSpy 只能作为 attribution / managed reconstruction 辅助，不得反过来覆盖 original-native evidence。

## 7. 下一位接手者的唯一正确下一阶段

**下一 gate：正式 Unity import / compile。**

执行顺序：

1. 读取仓库当前 HEAD，确认本交接之后没有新的并发提交覆盖输入。
2. 锁定 Stage9.1 final candidate SHA256：
   `047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee`。
3. 使用 run `35238895838` 的 67/67 package GUID evidence。
4. 使用 run `35239319822` 的 dependency closure artifact。
5. 使用 run `35239547918` 的 pinned Unity package bundle。
6. 在 exact Unity `2022.3.44f1c1` 环境执行正式 project import。
7. 执行 compile，并保存完整 Editor log / compiler diagnostics / package resolution evidence。
8. 如果 import/compile 失败，只处理**可复现、确定性的 Unity 错误**；不要回到旧 Administrator.Start 猜测链，除非新的证据直接指向该 MethodDef。
9. compile clean 后进入 MainMenu 启动验证。
10. MainMenu 通过后进入 Board/gameplay runtime 验证。
11. 只有新的 runtime evidence 证明存在 residual managed reconstruction damage 时，才重新打开具体 MethodDef 的 native-backed recovery。
12. 所有 Stage9.1 Unity gate 通过后，再更新 `Recovery/STATUS.md` 和本交接文件；HF56 是否开启仍由正式 HF gate 单独决定。

## 8. 当前不要再做的事情

- 不要重新修 `Administrator.Start()`，除非新的 Unity/runtime evidence 明确回归到该方法。
- 不要重新以旧 `26064265...` DLL 作为 Stage9.1 当前 candidate。
- 不要重复 generic `List<GameObject>` MemberRef / Enumerator 猜测性试验。
- 不要把 workflow trigger commit 当成 formal HF stage。
- 不要把 Stage9.1 development candidate 自动写成 HF56。
- 不要修改 MethodDef / FieldDef visibility 或添加无 native evidence 的 wrapper/fallback。
- 不要添加 catch/try-catch 来吞掉恢复错误。
- 不要为了“能编译”而改变原版 gameplay 行为。
- 不要重新选择浮动 package 版本替代已固定的 package bundle 和 dependency closure。
- 不要在 formal compile gate 前继续做与当前 blocker 无关的大范围代码改写。

## 9. 固定原始文件身份

继续保留并核验既有固定基线：

- PC ZIP SHA256：
  `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`
- PC GameAssembly SHA256：
  `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- PC metadata SHA256：
  `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`
- Android APK SHA256：
  `428e0ba2e46645a905fb9fbb00cfde42406727a3ddfce9a7df1e0875889a739f`
- Android arm64 `libil2cpp.so` SHA256：
  `cfa13d53d7c3e218221a90c5fb615a012339393774af3160ff1c17f3826c61a6`
- Android metadata SHA256：
  `e7a4412e3af25da3ba2c806d3c691ec68d00c1e7915d8fb6843c3a82422f5005`

## 10. 一句话状态

**HF55 仍是 formal sealed baseline；Stage9.1 已得到 runtime-qualified final candidate `047054e0...d43ee`，exact-R3 Invalid IL 已归零，package-script 已 67/67，dependency closure unresolved=0，pinned package bundle 已生成；当前唯一主线是 exact Unity 2022.3.44f1c1 的正式 import/compile。**
