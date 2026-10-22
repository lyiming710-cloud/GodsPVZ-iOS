"""Source declarations for Native119, composed as text only; never execute game methods."""
from pathlib import Path
import json,sys
sys.dont_write_bytecode=True
R=Path(__file__).resolve().parents[1];N=R/'audit/native119-codex-2026-10-22';O=R/'audit/native-bulk350-codex-2026-10-22'
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
def bind(name,token):bindings.append({'name':'N119_'+name,'token':token});return token
def member(name,blob,template):
    owner=spec(blob);rr=md.MemberRef.rows[template-1];method=str(rr.Name);sig=rr.Signature.value.hex();cc=owner*8+4
    tok=next((0x0A000000+i for i,r in enumerate(md.MemberRef.rows,1) if r.Class.table.name=='TypeSpec' and r.Class.row_index==owner and str(r.Name)==method and r.Signature.value.hex()==sig),None)
    if tok is None:
        found=next((x for x in old['member_definitions']+defs if x['class_code']==cc and (x.get('new_name') or x['template_name'])==method and x['signature']==sig),None)
        if found:tok=found['token']
    if tok is None:
        tok=0x0A000000+plan['memberref_count_after']+1+len(defs)
        defs.append(dict(name='N119_'+name,token=tok,class_code=cc,owner_pin=blob,template=template,template_name=method,signature=sig,new_name=None))
    return bind(name,tok)
def renamed(name,template,newname):
    rr=md.MemberRef.rows[template-1];owner=rr.Class.row;pin=(str(owner.TypeNamespace)+'.' if str(owner.TypeNamespace) else '')+str(owner.TypeName)
    tok=0x0A000000+plan['memberref_count_after']+1+len(defs)
    defs.append(dict(name='N119_'+name,token=tok,class_code=rr.Class.row_index*8+1,owner_pin=pin,template=template,template_name=str(rr.Name),signature=rr.Signature.value.hex(),new_name=newname))
    return bind(name,tok)

def external_from(name,owner,template,newname):
    rr=md.MemberRef.rows[template-1];ty=md.TypeRef.rows[owner-1]
    pin=str(ty.TypeNamespace)+'.'+str(ty.TypeName);sig=rr.Signature.value.hex();cc=owner*8+1
    found=next((x for x in old['member_definitions']+defs if x['class_code']==cc and (x.get('new_name') or x['template_name'])==newname and x['signature']==sig),None)
    if found:return bind(name,found['token'])
    tok=0x0A000000+plan['memberref_count_after']+1+len(defs)
    defs.append(dict(name='N119_'+name,token=tok,class_code=cc,owner_pin=pin,template=template,template_name=str(rr.Name),signature=sig,new_name=newname))
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
    methodspecs.append(dict(name='N119_'+alias,token=tok,definition=definition,owner=str(owner.TypeName),method_name=str(method.Name),signature=method.Signature.value.hex(),instantiation=inst));return bind(alias,tok)

userstrings=[];user_end=plan['user_string_heap_before']+plan['appended_user_string_payload_bytes']
original=read(N/'ORIGINAL-SOURCE.json');cells={int(x['address'],16):x for x in original['cells']}
def literal(alias,cell):
    global user_end
    x=cells[cell];value=bytes.fromhex(x['utf8_hex']).decode('utf-8')
    prior=next((z for z in old['user_strings']+userstrings if z['value']==value),None)
    if prior:return bind(alias,prior['token'])
    stream=md # managed user heap is read independently below
    tok=None
    for t in c['methods']:
        for i in t['instructions']:
            o=i['operand']
            if i['opcode']=='ldstr' and isinstance(o,dict) and o.get('identity')==value:
                tok=int(o['token'],16);break
        if tok:break
    if tok:return bind(alias,tok)
    data=value.encode('utf-16-le');flag=int(any(ord(ch)>255 or 1<=ord(ch)<=8 or 14<=ord(ch)<=31 or ord(ch) in (0x27,0x2D,0x7F) for ch in value));encoded=comp(len(data)+1)+data+bytes([flag]);tok=0x70000000+user_end
    userstrings.append(dict(name='N119_'+alias,token=tok,value=value,encoded_bytes=encoded.hex(),native_cell=x['address'],native_utf8_hex=x['utf8_hex']));user_end+=len(encoded);return bind(alias,tok)
