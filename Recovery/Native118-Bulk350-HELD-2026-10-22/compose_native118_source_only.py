"""Compose append-only source checkpoint. No candidate build, CIL execution or verification."""
from pathlib import Path
import json,hashlib,re,shutil
R=Path(__file__).resolve().parents[1];N=R/'audit/native118-codex-2026-10-22';OLD=R/'audit/native-bulk300-r2-codex-2026-10-22'
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,ensure_ascii=False,indent=2)+'\n')
def freeze(d):dump(d/'SOURCE-MANIFEST.json',{'schema':'source-only-manifest-v1','date':'2026-10-22','state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','candidate':None,'root_executed_project_tests':False,'files':[dict(path=p.relative_to(d).as_posix(),bytes=p.stat().st_size,sha256=sha(p.read_bytes())) for p in sorted(d.rglob('*')) if p.is_file() and p.name!='SOURCE-MANIFEST.json']})
v=read(N/'SOURCE-DECLARATIONS.json');oldplan=read(OLD/'IMPLEMENTATION-PLAN.json');oldmeta=read(OLD/'METADATA-ADDITIONS.json')
c=read(N/'CURRENT-SOURCE.json');o=read(N/'ORIGINAL-SOURCE.json');cm={m['token']:m for m in c['methods']};om={m['token']:m for m in o['methods']}
rows=v['rows'];count=len(rows);total=300+count;OUT=R/f'audit/native-bulk{total}-codex-2026-10-22';OUT.mkdir(exist_ok=True)
targets=[];bindings=list(v['bindings']);locs=[]
for i,t in enumerate(rows,1):
    m=cm[t['token']];a=om[t['token']];name=f'N118_Locals{i}';tok=0x11000000+oldplan['standalone_count_after']+i
    bindings.append(dict(name=name,token=tok));locs.append(t['local_blob'])
    targets.append(dict(token=t['token'],name=m['name'],signature=m['signature'],old_code_size=m['code_size'],old_code_sha256=m['il_sha256'],old_max_stack=m['max_stack'],old_local_sig_token=f'0x{m["local_sig"]:08X}',old_local_sig_blob=m['locals_blob'],new_declared_max_stack=t['max_stack'],local_constant=name,local_blob=t['local_blob'],emission_method=t['emission_method'],explicit_companion=False,source_batch=N.name,original_native={k:a[k] for k in ['start','end','sha256','pdata']}))
eh={t['token']:len(t['eh_clauses']) for t in rows if t['eh_clauses']}
colors=''.join('    private static readonly uint[] '+x['name']+'=['+','.join('0x%08Xu'%i for i in x['float32_bits'])+'];\n' for x in v.get('color_constants',[]))
literal_tables=''.join('    private static readonly int[] '+x['name']+'=['+','.join(f'0x{t:08X}' for t in x['values'])+'];\n' for x in v['literal_tables'])
userdefs='    private record N118UserString(int Token,string Value,string EncodedBytes);\n    private static readonly N118UserString[] N118UserStrings=[\n'+''.join('        new(0x%08X,%s,%s),\n'%(x['token'],json.dumps(x['value']),json.dumps(x['encoded_bytes'])) for x in v['user_strings'])+'    ];\n'
write(N/'Bindings.cs','internal static partial class Program\n{\n'+''.join(f'    private const int {x["name"]} = 0x{x["token"]:08X};\n' for x in bindings)+colors+literal_tables+userdefs+'    private static readonly HashSet<int> N118_EhTargets=['+','.join(eh)+'];\n}\n')
dump(N/'IMPLEMENTATION-PLAN.json',dict(schema='native118-source-implementation-v1',state='SOURCE_ONLY_UNBUILT_UNEXECUTED',candidate=None,new_qualified_methods=0,root_executed_project_tests=False,targets=targets,finally_clauses=eh))
nd=dict(schema='native118-source-metadata-v1',member_definitions=v['member_definitions'],local_blobs=locs,constants=bindings,added_typespecs=v['added_typespecs'],added_methodspecs=v['added_methodspecs'],added_user_strings=[],appended_blob_payload_bytes=sum(1+len(bytes.fromhex(x)) for x in locs)+sum(1+len(bytes.fromhex(x['blob'])) for x in v['added_typespecs']),appended_string_payload_bytes=sum(len(x['new_name'].encode())+1 for x in v['member_definitions'] if x['new_name']))
dump(N/'METADATA-ADDITIONS.json',nd)
for p in OLD.glob('*.cs'):shutil.copyfile(p,OUT/p.name)
for f in ['MethodBodies.cs','Bindings.cs']:shutil.copyfile(N/f,OUT/('N118.'+f))
p=(OUT/'Program.cs').read_text('utf-8');pins=[]
for t in targets:
    m=cm[t['token']];owner,name=m['name'].split('::');owner=owner.rsplit('.',1)[-1]
    pins.append(f'        new({t["token"]},"{owner}","{name}","{m["signature"]}",{m["code_size"]},{m["max_stack"]},0x{m["local_sig"]:08X},"{m["locals_blob"]}","{m["il_sha256"]}"),')
