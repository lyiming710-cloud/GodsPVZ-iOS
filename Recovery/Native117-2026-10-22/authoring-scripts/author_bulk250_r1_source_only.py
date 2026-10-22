"""Copy manifest-bound sources, author eight scoped repairs; never build or test."""
from pathlib import Path
import json, hashlib, shutil, re
R=Path(__file__).resolve().parents[1]
OLD=R/'audit/native-bulk250-codex-2026-10-22'
OUT=R/'audit/native-bulk250-r1-codex-2026-10-22'
E=R/'audit/native116-bulk250-r1-codex-2026-10-22'
sha=lambda b:hashlib.sha256(b).hexdigest()
read=lambda p:json.loads(p.read_text('utf-8-sig'))
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,ensure_ascii=False,indent=2)+'\n')
OUT.mkdir(exist_ok=True)
# Only frozen source-manifest entries, never the AGY evidence appended later.
for f in read(OLD/'SOURCE-MANIFEST.json')['files']:
    p=OLD/f['path'];data=p.read_bytes()
    if sha(data)!=f['sha256']:raise ValueError('Frozen source delivery changed: '+f['path'])
    q=OUT/f['path'];q.parent.mkdir(parents=True,exist_ok=True);q.write_bytes(data)
changes=[]
def change(file,method,token,replacements,reason):
    p=OUT/file;text=p.read_text('utf-8');start=text.index('    private static byte[] '+method+'(')
    end=text.find('\n    private static ',start+1);end=len(text) if end<0 else end
    body=text[start:end];before=body
    for old,new,n in replacements:
        if body.count(old)!=n:raise ValueError((method,old,body.count(old),n))
        body=body.replace(old,new)
    write(p,text[:start]+body+text[end:])
    changes.append(dict(token=token,source_file=file,emitter=method,reason=reason,old_source_body_sha256=sha(before.encode()),new_source_body_sha256=sha(body.encode()),expected_candidate_body_change=True))
change('N114.MethodBodies.cs','N114_LightSaberExplosionBody','0x060003F4',[
    ('.Static(0x04000278)','.Call(0x060001E5)',1)],'Use the existing public getter with proven static-field-return native body; no private cross-type field access.')
change('N115.MethodBodies.cs','N115_BenchmarkBody','0x06000753',[
    ('b.Load(3).Integer(1).Virtual(0x0A000304)','b.Load(3).Ref(0x74,0x0100005E).Integer(1).Virtual(0x0A000304)',1)],'Cast the TMP_Text local to the actual TextMeshPro created in this branch before its derived-only setter. Other branches retain the base local type.')
change('N116.MethodBodies.cs','N116_SaveCompleteness','0x060004BF',[
    ('0x0A0000C2','0x0A00023A',1),('0x0A0000C3','0x0A000233',4)],'Use existing List<Supplies> Count/Item references for the Supplies field; preserve sorting and callback order.')
change('N116.MethodBodies.cs','N116_LoadReward','0x06000163',[
    ('0x0A0000BF','0x0A00027F',2)],'Both Add receivers are List<Supplies>; use its existing canonical !0 member, not List<Buff>.Add.')
change('N116.MethodBodies.cs','N116_GridCanPlace','0x06000297',[
    ('b.Load(1).Arg(1).Field(0x04000473).Virtual(0x06000311)','b.Load(1).Arg(1).Field(0x04000473).Ref(0x72,0x7000AE84).Virtual(0x06000311)',1)],'Supply the original device literal, independently mapped from native r8 metadata cell and the immutable original managed string token.')
change('N116.MethodBodies.cs','N116_NutTaunt','0x06000384',[
    ('.Static(0x04000278)','.Call(0x060001E5)',1),
    ('b.Load(1).Virtual(0x0600046A);','b.Load(1).Virtual(0x0600046A).Op(0x26);',1)],'Discard Path_Finding bool return before the loop merge; use the public particle getter. Preserve the native path and abs/Int32.MinValue behavior.')
