from pathlib import Path
import base64,json,sys,struct
p=Path('audit/native74-codex-2026-10-12');p.mkdir(exist_ok=True)
s=Path('audit/native73-codex-2026-10-12/Inspect.cs').read_text().replace('new[]{0x0600000F,0x06000010,0x0600039B,0x06000517}','new[]{0x0600027C,0x06000208,0x06000173,0x060005CE}')
s=s.replace('new[]{"Admin_system2_boardEdior","Plant","SkillManager","ParticlesManager","VFXAnimationEvent","Project","GlobalStaticVars"}','new[]{"ZombieManager","ProjectManager","BoardManager","BoardPreview","SettlementUIController","Board"}')
s=s.replace('type_refs=a.MainModule.GetTypeReferences()', 'specs=Enumerable.Range(1,new System.Reflection.PortableExecutable.PEReader(File.OpenRead(args[0])).GetMetadataReader().GetTableRowCount(System.Reflection.Metadata.Ecma335.TableIndex.TypeSpec)).Select(i=>a.MainModule.LookupToken(0x1B000000+i)).OfType<TypeReference>().Where(t=>t.FullName.Contains("SettlementUIController")).Select(t=>new{token=$"0x{t.MetadataToken.ToInt32():X8}",name=t.FullName}),external_fields=a.MainModule.GetMemberReferences().OfType<FieldReference>().Where(f=>f.DeclaringType.FullName=="UnityEngine.Vector3").Select(Field),type_refs=a.MainModule.GetTypeReferences()')
s='using System.Reflection.Metadata;\nusing System.Reflection.Metadata.Ecma335;\nusing AssemblyDefinition=Mono.Cecil.AssemblyDefinition;\nusing TypeReference=Mono.Cecil.TypeReference;\n'+s
(p/'Inspect.cs').write_text(s)
remote=Path('audit/native73-codex-2026-10-12/inspect_remote.py').read_text();a=remote.index("(s/'Program.cs').write_bytes");b=remote.index('\n',a)
remote=remote[:a]+"(s/'Program.cs').write_bytes(base64.b64decode("+repr(base64.b64encode(s.encode()).decode())+"))"+remote[b:]
remote=remote.replace('godspvz-native73-codex','godspvz-native74-codex').replace('/tmp/godspvz-native72-restored-2026-10-12/snapshot/candidates/native72/candidate1.dll','/tmp/godspvz-native73-codex-2026-10-12/candidate1.dll')
(p/'inspect_remote.py').write_text(remote)
e=json.loads(Path('audit/native30-continuation-2026-10-02/native55-native.json').read_text())
(p/'native-methods.json').write_text(json.dumps([m for m in e['methods']if m['token']in('0x0600027C','0x06000208','0x06000173','0x060005CE')],indent=2))
sys.path.insert(0,'audit/stage9-native3/repository/Tools/Stage9NativeProbe')
from pc_method_probe import PE
raw=Path('audit/stage9-native3/inputs/GameAssembly.dll').read_bytes();pe=PE(raw)
rows=[]
for va in (0x1815A7A50,):
 b=raw[pe.va_to_offset(va):pe.va_to_offset(va)+4];rows.append({'va':hex(va),'bytes':b.hex(),'float':struct.unpack('<f',b)[0]})
rows.append({'va':'0x1803422bc','kind':'immediate-mov-float','bytes':'0ad7233c','float':struct.unpack('<f',bytes.fromhex('0ad7233c'))[0]})
(p/'native-inputs.json').write_text(json.dumps(rows,indent=2));print(rows)
