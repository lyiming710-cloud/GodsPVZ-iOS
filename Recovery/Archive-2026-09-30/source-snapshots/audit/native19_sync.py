import pathlib,sys,json,base64,subprocess
root=pathlib.Path(__file__).parent/'native19-work';files={}
for arg in sys.argv[1:]:
 p=root/arg
 for f in ([p] if p.is_file() else p.rglob('*')):
  if f.is_file() and not {'bin','obj'}.intersection(f.relative_to(root).parts):files[f.relative_to(root).as_posix()]=base64.b64encode(f.read_bytes()).decode()
script='''import pathlib,base64,json
root=pathlib.Path('/workspaces/GodsPVZ-native19')
files=__PAYLOAD__
for n,data in files.items():
 p=(root/n).resolve();assert p.is_relative_to(root)
 p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(base64.b64decode(data))
print('SYNC_FILES',len(files))
'''.replace('__PAYLOAD__',repr(files))
r=subprocess.run(['C:/Program Files/GitHub CLI/gh.exe','codespace','ssh','-c','glowing-train-p7j9gp74q6jwc76v6','--','python3 -'],input=script.encode());sys.exit(r.returncode)
