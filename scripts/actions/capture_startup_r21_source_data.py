"""Small source-only archive for serialization diagnosis; never launches Unity."""
from pathlib import Path
import argparse,hashlib,json,re,zipfile
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
def capture(project,output):
 root=Path(project).resolve();assets=root/'Assets';out=Path(output).resolve();assert (root/'ProjectSettings/ProjectVersion.txt').is_file();out.mkdir(exist_ok=True,parents=True);target=out/'r21-source-prefabs-scenes-audio-metadata.zip';assert not target.exists();files=[];audio=[]
 for p in sorted(assets.rglob('*')):
  if not p.is_file():continue
  assert not p.is_symlink() and p.resolve().is_relative_to(root);rel=p.relative_to(root).as_posix()
  if p.suffix in ['.prefab','.unity'] or (p.name.endswith(('.prefab.meta','.unity.meta','.cs.meta'))):files.append(p)
  if p.suffix.lower() in ['.wav','.ogg','.mp3','.aif','.aiff','.flac']:
   meta=p.with_suffix(p.suffix+'.meta');assert meta.is_file();b=meta.read_bytes();matches=re.findall(rb'(?m)^guid: ([0-9a-fA-F]{32})\s*$',b);assert len(matches)==1;audio.append(dict(file=rel,bytes=p.stat().st_size,sha256=sha(p),meta_sha256=sha(meta),guid=matches[0].decode().lower(),basename=p.stem));files.append(meta)
 plugin=assets/'Plugins/Assembly-CSharp.dll'
 if plugin.is_file():files.append(plugin)
 manifest=[]
 with zipfile.ZipFile(target,'w',zipfile.ZIP_DEFLATED) as z:
  for p in sorted(set(files)):
   name=p.relative_to(root).as_posix();b=p.read_bytes();manifest.append(dict(file=name,bytes=len(b),sha256=hashlib.sha256(b).hexdigest()));z.writestr(name,b)
  z.writestr('AUDIO-SOURCE-INVENTORY.json',json.dumps(audio,indent=2)+'\n');z.writestr('MANIFEST.json',json.dumps(manifest,indent=2)+'\n')
 with zipfile.ZipFile(target) as z:
  assert z.testzip() is None
  for row in manifest:b=z.read(row['file']);assert len(b)==row['bytes'] and hashlib.sha256(b).hexdigest()==row['sha256']
 result=dict(scope='SOURCE_PREFABS_SCENES_META_AND_ORIGINAL_PROJECT_DLL_ONLY_NO_UNITY_OR_GAME_TEST',archive=target.name,bytes=target.stat().st_size,sha256=sha(target),source_files=len(manifest),audio_sources=len(audio),game_data_changed=False,unity_started=False);(out/'r21-source-data-capture.json').write_bytes((json.dumps(result,indent=2)+'\n').encode());print(json.dumps(result))
if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--output',required=True);a=p.parse_args();capture(a.project,a.output)
