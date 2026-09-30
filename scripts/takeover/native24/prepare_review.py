"""Verify the original review archive and restore only named analysis inputs."""
import argparse
import hashlib
import json
from pathlib import Path
import zipfile

ROOT = Path(__file__).resolve().parents[3]
EXPECTED = 'b7565f28d68a91620d42f9b6d154d7943beefe9a5932ece290eadf93fe994aed'


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--work', type=Path, required=True)
    args = parser.parse_args()
    archive = ROOT/'Recovery/Native23-Review-2026-09-30/full-review-evidence.zip'
    assert hashlib.sha256(archive.read_bytes()).hexdigest() == EXPECTED
    args.work.mkdir(parents=True, exist_ok=True)
    with zipfile.ZipFile(archive) as source:
        manifest = json.loads(source.read('archive-manifest.json'))
        assert len(manifest) == 1259 and len(source.namelist()) == 1260
        for name, record in manifest.items():
            payload = source.read(name)
            assert len(payload) == record['bytes'] and hashlib.sha256(payload).hexdigest() == record['sha256'], name
        wanted = ['inputs/baseline.dll', 'inputs/edits-batch1.json', 'inputs/edits-batch2.json',
                  'inputs/edits-batch2a.json', 'review/analysis.json', 'review/status.json',
                  'review/all-methods.baseline.json', 'review/all-methods.batch1.json',
                  'review/all-methods.batch2a.json', 'review/batch1.dll']
        for name in wanted:
            target = args.work/name
            target.parent.mkdir(parents=True, exist_ok=True)
            payload = source.read(name)
            if target.exists():
                assert target.read_bytes() == payload, 'refusing to overwrite different input '+name
            else:
                target.write_bytes(payload)
    report = {'review_zip_sha256': EXPECTED, 'files_verified': len(manifest),
              'restored_inputs': {name: manifest[name] for name in wanted},
              'purpose': 'analysis fixtures only; no Unity project or game runtime selected'}
    (args.work/'input-locks.json').write_text(json.dumps(report, indent=2)+'\n')
    print(json.dumps(report, indent=2))


if __name__ == '__main__':
    main()
