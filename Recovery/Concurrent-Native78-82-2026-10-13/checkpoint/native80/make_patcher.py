from pathlib import Path
import base64,json
p=Path('audit/native80-codex-2026-10-13')
s=Path('audit/native79-codex-2026-10-13/Patcher79.cs').read_text().replace('RepairNative79','RepairNative80')
s=s.replace('de2f98d4e793b91fba17ba7eff3aeba3130282f5e477f0daae1aa358287868f7','3b3bcfca51402e1eedf5d59fbe1bf9d923336730325550205f960bd8a8bceec6').replace('Pinned untested Native78 parent','Pinned untested Native79 parent')
s=s.replace('List<(int p,string l,int width)> branches=new();','List<(int p,string l,int width)> branches=new();List<(int p,int end,string l)> switches=new();')
s=s.replace('  public byte[] Done(){','  public IL Switch(params string[] names){O(0x45).I(names.Length);int end=B.Count+4*names.Length;foreach(var name in names){switches.Add((B.Count,end,name));I(0);}return this;}\n  public byte[] Done(){foreach(var(p,end,l)in switches)BitConverter.GetBytes(labels[l]-end).CopyTo(B,p);')
a=s.index('  var quaternion=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  var method=(MD)module.LookupToken(0x060003A0);
  byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==15&&method.Body.Variables.Count==15&&method.Body.Variables[10].VariableType.FullName=="System.Collections.Generic.List`1/Enumerator<System.Object>"&&method.Body.Variables[12].VariableType.FullName=="System.Collections.Generic.List`1/Enumerator<System.Object>","Pinned damaged brightness local slots");
  var enumType=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Void System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>::Dispose()").DeclaringType;
  int enumRid=TS(enumType.FullName);byte[] enumSig=md.GetBlobBytes(md.GetTypeSpecification(MetadataTokens.TypeSpecificationHandle(enumRid)).Signature);
  R(enumSig[0]==0x15&&enumSig[1]==0x11,"Value-type closed enumerator TypeSpec");
  var enumeratorRef=module.GetTypeReferences().Single(t=>t.FullName=="System.Collections.Generic.List`1/Enumerator");
  byte[] damagedEnum=new byte[]{0x15,0x11}.Concat(Compress(checked((int)enumeratorRef.MetadataToken.RID*4+1))).Concat(new byte[]{1,0x1C}).ToArray();
  var positions=Enumerable.Range(2,old.Length-damagedEnum.Length-1).Where(i=>old[i..(i+damagedEnum.Length)].SequenceEqual(damagedEnum)).ToArray();R(positions.Length==2,"Exact two damaged Enumerator<object> encodings");
  byte[] sig=old[..positions[0]].Concat(enumSig).Concat(old[(positions[0]+damagedEnum.Length)..]).ToArray();int ix=Blob(sig);localRows.AddRange(Words(ix));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",target="0x060003A0",active_local=10,signature_index=ix,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig)});
  R(memberRows.Count==0&&localRows.Count==2,"Only one scoped brightness enumerator local signature");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.StandAloneSig,localRows.ToArray()}};
'''+s[b:]
s=s.replace('clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig)','clear=1UL<<(int)TableIndex.StandAloneSig')
a=s.index('  int StringToken(');b=s.index('  foreach(var(token,(code,max))in targets)',a)
s=s[:a]+'''  var random=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="System.Single UnityEngine.Random::Range(System.Single,System.Single)");
  var m=M(0x06000206);
  R(m.Body.Variables[0].VariableType.FullName=="System.Single"&&m.Body.Variables[1].VariableType.FullName=="ProjectType"&&m.Body.Variables[8].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[9].VariableType.FullName=="UnityEngine.AudioClip"&&m.Body.Variables[17].VariableType.FullName=="System.Single","Existing audio capture scratch");
  var il=new IL().Ref(0x28,C(m,"AudioVolume")).S(0).Float(1f).S(17).Int(0).S(1)
   .Arg(1).Ref(0x7B,F("Project","projectType")).Switch("sun","coin","coin","common","money","common","common","common","coin").Branch(0x2B,"common")
   .Label("sun").Float(0.9f).Float(1.1f).Ref(0x28,random).S(17).Int(4).S(1).Branch(0x2B,"common")
   .Label("coin").L(0).Float(0.8f).O(0x5A,0x6B).S(0)
   .Label("money").Float(1f).Float(1.8f).Ref(0x28,random).S(17).Int(1).S(1)
   .Label("common").Ref(0x28,C(m,"get_main")).Ref(0x6F,C(m,"get_transform")).Ref(0x6F,C(m,"get_position")).S(8)
   .Ref(0x7E,F("ResourceManager","projectClips")).L(1).Ref(0x6F,C(m,"get_Item")).S(9)
   .L(9).L(8).L(0).L(17).Ref(0x28,C(m,"CreateAudioAtPoint")).O(0x26,0x2A);
  Set(0x06000206,il,4);
