"""Author source and metadata descriptions only. Never build or execute project code."""
from pathlib import Path
import hashlib,json,re,sys
ROOT=Path(__file__).resolve().parents[1]
N=ROOT/'audit/native111-codex-2026-10-20'
OLD=ROOT/'audit/native-bulk162-r1-codex-2026-10-20'
OUT=ROOT/'audit/native-bulk168-codex-2026-10-20'
sys.dont_write_bytecode=True
sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile
parent=ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'
md=dnfile.dnPE(str(parent)).net.mdtables
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
sha=lambda b:hashlib.sha256(b).hexdigest()
def write(p,s):p.write_text(s,encoding='utf-8',newline='\n')
def dump(p,x):write(p,json.dumps(x,indent=2,ensure_ascii=False)+'\n')
def freeze(p):
 files=[{'path':f.relative_to(p).as_posix(),'bytes':f.stat().st_size,'sha256':sha(f.read_bytes())} for f in sorted(p.rglob('*')) if f.is_file() and f.name!='SOURCE-MANIFEST.json']
 dump(p/'SOURCE-MANIFEST.json',{'schema':'source-only-manifest-v1','date':'2026-10-20','state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','root_executed_project_tests':False,'candidate':None,'files':files})
def compressed(v):
 if v<128:return bytes([v])
 return bytes([(v>>8)|0x80,v&255])
def sig(name,value=False,ref=False):
 rows=md.TypeRef.rows if ref else md.TypeDef.rows
 rid=next(i for i,t in enumerate(rows,1) if str(t.TypeName)==name)
 return (b'\x11' if value else b'\x12')+compressed((rid<<2)|(1 if ref else 0))

cur=read(N/'CURRENT-SOURCE.json');orig=read(N/'ORIGINAL-SOURCE.json')
cm={m['token']:m for m in cur['methods']};om={m['token']:m for m in orig['methods']}
portal_blob=(b'\x07\x08'+sig('Map')+sig('DeviceManager')+sig('Device')+b'\x08'+bytes.fromhex('15115101128190')+sig('Grid')+sig('DeviceData')+sig('Device')).hex()
color_blob=(b'\x07\x06'+b'\x0c'*3+sig('Color',True,True)+bytes.fromhex('151151011209')+sig('GameObject',ref=True)).hex()
loop_blob=(b'\x07\x02'+sig('IEnumerator',ref=True)+sig('IDisposable',ref=True)).hex()
shoot_blob=(b'\x07\x04'+sig('AnimatorStateInfo',True,True)+b'\x0c'+sig('Image',ref=True)+sig('Animator',ref=True)).hex()
mortar_blob=(b'\x07\x07'+sig('Vector3',True,True)*3+sig('Projectile')+sig('Damage')+sig('Skill')+bytes.fromhex('15120d0108')).hex()
blobs=[color_blob,loop_blob,portal_blob,shoot_blob,mortar_blob]
defs=[];constants=[]
def member(name,owner_rid,template,new_name=None):
 row=md.MemberRef.rows[template-1]
 owner=md.TypeSpec.rows[owner_rid-1] if isinstance(owner_rid,int) else None
 if owner is not None:
  code=owner_rid*8+4;pin=owner.Signature.value.hex()
 else:
  rid=next(i for i,t in enumerate(md.TypeRef.rows,1) if str(t.TypeName)==owner_rid)
  t=md.TypeRef.rows[rid-1];code=rid*8+1;pin=str(t.TypeNamespace)+'.'+str(t.TypeName)
 tok=0x0A000000+1171+len(defs)
 defs.append({'name':name,'token':tok,'class_code':code,'owner_pin':pin,'template':template,'template_name':str(row.Name),'signature':row.Signature.value.hex(),'new_name':new_name})
 constants.append({'name':name,'token':tok})
