from pathlib import Path
import base64
p=Path('audit/native76-codex-2026-10-12')
s=Path('audit/native69-codex-2026-10-12/Patcher69.cs').read_text().replace('RepairNative69','RepairNative76')
s=s.replace('58e44b184e89ee3d587b7c7c8d9f57bf58740b96e9c8c71303414aac59cad7a0','c2e0f13211a981b0fa9158cef09cc468e051ca2822ad7017f3450c87f5add725').replace('Pinned untested Native68 parent','Pinned untested Native75 parent')
a=s.index('  int paths=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  Member("get_Count",TS("System.Collections.Generic.List`1<UnityEngine.Sprite>")*8+4,new byte[]{0x20,0,8});
  Member("Remove",TS("System.Collections.Generic.List`1<Device>")*8+4,new byte[]{0x20,1,2,0x13,0});
  Member("Remove",TS("System.Collections.Generic.List`1<SettlementUIController>")*8+4,new byte[]{0x20,1,2,0x13,0});
  Member("get_Count",TS("System.Collections.Generic.List`1<SettlementUIController>")*8+4,new byte[]{0x20,0,8});
  Member("get_Count",TS("System.Collections.Generic.List`1<SavesUIController_PlayerSave>")*8+4,new byte[]{0x20,0,8});
  foreach(var(token,count,scratch)in new[]{(0x06000039,12,new byte[]{8}),(0x06000703,14,new byte[]{0x12}.Concat(Compress(checked((int)module.GetType("SavesUIController").MetadataToken.RID*4))).Concat(new byte[]{8}).ToArray())}){
   var method=(MD)module.LookupToken(token);byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
   R(old[0]==7&&old[1]==count&&method.Body.Variables.Count==count,"Pinned original editor/window local counts");
   byte[] sig=new byte[]{7,(byte)(count+(token==0x06000039?1:2))}.Concat(old[2..]).Concat(scratch).ToArray();int ix=Blob(sig);localRows.AddRange(Words(ix));
   added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst+localRows.Count/2-1:X8}",target=$"0x{token:X8}",signature_index=ix,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig)});
  }
  R(memberRows.Count==30&&localRows.Count==4,"Five public collection bindings and two append-only scratch signatures");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
