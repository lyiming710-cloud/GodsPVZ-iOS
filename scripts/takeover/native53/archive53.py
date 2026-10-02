from pathlib import Path
import subprocess,json,hashlib,shutil,zipfile,tempfile
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native53-2026-10-02';r=w/'Recovery/Native53-2026-10-02';r.mkdir(exist_ok=True)
assert json.loads((n/'COMPILER-COMPLETE.json').read_text())['exit']==0
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
cpp=json.loads((n/'CPP-STATUS.json').read_text());ci=json.loads((n/'CPP-ISOLATION.json').read_text());fixture=json.loads((n/'RUNTIME-FIXTURE.json').read_text());iso=json.loads((n/'ISOLATION.json').read_text());neg=json.loads((n/'ACTUAL-DLL-NEGATIVES.json').read_text());targets={row['token'] for row in json.loads((n/'patch1.json').read_text())['methods']}
assert fixture['gate_pass'] and iso['pass'] and not cpp['pass_to_fail_methods'] and not ci['differing_non_target_methods'] and len(targets)==4
assert all(not cpp['targets'][t]['errors'] for t in targets) and len(neg)==4 and all(row['tool_errors']==0 for row in neg)
q={'candidate_sha256':sha(n/'candidate1.dll'),'parent_dll_sha256':sha(w/'.validation/native52-2026-10-02/candidate1.dll'),'targets':4,'non_target_raw_bodies_equal':iso['non_target_raw_bodies_equal'],'declarations_equal':True,'typed_targets_pass':4,'clr_positive_cases':fixture['positive_cases'],'emitter_negative_controls':fixture['negative_controls'],'actual_dll_negative_controls':4,'tool_errors':0,'cpp_diagnostics':cpp['errors'],'cpp_errored_methods':cpp['mapping']['methods_with_errors'],'cpp_failed_units':cpp['failed'],'cpp_TUs':cpp['TUs'],'non_target_cpp_equal':ci['non_target_mapped_cpp_methods_equal'],'ipa_produced':False,'status':'batch-qualified; whole assembly not qualified'}
(n/'QUALIFICATION.json').write_text(json.dumps(q,indent=2))
contracts={'0x060001A9':'Capture board/config; GetGridPosition(gridX,row), ignore ID; X+57/Y+28/Z unchanged by value. Native constants independently pinned.', '0x06000188':'Capture Grid from Board.GetGrid; null Grid returns null; Unity op_Implicit tests top/sheath/common/bottom; reread accepted field after dependency callback.', '0x0600083D':'Capture current frame/Labels; missing frame/array or negative/out-of-range signed index returns String.Empty; valid selected null remains null.', '0x06000903':'Capture ctrl.clip.sequence once, then loop: read current idle array length, Range(0,length), reread current static array, load selected string; compare against captured prior string; return when unequal. Preserve no retry cap and null/bounds order.'}
evidence=json.loads((n/'NATIVE-EVIDENCE.json').read_text());review=[{k:row[k] for k in ['token','method','start','end','boundary_scope','native_sha256']}|{'contract':contracts[row['token']]} for row in evidence['methods'] if row['token']in targets]
(n/'NATIVE-REVIEW.json').write_text(json.dumps({'GameAssembly_sha256':evidence['GameAssembly_sha256'],'methods':review,'oracle_preconditions':'Four unchanged original PC callers; initialized metadata/declared static identities, typed alias graphs and recording grid/Unity truth/frame/random/string comparison/exception dependencies. Original native float/bounds/priority/captured value/array reread/loop behavior observed. CLR executes actual candidate IL with pinned real Vector3, real String.Empty and string inequality in recording wrapper. Distinct equal-content and nullable strings included. Nonterminating singleton/constant selection only prefix-observed under scripted ninth dependency throw; extra 32-call CLR mutant containment never triggers on positive candidate. No original Unity/random/string/class-init/throw/dependency implementation, iOS or full game proof.','diagnostics_baseline':evidence.get('diagnostics_baseline')},indent=2))
readme=f"""# Native53 — coordinate, device, frame label and idle sequence callers

Candidate `{q['candidate_sha256']}` derives from Native52 `{q['parent_dll_sha256']}`. Four original native-backed target contracts are listed in NATIVE-REVIEW.json and complete instruction/disassembly evidence.

* EnemySelecter.GetRandomPosition (0x060001A9): BoardConfig.GetGridPosition(gridX,row), then X+57, Y+28, original Z. ID is unused. Constants come from original PE bytes, rather than the misleading method name or damaged/uninitialized managed locals.
* DeviceManager.GetDevice (0x06000188): capture returned Grid; null Grid returns null; Unity truth tests top, sheath, common, bottom. Reread accepted field after the test, including callback replacement or nulling. Reference null checks cannot replace Unity truth.
* SwfClip.GetCurrentFrameLabel (0x0600083D): null frame/Labels and invalid signed index return String.Empty. A valid array entry can itself be null and must remain null.
* PurpleFlowerLogic.GetRandomIdleSequence (0x06000903): capture previous clip sequence once; read static idle array length, call Random.Range, reread static array, capture selected string, compare with the captured previous string, retry only while equal. Null/bounds exceptions and callback timing preserved. No bounded retry/default return added. A singleton equal to the previous string need not terminate.

Only four existing method slots change; unused original code padding is zero. Entire metadata/reference/PE bytes remain unchanged, and {q['non_target_raw_bodies_equal']} non-target raw bodies match. Three targets retain original locals; idle selection adopts existing immutable StandAloneSig 0x110001AA from ResourceManager.LoadSprites (string,int,string,Sprite), using V0 previous/V1 index/V2 selected and leaving Sprite unused. The source signature, metadata blob and source method remain byte-identical. The auditor independently checks complete opcode/operand/branch/constants/locals contracts and derives bounded PE scope without trusting forged hashes.

16,384 original native caller records match actual candidate CIL executions: float raw bits/NaN/signed zero, grid arguments, nulls/dependency exceptions, all device truth masks, field callback rereads, label boundaries and null entries, static-array replacement/shortening/nulling after Range, equal-content distinct strings, previous-sequence callback mutation, retries and finite prefixes of nontermination. Native caller bytes remain untouched while called dependencies are explicitly supplied recordings. Native metadata/static identities are initialized. Actual CLR uses the pinned public CoreModule Vector3 and public System.String fields/operators. Original Unity/random/string/engine/class-init/throw implementations and whole-game behavior are not proved. A separate 32-call CLR harness guard contains erroneous variants and never fires on the positive candidate.

Twelve emitted bad variants, four actual bad DLLs (wrong offset/priority/bounds/loop), ten forged-report contract/scope corruptions, Native24–52 regressions and the unchanged Native40 typed verifier pass their detection checks, with zero tool errors. All four targets pass the verifier, and non-target verdicts match parent. Original 14 field identities/+57/+28 constants and reused local signature are independently inspected and replayed.

Full IL2CPP conversion plus {q['cpp_TUs']} Linux Clang syntax checks clear all four targets and retain {q['non_target_cpp_equal']} other C++ bodies. Remaining: {q['cpp_errored_methods']} errored methods, {q['cpp_diagnostics']} diagnostics, {q['cpp_failed_units']} failing units. Generated nonliteral metadata table payloads and non-target literals remain unchanged. This batch is qualified within those scopes; the assembly remains unqualified. No licensed Unity/Xcode/IPA/device build was run.

`replay_native53.py ARCHIVE.zip FRESH_DIRECTORY` checks every archive member, two exact candidate reproductions, bit-identical original native observations, real candidate CIL, emitted/actual/scope negative controls, field constants/signature proofs and generated metadata. Full compiler results are archived; replay does not rerun all Clang units. Requires Python3/.NET10/Linux x64/Clang.

Tool development issues were corrected before qualification: metadata namespace import in the auditor, manually specified short-branch offsets, and missing fixture containment of deliberately wrong loop variants. Initial singleton diagnostic observation also needed an explicit recording-dependency limit. Wrong fallback control now returns an existing alternate string identity, allowing a behavioral mismatch to be reported without a fixture identity error. Candidate bytes were never adjusted to suppress these tests; final gates have zero tool errors.
"""
(r/'README.md').write_text(readme)
for name in ['QUALIFICATION.json','NATIVE-REVIEW.json','ISOLATION.json','RUNTIME-FIXTURE.json','CPP-STATUS.json','CPP-ISOLATION.json','ACTUAL-DLL-NEGATIVES.json','REGRESSION.json','AUDIT-NEGATIVE.json','BYTE-AUDIT-NEGATIVES.json','TYPED-NON-TARGET-ISOLATION.json','patch1.json','FIELD-CONSTANT-PROOF.json','LOCAL-SIGNATURE-PROOF.json']:shutil.copyfile(n/name,r/name)
shutil.copyfile(n/'native53_oracle.cpp',w/'scripts/takeover/native53/native53_oracle.cpp')
files={}
for p in n.rglob('*'):
 if p.is_file():files['work/'+str(p.relative_to(n))]=p
