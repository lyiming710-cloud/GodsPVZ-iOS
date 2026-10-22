"""Compose source files and declarative manifests only. Never build or run a patcher."""
from pathlib import Path
import hashlib
import json
import re
import shutil
import sys

sys.dont_write_bytecode = True
ROOT = Path(__file__).resolve().parents[1]
BASE = ROOT / 'audit/bulk350-root-verification-2026-10-22/revision-r6'
N119 = ROOT / 'audit/native119-codex-2026-10-22'
N120 = ROOT / 'audit/native120-codex-2026-10-23'
OUT = ROOT / 'audit/native-bulk388-source-r1-2026-10-23'
read = lambda p: json.loads(p.read_text('utf-8-sig'))
sha = lambda b: hashlib.sha256(b).hexdigest()
cs = lambda s: json.dumps(s, ensure_ascii=True)

def write(path, value):
    path.write_text(value, encoding='utf-8', newline='\n')

def dump(path, value):
    write(path, json.dumps(value, ensure_ascii=False, indent=2) + '\n')

OUT.mkdir(exist_ok=False)
v = read(N119 / 'SOURCE-DECLARATIONS.json')
current = read(N119 / 'CURRENT-SOURCE.json')
original = read(N119 / 'ORIGINAL-SOURCE.json')
c119 = {m['token']: m for m in current['methods']}
a119 = {m['token']: m for m in original['methods']}
last = read(N120 / 'SOURCE-DECLARATIONS.json')['rows'][0]
oldplan = read(ROOT / 'audit/native-bulk350-codex-2026-10-22/IMPLEMENTATION-PLAN.json')
oldmeta = read(ROOT / 'audit/native-bulk350-codex-2026-10-22/METADATA-ADDITIONS.json')
if v['added_methodspecs']:
    raise ValueError('Native119 appender explicitly models no new MethodSpecs; update it before extending')

for p in BASE.glob('*.cs'):
    shutil.copyfile(p, OUT / p.name)
for p in N119.glob('*.cs'):
    shutil.copyfile(p, OUT / ('N119.' + p.name))
for name in ['WidthAwareTables.cs', 'AppendNative119Metadata.cs']:
    shutil.copyfile(N120 / name, OUT / name)
shutil.copyfile(N120 / 'MethodBodies.cs', OUT / 'N120.MethodBodies.cs')

# Identity pins come from the immutable Native93 source read, never inferred from C++ diagnostics.
targets = []
for i, row in enumerate(v['rows'], 1):
    m, a = c119[row['token']], a119[row['token']]
    targets.append(dict(token=row['token'], name=m['name'], signature=m['signature'],
        old_code_size=m['code_size'], old_code_sha256=m['il_sha256'], old_max_stack=m['max_stack'],
        old_local_sig_token=f"0x{m['local_sig']:08X}", old_local_sig_blob=m['locals_blob'],
        new_declared_max_stack=row['max_stack'], local_constant=f'N119_Locals{i}',
        local_blob=row['local_blob'], emission_method=row['emission_method'], explicit_companion=False,
        source_batch=N119.name, original_native={k: a[k] for k in ['start','end','sha256','pdata']}))
targets.append(dict(last, new_declared_max_stack=last['max_stack'], local_constant='0', explicit_companion=False))

program = (OUT / 'Program.cs').read_text('utf-8')
actual_base_plans = re.findall(r'\(Token:(0x[0-9A-F]{8}), Code:(.*?), Max:(\d+), Locals:(\w+)\)', program)
if len(actual_base_plans) != 350 or len({x[0] for x in actual_base_plans}) != 350:
    raise ValueError('Source composition requires all 350 unique R6 tuples')
base_tuples = {t: (emit, int(maximum), local) for t, emit, maximum, local in actual_base_plans}
base_targets = []
for old in oldplan['targets']:
    emit, maximum, local = base_tuples[old['token']]
    base_targets.append(dict(old, emission_method=emit, new_declared_max_stack=maximum, local_constant=local,
        source_revision='Bulk350 R6; inherit actual corrected emitter/header source, not old source-plan MaxStack'))
pins = []
for t in targets:
    owner, name = t['name'].split('::')
    pins.append('        new(%s,%s,%s,%s,%d,%d,%s,%s,%s),' % (
        t['token'], cs(owner.rsplit('.',1)[-1]), cs(name), cs(t['signature']), t['old_code_size'],
        t['old_max_stack'], t['old_local_sig_token'], cs(t['old_local_sig_blob']), cs(t['old_code_sha256'])))
at = program.index('];', program.index('private static readonly Pin[] Pins = ['))
program = program[:at] + '\n'.join(pins) + '\n    ' + program[at:]
at = program.index('};', program.index('var plans = new[] {'))
program = program[:at].rstrip() + ',\n' + ',\n'.join(
    f'            (Token:{t["token"]}, Code:{t["emission_method"]}, Max:{t["new_declared_max_stack"]}, Locals:{t["local_constant"]})'
    for t in targets) + '\n        ' + program[at:]
