from pathlib import Path
import base64
p=Path('audit/native72-codex-2026-10-12')
s=Path('audit/native70-codex-2026-10-12/Patcher70.cs').read_text().replace('RepairNative70','RepairNative72')
s=s.replace('67611b6286fe37c58f706eaab114037bc1bce4a10187a3f28e8327e519e28359','e2cbc546cf479f12076d27661aabecf77acf00f27c02eca3f939983eb2794a8e').replace('Pinned untested Native69 parent','Pinned untested Native71 parent')
a=s.index('  int popup=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  var localRows=new List<byte>();int localFirst=md.GetTableRowCount(TableIndex.StandAloneSig)+1;
  Member("Add",TS("System.Collections.Generic.List`1<Zombie_charred>")*8+4,new byte[]{0x20,1,1,0x13,0});
  var field=module.GetType("ResourceManager").Fields.Single(f=>f.Name=="zombie_Charreds");var list=(GenericInstanceType)field.FieldType;var element=list.GenericArguments.Single();
  R(list.ElementType.MetadataToken.TokenType==TokenType.TypeRef&&element.MetadataToken.TokenType==TokenType.TypeDef,"Existing list/class signature references");
  byte[] concrete=new byte[]{0x15,0x12}.Concat(Compress(checked((int)list.ElementType.MetadataToken.RID*4+1))).Concat(new byte[]{1,0x12}).Concat(Compress(checked((int)element.MetadataToken.RID*4))).ToArray();
  var target=(MD)module.LookupToken(0x0600021A);var old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)target.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==10&&target.Body.Variables.Count==10,"Pinned ten-local resource method");
  byte[] signature=new byte[]{7,11}.Concat(old[2..]).Concat(concrete).ToArray();int sig=Blob(signature);localRows.AddRange(Words(sig));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",signature_index=sig,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(signature),appended_local=list.FullName});
  R(memberRows.Count==6&&localRows.Count==2,"One public Add and one appended typed-list scratch signature");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
