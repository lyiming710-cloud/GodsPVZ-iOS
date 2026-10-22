"""Synchronize the saved +50 source checkpoint with two R2 repairs; no execution."""
from pathlib import Path
import json,hashlib,shutil
R=Path(__file__).resolve().parents[1]
S=R/'audit/native-bulk250-r2-codex-2026-10-22';E=R/'audit/native116-bulk250-r2-codex-2026-10-22'
OLD=R/'audit/native-bulk300-codex-2026-10-22';OUT=R/'audit/native-bulk300-r2-codex-2026-10-22'
read=lambda p:json.loads(p.read_text('utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,ensure_ascii=False,indent=2)+'\n')
def freeze(d):dump(d/'SOURCE-MANIFEST.json',dict(schema='source-only-manifest-v1',date='2026-10-22',state='SOURCE_ONLY_UNBUILT_UNEXECUTED',candidate=None,root_executed_project_tests=False,files=[dict(path=p.relative_to(d).as_posix(),bytes=p.stat().st_size,sha256=sha(p.read_bytes())) for p in sorted(d.rglob('*')) if p.is_file() and p.name!='SOURCE-MANIFEST.json']))
# Copy exact manifest-bound source records; never copy later AGY execution trees.
OUT.mkdir(exist_ok=False)
for f in read(OLD/'SOURCE-MANIFEST.json')['files']:
    p=OLD/f['path'];data=p.read_bytes()
    if sha(data)!=f['sha256']:raise ValueError('Frozen Bulk300 delivery differs: '+f['path'])
    q=OUT/f['path'];q.parent.mkdir(parents=True,exist_ok=True);q.write_bytes(data)
shutil.copyfile(S/'N116.MethodBodies.cs',OUT/'N116.MethodBodies.cs')
shutil.copyfile(E/'PUBLIC-GETTER-SOURCE-EVIDENCE.json',OUT/'R2-PUBLIC-GETTER-SOURCE-EVIDENCE.json')
p=read(OUT/'IMPLEMENTATION-PLAN.json');r=read(S/'IMPLEMENTATION-PLAN.json')
p.update(schema='bulk300-r2-held-source-implementation-v1',state='SOURCE_ONLY_HELD_NOT_EXECUTED',source_revision='Bulk300-Held-R2',candidate=None,new_unique_source_methods_this_turn=0,new_source_method_count_after_bulk250=50,source_predecessor=OLD.relative_to(R).as_posix(),inherited_source_predecessor=S.relative_to(R).as_posix(),inherited_r2_repair_tokens=r['revision_changed_method_tokens'],inherited_cumulative_repair_tokens=r['cumulative_revision_changed_method_tokens'],inherited_cumulative_repair_count=10,previous_50_report_reviewed=True,previous_50_run_id=39703773021,previous_50_status='BULK250_R1_MEMBER_ACCESS_FAIL_R2_RETEST_PENDING',next_scope_testing='HOLD_UNTIL_BULK250_R2_ACCEPTANCE_REVIEW',comparison_to_bulk250_r2={'candidate_sha256':None,'expected_changed_bodies':50,'expected_unchanged_bodies':2267,'identify_after_agy_retest':True,'not_executed':True},comparison_to_bulk250_r1={'candidate_sha256':'dceb44b834889ec6ebcd36baaadbf0b2062fc41982f3d3b64144461a99338996','expected_changed_bodies':52,'expected_unchanged_bodies':2265,'not_executed':True},comparison_to_identified_bulk250={'candidate_sha256':'ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8','expected_changed_bodies':60,'expected_unchanged_bodies':2257,'not_executed':True},comparison_to_previous_held_bulk300={'expected_changed_bodies':2,'expected_unchanged_bodies':2315,'no_previous_candidate_exists':True,'not_executed':True},root_executed_project_tests=False,new_qualified_methods=0)
p.update(revision_changed_method_tokens=r['revision_changed_method_tokens'],revision_changed_method_count=2,cumulative_revision_changed_method_tokens=r['cumulative_revision_changed_method_tokens'],cumulative_revision_changed_method_count=10,previous_batch_gate='R1_MEMBER_ACCESS_FAIL_RETEST_R2',supersedes_source_bundle=S.relative_to(R).as_posix())
dump(OUT/'IMPLEMENTATION-PLAN.json',p)
dump(OUT/'SOURCE-LINEAGE.json',dict(schema='bulk300-held-r2-source-lineage-v1',parent_sha256=p['parent_sha256'],candidate_parent_is_native93=True,source_predecessor=OLD.relative_to(R).as_posix(),inherited_source_predecessor=S.relative_to(R).as_posix(),new_unique_method_sources_after_bulk250=50,new_method_sources_this_revision=0,expected_total_method_bodies=300,inherited_r2_repair_tokens=r['revision_changed_method_tokens'],expected_finally_regions=131,candidate=None,tests_inherited=False,new_acceptance_inherited=False,testing='HOLD'))
oldverify=(OUT/'VERIFY-NEXT.md').read_text('utf-8')
fidelity=oldverify[oldverify.index('## Fidelity and meaningful test priorities'):]
head='''# Native117 / Bulk300 Held R2 — fifty saved source methods, DO NOT EXECUTE

The same50 new unique sources remain saved; total300=294 historical worklist entries+6 companions,87 direct entries unwritten. No new method was added in this revision. Only two inherited N116 caller bodies are synchronized with Bulk250 R2, using proven existing public Map and AnimationSprites accessors. Metadata and131 finally clauses remain as planned, not freshly executed evidence.

Previous Bulk250 R1 run39703773021 has3 private-field rejections in2 methods. Bulk250 R2 authoring repairs those sites. **Execute Bulk250 R2 first and wait for primary-author acceptance review; no build or test of this held bundle yet.** Count-only claims that250 targets are compile-clean remain unproven until an explicit failing-token/target intersection is delivered.

Once released: construct once from fixed Native93 b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162 (16033792B). Expected differences to actual corrected Bulk250 R2:50 changed/2267 identical; to actual R1 dceb44b8...:52/2265; to original250 ad25359a...:60/2257. Aggregate isolation300 allowed/2017 unchanged others. These are construction plans, not observations.

Metadata plan totals1259 MemberRef/2008 StandAloneSig/176 TypeSpec/191 MethodSpec,131 finally clauses. The925 additional #Blob bytes estimate relative to recorded63154 yields64079, but AGY must measure actual heap widths and keep short-index guards intact. Fifty bodies and full source fidelity evidence remain from the frozen Native117 delivery at b918382f6b741e045140d4dda8bba9dcc378fa72; do not edit that history or reinterpret80 inspected evidence methods as80 targets. R2 accessor proof is included separately.

'''
write(OUT/'VERIFY-NEXT.md',head+fidelity)
write(OUT/'README.md','# Native117 / Bulk300 Held R2\n\nSame50 saved new sources; aggregate300=294 direct+6 companions,87 direct entries remain unwritten. Two inherited callers now include Bulk250 R2 public-accessor repairs. No new candidate, project test, acceptance or IPA. Execute previous Bulk250 R2 first, wait for review; this scope stays held. See VERIFY-NEXT.md.\n')
write(S/'README.md','# Bulk250 R2 — two source repairs, awaiting AGY retest\n\nTwo existing methods (0600001C,06000183),three private-field sites,now use existing proven public accessors. Total250 targets unchanged; no metadata additions, visibility changes or candidate. All8 R1 source fixes remain. Construct from Native93; compare to actual R1 for2 changed/2315 identical. Bulk300 stays held. See VERIFY-NEXT.md and SOURCE-REPAIR-CHANGELOG-R2.json.\n')
ledger=read(R/'audit/NEXT50-AFTER-BULK250-2026-10-22.json')
ledger.update(schema='next50-source-ledger-r2-v1',previous_50_report_reviewed=True,previous_50_run_id=39703773021,previous_50_status='BULK250_R1_MEMBER_ACCESS_FAIL_R2_RETEST_PENDING',new_scope_testing='HOLD_UNTIL_BULK250_R2_ACCEPTANCE_REVIEW',source_aggregate=OUT.relative_to(R).as_posix(),source_predecessor=S.relative_to(R).as_posix(),completed_new_unique_method_sources=50,remaining_new_method_sources=0,previous_ledger_sha256=sha((R/'audit/NEXT50-AFTER-BULK250-2026-10-22.json').read_bytes()))
dump(R/'audit/NEXT50-AFTER-BULK250-R2-2026-10-22.json',ledger)
reg=read(R/'audit/NATIVE381-BULK-WORK-REGISTRY-v18-2026-10-22.json')
for x in reg['methods']:
    if x['token'] in r['revision_changed_method_tokens']:x.update(inherited_source_revision='Bulk250-R2',inherited_revision_verification='PENDING_AGY_RETEST',new_semantic_acceptance=False)
reg.update(schema='native381-bulk-work-registry-v19',date='2026-10-22',source_written_pending_methods=294,direct_methods_without_new_source=87,aggregate_source_bundle=OUT.relative_to(R).as_posix(),aggregate_method_body_targets=300,new_source_methods_this_turn=0,inherited_revision_changed_source_methods=10,previous_50_run_id=39703773021,previous_50_status='BULK250_R1_MEMBER_ACCESS_FAIL_R2_RETEST_PENDING',next_scope_testing='HOLD_UNTIL_BULK250_R2_ACCEPTANCE_REVIEW',previous_registry_sha256=sha((R/'audit/NATIVE381-BULK-WORK-REGISTRY-v18-2026-10-22.json').read_bytes()),new_qualified_methods=0)
dump(R/'audit/NATIVE381-BULK-WORK-REGISTRY-v19-2026-10-22.json',reg)
for token in ['0x0600001C','0x06000183']:
    for suffix in ['-current-il.txt','-original.asm']:shutil.copyfile(R/'audit/native116-codex-2026-10-22'/(token+suffix),E/(token+suffix))
freeze(S);freeze(E);freeze(OUT)
print(json.dumps(dict(scope='SOURCE_CHECKPOINT_SYNCHRONIZATION_ONLY',new_unique_method_sources=0,previous_fifty_source_count=50,aggregate_method_sources=300,project_tests_executed=False,candidate_constructed=False)))
