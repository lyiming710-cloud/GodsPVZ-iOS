from pathlib import Path
import subprocess,json,hashlib
p=Path(__file__).parent;gh='C:/Program Files/GitHub CLI/gh.exe';repo='lyiming710-cloud/GodsPVZ-iOS';tag='recovery-archive-2026-09-30'
releases=json.loads(subprocess.check_output([gh,'api','repos/'+repo+'/releases'],text=True));release=next(x for x in releases if x['tag_name']==tag)
actual={a['name']:a for a in release['assets']};expected=json.loads((p/'ASSET-SHA256.json').read_text(encoding='utf-8'))
for name in ['ASSET-SHA256.json','SHA256SUMS.txt']:
 data=(p/name).read_bytes();expected[name]={'bytes':len(data),'sha256':hashlib.sha256(data).hexdigest()}
assert len(actual)==len(expected),(len(actual),len(expected))
results=[]
for name,e in expected.items():
 a=actual[name];assert a['state']=='uploaded',a;assert a['size']==e['bytes'],(name,a['size'],e['bytes']);assert a.get('digest')=='sha256:'+e['sha256'],(name,a.get('digest'),e['sha256'])
 results.append({'name':name,'bytes':a['size'],'sha256':e['sha256'],'server_digest_verified':True,'id':a['id'],'url':a['browser_download_url']})
result={'release_id':release['id'],'tag':tag,'draft':release['draft'],'prerelease':release['prerelease'],'assets':results,'bytes':sum(x['bytes'] for x in results),'server_size_digest_and_uploaded_state':'PASS'}
(p/'RELEASE-VERIFICATION.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(json.dumps({'assets':len(results),'bytes':result['bytes'],'verification':'PASS','draft':result['draft']}))
