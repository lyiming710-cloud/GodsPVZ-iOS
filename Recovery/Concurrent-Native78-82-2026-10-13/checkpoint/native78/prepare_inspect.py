from pathlib import Path
import base64,json
p=Path('audit/native78-codex-2026-10-13')
s=Path('audit/native77-codex-2026-10-12/Inspect.cs').read_text()
s=s.replace('new[]{0x060003D9,0x06000491,0x060001E5}', 'new[]{0x0600061D,0x06000483,0x06000238,0x06000365,0x060003CA,0x060001E5,0x06000636}')
s=s.replace('new[]{"Projectile","Zombie","ZombieAnimationEvent","ParticlesManager"}', 'new[]{"GridConfig_A","System2","GridState","Zombie","Plant","Save","SaveList","SavesManager","MainUIController","ParticlesManager"}')
s=s.replace('var refs=a.MainModule.GetMemberReferences().OfType<MethodReference>().Where(m=>m.DeclaringType.FullName=="UnityEngine.Vector3"||m.DeclaringType.FullName=="UnityEngine.Color"||m.FullName=="UnityEngine.Transform UnityEngine.GameObject::get_transform()").Select(Method);', 'var refs=a.MainModule.GetMemberReferences().OfType<MethodReference>().Where(m=>m.DeclaringType.FullName=="UnityEngine.Vector3"||m.DeclaringType.FullName=="System.Enum"||m.DeclaringType.FullName=="System.Type"||m.Name=="Add"||m.Name=="get_transform").Select(Method);')
s=s.replace('t.FullName.Contains("SettlementUIController")','t.FullName.Contains("List`1<Save>")')
(p/'Inspect.cs').write_text(s)
parent='/tmp/godspvz-native77-restored-2026-10-13/snapshot/candidates/native77/candidate1.dll'
t='''from pathlib import Path
import subprocess,base64
w=Path('/workspaces/GodsPVZ-native24-codespace')
n=Path('/tmp/godspvz-native78-codex-2026-10-13');n.mkdir(exist_ok=True)
s=n/'Inspect';s.mkdir(exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(SOURCE))
(s/'Inspect.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Inspect.csproj'),'-c','Release','-o',str(n/'inspect-bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'inspect-bin/Inspect.dll'),PARENT],capture_output=True,text=True)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
(n/'metadata.json').write_text(r.stdout)
print(r.stdout)
'''.replace('SOURCE',repr(base64.b64encode(s.encode()).decode())).replace('PARENT',repr(parent))
(p/'inspect_remote.py').write_text(t)
print('Prepared read-only metadata inspector')
