"""Author and package source metadata only. Never construct or execute a candidate."""
from pathlib import Path
import hashlib,json,shutil,sys
ROOT=Path(__file__).resolve().parents[1]
INPUT=ROOT/'audit/native114-codex-2026-10-21'
N=ROOT/'audit/native115-codex-2026-10-22'
OLD=ROOT/'audit/native-bulk195-codex-2026-10-22'
OUT=ROOT/'audit/native-bulk200-codex-2026-10-22'
sys.dont_write_bytecode=True;sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,ensure_ascii=False,indent=2)+'\n')
def freeze(p):
    files=[{'path':f.relative_to(p).as_posix(),'bytes':f.stat().st_size,'sha256':sha(f.read_bytes())} for f in sorted(p.rglob('*')) if f.is_file() and f.name!='SOURCE-MANIFEST.json']
    dump(p/'SOURCE-MANIFEST.json',{'schema':'source-only-manifest-v1','date':'2026-10-22','state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,'files':files})
md=dnfile.dnPE(str(ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).net.mdtables
def comp(v):return bytes([v]) if v<128 else bytes([(v>>8)|128,v&255])
def typ(name,ref=False,value=False):
    rid=next(i for i,t in enumerate((md.TypeRef if ref else md.TypeDef).rows,1) if str(t.TypeName)==name)
    return (b'\x11' if value else b'\x12')+comp((rid<<2)|(1 if ref else 0))
def arr(t):return b'\x1d'+t
def local(*items):return (b'\x07'+comp(len(items))+b''.join(items)).hex()
vec=typ('Vector3',True,True);vec2=typ('Vector2',True,True);quat=typ('Quaternion',True,True)
single=b'\x0c';integer=b'\x08';go=typ('GameObject',True);transform=typ('Transform',True)
blobs=[local(b'\x0e',b'\x0e',typ('TMP_Text',True),b'\x09',*([single]*9)),
       local(vec,vec,transform,b'\x02',single,single,quat,quat),
       local(integer,go,transform,typ('TextMesh',True),typ('TMP_Text',True),single,single),
       local(integer,go,go,typ('TMP_Text',True),single,single,transform,typ('TextMesh',True),vec2),
       local(integer,integer,arr(vec),arr(vec),arr(typ('Color32',True,True)),arr(typ('Color32',True,True)),arr(vec2),arr(vec2),arr(vec2),arr(vec2),integer)]
defs=[];constants=[]
for name,template,new_name in [('N115_SharedMaterial',0x2F6,'set_sharedMaterial'),('N115_Uvs0',0x33E,'uvs0'),('N115_Uvs2',0x33E,'uvs2')]:
    r=md.MemberRef.rows[template-1];t=r.Class.row
    d={'name':name,'token':0x0A000000+1190+len(defs),'class_code':r.Class.row_index*8+1,
       'owner_pin':str(t.TypeNamespace)+'.'+str(t.TypeName),'template':template,'template_name':str(r.Name),
       'signature':r.Signature.value.hex(),'new_name':new_name}
    if new_name in ['uvs0','uvs2']:d['emitted_signature']=(b'\x06'+arr(vec2)).hex()
    defs.append(d);constants.append({'name':name,'token':d['token']})
constants += [{'name':f'N115_Locals{i+1}','token':0x11000000+1904+i} for i in range(5)]
field_overrides=[{'token':d['token'],'owner_pin':d['owner_pin'],'name':d['new_name'],'signature':d['emitted_signature'],
                  'template_signature':d['signature'],'element_type_ref_pin':'UnityEngine.Vector2'} for d in defs if 'emitted_signature' in d]
cur=read(INPUT/'CURRENT-SOURCE.json');orig=read(INPUT/'ORIGINAL-SOURCE.json')
cm={m['token']:m for m in cur['methods']};om={m['token']:m for m in orig['methods']}
rows=[('0x0600065C','N115_SetPlantDataBody',5,1),('0x0600075C','N115_CameraLateUpdateBody',6,2),
      ('0x060007AA','N115_TextSpawnerBody',5,3),('0x06000753','N115_BenchmarkBody',5,4),('0x060007CE','N115_RestoreVertexBody',5,5)]
targets=[]
for tok,body,maxs,loc in rows:
    m=cm[tok];o=om[tok]
    targets.append({'token':tok,'name':m['name'],'signature':m['signature'],'old_code_size':m['code_size'],
        'old_code_sha256':m['il_sha256'],'old_max_stack':m['max_stack'],'old_local_sig_token':f'0x{m["local_sig"]:08X}',
        'old_local_sig_blob':m['locals_blob'],'new_declared_max_stack':maxs,'local_constant':f'N115_Locals{loc}',
        'local_blob':blobs[loc-1],'emission_method':body,'explicit_companion':False,'source_batch':N.name,
        'original_native':{k:o[k] for k in ['start','end','sha256','pdata']}})
tokens={t['token'] for t in targets}
for name in ['CURRENT-SOURCE.json','ORIGINAL-SOURCE.json']:shutil.copyfile(INPUT/name,N/name)
for tok in tokens:
    for suffix in ['-current-il.txt','-original.asm']:shutil.copyfile(INPUT/(tok+suffix),N/(tok+suffix))
dump(N/'SOURCE-INPUT-LINEAGE.json',{'date':'2026-10-22','input_commit':'c4810d2bb2ab10659b8c823e741419d5e8dd7c92',
    'input_directory':INPUT.relative_to(ROOT).as_posix(),'input_manifest_sha256':sha((INPUT/'SOURCE-MANIFEST.json').read_bytes()),
    'implemented_method_count':5,'prior_input_set_methods':12,'prior_input_set_other_seven_source_batch':'native114-codex-2026-10-22',
    'remaining_deferred_from_this_input_set':[],'input_files_modified':False})
nd={'schema':'native115-source-metadata-v1','member_definitions':defs,'local_blobs':blobs,'constants':constants,
    'emitted_field_signatures':field_overrides,'appended_blob_payload_bytes':sum(1+len(bytes.fromhex(b)) for b in blobs)+sum(1+len(bytes.fromhex(d['signature'])) for d in field_overrides),
    'appended_string_payload_bytes':sum(len(d['new_name'].encode())+1 for d in defs),'added_typespecs':[],'added_methodspecs':[],'added_user_strings':[]}
dump(N/'METADATA-ADDITIONS.json',nd)
dump(N/'IMPLEMENTATION-PLAN.json',{'schema':'native115-source-implementation-v1','date':'2026-10-22','state':'SOURCE_ONLY_UNBUILT_UNEXECUTED',
    'root_executed_project_tests':False,'candidate':None,'new_qualified_methods':0,'targets':targets,'appended_finally_method_tokens':[]})
write(N/'README.md','# Native115: five native-backed body sources\n\nSource only. Plant-data UI, camera LateUpdate, two text spawners and two-quad TMP vertex restoration. Completes source coverage of the previous twelve-method input set, without claiming execution or acceptance. New UV MemberRefs use explicit Vector2[] field signatures, not the Vector3[] template signature. See VERIFY-NEXT.md.\n')
freeze(N)
OUT.mkdir(exist_ok=False)
for item in read(OLD/'SOURCE-MANIFEST.json')['files']:
    p=OLD/item['path']
    if p.suffix=='.cs':shutil.copyfile(p,OUT/p.name)
shutil.copyfile(N/'MethodBodies.cs',OUT/'N115.MethodBodies.cs')
program=(OUT/'Program.cs').read_text(encoding='utf-8');pins=[]
for t in targets:
    m=cm[t['token']];owner,name=m['name'].split('::');owner=owner.rsplit('.',1)[-1]
    pins.append(f'        new({t["token"]},"{owner}","{name}","{m["signature"]}",{m["code_size"]},{m["max_stack"]},0x{m["local_sig"]:08X},"{m["locals_blob"]}","{m["il_sha256"]}"),')
at=program.index('];',program.index('private static readonly Pin[] Pins = ['));program=program[:at]+'\n'.join(pins)+'\n    '+program[at:]
plans=[f'            (Token:{t["token"]}, Code:{t["emission_method"]}(), Max:{t["new_declared_max_stack"]}, Locals:{t["local_constant"]})' for t in targets]
at=program.index('};',program.index('var plans = new[] {'));program=program[:at].rstrip()+',\n'+',\n'.join(plans)+'\n        '+program[at:]
program=program.replace('fresh-bulk195.dll','fresh-bulk200.dll').replace('appended_memberref_count = 89, appended_local_signature_count = 138,','appended_memberref_count = 92, appended_local_signature_count = 143,').replace('independent 195-target','independent 200-target')
write(OUT/'Program.cs',program)
meta=(OUT/'MetadataAppend.cs').read_text(encoding='utf-8')
meta=meta.replace('    private record MemberAddition(', '\n'.join(f'    private const int {x["name"]} = 0x{x["token"]:08X};' for x in constants)+'\n    private record MemberAddition(',1)
at=meta.index('];',meta.index('private static readonly MemberAddition[] MemberAdditions = ['))
meta=meta[:at]+'\n'.join('        new(0x%08X,%d,%s,0x%X,%s,%s,%s),'%(x['token'],x['class_code'],json.dumps(x['owner_pin']),x['template'],json.dumps(x['template_name']),json.dumps(x['signature']),json.dumps(x['new_name'])) for x in defs)+'\n    '+meta[at:]
at=meta.index('];',meta.index('private static readonly string[] NewLocalBlobs = ['));meta=meta[:at]+'\n'.join('        '+json.dumps(x)+',' for x in blobs)+'\n    '+meta[at:]
# Two explicit new field signatures only; all original/inherited rows and blobs stay immutable.
field_code='''
        Require(N115_Uvs0==0x0A0004A7 && N115_Uvs2==0x0A0004A8,"Pinned two new UV field references");
        var vector2Ref=md.GetTypeReference(MetadataTokens.TypeReferenceHandle(75));
        Require(md.GetString(vector2Ref.Namespace)=="UnityEngine" && md.GetString(vector2Ref.Name)=="Vector2","Pinned UV element TypeRef");
        foreach(int token in new[]{N115_Uvs0,N115_Uvs2})
        {
            int index=token-0x0A000000-baseMembers-1;var entry=MemberAdditions[index];
            string expectedName=token==N115_Uvs0 ? "uvs0" : "uvs2";
            Require(entry.OwnerPin=="TMPro.TMP_MeshInfo" && entry.ClassCode==849 && entry.NewName==expectedName && entry.Signature=="061d114d" && entry.Template==0x33E,"Exact UV field append scope");
            byte[] signature=Convert.FromHexString("061d11812d");
            BinaryPrimitives.WriteUInt16LittleEndian(memberRows.AsSpan(index*6+4,2),checked((ushort)(blobs.Size+blobSuffix.Count)));
            blobSuffix.Add(checked((byte)signature.Length));blobSuffix.AddRange(signature);
        }
'''
meta=meta.replace('        byte[] typeRows=new byte[checked(AddedTypeBlobs.Length*2)];',field_code+'        byte[] typeRows=new byte[checked(AddedTypeBlobs.Length*2)];',1)
write(OUT/'MetadataAppend.cs',meta)
desc=read(OLD/'METADATA-ADDITIONS.json');desc['schema']='bulk200-source-metadata-v1'
for k in ['member_definitions','local_blobs','constants']:desc[k]+=nd[k]
desc['emitted_field_signatures']=desc.get('emitted_field_signatures',[])+field_overrides
for k in ['appended_blob_payload_bytes','appended_string_payload_bytes']:desc[k]+=nd[k]
dump(OUT/'METADATA-ADDITIONS.json',desc)
write(OUT/'Bulk200.csproj',(OLD/'Bulk195.csproj').read_text(encoding='utf-8').replace('  </ItemGroup>','    <Compile Include="N115.MethodBodies.cs"/>\n  </ItemGroup>'))
plan=read(OLD/'IMPLEMENTATION-PLAN.json');plan.update(schema='bulk200-source-implementation-v1',date='2026-10-22',targets=plan['targets']+targets,
    method_body_targets=200,target_tokens=plan['target_tokens']+[t['token'] for t in targets],historical_direct_targets=194,explicit_companion_targets=6,
    unchanged_other_method_bodies=2117,memberref_count_after=1192,standalone_count_after=1908,methodspec_count_after=189,metadata_row_payload_added_bytes=858,
    appended_blob_payload_bytes=desc['appended_blob_payload_bytes'],new_unique_source_methods_this_turn=5,supersedes_source_bundle=OLD.relative_to(ROOT).as_posix())
dump(OUT/'IMPLEMENTATION-PLAN.json',plan)
previous=read(OLD/'SOURCE-LINEAGE.json');delta=previous['new_target_tokens']+[t['token'] for t in targets]
dump(OUT/'SOURCE-LINEAGE.json',{'schema':'bulk200-source-lineage-v1','parent_sha256':plan['parent_sha256'],'candidate_parent_is_native93':True,
    'tests_inherited':False,'new_acceptance_inherited':False,'expected_previous_candidate_sha256':'1ed555461d3eb2c6003487ba36ec1a09d66e4061aba3c2c877c49f026f25be93',
    'previous_candidate_label':'Bulk188 last known executed checkpoint; Bulk195 source is not presumed to be an executed candidate',
    'new_target_tokens':delta,'comparison_delta_tokens':delta,'new_source_batch_tokens':[t['token'] for t in targets],
    'expected_preserved_inherited_method_bodies':188,'expected_previous_source_method_bodies':195,'expected_total_method_bodies':200,'expected_finally_regions':39,
    'source_manifests':[{'path':p.relative_to(ROOT).as_posix()+'/SOURCE-MANIFEST.json','sha256':sha((p/'SOURCE-MANIFEST.json').read_bytes())} for p in [OLD,N]]})
shutil.copyfile(N/'VERIFY-NEXT.md',OUT/'VERIFY-NEXT.md')
write(OUT/'README.md','# Bulk200 source-only delivery\n\nFive Native115 bodies added to unchanged Bulk195 body sources. Fixed Native93 construction parent; compare against executed Bulk188 for12 body differences (or independently identified Bulk195 for5). 200 targets =194 direct plus6 companions;2,117 non-targets;39 finally clauses. MemberRef1192 / StandAloneSig1908 / TypeSpec175 / MethodSpec189;858-byte table-row payload increment. Three new MemberRefs include two explicit Vector2[] UV field signatures. No candidate construction, tests or new qualification by primary author.\n')
freeze(OUT)
rp=ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v15-2026-10-22.json';reg=read(rp);touched=0
for m in reg['methods']:
    if m['token'] in tokens:m.update(state='SOURCE_WRITTEN_PENDING_VERIFICATION',source_batch=N.name,new_semantic_acceptance=False,source_aggregate=OUT.relative_to(ROOT).as_posix());touched+=1
reg.update(schema='native381-bulk-work-registry-v16',date='2026-10-22',previous_registry_sha256=sha(rp.read_bytes()),new_source_methods_this_turn=5,
    source_written_pending_methods=reg['source_written_pending_methods']+touched,direct_methods_without_new_source=reg['direct_methods_without_new_source']-touched,
    aggregate_source_bundle=OUT.relative_to(ROOT).as_posix(),aggregate_method_body_targets=200,additional_source_batch=N.relative_to(ROOT).as_posix(),new_qualified_methods=0)
dump(ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v16-2026-10-22.json',reg)
print(json.dumps({'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','new_method_sources':5,'worklist_tokens_touched':touched,'aggregate_sources':200,
    'project_tests_executed':False,'source_manifest_sha256':sha((OUT/'SOURCE-MANIFEST.json').read_bytes())},indent=2))