# SOURCE_ROWS
node_enum=enum_refs('Node','128198')
for suffix,t in [('Ctor',0xAA),('Count',0x144),('Item',0xAB),('Add',0xBF)]:member('Node'+suffix,'15120d01128198',t)
for suffix,t in [('Ctor',0xAA),('Count',0x144),('Item',0xAB),('Add',0xBF),('Insert',0x49),('RemoveAt',0x72),('Clear',0xC0)]:member('Path'+suffix,'15120d0112819c',t)
row(0x06000282,'N119_AstarFinding()',7,[node_enum,'128198','15120d01128198','08','08','08','128198','08','128198','12819c','08'],[['tryReset','finallyReset','resetDone']])
for prefix,f in [('Initial',0x04000314),('Data',0x04000315),('Level',0x04000316),('Order',0x04000317),('Skill',0x04000318)]:
    for suffix,t in [('Count',0x144),('Add',0xBF)]:member(prefix+suffix,field(f),t)
    if prefix in ['Level','Order']:member(prefix+'Ctor',field(f),0xAA)
row(0x06000251,'N119_UpdatePlantInitial()',3,[field(t) for t in [0x04000314,0x04000315,0x04000316,0x04000317,0x04000318]]+['08','128158'])
# END_SOURCE_ROWS
broken_obj_enum=enum_refs('BrokenObject','1209')
for alias,cell in [('Open',0x181BC46E8),('Extinguish',0x181BC3100),('Broken',0x181BA93B0)]:literal(alias,cell)
row(0x0600030F,'N119_BrokenDevice()',5,[type_blob('AudioClip',True),'0c','0c','128190','08','08','1209','114d','1215',broken_obj_enum,'1209','128094','08'],[['trySprites','finallySprites','spritesDone']])
for alias,cell in [('AnimCone',0x181BA5E68),('AnimBucket',0x181BA5E08),('AnimBrick',0x181BA5DA8),('IceCube1',0x181BCBEC8),('IceCube2',0x181BCBF18),('LadderHelmet',0x181BD4FC8),('LadderMask',0x181BD5230)]:literal(alias,cell)
row(0x06000460,'N119_InjuryArmor1()',3,[type_blob('SpriteRenderer',True),type_blob('SpriteRenderer',True),'1209','1209','08'])
member('CollectProjectAdd',field(0x040002C2),0xBF)
row(0x060003C3,'N119_CollectProject()',5,[type_blob('Project'),'1215','114d','08','0d','0c',type_blob('ProjectManager'),'114d','0c','0c'])
save_enums=[]
for prefix,f in [('SavePlant',0x04000639),('SaveProp',0x0400063A),('SaveSupply',0x0400063D)]:
    save_enums.append(enum_refs(prefix,field(f)[8:]));member(prefix+'Add',field(f),0xBF)
member('SavePlantClear',field(0x04000639),0xC0)
member('SaveTeamItem',field(0x04000642),0xAB);member('SaveTeamCount',field(0x04000642),0x144)
row(0x060004C0,'N119_CopySave()',4,['08',save_enums[0],'128228',field(0x04000639),save_enums[1],'12822c','12822c',field(0x0400063A),save_enums[2],'128234','128234',field(0x0400063D),'11821c','11821c','1d08','1d02'],[['tryPlants','finallyPlants','plantsDone'],['tryProps','finallyProps','propsDone'],['trySupplies','finallySupplies','suppliesDone']])
board_copy_locals=[type_blob('BoardConfig'),'08','08']
for prefix,f in [('CopyTypes',0x04000196),('CopyLoot',0x0400019C),('CopyEnemies',0x0400019D),('CopyDevices',0x0400019E),('CopyCards',0x0400019F),('CopyEntries',0x040001A0)]:
    element=field(f)[8:];board_copy_locals += [enum_refs(prefix,element),element];member(prefix+'Add',field(f),0xBF)
