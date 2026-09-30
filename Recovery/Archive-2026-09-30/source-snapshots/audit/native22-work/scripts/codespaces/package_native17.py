"""Fail-closed candidate packaging after the exact native17 conversion gate."""
from pathlib import Path
import hashlib,json,shutil,subprocess
root=Path(__file__).resolve().parents[2]
report=json.loads((root/'.validation/native17/latest-result.json').read_text())
assert report['status']=='NATIVE17_DIRECT_CONVERSION_PASS'
assert report['native17']['exit']==0 and report['native17']['methods']==[]
expected={'unlinked':'74c7474f0592d3f577f8376d75d26528624798794983b3f07db5fd973e7456b2','linked':'9bf2d7a033632061e8e71d32c70312c3c234298649495afc384d0dfe7120a125'}
out=root/'.validation/native17/package';out.mkdir(parents=True,exist_ok=True)
for label,want in expected.items():
    source=Path(report['outputs'][label]['path'])
    assert hashlib.sha256(source.read_bytes()).hexdigest()==want
source=Path(report['outputs']['unlinked']['path'])
shutil.copy2(source,out/'Assembly-CSharp-native17.dll')
text=subprocess.check_output(['dotnet','run','--project',str(root/'scripts/codespaces/InspectMethod'),'--',str(source),'FTRuntime.Internal.SwfList`1','AssignTo','1','System.Collections.Generic.List`1<T>'],cwd=root,text=True)
names=[line.split('=',1)[1] for line in text.splitlines() if line.startswith('ASSEMBLY=')]
assert len(names)==1 and names[0] in ('Assembly-CSharp','GodsPVZRuntime1'),names
(out/'candidate-assembly-name.txt').write_text(names[0]+'\n')
(out/'candidate-inspection.txt').write_text(text)
(out/'PROVENANCE.json').write_text(json.dumps(report,indent=2)+'\n')
print('NATIVE17_CONVERSION_QUALIFIED_PACKAGE_PASS',expected['unlinked'],names[0])
