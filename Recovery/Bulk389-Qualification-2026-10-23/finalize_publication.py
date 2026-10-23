from pathlib import Path
import json,hashlib,shutil,sys,struct,importlib.util
Q=Path(__file__).resolve().parent;R=Q.parents[1];repo=R/'audit/repo-native113';D=repo/'Recovery/Bulk389-Qualification-2026-10-23';sys.path.insert(0,str(Q));import audit_candidate as a
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
def dump(path,obj):path.write_text(json.dumps(obj,indent=2,ensure_ascii=False)+'\n','utf-8')
old=(Q/'candidates/candidate1.dll').read_bytes();new=(Q/'revision-r3/candidates/candidate1.dll').read_bytes();o=a.dnfile.dnPE(data=old);n=a.dnfile.dnPE(data=new);changes=[]
for rid,(x,y) in enumerate(zip(o.net.mdtables.MethodDef.rows,n.net.mdtables.MethodDef.rows),1):
 if not x.Rva:continue
 xo=o.get_offset_from_rva(x.Rva);yo=n.get_offset_from_rva(y.Rva);xl=a.h.parse_raw_method_body_span(old,xo)[0];yl=a.h.parse_raw_method_body_span(new,yo)[0]
 if old[xo:xo+xl]!=new[yo:yo+yl]:changes.append(dict(token=f'0x{0x06000000+rid:08X}',name=str(y.Name),r1_body_sha256=hashlib.sha256(old[xo:xo+xl]).hexdigest(),r3_body_sha256=hashlib.sha256(new[yo:yo+yl]).hexdigest()))
assert len(changes)==9,len(changes)
dump(D/'SOURCE-REVISION-MATRIX.json',dict(r1_candidate_sha256=hashlib.sha256(old).hexdigest(),r3_candidate_sha256=hashlib.sha256(new).hexdigest(),changed_raw_method_bodies=changes,additional_scope_token='0x060003E9',frozen_checks=[dict(path=str(p.relative_to(R)),sha256=sha(p)) for p in [R/'audit/bulk350-root-verification-2026-10-22/reviewed-tools/verify_types_reviewed.py',R/'audit/bulk350-root-verification-2026-10-22/reviewed-tools/exporter-bin/CilExporter.dll',R/'audit/bulk350-root-verification-2026-10-22/reviewed-tools/member-bin/MemberAccess.dll']],width_writer_unchanged=sha(Q/'source/WidthAwareTables.cs')==sha(Q/'revision-r3/source/WidthAwareTables.cs'),legacy_metadata_appender_unchanged=sha(Q/'source/MetadataAppend.cs')==sha(Q/'revision-r3/source/MetadataAppend.cs')))
# Publish the CI preparation source separately because the core ZIP was sealed earlier.
shutil.copy2(Q/'prepare_ci.py',D/'prepare_ci.py');shutil.copy2(Q/'finalize_publication.py',D/'finalize_publication.py')
summary=repo/'Recovery/REMAINING-WORK-2026-10-23.md'
summary.write_text('''# 剩余工作口径，2026-10-23（Bulk389 R3 更新）

| 口径 | 当前可核对状态 | 含义 |
| --- | --- | --- |
| 最后完整编译报错方法 | 38 个 | Bulk350-R6 / 39790796339，2026-10-22；待新候选完整编译替代 |
| 该次 Clang 诊断 | 1,616 条 | 1,592 直接 + 24 头文件；不是独立方法数 |
| 该次失败游戏编译单元 | 14 / 16 | 去重报错方法 38 个 |
| 最新实际候选 | Bulk389 R3 | 原 Bulk388 加已声明的 Collision_Wall 伴随恢复；SHA 9c8cefd7…51ce1c1 |
| 构造／独立隔离 | 通过有限范围门 | 两份逐字节一致；1,928 非目标方法保持原样，37,512 旧数值列保持原值，201 finally |
| 静态类型／成员访问 | 389 / 389；0 拒绝、0 未解析 | 不等于行为或完整 ECMA 验收 |
| 新伴随方法受控原版对照 | 184 正常路径用例通过 | 实际原版机器码与候选实际 CIL；仅 0x060003E9，外部调用使用受控回调；空指针／异常路径未执行 |
| 当前 50 方法 CLR 队列 | 14 通过 / 14 待执行 / 22 上下文缺项 | 属于 Bulk250-R2；另外 2 个辅助 getter 单独计数 |
| IPA | 尚无合格产物 | 本轮未执行持证 Unity／真实 Xcode 或设备门 |

38 个已知编译报错方法的源码方案已经集成到新候选，仍不能在未重新编译时扣减。新编译可能继续暴露其他问题。389 的静态通过数、184 个受控用例以及另一个候选的 50 方法队列不是可相加的资格数。

初始 Bulk388 候选曾为 383 / 388 类型通过且有 6 处非法访问；R2 为 387 / 388 且有 2 处非法访问。最终 R3 才达到当前静态状态；所有失败和修复过程保留在 Bulk389-Qualification-2026-10-23/。新 Linux compiler workflow 仅做诊断，不导出 IPA，也不以旧的 Native109 108-method 归属表作为全游戏行为资格。

详细证据见 [Bulk389 R3 检查点](Bulk389-Qualification-2026-10-23/README.md)。此前 Bulk250-R2 的跨平台受控验证见 Bulk250-R2-Behavior-v7-2026-10-23/；不得将其移植成 Bulk389 行为结论。
''','utf-8')
files=sorted(p for p in D.rglob('*') if p.is_file() and p.name!='DELIVERY-MANIFEST.json');entries=[dict(path=p.relative_to(D).as_posix(),bytes=p.stat().st_size,sha256=sha(p)) for p in files]
external=[repo/'.github/workflows/bulk389-root-compiler-check.yml',repo/'scripts/actions/check_bulk389_compiler.py',repo/'scripts/actions/verify_bulk152_portable.py',repo/'scripts/actions/restore_bulk152_checkpoint.py',summary]
dump(D/'DELIVERY-MANIFEST.json',dict(schema='bulk389-delivery-v1',entries=entries,external_git_paths=[dict(path=p.relative_to(repo).as_posix(),bytes=p.stat().st_size,sha256=sha(p)) for p in external],core_archive_entries_verified=711))
print(json.dumps(dict(files=len(entries),external=len(external),modified_bodies_from_first_candidate=len(changes),total_bytes=sum(x['bytes'] for x in entries))))
