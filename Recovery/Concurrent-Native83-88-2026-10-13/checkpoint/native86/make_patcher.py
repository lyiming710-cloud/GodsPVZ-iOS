from pathlib import Path
import base64

p = Path('audit/native86-codex-2026-10-13')
p.mkdir(exist_ok=True)
source = Path('audit/native85-codex-2026-10-13/Patcher85.cs').read_text()
source = source.replace('RepairNative85', 'RepairNative86')
source = source.replace('b3284d01f0a57c9d22f6297741e96a831874cb0a584af2e165b98c42def84587', 'ba1b6ae5cd5b27b89797c16484cab5a6eb52157deab0bd4251e5fefe1aa562b0')
source = source.replace('Pinned untested Native84 parent', 'Pinned untested Native85 parent')
a = source.index('  var added=new List<object>();')
b = source.index('  ulong valid=', a)
source = source[:a] + r'''  var added=new List<object>();
  R(module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.EventSystems.PointerEventData").MetadataToken.ToInt32()==0x01000066&&module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.Vector2").MetadataToken.ToInt32()==0x0100004B,"Pinned legal position getter types");
  int first=md.GetTableRowCount(TableIndex.MemberRef)+1,ni=Name("get_position"),si=Blob(new byte[]{0x20,0,0x11,0x81,0x2D});
  byte[] memberRow=Words(0x331,ni,si);R(memberRow.Length==6,"Pinned getter row width");
  added.Add(new{kind="MemberRef",token=$"0x{0x0A000000+first:X8}",name="get_position",declaring_type="0x01000066",parent=0x331,signature_hex="200011812D",signature_index=si,name_index=ni});
  var method=(MD)module.LookupToken(0x060006DF);byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==9&&method.Body.Variables.Count==9,"Pinned original text-link local count");
  byte[] sig=new byte[]{7,10}.Concat(old[2..]).Concat(new byte[]{0x11,0x81,0x2D}).ToArray();int li=Blob(sig),localFirst=md.GetTableRowCount(TableIndex.StandAloneSig)+1;
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+localFirst:X8}",target="0x060006DF",active_local=9,old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig),signature_index=li});
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.MemberRef,memberRow},{TableIndex.StandAloneSig,Words(li)}};
''' + source[b:]
source = source.replace('t==TableIndex.MethodSpec&&width==4', 'width==(t==TableIndex.MemberRef?6:2)')
source = source.replace('clear=1UL<<(int)TableIndex.MethodSpec', 'clear=(1UL<<(int)TableIndex.MemberRef)|(1UL<<(int)TableIndex.StandAloneSig)')
source = source.replace('methodspec_sorted_bit_cleared=(sorted&(1UL<<43))!=0', 'memberref_sorted_bit_cleared=(sorted&(1UL<<10))!=0')
source = source.replace('int initialZombie=0x2B000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MethodSpec)+1;', 'int positionGetter=0x0A000000+pe.GetMetadataReader().GetTableRowCount(TableIndex.MemberRef)+1;')
a = source.index('  var m=M(0x06000443);')
b = source.index('  foreach(var(token,(code,max))in targets)', a)
source = source[:a] + r'''  var m=M(0x060006DF);R(m.Body.Variables[0].VariableType.FullName=="UnityEngine.Camera"&&m.Body.Variables[1].VariableType.FullName=="System.Int32"&&m.Body.Variables[5].VariableType.FullName=="System.String"&&m.Body.Variables[8].VariableType.FullName=="TMPro.TMP_LinkInfo","Pinned typed link captures");
  R(StringToken(m,"jump0")==0x7001CCC2&&StringToken(m,"https://space.bilibili.com/525659346")==0x7001CCCE,"Original exact fixed link literals");
  var linkInfo=module.GetTypeReferences().Single(t=>t.FullName=="TMPro.TMP_LinkInfo");R(linkInfo.MetadataToken.ToInt32()==0x01000065,"Exact struct array element");
  var il=new IL().Arg(0).Ref(0x7B,F("TextLink","TMP_Text"))
   .Arg(1).Ref(0x6F,positionGetter).S(9).Ref(0x28,0x0A000004).S(0)
   .A(9).Ref(0x7B,0x0A0001D4).A(9).Ref(0x7B,0x0A0001D5).Float(0f).Ref(0x73,0x0A00009C).L(0).Ref(0x28,0x0A0002B5).S(1)
   .Arg(0).L(1).Ref(0x7D,F("TextLink","linkIndex")).L(1).Int(-1).Branch(0x3B,"return")
   .Arg(0).Ref(0x7B,F("TextLink","TMP_Text")).Ref(0x6F,0x0A0002B8).Ref(0x7B,0x0A0002B9).L(1).Ref(0xA3,linkInfo).S(8)
   .A(8).Ref(0x28,0x0A0002B6).S(5).L(5).Ref(0x28,0x0A000010)
   .L(5).O(0x72).I(StringToken(m,"jump0")).Ref(0x28,0x0A00003A).Branch(0x39,"return")
   .O(0x72).I(StringToken(m,"https://space.bilibili.com/525659346")).Ref(0x28,0x0A0002B7)
   .Label("return").O(0x2A);
  Set(0x060006DF,il,4);
  m=M(0x060006FE);R(m.Body.Variables[0].VariableType.FullName=="System.Int32","Existing window index/ID local");
  il=new IL().Arg(0).Ref(0x7B,F("Window_Q","Q")).Switch("return","return-main","resume","resume","delete","backup").Branch(0x38,"return")
   .Label("return-main").Arg(0).Ref(0x28,M(0x06000702)).Branch(0x38,"return")
   .Label("resume").Arg(0).Ref(0x28,M(0x06000701)).Branch(0x38,"return")
   .Label("delete").Arg(0).Ref(0x7B,F("Window_Q","info0_int")).Int(0).Branch(0x3F,"return")
   .Ref(0x28,M(0x06000636)).Ref(0x7B,F("MainUIController","savesUIController")).O(0x25,0x14).Ref(0x28,0x0A000012).Branch(0x3A,"pop-return")
   .Ref(0x7B,F("SavesUIController","playerSaveList")).Arg(0).Ref(0x7B,F("Window_Q","info0_int")).S(0)
   .O(0x25).Ref(0x6F,0x0A000443).L(0).Branch(0x3E,"pop-return")
   .L(0).Ref(0x6F,0x0A0002C7).O(0x25).Ref(0x28,0x0A00000B).Branch(0x39,"pop-destroy")
   .O(0x25).Ref(0x7B,F("SavesUIController_PlayerSave","savesManager")).Branch(0x39,"pop-destroy")
   .O(0x25).Ref(0x7B,F("SavesUIController_PlayerSave","playerSaveID")).S(0)
   .Ref(0x7B,F("SavesUIController_PlayerSave","savesManager")).L(0).Ref(0x6F,M(0x0600023C))
   .Ref(0x28,M(0x06000636)).Ref(0x6F,M(0x0600063A)).Branch(0x38,"destroy")
   .Label("backup").Ref(0x28,M(0x06000636)).Ref(0x7B,F("MainUIController","savesManager")).O(0x25).Branch(0x39,"pop-return")
   .Ref(0x6F,M(0x06000238)).Branch(0x38,"destroy")
   .Label("pop-return").O(0x26).Branch(0x38,"return")
   .Label("pop-destroy").O(0x26)
   .Label("destroy").Arg(0).Ref(0x28,0x0A000009).Ref(0x28,0x0A000025)
   .Label("return").O(0x2A);
  Set(0x060006FE,il,3);
''' + source[b:]
source = source.replace('BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);', 'BitConverter.GetBytes(code.Length).CopyTo(after,h+4);BitConverter.GetBytes((ushort)max).CopyTo(after,h+2);int local=token==0x060006DF?0x11000000+localFirst:m.Body.LocalVarToken.ToInt32();BitConverter.GetBytes(local).CopyTo(after,h+8);')
source = source.replace('local_sig=m.Body.LocalVarToken.ToInt32(),code_sha256=H(code)});', 'old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,code_sha256=H(code)});')
source = source.replace('if(code.Length>m.Body.CodeSize){', 'if(code.Length>m.Body.CodeSize){R(token!=0x060006DF,"TextLink must fit old slot with its scoped new locals");')
a = source.index(',scope="') + 1
b = source.index('"},new JsonSerializerOptions', a)
source = source[:a] + 'scope="Two complete original-native-backed callers; TextLink captures old text before event position and Camera, passes Vector3 by value, writes saved Find index before exact -1 return, rereads current text, copies array struct before GetLinkID and captures string across Log; one proven public PointerEventData position MemberRef and one target-only appended Vector2 local signature; Window retains six-case switch, initial/reread signed index distinctions, Unity equality/implicit and raw manager checks, captured item/list across callbacks, reread current MainUI after deletion, exact return-versus-Destroy scope without finally; complete oversize Window body relocated with only its MethodDef RVA changed; no tests executed; unqualified parent chain pending' + source[b:]
(p/'Patcher86.cs').write_text(source)
remote = '''from pathlib import Path
import subprocess,base64
w=Path('/workspaces/GodsPVZ-native24-codespace')
n=Path('/tmp/godspvz-native86-codex-2026-10-13');n.mkdir(exist_ok=True)
s=n/'Patcher';s.mkdir(exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(PATCHER_B64))
(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True)
(n/'build.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'bin/Patcher.dll'),'/tmp/godspvz-native85-codex-2026-10-13/candidate1.dll',str(n/'candidate1.dll'),str(n/'generation1.json')],capture_output=True,text=True)
(n/'generation.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
print((n/'generation1.json').read_text())
'''.replace('PATCHER_B64', repr(base64.b64encode(source.encode()).decode()))
(p/'generate_remote.py').write_text(remote)