for p in (n/'fresh').rglob('*'):
 if p.is_file():files['work/fresh/'+str(p.relative_to(n/'fresh'))]=p
for p in (w/'scripts/takeover/native53').rglob('*'):
 if p.is_file() and p.suffix in ['.cs','.csproj','.py','.md','.cpp'] and not any(part in ['bin','obj'] for part in p.parts):files['sources/takeover/native53/'+str(p.relative_to(w/'scripts/takeover/native53'))]=p
files['inputs/native52-parent.dll']=w/'.validation/native52-2026-10-02/candidate1.dll'
native=Path('/workspaces/GodsPVZ-native19/.validation/native22/pcnative')
for name in ['GameAssembly.dll','native-method-map.json','native-fields.json']:files['original/'+name]=native/name
files['support/Mono.Cecil.dll']=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
files['support/INPUT-SUPPORT-LOCK.json']=w/'.validation/native25-2026-10-01/native24-restored/work/INPUT-SUPPORT-LOCK.json'
files['sources/frozen-native40/verify_types.py']=w/'scripts/takeover/native40/verify_types.py'
files['README.md']=r/'README.md'
for tag in ['native52','native53']:files['supplement/'+tag+'-generated-metadata.dat']=w/('.validation/'+tag+'-2026-10-02/fresh/data/Metadata/global-metadata.dat')
files['supplement/GENERATED-METADATA-AUDIT.json']=n/'GENERATED-METADATA-AUDIT.json'
files['sources/audit_generated_metadata.py']=w/'scripts/takeover/native40/audit_generated_metadata.py'
files['support/UnityEngine.CoreModule.dll']=w/'.validation/native25-2026-10-01/native24-restored/managed-support/UnityEngine.CoreModule.dll'
assert sha(files['support/UnityEngine.CoreModule.dll'])=='d4d9c2d8f48af011f2dec31328a482794eceba9f3eece005623448443c184f50'
shutil.copyfile(n/'GENERATED-METADATA-AUDIT.json',r/'GENERATED-METADATA-AUDIT.json')
manifest={name:{'bytes':p.stat().st_size,'sha256':sha(p)} for name,p in sorted(files.items())}
archive=Path('/tmp/native53-2026-10-02-evidence.zip')
with zipfile.ZipFile(archive,'w',zipfile.ZIP_DEFLATED,compresslevel=6) as z:
 for name,p in sorted(files.items()):z.write(p,name)
 z.writestr('MANIFEST.json',json.dumps(manifest,indent=2))
with zipfile.ZipFile(archive) as z:
 assert set(z.namelist())==set(manifest)|{'MANIFEST.json'}
 for name,row in manifest.items():assert hashlib.sha256(z.read(name)).hexdigest()==row['sha256'],name
a={'archive':str(archive),'sha256':sha(archive),'bytes':archive.stat().st_size,'verified_files':len(manifest),'qualification':q};(r/'ARCHIVE.json').write_text(json.dumps(a,indent=2));print(json.dumps(a))
replay=Path(tempfile.mkdtemp(prefix='native53-replay-'))/'result'
p=subprocess.run(['python3',str(w/'scripts/takeover/native53/replay_native53.py'),str(archive),str(replay)],capture_output=True,text=True);(n/'archive-replay.log').write_text(p.stdout+p.stderr);assert p.returncode==0,p.stdout+p.stderr
result=json.loads((replay/'ARCHIVE-REPLAY.json').read_text());(r/'ARCHIVE-REPLAY.json').write_text(json.dumps(result,indent=2));print(json.dumps(result))
