"""Archive source-only progress and update a new registry version; no target execution."""
from pathlib import Path
import hashlib
import json
import shutil
import zipfile

ROOT=Path(__file__).resolve().parents[1]
SOURCE=ROOT/'audit/native-bulk388-source-r1-2026-10-23'
N120=ROOT/'audit/native120-codex-2026-10-23'
REPO=ROOT/'audit/native111-publication-2026-10-20/repository'
DEST=REPO/'Recovery/Native120-Bulk388-Source-2026-10-23'
sha=lambda b:hashlib.sha256(b).hexdigest()
read=lambda p:json.loads(p.read_text('utf-8-sig'))
def dump(p,x):p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+'\n',encoding='utf-8',newline='\n')
def freeze(directory):
    dump(directory/'SOURCE-MANIFEST.json',dict(schema='source-only-manifest-v1',state='SOURCE_ONLY_UNBUILT_UNEXECUTED',
        date='2026-10-23',candidate=None,new_qualified_methods=0,root_ran_project_tests_this_turn=False,
        files=[dict(path=p.relative_to(directory).as_posix(),bytes=p.stat().st_size,sha256=sha(p.read_bytes()))
            for p in sorted(directory.rglob('*')) if p.is_file() and p!=directory/'SOURCE-MANIFEST.json']))

prior=ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v22-2026-10-22.json'
registry=read(prior)
registry.update(schema='native381-bulk-work-registry-v23',date='2026-10-23',
    previous_registry_sha256=sha(prior.read_bytes()),root_ran_project_tests_this_turn=False,
    new_source_methods_this_turn=1,additional_source_batch=N120.relative_to(ROOT).as_posix(),
    aggregate_source_bundle=SOURCE.relative_to(ROOT).as_posix(),aggregate_method_body_targets=388,
    source_proposals_including_companions=388,source_count_breakdown=dict(historical_direct=381,explicit_companions=6,outside_historical_direct_list=1),
    new_unwritten_methods_outside_historical381=0,
    new_header_only_outside_historical_worklist=[dict(token='0x060003F9',name='Projectile.SetSortingGroup',state='SOURCE_WRITTEN_UNBUILT_UNEXECUTED')],
    native119_aggregate_source_composition_completed=True,aggregate_candidate_constructed=False,
    source_aggregate_compiled=False,new_qualified_methods=0,
    current_root_assignment='SOURCE_AUTHORING_AND_ARCHIVAL_ONLY_NO_TESTS',
    next_scope_testing='AWAIT_INDEPENDENT_BULK250_R2_TOOL_CORRECTION_THEN_COORDINATOR_HANDOFF',
    latest_source_checkpoint='Recovery/Native120-Bulk388-Source-2026-10-23',
    latest_metadata_writer_source='EXPLICIT_ALL_PRESENT_TABLE_COLUMN_TRANSCODE_UNBUILT_UNEXECUTED')
for row in registry.get('additional_attributed_methods',[]):
    if row.get('token')=='0x060003F9':
        row.update(source_state='SOURCE_WRITTEN_UNBUILT_UNEXECUTED',source_batch=N120.name,semantic_status='NOT_QUALIFIED')
regpath=ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v23-2026-10-23.json'
dump(regpath,registry)
plan=read(SOURCE/'IMPLEMENTATION-PLAN.json');meta=read(SOURCE/'METADATA-ADDITIONS.json')
status=dict(schema='native120-bulk388-source-checkpoint-v1',date='2026-10-23',state='SOURCE_ONLY_UNBUILT_UNEXECUTED',
    source_method_count=388,new_source_methods_this_turn=1,source_methods_added_after_bulk350=38,
    declared_non_target_method_count=1929,declared_finally_regions=plan['expected_finally_regions'],
    expected_blob_heap_bytes=meta['expected_blob_heap_bytes'],old_r6_blob_bytes=65075,
    expected_blob_index_width_transition=[2,4],old_bulk350_guard_preserved=True,
    candidate=None,project_compiled=False,patcher_executed=False,project_tests_executed=False,
    new_workflows_started=False,new_qualified_methods=0,
    latest_actual_candidate=dict(scope=350,sha256='ef95567ce714146b2f7aed1a92e98798023090bb47757f6c5b377f9ce6d9c0f4',
        run_id=39790796339,source_date='2026-10-22',il2cpp='PASS_EXIT_0',game_cpp='FAIL_14_OF_16_TUS_1616_ERRORS',
        note='Historical observed result, not rerun in this turn'),
    external_verifier_assignment='AGY Flash: Bulk250-R2 actual candidate CIL execution and real mutants; no automatic handoff of Bulk388')
