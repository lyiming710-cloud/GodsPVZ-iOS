"""Source declarations for Native117, composed as text only; never execute game methods."""
from pathlib import Path
import json,sys
sys.dont_write_bytecode=True
R=Path(__file__).resolve().parents[1];N=R/'audit/native117-codex-2026-10-22';O=R/'audit/native-bulk250-r1-codex-2026-10-22'
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile
md=dnfile.dnPE(str(R/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).net.mdtables
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
c=read(N/'CURRENT-SOURCE.json');old=read(O/'METADATA-ADDITIONS.json');plan=read(O/'IMPLEMENTATION-PLAN.json')
types=[x.Signature.value.hex() for x in md.TypeSpec.rows]+old['type_blobs']
# Historical type_blobs is append-only payload, not the complete original table.
if len(types)!=plan['typespec_count_after']:raise ValueError(('type catalog construction',len(types),plan['typespec_count_after']))
newtypes=[];defs=[];bindings=[];locals=[];rows=[]
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
def bind(name,token):bindings.append({'name':'N117_'+name,'token':token});return token
def member(name,blob,template):
    owner=spec(blob);rr=md.MemberRef.rows[template-1];method=str(rr.Name);sig=rr.Signature.value.hex();cc=owner*8+4
    tok=next((0x0A000000+i for i,r in enumerate(md.MemberRef.rows,1) if r.Class.table.name=='TypeSpec' and r.Class.row_index==owner and str(r.Name)==method and r.Signature.value.hex()==sig),None)
    if tok is None:
        found=next((x for x in old['member_definitions']+defs if x['class_code']==cc and (x.get('new_name') or x['template_name'])==method and x['signature']==sig),None)
        if found:tok=found['token']
    if tok is None:
        tok=0x0A000000+plan['memberref_count_after']+1+len(defs)
        defs.append(dict(name='N117_'+name,token=tok,class_code=cc,owner_pin=blob,template=template,template_name=method,signature=sig,new_name=None))
    return bind(name,tok)
def renamed(name,template,newname):
    rr=md.MemberRef.rows[template-1];owner=rr.Class.row;pin=(str(owner.TypeNamespace)+'.' if str(owner.TypeNamespace) else '')+str(owner.TypeName)
    tok=0x0A000000+plan['memberref_count_after']+1+len(defs)
    defs.append(dict(name='N117_'+name,token=tok,class_code=rr.Class.row_index*8+1,owner_pin=pin,template=template,template_name=str(rr.Name),signature=rr.Signature.value.hex(),new_name=newname))
    return bind(name,tok)
def enum_refs(prefix,element):
    l='15120d01'+element;e='15115101'+element
    for suffix,owner,t in [('Enum',l,0xB7),('Current',e,0xB8),('Move',e,0xB9),('Dispose',e,0x45)]:member(prefix+suffix,owner,t)
    return e
renamed('MaterialSetFloat',0x32D,'SetFloat');renamed('WordLast',0x373,'lastCharacterIndex')
bind('TmpWordType',type_token('TMP_WordInfo',True));bind('MatrixType',type_token('Matrix4x4',True))
member('GridCount','15120d01128190',0x144)
for name,t in [('ZombieItem',0x99),('ZombieAdd',0xBF),('ZombieRemove',0x36)]:member(name,'15120d01128210',t)
member('FlagCount',field(0x0400020B),0x144)
event=field(0x04000482)[8:];event_enum=enum_refs('Event',event)
for name,t in [('EventAdd',0xBF),('EventClear',0xC0),('EventRemove',0x36)]:member(name,field(0x04000482),t)
game_enum=enum_refs('Game','1209');member('GameIndex','15120d011209',0xAD)
member('BuffAdd',field(0x0400010D),0xBF)
def row(tok,emit,maxs,items=(),eh=()):
    blob=(b'\x07'+comp(len(items))+b''.join(bytes.fromhex(x) for x in items)).hex()
    rows.append({'token':f'0x{tok:08X}','emission_method':emit,'max_stack':maxs,'local_blob':blob,'eh_clauses':eh})
row(0x06000768,'N117_AnimateProperties()',3,['08',field(0x04000996),'0c','0c'])
row(0x0600078D,'N117_RevealCharacters()',3,['08',field(0x040009B1)])
row(0x06000793,'N117_RevealWords()',4,['08','08'])
row(0x0600077F,'N117_TeleType()',3,['08',field(0x040009A9),'08'])
row(0x06000720,'N117_EnvMap()',5,['08',field(0x04000924),'114d']+['0c']*6+[type_blob('Quaternion',True,True)])
row(0x0600028E,'N117_RandomGrids()',3,['15120d01128190','15120d01128190','08',md.TypeSpec.rows[84].Signature.value.hex(),md.TypeSpec.rows[85].Signature.value.hex(),'128190'],[['try','finally','return']])
row(0x0600027A,'N117_CreateZombie()',4,['128210','114d'])
row(0x060001B7,'N117_ExtendFlags()',4,[type_blob('Flag'),type_blob('Wave'),'08'])
row(0x0600030C,'N117_DeviceEvents()',2,[event_enum,type_blob('DeviceEvent')],[['tryAdd','finallyAdd','afterAdd'],['tryUpdate','finallyUpdate','afterUpdate'],['tryRemove','finallyRemove','afterRemove']])
row(0x06000475,'N117_ZombieBrightness()',3,[game_enum,'1209'],[['tryBody','finallyBody','armorCheck'],['tryArmor','finallyArmor','done']])
for tok,n in [(0x0600048E,1),(0x06000493,2)]:row(tok,f'N117_IvhEnhance{n}()',4,['15115101128210','128210',type_blob('StatsIncreased'),'1209'],[['try','finally','after']])
row(0x06000354,'N117_PlantFight()',4,['08','128210','0c','0c','1281a4','128188','0c',type_blob('Element')])
projectile_enum=enum_refs('Projectile','1281f4')
for name,t in [('RowCount',0x144),('RowAdd',0xBF)]:member(name,field(0x0400037D),t)
preview_enum=enum_refs('EnemyPreview',field(0x04000785)[8:]);bind('PreviewWave',0x040005BA)
member('StringCount',field(0x040006AE),0x144);member('ChallengeCount',field(0x0400011A),0x144)
row(0x060001D9,'N117_MouseSkill()',3,['08','128210','128094','1281d4','128244','1281e0','1260','114d'])
row(0x06000392,'N117_TrySwordFormation()',4,['15120d011281f4',projectile_enum,'1281f4','08','0c','08'],[['try','finally','after']])
row(0x06000291,'N117_TurnScale()',4,[field(0x0400037D),'1511510112818c','12818c','15120d01128190'],[['try','finally','after']])
row(0x060005DC,'N117_ViewWave()',4,['02',preview_enum,field(0x04000785)[8:],'1271','02',type_blob('BoardConfig'),'128085','08','08','0e'],[['try','finally','after']])
row(0x06000597,'N117_Talents()',5,['08','08','08'])
row(0x06000574,'N117_SkillUp()',6,['15115101128204','128204',type_blob('Almanac_SkillSystem'),'128244','128228'],[['try','finally','after']])
row(0x06000634,'N117_HardLocked()',4,['08','0e','08'])
row(0x060005FF,'N117_ChallengeAwake()',5,['08','08','1209','08','08','1209','08',type_blob('RectTransform',True),type_blob('Vector2',True,True),'128085','08','0e'])
save_enum=enum_refs('Save','128218');image_enum=enum_refs('Image','1275');member('ImageCount',field(0x040008F3),0x144)
member('SequenceCount',field(0x04000A81),0x144);member('FrameCount',field(0x04000A8E),0x144)
member('IntClear','15120d0108',0xC0);member('EntryListCtor',field(0x040001A0),0x110);member('EntryAdd',field(0x040001A0),0xBF);member('PreDeviceAdd',field(0x040001D7),0xBF)
row(0x060006F9,'N117_RenamePlayer()',3,[type_blob('SavesManager'),'128218',save_enum,'128218'],[['try','finally','after']])
row(0x060006F1,'N117_SuppliesInfo()',5,['128085','08','08','1275',image_enum],[['try','finally','after']])
row(0x06000843,'N117_ChangeSequence()',4,['08','08',field(0x04000A72),'1d1c'])
row(0x060004C8,'N117_CloneTeam()',2,['1511510108','08'],[['tryids','finallyids','afterids'],['tryskills','finallyskills','afterskills'],['tryprops','finallyprops','afterprops']])
row(0x0600018B,'N117_SetupPreDevices()',3,[field(0x040001A0),'151151011282dc','1282dc','151151011280f8','1280f8'],[['tryEntries','finallyEntries','afterEntries'],['tryPre','finallyPre','afterPre']])
loot_enum=enum_refs('Loot',type_blob('Loot'))
member('CardCount',field(0x040006C8),0x144);member('CardAdd',field(0x040006C8),0xBF);member('SpriteCount',field(0x040006E2),0x144)
row(0x060005D1,'N117_DevicePreviews()',3,[field(0x040001A0),'151151011282dc','1282dc','151151011280f8','1280f8'],[['tryEntries','finallyEntries','afterEntries'],['tryPre','finallyPre','afterPre']])
row(0x060006D0,'N117_CreateLootList()',3,['15120d01128234',loot_enum,type_blob('Loot'),type_blob('LawnApp'),'128234','151151011282dc','1282dc'],[['tryLoot','finallyLoot','afterLoot'],['tryEntry','finallyEntry','afterEntry']])
row(0x06000547,'N117_LoadAlmanacPlants()',4,[field(0x040002FC),'15115101128228','128228','1281c0','08','0c','0c'],[['try','finally','done']])
for tok,zombie in [(0x0600055D,False),(0x060005A9,True)]:
    row(tok,'N117_AlmanacHome('+str(zombie).lower()+')',4,[field(0x04000308 if zombie else 0x04000309),'128218',type_blob('IEnumerator',True),type_blob('IDisposable',True),'08',field(0x04000308 if zombie else 0x04000309)],[['try','finally','after']])
row(0x06000585,'N117_TestSupplied()',4,['02','15115101128204','128204',type_blob('RectTransform',True),'0c','128085'],[['try','finally','after']])
# Freeze the actual native float32 bit patterns, including its non-primary red/green colors.
import struct,re,pefile
pe=pefile.PE(str(R/'audit/offline-test-handoff-2026-10-14/payload/inputs/original-pc/GameAssembly.dll'))
orig=read(N/'ORIGINAL-SOURCE.json');color=[]
for address,name in [(0x18038f1a8,'SufficientColor'),(0x18038f1b0,'InsufficientColor')]:
    i=next(i for m in orig['methods'] if m['token']=='0x06000585' for i in m['instructions'] if int(i['address'],16)==address)
    va=address+len(bytes.fromhex(i['bytes']))+int(re.search(r'\+ (0x[\da-f]+)',i['operands']).group(1),16)
    raw=pe.get_data(va-pe.OPTIONAL_HEADER.ImageBase,16);bits=list(struct.unpack('<4I',raw))
    color.append({'name':'N117_'+name,'native_va':f'0x{va:X}','bytes':raw.hex(),'float32_bits':bits})
(N/'COLOR-SOURCE.json').write_text(json.dumps(color,indent=2)+'\n',encoding='utf-8')
# Additional bindings are resolved by declared owner and name, never guessed from numeric proximity.
def field_token(owner,name):return int(next(x['token'] for x in c['fields'] if x['owner']==owner and x['name']==name),16)
bind('DeviceList',field_token('DeviceManager','deviceList'))
for name,offset in [('ZombieId',0x60),('ZombieWidth',0x40),('ZombieDepth',0x44)]:bind(name,int(next(f['token'] for f in orig['fields'] if f['type']=='Zombie' and f['offset']==offset),16))
member('PathInsert',field(0x040005F9),0x49);member('PathRemoveAt',field(0x040005F9),0x72)
device_editor_enum=enum_refs('DeviceEditor','121c');enemy_editor_enum=enum_refs('EnemyEditor','1224')
member('DeviceEditorClear',field(0x0400001A),0xC0);member('EnemyEditorClear',field(0x0400001B),0xC0)
for name,t in [('GridConfigCount',0x144),('GridConfigRemove',0x36),('GridConfigAdd',0xBF)]:member(name,field(0x040000AC),t)
plant_enum=enum_refs('Plant','1281e0');member('SpriteSetItem',field(0x040002E6),0xA8)
row(0x060001D6,'N117_MousePlant()',3,[type_blob('Plant_DetaiPage'),'1281e0','1281e0','114d','114d','0c','0c','0c','0c',type_blob('Component',True),field(0x04000871)])
row(0x060004AC,'N117_SnowbeastBait()',4,['1281d4','08','151151011281d4','1281d4','08','08','12819c','08','1d0e'],[['try','finally','after']])
row(0x06000074,'N117_LoadOrderData()',3,[])
row(0x06000088,'N117_EditorStart()',4,['08','08'])
row(0x06000012,'N117_InitializeBoardEditor()',3,[device_editor_enum,enemy_editor_enum,'128188','0e'],[['tryDevices','finallyDevices','afterDevices'],['tryEnemies','finallyEnemies','afterEnemies']])
row(0x0600008B,'N117_CreateGridConfig()',3,['128188','08','08','08',type_blob('GridConfig_A'),'08','08','128190'])
row(0x06000758,'N117_Benchmark4()',5,['0c','0c','0c','08','1209','128081','114d','0c','0c',type_blob('Vector2',True,True),'1215'])
for tok,stove in [(0x0600031F,False),(0x06000321,True)]:
    row(tok,'N117_Warm('+str(stove).lower()+')',3,['0c',plant_enum,'1281e0','15120d011281d4','0c','0c','0c','0c','151151011281d4','1281d4'],[['tryPlant','finallyPlant','afterPlant'],['tryChoose','finallyChoose','afterChoose'],['tryApply','finallyApply','done']])
row(0x06000387,'N117_Potatomortar()',3,['0c','128210','15115101128210','128210','0c','0c','0c','0c'],[['tryAll','finallyAll','afterAll'],['tryBlocked','finallyBlocked','afterBlocked']])
for tok,card in [(0x06000232,False),(0x06000233,True)]:row(tok,'N117_ZombiePortraits('+str(card).lower()+')',4,['0e',type_blob('IEnumerator',True),type_blob('IDisposable',True),'08','0e','125d'],[['try','finally','return']])
member('PlantPreviewAdd',field(0x040002F6),0xBF);member('SuppliesItemCount',field(0x04000717),0x144);member('SuppliesItemClear',field(0x04000717),0xC0);member('NodeCount','15120d01128198',0x144)
row(0x0600022A,'N117_LoadPlantPreviews()',3,['0e',type_blob('IEnumerator',True),type_blob('IDisposable',True),'08','0e','1281e4'],[['try','finally','return']])
row(0x0600057E,'N117_LoadDisplayPlant()',4,['1281e0',type_blob('SortingGroup',True),field(0x040004AF),game_enum,'1209'],[['try','finally','after']])
row(0x0600057A,'N117_LevelUpSupplies()',4,['08','08','08','08','08','08','08','128234',field(0x04000717),'08','0c','128228'])
row(0x060002A1,'N117_FindNeighbors()',3,['15120d01128198','08','08','08','128198'])
row(0x06000360,'N117_SeekZombie()',3,[field(0x040004AF),'08','1260','128210','0c','15115101128210','128210'],[['tryPea','finallyPea','afterPea'],['tryOther','finallyOther','afterOther']])
row(0x06000363,'N117_SeekDevice()',4,[field(0x040004AF),'08','1260','1281d4','0c','151151011281d4','1281d4','08','128094'],[['tryPea','finallyPea','afterPea'],['tryOther','finallyOther','afterOther']])
# MORE_ROWS
(N/'SOURCE-DECLARATIONS.json').write_text(json.dumps({'rows':rows,'bindings':bindings,'member_definitions':defs,'added_typespecs':newtypes,'type_blobs':types[173:],'added_methodspecs':[],'color_constants':color},indent=2)+'\n',encoding='utf-8')
print(json.dumps({'scope':'SOURCE_DECLARATIONS_ONLY','method_sources':len(rows),'new_member_refs':len(defs),'new_typespecs':len(newtypes),'candidate':None}))
