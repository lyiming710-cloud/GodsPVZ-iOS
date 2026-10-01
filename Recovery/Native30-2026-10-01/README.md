# Native30：17 个目标方法纯净恢复与零元数据扩充

2026-10-01。目标是从高保真原版恢复至最终未签名 iOS IPA。当前分支 `repair/codex-native24-codespace`，工作区 `/workspaces/GodsPVZ-native24-codespace`，基于 Native29 提交 `6dc2d8f985fd27423d77a550ea57e645470a5559`。此阶段成果为候选二进制恢复，尚不能代替已通过的完整游戏验收或未签名 IPA 目标。

## 关键指标与固定依据

- **基线输入**：Native29 接受候选 `trial2.dll`（SHA-256 `4572e7e2d121f58e84af65c78056a42b5e919b06275d8915024401e3b4256fe2`）。
- **生成候选**：Native30 接受候选 `native30-final1.dll`（SHA-256 `85df0b31762b235e4bd8e83f072499777ba865d6fb196bbb06f8c4b52d6e5e2b`）。
- **两次构建确定性**：`native30-final1.dll` 与 `native30-final2.dll` 逐字节一致。
- **原版 GameAssembly.dll 固定依据**：SHA-256 `9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`。

## 验证与门禁全绿结果

1. **类型检查与元数据独立审计**：
   - 17 个目标方法在保留 slot 处写入，局部变量签名、InitLocals 及异常处理区保持合法。
   - 2,280 个非目标方法体逐字节完全一致（0 漂移）。
   - 全部 26 个元数据表行数与宽度严格一致，各堆大小与内容完全一致，原始节及 RVA 无漂移。
   - 旧文件修改字节数仅 775 字节，新增附加字节数 0 字节（零元数据扩充）。
   - 负控制测试：针对非目标方法体突变与老表行突变均触发 exit code 2 阻断。
2. **IL2CPP 与 Clang C++ 语法验证**：
   - 全部 264 个 Translation Units (TUs) 完成语法检查，无任何编译衰退（`pass_to_fail_methods: []`）。
   - 17 个目标方法在 C++ 层面全部达到 0 语法诊断错误（从之前的 21 个错误降为 0）。
   - 总语法错误数由 5,941 降至 5,920（减少 21 个）。
   - 2,250 个映射非目标 C++ 方法完全一致（0 漂移）。
   - `global_metadata_equal_parent: true`。
3. **外层 CLR 运行时夹具**：
   - 覆盖 48 个方法（31 个继承 + 17 个新增）。
   - 1,958 个正向用例全部通过（0 失败）。
   - 61 个负控制变异全部捕获（0 工具错误）。
   - 故障注入控制（`fault_emit`, `fault_invoke`）均以 exit code 2 闭合失败，`gate_pass: true`。
   - 回归测试：继承自 Native24..29 的各历史套件均通过。

## 修复方法要点

- **SunManager::SunPointTextUpdate**：修复使用实例字段地址（`ldflda` 0x7C）替代之前的静态地址（0x7F），平衡计算栈。
- **FTRuntime.Internal.SwfUtils::UnpackUV**：严格还原无符号右移（`shr.un` 0x64）与浮点间接存储（`stind.r4` 0x56），实现与原版一致的高精度 UV 解码。
- **Project::Rotating**：严格还原原生 x86-64 `xor r9d, r9d`，在 CLR 中对应 `Space.World` (0) 旋转。
- **SunManager::SunFall**：还原太阳下落生成逻辑与随机列坐标分配。
- **Board::ButtonDown_Menu / Popup / Window_Q**：还原 UI 窗口与弹窗实例化、层级及父节点引用链。
- **Zombie (TeleportTo, GetElementPoint, TryShooting)**：严格还原瞬移坐标赋值、属性加成点数查询与射击冷却判定。
- **Audio (PlayWinAudio, AudioPlay)**：还原相机坐标获取与统一音效点播放。

## 归档与复现

```bash
python replay_native30.py native30-codespace-2026-10-01-evidence.zip <new_directory>
```
运行后将解压并验证清单中的全部哈希，独立编译 Patcher、Audit、RuntimeFixture，复现确定性候选二进制并跑通 1,958/61 夹具用例。
