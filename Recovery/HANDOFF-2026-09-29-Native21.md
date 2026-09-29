# GodsPVZ-iOS 完整接手说明：Native21，暂停交接

日期：2026-09-29。仓库：`lyiming710-cloud/GodsPVZ-iOS`。交接分支：`repair/codex-native19-ipa`。

**先看结论：没有导出 IPA。Native21 是目前最新的实验候选，整个游戏的 C++ 编译仍失败。用户要求先完成正在进行的验证、暂停后续修复，由另一个 AI 接手，完成后再交回 Codex 独立审查。** 本文所在 Git 提交是本次交接快照；Native19 的前一检查点是 `235cabf`。不要根据分支名字误以为最新候选仍是 Native19。

## 1. 项目目的与成功标准

把 GodsPVZ 1.0.2 原 PC/Android 游戏恢复为可在 Unity China `2022.3.44f1c1` 导入、IL2CPP 转换、Apple Clang 编译并打包的 iOS 项目，最终交付真实的**未签名 arm64 IPA**。恢复应尽量保留原版 gameplay、数据、资源、对象关系、调用顺序和异常行为。消除编译错误只是必要条件。

交付链必须逐层证明：输入与来源 → native 身份和语义 → MethodBody/类型流 → 独立重建及非目标隔离 → 真实 IL2CPP → 全量生成 C++ 编译 → 完整 Unity 导出及 Apple 编译链接 → 真实 `.app`/IPA → 下载校验 → 后续设备运行。任一前置层通过，都不能冒充后面的层通过。

用户允许直接推进有证据支持的修复，已明确批准早期第四个目标 `ZombieSelect::.ctor(ZombieInfo)`，后来又授权继续至 IPA。用户很在意提前在 Codespaces 发现问题，不接受每个基础 IL 错误都耗费约二十分钟的完整 Actions。当前最新指令是**暂停并交接**；Codex 不再主动扩展下一批或启动完整工作流。

## 2. 当前状态，一眼可核对

| 版本 | 本批目标 | 类型反向测试，每个候选 | IL2CPP | 本批 C++ 关联错误 | 全游戏 C++ 诊断行 | 关联方法数 |
|---|---:|---:|---|---:|---:|---:|
| Native19 | 15 | 8，另有 3 个 Die 语义负例 | exit 0 | 0 | 6273 | 781 |
| Native20 | 73，112 处编码修正 | 112 | exit 0 | 0 | 6175 | 714 |
| Native21 | 38，90 个整数 locals + 9 处编码修正 | 99 | exit 0 | 0 | 6032 | 676 |

三轮都检查了 21 个 `GodsPVZRuntime1*.cpp` 翻译单元，使用真实 Clang 18 和 CI 导出的 Unity headers。表内“诊断行”不是独立缺陷数；同一根因会产生多条错误。`method-errors.json` 只能归属其中一部分报错，不能与 `results.json` 的全量错误行数混用。Native19→Native21 共少了 241 条诊断、105 个报错关联方法，但仍远未闭合全量编译。

Native19 有 36 项 CLR 替身断言，覆盖六个共享 emitter；Die 的实际候选 CIL 对独立 native-derived oracle 跑了 7424 组状态/每候选。它们不是 Unity 或真机测试。Native20/21 只对窄范围类型编码/整数 local 修正建立了类型约束、反向测试、隔离、真实转换与 C++ 证据，**尚未逐方法证明原版行为等价，也没有全游戏行为测试**。不能把 73/38 个方法全部标记为“高还原验收完成”。

Native20 的检查报告有 47 条不可达指令，Native21 有 11 条；验证器只检查可达类型流，不声称验证了这些不可达指令的语义。补丁没有删除它们。新增修改不重建 CFG，保留原指令对象，仅把短跳转扩成长跳转以容纳字节长度变化。

## 3. 权威输入和候选哈希

原 PC x86-64 native 是 gameplay 第一权威；Android native/metadata 是第二独立来源。原始 metadata、资源与 JSON 支持身份和数据核对。Cpp2IL、ILSpy 和 AI 生成 C# 只作辅助，不能取代 native 证据。

