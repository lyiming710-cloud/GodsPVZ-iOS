from pathlib import Path,PurePosixPath
import argparse,hashlib,json,zipfile,subprocess,shutil
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
def main():
 p=argparse.ArgumentParser();p.add_argument('parent_zip',type=Path);p.add_argument('native25_zip',type=Path);p.add_argument('destination',type=Path);p.add_argument('--sha256',required=True);a=p.parse_args();assert sha(a.parent_zip)=='c6fcfa890d4b23266a1a5dae3a488c22112470a5f961a1980a78c2854b90bd16';assert sha(a.native25_zip)==a.sha256
 root=a.destination.resolve();assert not root.exists(),'Destination must be new';root.mkdir(parents=True)
 parent_tool=Path(__file__).resolve().parents[2]/'native24/checkpoint/restore_checkpoint.py'
 subprocess.run(['python3',str(parent_tool),str(a.parent_zip),str(root/'parent'),'--sha256',sha(a.parent_zip)],check=True)
 with zipfile.ZipFile(a.native25_zip) as z:
  manifest=json.loads(z.read('MANIFEST.json'));assert len(z.namelist())==len(set(z.namelist())) and set(z.namelist())==set(manifest['files'])|{'MANIFEST.json'}
  for name,record in manifest['files'].items():
   rel=PurePosixPath(name);assert not rel.is_absolute() and '..' not in rel.parts and '\\' not in name
   data=z.read(name);assert len(data)==record['bytes'] and hashlib.sha256(data).hexdigest()==record['sha256'],name
   out=root/'native25'/Path(*rel.parts);out.parent.mkdir(parents=True,exist_ok=True);out.write_bytes(data)
 target=root/'generated-native25/cpp';shutil.copytree(root/'parent/generated/raw/cpp',target)
 for f in (root/'native25/generated-delta').iterdir():shutil.copyfile(f,target/f.name)
 expected=json.loads((root/'native25/work/IL2CPP-RESULT.json').read_text())['generated_file_hashes'];assert set(p.name for p in target.iterdir() if p.is_file())==set(expected)
 for name,value in expected.items():assert sha(target/name)==value,name
 assert sha(root/'native25/work/native25-final1.dll')=='9cbf08472d6c22f1b68393656ac2d36576fd6b21d49c4738f80aec2e2cbe749a'
 print(json.dumps({'native25_archive_files_verified':len(manifest['files']),'generated_files_verified':len(expected),'candidate_sha256':sha(root/'native25/work/native25-final1.dll'),'destination':str(root)}))
if __name__=='__main__':main()