'''+s[b:]
s=s.replace('token==0x06000365?0x11000000+localFirst:oldLocal','oldLocal')
a=s.index('  R(targets.Count==3,');b=s.index('File.WriteAllBytes(args[1]',a)
s=s[:a]+'''  m=M(0x060003A0);R(m.Body.ExceptionHandlers.Count==0,"Pinned damaged brightness no-EH body");
  int Binding(string name){string owner=name=="GetEnumerator"?"System.Collections.Generic.List`1<UnityEngine.GameObject>":"System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>";return module.GetMemberReferences().OfType<MR>().Single(r=>r.Name==name&&r.DeclaringType.FullName==owner&&r.Parameters.Count==0).MetadataToken.ToInt32();}
  var material=module.GetMemberReferences().OfType<MR>().Single(r=>r.FullName=="UnityEngine.Material UnityEngine.Renderer::get_material()");
  il=new IL().Arg(0).Ref(0x7B,F("Plant","animationSprites")).Ref(0x6F,Binding("GetEnumerator")).S(10);int tryStart=il.B.Count;
  il.Branch(0x2B,"check").Label("body").A(10).Ref(0x28,Binding("get_Current")).Ref(0x6F,C(m,"GetComponent"))
   .Ref(0x6F,material).Ref(0x7E,F("Plant","Mat_Brightness")).Arg(1).Ref(0x6F,C(m,"SetFloat"))
   .Label("check").A(10).Ref(0x28,Binding("MoveNext")).Branch(0x2D,"body").Branch(0xDE,"return");int finallyStart=il.B.Count;
  il.A(10).Ref(0x28,Binding("Dispose")).O(0xDC);int finallyEnd=il.B.Count;
  il.Label("return").O(0x2A);byte[] enumCode=il.Done();
  var section=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int header=section.PointerToRawData+m.RVA-section.VirtualAddress;
  R((before[header]&3)==3&&(BitConverter.ToUInt16(before,header)>>12)==3,"Pinned fat brightness header");int eh=Align(header+12+enumCode.Length,4),extent=eh+28-(header+12);
  R(extent<=m.Body.CodeSize,"Brightness body/finally fits reserved slot");Array.Clear(after,header+12,m.Body.CodeSize);enumCode.CopyTo(after,header+12);
  BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(before,header)|8)).CopyTo(after,header);BitConverter.GetBytes((ushort)3).CopyTo(after,header+2);BitConverter.GetBytes(enumCode.Length).CopyTo(after,header+4);int local=0x11000000+localFirst;BitConverter.GetBytes(local).CopyTo(after,header+8);
  after[eh]=0x41;after[eh+1]=28;foreach(var(offset,value)in new[]{(4,2),(8,tryStart),(12,finallyStart-tryStart),(16,finallyStart),(20,finallyEnd-finallyStart),(24,0)})BitConverter.GetBytes(value).CopyTo(after,eh+offset);
  report.Add(new{token="0x060003A0",method=m.FullName,header,code_start=header+12,reserved_code_size=m.Body.CodeSize,new_code_size=enumCode.Length,old_max=m.Body.MaxStackSize,new_max=3,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(enumCode),new_finally=new{section_offset=eh,try_start=tryStart,try_length=finallyStart-tryStart,handler_start=finallyStart,handler_length=finallyEnd-finallyStart}});
  R(report.Count==2,"Pinned scope 2");'''+s[b:]
old_scope=s[s.index('scope="')+7:s.index('"},new JsonSerializerOptions',s.index('scope="'))]
s=s.replace(old_scope,'Two original-native-backed bodies; typed GameObject foreach with finally, per-iteration static brightness name and raw-null faults, no brightness assignment or clamp; public Renderer.get_material original native pointer aliases private GetMaterial but full reference persistent branch explicitly requires target-runtime qualification; original audio float dispatch and order/captured arguments; only one scoped enumerator local signature added, no MemberRef additions; no tests executed; unqualified parent chain pending')
(p/'Patcher80.cs').write_text(s)
t=Path('audit/native79-codex-2026-10-13/generate_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native79-codex-2026-10-13','godspvz-native80-codex-2026-10-13').replace("'/tmp/godspvz-native78-codex-2026-10-13/candidate1.dll'","'/tmp/godspvz-native79-codex-2026-10-13/candidate1.dll'")
(p/'generate_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text());(p/'native-methods.json').write_text(json.dumps([x for x in d['methods']if x['token']in('0x060003A0','0x06000206')],indent=2))
print('Prepared two-method patcher including scoped enumerator finally')
