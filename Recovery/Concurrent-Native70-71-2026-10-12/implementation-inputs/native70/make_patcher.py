from pathlib import Path
import base64
p=Path('audit/native70-codex-2026-10-12')
s=Path('audit/native69-codex-2026-10-12/Patcher69.cs').read_text().replace('RepairNative69','RepairNative70')
s=s.replace('58e44b184e89ee3d587b7c7c8d9f57bf58740b96e9c8c71303414aac59cad7a0','67611b6286fe37c58f706eaab114037bc1bce4a10187a3f28e8327e519e28359').replace('Pinned untested Native68 parent','Pinned untested Native69 parent')
s=s.replace('public IL L(int i)', 'public IL Double(double x)=>O(0x23).O(BitConverter.GetBytes(x));\n  public IL L(int i)')
a=s.index('  var memberRows=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  var memberRows=new List<byte>();var added=new List<object>();int memberFirst=md.GetTableRowCount(TableIndex.MemberRef)+1;
  int Member(string name,int parent,byte[] signature){int rid=memberFirst+memberRows.Count/6,ni=Name(name),si=Blob(signature);memberRows.AddRange(Words(parent,ni,si));added.Add(new{kind="MemberRef",token=$"0x{0x0A000000+rid:X8}",name,parent,name_index=ni,signature_index=si});return 0x0A000000+rid;}
  int popup=TS("System.Collections.Generic.List`1<Popup>")*8+4;
  Member("get_Count",popup,new byte[]{0x20,0,8});Member("RemoveAt",popup,new byte[]{0x20,1,1,8});
  R(memberRows.Count==12,"Exactly two public popup bindings");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()}};
