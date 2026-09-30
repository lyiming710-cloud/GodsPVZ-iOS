from pathlib import Path
import subprocess,base64
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
root=Path(__file__).parent/'native19-work';bundle=Path(__file__).parent/'native19-checkpoint.bundle'
subprocess.run([git,'-C',str(root),'bundle','create',str(bundle.resolve()),'64b86ed725c40e8e00cecd7a917f72bbed37777e..repair/codex-native19-ipa'],check=True)
script='''from pathlib import Path
import subprocess,base64
root=Path('/workspaces/GodsPVZ-native19');bundle=root/'.validation/native19/checkpoint.bundle'
bundle.write_bytes(base64.b64decode(__DATA__))
subprocess.run(['git','fetch',str(bundle),'repair/codex-native19-ipa:refs/remotes/origin/repair/codex-native19-ipa'],cwd=root,check=True)
subprocess.run(['git','reset','--mixed','refs/remotes/origin/repair/codex-native19-ipa'],cwd=root,check=True)
subprocess.run(['git','status','--short'],cwd=root,check=True)
'''.replace('__DATA__',repr(base64.b64encode(bundle.read_bytes()).decode()))
subprocess.run(['C:/Program Files/GitHub CLI/gh.exe','codespace','ssh','-c','glowing-train-p7j9gp74q6jwc76v6','--','python3 -'],input=script.encode(),check=True)
