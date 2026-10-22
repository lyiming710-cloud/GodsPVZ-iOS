"""Publishable source/archive preparation only; no build, candidate, or project tests."""
from pathlib import Path
import hashlib,json,shutil,zipfile
R=Path(__file__).resolve().parents[1]
PUB=R/'audit/native117-publication-2026-10-22'
REPO=R/'audit/native111-publication-2026-10-20/repository'
DEST=REPO/'Recovery/Native117-2026-10-22'
PUB.mkdir(exist_ok=False);DEST.mkdir(exist_ok=False)
sha=lambda b:hashlib.sha256(b).hexdigest()
read=lambda p:json.loads(p.read_text('utf-8-sig'))
def dump(p,x):p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+'\n',encoding='utf-8',newline='\n')
(DEST/'.gitattributes').write_text('* -text\n**/* -text\n',encoding='utf-8')
scripts=['author_bulk250_r1_source_only.py','read_bulk250_repair_source_evidence.py','archive_bulk250_report_review_source_only.py','finalize_native117_source_delivery.py','declare_native117_source_only.py','compose_native117_source_only.py','prepare_native117_evidence_source_only.py','native117_inventory_source_only.py','package_native117_sources_2026_10_22.py']
for name in scripts:
    q=DEST/'authoring-scripts'/name;q.parent.mkdir(exist_ok=True);shutil.copyfile(R/'audit'/name,q)
for name in ['NATIVE381-BULK-WORK-REGISTRY-v18-2026-10-22.json','NEXT50-AFTER-BULK250-2026-10-22.json']:shutil.copyfile(R/'audit'/name,DEST/name)
deliveries=[]
def package(name,role,folders,extra_files,counts):
    archive=PUB/name;entries=[]
    with zipfile.ZipFile(archive,'x',compression=zipfile.ZIP_DEFLATED,compresslevel=9) as z:
        for src,relative in folders:
            target=DEST/relative;target.mkdir(exist_ok=True)
            # Inputs are source-only frozen inventories; no AGY execution trees are copied.
            manifest=read(src/'SOURCE-MANIFEST.json')
            paths=[x['path'] for x in manifest['files']]+['SOURCE-MANIFEST.json']
            for f in paths:
                p=src/f;data=p.read_bytes();q=target/f;q.parent.mkdir(parents=True,exist_ok=True);q.write_bytes(data)
                arc=(Path(relative)/f).as_posix();z.writestr(arc,data)
                entries.append(dict(path=arc,bytes=len(data),sha256=sha(data)))
        for f in extra_files:
            p=DEST/f;data=p.read_bytes();z.writestr(f,data);entries.append(dict(path=f,bytes=len(data),sha256=sha(data)))
    manifest=dict(schema='source-only-delivery-v1',date='2026-10-22',role=role,state='SOURCE_ONLY_NOT_ACCEPTANCE',construction_parent_sha256='b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162',primary_executed_project_tests=False,primary_constructed_candidate=False,new_formal_qualifications=0,counts=counts,archive=dict(name=name,bytes=archive.stat().st_size,sha256=sha(archive.read_bytes())),files=entries)
    label='BULK250-R1' if role=='RETEST_PREVIOUS_SCOPE_ONLY' else 'BULK300-HELD'
    dump(PUB/(label+'-DELIVERY-MANIFEST.json'),manifest);dump(DEST/(label+'-DELIVERY-MANIFEST.json'),manifest)
    deliveries.append(dict(role=role,archive=manifest['archive'],delivery_manifest=label+'-DELIVERY-MANIFEST.json',delivery_manifest_sha256=sha((PUB/(label+'-DELIVERY-MANIFEST.json')).read_bytes())))