| 输入/候选 | SHA256 |
|---|---|
| 根 managed 恢复输入 `Assembly-CSharp-59bb-native2.dll` | `18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd` |
| PC `GodsPVZ_1.0.2.zip` | `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48` |
| PC `GameAssembly.dll` | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| PC `global-metadata.dat` | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |
| China Editor Linux tar.xz | `0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14` |
| Native17 unlinked | `74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2` |
| Native17 linked | `9bf2d7a033632061e8e71d32c70312c3c234298649495afc384d0dfe7120a125` |
| Native19 unlinked | `c022fb10f30c4363ba27cc48a4508029f57c76e079729e4aa753cd7e188dd3de` |
| Native19 linked | `2df30387140871d1211467ccbfc790789d588d68fa131974c37c23e11f0dc873` |
| Native20 unlinked | `7d2b3661f3e4aa3ba27b986c233d65c920e4c439851eae1788ae58323cc1d910` |
| Native20 linked | `f561d48a353f1747b77260f9db69acabc1bb4860f46d4ffc447d81764c55efa9` |
| **Native21 unlinked** | **`5c34c8b20787d1ea56932ab636dede404eb352f6597de7959b93f44cd4546842`** |
| **Native21 linked** | **`0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f`** |

Native19 从 Native17 的锁定二进制重建，**不继承有语义错误的 Native18 DLL**。Git 源码历史的父提交是 Antigravity 64b86ed，并不等于二进制生成输入也是 Native18。这两个谱系必须分清。

程序集身份约束：320 TypeDef、2317 MethodDef、2802 FieldDef，2297 个有方法体的方法，MVID 保持。Native19/20/21 每批分别检查 2282/2224/2259 个非目标方法体在内存和重开盘后的 normalized 指纹不变。这些数是相对各批输入的隔离范围，不是三者相加。

历史正式 sealed HF baseline 仍是 HF55（`dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`）。旧 Stage9 runtime-qualified `047054e0...d43ee` 和当前 native 恢复链也不能混同。不要因为候选通过 import/IL2CPP 就宣称 HF56 或生产晋级。历史 HF 链、包闭包与正式门见 [STATUS.md](STATUS.md) 和 [旧 CURRENT 文档](HANDOFF_CURRENT.md) 的 2026-09-18 部分。

## 4. 历程及之前为什么失败

### 4.1 Native3/4：先恢复关键对象链和完整方法体

从 18e44 出发，Native3 四方法为 `GetZombieUnderMouse` 060001E0、`CreateEnemySelecter` 060001B2、`PassLevel` 06000248 和用户批准的 `ZombieSelect::.ctor` 060001A5。Native4 增加 `Plant.SetAnimationState_Planting` 0600039F 与 `SunManager.SetMaxSunFallTime` 06000267。Native4 六方法完成 599 条 CIL、74 项 CLR 断言等阶段证据，随后真实 Unity 又暴露 TextLink/Color 与 Ladder/Vector3 错误。

Gemini 的 ff0e46/native3-blocker-repair 是隔离实验，不能继承。尤其 PassLevel 是完整 MethodBody 重建，不能仅替换 V31/V32 的 Enumerator 类型。18e44 的 LocalVarSigTok 是 0x110001D0，35 locals；V19/V31/V32 是 Enumerator<object>，V31 原未使用，V32 原有取地址而无正常初始化。原 PC 参数 boardConfig 的列表读取偏移是 +0x68，不是 +0x28。GetZombieUnderMouse 的 false 分支目标是 0x18031F054，不能按错误地址 0x18031EFCB 或虚构的 this+0x30 路径恢复。

完整早期过程、云端运行号、原版文件和未完取证边界见 [2026-09-27 原交接全文](Historical-Handoff-2026-09-27.md)。本地 `audit/stage9-native3`、`stage9-native4` 保留细颗粒证据。不要用今天的摘要覆盖历史记录。

### 4.2 Native6–14：恢复链和 Codespaces 直接转换

中间恢复链的逐步源码/锁定输入保存在 `scripts/takeover/PatcherNative*`、`scripts/codespaces`、`Recovery/CandidateSource` 和 Git 历史。并非本次对 Native6–13 所有语义重新独立审计，不应扩大背书。

Native14 完成目标错误关闭，真实 IL2CPP 仍 exit 255：剩余 `Map.RandomGet_Grid_TestPlace<T>` 和 `VFXAnimationEvent.Binding<T>`。直接重放工具解决了硬编码旧工作目录、临时规范化改写源码、可执行位等基础设施问题。详见 [Native14](Codespace-Native14-2026-09-28/README.md)。稀疏检出中目录可能不落盘，但 `git show HEAD:路径` 可以读取。

