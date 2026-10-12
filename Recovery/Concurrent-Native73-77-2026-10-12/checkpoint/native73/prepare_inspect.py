from pathlib import Path
import base64,json,struct,hashlib,sys
p=Path('audit/native73-codex-2026-10-12')
source=Path('audit/native72-codex-2026-10-12/Inspect.cs').read_text()
source=source.replace('new[]{0x0600013A,0x06000544,0x060006BC,0x0600021A,0x06000683}','new[]{0x0600000F,0x06000010,0x0600039B,0x06000517}')
source=source.replace('new[]{"ResourceManager","Zombie_charred","AlmanacUIController","SeedChooserScreen"}','new[]{"Admin_system2_boardEdior","Plant","SkillManager","ParticlesManager","VFXAnimationEvent","Project","GlobalStaticVars"}')
a=source.index('.Where(m=>m.DeclaringType.FullName=="UnityEngine.Vector3"');b=source.index('.Select(Method);',a)
source=source[:a]+'.Where(m=>m.DeclaringType.FullName=="UnityEngine.Vector3"||m.DeclaringType.FullName=="UnityEngine.Input"||m.Name=="SetEvent")'+source[b:]
source=source.replace('targets=body,refs,internals,getters','targets=body,refs,internals,getters,type_refs=a.MainModule.GetTypeReferences().Select(t=>new{token=$"0x{t.MetadataToken.ToInt32():X8}",name=t.FullName,scope=t.Scope.Name})')
(p/'Inspect.cs').write_text(source)
remote='''from pathlib import Path
import subprocess,base64
w=Path('/workspaces/GodsPVZ-native24-codespace')
n=Path('/tmp/godspvz-native73-codex-2026-10-12');n.mkdir(exist_ok=True)
s=n/'Inspect';s.mkdir(exist_ok=True)
'''+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(source.encode()).decode())+"))\n"+'''(s/'Inspect.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Inspect.csproj'),'-c','Release','-o',str(n/'inspect-bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'inspect-bin/Inspect.dll'),'/tmp/godspvz-native72-restored-2026-10-12/snapshot/candidates/native72/candidate1.dll'],capture_output=True,text=True)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
(n/'metadata.json').write_text(r.stdout)
print(r.stdout)
'''
(p/'inspect_remote.py').write_text(remote)
e=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text())
methods=[m for m in e['methods']if m['token']in['0x0600000F','0x06000010','0x0600039B','0x06000517']]
(p/'native-methods.json').write_text(json.dumps(methods,indent=2))
sys.path.insert(0,'audit/stage9-native3/repository/Tools/Stage9NativeProbe')
from pc_method_probe import PE
native=Path('audit/stage9-native3/inputs/GameAssembly.dll').read_bytes();data=Path('audit/stage9-native3/inputs/global-metadata.dat').read_bytes();pe=PE(native)
magic,version,lo,lb,do,db=struct.unpack_from('<6I',data)
out={'GameAssembly_sha256':hashlib.sha256(native).hexdigest(),'metadata_sha256':hashlib.sha256(data).hexdigest(),'constants':[],'literals':[]}
for va in (0x1815A7A70,0x1815A7A4C):
    bits=native[pe.va_to_offset(va):pe.va_to_offset(va)+4]
    out['constants'].append({'va':hex(va),'bytes':bits.hex(),'float':struct.unpack('<f',bits)[0]})
for va in(0x181B9FCD0,0x181BCA140,0x181BC9C20,0x181BB5E40):
    seed=struct.unpack_from('<Q',native,pe.va_to_offset(va))[0];kind=seed>>29;idx=(seed&0x1FFFFFFE)>>1
    if kind!=5:raise RuntimeError('Unexpected literal usage kind')
    size,off=struct.unpack_from('<2I',data,lo+idx*8)
    out['literals'].append({'va':hex(va),'seed':hex(seed),'kind':kind,'literal_index':idx,'text':data[do+off:do+off+size].decode()})
(p/'native-inputs.json').write_text(json.dumps(out,indent=2))
print(json.dumps(out,indent=2))