needle = '        byte[] after = AppendMetadata(before, pe, out var metadataEvidence);'
if program.count(needle) != 1:
    raise ValueError('Exact original metadata-stage insertion point required')
program = program.replace(needle, needle + '\n        after = AppendNative119WidthAware(after, out var widthMetadataEvidence);')
program = program.replace('        var bodies = new List<object>();',
    '        Require(Pins.Length==388 && plans.Length==388 && plans.Select(p=>p.Token).Distinct().Count()==388 &&\n'
    '            Pins.Select(p=>p.Token).Order().SequenceEqual(plans.Select(p=>p.Token).Order()),"Exact 388-method construction scope");\n'
    '        var bodies = new List<object>();')
program = program.replace('fresh-bulk350.dll', 'fresh-bulk388.dll').replace('independent 350-target', 'independent 388-target')
program = program.replace('appended_memberref_count = 180, appended_local_signature_count = 293, appended_typespec_count = 5, appended_methodspec_count = 8, appended_user_string_count = 24,',
    'appended_memberref_count = 217, appended_local_signature_count = 330, appended_typespec_count = 8, appended_methodspec_count = 8, appended_user_string_count = 37, appended_typeref_count = 1,')
program = program.replace('metadata = metadataEvidence,', 'metadata = metadataEvidence, width_metadata = widthMetadataEvidence,')
write(OUT / 'Program.cs', program)