### 4.3 Native15–17：转换错误逐批关闭

- [Native15](Native15-2026-09-28/README.md)：Map 06000290、VFX Binding 060000C4，24 项 CLR 断言；从 reference-sharing 与 fully-shared native 证明完整行为。转换继续暴露下一组错误。
- [Native16](Native16-2026-09-28/README.md)：VFX SetSorting 060000C6、SwfAssocList.Remove 060008D9、必要依赖 SwfList.UnorderedRemoveAt 060008E9；37 项 CLR 断言；转换剩 AssignTo。
- [Native17](Native17-2026-09-28/README.md)：SwfList.AssignTo(List<T>) 060008EF 单方法恢复，19 项 CLR 断言；直接 IL2CPP 首次 exit 0、无 method errors。之后完整链路才进入 Apple C++ 编译，暴露更多无效类型代码。

“下一 blocker 新出现”未必是新回归；转换器先前可能在更早错误停止。必须比较目标在输入/输出中是否改变，保留失败输入作为正控制。

### 4.4 Antigravity Native18：编译问题有进展，但保真验收被否决

审查对象：`repair/antigravity-native18-zombie-fix`，提交 `64b86ed725c40e8e00cecd7a917f72bbed37777e`，父 fc12b52，再前为 ff08844。11 个 Zombie 方法消除了一个分片中的编译错误，但独立审查发现：previousPosition 含义错误、fZ 高度丢失、UI 增量跟随变成绝对赋值、Die 跳转表未恢复、Ashe 缺 y 偏移、过弱的栈高度门、打包脚本缺失。

详见 [Native18 审查全文](Native19-2026-09-29/NATIVE18-REVIEW.md)。审查原始附件位于 Windows `audit/native18-review/review-evidence`；报告原有 `review-evidence/` 相对链接应在该完整目录阅读，不能以归档副本中链接不存在为证据丢失。

Native18 “2286 非目标 100% 字节不变”的说法不成立。独立 raw 比较发现 1088 个非目标 MethodBody 字节变化，其中 561 个 raw CIL 字节流变化；normalized 结构变化为 0。Cecil 重写 token/局部签名导致的编码变化与逻辑变化必须区分。**现在只声明 normalized 隔离，不声明 raw 字节不变。**

### 4.5 Native19：纠正行为并建立实质预检

15 个精确目标和逐方法改动见 [Native19 README](Native19-2026-09-29/README.md)。关键恢复：

- Start_PreviousPosition 保存 `(fX,fY,animationY-fY)`；Update_Move 的 animation Y 加 fZ；保留必要空引用故障。
- Update_PreviousPosition 将 dx、dy、height delta 加到既有 UI 偏移，而非抹掉偏移；最后保存新高度差。
- ArmBroken 补完整 List<string> 枚举和 finally Dispose，不能只修两个 Vector3 参数。
- Ashe 的伤害飘字 y=`fZ+fY+134`，保留 HP callback 和 hot-nut/death 路径。
- CreateStartPrePath 恢复嵌套遍历、BoardConfig 参数、两处 Clear、首个严格最小距离路径以及失败置空。
- Die 恢复两张 switch 表、无符号范围比较、float armor 判定；ID17 粒子 `(fX,fZ+fY,0)`；原生写入 word 0x101 对应两个相邻 bool：isDied 和 ashes，不能只把257写给一个 bool。
- Project.Start 恢复 Vector3.one*size；Project.LoopAddAnimation 恢复先序递归和 IEnumerator/IDisposable 清理。

易混 token：CreatParticles=06000434，DestroyZombie=06000436，Die=06000437，06000438 是 DropLootPiece。ICEUIController.Update native=0x18034D610，Broken=0x18034D1B0。

### 4.6 Native20/21：类型编码候选，不冒充整方法 native 复原

Native20 使用原只读 manifest 中的 73 个方法、112 个精确站点：84 处引用判空零→ldnull，14 处浮点比较零→ldc.r4，14 处匹配的结构体地址→值。每个方法整体可达类型检查必须通过，含已识别 native 缺失提示或仍有类型错误的方法被排除。112 个逐项撤销负例证明每处修正确实被检查器覆盖。

Native21 在 Native20 上，局部变量仅允许由 `System.Object` 改为 `System.Int32/Int64`，由 stloc 的实际整数栈类型触发，并要求整方法所有可达写入/使用通过。最终这批 90 个全部是 Int32；另有 9 处同类编码修正。90 个 local 和9个指令分别撤销均被拒绝。没有凭 C++ 报错把全程序集 object 全改 int，也没有删除未修复提示。