row(0x06000156,'N119_CopyBoardConfig()',3,board_copy_locals,[['try'+str(i),'finally'+str(i),'after'+str(i)] for i in range(6)])
enemy_path_enum=enum_refs('EnemyPath','12819c')
row(0x060004AA,'N119_SnowImpact()',4,['128190','128094',enemy_path_enum,'12819c',type_blob('AudioClip',True),'114d','0c','1281d4','1281e0','08'],[['tryPath','finallyPath','pathDone']])
plant_enum=enum_refs('Plant','1281e0');device_enum=enum_refs('Device','1281d4');zombie_enum=enum_refs('Zombie','128210')
row(0x060004B2,'N119_SphSeek()',4,['1281e0','1281d4','128210',plant_enum,'1281e0',device_enum,'1281d4',zombie_enum,'128210','128094','0c','0c'],[['tryPlants','finallyPlants','plantsDone'],['tryDevices','finallyDevices','devicesDone'],['tryZombies','finallyZombies','zombiesDone']])
row(0x06000284,'N119_AstarText()',4,['12819c']*3+['08']*8+['0c']+['08']*6+['0c','0c']+['08']*6+['0c','08'])
for suffix,t in [('Count',0x144),('Item',0xAB)]:member('Row'+suffix,field(0x0400037D),t)
member('GridItem',field(0x04000381),0xAB);literal('MissingMapEntry',0x181BA3308)
row(0x0600028C,'N119_PathFinding()',9,[type_blob('Board'),'08','08','08','08','15120d01128198','12818c','08','08','128190','128198','15120d0112819c','128198','12819c','128198','08','08','08'])

member('PlantStarsCount',field(0x0400070A),0x144)
# Constructor signatures are taken from an existing Color constructor MemberRef.
color_ctor=next(i for i,r in enumerate(md.MemberRef.rows,1) if str(r.Name)=='.ctor' and r.Class.table.name=='TypeRef' and str(r.Class.row.TypeName)=='Color' and r.Signature.value.hex()=='2004010c0c0c0c')
bind('ColorCtor',0x0A000000+color_ctor)
for alias,cell in [('Deify',0x181BA57C8),('DeifyPreview',0x181BA58E8),('Max',0x181B9BE20),('DeifyMax',0x181BA5828)]:literal(alias,cell)
row(0x0600057F,'N119_LoadPlantData()',5,['08',field(0x0400070A),'02',type_blob('Button',True),'1209','08','08'])


preview_enum=enum_refs('Preview',field(0x040006F8)[8:])
member('PreviewSupplyCount',field(0x04000717),0x144);member('PreviewSupplyClear',field(0x04000717),0xC0)
for alias,cell in [('Insufficient',0x181BA4A48),('Confirm',0x181BA5768),('LowLevel',0x181BA5C48)]:literal(alias,cell)
row(0x06000582,'N119_LevelPreview()',5,[preview_enum,'08','08','08','02'],[['tryClose','finallyClose','closed']])
row(0x06000583,'N119_OrderPreview()',5,[preview_enum,'08','08','08','02'],[['tryClose','finallyClose','closed']])


