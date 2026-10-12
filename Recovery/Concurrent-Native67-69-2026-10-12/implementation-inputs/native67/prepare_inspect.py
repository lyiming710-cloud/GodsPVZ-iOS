from pathlib import Path
import base64

p = Path('audit/native67-codex-2026-10-12')
s = Path('audit/native60-codex-2026-10-12/Inspect.cs').read_text()
s = s.replace('0x060000E6,0x06000218,0x060002FF,0x060004DB,0x060004DC,0x060002A4,0x0600029F', '0x0600023C,0x06000877')
s = s.replace('new[]{"Supplies","DeviceContent","ContentDeviceData","SuppliesInitialValue","EnemyPath"}', 'new[]{"SavesManager","SaveList","Save","SwfClipController"}')
s = s.replace('new[]{0x060004DB,0x060002A4}', 'new[]{0x0600023C,0x06000877}')
s = s.replace('var refs=a.MainModule.GetMemberReferences()', 'var refs=a.MainModule.GetMemberReferences()')
start = s.index('.Where(m=>m.DeclaringType.FullName.Contains("Renderer")')
end = s.index('.Select(Method);', start)
s = s[:start] + '.Where(m=>m.DeclaringType.FullName.Contains("List`1<Save>")||m.DeclaringType.FullName.Contains("Action`1<FTRuntime.SwfClipController>"))' + s[end:]
(p/'Inspect.cs').write_text(s)
template = Path('audit/native60-codex-2026-10-12/inspect_metadata_remote.py').read_text()
start = template.index("(s/'Program.cs').write_bytes")
end = template.index('\n', start)
template = template[:start] + "(s/'Program.cs').write_bytes(base64.b64decode(" + repr(base64.b64encode(s.encode()).decode()) + "))" + template[end:]
template = template.replace('godspvz-native60-codex', 'godspvz-native67-codex')
template = template.replace("str(w/'.validation/native59-accepted-candidate.dll')", "'/tmp/godspvz-pending-restored-2026-10-12/snapshot/candidates/native66/candidate1.dll'")
(p/'inspect_remote.py').write_text(template)
