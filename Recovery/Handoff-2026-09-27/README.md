# GodsPVZ Stage9 最新交接 — 2026-09-27

## 先读结论

本交接覆盖本任务的 native3/native4 恢复、全程序集本地预检、尚未完成的 native5 取证、Windows 托管 IL2CPP 实验、Codespace 访问阻塞，以及交接时新发现的远端 native6 提交。**完整 Unity iOS 导出尚未通过，没有可交付 IPA，也没有进入 macOS 构建。**

最新远端恢复分支 `stage9-native3-codex-recovery` 在本次读取时为 `b873b8f75173dcc022c1a4de555bc21e78e6e40d`。它已超过本任务此前完成的 `78f7270bac5699f0ea79d1ec5dfdb1a0aed63ce7`；新增三笔提交与 Codespaces/native6 有关，源码已同步保存，但本任务没有独立验证这些新方法的语义或候选 DLL。不得把“远端存在”写成“已通过验证”。

本次交接通过独立分支 `handoff/stage9-latest-2026-09-27` 发布。Drive 文件夹位于原 Stage9 交接文件夹内：
https://drive.google.com/drive/folders/1saNnayoUNJwphQ634dk9KUV7K5KYjP2h

## 1. 权威输入、冻结边界

| 输入/分支 | SHA256 或 Git commit |
|---|---|
| 唯一可信 managed baseline：根目录 Assembly-CSharp-59bb-native2.dll | `18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd` |
| 原 PC GodsPVZ_1.0.2.zip | `2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48` |
| GameAssembly.dll | `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d` |
| global-metadata.dat | `ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9` |
| 精确 Unity China 2022.3.44f1c1 Linux tar.xz | `0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14` |
| production：stage9-admin-start-static | `91a8adacb42414e6bce8b4df8b8811be9e488dab` |
| 旧 audit：stage9-local-audit-tools | `e40bdb0bc54dd5fc6e48d919387376f3e62e4c58` |

生产和旧 audit 在本次上传准备时直接读取远端，均未变化。程序集身份约束：2317 MethodDef、2802 FieldDef、320 TypeDef，2297 个方法体。原 PC native 是主要语义依据；Gemini 的 ff0e46/native3-blocker-repair 实验不可继承。旧交接全文保存在 `audit/stage9-native3/sources/handoff-2026-09-26.txt`。

用户已明确批准第四方法 ZombieSelect 构造函数，并要求能直接推进就直接推进；同时要求尽量本地发现问题，不要每个基本 IL 错误都烧一次约二十分钟的 Unity Actions。没有独立工作可做时不要空等长工作流。

## 2. 本任务完成的 native3 与 native4

| 版本 | 修复范围 | 候选 SHA256 |
|---|---|---|
| native3 四方法 | 060001E0 MouseManager.GetZombieUnderMouse；060001B2 EnemyManager.CreateEnemySelecter；06000248 SavesManager.PassLevel；060001A5 ZombieSelect::.ctor(ZombieInfo) | `72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc` |
| native4 六方法 | 保留前四个，新增 0600039F Plant.SetAnimationState_Planting 和 06000267 SunManager.SetMaxSunFallTime | `11c1a9648ea67cb0ac66b84ddde50cb4ef46d24152f67515a99afb526a2d4b4e` |

两个版本都从 18e44 生成。native4 只把 native3 用作原四方法的逐方法语义对照，不以它作生成输入。完整源码、native 反汇编、字段偏移、调用映射、逐条 CIL 规格、回读、引用差异及 CLR harness 位于两个 `audit/stage9-native*` 目录。

native4 验证结果：六方法共 599 条 CIL；六个负例；74 项 CLR 行为断言；原四方法保持；非目标方法零语义变化；24/24 历史引用、23/23 Resolve keys、3/3 transform sites、无孤立泛型。该门只证明这些修复与所测条件，不是整个游戏通过。

关键语义：PassLevel 是完整方法体重建，不是枚举器类型替换；原 baseline V31 未使用，不应凭想象修改。Plant 恢复原版 null 抛异常、ID 4/49 不可攻击、ID 2 的 state=1 和 ExplodeTrigger 分支。Sun 使用 boardConfig.map，第二个 Map 参数不读取，场景与词条规则、finally Dispose 和成功后写回均有证据。

## 3. 云端真实结果

