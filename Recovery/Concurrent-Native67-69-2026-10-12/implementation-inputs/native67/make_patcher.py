from pathlib import Path
import base64

p=Path('audit/native67-codex-2026-10-12')
s=Path('audit/native64-codex-2026-10-12/Patcher64.cs').read_text().replace('RepairNative64','RepairNative67')
s=s.replace('231d279f29cdda2343baef191e9d7a093691671bc3bfa4cd67a74ee88892596a','b87bd9b8670f5cc41fa3a138de156dac1aeeb8699b4aaa4cc67a7d99cda2bb3c').replace('Pinned untested Native63 parent','Pinned untested Native66 parent')
a=s.index('  var getEnum=');b=s.index('  ulong valid=',a)
s=s[:a]+'''  var memberRows=new List<byte>();var localRows=new List<byte>();var added=new List<object>();
  int localFirst=md.GetTableRowCount(TableIndex.StandAloneSig)+1,memberFirst=md.GetTableRowCount(TableIndex.MemberRef)+1;
  int Member(string name,int parent,byte[] signature){int rid=memberFirst+memberRows.Count/6,ni=Name(name),si=Blob(signature);memberRows.AddRange(Words(parent,ni,si));added.Add(new{kind="MemberRef",token=$"0x{0x0A000000+rid:X8}",name,parent,name_index=ni,signature_index=si});return 0x0A000000+rid;}
  Member("Remove",TS("System.Collections.Generic.List`1<Save>")*8+4,new byte[]{0x20,1,2,0x13,0});
  Member("get_Count",TS("System.Collections.Generic.List`1<Save>")*8+4,new byte[]{0x20,0,8});
  Member("Invoke",TS("System.Action`1<FTRuntime.SwfClipController>")*8+4,new byte[]{0x20,1,1,0x13,0});
  var target=(MD)module.LookupToken(0x0600023C);
  var oldSig=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)target.Body.LocalVarToken.RID)).Signature);
  R(oldSig.Length>2&&oldSig[0]==7&&oldSig[1]==13&&target.Body.Variables.Count==13,"Pinned existing 13-local save method");
  var newSig=new byte[]{7,14}.Concat(oldSig[2..]).Concat(new byte[]{0x0E}).ToArray();
  int newSignature=Blob(newSig);localRows.AddRange(Words(newSignature));
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",signature_index=newSignature,old_signature_hex=Convert.ToHexString(oldSig),signature_hex=Convert.ToHexString(newSig),appended_local="System.String"});
  R(memberRows.Count==18&&localRows.Count==2,"Exactly three public member bindings and one local signature");
  R(md.GetTableRowCount(TableIndex.TypeSpec)<8192&&md.GetTableRowCount(TableIndex.TypeRef)<8192&&md.GetTableRowCount(TableIndex.TypeDef)<8192&&md.GetTableRowCount(TableIndex.MethodDef)<8192,"No parent index widening");
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRows.ToArray()},{TableIndex.StandAloneSig,localRows.ToArray()}};
'''+s[b:]
s=s.replace('|(1UL<<(int)TableIndex.TypeSpec)','')
a=s.index('  MD M(int token)');b=s.index('  R(report.Count==4',a)
s=s[:a]+'''  MD M(int token)=>(MD)module.LookupToken(token);
  FR F(string owner,string name)=>module.GetTypes().Single(t=>t.FullName==owner).Fields.Single(f=>f.Name==name);
  MR C(MD method,string name)=>method.Body.Instructions.Select(i=>i.Operand).OfType<MR>().First(r=>r.Name==name);
  int localFirst=pe.GetMetadataReader().GetTableRowCount(TableIndex.StandAloneSig)+1;
  var targets=new Dictionary<int,(byte[] code,int max)>();var report=new List<object>();void Set(int token,IL il,int max){targets.Add(token,(il.Done(),max));}
  var m=M(0x0600023C);
  R(m.Body.Variables[3].VariableType.FullName=="Save"&&m.Body.Variables[7].VariableType.FullName=="System.Collections.Generic.List`1<Save>","Pinned save and collection scratch types");
  var il=new IL().Arg(0).Ref(0x7B,F("SavesManager","saveList")).Ref(0x7B,F("SaveList","saveList")).Arg(1).Ref(0x6F,C(m,"get_Item")).S(3)
   .Arg(0).Ref(0x7B,F("SavesManager","saveList")).Ref(0x7B,F("SaveList","saveList")).L(3).Ref(0x6F,first).O(0x26)
   .Arg(0).Ref(0x7B,F("SavesManager","playerSave")).L(3).Branch(0x40,"save")
   .Arg(0).Ref(0x7B,F("SavesManager","saveList")).Ref(0x7B,F("SaveList","saveList")).S(7)
   .L(7).Ref(0x6F,first+1).Int(0).Branch(0x3D,"has-save")
   .Arg(0).Ref(0x28,C(m,"TryCreateNewPlayerSave")).Branch(0x38,"load-base")
   .Label("has-save").Arg(0).L(7).Int(0).Ref(0x6F,C(m,"get_Item")).Ref(0x7D,F("SavesManager","playerSave"))
   .Arg(0).Ref(0x7B,F("SavesManager","playerSave")).Ref(0x7B,F("Save","playerName")).S(13)
   .Arg(0).Ref(0x7B,F("SavesManager","saveList")).L(13).Ref(0x7D,F("SaveList","defaultPlayerName"))
   .Label("load-base").Arg(0).Ref(0x28,C(m,"LoadBaseData"))
   .Label("save").Arg(0).Ref(0x28,C(m,"SavePlayerSave")).O(0x2A);
  Set(0x0600023C,il,3);
  m=M(0x06000877);
  var loop=F("FTRuntime.SwfClipController","_loopMode");
  R(m.Body.Variables[8].VariableType.FullName==loop.FieldType.FullName&&m.Body.Variables[1].VariableType.FullName=="System.Boolean"&&m.Body.Variables[6].VariableType.FullName=="System.Object","Existing enum/boolean/boxed scratch");
  var literal=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Ldstr&&i.Operand is string text&&text=="SwfClipController. Incorrect loop mode: {0}");
  var sec0=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);
  int h0=sec0.PointerToRawData+m.RVA-sec0.VirtualAddress,hs0=(BitConverter.ToUInt16(before,h0)>>12)*4;
  int stringToken=BitConverter.ToInt32(before,h0+hs0+literal.Offset+1);
  R(before[h0+hs0+literal.Offset]==0x72&&(uint)stringToken>>24==0x70,"Reuse exact existing string token");
  il=new IL().Arg(0).Ref(0x28,C(m,"NextClipFrame")).Branch(0x3A,"return")
   .Arg(0).Ref(0x7B,loop).S(8).L(8).Int(0).Branch(0x40,"nonzero")
   .Arg(0).Ref(0x7B,F("FTRuntime.SwfClipController","_isPlaying")).Branch(0x39,"return")
   .Arg(0).Ref(0x7B,F("FTRuntime.SwfClipController","OnStopPlayingEvent")).O(0x14,0xFE,0x01).S(1)
   .Arg(0).Int(0).Ref(0x7D,F("FTRuntime.SwfClipController","_isPlaying"))
   .Arg(0).Float(0f).Ref(0x7D,F("FTRuntime.SwfClipController","_tickTimer"))
   .L(1).Branch(0x3A,"return")
   .Arg(0).Ref(0x7B,F("FTRuntime.SwfClipController","OnStopPlayingEvent")).Arg(0).Ref(0x6F,first+2).Branch(0x38,"return")
   .Label("nonzero").L(8).Int(1).Branch(0x40,"invalid")
   .Arg(0).Ref(0x28,C(m,"Rewind")).Branch(0x38,"return")
   .Label("invalid").Arg(0).Ref(0x28,C(m,"get_loopMode")).Ref(0x8C,loop.FieldType).S(6)
   .O(0x72).I(stringToken).L(6).Ref(0x28,C(m,"Format")).Ref(0x73,C(m,".ctor")).O(0x7A)
   .Label("return").O(0x2A);
  Set(0x06000877,il,2);
  foreach(var(token,(code,max))in targets){
   m=M(token);R(m.Body.ExceptionHandlers.Count==0,"EH relocation unsupported");var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress,hs=(BitConverter.ToUInt16(before,h)>>12)*4;
   R((before[h]&3)==3&&hs==12,"Pinned fat body");R(code.Length<=m.Body.CodeSize,"Body exceeds reserved slot "+m.FullName);
   Array.Clear(after,h+hs,m.Body.CodeSize);code.CopyTo(after,h+hs);BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);
   int oldLocal=m.Body.LocalVarToken.ToInt32(),newLocal=token==0x0600023C?0x11000000+localFirst:oldLocal;if(newLocal!=oldLocal)BitConverter.GetBytes(newLocal).CopyTo(after,h+8);
   report.Add(new{token=$"0x{token:X8}",method=m.FullName,header=h,code_start=h+hs,reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=max,old_local_sig=oldLocal,local_sig=newLocal,code_sha256=H(code)});
  }
'''+s[b:]
s=s.replace('R(report.Count==4,"Pinned scope 4")','R(targets.Count==2,"Pinned scope 2")')
s=s.replace('Four native foreach reconstructions; ten public iterator bindings, three closed TypeSpecs and three one-local signatures appended; finally bodies within original slots; parent chain pending','Two native-backed bodies; public List<Save> Remove/Count and Action<SwfClipController> Invoke; one appended string scratch signature, original rows unchanged; unqualified parent chain pending')
(p/'Patcher67.cs').write_text(s)
encoded=base64.b64encode(s.encode()).decode()
remote='''from pathlib import Path
import subprocess,base64
w=Path('/workspaces/GodsPVZ-native24-codespace')
n=Path('/tmp/godspvz-native67-codex-2026-10-12');n.mkdir(exist_ok=True)
s=w/'scripts/takeover/native67/Patcher';s.mkdir(parents=True,exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(SOURCE))
(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
(s/'.gitignore').write_text('obj/\\nbin/\\n')
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
def produce(command,label):
    r=subprocess.run(command,capture_output=True,text=True)
    (n/(label+'.log')).write_text(r.stdout+r.stderr)
    if r.returncode:raise RuntimeError(label+' '+r.stdout+r.stderr)
    return r.stdout
produce(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'patcher-bin'),'-p:MonoCecilPath='+str(c)],'patcher-build')
print(produce(['dotnet',str(n/'patcher-bin/Patcher.dll'),'/tmp/godspvz-pending-restored-2026-10-12/snapshot/candidates/native66/candidate1.dll',str(n/'candidate1.dll'),str(n/'patch1.json')],'patcher-generate'))
print((n/'patch1.json').read_text())
'''.replace('SOURCE',repr(encoded))
(p/'generate_remote.py').write_text(remote)