for alias,cell in [('Unknown',0x181BA8EF8),('HpUnknown',0x181BA4988),('AttackUnknown',0x181BA3AE8),('DefenseUnknown',0x181BA72C8),('Armor1Unknown',0x181BA0370),('Armor2Unknown',0x181BA09D0),('SpeedUnknown',0x181BA5AC8),('Dash',0x181B9F888),('HpDash',0x181BA4928),('AttackDash',0x181BA3A28),('DefenseDash',0x181BA7208),('Armor1Dash',0x181BA0310),('Armor2Dash',0x181BA0970),('SpeedDash',0x181BA5A68),('HpPrefix',0x181BA48C8),('AttackPrefix',0x181BA39C8),('DefensePrefix',0x181BA7148),('SpeedPrefix',0x181BA5A08),('PainterPrefix',0x181BA5348),('Elite',0x181BA5CA8),('Boss',0x181BA74A8),('Speed0',0x181BA30C8),('Speed1',0x181BA31E8),('Speed2',0x181BA6A88),('Speed3',0x181BA0730),('Speed4',0x181BA6A28),('Speed5',0x181BA3188),('Speed6',0x181BA3068)]:literal(alias,cell)
row(0x0600059F,'N119_ZombieAlmanac()',5,['0e','1d0e','08'])


projectile_enum=enum_refs('Projectile',type_blob('Projectile'))
for alias,cell in [('SlowName',0x181BA6300),('MoveSpeed',0x181B9FF10),('Crossbow1',0x181BA9CD8),('Crossbow2',0x181BA9D30)]:literal(alias,cell)
row(0x06000507,'N119_Update10()',4,[type_blob('Board'),'15120d01'+type_blob('Projectile'),type_blob('AttackRange'),projectile_enum,type_blob('Projectile'),'0c','08','0c',type_blob('AudioClip',True),'114d',zombie_enum,type_blob('Zombie'),type_blob('StatsIncreased')],[['tryFilter','finallyFilter','filtered'],['tryDestroy','finallyDestroy','destroyed'],['trySlow','finallySlow','slowed']])
game_spec('CrossbowInitial',0x060003ED,type_blob('Plant'))
for suffix,t in [('Item',0xAB),('Add',0xBF),('Count',0x144)]:member('Vector'+suffix,'15120d01114d',t)
member('ZombieListCount','15120d01'+type_blob('Zombie'),0x144);member('DeviceListCount','15120d01'+type_blob('Device'),0x144)
row(0x06000513,'N119_Function22()',9,[type_blob('Board'),'08','114d',type_blob('Projectile'),type_blob('Projectile'),type_blob('Damage'),type_blob('Damage'),'15120d01114d','15120d01'+type_blob('Zombie'),'15120d01'+type_blob('Device'),'08','114d','0c','0c','114d','114d','114d'])


row(0x0600051B,'N119_Function150()',9,['15120d01'+type_blob('Zombie'),zombie_enum,type_blob('Zombie'),'15120d01'+type_blob('Device'),type_blob('Damage'),device_enum,type_blob('Device'),'114d','1d0c','08',type_blob('AudioClip',True),'114d','114d',type_blob('Projectile'),'08'],[['tryZ','finallyZ','zDone'],['tryD','finallyD','dDone']])


member('ZombieListCtor','15120d01'+type_blob('Zombie'),0xAA);member('ZombieIndexOf','15120d01'+type_blob('Zombie'),0xAD)
literal('DevourTrigger',next(k for k,v in cells.items() if v.get('text')=='DevourTrigger'))
row(0x060003A9,'N119_ChomperViking()',5,[type_blob('Damage'),'0c','15120d01'+type_blob('Zombie'),'15120d01'+type_blob('Device'),'15120d01'+type_blob('Zombie'),type_blob('AttackRange'),zombie_enum,type_blob('Zombie'),device_enum,type_blob('Device'),type_blob('Damage'),type_blob('AttackRangeUIController'),'08'],[[t,h,a] for t,h,a in [('tryZ','finallyZ','zDone'),('tryD','finallyD','dDone'),('tryHit','finallyHit','hitDone'),('tryDevour','finallyDevour','devourDone'),('tryHitD','finallyHitD','hitDDone')]])


row(0x060002FD,'N119_DeviceReward()',4,[type_blob('PlantSave'),type_blob('Card'),type_blob('CardDeviceData'),type_blob('PreCard')])


for prefix,f in [('KeyCard',0x040000E9),('KeyProp',0x040000F1)]:
 enum_refs(prefix,field(f)[8:]);member(prefix+'Count',field(f),0x144);member(prefix+'Item',field(f),0xAB)
