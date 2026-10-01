from pathlib import Path,PurePosixPath
import argparse,hashlib,json,zipfile,subprocess,shutil
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
def main():
 p=argparse.ArgumentParser();p.add_argument('native24_zip',type=Path);p.add_argument('native25_zip',type=Path);p.add_argument('native26_zip',type=Path);p.add_argument('destination',type=Path);p.add_argument('--sha256',required=True);a=p.parse_args()
 assert sha(a.native24_zip)=='c6fcfa890d4b23266a1a5dae3a488c22112470a5f961a1980a78c2854b90bd16';assert sha(a.native25_zip)=='90107c6243f94d68b2f65bd578efdeb0dd1b4c7d6d787dc5ec257696988dc2bf';assert sha(a.native26_zip)==a.sha256
 root=a.destination.resolve();assert not root.exists(),'Destination must be new';root.mkdir(parents=True)
 parent_tool=Path(__file__).resolve().parents[2]/'native25/checkpoint/restore_native25.py'
 subprocess.run(['python3',str(parent_tool),str(a.native24_zip),str(a.native25_zip),str(root/'parent'),'--sha256',sha(a.native25_zip)],check=True)
 with zipfile.ZipFile(a.native26_zip) as z:
  manifest=json.loads(z.read('MANIFEST.json'));assert len(z.namelist())==len(set(z.namelist())) and set(z.namelist())==set(manifest['files'])|{'MANIFEST.json'}
  for name,row in manifest['files'].items():
   rel=PurePosixPath(name);assert not rel.is_absolute() and '..' not in rel.parts and '\\' not in name;data=z.read(name);assert len(data)==row['bytes'] and hashlib.sha256(data).hexdigest()==row['sha256'],name
   dest=root/'native26'/Path(*rel.parts);dest.parent.mkdir(parents=True,exist_ok=True);dest.write_bytes(data)
 target=root/'generated-native26/cpp';shutil.copytree(root/'parent/generated-native25/cpp',target)
 for f in (root/'native26/generated-delta').iterdir():shutil.copyfile(f,target/f.name)
 expected=json.loads((root/'native26/work/IL2CPP-RESULT.json').read_text())['generated_file_hashes'];assert {p.name for p in target.iterdir() if p.is_file()}==set(expected)
 for name,value in expected.items():assert sha(target/name)==value,name
 assert sha(root/'native26/work/native26-final1.dll')=='affd9276454e70d6f39d9e28144ba2006021e818b1a617ed517eea90741ca878'
 print(json.dumps({'native26_members_verified':len(manifest['files']),'generated_files_verified':len(expected),'candidate_sha256':sha(root/'native26/work/native26-final1.dll'),'destination':str(root)}))
if __name__=='__main__':main()
