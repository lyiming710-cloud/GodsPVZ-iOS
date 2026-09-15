# GodsPVZ 1.0.2 高保真恢复 / iOS 移植 — CURRENT HANDOFF

更新时间：2026-09-15（UTC+8）

仓库：`lyiming710-cloud/GodsPVZ-iOS`

正式分支：`high-fidelity`

阶段：`Stage9.1 Unity runtime integration / startup-path recovery`

本文件是仓库内当前交接入口。旧状态文档保留用于审计，但不得替代本文件判断当前主线。

## 1. 核心目标与恢复纪律

目标是把 GodsPVZ 1.0.2 尽可能高保真地恢复为可维护 Unity 工程，再做必要 iOS 适配并最终导出 unsigned IPA。不是重做“类似玩法”。

Gameplay authority 顺序：

1. 原始 PC x86-64 IL2CPP native；
2. Android IL2CPP 交叉验证；
3. metadata / assets / JSON / Unity serialized data；
4. Cpp2IL / ILSpy 只作 managed attribution/reconstruction aid。

标准闭环：

`runtime promotion -> token/RID/RVA/fingerprint -> PC native attribution -> exactly one MethodDef -> semantic/field isolation -> exact-reference readback -> exactly one expensive exact-R3 runtime`

禁止：publicize private fields、swallow exception、无依据 null guard、mass gameplay rewrite、`System.Private.CoreLib` 污染、显式 BCL exception ctor 模拟 CLR 失败。

Unity：`2022.3.44f1c1`

Assembly-CSharp：`2317` MethodDefs / `2802` fields。

Sealed baseline 仍为 `HF55`。不要提前 seal HF56。

## 2. 当前正式输入与 Zombie 主线

当前 runtime-qualified SkillProgress v2 input：

`2b2fdf165076ff9f48c577b84fb4c62346b78c925e46c6bff5f8ec6a79f67ce5`

来源 workflow run：`34922788006`

artifact：`Stage9.1-4736-SKILLPROGRESS-V2-QUALIFIED`

artifact ID：`10377894443`

### Zombie::.ctor

- token `0x060004BB`
- RID `1211`
- PC pointer-table entry `0x181B85330`
- native `0x1803741D0–0x1803744DD`
- native SHA256 `84d27db79e7cf21970718ac79287badd239f62af082e008e02fea7e6cef88cb0`

旧 managed body 的 runtime blocker：`InvalidProgramException ... Zombie:.ctor ... IL_0055: stfld`，本质是把 native-sized integer 写入 `UnityEngine.Color`，同时还有 fake `Vector3.zeroVector` / unmanaged-memory scaffolding。

PC ctor authority：`attackPoint=100`、`isStant=true`、`isOnBoard=true`、`camp=2`、ElementManager、`waitingTime=3`、两个 `List<EnemyPath>`、三个 zero Vector3、`List<GameObject>`、white Color、两项 brightness=1、HPUIController_Zombie、`List<ElementUIController>`、BuffManager、`updateRate=1`、MonoBehaviour base ctor。

Zombie recovery workflow：`34925548959`

qualify job `104242716395`：SUCCESS。

候选：

`68a105d366f0e7bfa9215c6e09040b9275bcad7e6c2771fec745340eb8e08d97`

qualified artifact ID：`10380151617`

Cheap/static/exact-ref 已确认：只改 `0x060004BB`；其它 `2316` MethodDefs 保持；`2802` fields metadata 保持；4 个 `List<T>` ctor resolve 到 exact Unity Mono mscorlib；无 CoreLib pollution、zeroVector、unmanaged scaffolding、object/intptr damaged locals。

### Zombie runtime 的精确边界

runtime job `104242861397` 最终是 failure，但失败点是 targeted reflection harness 没产生 `CTOR_METHOD / INVOKE / VERIFY` marker，不是 Zombie candidate 再次抛异常。

同一次 exact-R3 natural prefab import 对 `68a105...`：

- `Zombie::.ctor InvalidProgramException` = `0`
- `System.Private.CoreLib` = `0`
- `zeroVector` = `0`
- 已恢复 SkillProgress / PlantData / Camera / Plant_DetaiPage / Dialogue / Grid / CreateMap / LoadData / LoadSkillLogo / Projectile / Board.GameContinue / BGMUnPasue 回归异常 = `0`

旧 `2b2f...` natural import 中 Zombie ctor 同一 `IL_0055` 曾出现约 26 次；在 `68a105...` 中已经消失。

因此当前定义：

`68a105...` = cheap-qualified + exact-ref-qualified + natural exact-R3 cleared Zombie ctor；**尚不能写成 direct-targeted-runtime-qualified**。

runtime artifact：`Stage9.1-ZOMBIE-CTOR-RUNTIME`

artifact ID：`10379374535`

digest：`sha256:84577a2c0b3eb9a7aaa7fa4226cdf0dff3ced50014494f65062c0dc3f0bc6f28`

## 3. Natural runtime 已推进出的 blocker 顺序

`68a105...` natural import 里：

1. `ZombieInfo::.ctor` — Invalid IL `IL_0049: stfld`，6 次；
2. `Device::.ctor` — private `HPUIController:maxHPEffect` FieldAccess，13 次；
3. `SuppliesInitialValue::.ctor` — private `List<T>._version` FieldAccess，2 次。

