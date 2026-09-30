from pathlib import Path
import subprocess,urllib.request,urllib.error,json
R=Path(__file__).resolve().parent
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
p=subprocess.run([git,'credential','fill'],cwd=R.parent/'stage9-native3/repository',input='protocol=https\nhost=github.com\npath=lyiming710-cloud/GodsPVZ-iOS.git\n\n',text=True,capture_output=True,check=True)
c=dict(l.split('=',1) for l in p.stdout.splitlines() if '=' in l)
req=urllib.request.Request('https://api.github.com/user/codespaces/glowing-train-p7j9gp74q6jwc76v6',headers={'Authorization':'Bearer '+c['password'],'Accept':'application/vnd.github+json','User-Agent':'Codex-GodsPVZ-recovery','X-GitHub-Api-Version':'2022-11-28'})
try:
 with urllib.request.urlopen(req,timeout=30) as response: data=json.load(response)
 selected={k:data.get(k) for k in ['name','display_name','state','machine','git_status','devcontainer_path','last_used_at','idle_timeout_minutes']}
 selected['repository']=data.get('repository',{}).get('full_name')
 (R/'codespace-probe.json').write_text(json.dumps(selected,indent=2))
 print(json.dumps(selected,indent=2))
except urllib.error.HTTPError as e:
 print('Codespaces API status:',e.code)
 print(e.read().decode()[:700])
