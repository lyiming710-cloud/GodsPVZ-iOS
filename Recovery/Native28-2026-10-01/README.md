# Native28：十二方法批次与类型正确但行为错误的反例

2026-10-01。工作树 `/workspaces/GodsPVZ-native24-codespace`；分支 `repair/codex-native24-codespace`；父提交 `74340d2fd33c5679e722ac747ec254971f50d82c`。本轮仍无 IPA，未执行持证 Unity 导出、Apple/Xcode 编译链接或设备测试。

## 本次实际结果

十二个新目标按完整原版 PC 方法反汇编和数据流核对后统一施工。相对 Native27 只修改 30 个字节，两个独立输出逐字节一致。候选 SHA-256 `02390a90a9ced312c67b4b8ff085b5329234920c8f6e03bb3a2cdb294d1be465`；父候选 `ae6ac972e0615666091b7359774f0c0ec70d01d8e5d05d0942324b7d13b34920`。

十二目标以外 2285 个方法体原始字节完全相同；累计29个目标之外 2268 个方法体完全相同。整程序集 metadata、声明、RVA、方法头、locals 与文件长度不变。直接修改白名单原始字节，不执行 Cecil 程序集序列化。

全方法类型预检查 PASS1442→1454，FAIL855→843，仅本批十二目标转为PASS，没有PASS→FAIL。实际CLR执行候选CIL，854个用例全部通过；28个负面变体：26个行为不符、2个CLR拒绝，工具错误不会计入检出。另两组工具故障注入均返回exit2、gate=false、Detected=false。继承前四批499个用例和Native27加固后的72个负面变体继续通过。

只执行一次 fresh 历史 IL2CPP qualifier，exit0；使用Linux上的unityaot-macos参数，不等于持证Editor或iOS导出。Clang18.1.3 syntax-only检查264个TU，6033→6006条诊断，24个TU仍失败；报错方法659→647。十二目标的27条诊断归零，没有其他方法诊断增加。2255个非目标映射C++函数文本完全相同；global-metadata不变。父/候选MetadataUsage.c额外按C++编译均无错误。

## 修复与保真依据

目标及逐站点依据详见 `validation/NATIVE-REVIEW.json`：PlantManager.GetPlantPrefab、AttackRangeUIController.GetAttackRange、VertexZoom排序闭包、SuppliesInitialValue.GetSuppliesInfo、GlobalStaticVars.IsOnBoard、ResourceManager.Load_supplies_sprite、DeviceManager与BoardPreview.GetDevicePrefab、Zombie.FindPlant_Grid、SwfWaitExtensions.GotoAndStopAndWaitPlay(controller,string,int)、Plant.EnemySeeking_plural(id)、DialogueManager_OnBoard.TriggerCheck。

17处损坏引用零比较改为ldnull，12处经原版NRE出口核对的错误正常返回改为throw。原5字节ldc.i4保留4个可达nop，所有原偏移与分支保持。真整型零（负数索引、bool与枚举）不改。特别保留原版DevicePrefab的size < id判断：id==size仍调用get_Item并抛异常，不能擅自修成更友好的边界判断。Supplies负索引在this/list读取前返回null；AttackRange负索引会进入get_Item。Unity物理null与已销毁对象语义分别保留。

排序闭包0x0600080E存在类型检查无法发现的独立数据流损伤：第一get_Item结果存V3，但Single.CompareTo调用使用从未赋值的V9地址。原版0x1803CD3AE将第一值保存到stack+0x38，0x1803CD3CA取其地址作为比较receiver。仅将IL_004A的ldloca V9操作数改为V3，一字节变化。原版浮点leaf在0x180CC4FC0..0x180CC5000，逐分支核对-1/0/+1及NaN排序；此leaf无pdata，不编造函数表边界。保留初始128字节检查窗口，后64字节包含下一个函数，仅供来源核查。

`default_comparer`反例重现仅修判空/异常后仍比较默认零的错误。它不会造成类型或编译失败，却被固定native预期的行为测试检出。这直接说明工具打印PASS不能代替行为恢复。测试期望按原版证据和固定MethodDef契约编写，不从候选调用推导。

## 六个方法隔离及筛选教训

十八个待审方法中另六个没有套用本批规则：0x060008AA/8AC/8AE/8B6/8B7的原版事件间接调用被反编译成Indirect call字符串并丢弃，部分逻辑还错误读取写入后的_isPlaying，未保留native先捕获的原状态。0x0600019B TestLogTrigger重复使用V4=(类型==0)，使类型==1路径无法到达；native减一分派明确进入gameStart判断。这些需方法体重建，不能只消除编译报错。

之前过滤器仅补了Method not found/Unknown call target，仍漏掉Indirect call。当前审核使用完整方法读取和显式隔离表。本批patcher在白名单中遇到这些占位则拒绝。旧队列作为历史诊断证据保留，不能将旧E1类型PASS直接提升为修复资格。

## 验证范围与后续

fixture执行的是候选真实CIL，Unity.Object、List、RangeUI、Plant.Seek、Dialogue.Load及Swf helper均为公开说明的double。它验证原版外层控制流、参数、返回值、异常顺序及字段重读，不执行原版helper内部或UnityPlayer。保留null this/list、负索引、id==size、对象已销毁、NaN/无穷/正负零、Grid优先级、静态Instance/列表重读与匹配触发路径用例。854不是854种独立根因，也不代表游戏行为全部恢复。

候选仍有843个方法未通过类型预检查和6006条Linux编译诊断，不能进入完整Unity/Xcode验收。下一步先重建这六个已明确缺失调用/分支的方法，再处理已分类的数值locals、结构体传值和泛型绑定；按证据合并更大批次，而非按数量强行接受。最终交付目标仍是未签名IPA。

本轮工具接入曾因未映射IntPtr局部变量而中止，随后因Native27Verifier缺少legacy fixture参数中止；均属工具调用失败，没有计为行为检出，初始完整驱动输出归档。游戏候选未因工具接入修复而变化。

## 归档与复现

源码在 `scripts/takeover/native28`。ZIP包含两份候选、父DLL、原版十二方法字节/反汇编、完整CIL/metadata/声明、330个新生成文件、264份编译日志、fixture源码/Mono.Cecil、所有报告与执行脚本。原版PC完整GameAssembly、字段/方法映射、真实Unity支持程序集及Clang头文件通过固定Native24–27父Release清单恢复，不重复打包。

逐文件大小/hash在MANIFEST.json；本轮档案URL与hash在EVIDENCE-ARCHIVE.json。源码和新CLR检查可从档案普通复制到新目录重建；不要对只读原版或历史参考使用可写硬链接。整链编译需按父档案恢复工具/headers，再调整脚本固定Codespace路径。