'''+s[b:]
s=s.replace('clear=(1UL<<(int)TableIndex.MemberRef)', 'clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig)')
a=s.index('  var m=M(0x060002AE)');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  int StringToken(MD method,string text){
   var literal=method.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&i.Operand is string value&&value==text);
   var section=pe.PEHeaders.SectionHeaders.Single(x=>method.RVA>=x.VirtualAddress&&method.RVA<x.VirtualAddress+x.SizeOfRawData);int h=section.PointerToRawData+method.RVA-section.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   int token=BitConverter.ToInt32(before,h+hs+literal.Offset+1);R(before[h+hs+literal.Offset]==0x72&&(uint)token>>24==0x70,"Existing exact string token");return token;
  }
  var m=M(0x0600013A);R(!m.HasThis,"Static sprite replacement signature");
  var equality=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)");
  var il=new IL().Arg(0).Arg(1).Ref(0x28,C(m,"GetAnimationSprite_Name")).S(3)
   .L(3).O(0x14).Ref(0x28,equality).Branch(0x2D,"false")
   .L(3).Ref(0x6F,C(m,"GetComponent")).S(8).L(8).Ref(0x28,C(m,"op_Implicit")).Branch(0x2C,"false")
   .L(8).Arg(2).Ref(0x6F,C(m,"set_sprite")).Int(1).O(0x2A).Label("false").Int(0).O(0x2A);
  Set(0x0600013A,il,2);
  m=M(0x06000544);
  MR Transform(string owner)=>m.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name=="get_transform"&&r.DeclaringType.FullName==owner);
  var componentTransform=Transform("UnityEngine.Component");var gameTransform=Transform("UnityEngine.GameObject");
  var vector=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
  il=new IL().Arg(0).Ref(0x7B,F("AlmanacUIController","crossbarExpand")).S(0)
   .Arg(0).L(0).Int(0).O(0xFE,0x01).Ref(0x7D,F("AlmanacUIController","crossbarExpand"))
   .Arg(0).Ref(0x7B,F("AlmanacUIController","image_CrossbarController")).Ref(0x6F,componentTransform).S(2)
   .L(0).Branch(0x3A,"expanded")
   .L(2).Float(-1f).Float(1f).Float(1f).Ref(0x73,vector).Ref(0x6F,C(m,"set_localScale")).Branch(0x38,"collapsed-position")
   .Label("expanded").L(2).Float(1f).Float(1f).Float(1f).Ref(0x73,vector).Ref(0x6F,C(m,"set_localScale"))
   .Arg(0).Ref(0x7B,F("AlmanacUIController","crossbar")).Ref(0x6F,gameTransform).Float(-1435f).Branch(0x38,"position-yz")
   .Label("collapsed-position").Arg(0).Ref(0x7B,F("AlmanacUIController","crossbar")).Ref(0x6F,gameTransform).Float(-715f)
   .Label("position-yz").Float(-435f).Float(0f).Ref(0x73,vector).Ref(0x6F,C(m,"set_localPosition")).O(0x2A);
  Set(0x06000544,il,4);
  m=M(0x0600021A);
  R(m.Body.Variables[0].VariableType.FullName=="System.String"&&m.Body.Variables[3].VariableType.FullName=="Zombie_charred","Existing path/result scratch");
  il=new IL().O(0x72).I(StringToken(m,"prefabs")).O(0x72).I(StringToken(m,"Charred")).Ref(0x28,C(m,"Combine")).S(0)
   .Ref(0x7E,F("ResourceManager","zombie_Charreds")).S(10)
   .L(0).O(0x72).I(StringToken(m,"Zombie_charred")).Ref(0x28,C(m,"Combine")).Ref(0x28,C(m,"Load")).S(3)
   .L(10).L(3).Ref(0x6F,first).O(0x2A);
  Set(0x0600021A,il,2);
'''+s[b:]
s=s.replace('report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});', 'int oldSig=m.Body.LocalVarToken.ToInt32(),newSig=token==0x0600021A?0x11000000+localFirst:oldSig;if(newSig!=oldSig)BitConverter.GetBytes(newSig).CopyTo(after,h+8);report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,old_local_sig=oldSig,local_sig=newSig,code_sha256=H(code)});')
a=s.index('  m=M(0x060001FA)');b=s.index('  R(report.Count==4',a)
s=s[:a]+'''  m=M(0x060006BC);R(m.Body.ExceptionHandlers.Count==0,"Pinned no-EH seed method");
  int CardBinding(string name){string owner=name=="GetEnumerator"?"System.Collections.Generic.List`1<Card_Choose>":"System.Collections.Generic.List`1/Enumerator<Card_Choose>";return module.GetMemberReferences().OfType<MR>().Where(r=>r.Name==name&&r.DeclaringType.FullName==owner&&r.HasThis&&r.Parameters.Count==0).OrderByDescending(r=>r.MetadataToken.RID).First().MetadataToken.ToInt32();}
  var reusable=M(0x06000683);R(reusable.Body.Variables.Count==2&&reusable.Body.Variables[0].VariableType.FullName=="System.Collections.Generic.List`1/Enumerator<Card_Choose>"&&reusable.Body.Variables[1].VariableType.FullName=="Card_Choose","Reuse Native68 enum/current signature");
  il=new IL().Arg(0).Arg(1).Ref(0x28,C(m,"SetPrechooseList"))
   .Arg(0).Ref(0x7B,F("SeedChooserScreen","card_ChooseList")).Ref(0x6F,CardBinding("GetEnumerator")).S(0);
  int tryStart=il.B.Count;
  il.Label("loop").A(0).Ref(0x28,CardBinding("MoveNext")).Branch(0x2C,"leave").A(0).Ref(0x28,CardBinding("get_Current")).S(1)
   .L(1).Ref(0x7B,F("Card_Choose","plantID")).Arg(2).Branch(0x33,"loop")
   .Arg(0).Ref(0x7B,F("SeedChooserScreen","plantDetail")).L(1).Ref(0x6F,C(m,"Awake")).Branch(0x2B,"loop")
   .Label("leave").Branch(0xDE,"return");int finallyStart=il.B.Count;
  il.A(0).Ref(0x28,CardBinding("Dispose")).O(0xDC);int finallyEnd=il.B.Count;
  il.Label("return").O(0x2A);byte[] enumCode=il.Done();
  var section=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int header=section.PointerToRawData+m.RVA-section.VirtualAddress;
  R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Existing fat iterator header");int ehRelative=Align(12+enumCode.Length,4),extent=ehRelative+28-12;
  R(extent<=m.Body.CodeSize,"Iterator and finally fit original slot");Array.Clear(after,header+12,m.Body.CodeSize);enumCode.CopyTo(after,header+12);
  BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(before,header)|0x18)).CopyTo(after,header);BitConverter.GetBytes((ushort)2).CopyTo(after,header+2);BitConverter.GetBytes(enumCode.Length).CopyTo(after,header+4);
  int local=reusable.Body.LocalVarToken.ToInt32();BitConverter.GetBytes(local).CopyTo(after,header+8);int eh=header+ehRelative;new byte[]{0x41,28,0,0}.CopyTo(after,eh);
  foreach(var(offset,value)in new[]{(4,2),(8,tryStart),(12,finallyStart-tryStart),(16,finallyStart),(20,finallyEnd-finallyStart),(24,0)})BitConverter.GetBytes(value).CopyTo(after,eh+offset);
  report.Add(new{token="0x060006BC",method=m.FullName,header,code_start=header+12,reserved_code_size=m.Body.CodeSize,new_code_size=enumCode.Length,old_max=m.Body.MaxStackSize,new_max=2,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(enumCode),new_finally=new{section_offset=eh,try_start=tryStart,try_length=finallyStart-tryStart,handler_start=finallyStart,handler_length=finallyEnd-finallyStart}});
'''+s[b:]
s=s.replace('Four native-backed bodies; project-ID filtered foreach with finally, popup queue handling, chomper classification and bonfire scale; two public Popup bindings appended; existing enum/local signatures and Vector3 constructor reused; scalar native constants and NaN guards retained; unqualified parent chain pending','Four native-backed bodies; sprite replacement null predicate, captured-state almanac vectors, seed prechoose iteration/finally and typed charred resource Add; one public binding and one appended local signature; existing iterator bindings reused; native strings/coordinates preserved; unqualified parent chain pending')
(p/'Patcher72.cs').write_text(s)
t=Path('audit/native71-codex-2026-10-12/generate_remote.py').read_text();start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native71-codex','godspvz-native72-codex').replace('scripts/takeover/native71/Patcher','scripts/takeover/native72/Patcher')
t=t.replace('/tmp/godspvz-native70-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native71-codex-2026-10-12/candidate1.dll')
(p/'generate_remote.py').write_text(t)
