# R19 天赋升级预览：完整浮点/结构体调用方对照

0x06000598 Almanac_TalentSystem.PreviewOrderUp(bool) 在两个固定候选 R19 candidate9 / R20 authored3 上实际发射并执行完整 CIL。1,730条CLR记录相同，其中1,728条正常路径与未改的完整原版调用方一致；两个空存档 CLR 异常另列。游戏DLL和冻结Rehost不变。

原版执行 comiss 与无符号跳转，100f阈值直接从PC常量字节0000c842核验。覆盖NaN、正负零、正负Infinity、99/100/101、旧/新天赋数量、开关、order=-1/0/Int32.MaxValue以及激活/Transform/位置回调的字段替换；同时保留原生Vector3隐藏返回缓冲区协议，在CLR内实际按值传递结构体。正常路径0差异。

两个物理故障进程正常完成并产生可定位差异：阈值100f改0f（162条）、保持栈平衡但漏掉最终Preview调用（1,152条）。完整变体通过增量重建和SHA验证保留，没有靠异常/崩溃报通过。

天赋数量、子Preview及Unity组件/激活/Transform/localPosition为明确受控叶函数，不代表其内部已恢复，也不证明实际Unity画面、Windows SEH或普遍等价。R19有限profile联合数75，累计原版正常记录21,456，25个完整profile待完成。没有新增游戏改动，没有新的编译/Unity导出、链接、IPA或设备验收；R20真实Editor诊断作为独立工作流另行记录。
