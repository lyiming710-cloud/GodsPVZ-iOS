"""Prepare two distinct source deliveries; never build or execute project code."""
from pathlib import Path
import json,hashlib,shutil,zipfile
R=Path(__file__).resolve().parents[1]
PUB=R/'audit/bulk250-r2-publication-2026-10-22'
DEST=R/'audit/native111-publication-2026-10-20/repository/Recovery/Bulk250-R2-2026-10-22'
PUB.mkdir(exist_ok=False);DEST.mkdir(exist_ok=False)
read=lambda p:json.loads(p.read_text('utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def dump(p,x):p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+'\n',encoding='utf-8',newline='\n')
(DEST/'.gitattributes').write_text('* -text\n**/* -text\n',encoding='utf-8')
extras=['author_bulk250_r2_source_only.py','compose_bulk300_r2_held_source_only.py','package_bulk250_r2_sources_2026_10_22.py','NATIVE381-BULK-WORK-REGISTRY-v19-2026-10-22.json','NEXT50-AFTER-BULK250-R2-2026-10-22.json']
for f in extras:shutil.copyfile(R/'audit'/f,DEST/f)
deliveries=[]
def package(label,role,folders):
    name=label+'-SOURCE.zip';archive=PUB/name;files=[]
    with zipfile.ZipFile(archive,'x',compression=zipfile.ZIP_DEFLATED,compresslevel=9) as z:
        for src,prefix in folders:
            for f in [x['path'] for x in read(src/'SOURCE-MANIFEST.json')['files']]+['SOURCE-MANIFEST.json']:
                p=src/f;data=p.read_bytes();q=DEST/prefix/f;q.parent.mkdir(parents=True,exist_ok=True);q.write_bytes(data)
                arc=(Path(prefix)/f).as_posix();z.writestr(arc,data);files.append(dict(path=arc,bytes=len(data),sha256=sha(data)))
        for f in extras:
            data=(DEST/f).read_bytes();z.writestr(f,data);files.append(dict(path=f,bytes=len(data),sha256=sha(data)))
    p=read(folders[0][0]/'IMPLEMENTATION-PLAN.json')
    out=dict(schema='bulk250-r2-source-delivery-v1',date='2026-10-22',role=role,state='SOURCE_ONLY_NOT_ACCEPTANCE',primary_project_tests_executed=False,primary_candidate_constructed=False,new_formal_acceptance=0,parent_sha256=p['parent_sha256'],method_sources=p['method_body_targets'],new_unique_method_sources_this_revision=0,revised_existing_method_sources=2,metadata_changes_by_revision=0,source_manifest_sha256=sha((folders[0][0]/'SOURCE-MANIFEST.json').read_bytes()),archive=dict(name=name,bytes=archive.stat().st_size,sha256=sha(archive.read_bytes())),files=files)
    fname=label+'-DELIVERY-MANIFEST.json';dump(PUB/fname,out);dump(DEST/fname,out)
    deliveries.append(dict(role=role,archive=out['archive'],source_manifest_sha256=out['source_manifest_sha256'],delivery_manifest=fname,delivery_manifest_sha256=sha((PUB/fname).read_bytes())))
package('BULK250-R2-RETEST','RETEST_PREVIOUS_SCOPE_ONLY',[(R/'audit/native-bulk250-r2-codex-2026-10-22','source250-r2'),(R/'audit/native116-bulk250-r2-codex-2026-10-22','repair-evidence')])
package('BULK300-R2-HELD','SAVE_ONLY_DO_NOT_EXECUTE',[(R/'audit/native-bulk300-r2-codex-2026-10-22','source300-held-r2')])
index=dict(schema='bulk250-r2-delivery-index-v1',date='2026-10-22',previous_batch_run=39703773021,previous_batch_candidate_sha256='dceb44b834889ec6ebcd36baaadbf0b2062fc41982f3d3b64144461a99338996',previous_batch_acceptance='FAIL_MEMBER_ACCESS',dispatch_now='BULK250 R2 ONLY',held_scope='Native117/Bulk300',new_source_method_count=0,new_formal_acceptance=0,deliveries=deliveries)
dump(DEST/'DELIVERY-INDEX.json',index);dump(PUB/'DELIVERY-INDEX.json',index)
readme='''# Bulk250 R2: two source repairs; next fifty remain saved — 2026-10-22

**Bulk250 R1 has not passed acceptance. This delivery repairs its remaining reported private-member accesses and requests R2 retest only. Native117/Bulk300 remains held.** The primary author ran no project tests, generated no candidate, and dispatched no workflow. Two existing method sources changed; no new target method or metadata row was added.

[AGY run39703773021](https://github.com/lyiming710-cloud/GodsPVZ-iOS/actions/runs/39703773021) constructed R1 candidate dceb44b834889ec6ebcd36baaadbf0b2062fc41982f3d3b64144461a99338996,16573440 bytes. Its recorded construction/isolation/type/reference gates progressed, but the full250-method member-access report is pass=false,3 rejected sites,0 unresolved. Those sites are in0600001C CreateNewEnemy and06000183 DisplayBan_device. Raw reports and the prior private-field negative control are archived under repair-evidence without re-executing them.

## Source repairs

- CreateNewEnemy: two private BoardConfig.map reads become calls to existing public BoardConfig.Map06000158. Keep separate map reads and the original GetMapX/GetMapY/order.
- DisplayBan_device: one private Device.animationSprites read becomes a call to existing public Device.AnimationSprites06000303. Keep nested enumeration, two finally regions and rendering writes.

Both accessors return the exact same field with no added allocation/caching/guard, proven by saved current CIL and original PC native bytes at18030EF70 and180346070. Field visibility and unaffected own-class accesses stay intact. R1's eight earlier method repairs are retained.

## Retest and hold packages

`source250-r2/` / **BULK250-R2-RETEST-SOURCE.zip** is the only execution scope now. Build Bulk250.csproj twice from the fixed Native93 parent b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162,16033792 bytes. Proposed expectations:2 changed/2315 identical relative to actualR1 dceb44b8...,10/2307 relative to original250 ad25359a...,52/2265 relative to actualBulk200 667e032f.... Aggregate scope250 allowed/2067 other bodies. These are expectations, not fresh observations. Metadata totals remain1232/1958/176/191 (MemberRef/local/TypeSpec/MethodSpec),85 finally clauses.

`source300-held-r2/` / **BULK300-R2-HELD-SOURCE.zip** saves the same next50 new sources plus the two inherited R2 fixes.300=294 direct worklist entries+6 companions;87 direct entries remain without new source. No new unique method was authored in this revision. Expected difference to an actual correctedR2 candidate is50 bodies, toR1 is52, to original250 is60;2017 aggregate non-target bodies must remain unchanged. Construction plan1259/2008/176/191 metadata rows and131 finally clauses is unchanged. Native117's complete80-method read-only evidence set remains available at [the frozen source checkpoint](https://github.com/lyiming710-cloud/GodsPVZ-iOS/tree/b918382f6b741e045140d4dda8bba9dcc378fa72/Recovery/Native117-2026-10-22/native117). Do not build/test this held scope until preceding-batch acceptance is reviewed.

## Corrections required in the preceding report

AGY reported IL2CPP exit0 and18 Game-TU Clang checks with3463 errors/137 failing methods/2130 clean methods/44 included-file errors. This is compiler progress under an explicitly bypassed blocked-dependency profile, not whole-project/platform acceptance. The claim that all250 targets are clean because137 equals the unwritten-in-that-candidate count is not established by those totals. Deliver actual failing-token and target-token intersection/difference, with unresolved/ambiguous attributions and included-file errors retained. Explain the2267-method compiler denominator versus2317 managed MethodDefs. v18 actually records294 direct sources and87 unwritten;137 pertains to the tested250 aggregate,244 direct sources.

Execute the applicable existing gates through AGY/GitHub workflows, including baseline-pass/mutant-fail member-access controls and actual candidate-bound normal/exception/native-versus-CIL behavior for the preceding50. Follow source250-r2/VERIFY-NEXT.md. Do not stop after construction, weaken frozen checks, mark all-NRE fixtures or signal999 qualified, or promote Linux SEH-unmodeled behavior to PASS.01D3→01E0→0677 UNPROVEN and Windows SEH UNSUPPORTED remain visible. No new formal acceptance, Unity/Xcode/device pass or IPA is claimed. Codespace stays stopped; main/production and previous histories remain unchanged.
'''
(DEST/'README.md').write_text(readme,encoding='utf-8',newline='\n');(PUB/'RELEASE-NOTES.md').write_text(readme,encoding='utf-8',newline='\n')
print(json.dumps(dict(destination=str(DEST),deliveries=deliveries,project_tests_executed=False),indent=2))
