from pathlib import Path
import base64,json
p=Path('audit/native81-codex-2026-10-13');p.mkdir(exist_ok=True)
s=Path('audit/native80-codex-2026-10-13/Patcher80.cs').read_text().replace('RepairNative80','RepairNative81')
s=s.replace('3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6','119c847000d84922e5089cfed3bb464366251aa05597f4c7fa22706368211950').replace('Pinned untested Native79 parent','Pinned untested Native80 parent')
a=s.index('  var method=(MD)module.LookupToken(0x060003A0);');b=s.index('  ulong valid=',a)
s=s[:a]+'''  var enumType=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1/Enumerator<ZombieSelect>::Dispose()").DeclaringType;
  int enumRid=TS(enumType.FullName),listRid=TS("System.Collections.Generic.List`1<ZombieSelect>");
  var enumeratorRef=module.GetTypeReferences().Single(t=>t.FullName=="System.Collections.Generic.List`1/Enumerator");
  byte[] openEnum=new byte[]{0x15,0x11}.Concat(Compress(checked((int)enumeratorRef.MetadataToken.RID*4+1))).Concat(new byte[]{1,0x13,0}).ToArray();
  Member("GetEnumerator",listRid*8+4,new byte[]{0x20,0}.Concat(openEnum).ToArray());
  Member("MoveNext",enumRid*8+4,new byte[]{0x20,0,2});
  Member("get_Current",enumRid*8+4,new byte[]{0x20,0,0x13,0});
  var method=(MD)module.LookupToken(0x060001AC);byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==16&&method.Body.Variables.Count==16,"Pinned damaged random-selector local count");
  byte[] enumSig=md.GetBlobBytes(md.GetTypeSpecification(MetadataTokens.TypeSpecificationHandle(enumRid)).Signature);R(enumSig[0]==0x15&&enumSig[1]==0x11,"Closed value-type ZombieSelect enumerator");
  byte[] sig=new byte[]{7,17}.Concat(old[2..]).Concat(enumSig).ToArray();int ix=Blob(sig);localRows.AddRange(Words(ix));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",target="0x060001AC",active_local=16,signature_index=ix,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig)});
  R(memberRows.Count==18&&localRows.Count==2,"Three typed public iterator bindings and one appended typed enumerator local");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
'''+s[b:]
s=s.replace('clear=1UL<<(int)TableIndex.StandAloneSig','clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig)')
insert=s.index(' static void CopyTo(')
s=s[:insert]+''' static (byte[] Image,object Evidence) AppendBody(byte[] current,MD method,byte[] code,int max){
  using var pe=new PEReader(new MemoryStream(current));var ph=pe.PEHeaders;var opt=ph.PEHeader!;var md=pe.GetMetadataReader();var last=ph.SectionHeaders.Last();
  R(opt.Magic==PEMagic.PE32&&last.Name==".n29meta"&&last.PointerToRawData+last.SizeOfRawData==current.Length&&last.VirtualSize<=last.SizeOfRawData&&last.SectionCharacteristics==(SectionCharacteristics)0x40000040&&opt.CertificateTableDirectory.Size==0&&ph.CorHeader!.StrongNameSignatureDirectory.Size==0&&(ph.CorHeader.Flags&CorFlags.StrongNameSigned)==0,"Pinned unsigned EOF body relocation layout");
  int rootRva=ph.CorHeader.MetadataDirectory.RelativeVirtualAddress,root=checked(last.PointerToRawData+rootRva-last.VirtualAddress),rootSize=ph.CorHeader.MetadataDirectory.Size;
  R(root>=last.PointerToRawData&&checked(root+rootSize)<=current.Length,"Active metadata in extensible last section");
  int row=checked(md.GetTableMetadataOffset(TableIndex.MethodDef)+(checked((int)method.MetadataToken.RID)-1)*md.GetTableRowSize(TableIndex.MethodDef));
  R(row>=0&&checked(row+4)<=rootSize&&BitConverter.ToInt32(current,root+row)==method.RVA,"Pinned target RVA in active metadata row");
  R(method.Body.ExceptionHandlers.Count==0&&code.Length>method.Body.CodeSize,"Only oversize no-EH Project body is relocated");
  int body=Align(current.Length,4),rva=checked(last.VirtualAddress+body-last.PointerToRawData),end=checked(body+12+code.Length);R((rva&3)==0,"Fat MethodBody RVA alignment");
  var after=new byte[Align(end,opt.FileAlignment)];current.CopyTo(after,0);
  ushort flags=(ushort)(0x3003|(method.Body.InitLocals?0x10:0));BitConverter.GetBytes(flags).CopyTo(after,body);BitConverter.GetBytes(checked((ushort)max)).CopyTo(after,body+2);BitConverter.GetBytes(code.Length).CopyTo(after,body+4);BitConverter.GetBytes(method.Body.LocalVarToken.ToInt32()).CopyTo(after,body+8);code.CopyTo(after,body+12);
  BitConverter.GetBytes(rva).CopyTo(after,root+row);
  int sh=ph.PEHeaderStartOffset+ph.CoffHeader.SizeOfOptionalHeader+(ph.CoffHeader.NumberOfSections-1)*40,virtualSize=checked(end-last.PointerToRawData);
  BitConverter.GetBytes(virtualSize).CopyTo(after,sh+8);BitConverter.GetBytes(after.Length-last.PointerToRawData).CopyTo(after,sh+16);BitConverter.GetBytes(checked(opt.SizeOfInitializedData+after.Length-current.Length)).CopyTo(after,ph.PEHeaderStartOffset+8);BitConverter.GetBytes(Align(checked(last.VirtualAddress+virtualSize),opt.SectionAlignment)).CopyTo(after,ph.PEHeaderStartOffset+56);new byte[4].CopyTo(after,ph.PEHeaderStartOffset+64);
  return(after,new{old_rva=method.RVA,new_rva=rva,body_offset=body,active_root_offset=root,active_root_size=rootSize,method_row_offset_in_root=row,method_row_rva_file_offset=root+row,old_slot_preserved_by_construction=true,old_image_bytes=current.Length,new_image_bytes=after.Length,old_virtual_size=last.VirtualSize,new_virtual_size=virtualSize,old_raw_size=last.SizeOfRawData,new_raw_size=after.Length-last.PointerToRawData,metadata_directory_unchanged_by_construction=true});
 }
'''+s[insert:]
a=s.index('  var random=');b=s.index('File.WriteAllBytes(args[1]',a)
s=s[:a]+'''  var random=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Single UnityEngine.Random::Range(System.Single,System.Single)");
  var m=M(0x060001AC);R(m.Body.ExceptionHandlers.Count==0&&m.HasThis&&m.Parameters.Count==2&&m.Body.Variables[0].VariableType.FullName=="System.Single"&&m.Body.Variables[1].VariableType.FullName=="System.Collections.Generic.List`1<ZombieSelect>"&&m.Body.Variables[5].VariableType.FullName=="ZombieSelect"&&m.Body.Variables[11].VariableType.FullName=="ZombieSelect","Pinned selector signature and captured locals");
  var dispose=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1/Enumerator<ZombieSelect>::Dispose()");
  var il=new IL().Arg(1).S(1).Float(0f).Arg(2).Ref(0x28,random).S(0).L(1).Ref(0x6F,first).S(16).O(0x14).S(11);int tryStart=il.B.Count;
  il.Label("loop").A(16).Ref(0x28,first+1).Branch(0x2C,"leave")
   .A(16).Ref(0x28,first+2).S(5).L(0).L(5).Ref(0x7B,F("ZombieSelect","weight")).Branch(0x30,"subtract")
   .L(5).S(11).Branch(0xDE,"return")
   .Label("subtract").L(0).L(5).Ref(0x7B,F("ZombieSelect","weight")).O(0x59,0x6B).S(0).Branch(0x2B,"loop")
   .Label("leave").Branch(0xDE,"return");int finallyStart=il.B.Count;
  il.A(16).Ref(0x28,dispose).O(0xDC);int finallyEnd=il.B.Count;il.Label("return").L(11).O(0x2A);byte[] enumCode=il.Done();
  var section=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int header=section.PointerToRawData+m.RVA-section.VirtualAddress;
  R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat selector header");int eh=Align(header+12+enumCode.Length,4),extent=eh+28-(header+12);
  R(extent<=m.Body.CodeSize,"Selector body/finally fits reserved slot");Array.Clear(after,header+12,m.Body.CodeSize);enumCode.CopyTo(after,header+12);
  BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(before,header)|8)).CopyTo(after,header);BitConverter.GetBytes((ushort)2).CopyTo(after,header+2);BitConverter.GetBytes(enumCode.Length).CopyTo(after,header+4);int local=0x11000000+localFirst;BitConverter.GetBytes(local).CopyTo(after,header+8);
  after[eh]=0x41;after[eh+1]=28;foreach(var(offset,value)in new[]{(4,2),(8,tryStart),(12,finallyStart-tryStart),(16,finallyStart),(20,finallyEnd-finallyStart),(24,0)})BitConverter.GetBytes(value).CopyTo(after,eh+offset);
  report.Add(new{token="0x060001AC",method=m.FullName,header,code_start=header+12,reserved_code_size=m.Body.CodeSize,new_code_size=enumCode.Length,old_max=m.Body.MaxStackSize,new_max=2,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(enumCode),new_finally=new{section_offset=eh,try_start=tryStart,try_length=finallyStart-tryStart,handler_start=finallyStart,handler_length=finallyEnd-finallyStart}});
  m=M(0x060003C5);R(m.Body.Variables[3].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[5].VariableType.FullName=="System.Single","Existing Project position/value scratch");
  var x=module.GetMemberReferences().OfType<FR>().Single(f=>f.FullName=="System.Single UnityEngine.Vector3::x");var y=module.GetMemberReferences().OfType<FR>().Single(f=>f.FullName=="System.Single UnityEngine.Vector3::y");
  var vector=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void UnityEngine.Vector3::.ctor(System.Single,System.Single,System.Single)");
  il=new IL().Arg(0).Ref(0x7B,F("Project","movementTracks")).Branch(0x2D,"check-four")
   .Arg(0).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position")).S(3)
   .Arg(0).Ref(0x7C,F("Project","endPosition")).A(3).Ref(0x7B,x).Ref(0x7D,x)
   .Float(-380f).Float(120f).Ref(0x28,random).S(5)
   .Arg(0).Ref(0x7C,F("Project","endPosition")).L(5).Ref(0x7D,y)
   .Arg(0).Arg(0).Ref(0x7B,F("Project","endPosition")).Ref(0x28,C(m,"PositionRevision_Camera")).Ref(0x7D,F("Project","endPosition"))
   .Label("check-four").Arg(0).Ref(0x7B,F("Project","movementTracks")).Int(4).Branch(0x40,"return")
   .Arg(0).Ref(0x7B,F("Project","ID")).Switch("sun","coin","coin","diamond","fixed","fixed","return","return","coin").Branch(0x38,"return")
   .Label("sun").Arg(0).Arg(0).Ref(0x7B,F("Project","board")).Ref(0x7B,F("Board","banks")).Ref(0x6F,M(0x060000E9)).Ref(0x7D,F("Project","endPosition"))
   .Arg(0).Ref(0x7C,F("Project","endPosition")).O(0x25).Ref(0x7B,y).Float(12f).O(0x58,0x6B).Ref(0x7D,y).O(0x2A)
   .Label("coin").Arg(0).Arg(0).Ref(0x7B,F("Project","board")).Ref(0x7B,F("Board","projectManager")).Ref(0x6F,M(0x06000204)).Branch(0x2B,"bank-ready")
   .Label("diamond").Arg(0).Arg(0).Ref(0x7B,F("Project","board")).Ref(0x7B,F("Board","projectManager")).Ref(0x6F,M(0x06000205))
   .Label("bank-ready").Ref(0x7D,F("Project","endPosition"))
   .Arg(0).Ref(0x7C,F("Project","endPosition")).O(0x25).Ref(0x7B,y).Float(5f).O(0x59,0x6B).Ref(0x7D,y)
   .Arg(0).Ref(0x7C,F("Project","endPosition")).O(0x25).Ref(0x7B,x).Float(85f).O(0x59,0x6B).Ref(0x7D,x).O(0x2A)
   .Label("fixed").Arg(0).Float(0f).Float(115f).Float(0f).Ref(0x73,vector).Ref(0x7D,F("Project","endPosition"))
   .Arg(0).Arg(0).Ref(0x7B,F("Project","endPosition")).Ref(0x28,C(m,"PositionRevision_Camera")).Ref(0x7D,F("Project","endPosition")).O(0x2A)
   .Label("return").O(0x2A);
  byte[] projectCode=il.Done();var relocated=AppendBody(after,m,projectCode,4);after=relocated.Image;
  report.Add(new{token="0x060003C5",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=projectCode.Length,old_max=m.Body.MaxStackSize,new_max=4,local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(projectCode),relocation=relocated.Evidence});
  R(report.Count==2,"Pinned scope 2");'''+s[b:]
old_scope=s[s.index('scope="')+7:s.index('"},new JsonSerializerOptions',s.index('scope="'))]
s=s.replace(old_scope,'Two original-native-backed bodies; weighted selector uses float Range before GetEnumerator, ordered greater-than with NaN selection, second weight read and real finally; three public typed iterator references and one appended scoped enum local; Project uses independent track0 and fresh track4 check, original nine-case ID switch, captured helper results and partial writes with exact float32 operand order; complete Project body exceeds old slot and is appended to existing readonly section, only its MethodDef RVA changes, old slot/local signature preserved; no tests executed; unqualified parent chain pending')
(p/'Patcher81.cs').write_text(s)
t=Path('audit/native80-codex-2026-10-13/generate_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native80-codex-2026-10-13','godspvz-native81-codex-2026-10-13').replace("'/tmp/godspvz-native79-codex-2026-10-13/candidate1.dll'","'/tmp/godspvz-native80-codex-2026-10-13/candidate1.dll'")
(p/'generate_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text());(p/'native-methods.json').write_text(json.dumps([x for x in d['methods']if x['token']in('0x060001AC','0x060003C5')],indent=2))
print('Prepared two-method patcher with iterator finally and scoped target relocation')
