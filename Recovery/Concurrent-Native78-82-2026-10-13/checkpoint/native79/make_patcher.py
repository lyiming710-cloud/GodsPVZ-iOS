from pathlib import Path
import base64,json
p=Path('audit/native79-codex-2026-10-13')
s=Path('audit/native76-codex-2026-10-12/Patcher76.cs').read_text().replace('RepairNative76','RepairNative79')
s=s.replace('c2e0f13211a981b0fa9158cef09cc468e051ca2822ad7017f3450c87f5add725','de2f98d4e793b91fba17ba7eff3aeba3130282f5e477f0daae1aa358287868f7').replace('Pinned untested Native75 parent','Pinned untested Native78 parent')
s=s.replace('throw new Exception("Existing public member name required: "+name);','int ix=strings.Length;strings=strings.Concat(raw).ToArray();R(strings.Length<65536,"No string heap width growth");return ix;')
a=s.index('  Member("get_Count",');b=s.index('  ulong valid=',a)
s=s[:a]+'''  var quaternion=module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.Quaternion");
  var vector=module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.Vector3");
  Member("get_eulerAngles",checked((int)quaternion.MetadataToken.RID*8+1),new byte[]{0x20,0,0x11}.Concat(Compress(checked((int)vector.MetadataToken.RID*4+1))).ToArray());
  var method=(MD)module.LookupToken(0x06000365);
  byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==11&&method.Body.Variables.Count==11,"Pinned original Explode local count");
  byte[] sig=new byte[]{7,12}.Concat(old[2..]).Concat(new byte[]{0x0C}).ToArray();int ix=Blob(sig);localRows.AddRange(Words(ix));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",target="0x06000365",signature_index=ix,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig)});
  R(memberRows.Count==6&&localRows.Count==2,"One public quaternion getter and one appended Single scratch signature");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
'''+s[b:]
s=s.replace('streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);','streams[streams.FindIndex(s=>s.Name=="#Blob")]=new("#Blob",blob);streams[streams.FindIndex(s=>s.Name=="#Strings")]=new("#Strings",strings);')
a=s.index('  MD PureGetter('); b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  int StringToken(MD method,string text){var i=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&(string)i.Operand==text);var sec=pe.PEHeaders.SectionHeaders.Single(s=>method.RVA>=s.VirtualAddress&&method.RVA<s.VirtualAddress+s.SizeOfRawData);int header=sec.PointerToRawData+method.RVA-sec.VirtualAddress;R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat literal body");return BitConverter.ToInt32(before,header+12+i.Offset+1);}
  MD PureGetter(int token,string owner,string field){var getter=M(token);var gi=getter.Body.Instructions.ToArray();R(getter.IsPublic&&getter.IsStatic&&getter.Parameters.Count==0&&gi.Length==3&&gi[0].OpCode.FlowControl==FlowControl.Branch&&gi[0].Operand==gi[1]&&gi[1].OpCode==OpCodes.Ldsfld&&gi[1].Operand is FR gf&&gf.FullName==F(owner,field).FullName&&gi[2].OpCode==OpCodes.Ret,"Exact unchanged field-only static getter");return getter;}
  var mainInstance=PureGetter(0x06000636,"MainUIController","<instance>k__BackingField");
  var particles=PureGetter(0x060001E5,"ParticlesManager","<instance>k__BackingField");
  var vector=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
  var color=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Color::.ctor(System.Single,System.Single,System.Single,System.Single)");
  var add=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1<Save>::Add(!0)");
  var m=M(0x06000238);
  R(m.Body.Variables[1].VariableType.FullName=="Save","Existing captured new Save local");
  var il=new IL().Ref(0x73,C(m,".ctor")).S(1)
   .Arg(0).L(1).Ref(0x28,C(m,"InitializeSave"))
   .L(1).Arg(0).Ref(0x7B,F("SavesManager","playerSave")).Ref(0x6F,C(m,"Copy"))
   .L(1).Arg(0).Ref(0x7B,F("SavesManager","playerSave")).Ref(0x7B,F("Save","playerName"))
   .O(0x72).I(StringToken(m,"-备份")).Ref(0x28,C(m,"Concat")).Ref(0x7D,F("Save","playerName"))
   .Arg(0).Ref(0x7B,F("SavesManager","saveList")).Ref(0x7B,F("SaveList","saveList")).L(1).Ref(0x6F,add)
   .Arg(0).Ref(0x28,C(m,"SavePlayerSave")).Ref(0x28,mainInstance).Ref(0x6F,C(m,"LoadData")).O(0x2A);
  Set(0x06000238,il,3);
  m=M(0x06000365);
  R(m.Body.Variables[3].VariableType.FullName=="UnityEngine.GameObject"&&m.Body.Variables[4].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[7].VariableType.FullName=="Skill","Existing captured explosion locals");
  il=new IL().Arg(0).Ref(0x7B,F("Plant","ID")).Int(2).Branch(0x40,"return")
   .Ref(0x28,particles).Int(2).Ref(0x6F,C(m,"CreatNewParticle")).S(3)
   .L(3).Ref(0x6F,C(m,"get_transform")).S(4)
   .Arg(0).Ref(0x7B,F("Plant","fZ")).Arg(0).Ref(0x7B,F("Plant","fY")).O(0x58,0x6B).S(11)
   .L(4).O(0x25).Branch(0x2D,"position-transform-live").O(0x26,0x14,0x7A)
   .Label("position-transform-live").Arg(0).Ref(0x7B,F("Plant","fX")).L(11).Float(0f).Ref(0x73,vector).Ref(0x6F,C(m,"set_position"))
   .L(3).Ref(0x6F,C(m,"get_transform")).O(0x25).Branch(0x2D,"scale-transform-live").O(0x26,0x14,0x7A)
   .Label("scale-transform-live").Float(1.2f).Float(1.2f).Float(1.2f).Ref(0x73,vector).Ref(0x6F,C(m,"set_localScale"))
   .Arg(0).Ref(0x7B,F("Plant","board")).Float(0.15f).Float(8f).Ref(0x6F,C(m,"Shake"))
   .Arg(0).O(0x14).Int(-1).Int(0).Ref(0x28,C(m,"GetDamage")).Ref(0x6F,C(m,"AreaDamage")).O(0x26)
   .Arg(0).Ref(0x7B,F("Plant","skill")).S(7)
   .L(7).Ref(0x7B,F("Skill","ID")).Int(1).Branch(0x33,"die")
   .L(7).Arg(0).Ref(0x6F,C(m,"Function"))
   .Label("die").Arg(0).Ref(0x28,C(m,"PlantDie"))
   .Label("return").O(0x2A);
  Set(0x06000365,il,4);
  m=M(0x0600075F);
  R(m.Body.Variables[1].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[2].VariableType.FullName=="UnityEngine.Quaternion"&&m.Body.Variables[3].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[8].VariableType.FullName=="UnityEngine.Light"&&m.Body.Variables[10].VariableType.FullName=="UnityEngine.Color","Existing typed spin scratch");
  il=new IL().Arg(0).Arg(0).Ref(0x6F,C(m,"get_transform")).Ref(0x7D,F("TMPro.Examples.ObjectSpin","m_transform"))
   .Arg(0).Ref(0x7B,F("TMPro.Examples.ObjectSpin","m_transform")).Ref(0x6F,C(m,"get_rotation")).S(2)
   .A(2).Ref(0x28,first).S(3)
   .Arg(0).Ref(0x7B,F("TMPro.Examples.ObjectSpin","m_transform")).S(1)
   .Arg(0).L(3).Ref(0x7D,F("TMPro.Examples.ObjectSpin","m_initial_Rotation"))
   .L(1).Ref(0x6F,C(m,"get_position")).S(7)
   .Arg(0).L(7).Ref(0x7D,F("TMPro.Examples.ObjectSpin","m_initial_Position"))
   .Arg(0).Ref(0x28,C(m,"GetComponent")).S(8)
   .L(8).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"fallback")
   .L(8).Ref(0x6F,C(m,"get_color")).S(10).Branch(0x2B,"color-ready")
   .Label("fallback").Float(0f).Float(0f).Float(0f).Float(1f).Ref(0x73,color).S(10)
   .Label("color-ready").Arg(0).L(10).Ref(0x28,C(m,"op_Implicit")).Ref(0x7D,F("TMPro.Examples.ObjectSpin","m_lightColor")).O(0x2A);
  Set(0x0600075F,il,4);