dump(SOURCE/'CHECKPOINT-STATUS.json',status)
freeze(N120);freeze(SOURCE)
DEST.mkdir(exist_ok=False)
shutil.copytree(SOURCE,DEST/'source388')
shutil.copytree(N120,DEST/'Native120-source-evidence')
(DEST/'authoring-scripts').mkdir()
for filename in ['read_native120_source_only.py','compose_bulk388_source_only.py','archive_native120_source_only.py']:
    shutil.copyfile(ROOT/'audit'/filename,DEST/'authoring-scripts'/filename)
shutil.copyfile(regpath,DEST/'NATIVE381-BULK-WORK-REGISTRY-v23.json')
dump(DEST/'CHECKPOINT-STATUS.json',status)
(DEST/'README.md').write_text('''# Native120 / Bulk388 source checkpoint

2026-10-23. **Source only, uncompiled, unexecuted. No new candidate, tests, workflow dispatch, IPA or qualification.**

The author completed the newly discovered `0x060003F9 Projectile.SetSortingGroup` source from original PC native instructions. The historical 381 direct worklist entries, six companions and this additional method now have 388 source proposals. Native119's previous 37 sources are now composed into the aggregate source; they have still not been inserted into a constructed candidate DLL.

`source388/` contains the complete standalone patcher source project and explicit scope/metadata plans. It inherits actual corrected Bulk350 R6 source. The old short-index metadata writer remains unchanged. A separate full-column writer source handles the expected 65,075 → 66,497 byte #Blob transition; it is not a weakened guard. Its source declares 388 target bodies, 1,929 non-target MethodDefs and 201 finally regions. These are input requirements to verify, not observed PASS counts.

`Native120-source-evidence/` contains the original native disassembly, managed source readout, field offsets and the new emitter. An initial ambiguous-token read from another image is retained as explicitly withdrawn; active evidence pins image Assembly-CSharp.dll and native VA 0x18037C5E0. The original 37 methods' evidence remains in the earlier Native119-Bulk350 recovery directory in this branch history.

Read `source388/VERIFY-NEXT.md` for the independent verification backlog. The current AGY verifier remains assigned to correcting Bulk250-R2's behavior runner. Its prior 28-PASS report is rejected as candidate behavior evidence. This source checkpoint does not hand off a new execution task or widen the formal accepted scope.

The last actually built candidate is Bulk350 R6 (SHA ef95567ce714146b2f7aed1a92e98798023090bb47757f6c5b377f9ce6d9c0f4), from historical Actions 39790796339: IL2CPP exit 0, Clang still failed in 14/16 game TUs. No new conversion or compilation result is claimed here.

The authoring scripts are archived for provenance and assume the original workspace/audit directory layout. To build the future patcher, the self-contained source388/Bulk388.csproj does not need those authoring scripts. SOURCE-MANIFEST.json lists the delivered source bytes; it is an identity inventory, not a correctness certificate.
''',encoding='utf-8',newline='\n')
(DEST/'.gitattributes').write_text('** -text\n',encoding='utf-8',newline='\n')
freeze(DEST)
assets=ROOT/'audit/native120-publication-2026-10-23'
assets.mkdir(exist_ok=True)
archive=assets/'NATIVE120-BULK388-SOURCE-ONLY-R1.zip'
with zipfile.ZipFile(archive,'x',compression=zipfile.ZIP_DEFLATED,compresslevel=6) as z:
    for p in sorted(DEST.rglob('*')):
        if p.is_file():z.write(p,p.relative_to(DEST).as_posix())
dump(assets/'ASSET-MANIFEST.json',dict(state='SOURCE_ONLY_UNBUILT_UNEXECUTED',files=[dict(name=archive.name,bytes=archive.stat().st_size,sha256=sha(archive.read_bytes()))]))
print(json.dumps(dict(scope='SOURCE_AND_HISTORY_ARCHIVAL_ONLY',recovery_path=str(DEST),archive=str(archive),bytes=archive.stat().st_size,sha256=sha(archive.read_bytes()),new_qualified_methods=0)))
