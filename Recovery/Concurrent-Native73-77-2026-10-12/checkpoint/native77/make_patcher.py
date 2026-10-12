from pathlib import Path
import base64
p=Path('audit/native77-codex-2026-10-12')
s=Path('audit/native75-codex-2026-10-12/Patcher75.cs').read_text().replace('RepairNative75','RepairNative77')
s=s.replace('71c28b1cad9ff8e5dd456121c2b0747792c3281f66c3da701002e91d9e3b9f4c','9816bf53598fdbc2a63f95fadb6b2b86873561181d4ac3a7acc44ede36d25c1c').replace('Pinned untested Native74 parent','Pinned untested Native76 parent')
a=s.index('  var vector=module.');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var m=M(0x060003D9);
  R(m.Body.Variables[3].VariableType.FullName=="System.Single"&&m.Body.Variables[4].VariableType.FullName=="System.Single"&&m.Body.Variables[9].VariableType.FullName=="System.Single","Existing native scalar scratch");
  var vx=module.GetMemberReferences().OfType<FR>().Single(r=>r.FullName=="System.Single UnityEngine.Vector3::x");
  var il=new IL().Arg(0).Ref(0x7B,F("Projectile","livingTime")).Float(1.5f).Branch(0x31,"formula")
   .Arg(0).Ref(0x7C,F("Projectile","speed")).Float(1500f).Ref(0x7D,vx)
   .Arg(0).Float(0f).Ref(0x7D,F("Projectile","zSpeed"))
   .Arg(0).Ref(0x28,C(m,"Update_Tracking"))
   .Arg(0).Int(0).Ref(0x7D,F("Projectile","movementTracks")).O(0x2A)
   .Label("formula").Double(10d).Ref(0x28,C(m,"Log")).O(0x6B).S(3)
   .O(0x72).I(StringToken(m,"a=")).A(3).Ref(0x28,C(m,"ToString")).Ref(0x28,C(m,"Concat"))
   .Ref(0x28,m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name=="Log"&&r.DeclaringType.FullName=="UnityEngine.Debug"))
   .Float(95f).Arg(0).Ref(0x7B,F("Projectile","fZ")).O(0x59,0x6B).S(4)
   .O(0x72).I(StringToken(m,"d=")).A(4).Ref(0x28,C(m,"ToString")).Ref(0x28,C(m,"Concat"))
   .Ref(0x28,m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name=="Log"&&r.DeclaringType.FullName=="UnityEngine.Debug"))
   .Arg(0).Ref(0x7B,F("Projectile","livingTime")).Float(6f).O(0x5A,0x6B).L(3).O(0x5A,0x6B).L(3).O(0x58,0x6B).S(9)
   .L(4).Float(6f).O(0x5A,0x6B).S(10)
   .Arg(0).L(10).L(9).O(0x5B,0x6B).Ref(0x7D,F("Projectile","zSpeed"))
   .Arg(0).Ref(0x28,C(m,"Update_Tracking")).O(0x2A);
  Set(0x060003D9,il,3);
  m=M(0x06000491);
  var getter=M(0x060001E5);var gi=getter.Body.Instructions.ToArray();
  R(getter.IsPublic&&getter.IsStatic&&getter.Parameters.Count==0&&gi.Length==3&&gi[0].OpCode.FlowControl==FlowControl.Branch&&gi[0].Operand==gi[1]&&gi[1].OpCode==OpCodes.Ldsfld&&gi[1].Operand is FR gf&&gf.FullName==F("ParticlesManager","<instance>k__BackingField").FullName&&gi[2].OpCode==OpCodes.Ret,"Exact unchanged pure particle singleton getter");
  R(m.Body.Variables[7].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[8].VariableType.FullName=="UnityEngine.GameObject","Existing copied position and captured particle");
  il=new IL().Arg(0).Ref(0x7B,F("Zombie","board")).Ref(0x7B,F("Board","boardConfig")).Arg(1).Arg(2).Ref(0x6F,C(m,"GetPlantingPosition")).S(7)
   .Ref(0x28,getter).Int(13).Ref(0x6F,C(m,"CreatNewParticle")).S(8)
   .L(8).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"return")
   .L(8).Ref(0x6F,C(m,"get_transform")).L(7).Ref(0x6F,C(m,"set_position"))
   .L(8).Ref(0x6F,C(m,"GetComponent")).Arg(0).Ref(0x7D,F("ZombieAnimationEvent","zombie"))
   .Label("return").O(0x2A);
  Set(0x06000491,il,3);
'''+s[b:]
s=s.replace('R(report.Count==4,"Pinned scope 4")','R(report.Count==2,"Pinned scope 2")')
a=s.index('scope="Four native-backed bodies;');b=s.index('"},new JsonSerializerOptions',a)
s=s[:a]+'scope="Two native-backed bodies; ordered 1.5 threshold including NaN reset, x-only speed write, real log and string formatting helpers, separate float32 numerator/denominator with field rereads after callbacks, captured boxing particle positioned before event assignment; all metadata and local signatures unchanged; pure public singleton getter guarded; unqualified parent chain pending'+s[b:]
(p/'Patcher77.cs').write_text(s)
t=Path('audit/native76-codex-2026-10-12/generate_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native76-codex','godspvz-native77-codex').replace('scripts/takeover/native76/Patcher','scripts/takeover/native77/Patcher').replace('/tmp/godspvz-native75-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native76-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