at=p.index('];',p.index('private static readonly Pin[] Pins = ['));p=p[:at]+'\n'.join(pins)+'\n    '+p[at:]
at=p.index('};',p.index('var plans = new[] {'));p=p[:at].rstrip()+',\n'+',\n'.join(f'            (Token:{t["token"]}, Code:{t["emission_method"]}, Max:{t["new_declared_max_stack"]}, Locals:{t["local_constant"]})' for t in targets)+'\n        '+p[at:]
p=p.replace('fresh-bulk300.dll',f'fresh-bulk{total}.dll').replace('independent 300-target',f'independent {total}-target')
p=re.sub(r'appended_user_string_count = \d+,',f'appended_user_string_count = {len(oldmeta["user_strings"])+len(v["user_strings"])},',p)
nm=len(v['member_definitions']);nt=len(v['added_typespecs']);ne=sum(eh.values());nms=len(v['added_methodspecs'])
p=re.sub(r'appended_memberref_count = \d+, appended_local_signature_count = \d+,',f'appended_memberref_count = {oldplan["memberref_count_after"]-1100+nm}, appended_local_signature_count = {oldplan["standalone_count_after"]-1765+count},',p)
p=re.sub(r'appended_typespec_count = \d+, appended_methodspec_count = \d+,',f'appended_typespec_count = {oldplan["typespec_count_after"]-173+nt}, appended_methodspec_count = {oldplan["methodspec_count_after"]-185+nms},',p)
write(OUT/'Program.cs',p)
m=(OUT/'MetadataAppend.cs').read_text('utf-8');at=m.index('];',m.index('private static readonly MemberAddition[] MemberAdditions = ['))
m=m[:at]+'\n'.join('        new(0x%08X,%d,%s,0x%X,%s,%s,%s),'%(x['token'],x['class_code'],json.dumps(x['owner_pin']),x['template'],json.dumps(x['template_name']),json.dumps(x['signature']),json.dumps(x['new_name']) if x['new_name'] else 'null') for x in v['member_definitions'])+'\n    '+m[at:]
at=m.index('];',m.index('private static readonly string[] NewLocalBlobs = ['));m=m[:at]+'\n'.join('        '+json.dumps(x)+',' for x in locs)+'\n    '+m[at:]
at=m.index('];',m.index('private static readonly ExtraSpec[] ExtraMethodSpecs = ['));m=m[:at]+'\n'.join('        new(0x%08X,0x%X,%s,%s,%s,%s),'%(x['token'],x['definition']&0xFFFFFF,json.dumps(x['owner']),json.dumps(x['method_name']),json.dumps(x['signature']),json.dumps(x['instantiation'])) for x in v['added_methodspecs'])+'\n    '+m[at:]
m=re.sub(r'private static readonly string\[\] AddedTypeBlobs = \[.*?\];','private static readonly string[] AddedTypeBlobs = ['+','.join(json.dumps(x) for x in v['type_blobs'])+'];',m)
needle='        Require(blobs.Size+blobSuffix.Count<65536 && strings.Size+names.Count<65536,"No heap index widening");'
usercode='''        // Native118 recovered literals: keep inherited ASCII encoder/guards unchanged.
        // Use the framework ECMA-335 writer and independently pinned source encoding.
        foreach(var entry in N118UserStrings)
        {
            int offset=checked(userStream.Size+userSuffix.Count);
            Require(offset<0x1000000 && entry.Token==0x70000000+offset,"Native118 pinned user string token");
            var literalBuilder=new BlobBuilder();literalBuilder.WriteUserString(entry.Value);
            byte[] encoded=literalBuilder.ToArray();
            Require(encoded.SequenceEqual(Convert.FromHexString(entry.EncodedBytes)),"Native118 literal UTF16/length/terminal-byte pin");
            Require(checked(offset+encoded.Length)<0x1000000,"Native118 bounded append-only user string heap");
            userSuffix.AddRange(encoded);
        }
'''
if needle not in m:raise ValueError('Pinned source insertion point for user strings is absent')
m=m.replace(needle,usercode+needle).replace('appended_user_strings=AddedUserStrings.Length,','appended_user_strings=AddedUserStrings.Length+N118UserStrings.Length,')
write(OUT/'MetadataAppend.cs',m)
meta=dict(oldmeta)
for k in ['member_definitions','local_blobs','constants']:meta[k]=oldmeta[k]+nd[k]
meta['added_typespecs']=oldmeta['added_typespecs']+v['added_typespecs'];meta['type_blobs']=v['type_blobs']
meta['added_methodspecs']=oldmeta['added_methodspecs']+v['added_methodspecs']
for k in ['appended_blob_payload_bytes','appended_string_payload_bytes']:meta[k]+=nd[k]
meta['appended_blob_payload_bytes']+=sum(1+len(bytes.fromhex(x['instantiation'])) for x in v['added_methodspecs'])
meta['user_strings']=oldmeta['user_strings']+v['user_strings'];meta['appended_user_string_payload_bytes']=oldmeta['appended_user_string_payload_bytes']+sum(len(bytes.fromhex(x['encoded_bytes'])) for x in v['user_strings'])
meta['schema']=f'bulk{total}-source-metadata-v1';dump(OUT/'METADATA-ADDITIONS.json',meta)
proj=(OLD/'Bulk300.csproj').read_text('utf-8').replace('  </ItemGroup>','    <Compile Include="N118.MethodBodies.cs"/>\n    <Compile Include="N118.Bindings.cs"/>\n  </ItemGroup>');write(OUT/f'Bulk{total}.csproj',proj)
plan={**oldplan,'schema':f'bulk{total}-source-implementation-v1','targets':oldplan['targets']+targets,'method_body_targets':total,'target_tokens':oldplan['target_tokens']+[t['token'] for t in targets],'historical_direct_targets':294+count,'explicit_companion_targets':6,'unchanged_other_method_bodies':2317-total,'memberref_count_after':1259+nm,'standalone_count_after':2008+count,'typespec_count_after':176+nt,'methodspec_count_after':191+nms,'metadata_row_payload_added_bytes':1470+nm*6+count*2+nt*2+nms*4,'expected_finally_regions':131+ne,'root_executed_project_tests':False,'new_qualified_methods':0,'source_checkpoint_only':count<50,'new_source_method_count_after_bulk250':count,'appended_finally_method_tokens':oldplan['appended_finally_method_tokens']+list(eh),'new_finally_clauses_by_method':eh,'new_finally_clause_count':ne,'new_unique_source_methods_this_turn':count,'supersedes_source_bundle':OLD.relative_to(R).as_posix(),'comparison_to_identified_bulk250':{'changed_bodies':count,'unchanged_bodies':2317-count,'identity_hash':'MUST_BE_SUPPLIED_BY_AGY_AFTER_PREVIOUS_50_RESULT_REVIEW'},'source_evidence_method_count':len(cm),'source_evidence_extra_methods_are_not_targets':True,'delivery_requires_previous_50_result_review':True}
# Previous static progress is scoped, not semantic acceptance. No new scope is dispatched.
plan.update(state='SOURCE_ONLY_UNBUILT_UNEXECUTED',new_source_method_count_after_bulk300=count,new_source_method_count_after_bulk250=50+count,new_unique_source_methods_this_turn=count,previous_50_status='BULK300_HELD_PENDING_PRECEDING_BULK250_BEHAVIOR',preceding_bulk250_run_id=39731970762,preceding_bulk250_status='SCOPED_STATIC_PROGRESS_BEHAVIOR_PENDING',next_scope_testing='HELD_PRECEDING_BULK250_BEHAVIOR_REVIEW_REJECTED',comparison_to_bulk300_r2={'candidate_sha256':None,'expected_changed_bodies':count,'expected_unchanged_bodies':2317-count,'not_executed':True},comparison_to_bulk250_r2={'candidate_sha256':'4732b0c1ee64be4224649ee43c88ddd805f5e1ea461e43ecf52febd7a756f903','expected_changed_bodies':50+count,'expected_unchanged_bodies':2317-50-count,'not_executed':True},comparison_to_bulk250_r1={'candidate_sha256':'dceb44b834889ec6ebcd36baaadbf0b2062fc41982f3d3b64144461a99338996','expected_changed_bodies':52+count,'expected_unchanged_bodies':2317-52-count,'not_executed':True},comparison_to_identified_bulk250={'candidate_sha256':'ad25359a2a36b3b9632e89ad0752bb25657de0f917a23aed2b664dd479c384f8','expected_changed_bodies':60+count,'expected_unchanged_bodies':2317-60-count,'not_executed':True},comparison_to_identified_bulk200={'candidate_sha256':'667e032fe7512f471bcc891c80fc81fd22d27e0ce0ae675bb226f7bbe0e00bc0','expected_changed_bodies':102+count,'expected_unchanged_bodies':2317-102-count,'not_executed':True},appended_user_string_payload_bytes=meta['appended_user_string_payload_bytes'],appended_user_string_count=len(meta['user_strings']),new_user_strings_from_original_native=len(v['user_strings']),metadata_writer_source_extension='PINNED_UTF16_APPEND_ONLY_FRAMEWORK_BLOB_WRITER_UNTESTED')
plan.update(source_checkpoint_only=True,preceding_behavior_review='REJECTED_SOURCE_REVIEW_ONLY_NO_PROJECT_TESTS',preceding_behavior_review_path='audit/review-bulk250-r2-behavior-2026-10-22/REVIEW.md',candidate=None)
dump(OUT/'IMPLEMENTATION-PLAN.json',plan)
dump(OUT/'SOURCE-LINEAGE.json',dict(schema=f'bulk{total}-source-lineage-v1',parent_sha256=plan['parent_sha256'],candidate_parent_is_native93=True,tests_inherited=False,new_acceptance_inherited=False,new_source_batch_tokens=[x['token'] for x in targets],source_predecessor=OLD.relative_to(R).as_posix(),expected_previous_source_method_bodies=300,expected_total_method_bodies=total,expected_finally_regions=131+ne,candidate=None))
ledger=read(R/'audit/NEXT50-AFTER-BULK300-2026-10-22.json');ledger.update(completed_new_unique_method_sources=count,remaining_new_method_sources=50-count,new_source_tokens=[x['token'] for x in targets],source_aggregate=OUT.relative_to(R).as_posix(),source_state='SOURCE_WRITTEN_UNBUILT_UNEXECUTED',new_scope_testing='HELD_PRECEDING_BULK250_BEHAVIOR_REVIEW_REJECTED',previous_result_review_completed=True,previous_result_review='REJECTED_SOURCE_REVIEW_ONLY_NO_PROJECT_TESTS')
dump(R/'audit/NEXT50-AFTER-BULK300-2026-10-22.json',ledger)
reg=read(R/'audit/NATIVE381-BULK-WORK-REGISTRY-v19-2026-10-22.json')
for x in reg['methods']:
    if x['token'] in ledger['new_source_tokens']:x.update(state='SOURCE_WRITTEN_PENDING_VERIFICATION',source_batch=N.name,source_aggregate=OUT.relative_to(R).as_posix(),new_semantic_acceptance=False)