renamed('KeyUp',0xD,'GetKeyUp')
for alias,text in [('KeyPrefix','\u6309\u4e0b\uff1a'),('Equals','=')]:literal(alias,next(k for k,v in cells.items() if v.get('text')==text))
row(0x060002AC,'N119_BoardKeys()',4,['1129','08','08','0e','08',type_blob('Card'),type_blob('Prop'),'02',type_blob('AudioClip',True),'114d','15115101'+type_blob('Card'),'15115101'+type_blob('Prop')],[['tryCards','finallyCards','cardsDone'],['tryProps','finallyProps','propsDone']])


def custom_member(alias,owner,method,sig,template):
 ty=next(i for i,r in enumerate(md.TypeRef.rows,1) if str(r.TypeName)==owner);cc=ty*8+1
 prior=next((x for x in old['member_definitions']+defs if x['class_code']==cc and (x.get('new_name') or x['template_name'])==method and x['signature']==sig),None)
 if prior:return bind(alias,prior['token'])
 original_ref=next((0x0A000000+i for i,r in enumerate(md.MemberRef.rows,1) if r.Class.table.name=='TypeRef' and r.Class.row_index==ty and str(r.Name)==method and r.Signature.value.hex()==sig),None)
 if original_ref:return bind(alias,original_ref)
 tok=0x0A000000+plan['memberref_count_after']+1+len(defs);tr=md.TypeRef.rows[ty-1]
 defs.append(dict(name='N119_'+alias,token=tok,class_code=cc,owner_pin=str(tr.TypeNamespace)+'.'+str(tr.TypeName),template=template,template_name=str(md.MemberRef.rows[template-1].Name),signature=sig,new_name=method));return bind(alias,tok)
custom_member('TmpFont','TMP_Text','get_font','20001281f1',0x2D7)
custom_member('UiText','Text','set_text','2001010e',0x62);bind('TextMeshText',0x0A0002FD)
for alias,text in [('Arial','Fonts/ARIAL'),('Bevel','Fonts & Materials/LiberationSans SDF - BEVEL'),('DropShadow','Fonts & Materials/LiberationSans SDF - Drop Shadow'),('CountLong','The <color=#0050FF>count is: </color>'),('CountShort','The <#0050FF>count is: </color>'),('CountFormat','The <#0050FF>count is: </color>{0}')]:literal(alias,next(k for k,v in cells.items() if v.get('text')==text))
row(0x06000747,'N119_BenchmarkStart(false)',5,[type_blob('Benchmark01'),'08','08',type_blob('TextMeshPro',True),'1279'])
row(0x0600074F,'N119_BenchmarkStart(true)',5,[type_blob('Benchmark01_UGUI'),'08','08',type_blob('TextMeshProUGUI',True),'1279'])


for tok,tmp in [(0x0600079F,False),(0x060007A5,True)]:row(tok,'N119_FloatingText('+str(tmp).lower()+')',6,[type_blob('TextMeshProFloatingText'),'08','0c','08','1215','114d'])


custom_member('CharacterChar','TMP_CharacterInfo','character','0603',0x36B)
additional_typerefs=[dict(token=0x01000000+len(md.TypeRef.rows)+1,namespace='TMPro',name='TMP_TextElementType',resolution_scope_table='AssemblyRef',resolution_scope_rid=2)]
custom_member('CharacterElement','TMP_CharacterInfo','elementType','0611'+comp((len(md.TypeRef.rows)+1)*4+1).hex(),0x36B)
custom_member('LineCount','TMP_LineInfo','characterCount','0608',0x373);custom_member('LineStart','TMP_LineInfo','firstCharacterIndex','0608',0x373)
custom_member('StringChars','String','.ctor','2001011d03',0x2E8)
member('WordInvoke',field(0x0400092D),0x2EA)
row(0x06000735,'N119_TextEvents()',5,['08','08','03',field(0x0400092B),type_blob('TMP_WordInfo',True,True),'0e','08','08',field(0x0400092D),type_blob('TMP_LineInfo',True,True),'1d03','08',type_blob('TMP_LinkInfo',True,True),'0e',field(0x0400092F)])


