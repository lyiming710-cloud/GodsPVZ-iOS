# Native26 — AttackRange.TestInRange_Device 完整方法恢复

2026-10-01，在 Codespace 的持久工作区完成。继承 Native25，新增一个完整方法体。**没有IPA，没有本轮持证Unity/iOS导出、Apple/Xcode构建、链接或设备测试；整程序集仍未通过。**

工作分支 `repair/codex-native24-codespace`，本轮父提交 `3ca09b7f599ead24b002a6095073d57c2d84a12c`；main及旧工作树保持原样。原版PC输入只读。

## 锁定输入与输出

输入 SHA-256：`9cbf08472d6c22f1b68393656ac2d36576fd6b21d49c4738f80aec2e2cbe749a`。

确定性候选 SHA-256：`affd9276454e70d6f39d9e28144ba2006021e818b1a617ed517eea90741ca878`，两次输出逐字节相同。

## native证据与恢复规格

目标 MethodDef `0x060000D9`，`bool AttackRange::TestInRange_Device(Device)`。原版PC函数 `0x1802FFCC0..0x1802FFDE1`，289字节。本轮固定native字节、反汇编、方法映射及历史field-offset记录SHA；字段偏移来自既有原版提取表，本轮核对锁定文件，未重新提取全部字段。fX=0x38、fY=0x3C、fW=0x48、fD=0x4C，AttackRange.rangeType=0x38。PC常量地址0x1815A7A08重新读取为0.5。

原版先检查Device物理null，null跳NRE helper；然后读取并计算Rect尺寸与Vector3中心，保存rangeType，最后分派：0/其他→false，1→Rect，2→Circle，3→Rect成功即true、失败才Circle，4→true。所有字段快照发生在业务helper之前。旧IL丢失了构造，多个case重复使用旧bool判据，并把float managed pointer作为Rect/Vector3传参；null路径将NRE对象作为bool返回。仅修判空不能恢复。

新body复用已有StandaloneSig `0x110000AB`。它的实际blob是 `070E1281D41281E01281F41282101180CD114D0C0C0C0C0C0C0C115C`，14个locals全部闭合，无!0/!!0；未增加metadata行。复用已有Rect/Vector3构造引用0x0A00009B/0x0A00009C；实际锁定UnityEngine.CoreModule.dll中的两个构造方法已解析，均只是对应float字段赋值，无额外业务调用。保留原始字段读顺序、float32运算、正零z和短路顺序；使用switch处理负数及超范围值。完整CIL、每个分支和header见METHOD-SPECIFICATION.json及final-metadata快照。

新body 206字节、72条指令，MaxStack=5、无EH；占用原348字节存储区。调整方法头MaxStack、CodeSize和LocalVarSigTok，其余存储区清零，填充不属于CIL。保留RVA、文件长度、metadata和其它方法位置。

## 实际验证

|门|实测|
|---|---|
|确定性与拒绝控制|两次相同；错误父DLL、同路径、已存在输出均拒绝|
|增量隔离|272字节只变于目标原存储区/方法头；2316个非目标MethodDef、其中2296个方法体原始字节及完整内容一致|
|累计隔离|相对0d6bc587…初始基线，五目标外2312个MethodDef、2292个方法体完全相同|
|完整声明|assembly/module/MVID、类型、字段、方法签名、属性、参数、资源、约束和metadata行数相同|
|E1类型检查|2297个body：PASS1429→1430，FAIL868→867；只有目标FAIL→PASS，0个PASS→FAIL；72/72可达|
|实际CLR|.NET10.0.12：255个新用例通过；覆盖9种rangeType、4组浮点数据、7种helper状态及3种null组合；覆盖NaN、无穷、极值、负零、异常和对象修改|
|行为反例|六种可执行错误形态全部检出：反转短路、错误z、交换中心坐标、深度误用宽度、All返回false、跳过Rect|
|继承回归|Native25的46个和Native24的24个CLR用例继续通过，既有行为反例继续检出|
|新IL2CPP|exit0，264个CPP TU；历史unityaot-macos qualifier on Linux，不是持证Unity iOS导出|
|CPP隔离|2266个已映射非目标函数文本一致；只改变GodsPVZRuntime1__2.cpp和Il2CppMetadataUsage.c；global-metadata.dat相同|
|完整Linux预检|6063→6057条错误，24个失败TU；含错误方法672→671；仅目标错误6→0，无方法错误增长|
|C文件额外检查|父版及候选Il2CppMetadataUsage.c显式作为C++检查均exit0、0错误|

**行为边界**：真正执行候选CIL，但Rect/Vector3形状和两个AttackRange业务调用显式映射到fixture类型与helper doubles。预期来自原版caller CFG/数据流；没有执行原版GameAssembly函数或原版AttackRange helper。两个helper本身仍有损坏，不能把caller通过说成整个范围判定行为已还原。没有Unity Player或真机验收。Clang为Ubuntu18.1.3，使用锁定Native18导出头文件，Linux语法预检不等于Apple编译/链接。

## 失败记录、归档与恢复

首次整程序集类型导出进程非零退出、日志为空；未记录到可归因错误，因此不声称其原因为OOM或候选错误。单独重跑exit0，随后完整类型/隔离检查通过。driver、独立export、resume及完整日志保留；运行时释放大型快照并分阶段执行，避免同时保留重复数据。日常工具构建错误已修正，不算游戏候选回归。

完整归档恢复第一次因Codespace磁盘满而失败。只删除了Native25及Native26的重复恢复测试副本，释放4.5GiB；保留锁定输入、候选、编译输出和父ZIP。磁盘事件及重跑验证均保存，不能把首次失败记录说成归档验收通过。

源码在scripts/takeover/native26，复现脚本在checkpoint，持久工作目录/workspaces/GodsPVZ-native24-codespace/.validation/native26-2026-10-01。归档包含本轮全部根目录证据/候选/日志、264份编译日志、完整元数据/声明/typed输出、native字段表和变化生成文件。固定Native24、Native25父ZIP恢复全部330个生成文件；必须按SHA核对三份ZIP，不直接编辑参考输入，不使用硬链接副本。下载信息见EVIDENCE-ARCHIVE.json。

下一步：完整审计并恢复TestInRects_Rect(0x060000D7)、TestInCircles_Position(0x060000D4)的循环和数值逻辑，再处理BoardInfoPage.SetLootList。后两者不是本批目标，未改。全包静态及平台条件满足后才进行持证Unity iOS导出和真实Mac/Xcode构建，最终目标仍为未签名IPA。

三层ZIP已实际恢复，全部330个生成文件及候选SHA逐个一致。
