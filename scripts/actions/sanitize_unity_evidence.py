"""Produce a public evidence tree; raw account logs are never uploaded."""
from pathlib import Path
import os,re,sys,shutil
src=Path(sys.argv[1]).resolve();dst=Path(sys.argv[2]).resolve();dst.mkdir(parents=True,exist_ok=False)
if not src.is_dir():raise SystemExit(0)
secrets=[os.environ.get(k,'') for k in ('UNITY_EMAIL','UNITY_PASSWORD')];secrets=[s for s in secrets if s]
sensitive=re.compile(r'(access.?token|refresh.?token|password|e-?mail|username|user name|license.?key|license.?hash|serial number|serial:|machine id|session id|correlation id)',re.I)
for p in sorted(src.rglob('*')):
 if not p.is_file():continue
 out=dst/p.relative_to(src);out.parent.mkdir(parents=True,exist_ok=True)
 # Evidence tree consists of logs, JSON, hashes and small text receipts.
 text=p.read_text('utf-8',errors='replace')
 for secret in secrets:text=text.replace(secret,'[REDACTED_ACCOUNT_SECRET]')
 out.write_text('\n'.join('[REDACTED_POTENTIALLY_SENSITIVE_LICENSE_LINE]' if sensitive.search(line) else line for line in text.splitlines())+'\n','utf-8')
