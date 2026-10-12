from pathlib import Path
import json,base64,sys,struct
p=Path('audit/native77-codex-2026-10-12');p.mkdir(exist_ok=True)
s=Path('audit/native75-codex-2026-10-12/Inspect.cs').read_text().replace('new[]{0x060002B5,0x060002B7,0x0600056F,0x060006DB}','new[]{0x060003D9,0x06000491,0x060001E5}')
s=s.replace('new[]{"Board","Almanac_PlantDetail","SkillButtonController","ResourceManager","FlagMeter"}','new[]{"Projectile","Zombie","ZombieAnimationEvent","ParticlesManager"}')
(p/'Inspect.cs').write_text(s)
t=Path('audit/native76-codex-2026-10-12/inspect_remote.py').read_text();a=t.index("(s/'Program.cs').write_bytes");b=t.index('\n',a)
t=t[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+t[b:]
t=t.replace('godspvz-native76-codex','godspvz-native77-codex').replace('/tmp/godspvz-native75-codex-2026-10-12/candidate1.dll','/tmp/godspvz-native76-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(t)
d=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text())
(p/'native-methods.json').write_text(json.dumps([m for m in d['methods']if m['token']in('0x060003D9','0x06000491')],indent=2))
sys.path.insert(0,'audit/stage9-native3/repository/Tools/Stage9NativeProbe');from pc_method_probe import PE
raw=Path('audit/stage9-native3/inputs/GameAssembly.dll').read_bytes();pe=PE(raw);data=Path('audit/stage9-native3/inputs/global-metadata.dat').read_bytes();magic,v,lo,lb,do,db=struct.unpack_from('<6I',data)
out={'constants':[],'literals':[]}
for va,size in((0x1815A7ACC,4),(0x1815A7E08,8),(0x1815A7E28,4),(0x1815A7E14,4)):
 b=raw[pe.va_to_offset(va):pe.va_to_offset(va)+size];out['constants'].append({'va':hex(va),'bytes':b.hex(),'value':struct.unpack('<d'if size==8 else'<f',b)[0]})
out['constants'].append({'kind':'immediate speed-x','bits':'44BB8000','value':struct.unpack('<f',struct.pack('<I',0x44BB8000))[0]})
for va in(0x181BA5208,0x181BAE898):
 seed=struct.unpack_from('<Q',raw,pe.va_to_offset(va))[0];kind=seed>>29;idx=(seed&0x1FFFFFFE)>>1
 if kind!=5:raise RuntimeError('Unexpected string seed')
 size,off=struct.unpack_from('<2I',data,lo+8*idx);out['literals'].append({'va':hex(va),'seed':hex(seed),'literal_index':idx,'text':data[do+off:do+off+size].decode()})
(p/'native-inputs.json').write_text(json.dumps(out,indent=2));print(json.dumps(out,indent=2))
