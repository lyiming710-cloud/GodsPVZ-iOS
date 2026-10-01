# Native27：问题分类与首批十二方法修复

2026-10-01，Codespace持久工作区 /workspaces/GodsPVZ-native24-codespace。分支 repair/codex-native24-codespace，父提交94f162ae75f73b5356fa9ddc4338cca89967dc2c。main、旧工作树和原版输入未改。**仍无IPA；本轮没有持证Unity导出、真实Apple/Xcode编译、链接或设备测试。**

## 当前结果

本轮一次处理十二个方法，完成整程序集隔离、实际CLR执行、一次fresh IL2CPP转换和264个TU的全量Clang预检查。相对Native26仅24个字节变化；十二目标之外2285个方法体原始字节完全相同。累计十七目标之外2280个body完全相同。完整metadata、声明、RVA、文件大小、方法头及locals签名保持不变。

父DLL SHA-256 `affd9276454e70d6f39d9e28144ba2006021e818b1a617ed517eea90741ca878`。候选 `ae6ac972e0615666091b7359774f0c0ec70d01d8e5d05d0942324b7d13b34920`，四次输出逐字节相同。构造器对错误父输入、相同路径、现有输出及report覆盖输入均拒绝。

类型检查 PASS1430→1442，FAIL867→855；仅十二目标FAIL→PASS，无新增失败。实际CLR执行174个新用例全部通过，66个负面变体检出；部分非法返回变体由CLR程序有效性检查拒绝，不能将全部66个统称独立行为证明。继承Native24/25/26的325个用例及既有反例继续通过。

Fresh IL2CPP exit0，历史unityaot-macos qualifier on Linux；它不等于持证Editor/iOS导出。Clang18.1.3 syntax-only检查264个TU，错误6057→6033，失败TU24，含错误方法671→659。只有十二目标的错误下降至0，无方法错误增加。2255个已映射非目标CPP函数文本完全相同；仅GodsPVZRuntime1__19.cpp及Il2CppMetadataUsage.c变化，global-metadata.dat保持不变。额外将父/候选MetadataUsage.c作为C++检查均通过。

## 分类和批次边界

初始6057条诊断中5955条归属671个游戏方法，另38条方法外、64条头文件。初始症状分类：指针整数比较2489、数值引用混用1436、调用/泛型836、赋值589、强转167、值类型形状155、算术132、返回88、数组下标48、接收者15。每条只入一类，同一方法可属多类；这不是已证明的独立根因。完整当前分类见CURRENT-CLASSIFICATION.json。

首轮筛选33个只代表内存类型闭环。独立native核对发现TMP Word/Line在IL中丢失原版0x180A66460事件Invoke，只剩Method not found字符串；DeviceContent复制构造同样含占位。这三方法移出批次，严禁仅翻判空后宣称恢复。补全Method not found及Unknown call target operand标记后，347个报错方法含占位，合格于类型筛选的队列30个。本轮十二个完成，另外十八个仍未通过native站点门，见NEXT-QUEUE。记录旧漏筛和精确反例，未删除负面结论。

目标：SwfManager.get_clipCount(0x0600087E)、get_controllerCount(0x0600087F)，以及SwfWaitExtensions的十个string/int包装重载：0x060008AB、8AD、8AF、8B0、8B1、8B2、8B3、8B4、8B5、8B8。具体签名、native范围、原始字节hash、field offsets、calls及每个CIL修改站点见NATIVE-REVIEW。

两个getter原版从this+0x20或+0x28读关联列表，物理null跳0x180250150后int3；否则尾调用0x18065BAA0 Count。十个114-byte包装函数均先对controller物理null做异常出口；非null按原参数调用Play/GotoAndPlay/GotoAndStop，再分配对应Wait、调用共享ctor(null)、Subscribe原controller，并返回最初分配对象。原IL业务调用和对象传递尚完整，损坏是判空零常量和将NRE作为正常返回值。只在这些逐方法匹配的站点改ldc.i4 0→ldnull和ret→throw，无全局异常规则、无新防御分支。

每个5-byte ldc.i4替为1-byte ldnull加4-byte可达nop，保留CodeSize和所有原分支偏移；这些nop属于实际CIL，不是隐藏的不可达尾部。异常ret单字节替throw。patcher直接写锁定raw bytes，不进行Cecil重写程序集，不增加metadata引用或修改locals。

## 高还原证据边界

native证据来自锁定原版PC GameAssembly，完整字节/反汇编和方法映射保持；field-offset使用历史锁定提取表并校验hash，未重新提取全部字段。NRE helper身份沿用既有审计，本轮实际核对call+int3，不编造MethodDef。Wait类型由已匹配的IL ctor和native Subscribe receiver支持，未独立解码每个allocation class pointer。

CLR执行的是候选CIL，列表、controller和Wait helper明确替换为fixture doubles；检查参数、调用次序、异常传播、ctor(null)、Subscribe原对象及返回身份。没有执行原版helper内部或UnityPlayer。这批属于native外层调用契约和有限执行测试通过，不能扩张为整个游戏高还原或真机验收。

## 下一批与复现

分类→来源可信与完整方法类型闭环→逐方法native对照→批量materialize→轻量目标执行/字节隔离→每批一次fresh转换和全量Clang→归档。先审计NEXT-QUEUE十八个，其中存在不同字段、泛型、短路及列表/循环结构；不得按本批模式直接授权。再处理object数值locals、Vector3/Rect传值、泛型绑定和缺失调用/CFG重建。保留整型真零、LOCAL/TAINTED隔离和缺失调用反例。原版语义不明时隔离，不用编译成功代替行为证据。静态全包和平台条件满足后才进入持证Unity iOS与真实Mac/Xcode，最终目标未签名IPA。

工具源码 scripts/takeover/native27，checkpoint中保存实际执行脚本。脚本使用本次Codespace固定路径和父工作区输入；迁移需按锁定清单恢复并调整工作区路径。native批次抽取使用保留的INITIAL-33-REVIEW-QUEUE作为负面证据输入，当前施工队列使用NEXT-QUEUE，二者不可混淆。

本轮工具调试中修复fixture的C#变量命名和真实Internal.SwfAssocList命名空间，以及驱动的env/returncode参数错误；失败日志保存。这些修复属于验证工具，不能计为游戏行为恢复。

完整ZIP含全部330个当前生成文件、264份编译日志、完整typed/metadata/声明、四份确定性候选、native原始证据、原版PC DLL与映射、父候选、脚本和本轮所有日志。支持程序集、原始Unity工具链/导出headers仍锁定于Native24父归档；相关依赖链列入MANIFEST和EVIDENCE-ARCHIVE。restore_native27.py将本ZIP恢复到新目录，逐成员hash和候选及330生成文件核对；不复制三代大目录。下载见EVIDENCE-ARCHIVE.json。

完整ZIP已实际恢复；逐成员hash、候选和330个生成文件全部一致。
