# Stage9.1 Administrator.Start — 专用交接文档

快照：2026-09-16 UTC
仓库/分支：lyiming710-cloud/GodsPVZ-iOS / high-fidelity
上传前 HEAD：0cd13b110189041a8ded70e54ad58ccbe8384ac9

## A. 状态边界

HF55 是唯一 sealed baseline。Administrator.Start 只是 HF55 之上的 Stage9.1 development candidate 恢复目标，当前没有任何资格结论。已有 patcher source 不是 patched DLL，也不是 runtime qualification。

当前输入 candidate：26064265fdba4e9b501f0c3312260503fdde43b9e111072c19dd39891e5ca022。
当前 target：Administrator.Start() / MethodDef 0x06000004 / RID 4。
target managed preimage：b6aa91789131594a0b1ac89a83197f026261ea6e9867df34cc2b8b15b46b8089。

## B. PC native authority

method pointer table base：0x181B82D60。
RID 4 entry：0x181B82D78。
native body：0x1802FDD20–0x1802FE980。
native body size：3168 bytes。
native slice SHA256：3d83a514d37c18b7d91df7ceba17803bb0ac3cbd670dab15e823af4be8b58d02。

原 managed body 指纹：635 bytes / 30 locals / 168 instructions / 0 handlers。恢复时必须验证完整 preimage，不能只按方法名匹配。

## C. 已闭环的 source-level 语义

| mode | native 对齐的恢复语义 |
|---:|---|
| -1 | mainSystem=false；boardEdior.gameObject=false；Camera.main.orthographicSize=540；若 MainUIController.instance 有效则调用零参数 SetGloveButtons() |
| 0 | 激活 mainSystem；通过 GlobalStaticVars.gLawnApp -> LawnApp.savesManager -> SavesManager.playerSave 调用 SystemSkipLevel.Start0(Save) |
| 1 | 激活 systems[0]；调用 Start_植物存档数据()；调用 system0.LoadPlantData(-1) |
| 2 | 激活 systems[1]；调用 Start_出怪挑选器数据()；调用 system1.LoadZombieInfo(-1) |
| 3 | 激活 systems[2]；通过 BoardManager.get_Instance() -> Board_OnPlay() 取得 current board；调用 Start_关卡配置文件(board/null)；调用 system2.Start2() |
| 4 | 激活 systems[3]；先调用 system3.LoadSuppliesInfo(-1)，再调用 Start_材料基础数据() |
| common tail | mode != -1 且 current board 有效时调用 Board.GamePause(true) |

mode gate 是 unsigned (mode + 1) <= 5。非法 mode 不执行公共 systems deactivation foreach，而是直接进入 common tail；不能把 foreach 无条件前置。

公共 valid-mode 路径必须使用 List<GameObject>.GetEnumerator()、get_Current()、MoveNext()、Dispose()，并保留 try/finally 结构。不得访问 List<T> 的内部字段。

BoardManager.board 是 private。即使 native 直接读取对应 offset，也不能在 Administrator.Start 中生成跨类型 ldfld；当前合法恢复路径是 get_Instance() + Board_OnPlay()。

## D. 已上传实现

- Tools/Stage9AdministratorStartPatch/Program.cs：输入 SHA 和 preimage 双锁；target 仅 0x06000004；生成 foreach/finally、六 case 和 common tail；写回后执行 reopen/isolation/reference gates。
- Tools/Stage9AdministratorStartPatch/Stage9AdministratorStartPatch.csproj：Mono.Cecil 0.11.6，TargetFramework net10.0。

目前没有与该 patcher 对应的 buildcheck/static qualification workflow；当前三个 Administrator probe workflow 只负责前置证据，不代表 patch 已执行。

## E. 已上传的证据与 CI

- dependency probe success：run 35049752106，artifact 10428660878，digest sha256:f76f7fcaad17070ac8b7ab93337b8cf2e76db10dc401303c96e8a7d2a625a669。
- exact-ref probe success：run 35043040058，artifact 10425308520，digest sha256:639b506e8750faee339f7d35cd022f1d9b19aa3f023156b20cf4afb21bb8b736。
- TextLink.Update natural runtime success：run 35041620741，artifact 10425941126，digest sha256:1b02ffb7484c80632f51fa6a8d46328d3c2f3cada6094d6691bc751fd3399745。
- 旧 dependency probe run 35046561491 的失败是 methods=17 机械断言，不是 target IL 失败。

## F. 下一阶段门禁

先从最新 HEAD 重新确认 candidate input，再完成以下顺序：

1. 构建并运行专用 Cecil patcher。
2. 读取 pre/post IL、output SHA、Cecil reopen 结果。
3. 验证只改变 target body；method signature、field metadata、assembly refs、MemberRefs、TypeRefs 和 2317/2802 总数不漂移。
4. 扫描 InvalidIL、FieldAccess、MissingMethod、CoreLib、zeroVector、unmanaged scaffolding 和 List<T> 私有字段污染。
5. 检查 Zombie/ZombieInfo/Device/Supplies/Almanac/TextLink 等已恢复路径不回归。
6. 静态全绿后再使用 exact-R3 与固定 China Editor 做一次 natural runtime。
7. runtime 成功后读取 FIRST_INVALID_IL，再按真实因果顺序继续。

除非用户明确要求，否则不要晋级 sealed baseline，也不要命名为 HF56/HF57。