from pathlib import Path
import base64,json
p=Path('audit/native78-codex-2026-10-13')
s=Path('audit/native77-codex-2026-10-12/Patcher77.cs').read_text().replace('RepairNative77','RepairNative78')
s=s.replace('9816bf53598fdbc2a63f95fadb6b2b86873561181d4ac3a7acc44ede36d25c1c','290767e0d4e513612c56b3d06cff3edf0894e58808a6338d6c6b87019a2a3f83').replace('Pinned untested Native76 parent','Pinned untested Native77 parent')
a=s.index('  var m=M(0x060003D9);'); b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var m=M(0x0600061D);
  R(m.Body.Variables[0].VariableType.FullName=="System.Int32","Existing explicitly initialized enum scratch");
  var i4=module.GetTypeReferences().Single(t=>t.FullName=="System.Int32");
  var il=new IL().Int(0).S(0)
   .Ref(0xD0,module.GetType("GridState")).Ref(0x28,C(m,"GetTypeFromHandle"))
   .Arg(1).Ref(0x8C,i4).Ref(0x28,C(m,"IsDefined")).Branch(0x2C,"invalid")
   .Arg(1).S(0).Branch(0x2B,"check-system")
   .Label("invalid").O(0x72).I(StringToken(m,"此序号对应格子类型不存在")).Ref(0x28,C(m,"Log"))
   .Label("check-system").Arg(0).Ref(0x7B,F("GridConfig_A","system")).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"return")
   .Arg(0).Ref(0x7B,F("GridConfig_A","system")).O(0x25).Branch(0x2D,"system-live").O(0x26,0x14,0x7A)
   .Label("system-live").L(0).Arg(0).Ref(0x7B,F("GridConfig_A","gridX")).Arg(0).Ref(0x7B,F("GridConfig_A","gridY")).Ref(0x28,C(m,"SetGridState"))
   .Label("return").O(0x2A);
  Set(0x0600061D,il,4);
  m=M(0x06000483);
  var zero=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="UnityEngine.Vector3 UnityEngine.Vector3::get_zero()");
  il=new IL().Arg(0).Ref(0x7B,F("Zombie","targetPlant")).Arg(1).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"tail")
   .Arg(0).Ref(0x7B,F("Zombie","targetPlant")).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"new-target")
   .Arg(0).Ref(0x7B,F("Zombie","targetPlant")).O(0x25).Branch(0x2D,"old-live").O(0x26,0x14,0x7A)
   .Label("old-live").Arg(0).Int(0).Ref(0x28,C(m,"BlockZombie"))
   .Label("new-target").Arg(1).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"tail")
   .Arg(1).O(0x25).Branch(0x2D,"new-live").O(0x26,0x14,0x7A)
   .Label("new-live").Arg(0).Int(1).Ref(0x28,C(m,"BlockZombie"))
   .Label("tail").Arg(0).Ref(0x7B,F("Zombie","animator")).O(0x25).Branch(0x2D,"animator-live").O(0x26,0x14,0x7A)
   .Label("animator-live").O(0x72).I(StringToken(m,"isAttacking")).Int(1).Ref(0x28,C(m,"SetBool"))
   .Arg(0).Ref(0x28,C(m,"ResetAttackSpeed"))
   .Arg(0).Ref(0x28,zero).Ref(0x7D,F("Zombie","rSpeed"))
   .Arg(0).Int(1).Ref(0x7D,F("Zombie","isEat")).O(0x2A);
  Set(0x06000483,il,3);
'''+s[b:]
s=s.replace('Two native-backed bodies; ordered 1.5 threshold including NaN reset, x-only speed write, real log and string formatting helpers, separate float32 numerator/denominator with field rereads after callbacks, captured boxing particle positioned before event assignment; all metadata and local signatures unchanged; pure public singleton getter guarded; unqualified parent chain pending','Two original-native-backed bodies; GridState typeof and boxed Int32 independently mapped from original usage cells; one GetTypeFromHandle; invalid state logs then continues with zero; system and targetPlant rereads after Unity comparisons/callbacks; native method does not assign targetPlant; raw-null branches throw NullReferenceException; public Vector3.get_zero exact getter IL evidence; all metadata and local signatures unchanged; no tests executed; unqualified parent chain pending')
(p/'Patcher78.cs').write_text(s)
parent='/tmp/godspvz-native77-restored-2026-10-13/snapshot/candidates/native77/candidate1.dll'
t='''from pathlib import Path
import subprocess,base64,json
w=Path('/workspaces/GodsPVZ-native24-codespace')
n=Path('/tmp/godspvz-native78-codex-2026-10-13');n.mkdir(exist_ok=True)
s=n/'Patcher';s.mkdir(exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(SOURCE))
(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True)
(n/'build.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'bin/Patcher.dll'),PARENT,str(n/'candidate1.dll'),str(n/'generation1.json')],capture_output=True,text=True)
(n/'generation.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
print((n/'generation1.json').read_text())
'''.replace('SOURCE',repr(base64.b64encode(s.encode()).decode())).replace('PARENT',repr(parent))
(p/'generate_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text())
(p/'native-methods.json').write_text(json.dumps([x for x in d['methods'] if x['token']in('0x0600061D','0x06000483')],indent=2))
print('Prepared two-method patcher and implementation generation')
