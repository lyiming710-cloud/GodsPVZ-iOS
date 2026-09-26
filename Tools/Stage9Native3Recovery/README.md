# Stage9.1 native3 四方法恢复记录

本轮从 2026-09-26 Drive 交接及对话最后状态继续。所有原始 ZIP、原始 native 文件和 `18e44` 输入均保持不变。生产分支不参与本轮。

## 输入与范围

- 唯一 managed 输入：`18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd`。
- PC ZIP：`2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`。
- GameAssembly：`9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`。
- metadata：`ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`。
- 远端读取核实：audit `e40bdb0bc54dd5fc6e48d919387376f3e62e4c58`；production `91a8adacb42414e6bce8b4df8b8811be9e488dab`。
- 原定三个方法加上用户本轮明确批准的 `0x060001A5 ZombieSelect::.ctor(ZombieInfo)`，共四个 MethodBody。
- `59bb` 仅用于追溯历史 24 项 MemberRef 的语义身份；不是 candidate 输入。`ff0e46` 不参与生成。

## 本轮发现与恢复

| MethodDef | PC Native 范围（末端不含） | 恢复内容 |
|---|---|---|
| 060001E0 | 18031EE10–18031F2B3 | onBoard 两条路径、Unity 对象判断、gameObject.activeSelf、仅场内且 plantID=0 的 CanAttacked、四项碰撞界限、最后命中结果、两段 finally |
| 060001B2 | 180316AB0–1803170CF | ZombieInfo 枚举与构造、两个 owner 字段、BoardEntry 倍率枚举、整型截断、按 zombieTypes 标记、删除未选项、两段 finally |
| 06000248 | 18033FDE0–180340013 | challengeType 完整分支、可选且已选词条计数、hard 最大星数和通过标记、rescue 计数、adventure 上限、finally |
| 060001A5 | 180325270–180325351 | 标量字段初始化、elite OR boss、固定 10 项 bool 数组深复制、保留 null/越界异常 |

新增第四个方法的原因：CreateEnemySelecter 的原版内联了该构造方法。18e44 构造方法把 bool[] 存入 ZombieSelect local，缺失有效数组复制写入，并包含与原版 225 字节机器码不相符的数学片段。仅调用旧 ctor 不能完成保真恢复。用户已明确批准扩展范围。

GetZombie 的返回值是列表遍历中最后一个命中对象；没有证据支持把它描述成按深度排序的“最上层”。浮点过滤使用有序比较，保留原版 COMISS/JA 对 NaN 的行为。onBoard=false 不调用 CanAttacked。对 board/list 等的 null 解引用继续抛异常；只有原版明确的 Unity 对象存在性分支允许正常返回 null。

PassLevel 的 Adventure 分支在 adventureLevel <= 180 时自增，因此 180 会变成 181；没有擅自改成 180 封顶。HardAdventure 只统计 `!must && select`。其余 challengeType 直接返回。

## 规格、工具与验证

- `specification/*.spec.il`：每条 CIL、具体 locals、EH 半开区间、native 块对应关系；无占位指令。
- `SpecBuilder.cs`：先在内存生成规格；第三个参数显式指定 candidate 输出时才写 DLL。输入哈希硬锁。
- `extract_native.py` / `map_native.py`：从原版 ZIP 提取与核验，依 .pdata 定界，解析 metadata 字段和 CodeGenModule 方法指针；输出可重跑证据。
- `verify_stack.py`：针对目标方法使用的指令集进行类型化 CFG 数据流检查，遇到未知 opcode 失败；检查中间栈、合流、字段接收者、调用参数、值/引用/地址类型和 EH。不是宣称验证整程序集所有 ECMA-335 特性。
- `Gate.cs`：Cecil 回读、2317 MethodDef/2802 FieldDef/320 TypeDef、身份与非目标方法差异、24/24 targets、23/23 Resolve keys、3/3 transform sites、orphan=0，以及目标字段和方法引用 Resolve。
- `verify_spec_readback.py`：实际 candidate 与规格的逐指令等价检查，以及完整方法引用使用增减清单锁定。
- `Harness.cs` / `Rehost.cs`：把 candidate 的实际四个 MethodBody 复制到具有最小类型依赖的 .NET Framework 测试程序集，执行 31 项行为/异常检查。Unity 对象部分使用测试替身，因此不代表真实 Unity 或设备验收。
- 栈检查负例：缺失 stfld objref、错误字段 declaring type、枚举器值/引用错配、Vector3 值/地址错配，四种均被拒绝。
- `build-and-verify.ps1`：完整本地/Windows CI 静态门；每个子程序非零退出均终止。

候选文件：`candidate/Assembly-CSharp-native3-four-method.dll`。

SHA256：`72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc`。

本地静态门通过，四方法实际 DLL 回读栈检查通过，31 项隔离 CLR 测试通过。原 `GetPlantUnderMouse` 和 `Start_BoardEntry` 语义不变；所有非目标方法语义差异为 0。Cecil 会重排引用 token，故这里使用 token-independent semantic comparison，不把原始字节相同作为结论。

Unity export 仍须独立门禁。只有 Export、Audit Xcode 两步成功且上传 Xcode 工件，才能继续 macOS IPA。若出现新 blocker，记录实际错误并停止此次 Unity 推进，不扩大批量修复范围。
