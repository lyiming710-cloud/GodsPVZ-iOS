from pathlib import Path
import json,hashlib,zipfile,subprocess,re
base=Path(__file__).parent;root=base/'native19-work';out=base/'handoff-2026-09-29';out.mkdir(exist_ok=True)
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
head=subprocess.check_output([git,'-C',str(root),'rev-parse','HEAD'],text=True).strip()
package=out/'GodsPVZ-Native21-Handoff-2026-09-29.zip'
subprocess.run([git,'-C',str(root),'archive','--format=zip','--prefix=repo/','-o',str(package.resolve()),head,'Recovery','scripts/codespaces','scripts/takeover','.github/workflows'],check=True)
extras={
 'locked-inputs/Assembly-CSharp-59bb-native2.dll':base.parent/'Assembly-CSharp-59bb-native2.dll',
 'locked-inputs/GameAssembly.dll':base/'stage9-native4/inputs/GameAssembly.dll',
 'locked-inputs/global-metadata.dat':base/'stage9-native4/inputs/global-metadata.dat',
 'locked-inputs/native-method-map.json':base/'stage9-native4/evidence/native-method-map.json',
 'locked-inputs/native-fields.json':base/'stage9-native4/evidence/native-fields.json',
 'candidates/native19-linked.dll':base/'native19-evidence/replay/native19-linked.dll',
 'candidates/native19-unlinked.dll':base/'native19-evidence/replay/native19-unlinked.dll',
 'native18-independent-review/REVIEW.md':base/'native18-review/REVIEW-64b86ed-2026-09-29.md',
}
for source in (base/'native18-review/review-evidence').iterdir():
    if source.is_file() and source.suffix in {'.txt','.json','.log','.tsv','.cpp'}:
        extras['native18-independent-review/review-evidence/'+source.name]=source
for name in ['compare_raw.py','Inspect.cs','codespace_review.cs','run_codespace_review.py']:
    extras['native18-independent-review/'+name]=base/'native18-review'/name
with zipfile.ZipFile(package,'a',compression=zipfile.ZIP_DEFLATED,compresslevel=6) as z:
    for name,source in extras.items():z.writestr(name,source.read_bytes())
    z.writestr('START-HERE.txt',f'''GodsPVZ Native21 paused handoff, 2026-09-29
Git commit: {head}
Branch: repair/codex-native19-ipa
Read repo/Recovery/HANDOFF-2026-09-29-Native21.md first.
Next AI prompt: repo/Recovery/TAKEOVER-PROMPT-Native21.md
No IPA. Whole-game C++ still fails. Native21 is an experimental candidate.
Includes tracked recovery/code/workflows, candidate DLLs, original PC native inputs and selected independent review evidence.
Does not contain the full Unity Editor, full Unity asset project, external caches, credentials or licenses.
Use the repository and existing Codespace for the full environment; do not treat this archive as a clean-machine bootstrap.
''')
    manifest={name:hashlib.sha256(z.read(name)).hexdigest() for name in z.namelist() if not name.endswith('/')}
    z.writestr('FILES-SHA256.json',json.dumps(manifest,indent=2)+'\n')
with zipfile.ZipFile(package) as z:
    assert z.testzip() is None
    assert len(z.namelist())==len(set(z.namelist()))
    for name,want in json.loads(z.read('FILES-SHA256.json')).items():assert hashlib.sha256(z.read(name)).hexdigest()==want,name
digest=hashlib.sha256(package.read_bytes()).hexdigest()
(out/(package.name+'.sha256')).write_bytes((digest+'  '+package.name+'\n').encode())
(out/'PACKAGE.json').write_bytes((json.dumps({'commit':head,'branch':'repair/codex-native19-ipa','files':len(manifest)+1,'size':package.stat().st_size,'sha256':digest,'zip_crc':'PASS','ipa_exported':False,'status':'PAUSED_FOR_HANDOFF'},indent=2)+'\n').encode())
print((out/'PACKAGE.json').read_text())