'''+s[b:]
s=s.replace('foreach(var(token,count,scratch)', 'foreach(var(token,localCount,scratch)').replace('old[1]==count&&method.Body.Variables.Count==count','old[1]==localCount&&method.Body.Variables.Count==localCount').replace('(byte)(count+(token==','(byte)(localCount+(token==')
a=s.index('  var m=M(0x06000463)');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  MD PureGetter(int token,string owner,string field){var method=M(token);var body=method.Body.Instructions.Where(i=>i.OpCode!=OpCodes.Nop&&i.OpCode.FlowControl!=FlowControl.Branch).ToArray();R(method.IsPublic&&method.IsStatic&&method.Parameters.Count==0&&body.Length==2&&body[0].OpCode==OpCodes.Ldsfld&&body[1].OpCode==OpCodes.Ret&&body[0].Operand is FR f&&f.FullName==F(owner,field).FullName,"Exact unchanged field-only static getter");return method;}
  var boardInstance=PureGetter(0x06000169,"BoardManager","<Instance>k__BackingField");var mainInstance=PureGetter(0x06000636,"MainUIController","<instance>k__BackingField");
  var eq=module.GetMemberReferences().OfType<MR>().First(r=>r.FullName=="System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)");
  var m=M(0x06000039);
  R(m.Body.Variables[2].VariableType.FullName=="DeviceData"&&m.Body.Variables[3].VariableType.FullName=="PreDevice"&&m.Body.Variables[5].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[6].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[7].VariableType.FullName=="System.Collections.Generic.List`1<UnityEngine.Sprite>","Existing editor capture scratch");
  var il=new IL().Arg(0).Ref(0x7B,F("Device_boardEditor","preDevice")).Ref(0x7B,F("PreDevice","data")).S(2)
   .Ref(0x7E,F("ResourceManager","card_Choose_DevicePortraits")).S(7)
   .L(7).O(0x25).Branch(0x2D,"list-live").O(0x26,0x14,0x7A).Label("list-live").O(0x26)
   .L(2).Ref(0x7B,F("DeviceData","ID")).S(12)
   .Arg(0).Ref(0x7B,F("Device_boardEditor","image"))
   .L(7).Ref(0x6F,first).L(12).Branch(0x30,"sprite").O(0x14).Branch(0x2B,"assign")
   .Label("sprite").L(7).L(12).Ref(0x6F,C(m,"get_Item"))
   .Label("assign").Ref(0x6F,C(m,"set_sprite"))
   .Arg(0).Ref(0x7B,F("Device_boardEditor","boardEdior")).Ref(0x7B,F("Admin_system2_boardEdior","boardConfig"))
   .Arg(0).Ref(0x6F,C(m,"get_transform")).S(5)
   .Arg(0).Ref(0x7B,F("Device_boardEditor","preDevice")).S(3)
   .L(3).Ref(0x7B,F("PreDevice","gridX")).L(3).Ref(0x7B,F("PreDevice","gridY")).Ref(0x6F,C(m,"GetGridCenterPosition")).S(6)
   .L(5).L(6).Ref(0x6F,C(m,"set_localPosition")).O(0x2A);
  Set(0x06000039,il,3);
  m=M(0x06000322);
  R(m.Body.Variables[3].VariableType.FullName=="Grid"&&m.Body.Variables[4].VariableType.FullName=="System.Int32"&&m.Body.Variables[8].VariableType.FullName=="System.Int32","Existing grid/point scratch");
  il=new IL().Arg(0).Ref(0x7B,F("Device","UI_HP")).O(0x14).Ref(0x28,C(m,"op_Inequality")).Branch(0x2C,"grid")
   .Arg(0).Ref(0x7B,F("Device","UI_HP")).Ref(0x28,C(m,"Destroy"))
   .Label("grid").Arg(0).Ref(0x7B,F("Device","board")).Arg(0).Ref(0x7B,F("Device","gridX")).Arg(0).Ref(0x7B,F("Device","gridY")).Ref(0x6F,C(m,"GetGrid")).S(3)
   .L(3).Ref(0x6F,C(m,"GetPassablePoint")).S(4)
   .L(3).Arg(0).Ref(0x6F,C(m,"RemoveDevice"))
   .L(3).Ref(0x6F,C(m,"GetPassablePoint")).S(8)
   .Arg(0).Ref(0x7B,F("Device","board")).Ref(0x7B,F("Board","deviceManager")).Ref(0x7B,F("DeviceManager","deviceList")).Arg(0).Ref(0x6F,first+1).O(0x26)
   .L(4).L(8).Branch(0x2E,"destroy")
   .Arg(0).Ref(0x7B,F("Device","board")).Ref(0x7B,F("Board","zombieManager")).Ref(0x6F,C(m,"AllFindPath"))
   .Label("destroy").Arg(0).Ref(0x6F,C(m,"get_gameObject")).Ref(0x28,C(m,"Destroy")).O(0x2A);
  Set(0x06000322,il,3);
  m=M(0x060006D4);
  il=new IL().Ref(0x7E,F("GlobalStaticVars","gLawnApp")).Ref(0x7B,F("GlobalStaticVars/LawnApp","challengeTypeOnPlay")).Int(0).Branch(0x33,"remove")
   .Ref(0x7E,F("GlobalStaticVars","gLawnApp")).O(0x25).Ref(0x7B,F("GlobalStaticVars/LawnApp","levelOnPlay")).Int(1).O(0x58).Ref(0x7D,F("GlobalStaticVars/LawnApp","levelOnPlay"))
   .Label("remove").Ref(0x28,boardInstance).Ref(0x7B,F("BoardManager","settlementUIControllers")).Arg(0).Ref(0x6F,first+2).O(0x26)
   .Ref(0x28,boardInstance).Ref(0x7B,F("BoardManager","settlementUIControllers")).Ref(0x6F,first+3).Int(0).Branch(0x30,"destroy")
   .Ref(0x28,boardInstance).Ref(0x6F,C(m,"Pass"))
   .Label("destroy").Arg(0).Ref(0x6F,C(m,"get_gameObject")).Ref(0x28,C(m,"Destroy")).O(0x2A);
  Set(0x060006D4,il,3);
  m=M(0x06000703);
  R(m.Body.Variables[6].VariableType.FullName=="System.Collections.Generic.List`1<SavesUIController_PlayerSave>"&&m.Body.Variables[8].VariableType.FullName=="SavesUIController_PlayerSave","Existing save-list/tile scratch");
  il=new IL().Arg(0).Ref(0x7B,F("Window_Q","info0_int")).Int(0).Branch(0x3F,"return")
   .Ref(0x28,mainInstance).Ref(0x7B,F("MainUIController","savesUIController")).S(14)
   .L(14).O(0x14).Ref(0x28,eq).Branch(0x3A,"return")
   .L(14).Ref(0x7B,F("SavesUIController","playerSaveList")).S(6)
   .Arg(0).Ref(0x7B,F("Window_Q","info0_int")).S(15)
   .L(6).Ref(0x6F,first+4).L(15).Branch(0x31,"return")
   .L(6).L(15).Ref(0x6F,C(m,"get_Item")).S(8)
   .L(8).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"destroy")
   .L(8).Ref(0x7B,F("SavesUIController_PlayerSave","savesManager")).Branch(0x2C,"destroy")
   .L(8).Ref(0x7B,F("SavesUIController_PlayerSave","savesManager")).L(8).Ref(0x7B,F("SavesUIController_PlayerSave","playerSaveID")).Ref(0x6F,C(m,"DeletePlayerSave_ID"))
   .Ref(0x28,mainInstance).Ref(0x6F,C(m,"LoadData"))
   .Label("destroy").Arg(0).Ref(0x6F,C(m,"get_gameObject")).Ref(0x28,C(m,"Destroy"))
   .Label("return").O(0x2A);
  Set(0x06000703,il,2);
'''+s[b:]
s=s.replace('int oldLocal=m.Body.LocalVarToken.ToInt32(),newLocal=token==0x06000055?0x11000000+localFirst:oldLocal;', 'int oldLocal=m.Body.LocalVarToken.ToInt32(),newLocal=token==0x06000039?0x11000000+localFirst:token==0x06000703?0x11000000+localFirst+1:oldLocal;')
a=s.index('scope="Four native-backed bodies;');b=s.index('"},new JsonSerializerOptions',a)
s=s[:a]+'scope="Four native-backed bodies; captured editor data/image/board-config before transform callbacks, device deletion with conditional repath, settlement removal and conditional pass, window save deletion with correct Unity equality and captured index/list; five public collection bindings and two appended scratch signatures; public static getters asserted to be unchanged pure field reads; unqualified parent chain pending'+s[b:]
(p/'Patcher76.cs').write_text(s)
t=Path('audit/native75-codex-2026-10-12/generate_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native75-codex','godspvz-native76-codex').replace('scripts/takeover/native75/Patcher','scripts/takeover/native76/Patcher').replace('/tmp/godspvz-native74-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native75-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
