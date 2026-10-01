from pathlib import Path
import subprocess,json,struct
w=Path('/workspaces/GodsPVZ-native24-codespace'); n=w/'.validation/native29-2026-10-01'; src=n/'inspect'
s='''using Mono.Cecil;using System.Text.Json;using System.Reflection.PortableExecutable;using System.Reflection.Metadata;using System.Reflection.Metadata.Ecma335;
using var a=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);var m=a.MainModule;
foreach(var t in m.Types.Where(t=>t.FullName=="FTRuntime.SwfSettingsData"))Console.WriteLine("SETTINGS "+JsonSerializer.Serialize(t.Fields.Select(f=>new{f.Name,type=f.FieldType.FullName,token=f.MetadataToken.ToInt32()})));
Console.WriteLine("HASH_REFS "+JsonSerializer.Serialize(m.GetMemberReferences().OfType<MethodReference>().Where(r=>r.DeclaringType.FullName=="System.Collections.Generic.HashSet`1<System.String>").Select(r=>new{r.Name,r.FullName,token=r.MetadataToken.ToInt32()})));
using var f=File.OpenRead(args[0]);using var pe=new PEReader(f);var md=pe.GetMetadataReader();Console.WriteLine("PE "+JsonSerializer.Serialize(new{pe.PEHeaders.CoffHeaderStartOffset,pe.PEHeaders.PEHeaderStartOffset,pe.PEHeaders.CorHeaderStartOffset,sections=pe.PEHeaders.SectionHeaders.Select(s=>new{s.Name,s.VirtualAddress,s.VirtualSize,s.SizeOfRawData,s.PointerToRawData}),memberRowSize=md.GetTableRowSize(TableIndex.MemberRef),memberOffset=md.GetTableMetadataOffset(TableIndex.MemberRef),memberCount=md.GetTableRowCount(TableIndex.MemberRef),ts=Enumerable.Range(1,md.GetTableRowCount(TableIndex.TypeSpec)).Select(i=>m.LookupToken(0x1b000000+i)).OfType<Mono.Cecil.TypeReference>().Where(t=>t.FullName=="System.Action`1<FTRuntime.SwfClipController>"||t.FullName=="System.Collections.Generic.HashSet`1<System.String>").Select(t=>new{t.FullName,token=t.MetadataToken.ToInt32()})}));
'''
(src/'Program.cs').write_text(s)
r=subprocess.run(['dotnet','build',str(src/'Inspect.csproj'),'-c','Release','-o',str(n/'probe-bin')],capture_output=True,text=True);(n/'probe-build.log').write_text(r.stdout+r.stderr);assert r.returncode==0,r.stdout+r.stderr
r=subprocess.run(['dotnet',str(n/'probe-bin/Inspect.dll'),str(w/'.validation/native28-2026-10-01/native28-final1.dll')],capture_output=True,text=True);(n/'probe.txt').write_text(r.stdout+r.stderr);assert r.returncode==0,r.stdout+r.stderr;print(r.stdout)
scope={'__name__':'probe'};exec((w/'scripts/codespaces/native23_native.py').read_text(),scope);p=scope['PE'](Path('/workspaces/GodsPVZ-native19/.validation/native22/pcnative/GameAssembly.dll'))
print('CONSTANTS',[(hex(x),p.data[p.va_to_offset(x):p.va_to_offset(x)+4].hex(),struct.unpack_from('<f',p.data,p.va_to_offset(x))[0]) for x in [0x1815A7B18,0x1815A7B10,0x1815A7B0C,0x1815A7A10,0x1815A7A0C,0x1815A7A14]])
for t in ['SwfSettingsData']:
 data=json.loads(Path('/workspaces/GodsPVZ-native19/.validation/native22/pcnative/native-fields.json').read_text()); print('FIELD_FORMAT',str(data)[:150]); print('SETTINGS_NATIVE',[x for x in data if isinstance(x,dict) and x.get('type')==t])
