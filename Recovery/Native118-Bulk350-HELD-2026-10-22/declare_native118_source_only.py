"""Source declarations for Native118, composed as text only; never execute game methods."""
from pathlib import Path
import json,sys
sys.dont_write_bytecode=True
R=Path(__file__).resolve().parents[1];N=R/'audit/native118-codex-2026-10-22';O=R/'audit/native-bulk300-r2-codex-2026-10-22'
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile
md=dnfile.dnPE(str(R/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).net.mdtables
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
c=read(N/'CURRENT-SOURCE.json');old=read(O/'METADATA-ADDITIONS.json');plan=read(O/'IMPLEMENTATION-PLAN.json')
types=[x.Signature.value.hex() for x in md.TypeSpec.rows]+old['type_blobs']
# Historical type_blobs is append-only payload, not the complete original table.
if len(types)!=plan['typespec_count_after']:raise ValueError(('type catalog construction',len(types),plan['typespec_count_after']))
newtypes=[];defs=[];bindings=[];locals=[];rows=[];methodspecs=[]
def comp(v):return bytes([v]) if v<128 else bytes([128|(v>>8),v&255])
def type_blob(name,ref=False,value=False):
    t=(md.TypeRef if ref else md.TypeDef).rows
    i=next(i for i,x in enumerate(t,1) if str(x.TypeName)==name)
    return ((b'\x11' if value else b'\x12')+comp(i*4+int(ref))).hex()
def type_token(name,ref=False):
    t=(md.TypeRef if ref else md.TypeDef).rows
    return (0x01000000 if ref else 0x02000000)+next(i for i,x in enumerate(t,1) if str(x.TypeName)==name)
def field(tok):return next(f['signature'][2:] for f in c['fields'] if int(f['token'],16)==tok)
def spec(blob):
    if blob not in types:types.append(blob);newtypes.append({'token':0x1B000000+len(types),'blob':blob})
    return types.index(blob)+1
def bind(name,token):bindings.append({'name':'N118_'+name,'token':token});return token
def member(name,blob,template):
    owner=spec(blob);rr=md.MemberRef.rows[template-1];method=str(rr.Name);sig=rr.Signature.value.hex();cc=owner*8+4
    tok=next((0x0A000000+i for i,r in enumerate(md.MemberRef.rows,1) if r.Class.table.name=='TypeSpec' and r.Class.row_index==owner and str(r.Name)==method and r.Signature.value.hex()==sig),None)
    if tok is None:
        found=next((x for x in old['member_definitions']+defs if x['class_code']==cc and (x.get('new_name') or x['template_name'])==method and x['signature']==sig),None)
        if found:tok=found['token']
    if tok is None:
        tok=0x0A000000+plan['memberref_count_after']+1+len(defs)
        defs.append(dict(name='N118_'+name,token=tok,class_code=cc,owner_pin=blob,template=template,template_name=method,signature=sig,new_name=None))
    return bind(name,tok)
def renamed(name,template,newname):
    rr=md.MemberRef.rows[template-1];owner=rr.Class.row;pin=(str(owner.TypeNamespace)+'.' if str(owner.TypeNamespace) else '')+str(owner.TypeName)
    tok=0x0A000000+plan['memberref_count_after']+1+len(defs)
    defs.append(dict(name='N118_'+name,token=tok,class_code=rr.Class.row_index*8+1,owner_pin=pin,template=template,template_name=str(rr.Name),signature=rr.Signature.value.hex(),new_name=newname))
    return bind(name,tok)

def external_from(name,owner,template,newname):
    rr=md.MemberRef.rows[template-1];ty=md.TypeRef.rows[owner-1]
    pin=str(ty.TypeNamespace)+'.'+str(ty.TypeName);sig=rr.Signature.value.hex();cc=owner*8+1
    found=next((x for x in old['member_definitions']+defs if x['class_code']==cc and (x.get('new_name') or x['template_name'])==newname and x['signature']==sig),None)
    if found:return bind(name,found['token'])
    tok=0x0A000000+plan['memberref_count_after']+1+len(defs)
    defs.append(dict(name='N118_'+name,token=tok,class_code=cc,owner_pin=pin,template=template,template_name=str(rr.Name),signature=sig,new_name=newname))
    return bind(name,tok)
def enum_refs(prefix,element):
    l='15120d01'+element;e='15115101'+element
    for suffix,owner,t in [('Enum',l,0xB7),('Current',e,0xB8),('Move',e,0xB9),('Dispose',e,0x45)]:member(prefix+suffix,owner,t)
    return e
def row(tok,emit,maxs,localtypes,eh=[]):
    blob='07'+comp(len(localtypes)).hex()+''.join(localtypes)
    rows.append(dict(token=f'0x{tok:08X}',emission_method=emit,max_stack=maxs,local_blob=blob,eh_clauses=eh))

def game_spec(alias,definition,element):
    inst='0a01'+element
    for i,x in enumerate(md.MethodSpec.rows,1):
        if x.Method.table.name=='MethodDef' and x.Method.row_index==(definition&0xFFFFFF) and x.Instantiation.value.hex()==inst:return bind(alias,0x2B000000+i)
    oldspec=next((x for x in old['added_methodspecs']+methodspecs if x['definition']==definition and x['instantiation']==inst),None)
    if oldspec:return bind(alias,oldspec['token'])
    method=md.MethodDef.rows[(definition&0xFFFFFF)-1];owner=next(t for t in md.TypeDef.rows if any(m.row_index==(definition&0xFFFFFF) for m in t.MethodList))
    tok=0x2B000000+plan['methodspec_count_after']+len(methodspecs)+1
    methodspecs.append(dict(name='N118_'+alias,token=tok,definition=definition,owner=str(owner.TypeName),method_name=str(method.Name),signature=method.Signature.value.hex(),instantiation=inst));return bind(alias,tok)
# Missing managed string literals are restored from original metadata, not translated.
j=read(N/'JUMP-TABLES-AND-LITERALS.json');cells={int(x['address'],16):x for x in j['string_cells']}
userstrings=[];user_end=plan['user_string_heap_before']+plan['appended_user_string_payload_bytes']
def literal(alias,cell):
    global user_end
    x=cells[cell];tok=int(x['managed_token'],16) if x['managed_token'] else None
    if tok is None:
        inherited=next((y for y in old['user_strings'] if y['value']==x['text']),None)
        if inherited:tok=inherited['token']
    if tok is None:
        found=next((y for y in userstrings if y['value']==x['text']),None)
        if found:tok=found['token']
        else:
            payload=x['text'].encode('utf-16-le');flag=int(any(ord(c)>255 or 1<=ord(c)<=8 or 14<=ord(c)<=31 or ord(c) in (0x27,0x2D,0x7F) for c in x['text']))
            encoded=comp(len(payload)+1)+payload+bytes([flag]);tok=0x70000000+user_end
            userstrings.append(dict(name='N118_'+alias,token=tok,value=x['text'],encoded_bytes=encoded.hex(),native_cell=x['address'],native_literal_index=x['index'],native_utf8_hex=x['utf8_hex']))
            user_end+=len(encoded)
    return bind(alias,tok)
tables=[]
for t in j['jump_tables']:
    values=[literal(t['name']+str(x['case']),int(x['cell'],16)) for x in t['entries']]
    tables.append(dict(name='N118_'+t['name']+'Literals',values=values,source_table=t['table'],source_bytes=t['table_bytes']))
for name,cell in [('Empty',0x181BCF088),('PopupPrefix',0x181BA5288),('PopupSuffix',0x181BA08B0),('Adventure',0x181BA12D0),('Rescue',0x181BA3BA8),('HighDifficulty',0x181BA7508),('ArmorPrefix',0x181BA02B0),('HpPrefix',0x181BA60C8),('ToughPrefix',0x181BA7448),('DefPrefix',0x181BA71A8)]:literal(name,cell)
row(0x06000670,'N118_PopupDescription()',3,['0e','08','0e'])
row(0x060005A0,'N118_Armor1Info()',3,['08','0c','0e',type_blob('TMP_Text',True)])
row(0x06000385,'N118_Peasniper()',5,['1209','114d','1215','128210','1281d4'])
prop_selector_enum=enum_refs('PropSelector','12831c')
for name,t in [('PropChooseCtor',0xAA),('PropChooseAdd',0xBF),('PropChooseCount',0x144),('PropChooseItem',0xAB)]:member(name,'15120d01128200',t)
member('PropInsert',field(0x040000F1),0x49)
row(0x060000E8,'N118_LoadProps()',4,['15120d01128200',prop_selector_enum,'12831c',type_blob('Banks'),'08','128200','1281fc','08','08','1215'],[['try','finally','after']])
zselect_enum=enum_refs('ZombieSelect','12811c')
enemy_enum=enum_refs('Enemy','128118');entry_enum=enum_refs('Entry','1282dc')
for name,t in [('EnemyCtor',0xAA),('EnemyAdd',0xBF),('EnemyCount',0x144),('EnemyItem',0xAB)]:member(name,'15120d01128118',t)
member('ZombieSelectCount',field(0x04000207),0x144)
member('EntryListCtor','15120d011282dc',0xAA);member('EntryListAdd','15120d011282dc',0xBF)
row(0x060001A7,'N118_SelectorPreviews()',3,['08',zselect_enum,'12811c','15120d01128118','128118','0c'],[['tryCount','finallyCount','afterCount'],['tryEach','finallyEach','afterEach'],['tryWeight','finallyWeight','afterWeight']])
row(0x060005D3,'N118_BoardEnemyPreviews()',4,['15120d01128118','128118','08','0c','0c','0c','15120d011282dc',entry_enum,'1282dc',enemy_enum], [['tryEntries','finallyEntries','afterEntries'],['tryEnemies','finallyEnemies','done']])
row(0x06000080,'N118_ZombieInputString()',3,['0e','08','0c'])
row(0x06000096,'N118_BoardInputString()',3,['0e','08','0c','128188'])
row(0x06000032,'N118_DeviceEditorInputString()',3,['0e','08'])
row(0x06000043,'N118_EnemyEditorInputString()',3,['0e','08','0c','1224',type_blob('TMP_InputField',True),'0e'])
row(0x06000042,'N118_EnemyEditorLoadData()',3,['128118','08',type_blob('TMP_InputField',True),'0e'])
member('PathEditorCount',field(0x04000054),0x144);member('PathEditorItem',field(0x04000054),0xAB)
row(0x06000058,'N118_PathEditorLoadData()',3,['08','122c','08','12819c',type_blob('TMP_InputField',True),'0e',type_blob('TMP_Text',True),'08'])
row(0x06000030,'N118_DeviceEditorLoadData()',3,['1280f8','1281c8','1281cc','08','08',type_blob('TMP_InputField',True),'0e','1281d0'])
for name,ty in [('TmpCharType','TMP_CharacterInfo'),('TmpMeshType','TMP_MeshInfo'),('Color32Type','Color32')]:bind(name,type_token(ty,True))
row(0x060007DB,'N118_VertexColorCycler()',4,['08',type_blob('VertexColorCycler'),'08','08','08','1d1181a5','1181a5'])
row(0x0600006C,'N118_ComputePlantData()',4,['1d08',type_blob('PlantData'),'0c','0c','1d0c',type_blob('TMP_Text',True),'08','08','08','0c','0d','0d','0e'])
row(0x060006CB,'N118_SetDataUi()',5,['0e',type_blob('TMP_Text',True),'0e']+['0c']*9)
block_enum=enum_refs('BlockZombie','128210');member('BlockZombieCount',field(0x040004DD),0x144)
row(0x06000396,'N118_AttackStartTry()',2,['08',block_enum,'128210','02'],[['tryBlock','finallyBlock','afterBlock']])
row(0x060001DB,'N118_GetDeviceFromBank()',3,['1281d4',type_blob('IEnumerator',True),type_blob('IDisposable',True),'1281d4'],[['tryChildren','finallyChildren','afterChildren']])
member('ProjectRemove',field(0x040002C2),0x36)
member('ProjectSpriteCount',field(0x04000526),0x144)
row(0x060003C1,'N118_ProjectUpdate()',5,['0c','0c','0c','1215','114d','08',type_blob('SpriteRenderer',True)])
element_enum=enum_refs('Element','1280ac');ui_enum=enum_refs('ElementUi','1280b4')
row(0x0600047A,'N118_ZombieUis()',2,['08',element_enum,ui_enum,'1280ac','1280b4'],[['tryElements','finallyElements','return'],['tryUi','finallyUi','afterUi']])
row(0x06000908,'N118_FlowerCoroutine()',2,['08','128484','0e','0e','08','1284b4'])
bind('Vector2Type',type_token('Vector2',True))
for name,f in [('MeshIndicesCtor',0x04000ADB),('MeshVerticesCtor',0x04000ADD),('MeshUvsCtor',0x04000AE2),('MeshAddColorsCtor',0x04000AE4),('MeshMulColorsCtor',0x04000AE6)]:member(name,field(f),0x3F6)
row(0x06000900,'N118_MeshCacheInit()',4,['11812d'])
member('ZombieInfoCount',field(0x04000095),0x144);member('ZombieInfoItem',field(0x04000095),0xAB)
row(0x06000081,'N118_LoadZombieInfo()',3,[type_blob('TMP_InputField',True),'0e','1280d4'])
member('InitialPlantItem',field(0x04000314),0xAB)
row(0x06000075,'N118_LoadSkillEditor()',3,[type_blob('TMP_InputField',True),'0e','128244'])
card_enum=enum_refs('Card','1281b4');member('StaticEntriesClear',field(0x04000151),0xC0);member('StaticEntriesAdd',field(0x04000151),0xBF)
row(0x060000E5,'N118_BankEntries()',3,[entry_enum,'1282dc',card_enum,'1281b4']+['0c']*9+['08','08'],[['tryEntries','finallyEntries','afterEntries'],['tryCards','finallyCards','return']])
precard_enum=enum_refs('PreCard','1281ac')
member('PreCardCtor','15120d011281ac',0xAA);member('PreCardAdd','15120d011281ac',0xBF);member('CardCount',field(0x040000E9),0x144);member('CardAdd',field(0x040000E9),0xBF)
row(0x060000ED,'N118_SetPreCards()',3,['15120d011282dc',entry_enum,'1282dc','15120d011281ac',precard_enum,'1281ac','1281b4','08'],[['tryFilter','finallyFilter','afterFilter'],['tryPre','finallyPre','afterPre'],['tryCreate','finallyCreate','return']])
member('RangeRadiusCount',field(0x040000E4),0x144);member('RangeCenterItem',field(0x040000E5),0xAB)
member('RangeCentersAdd',field(0x040000E5),0xBF);member('RangeDistanceAdd',field(0x040000E4),0xBF)
row(0x060000DE,'N118_RestrainCircle()',5,['114d','15120d010c','15120d01114d','08','114d']+['0c']*4+['08','0c','114d'])
gameobj_enum=enum_refs('GameObject','1209')
row(0x060003B5,'N118_Transplant()',3,['114d','114d','114d','1215','114d',gameobj_enum,'1209'],[['tryCharacteristic','finallyCharacteristic','afterCharacteristic'],['tryParticles','finallyParticles','afterParticles']])
row(0x0600032E,'N118_PlaceDevice()',3,['128190','114d','1281d4','114d','1215','114d','0c','08','08'])
member('SkillTextItem',field(0x040008C5),0xAB)
row(0x060006C6,'N118_PlantSkillDetail()',4,[type_blob('TMP_Text',True),'0e','0e','1d1c','0e'])
for name,t in [('SuppliesItemCount',0x144),('SuppliesItemItem',0xAB),('SuppliesItemClear',0xC0),('SuppliesItemAdd',0xBF)]:member(name,field(0x04000717),t)
supplies_enum=enum_refs('Supplies','128234')
for tok,emit in [(0x0600057B,'N118_UpgradeSupplies(false)'),(0x0600057C,'N118_UpgradeSupplies(true)')]:
    row(tok,emit,3,['08','08','08','128204','08','08',supplies_enum,'128234'],[['trySupplies','finallySupplies','afterSupplies']])
row(0x06000819,'N118_WarpCtor()',4,['1d118249'])
external_from('AnimatorFullPathHash',79,0x8,'get_fullPathHash')
row(0x06000334,'N118_DevicePreviewState()',4,[gameobj_enum,'1209','1180d1','1215','11813d',type_blob('Animator',True)], [['trySprites','finallySprites','afterSprites']])
save_enum=enum_refs('Save','128218')
row(0x060006F8,'N118_CreatePlayerSave()',3,['12814c',save_enum,'128218'],[['tryNames','finallyNames','afterNames']])
member('PlantDataItem',field(0x04000315),0xAB)
row(0x06000073,'N118_PlantEditorLoad()',3,[type_blob('TMP_InputField',True),'0e','128154',type_blob('TMP_Text',True)])
member('PlantDataUiItem',field(0x040008C4),0xAB);member('PlantInfoCount',field(0x040008C1),0x144)
row(0x060006C3,'N118_PlantDetailLoad()',3,['128228','128244','08','08',type_blob('Image',True),type_blob('TextMeshProUGUI',True),'0e'])
literal('LadderHead',0x181BD4F70)
string_enum=enum_refs('String','0e');member('StringListCtor','15120d010e',0xAA);member('StringListAdd','15120d010e',0xBF)
row(0x06000439,'N118_DropArmor1()',3,['08','15120d010e',string_enum,'0e','1209','114d'],[['tryNames','finallyNames','afterNames']])
row(0x0600043A,'N118_DropArmor2()',4,['0e','1209','114d','08'])
row(0x06000756,'N118_Benchmark3()',8,[type_blob('TMP_FontAsset',True),'08','08','1209',type_blob('TextMeshPro',True),type_blob('TextMesh',True)])
row(0x06000798,'N118_FloatingStart()',5,['114d','11812d','1215',type_blob('TextMesh',True),type_blob('TextMeshPro',True),'08','08','08',type_blob('<DisplayTextMeshProFloatingText>d__15')])
game_spec('InitialFullZombie',0x060003EF,'128210');game_spec('InitialFullPlant',0x060003EF,'1281e0');game_spec('InitialShortPlant',0x060003ED,'1281e0')
row(0x060004AF,'N118_SphShot()',11,['1281f4','114d','128094','08',type_blob('AudioClip',True),'114d','0c'])
row(0x06000391,'N118_SummonSaber()',11,['128188','114d','114d','08','08','0c','0c','1281f4','128094','08','0c','0d','0c','0c',type_blob('BoardConfig')])
row(0x060003AD,'N118_Repearbalester()',7,['114d','114d','1281f4','1281f4','128094','128094','128210','1281d4','114d','0c','0c'])
game_spec('BindVfxPlant',0x060000C4,'1281e0');member('ChildBuffAdd',field(0x0400010D),0xBF)
row(0x06000506,'N118_SkillAwake10()',5,['0c',type_blob('StatsIncreased'),field(0x0400010D),type_blob('StatsIncreased'),type_blob('BoardConfig'),'1209','1215','114d','0c'])
zombie_enum=enum_refs('TargetZombie','128210');device_enum=enum_refs('TargetDevice','1281d4')
member('TargetZombieCount','15120d01128210',0x144);member('TargetDeviceCount','15120d011281d4',0x144)
row(0x06000518,'N118_Skill149()',4,['15120d01128210',zombie_enum,'128210','128094','1209','114d','02','15120d011281d4',device_enum,'1281d4',type_blob('AudioClip',True),'0c'],[['tryZombies','finallyZombies','zombiesDone'],['tryDevices','finallyDevices','checkAudio']])
member('DeviceDetailPrefabItem',field(0x040006E0),0xAB)
row(0x0600055B,'N118_DeviceDetailLoad()',3,[type_blob('TMP_Text',True),'0e',type_blob('Image',True),type_blob('Sprite',True)])
for suffix,t in [('Count',0x144),('Item',0xAB),('Clear',0xC0),('Add',0xBF)]:member('ConfigSelect'+suffix,field(0x040000B7),t)
member('ConfigZombieCount','15120d01128210',0x144);member('ConfigZombieItem','15120d01128210',0xAB)
member('ConfigTypeCount',field(0x04000196),0x144)
for suffix,t in [('Count',0x144),('Item',0xAB),('Add',0xBF),('RemoveAt',0x72)]:member('ConfigInput'+suffix,field(0x040000B9),t)
for suffix,t in [('Count',0x144),('Item',0xAB)]:member('ConfigRow'+suffix,field(0x0400037D),t)
literal('MissingSelectPrefab',0x181BA1450)
row(0x0600008A,'N118_CreateEnemyConfig()',3,['128358','08','08','02','128358','15120d01128210',field(0x040000B9),type_blob('TMP_InputField',True),'0e'])
# MORE_ROWS
path_enum=enum_refs('Path','12819c')
for suffix,t in [('Count',0x144),('Item',0xAB),('Remove',0x36),('Clear',0xC0)]:member('Path'+suffix,field(0x040005F9),t)
row(0x0600046B,'N118_PathTest()',3,[field(0x040005F9),path_enum,'12819c',type_blob('Board'),'08','08','128190','1281d4','128190'],[['tryPaths','finallyPaths','pathsDone']])
plant_enum=enum_refs('TargetPlant','1281e0')
row(0x06000490,'N118_Boxing()',4,[type_blob('PlantManager'),type_blob('DeviceManager'),'128094',plant_enum,'1281e0',device_enum,'1281d4','114d','1209','08','08',type_blob('BoardConfig'),type_blob('Board')],[['tryPlants','finallyPlants','plantsDone'],['tryDevices','finallyDevices','devicesDone']])
(N/'SOURCE-DECLARATIONS.json').write_text(json.dumps(dict(rows=rows,bindings=bindings,member_definitions=defs,added_typespecs=newtypes,type_blobs=types[173:],added_methodspecs=methodspecs,user_strings=userstrings,literal_tables=tables),ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print(json.dumps(dict(scope='SOURCE_DECLARATIONS_ONLY',method_sources=len(rows),new_member_refs=len(defs),new_typespecs=len(newtypes),new_user_strings=len(userstrings),candidate=None)))
