from pathlib import Path
import json,hashlib
p=Path(__file__).parent/'repository/Recovery/Codespace-Native14-2026-09-28'
for n,h in json.loads((p/'SHA256.json').read_text()).items():
 b=(p/n).read_bytes()
 print(n,hashlib.sha256(b).hexdigest()==h,'size',len(b),'CRLF',b.count(b'\r\n'),'normalized',hashlib.sha256(b.replace(b'\r\n',b'\n')).hexdigest()==h,hashlib.sha256(b).hexdigest())