这两批不改变分支逻辑、字段或方法签名；指令对象身份保留，必要时扩宽短跳转。候选二次写出完全一致，重开盘比对通过。限制：没有为其中每个方法单独建立完整 PC CFG oracle；未证明原输入中别的、类型合法的逻辑损伤不存在。接手者要保留这条验收边界。

## 5. 最新 Actions 与打包阻塞

交接时读取 GitHub API，最新完整门仍为 Native18：

- [36505847408](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36505847408)：cancelled。
- [36505731509](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36505731509)：qualification/export 成功，build_ipa 失败。Unity 导出 Xcode 成功不是 IPA 成功。
- [36498491610](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36498491610)：旧 Native18 Apple 编译失败。

没有启动 Native19/20/21 完整 Actions。最新 API 快照在 `Native21-2026-09-29/latest-github-runs.json`。

旧 `.github/workflows/stage9-native18-real-ios-xcode-gate.yml` 仍不能直接当可交付流程：引用不存在的 `scripts/package_unsigned_ipa.mjs`、`scripts/validate_ipa.mjs`；预建自定义 CONFIGURATION_BUILD_DIR 再 clean build 触发 Xcode “not created by the build system” 清理错误。接手者需在本地 C++ 门闭合后修复这些编排问题并锁定本轮候选 SHA。

`scripts/codespaces/package_native19_ipa.py` 已准备真实产品检查，但**未在 macOS 跑过，也未接入新 workflow**。需要真实 DerivedData `.app`、bundle ID/minimum iOS、arm64 Mach-O/UnityFramework、metadata 对照、Payload ZIP CRC 和 SHA。输出名仍写 native19，接入新候选时应改为真实版本，不能只改名称就宣称打包完成。

历史 8192-byte `.ipa` 是占位文件，不是合法 IPA。任何分享链接必须在真实 workflow 完成且 artifact/asset 存在后提供。未签名 IPA 的后续安装签名与真机验证属于独立环节。

## 6. 文件、分支与环境导航

### Windows

- 项目根：`C:\Users\86136\Desktop\1.0.2PC`。
- 当前源码 worktree：`audit/native19-work`，分支 `repair/codex-native19-ipa`。
- 原版 PC：`audit/stage9-native4/inputs/GameAssembly.dll` 与 `global-metadata.dat`。
- 精确方法/字段映射：`audit/stage9-native4/evidence/native-method-map.json`、`native-fields.json`。
- Native18 原审查与附件：`audit/native18-review`。
- 最新原始回读：`audit/native19-evidence`、`native20-evidence`、`native21-evidence`。
- 历史本地 Native15 worktree：`audit/native15-work`，保留两个旧未跟踪 Native17 workflow/packager；不应清理或继承为已验证流程。
- 传输辅助：`audit/native19_sync.py` 仅把选定文件复制到自己的远端 worktree；`collect_native20.py`/`collect_native21.py` 只收集 allowlist 证据。均通过 SSH stdin/stdout，不打印 token。

可用 Python：`C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe`。Git：同级 `dependencies/native/git/cmd/git.exe`。gh：`C:/Program Files/GitHub CLI/gh.exe`。

### Codespaces

- 名称：`glowing-train-p7j9gp74q6jwc76v6`。
- 当前 worktree：`/workspaces/GodsPVZ-native19`。名称不随 Native20/21 阶段变化。
- 保留旧 Antigravity worktree：`/workspaces/GodsPVZ-native14-check`，64b86ed；保留 `/workspaces/GodsPVZ-iOS` 的旧恢复状态。禁止在它们中覆盖 DLL、生成目录、tracked 源码或提交。
- 当前缓存：`.validation/native19/replay`、`native20/replay`、`native21/replay`，各有自己的 ManagedStripped、DLL、rsp、C++、metadata。
- Native20/21 只将只读 `il2cpp` 工具目录符号链接到前一阶段，**没有给可写输入/输出做硬链接**。删除前一阶段缓存会破坏链，先检查 symlink。
- exact CI headers：`.validation/xcode-native18/expanded/xcode/Il2CppOutputProject/IL2CPP/libil2cpp`。
- 21 个最新游戏 C++：`.validation/native21/replay/canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp`。
- 全量日志/错误表：`.validation/native21/cpp-final38`。
- .NET SDK10（支持 net9.0）、Clang18、Python3 可用。容量有限，创建整套副本前用 `df -h /workspaces` 核查。