'''+s[b:]
s=s.replace('token==0x06000039?0x11000000+localFirst:token==0x06000703?0x11000000+localFirst+1:oldLocal','token==0x06000365?0x11000000+localFirst:oldLocal')
s=s.replace('R(targets.Count==4,"Pinned scope 4")','R(targets.Count==3,"Pinned scope 3")')
s=s.replace('Four native-backed bodies; captured editor data/image/board-config before transform callbacks, device deletion with conditional repath, settlement removal and conditional pass, window save deletion with correct Unity equality and captured index/list; five public collection bindings and two appended scratch signatures; public static getters asserted to be unchanged pure field reads; unqualified parent chain pending','Three original-native-backed bodies; Save allocation/ctor before Initialize, current source reread after Copy, typed public Add, original backup suffix and post-save MainUI lookup; explosion captured particle and two transform getters, fZ-first float32 y before transform null and x read, exact scale/shake, Damage Boolean discarded and skill captured after AreaDamage; spin uses proven public Quaternion.get_eulerAngles with typed quaternion address, writes computed rotation before second transform failure, captured Light and black alpha-one fallback converted through Color32; one appended public MemberRef and one scoped local signature; no tests executed; unqualified parent chain pending')
(p/'Patcher79.cs').write_text(s)
t=Path('audit/native78-codex-2026-10-13/generate_remote.py').read_text()
a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native78-codex-2026-10-13','godspvz-native79-codex-2026-10-13').replace('/tmp/godspvz-native77-restored-2026-10-13/snapshot/candidates/native77/candidate1.dll','/tmp/godspvz-native78-codex-2026-10-13/candidate1.dll')
(p/'generate_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text())
(p/'native-methods.json').write_text(json.dumps([x for x in d['methods'] if x['token']in('0x06000238','0x06000365','0x0600075F')],indent=2))
print('Prepared three-method patcher with scoped metadata extension')
