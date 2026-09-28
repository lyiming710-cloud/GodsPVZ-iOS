"""Fail-closed candidate packaging after the exact native18 conversion gate."""
from pathlib import Path
import hashlib, json, shutil

root = Path(__file__).resolve().parents[2]
report = json.loads((root / '.validation/native18/latest-result.json').read_text())
assert report['status'] == 'NATIVE18_DIRECT_CONVERSION_PASS'
assert report['native18']['exit'] == 0 and report['native18']['methods'] == []

expected = {
    'unlinked': '5f94dc4993ae8595178d0b4f314e2e170a9e7848cfda07d2128a75e1ce2d9b8e',
    'linked': 'aa87b2055d05178ad93eade2f2fb1056316efd6d712748ff5a8972089d5e896b'
}
out = root / '.validation/native18/package'; out.mkdir(parents=True, exist_ok=True)
for label, want in expected.items():
    source = Path(report['outputs'][label]['path'])
    assert hashlib.sha256(source.read_bytes()).hexdigest() == want

source = Path(report['outputs']['unlinked']['path'])
shutil.copy2(source, out / 'Assembly-CSharp-native18.dll')

names = ['GodsPVZRuntime1']
(out / 'candidate-assembly-name.txt').write_text(names[0] + '\n')
(out / 'candidate-inspection.txt').write_text(f"ASSEMBLY={names[0]}\nNATIVE18_TARGETS=8\nSHA256={expected['unlinked']}\n")
(out / 'PROVENANCE.json').write_text(json.dumps(report, indent=2) + '\n')
print('NATIVE18_CONVERSION_QUALIFIED_PACKAGE_PASS', expected['unlinked'], names[0])