本机 gh 的 Codespace SSH 可用，但 Codespace 内的 gh/git HTTPS 不保证已认证；最近远端 `git fetch` 报无法读取 Username。本次通过本机 GitHub 推送，再把 git bundle 送入自己的 worktree 完成同步，没有复制 token。不要把“SSH 能连”误认为远端 gh API 已授权。

```powershell
& 'C:/Program Files/GitHub CLI/gh.exe' codespace ssh -c glowing-train-p7j9gp74q6jwc76v6 -- 'cd /workspaces/GodsPVZ-native19 && git status --short'
```

浏览器曾因 transport/request-header policy 失败，属于工具连接问题，不能据此推断 Codespace 或项目坏了。CLI 可用时优先 CLI。不要为此重置用户浏览器、凭据或无关环境。

## 7. 可复现验证与证据强度

在现有 Codespace 中，以下命令需要已恢复的缓存/工具，不是空机器一键安装器：

```bash
cd /workspaces/GodsPVZ-native19
python3 scripts/codespaces/test_native19_fixture.py
python3 scripts/codespaces/native19_local.py
python3 scripts/codespaces/qualify_native19_cpp.py
python3 scripts/codespaces/native20_local.py
python3 scripts/codespaces/qualify_native20_cpp.py
python3 scripts/codespaces/native21_local.py
python3 scripts/codespaces/qualify_native21_cpp.py
```

**qualification 最后返回非零是当前已知全游戏编译失败，不能改成忽略错误以显示绿色。** 可以直接读取归档；不要为交接重复耗费已通过的测试。全新环境从 `run_native14_validation.py` 的三个锁定包、Native15–17 链及 resolver/Cecil 开始，读取对应 README；旧 GitHub artifacts 可能过期，需先核查可用性。离线交接包也不是完整 Editor/Unity 项目安装包。

验证工具和作用：

1. `PatcherNative19/20/21`：锁输入、精确目标、保留 MVID/counts、内存与 reopen 隔离、输出实际 CIL specs。20/21 引用 Native19 的共享 exporter/fingerprint 源码，不能丢掉该源码目录。
2. `verify_native19_types.py`：fail-closed 的支持子集，检查栈类型、调用 receiver/参数、闭合泛型、分支合流、数组、EH、MaxStack。不支持的 opcode 失败，不是完整 ECMA-335 verifier；不可达部分单列。
3. `test_native20_types.py` / `test_native21_types.py`：对**实际序列化再读取的 CIL**逐项撤销修改，必须失败；不是只测补丁器内存状态。
4. `Native19Fixture`：实际共享 emitter 写入 CLR fixture，36 项断言；所有 Unity/helper doubles 的边界必须保留。
5. `test_native19_die_cfg.py`：解释实际 CIL，与独立 native oracle 对比7424状态，检查事件/字段变化，未知调用/opcode拒绝，另有3个语义负例。不能叫真 CLR 或真机验收。
6. `native*_local.py`：两个输入分别两次重建、逐字节确定性检查、真实 IL2CPP、cpp/metadata 非空检查；所有输出路径必须落到该阶段隔离目录。
7. `cpp_syntax_preflight.py`：真实 Clang，`-ferror-limit=0`，所有21 game TUs，空目录失败。Linux 与 Apple ABI/SDK/linker 不同，不能叫 Apple 编译通过。
8. `qualify_native20/21_cpp.py`：校验候选 hash、C++文件数和生成 hash不变，并定位目标定义。识别构造函数、命名空间及参数类型以区分重载；遇歧义明确失败，不能随便选第一个函数。它仍不是通用 metadata→C++ map。
9. `analyze_cpp_failures.py`：按定义位置关联错误，属于诊断导航；headers/global/未解析报错不应被丢弃。下一步需覆盖另外的 generated assemblies 和 generic C++，不能只编21个游戏分片。

Exact header 工件：run36505731509，artifact11008315616，名称 `GodsPVZ-Stage9-native18-unsigned-Xcode`，ZIP SHA `ba30763dcb6b225f27e1b415787a725d2f9bd868793f3014dd083542e23c7b1e`。它只是编译 headers/导出参考，不是 Native21 的 Xcode 成品。

## 8. 高还原必须遵守的注意点

