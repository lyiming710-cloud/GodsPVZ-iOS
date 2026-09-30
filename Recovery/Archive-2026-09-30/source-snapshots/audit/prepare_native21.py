from pathlib import Path
import json,hashlib
root=Path(__file__).parent/'native19-work';folder=root/'scripts/takeover/PatcherNative21';folder.mkdir(exist_ok=True)
data=json.loads((Path(__file__).parent/'native20-evidence/replay/all-methods.integer-audit.json').read_text());data.pop('blocked')
archive=root/'Recovery/Native21-2026-09-29';archive.mkdir(parents=True,exist_ok=True)
plan=archive/'PROPOSALS.json';plan.write_bytes((json.dumps(data,indent=2)+'\n').encode());plan_sha=hashlib.sha256(plan.read_bytes()).hexdigest()
source=(root/'scripts/takeover/PatcherNative20/Mechanical.cs').read_text().replace('Native20','Native21').replace('RunMechanical','RunIntegerLocals').replace('NATIVE20','NATIVE21')
source=source.replace('2df30387140871d1211467ccbfc790789d588d68fa131974c37c23e11f0dc873','f561d48a353f1747b77260f9db69acabc1bb4860f46d4ffc447d81764c55efa9').replace('c022fb10f30c4363ba27cc48a4508029f57c76e079729e4aa753cd7e188dd3de','7d2b3661f3e4aa3ba27b986c233d65c920e4c439851eae1788ae58323cc1d910').replace('Native19 input','Native20 input')
source=source.replace('proposals.Length!=73||proposals.Sum(p=>p.GetProperty("changes").GetArrayLength())!=112','proposals.Length!=38||proposals.Sum(p=>p.GetProperty("changes").GetArrayLength())!=9||proposals.Sum(p=>p.GetProperty("locals").GetArrayLength())!=90').replace('Count()!=73','Count()!=38')
marker='            foreach(var change in proposal.GetProperty("changes").EnumerateArray()){'
insertion='''            foreach(var local in proposal.GetProperty("locals").EnumerateArray()){
                var variable=method.Body.Variables[local.GetProperty("index").GetInt32()];
                if(variable.VariableType.FullName!="System.Object"||local.GetProperty("before").GetString()!="System.Object")throw new Exception("local input type drift");
                variable.VariableType=local.GetProperty("after").GetString() switch {
                    "System.Int32"=>mod.TypeSystem.Int32,
                    "System.Int64"=>mod.TypeSystem.Int64,
                    _=>throw new Exception("unsupported inferred local type")};
            }
'''
assert source.count(marker)==1;source=source.replace(marker,insertion+marker)
source=source.replace('using var manifest=JsonDocument.Parse',f'if(Hash(args[2])!="{plan_sha}")throw new Exception("proposal hash drift");\n        using var manifest=JsonDocument.Parse')
(folder/'Mechanical.cs').write_bytes(source.encode())
(folder/'PatcherNative21.csproj').write_bytes((root/'scripts/takeover/PatcherNative20/PatcherNative20.csproj').read_bytes().replace(b'Native20Entry',b'Native21Entry'))
source=(root/'scripts/codespaces/native20_local.py').read_text().replace('Native20','Native21').replace('native20','native21').replace("old=root/'.validation/native19/replay'","old=root/'.validation/native20/replay'").replace('c022fb10f30c4363ba27cc48a4508029f57c76e079729e4aa753cd7e188dd3de','7d2b3661f3e4aa3ba27b986c233d65c920e4c439851eae1788ae58323cc1d910').replace('2df30387140871d1211467ccbfc790789d588d68fa131974c37c23e11f0dc873','f561d48a353f1747b77260f9db69acabc1bb4860f46d4ffc447d81764c55efa9').replace("source=old/f'native19-{kind}.dll'","source=old/f'native20-{kind}.dll'").replace('Recovery/Native19-2026-09-29/NEXT-BATCH-PROPOSALS.json','Recovery/Native21-2026-09-29/PROPOSALS.json').replace('962257a461978f5fd63a136c0b87c62604dd4f00e87761718b52cfb263de1eed',plan_sha)
(root/'scripts/codespaces/native21_local.py').write_bytes(source.encode())
source=(root/'scripts/codespaces/qualify_native20_cpp.py').read_text().replace(".validation/native20'",".validation/native21'").replace('cpp-final73','cpp-final38').replace('native20-linked','native21-linked')
(root/'scripts/codespaces/qualify_native21_cpp.py').write_bytes(source.encode())
print('NATIVE21_PROPOSALS_SHA256',plan_sha)