change('N116.MethodBodies.cs','N116_Awake148','0x06000514',[
    ('.Static(0x04000278)','.Call(0x060001E5)',1)],'Same observed private particle-field mechanism, corrected in this existing target using the proven getter.')
change('N116.MethodBodies.cs','N116_PlantParticleAudio','0x0600035B',[
    ('.Static(0x04000278)','.Call(0x060001E5)',2)],'Same private particle field at two existing target sites; preserve both separate reads and original side-effect order.')
p=read(OUT/'IMPLEMENTATION-PLAN.json')
history={k:p.get(k) for k in ['schema','new_unique_source_methods_this_turn','revision_changed_method_tokens','revision_changed_method_count','comparison_to_identified_bulk200']}
tokens=[x['token'] for x in changes]
p.update(schema='bulk250-r1-source-implementation-v1',state='SOURCE_REPAIRS_WRITTEN_PENDING_AGY_EXECUTION',source_revision='Bulk250-R1',candidate=None,new_unique_source_methods_this_turn=0,new_qualified_methods=0,new_accepted_methods=0,root_executed_project_tests=False,revision_changed_method_tokens=tokens,revision_changed_method_count=len(tokens),historical_predecessor_plan=history,source_predecessor=OLD.relative_to(R).as_posix(),comparison_to_identified_bulk250={'candidate_sha256':'ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8','expected_changed_bodies':8,'expected_unchanged_bodies':2309,'not_executed':True},comparison_to_identified_bulk200={'candidate_sha256':'667e032fe7512f471bcc891c80fc81fd22d27e0ce0ae675bb226f7bbe0e00bc0','expected_changed_bodies':52,'expected_unchanged_bodies':2265,'inherited_changed_tokens':['0x060003F4','0x06000753'],'not_executed':True},metadata_changed_by_revision=False,previous_batch_gate='FAIL_PENDING_RETEST',next_bulk300_execution='HOLD_UNTIL_PREVIOUS_BATCH_REVIEW')
dump(OUT/'IMPLEMENTATION-PLAN.json',p)
dump(OUT/'SOURCE-LINEAGE.json',dict(schema='bulk250-r1-source-lineage-v1',parent_sha256=p['parent_sha256'],candidate_parent_is_native93=True,source_predecessor=OLD.relative_to(R).as_posix(),original_bulk250_source_manifest_sha256=sha((OLD/'SOURCE-MANIFEST.json').read_bytes()),original_bulk250_candidate_sha256='ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8',expected_total_method_bodies=250,repair_tokens=tokens,metadata_changed=False,tests_inherited=False,new_acceptance_inherited=False,candidate=None))
dump(E/'SOURCE-REPAIR-CHANGELOG.json',dict(schema='bulk250-r1-source-repairs-v1',state='SOURCE_ONLY_NOT_TESTED',original_reported_defective_methods=6,additional_same_mechanism_existing_methods=2,changed_method_sources=8,changes=changes,metadata_changes=0,candidate_constructed=False,project_tests_executed=False,formal_acceptance=False))
shutil.copyfile(E/'SOURCE-API-EVIDENCE.json',OUT/'SOURCE-API-EVIDENCE.json')
shutil.copyfile(E/'SOURCE-REPAIR-CHANGELOG.json',OUT/'SOURCE-REPAIR-CHANGELOG.json')
write(OUT/'README.md','# Bulk250 R1: eight source repairs, awaiting AGY retest\n\n250 existing target bodies; 244 direct +6 companions. Eight method sources differ from the first Bulk250 source bundle. No new method targets, metadata rows, candidate or acceptance. Construct from fixed Native93, never patch the previous candidate. Previous Bulk250 is FAIL; Bulk300 testing remains on hold. See VERIFY-NEXT.md and SOURCE-REPAIR-CHANGELOG.json.\n')
print(json.dumps(dict(scope='SOURCE_FILE_AUTHORING_ONLY',changed_method_sources=len(changes),aggregate_targets=250,candidate_constructed=False,project_tests_executed=False)))