loot_enum=enum_refs('Loot',type_blob('Loot'));loot_entry_enum=enum_refs('LootEntry',type_blob('BoardEntry'));loot_supply_enum=enum_refs('LootSupply',type_blob('Supplies'));member('LootSupplyAdd','15120d01'+type_blob('Supplies'),0xBF)
row(0x060005C8,'N119_LootList()',5,['15120d01'+type_blob('Supplies')]*3+[loot_enum,type_blob('Loot'),type_blob('Supplies'),loot_entry_enum,type_blob('BoardEntry'),loot_supply_enum,type_blob('SuppliesItem')],[['tryLoot','finallyLoot','lootDone'],['tryEntries','finallyEntries','entriesDone']]+[[f'display{i}Try',f'display{i}Finally',f'display{i}Done'] for i in range(3)])


choose_enum=enum_refs('Choose',type_blob('Card_Choose'));member('ChooseClear',field(0x040008B8),0xC0);member('ChooseCount',field(0x040008B8),0x144);member('ChooseSetItem',field(0x040008B8),0xA8)
row(0x060006B4,'N119_ChooseDetermine()',5,[field(0x040008B8),choose_enum,type_blob('Card_Choose'),'08','08','0c','0c'],[['tryDestroy','finallyDestroy','destroyed'],['tryClone','finallyClone','cloned'],['tryLayout','finallyLayout','laidOut']])


for alias,cell in [('Permanent',0x181BA5048),('PowerHit',0x181BA2F48),('DurationFormat',0x181BA3788),('AmmoFormat',0x181BA2EE8),('InfiniteDuration',0x181BA3728),('ChargeNatural',0x181BA5EE8),('ChargeAttack',0x181BA3968),('ChargeHit',0x181BA1E10),('ChargeNone',0x181BA3F08),('TriggerAuto',0x181BA5E88),('TriggerManual',0x181BA3248),('TriggerImmediate',0x181BA5BE8),('FourDashes',0x181B9F9A8),('ChargeEmpty',0x181BA0FD0),('ChargeFormat',0x181BA1090),('LayerFormat',0x181BA1E70)]:literal(alias,cell)
row(0x06000591,'N119_SkillTexts()',5,['0e',field(0x04000727),'0e','1d1c'])


literal('MaxChargeFormat',next(k for k,v in cells.items() if v.get('text')=='\u5145\u80fd\uff1a{0}'))
row(0x060005E2,'N119_SkillTexts(true)',5,['0e',field(0x040007A9),'0e','1d1c'])


custom_member('GetAxis','Input','GetAxis','00010c0e',0xD)
custom_member('TouchDelta','Touch','get_deltaPosition','2000'+type_blob('Vector2',True,True),0x31E);custom_member('TouchPosition','Touch','get_position','2000'+type_blob('Vector2',True,True),0x31E)
custom_member('Vector2Ctor','Vector2','.ctor','2002010c0c',0x9C);custom_member('Vector2Magnitude','Vector2','get_magnitude','20000c',0x43)
for alias,text in [('MouseWheel','Mouse ScrollWheel'),('MouseY','Mouse Y'),('MouseX','Mouse X'),('CameraTarget','Camera Target')]:literal(alias,next(k for k,v in cells.items() if v.get('text')==text))
row(0x0600075D,'N119_CameraInput()',6,['08','0c','0c','0c',type_blob('Touch',True,True),type_blob('Vector2',True,True),type_blob('RaycastHit',True,True),type_blob('Touch',True,True)]+[type_blob('Vector2',True,True)]*5)


custom_member('PingPong','Mathf','PingPong','00020c0c0c',0x83);custom_member('SmoothStep','Mathf','SmoothStep','00030c0c0c0c',0x83)
row(0x060007E8,'N119_VertexJitter()',9,[type_blob('VertexJitter'),'08','08','08','08',type_blob('VertexAnim',False,True),'1d114d','1d114d','114d','114d',type_blob('Quaternion',True,True),type_blob('Matrix4x4',True,True),'0c','0c','0c','0c','0c'])


