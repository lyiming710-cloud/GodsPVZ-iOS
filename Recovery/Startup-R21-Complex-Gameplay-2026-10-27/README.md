# R21 原版复杂关卡真实Unity游玩尝试

候选仍固定75160a32…7b50e15；139处/7方法原有边界不变。目标原版冒险19“裂鸣共振”：14类僵尸、3旗30波、初始阳光1500。启动前核验实际StreamingAssets配置SHA与原版PC完全相同。通过游戏公开EnterBoard(Adventure,19,null)进入现有关卡，这会绕过菜单/存档解锁；不得称为正常解锁菜单通过，不修改任何解锁字段。CI若没有玩家，只走游戏原有新玩家创建回调。

进入非batch Unity Editor真实GameView，在Xvfb上通过xdotool/XTest发出OS鼠标。先移动鼠标并比对Unity Input.mousePosition校准，再选卡、确定、开始、点击卡槽及合法空草格。按真实GetMouseButton观察至少两帧再释放；UI点击后等待真实玩家循环处理。采集每帧进度、0.2秒间隔的真实单位位置/路径首节点/血量/护甲/攻击倒计时/弹丸来源/命中计数，以及PNG画面和完整运行错误。

不调用单位Update/Attack/TakeDamage，不直接生成植物或僵尸，不赋值HP/坐标/阳光/波次/地图，不为凑结果修改DLL或源配置。预置装置也会伤害僵尸，因此HP下降只记为来源未证明；植物来源弹丸、命中计数与具体目标/伤害归属分开判断。轨迹采样与已有Path字段只是运行观察，不冒充原生语义等价。测试不会自动把complex_gameplay_pass置真，需要读回轨迹/图像和攻击证据再评估。

真实UI前置已有Plant.animationGroup未赋值错误；本轮尝试可能在自然创建植物时被阻断，必须保存真实失败、未达路径和原始异常，不能视为通过。独立Editor API编译与工作流负控制属于工具预检；当前文档创建时尚未运行本轮游戏。实际证据以之后固定source/run与artifact读回为准。

## 首次真实桌面尝试40380298774：未进入游戏

source a56d0fa…，候选未变，实际Unity完成Editor辅助程序集编译后，图形启动未执行到Begin；宿主525秒超时，鼠标请求0，轨迹0，无GAMEPLAY-RESULT。不能归因为植物/僵尸行为错误，也不能算复杂关卡运行。完整原始日志和宿主记录已读回校验，席位归还exit0。

重试增加Openbox/D-Bus、真实桌面截图和窗口列表；90秒未达到helper即提前终止。先在batch模式编译相同helper，在一次性Editor工具GodsPVZIOSBuild中精确改名唯一过时symlinkLibraries属性为symlinkSources，保存前后哈希，不改游戏DLL。非batch启动禁止更新GodsPVZRuntime1程序集。原日志只能支持API updater/GUI启动疑点，未确证具体模态窗口。另恢复原版九个Plant组件缺失的序列化数据，见相邻Plant-Serialization检查点；本次历史失败不改写。

## 实际重试40404766623：成功进入原版19关，战斗尚未验收

固定source2c6a6f2…、游戏DLL未变。桌面服务和提前编译修正后，真实GameView正常运行，实际Input.mousePosition校准成功；OS鼠标完成选植物、确定和开始，三次点击均观察到真实GetMouseButton及对应UI状态转换，实际Board.level=19且gameStart=true。

九植物在AssetDatabase/Resources两路合计18个实际资源实例路由，原始846字段展开3040项实际标量/数组/持久引用比对全部通过，无缺项/未决引用。真实旧损坏报告58项失败；对当前报告副本篡改ID、动画引用、Sprite GUID及owner身份四种控制均拒绝，属于读回验证器控制，不冒充物理行为负控制。

第135帧、开局约1.95游戏秒，工具在点击卡槽前错误要求CardTemplate根为RectTransform而停止；根实际普通Transform，cardBackground子对象才是RectTransform。没有种植尝试、没有单位轨迹，不能评价正常攻击/轨迹。已保留PNG、所有原始JSON/日志、输入回执和完整失败。图鉴仍有LoadPlant IL_0099空引用，Plant.animationGroup未赋值错误已消失；不将工具问题和游戏问题混同。席位归还exit0，无新IPA。

后续修正卡槽点击使用原cardBackground、记录真实可见范围/Canvas/位置及Ready状态；卡槽不可操作时仍继续自然刷怪观察。另一个只读资源扫描发现26个Zombie根同类数据缺失，已构建2990字段恢复，见相邻Zombie-Serialization检查点。下一轮先验证全部35资源70个路由的实际字段值再进入UI/复杂战斗。

## 实测40413139415：全部资源值正确，复杂关卡仍停滞

固定source b945553、游戏DLL75160a32未变。9植物及26僵尸合计3836源字段，在AssetDatabase/Resources两路70个实际资源实例路由展开9902项标量/数组/持久引用比对，全部通过，0失败/0未决。实际桌面鼠标完成选卡、确定和开始，Board.level=19且gameStart=true；继续观察153.6877游戏秒/1614帧后仍没有任何单位轨迹或植物弹丸。Ready卡牌根世界位置一直(0,0,0)，背景无父Canvas，种植尝试0。真实游戏游玩验收失败；不将无异常日志当作成功。席位归还exit0。

实际UI进程加载的Unity.VisualScripting.Core.dll SHA dc1b51cc…7c8f9，ComponentHolderProtocol.GameObject的IL为142a，AddComponent<T>为1200fe156900001b062a，确认为空值/默认值返回；先前参考DLL线索已由实际加载二进制闭合。图鉴仍失败。原版PC对照另证当前Board.Start省略出怪管理器创建及配置、Banks.Initialize省略真实父节点/位置设置，后续按整条开局调用链恢复，不能给探针直接注入单位或强改显示状态。完整真实失败和二进制保存在Complex-Gameplay相邻目录。无新IPA。
