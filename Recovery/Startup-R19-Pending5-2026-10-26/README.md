# R19 待对照方法补充：5 个完整调用方

本次没有修改游戏 DLL。对固定 R19 candidate9 和当前 R20 authored3 都实际发射并执行完整候选 CIL，868 条 CLR 记录逐项相同；其中 861 条正常记录与完整原版 PC 调用方一致，7 条 CLR 异常另列。

| Token | 方法 | 原版正常记录 |
|---|---|---:|
| 0x060004CA | PlantSave.Clone | 21 |
| 0x06000341 | Device.ICEUIController.Start | 252 |
| 0x06000599 | Almanac_TalentSystem.Preview | 336 |
| 0x06000578 | Almanac_PlantDetail.CloseOrderUpPage | 126 |
| 0x06000579 | Almanac_PlantDetail.CloseSkillUpPage | 126 |

克隆检查完整字段位模式、数组独立性、字符串浅复制和受控 Skill.Copy 返回对象；冰冻初始化覆盖正负零/有限值/Infinity/NaN、Unity 伪空及回调后的接收者重读。天赋/关闭页面覆盖预览分支、按钮状态、销毁次序、列表替换、真实 CLR List.Clear 的版本/容量/元素效果。升级资格判断保留原版纯整数叶函数实际执行。

五个物理单点故障全部被真实执行差异捕获：少复制一个数组元素、错误动画速度、反向按钮状态、分别漏掉两个 Clear。所有故障进程退出 0，未将异常/崩溃当成功；其他目标无差异。完整故障 DLL 可由当前候选与 PHYSICAL-CONTROL-DELTAS 重建并校验 SHA。

初次对照保留 252 条失败：CLR 输入通过 LINQ ToList 生成非空列表版本 1，而原生输入设为 0。所有轨迹及非版本字段一致。保存旧源码/完整结果后，改用公开 List<T>(T[]) 构造，并在调用前断言初始版本 0、容量等于数量；游戏和冻结验证器/发射器未改。重新执行全部记录后 0 差异。

分配、克隆子对象、Unity 对象生命周期/动画/UI/销毁、天赋数量和页面内部叶函数有明确受控契约。本测试没有证明这些叶函数内部、Windows SEH、实际 Unity 渲染或普遍等价。R19 有限 profile 从 65 增至70，累计原版正常记录 18,938，30 个完整 profile 仍待完成。这些是验证工作项数量，不是剩余 bug 总数。没有新持证 Editor 导出、链接、IPA 或 iPad 验收。新代码仍按超过100处后集中测试的政策累计；本次新增游戏改动为0。
