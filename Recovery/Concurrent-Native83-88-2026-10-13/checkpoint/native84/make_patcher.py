from pathlib import Path
import base64

p = Path('audit/native84-codex-2026-10-13')
p.mkdir(exist_ok=True)
source = Path('audit/native82-codex-2026-10-13/Patcher82.cs').read_text()
template81 = Path('audit/native81-codex-2026-10-13/Patcher81.cs').read_text()
source = source.replace('RepairNative82', 'RepairNative84')
source = source.replace('5b143bae30c9747fa7beab7d92c580b8c556523857fd35b0e4bb1dab0164ddae',
                        'd1727135f7fcb7141a62ad5bbe0e2342e6d25efb97d25b59ae223bb26fcd9975')
source = source.replace('Pinned untested Native81 parent', 'Pinned untested Native83 parent')
source = source.replace('metadata_unchanged_by_construction=true', 'metadata_scope="Existing table rows except target MethodDef RVA remain unchanged by construction; no new rows or heap edits"')
a = source.index(' class IL {')
b = source.index(' record Stream', a)
source = source[:a] + template81[template81.index(' class IL {'):template81.index(' record Stream')] + source[b:]
insert = source.index(' static void CopyTo(')
append = template81[template81.index(' static (byte[] Image,object Evidence) AppendBody('):template81.index(' static void CopyTo(')]
source = source[:insert] + append.replace('Only oversize no-EH Project body is relocated', 'Only complete oversize no-EH target bodies are relocated') + source[insert:]
a = source.index('  var m=M(0x0600087D);')
b = source.index('File.WriteAllBytes(args[1]', a)
source = source[:a] + r'''  var m=M(0x060001C8);R(m.HasThis&&m.Parameters.Count==0&&m.Body.ExceptionHandlers.Count==0,"Pinned DropDownAll signature");
  var il=new IL().Arg(0).Ref(0x7B,F("MouseManager","handItemType"))
   .Switch("none","card","return","return","skill","shovel","glove").Branch(0x38,"return")
   .Label("none").Arg(0).Ref(0x7B,F("MouseManager","board")).O(0x25).Ref(0x7B,F("Board","onDetailPage")).Branch(0x39,"pop-return")
   .Int(1).Int(1).Ref(0x6F,M(0x060002BA)).Branch(0x38,"return")
   .Label("pop-return").O(0x26).Branch(0x38,"return")
   .Label("card").Arg(0).Ref(0x28,M(0x060001CA)).Branch(0x38,"return")
   .Label("skill").Arg(0).Ref(0x7B,F("MouseManager","plantSkillCrosshairs")).Ref(0x28,0x0A00000B).Branch(0x39,"detail-reset")
   .Arg(0).Ref(0x7B,F("MouseManager","skillOnHandOwnerPlant")).Ref(0x7B,F("Plant","attackRangeUIController")).Ref(0x6F,M(0x060005BE))
   .Arg(0).Ref(0x7B,F("MouseManager","plantSkillCrosshairs")).Ref(0x28,0x0A000025)
   .Arg(0).Ref(0x7B,F("MouseManager","board")).Ref(0x6F,M(0x060002B8))
   .Label("detail-reset").Arg(0).Ref(0x7B,F("MouseManager","board")).O(0x25).Ref(0x7B,F("Board","onDetailPage")).Branch(0x39,"pop-reset")
   .Int(1).Int(1).Ref(0x6F,M(0x060002BA)).Branch(0x38,"reset")
   .Label("pop-reset").O(0x26).Branch(0x38,"reset")
   .Label("shovel").Arg(0).Ref(0x7B,F("MouseManager","propOnHand")).O(0x14).Ref(0x28,0x0A00000C).Branch(0x39,"reset")
   .Arg(0).Ref(0x7B,F("MouseManager","propOnHand")).Ref(0x6F,M(0x06000401)).Branch(0x38,"reset")
   .Label("glove").Arg(0).Ref(0x7B,F("MouseManager","propOnHand")).Ref(0x28,0x0A00000B).Branch(0x39,"drop-glove")
   .Arg(0).Ref(0x7B,F("MouseManager","propOnHand")).Ref(0x6F,M(0x06000401))
   .Label("drop-glove").Arg(0).Ref(0x28,M(0x060001C9))
   .Label("reset").Arg(0).Ref(0x28,M(0x060001E1))
   .Label("return").O(0x2A);
  Set(0x060001C8,il,3);
  m=M(0x060002CE);R(m.Body.Variables[3].VariableType.FullName=="System.Boolean"&&m.Body.Variables[9].VariableType.FullName=="UnityEngine.AudioClip"&&m.Body.Variables[10].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[13].VariableType.FullName=="System.Single","Pinned typed pause capture locals");
  R(StringToken(m,"GamePause")==0x70009EE2,"Exact existing native literal");
  il=new IL().Arg(0).Ref(0x7B,F("Board","isFailed")).Branch(0x39,"pause-gates")
   .Ref(0x28,M(0x06000169)).Ref(0x6F,M(0x0600016E))
   .Label("pause-gates").Arg(0).Ref(0x7B,F("Board","pauseTemp")).Branch(0x3A,"return")
   .Arg(0).Ref(0x7B,F("Board","menuOpen")).Branch(0x3A,"return")
   .Arg(0).Ref(0x7B,F("Board","gamePause")).S(3)
   .Arg(0).Int(1).Ref(0x7D,F("Board","pauseTemp"))
   .L(3).Branch(0x3A,"continue")
   .Ref(0x7E,F("ResourceManager","boardClips")).Int(11).Ref(0x6F,0x0A000084).S(9)
   .Ref(0x28,0x0A000004).S(6).L(6).Ref(0x6F,0x0A000001).S(7).L(7).Ref(0x6F,0x0A000037).S(10)
   .Ref(0x28,M(0x06000138)).S(13)
   .L(9).L(10).L(13).Ref(0x28,M(0x0600013D)).O(0x26)
   .Arg(0).Ref(0x7B,F("Board","pause")).Int(1).Ref(0x6F,0x0A000007)
   .O(0x72).I(StringToken(m,"GamePause")).Ref(0x28,0x0A000010)
   .Arg(0).Int(1).Ref(0x7D,F("Board","gamePause"))
   .Float(0f).Ref(0x28,0x0A0000F2).Branch(0x38,"return")
   .Label("continue").Arg(0).Ref(0x28,M(0x060002BD))
   .Label("return").O(0x2A);
  Set(0x060002CE,il,3);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"No EH target");
   if(code.Length>m.Body.CodeSize){var relocated=AppendBody(after,m,code,max);after=relocated.Image;report.Add(new{token=$"0x{token:X8}",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code),relocation=relocated.Evidence});continue;}
   var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;
   R((before[h]&3)==3&&(BitConverter.ToUInt16(before,h)>>12)==3,"Pinned fat header");Array.Clear(after,h+12,m.Body.CodeSize);code.CopyTo(after,h+12);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+12,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  R(targets.Count==2,"Pinned scope 2");''' + source[b:]
