from pathlib import Path
import json,subprocess,hashlib,zipfile,shutil,collections
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native28-2026-10-01';base=w/'.validation/native27-2026-10-01';out=w/'Recovery/Native28-2026-10-01';out.mkdir(exist_ok=True);v=out/'validation';v.mkdir(exist_ok=True)
def sha(p):
 h=hashlib.sha256()
 with Path(p).open('rb') as f:
  while b:=f.read(1048576):h.update(b)
 return h.hexdigest()
q=json.loads((n/'QUALIFICATION.json').read_text());c=json.loads((n/'CPP-STATUS.json').read_text());iso=json.loads((n/'CPP-ISOLATION.json').read_text());rv=json.loads((n/'NATIVE-REVIEW.json').read_text());f=json.loads((n/'RUNTIME-FIXTURE.json').read_text());assert f['gate_pass'] and q['typed_fail']==843 and not c['pass_to_fail_methods'];assert c['errors']==6006
stats=dict(collections.Counter(x['Status'] for x in f['mutations']));(n/'FIXTURE-STATUS.json').write_text(json.dumps({'positive_cases':f['positive_cases'],'positive_failures':f['positive_failures'],'negative_statuses':stats,'default_comparer':next(x for x in f['mutations'] if x['Name']=='default_comparer'),'tool_error_controls':{mode:json.loads((n/(mode+'.json')).read_text()) for mode in ['fault_emit','fault_invoke']}},indent=2))
for name in ['QUALIFICATION.json','CPP-STATUS.json','CPP-ISOLATION.json','IL2CPP-RESULT.json','NATIVE-REVIEW.json','FIXTURE-STATUS.json','patch-final1.json']:
 shutil.copyfile(n/name,v/name)
readme=f'''# Native28：十二方法批次与类型正确但行为错误的反例

2026-10-01。工作树 `/workspaces/GodsPVZ-native24-codespace`；分支 `repair/codex-native24-codespace`；父提交 `74340d2fd33c5679e722ac747ec254971f50d82c`。本轮仍无 IPA，未执行持证 Unity 导出、Apple/Xcode 编译链接或设备测试。

## 本次实际结果

十二个新目标按完整原版 PC 方法反汇编和数据流核对后统一施工。相对 Native27 只修改 {q['changed_bytes']} 个字节，两个独立输出逐字节一致。候选 SHA-256 `{q['candidate_sha256']}`；父候选 `{q['parent_sha256']}`。

十二目标以外 {q['incremental_non_target_bodies_raw_exact']} 个方法体原始字节完全相同；累计29个目标之外 {q['cumulative_non_target_bodies_raw_exact']} 个方法体完全相同。整程序集 metadata、声明、RVA、方法头、locals 与文件长度不变。直接修改白名单原始字节，不执行 Cecil 程序集序列化。

全方法类型预检查 PASS1442→{q['typed_pass']}，FAIL855→{q['typed_fail']}，仅本批十二目标转为PASS，没有PASS→FAIL。实际CLR执行候选CIL，854个用例全部通过；28个负面变体：{stats.get('BEHAVIOR_MISMATCH',0)}个行为不符、{stats.get('CLR_REJECTED',0)}个CLR拒绝，工具错误不会计入检出。另两组工具故障注入均返回exit2、gate=false、Detected=false。继承前四批499个用例和Native27加固后的72个负面变体继续通过。

只执行一次 fresh 历史 IL2CPP qualifier，exit0；使用Linux上的unityaot-macos参数，不等于持证Editor或iOS导出。Clang18.1.3 syntax-only检查264个TU，6033→{c['errors']}条诊断，{c['failed']}个TU仍失败；报错方法659→{c['mapping']['methods_with_errors']}。十二目标的27条诊断归零，没有其他方法诊断增加。{iso['non_target_mapped_cpp_methods_equal']}个非目标映射C++函数文本完全相同；global-metadata不变。父/候选MetadataUsage.c额外按C++编译均无错误。

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
'''
(out/'README.md').write_text(readme)
files={}
def add(p,name):assert name not in files;files[name]=Path(p)
for p in n.iterdir():
 if p.is_file() and p.suffix in ['.json','.log','.dll','.asm','.bin','.txt']:add(p,'work/'+p.name)
for token in [x['token'] for x in rv['approved_methods']]:
 for ext in ['bin','asm']:add(base/'native'/(token+'.'+ext),'native/'+token+'.'+ext)
for p in (n/'fresh/clang').glob('*.log'):add(p,'clang-logs/'+p.name)
for p in (n/'fresh/cpp').iterdir():
 if p.is_file():add(p,'generated/cpp/'+p.name)
for p in (n/'fresh/data').rglob('*'):
 if p.is_file():add(p,'il2cpp-data/'+p.relative_to(n/'fresh/data').as_posix())
for p in (w/'scripts/takeover/native28').rglob('*'):
 if p.is_file() and not any(x in p.relative_to(w/'scripts/takeover/native28').parts for x in ['obj','bin','__pycache__']):add(p,'tools/'+p.relative_to(w/'scripts/takeover/native28').as_posix())
add(base/'native27-final1.dll','inputs/native27-parent.dll');add(n/'fixture-bin/Mono.Cecil.dll','fixture-support/Mono.Cecil.dll');add(out/'README.md','README.md')
parents=json.loads((w/'Recovery/Native27-2026-10-01/EVIDENCE-ARCHIVE.json').read_text());manifest={'parent_archives':parents['parent_archives']+[{k:parents[k] for k in ['file','sha256','url']}],'files':{name:{'bytes':p.stat().st_size,'sha256':sha(p)} for name,p in sorted(files.items())}};asset=n/'native28-codespace-2026-10-01-evidence.zip'
with zipfile.ZipFile(asset,'w',zipfile.ZIP_DEFLATED,compresslevel=6) as z:
 for name,p in sorted(files.items()):z.write(p,name)
 z.writestr('MANIFEST.json',json.dumps(manifest,indent=2))
with zipfile.ZipFile(asset) as z:
 for name,row in manifest['files'].items():raw=z.read(name);assert len(raw)==row['bytes'] and hashlib.sha256(raw).hexdigest()==row['sha256']
ref={'file':asset.name,'sha256':sha(asset),'bytes':asset.stat().st_size,'members':len(manifest['files'])+1,'url':'https://github.com/lyiming710-cloud/GodsPVZ-iOS/releases/download/native28-codespace-2026-10-01/'+asset.name,'parent_archives':manifest['parent_archives']};(out/'EVIDENCE-ARCHIVE.json').write_text(json.dumps(ref,indent=2));(v/'SHA256.json').write_text(json.dumps({p.name:sha(p) for p in sorted(v.iterdir()) if p.name!='SHA256.json'},indent=2));print(json.dumps(ref));print('STATUS',subprocess.check_output(['git','status','--porcelain'],cwd=w,text=True))
