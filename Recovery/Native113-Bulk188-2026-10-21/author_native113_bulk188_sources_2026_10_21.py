"""Compose unexecuted source files and metadata descriptions from immutable inputs."""
from pathlib import Path
import hashlib
import json
import shutil
import sys

ROOT=Path(__file__).resolve().parents[1]
N=ROOT/'audit/native113-codex-2026-10-21'
OLD=ROOT/'audit/native-bulk180-codex-2026-10-21'
OUT=ROOT/'audit/native-bulk188-codex-2026-10-21'
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
def comp(v):return bytes([v]) if v<128 else bytes([(v>>8)|128,v&255])
def typ(name,ref=False,value=False):
    rows=md.TypeRef.rows if ref else md.TypeDef.rows
    rid=next(i for i,t in enumerate(rows,1) if str(t.TypeName)==name)
    return (b'\x11' if value else b'\x12')+comp((rid<<2)|(1 if ref else 0))
def local(*items):return (b'\x07'+comp(len(items))+b''.join(items)).hex()
vec=typ('Vector3',True,True);image=typ('Image',True);color=typ('Color',True,True)
blobs=[
    local(b'\x1d\x0e',b'\x08'),
    local(vec,b'\x0c',b'\x0c',b'\x0c',image),
    local(typ('Almanac_PlantDetail'),typ('SeedChooserScreen')),
    local(typ('Save')),
    local(b'\x08',image,color,b'\x0c',b'\x0c',b'\x0c'),
    local(typ('StatsIncreased'),b'\x0c',b'\x0c',b'\x0c',typ('Animator',True),b'\x08'),
    local(typ('StatsIncreased'),typ('AudioClip',True),vec,b'\x0c'),
    local(bytes.fromhex('15115101128210'),typ('Zombie')),
]
constants=[{'name':f'N113_Locals{i+1}','token':0x11000000+1889+i} for i in range(len(blobs))]
rows=[('0x06000306','N113_DeviceStartAnimationBody',3,1),
      ('0x060003FF','N113_PropUpdateBody',4,2),
      ('0x0600054A','N113_ClosePlantDetailBody',2,3),
      ('0x060006AC','N113_SaveRowStartBody',5,4),
      ('0x060002F4','N113_ChooseCardBody',5,5),
      ('0x0600038D','N113_StarfruitBody',4,6),
      ('0x060004A7','N113_SnowbeastWallBody',3,7),
      ('0x060004A8','N113_SnowbeastHitTestBody',3,8)]
cm={m['token']:m for m in read(N/'CURRENT-SOURCE.json')['methods']}
om={m['token']:m for m in read(N/'ORIGINAL-SOURCE.json')['methods']}
targets=[]
for tok,body,maxs,loc in rows:
    m=cm[tok];o=om[tok]
    targets.append({'token':tok,'name':m['name'],'signature':m['signature'],'old_code_size':m['code_size'],
        'old_code_sha256':m['il_sha256'],'old_max_stack':m['max_stack'],'old_local_sig_token':f'0x{m["local_sig"]:08X}',
        'old_local_sig_blob':m['locals_blob'],'new_declared_max_stack':maxs,'local_constant':f'N113_Locals{loc}',
        'local_blob':blobs[loc-1],'emission_method':body,'explicit_companion':False,'source_batch':N.name,
        'original_native':{k:o[k] for k in ['start','end','sha256','pdata']}})
nd={'schema':'native113-source-metadata-v1','member_definitions':[],'local_blobs':blobs,'constants':constants,
    'appended_blob_payload_bytes':sum(1+len(bytes.fromhex(b)) for b in blobs),
    'appended_string_payload_bytes':0,'added_typespecs':[],'added_methodspecs':[],'added_user_strings':[]}
dump(N/'METADATA-ADDITIONS.json',nd)
dump(N/'IMPLEMENTATION-PLAN.json',{'schema':'native113-source-implementation-v1','date':'2026-10-21',
    'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,
    'new_qualified_methods':0,'targets':targets,'appended_finally_method_tokens':['0x060004A8']})
