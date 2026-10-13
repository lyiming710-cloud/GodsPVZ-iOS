from pathlib import Path
import base64

p=Path('audit/native88-codex-2026-10-13');p.mkdir(exist_ok=True)
source=Path('audit/native87-codex-2026-10-13/Patcher87.cs').read_text().replace('RepairNative87','RepairNative88')
source=source.replace('20ec9187fb3edc51d80cd486a70f6e5e91d7a1958ce4f4db456a2bee7124fa11','78aae6f5b3e96006a5bf278fabe1a7e86dbfcd7dfe2ac6279d1df47035d272d8')
source=source.replace('Pinned untested Native86 parent','Pinned untested Native87 parent')
template=Path('audit/native85-codex-2026-10-13/Patcher85.cs').read_text()
metadata=template[template.index(' static (byte[] Image,object Evidence) Metadata('):template.index(' static (byte[] Image,object Evidence) AppendBody(')]
a=metadata.index('  var added=new List<object>();');b=metadata.index('  ulong valid=',a)
metadata=metadata[:a]+r'''  var added=new List<object>();var method=(MD)module.LookupToken(0x0600013E);
  byte[] old=md.GetBlobBytes(md.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle((int)method.Body.LocalVarToken.RID)).Signature);
  R(old[0]==7&&old[1]==2&&method.Body.Variables.Count==2&&method.Body.Variables[0].VariableType.FullName=="UnityEngine.GameObject"&&method.Body.Variables[1].VariableType.FullName=="System.Single","Pinned original audio helper locals");
  byte[] sig=new byte[]{7,3}.Concat(old[2..]).Concat(new byte[]{0x0C}).ToArray();int si=Blob(sig),first=md.GetTableRowCount(TableIndex.StandAloneSig)+1;
  added.Add(new{kind="StandAloneSig",token=$"0x{0x11000000+first:X8}",target="0x0600013E",active_local=2,type="System.Single captured clip.length",old_signature_hex=Convert.ToHexString(old),signature_hex=Convert.ToHexString(sig),signature_index=si});
  var additions=new Dictionary<TableIndex,byte[]>{{TableIndex.StandAloneSig,Words(si)}};
'''+metadata[b:]
metadata=metadata.replace('t==TableIndex.MethodSpec&&width==4','t==TableIndex.StandAloneSig&&width==2')
metadata=metadata.replace('clear=1UL<<(int)TableIndex.MethodSpec','clear=1UL<<(int)TableIndex.StandAloneSig')
metadata=metadata.replace('methodspec_sorted_bit_cleared=(sorted&(1UL<<43))!=0','standalonesig_sorted_bit_cleared=(sorted&(1UL<<17))!=0')
insert=source.index(' static (byte[] Image,object Evidence) AppendBody(')
source=source[:insert]+metadata+source[insert:]
source=source.replace('AppendBody(byte[] current,MD method,byte[] code,int max)', 'AppendBody(byte[] current,MD method,byte[] code,int max,int local=0)')
source=source.replace('BitConverter.GetBytes(method.Body.LocalVarToken.ToInt32()).CopyTo(after,body+8)', 'BitConverter.GetBytes(local==0?method.Body.LocalVarToken.ToInt32():local).CopyTo(after,body+8)')
source=source.replace('  byte[] after=before.ToArray();','  var(after,metadata)=Metadata(before,pe,module);')
a=source.index('  var m=M(0x06000199);');b=source.index('File.WriteAllBytes(args[1]',a)
source=source[:a]+r'''  var m=M(0x0600013E);R(m.IsStatic&&m.Parameters.Count==4&&m.Body.ExceptionHandlers.Count==0&&m.Body.CodeSize==145&&m.Body.LocalVarToken.ToInt32()==0x110000FD,"Exact existing four-argument helper body boundary");
  var sec=pe.PEHeaders.SectionHeaders.Single(x=>m.RVA>=x.VirtualAddress&&m.RVA<x.VirtualAddress+x.SizeOfRawData);int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;
  R((before[h]&3)==3&&(BitConverter.ToUInt16(before,h)>>12)==3&&H(before[h..(h+12+m.Body.CodeSize)])=="5520ac1a25114890963a4712d8865cd1027064d3d7423f015ec678e3a1ee6f77","Pinned original existing helper bytes, not a guessed reconstruction");
  R(m.Body.Instructions.Single(i=>i.Offset==100).OpCode==OpCodes.Callvirt&&((MR)m.Body.Instructions.Single(i=>i.Offset==100).Operand).FullName=="System.Void UnityEngine.AudioSource::Play()","Exact prefix boundary after Play retaining captured AudioSource");
  var il=new IL();il.B.AddRange(before[(h+12)..(h+12+105)]);
  il.Arg(0).Ref(0x6F,0x0A0000FA).S(2)
   .Float(0.01f).Ref(0x28,0x0A0000F9).Branch(0x30,"floor")
   .Ref(0x28,0x0A0000F9).Branch(0x2B,"factor")
   .Label("floor").Float(0.01f).Label("factor").S(1)
   .L(0).L(2).L(1).O(0x5A,0x6B).Ref(0x28,0x0A000088).O(0x2A);
  byte[] code=il.Done();int local=0x11000000+localFirst;var relocated=AppendBody(after,m,code,4,local);after=relocated.Image;
  report.Add(new{token="0x0600013E",method=m.FullName,old_reserved_code_size=m.Body.CodeSize,new_code_size=code.Length,old_max=m.Body.MaxStackSize,new_max=4,old_local_sig=m.Body.LocalVarToken.ToInt32(),local_sig=local,unchanged_code_prefix_length=105,unchanged_prefix_sha256=H(before[(h+12)..(h+12+105)]),code_sha256=H(code),relocation=relocated.Evidence});
  R(report.Count==1,"Pinned scoped audio ordering repair");'''+source[b:]
