from pathlib import Path
import json,hashlib,shutil,subprocess,zipfile,collections
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native29-2026-10-01';base=w/'.validation/native28-2026-10-01';out=w/'Recovery/Native29-2026-10-01';out.mkdir(exist_ok=True);v=out/'validation';v.mkdir(exist_ok=True)
def sha(p):
 h=hashlib.sha256()
 with Path(p).open('rb') as f:
  while b:=f.read(1048576):h.update(b)
 return h.hexdigest()
q=json.loads((n/'QUALIFICATION.json').read_text());f=json.loads((n/'RUNTIME-FIXTURE.json').read_text());c=json.loads((n/'CPP-STATUS.json').read_text());iso=json.loads((n/'CPP-ISOLATION.json').read_text());assert f['gate_pass'] and c['errors']==5941 and q['typed_fail']==812 and iso['non_target_mapped_cpp_methods_equal']==2236 and not iso['differing_non_target_methods'];stats=dict(collections.Counter(m['Status'] for m in f['mutations']));assert stats=={'BEHAVIOR_MISMATCH':41,'CLR_REJECTED':2}
allm=json.loads((n/'all-methods.json').read_text());assert not any('FTRuntime.SwfSettingsData::.cctor(' in m['name'] for m in allm['methods']);(n/'SETTINGS-INITIALIZATION-CHECK.json').write_text(json.dumps({'type':'FTRuntime.SwfSettingsData','static_constructor_present':False,'reset_calls_candidate_identity':True,'fixture_executes_candidate_identity_body':True},indent=2))
data=json.loads((n/'NATIVE-EVIDENCE.json').read_text());notes={
 '0x0600019B':'Unity-null board returns false before touching dialogue; otherwise store trigger type, read it again; kind0 true, kind1 reload board and return gameStart, other false. Physical-null dialogue/board access throws.',
 '0x060008AA':'Capture prior isPlaying; if false set true and timer0; optional Rewind; event OnPlayStopped only if prior false; construct wait(null), Subscribe, return allocated wait ignoring Subscribe return.',
 '0x060008AC':'Same captured prior contract, with SwfWaitRewindPlaying.',
 '0x060008AE':'Same captured prior contract, with SwfWaitStopOrRewindPlaying.',
 '0x060008B6':'Capture prior; if true set false and timer0; optional Rewind; event OnStopPlaying only if prior true; wait(null), Subscribe, return allocated wait.',
 '0x060008B7':'Physical controller guard, Unity-live clip -> reload and set_sequence; capture isPlaying after sequence, conditionally stop/reset, always Rewind, reload event after Rewind when prior true; construct/Subscribe/return allocated wait.',
 '0x060008A1':'All nine settings fields restored from original 24-byte value: 2048,1,100, true,false,true,true,1,2. Four boolean bytes 01 00 01 01; no .cctor.',
 '0x060008A3':'Store identity settings, all nine fields. Calls rebuilt pure identity body.',
 '0x06000820':'Fresh byte[0] each reset, String.Empty, null Atlas, then independent identity settings copies to Settings and Overridden; preserve order.',
 '0x0600082C':'Copy all four tint components, no truncation to r.',
 '0x0600082D':'Store all four tint components before UpdatePropBlock call; exceptions propagate after write.',
 '0x060000DF':'Return complete Vector3 input unchanged, this unused.',
 '0x0600013C':'AudioVolume before CreateAudioAtPoint(clip, by-value input position, volume, 1).',
 '0x06000203':'CreateProject(2,4,by-value position), physical result guard before SetEndPosition<Zombie>(zombie).',
 '0x0600088B':'Null/empty group no-op before this/set access; otherwise HashSet<string>.Remove.',
 '0x0600088C':'HashSet<string>.Contains; physical null set throws; null/empty group is a normal key.',
 '0x0600088D':'Logical inverse of HashSet<string>.Contains.',
 '0x0600088E':'Null/empty group no-op; otherwise Add when true, Remove when false.',
 '0x0600088F':'HashSet<string>.Contains on groupUnscales, no empty-name guard.'}
