from pathlib import Path
import sqlite3,json,subprocess
root=Path('C:/Users/86136/AppData/Roaming/UnityHub')
for rel in ['hub.db','install-state/install-state.db']:
 p=root/rel
 if not p.exists():continue
 c=sqlite3.connect(p.as_uri()+'?mode=ro',uri=True)
 tables=c.execute("select name from sqlite_master where type='table'").fetchall()
 print(rel,'tables',tables)
 for (name,) in tables:
  if any(k in name.lower() for k in ['editor','install']):
   print(name,'columns',c.execute('pragma table_info("'+name.replace('"','""')+'")').fetchall())
   print('rows',c.execute('select * from "'+name.replace('"','""')+'" limit 10').fetchall())
 c.close()
r=subprocess.run(['wsl.exe','--list','--quiet'],capture_output=True)
print('WSL exit',r.returncode,r.stdout.decode('utf-16-le',errors='replace'),r.stderr.decode('utf-16-le',errors='replace'))
