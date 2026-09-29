# GodsPVZ Native18 独立审查

审查日期：2026-09-29。结论：**Request changes；不能通过保真恢复验收，也没有导出 IPA。**

对象是 `lyiming710-cloud/GodsPVZ-iOS` 的 `repair/antigravity-native18-zombie-fix`，精确提交 `64b86ed725c40e8e00cecd7a917f72bbed37777e`。审查覆盖该提交及其父提交相对 Native17 基线的 13 个文件，核对补丁实现、资格验证、打包、工作流、归档证据，并独立检查候选二进制、原版 PC 机器码及两次 Actions 失败日志。没有修改修复分支、候选 DLL 或发起构建。

本地证据目录：`review-evidence/`。Codespaces 独立实验位于 `/tmp/godspvz-native18-independent-review-64b86ed`，只读取仓库文件。代码快照来自精确 Git commit，不依赖工作目录中未提交的源文件。

## 1. 重要发现

### F1 [P1] Start_PreviousPosition 重写了 previousPosition 的含义

位置：[Program.cs:269](scripts/takeover/PatcherNative18/Program.cs#L269)。补丁直接设置 `previousPosition = transform.position`。

原版 `0x180369580` 的明确行为是：

```text
previousPosition.x = fX
previousPosition.y = fY
previousPosition.z = animationGroup.transform.position.y - previousPosition.y
```

证据：`0x180369586/595` 分别读取 `Zombie+0x30/+0x34` 写入 `+0x148/+0x14C`；`0x18036959B` 从 `+0x1A0` 读取 animationGroup；`0x1803695C3–5D0` 取动画 y 减 previousPosition.y，写入 `+0x150`。字段表确认上述偏移。

因此这里 Vector3 的第三分量储存高度差，并非世界 z。举例：fX=10、fY=20、动画 y=25、transform=(10,20,7)，原版为 `(10,20,5)`，补丁为 `(10,20,7)`。这是新引入的行为改变，必须按原版恢复。证据：[Start_PreviousPosition.native.txt](review-evidence/Start_PreviousPosition.native.txt)、[Zombie.fields.json](review-evidence/Zombie.fields.json)。

### F2 [P1] Update_Move 丢失 animationGroup 的 fZ 高度偏移，并改变空引用行为

位置：[Program.cs:381](scripts/takeover/PatcherNative18/Program.cs#L381)。补丁把与根 transform 完全相同的 newPos 写入 animationGroup，并新增 `Object.op_Inequality(animationGroup,null)` 后跳过更新。

原版根 transform 为 `(fX,fY,原z)`，动画对象为 `(fX,fY+fZ,原z)`：`0x18036C5F8` 读取 `+0x38=fZ`，`0x18036C60F` 加 `+0x34=fY`，`0x18036C63D` 写动画位置。补丁根本没有读取 fZ。fZ 非零时动画高度必然错误。原版 animationGroup 为真空引用时走异常辅助路径，补丁静默跳过；这也违背既定保真要求。

证据：[Update_Move.native.txt](review-evidence/Update_Move.native.txt)。Time.deltaTime 从两次读取改为一次不作为主要缺陷；上述 fZ 与空引用差异已经足以阻断。

### F3 [P1] Update_PreviousPosition 把增量跟随改成绝对对齐，丢失 UI 的位置和高度变化

位置：[Program.cs:427](scripts/takeover/PatcherNative18/Program.cs#L427)。补丁令 UI.x=self.x、保留 UI.y、UI.z=self.z，随后保存整个 self.position。

原版 `0x18036C720` 计算：

```text
dx = self.position.x - previousPosition.x
dy = self.position.y - previousPosition.y
height = shadow.position.y - self.position.y
dh = height - previousPosition.z
对存在的 UI_HP、UI_Elements、UI_Buff：
    UI.x += dx
    UI.y += dy + dh
    UI.z = self.position.z
previousPosition = (self.position.x, self.position.y, height)
```

原版 x/y 差值位于 `0x18036C7A5–7DD`；shadow 来自 `Zombie+0x1F0`，高度差位于 `0x18036C80E–84F`；第一个 UI 的 x/y 增量加法在 `0x18036C8B5/8BF/8C7`，另外两个 UI 重复同样操作。最后 `0x18036CB56–BCE` 保存 shadow 与自身的 y 差。

静态反例：previous=(10,20,5)、self=(12,23,7)、shadow.y=29、UI=(13,30,0)。原版 UI=(15,34,7)，新 previous=(12,23,6)；补丁 UI=(12,30,7)，新 previous=(12,23,7)。横向偏移被抹去，UI 纵向不再跟随。

证据：[Update_PreviousPosition.native.txt](review-evidence/Update_PreviousPosition.native.txt)。以上是来自实际 CFG 的反例，不是声称已经进行真机画面测试。

### F4 [P1] Die 未恢复跳转表；NOP 删除的是缺失语义的证据

位置：[Program.cs:641](scripts/takeover/PatcherNative18/Program.cs#L641)、[Program.cs:663](scripts/takeover/PatcherNative18/Program.cs#L663)。V29–V32 是反编译残留的 PC 映像基址/间接跳转临时量，不能称作已经恢复的游戏掉落物偏移。生成 C++ 仍含十进制 `6442450944 = 0x180000000`，没有相应 switch。

从原版读取的两个跳转表：

| ID | 非灰烬分支表 0x18035ED74 | 灰烬分支表 0x18035ED8C |
|---|---|---|
| 0 | 0x18035E906 | 0x18035EA3C |
| 1 | 0x18035E8D0 | 0x18035EA06 |
| 2 | 0x18035E906 | 0x18035EA3C |
| 3 | 0x18035E8D0 | 0x18035EA06 |
| 4 | 0x18035E906 | 0x18035EA3C |
| 5 | 0x18035E906 | 0x18035EA3C |

补丁输出在 `IL_008C` 和 `IL_030A` 只计算基址后顺序落入下一块。以 ID=0、ashe=false、ashes=false、isDied=false、isDying=true 为例：原版沿跳转表检查 isDying 后跳过 DropLootPiece；候选通过默认分支将 isDied=false 存入 V24，随后在 `IL_069C → IL_01D8` 调用 DropLootPiece。原版 IsPlantZombie 对 ID=0 返回 false 已独立核对。

此外 `ldc.i4.0; conv.i; ldc.i8 6442450944; add` 混合 native int 与 int64，并不属于规范列出的 add 类型组合。Codespaces .NET 10 的最小实验接受该序列，说明运行时宽容不能替代规范验证；本报告没有据此声称它必然在所有 CLR 上抛异常。

还需澄清：`ldstr` 换 `nop` 不会保持原始字节偏移；Cecil 应以指令对象维护目标。这里通过下标换成新的 Instruction 对象，并无通用分支/EH 重定向。独立重开盘检查未发现本候选中的悬空 branch operand，因此不把这个工程风险误报为已证实的分支损坏。已证实的错误是跳转表语义缺失。

证据：[Die.native.txt](review-evidence/Die.native.txt)、[Die-jump-tables.json](review-evidence/Die-jump-tables.json)、[native18.il.txt](review-evidence/native18.il.txt)、[GodsPVZRuntime1__9.cpp](review-evidence/GodsPVZRuntime1__9.cpp)。

### F5 [P1] IPA 工作流引用不存在的打包与验证脚本

位置：[workflow:664](.github/workflows/stage9-native18-real-ios-xcode-gate.yml#L664)、[workflow:670](.github/workflows/stage9-native18-real-ios-xcode-gate.yml#L670)。精确 commit 树与 Codespaces 均没有：

```text
scripts/package_unsigned_ipa.mjs
scripts/validate_ipa.mjs
```

macOS job 使用 checkout 后没有生成这些脚本的步骤。因此即使 C++ 编译通过，下一阶段也会因找不到模块失败。不能将现有工作流称为已经具备完整 IPA 交付能力。

另一个实际配置错误在 workflow:630：预先 mkdir 自定义 CONFIGURATION_BUILD_DIR，再执行 `clean build`。两次日志均出现 `Could not delete .../Release-iphoneos because it was not created by the build system`。应使用 Xcode 管理的构建目录或适当调整 clean/build；不要只关注 C++ 错误。macOS 日志、xcresult、编译命令应在失败时也上传。

### F6 [P2] Ashe 虽修正值参数，仍遗漏伤害飘字坐标的 y 赋值

位置：[Program.cs:521](scripts/takeover/PatcherNative18/Program.cs#L521)。候选将 V4=transform.position 直接传入 CreatDamageText；中间没有修改 Vector3.y。

原版 `0x18035DA0E–DA20` 明确设置 `position.y = fY + fZ + 134.0f`。134.0f 是直接从该 RIP 相对操作数对应的 PC 二进制常量读取，不是经验猜测。这个计算在输入损坏 IL 中已经丢失，本次修复也没有恢复。故可确认 ABI/按值传参方向正确，但不能宣称该方法原版行为已经恢复。

证据：[Ashe.native.txt](review-evidence/Ashe.native.txt)、[native18.il.txt](review-evidence/native18.il.txt)。

### F7 [P2] 资格门只验证栈高度和 C++ 文本，不能证明类型正确或 Clang 通过

位置：[Program.cs:209](scripts/takeover/PatcherNative18/Program.cs#L209)、[validate_native18.py:83](scripts/codespaces/validate_native18.py#L83)、[workflow:38](.github/workflows/stage9-native18-real-ios-xcode-gate.yml#L38)。

StackCheck 的状态是 `Dictionary<Instruction,int>`，没有栈类型。11 个负控制全部只是入口插入 Pop，能证明检测下溢，不能检测 Vector3& 与 Vector3、object 与整数、泛型实例不匹配等问题。

在 Codespaces 编译并直接调用精确审查源码的私有 StackCheck，构造 `ldloca Vector3; call ConsumeValue(Vector3); ret`，实测结果：

```text
PATCHER_STACKCHECK_TYPE_NEGATIVE_CONTROL_ACCEPTED:
Vector3& supplied to Vector3 value parameter
```

`validate_native18.py` 虽在 docstring 写 Clang syntax qualification，实际没有调用 Clang。它检查文本片段、方法名称和无反编译提示字符串。删除提示文本即可满足部分断言，缺失的跳转表不会因此恢复。workflow 中运行的是 Native15–17 的 CLR fixtures，没有 Native18 行为 fixtures。

证据：[codespace-review.log](review-evidence/codespace-review.log)、[codespace_review.cs](codespace_review.cs)。本次 Codespaces 有 clang++ 18，但所检查的工作区、/opt、/tmp 未找到所需 libil2cpp headers；没有冒充已经完成本地 Apple Clang 编译。

### F8 [P2] “2286 个非目标方法 100% 字节不变”被独立二进制比较否定

位置：[Program.cs:143](scripts/takeover/PatcherNative18/Program.cs#L143)、[Program.cs:169](scripts/takeover/PatcherNative18/Program.cs#L169)、[README.md:20](Recovery/Native18-2026-09-29/README.md#L20)。

Fingerprint 将成员引用解析为名字、分支解析为指令序号，并将短分支标准化。这不是字节比较。

对哈希锁定的 Native17 与 Native18 unlinked 文件，独立使用 PE 原始数据得到：

| 检查 | 结果 |
|---|---:|
| 有方法体的方法总数 | 2297 |
| 实际目标方法数 | 11 |
| 非目标方法体数 | 2286 |
| 原始 MethodBody 字节变化（包含头、IL、EH 段） | 1088 |
| 原始 CIL 指令流字节变化 | 561 |
| 解析引用后的非目标方法结构变化 | 0 |

Codespaces 使用独立的 System.Reflection.Metadata/PEReader 实现，再次得到非目标 raw CIL 变化 561。1088 与 561 是按实际方法 Token 重算的最终结果；最初沿用报告中的错误 Token 得到 1089/562，已纠正。

解释：结果符合 Cecil 重写后元数据 token/局部变量签名 token 重排的表现；不能据此认定额外 1088 个方法被逻辑破坏。我们按方法签名、属性、locals、指令操作码和解析后的操作数、EH 结构比较，只有指定 11 个方法发生结构变化。不过原任务如果坚持逐字节一致，这个候选明确不满足；若接受语义隔离，必须改成真实、精确的验收表述并补充 metadata/token 映射验证。

证据：[raw-comparison.json](review-evidence/raw-comparison.json)、[normalized-comparison.json](review-evidence/normalized-comparison.json)、[compare_raw.py](compare_raw.py)、[Inspect.cs](Inspect.cs)、[codespace-review.log](review-evidence/codespace-review.log)。

## 2. 11 个目标方法的逐项结论

原版 PC 元数据和候选 managed 元数据一致。用户报告中的最后三个 Token 有误；修复器按名字定位，所以不能据此声称补丁实际修错了方法。

| 方法 | 实际 MethodDef | 本轮独立结论 |
|---|---|---|
| PreviousPosition | 0x06000418 | 直接返回 previousPosition，符合所核对原版 native |
| Start_PreviousPosition | 0x0600041E | 不通过，F1 |
| Update_Move | 0x06000427 | 不通过，F2 |
| Update_PreviousPosition | 0x06000429 | 不通过，F3 |
| ArmBroken | 0x0600042F | 两处值参数来源有依据：camera.position 与 GetAnimationSpritePosition 返回值；未授予整方法运行行为通过 |
| Ashe | 0x06000430 | 值参数类型方向正确，遗漏 y 计算，F6 |
| CheckZombieWin | 0x06000432 | Grid 普通引用判空改 ldnull 方向正确；应加精确匹配次数和类型流验证 |
| CreateStartPrePath | 0x06000433 | EnemyPath 判空修正合理，循环起值仍需完整 use-def/CFG 验证；不能等同整个方法已恢复 |
| CreatParticles | **0x06000434** | 从 shadow.position 获得 V3 并按值传入 set_position，符合所核对 native 链 |
| DestroyZombie | **0x06000436** | 实际输出为 List<Zombie>::Remove(!0)，该泛型签名正确；GameObject[] 使用 ldelem.ref、索引 int32 方向正确 |
| Die | **0x06000437** | 不通过，F4；V15/V18 是布尔反转产生的标志，V33/V36 是 ID-14 范围检查值，不能统称循环计数器 |

**0x06000438 实际是 DropLootPiece，不是 Die。** DropLootPiece 未发生本次 normalized 修改。没有把发现的其他旧损坏擅自扩大为本次补丁目标。

ECMA-335 的要求不仅是栈深相等，还包含指令允许的类型、调用签名匹配和控制流合流类型。`ldloca` 压入地址，`ldloc` 压入值；必须从正确构造的 Vector3 局部值取参。`List<Zombie>::Remove(!0)` 的 !0 在所属泛型实例中绑定为 Zombie；不能错误地引用 List<object>。object 改 int32/int64 必须由定义和所有使用共同证明，不能按最终 C++ 错误机械替换。

规范来源：[ECMA-335](https://ecma-international.org/publications-and-standards/standards/ecma-335/)、[Microsoft OpCodes.Ldloc](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc)、[Microsoft OpCodes.Add 类型组合](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.add)。

## 3. 构建、来源与分支证据

链路：`ff088446839f67161045561e4a19e56799a6d039 → fc12b528ea81751655dd2cbc276a9c0f82a37534 → 64b86ed725c40e8e00cecd7a917f72bbed37777e`。两条 Native18 提交线性继承 Native17，没有 merge commit。共新增 13 个 Native18 文件。作者邮箱只是 Git 作者字段；不是签名认证结论。

审查前后 Codespaces HEAD 均为 64b86ed，工作树干净；另一个 `/workspaces/GodsPVZ-iOS` 工作树仍在 b873b8f 的 stage9-native3-codex-recovery。当前分支与另一个工作树分开，但工作树共享 Git 对象数据库，不能将它描述为物理完全隔离。不能仅凭现在 clean 证明历史上从未使用过未提交源文件。

仓库归档 native18-result.json 的 source_commit 是 fc12b52，说明归档报告不是精确提交绑定。但本次下载的同次 CI qualification 报告与 PROVENANCE 是 64b86ed，产物实测哈希与 Codespaces 相同，因此当前候选来源得到更强的 CI 证据补足。

| 输入/输出 | SHA256 |
|---|---|
| PC GameAssembly.dll | 9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d |
| Native17 unlinked | 74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2 |
| Native18 unlinked（Codespaces 与下载的 CI 文件均实测） | f9122641902d0c4a65bf7120c29b64c01cf830506e4b1dd14479ad1fd81226b1 |
| Native18 linked（同次 CI 报告） | 7e04eb13515e204a8638a300cea3b6628d67266ab37be7a59c3e11c9bbe39543 |

Run [36505731509](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36505731509) 精确绑定 64b86ed：qualification 成功、export 成功、build_ipa 失败，打包/验证/上传 IPA 跳过。四个 artifacts 为 qualification evidence、conversion-qualified、Unity evidence、unsigned Xcode。没有 IPA artifact。

旧 Run [36498491610](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/36498491610) 确有 __9.cpp 中 Grid 判空、Vector3、List.Remove、数组下标相关编译错误；新运行显示的阻塞来自 __8.cpp。这个变化与补丁的编译层面进展相符。但新日志没有 __9.cpp 独立成功编译记录/对象文件证据，不能单凭未出现错误证明该单元和 11 方法“100% 正确”。

新日志包含 `fatal error: too many errors emitted, stopping now [-ferror-limit=]`。因此 20 是达到诊断上限，**不是全工程剩余缺陷的总数**。同一组诊断在日志中重复输出，统计必须去重。

README 将 36435550922 也列为 Apple Clang 失败来源不准确；该 Native17 工作流是 qualification/export 成功，未包含 macOS build_ipa job。

## 4. 下一步建议

1. 先修正 Native18 已证实的 F1–F4/F6，并保留错误候选及日志用于对照；不要把这个候选作为语义合格基线直接扩大修复范围。优先补 UI 位移/高度、fZ 非零、普通死亡与灰烬死亡的 ID 跳转表、已死亡/正在死亡/掉落次数等行为 fixtures。
2. 补齐打包和验证实现，预检 workflow 引用的每一个仓库脚本存在；最终 IPA 要核验 ZIP/Payload/*.app、Info.plist、主执行文件和 UnityFramework 的 ARM64 Mach-O、Data 目录与产物来源，不能以后缀或大小代替验收。
3. 对下一批四个目标先形成明确的 MethodBody specification：Device/ICEUIController.Update、Device/ICEUIController.Broken、Project.Start、Project.LoopAddAnimation。从原版 native 逐块还原 Vector3 的每一个分量与 localScale；对 Project 的 List<GameObject>、Transform 引用、递归遍历和 loop index 建立完整 use-def，不要再批量把 object 统一改成 int。
4. 在 Codespaces 扩展为带类型的 CIL 门：地址/值参数、字段宿主、泛型实例、数组元素和下标、比较操作数、所有 branch/switch/EH 目标、合流类型、reachable terminal 验证。加入针对本轮失败类型的负控制。保留现有 Native15–17 fixtures 并新增 Native18/下一批的 native-backed fixtures。
5. 取得与该 Unity/iOS 模块匹配的 libil2cpp headers 后，可先在 Codespaces 运行明确标记为 Linux 的 C++ syntax preflight，使用 `-ferror-limit=0` 扫描所有目标分片以集中收集问题。它不能替代 Apple SDK/arm64 的最终编译、链接与运行验证。不要直接修改 IL2CPP 生成的 C++ 作为最终修复。
6. Apple Clang 的最终门建议保留完整命令、compiler version、生成源文件哈希、对象生成状态、失败日志与 xcresult；以失败也上传的 artifacts 提供证明。稳定后再运行一次完整 Unity → Xcode → IPA 链路。

范围限制：本报告不是设备运行安全证明，也没有逐条证明全部 2317 个方法的游戏语义。已完成精确提交源代码审查、目标/非目标二进制差分、关键 native CFG 反证、Codespaces 独立门验证和 Actions 原始日志/产物核对。对未完全证明的方法明确保留未通过状态，不以编译成功推断游戏还原度。