member('N111_GridEnumerator',24,0xB7)
member('N111_GridCurrent',83,0xB8)
member('N111_GridMove',83,0xB9)
member('N111_NormalizedTime','AnimatorStateInfo',0xFA,'get_normalizedTime')
for i,b in enumerate(blobs):constants.append({'name':f'N111_Locals{i+1}','token':0x11000000+1873+i})
specs=[
 {'name':'N111_MapPlaceDevice','token':0x2B0000BC,'definition':0x06000290,'owner':'Map','method_name':'RandomGet_Grid_TestPlace','signature':'30010415120d01128190081e000808','instantiation':'0a01'+sig('Device').hex(),'original_slot':'0x181baf660'},
 {'name':'N111_ProjectileInitialPlant','token':0x2B0000BD,'definition':0x060003EE,'owner':'Projectile','method_name':'Initial','signature':'30010701114d114d114d0c0c081e00','instantiation':'0a01'+sig('Plant').hex(),'original_slot':'0x181bbb878'}]
for s in specs:constants.append({'name':s['name'],'token':s['token']})
rows=[('0x06000352','N111_PlantColorBody',5,1),('0x06000308','N111_DeviceLoopAnimationBody',2,2),
 ('0x0600048F','N111_ZombiePortalBody(false)',6,3),('0x06000494','N111_ZombiePortalBody(true)',6,3),
 ('0x060004B1','N111_ZombieTryShootingBody',4,4),('0x060003AC','N111_PotatomortarBody',8,5)]
targets=[]
for tok,body,maxs,local in rows:
 m=cm[tok];o=om[tok]
 targets.append({'token':tok,'name':m['name'],'signature':m['signature'],'old_code_size':m['code_size'],'old_code_sha256':m['il_sha256'],
  'old_max_stack':m['max_stack'],'old_local_sig_token':f"0x{m['local_sig']:08X}",'old_local_sig_blob':m['locals_blob'],
  'new_declared_max_stack':maxs,'local_constant':f'N111_Locals{local}','local_blob':blobs[local-1],
  'emission_method':body,'explicit_companion':False,'source_batch':N.name,'original_native':{k:o[k] for k in ('start','end','sha256','pdata')}})
nd={'schema':'native111-source-metadata-v1','member_definitions':defs,'local_blobs':blobs,'constants':constants,'added_methodspecs':specs,
 'appended_blob_payload_bytes':sum(1+len(bytes.fromhex(b)) for b in blobs)+sum(1+len(bytes.fromhex(s['instantiation'])) for s in specs),
 'appended_string_payload_bytes':sum(len(d['new_name'].encode())+1 for d in defs if d['new_name']),
 'added_typespecs':[],'added_user_strings':[]}
dump(N/'METADATA-ADDITIONS.json',nd)
dump(N/'IMPLEMENTATION-PLAN.json',{'schema':'native111-source-implementation-v1','state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','targets':targets,
 'root_executed_project_tests':False,'candidate':None,'new_qualified_methods':0,'appended_finally_method_tokens':['0x06000352','0x06000308','0x0600048F','0x06000494']})
write(N/'README.md','# Native111: six method-body sources\n\nSource only. No candidate construction, project tests or acceptance by the primary author. Original PC closed generic metadata identifies Map.RandomGet_Grid_TestPlace<Device> and Projectile.Initial<Plant>; exact slots are saved in ORIGINAL-GENERIC-BINDINGS.json. Four finally regions are restored. Previous source and test states remain independent.\n')
freeze(N)

OUT.mkdir(exist_ok=False)
for item in read(OLD/'SOURCE-MANIFEST.json')['files']:
 p=OLD/item['path']
 if p.suffix=='.cs':write(OUT/p.name,p.read_text(encoding='utf-8-sig'))
write(OUT/'N111.MethodBodies.cs',(N/'MethodBodies.cs').read_text(encoding='utf-8'))
program=(OUT/'Program.cs').read_text(encoding='utf-8')
pins=[]
for t in targets:
 m=cm[t['token']];owner,name=m['name'].split('::');owner=owner.rsplit('.',1)[-1]
 pins.append(f'        new({t["token"]},"{owner}","{name}","{m["signature"]}",{m["code_size"]},{m["max_stack"]},0x{m["local_sig"]:08X},"{m["locals_blob"]}","{m["il_sha256"]}"),')
