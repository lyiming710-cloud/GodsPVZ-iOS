from pathlib import Path
import base64
p=Path('audit/native71-codex-2026-10-12')
s=Path('audit/native70-codex-2026-10-12/Inspect.cs').read_text()
s=s.replace('new[]{0x060001FA,0x060002AE,0x060003AA,0x06000318,0x06000261}', 'new[]{0x06000532,0x06000632}')
s=s.replace('new[]{"ProjectManager","Board","Plant","Project","Device","Damage","Zombie"}', 'new[]{"AdventureMenu_ModeButton","LevelItem_HA"}')
start=s.index('.Where(m=>m.DeclaringType.FullName=="UnityEngine.Vector3"');end=s.index('.Select(Method);',start)
s=s[:start]+'.Where(m=>m.DeclaringType.FullName.Contains("Selectable")||m.Name=="get_interactable")'+s[end:]
(p/'Inspect.cs').write_text(s)
t=Path('audit/native70-codex-2026-10-12/inspect_remote.py').read_text();start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[end:]
t=t.replace('godspvz-native70-codex','godspvz-native71-codex').replace('/tmp/godspvz-native69-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native70-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
engine='''using Mono.Cecil;
using System.Text.Json;
using System.Security.Cryptography;
var files=new[]{"/workspaces/GodsPVZ-iOS/Tools/Stage9Native4Recovery/resolver/UnityEngine.UI.dll","/workspaces/GodsPVZ-native14-check/Tools/Stage9Native4Recovery/resolver/UnityEngine.UI.dll","/workspaces/GodsPVZ-native24-codespace/.validation/native25-2026-10-01/native24-restored/managed-support/UnityEngine.UI.dll"};
var rows=new List<object>();
foreach(var file in files){if(!File.Exists(file)){rows.Add(new{path=file,exists=false});continue;}using var a=AssemblyDefinition.ReadAssembly(file);var t=a.MainModule.GetType("UnityEngine.UI.Selectable");var m=t.Methods.SingleOrDefault(m=>m.Name=="get_interactable"&&m.Parameters.Count==0);
 rows.Add(new{path=file,exists=true,bytes=new FileInfo(file).Length,sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(file))).ToLowerInvariant(),assembly=a.FullName,mvid=a.MainModule.Mvid.ToString(),type=t.FullName,field=t.Fields.SingleOrDefault(f=>f.Name=="m_Interactable")?.Attributes.ToString(),getter=m==null?null:new{token=$"0x{m.MetadataToken.ToInt32():X8}",name=m.FullName,attributes=m.Attributes.ToString(),il=m.HasBody?m.Body.Instructions.Select(i=>i.ToString()).ToArray():Array.Empty<string>()}});}
Console.WriteLine(JsonSerializer.Serialize(rows,new JsonSerializerOptions{WriteIndented=true}));
'''
(p/'InspectUI.cs').write_text(engine)
t=t.replace('metadata.json','ui-reference.json').replace('Inspect.csproj','InspectUI.csproj').replace('inspect-bin','ui-inspect-bin').replace('/Inspect.dll','/InspectUI.dll')
start=t.index("(s/'Program.cs').write_bytes");end=t.index('\n',start)
t=t[:start]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(engine.encode()).decode())+"))"+t[end:]
t=t.replace("s=n/'Inspect'", "s=n/'InspectUI'")
(p/'inspect_ui_remote.py').write_text(t)