# Generate explicit append-only metadata row data. Source extraction is not execution validation.
sys.path.insert(0, str(ROOT / 'audit/stage9-native3/python-deps'))
import dnfile
md = dnfile.dnPE(str(ROOT / 'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).net.mdtables
lines = ['// Generated source data only; no candidate construction or execution.', 'internal static partial class Program', '{',
    '    private record Native119TypeRef(int Token,string Namespace,string Name,int ScopeTable,int ScopeRid,string ScopeName);',
    '    private record Native119TypeSpec(int Token,string Blob);',
    '    private record Native119Member(int Token,int ClassCode,string OwnerPin,int Template,string TemplateName,string TemplateSignature,string Signature,string? NewName);',
    '    private record Native119Local(int Token,string Blob);',
    '    private record Native119UserString(int Token,string Value,string EncodedBytes);']
def array(typ, name, entries):
    lines.append(f'    private static readonly {typ}[] {name} = [')
    lines.extend('        new(' + ','.join(parts) + '),' for parts in entries)
    lines.append('    ];')
array('Native119TypeRef', 'N119NewTypeRefs', [
    [f'0x{x["token"]:08X}', cs(x['namespace']), cs(x['name']), '35', str(x['resolution_scope_rid']),
     cs(str(md.AssemblyRef.rows[x['resolution_scope_rid']-1].Name))] for x in v['additional_typerefs']])
array('Native119TypeSpec', 'N119NewTypeSpecs', [[f'0x{x["token"]:08X}',cs(x['blob'])] for x in v['added_typespecs']])
array('Native119Member', 'N119NewMembers', [
    [f'0x{x["token"]:08X}', str(x['class_code']), cs(x['owner_pin']), str(x['template']), cs(x['template_name']),
     cs(md.MemberRef.rows[x['template']-1].Signature.value.hex()), cs(x['signature']), 'null' if x['new_name'] is None else cs(x['new_name'])]
    for x in v['member_definitions']])
array('Native119Local', 'N119NewLocals', [[f'0x{0x11000000+2058+i:08X}',cs(x['local_blob'])] for i,x in enumerate(v['rows'],1)])
array('Native119UserString', 'N119NewUserStrings', [[f'0x{x["token"]:08X}',cs(x['value']),cs(x['encoded_bytes'])] for x in v['user_strings']])
lines.append('}')
write(OUT / 'Native119MetadataData.cs', '\n'.join(lines)+'\n')

project = (BASE / 'Bulk350.csproj').read_text('utf-8')
extra = ['N119.' + p.name for p in N119.glob('*.cs')] + ['N120.MethodBodies.cs','WidthAwareTables.cs','AppendNative119Metadata.cs','Native119MetadataData.cs']
project = project.replace('  </ItemGroup>', ''.join(f'    <Compile Include="{name}"/>\n' for name in extra)+'  </ItemGroup>')
write(OUT / 'Bulk388.csproj', project)

eh_added = sum(len(row['eh_clauses']) for row in v['rows'])
plan = dict(schema='bulk388-source-implementation-v1', date='2026-10-23', state='SOURCE_ONLY_UNBUILT_UNEXECUTED',
    parent_sha256='b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162',parent_bytes=16033792,
    targets=base_targets+targets, target_tokens=oldplan['target_tokens']+[t['token'] for t in targets],
    method_body_targets=388, historical_direct_targets=381, additional_outside_historical_worklist=1,
    explicit_companion_targets=6, unchanged_other_method_bodies=1929,
    memberref_count_before=1100,memberref_count_after=1317,standalone_count_before=1765,standalone_count_after=2095,
    typespec_count_before=173,typespec_count_after=181,typeref_count_before=177,typeref_count_after=178,
    methodspec_count_before=185,methodspec_count_after=193,expected_finally_regions=158+eh_added,candidate=None,
    companion_tokens=oldplan['companion_tokens'],metadata_additions='METADATA-ADDITIONS.json',
    previous_source_plan='audit/native-bulk350-codex-2026-10-22/IMPLEMENTATION-PLAN.json',
    metadata_writer='Bulk350 R6 unchanged short-heap stage, then separate full table-column transcoding stage',
    root_executed_project_tests=False, new_qualified_methods=0, inherited_behavior_claims_accepted=False,
    basis='Source composition from immutable Native93, inheriting Bulk350 R6 corrected source; no R6 candidate input',
    base_r6_candidate_reference='ef95567ce714146b2f7aed1a92e98798023090bb47757f6c5b377f9ce6d9c0f4',
    new_source_targets_after_bulk350=38, source_evidence_extra_methods_are_not_targets=True)
plan['appended_finally_method_tokens'] = oldplan['appended_finally_method_tokens'] + [x['token'] for x in v['rows'] if x['eh_clauses']]
plan['new_finally_clauses_by_method'] = {x['token']:len(x['eh_clauses']) for x in v['rows'] if x['eh_clauses']}
dump(OUT / 'IMPLEMENTATION-PLAN.json', plan)
dump(OUT / 'NATIVE119-METADATA-DELTA.json', {k:v[k] for k in ['additional_typerefs','member_definitions','added_typespecs','added_methodspecs','user_strings']} |
    {'local_signatures':[dict(token=f'0x{0x11000000+2058+i:08X}',blob=x['local_blob']) for i,x in enumerate(v['rows'],1)],
     'state':'SOURCE_DECLARATIONS_NOT_EXECUTION_EVIDENCE'})
def blob_storage(hex_value):
    n=len(bytes.fromhex(hex_value))
    return n+(1 if n<128 else 2 if n<16384 else 4)
new_blob_payload=sum(blob_storage(x['local_blob']) for x in v['rows'])+sum(blob_storage(x['signature']) for x in v['member_definitions'])+sum(blob_storage(x['blob']) for x in v['added_typespecs'])
names_payload=sum(len(x['new_name'].encode('utf-8'))+1 for x in v['member_definitions'] if x['new_name'] is not None)+sum(len(x['namespace'].encode('utf-8'))+len(x['name'].encode('utf-8'))+2 for x in v['additional_typerefs'])
metadata=dict(oldmeta,schema='bulk388-source-metadata-v1',state='SOURCE_DECLARATIONS_NOT_EXECUTION_EVIDENCE',
    member_definitions=oldmeta['member_definitions']+v['member_definitions'],
    local_blobs=oldmeta['local_blobs']+[x['local_blob'] for x in v['rows']],
    constants=oldmeta['constants']+v['bindings'],type_blobs=v['type_blobs'],
    added_typespecs=oldmeta['added_typespecs']+v['added_typespecs'],additional_typerefs=v['additional_typerefs'],
    user_strings=oldmeta['user_strings']+v['user_strings'],
    appended_blob_payload_bytes=oldmeta['appended_blob_payload_bytes']+new_blob_payload,
    appended_string_payload_bytes=oldmeta['appended_string_payload_bytes']+names_payload,
    appended_user_string_payload_bytes=oldmeta['appended_user_string_payload_bytes']+sum(len(bytes.fromhex(x['encoded_bytes'])) for x in v['user_strings']),
    expected_blob_heap_bytes=65075+new_blob_payload,required_blob_index_bytes=4,
    metadata_writer='Two stages: unchanged Bulk350 R6 short writer, then explicit table-column transcoding')
dump(OUT / 'METADATA-ADDITIONS.json',metadata)
dump(OUT / 'SOURCE-LINEAGE.json', dict(date='2026-10-23',state='SOURCE_ONLY_UNBUILT_UNEXECUTED',candidate=None,
    parent_sha256='b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162',
    base_source=BASE.relative_to(ROOT).as_posix(),new_source_batches=[N119.name,N120.name],
    historical_behavior_report_28_pass='REJECTED_AS_CANDIDATE_BEHAVIOR_EVIDENCE',
    old_350_metadata_guard_preserved=True,new_qualified_methods=0,tests_executed_by_root=False,
    base_cs=[dict(path=p.name,sha256=sha(p.read_bytes())) for p in sorted(BASE.glob('*.cs'))]))
print(json.dumps({'scope':'SOURCE_COMPOSITION_ONLY','directory':str(OUT),'method_sources':388,
    'new_native120_method_sources':1,'new_targets_after_bulk350':38,'declared_finally_regions':158+eh_added,
    'candidate_constructed':False,'compiled':False,'tests_executed':False}))