p=N/'PENDING-SOURCE-INPUTS.json';prep=read(p)
prep.update(scope='SOURCE_INPUTS_AND_EIGHT_UNEXECUTED_BODY_SOURCES',new_source_method_count=8)
dump(p,prep)
write(N/'README.md','# Native113: eight native-backed method-body sources\n\nSource-only. Device animation switch, prop update, plant-detail close, save row, card selection, Starfruit and two Snowbeast methods. No candidate construction or primary project tests. Original switch data and exact typed Enumerator<Zombie> references are preserved. See VERIFY-NEXT.md.\n')
freeze(N)

OUT.mkdir(exist_ok=False)
for item in read(OLD/'SOURCE-MANIFEST.json')['files']:
    p=OLD/item['path']
    if p.suffix=='.cs':shutil.copyfile(p,OUT/p.name)
shutil.copyfile(N/'MethodBodies.cs',OUT/'N113.MethodBodies.cs')
program=(OUT/'Program.cs').read_text(encoding='utf-8')
pins=[]
for t in targets:
    m=cm[t['token']];owner,name=m['name'].split('::');owner=owner.rsplit('.',1)[-1]
    pins.append(f'        new({t["token"]},"{owner}","{name}","{m["signature"]}",{m["code_size"]},{m["max_stack"]},0x{m["local_sig"]:08X},"{m["locals_blob"]}","{m["il_sha256"]}"),')
at=program.index('];',program.index('private static readonly Pin[] Pins = ['))
program=program[:at]+'\n'.join(pins)+'\n    '+program[at:]
plans=[f'            (Token:{t["token"]}, Code:{t["emission_method"]}(), Max:{t["new_declared_max_stack"]}, Locals:{t["local_constant"]})' for t in targets]
at=program.index('};',program.index('var plans = new[] {'))
program=program[:at].rstrip()+',\n'+',\n'.join(plans)+'\n        '+program[at:]
program=program.replace('fresh-bulk180.dll','fresh-bulk188.dll').replace('appended_memberref_count = 85, appended_local_signature_count = 123,','appended_memberref_count = 85, appended_local_signature_count = 131,').replace('independent 180-target','independent 188-target')
write(OUT/'Program.cs',program)
f=(OUT/'FinallyBodies.cs').read_text(encoding='utf-8').replace(',"Explicit EH target allowlist"',' or 0x060004A8,"Explicit EH target allowlist"')
write(OUT/'FinallyBodies.cs',f)
meta=(OUT/'MetadataAppend.cs').read_text(encoding='utf-8')
meta=meta.replace('    private record MemberAddition(',
    '\n'.join(f'    private const int {x["name"]} = 0x{x["token"]:08X};' for x in constants)+'\n    private record MemberAddition(',1)
at=meta.index('];',meta.index('private static readonly string[] NewLocalBlobs = ['))
meta=meta[:at]+'\n'.join('        '+json.dumps(x)+',' for x in blobs)+'\n    '+meta[at:]
write(OUT/'MetadataAppend.cs',meta)
desc=read(OLD/'METADATA-ADDITIONS.json');desc['schema']='bulk188-source-metadata-v1'
for k in ['local_blobs','constants']:desc[k]+=nd[k]
desc['appended_blob_payload_bytes']+=nd['appended_blob_payload_bytes']
dump(OUT/'METADATA-ADDITIONS.json',desc)
write(OUT/'Bulk188.csproj',(OLD/'Bulk180.csproj').read_text(encoding='utf-8').replace('  </ItemGroup>','    <Compile Include="N113.MethodBodies.cs"/>\n  </ItemGroup>'))
plan=read(OLD/'IMPLEMENTATION-PLAN.json')
plan.update(schema='bulk188-source-implementation-v1',date='2026-10-21',targets=plan['targets']+targets,
    method_body_targets=188,target_tokens=plan['target_tokens']+[t['token'] for t in targets],
    historical_direct_targets=182,explicit_companion_targets=6,unchanged_other_method_bodies=2129,
    memberref_count_after=1185,standalone_count_after=1896,methodspec_count_after=189,
    metadata_row_payload_added_bytes=792,appended_blob_payload_bytes=desc['appended_blob_payload_bytes'],
    new_unique_source_methods_this_turn=8,supersedes_source_bundle=OLD.relative_to(ROOT).as_posix())
