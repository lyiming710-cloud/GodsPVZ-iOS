from pathlib import Path
import argparse, hashlib, json, subprocess, zipfile

p = argparse.ArgumentParser()
p.add_argument('archive')
p.add_argument('output')
a = p.parse_args()
out = Path(a.output)
assert not out.exists(), 'Replay output must be new'
out.mkdir(parents=True)

def sha(b): return hashlib.sha256(b).hexdigest()

with zipfile.ZipFile(a.archive) as z:
    manifest = json.loads(z.read('MANIFEST.json'))
    for name, row in manifest['files'].items():
        path = out / name
        assert path.resolve().is_relative_to(out.resolve())
        b = z.read(name)
        assert len(b) == row['bytes'] and sha(b) == row['sha256'], name
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_bytes(b)

candidate = out / 'work/native30-revised-final1.dll'
parent = out / 'inputs/native29-parent.dll'
cecil = out / 'fixture-support/Mono.Cecil.dll'

def run(args, name, rc=0):
    r = subprocess.run(args, capture_output=True, text=True)
    (out / (name + '.log')).write_text(r.stdout + r.stderr)
    assert r.returncode == rc, (name, r.stdout + r.stderr)
    return r

for folder in ['Patcher', 'Audit', 'RuntimeFixture']:
    run(['dotnet', 'build', str(out / 'tools' / folder / (folder + '.csproj')), '-c', 'Release', '-o', str(out / 'built' / folder), '-p:MonoCecilPath=' + str(cecil)], folder + '-build')

run(['dotnet', str(out / 'built/Patcher/Patcher.dll'), str(parent), str(out / 'rebuilt.dll'), str(out / 'rebuilt.json')], 'rebuild-candidate')
assert (out / 'rebuilt.dll').read_bytes() == candidate.read_bytes(), 'Rebuilt candidate mismatch!'

run(['dotnet', str(out / 'built/Audit/Audit.dll'), str(parent), str(candidate), str(out / 'rebuilt.json'), str(out / 'replay-isolation.json')], 'audit')

reports = {}
for mode in ['normal', 'fault_emit', 'fault_invoke']:
    report = out / (mode + '.json')
    args = ['dotnet', str(out / 'built/RuntimeFixture/RuntimeFixture.dll'), str(candidate), str(report)] + ([] if mode == 'normal' else [mode])
    run(args, mode, 0 if mode == 'normal' else 2)
    r = json.loads(report.read_text())
    reports[mode] = {k: r[k] for k in ['methods', 'positive_cases', 'positive_failures', 'negative_controls', 'tool_errors', 'gate_pass']}
    if mode != 'normal':
        assert not r['gate_pass'] and r['tool_errors'] == 1 and not r['mutations'][0]['Detected']

# Adversarial actual DLL mutant test (wrong rotation axis)
mut_dir = out / 'mutant-writer'; mut_dir.mkdir()
(mut_dir / 'Writer.csproj').write_text(f'<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework></PropertyGroup><ItemGroup><Reference Include="Mono.Cecil"><HintPath>{cecil}</HintPath></Reference></ItemGroup></Project>')
(mut_dir / 'Program.cs').write_text("""using System;using System.IO;using System.Linq;using Mono.Cecil;using System.Reflection.PortableExecutable;
var raw=File.ReadAllBytes(args[0]);using var asm=AssemblyDefinition.ReadAssembly(args[0]);var m=(MethodDefinition)asm.MainModule.LookupToken(0x060003C9);
var call=m.Body.Instructions.Single(i=>i.Operand is MethodReference r&&r.Name=="get_forward");
var zero=asm.MainModule.GetMemberReferences().OfType<MethodReference>().Single(r=>r.FullName=="UnityEngine.Vector3 UnityEngine.Vector3::get_zero()");
using var pe=new PEReader(new MemoryStream(raw));var sec=pe.PEHeaders.SectionHeaders.Single(s=>m.RVA>=s.VirtualAddress&&m.RVA<s.VirtualAddress+s.SizeOfRawData);
int h=sec.PointerToRawData+m.RVA-sec.VirtualAddress;int hs=(BitConverter.ToUInt16(raw,h)>>12)*4;BitConverter.GetBytes(zero.MetadataToken.ToInt32()).CopyTo(raw,h+hs+call.Offset+1);File.WriteAllBytes(args[1],raw);
""")
run(['dotnet', 'build', str(mut_dir / 'Writer.csproj'), '-c', 'Release', '-o', str(mut_dir / 'built')], 'mutant-writer-build')
mutdll = out / 'wrong-axis-dll.dll'
run(['dotnet', str(mut_dir / 'built/Writer.dll'), str(candidate), str(mutdll)], 'make-wrong-axis-dll')
adv_rep = out / 'adversarial-wrong-axis.json'
r_adv = subprocess.run(['dotnet', str(out / 'built/RuntimeFixture/RuntimeFixture.dll'), str(mutdll), str(adv_rep)], capture_output=True, text=True)
assert r_adv.returncode == 2, f"Adversarial wrong-axis mutant must fail closed with exit 2, got {r_adv.returncode}"
adv_data = json.loads(adv_rep.read_text())
assert not adv_data['gate_pass'] and adv_data['positive_failures'] >= 1
reports['adversarial_wrong_axis_actual_dll'] = {'exit': r_adv.returncode, 'gate_pass': adv_data['gate_pass'], 'positive_failures': adv_data['positive_failures']}

# If historical un-revised candidate is in evidence, verify fixture rejects it
old_cand_path = out / 'inputs/native30-prior-candidate.dll'
if old_cand_path.exists():
    old_rep = out / 'adversarial-old-candidate.json'
    r_old = subprocess.run(['dotnet', str(out / 'built/RuntimeFixture/RuntimeFixture.dll'), str(old_cand_path), str(old_rep)], capture_output=True, text=True)
    assert r_old.returncode == 2, f"Defective old candidate must be rejected with exit 2, got {r_old.returncode}"
    old_data = json.loads(old_rep.read_text())
    assert not old_data['gate_pass'] and old_data['positive_failures'] >= 1
    reports['adversarial_old_candidate'] = {'exit': r_old.returncode, 'gate_pass': old_data['gate_pass'], 'positive_failures': old_data['positive_failures']}

result = {
    'files_verified': len(manifest['files']),
    'archive_sha256': sha(Path(a.archive).read_bytes()),
    'candidate_sha256': sha(candidate.read_bytes()),
    'candidate_rebuilt_exact': True,
    'isolated_copy_without_hardlinks': True,
    'reports': reports
}
(out / 'ARCHIVE-REPLAY.json').write_text(json.dumps(result, indent=2))
print(json.dumps(result, indent=2))
