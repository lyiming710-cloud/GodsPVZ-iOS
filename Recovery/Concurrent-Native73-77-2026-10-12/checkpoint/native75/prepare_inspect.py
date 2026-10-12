from pathlib import Path
import json,base64,sys,struct
p=Path('audit/native75-codex-2026-10-12');p.mkdir(exist_ok=True)
s=Path('audit/native74-codex-2026-10-12/Inspect.cs').read_text().replace('new[]{0x0600027C,0x06000208,0x06000173,0x060005CE}','new[]{0x060002B5,0x060002B7,0x0600056F,0x060006DB}')
s=s.replace('new[]{"ZombieManager","ProjectManager","BoardManager","BoardPreview","SettlementUIController","Board"}','new[]{"Board","Almanac_PlantDetail","SkillButtonController","ResourceManager","FlagMeter"}')
s=s.replace('m.DeclaringType.FullName=="UnityEngine.Vector3"||m.DeclaringType.FullName=="UnityEngine.Input"||m.Name=="SetEvent"','m.DeclaringType.FullName=="UnityEngine.Vector3"||m.DeclaringType.FullName=="UnityEngine.Color"||m.FullName=="UnityEngine.Transform UnityEngine.GameObject::get_transform()"')
(p/'Inspect.cs').write_text(s)
t=Path('audit/native74-codex-2026-10-12/inspect_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native74-codex','godspvz-native75-codex').replace('/tmp/godspvz-native73-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native74-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text())
(p/'native-methods.json').write_text(json.dumps([m for m in d['methods']if m['token']in('0x060002B5','0x060002B7','0x0600056F','0x060006DB')],indent=2))
sys.path.insert(0,'audit/stage9-native3/repository/Tools/Stage9NativeProbe')
from pc_method_probe import PE
raw=Path('audit/stage9-native3/inputs/GameAssembly.dll').read_bytes();pe=PE(raw);data=Path('audit/stage9-native3/inputs/global-metadata.dat').read_bytes()
magic,version,lo,lb,do,db=struct.unpack_from('<6I',data)
out={'constants':[],'literals':[],'fields':[]}
for va,length in((0x1815A7B70,16),(0x1815A7FB0,16),(0x1815A7BF4,4),(0x1815A7B00,4),(0x1815A7A10,4)):
 bs=raw[pe.va_to_offset(va):pe.va_to_offset(va)+length];out['constants'].append({'va':hex(va),'bytes':bs.hex(),'values':struct.unpack('<'+'f'*(length//4),bs)})
for va in(0x181B9B580,0x181B9B6A0,0x181BC6F38):
 seed=struct.unpack_from('<Q',raw,pe.va_to_offset(va))[0];kind=seed>>29;idx=(seed&0x1FFFFFFE)>>1
 if kind!=5:raise RuntimeError('Literal kind differs')
 size,off=struct.unpack_from('<2I',data,lo+idx*8);out['literals'].append({'va':hex(va),'seed':hex(seed),'literal_index':idx,'text':data[do+off:do+off+size].decode()})
out['fields']=[f for f in json.loads(Path('audit/stage9-native3/evidence/native-fields.json').read_text())if f['type']=='Almanac_PlantDetail'and f['offset']in(408,412)]
(p/'native-inputs.json').write_text(json.dumps(out,indent=2));print(json.dumps(out,indent=2))
