"""Compose source text and pinned metadata descriptions; no project code execution."""
from pathlib import Path
import hashlib
import json
import shutil
import sys

ROOT=Path(__file__).resolve().parents[1]
N=ROOT/'audit/native112-codex-2026-10-20'
OLD=ROOT/'audit/native-bulk168-codex-2026-10-20'
OUT=ROOT/'audit/native-bulk180-codex-2026-10-21'
sys.dont_write_bytecode=True
sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile
md=dnfile.dnPE(str(ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).net.mdtables

read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,ensure_ascii=False,indent=2)+'\n')
def freeze(p):
    files=[{'path':f.relative_to(p).as_posix(),'bytes':f.stat().st_size,'sha256':sha(f.read_bytes())}
           for f in sorted(p.rglob('*')) if f.is_file() and f.name!='SOURCE-MANIFEST.json']
    dump(p/'SOURCE-MANIFEST.json',{'schema':'source-only-manifest-v1','date':'2026-10-21',
        'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,'files':files})
def compressed(v):
    if v<128:return bytes([v])
    return bytes([(v>>8)|128,v&255])
def typ(name,ref=False,value=False):
    table=md.TypeRef.rows if ref else md.TypeDef.rows
    rid=next(i for i,t in enumerate(table,1) if str(t.TypeName)==name)
    return (b'\x11' if value else b'\x12')+compressed((rid<<2)|(1 if ref else 0))
def locals_blob(*types):return (b'\x07'+compressed(len(types))+b''.join(types)).hex()

refs=[];constants=[]
def member(name,owner,template,new_name=None):
    old=md.MemberRef.rows[template-1]
    if isinstance(owner,int):code=owner*8+4;pin=md.TypeSpec.rows[owner-1].Signature.value.hex()
    else:
        rid=next(i for i,t in enumerate(md.TypeRef.rows,1) if str(t.TypeName)==owner)
        t=md.TypeRef.rows[rid-1];code=rid*8+1;pin=str(t.TypeNamespace)+'.'+str(t.TypeName)
    token=0x0A000000+1175+len(refs)
    refs.append({'name':name,'token':token,'class_code':code,'owner_pin':pin,'template':template,
                 'template_name':str(old.Name),'signature':old.Signature.value.hex(),'new_name':new_name})
    constants.append({'name':name,'token':token})
for name,owner in [('Vector3',29),('Vector2',162),('Vector4',163),('Color',164)]:
    member('N112_'+name+'ListClear',owner,0xC0)
for name,owner in [('Vector2',162),('Vector4',163),('Color',164)]:
    member('N112_'+name+'ListAdd',owner,0xBF)
for axis in 'XYZW':member('N112_Vector4'+axis,'Vector4',0xEE,axis.lower())

iref=typ('IEnumerator',ref=True);disp=typ('IDisposable',ref=True)
transform=typ('Transform',ref=True);vec3=typ('Vector3',ref=True,value=True)
blobs=[
    locals_blob(iref,disp),
    locals_blob(iref,disp,transform),
    locals_blob(iref,disp,transform,vec3,vec3,transform,vec3),
    locals_blob(iref,disp,transform,vec3,b'\x0c'),
    locals_blob(b'\x08',b'\x08'),
    locals_blob(b'\x08',b'\x08',b'\x0c',b'\x0c'),
    locals_blob(b'\x08',b'\x08',b'\x09',b'\x09',b'\x0c',b'\x0c',b'\x0c',b'\x0c'),
    locals_blob(b'\x08',b'\x08',b'\x08',b'\x08'),
    locals_blob(*([b'\x08']*9),typ('EnemyPath')),
    locals_blob(b'\x02',b'\x08',typ('Image',ref=True),typ('Skill'),b'\x0c',b'\x0c'),
    locals_blob(typ('Board'),typ('AudioClip',ref=True),vec3),
]
for i in range(len(blobs)):constants.append({'name':f'N112_Locals{i+1}','token':0x11000000+1878+i})
rows=[
    ('0x060000C7','N112_VfxLoopBody',3,1),
    ('0x060005E5','N112_BuffImagesBody',2,2),
    ('0x0600047C','N112_ZombieTransformBody',5,3),
    ('0x06000478','N112_ZombieRowBody',4,4),
    ('0x060008FB','N112_FillTrianglesBody',3,5),
    ('0x060008FC','N112_FillVerticesBody',3,6),
    ('0x060008FD','N112_FillUvsBody',3,7),
    ('0x060008FE','N112_FillColorsBody(false)',3,8),
    ('0x060008FF','N112_FillColorsBody(true)',3,8),
    ('0x060001BE','N112_EnemyTargetBody',4,9),
    ('0x06000623','N112_SkillUiBody',5,10),
    ('0x06000405','N112_PropClickBody',3,11),
]
current=read(N/'CURRENT-SOURCE.json');original=read(N/'ORIGINAL-SOURCE.json')
cm={m['token']:m for m in current['methods']};om={m['token']:m for m in original['methods']}
targets=[]
for token,body,maxs,local in rows:
    m=cm[token];o=om[token]
    targets.append({'token':token,'name':m['name'],'signature':m['signature'],'old_code_size':m['code_size'],
        'old_code_sha256':m['il_sha256'],'old_max_stack':m['max_stack'],'old_local_sig_token':f'0x{m["local_sig"]:08X}',
        'old_local_sig_blob':m['locals_blob'],'new_declared_max_stack':maxs,'local_constant':f'N112_Locals{local}',
        'local_blob':blobs[local-1],'emission_method':body,'explicit_companion':False,'source_batch':N.name,
        'original_native':{k:o[k] for k in ['start','end','sha256','pdata']}})
finally_tokens=['0x060000C7','0x060005E5','0x0600047C','0x06000478']
nd={'schema':'native112-source-metadata-v1','member_definitions':refs,'local_blobs':blobs,'constants':constants,
    'appended_blob_payload_bytes':sum(1+len(bytes.fromhex(b)) for b in blobs),
    'appended_string_payload_bytes':sum(len(v['new_name'].encode())+1 for v in refs if v['new_name']),
    'added_typespecs':[],'added_methodspecs':[],'added_user_strings':[]}
dump(N/'METADATA-ADDITIONS.json',nd)
dump(N/'IMPLEMENTATION-PLAN.json',{'schema':'native112-source-implementation-v1','date':'2026-10-21',
    'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,
    'new_qualified_methods':0,'targets':targets,'appended_finally_method_tokens':finally_tokens,
    'deferred_input_tokens':[t for t in cm if t not in {x['token'] for x in targets}]})
write(N/'README.md','# Native112: twelve native-backed method-body sources\n\nSource authoring only; no candidate construction or project test execution. The selected twelve bodies include four IEnumerator finally regions and five GeneratedMeshCache methods. Eight other pre-read methods remain deferred, with no count increment. See VERIFY-NEXT.md for behavior, exception and callback requirements. Public API binding is kept separate from the historical stripped-support limitation of Native111.\n')
freeze(N)

OUT.mkdir(exist_ok=False)
for item in read(OLD/'SOURCE-MANIFEST.json')['files']:
    p=OLD/item['path']
    if p.suffix=='.cs':shutil.copyfile(p,OUT/p.name)
shutil.copyfile(N/'MethodBodies.cs',OUT/'N112.MethodBodies.cs')
program=(OUT/'Program.cs').read_text(encoding='utf-8')
pins=[]
for t in targets:
    m=cm[t['token']];owner,name=m['name'].split('::');owner=owner.rsplit('.',1)[-1]
    pins.append(f'        new({t["token"]},"{owner}","{name}","{m["signature"]}",{m["code_size"]},{m["max_stack"]},0x{m["local_sig"]:08X},"{m["locals_blob"]}","{m["il_sha256"]}"),')
at=program.index('];',program.index('private static readonly Pin[] Pins = ['))
program=program[:at]+'\n'.join(pins)+'\n    '+program[at:]
plans=[]
for t in targets:
    call=t['emission_method'] if '(' in t['emission_method'] else t['emission_method']+'()'
    plans.append(f'            (Token:{t["token"]}, Code:{call}, Max:{t["new_declared_max_stack"]}, Locals:{t["local_constant"]})')
at=program.index('};',program.index('var plans = new[] {'))
program=program[:at].rstrip()+',\n'+',\n'.join(plans)+'\n        '+program[at:]
program=program.replace('fresh-bulk168.dll','fresh-bulk180.dll').replace('appended_memberref_count = 74, appended_local_signature_count = 112,','appended_memberref_count = 85, appended_local_signature_count = 123,').replace('independent 168-target','independent 180-target')
write(OUT/'Program.cs',program)
f=(OUT/'FinallyBodies.cs').read_text(encoding='utf-8')
f=f.replace(',"Explicit EH target allowlist"',' or '+' or '.join(finally_tokens)+',"Explicit EH target allowlist"')
write(OUT/'FinallyBodies.cs',f)
meta=(OUT/'MetadataAppend.cs').read_text(encoding='utf-8')
meta=meta.replace('    private record MemberAddition(',
    '\n'.join(f'    private const int {x["name"]} = 0x{x["token"]:08X};' for x in constants)+'\n    private record MemberAddition(',1)
at=meta.index('];',meta.index('private static readonly MemberAddition[] MemberAdditions = ['))
newrefs='\n'.join('        new(0x%08X,%d,%s,0x%X,%s,%s,%s),'%
    (x['token'],x['class_code'],json.dumps(x['owner_pin']),x['template'],json.dumps(x['template_name']),json.dumps(x['signature']),
     'null' if x['new_name'] is None else json.dumps(x['new_name'])) for x in refs)
meta=meta[:at]+newrefs+'\n    '+meta[at:]
at=meta.index('];',meta.index('private static readonly string[] NewLocalBlobs = ['))
meta=meta[:at]+'\n'.join('        '+json.dumps(x)+',' for x in blobs)+'\n    '+meta[at:]
write(OUT/'MetadataAppend.cs',meta)
desc=read(OLD/'METADATA-ADDITIONS.json');desc['schema']='bulk180-source-metadata-v1'
for key in ['member_definitions','local_blobs','constants']:desc[key]+=nd[key]
desc['appended_blob_payload_bytes']+=nd['appended_blob_payload_bytes']
desc['appended_string_payload_bytes']+=nd['appended_string_payload_bytes']
dump(OUT/'METADATA-ADDITIONS.json',desc)
write(OUT/'Bulk180.csproj',(OLD/'Bulk168.csproj').read_text(encoding='utf-8').replace('  </ItemGroup>','    <Compile Include="N112.MethodBodies.cs"/>\n  </ItemGroup>'))
plan=read(OLD/'IMPLEMENTATION-PLAN.json')
plan.update(schema='bulk180-source-implementation-v1',date='2026-10-21',targets=plan['targets']+targets,
    method_body_targets=180,target_tokens=plan['target_tokens']+[t['token'] for t in targets],
    historical_direct_targets=174,explicit_companion_targets=6,unchanged_other_method_bodies=2137,
    memberref_count_after=1185,standalone_count_after=1888,methodspec_count_after=189,
    metadata_row_payload_added_bytes=776,appended_blob_payload_bytes=desc['appended_blob_payload_bytes'],
    new_unique_source_methods_this_turn=12,supersedes_source_bundle=OLD.relative_to(ROOT).as_posix())
dump(OUT/'IMPLEMENTATION-PLAN.json',plan)
dump(OUT/'SOURCE-LINEAGE.json',{'schema':'bulk180-source-lineage-v1','parent_sha256':plan['parent_sha256'],
    'candidate_parent_is_native93':True,'tests_inherited':False,'new_acceptance_inherited':False,
    'expected_previous_candidate_sha256':'d7026d0121a4d3360dfa102ccb0e5999388faa75f95d18ac49f0c486ef4e3a16',
    'previous_candidate_status':'BLOCKED_DEPENDENCY_PROFILE_NO_PROMOTION',
    'new_target_tokens':[t['token'] for t in targets],'expected_preserved_inherited_method_bodies':168,
    'expected_total_method_bodies':180,'expected_finally_regions':38,
    'source_manifests':[{'path':p.relative_to(ROOT).as_posix()+'/SOURCE-MANIFEST.json',
                         'sha256':sha((p/'SOURCE-MANIFEST.json').read_bytes())} for p in [OLD,N]]})
shutil.copyfile(N/'VERIFY-NEXT.md',OUT/'VERIFY-NEXT.md')
write(OUT/'README.md','# Bulk180 source-only delivery\n\nAdds twelve Native112 methods to Bulk168 source; fixed construction parent remains Native93. 180 method bodies = 174 historical direct targets + six companions. 38 finally regions; MemberRef1185 / StandAloneSig1888 / TypeSpec175 / MethodSpec189. No primary candidate construction, compilation or game tests. Predecessor dependency-profile failure and all prior acceptance limits remain explicit. See VERIFY-NEXT.md.\n')
freeze(OUT)

rp=ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v12-2026-10-20.json'
registry=read(rp);tokens={x['token'] for x in targets}
written=0
for m in registry['methods']:
    if m['token'] in tokens:
        m.update(state='SOURCE_WRITTEN_PENDING_VERIFICATION',source_batch=N.name,new_semantic_acceptance=False,
                 source_aggregate=OUT.relative_to(ROOT).as_posix())
        written+=1
registry.update(schema='native381-bulk-work-registry-v13',date='2026-10-21',
    previous_registry_sha256=sha(rp.read_bytes()),new_source_methods_this_turn=12,
    source_written_pending_methods=registry['source_written_pending_methods']+written,
    direct_methods_without_new_source=registry['direct_methods_without_new_source']-written,
    aggregate_source_bundle=OUT.relative_to(ROOT).as_posix(),aggregate_method_body_targets=180,
    additional_source_batch=N.relative_to(ROOT).as_posix(),new_qualified_methods=0,
    latest_fresh_conversion={'run_id':39443473339,'candidate':'Bulk162','errors':4477,
                            'heuristically_attributed_methods':225,'attribution_is_strict_semantic_proof':False,'qualification':False},
    latest_candidate={'run_id':39456112419,'candidate':'Bulk168','sha256':'d7026d0121a4d3360dfa102ccb0e5999388faa75f95d18ac49f0c486ef4e3a16',
                      'status':'BLOCKED_DEPENDENCY_PROFILE_MEMBERACCESS_UNRESOLVED_ANIMATORSTATEINFO_GET_NORMALIZEDTIME',
                      'qualification':False})
dump(ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v13-2026-10-21.json',registry)
print(json.dumps({'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','new_method_sources':12,
    'historical_worklist_tokens_touched':written,'aggregate_sources':180,'project_tests_executed':False,
    'source_manifest_sha256':sha((OUT/'SOURCE-MANIFEST.json').read_bytes())},indent=2))