dump(OUT/'IMPLEMENTATION-PLAN.json',plan)
dump(OUT/'SOURCE-LINEAGE.json',{'schema':'bulk188-source-lineage-v1','parent_sha256':plan['parent_sha256'],
    'candidate_parent_is_native93':True,'tests_inherited':False,'new_acceptance_inherited':False,
    'expected_previous_candidate_sha256':'e9abad3a618ceeac93a9494507c40669f84efdaa8f917c144dbc411736cb3157',
    'previous_candidate_identity_reported_by':'AGY Run 39537961323; primary did not execute candidate tests',
    'previous_candidate_status':'BLOCKED_DEPENDENCY_PROFILE_NO_PROMOTION',
    'new_target_tokens':[t['token'] for t in targets],'expected_preserved_inherited_method_bodies':180,
    'expected_total_method_bodies':188,'expected_finally_regions':39,
    'source_manifests':[{'path':p.relative_to(ROOT).as_posix()+'/SOURCE-MANIFEST.json',
                         'sha256':sha((p/'SOURCE-MANIFEST.json').read_bytes())} for p in [OLD,N]]})
shutil.copyfile(N/'VERIFY-NEXT.md',OUT/'VERIFY-NEXT.md')
write(OUT/'README.md','# Bulk188 source-only delivery\n\nEight Native113 bodies added to unchanged Bulk180 source. Fixed Native93 construction parent, 188 bodies = 182 historical direct targets + six companions. 39 finally regions. MemberRef1185 / StandAloneSig1896 / TypeSpec175 / MethodSpec189. No primary candidate construction, compilation or project tests. Prior dependency-profile block and behavior limits are retained.\n')
freeze(OUT)
rp=ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v13-2026-10-21.json';reg=read(rp)
tokens={t['token'] for t in targets};touched=0
for m in reg['methods']:
    if m['token'] in tokens:
        m.update(state='SOURCE_WRITTEN_PENDING_VERIFICATION',source_batch=N.name,new_semantic_acceptance=False,
                 source_aggregate=OUT.relative_to(ROOT).as_posix());touched+=1
reg.update(schema='native381-bulk-work-registry-v14',date='2026-10-21',previous_registry_sha256=sha(rp.read_bytes()),
    new_source_methods_this_turn=8,source_written_pending_methods=reg['source_written_pending_methods']+touched,
    direct_methods_without_new_source=reg['direct_methods_without_new_source']-touched,
    aggregate_source_bundle=OUT.relative_to(ROOT).as_posix(),aggregate_method_body_targets=188,
    additional_source_batch=N.relative_to(ROOT).as_posix(),new_qualified_methods=0,
    latest_candidate={'run_id':39537961323,'candidate':'Bulk180','sha256':'e9abad3a618ceeac93a9494507c40669f84efdaa8f917c144dbc411736cb3157',
                      'status':'BLOCKED_DEPENDENCY_PROFILE_MEMBERACCESS_UNRESOLVED_ANIMATORSTATEINFO_GET_NORMALIZEDTIME',
                      'qualification':False,'report_source':'AGY completion; primary did not rerun project tests'})
dump(ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v14-2026-10-21.json',reg)
print(json.dumps({'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','new_method_sources':8,'worklist_tokens_touched':touched,
    'aggregate_sources':188,'project_tests_executed':False,'source_manifest_sha256':sha((OUT/'SOURCE-MANIFEST.json').read_bytes())},indent=2))