按 first-causal runtime order，完成 Zombie natural-log re-audit 后应先处理 `ZombieInfo::.ctor`，不能因为 Device 数量更多就抢先修 Device。

## 4. ZombieInfo::.ctor 已准备好

- token `0x06000152`
- RID `338`
- managed fingerprint `4050d7219be77f3a38d71ee97ce8ac4f26b3619c90fdc638aab92cb0eda36769`
- PC pointer entry `0x181B837E8`
- native `0x1803251F0–0x180325261`
- native SHA256 `4ccabbc3bb2ebdc0f06c97337a2473d0855c91c94c5c753635bf8e167ed90c56`

PC semantics：`healthPoint=270f`、`attackPoint=100f`、`speedRating=3`、`new bool[10]`、`painterID=-1`、Object base ctor。

坏 IL 把 `painterID=-1` 生成为 `ldc.i8 4294967295 -> stfld int32`，并用 fake Method-not-found 取代 base ctor。

Evidence：`Tools/Stage9NativeProbe/ZombieInfo.ctor.native-evidence.md`

Dormant patcher：

- `Tools/Stage9ZombieInfoCtorPatch/Stage9ZombieInfoCtorPatch.csproj`
- `Tools/Stage9ZombieInfoCtorPatch/Program.cs`

最近准备 commit：`a28409d1d95570c61b7e9c626651f58e1174abad`

不要在 Zombie natural runtime qualification 前把它直接接到 formal mutation mainline。

## 5. Device::.ctor authority 已完成

当前异常：`HPUIController:maxHPEffect` private FieldAccess。

关键结论：nested `Device.HPUIController::.ctor` 自己已经初始化 `maxHPEffect=1f`。PC native 的外层 write 是 ctor inline/展开结果；Cpp2IL 错误地同时保留 `new HPUIController()` 和额外 private stfld。

正确 CLR adaptation：保留 nested ctor，删除冗余非法外层 stfld。绝对不要把字段改 public。

Evidence：`Tools/Stage9NativeProbe/Device.ctor.native-evidence.md`

commit：`70e04181039e0424837c1bb70248524fe74125c9`

## 6. BuffManager generic refs 已排除为当前风险

`BuffManager::.ctor` token `0x06000108`, RID `264`。

preflight run `34925893247` 已成功：3 个现有 `List<Buff>..ctor()` refs 全部 `Resolve()` 到 exact Unity Mono `unityjit-linux/mscorlib.dll`。

artifact ID：`10380132132`

所以当前没有证据表明 BuffManager generic MemberRef 是 Zombie ctor 后续 blocker。

## 7. 下一步（不要重跑无意义 Unity）

1. 对 artifact `10379374535` 做纯日志 re-audit，不启动 Unity：证明 old natural Zombie ctor count > 0，而 `68a105...` count = 0，并检查 CoreLib/zeroVector/历史恢复项 regression=0。
2. 用该现成 exact-R3 evidence 正式完成 Zombie natural-runtime qualification。
3. 然后 promotion `ZombieInfo::.ctor`。
4. 以 `68a105...` 为正式输入，仅 patch token `0x06000152`；static/field/exact-ref 全过后只跑一次 expensive exact-R3。
5. runtime 后按第一条真实 causal exception 决定下一 MethodDef。当前 evidence 倾向 Device，但不能预先假定。

## 8. 关键基础设施

PC source ZIP SHA256：`2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48`

PC GameAssembly SHA256：`9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d`

metadata SHA256：`ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9`

method pointer table：`0x181B82D60`

Exact ILSpy：run `34173085137`, artifact `10036340376`

Compact Unity refs：run `34804463514`, artifact `10332298345`

Exact editor：run `34668863583`, 15 parts, concat SHA `0008115c785784baddb19e2b13b384ac845438239f293efb0c2e8b1379a1fe14`

Exact R3：run `34747460206`, archive SHA `d4264f12a00e86b6149d20ecf04caa9761af510aece6107bcceb155ea4ab0bbc`

Original R3 Assembly-CSharp SHA：`dc205a40dc2478b3aacbb3a7d6bb1ca96ffb0a964648d34062b4ddc75f3b3655`

Exact Unity Mono mscorlib SHA：`4d1725f1ae54a22f69b79c2b1569181ed6653dd484c35005e29c40300c6673be`

Google Drive `PVZ GOD` folder ID：`1mvXIyCNmnBbPydoKmC5JVptjfdC0tEab`

PC ZIP Drive ID：`1_5kVGTFmsvwcdRLQ2ah_rtyC6DqvLnf0`

Android APK Drive ID：`1ZLNYiTs1G1rOIVWsCr37tE9lobWg6X4f`

## 9. 交接给下一 AI 的一句指令

不要重新从头分析，也不要先问用户是否继续；先用现有 Zombie runtime artifact 做 zero-cost natural-log qualification，然后按 runtime 顺序修 `ZombieInfo::.ctor`，每次只恢复一个 MethodDef，并维持 native authority、semantic isolation、field metadata preservation、exact-reference readback 和最少 exact-R3 runs。