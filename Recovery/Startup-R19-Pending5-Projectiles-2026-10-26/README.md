# R19 五个投射物初始化调用方：原生 ABI / 完整 CIL 对照

实际发射完整目标 CIL，并运行未改完整原版调用方：0x060000BA PlantAnimationEvent.PC_SW_IcicleCircle、0x0600037E Plant.PC_CBKillZombie、0x060003AE Plant.ST_Snowpeawitch、0x0600049B Zombie.ZC_LadderSaboteursPlaceC4、0x060004A2 Zombie.ZC_PoleCommanderThrowC4。

5,316条正常记录0差异，逐方法1,728/1,152/324/1,920/192条。两个固定候选 R19 candidate9 / R20 authored3 的5,344条完整CLR记录相同；28个空board/manager/skill的CLR异常另列，不宣称Windows原生异常展开已支持。冻结Rehost、游戏DLL及原生调用方比较/分支不变。

覆盖Unity对象伪空、返回投射物伪空/实际空、技能ID分支、正负零/最小非零量/极大有限向量、C4的Int32溢出边界、不同Camp值，并在Create/Initial/GetDamage/Skill.Function回调后替换阵营、动画事件plant接收者与skill，实际比较后续字段重读和调用顺序。原生Vector3隐藏返回缓冲区、generic Initial的三个Vector3/float/int/owner/MethodInfo栈参数均按Windows x64协议执行。原版元数据中的四个字符串字面量被实际物化，未用猜测名替代。

五个物理单站点错误（冰锥X偏移、吞噬爆弹时长、雪豌豆速度、梯子C4时长、撑杆C4方向倍率）都由正常完成进程检测，分别48/64/128/640/64条可定位差异，其他方法0差异，没有把异常或进程崩溃算检测通过。变体按真实DLL增量与SHA重建。首次fixture误将实例Component.get_transform映射为静态shim，被冻结签名检查挡在执行前；原源/日志/二进制保留，随后只修正公开fixture形状，未修改冻结器。

Unity lifetime、transform、动画精灵查询/隐藏、投射物管理器、Initial/GetDamage/SetDamage和Skill子调用为明确受控叶函数；该结果证明有限输入下调用方路径和实参，不证明子方法内部、实际画面/音频、普遍等价或真机行为。R19有限profile联合数80，累计正常原版记录26,772，另20个profile待验收，其中两个浮点差异方法继续保留失败。此前三个运行时故障未检出和一个仅源码EH guard的状态不变。