for prefix,blob in [('Float','15120d010c'),('Int','15120d0108')]:
 member(prefix+'Clear',blob,0xC0);member(prefix+'Count',blob,0x144)
custom_member('QuaternionCtor','Quaternion','.ctor','2004010c0c0c0c',0xBE)
row(0x06000811,'N119_VertexZoom()',9,[type_blob('VertexZoom'),'08','08','08','08','0c','1d114d','1d114d','114d',type_blob('Matrix4x4',True,True),'1d'+type_blob('Vector2',True,True),'1d'+type_blob('Vector2',True,True),'1d'+type_blob('Color32',True,True),'1d'+type_blob('Color32',True,True),'15120d0108',field(0x04000A57)])


custom_member('LineEnd','TMP_LineInfo','lastCharacterIndex','0608',0x373)
row(0x060007F5,'N119_ShakeA()',8,[type_blob('VertexShakeA'),'08','08','08','08','08','1d114d','1d114d','114d','08','0c',type_blob('Matrix4x4',True,True),type_blob('Quaternion',True,True),'114d','114d','08'])

row(0x06000802,'N119_ShakeB()',8,[type_blob('VertexShakeB'),'08','08','08','08','08','1d114d','1d114d','114d','08','0c',type_blob('Matrix4x4',True,True),type_blob('Quaternion',True,True),'114d','114d','08','114d'])

for name in ['HealthBase','HealthFactor','AttackBase','AttackFactor','DefenseBase','DefenseFactor','PlantName','PlantID','SkillName','SkillID','Order','SunPrice','ArmorPoint','SkillDuration','AttackInterva','ReplantCD']:
 literal('Input'+name,next(k for k,v in cells.items() if v.get('text')==name))
literal('InputInvalid',0x181BA6B48)
row(0x06000072,'N119_InputString()',3,['0e','0c','08'])

member('BoxPlantCtor','15120d01'+type_blob('Plant'),0xAA);member('BoxPlantAdd','15120d01'+type_blob('Plant'),0xBF);member('BoxZombieAdd','15120d01'+type_blob('Zombie'),0xBF)
row(0x06000492,'N119_BoxingStep3()',4,[type_blob('Board'),type_blob('PlantManager'),type_blob('DeviceManager'),type_blob('ZombieManager'),type_blob('Damage'),'15120d01'+type_blob('Plant'),'15120d01'+type_blob('Zombie'),'15120d01'+type_blob('Device'),plant_enum,type_blob('Plant'),device_enum,type_blob('Device'),zombie_enum,type_blob('Zombie'),type_blob('Damage'),'08','0c',type_blob('AttackRange'),'114d'],[[n+'Try',n+'Finally',n+'Done'] for n in ['p','d','z','pHit','zHit','dHit']])

for i,r in enumerate(rows,1):bind('Locals'+str(i),0x11000000+plan['standalone_count_after']+i)
(N/'SOURCE-DECLARATIONS.json').write_text(json.dumps(dict(rows=rows,bindings=bindings,additional_typerefs=additional_typerefs,member_definitions=defs,added_typespecs=newtypes,type_blobs=types[173:],added_methodspecs=methodspecs,user_strings=userstrings),ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
(N/'Bindings.cs').write_text('internal static partial class Program\n{\n'+''.join('    private const int %s = 0x%08X;\n'%(x['name'],x['token']) for x in bindings)+'    private static readonly HashSet<int> N119_EhTargets=['+','.join(r['token'] for r in rows if r['eh_clauses'])+'];\n}\n',encoding='utf-8')
print(json.dumps(dict(scope='SOURCE_DECLARATIONS_ONLY',method_sources=len(rows),new_member_refs=len(defs),new_typespecs=len(newtypes),new_user_strings=len(userstrings),candidate=None)))
