"""Restore and authenticate the immutable Codespace checkpoint; no project execution."""
import argparse, hashlib, json, pathlib, tarfile

ARCHIVE_SHA = '40603568bae03a8e99ef9530650e6927d400e8503cabfcc58fc86ec92cbb3947'
MANIFEST_SHA = '9cafa4298205d4156f5c7f6e8a4c8896877e7bc2c6c94e6eedabc8688275e2e1'
CANDIDATE_SHA = '8ef0f2bc62658bd7a4bdeb2cb25a10cadaa9f5418e8f97910f7ac1b9604823b1'

def sha(p):
    h = hashlib.sha256()
    with p.open('rb') as f:
        for chunk in iter(lambda: f.read(1024 * 1024), b''): h.update(chunk)
    return h.hexdigest()

def main():
    a = argparse.ArgumentParser()
    a.add_argument('--downloads', type=pathlib.Path, required=True)
    a.add_argument('--destination', type=pathlib.Path, required=True)
    a.add_argument('--receipt', type=pathlib.Path, required=True)
    args = a.parse_args()
    manifest_path = args.downloads / 'PORTABLE-MANIFEST.json'
    archive_path = args.downloads / 'PORTABLE-BULK152-CHECKPOINT.tar.gz'
    assert sha(manifest_path) == MANIFEST_SHA, 'Manifest digest mismatch'
    manifest = json.loads(manifest_path.read_text())
    assert manifest['archive']['sha256'] == ARCHIVE_SHA
    assert sha(archive_path) == ARCHIVE_SHA, 'Archive digest mismatch'
    assert archive_path.stat().st_size == manifest['archive']['bytes']
    root = args.destination.resolve()
    root.mkdir(parents=True, exist_ok=False)
    with tarfile.open(archive_path, 'r:gz') as tf:
        members = tf.getmembers()
        names = set()
        for m in members:
            path = pathlib.PurePosixPath(m.name)
            assert not path.is_absolute() and '..' not in path.parts
            assert m.isfile(), 'Non-regular archive member refused: ' + m.name
            assert m.name not in names, 'Duplicate archive member: ' + m.name
            names.add(m.name)
        assert names == {f['path'] for f in manifest['files']}, 'File set mismatch'
        tf.extractall(root, filter='data')
    for f in manifest['files']:
        p = root / f['path']
        assert p.stat().st_size == f['bytes'], f['path']
        assert sha(p) == f['sha256'], f['path']
    assert sha(root / 'work/candidate1.dll') == CANDIDATE_SHA
    assert sha(root / 'original/GameAssembly.dll') == '9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
    receipt = {'archive_sha256': ARCHIVE_SHA, 'manifest_sha256': MANIFEST_SHA,
               'candidate_sha256': CANDIDATE_SHA, 'authenticated_files': len(manifest['files']),
               'scope': 'file restoration only; no behavioral or build qualification'}
    args.receipt.parent.mkdir(parents=True, exist_ok=True)
    args.receipt.write_text(json.dumps(receipt, indent=2))
    print(json.dumps(receipt))

if __name__ == '__main__': main()