| Run | 结论与边界 |
|---|---|
| [36213547473](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36213547473) | native3 静态门 success，工件 10896577104 |
| [36213730164](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36213730164) | native3 真实门 failure；Plant.SetAnimationState_Planting / SunManager.SetMaxSunFallTime，已由 native4 恢复 |
| [36228526004](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36228526004) | native4 静态门 success，工件 10902205446；已下载回读候选一致 |
| [36228617935](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36228617935) | native4 真实门第 23 步 failure；TextLink.ResetLink / Color 和 Zombie.ZC_LadderPlaceEnd / Vector3；后续审计和 Xcode 上传跳过 |
| [36249264374](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36249264374) | 后续远端提交触发的 native4 静态门 success；名称和范围仍是六方法，不等于 native6 通过 |
| [36249901466](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36249901466) | 新远端 native6 工作流 failure；本次只核实终态 |
| [36250081429](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36250081429) | 最新 native6 failure；jobs API 明确失败在第 5 步 Verify exact native6 candidate DLL，尚未到 Unity 转换；具体校验错误尚未提取 |

native4 真实门证据工件 10902486495，ZIP SHA256 `9df977c1c1df8863da53fff6ccea4cfaa6c35b93a047275ab92c672fe4fdccc6`。重导入后 candidate SHA 未变。两个新 blocker 在 baseline 与 native4 方法体相同，非六方法修复引入。新 native6 工件 10909190110 只有 696 bytes，不可当作完整导出；metadata 已保存。

## 4. 新发现的远端 Codespaces/native6 工作

新增提交：`087f15adb131f6ec3bf1830bfd84ec7bc58883db`（TextLink、Ladder patcher 和 Codespaces 管道）、`220e05fc4b7a2b08a6387a9014f5f1877ff7e4da`（native6 真门）、`b873b8f75173dcc022c1a4de555bc21e78e6e40d`（候选环境变量）。

`scripts/codespaces/` 包含环境检查、六方法静态门、Unity/R3 安装、iOS 导出及 run_all。Patcher 从 native4 文件开始，生成 native5-textlink 与 native6-ladder；涉及 ResetLink、SetLink、ZC_LadderPlaceEnd。这里的 native5 命名与本地 `audit/stage9-native5` 未完取证不同。

本次只读取、备份这些提交，没有执行或认可 patcher 的新增引用、栈、异常、颜色转换、向量语义。远端还修改了原 native4 input-locks 与 expected-reference-use-deltas，交接分支保留原样；本地旧六方法工具在备份中独立保留。下一人应先核查 native6 第 5 步的具体输入来源、路径、SHA 与工件，再决定后续验证。不得用静态门名字或源码注释中的“高保真”替代独立证据。

## 5. 全程序集本地预检

`audit/local-preflight/AssemblyPreflight.cs` 覆盖 2297 个方法体：CFG 栈深、合流、下溢、返回残栈、MaxStack、相邻生产者/local 类型，及基本块内结构体参与原始运算；占位文本和枚举器线索另列。现在加入精确 Unity resolver，减少外部 enum 误报。

最新结果为 baseline 247 个方法有标记、native4 242 个方法有标记，native4 648 个方法有占位/枚举器线索；这些集合有重叠，不能相加。旧 README/历史失败报告中的 245 是前一次扫描值；以本包 JSON、TSV、scan-validation.json 为准。

已在本地复现历史 Plant/Sun 和当前 Color/Vector3 错误模式；六个修复方法清零，77 个正常编译方法体对照清零。**242 不是已证明的 242 个 IL2CPP 失败，更不允许批量自动修复。** 工具不是完整 ECMA verifier，没有完整跨块类型推导或 native 语义证明。

TextLink.ResetLink：IL_01BA cgt 将 I4 与完整 Color 比较；LadderPlaceEnd：IL_0024 xor 将完整 Vector3 与 native int 运算。这些可提前定位，不必等待下一次完整 Unity。

## 6. 未完成 native 取证及必须修正的提取器问题

`audit/stage9-native5` 是草稿取证，无本任务生成的 candidate。部分 `.pdata` 函数被 CHAININFO 拆成多个片段：LadderPlaceEnd 真入口 0x180370340，第一段只有 98 bytes，完整已识别片段至 0x18037054F，总 527 bytes。旧按首条 pdata 的方法会截断函数，不能据此写高保真规格。

`audit_struct_family.py` 正在增加递归 unwind chain 合并，但重新提取在 060003C4、0x376509–0x3767D4 遇到不完整线性反汇编而主动停止。当前目录保留新旧混合的中间文件和旧 manifest，**全部必须按未完成草稿处理，不可作为已通过全量提取的集合**。单独 `LadderPlaceEnd.fragments.txt` 含后续邻近函数调试输出，制作规格时需明确边界。

