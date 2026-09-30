"""Use current gh credential only in transient SSH stdin/process environment."""
from pathlib import Path
import subprocess,shlex
p=Path(__file__).parent;gh='C:/Program Files/GitHub CLI/gh.exe'
token=subprocess.check_output([gh,'auth','token']).strip()
code='import os,sys,subprocess; e=dict(os.environ); e["GH_TOKEN"]=sys.stdin.read().strip(); sys.exit(subprocess.call(["gh","release","upload","recovery-archive-2026-09-30","/workspaces/GodsPVZ-native19/.validation/xcode-native18/GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst","--repo","lyiming710-cloud/GodsPVZ-iOS"],env=e))'
with (p/'xcode-authenticated-upload.log').open('wb') as out:
 r=subprocess.run([gh,'codespace','ssh','-c','glowing-train-p7j9gp74q6jwc76v6','--','python3 -c '+shlex.quote(code)],input=token,stdout=out,stderr=subprocess.STDOUT)
del token
print('XCODE_UPLOAD_EXIT',r.returncode)
if r.returncode:print((p/'xcode-authenticated-upload.log').read_text(encoding='utf-8',errors='replace')[-1000:])
raise SystemExit(r.returncode)