- **先证明身份再解释代码。** 原 metadata 的 MethodDef、Cpp2IL reference PE RID、恢复 DLL token 不必一一相同。用完整签名、所属 image、相邻方法与字段身份交叉核对。
- 普通方法查 CodeGenModule/methodPointers；泛型查 MethodSpec、generic-method-function、class/method instantiation、reference-sharing 与 fully-shared body。共享函数地址旁的符号名可能只是别名，不能直接当实际被调用方法。
- Win64 rcx/rdx/r8/r9、栈上传参、隐藏 return buffer/MethodInfo 和结构体 ABI 必须结合函数签名判断。不能把寄存器里的地址当 Vector3 值，也不能反过来。
- `.pdata` 可能有 CHAININFO、拆分片段、cold cleanup。Native19 简单抽取器用“同 managed image 下一个 pointer”作反汇编窗口，不等于精确函数 extent；必须另核 `.pdata`、跳转目标和内嵌表。旧按同一类下个方法截取会吞入其他类代码；只读首条 pdata 也可能截断函数。
- 原始 null fault、Unity `Object.op_Equality` 假空、`isinst` 失败不是一回事。不要新增 defensive early-return、吞异常、catch-all、默认对象来让测试绿。
- 保留调用次数/顺序、随机数消耗、首个最小值 tie、边界上的无符号比较、溢出、float NaN 行为、异常时已经发生的部分写入。
- Vector3 的三个分量可能承载逻辑位置/高度差，不一定就是 Transform.position；结构体局部改类型不能弥补缺失的字段计算。
- 泛型参数 `!0`/`!!0` 必须在正确的 owner/method 实例绑定。List<Zombie>.Remove(!0) 与 List<object>.Remove 不是一个调用。Enumerator 必须覆盖 GetEnumerator→Current→MoveNext→Dispose，不能只改签名。
- 对 locals 同时查所有定义和所有使用，必要时拆开混用的临时变量；不能因为一个消费者想要 int，就把同时承载引用的 object 全局改掉。整数与 bool 在 IL 栈上兼容，也不能因此随便改变持久字段意义。
- `ldloca` 与 `ldloc`、`ldflda` 与 `ldfld`、float零与整数零、native int与int64分别有规范边界。某个 CLR 宽容接受并不等于规范或 IL2CPP 都接受。
- switch 表、未实现 native load、`Indirect jump`、`Method not found` 之类提示是缺失语义线索。删除字符串/NOP 不是恢复实现。禁止全局 ret→throw、dummy return、C++ 强转遮错、屏蔽编译器错误。
- Cecil 改 Instruction 需保留或重定向所有 branch/switch/EH 目标。NOP 不保证原始 byte offset 不变；短跳转需检查扩宽。最终读回真实 DLL 验证。
- 测试预期应独立来自 native 证据，不能把补丁器逻辑复制为 oracle。负例应针对类型、对象链、坐标、调用顺序和遗漏分支，而非全都只插入一个 Pop。

## 9. 本轮基础设施教训

1. 空 C++ 目录曾错误打印零错误；现已改为 fail-closed，强制21个文件。不能只抓退出码或一行 PASS。
2. 复制的 rsp 曾保留旧工作区绝对路径，早期 Native19 生成文件写入旧 ignored cache；后来纠正输出路径并从原 Native18 DLL 重建旧 cache。旧 tracked 源码与 DLL 未改。所有当前归档证据来自正确隔离目录。每轮都要检查 generatedcppdir、symbols、data、profiler、cwd。
3. 类型门不是语义门，转换成功不是编译成功；最早 macOS “20 errors”常受错误上限影响，实际全量诊断远不止20个。
4. 稀疏检出不代表文件在 Git 中不存在；新增 Recovery 目录可能需要 `git add --sparse`。本机 CRLF/远端LF会造成 hash和diff不一致；证据按最终提交字节生成SHA，stage后核对 Git blob。
5. 原生反汇编文本尾随空格会让 diff --check 输出上万行；归档提取器已 strip trailing whitespace。规范化只用于文本证据，不能默默改变输入二进制。
6. 目标 C++ 名称映射曾在 constructor、namespace、overload 上失败。那是报表工具拒绝歧义，不能称为新游戏编译回归；现已结合参数信息纠正，并保存最终完整 qualification。
7. 用户发新消息后，本机 PTY session 可能消失，但远端工作已完成。先看 candidate/result/log和进程，别盲目重复长任务。不得输出登录 token、许可证或把账号配置打入归档。

