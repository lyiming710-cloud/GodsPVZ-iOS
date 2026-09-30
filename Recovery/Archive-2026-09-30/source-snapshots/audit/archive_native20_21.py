from pathlib import Path
import json,hashlib,shutil,subprocess
base=Path(__file__).parent;root=base/'native19-work';sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
for stage,count,negative,nontarget in [(20,73,112,2224),(21,38,99,2259)]:
    evidence=base/f'native{stage}-evidence';out=root/f'Recovery/Native{stage}-2026-09-29';out.mkdir(parents=True,exist_ok=True)
    candidate=json.loads((evidence/'replay/candidate.json').read_text());q=json.loads((evidence/f'cpp-final{count}/qualification.json').read_text())
    assert q['candidate']==candidate['outputs'] and len(q['target_methods'])==count
    assert sum(m['diagnostics'] for m in q['target_methods'])==0
    for kind in ['linked','unlinked']:
        dll=evidence/f'replay/native{stage}-{kind}.dll';assert sha(dll)==candidate['outputs'][kind]
        report=json.loads((evidence/f'replay/native{stage}-{kind}.dll.targets.typed-negative.json').read_text())
        assert report['status']=='PASS' and report['targets']==count and report['negative_controls']==negative
        assert f'NON_TARGET_ISOLATION_PASS stage=reopened methods={nontarget}' in (evidence/f'replay/patch-{kind}.log').read_text()
        dest=out/'candidates'/dll.name;dest.parent.mkdir(exist_ok=True);shutil.copy2(dll,dest)
    for folder in ['replay',f'cpp-final{count}']:
        for source in (evidence/folder).iterdir():
            if source.suffix=='.dll' or source.name=='all-methods.integer-audit.json':continue
            dest=out/'validation'/folder/source.name;dest.parent.mkdir(parents=True,exist_ok=True)
            dest.write_bytes(source.read_bytes().replace(b'\r\n',b'\n'))
    result={'status':'PAUSED_FOR_HANDOFF','ipa_exported':False,'candidate':candidate['outputs'],'targets':count,'typed_negative_controls_per_candidate':negative,'unreachable_instructions':report['unreachable'],'normalized_non_targets':nontarget,'cpp_diagnostics':q['diagnostics'],'cpp_associated_methods':q['associated_methods'],'target_cpp_diagnostics':0,'full_game_cpp':'FAIL','native_semantic_equivalence':'NOT_INDEPENDENTLY_PROVEN_FOR_EVERY_TARGET'}
    (out/'RESULT.json').write_bytes((json.dumps(result,indent=2)+'\n').encode())
    print(stage,json.dumps(result))
historical=root/'Recovery/Historical-Handoff-2026-09-27.md'
historical.write_bytes((base/'handoff-2026-09-27/HANDOFF.md').read_bytes().replace(b'\r\n',b'\n'))
git='C:/Users/86136/.cache/codex-runtimes/codex-primary-runtime/dependencies/native/git/cmd/git.exe'
current=subprocess.check_output(['C:/Program Files/GitHub CLI/gh.exe','run','list','-R','lyiming710-cloud/GodsPVZ-iOS','--limit','5','--json','databaseId,headSha,name,status,conclusion,url'])
(root/'Recovery/Native21-2026-09-29/latest-github-runs.json').write_bytes(current.replace(b'\r\n',b'\n'))
print('ARCHIVE_VALIDATION_PASS')