'''+s[b:]
s=s.replace('clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig)', 'clear=(1UL<<(int)TableIndex.MemberRef)')
a=s.index('  var m=M(0x06000463)');b=s.index('  R(targets.Count==4',a)
s=s[:a]+'''  var m=M(0x060002AE);
  R(m.Body.Variables[0].VariableType.FullName=="System.Collections.Generic.List`1<Popup>"&&m.Body.Variables[1].VariableType.FullName=="System.Boolean","Existing popup/bool scratch");
  var il=new IL().Arg(0).Ref(0x7B,F("Board","popups")).S(0).L(0).Ref(0x6F,first).Int(0).Branch(0x3E,"return")
   .L(0).Int(0).Ref(0x6F,C(m,"get_Item")).O(0x14).Ref(0x28,C(m,"op_Equality")).S(1)
   .Arg(0).Ref(0x7B,F("Board","popups")).S(0).L(1).Branch(0x3A,"remove")
   .L(0).Int(0).Ref(0x6F,C(m,"get_Item")).Ref(0x6F,C(m,"get_gameObject")).Ref(0x6F,C(m,"get_activeSelf")).Branch(0x3A,"return")
   .Arg(0).Ref(0x7B,F("Board","popups")).S(0).Branch(0x38,"activate")
   .Label("remove").L(0).Int(0).Ref(0x6F,first+1)
   .Arg(0).Ref(0x7B,F("Board","popups")).S(0).L(0).Ref(0x6F,first).Int(0).Branch(0x3E,"return")
   .Label("activate").L(0).Int(0).Ref(0x6F,C(m,"get_Item")).Ref(0x6F,C(m,"get_gameObject")).Int(1).Ref(0x6F,C(m,"SetActive"))
   .Label("return").O(0x2A);
  Set(0x060002AE,il,2);
  m=M(0x060003AA);
  il=new IL().Arg(3).Ref(0x7B,F("Zombie","armor1Type")).Branch(0x3A,"normal")
   .Arg(3).Ref(0x7B,F("Zombie","armor2Type")).Branch(0x3A,"normal")
   .Arg(4).Ref(0x7B,F("Damage","damagePoint")).Float(7f).O(0x5A,0x6B).Arg(3).Ref(0x7B,F("Zombie","healthPoint")).Branch(0x43,"normal")
   .Arg(2).Arg(3).Ref(0x6F,C(m,"Add")).O(0x2A)
   .Label("normal").Arg(1).Arg(3).Ref(0x6F,C(m,"Add")).O(0x2A);
  Set(0x060003AA,il,2);
  m=M(0x06000318);
  R(m.Body.Variables[2].VariableType.FullName=="System.Single"&&m.Body.Variables[4].VariableType.FullName=="System.Single","Existing health/scale scratch");
  var vector=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
  il=new IL().Arg(0).Ref(0x7B,F("Device","healthPoint")).S(2).L(2).Float(0f).Branch(0x43,"return")
   .Arg(0).L(2).Ref(0x28,C(m,"get_deltaTime")).Float(10f).O(0x5A,0x6B,0x59,0x6B).Ref(0x7D,F("Device","healthPoint"))
   .Arg(0).Ref(0x28,C(m,"DC_BonfireWarm"))
   .Arg(0).Ref(0x7B,F("Device","healthPoint")).Float(0f).Branch(0x42,"scale")
   .Arg(0).Float(0f).Ref(0x7D,F("Device","healthPoint")).Arg(0).Ref(0x28,C(m,"Broken"))
   .Label("scale").Arg(0).Ref(0x7B,F("Device","healthPoint")).Arg(0).Ref(0x7B,F("Device","maxHealthPoint")).O(0x5B,0x6B,0x6C).Double(0.5d).Ref(0x28,C(m,"Pow")).O(0x6B).S(4)
   .Arg(0).Ref(0x7B,F("Device","animationGroup")).Ref(0x6F,C(m,"get_transform")).Int(1).Ref(0x6F,C(m,"GetChild"))
   .L(4).L(4).Float(0f).Ref(0x73,vector).Ref(0x6F,C(m,"set_localScale"))
   .Label("return").O(0x2A);
  Set(0x06000318,il,4);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});
  }
  m=M(0x060001FA);R(m.Body.ExceptionHandlers.Count==0,"Pinned damaged no-EH project iterator");
  int ProjectBinding(string name){string owner=name=="GetEnumerator"?"System.Collections.Generic.List`1<Project>":"System.Collections.Generic.List`1/Enumerator<Project>";return module.GetMemberReferences().OfType<MR>().Where(r=>r.Name==name&&r.DeclaringType.FullName==owner&&r.HasThis&&r.Parameters.Count==0).OrderByDescending(r=>r.MetadataToken.RID).First().MetadataToken.ToInt32();}
  var reusable=M(0x06000261);R(reusable.Body.Variables.Count==2&&reusable.Body.Variables[0].VariableType.FullName=="System.Collections.Generic.List`1/Enumerator<Project>"&&reusable.Body.Variables[1].VariableType.FullName=="Project","Reuse typed enum/current signature from Native66");
  il=new IL().Arg(0).Ref(0x7B,F("ProjectManager","projects")).Ref(0x6F,ProjectBinding("GetEnumerator")).S(0);
  int tryStart=il.B.Count;
  il.Label("loop").A(0).Ref(0x28,ProjectBinding("MoveNext")).Branch(0x2C,"leave").A(0).Ref(0x28,ProjectBinding("get_Current")).S(1)
   .L(1).Ref(0x7B,F("Project","ID")).Int(1).Branch(0x2E,"qualifying-id")
   .L(1).Ref(0x7B,F("Project","ID")).Int(2).Branch(0x2E,"qualifying-id")
   .L(1).Ref(0x7B,F("Project","ID")).Int(8).Branch(0x33,"loop")
   .Label("qualifying-id").L(1).Ref(0x7B,F("Project","movementTracks")).Int(5).Branch(0x33,"loop")
   .L(1).Ref(0x7B,F("Project","stayedTime")).Float(0.4f).Branch(0x37,"loop")
   .L(1).Arg(0).Ref(0x7B,F("ProjectManager","board")).Ref(0x7B,F("Board","mouseManager")).Ref(0x6F,C(m,"Click")).Branch(0x2B,"loop")
   .Label("leave").Branch(0xDE,"return");int finallyStart=il.B.Count;
  il.A(0).Ref(0x28,ProjectBinding("Dispose")).O(0xDC);int finallyEnd=il.B.Count;
  il.Label("return").O(0x2A);byte[] enumCode=il.Done();
  var section=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int header=section.PointerToRawData+m.RVA-section.VirtualAddress;
  R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Existing fat iterator header");int ehRelative=Align(12+enumCode.Length,4),extent=ehRelative+28-12;
  R(extent<=m.Body.CodeSize,"Project iterator body and finally fit reserved slot");Array.Clear(after,header+12,m.Body.CodeSize);enumCode.CopyTo(after,header+12);
  BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(before,header)|0x18)).CopyTo(after,header);BitConverter.GetBytes((ushort)2).CopyTo(after,header+2);BitConverter.GetBytes(enumCode.Length).CopyTo(after,header+4);
  int local=reusable.Body.LocalVarToken.ToInt32();BitConverter.GetBytes(local).CopyTo(after,header+8);int eh=header+ehRelative;new byte[]{0x41,28,0,0}.CopyTo(after,eh);
  foreach(var(offset,value)in new[]{(4,2),(8,tryStart),(12,finallyStart-tryStart),(16,finallyStart),(20,finallyEnd-finallyStart),(24,0)})BitConverter.GetBytes(value).CopyTo(after,eh+offset);
  report.Add(new{token="0x060001FA",method=m.FullName,header,code_start=header+12,reserved_code_size=m.Body.CodeSize,new_code_size=enumCode.Length,old_max=m.Body.MaxStackSize,new_max=2,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(enumCode),new_finally=new{section_offset=eh,try_start=tryStart,try_length=finallyStart-tryStart,handler_start=finallyStart,handler_length=finallyEnd-finallyStart}});
'''+s[b:]
s=s.replace('R(targets.Count==4,"Pinned scope 4")','R(report.Count==4,"Pinned scope 4")')
s=s.replace('Four native-backed bodies; arrived-prefix insert, editor deletion, win audio and skill level update; four public collection bindings, one captured EnemyPath scratch signature; original Color constructor reused with native RGBA bits; unqualified parent chain pending','Four native-backed bodies; project-ID filtered foreach with finally, popup queue handling, chomper classification and bonfire scale; two public Popup bindings appended; existing enum/local signatures and Vector3 constructor reused; scalar native constants and NaN guards retained; unqualified parent chain pending')
(p/'Patcher70.cs').write_text(s)
t=Path('audit/native69-codex-2026-10-12/generate_remote.py').read_text();start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native69-codex','godspvz-native70-codex').replace('scripts/takeover/native69/Patcher','scripts/takeover/native70/Patcher')
t=t.replace('/tmp/godspvz-native68-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native69-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
