# R19 两个已有方法的浮点差异：保留失败，尚未修补

对固定 R19 candidate9 与 R20 authored3 实际发射并执行完整 CIL，2,814 条 CLR 记录完全相同。运行两个未改比较和条件跳转的完整原版 PC 调用方，共发现 228 条差异（2586 条一致），原生受控回调错误 0，子进程正常写出失败报告后退出 2。冻结 Rehost 不变，未修改游戏 DLL。这里是已有百方法批次的补充验证；后续新修复仍待累计超过 100 个有证据的位置后集中检测。

- 0x060005E8 BuffUIController_Zombie.SetStatsImageSet(int,float)：126 个实参记录中 12 个 NaN 记录不同。原版调用 SetStatsImageSet(key,3,+1)，当前 CIL 调用 (key,3,-1)。该 float 参数的真实取值域没有得到缩窄证明，不能判该完整方法通过。
- 0x060000C3 VFXAnimationEvent.Update：2304 个非负有限 delta 的组合均一致，另 384 个扩展 delta（NaN/Infinity）输入中 216 个不同。例如 delta=NaN、duration=.1 时，当前 CIL 会提前 Close，原版不会。未证实真实 Unity deltaTime 会返回这些输入，不能将其作为当前 iPad 黑屏的已证原因。

原版四处 COMISS+JAE 对 unordered 为 false；当前三条指令 clt; ldc.i4.0; ceq 对 unordered 为 true。逐站点依据与拟改 clt.un 的四个位置在 PROPOSED-FOUR-SITES.json 中，尚未写入候选。不得全局替换 clt/cgt 或无依据改整数比较。VFX 原版先捕获 duration 再取 delta、当前 CIL 先 getter 再重读字段这一顺序差异另保留观察，不凭人为有副作用的 Time getter 宣称实际引擎缺陷。

测试覆盖 ±NaN、±0、相邻阈值、Infinity，保持原生 XMM 浮点返回协议并单独核对 NaN payload 7FC07654。Unity 对象伪空、时间/图标/Close 等是明确受控叶函数，其内部未被证明；不是实际场景、真机或全游戏验收。未删指令、EH、分支或私有访问规则来取得通过。全部输入、完整 CLR 与原生行、源字节、stderr/stdout 和退出码均归档。

R19 已有有限对照联合数仍为75、正常原版记录21,456，另25个方法仍待验收，其中这两个已有差异。不会把本次2586条相等记录追加到已通过方法总数。R20 本轮8个新方法的原有768条对照范围不变；当前 iOS 构建仍只用于启动诊断和设备反馈，不能标整个游戏完成。
