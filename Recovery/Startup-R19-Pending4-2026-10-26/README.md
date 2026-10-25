# R19 再补四个完整调用方

游戏 DLL 保持固定 R19 candidate9 和当前 R20 authored3 的原字节。两个候选均实际发射并执行完整 CIL，794 条记录逐项相同；其中790条正常记录与完整原版 PC 调用方一致，四条 CLR-only 异常单列。冻结 Rehost、类型及成员审核器未改。

| Token | 方法 | 原版正常记录 |
|---|---|---:|
| 0x060001C6 | MouseManager.Update_SkillOnHand | 216 |
| 0x0600031A | Device.DC_PortalSummon | 270 |
| 0x06000686 | PrepareUIController.SeedChoose | 16 |
| 0x06000584 | Almanac_PlantDetail.SetSkillUpPreview | 288 |

检查技能所有者重新读取、Zombie/Device 高亮分支、Damage 的 -1 参数；传送门在随机回调前捕获列表并在回调后重读 board/manager/坐标，伪空判断、Hide 与 Buff 参数；教程数组写回、界面激活和最后 -1 预选索引；技能预览状态、资源/按钮回调后的字段重载、销毁与真实 List.Clear 结果。原版调用方指令和方法指针在安装叶函数桩之后逐字节检查，明确保留外部尾调用边界。

四个物理单点故障全部产生运行差异：Damage 参数 -1 改0（48条）、Hide false改true（18条）、预选索引 -1改0（16条）、漏 Clear（72条）。各故障进程正常退出，其他方法无差异。完整变体可从当前候选与增量文件重建并按SHA核验。

游戏查询/伤害/RNG/生成/Buff、Unity 生命周期与组件/UI操作、资源构造与库存判断等叶函数是受控契约，未证明其内部或实际引擎行为。没有 Windows SEH、Unity渲染或普遍等价结论。R19有限 profile 从70增至74，原版正常记录从18,938增至19,728，26个profile仍待完成；这些数量不是剩余bug数。

没有新增游戏改动，未重复跑候选字节未变的编译流程，没有新持证Editor导出、链接、IPA或iPad验收。后续新代码仍累计超过100处再集中检测。