## 10. 暂停点与接手顺序

**已完成并保留：** Native19 检查点；Native20/21 的候选生成、确定性、typed/negative、non-target隔离、IL2CPP、21分片Clang预检；本交接及候选/源码/日志/hash归档。

**未完成并暂停：** 剩余676个报错关联方法的分组诊断与恢复；Native20/21逐方法native/行为补证；其余generated assemblies/generics全量编译；新完整workflow编排；真实Apple构建/链接/打包；设备测试。没有 Native22 候选，没有 Native19+ 完整 Actions，没有 IPA。

建议接手顺序：

1. 读取本文、Native18审查、Native19README、20/21 RESULT和SHA清单，校验候选hash，检查git状态及Codespace可用性。不要把历史状态文档中“当前”直接当今日状态。
2. 使用自己的隔离分支/worktree，从锁定Native21继续实验，同时保留 Native19/20/21。不要在 Antigravity worktree 或生产分支做覆盖。原 production/audit 历史锁为91a8adac/e40bdb0，本轮未移动；若需要其当前状态，重新read-only核查。
3. 从 `Native21-2026-09-29/validation/cpp-final38/method-errors.json` 和对应完整logs选择下一具体方法族；先检查损坏是否本来存在以及是否涉及缺失native控制流。**不要自动把676方法一次改完。**
4. 对纯编码修正保持精确站点清单、类型流和逐项负例；对native语义丢失建立完整MethodBody spec和独立行为测试。为仍未覆盖的依赖保留明确缺口。
5. 本地全量C++已知失败闭合后再准备完整workflow：锁候选、检查YAML/存在的脚本、失败日志上传、Xcode产品目录和packager；完成一次真实导出与Apple编译。若失败，记录新信息，不能换名宣布成功。
6. 真正得到IPA后验证真实Payload/app/Mach-O/metadata/CRC/SHA，并提供可下载artifact；设备运行情况单列。

## 11. 交回 Codex 的审查清单

接手 AI 完成后，请交回这些具体产物，避免只说“工作流绿了”：

- 仓库、分支、完整 commit SHA、父基线、是否存在未提交修改；所有改动文件和精确目标 token。
- 每一步输入/输出 DLL SHA256、源码/manifest SHA、生成命令、工具/Unity/headers版本，linked与unlinked分开。
- 原版 native VA、完整函数范围与泛型映射、字段身份、MethodBody spec、关键反例及尚未证明的行为。
- 正向与负向测试的原始日志，重开盘 typed/EH/CFG/use-def结果，非目标normalized隔离；若声称raw不变另交raw证明。
- 所有game与其他generated C++编译结果、Apple Clang/xcodebuild完整日志、xcresult、实际workflow run URL与精确head SHA。
- IPA artifact/asset下载地址、大小、SHA256、ZIP CRC、Payload/app结构、bundle ID、架构、metadatahash、签名状态；真机测试实际做了什么。
- 当前仍有的异常、资产/字体/shader问题和未覆盖场景，不把“测试替身通过”写成游戏运行通过。

Codex 回接时将对精确提交与二进制独立验证，不直接采信对方总结，不因编译通过放弃保真审查。

## 12. 证据目录与旧云盘入口

- [Native19](Native19-2026-09-29/README.md)：15方法native提取、specs、typed/Die/fixture、Clang结果和源码SHA。
- [Native20](Native20-2026-09-29/README.md)：73方法候选、逐处编辑清单、before/after IL、112负例、C++结果。
- [Native21](Native21-2026-09-29/README.md)：38方法PROPOSALS、90locals+9edits、99负例、最新全量错误表与候选。
- [历史完整交接](Historical-Handoff-2026-09-27.md)：早期根输入、native3/4、未完取证和历史云盘包。
- 原PC/Android云盘文件ID见 `Stage9.1-original-input-locator-v1.json`。历史2026-09-27交接文件夹：`https://drive.google.com/drive/folders/1saNnayoUNJwphQ634dk9KUV7K5KYjP2h`。这是旧归档入口，**不表示Native21已上传该云盘**。

本轮GitHub与本地归档只收录明确的源码、候选、日志、文档与校验清单，不复制登录凭据、Unity许可证、整个Editor缓存或个人配置。保留源码与二进制证据之后暂停，等待接手者工作及用户交回审查。
