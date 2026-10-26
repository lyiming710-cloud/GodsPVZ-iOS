# R21 序列化/重新导入观察实验（同一游戏DLL）

实际run40365525028已证明选卡/确认恢复，但图鉴仍失败。source-only run40368892424进一步逐项确认：ZombieWindow六个引用和ZombieManager的76个音频槽位（63音频+13空槽）在源YAML本来正确，全部预制体/场景本地fileID图未发现悬空引用；2,802个游戏字段声明类型与原版一致。因此停止重写正确源引用，转查导入/对象加载。

新Editor-only probe在强制重新导入之前/之后，分别通过AssetDatabase和Resources加载同一ZombieWindow、ZombieManager和Sunflower。记录实际程序集位置、类型/属性、MonoScript.GetClass、Reflection字段值、SerializedObject属性路径和对象/数组值、每个对象实际GUID/local fileID和EditorJsonUtility原始输出。再ForceUpdate+ForceSynchronousImport全部原有prefab/scene，记录每个路径和完整Assets前后SHA；任何源字节或游戏DLL变化、异常/收集不全都会拒绝该实验。不会SetValue/改SerializedProperty/SaveAssets/保存预制体/赋值游戏字段或替换方法。

“收集完成”仅说明执行和字节边界，不代表资源值恢复或缓存假设成立。后续实际交互另行断言六个引用、76音频顺序、选卡/确认、图鉴、菜单。菜单目标改用已读源中的Adventure/Windows/Button/MenuButton，源回调确为Board.ButtonDown_Menu；等待真实gameStart或开场对话启动，再推进实际对话，移除不存在的senarioState观察项。不靠显示文字猜控件，不写游戏状态。

本次仍固定75160a32…7b50e15，原139处/7方法的静态/有限对照与Linux/Apple证明继续按同一DLL身份保留。没有新游戏代码、没有声称额外76槽位已修复；强制导入能否改善要看实际前后结果。helper先通过精确Editor API编译，CI再在许可激活前独立编译；仍仅audit_only=true和interaction_audit=true，失败保存与归还席位原样保留，不导出新IPA。

## 实际结果（40374543207）

同一候选75160a32…7b50e15，强制导入188个原有prefab/scene，Assets和游戏DLL字节均未变化。导入前ZombieWindow六个字段为空、ZombieManager音频数组为0；导入后AssetDatabase和Resources两路均读回正确目标与76个有序槽位。后续真实Play Mode再次独立通过这7项绑定检查，僵尸图鉴窗口数24，选卡、确定与源路径菜单指针操作通过。该结论仅针对本次观察的资源子集。

植物图鉴仍失败：Plant.animationGroup未赋值，LoadPlant抛NRE。实际UI共26项检查，23通过、3失败；完整错误保留在原始JSON/日志。不能声称植物战斗、iPad接受或新IPA。下一步按用户要求尝试原版复杂关卡真实OS输入与轨迹/攻击观察，见相邻Complex-Gameplay交接。
