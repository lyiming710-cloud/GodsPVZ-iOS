from pathlib import Path
import json
p=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review')
d=json.loads((p/'all-methods.batch1.json').read_text())
for m in d['methods']:
 if m['token'] in ('0x06000339','0x06000348','0x0600034C'):
  print(json.dumps(m,indent=2))
for n in ('inspector-build.log','inspect-driver.log','patcher-swap-probe.json'):
 if (p/n).exists():print(n,(p/n).read_text()[-1800:])
