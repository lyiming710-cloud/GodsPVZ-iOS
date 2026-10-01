from pathlib import Path,PurePosixPath
import json,hashlib,zipfile,argparse
def sha(p):
 h=hashlib.sha256()
 with Path(p).open('rb') as f:
  while block:=f.read(1024*1024):h.update(block)
 return h.hexdigest()
def main():
 p=argparse.ArgumentParser();p.add_argument('archive',type=Path);p.add_argument('destination',type=Path);p.add_argument('--sha256',required=True);a=p.parse_args();assert sha(a.archive)==a.sha256
 root=a.destination.resolve();assert not root.exists(),'New destination required';root.mkdir(parents=True)
 with zipfile.ZipFile(a.archive) as z:
  manifest=json.loads(z.read('MANIFEST.json'));assert len(z.namelist())==len(set(z.namelist())) and set(z.namelist())==set(manifest['files'])|{'MANIFEST.json'}
  for name,row in manifest['files'].items():
   rel=PurePosixPath(name);assert not rel.is_absolute() and '..' not in rel.parts and '\\' not in name
   dest=root/Path(*rel.parts);assert dest.resolve().is_relative_to(root);dest.parent.mkdir(exist_ok=True,parents=True);h=hashlib.sha256();size=0
   with z.open(name) as f,dest.open('xb') as output:
    while block:=f.read(1024*1024):h.update(block);size+=len(block);output.write(block)
   assert size==row['bytes'] and h.hexdigest()==row['sha256'],name
  (root/'MANIFEST.json').write_text(json.dumps(manifest,indent=2))
 expected=json.loads((root/'work/IL2CPP-RESULT.json').read_text())['generated_file_hashes'];generated=root/'generated/cpp';assert {f.name for f in generated.iterdir() if f.is_file()}==set(expected)
 for name,value in expected.items():assert sha(generated/name)==value,name
 assert sha(root/'work/native27-final1.dll')=='ae6ac972e0615666091b7359774f0c0ec70d01d8e5d05d0942324b7d13b34920'
 print(json.dumps({'members_verified':len(manifest['files']),'generated_files_verified':len(expected),'candidate_sha256':sha(root/'work/native27-final1.dll'),'destination':str(root),'scope':'Full Native27 output restored; locked parent archives still required for unchanged support/toolchain inputs'}))
if __name__=='__main__':main()
