# R19 ICEUI.Update 完整调用方补充

0x06000342 Device/ICEUIController.Update(float,float,Device,bool) 的完整未改原版调用方与实际发射的完整候选 CIL，5,040条正常记录0差异，两个固定R19/R20候选全部结果一致。保留原版除法、COMISS、所有分支和while循环，原生Vector3隐藏返回缓冲区与按值/栈参数按真实ABI传递。

覆盖各破损阈值、零分母、NaN/Infinity、初始brokenLevel=-1/0/1/2/Int32.Max、sound开关、返回粒子实际空/伪空/有效，以及Sprite/Particle/Transform getter/AudioVolume/Unity比较回调后替换字段、资源列表和Camera。两个物理故障（sprite5误为6、while终止级别2误为3）分别产生432条正常行为差异，其他方法未变，无进程/异常算检测。完整输入、原生/CLR输出、两个候选重放、源码和故障增量SHA均保存。

Unity/粒子/音频/Camera/伤害终结子方法是受控叶函数，本结果不证明其内部、实际音频/画面、普遍等价或真机。R19有限联合81，正常原版记录31,812，另19个方法profile待验收（包含两处浮点差异方法）。没有新增游戏DLL改动，当前R20新8方法768条的原有范围不变；真实Unity导出的四条BuildReport错误在单独复现中定位，不与此有限profile混为通过。
