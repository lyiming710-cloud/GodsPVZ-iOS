import subprocess,json
from pathlib import Path
p=Path('/workspaces/GodsPVZ-native19/.validation/xcode-native18/GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst')
r=subprocess.run(['gh','release','upload','recovery-archive-2026-09-30',str(p),'--repo','lyiming710-cloud/GodsPVZ-iOS'],capture_output=True,text=True)
print(json.dumps({'exit':r.returncode,'stdout':r.stdout,'stderr':r.stderr}));raise SystemExit(r.returncode)
