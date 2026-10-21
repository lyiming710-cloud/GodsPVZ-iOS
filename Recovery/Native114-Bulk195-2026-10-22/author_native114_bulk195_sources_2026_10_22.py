"""Compose source and metadata descriptions only; never execute the patcher/game/tests."""
from pathlib import Path
import hashlib,json,shutil,sys

ROOT=Path(__file__).resolve().parents[1]
INPUT=ROOT/'audit/native114-codex-2026-10-21'
N=ROOT/'audit/native114-codex-2026-10-22'
OLD=ROOT/'audit/native-bulk188-codex-2026-10-21'
OUT=ROOT/'audit/native-bulk195-codex-2026-10-22'
sys.dont_write_bytecode=True
sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile

read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,ensure_ascii=False,indent=2)+'\n')
def freeze(p):
    files=[{'path':f.relative_to(p).as_posix(),'bytes':f.stat().st_size,'sha256':sha(f.read_bytes())}
           for f in sorted(p.rglob('*')) if f.is_file() and f.name!='SOURCE-MANIFEST.json']
    dump(p/'SOURCE-MANIFEST.json',{'schema':'source-only-manifest-v1','date':'2026-10-22',
        'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,'files':files})

parent=ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'
md=dnfile.dnPE(str(parent)).net.mdtables
def comp(v):return bytes([v]) if v<128 else bytes([(v>>8)|128,v&255])
def typ(name,ref=False,value=False):
    rows=md.TypeRef.rows if ref else md.TypeDef.rows
    rid=next(i for i,t in enumerate(rows,1) if str(t.TypeName)==name)
    return (b'\x11' if value else b'\x12')+comp((rid<<2)|(1 if ref else 0))
def local(*items):return (b'\x07'+comp(len(items))+b''.join(items)).hex()

cur=read(INPUT/'CURRENT-SOURCE.json');orig=read(INPUT/'ORIGINAL-SOURCE.json')
cm={m['token']:m for m in cur['methods']};om={m['token']:m for m in orig['methods']}
vec=typ('Vector3',True,True);single=b'\x0c';integer=b'\x08'
blobs=[
    local(typ('DeviceData'),typ('PreDevice'),typ('DeviceContent'),integer),
    local(single),
    local(single,single,vec,typ('Project'),integer,typ('AudioSource',True),b'\x02',typ('Skill')),
    local(vec,vec,typ('Transform',True),single,single,single,single,integer,typ('SpriteRenderer',True)),
    local(vec,typ('GameObject',True),typ('AudioClip',True),md.TypeSpec.rows[25].Signature.value,integer),
    local(typ('GameObject',True),typ('SpriteRenderer',True),single),
    local(typ('MaterialPropertyBlock',True),typ('Sprite',True),typ('Texture2D',True),typ('Texture2D',True),integer),
]
defs=[];constants=[]
for name,template,new_name in [('N114_GetPropertyBlock',0x39F,'GetPropertyBlock'),
                             ('N114_SetPropertyBlock',0x3A5,'SetPropertyBlock'),
                             ('N114_SetTexture',0x3A4,'SetTexture'),
                             ('N114_SetFloat',0x3A6,'SetFloat')]:
    r=md.MemberRef.rows[template-1];t=r.Class.row
    code=r.Class.row_index*8+1
    defs.append({'name':name,'token':0x0A000000+1186+len(defs),'class_code':code,
        'owner_pin':str(t.TypeNamespace)+'.'+str(t.TypeName),'template':template,
        'template_name':str(r.Name),'signature':r.Signature.value.hex(),'new_name':new_name})
    constants.append({'name':name,'token':defs[-1]['token']})
constants += [{'name':f'N114_Locals{i+1}','token':0x11000000+1897+i} for i in range(7)]
rows=[('0x0600018C','N114_CreateIceCubeBody',4,1),('0x0600030B','N114_DeviceCharacteristicBody',5,2),
      ('0x0600034E','N114_PlantAttackBody',5,3),('0x060003C4','N114_ProjectCollectBody',6,4),
      ('0x060003F4','N114_LightSaberExplosionBody',5,5),('0x06000461','N114_Armor2StatusBody',3,6),
      ('0x06000846','N114_SwfPropertyBlockBody',3,7)]
targets=[]
for tok,body,maxs,loc in rows:
    m=cm[tok];o=om[tok]
    targets.append({'token':tok,'name':m['name'],'signature':m['signature'],'old_code_size':m['code_size'],
        'old_code_sha256':m['il_sha256'],'old_max_stack':m['max_stack'],'old_local_sig_token':f'0x{m["local_sig"]:08X}',
        'old_local_sig_blob':m['locals_blob'],'new_declared_max_stack':maxs,'local_constant':f'N114_Locals{loc}',
        'local_blob':blobs[loc-1],'emission_method':body,'explicit_companion':False,'source_batch':N.name,
        'original_native':{k:o[k] for k in ['start','end','sha256','pdata']}})
tokens={t['token'] for t in targets}
for name in ['CURRENT-SOURCE.json','ORIGINAL-SOURCE.json']:
    shutil.copyfile(INPUT/name,N/name)
for tok in tokens:
    for suffix in ['-current-il.txt','-original.asm']:shutil.copyfile(INPUT/(tok+suffix),N/(tok+suffix))
dump(N/'SOURCE-INPUT-LINEAGE.json',{'date':'2026-10-22','input_commit':'c4810d2bb2ab10659b8c823e741419d5e8dd7c92',
    'input_directory':INPUT.relative_to(ROOT).as_posix(),'input_manifest_sha256':sha((INPUT/'SOURCE-MANIFEST.json').read_bytes()),
    'retained_input_method_count':12,'implemented_method_count':7,
    'deferred_input_tokens':[t['token'] for t in read(INPUT/'INPUT-SCOPE.json')['targets'] if t['token'] not in tokens],
    'input_files_modified':False,'scope':'immutable source data only, no executed game or tests'})
dump(N/'PROJECT-COLLECT-JUMP-TABLE.json',{'scope':'original data decoded, no target execution',
    'address':'0x1803767B0','hex':'d36637004a6737004a673700276737006c6737002767370027673700276737004a673700',
    'signed_or_unsigned_bounds':'unsigned projectType <= 8; all other values return',
    'entries':[{'value':i,'target':f'0x{a:X}','meaning':s} for i,a,s in [
        (0,0x1803766D3,'SunManager.CollectSun; destroy current gameObject'),
        (1,0x18037674A,'ProjectManager.CollectProject; destroy current gameObject'),
        (2,0x18037674A,'ProjectManager.CollectProject; destroy current gameObject'),
        (3,0x180376727,'return'),(4,0x18037676C,'speed=Vector3.zero'),
        (5,0x180376727,'return'),(6,0x180376727,'return'),(7,0x180376727,'return'),
        (8,0x18037674A,'ProjectManager.CollectProject; destroy current gameObject')]]})
dump(N/'PUBLIC-API-BINDINGS.json',{'state':'SOURCE_BINDING_NOT_RUNTIME_ACCEPTANCE','references':defs,
    'original_coremodule_native_aliases':[
        {'public':'Renderer.GetPropertyBlock','internal':'Renderer.Internal_GetPropertyBlock','va':'0x1812FAEB0'},
        {'public':'Renderer.SetPropertyBlock','internal':'Renderer.Internal_SetPropertyBlock','va':'0x1812FAF40'},
        {'public':'MaterialPropertyBlock.SetTexture(int,Texture)','internal':'MaterialPropertyBlock.SetTextureImpl','va':'0x1812EEB90'},
        {'public':'MaterialPropertyBlock.SetFloat(int,float)','internal':'MaterialPropertyBlock.SetFloatImpl','va':'0x1812EE4D0'}],
    'no_support_assembly_modified':True,'executor_requirement':'Resolve the public overload/access/signature in the pinned actual Player support profile; do not create shim APIs.'})
nd={'schema':'native114-source-metadata-v1','member_definitions':defs,'local_blobs':blobs,'constants':constants,
    'appended_blob_payload_bytes':sum(1+len(bytes.fromhex(b)) for b in blobs),
    'appended_string_payload_bytes':sum(len(d['new_name'].encode())+1 for d in defs),
    'added_typespecs':[],'added_methodspecs':[],'added_user_strings':[]}
dump(N/'METADATA-ADDITIONS.json',nd)
dump(N/'IMPLEMENTATION-PLAN.json',{'schema':'native114-source-implementation-v1','date':'2026-10-22',
    'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,
    'new_qualified_methods':0,'targets':targets,'appended_finally_method_tokens':[]})
write(N/'README.md','# Native114: seven native-backed body sources\n\nSource only, seven implementations from the prior twelve-method evidence input set. No candidate construction or project tests by the primary author. Original branch tables and public Unity API aliases are retained as evidence. The other five inputs are deferred; counts do not include them. See VERIFY-NEXT.md.\n')
freeze(N)

OUT.mkdir(exist_ok=False)
for item in read(OLD/'SOURCE-MANIFEST.json')['files']:
    p=OLD/item['path']
    if p.suffix=='.cs':shutil.copyfile(p,OUT/p.name)
shutil.copyfile(N/'MethodBodies.cs',OUT/'N114.MethodBodies.cs')
program=(OUT/'Program.cs').read_text(encoding='utf-8')
pins=[]
for t in targets:
    m=cm[t['token']];owner,name=m['name'].split('::');owner=owner.rsplit('.',1)[-1]
    pins.append(f'        new({t["token"]},"{owner}","{name}","{m["signature"]}",{m["code_size"]},{m["max_stack"]},0x{m["local_sig"]:08X},"{m["locals_blob"]}","{m["il_sha256"]}"),')
at=program.index('];',program.index('private static readonly Pin[] Pins = ['));program=program[:at]+'\n'.join(pins)+'\n    '+program[at:]
plans=[f'            (Token:{t["token"]}, Code:{t["emission_method"]}(), Max:{t["new_declared_max_stack"]}, Locals:{t["local_constant"]})' for t in targets]
at=program.index('};',program.index('var plans = new[] {'));program=program[:at].rstrip()+',\n'+',\n'.join(plans)+'\n        '+program[at:]
program=program.replace('fresh-bulk188.dll','fresh-bulk195.dll').replace('appended_memberref_count = 85, appended_local_signature_count = 131,','appended_memberref_count = 89, appended_local_signature_count = 138,').replace('independent 188-target','independent 195-target')
write(OUT/'Program.cs',program)
meta=(OUT/'MetadataAppend.cs').read_text(encoding='utf-8')
meta=meta.replace('    private record MemberAddition(', '\n'.join(f'    private const int {x["name"]} = 0x{x["token"]:08X};' for x in constants)+'\n    private record MemberAddition(',1)
at=meta.index('];',meta.index('private static readonly MemberAddition[] MemberAdditions = ['))
meta=meta[:at]+'\n'.join('        new(0x%08X,%d,%s,0x%X,%s,%s,%s),'%(x['token'],x['class_code'],json.dumps(x['owner_pin']),x['template'],json.dumps(x['template_name']),json.dumps(x['signature']),json.dumps(x['new_name'])) for x in defs)+'\n    '+meta[at:]
at=meta.index('];',meta.index('private static readonly string[] NewLocalBlobs = ['))
meta=meta[:at]+'\n'.join('        '+json.dumps(x)+',' for x in blobs)+'\n    '+meta[at:]
write(OUT/'MetadataAppend.cs',meta)
desc=read(OLD/'METADATA-ADDITIONS.json');desc['schema']='bulk195-source-metadata-v1'
for k in ['member_definitions','local_blobs','constants']:desc[k]+=nd[k]
for k in ['appended_blob_payload_bytes','appended_string_payload_bytes']:desc[k]+=nd[k]
dump(OUT/'METADATA-ADDITIONS.json',desc)
write(OUT/'Bulk195.csproj',(OLD/'Bulk188.csproj').read_text(encoding='utf-8').replace('  </ItemGroup>','    <Compile Include="N114.MethodBodies.cs"/>\n  </ItemGroup>'))
plan=read(OLD/'IMPLEMENTATION-PLAN.json')
plan.update(schema='bulk195-source-implementation-v1',date='2026-10-22',targets=plan['targets']+targets,
    method_body_targets=195,target_tokens=plan['target_tokens']+[t['token'] for t in targets],
    historical_direct_targets=189,explicit_companion_targets=6,unchanged_other_method_bodies=2122,
    memberref_count_after=1189,standalone_count_after=1903,methodspec_count_after=189,
    metadata_row_payload_added_bytes=830,appended_blob_payload_bytes=desc['appended_blob_payload_bytes'],
    new_unique_source_methods_this_turn=7,supersedes_source_bundle=OLD.relative_to(ROOT).as_posix())
dump(OUT/'IMPLEMENTATION-PLAN.json',plan)
dump(OUT/'SOURCE-LINEAGE.json',{'schema':'bulk195-source-lineage-v1','parent_sha256':plan['parent_sha256'],
    'candidate_parent_is_native93':True,'tests_inherited':False,'new_acceptance_inherited':False,
    'expected_previous_candidate_sha256':'1ed555461d3eb2c6003487ba36ec1a09d66e4061aba3c2c877c49f026f25be93',
    'previous_candidate_identity_reported_by':'AGY Run 39606934064, recorded Release digest and saved preparation manifest',
    'previous_candidate_status':'IL2CPP_EXIT_0_WITH_ANIMATIONMODULE_OVERLAY; CLANG_FAILED_18_OF_18_GAME_TUS; NO_PROMOTION',
    'new_target_tokens':[t['token'] for t in targets],'expected_preserved_inherited_method_bodies':188,
    'expected_total_method_bodies':195,'expected_finally_regions':39,
    'source_manifests':[{'path':p.relative_to(ROOT).as_posix()+'/SOURCE-MANIFEST.json','sha256':sha((p/'SOURCE-MANIFEST.json').read_bytes())} for p in [OLD,N]]})
shutil.copyfile(N/'VERIFY-NEXT.md',OUT/'VERIFY-NEXT.md')
write(OUT/'README.md','# Bulk195 source-only delivery\n\nSeven Native114 bodies added to unchanged Bulk188 source. Fixed Native93 construction parent. 195 bodies = 189 direct historical targets plus six companions; 2,122 other bodies; 39 finally regions. MemberRef1189 / StandAloneSig1903 / TypeSpec175 / MethodSpec189. Source only, unbuilt and unexecuted by the primary author. IL2CPP/Clang, behavior and production dependency acceptance are independently owned by AGY.\n')
freeze(OUT)
rp=ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v14-2026-10-21.json';reg=read(rp)
touched=0
for m in reg['methods']:
    if m['token'] in tokens:
        m.update(state='SOURCE_WRITTEN_PENDING_VERIFICATION',source_batch=N.name,new_semantic_acceptance=False,source_aggregate=OUT.relative_to(ROOT).as_posix());touched+=1
reg.update(schema='native381-bulk-work-registry-v15',date='2026-10-22',previous_registry_sha256=sha(rp.read_bytes()),
    new_source_methods_this_turn=7,source_written_pending_methods=reg['source_written_pending_methods']+touched,
    direct_methods_without_new_source=reg['direct_methods_without_new_source']-touched,
    aggregate_source_bundle=OUT.relative_to(ROOT).as_posix(),aggregate_method_body_targets=195,
    additional_source_batch=N.relative_to(ROOT).as_posix(),new_qualified_methods=0,
    latest_candidate={'run_id':39606934064,'candidate':'Bulk188','sha256':'1ed555461d3eb2c6003487ba36ec1a09d66e4061aba3c2c877c49f026f25be93',
        'status':'IL2CPP_EXIT_0_ON_OVERLAID_SUPPORT_PROFILE; CLANG_18_OF_18_GAME_TUS_FAILED',
        'qualification':False,'profile_comparability':'PENDING_EQUAL_PROFILE_BASELINE','report_source':'saved AGY evidence, primary did not rerun project tests'})
dump(ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v15-2026-10-22.json',reg)
print(json.dumps({'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','new_method_sources':7,'worklist_tokens_touched':touched,
    'aggregate_sources':195,'project_tests_executed':False,'source_manifest_sha256':sha((OUT/'SOURCE-MANIFEST.json').read_bytes())},indent=2))