for t in ['0x06000160','0x06000161','0x06000162']:notes[t]='Copy by-value Vector3; x +=57, y +='+('28' if t=='0x06000162' else '44')+'; z unchanged; this unused. Original constants read directly from PE.'
for t in ['0x0600015D','0x0600015E','0x0600015F']:notes[t]='GetGridPosition(arg1,arg2), capture full result, x +=57, y +='+('28' if t=='0x0600015E' else '44')+'; z unchanged; preserve helper exceptions.'
for t in ['0x060000A8','0x060000AF','0x060000BB','0x060002C6','0x0600046C','0x060006D3']:notes[t]='Preserve full native-pinned audio chain: '+('self transform, position, AudioVolume, CreateAudioAtPoint(clip,position,volume)' if t in ['0x060000A8','0x060000AF'] else 'Random.Range(.95f,1.1f) before camera/transform/position, AudioVolume, CreateAudioAtPoint(clip,position,volume,pitch)' if t=='0x060000BB' else 'camera/transform/position, reload gLawnApp.audioVolume, PlayClipAtPoint' if t=='0x060006D3' else 'camera/transform/position, AudioVolume, CreateAudioAtPoint')+'. Physical null guards precede engine calls; actual Vector3 value is passed.'
review={'original_GameAssembly_sha256':data['GameAssembly_sha256'],'candidate_input_sha256':data['candidate_input_sha256'],'approved_methods':[{'token':m['token'],'method':m['method'],'native_start':m['start'],'native_end':m['end'],'boundary_scope':m['boundary_scope'],'native_sha256':m['native_sha256'],'cpp_diagnostics_before':m['diagnostics_before'],'contract':notes[m['token']]} for m in data['methods']],'scope':'Original PC outer MethodBody contracts; real emitted candidate CIL + explicit helper doubles; no whole-game/device equivalence claim'};(n/'NATIVE-REVIEW.json').write_text(json.dumps(review,indent=2))
(n/'FIXTURE-STATUS.json').write_text(json.dumps({'positive_cases':f['positive_cases'],'positive_failures':0,'negative_statuses':stats,'tool_faults':{mode:json.loads((n/(mode+'.json')).read_text()) for mode in ['fault_emit','fault_invoke']},'post_state_mutants':[m for m in f['mutations'] if m['Name']=='post_state']},indent=2))
readme=f'''# Native29：31 方法重建及结构体／泛型批次

2026-10-01。目标仍是高还原恢复并最终导出未签名 iOS IPA。当前分支 `repair/codex-native24-codespace`，工作树 `/workspaces/GodsPVZ-native24-codespace`，父提交 `b7135388d969c876c0e10a7fa436ea377a9daaa5`。本轮完成的是候选的局部门禁；无 IPA，未启动持证 Unity、Apple/Xcode 链接或设备测试。

## 结果与固定输入

Native28 输入 SHA-256 `{q['parent_sha256']}`；Native29 候选 `{q['candidate_sha256']}`，两次独立生成逐字节一致。原版 PC `GameAssembly.dll` 固定 `{data['GameAssembly_sha256']}`，方法/字段映射 hash 在完整 NATIVE-EVIDENCE.json。本轮不可继承此前未经接受的 Native23 批量替换。

31 个目标清除 65 条实际 C++ 诊断：6006→5941；报错方法647→616。Linux Clang18.1.3 对264个 TU 完成检查，24个仍失败，0个新增方法报错。只执行一次 fresh 历史 IL2CPP CLI qualifier，exit0；这是 Linux 的历史 unityaot-macos 配置，不是持证 Unity iOS 导出。2236个非目标映射 C++ 函数全文完全相同。

独立 raw/metadata 审计：2266个非目标方法体字节完全相同，所有既有表行和四个堆字节完全相同；原始 section/RVA、声明、locals、InitLocals、非目标头不变。本批目标允许重建 CodeSize/MaxStack 和 reserved code slot。原文件有2064字节差异，另附加401408字节。**本轮不再声称整个 metadata 或文件长度不变**：新增一段只读 `.n29meta` 和三条 MemberRef，调整必需 PE/CLR 元数据目录字段；旧 metadata 仍原样保留，既有 token 不重排。

新增引用仅为 `Action<SwfClipController>.Invoke(!0)`、`HashSet<string>.Remove(!0)`、`HashSet<string>.Contains(!0)`，TypeSpec、名称字符串及签名 blob 全部复用既有内容。HashSet<string>.Add 原本存在。新增索引未越宽度阈值；重开盘验证闭合声明类型、参数 VAR0 和返回类型。独立审计对破坏非目标体、既有表行、泛型接收者的三个反例均 exit2 拒绝。

全方法类型检查 PASS1454→1485、FAIL843→812，仅本批31方法转为PASS。实际 CLR 发射并执行候选 CIL，1915用例通过；43负控制全检出：41个有效 IL 的行为错误、2个 CLR 栈拒绝。工具发射/调用故障分别 exit2、gate=false、Detected=false，不计作行为检出。继承 Native24–28 的1353用例、Native27加固后的72负控制继续通过。1915是有限用例数，不代表游戏已验证1915个独立行为。

## 方法恢复与保真要点

五个 SwfWait 包装方法完整恢复原版丢失的真实事件调用。bool重载先捕获原 `_isPlaying`，写字段／reset timer、可选Rewind后，仍依据原状态决定事件。string重载在set_sequence之后读取当前状态，再Rewind并重读事件。使用真实CLR multicast Action检测顺序、正确controller实参、回调抛异常、Rewind改状态／替换事件；返回原分配wait，不接受Subscribe返回值。五个“用修改后状态判断”有效IL反例均被行为测试检出。

TestLogTrigger恢复类型0/1/其他的原版分派；类型1重新读取Board.gameStart。Unity假null与物理null分开，board不存活时不读取dialogue。保持先写管理器trigger字段，再次读dialogue类型的顺序。

SwfSettingsData不是简单把ldloc换成ldloca：原24字节默认值里四个bool为01/00/01/01，旧IL丢失三个字段；本批九字段全部恢复。SwfSettings.Reset、SwfAsset.Reset复用此次重建的纯identity方法，fixture实际发射该候选方法；已确认该类型不存在.cctor。SwfAsset每次创建新byte[0]，不改用共享Array.Empty。

Vector3坐标用原PE读取的float32常量57/44/28，完整传递z；按值输入不能写回调用者。AttackRange.Rect约束原版是直接返回输入，不能凭业务猜测增加截断。Color完整复制r/g/b/a，set_tint先写字段再调用UpdatePropBlock，回调异常不撤回字段。

七个音效入口和一个掉落物入口按原调用顺序传递实际Vector3值，保留摄像机／transform／返回Project的原版物理null门。音效随机pitch在camera之前计算，不擅自把所有音效都放在同一来源位置。HashSet改为闭合string实例，查询仍允许null/空字符串作为键；只有Resume/Set方法按原版做空名称短路。

逐方法契约、原始native边界说明、hash详见 validation/NATIVE-REVIEW.json。无pdata的leaf只使用下一映射指针检查窗口，不伪称它是确切函数长度。

## 可信边界与下步

fixture运行真实候选外层IL，但Unity.Object、engine、AudioVolume/Create、GetGridPosition、Rewind/sequence、Wait.Subscribe、Project.End等是显式double；不执行原版helper内部或UnityPlayer。真实CLR delegates与HashSet执行平台实现，设置Reset执行候选identity。测试覆盖NaN／无穷／负零与payload复制、null receiver、destroyed对象、字段重读、事件异常与已完成写入；仍不是跨平台全游戏等价证明。

新增MemberRef导致global-metadata.dat哈希从父版变化到 `17802ac102cc7f09ff98f709c320a4dbfe2f96358d3ab94754873982f79069c1`，已记录为有意变化；MetadataUsage.c父／新各按C++额外检查无错。不得套用旧批次metadata完全不变的结论。

下一批从812个类型失败／616个仍有C++诊断的方法继续分类，优先原版数据流明确、可共享有鉴别力规则的结构体、数值locals、泛型绑定；不按数量强行接受。5941条仍是Linux预检查诊断，可能包含同一根因重复或头文件问题，不能当作5941个方法。完整门禁清零后才持证Unity导出与真实Apple编译链接、未签名IPA检查及设备验收。

本批实现及工具接入曾因Cecil/Metadata同名类型、局部命名、泛型方法参数绑定、类型checker返回结构及测试增强作用域发生失败；全部中止并保存日志，未被计入游戏通过或负控制检出。第一候选缺少native明确调用前物理guard，检查后增加；最终候选与再次输出固定一致，不使用trial1作为当前输入。

## 归档／重放

源码 scripts/takeover/native29；ZIP包含最终候选／重复候选、父DLL、首候选及失败日志、31方法原native字节／反汇编、完整IL／metadata、330生成文件、264编译日志、新global-metadata、独立auditor、CLR fixture、Cecil支持与可移植重放程序。MANIFEST逐文件记录大小/SHA-256；完整原版PC及真实Unity／Clang支持由父Release清单恢复。禁止编辑只读历史参考或使用硬链接工作副本。

`python replay_native29.py evidence.zip new-directory` 普通解包复制、逐文件验hash，重新构建补丁器／审计器／fixture，重生成候选必须逐字节一致，再执行1915/43和两种故障控制。完整IL2CPP/Clang复现另需按父档案恢复实际支持、头文件及工具，并调整固定Codespace路径。
'''
(out/'README.md').write_text(readme)
for name in ['QUALIFICATION.json','ISOLATION.json','AUDIT-NEGATIVE.json','REGRESSION.json','CPP-STATUS.json','CPP-ISOLATION.json','IL2CPP-RESULT.json','NATIVE-REVIEW.json','FIXTURE-STATUS.json','SETTINGS-INITIALIZATION-CHECK.json','trial2.json']:shutil.copyfile(n/name,v/name)
files={}
def add(p,name):assert name not in files;files[name]=Path(p)
for p in n.iterdir():
 if p.is_file() and p.suffix in ['.json','.log','.dll','.txt']:add(p,'work/'+p.name)
