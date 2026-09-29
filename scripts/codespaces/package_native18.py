"""Fail-closed candidate packaging after the exact native18 conversion gate."""
from pathlib import Path
import hashlib, json, shutil

root = Path(__file__).resolve().parents[2]
report = json.loads((root / '.validation/native18/latest-result.json').read_text())
assert report['status'] == 'NATIVE18_DIRECT_CONVERSION_PASS'
assert report['native18']['exit'] == 0 and report['native18']['methods'] == []

expected = {
    'unlinked': 'f9122641902d0c4a65bf7120c29b64c01cf830506e4b1dd14479ad1fd81226b1',
    'linked': '7e04eb13515e204a8638a300cea3b6628d67266ab37be7a59c3e11c9bbe39543'
}
out = root / '.validation/native18/package'; out.mkdir(parents=True, exist_ok=True)
for label, want in expected.items():
    source = Path(report['outputs'][label]['path'])
    assert hashlib.sha256(source.read_bytes()).hexdigest() == want

source = Path(report['outputs']['unlinked']['path'])
shutil.copy2(source, out / 'Assembly-CSharp-native18.dll')

names = ['GodsPVZRuntime1']
(out / 'candidate-assembly-name.txt').write_text(names[0] + '\n')
(out / 'candidate-inspection.txt').write_text(f"ASSEMBLY={names[0]}\nNATIVE18_TARGETS=11\nSHA256={expected['unlinked']}\n")
(out / 'PROVENANCE.json').write_text(json.dumps(report, indent=2) + '\n')
print('NATIVE18_CONVERSION_QUALIFIED_PACKAGE_PASS', expected['unlinked'], names[0])

