"""Safely restore the checkpoint's hash-addressed generated source and evidence."""
from pathlib import Path,PurePosixPath
import argparse,json,zipfile,hashlib
def sha(data):return hashlib.sha256(data).hexdigest()
def main():
 p=argparse.ArgumentParser();p.add_argument('archive',type=Path);p.add_argument('destination',type=Path);p.add_argument('--sha256',required=True);a=p.parse_args()
 assert sha(a.archive.read_bytes())==a.sha256,'Archive SHA mismatch'
 root=a.destination.resolve();assert not root.exists(),'Destination must be new'
 with zipfile.ZipFile(a.archive) as z:
  manifest=json.loads(z.read('MANIFEST.json'));assert set(z.namelist())==set(manifest['files'])|{'MANIFEST.json'},'Archive inventory mismatch'
  for name,record in manifest['files'].items():
   path=PurePosixPath(name);assert not path.is_absolute() and '..' not in path.parts and '\\' not in name
   data=z.read(name);assert len(data)==record['bytes'] and sha(data)==record['sha256'],name
   out=root.joinpath(*path.parts);out.parent.mkdir(parents=True,exist_ok=True);out.write_bytes(data)
 generated=json.loads((root/'GENERATED-SOURCE-MAP.json').read_text())
 for stage,files in generated.items():
  for name,hash_value in files.items():
   assert PurePosixPath(name).name==name
   data=(root/'cpp-blobs'/hash_value).read_bytes();assert sha(data)==hash_value
   out=root/'generated'/stage/'cpp'/name;out.parent.mkdir(parents=True,exist_ok=True);out.write_bytes(data)
 print(json.dumps({'archive_files_verified':len(manifest['files']),'generated_stages':{k:len(v) for k,v in generated.items()},'destination':str(root)}))
if __name__=='__main__':main()