for folder,label in [(n/'native','native'),(n/'fresh/clang','clang-logs')]:
 for p in folder.iterdir():
  if p.is_file():add(p,label+'/'+p.name)
for p in (n/'fresh/cpp').iterdir():
 if p.is_file():add(p,'generated/cpp/'+p.name)
for p in (n/'fresh/data').rglob('*'):
 if p.is_file():add(p,'il2cpp-data/'+p.relative_to(n/'fresh/data').as_posix())
for p in (w/'scripts/takeover/native29').rglob('*'):
 if p.is_file() and not any(x in p.relative_to(w/'scripts/takeover/native29').parts for x in ['obj','bin','__pycache__']):add(p,'tools/'+p.relative_to(w/'scripts/takeover/native29').as_posix())
add(base/'native28-final1.dll','inputs/native28-parent.dll');add(n/'fixture-bin/Mono.Cecil.dll','fixture-support/Mono.Cecil.dll');add(out/'README.md','README.md')
parents=json.loads((w/'Recovery/Native28-2026-10-01/EVIDENCE-ARCHIVE.json').read_text());manifest={'parent_archives':parents['parent_archives']+[{k:parents[k] for k in ['file','sha256','url']}],'files':{name:{'bytes':p.stat().st_size,'sha256':sha(p)} for name,p in sorted(files.items())}};asset=n/'native29-codespace-2026-10-01-evidence.zip'
with zipfile.ZipFile(asset,'w',zipfile.ZIP_DEFLATED,compresslevel=6) as z:
 for name,p in sorted(files.items()):z.write(p,name)
 z.writestr('MANIFEST.json',json.dumps(manifest,indent=2))