source=source.replace('metadata_scope="Existing table rows except target MethodDef RVA remain unchanged by construction; no new rows or heap edits"','metadata')
a=source.index(',scope="')+1;b=source.index('"},new JsonSerializerOptions',a)
source=source[:a]+'scope="One explicit existing helper ordering repair: preserves original145-byte-body prefix through AudioSource.Play byte-for-byte, captures clip.length before any Time.timeScale getter, ordered floor comparison retains optional second getter including NaN branch, captured length multiplied on left by selected Single factor on right with explicit Single rounding, same captured GameObject Destroy and AudioSource return; one appended target-only Single local signature, complete148-byte body relocated and only its MethodDef RVA updated, original slot unchanged; other pre-existing helper behavior is preserved and full native/CIL dependency acceptance remains pending; no tests executed'+source[b:]
(p/'Patcher88.cs').write_text(source)
remote='''from pathlib import Path
import subprocess,base64,zipfile,json,hashlib
w=Path('/workspaces/GodsPVZ-native24-codespace');n=Path('/tmp/godspvz-native88-codex-2026-10-13');n.mkdir(exist_ok=True)
s=n/'Patcher';s.mkdir(exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(PATCHER_B64));(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True);(n/'build.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'bin/Patcher.dll'),'/tmp/godspvz-native87-codex-2026-10-13/candidate1.dll',str(n/'candidate1.dll'),str(n/'generation1.json')],capture_output=True,text=True);(n/'generation.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
dest=w/'.validation/native83-86-2026-10-13-checkpoints/native88-generated.zip'
if dest.exists():raise RuntimeError('Fresh persistent checkpoint required')
with zipfile.ZipFile(dest,'w',zipfile.ZIP_DEFLATED,compresslevel=6)as z:
 for f in n.rglob('*'):
  if f.is_file() and 'obj' not in f.parts:z.write(f,'build/native88/'+f.relative_to(n).as_posix())
print(json.dumps({'checkpoint_path':str(dest),'checkpoint_bytes':dest.stat().st_size,'checkpoint_sha256':hashlib.sha256(dest.read_bytes()).hexdigest()}))
print((n/'generation1.json').read_text())
'''.replace('PATCHER_B64',repr(base64.b64encode(source.encode()).decode()))
(p/'generate_remote.py').write_text(remote)
