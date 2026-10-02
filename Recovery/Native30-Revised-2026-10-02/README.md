# Native30 修订资格与复核报告 (2026-10-02)

## 一、固定输入与候选基准

- **父批次提交 (Native29)**: `6dc2d8f985fd27423d77a550ea57e645470a5559`
- **原版 PC GameAssembly.dll SHA-256**: `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`
- **Native29 输入 DLL SHA-256**: `4572e7e2d121f58e84af65c78056a42b5e919b06275d8915024401e3b4256fe2`
- **Native30 历史候选 (存在已证实缺陷，撤回)**: `85df0b31762b235e4bd8e83f072499777ba865d6fb196bbb06f8c4b52d6e5e2b`
- **Native30 修订候选 SHA-256**: `b88946ca8e7aab593a802de406292fb0ac89ed4121f2e1dbccfcfedda3908516`
- **修订分支**: `repair/native30-revision-2026-10-02`
- **证据包**: `.validation/native30-revised-2026-10-02/native30-revised-evidence.zip`
  - SHA-256: `526f6bfd14293ac4ff8a2ac0356a52313e7642cb65572d5fb67a5b1f9e6a6ae4`

---

## 二、针对已确认缺陷的纠偏说明

### 1. `Prop.DropDown` (MethodDef `0x06000401`) 原版 Unity 判空分支恢复
- **原版反汇编证据**:
  - 原版 PC VA: `0x18037E320` - `0x18037E400` (SHA-256: `8fe281ca9a855d42b78f4ee090ce0ecfe011d7eb01bb6d5a7077d4a196ab8eb0`)
  - 在 `0x18037E3B7` 调用 `0x18131F900` (`UnityEngine.Object::op_Inequality(propImagex, null)`)。
  - 在 `0x18037E3BC` 测试 `%al`，仅当为 true 时调用 `0x18131E940` (`UnityEngine.Object::Destroy(propImagex)`)。
- **历史缺陷分析**:
  - 历史补丁使用普通 CLR 引用判空 (`dup; brfalse.s skip; ...`) 代替 Unity 运算符。
  - 当 Unity 对象在 C++ 原生层已销毁（`Alive == false`），但托管引用仍未被 GC 时，普通引用判空仍然为 true，导致错误调用了 `Destroy`。
- **修复方案**:
  - 引用已存在的 MemberRef token `0x0A00000C` (`System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)`)。
  - CIL 发射序列：
    `ldarg.0; ldfld propImagex; ldnull; call 0x0A00000C; brfalse.s bank; ldarg.0; ldfld propImagex; call Destroy; bank: ldarg.0; callvirt PropBankStateSet; ret;`
  - 体积：61 字节（完全容纳在原版 71 字节预留槽内，无需扩展节区或元数据堆）。
- **测试覆盖与对抗门禁**:
  - 覆盖托管引用为 `null`、存活 Unity 对象 (`Alive == true`)、已销毁 Unity 对象 (`Alive == false`) 及 null-this 四种状态。
  - 增加 `plain_null_check` 行为变体负控制，并直接测试历史旧候选，证明缺少 Unity 判空的候选会被严格拒绝（exit 2, gate_pass=false）。

### 2. `Project.Rotating` (MethodDef `0x060003C9`) 行为验证盲区消除
- **历史缺陷分析**:
  - 原有测试替身 `Transform.Rotate(axis, angle, space)` 仅记录了 `angle` 与 `space`，完全忽略了 `axis` 参数。
  - 独立审查已证明将 `Vector3.get_forward()` 替换为 `Vector3.get_zero()` 的错误 DLL 仍能通过原有夹具。
- **修复方案**:
  - 强化测试替身，记录旋转轴的全部分量：
    `rotate:{BitConverter.SingleToInt32Bits(axis.x)}:{BitConverter.SingleToInt32Bits(axis.y)}:{BitConverter.SingleToInt32Bits(axis.z)}:{BitConverter.SingleToInt32Bits(angle)}:{space}`。
  - 正向用例断言包含前向轴 `(0f, 0f, 1f)`。
  - 在行为夹具中新增 `wrong_axis` 突变负控制。
  - 生成独立的实际合法 CIL 错误轴 DLL (`wrong-axis-dll.dll`)，重新执行夹具，证实其被行为测试明确拒绝（exit 2, positive_failures=1, gate_pass=false）。

---

## 三、Native30 全部 17 个方法的真实反汇编契约

