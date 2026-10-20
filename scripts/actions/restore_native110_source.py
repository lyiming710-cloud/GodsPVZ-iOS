"""Authenticate source delivery only; never execute the contained patcher or tools."""
import argparse,hashlib,json,pathlib,stat,zipfile
ZIP_SHA='6091ec35681dabf63db93abed5f4de1de242e54f10044a6aa07704d4c687fc4d'
MANIFEST_SHA='210de1d5055b68b9737c4ccc1a3a4bf0e6de414aaecc089ffda023d5c4604b3d'
SOURCE_SHA='63f1e651fce1576110482efd3d70bca104e938683bd3d20f150022ae3aa8e793'
def sha(b):return hashlib.sha256(b).hexdigest()
def main():
    p=argparse.ArgumentParser();p.add_argument('--archive',type=pathlib.Path,required=True);p.add_argument('--destination',type=pathlib.Path,required=True)
    a=p.parse_args();assert sha(a.archive.read_bytes())==ZIP_SHA,'Source archive digest mismatch'
    root=a.destination.resolve();root.mkdir(parents=True,exist_ok=False)
    with zipfile.ZipFile(a.archive) as z:
        assert len(z.namelist())==len(set(z.namelist())), 'Duplicate entry'
        manifest_bytes=z.read('CAPSULE-MANIFEST.json');assert sha(manifest_bytes)==MANIFEST_SHA
        m=json.loads(manifest_bytes);entries={x['path']:x for x in m['files']}
        assert set(z.namelist())==set(entries)|{'CAPSULE-MANIFEST.json'}
        for f in z.infolist():
            n=pathlib.PurePosixPath(f.filename);assert not n.is_absolute() and '..' not in n.parts and '\\' not in f.filename
            assert not stat.S_ISLNK(f.external_attr>>16), 'Symlink refused'
            b=z.read(f)
            if f.filename in entries:
                expected=entries[f.filename];assert len(b)==expected['bytes'] and sha(b)==expected['sha256'],f.filename
            target=root/f.filename;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(b)
    assert sha((root/'src/SOURCE-MANIFEST.json').read_bytes())==SOURCE_SHA
    print(json.dumps({'source_sha256':SOURCE_SHA,'authenticated_payload_files':len(entries),'project_tests_executed':False}))
if __name__=='__main__':main()
