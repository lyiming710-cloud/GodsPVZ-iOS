# 原版26个Zombie组件恢复：2990字段

只读对照原版PC resources.assets与固定R3源，107个匹配资源根组件中35个存在“原版有自定义数据、源YAML仅组件头”的线索：9 Plant和26 Zombie。该数是此根组件范围的数据缺失线索，不是全游戏bug总数。Plant先前已实际验收字段；本批26 Zombie全部115字段逐一从原版完整解码并反写原始字节一致，合计2990项。类型/字段声明来自已核对原版metadata的同一75160a32…游戏程序集，不改方法体。

引用按原版资源层级、组件类型、同类组件序号及源localID闭合；所有26计划无隔离缺项。源脚本身份由候选类型的Unity DLL script ID算法与真实Plant/ZombieWindow ID控制对应，实际MonoScript加载仍由后续Unity验证。原版ImperialVanguardHarbinger.animator确实指向Flag预制体的Animator（原版pathID9009）；已按原始跨预制体关系映射Flag源GUID/组件fileID，未擅自改成本体Animator。旧25/26隔离版计划保留用于说明该边界。

恢复只在普通复制的工程副本追加26个已证明空owner的115字段。解析2990字段与原始计划一致，九植物846项及所有其他资产字节、GUID和游戏DLL保持不变，合计3836源字段。错误owner、错误文档hash、缺字段、篡改原ID、篡改速度及丢弃原版跨预制体Animator六个控制被拒绝；后三项属于固定原始计划hash门，不冒充行为测试。计划SHA固定7ce35a7d…4274a2ad。

下一轮真实Unity在同一75160a3233f1e1ab13a44f712efb4b47fdeb9b57bd38fa25f180276bb7b50e15下同时恢复9 Plant+26 Zombie，按AssetDatabase/Resources共70个路由与固定原值逐字段比对，未闭环即在Shader/UI/游玩前拒绝。桌面输入已由40404766623实际证明可用；修正CardTemplate普通Transform/子CardBackground RectTransform定位并增加可见性与卡槽状态记录，不赋值游戏字段。即使种植受阻也继续观察自然刷怪，保留未达路径。

图鉴还有LoadPlant IL_0099空引用。参考用VisualScripting Core包含默认返回IL，尚未证明是实际部署实现；仅作为线索。后续捕获实际UI进程加载程序集的hash/方法IL及原始字节，不据参考DLL直接改代码。

当前26僵尸恢复仅源构建/组合隔离/解析/工具API门通过，实际导入和正常轨迹/攻击均待测，正式新增合格方法0，无新IPA。

后续dispatch在启动runner前因job级env误用runner.temp被GitHub422拒绝，没有实际新run/许可占用。已移到两个实际执行步骤的step env，并由actionlint 1.7.12检查当前完整工作流通过；同工具对84f024e旧配置控制明确拒绝。拒绝原因与修正源已保留，不修改游戏DLL、原始计划或已经完成的运行结果。

## 实测40413139415：全部资源值正确，复杂关卡仍停滞

固定source b945553、游戏DLL75160a32未变。9植物及26僵尸合计3836源字段，在AssetDatabase/Resources两路70个实际资源实例路由展开9902项标量/数组/持久引用比对，全部通过，0失败/0未决。实际桌面鼠标完成选卡、确定和开始，Board.level=19且gameStart=true；继续观察153.6877游戏秒/1614帧后仍没有任何单位轨迹或植物弹丸。Ready卡牌根世界位置一直(0,0,0)，背景无父Canvas，种植尝试0。真实游戏游玩验收失败；不将无异常日志当作成功。席位归还exit0。

实际UI进程加载的Unity.VisualScripting.Core.dll SHA dc1b51cc…7c8f9，ComponentHolderProtocol.GameObject的IL为142a，AddComponent<T>为1200fe156900001b062a，确认为空值/默认值返回；先前参考DLL线索已由实际加载二进制闭合。图鉴仍失败。原版PC对照另证当前Board.Start省略出怪管理器创建及配置、Banks.Initialize省略真实父节点/位置设置，后续按整条开局调用链恢复，不能给探针直接注入单位或强改显示状态。完整真实失败和二进制保存在Complex-Gameplay相邻目录。无新IPA。