已识别 LadderPlaceEnd 的重要操作包括 ladderZombie_place=false、LadderTestPlace、rDirection.x/y 符号翻转乘 67、坐标到 grid、DeviceData(11)、camp、PlaceDevice、zombieClips[5]、CreateAudioAtPoint、Drop_Armor2(true)。恢复仍需完整引用与依赖证据；同类 LadderTestPlace/PoleTestJump 也在待审计名单。

## 7. 本机与 Codespace 环境

本机 Windows 有 Framework csc、Mono.Cecil、Python；未安装真正 Unity Editor，unity.exe 是管理 CLI；未安装 WSL。精确 Linux Editor 15 分片已下载并校验，提取了 626 个工具/解析依赖文件，124848489 bytes，完整提取 manifest 已保存。

实验性 Windows 托管：从官方 Microsoft release metadata 获取并 SHA512 验证 .NET runtime 6.0.18；保留原提取工具，另建 windows-codegen，托管同一 IL2CPP managed DLL。`dotnet il2cpp.dll --help` 成功，日志和来源证明在包内。**没有完成实际 C++ 转换，更没有完成 Unity/iOS 导出。** 后续需要精确依赖闭包、stripped managed 输入和运行兼容性验证。

用户 Codespace：`https://glowing-train-p7j9gp74q6jwc76v6.github.dev/`。浏览器工具重装、桌面重启后仅偶尔成功读取标签列表，页面接管/内容读取仍失败；普通 GitHub Actions 与哔哩哔哩首页也同样 `Unable to load browser request-header policy`，因此问题不局限 Codespace。本任务没有实际进入其终端或验证安装状态。现有 Git 凭据请求 Codespace REST 返回 403。未绕过策略、修改凭据或执行旧 fix-browser.js。

`check-codespace.sh` 是只读环境盘点脚本，不打印 token、环境变量或许可证正文。它可检查工具、项目版本、资源与 ManagedStripped/il2cppOutput 缓存。Codespaces 仓库脚本的存在不证明远端机器已安装或已成功运行。

## 8. 其他未解决项

- AttackRangeUIController.CollapseView（060005BE）有 GetEnumerator/Dispose 占位与未初始化枚举器；六方法 harness 对它和 Unity 使用替身，因此不能宣称 UI 依赖已修复。
- TMP_FontAsset.m_AtlasTextures MissingReferenceException 在 native3/native4 均出现；构建继续到 IL2CPP，但资产异常仍独立未解。
- 禁止 defensive null swallowing、全局 ret→throw、凭推断批量替换 locals、重复改造旧 native2 workflow。
- 完整静态、引用与语义门之后才跑集成导出；真实导出、Xcode 审计、工件成功之后才进入 macOS；设备运行验收仍是后续门。

## 9. 下一步执行顺序

1. 核对本包 manifest、18e44 输入、远端 native6 新提交和最新 run 第 5 步错误；恢复目录中的同名版本不得混用。
2. 取得 Codespace 实际盘点和日志；如果已有正确 Editor 与 ManagedStripped 缓存，研究直接重复执行 IL2CPP 转换，避免反复安装与导入。
3. 对远端三个新增方法独立做完整 native/spec/readback/引用/栈/行为核对；补齐 chained pdata 提取，并区分机器码与内嵌数据。
4. 同时审计本地预检同类损伤；以闭合证据分组修复，记录未修复依赖与资产问题。
5. 符合门槛后执行一次完整真实导出，读取终态与产物，而不是只看 workflow 已启动。

## 10. 文件范围与可复现性

交接包含 baseline、两个候选、native3/native4 源码/规格/回读/解析依赖/native 与门禁证据、全量扫描、native5 未完草稿、本机工具实验来源与脚本、最新 GitHub API 状态，以及远端 b873b8f 的工作流、恢复工具和 Codespaces/native6 源码快照。远端快照来自明确的稀疏检出范围，不是整个仓库的离线克隆；其余历史跟踪文件仍在 GitHub 提交中。另附逐文件 SHA256 和排除文件清单。

不重复上传 3.9GB Editor 缓存分片、下载运行时、pip 库、重复预检目录、许可证与激活/归还日志。它们的来源和锁定哈希保留；原版 GameAssembly/metadata 纳入单独 input 包。证据文本若含 token 模式会脱敏，manifest 对归档后的字节计算。GitHub 已有历史提交继续保留。本交接不升级任何候选的验证等级。