at=program.index('];',program.index('private static readonly Pin[] Pins = ['));program=program[:at]+'\n'.join(pins)+'\n    '+program[at:]
plans=[]
for t in targets:
 call=t['emission_method'] if '(' in t['emission_method'] else t['emission_method']+'()'
 plans.append(f'            (Token:{t["token"]}, Code:{call}, Max:{t["new_declared_max_stack"]}, Locals:{t["local_constant"]})')
at=program.index('};',program.index('var plans = new[] {'));program=program[:at].rstrip()+',\n'+',\n'.join(plans)+'\n        '+program[at:]
program=program.replace('fresh-bulk162.dll','fresh-bulk168.dll').replace('appended_memberref_count = 70, appended_local_signature_count = 107,','appended_memberref_count = 74, appended_local_signature_count = 112,').replace('independent 162-target','independent 168-target')
write(OUT/'Program.cs',program)
f=(OUT/'FinallyBodies.cs').read_text(encoding='utf-8').replace(',"Explicit EH target allowlist"', ' or 0x06000352 or 0x06000308 or 0x0600048F or 0x06000494,"Explicit EH target allowlist"')
write(OUT/'FinallyBodies.cs',f)
desc=read(OLD/'METADATA-ADDITIONS.json');desc['schema']='bulk168-source-metadata-v1'
for key in ['member_definitions','local_blobs','constants']:desc[key]+=nd[key]
desc['added_methodspecs']=desc.get('added_methodspecs',[])+specs
desc['appended_blob_payload_bytes']+=nd['appended_blob_payload_bytes'];desc['appended_string_payload_bytes']+=nd['appended_string_payload_bytes']
dump(OUT/'METADATA-ADDITIONS.json',desc)
meta=(ROOT/'audit/native107-codex-2026-10-18/MetadataAppend.v4.template.txt').read_text(encoding='utf-8')
meta=meta.replace('__CONSTANTS__','\n'.join(f'    private const int {x["name"]} = 0x{x["token"]:08X};' for x in desc['constants']))
meta=meta.replace('__MEMBERS__','\n'.join('        new(0x%08X,%d,%s,0x%X,%s,%s,%s),'%(x['token'],x['class_code'],json.dumps(x['owner_pin']),x['template'],json.dumps(x['template_name']),json.dumps(x['signature']),'null' if x['new_name'] is None else json.dumps(x['new_name'])) for x in desc['member_definitions']))
meta=meta.replace('__LOCAL_BLOBS__','\n'.join('        '+json.dumps(x)+',' for x in desc['local_blobs'])).replace('__BASE_MEMBERS__','1100').replace('__BASE_LOCALS__','1765')
extra='''
    private record ExtraSpec(int Token,int Definition,string Owner,string Name,string Signature,string Instantiation);
    private static readonly ExtraSpec[] ExtraMethodSpecs = [
'''+ '\n'.join(f'        new(0x{s["token"]:08X},0x{s["definition"]&0xFFFFFF:X},{json.dumps(s["owner"])},{json.dumps(s["method_name"])},{json.dumps(s["signature"])},{json.dumps(s["instantiation"])}),' for s in specs)+ '\n    ];\n'
meta=meta.replace('    private static byte[] AppendMetadata',extra+'    private static byte[] AppendMetadata',1)
append='''
        byte[] extendedMethodRows=new byte[methodRows.Length+ExtraMethodSpecs.Length*4];methodRows.CopyTo(extendedMethodRows,0);
        for(int i=0;i<ExtraMethodSpecs.Length;i++)
        {
            var d=ExtraMethodSpecs[i];Require(d.Token==0x2B000000+188+i,"Pinned append-only extra MethodSpec RID");
            var def=md.GetMethodDefinition(MetadataTokens.MethodDefinitionHandle(d.Definition));
            var owner=md.GetTypeDefinition(def.GetDeclaringType());
            Require(md.GetString(owner.Name)==d.Owner && md.GetString(def.Name)==d.Name && md.GetBlobBytes(def.Signature).SequenceEqual(Convert.FromHexString(d.Signature)),"Pinned generic game MethodDef identity and signature");
            int row=methodRows.Length+i*4;
            BinaryPrimitives.WriteUInt16LittleEndian(extendedMethodRows.AsSpan(row,2),checked((ushort)(d.Definition<<1)));
            BinaryPrimitives.WriteUInt16LittleEndian(extendedMethodRows.AsSpan(row+2,2),checked((ushort)(blobs.Size+blobSuffix.Count)));
            byte[] inst=Convert.FromHexString(d.Instantiation);Require(inst.Length<128 && inst[0]==10 && inst[1]==1,"Single closed generic argument");
            blobSuffix.Add(checked((byte)inst.Length));blobSuffix.AddRange(inst);
        }
        methodRows=extendedMethodRows;
'''
meta=meta.replace('        methodRows=bothMethodRows;', '        methodRows=bothMethodRows;'+append,1)
meta=meta.replace('Write32(tablePayload,rowCountAt,187);','Write32(tablePayload,rowCountAt,187+ExtraMethodSpecs.Length);')
meta=meta.replace('appended_methodspecs=2,','appended_methodspecs=2+ExtraMethodSpecs.Length,')
write(OUT/'MetadataAppend.cs',meta)
write(OUT/'Bulk168.csproj',(OLD/'Bulk162.csproj').read_text(encoding='utf-8').replace('  </ItemGroup>','    <Compile Include="N111.MethodBodies.cs"/>\n  </ItemGroup>'))
plan=read(OLD/'IMPLEMENTATION-PLAN.json');plan.update(schema='bulk168-source-implementation-v1',targets=plan['targets']+targets,method_body_targets=168,
 target_tokens=plan['target_tokens']+[t['token'] for t in targets],historical_direct_targets=162,explicit_companion_targets=6,unchanged_other_method_bodies=2149,
 memberref_count_after=1174,standalone_count_after=1877,methodspec_count_after=189,metadata_row_payload_added_bytes=688,
 appended_blob_payload_bytes=desc['appended_blob_payload_bytes'],new_unique_source_methods_this_turn=6,supersedes_source_bundle=OLD.relative_to(ROOT).as_posix())