| Token | 方法签名 | 原版 PC VA 范围 | 原版调用顺序与状态写入 | 异常与边界条件 |
| :--- | :--- | :--- | :--- | :--- |
| `0x060000C1` | `ProjectAnimationEvent::PlayWinAudio()` | `0x180307150 - 0x18030727e` | `Camera.main` -> `get_transform()` -> `get_position()`; 读 `Board/Lawn.audioVolume`; 调用 `Global.CreateAudioAtPoint(winAudioClip, pos, vol)` | `Camera.main` 或 `transform` 为空时抛 NRE |
| `0x060001C7` | `MouseManager::AudioPlay(int)` | `0x18031d390 - 0x18031d4f2` | 根据 `id` (0..4) 索引对应音效 clip 字段; 读取相机位置与音量; 调用 `Global.CreateAudioAtPoint` | 超出 0..4 范围跳过播放，不抛异常 |
| `0x060002B4` | `Board::ButtonDown_Menu()` | `0x180325ce0 - 0x180325e1a` | 判空 `this.gameUI_down`; 加载 `Window_Menu` 资源; 在 `Canvas_Font` 下实例化; 赋值到 `this.window_Menu` | `gameUI_down` 为空时抛 NRE |
| `0x06000480` | `Zombie::TeleportTo(Vector3, float, bool)` | `0x180369f40 - 0x180369fb8` | `get_transform()` -> `set_position(pos)`; 写入 `position_x` (0x30) 与 `position_y` (0x34); 若 `isAnimate` 为 true 调用动画触发函数 | `transform` 为空时抛 NRE |
| `0x0600001F` | `Admin_system2_boardEdior::Return()` | `0x1802fce10 - 0x1802fcebc` | 重置关卡编辑器 UI 与编辑标志; 清理棋盘状态; 执行退出回调委托 | 回调为空时不调用 |
| `0x06000060` | `Path_enemyEditor::LoadData()` | `0x180306260 - 0x1803062fd` | 遍历路径节点; 调用 `BoardConfig.GetGridCenterPosition(x, y)` 计算路标; 写入路径数据结构 | 无 |
| `0x060003C9` | `Project::Rotating()` | `0x180378040 - 0x1803780ca` | 读取 `this.projectType` (0x20)，非 0 直接返回; 为 0 时 `get_transform()` -> `Rotate(Vector3.forward, -18.0f * deltaTime, Space.Self)` | `transform` 为空时抛 NRE |
| `0x0600026A` | `SunManager::SunFall()` | `0x180340c60 - 0x180340cfb` | 判空 `this.board` 与 `projectManager`; 调用 `CreateProject(0, 0, Vector3.zero)`; 调用 `SetNatureSunInitialPostion()`; `sunFallCount++` | **无间隔、冷却或计时检查**; 对象为空抛 NRE |
| `0x0600026B` | `SunManager::SunPointTextUpdate()` | `0x180340d00 - 0x180340d3c` | 判空 `this.sunPointText`; 将 `this.sunPoint` 转换为字符串; 调用 `TMP_Text.set_text` | `sunPointText` 为空时抛 NRE |
| `0x06000448` | `Zombie::GetElementPoint(ElementType)` | `0x180361060 - 0x180361095` | 判空 `this.elementManager`; 调用 `ElementManager.GetElement(type)`; 若为 null 返回 0.0f; 若非 null 读取 `element.point` (float 0x24) 并返回 | **非数组索引，无边界检查**; `elementManager` 为空抛 NRE |
| `0x06000488` | `Zombie::TryShooting()` | `0x18036ad70 - 0x18036adbe` | 判空 `this.animator`; 调用 `Animator.SetTrigger("shooting")`; 返回常量 `true` | **无冷却/射程检查或创建子弹逻辑**; `animator` 为空抛 NRE |
| `0x060006E9` | `WarehouseUIController::CheckSupplies(Supplies)` | `0x1803adfc0 - 0x1803ae055` | 判空 `this.suppliesInfoPage`; 写入 `supplies`; 若非 null 调用 `supplies.GetSuppliesInfo()` 存入 `suppliesInfo`; 若 null 存 null; 调用 `SetInfo()` | **无解锁/数量/等级计算**; `suppliesInfoPage` 为空抛 NRE |
| `0x06000675` | `Popup::PopupPopup(int)` | `0x1803a5de0 - 0x1803a5e8a` | 判空 `ResourceManager.prefab_Popup`; 实例化预制体; 写入 `popup.P = id`; 返回实例 | 预制体为空抛 NRE |
| `0x06000673` | `Popup::PopupPopup(int, int)` | `0x1803a6060 - 0x1803a6119` | 判空 `ResourceManager.prefab_Popup`; 实例化预制体; 写入 `popup.P = id`; 写入 `popup.info0_int = info0`; 返回实例 | 预制体为空抛 NRE |
| `0x06000704` | `Window_Q::PopupNewWindow(int, Transform, int)` | `0x1803afe60 - 0x1803afe85` | 调用 `Window_Q.Popup(type)`; 写入 `window.info0_int = info0`; 返回实例 | 窗口为空抛 NRE |
| `0x060008F2` | `SwfUtils::UnpackUV(uint, ref float, ref float)` | `0x1803ca720 - 0x1803ca760` | `*u = (float)(pack >> 16) / 65535.0f`; `*v = (float)(pack & 0xFFFF) / 65535.0f` | 无 |
| `0x06000401` | `Prop::DropDown()` | `0x18037e320 - 0x18037e400` | 判空 `this.prop` 与 `transform`; `set_localPosition(originalPosition)`; `propState = 1`; 若 `propImagex != null` (Unity 运算符) 调用 `Destroy`; 调用 `PropBankStateSet()` | **无下落协程/重力加速度/着地循环**; 对象为空抛 NRE |

