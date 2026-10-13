from pathlib import Path
import base64

p=Path('audit/native89-codex-2026-10-13');p.mkdir(exist_ok=True)
source=Path('audit/native85-codex-2026-10-13/Patcher85.cs').read_text().replace('RepairNative85','RepairNative89')
source=source.replace('b3284d01f0a57c9d22f6297741e96a831874cb0a584af2e165b98c42def84587','bff7fa2956077e5ac06a4b903fbb5a1be4aad989ac6b5cb08fd7d0aa6e690951')
source=source.replace('Pinned untested Native84 parent','Pinned untested Native88 parent')
a=source.index('  var initial=(MD)module.LookupToken(');b=source.index('  ulong valid=',a)
source=source[:a]+r'''  var binding=(MD)module.LookupToken(0x060000C4);R(binding.IsPublic&&binding.HasThis&&binding.GenericParameters.Count==1&&binding.Parameters.Count==4&&binding.Parameters[0].ParameterType.FullName=="T"&&binding.Parameters[1].ParameterType.FullName=="System.Boolean"&&binding.Parameters[2].ParameterType.FullName=="System.Single"&&binding.Parameters[3].ParameterType.FullName=="System.String","Exact public Binding<T> contract");
  R(module.GetTypes().Single(t=>t.FullName=="Plant").MetadataToken.ToInt32()==0x02000078,"Exact Plant TypeDef");
  int first=md.GetTableRowCount(TableIndex.MethodSpec)+1,si=Blob(new byte[]{0x0A,1,0x12,0x81,0xE0});
  byte[] row=Words(0xC4*2,si);R(row.Length==4,"Pinned MethodDefOrRef and blob widths");
  added.Add(new{kind="MethodSpec",token=$"0x{0x2B000000+first:X8}",method="0x060000C4",type_argument="0x02000078 Plant",instantiation_hex="0A011281E0",signature_index=si});
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MethodSpec,row}};
'''+source[b:]
source=source.replace('int initialZombie=0x2B000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MethodSpec)+1;', 'int bindingPlant=0x2B000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MethodSpec)+1;')
a=source.index('  var m=M(0x06000443);');b=source.index('  foreach(var(token,(code,max))in targets)',a)
source=source[:a]+r'''  var m=M(0x06000517);R(m.IsStatic&&m.Parameters.Count==4&&m.Parameters[0].ParameterType.FullName=="Plant"&&m.Parameters[1].ParameterType.FullName=="Zombie"&&m.Parameters[2].ParameterType.FullName=="Projectile"&&m.Parameters[3].ParameterType.FullName=="Damage"&&m.Body.Variables[1].VariableType.FullName=="UnityEngine.GameObject"&&m.Body.Variables[2].VariableType.FullName=="VFXAnimationEvent"&&m.Body.Variables[3].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[4].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.Transform","Static Awake149 and original typed captures");
  var component=(GenericInstanceMethod)module.LookupToken(0x2B000069);R(component.ElementMethod.Name=="GetComponent"&&component.DeclaringType.FullName=="UnityEngine.GameObject"&&component.GenericArguments.Single().FullName=="VFXAnimationEvent","Exact GetComponent<VFXAnimationEvent>");
  var il=new IL().Arg(0).Int(1).Ref(0x7D,F("Plant","blockable"))
   .Ref(0x28,M(0x060001E5)).Int(35).Ref(0x6F,M(0x060001EA)).S(1).L(1).Branch(0x39,"fail")
   .L(1).Ref(0x6F,component).S(2).L(2).Branch(0x39,"fail")
   .L(2).Arg(0).Int(1).Float(0f).O(0x72).I(ExistingStringToken("PlantSkill")).Ref(0x28,bindingPlant)
   .L(1).Ref(0x6F,0x0A00002E).S(3)
   .Arg(0).Ref(0x6F,0x0A000001).S(7).L(7).Branch(0x39,"fail")
   .L(7).Ref(0x6F,0x0A000037).S(4).L(3).Branch(0x39,"fail")
   .L(3).L(4).Ref(0x6F,0x0A000038).O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000517,il,5);
  m=M(0x06000510);R(m.IsStatic&&m.Parameters.Count==4&&m.Parameters[0].ParameterType.FullName=="Plant"&&m.Body.Variables[1].VariableType.FullName=="Board"&&m.Body.Variables[2].VariableType.FullName=="Skill"&&m.Body.Variables[4].VariableType.FullName=="Damage"&&m.Body.Variables[5].VariableType.FullName=="System.Single[]"&&m.Body.Variables[6].VariableType.FullName=="Element"&&m.Body.Variables[7].VariableType.FullName=="System.Single[]"&&m.Body.Variables[8].VariableType.FullName=="Projectile"&&m.Body.Variables[9].VariableType.FullName=="UnityEngine.Vector3","Static Function16 captured skill, two arrays, projectile/position");
  R(M(0x0600036C).IsPublic&&M(0x0600036C).Parameters.Count==2&&M(0x060003B9).IsPublic&&M(0x060003B9).ReturnType.FullName=="System.Collections.Generic.List`1<UnityEngine.GameObject>"&&M(0x06000110).IsPublic,"Actual public inherited getter/two-parameter GetDamage and FindElement contracts");
  var initial=(GenericInstanceMethod)module.LookupToken(0x2B000060);R(initial.ElementMethod.MetadataToken.ToInt32()==0x060003ED&&initial.GenericArguments.Single().FullName=="Plant","Exact existing Initial<Plant> six-argument binding");
  il=new IL().Arg(0).Ref(0x7B,F("Plant","board")).S(1).Arg(0).Ref(0x7B,F("Plant","skill")).S(2).L(1).Branch(0x39,"fail")
   .L(1).Ref(0x7B,F("Board","projectileManager")).Int(19).Ref(0x6F,M(0x060001F6)).S(8)
   .Arg(0).Ref(0x6F,M(0x060003B9)).O(0x72).I(ExistingStringToken("PeaShooter_mouth")).Ref(0x28,M(0x06000141)).S(9)
   .Arg(0).L(8).Int(19).Ref(0x28,M(0x0600036C)).S(4).L(4).Branch(0x39,"fail")
   .L(4).L(4).Ref(0x7B,F("Damage","damagePoint"))
   .L(2).Ref(0x7B,F("Skill","value_float")).S(5).L(5).Int(0).O(0x98,0x5A,0x6B).Ref(0x7D,F("Damage","damagePoint"))
   .L(4).L(5).Int(1).O(0x98).Ref(0x7D,F("Damage","penetrationVigour"))
   .L(4).Int(0).Ref(0x6F,M(0x06000110)).S(6).L(6).Branch(0x39,"initialize")
   .L(6).L(6).Ref(0x7B,F("Element","point"))
   .L(2).Ref(0x7B,F("Skill","value_float")).S(7).L(7).Int(2).O(0x98,0x5A,0x6B).Ref(0x7D,F("Element","point"))
   .Label("initialize").L(8).O(0x25).Branch(0x3A,"projectile-present").O(0x26).Branch(0x38,"fail")
   .Label("projectile-present").L(9).Float(1675f).Float(0f).Float(0f).Ref(0x73,0x0A00009C).Ref(0x28,0x0A0000FC).Float(70f).Int(0).Arg(0).Ref(0x28,initial)
   .L(8).Arg(0).Ref(0x7B,F("Plant","camp"))
   .L(8).L(4).Ref(0x7D,F("Projectile","damage")).Ref(0x7D,F("Projectile","camp"))
   .O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000510,il,7);
'''+source[b:]
a=source.index(',scope="')+1;b=source.index('"},new JsonSerializerOptions',a)
source=source[:a]+'scope="Two original-native-backed static SkillManager methods with exact Plant/Zombie/Projectile/Damage parameter identities; Awake149 sets blockable before creation, retains captured particle/component, restores Binding<Plant>(plant,true,+0,PlantSkill), acquires plant position before delayed effect-transform fault; Function16 captures skill before creation, captures position before fresh Damage, retains first value_float array for damage/penetration, rereads value_float after FindElement, preserves ordered partial writes and late projectile null fault, Initial<Plant> with speed1675/fZ70, fresh camp captured before damage/camp writes; one exact Binding<Plant> MethodSpec, no local mutations, complete oversize body relocation if needed; no tests executed; pending parent chain'+source[b:]
(p/'Patcher89.cs').write_text(source)
remote='''from pathlib import Path
import subprocess,base64,zipfile,json,hashlib
w=Path('/workspaces/GodsPVZ-native24-codespace');n=Path('/tmp/godspvz-native89-codex-2026-10-13');n.mkdir(exist_ok=True)
s=n/'Patcher';s.mkdir(exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(PATCHER_B64));(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True);(n/'build2.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'bin/Patcher.dll'),'/tmp/godspvz-native88-codex-2026-10-13/candidate1.dll',str(n/'candidate1.dll'),str(n/'generation1.json')],capture_output=True,text=True);(n/'generation2.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
dest=w/'.validation/native83-86-2026-10-13-checkpoints/native89-generated.zip'
if dest.exists():raise RuntimeError('Fresh persistent checkpoint required')
with zipfile.ZipFile(dest,'w',zipfile.ZIP_DEFLATED,compresslevel=6)as z:
 for f in n.rglob('*'):
  if f.is_file() and 'obj' not in f.parts:z.write(f,'build/native89/'+f.relative_to(n).as_posix())
print(json.dumps({'checkpoint_path':str(dest),'checkpoint_bytes':dest.stat().st_size,'checkpoint_sha256':hashlib.sha256(dest.read_bytes()).hexdigest()}))
print((n/'generation1.json').read_text())
'''.replace('PATCHER_B64',repr(base64.b64encode(source.encode()).decode()))
(p/'generate_remote.py').write_text(remote)
