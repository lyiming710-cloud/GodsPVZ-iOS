from pathlib import Path
import base64
p=Path('audit/native68-codex-2026-10-12')
s=Path('audit/native64-codex-2026-10-12/Patcher64.cs').read_text().replace('RepairNative64','RepairNative68')
s=s.replace('231d279f29cdda2343baef191e9d7a093691671bc3bfa4cd67a74ee88892596a','5c4cde6f71ee7f59c7cbfb721ae0c7f3096e4697f2cbf9d2d43dd347b01a4a25').replace('Pinned untested Native63 parent','Pinned untested Native67 parent')
a=s.index('  for(int k=0;k<3;k++)');b=s.index('  ulong valid=',a)
s=s[:a]+'''  for(int k=0;k<3;k++){
   string name=new[]{"Element","PlantSave","Card_Choose"}[k];var element=module.GetTypes().Single(t=>t.FullName==name);
   byte[] concrete=new byte[]{0x15,0x11}.Concat(enumType).Concat(new byte[]{1,0x12}).Concat(Compress(checked((int)element.MetadataToken.RID*4))).ToArray();
   int enumRid;
   if(k<2){
    int signature=Blob(concrete);enumRid=typeFirst+k;typeRows.AddRange(Words(signature));
    added.Add(new{kind="TypeSpec",token=$"0x{0x1B000000+enumRid:X8}",element=name,signature_hex=Convert.ToHexString(concrete),signature_index=signature});
   }else{
    var existing=Enumerable.Range(1,md.GetTableRowCount(TableIndex.TypeSpec)).Select(i=>module.LookupToken(0x1B000000+i)).OfType<GenericInstanceType>().Where(t=>t.FullName=="System.Collections.Generic.List`1/Enumerator<Card_Choose>"&&t.IsValueType).OrderByDescending(t=>t.MetadataToken.RID).First();
    enumRid=checked((int)existing.MetadataToken.RID);added.Add(new{kind="ReusedTypeSpec",token=$"0x{0x1B000000+enumRid:X8}",element=name});
   }
   byte[] current=new byte[]{0x12}.Concat(Compress(checked((int)element.MetadataToken.RID*4))).ToArray();
   byte[] trailing=k<2?current:new byte[]{8};
   int localSignature=Blob(new byte[]{7,3}.Concat(concrete).Concat(current).Concat(trailing).ToArray());
   localRows.AddRange(Words(localSignature));added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst+k:X8}",signature_index=localSignature});
   if(k==2){
    int reloadSignature=Blob(new byte[]{7,2}.Concat(concrete).Concat(current).ToArray());localRows.AddRange(Words(reloadSignature));
    added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst+3:X8}",signature_index=reloadSignature});
   }else{
    int parent=enumRid*8+4;
    Member("GetEnumerator",TS("System.Collections.Generic.List`1<"+name+">")*8+4,getEnumSig);
    Member("MoveNext",parent,new byte[]{0x20,0,2});Member("Dispose",parent,new byte[]{0x20,0,1});Member("get_Current",parent,new byte[]{0x20,0,0x13,0});
   }
  }
  R(typeRows.Count==4&&localRows.Count==8&&memberRows.Count==48,"Exactly two iterator TypeSpecs, four local signatures and eight new public bindings");
  R(typeFirst+1<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()},{TableIndex.TypeSpec,typeRows.ToArray()}};
'''+s[b:]
a=s.index('  var plans=new[]{');b=s.index('   var sec=pe.PEHeaders',a)
s=s[:a]+'''  int CardBinding(string name){
   string owner=name=="GetEnumerator"?"System.Collections.Generic.List`1<Card_Choose>":"System.Collections.Generic.List`1/Enumerator<Card_Choose>";
   var member=module.GetMemberReferences().OfType<MR>().Where(r=>r.Name==name&&r.DeclaringType.FullName==owner&&r.HasThis&&r.Parameters.Count==0).OrderByDescending(r=>r.MetadataToken.RID).First();
   return member.MetadataToken.ToInt32();
  }
  var plans=new[]{
   (token:0x06000110,kind:0,local:0,owner:"Damage",field:"elements",element:"Element",compare:"type"),
   (token:0x060004C1,kind:1,local:1,owner:"Save",field:"plantSaves",element:"PlantSave",compare:"ID"),
   (token:0x060006BF,kind:2,local:2,owner:"SeedChooserScreen",field:"card_ChooseList",element:"Card_Choose",compare:"isChosen"),
   (token:0x06000683,kind:2,local:3,owner:"PrepareUIController",field:"card_ChooseList",element:"Card_Choose",compare:"plantID")
  };
  foreach(var plan in plans){
   var m=M(plan.token);R(m.HasThis&&m.Body.ExceptionHandlers.Count==0,"Pinned damaged no-EH target");
   var il=new IL();if(plan.kind<2)il.O(0x14).S(2);else if(plan.token==0x060006BF)il.Int(0).S(2);
   int getEnum=plan.kind<2?first+plan.kind*4:CardBinding("GetEnumerator"),move=plan.kind<2?first+plan.kind*4+1:CardBinding("MoveNext"),dispose=plan.kind<2?first+plan.kind*4+2:CardBinding("Dispose"),current=plan.kind<2?first+plan.kind*4+3:CardBinding("get_Current");
   il.Arg(0).Ref(0x7B,F(plan.owner,plan.field)).Ref(0x6F,getEnum).S(0);
   int tryStart=il.B.Count;
   il.Label("loop").A(0).Ref(0x28,move).Branch(0x2C,"leave").A(0).Ref(0x28,current).S(1);
   if(plan.kind<2){
    il.L(1).Ref(0x7B,F(plan.element,plan.compare)).Arg(1).Branch(0x33,"loop").L(1).S(2).Branch(0xDE,"return");
   }else if(plan.token==0x060006BF){
    il.L(1).Ref(0x7B,F(plan.element,plan.compare)).Branch(0x2C,"loop").L(2).Int(1).O(0x58).S(2).Branch(0x2B,"loop");
   }else{
    il.L(1).Ref(0x7B,F(plan.element,plan.compare)).Arg(1).Branch(0x33,"loop")
     .L(1).Arg(0).Ref(0x7B,F("PrepareUIController","playerSave")).Ref(0x6F,C(m,"ReloadPlantSave")).Branch(0x2B,"loop");
   }
   il.Label("leave").Branch(0xDE,"return");int finallyStart=il.B.Count;
   il.A(0).Ref(0x28,dispose).O(0xDC);int finallyEnd=il.B.Count;
   il.Label("return");if(plan.local<3)il.L(2);il.O(0x2A);byte[] code=il.Done();
'''+s[b:]
s=s.replace('int local=0x11000000+localFirst+plan.kind','int local=0x11000000+localFirst+plan.local')
s=s.replace('Four native foreach reconstructions; ten public iterator bindings, three closed TypeSpecs and three one-local signatures appended; finally bodies within original slots; parent chain pending','Four native foreach bodies, two early-return lookups, chosen count and matching-ID reload; eight public bindings, two iterator TypeSpecs and four local signatures appended; existing Card_Choose bindings reused; unqualified parent chain pending')
(p/'Patcher68.cs').write_text(s)
t=Path('audit/native67-codex-2026-10-12/generate_remote.py').read_text()
start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native67-codex','godspvz-native68-codex').replace('scripts/takeover/native67/Patcher','scripts/takeover/native68/Patcher')
t=t.replace('/tmp/godspvz-pending-restored-2026-10-12/snapshot/candidates/native66/candidate1.dll','/tmp/godspvz-native67-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