---

## 四、验证度量口径与测试结果严格区分

### 1. 正向用例区分
- **继承正向用例 (来自 Native24..29)**: 1,915 个
- **本批新增正向用例 (Native30 17 方法)**: 45 个 (包含参数与边界状态组合)
- **累计执行用例数**: 1,960 个
- **执行结果**: 全部通过 (failed = 0)

### 2. 负控制区分
- **行为不匹配负控制 (BEHAVIOR_MISMATCH)**: 60 个（修改偏移、状态写入、调用参数、向量分量、枚举分发、回调丢弃等）
- **CLR 拒绝负控制 (CLR_REJECTED)**: 3 个（非法评估栈深 `illegal_stack`，由 CLR 运行期类型验证器拒绝）
- **累计负控制数**: 63 个
- **检出率**: 63 / 63 (100%) 全部成功检出，工具错误 0

### 3. 独立实体对抗变体 (Actual DLL Mutants)
- **实际错误轴 DLL (`wrong-axis-dll.dll`)**: 运行时夹具 exit 2, positive_failures = 1, gate_pass = false (成功检出)
- **历史缺陷候选 (`native30-final1.dll`)**: 运行时夹具 exit 2, positive_failures = 1, gate_pass = false (成功检出)

### 4. 工具故障注入与控制 (Fault Injection Controls)
- **发射故障 (`fault_emit`)**: exit 2, tool_errors = 1, gate_pass = false (正确非零退出且标记失败)
- **调用故障 (`fault_invoke`)**: exit 2, tool_errors = 1, gate_pass = false (正确非零退出且标记失败)

### 5. 编译预检查与平台界限
- **编译器**: Ubuntu clang version 18.1.3 (1ubuntu1)
- **性质**: Linux syntax-only 语法预检查，仅用于在受限 CI 环境下验证 C++ 生成有效性与方法级诊断改善，**绝对不代表 Apple Xcode 编译、链接或 iOS 真机运行**。
- **度量变化**:
  - 总诊断数: 5,941 -> 5,920 (-21)
  - 17 个目标方法诊断数: 全部由原报错降为 0
  - 剩余有编译诊断的方法: 599 个
  - 失败编译单元 (TUs): 24 / 264
  - 非目标方法 C++ 代码漂移: 0 (2,250 个非目标映射方法体完全一致)

---

## 五、未覆盖行为与未完成验证清单

1. **未完成真实设备/引擎验证**:
   - 依赖轻量 Helper Doubles 模拟引擎交互，未在完整 Unity 运行时或 iOS 设备上运行实际游戏循环。
2. **全程序集剩余编译错误**:
   - 程序集内仍有 599 个未恢复方法存在 5,920 条 C++ 编译诊断，分布在 24 个 translation units。
3. **未签名 IPA 目标尚未达成**:
   - 必须在完成全量或核心可运行方法闭环后，经由官方 Unity 导出与 Xcode 构建工作流生成，本轮不越级宣称。

---

## 六、可移植独立复现方法

在任何安装了 .NET 10.0 与 Python 3 的干净环境中执行：
```bash
python3 scripts/takeover/native30/replay_native30.py \
  .validation/native30-revised-2026-10-02/native30-revised-evidence.zip \
  /tmp/native30-replay-verify
```
该命令将：
1. 验证证据包文件完整性与 SHA-256；
2. 从头编译补丁器、审计器与运行时夹具；
3. 从 Native29 父 DLL 重新生成修订版候选，确认逐字节一致 (`b88946ca8e7aab593a802de406292fb0ac89ed4121f2e1dbccfcfedda3908516`)；
4. 运行隔离审计，确认 2,280 个非目标方法及既有元数据完全不受影响；
5. 执行正向用例、两项故障注入、实际错误轴 DLL 变体及旧缺陷候选拒绝测试；
6. 输出 `ARCHIVE-REPLAY.json` 判定报告。