package('BULK250-R1-RETEST-SOURCE.zip','RETEST_PREVIOUS_SCOPE_ONLY',[(R/'audit/native-bulk250-r1-codex-2026-10-22','source250-r1'),(R/'audit/native116-bulk250-r1-codex-2026-10-22','repair-evidence')],['authoring-scripts/'+s for s in scripts[:4]],dict(method_sources=250,direct_worklist_entries=244,companions=6,revised_existing_method_sources=8,new_unique_method_sources=0))
package('NATIVE117-BULK300-HELD-SOURCE.zip','SAVE_ONLY_DO_NOT_EXECUTE_UNTIL_PREVIOUS_ACCEPTANCE_REVIEW',[(R/'audit/native-bulk300-codex-2026-10-22','source300-held'),(R/'audit/native117-codex-2026-10-22','native117')],['authoring-scripts/'+s for s in scripts[4:]]+['NATIVE381-BULK-WORK-REGISTRY-v18-2026-10-22.json','NEXT50-AFTER-BULK250-2026-10-22.json'],dict(method_sources=300,direct_worklist_entries=294,companions=6,new_unique_method_sources=50,direct_entries_still_without_new_source=87,inherited_revised_method_sources=8))
dump(DEST/'DELIVERY-INDEX.json',dict(schema='native117-and-bulk250-r1-deliveries-v1',date='2026-10-22',previous_batch_status='FAIL_REQUIRES_RETEST',previous_batch_report_reviewed=True,previous_batch_run_id=39662784115,dispatch_now='BULK250-R1 ONLY',held_scope='NATIVE117/BULK300',new_candidate=None,primary_project_tests_executed=False,deliveries=deliveries))
shutil.copyfile(DEST/'DELIVERY-INDEX.json',PUB/'DELIVERY-INDEX.json')
readme='''# Native117 / Bulk300 saved; Bulk250 R1 retest — 2026-10-22

**The next fifty source methods are complete. The preceding fifty have been reviewed and are not accepted. Retest Bulk250 R1 first; Bulk300 testing is held.**

This delivery contains no newly constructed candidate, project test, workflow dispatch, semantic acceptance or IPA by the primary author. Source300 =294 direct historical worklist entries +6 companions; 87 direct entries remain without newly authored source. These counts do not measure compiler-error reduction or behavior restored.

## Preceding-batch result and repairs

[AGY run39662784115](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39662784115) recorded deterministic original Bulk250 construction, 50 changed bodies relative to actual Bulk200, and 250-target/2067-other-method isolation. Five type-flow failures are present in saved output; AGY additionally reported a private-member access failure. Source repair covers eight existing methods: **06000163, 060004BF, 06000297, 06000384, 06000753, 060003F4, 06000514, 0600035B**. The last two are further instances of the same private particle-field defect. Full changes, immutable source/API/native proof and original failure output are preserved under `repair-evidence/`.

Changes select existing List<Supplies> Add/Count/Item references, supply the original "device" string argument to Device.CanBear, discard Path_Finding's bool return before a loop merge, cast the TextMeshPro-created branch before its derived-only setter, and call the existing public particle getter instead of reading its private backing field across classes. No metadata additions, field-visibility changes or new target methods are proposed by R1.

The restored archive receipt still identifies old candidate8ef0f2bc..., whereas this run's construction/comparison records identifyad25359a.... Both records are retained. AGY must reconcile archive/manifest/candidate/source identities and provide the saved member-access invocation/output before acceptance. Do not erase the erroneous receipt or substitute a final narrative for gate evidence.

## Two separate packages

- **`source250-r1/` and BULK250-R1-RETEST-SOURCE.zip: execute this scope next.** Construct using Bulk250.csproj from fixed Native93, not from a previous candidate. Expected R1 comparison to original Bulk250 is8 changed/2309 identical bodies; comparison to Bulk200 is52 changed/2265 identical (including two inherited fixes). Both are unexecuted expectations. Targets remain250, unchanged other methods2067. Metadata totals stay1232 MemberRef/1958 StandAloneSig/176 TypeSpec/191 MethodSpec;85 finally clauses. Follow `source250-r1/VERIFY-NEXT.md`.
- **`source300-held/`, `native117/` and NATIVE117-BULK300-HELD-SOURCE.zip: save, do not execute yet.** Fifty new bodies plus all corrected inherited source; comparison to an identified R1 candidate should be50 changed bodies, to original Bulk25058. 2017 other bodies must stay unchanged. Metadata plan adds27 MemberRefs/50 local rows, no TypeSpec/MethodSpec/#US; totals1259/2008/176/191,131 finally clauses. The eighty inspected evidence methods are not eighty new targets. Per-family source fidelity concerns and test requirements are in `native117/VERIFY-NEXT.md`.

Native117 covers five TMP coroutines, previews/cards/portraits, events, selection and targeting, rewards/skill/supplies UI, board editing, pathfinding neighbors, combat/buffs and example layout. Preserve original callback order, aliasing, float32 arithmetic/NaN, partial writes, disposal and exceptional paths, including observed game quirks such as CloneTeam's destination-id props loop. Source construction does not prove behavior.

The fixed Native93 construction parent is16033792 bytes,SHA256 `b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162`. Original Bulk250 evidence candidate is16573440 bytes,SHA256 `ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8`; actual Bulk200 SHA256 `667e032fe7512f471bcc891c80fc81fd22d27e0ce0ae675bb226f7bbe0e00bc0`.

AGY owns execution through existing GitHub workflows; Codespace stays stopped. Preserve historical files, frozen validators, main/production and honest FAIL/UNSUPPORTED/UNPROVEN states. Finish applicable gates rather than stopping after constructing a candidate. The primary author reviews preceding-batch acceptance before releasing the next fifty. Linux prechecks, stack flow, isolation and a green job do not imply Unity/Xcode/iOS/IPA acceptance.
'''
(DEST/'README.md').write_text(readme,encoding='utf-8',newline='\n')
(PUB/'RELEASE-NOTES.md').write_text(readme,encoding='utf-8',newline='\n')
print(json.dumps(dict(destination=str(DEST),deliveries=deliveries,primary_project_tests_executed=False),indent=2))
