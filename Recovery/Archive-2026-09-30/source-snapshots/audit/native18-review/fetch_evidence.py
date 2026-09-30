import pathlib,subprocess,base64,hashlib
out=pathlib.Path(__file__).parent/'review-evidence'
remote='/workspaces/GodsPVZ-native14-check/.validation/native14/replay-edye4slp/'
files=['native17-unlinked.dll','native18-unlinked.dll','canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp/GodsPVZRuntime1__9.cpp','canary/Library/Bee/artifacts/iOS/il2cppOutput/cpp/GodsPVZRuntime1__8.cpp']
for file in files:
 p=subprocess.run(['C:/Program Files/GitHub CLI/gh.exe','codespace','ssh','-c','glowing-train-p7j9gp74q6jwc76v6','--','base64 -w0 '+remote+file],stdout=subprocess.PIPE,stderr=subprocess.PIPE)
 if p.returncode: raise RuntimeError(p.stderr.decode(errors='replace'))
 data=base64.b64decode(p.stdout,validate=True)
 (out/pathlib.PurePosixPath(file).name).write_bytes(data)
 print(file,len(data),hashlib.sha256(data).hexdigest(),flush=True)
