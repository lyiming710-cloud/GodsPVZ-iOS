from pathlib import Path
import hashlib,json,subprocess
W=Path(__file__).resolve().parent;root=W.parents[1]
r=root/'audit/local-preflight/exact-editor-tools/resolver';a=root/'audit/local-preflight/exact-editor-tools/aot-resolver'
refs=[a/n for n in ('mscorlib.dll','System.dll','System.Core.dll','netstandard.dll')]+[r/n for n in ('UnityEditor.CoreModule.dll','UnityEngine.CoreModule.dll','UnityEngine.ImageConversionModule.dll','UnityEngine.JSONSerializeModule.dll')]
source=root/'audit/repo-native113/Recovery/Custom-Shaders-2026-10-24/ios-support/Assets/Editor/GodsPVZCustomShaderAudit.cs'
out=W/'shader-r1-validation/helper-api';out.mkdir(exist_ok=True)
cmd=[str(root/'.local/dotnet10/dotnet.exe'),str(root/'.local/dotnet10/sdk/10.0.402/Roslyn/bincore/csc.dll'),'/noconfig','/nostdlib','/target:library','/langversion:9.0','/utf8output','/preferreduilang:en-US','/define:UNITY_EDITOR','/out:'+str(out/'ShaderHelper.dll')]+['/r:'+str(p) for p in refs]+[str(source)]
proc=subprocess.run(cmd,capture_output=True)
(out/'compile.log').write_bytes(proc.stdout+proc.stderr)
prior=json.loads((W/'asset-audit-40081048209/HELPER-COMPILATION.json').read_bytes())
hashes={Path(v['path']).name:v['sha256'] for v in prior['references']}
result=dict(scope='LOCAL_EDITOR_API_COMPILATION_USING_EXACT_FOUR_EDITOR_MODULES_AND_LOCAL_AOT_BCL',unity_executed=False,exit_code=proc.returncode,source_sha256=hashlib.sha256(source.read_bytes()).hexdigest(),
            editor_modules=[dict(file=p.name,sha256=hashlib.sha256(p.read_bytes()).hexdigest(),exact_latest_ci_module=hashlib.sha256(p.read_bytes()).hexdigest()==hashes.get(p.name)) for p in refs[4:]])
assert all(p['exact_latest_ci_module'] for p in result['editor_modules'])
(out/'RESULT.json').write_bytes((json.dumps(result,indent=2)+'\n').encode())
print(json.dumps(result));raise SystemExit(proc.returncode)