a = source.index(',scope="') + 1
b = source.index('"},new JsonSerializerOptions', a)
source = source[:a] + 'scope="Two complete original-native-backed callers; DropDownAll dispatches once and retains distinct Unity predicates, callback field rereads, static Destroy(null), captured board/Fold receiver, and ordinary-tail Reset; Space_down continues after BoardResume, captures pause state before pauseTemp write, captures clip/position before volume, uses exact three-argument audio and ordered partial writes; legal proven BoardManager getter; no new metadata rows, heap edits or local signature changes; complete oversize DropDownAll appended with target-only MethodDef RVA update; no tests executed; unqualified parent chain pending' + source[b:]
(p/'Patcher84.cs').write_text(source)
remote = '''from pathlib import Path
import subprocess,base64
w=Path('/workspaces/GodsPVZ-native24-codespace')
n=Path('/tmp/godspvz-native84-codex-2026-10-13');n.mkdir(exist_ok=True)
s=n/'Patcher';s.mkdir(exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(PATCHER_B64))
(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True)
(n/'build3.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'bin/Patcher.dll'),'/tmp/godspvz-native83-codex-2026-10-13/candidate1.dll',str(n/'candidate3.dll'),str(n/'generation3.json')],capture_output=True,text=True)
(n/'generation3.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
print((n/'generation3.json').read_text())
'''.replace('PATCHER_B64', repr(base64.b64encode(source.encode()).decode()))
(p/'generate_remote.py').write_text(remote)