with zipfile.ZipFile(asset) as z:
 for name,row in manifest['files'].items():b=z.read(name);assert len(b)==row['bytes'] and hashlib.sha256(b).hexdigest()==row['sha256']
ref={'file':asset.name,'sha256':sha(asset),'bytes':asset.stat().st_size,'members':len(manifest['files'])+1,'url':'https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/download/native29-codespace-2026-10-01/'+asset.name,'parent_archives':manifest['parent_archives']};(out/'EVIDENCE-ARCHIVE.json').write_text(json.dumps(ref,indent=2))
for name in ['STATUS.md','HANDOFF_CURRENT.md']:
 path=w/'Recovery'/name;old=path.read_text()
 if old.startswith('## Native29 checkpoint'):continue
 path.write_text('## Native29 checkpoint — 2026-10-01\n\n31 methods / 65 diagnostics cleared; 6006→5941, 24 TUs still fail, 616 methods still have C++ diagnostics. Candidate SHA-256 '+q['candidate_sha256']+'. 2266 non-target raw bodies and existing metadata rows/heaps exact; three explicit closed MemberRefs appended in new metadata section. 1915 actual CLR cases, 43 negative controls, 1353 inherited cases pass; 2236 non-target C++ functions exact. No licensed Unity/Xcode build or IPA. See [Native29](Native29-2026-10-01/README.md). Previous checkpoints retained below.\n\n'+old)
(v/'SHA256.json').write_text(json.dumps({p.name:sha(p) for p in sorted(v.iterdir()) if p.name!='SHA256.json'},indent=2));print(json.dumps(ref));print('ARCHIVE_COMPLETE')