reg.update(schema='native381-bulk-work-registry-v20',date='2026-10-22',source_written_pending_methods=294+count,direct_methods_without_new_source=87-count,aggregate_source_bundle=OUT.relative_to(R).as_posix(),aggregate_method_body_targets=total,new_source_methods_after_bulk300=count,new_source_methods_this_turn=count,additional_source_batch=N.relative_to(R).as_posix(),previous_registry_sha256=sha((R/'audit/NATIVE381-BULK-WORK-REGISTRY-v19-2026-10-22.json').read_bytes()),preceding_bulk250_run_id=39731970762,preceding_bulk250_status='SCOPED_STATIC_PROGRESS_BEHAVIOR_PENDING',previous_50_status='BULK300_HELD_PENDING_PRECEDING_BULK250_BEHAVIOR',new_qualified_methods=0)
reg.update(preceding_behavior_review='REJECTED_SOURCE_REVIEW_ONLY_NO_PROJECT_TESTS',preceding_behavior_review_path='audit/review-bulk250-r2-behavior-2026-10-22/REVIEW.md',next_scope_testing='HELD_PRECEDING_BULK250_BEHAVIOR_REVIEW_REJECTED')
dump(R/'audit/NATIVE381-BULK-WORK-REGISTRY-v20-2026-10-22.json',reg)
readme=f'# Native118 / Bulk{total} held source checkpoint\n\n{count}/50 new unique method sources after Bulk300. Aggregate {total} = {294+count} direct historical worklist entries + 6 companions; {87-count} direct entries remain unwritten. Source only, no candidate or project tests. No new formal acceptance.\n\nAt the fifty-source milestone the preceding Bulk250 R2 behavioral claim was reviewed and rejected: its script substitutes hand-written Python for candidate CIL and unconditionally increments most execution/mutant counters. Original evidence is preserved separately; previous scoped static/access results remain separate. Bulk300 and this complete next50 batch stay held and are not dispatched for execution. AGY was instructed to repair the behavior runner and finish the preceding batch. Native118 also restores {len(v["user_strings"])} exact native literals with a pinned append-only UTF16 user-string writer extension, unbuilt and untested. See VERIFY-NEXT.md.\n'
write(N/'README.md',readme);write(OUT/'README.md',readme)
if (N/'VERIFY-NEXT.md').exists():shutil.copyfile(N/'VERIFY-NEXT.md',OUT/'VERIFY-NEXT.md')
freeze(N);freeze(OUT)
print(json.dumps(dict(scope='SOURCE_TEXT_AND_METADATA_PLAN_COMPOSITION_ONLY',new_unique_method_sources=count,aggregate_sources=total,remaining_until_50=50-count,candidate_constructed=False,project_tests_executed=False)))
