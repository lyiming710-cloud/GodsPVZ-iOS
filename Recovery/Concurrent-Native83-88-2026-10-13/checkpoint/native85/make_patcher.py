from pathlib import Path
import base64

p = Path('audit/native85-codex-2026-10-13')
p.mkdir(exist_ok=True)
source = Path('audit/native84-codex-2026-10-13/Patcher84.cs').read_text()
template = Path('audit/native81-codex-2026-10-13/Patcher81.cs').read_text()
source = source.replace('RepairNative84', 'RepairNative85')
source = source.replace('d1727135f7fcb7141a62ad5bbe0e2342e6d25efb97d25b59ae223bb26fcd9975', 'b3284d01f0a57c9d22f6297741e96a831874cb0a584af2e165b98c42def84587')
source = source.replace('Pinned untested Native83 parent', 'Pinned untested Native84 parent')
metadata = template[template.index(' static (byte[] Image,object Evidence) Metadata('):template.index(' static (byte[] Image,object Evidence) AppendBody(')]
a = metadata.index('  var memberRows=')
b = metadata.index('  ulong valid=', a)
metadata = metadata[:a] + r'''  var added=new List<object>();
  var initial=(MD)module.LookupToken(0x060003ED);
  R(initial.IsPublic&&initial.HasThis&&initial.GenericParameters.Count==1&&initial.Parameters.Count==6&&initial.Parameters[0].ParameterType.FullName=="UnityEngine.Vector3"&&initial.Parameters[5].ParameterType.FullName=="T","Pinned six-argument public Projectile Initial definition");
  R(module.GetTypes().Single(t=>t.FullName=="Zombie").MetadataToken.ToInt32()==0x02000084,"Exact Zombie TypeDef");
  int first=md.GetTableRowCount(TableIndex.MethodSpec)+1,si=Blob(new byte[]{0x0A,1,0x12,0x82,0x10});
  byte[] row=Words(0x3ED*2,si);R(row.Length==4,"Pinned MethodDefOrRef and blob widths");
  added.Add(new{kind="MethodSpec",token=$"0x{0x2B000000+first:X8}",method="0x060003ED",type_argument="0x02000084 Zombie",instantiation_hex="0A01128210",signature_index=si});
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MethodSpec,row}};
''' + metadata[b:]
metadata = metadata.replace('width==(t==TableIndex.MemberRef?6:2)', 't==TableIndex.MethodSpec&&width==4')
metadata = metadata.replace('clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig)', 'clear=1UL<<(int)TableIndex.MethodSpec')
metadata = metadata.replace('table_sorted_bit_cleared=(sorted&(1UL<<10))!=0', 'methodspec_sorted_bit_cleared=(sorted&(1UL<<43))!=0')
insert = source.index(' static (byte[] Image,object Evidence) AppendBody(')
source = source[:insert] + metadata + source[insert:]
source = source.replace('  byte[] after=before.ToArray();', '  var(after,metadata)=Metadata(before,pe,module);int initialZombie=0x2B000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MethodSpec)+1;')
insert = source.index('  var m=M(0x060001C8);')
source = source[:insert] + r'''  int ExistingStringToken(string text){
   var md=pe.GetMetadataReader();byte[] content=pe.GetMetadata().GetContent().ToArray();int start=md.GetHeapMetadataOffset(HeapIndex.UserString),length=md.GetHeapSize(HeapIndex.UserString);
   for(int ix=1;ix<length;){int p=start+ix,size=content[p++];if((size&0x80)!=0){if((size&0xC0)==0x80)size=((size&0x3F)<<8)|content[p++];else{R((size&0xE0)==0xC0,"Valid four-byte compressed user-string length");size=((size&0x1F)<<24)|(content[p++]<<16)|(content[p++]<<8)|content[p++];}}
    R(size>=0&&p+size<=start+length,"Existing bounded user-string entry");if(size>0&&md.GetUserString(MetadataTokens.UserStringHandle(ix))==text)return 0x70000000+ix;ix=p+size-start;
   }throw new InvalidOperationException("Exact existing user string absent: "+text);
  }
''' + source[insert:]
a = source.index('  var m=M(0x060001C8);')
b = source.index('  foreach(var(token,(code,max))in targets)', a)
source = source[:a] + r'''  var m=M(0x06000443);R(m.Body.Variables[4].VariableType.FullName=="Damage"&&m.Body.Variables[7].VariableType.FullName=="Damage"&&m.Body.Variables[5].VariableType.FullName=="System.Int32"&&m.Body.Variables[6].VariableType.FullName=="System.Single","Exact ref slot/final Damage and numeric locals");
  R(M(0x06000447).Parameters[0].ParameterType.FullName=="Damage&"&&M(0x06000446).Parameters[0].ParameterType.FullName=="Damage&","Both helpers take the same managed-reference slot shape");
  var il=new IL().Ref(0x73,M(0x06000112)).S(4).L(4).Branch(0x39,"fail")
   .L(4).Arg(0).Ref(0x7B,F("Zombie","camp")).Ref(0x7D,F("Damage","camp"))
   .Arg(0).A(4).Arg(1).Arg(3).Ref(0x28,M(0x06000447))
   .Arg(0).A(4).Arg(1).Arg(2).Arg(3).Ref(0x28,M(0x06000446))
   .Arg(0).Ref(0x28,M(0x06000441)).S(6)
   .Arg(0).Ref(0x7B,F("Zombie","ID")).S(5).L(5).Int(3).O(0x59)
   .Switch("id3","common","common","common","common","common","common","common","id11","common","id13","common","common","id16","id17","id18","common","common","common","common").Branch(0x38,"common")
   .Label("id3").Arg(1).Int(0).Branch(0x3F,"common").L(6).Float(12f).Branch(0x38,"multiply")
   .Label("id11").L(6).Float(15f).Branch(0x38,"multiply")
   .Label("id13").Arg(0).Ref(0x28,M(0x0600044B)).L(6).O(0x5A,0x6B).S(6)
   .L(6).O(0x25,0x58,0x6B).S(6).Branch(0x38,"common")
   .Label("id16").Arg(1).Int(0).Branch(0x3F,"id16-negative").L(6).Float(4.5f).Branch(0x38,"multiply")
   .Label("id16-negative").L(6).Float(9f).Branch(0x38,"multiply")
   .Label("id17").Arg(1).Int(0).Branch(0x3F,"common").L(6).Float(5f).Branch(0x38,"multiply")
   .Label("id18").Arg(3).Branch(0x39,"id18-zero").Arg(3).Int(1).Branch(0x40,"common").L(6).Float(8f).Branch(0x38,"multiply")
   .Label("id18-zero").L(6).Float(0.5f)
   .Label("multiply").O(0x5A,0x6B).S(6)
   .Label("common").L(4).S(7).L(7).Branch(0x39,"fail")
   .L(7).L(6).Ref(0x7D,F("Damage","damagePoint"))
   .Arg(0).Ref(0x7B,F("Zombie","ID")).S(5).L(5).Int(7).Branch(0x3B,"check-projectile-id")
   .L(5).Int(20).Branch(0x40,"source-fields")
   .Label("check-projectile-id").Arg(1).Int(0).Branch(0x3F,"source-fields")
   .L(7).Int(0).Float(1000f).L(7).Int(1).Ref(0x73,M(0x0600011C)).Ref(0x28,M(0x0600010B))
   .Label("source-fields").L(7).Arg(0).Ref(0x7D,F("Damage","sourceZombie"))
   .L(7).Arg(0).Ref(0x7B,F("Zombie","board")).Ref(0x7D,F("Damage","board"))
   .L(7).O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000443,il,5);
  m=M(0x0600047D);R(m.Body.Variables[1].VariableType.FullName=="System.Object"&&m.Body.Variables[2].VariableType.FullName=="System.Boolean"&&m.Body.Variables[5].VariableType.FullName=="Board"&&m.Body.Variables[6].VariableType.FullName=="Projectile"&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[8].VariableType.FullName=="Damage","Existing Shooting captures");
  var stringType=module.GetTypeReferences().Single(t=>t.FullName=="System.String");
  il=new IL().Int(0).S(2).O(0x72).I(ExistingStringToken("")).S(1)
   .Arg(0).Ref(0x7B,F("Zombie","ID")).Int(6).O(0x59)
   .Switch("mouth","mouth-one","mouth","ordinary","ordinary","ordinary","ordinary","ordinary","ordinary","ordinary","ordinary","special","ordinary","mouth","mouth-one","mouth","ordinary").Branch(0x38,"ordinary")
   .Label("mouth-one").Int(1).S(2)
   .Label("mouth").O(0x72).I(ExistingStringToken("PeaShooter_mouth")).S(1).Branch(0x38,"ordinary")
   .Label("special").Arg(0).Ref(0x28,M(0x060004AF)).O(0x2A)
   .Label("ordinary").Arg(0).Ref(0x7B,F("Zombie","board")).S(5).L(5).Branch(0x39,"fail")
   .L(5).Ref(0x7B,F("Board","projectileManager")).O(0x25).Branch(0x3A,"manager-present").O(0x26).Branch(0x38,"fail")
   .Label("manager-present").L(2).O(0x69).Ref(0x28,M(0x060001F6)).S(6)
   .Arg(0).Ref(0x7B,F("Zombie","animationSprites")).L(1).Ref(0x74,stringType).Ref(0x28,M(0x06000141)).S(7)
   .Arg(0).L(2).O(0x69).L(6).Int(0).Ref(0x28,M(0x06000443)).S(8)
   .L(6).O(0x25).Branch(0x3A,"projectile-present").O(0x26).Branch(0x38,"fail")
   .Label("projectile-present").L(7)
   .O(0x22).I(unchecked((int)0xC40B9333)).Float(0f).Float(0f).Ref(0x73,0x0A00009C)
   .Ref(0x28,0x0A0000FC).Arg(0).Ref(0x7B,F("Zombie","fZ")).Float(120f).O(0x58,0x6B).Int(0).Arg(0).Ref(0x28,initialZombie)
   .L(6).L(8).Arg(0).Ref(0x7B,F("Zombie","camp")).Ref(0x28,M(0x060003F7)).O(0x2A)
   .Label("fail").O(0x14,0x7A);
  Set(0x0600047D,il,7);
''' + source[b:]
source = source.replace('metadata_scope="Existing table rows except target MethodDef RVA remain unchanged by construction; no new rows or heap edits"', 'metadata')
a = source.index(',scope="') + 1
b = source.index('"},new JsonSerializerOptions', a)
source = source[:a] + 'scope="Two complete original-native-backed Zombie bodies; GetDamage retains constructor camp timing, one byref slot across ordered helpers, late raw-null failure, two ID snapshots, exact Single operand/order and post-AddElement board reread; Shooting preserves 17-case dispatch, late projectile null failure after position/GetDamage, captured full position, exact speed bits, current fZ plus120 and post-Initial camp; one explicit MethodSpec Initial<Zombie> bound to six-argument public generic definition, no shared signature mutation; complete oversize bodies relocated with target-only RVA changes and old slots preserved; no tests executed; unqualified parent chain pending' + source[b:]
(p/'Patcher85.cs').write_text(source)
remote = '''from pathlib import Path
import subprocess,base64
w=Path('/workspaces/GodsPVZ-native24-codespace')
n=Path('/tmp/godspvz-native85-codex-2026-10-13');n.mkdir(exist_ok=True)
s=n/'Patcher';s.mkdir(exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(PATCHER_B64))
(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True)
(n/'build2.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'bin/Patcher.dll'),'/tmp/godspvz-native84-codex-2026-10-13/candidate3.dll',str(n/'candidate1.dll'),str(n/'generation1.json')],capture_output=True,text=True)
(n/'generation2.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
print((n/'generation1.json').read_text())
'''.replace('PATCHER_B64', repr(base64.b64encode(source.encode()).decode()))
(p/'generate_remote.py').write_text(remote)
