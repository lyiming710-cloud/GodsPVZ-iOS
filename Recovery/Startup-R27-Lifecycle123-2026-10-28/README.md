# R27 original card-to-hand and zombie-head lifecycle recovery

R25 parent `9490c1845298fa5afda04fc8dfba915ce2a939eaebe96be83e3311b39557bc4c` → R27 `eef6c35a2dceb0c38d1032837204abaff56f3a4c4d029ca47d76ff496fd69293`. Two full method bodies; 123 distinct original native semantic operation positions. These are not 123 independent bugs, methods or formal qualifications. The separately recovered SDK remains `51f72bcd…14c2b`.

R26 actual Unity run 40709087573 passed all ten real-engine SDK checks, loaded the exact SDK in both dumps, and passed actual OS-input card selection. It failed planting and reached the original defeat screen after 42.29 game seconds. Full failure evidence is retained in the R26 checkpoint. The 21 raw exception occurrences were HPUIController_Plant.Start, Plant.TryPlanting and Zombie.HeadDrop.

`MouseManager.GetPlantFromCardbank` (0x060001DD / PC 0x18031E4D0) now restores the full original sequence: required card/board data, original prefab-list index and Unity-null normalization, real prefab instantiation, Transform enumeration with discarded Shadow string comparison and finally Dispose, **handPlant.board before initialization**, separate hand item/type writes, complete first-open/already-open detail-page logic, correct card assignment order, and preview instantiation with **only** SetPrefabState_previewOnField. Original MonoBehaviour.print messages and missing-prefab/preview behavior remain. Original Equals and op_Equality share PC pointer 0x180B76170; the static string equality contract is retained, with no invented child mutation.

`Zombie.HeadDrop` (0x06000453 / PC 0x180361A70) restores original armor/noResidue handling, six-entry native jump table and additional/special IDs, exact audio slot/volume and sprite-position selection, camera-located sound, ID-specific particles, original **GameObject.AddComponent<SortingGroup>** and Particles layer, hiding anim_Head, target unblock, and final **isDying=true at +0xB7**. The field is isDying, not an invented isHeadDropped field.

The Plant.TryPlanting and HPUIController_Plant.Start leaf bodies were inspected and left unchanged. The captured Peasniper prefab already has its serialized HPUIController_Plant.plant reference; no fallback GetComponentInParent, defensive null check, dummy resource or field visibility change was added.

Independent byte isolation checks all 2,317 MethodDefs: exactly two targets differ and the other **2,315 remain byte-identical**. All old code slots, metadata rows/heaps, method flags and section permissions are preserved. Two local signatures are appended; no MemberRef or MethodSpec is added. Repeated construction is identical. Both targets pass the unchanged frozen checker; the whole assembly remains 2,200 PASS / 97 FAIL_OR_UNSUPPORTED. All 2,297 exported method member accesses pass; two exact-image JIT preparations pass without invoking gameplay.

The exact candidate raw CIL was actually executed in 269 finite cases, and the two byte-rechecked original PC callers matched all **268 normal cases**. One injected exception case observes candidate finally/Dispose but original Windows SEH unwind remains UNSUPPORTED. Controlled engine/game callee leaves are explicit test contracts, not restored nested game behavior. Six physically changed, type-valid wrong candidates create observable final-state differences and are detected. An initial audio-volume mutant exposed a missing audio-source state observation in the fixture; the fixture was extended to retain source clip, position and volume and both CIL and original-PC comparisons were rerun, without weakening the frozen checker.

Current-child full IL2CPP/Clang, actual Unity gameplay, Apple compilation and iPad testing remain pending. The old R25 Linux compiler result is parent evidence. The last IPA is R20; this checkpoint provides no new IPA and adds zero formally qualified methods.

R27实际Unity运行40722007191已启动，固定源码189a571。输入绑定及Editor helper预检已过，仍处于环境恢复阶段，暂无关卡结论。另准备独立的全量Linux编译：显式使用当前游戏eef6c35a与实际Editor SDK 51f72bcd，先复制认证过的portable support，仅替换SDK并重核其余文件字节；旧portable保持不变。当前子版本不能继承R25的编译结果。

全量Linux编译40723256900已启动，固定源码b504fa7；与Unity运行40722007191分别记录。回读R25原始转换输入证实旧SDK为Unity.VisualScripting.Core.exe（330240字节，16cabf56…a4fb），并非本轮实际Editor的51f72bcd SDK。新编译显式替换该唯一SDK槽并保留其他输入字节，结果尚未返回；不能用前置绑定成功代替转换或编译成功。

实际Linux运行40723256900已成功结束，固定3bdd831源码及9490c184候选。逐一重算755个CPP+274个C的源码大小/SHA，并回读全部1029份原始日志，0错误；编译器负控制仍检出真实损坏。全量生成源码tar的SHA及所有游戏分片保留，不能代指真实Unity导入、Apple链接或IPA。

实际Unity运行40722007191已回读：OS点击种下一株真实场上豌豆狙击手，board与血条plant引用一致，6826条单位观测且运行时异常0；47.25游戏秒后原版败北界面出现。观察到三次攻击计时重置及对应目标护甲变化。原版ST_Peasniper直接调用TakeDamage、不创建Projectile，旧的强制投射物条件不适用于该植物；当前没有完整伤害来源归因，不能把护甲下降单独作为植物攻击验收。所有原始失败结果、14幅图片和SDK实际加载证据保留。未执行Apple编译，未生成IPA。
