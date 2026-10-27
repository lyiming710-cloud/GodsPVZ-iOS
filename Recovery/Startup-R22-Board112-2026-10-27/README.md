# R22：恢复完整开局链，112个位置 / 5个方法

真实Unity父运行40413139415已确认35个Plant/Zombie资源、70个加载路由、9902项原始字段/引用全部正确；实际桌面鼠标进入第19关后154游戏秒仍没有单位，卡牌在原点且无Canvas。完整真实失败保留在相邻Complex-Gameplay检查点，没有被后续修复覆盖。

本批直接对照原版PC GameAssembly.dll完整调用者与metadata/原始字面量，恢复Board.Start、Board.Update、Board.GameStartOnBoard、Banks.Initialize、Banks.BankStart，计112个不重复原生语义位置，5个方法。位置不是112个独立游戏bug。创建并连接EnemyManager、按原关卡配置出怪参数/初始设备/预置卡牌，恢复开场阈值、卡槽动画、FlagMeter和音乐；卡槽恢复原版相邻价格比较、带finally的类型化枚举、父Canvas层级、零起始编号、(ordinal*92-677,22,0)位置与14张一页边界，以及原始道具栏启动。没有注入游戏单位、修改关卡、解锁/库存或直接赋值战斗状态。

父DLL75160a32…，子DLL15e01503…ac2cf。5个新方法全部迁移至追加区域，原槽位全部保留；其余2312个MethodDef、旧元数据行/堆/成员可见性及段权限不变。仅追加5个local签名、1个GetComponent<EnemyManager> MethodSpec，没有新增MemberRef。独立隔离验证含3个越界损坏控制。5个目标的冻结类型栈、全程序集成员访问和精确JIT准备通过；全程序集原始类型统计2199 PASS / 98 FAIL_OR_UNSUPPORTED，未将旧接口补充结果或其他方法当作本批行为证明。

实际执行子DLL原始CIL字节，仅重绑声明的外部成员token，保持操作码/分支/代码长度及原始EH偏移。131个有限用例运行，128个正常用例与5个未修改原版PC调用者实测逐条轨迹/状态一致；另3个受控父节点抛错用例确实执行CLR finally并记录Dispose。原版Windows C++异常展开未执行，这3个不能记为原生异常等价。所有Unity/列表/嵌套游戏调用叶子均为明确受控契约，不是Unity或iPad游玩。

8个物理DLL、类型有效的错误变体（遗漏manager、错误flags字段、错误ready阈值、错误NaN分支、错误meter显隐、卡牌间距、finally Dispose和道具索引）均在进程正常完成的情况下与已执行原生正常轨迹产生分歧。初版构建还保留4个回调敏感失败：资源加载回调改变background及GameObject查询回调改变ordinal；112位置版本修正为原版读取顺序，未把早期110位置版本冒充合格候选。

新EH执行器是独立工具，历史禁止EH的执行器与冻结验证器未改变。早期工具编译缺少PEReader扩展命名空间、未映射嵌套LawnApp，以及负控制驱动把ldloc.s误称ldloc，都属于工具问题，未计为游戏缺陷或语义拒绝控制。当前正常执行及8个语义控制均已实际完成。源码包保留依赖、原生证据、原始执行结果、所有负控制日志和字节变体位置。

下一门为固定子候选的全新IL2CPP转换及全部生成C/C++编译，再验证Apple输入并回到真实Unity复杂关卡种植/轨迹/攻击。实际加载的VisualScripting Core默认返回问题另行保留；本批尚未改SDK。当前不宣称子候选Unity导入、复杂关卡、iPad或IPA通过，正式新增合格方法0。

固定ba78423源码的全新Linux IL2CPP/全部生成代码编译已启动：[40452784098](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/40452784098)。当前只观察到运行中，不预记为PASS；该任务不消耗Unity授权，不导出IPA。
