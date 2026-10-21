"""Compose C# and append-only metadata plans. Never build or execute candidate code."""
from pathlib import Path
import hashlib,json,re,shutil,sys
sys.dont_write_bytecode=True
ROOT=Path(__file__).resolve().parents[1];N=ROOT/'audit/native116-codex-2026-10-22';OLD=ROOT/'audit/native-bulk200-codex-2026-10-22'
sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,ensure_ascii=False,indent=2)+'\n')
def freeze(p):dump(p/'SOURCE-MANIFEST.json',{'schema':'source-only-manifest-v1','date':'2026-10-22','state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,'files':[{'path':f.relative_to(p).as_posix(),'bytes':f.stat().st_size,'sha256':sha(f.read_bytes())} for f in sorted(p.rglob('*')) if f.is_file() and f.name!='SOURCE-MANIFEST.json']})
md=dnfile.dnPE(str(ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).net.mdtables
cur=read(N/'CURRENT-SOURCE.json');original=read(N/'ORIGINAL-SOURCE.json');cm={m['token']:m for m in cur['methods']};om={m['token']:m for m in original['methods']}
desc=read(OLD/'METADATA-ADDITIONS.json');oldtypes=[t.Signature.value.hex() for t in md.TypeSpec.rows]
extra_types=['1d1180dc','151151011282e4'];types=oldtypes+extra_types
newdefs=[];newtypes=[];bindings=[]
def comp(v):return bytes([v]) if v<128 else bytes([128|(v>>8),v&255])
def typ(name,ref=False,value=False):
    rows=(md.TypeRef if ref else md.TypeDef).rows
    rid=next(i for i,t in enumerate(rows,1) if str(t.TypeName)==name)
    return ((b'\x11' if value else b'\x12')+comp(rid*4+(1 if ref else 0))).hex()
def field(token):return next(f for f in cur['fields'] if int(f['token'],16)==token)['signature'][2:]
def spec(blob):
    if blob not in types:types.append(blob);newtypes.append({'token':0x1B000000+len(types),'blob':blob})
    return types.index(blob)+1
def member(name,owner_blob,template):
    row=md.MemberRef.rows[template-1];sig=row.Signature.value.hex();meth=str(row.Name);rid=spec(owner_blob);cc=rid*8+4
    tok=None
    for i,r in enumerate(md.MemberRef.rows,1):
        if r.Class.table.name=='TypeSpec' and r.Class.row_index==rid and str(r.Name)==meth and r.Signature.value.hex()==sig:tok=0x0A000000+i;break
    if tok is None:
        match=next((r for r in desc['member_definitions']+newdefs if r['class_code']==cc and r.get('new_name',r['template_name']) in (None,meth) and r['template_name']==meth and r['signature']==sig),None)
        if match:tok=match['token']
    if tok is None:
        tok=0x0A000000+1193+len(newdefs);newdefs.append({'name':name,'token':tok,'class_code':cc,'owner_pin':owner_blob,'template':template,'template_name':meth,'signature':sig,'new_name':None})
    bindings.append({'name':name,'token':tok,'owner_blob':owner_blob,'method':meth,'signature':sig});return tok
def enum_refs(prefix,element):
    lst='15120d01'+element;en='15115101'+element
    for suffix,own,t in [('Enum',lst,0xB7),('Current',en,0xB8),('Move',en,0xB9),('Dispose',en,0x45)]:member('N116_'+prefix+suffix,own,t)
    return en
pre=enum_refs('PreDevice','1280f8');enemy=enum_refs('Enemy','128118');path=enum_refs('Path','12819c');entry=enum_refs('BoardEntry','1282dc')
sel=enum_refs('ZombieSelect','12811c');prop=enum_refs('PropChoose','128200');sup=enum_refs('Supplies','128234');row=enum_refs('Row','12818c');zombie=enum_refs('Zombie','128210')
level=enum_refs('Level','1282f8');hard=enum_refs('HardLevel','1282fc');cardchoose=enum_refs('CardChoose','1281c0');node=enum_refs('Node','128198');plantsave=enum_refs('PlantSave','128228')
device=enum_refs('Device','1281d4')
deviceeditor=enum_refs('DeviceEditor','121c');tmpenum=enum_refs('Tmp','128085');propsave=enum_refs('PropSave','12822c');propbank=enum_refs('PropBank','12831c');intenum=enum_refs('Int','08');supitem=enum_refs('SuppliesItem','128204')
member('N116_FunctionTryGet',field(0x040006A1),0x240)
for name,own,t in [('DeviceEditorAdd','15120d01121c',0xBF),('EnemyEditorAdd','15120d011224',0xBF),('EnemyAdd','15120d01128118',0xBF),('PathEditorAdd','15120d01122c',0xBF),('ZombieSelectCount','15120d0112811c',0x144),('PropBankItem','15120d0112831c',0x99),('SuppliesItemAdd','15120d01128204',0xBF),('GridAdd','15120d01128190',0xBF),('PlantRemove','15120d011281e0',0x36)]:member('N116_'+name,own,t)
bindings += [{'name':'N116_ZombieSelectsField','token':0x04000207},{'name':'N116_ZombieListField','token':0x04000362},{'name':'N116_PropChooseSelectedField','token':0x040008A4},{'name':'N116_SetLocalScale','token':0x0A0000E5}]
for name,own,t in [('LevelAdd','15120d011282f8',0xBF),('HardLevelAdd','15120d011282fc',0xBF),('IntCount','15120d0108',0x144),('WaveCount','15120d01128114',0x144),('NodeIndexOf','15120d01128198',0xAD),('NodeRemove','15120d01128198',0x36),('SuppliesSetItem','15120d01128234',0xA8)]:member('N116_'+name,own,t)
for name,own,t in [('ZombieCount','15120d01128210',0x144),('ZombieSetItem','15120d01128210',0xA8),('DeviceCount','15120d011281d4',0x144),('DeviceSetItem','15120d011281d4',0xA8)]:member('N116_'+name,own,t)
for name,own,t in [('DeviceInfoAdd','15120d011280cc',0xBF),('ZombieInfoAdd','15120d011280d4',0xBF),('PathClear','15120d0112819c',0xC0),('PathAdd','15120d0112819c',0xBF),('PropChooseAdd','15120d01128200',0xBF)]:member('N116_'+name,own,t)
toggle=md.MemberRef.rows[0x17-1].Class
newdefs.append({'name':'N116_ToggleIsOn','token':0x0A000000+1193+len(newdefs),'class_code':toggle.row_index*8+1,'owner_pin':'UnityEngine.UI.Toggle','template':0xB9,'template_name':'MoveNext','signature':'200002','new_name':'get_isOn'})
bindings.append({'name':'N116_ToggleIsOn','token':newdefs[-1]['token']})
bindings.append({'name':'N116_PlantCanBear','token':int(next(m['token'] for m in cur['method_index'] if m['name']=='Plant::CanBear'),16)})
bindings.append({'name':'N116_PlantSaveHealth','token':int(next(f['token'] for f in cur['fields'] if f['owner']=='PlantSave' and f['name']=='healthPoint'),16)})
extra_ms=[{'name':'N116_DeviceCircle','token':0x2B0000BE,'definition':0x060000CF,'owner':'AttackRange','method_name':'NewCircleRange','signature':'10010312601e0012150c','instantiation':'0a011281d4'},
 {'name':'N116_PlantCircleOffset','token':0x2B0000BF,'definition':0x060000D0,'owner':'AttackRange','method_name':'NewCircleRange','signature':'10010412601e0012150c114d','instantiation':'0a011281e0'}]
bindings += [{'name':v['name'],'token':v['token']} for v in extra_ms]
def local(*items):return (b'\x07'+comp(len(items))+b''.join(bytes.fromhex(x) for x in items)).hex()
# Each row is a separately reviewed native method, not a diagnostic-category substitution.
rows=[
 ('06000017','N116_DevicePreviews()',4,local(pre,'1280f8','121c')),
 ('06000018','N116_EnemyPreviews()',4,local(enemy,'128118','1224')),
 ('0600001C','N116_CreateEnemy()',4,local('128118','1224')),
 ('0600004B','N116_EnemyEditorInitialize()',4,local(path,'12819c','122c')),
 ('06000187','N116_GetBoardEntry(false)',4,local('128100','128100',entry,'1282dc')),
 ('0600026F','N116_GetBoardEntry(true)',4,local('12817c','12817c',entry,'1282dc')),
 ('06000272','N116_StartZombieBoardEntry()',4,local(entry,'1282dc','12817c')),
 ('060001A8','N116_FillWave()',4,local('08','15120d0112811c','0c',sel,'12811c')),
 ('06000697','N116_UnfoldProps()',3,local(prop,'128200',field(0x040008A4),'02')),
 ('060006D2','N116_LootIcons()',5,local(sup,'128204')),
 ('06000287','N116_MapGrids()',3,local('15120d01128190',row,'15115101128190')),
 ('0600027B','N116_KillAll()',3,local('15120d01128210','15115101128210','128210','128094')),
 ('0600035C','N116_DestroyPlant()',2,local('151151011209')),
 ('0600061A','N116_Flags()',5,local('0c','08','08','0c')),
 ('0600069F','N116_RescueLevel()',5,local(level,'1282f8')),
 ('0600052A','N116_HardLevel()',4,local('128218',hard,'1282fc')),
 ('060006BB','N116_SetPrechoose()',2,local(cardchoose,cardchoose,'1281c0','1281c0')),
 ('060006B6','N116_LoadTeamCard()',4,local(cardchoose,'08','08','1281c0','128228')),
 ('0600060A','N116_FindBoardEntryKey()',2,local(entry,'1282dc','0e','1282dc')),
 ('060001AF','N116_EnemyManagerStart()',4,local(entry,'1282dc','128120','08','128110','08','08','128114')),
 ('06000283','N116_AstarProcess()',4,local(node,'128198','08')),
 ('060004BF','N116_SaveCompleteness()',5,local(plantsave,'128228','02','08','08','128234','15120d01128234')),
 ('0600064B','N116_Warehouse()',4,local('128344','128344','128218','128348')),
 ('060001BD','N116_SetPreZombie()',3,local(enemy,'128118','128114','114d')),
 ('06000362','N116_SeekManyZombies()',3,local('15120d01128210','15115101128210','128210','08','0c','128210')),
 ('06000364','N116_SeekManyDevices()',4,local('1282ac','08','1260','15120d011281d4',device,'1281d4','128094','08','0c')),
 ('060004E3','N116_SkillFunction()',5,local('08','128250','0c')),
 ('06000163','N116_LoadReward()',2,local(sup,plantsave,'128234')),
 ('06000326','N116_DeviceDamage()',3,local('128094','1260','0c')),
 ('06000514','N116_Awake148()',7,local('1209','1215','114d','114d','15115101128210')),
 ('060004D9','N116_CreateSuppliesItem()',5,local('128204','08','128159')),
 ('06000118','N116_HoleTeleport()',4,local('114d',typ('Hide'),'1280b5','114d')),
 ('06000226','N116_LoadInfoAll(false)',2,local('15120d011280cc',typ('IEnumerator',True),typ('IDisposable',True),'1280cc')),
 ('06000231','N116_LoadInfoAll(true)',2,local('15120d011280d4',typ('IEnumerator',True),typ('IDisposable',True),'1280d4')),
 ('060000D3','N116_ExpandRangeView()',5,local(typ('IEnumerator',True),typ('IDisposable',True),'123d','0c')),
 ('06000183','N116_DisplayBannedDevice()',5,local(device,'1281d4','151151011209','1209','0c')),
 ('06000011','N116_EditorMouse()',3,local('114d','08','08',deviceeditor,'121c')),
 ('060002F5','N116_ResetChosenCard()',5,local(typ('Image',True),typ('Color',True,True),typ('Color',True,True),typ('Color',True,True),cardchoose,'1281c0')),
 ('060001A1','N116_CopyEnemy()',4,local(path,'12819c','12819c')),
 ('06000384','N116_NutTaunt()',4,local('15115101128210','128210','08','08','08','0c','12819c')),
 ('06000297','N116_GridCanPlace()',3,local('1281e0','1281d4','08')),
 ('06000710','N116_SelectZombie()',4,local('15120d0108','08','08')),
 ('060006C5','N116_SkillButton()',4,local('128244','08',tmpenum)),
 ('06000335','N116_DeviceUis()',5,local(field(0x04000492),'0c','0c','08')),
 ('06000692','N116_CreatePropList()',4,local('08','12831c',propsave,'12822c','128200','0c')),
 ('06000693','N116_LoadTeamProp()',3,local(propbank,'08',intenum,'08',prop,'128200')),
 ('0600009D','N116_InputSupplies()',3,local('0e','09','08')),
 ('0600062C','N116_LevelState()',5,local('128218','02','0c')),
 ('06000572','N116_PlantLevelUp()',3,local(supitem,'128204','128234','128218','128228')),
 ('0600035B','N116_PlantParticleAudio()',3,local('1280b5','114d','1209')),
]
count=len(rows);total=200+count
OUT=ROOT/f'audit/native-bulk{total}-codex-2026-10-22';OUT.mkdir(exist_ok=True)
targets=[];locs=[]
for i,(tok,emit,maxs,blob) in enumerate(rows,1):
    tok='0x'+tok;m=cm[tok];o=om[tok];loc=0x11000000+1908+i
    locs.append(blob);bindings.append({'name':f'N116_Locals{i}','token':loc})
    targets.append({'token':tok,'name':m['name'],'signature':m['signature'],'old_code_size':m['code_size'],'old_code_sha256':m['il_sha256'],'old_max_stack':m['max_stack'],'old_local_sig_token':f'0x{m["local_sig"]:08X}','old_local_sig_blob':m['locals_blob'],'new_declared_max_stack':maxs,'local_constant':f'N116_Locals{i}','local_blob':blob,'emission_method':emit,'explicit_companion':False,'source_batch':N.name,'original_native':{k:o[k] for k in ['start','end','sha256','pdata']}})
bind='internal static partial class Program\n{\n'+''.join(f'    private const int {v["name"]} = 0x{v["token"]:08X};\n' for v in bindings)
eh_counts={'0x06000017':1,'0x06000018':1,'0x0600004B':1,'0x06000187':1,'0x0600026F':1,'0x06000272':1,'0x060001A8':1,'0x06000697':1,'0x060006D2':1,'0x06000287':2,'0x0600027B':2,'0x0600035C':1,'0x0600069F':1,'0x0600052A':1,'0x060006BB':3,'0x060006B6':2,'0x0600060A':1,'0x060001AF':1,'0x06000283':1,'0x060004BF':1}
eh_counts.update({'0x060001BD':1,'0x06000362':1,'0x06000364':1,'0x06000163':2,'0x06000514':1})
eh_counts.update({'0x06000226':1,'0x06000231':1,'0x060000D3':1,'0x06000183':2,'0x06000011':1,'0x060002F5':1,'0x060001A1':1,'0x06000384':1,'0x060006C5':1,'0x06000692':1,'0x06000693':3,'0x06000572':1})
bind+='    private static readonly HashSet<int> N116_EhTargets=['+','.join(eh_counts)+'];\n'
bind+='    private const float N116_FlagSpan=436f, N116_FlagOrigin=218f, N116_FlagY=5f;\n}\n'
bind=bind[:-2]+'    private const float N116_Range31=350f,N116_Range32=270f,N116_Damage31=5000f,N116_Damage32=1800f;\n    private const float N116_EmptyColorR=.6f,N116_EmptyColorG=.6f,N116_EmptyColorB=.6f,N116_EmptyColorA=1f,N116_TeleportVolume=1.2f,N116_TeleportPitch=1.5f;\n}\n'
names=[(0x70001ACE,0,0x04000177),(0x70001ADE,0,0x04000178),(0x70001B06,1,0),(0x70001B20,2,0),(0x70001B30,3,0),(0x70001B46,0,0x0400017A),(0x70001B4E,0,0x04000179)]
parent=dnfile.dnPE(str(ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'))
def fnv(s):
    v=2166136261
    for x in s:v=((v^ord(x))*16777619)&0xFFFFFFFF
    return v
name_rows=[{'hash':fnv(parent.net.user_strings.get(t&0xFFFFFF).value),'string_token':t,'literal':parent.net.user_strings.get(t&0xFFFFFF).value,'kind':k,'field':f} for t,k,f in names]
bind=bind[:-2]+'    private static readonly (uint Hash,int String,int Kind,int Field)[] N116_InputNames=[\n'+''.join(f'        (0x{x["hash"]:08X}u,0x{x["string_token"]:08X},{x["kind"]},0x{x["field"]:08X}),\n' for x in name_rows)+'    ];\n}\n'
write(N/'Bindings.cs',bind)
nd={'schema':'native116-source-metadata-v1','member_definitions':newdefs,'local_blobs':locs,'constants':bindings,'bindings':bindings,'added_typespecs':newtypes,'added_methodspecs':extra_ms,'added_user_strings':[],
 'appended_blob_payload_bytes':sum(1+len(bytes.fromhex(b)) for b in locs)+sum(1+len(bytes.fromhex(t['blob'])) for t in newtypes)+sum(1+len(bytes.fromhex(v['instantiation'])) for v in extra_ms),'appended_string_payload_bytes':sum(len(v['new_name'].encode())+1 for v in newdefs if v['new_name'] is not None),'input_string_dispatch':name_rows}
dump(N/'METADATA-ADDITIONS.json',nd)
dump(N/'IMPLEMENTATION-PLAN.json',{'schema':'native116-source-implementation-v1','date':'2026-10-22','state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,'new_qualified_methods':0,'targets':targets})
write(N/'README.md',f'# Native116 source checkpoint\n\n{count} new unique method-body sources after Bulk200. Source only; no candidate constructed, no project tests executed, no formal promotion. The next concentrated test batch is deferred until 50 new unique method sources. This checkpoint is not a test dispatch.\n')
for item in read(OLD/'SOURCE-MANIFEST.json')['files']:
    p=OLD/item['path']
    if p.suffix=='.cs':shutil.copyfile(p,OUT/p.name)
for name in ['MethodBodies.cs','Bindings.cs']:shutil.copyfile(N/name,OUT/('N116.'+name))
program=(OUT/'Program.cs').read_text(encoding='utf-8');pins=[]
for t in targets:
    m=cm[t['token']];owner,name=m['name'].split('::');owner=owner.rsplit('.',1)[-1]
    pins.append(f'        new({t["token"]},"{owner}","{name}","{m["signature"]}",{m["code_size"]},{m["max_stack"]},0x{m["local_sig"]:08X},"{m["locals_blob"]}","{m["il_sha256"]}"),')
at=program.index('];',program.index('private static readonly Pin[] Pins = ['));program=program[:at]+'\n'.join(pins)+'\n    '+program[at:]
plans=[f'            (Token:{t["token"]}, Code:{t["emission_method"]}, Max:{t["new_declared_max_stack"]}, Locals:{t["local_constant"]})' for t in targets]
at=program.index('};',program.index('var plans = new[] {'));program=program[:at].rstrip()+',\n'+',\n'.join(plans)+'\n        '+program[at:]
program=program.replace('fresh-bulk200.dll',f'fresh-bulk{total}.dll').replace('independent 200-target',f'independent {total}-target')
program=re.sub(r'appended_memberref_count = 92, appended_local_signature_count = 143,',f'appended_memberref_count = {92+len(newdefs)}, appended_local_signature_count = {143+count},',program)
program=program.replace('appended_typespec_count = 2, appended_methodspec_count = 2,',f'appended_typespec_count = {len(types)-173}, appended_methodspec_count = {4+len(extra_ms)},')
program=program.replace('including thirty finally regions and new US/MethodSpec support construction/isolation, all method fixtures/mutants and 112-method regression; no IPA','source restoration, metadata and byte isolation, every new method fixture/mutant and inherited regression; no acceptance from construction; no IPA')
write(OUT/'Program.cs',program)
meta=(OUT/'MetadataAppend.cs').read_text(encoding='utf-8')
at=meta.index('];',meta.index('private static readonly MemberAddition[] MemberAdditions = ['))
meta=meta[:at]+'\n'.join('        new(0x%08X,%d,%s,0x%X,%s,%s,%s),'%(x['token'],x['class_code'],json.dumps(x['owner_pin']),x['template'],json.dumps(x['template_name']),json.dumps(x['signature']),json.dumps(x['new_name']) if x['new_name'] is not None else 'null') for x in newdefs)+'\n    '+meta[at:]
at=meta.index('];',meta.index('private static readonly string[] NewLocalBlobs = ['));meta=meta[:at]+'\n'.join('        '+json.dumps(x)+',' for x in locs)+'\n    '+meta[at:]
old_array='private static readonly string[] AddedTypeBlobs = ["1d1180dc","151151011282e4"];'
meta=meta.replace(old_array,'private static readonly string[] AddedTypeBlobs = ['+','.join(json.dumps(x) for x in types[173:])+'];')
write(OUT/'MetadataAppend.cs',meta)
at=meta.index('];',meta.index('private static readonly ExtraSpec[] ExtraMethodSpecs = ['))
meta=meta[:at]+'\n'.join('        new(0x%08X,0x%X,%s,%s,%s,%s),'%(x['token'],x['definition']&0xFFFFFF,json.dumps(x['owner']),json.dumps(x['method_name']),json.dumps(x['signature']),json.dumps(x['instantiation'])) for x in extra_ms)+'\n    '+meta[at:]
write(OUT/'MetadataAppend.cs',meta)
for k in ['member_definitions','local_blobs','constants']:desc[k]+=nd[k]
desc['added_typespecs']=[{'token':0x1B000000+174+i,'blob':blob} for i,blob in enumerate(types[173:])]
desc['type_blobs']+= [t['blob'] for t in newtypes]
desc['added_methodspecs']+=extra_ms
for k in ['appended_blob_payload_bytes','appended_string_payload_bytes']:desc[k]+=nd[k]
desc['schema']=f'bulk{total}-source-metadata-v1';dump(OUT/'METADATA-ADDITIONS.json',desc)
proj=(OLD/'Bulk200.csproj').read_text(encoding='utf-8').replace('  </ItemGroup>','    <Compile Include="N116.MethodBodies.cs"/>\n    <Compile Include="N116.Bindings.cs"/>\n  </ItemGroup>');write(OUT/f'Bulk{total}.csproj',proj)
oldplan=read(OLD/'IMPLEMENTATION-PLAN.json')
eh_count=39+sum(eh_counts.values())
plan={**oldplan,'schema':f'bulk{total}-source-implementation-v1','targets':oldplan['targets']+targets,'method_body_targets':total,'target_tokens':oldplan['target_tokens']+[t['token'] for t in targets],'historical_direct_targets':194+count,'explicit_companion_targets':6,'unchanged_other_method_bodies':2317-total,'memberref_count_after':1192+len(newdefs),'standalone_count_after':1908+count,'typespec_count_after':len(types),'methodspec_count_after':189+len(extra_ms),'metadata_row_payload_added_bytes':858+len(newdefs)*6+count*2+len(newtypes)*2+len(extra_ms)*4,'expected_finally_regions':eh_count,'root_executed_project_tests':False,'new_qualified_methods':0,'source_checkpoint_only':count<50,'new_source_method_count_after_bulk200':count}
inherited_eh_tokens=re.findall(r'0x0600[0-9A-F]{4}',(OLD/'FinallyBodies.cs').read_text(encoding='utf-8'))
plan.update(appended_finally_method_tokens=inherited_eh_tokens+list(eh_counts),new_finally_clauses_by_method=eh_counts,
 new_finally_clause_count=sum(eh_counts.values()),new_unique_source_methods_this_turn=count,
 supersedes_source_bundle=OLD.relative_to(ROOT).as_posix(),comparison_to_identified_bulk200={'changed_bodies':count,'unchanged_bodies':2317-count,'identity_hash':'MUST_BE_SUPPLIED_BY_AGY_AFTER_CURRENT_SCOPE'},
 new_source_dependencies_requiring_integration=['0x060004E3 -> preserved Bulk200 body 0x060004E4 Skill.End','0x06000514 -> 0x06000362 and appended AttackRange.NewCircleRange<Plant> with offset','0x06000118 -> original Hide::.ctor(bool,Zombie)','0x06000011 -> public UnityEngine.UI.Toggle.get_isOn','0x060001AF -> CreateEnemySelecter / ExtendFlagList / FillWave / SetPreZombie'],
 source_evidence_method_count=len(cm),source_evidence_extra_methods_are_not_targets=True)
dump(OUT/'IMPLEMENTATION-PLAN.json',plan)
dump(OUT/'SOURCE-LINEAGE.json',{'schema':f'bulk{total}-source-lineage-v1','parent_sha256':plan['parent_sha256'],'candidate_parent_is_native93':True,'tests_inherited':False,'new_acceptance_inherited':False,'new_source_batch_tokens':[t['token'] for t in targets],'source_predecessor':OLD.relative_to(ROOT).as_posix(),'expected_previous_source_method_bodies':200,'expected_total_method_bodies':total,'expected_finally_regions':eh_count,'candidate':None,'execution_comparison_must_identify_actual_previous_candidate':True})
write(OUT/'README.md',f'# Bulk{total} source delivery\n\n200 inherited sources plus {count} new direct worklist methods = {total} bodies ({194+count} direct +6 companions); {2317-total} non-target bodies. No candidate constructed or tests executed by primary author. No semantic acceptance. MemberRef {1192+len(newdefs)}, StandAloneSig {1908+count}, TypeSpec {len(types)}, MethodSpec {189+len(extra_ms)}; {eh_count} finally clauses.\n\nConstruct from fixed Native93 only. Current AGY task through Bulk200 must retain priority; identify its real candidate SHA before comparing for {count} body differences / {2317-count} identical bodies. This delivery meets the requested +50 authoring threshold and queues one concentrated test batch; it does not retroactively pass Bulk200. See VERIFY-NEXT.md.\n')
ledger=read(ROOT/'audit/NEXT50-AFTER-BULK200-2026-10-22.json');ledger.update(completed_new_unique_method_sources=count,new_source_tokens=[t['token'] for t in targets],source_aggregate=OUT.relative_to(ROOT).as_posix(),remaining_new_method_sources=50-count,source_state='SOURCE_WRITTEN_UNBUILT_UNEXECUTED',new_scope_testing='QUEUED_AFTER_PRIOR_SCOPE_PASS' if count==50 else 'DEFERRED_UNTIL_50_NEW_METHOD_SOURCES')
dump(ROOT/'audit/NEXT50-AFTER-BULK200-2026-10-22.json',ledger)
reg=read(ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v16-2026-10-22.json')
for m in reg['methods']:
    if m['token'] in ledger['new_source_tokens']:m.update(state='SOURCE_WRITTEN_PENDING_VERIFICATION',source_batch=N.name,source_aggregate=OUT.relative_to(ROOT).as_posix(),new_semantic_acceptance=False)
reg.update(schema='native381-bulk-work-registry-v17',date='2026-10-22',source_written_pending_methods=194+count,direct_methods_without_new_source=187-count,aggregate_source_bundle=OUT.relative_to(ROOT).as_posix(),aggregate_method_body_targets=total,new_source_methods_after_bulk200=count,next_concentrated_test_requires_50_new_sources=True,new_qualified_methods=0)
reg.update(new_source_methods_this_turn=count,additional_source_batch=N.relative_to(ROOT).as_posix(),previous_registry_sha256=sha((ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v16-2026-10-22.json').read_bytes()))
dump(ROOT/'audit/NATIVE381-BULK-WORK-REGISTRY-v17-2026-10-22.json',reg)
if (N/'VERIFY-NEXT.md').exists():shutil.copyfile(N/'VERIFY-NEXT.md',OUT/'VERIFY-NEXT.md')
freeze(N);freeze(OUT)
print(json.dumps({'scope':'SOURCE_TEXT_AND_METADATA_PLAN_COMPOSITION_ONLY','new_unique_method_sources':count,'aggregate_sources':total,'remaining_until_test_batch':50-count,'candidate_constructed':False,'project_tests_executed':False}))