dump(OUT/'IMPLEMENTATION-PLAN.json',plan)
dump(OUT/'SOURCE-LINEAGE.json',{'schema':'bulk168-source-lineage-v1','parent_sha256':plan['parent_sha256'],'candidate_parent_is_native93':True,
 'tests_inherited':False,'new_acceptance_inherited':False,'expected_previous_candidate_sha256':'089cddb15cc5f2d543576c4935e5c861a082a5853118c15f65a4d36e40116da8',
 'new_target_tokens':[t['token'] for t in targets],'expected_preserved_inherited_method_bodies':162,'expected_total_method_bodies':168,
 'source_manifests':[{'path':str((p/'SOURCE-MANIFEST.json').relative_to(ROOT)).replace('\\','/'),'sha256':sha((p/'SOURCE-MANIFEST.json').read_bytes())} for p in (OLD,N)]})
write(OUT/'README.md','# Bulk168 authored source\n\nNative93 construction parent. Adds six Native111 methods to the unchanged Bulk162 R1 source: 168 bodies = 162 direct historical targets + six existing companions. Four new finally clauses, 34 total. MemberRef1174 / StandAloneSig1877 / TypeSpec175 / MethodSpec189. No candidate construction, tests, runtime or compiler acceptance is claimed by the primary author. Test ownership remains with AGY and GitHub Actions.\n')
freeze(OUT)
print(json.dumps({'state':'SOURCE_ONLY_UNBUILT_UNEXECUTED','new_method_sources':6,'aggregate_sources':168,'project_tests_executed':False,'source_manifest_sha256':sha((OUT/'SOURCE-MANIFEST.json').read_bytes())},indent=2))
