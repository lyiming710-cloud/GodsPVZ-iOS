from pathlib import Path
import json,base64
p=Path('audit/native76-codex-2026-10-12');p.mkdir(exist_ok=True)
s=Path('audit/native75-codex-2026-10-12/Inspect.cs').read_text().replace('new[]{0x060002B5,0x060002B7,0x0600056F,0x060006DB}','new[]{0x06000039,0x06000322,0x060006D4,0x06000703,0x06000169,0x06000636}')
s=s.replace('new[]{"Board","Almanac_PlantDetail","SkillButtonController","ResourceManager","FlagMeter"}','new[]{"Device_boardEditor","PreDevice","DeviceData","Device","BoardManager","MainUIController","SavesUIController","SavesUIController_PlayerSave","Window_Q"}')
s=s.replace('t.FullName.Contains("SettlementUIController")','t.FullName.Contains("SettlementUIController")||t.FullName.Contains("List`1<Device>")||t.FullName.Contains("List`1<UnityEngine.Sprite>")||t.FullName.Contains("SavesUIController_PlayerSave")')
s=s.replace('m.DeclaringType.FullName=="UnityEngine.Vector3"||m.DeclaringType.FullName=="UnityEngine.Color"||m.FullName=="UnityEngine.Transform UnityEngine.GameObject::get_transform()"','m.Name=="get_Count"||m.Name=="Remove"||m.Name=="op_Equality"||m.Name=="op_Inequality"')
s=s.replace('m.Body.Variables.Select(v=>v.VariableType.FullName)','m.Body.Variables.Select(v=>v.VariableType.FullName)')
(p/'Inspect.cs').write_text(s)
t=Path('audit/native75-codex-2026-10-12/inspect_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native75-codex','godspvz-native76-codex').replace('/tmp/godspvz-native74-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native75-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text())
(p/'native-methods.json').write_text(json.dumps([m for m in d['methods']if m['token']in('0x06000039','0x06000322','0x060006D4','0x06000703')],indent=2))
